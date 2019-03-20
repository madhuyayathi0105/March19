using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Farpoint = FarPoint.Web.Spread;
using System.Configuration;

public partial class TabulatedMarkResults : System.Web.UI.Page
{

    #region Field Declaration

    Hashtable hat = new Hashtable();

    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();

    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    string batch_year = string.Empty;
    string degree_code = string.Empty;
    string semester = string.Empty;
    string section = string.Empty;


    string subjectNo = string.Empty;

    string examMonth = string.Empty;
    string examYear = string.Empty;
    string examCode = string.Empty;

    string qry = string.Empty;
    string qrysec = string.Empty;

    bool isSchool = false;

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();

    #endregion Field Declaration

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Page.DataBind();
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
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }

            string grouporusercode1 = "";
            if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode1 = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
            }
            else
            {
                grouporusercode1 = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }

            DataSet schoolds = new DataSet();
            string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercode1 + "";
            schoolds.Clear();
            schoolds.Dispose();
            schoolds = d2.select_method_wo_parameter(sqlschool, "Text");
            if (schoolds.Tables[0].Rows.Count > 0)
            {
                string schoolvalue = Convert.ToString(schoolds.Tables[0].Rows[0]["value"]);
                if (schoolvalue.Trim() == "0")
                {
                    isSchool = true;
                }
            }
            if (!IsPostBack)
            {
                lblErrSearch.Text = string.Empty;
                lblErrSearch.Visible = false;
                popupdiv.Visible = false;
                lblpopuperr.Text = string.Empty;
                divMainContent.Visible = false;
                rptprint1.Visible = false;

                txtOrder.Text = string.Empty;
                ItemList.Clear();
                Itemindex.Clear();
                txtOrder.Visible = false;

                chkColumnOrderAll.Checked = false;
                string value = "";
                int index;
                value = string.Empty;

                for (int i = 0; i < cblColumnOrder.Items.Count; i++)
                {
                    if (cblColumnOrder.Items[i].Selected == false)
                    {
                        ItemList.Remove(Convert.ToString(cblColumnOrder.Items[i].Text).Trim());
                        Itemindex.Remove(Convert.ToString(i).Trim());
                    }
                    else
                    {
                        if (!Itemindex.Contains(i))
                        {
                            ItemList.Add(Convert.ToString(cblColumnOrder.Items[i].Text).Trim());
                            Itemindex.Add(Convert.ToString(i).Trim());
                        }
                    }
                }

                txtOrder.Visible = true;
                txtOrder.Text = "";
                for (int i = 0; i < ItemList.Count; i++)
                {
                    if (txtOrder.Text == "")
                    {
                        txtOrder.Text = Convert.ToString(ItemList[i]).Trim();
                    }
                    else
                    {
                        txtOrder.Text = txtOrder.Text + "," + Convert.ToString(ItemList[i]).Trim();
                    }
                }
                if (ItemList.Count == cblColumnOrder.Items.Count)
                {
                    chkColumnOrderAll.Checked = true;
                }

                if (ItemList.Count > 0)
                {
                    txtOrder.Visible = false;
                    lbtnRemoveAll.Visible = true;
                }
                else
                {
                    txtOrder.Visible = false;
                    lbtnRemoveAll.Visible = false;
                }

                #region LoadHeader

                Bindcollege();
                BindBatch();
                BindDegree();
                bindbranch();
                bindsem();
                BindExamMonthYear();
                GetSubject();

                #endregion LoadHeader

                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                string grouporusercode = string.Empty;

                if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
                }

                string Master = "select * from Master_Settings where " + grouporusercode + "";
                DataSet ds = d2.select_method(Master, hat, "Text");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim() == "Roll No" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim() == "Register No" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim() == "Student_Type" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim() == "1")
                    {
                        Session["Studflag"] = "1";
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

            string columnfield = "";
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
            ds.Dispose();
            ds.Clear();
            ds.Reset();
            ds = d2.select_method("bind_college", hat, "sp");
            ddlCollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlCollege.DataSource = ds;
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

    public void BindBatch()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "Batch_year";
                ddlbatch.DataValueField = "Batch_year";
                ddlbatch.DataBind();
                ddlbatch.SelectedIndex = ddlbatch.Items.Count - 1;
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
            ddldegree.Items.Clear();
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
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }

    }

    public void bindbranch()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            string course_id = Convert.ToString(ddldegree.SelectedValue);
            ddlbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            if (ds.Tables[0].Rows.Count > 0)
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
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void bindsem()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            string strbatchyear = Convert.ToString(ddlbatch.Text);
            string strbranch = Convert.ToString(ddlbranch.SelectedValue);

            ddlsem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            ds.Dispose();
            ds.Reset();
            ds = d2.BindSem(strbranch, strbatchyear, collegecode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]).Trim(), out first_year);
                int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]).Trim(), out duration);
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsem.Items.Add(Convert.ToString(i));
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsem.Items.Add(Convert.ToString(i));
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

    public void GetSubject(int type = 0)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            string subjectquery = string.Empty;
            string sections = string.Empty;
            string strsec = "";
            txtSubjects.Text = "--Select--";
            chkSubjects.Checked = false;
            cblSubjects.Items.Clear();
            //if (ddlsec.Items.Count > 0)
            //{
            //    sections = Convert.ToString(ddlsec.SelectedValue).Trim();
            //    if (Convert.ToString(ddlsec.SelectedItem.Text).Trim().ToLower() == "all" || Convert.ToString(ddlsec.SelectedItem.Text).Trim().ToLower() == "")
            //    {
            //        strsec = "";
            //    }
            //    else
            //    {
            //        strsec = " and st.Sections='" + Convert.ToString(sections) + "'";
            //    }
            //}

            string sems = "";
            if (ddlsem.Items.Count > 0)
            {
                if (Convert.ToString(ddlsem.SelectedValue).Trim() != "")
                {
                    if (Convert.ToString(ddlsem.SelectedValue).Trim() == "")
                    {
                        sems = "";
                    }
                    else
                    {
                        sems = "and SM.semester='" + Convert.ToString(ddlsem.SelectedValue).Trim() + "'";
                    }


                    //if (Convert.ToString(Session["Staff_Code"]) == "")
                    //{
                    //subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and  st.subject_no=s.subject_no  and SM.degree_code='" + Convert.ToString(ddlbranch.SelectedValue) + "' " + Convert.ToString(sems) + " and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue) + "' order by S.subject_no ";
                    subjectquery = "select distinct S.subject_no,subject_name,isnull(s.subjectpriority,'') as subjectpriority  from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and SM.degree_code='" + Convert.ToString(ddlbranch.SelectedValue) + "' " + Convert.ToString(sems) + " and  S.subtype_no = Sem.subtype_no and promote_count='1' " + ((type != 0) ? " and isnull(ltrim(rtrim(Sem.projThe)),'0')=1 " : "") + " and SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue) + "' order by subjectpriority,S.subject_no";
                    //}
                    //else if (Convert.ToString(Session["Staff_Code"]) != "")
                    //{
                    //    subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and st.subject_no=s.subject_no and s.syll_code=SM.syll_code and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.degree_code='" + Convert.ToString(ddlbranch.SelectedValue) + "' " + Convert.ToString(sems) + " and  SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue) + "'  and staff_code='" + Convert.ToString(Session["Staff_Code"]) + "' " + strsec + " order by S.subject_no ";
                    //}
                    if (subjectquery != "")
                    {
                        ds.Dispose();
                        ds.Reset();
                        ds = d2.select_method(subjectquery, hat, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            cblSubjects.DataSource = ds;
                            cblSubjects.DataTextField = "subject_name";
                            cblSubjects.DataValueField = "subject_no";
                            cblSubjects.DataBind();
                            for (int h = 0; h < cblSubjects.Items.Count; h++)
                            {
                                cblSubjects.Items[h].Selected = true;
                            }
                            txtSubjects.Text = "Subjects" + "(" + cblSubjects.Items.Count + ")";
                            chkSubjects.Checked = true;
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

    public void BindExamMonthYear()
    {
        try
        {
            if (ddlbatch.Items.Count > 0)
            {
                batch_year = Convert.ToString(ddlbatch.SelectedValue).Trim();
            }
            if (ddlbranch.Items.Count > 0)
            {
                degree_code = Convert.ToString(ddlbranch.SelectedValue).Trim();
            }
            if (ddlsem.Items.Count > 0)
            {
                semester = Convert.ToString(ddlsem.SelectedItem.Text).Trim();
            }
            ddlExamMonth.Items.Clear();
            ddlExamYear.Items.Clear();

            if (!string.IsNullOrEmpty(batch_year) && !string.IsNullOrEmpty(degree_code) && !string.IsNullOrEmpty(semester))
            {
                qry = "select Exam_Month,upper(convert(varchar(3),DateAdd(month,Exam_Month,-1))) as Month_Name,Exam_year from exam_details where batch_year='" + batch_year.Trim() + "' and degree_code='" + degree_code.Trim() + "' and current_semester='" + semester.Trim() + "'";
                ds.Clear();
                ds.Reset();
                ds.Dispose();
                ds = d2.select_method_wo_parameter(qry, "text");

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlExamMonth.DataSource = ds;
                    ddlExamMonth.DataTextField = "Month_Name";
                    ddlExamMonth.DataValueField = "Exam_Month";
                    ddlExamMonth.DataBind();

                    ddlExamYear.DataSource = ds;
                    ddlExamYear.DataTextField = "Exam_year";
                    ddlExamYear.DataValueField = "Exam_year";
                    ddlExamYear.DataBind();
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

    //public void GetSubject1()
    //{
    //    try
    //    {
    //        lblErrSearch.Text = string.Empty;
    //        lblErrSearch.Visible = false;
    //        string subjectquery = string.Empty;
    //        ddlsubject.Items.Clear();
    //        string sections = string.Empty;// Convert.ToString(ddlsec.SelectedValue);
    //        string strsec = "";
    //        if (ddlsec.Items.Count > 0)
    //        {
    //            sections = Convert.ToString(ddlsec.SelectedValue);
    //            if (Convert.ToString(ddlsec.SelectedItem.Text).Trim().ToLower() == "all" || Convert.ToString(ddlsec.SelectedItem.Text).Trim().ToLower() == "")
    //            {
    //                strsec = "";
    //            }
    //            else
    //            {
    //                strsec = " and st.Sections='" + Convert.ToString(sections) + "'";
    //            }
    //        }

    //        string sems = "";
    //        if (ddlsem.Items.Count > 0)
    //        {
    //            if (Convert.ToString(ddlsem.SelectedValue) != "")
    //            {
    //                if (Convert.ToString(ddlsem.SelectedValue) == "")
    //                {
    //                    sems = "";
    //                }
    //                else
    //                {
    //                    sems = "and SM.semester='" + Convert.ToString(ddlsem.SelectedValue) + "'";
    //                }


    //                if (Convert.ToString(Session["Staff_Code"]).Trim() == "")
    //                {
    //                    //subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and  st.subject_no=s.subject_no  and SM.degree_code=" + Convert.ToString(ddlbranch.SelectedValue) + " " + Convert.ToString(sems) + " and  S.subtype_no = Sem.subtype_no and promote_count=1 and SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue) + "' order by S.subject_no ";
    //                    subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and SM.degree_code='" + Convert.ToString(ddlbranch.SelectedValue) + "' " + Convert.ToString(sems) + " and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue).Trim() + "' order by S.subject_no ";
    //                }
    //                else if (Convert.ToString(Session["Staff_Code"]).Trim() != "")
    //                {
    //                    subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and st.subject_no=s.subject_no and s.syll_code=SM.syll_code and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.degree_code='" + Convert.ToString(ddlbranch.SelectedValue) + "' " + Convert.ToString(sems) + " and  SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue) + "'  and staff_code='" + Convert.ToString(Session["Staff_Code"]).Trim() + "' " + strsec + "  order by S.subject_no ";
    //                }
    //                if (subjectquery != "")
    //                {
    //                    ds.Dispose();
    //                    ds.Reset();
    //                    ds = d2.select_method(subjectquery, hat, "Text");
    //                    if (ds.Tables[0].Rows.Count > 0)
    //                    {
    //                        ddlsubject.Enabled = true;
    //                        ddlsubject.DataSource = ds;
    //                        ddlsubject.DataValueField = "Subject_No";
    //                        ddlsubject.DataTextField = "Subject_Name";
    //                        ddlsubject.DataBind();
    //                    }
    //                    else
    //                    {
    //                        ddlsubject.Enabled = false;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrSearch.Text = ex.StackTrace;
    //        lblErrSearch.Visible = true;
    //    }
    //}

    public void ChangeHeaderName(bool isschool)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = "";
            //lblCollege.Text = ((!isschool) ? "College" : "School");
            lbl_Batchyear.Text = ((!isschool) ? "Batch" : "Year");
            lbldegree.Text = ((!isschool) ? "Degree" : "School Type");
            lblbranch.Text = ((!isschool) ? "Department" : "Standard");
            lblsem.Text = ((!isschool) ? "Semester" : "Term");
            //lblsec.Text = ((!isschool) ? "Section" : "Section");
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

            //FpSpread1.Visible = false;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.RowHeader.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = false;
            FpSpread1.Sheets[0].RowCount = 0;
            if (type == 0)
            {
                FpSpread1.Sheets[0].ColumnCount = 3;
                FpSpread1.Sheets[0].FrozenColumnCount = 3;
            }
            else
            {
                FpSpread1.Sheets[0].ColumnCount = 4;
                FpSpread1.Sheets[0].FrozenColumnCount = 4;
            }

            #region SpreadStyles

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            //darkstyle.ForeColor = System.Drawing.Color.Black;
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
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

            FpSpread1.Sheets[0].Columns[0].Width = 40;
            FpSpread1.Sheets[0].Columns[1].Width = 120;
            FpSpread1.Sheets[0].Columns[2].Width = 240;

            FpSpread1.Sheets[0].Columns[0].Locked = true;
            FpSpread1.Sheets[0].Columns[1].Locked = true;
            FpSpread1.Sheets[0].Columns[2].Locked = true;

            FpSpread1.Sheets[0].Columns[0].Resizable = false;
            FpSpread1.Sheets[0].Columns[1].Resizable = false;
            FpSpread1.Sheets[0].Columns[2].Resizable = false;

            FpSpread1.Sheets[0].Columns[0].Visible = false;
            FpSpread1.Sheets[0].Columns[1].Visible = false;
            FpSpread1.Sheets[0].Columns[2].Visible = false;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Reg. No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Name";

            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);

            if (type != 0)
            {
                FpSpread1.Sheets[0].Columns[3].Width = 120;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Resizable = false;
                FpSpread1.Sheets[0].Columns[3].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Batch";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            }

            if (cblColumnOrder.Items.Count > 0)
            {
                foreach (ListItem li in cblColumnOrder.Items)
                {
                    string value = Convert.ToString(li.Value).Trim();
                    if (li.Selected)
                    {
                        switch (value)
                        {
                            case "0":
                                FpSpread1.Sheets[0].Columns[0].Visible = true;
                                break;
                            case "1":
                                FpSpread1.Sheets[0].Columns[1].Visible = true;
                                break;
                            case "2":
                                FpSpread1.Sheets[0].Columns[2].Visible = true;
                                break;
                            case "13":
                                if (type != 0)
                                {
                                    FpSpread1.Sheets[0].Columns[3].Visible = true;
                                }
                                break;
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

    #endregion Bind Header

    #region DropDown Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContent.Visible = false;
            rptprint1.Visible = false;

            BindDegree();
            bindbranch();
            bindsem();
            BindExamMonthYear();
            if (ddlReportFormat.SelectedIndex == 0)
            {
                GetSubject();
            }
            else
            {
                GetSubject(1);
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContent.Visible = false;
            rptprint1.Visible = false;

            ////btnSave.Visible = false;
            BindDegree();
            bindbranch();
            bindsem();
            BindExamMonthYear();
            if (ddlReportFormat.SelectedIndex == 0)
            {
                GetSubject();
            }
            else
            {
                GetSubject(1);
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContent.Visible = false;
            rptprint1.Visible = false;
            bindbranch();
            bindsem();
            BindExamMonthYear();
            if (ddlReportFormat.SelectedIndex == 0)
            {
                GetSubject();
            }
            else
            {
                GetSubject(1);
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContent.Visible = false;
            rptprint1.Visible = false;
            bindsem();
            BindExamMonthYear();
            if (ddlReportFormat.SelectedIndex == 0)
            {
                GetSubject();
            }
            else
            {
                GetSubject(1);
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContent.Visible = false;
            rptprint1.Visible = false;
            BindExamMonthYear();
            if (ddlReportFormat.SelectedIndex == 0)
            {
                GetSubject();
            }
            else
            {
                GetSubject(1);
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContent.Visible = false;
            rptprint1.Visible = false;
            if (ddlReportFormat.SelectedIndex == 0)
            {
                GetSubject();
            }
            else
            {
                GetSubject(1);
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
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContent.Visible = false;
            rptprint1.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlExamYear_Selectchanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContent.Visible = false;
            rptprint1.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlReportFormat_Selectchanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContent.Visible = false;
            rptprint1.Visible = false;
            if (ddlReportFormat.SelectedIndex == 0)
            {
                GetSubject();
            }
            else
            {
                GetSubject(1);
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void chkSubjects_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContent.Visible = false;
            rptprint1.Visible = false;

            txtSubjects.Text = "--Select--";
            int count = 0;
            if (chkSubjects.Checked == true)
            {
                count++;
                for (int i = 0; i < cblSubjects.Items.Count; i++)
                {
                    cblSubjects.Items[i].Selected = true;
                }
                txtSubjects.Text = "Subjects (" + (cblSubjects.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cblSubjects.Items.Count; i++)
                {
                    cblSubjects.Items[i].Selected = false;
                }
                txtSubjects.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void cblSubjects_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            chkSubjects.Checked = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContent.Visible = false;
            rptprint1.Visible = false;
            txtSubjects.Text = "--Select--";
            int commcount = 0;
            for (int i = 0; i < cblSubjects.Items.Count; i++)
            {
                if (cblSubjects.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cblSubjects.Items.Count)
                {
                    chkSubjects.Checked = true;
                }
                txtSubjects.Text = "Subjects (" + Convert.ToString(commcount) + ")";
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion DropDown Events

    #region Button Events

    #region Go Click

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContent.Visible = false;
            rptprint1.Visible = false;

            string eduLevel = string.Empty;
            string externalWrittenMaxMarks = string.Empty;
            collegecode = string.Empty;
            batch_year = string.Empty;
            degree_code = string.Empty;
            semester = string.Empty;

            double writtenMaxMark = 0;
            examMonth = string.Empty;
            examYear = string.Empty;
            examCode = string.Empty;

            subjectNo = string.Empty;

            int selSubjectCount = 0;
            int spreadHeight = 0;

            if (ddlCollege.Items.Count == 0)
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "School" : "College") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            else
            {
                collegecode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }

            if (ddlbatch.Items.Count == 0)
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "Year" : " Batch") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            else
            {
                batch_year = Convert.ToString(ddlbatch.SelectedItem.Text).Trim();
            }

            if (ddldegree.Items.Count == 0)
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "School Type" : "Degree") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (ddlbranch.Items.Count == 0)
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "Standard" : "Department") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            else
            {
                degree_code = Convert.ToString(ddlbranch.SelectedValue).Trim();
            }

            if (ddlsem.Items.Count == 0)
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "Term" : " Semester") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            else
            {
                semester = Convert.ToString(ddlsem.SelectedItem.Text).Trim();
            }

            if (cblSubjects.Items.Count == 0)
            {
                lblpopuperr.Text = "No Subject were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            else
            {
                selSubjectCount = 0;
                subjectNo = string.Empty;
                foreach (ListItem li in cblSubjects.Items)
                {
                    if (li.Selected)
                    {
                        selSubjectCount++;
                        if (string.IsNullOrEmpty(subjectNo))
                        {
                            subjectNo = "'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            subjectNo += ",'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (selSubjectCount == 0)
                {
                    lblpopuperr.Text = "Please Select Atleast One Subject";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
            }

            if (ddlExamMonth.Items.Count == 0)
            {
                lblpopuperr.Text = "No Exam Month were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            else
            {
                examMonth = Convert.ToString(ddlExamMonth.SelectedValue).Trim();
            }

            if (ddlExamYear.Items.Count == 0)
            {
                lblpopuperr.Text = "No Exam Year were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            else
            {
                examYear = Convert.ToString(ddlExamYear.SelectedValue).Trim();
            }
            bool norec=true;

            if (!string.IsNullOrEmpty(collegecode.Trim()) && !string.IsNullOrEmpty(batch_year.Trim()) && !string.IsNullOrEmpty(degree_code) && !string.IsNullOrEmpty(semester) && !string.IsNullOrEmpty(examMonth.Trim()) && !string.IsNullOrEmpty(examYear.Trim()) && !string.IsNullOrEmpty(subjectNo.Trim()))
            {
                qry = "select exam_code from Exam_Details where Exam_Month='" + examMonth + "' and Exam_year='" + examYear + "' and  batch_year='" + batch_year.Trim() + "' and degree_code='" + degree_code.Trim() + "'";
                examCode = d2.GetFunctionv(qry);

               

                if (!string.IsNullOrEmpty(examCode.Trim()))
                {
                    qry = "select r.Roll_No,r.Reg_No,r.Stud_Name,Convert(varchar(100),r.Batch_Year)+'-'+ Convert(varchar(100),(r.Batch_Year+Cast(Round(cast((NDurations/2) as decimal(2,1)),0,0) as int))) as NBatch,Convert(varchar(100),r.Batch_Year)+'-'+ Convert(varchar(100),(r.Batch_Year+Cast(Round(cast(dg.Duration as decimal(2,1))/2,0,0) as int))) as Batch,s.subType_no,ss.subject_type,s.subject_code,s.subject_no,s.subject_name,isnull(s.WrittenMaxMark,0) as WrittenMaxMark,isnull(s.min_int_marks,0) as min_int_marks,isnull(s.max_int_marks,0) as max_int_marks,isnull(m.internal_mark,0) as internal_mark,ISNULL(s.min_ext_marks,0) as min_ext_marks,isnull(s.max_ext_marks,0) as max_ext_marks,isnull( m.external_mark,0) as external_mark,isnull(s.mintotal,0) as mintotal,isnull(s.maxtotal,0) as maxtotal,(case when isnull(m.internal_mark,0) >=0  then   isnull(m.internal_mark,0) else 0 end) +   (case when isnull(m.external_mark,0) >=0  then   isnull(m.external_mark,0) else 0 end) as total,m.result,isnull(m.evaluation1,0) as Eval1,isnull(m.evaluation2,0)  as Eval2,isnull(m.evaluation3,0) as Eval3,ISNULL(M.Average,0) AS Average from mark_entry m,Registration r,subject s,sub_sem ss,syllabus_master sy ,Degree dg,Ndegree ndg where r.degree_code=dg.Degree_Code and ndg.Degree_code=dg.Degree_Code and ndg.batch_year=r.Batch_Year and sy.Batch_Year=ndg.batch_year and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and sy.Batch_Year=r.Batch_Year and r.degree_code=sy.degree_code and s.subject_no=m.subject_no and r.Roll_No=m.roll_no and r.Batch_Year='" + batch_year.Trim() + "' and r.degree_code='" + degree_code.Trim() + "' and r.college_code='" + collegecode.Trim() + "' and s.subject_no in(" + subjectNo.Trim() + ") " + ((ddlReportFormat.SelectedIndex == 0) ? "" : " and isnull(ltrim(rtrim(ss.projThe)),'0')=1") + " and m.exam_code='" + examCode.Trim() + "' order by r.Reg_No";
                    ds = d2.select_method_wo_parameter(qry, "Text");

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        norec=false;
                        eduLevel = d2.GetFunctionv("select c.Edu_Level from Course c,Department dt,Degree dg where c.Course_Id=dg.Course_Id and dt.Dept_Code=dg.Dept_Code and dg.Degree_Code='" + degree_code + "'");
                        if (eduLevel.Trim() != "")
                        {
                            externalWrittenMaxMarks = d2.GetFunctionv("select value from COE_Master_Settings where settings='MaxExternalMark " + eduLevel.Trim() + "'");
                        }

                        if (string.IsNullOrEmpty(externalWrittenMaxMarks.Trim()))
                        {
                            if (ddlReportFormat.SelectedIndex == 0)
                            {
                                lblpopuperr.Text = "Please Set Maximum External Written Marks For " + eduLevel.Trim();
                                lblpopuperr.Visible = true;
                                popupdiv.Visible = true;
                                return;
                            }
                        }
                        else
                        {
                            double.TryParse(externalWrittenMaxMarks.Trim(), out writtenMaxMark);
                        }

                        DataTable dtStudent = new DataTable();
                        DataTable dtThesisStudents = new DataTable();
                        DataTable dtSubjectsList = new DataTable();
                        if (ddlReportFormat.SelectedIndex == 0)
                        {
                            Init_Spread(FpSpreadTabMarks);
                            dtStudent = ds.Tables[0].DefaultView.ToTable(true, "Roll_No", "Reg_No", "Stud_Name");

                            if (dtStudent.Rows.Count > 0)
                            {
                                int col = 0;
                                double maxTotal = 0;
                                int selSubjects = 0;
                                int subjectColumn = 3;
                                int startSpan = 3;
                                int valuationCount = 0;
                                double totalValuationMarks = 0;

                                bool valuation1 = false;
                                bool valuation2 = false;
                                bool valuation3 = false;

                                //double.TryParse(writtenMaxMark, out maxTotal);
                                foreach (ListItem li in cblSubjects.Items)
                                {
                                    maxTotal = 0;
                                    totalValuationMarks = 0;
                                    if (li.Selected)
                                    {
                                        valuationCount = 0;
                                        maxTotal += writtenMaxMark;
                                        string maxMark = d2.GetFunctionv("select max_ext_marks from subject where subject_no='" + li.Value.Trim() + "'");
                                        string intMax = d2.GetFunctionv("select max_int_marks from subject where subject_no='" + li.Value.Trim() + "'");
                                        FpSpreadTabMarks.Sheets[0].ColumnCount += 9;
                                        Farpoint.StyleInfo MyStyle = new Farpoint.StyleInfo();
                                        MyStyle.Font.Size = FontUnit.Medium;
                                        MyStyle.Font.Name = "Book Antiqua";
                                        MyStyle.Font.Bold = true;
                                        MyStyle.HorizontalAlign = HorizontalAlign.Center;
                                        MyStyle.ForeColor = Color.Black;
                                        MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                                        for (int columns = selSubjects * 9 + subjectColumn; columns < FpSpreadTabMarks.Sheets[0].ColumnCount; columns++)
                                        {
                                            if (cbsubcode.Checked == true)  //added by Mullai
                                            {
                                                string subcode = d2.GetFunction("select subject_code from subject where subject_no='" + li.Value + "'");
                                                FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, columns].Text = Convert.ToString(subcode).Trim();
                                               // FpSpreadTabMarks.Sheets[0].Columns[columns].Width = 150;
                                                // FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, columns].Text = Convert.ToString(li.Value).Trim();
                                            }
                                            else
                                            {
                                                FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, columns].Text = Convert.ToString(li.Text).Trim();
                                            }
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, columns].Tag = Convert.ToString(li.Value).Trim();

                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, columns].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, columns].VerticalAlign = VerticalAlign.Middle;
                                            FpSpreadTabMarks.Sheets[0].Columns[columns].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].Columns[columns].Resizable = false;
                                            //FpSpreadTabMarks.Sheets[0].ColumnHeader
                                            //FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - columns, 1, columns);
                                        }
                                        selSubjects++;
                                        //FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 8, 1, 8);

                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 9].Text = Convert.ToString("Ext 1st(" + Convert.ToString(writtenMaxMark).Trim() + ")").Trim();
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 9].Tag = Convert.ToString(li.Value).Trim();
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 9].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 9].VerticalAlign = VerticalAlign.Middle;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 9].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 9].Resizable = false;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 9].Visible = false;

                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 8].Text = Convert.ToString("Ext 2nd(" + Convert.ToString(writtenMaxMark).Trim() + ")").Trim();
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 8].Tag = Convert.ToString(li.Value).Trim();
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 8].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 8].VerticalAlign = VerticalAlign.Middle;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 8].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 8].Resizable = false;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 8].Visible = false;

                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 7].Text = Convert.ToString("Ext 3rd(" + Convert.ToString(writtenMaxMark).Trim() + ")").Trim();
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 7].Tag = Convert.ToString(li.Value).Trim();
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 7].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 7].VerticalAlign = VerticalAlign.Middle;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 7].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 7].Resizable = false;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 7].Visible = false;

                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Text = Convert.ToString("Ext Total(" + Convert.ToString(totalValuationMarks).Trim() + ")").Trim();
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Tag = Convert.ToString(li.Value).Trim();
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 6].VerticalAlign = VerticalAlign.Middle;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Resizable = false;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Visible = false;

                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Text = Convert.ToString("Ext Total/" + valuationCount + "(" + Convert.ToString(totalValuationMarks / valuationCount).Trim() + ")").Trim();
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Tag = Convert.ToString(li.Value).Trim();
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 5].VerticalAlign = VerticalAlign.Middle;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Resizable = false;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Visible = false;

                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 4].Text = Convert.ToString("Ext (" + maxMark + ")").Trim();
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 4].Tag = Convert.ToString(li.Value).Trim();
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 4].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 4].VerticalAlign = VerticalAlign.Middle;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 4].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 4].Resizable = false;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 4].Visible = false;

                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 3].Text = Convert.ToString("Ext R (" + maxMark + ")").Trim();
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 3].Tag = Convert.ToString(li.Value).Trim();
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 3].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 3].VerticalAlign = VerticalAlign.Middle;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 3].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 3].Resizable = false;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 3].Visible = false;

                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 2].Text = Convert.ToString("INT(" + intMax + ")").Trim();
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 2].Tag = Convert.ToString(li.Value).Trim();
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 2].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 2].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 2].Resizable = false;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 2].Visible = false;

                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Text = Convert.ToString("Total").Trim();
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(li.Value).Trim();
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Resizable = false;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Visible = false;

                                        if (cblColumnOrder.Items.Count > 0)
                                        {
                                            bool hasOne = false;
                                            foreach (ListItem liOrder in cblColumnOrder.Items)
                                            {
                                                string value = Convert.ToString(liOrder.Value).Trim();
                                                if (liOrder.Selected)
                                                {
                                                    switch (value)
                                                    {
                                                        case "0":
                                                            FpSpreadTabMarks.Sheets[0].Columns[0].Visible = true;
                                                            break;
                                                        case "1":
                                                            FpSpreadTabMarks.Sheets[0].Columns[1].Visible = true;
                                                            break;
                                                        case "2":
                                                            FpSpreadTabMarks.Sheets[0].Columns[2].Visible = true;
                                                            break;
                                                        case "3":
                                                            valuation1 = true;
                                                            valuationCount++;
                                                            totalValuationMarks += writtenMaxMark;
                                                            if (!hasOne)
                                                            {
                                                                startSpan = 3;
                                                                FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 9, 1, 9);
                                                            }
                                                            hasOne = true;
                                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 9].Visible = true;
                                                            break;
                                                        case "4":
                                                            valuationCount++;
                                                            valuation2 = true;
                                                            totalValuationMarks += writtenMaxMark;
                                                            if (!hasOne)
                                                            {
                                                                startSpan = 4;
                                                                FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 8, 1, 8);
                                                            }
                                                            hasOne = true;
                                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 8].Visible = true;
                                                            break;
                                                        case "5":
                                                            valuationCount++;
                                                            valuation3 = true;
                                                            totalValuationMarks += writtenMaxMark;
                                                            if (!hasOne)
                                                            {
                                                                startSpan = 5;
                                                                FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 7, 1, 7);
                                                            }
                                                            hasOne = true;
                                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 7].Visible = true;
                                                            break;
                                                        case "6":
                                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Text = Convert.ToString("Ext Total(" + Convert.ToString(totalValuationMarks).Trim() + ")").Trim();
                                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Tag = Convert.ToString(li.Value).Trim();
                                                            if (!hasOne)
                                                            {
                                                                startSpan = 6;
                                                                FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 6, 1, 6);
                                                            }
                                                            hasOne = true;
                                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Visible = true;
                                                            break;
                                                        case "12":
                                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Text = Convert.ToString("Ext Total/" + valuationCount + "(" + Convert.ToString(totalValuationMarks / valuationCount).Trim() + ")").Trim();
                                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Tag = Convert.ToString(li.Value).Trim();
                                                            if (!hasOne)
                                                            {
                                                                startSpan = 7;
                                                                FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 5, 1, 5);
                                                            }
                                                            hasOne = true;
                                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Visible = true;
                                                            break;
                                                        case "7":
                                                            if (!hasOne)
                                                            {
                                                                startSpan = 8;
                                                                FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 4, 1, 4);
                                                            }
                                                            hasOne = true;
                                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 4].Visible = true;
                                                            break;
                                                        case "8":
                                                            if (!hasOne)
                                                            {
                                                                startSpan = 9;
                                                                FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 3, 1, 3);
                                                            }
                                                            hasOne = true;
                                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 3].Visible = true;
                                                            break;
                                                        case "9":
                                                            if (!hasOne)
                                                            {
                                                                startSpan = 10;
                                                                FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 2, 1, 2);
                                                            }
                                                            hasOne = true;
                                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 2].Visible = true;
                                                            break;
                                                        case "10":
                                                            if (!hasOne)
                                                            {
                                                                startSpan = 11;
                                                                FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1, 1, 1);
                                                            }
                                                            hasOne = true;
                                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Visible = true;
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                FpSpreadTabMarks.Sheets[0].ColumnCount++;
                                FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Text = Convert.ToString("OVER ALL TOTAL").Trim() + "\n" + maxTotal;
                                //FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 8].Tag = Convert.ToString(li.Value).Trim();
                                FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Locked = true;
                                FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                                FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Locked = true;
                                FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Resizable = false;
                                FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1, 2, 1);
                                FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Visible = false;
                                if (cblColumnOrder.Items.Count > 0)
                                {
                                    foreach (ListItem liOrder in cblColumnOrder.Items)
                                    {
                                        string value = Convert.ToString(liOrder.Value).Trim();
                                        if (liOrder.Selected)
                                        {
                                            switch (value)
                                            {
                                                case "11":
                                                    FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Visible = true;
                                                    break;
                                            }
                                        }
                                    }
                                }

                                for (int stud = 0; stud < dtStudent.Rows.Count; stud++)
                                {
                                    FpSpreadTabMarks.Sheets[0].RowCount++;
                                    string rollNo = Convert.ToString(dtStudent.Rows[stud]["Roll_No"]).Trim();
                                    string studName = Convert.ToString(dtStudent.Rows[stud]["Stud_Name"]).Trim();
                                    string regNo = Convert.ToString(dtStudent.Rows[stud]["Reg_No"]).Trim();
                                    DataView dvStudSubMarks = new DataView();

                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(stud + 1).Trim();
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 0].Locked = true;
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(regNo).Trim();
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(rollNo).Trim();
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 1].Locked = true;
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(studName).Trim();
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 2].Locked = true;
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                    double overAllTotal = 0;
                                    int absentCount = 0;
                                    for (int subcol = 3, spanCol = 0; subcol < FpSpreadTabMarks.Sheets[0].ColumnCount - 1; subcol += 9, spanCol++)
                                    {
                                        dvStudSubMarks = new DataView();
                                        string subNo = Convert.ToString(FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, subcol].Tag).Trim();
                                        ds.Tables[0].DefaultView.RowFilter = "Roll_No='" + rollNo + "' and subject_no='" + subNo + "'";
                                        dvStudSubMarks = ds.Tables[0].DefaultView;
                                        if (dvStudSubMarks.Count > 0)
                                        {
                                            string eval1 = Convert.ToString(dvStudSubMarks[0]["Eval1"]).Trim();
                                            string eval2 = Convert.ToString(dvStudSubMarks[0]["Eval2"]).Trim();
                                            string eval3 = Convert.ToString(dvStudSubMarks[0]["Eval3"]).Trim();
                                            string extTot = Convert.ToString(dvStudSubMarks[0]["Eval3"]).Trim();
                                            string extMark = Convert.ToString(dvStudSubMarks[0]["external_mark"]).Trim();
                                            string extMarkRound = Convert.ToString(dvStudSubMarks[0]["external_mark"]).Trim();
                                            string internalMark = Convert.ToString(dvStudSubMarks[0]["internal_mark"]).Trim();
                                            string totalMarks = Convert.ToString(dvStudSubMarks[0]["total"]).Trim();
                                            string externalMaxMark = Convert.ToString(dvStudSubMarks[0]["max_ext_marks"]).Trim();

                                            bool internalAbsent = false;
                                            bool externalAbsent = false;
                                            double externMaxMArk = 0;
                                            double.TryParse(externalMaxMark, out externMaxMArk);

                                            double evaluation1 = 0;
                                            double evaluation2 = 0;
                                            double evaluation3 = 0;
                                            double evaluationTot = 0;
                                            double externalMark60 = 0;
                                            double externalMarkRounded = 0;
                                            double internalMarks = 0;

                                            double.TryParse(extMarkRound, out externalMarkRounded);
                                            double.TryParse(eval1.Trim(), out evaluation1);
                                            double.TryParse(eval2.Trim(), out evaluation2);
                                            double.TryParse(eval3.Trim(), out evaluation3);
                                            double.TryParse(internalMark, out internalMarks);

                                            int countEmpty = 0;
                                            int countNotEmpty = 0;
                                            double total = 0;
                                            double totalAvg = 0;
                                            double totalRound = 0;
                                            double totalAll = evaluation1 + evaluation2 + evaluation3;

                                            double allValuationMarks = 0;
                                            double averageValuation = 0;
                                            double averageWithoutValuation = 0;
                                            double averageValuationOld = 0;
                                            double externalWithoutValuation = 0;
                                            double externalMarks = 0;
                                            string externalRoundOff = string.Empty;

                                            if (internalMarks < 0)
                                            {
                                                internalMark = "AAA";
                                                internalAbsent = true;
                                            }
                                            if (valuationCount == 0)
                                            {
                                                externalWithoutValuation = externalMarkRounded;
                                            }
                                            if (valuation1)
                                            {
                                                if (string.IsNullOrEmpty(eval1.Trim()) || eval1.Trim() == "0")
                                                {
                                                    countEmpty++;
                                                    allValuationMarks += 0;
                                                }
                                                else
                                                {
                                                    countNotEmpty++;
                                                    if (evaluation1 > 0)
                                                    {
                                                        total = evaluation1;
                                                        allValuationMarks += evaluation1;
                                                    }
                                                    else
                                                    {
                                                        //if (externalMarkRounded < 0)
                                                        //{
                                                        externalAbsent = true;
                                                        //}
                                                        //else
                                                        //{
                                                        //    eval1 = "0";
                                                        //}
                                                    }
                                                }
                                            }
                                            if (valuation2)
                                            {
                                                if (string.IsNullOrEmpty(eval2.Trim()) || eval2.Trim() == "0")
                                                {
                                                    countEmpty++;
                                                    allValuationMarks += 0;
                                                }
                                                else
                                                {
                                                    countNotEmpty++;
                                                    if (evaluation2 > 0)
                                                    {
                                                        total += evaluation2;
                                                        allValuationMarks += evaluation2;
                                                    }
                                                    else
                                                    {
                                                        //if (externalMarkRounded < 0)
                                                        //{
                                                        //    evaluation2 = 0;
                                                        externalAbsent = true;
                                                        //}
                                                        //else
                                                        //{
                                                        //    eval2 = "0";
                                                        //}
                                                    }
                                                }
                                            }
                                            if (valuation3)
                                            {
                                                if (string.IsNullOrEmpty(eval3.Trim()) || eval3.Trim() == "0")
                                                {
                                                    //if (total > 0 && countEmpty == 0)
                                                    //{
                                                    //    total = evaluation1 + evaluation2;
                                                    //    totalAvg = Math.Round((total / 2), 1, MidpointRounding.AwayFromZero);
                                                    //    totalRound = Math.Round((total / 2), 0, MidpointRounding.AwayFromZero);
                                                    //}
                                                    //else if(total > 0 && countEmpty == 1)
                                                    //{
                                                    //    totalAvg = Math.Round((total), 1, MidpointRounding.AwayFromZero);
                                                    //    totalRound = Math.Round((total), 0, MidpointRounding.AwayFromZero); 
                                                    //}
                                                    countEmpty++;
                                                    allValuationMarks += 0;
                                                }
                                                else
                                                {
                                                    countNotEmpty++;
                                                    if (evaluation3 > 0)
                                                    {
                                                        total += evaluation3;
                                                        allValuationMarks += evaluation3;
                                                    }
                                                    else
                                                    {
                                                        //if (externalMarkRounded < 0)
                                                        //{
                                                        externalAbsent = true;
                                                        //}
                                                        //else
                                                        //{
                                                        //    eval3 = "0";
                                                        //}
                                                    }
                                                }
                                            }

                                            if (countNotEmpty == 0)
                                            {
                                                externalMarks = Math.Round((externalMarkRounded), 1, MidpointRounding.AwayFromZero);
                                            }
                                            else if (countNotEmpty > 0 && allValuationMarks > 0)
                                            {
                                                if (valuationCount > 0)
                                                {
                                                    averageValuation = Math.Round((allValuationMarks / valuationCount), 1, MidpointRounding.AwayFromZero);
                                                }
                                                else
                                                {
                                                    averageWithoutValuation = Math.Round((externalWithoutValuation), 1, MidpointRounding.AwayFromZero);
                                                }
                                            }

                                            if (countNotEmpty == 0)
                                            {
                                                double avg = 0;
                                                externalMarks = Math.Round(externalMarks, 1, MidpointRounding.AwayFromZero);
                                            }
                                            else if (countNotEmpty > 0 && !externalAbsent)
                                            {
                                                double avg = totalAvg * (externMaxMArk / writtenMaxMark);
                                                externalMark60 = Math.Round(avg, 1, MidpointRounding.AwayFromZero);
                                                if (valuationCount > 0)
                                                {
                                                    avg = averageValuation * (externMaxMArk / writtenMaxMark);
                                                    externalMarks = Math.Round(avg, 1, MidpointRounding.AwayFromZero);
                                                }
                                                else
                                                {
                                                    avg = externalWithoutValuation * (externMaxMArk / writtenMaxMark);
                                                    externalMarks = Math.Round(avg, 1, MidpointRounding.AwayFromZero);
                                                }
                                            }
                                            else
                                            {
                                                externalMark60 = externalMarkRounded;
                                                externalMarks = externalWithoutValuation;
                                            }

                                            if (externalWithoutValuation < 0)
                                            {
                                                externalRoundOff = "AAA";
                                                externalAbsent = true;
                                            }
                                            else
                                            {
                                                externalRoundOff = Convert.ToString(Math.Round(externalMarks, 0, MidpointRounding.AwayFromZero));
                                            }

                                            if (externalMarkRounded < 0)
                                            {
                                                extMarkRound = "AAA";
                                                externalAbsent = true;
                                            }
                                            else
                                            {
                                                extMarkRound = Convert.ToString(Math.Round(externalMark60, 0, MidpointRounding.AwayFromZero));
                                            }

                                            if (!externalAbsent && !internalAbsent)
                                            {
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol].Text = (externalAbsent) ? "AAA" : Convert.ToString(eval1).Trim();
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol].Font.Name = "Book Antiqua";
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol].Locked = true;
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol].VerticalAlign = VerticalAlign.Middle;

                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 1].Text = (externalAbsent) ? "AAA" : Convert.ToString(eval2).Trim();
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 1].Font.Name = "Book Antiqua";
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 1].Locked = true;
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 1].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 1].VerticalAlign = VerticalAlign.Middle;

                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 2].Text = (externalAbsent) ? "AAA" : Convert.ToString(eval3).Trim();
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 2].Font.Name = "Book Antiqua";
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 2].Locked = true;
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 2].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 2].VerticalAlign = VerticalAlign.Middle;

                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 3].Text = (externalAbsent) ? "AAA" : Convert.ToString(allValuationMarks).Trim();
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 3].Font.Name = "Book Antiqua";
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 3].Locked = true;
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 3].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 3].VerticalAlign = VerticalAlign.Middle;

                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 4].Text = (externalAbsent) ? "AAA" : Convert.ToString(averageValuation).Trim();
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 4].Font.Name = "Book Antiqua";
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 4].Locked = true;
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 4].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 4].VerticalAlign = VerticalAlign.Middle;

                                                string ext60 = string.Format("{0:0.0}", externalMarks);
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 5].Text = (externalAbsent) ? "AAA" : string.Format("{0:0.0}", externalMarks);
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 5].Font.Name = "Book Antiqua";
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 5].Locked = true;
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 5].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 5].VerticalAlign = VerticalAlign.Middle;

                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 6].Text = (externalAbsent) ? "AAA" : Convert.ToString(externalRoundOff).Trim();
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 6].Font.Name = "Book Antiqua";
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 6].Locked = true;
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 6].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 6].VerticalAlign = VerticalAlign.Middle;

                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 7].Text = (internalAbsent) ? "AAA" : Convert.ToString(internalMark).Trim();
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 7].Font.Name = "Book Antiqua";
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 7].Locked = true;
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 7].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 7].VerticalAlign = VerticalAlign.Middle;

                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 8].Text = (internalAbsent || externalAbsent) ? "AAA" : Convert.ToString(totalMarks).Trim();
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 8].Font.Name = "Book Antiqua";
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 8].Locked = true;
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 8].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 8].VerticalAlign = VerticalAlign.Middle;
                                                double calculateTotal = 0;
                                                double.TryParse(Convert.ToString(totalMarks).Trim(), out calculateTotal);
                                                overAllTotal += calculateTotal;
                                            }
                                            else
                                            {
                                                absentCount++;
                                                for (int columns = subcol, span = 9; columns < subcol + 9; columns++, span--)
                                                {
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, columns].Text = "Absent";
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, columns].Font.Name = "Book Antiqua";
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, columns].Locked = true;
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, columns].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, columns].VerticalAlign = VerticalAlign.Middle;
                                                    if (startSpan == 12 - span)
                                                    {
                                                        //FpSpreadTabMarks.Sheets[0].AddSpanCell(FpSpreadTabMarks.Sheets[0].RowCount - 1, ((absentCount == selSubjectCount) ? subcol : columns), 1, ((absentCount == selSubjectCount) ? subcol + 8 : span));
                                                        FpSpreadTabMarks.Sheets[0].AddSpanCell(FpSpreadTabMarks.Sheets[0].RowCount - 1, columns, 1, span);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            absentCount++;
                                            for (int columns = subcol, span = 9; columns < subcol + 9; columns++, span--)
                                            {
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, columns].Text = Convert.ToString("--").Trim();
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, columns].Font.Name = "Book Antiqua";
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, columns].Locked = true;
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, columns].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, columns].VerticalAlign = VerticalAlign.Middle;
                                            }
                                        }
                                    }
                                    if (absentCount == selSubjectCount)
                                    {
                                        //FpSpreadTabMarks.Sheets[0].AddSpanCell(FpSpreadTabMarks.Sheets[0].RowCount - 1, 3, 1, FpSpreadTabMarks.Sheets[0].ColumnCount - 1);
                                        //FpSpreadTabMarks.Sheets[0].SetRowMerge(FpSpreadTabMarks.Sheets[0].RowCount - 1, Farpoint.Model.MergePolicy.Always);
                                    }
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Text = (absentCount != selSubjects) ? Convert.ToString(overAllTotal).Trim() : Convert.ToString("").Trim();
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Locked = true;
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                                    spreadHeight += FpSpreadTabMarks.Sheets[0].Rows[FpSpreadTabMarks.Sheets[0].RowCount - 1].Height;
                                }
                                for (int sh = 0; sh < FpSpreadTabMarks.Sheets[0].ColumnHeader.RowCount; sh++)
                                {
                                    spreadHeight += FpSpreadTabMarks.Sheets[0].ColumnHeader.Rows[sh].Height;
                                }
                                FpSpreadTabMarks.Sheets[0].PageSize = FpSpreadTabMarks.Sheets[0].RowCount;
                                FpSpreadTabMarks.Width = 900;
                                FpSpreadTabMarks.Height = 500;//(spreadHeight) + 45;
                                FpSpreadTabMarks.SaveChanges();
                                FpSpreadTabMarks.Visible = true;
                                divMainContent.Visible = true;
                                rptprint1.Visible = true;
                            }
                            else
                            {
                                lblpopuperr.Text = "No Student Were Found";
                                lblpopuperr.Visible = true;
                                popupdiv.Visible = true;
                                return;
                            }
                        }
                        else
                        {
                            Init_Spread(FpSpreadTabMarks, 1);
                            dtStudent = ds.Tables[0].DefaultView.ToTable(true, "Roll_No", "Reg_No", "Stud_Name", "Batch", "NBatch");
                            ds.Tables[0].DefaultView.RowFilter = "";
                            dtSubjectsList = ds.Tables[0].DefaultView.ToTable(true, "subType_no", "subject_code", "subject_name", "subject_no", "WrittenMaxMark", "min_int_marks", "max_int_marks", "min_ext_marks", "max_ext_marks", "mintotal", "maxtotal");
                            DataTable dtInternalOnly = new DataTable();
                            DataTable dtExternalOnly = new DataTable();
                            DataTable dtInternalExternal = new DataTable();

                            if (dtSubjectsList.Rows.Count > 0)
                            {
                                dtSubjectsList.DefaultView.RowFilter = "max_int_marks>0 and max_ext_marks>0";
                                dtInternalExternal = dtSubjectsList.DefaultView.ToTable();

                                dtSubjectsList.DefaultView.RowFilter = "max_int_marks>0 and max_ext_marks=0 and min_ext_marks=0 and max_int_marks=maxtotal";
                                dtInternalOnly = dtSubjectsList.DefaultView.ToTable();

                                dtSubjectsList.DefaultView.RowFilter = "max_int_marks=0 and max_int_marks=0 and max_ext_marks>0 and max_ext_marks=maxtotal";
                                dtExternalOnly = dtSubjectsList.DefaultView.ToTable();
                            }

                            if (dtStudent.Rows.Count > 0)
                            {
                                int col = 0;
                                double maxTotal = 0;
                                int selSubjects = 0;
                                int subjectColumn = 4;
                                int startSpan = 4;
                                int valuationCount = 0;
                                double totalValuationMarks = 0;

                                bool valuation1 = false;
                                bool valuation2 = false;
                                bool valuation3 = false;


                                for (int stud = 0; stud < dtStudent.Rows.Count; stud++)
                                {
                                    FpSpreadTabMarks.Sheets[0].RowCount++;
                                    string rollNo = Convert.ToString(dtStudent.Rows[stud]["Roll_No"]).Trim();
                                    string studName = Convert.ToString(dtStudent.Rows[stud]["Stud_Name"]).Trim();
                                    string regNo = Convert.ToString(dtStudent.Rows[stud]["Reg_No"]).Trim();
                                    string batchName = Convert.ToString(dtStudent.Rows[stud]["Batch"]).Trim();
                                    string nBatchName = Convert.ToString(dtStudent.Rows[stud]["NBatch"]).Trim();
                                    DataView dvStudSubMarks = new DataView();

                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(stud + 1).Trim();
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 0].Locked = true;
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(regNo).Trim();
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(rollNo).Trim();
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 1].Locked = true;
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(studName).Trim();
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 2].Locked = true;
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(batchName).Trim();
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 3].Locked = true;
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                    spreadHeight += FpSpreadTabMarks.Sheets[0].Rows[FpSpreadTabMarks.Sheets[0].RowCount - 1].Height;
                                }

                                if (dtSubjectsList.Rows.Count > 0)
                                {
                                    Dictionary<string, double> dicTotalMarks = new Dictionary<string, double>();
                                    Dictionary<string, double> dicValuation1 = new Dictionary<string, double>();
                                    Dictionary<string, double> dicValuation2 = new Dictionary<string, double>();
                                    Dictionary<string, double> dicValuation3 = new Dictionary<string, double>();
                                    Dictionary<string, double> dicTotalValuation = new Dictionary<string, double>();
                                    if (dtInternalExternal.Rows.Count > 0)
                                    {
                                        string subjectTypeIE = Convert.ToString(dtInternalExternal.Rows[0]["subType_no"]).Trim();
                                        string subjectCodeIE = Convert.ToString(dtInternalExternal.Rows[0]["subject_code"]).Trim();
                                        string subjectNoIE = Convert.ToString(dtInternalExternal.Rows[0]["subject_no"]).Trim();
                                        string subjectNameIE = Convert.ToString(dtInternalExternal.Rows[0]["subject_name"]).Trim();
                                        string writernMaxIE = Convert.ToString(dtInternalExternal.Rows[0]["WrittenMaxMark"]).Trim();
                                        string minInternalIE = Convert.ToString(dtInternalExternal.Rows[0]["min_int_marks"]).Trim();
                                        string maxInternalIE = Convert.ToString(dtInternalExternal.Rows[0]["max_int_marks"]).Trim();
                                        string minExternalIE = Convert.ToString(dtInternalExternal.Rows[0]["min_ext_marks"]).Trim();
                                        string maxExternalIE = Convert.ToString(dtInternalExternal.Rows[0]["max_ext_marks"]).Trim();
                                        string minTotalIE = Convert.ToString(dtInternalExternal.Rows[0]["mintotal"]).Trim();
                                        string maxTotalIE = Convert.ToString(dtInternalExternal.Rows[0]["maxtotal"]).Trim();
                                        double maxInternal = 0;
                                        double.TryParse(maxInternalIE.Trim(), out maxInternal);
                                        maxTotal += maxInternal;
                                        FpSpreadTabMarks.Sheets[0].ColumnCount++;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Text = "Internal\n(" + Convert.ToString(maxInternalIE).Trim() + ")";
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(subjectNoIE).Trim();
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Resizable = false;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1, 2, 1);
                                        int row = 0;
                                        foreach (DataRow drStudents in dtStudent.Rows)
                                        {
                                            string rollNumber = Convert.ToString(drStudents["Roll_No"]).Trim();

                                            ds.Tables[0].DefaultView.RowFilter = "Roll_No='" + rollNumber + "' and subject_no='" + subjectNoIE + "'";
                                            DataView dvMark = new DataView();
                                            dvMark = ds.Tables[0].DefaultView;
                                            string marks = Convert.ToString(dvMark[0]["internal_mark"]).Trim();
                                            double internalMark = 0;
                                            double.TryParse(marks.Trim(), out internalMark);
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Text = Convert.ToString(marks).Trim();
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;

                                            if (!dicTotalMarks.ContainsKey(rollNumber.Trim()))
                                            {
                                                dicTotalMarks.Add(rollNumber.Trim(), internalMark);
                                            }
                                            else
                                            {
                                                double mark = 0;
                                                mark = dicTotalMarks[rollNumber.Trim()] + internalMark;
                                                dicTotalMarks[rollNumber.Trim()] = mark;
                                            }
                                        }
                                    }
                                    if (dtExternalOnly.Rows.Count > 0)
                                    {
                                        string subjectTypeE = Convert.ToString(dtExternalOnly.Rows[0]["subType_no"]).Trim();
                                        string subjectCodeE = Convert.ToString(dtExternalOnly.Rows[0]["subject_code"]).Trim();
                                        string subjectNoE = Convert.ToString(dtExternalOnly.Rows[0]["subject_no"]).Trim();
                                        string subjectNameE = Convert.ToString(dtExternalOnly.Rows[0]["subject_name"]).Trim();
                                        string writernMaxE = Convert.ToString(dtExternalOnly.Rows[0]["WrittenMaxMark"]).Trim();
                                        string minInternalE = Convert.ToString(dtExternalOnly.Rows[0]["min_int_marks"]).Trim();
                                        string maxInternalE = Convert.ToString(dtExternalOnly.Rows[0]["max_int_marks"]).Trim();
                                        string minExternalE = Convert.ToString(dtExternalOnly.Rows[0]["min_ext_marks"]).Trim();
                                        string maxExternalE = Convert.ToString(dtExternalOnly.Rows[0]["max_ext_marks"]).Trim();
                                        string minTotalE = Convert.ToString(dtExternalOnly.Rows[0]["mintotal"]).Trim();
                                        string maxTotalE = Convert.ToString(dtExternalOnly.Rows[0]["maxtotal"]).Trim();

                                        double maxExternal = 0;
                                        double.TryParse(maxExternalE.Trim(), out maxExternal);
                                        maxTotal += maxExternal;

                                        FpSpreadTabMarks.Sheets[0].ColumnCount++;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Text = Convert.ToString(subjectNameE).Trim() + "\n(" + Convert.ToString(maxExternalE).Trim() + ")";
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(subjectNoE).Trim();
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Resizable = false;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1, 2, 1);

                                        int row = 0;
                                        foreach (DataRow drStudents in dtStudent.Rows)
                                        {
                                            string rollNumber = Convert.ToString(drStudents["Roll_No"]).Trim();

                                            ds.Tables[0].DefaultView.RowFilter = "Roll_No='" + rollNumber + "' and subject_no='" + subjectNoE + "'";
                                            DataView dvMark = new DataView();
                                            dvMark = ds.Tables[0].DefaultView;
                                            string marks = Convert.ToString(dvMark[0]["external_mark"]).Trim();
                                            double externalMark = 0;
                                            double.TryParse(marks.Trim(), out externalMark);
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Text = Convert.ToString(marks).Trim();
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;

                                            if (!dicTotalMarks.ContainsKey(rollNumber.Trim()))
                                            {
                                                dicTotalMarks.Add(rollNumber.Trim(), externalMark);
                                            }
                                            else
                                            {
                                                double mark = 0;
                                                mark = dicTotalMarks[rollNumber.Trim()] + externalMark;
                                                dicTotalMarks[rollNumber.Trim()] = mark;
                                            }
                                        }

                                    }
                                    if (dtInternalOnly.Rows.Count > 0)
                                    {
                                        string subjectTypeI = Convert.ToString(dtInternalOnly.Rows[0]["subType_no"]).Trim();
                                        string subjectCodeI = Convert.ToString(dtInternalOnly.Rows[0]["subject_code"]).Trim();
                                        string subjectNoI = Convert.ToString(dtInternalOnly.Rows[0]["subject_no"]).Trim();
                                        string subjectNameI = Convert.ToString(dtInternalOnly.Rows[0]["subject_name"]).Trim();
                                        string writernMaxI = Convert.ToString(dtInternalOnly.Rows[0]["WrittenMaxMark"]).Trim();
                                        string minInternalI = Convert.ToString(dtInternalOnly.Rows[0]["min_int_marks"]).Trim();
                                        string maxInternalI = Convert.ToString(dtInternalOnly.Rows[0]["max_int_marks"]).Trim();
                                        string minExternalI = Convert.ToString(dtInternalOnly.Rows[0]["min_ext_marks"]).Trim();
                                        string maxExternalI = Convert.ToString(dtInternalOnly.Rows[0]["max_ext_marks"]).Trim();
                                        string minTotalI = Convert.ToString(dtInternalOnly.Rows[0]["mintotal"]).Trim();
                                        string maxTotalI = Convert.ToString(dtInternalOnly.Rows[0]["maxtotal"]).Trim();
                                        double maxInternal = 0;
                                        double.TryParse(maxInternalI.Trim(), out maxInternal);
                                        maxTotal += maxInternal;

                                        FpSpreadTabMarks.Sheets[0].ColumnCount++;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Text = Convert.ToString(subjectNameI).Trim() + "\n(" + Convert.ToString(maxInternalI).Trim() + ")";
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(subjectNoI).Trim();
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Resizable = false;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1, 2, 1);
                                        int row = 0;
                                        foreach (DataRow drStudents in dtStudent.Rows)
                                        {
                                            string rollNumber = Convert.ToString(drStudents["Roll_No"]).Trim();

                                            ds.Tables[0].DefaultView.RowFilter = "Roll_No='" + rollNumber + "' and subject_no='" + subjectNoI + "'";
                                            DataView dvMark = new DataView();
                                            dvMark = ds.Tables[0].DefaultView;
                                            string marks = Convert.ToString(dvMark[0]["internal_mark"]).Trim();
                                            double internalMark = 0;
                                            double.TryParse(marks.Trim(), out internalMark);
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Text = Convert.ToString(marks).Trim();
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;

                                            if (!dicTotalMarks.ContainsKey(rollNumber.Trim()))
                                            {
                                                dicTotalMarks.Add(rollNumber.Trim(), internalMark);
                                            }
                                            else
                                            {
                                                double mark = 0;
                                                mark = dicTotalMarks[rollNumber.Trim()] + internalMark;
                                                dicTotalMarks[rollNumber.Trim()] = mark;
                                            }

                                        }
                                    }
                                    if (dtInternalExternal.Rows.Count > 0)
                                    {
                                        string subjectTypeIE = Convert.ToString(dtInternalExternal.Rows[0]["subType_no"]).Trim();
                                        string subjectCodeIE = Convert.ToString(dtInternalExternal.Rows[0]["subject_code"]).Trim();
                                        string subjectNoIE = Convert.ToString(dtInternalExternal.Rows[0]["subject_no"]).Trim();
                                        string subjectNameIE = Convert.ToString(dtInternalExternal.Rows[0]["subject_name"]).Trim();
                                        string writernMaxIE = Convert.ToString(dtInternalExternal.Rows[0]["WrittenMaxMark"]).Trim();
                                        string minInternalIE = Convert.ToString(dtInternalExternal.Rows[0]["min_int_marks"]).Trim();
                                        string maxInternalIE = Convert.ToString(dtInternalExternal.Rows[0]["max_int_marks"]).Trim();
                                        string minExternalIE = Convert.ToString(dtInternalExternal.Rows[0]["min_ext_marks"]).Trim();
                                        string maxExternalIE = Convert.ToString(dtInternalExternal.Rows[0]["max_ext_marks"]).Trim();
                                        string minTotalIE = Convert.ToString(dtInternalExternal.Rows[0]["mintotal"]).Trim();
                                        string maxTotalIE = Convert.ToString(dtInternalExternal.Rows[0]["maxtotal"]).Trim();
                                        double maxExternal = 0;
                                        double.TryParse(maxExternalIE.Trim(), out maxExternal);
                                        maxTotal += maxExternal;

                                        FpSpreadTabMarks.Sheets[0].ColumnCount += 6;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Text = Convert.ToString("Thesis\nExternal 1\n").Trim() + "\n(" + Convert.ToString(writernMaxIE).Trim() + ")";
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Tag = Convert.ToString(subjectNoIE).Trim();
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 6].VerticalAlign = VerticalAlign.Middle;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Visible = false;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Resizable = false;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 6, 2, 1);


                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Text = Convert.ToString("Thesis\nExternal 2\n").Trim() + "\n(" + Convert.ToString(writernMaxIE).Trim() + ")";
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Tag = Convert.ToString(subjectNoIE).Trim();
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 5].VerticalAlign = VerticalAlign.Middle;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Resizable = false;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Visible = false;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 5, 2, 1);

                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 4].Text = Convert.ToString("Thesis\nExternal 3\n").Trim() + "\n(" + Convert.ToString(writernMaxIE).Trim() + ")";
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 4].Tag = Convert.ToString(subjectNoIE).Trim();
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 4].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 4].VerticalAlign = VerticalAlign.Middle;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 4].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 4].Resizable = false;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 4].Visible = false;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 4, 2, 1);

                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 3].Text = Convert.ToString("Average\n").Trim() + "\n(" + Convert.ToString(writernMaxIE).Trim() + ")";
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 3].Tag = Convert.ToString(subjectNoIE).Trim();
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 3].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 3].VerticalAlign = VerticalAlign.Middle;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 3].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 3].Resizable = false;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 3].Visible = false;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 3, 2, 1);

                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 2].Text = Convert.ToString("External\n").Trim() + "\n(" + Convert.ToString(maxExternalIE).Trim() + ")";
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 2].Tag = Convert.ToString(subjectNoIE).Trim();
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 2].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 2].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 2].Resizable = false;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 2].Visible = false;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 2, 2, 1);

                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Text = Convert.ToString("Total\n").Trim() + "\n(" + Convert.ToString(maxTotal).Trim() + ")";
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(subjectNoIE).Trim();
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Resizable = false;
                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Visible = false;
                                        FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1, 2, 1);

                                        valuationCount = 0;
                                        if (cblColumnOrder.Items.Count > 0)
                                        {
                                            foreach (ListItem liOrder in cblColumnOrder.Items)
                                            {
                                                string selValue = liOrder.Value;
                                                if (liOrder.Selected)
                                                {
                                                    switch (selValue)
                                                    {
                                                        case "3":
                                                            valuationCount++;
                                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Visible = true;
                                                            break;
                                                        case "4":
                                                            valuationCount++;
                                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Visible = true;
                                                            break;
                                                        case "5":
                                                            valuationCount++;
                                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 4].Visible = true;
                                                            break;
                                                        case "12":
                                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 3].Visible = true;
                                                            break;
                                                        case "7":
                                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 2].Visible = true;
                                                            break;
                                                        case "10":
                                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Visible = true;
                                                            break;
                                                    }
                                                }
                                            }
                                        }

                                        int row = 0;
                                        foreach (DataRow drStudents in dtStudent.Rows)
                                        {
                                            string rollNumber = Convert.ToString(drStudents["Roll_No"]).Trim();

                                            ds.Tables[0].DefaultView.RowFilter = "Roll_No='" + rollNumber + "' and subject_no='" + subjectNoIE + "'";
                                            DataView dvMark = new DataView();
                                            dvMark = ds.Tables[0].DefaultView;
                                            string marks = Convert.ToString(dvMark[0]["internal_mark"]).Trim();
                                            string evaluation1 = Convert.ToString(dvMark[0]["Eval1"]).Trim();
                                            string evaluation2 = Convert.ToString(dvMark[0]["Eval2"]).Trim();
                                            string evaluation3 = Convert.ToString(dvMark[0]["Eval3"]).Trim();
                                            string externalMarks = Convert.ToString(dvMark[0]["external_mark"]).Trim();
                                            string internalMarks = Convert.ToString(dvMark[0]["internal_mark"]).Trim();
                                            string totalMarks = Convert.ToString(dvMark[0]["total"]).Trim();

                                            string average = string.Empty;

                                            double eval1 = 0;
                                            double eval2 = 0;
                                            double eval3 = 0;

                                            double TotalValuation = 0;
                                            double.TryParse(evaluation1.Trim(), out eval1);
                                            double.TryParse(evaluation2.Trim(), out eval2);
                                            double.TryParse(evaluation3.Trim(), out eval3);

                                            double internalMark = 0;
                                            double.TryParse(internalMarks.Trim(), out internalMark);

                                            double externalMark = 0;
                                            double.TryParse(externalMarks.Trim(), out externalMark);

                                            //valuationCount = 0;
                                            if (cblColumnOrder.Items.Count > 0)
                                            {
                                                foreach (ListItem liOrder in cblColumnOrder.Items)
                                                {
                                                    string selValue = liOrder.Value;
                                                    if (liOrder.Selected)
                                                    {
                                                        switch (selValue)
                                                        {
                                                            case "3":
                                                                TotalValuation += eval1;
                                                                //valuationCount++;
                                                                break;
                                                            case "4":
                                                                TotalValuation += eval2;
                                                                //valuationCount++;
                                                                break;
                                                            case "5":
                                                                TotalValuation += eval3;
                                                                //valuationCount++;
                                                                break;
                                                        }
                                                    }
                                                }
                                            }
                                            double averageValuation = 0;
                                            if (valuationCount > 0)
                                            {
                                                averageValuation = TotalValuation / valuationCount;
                                                averageValuation = Math.Round(averageValuation, 0, MidpointRounding.AwayFromZero);
                                            }
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Text = Convert.ToString(evaluation1).Trim();
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Font.Name = "Book Antiqua";
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 6].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 6].VerticalAlign = VerticalAlign.Middle;

                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Text = Convert.ToString(evaluation2).Trim();
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Font.Name = "Book Antiqua";
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 5].VerticalAlign = VerticalAlign.Middle;

                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 4].Text = Convert.ToString(evaluation3).Trim();
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 4].Font.Name = "Book Antiqua";
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 4].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 4].VerticalAlign = VerticalAlign.Middle;

                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 3].Text = Convert.ToString(averageValuation).Trim();
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 3].Font.Name = "Book Antiqua";
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 3].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 3].VerticalAlign = VerticalAlign.Middle;

                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 2].Text = Convert.ToString(externalMarks).Trim();
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 2].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;

                                            double overallTotalMarks = 0;
                                            if (!dicTotalMarks.ContainsKey(rollNumber.Trim()))
                                            {
                                                dicTotalMarks.Add(rollNumber.Trim(), internalMark + externalMark);
                                                overallTotalMarks = internalMark + externalMark;
                                            }
                                            else
                                            {
                                                double mark = 0;
                                                mark = dicTotalMarks[rollNumber.Trim()] + externalMark;
                                                dicTotalMarks[rollNumber.Trim()] = mark;
                                                overallTotalMarks = dicTotalMarks[rollNumber.Trim()];
                                            }

                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Text = Convert.ToString(overallTotalMarks).Trim();
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpreadTabMarks.Sheets[0].Cells[row, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;

                                        }

                                    }
                                }

                                for (int sh = 0; sh < FpSpreadTabMarks.Sheets[0].ColumnHeader.RowCount; sh++)
                                {
                                    spreadHeight += FpSpreadTabMarks.Sheets[0].ColumnHeader.Rows[sh].Height;
                                }
                                FpSpreadTabMarks.Sheets[0].PageSize = FpSpreadTabMarks.Sheets[0].RowCount;
                                FpSpreadTabMarks.Width = 1000;
                                FpSpreadTabMarks.Height = 500;//(spreadHeight) + 45;
                                FpSpreadTabMarks.SaveChanges();
                                FpSpreadTabMarks.Visible = true;
                                divMainContent.Visible = true;
                                rptprint1.Visible = true;
                            }
                            else
                            {
                                lblpopuperr.Text = "No Student Were Found";
                                lblpopuperr.Visible = true;
                                popupdiv.Visible = true;
                                return;
                               
                            }
                        }
                    }
                    else  //added by Mullai
                    {
                        string qry1 = "select r.roll_no,r.Reg_No,r.Stud_Name,c.total,c.subject_no from camarks c,Registration r,subject s where c.roll_no=r.Roll_No and c.subject_no=s.subject_no and r.degree_code='" + degree_code.Trim() + "' and r.Batch_Year='" + batch_year.Trim() + "' and s.subject_no in(" + subjectNo.Trim() + ") and r.Roll_No=c.roll_no order by r.roll_no";
                        DataSet dsmk = d2.select_method_wo_parameter(qry1, "text");
                        if (dsmk.Tables.Count > 0 && dsmk.Tables[0].Rows.Count > 0)
                        {
                            norec=false;
                            DataTable dtStudent=new DataTable();
                            if (ddlReportFormat.SelectedIndex == 0)
                            {
                                Init_Spread(FpSpreadTabMarks);
                                dtStudent = dsmk.Tables[0].DefaultView.ToTable(true, "Roll_No", "Reg_No", "Stud_Name");

                                if (dtStudent.Rows.Count > 0)
                                {
                                    Init_Spread(FpSpreadTabMarks);
                                    int col = 0;
                                    double maxTotal = 0;
                                    int selSubjects = 0;
                                    int subjectColumn = 3;
                                    int startSpan = 3;
                                    int valuationCount = 0;
                                    double totalValuationMarks = 0;

                                    bool valuation1 = false;
                                    bool valuation2 = false;
                                    bool valuation3 = false;
                                    foreach (ListItem li in cblSubjects.Items)
                                    {

                                        if (li.Selected)
                                        {
                                            string intMax = d2.GetFunctionv("select max_int_marks from subject where subject_no='" + li.Value.Trim() + "'");
                                            maxTotal += Convert.ToDouble(intMax);
                                            FpSpreadTabMarks.Sheets[0].ColumnCount += 9;
                                            Farpoint.StyleInfo MyStyle = new Farpoint.StyleInfo();
                                            MyStyle.Font.Size = FontUnit.Medium;
                                            MyStyle.Font.Name = "Book Antiqua";
                                            MyStyle.Font.Bold = true;
                                            MyStyle.HorizontalAlign = HorizontalAlign.Center;
                                            MyStyle.ForeColor = Color.Black;
                                            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                                            for (int columns = selSubjects * 9 + subjectColumn; columns < FpSpreadTabMarks.Sheets[0].ColumnCount; columns++)
                                            {
                                                if (cbsubcode.Checked == true)  
                                                {
                                                    string subcode = d2.GetFunction("select subject_code from subject where subject_no='" + li.Value + "'");
                                                    FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, columns].Text = Convert.ToString(subcode).Trim();
                                                  //  FpSpreadTabMarks.Sheets[0].Columns[columns].Width = 150;
                                                }
                                                else
                                                {
                                                    FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, columns].Text = Convert.ToString(li.Text).Trim();
                                                }
                                                FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, columns].Tag = Convert.ToString(li.Value).Trim();

                                                FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, columns].Locked = true;
                                                FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, columns].VerticalAlign = VerticalAlign.Middle;
                                                FpSpreadTabMarks.Sheets[0].Columns[columns].Locked = true;
                                                FpSpreadTabMarks.Sheets[0].Columns[columns].Resizable = false;
                                            }
                                            selSubjects++;
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 9].Text = Convert.ToString("Ext 1st").Trim();
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 9].Tag = Convert.ToString(li.Value).Trim();
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 9].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 9].VerticalAlign = VerticalAlign.Middle;
                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 9].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 9].Resizable = false;
                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 9].Visible = false;

                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 8].Text = Convert.ToString("Ext 2nd").Trim();
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 8].Tag = Convert.ToString(li.Value).Trim();
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 8].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 8].VerticalAlign = VerticalAlign.Middle;
                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 8].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 8].Resizable = false;
                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 8].Visible = false;

                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 7].Text = Convert.ToString("Ext 3rd").Trim();
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 7].Tag = Convert.ToString(li.Value).Trim();
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 7].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 7].VerticalAlign = VerticalAlign.Middle;
                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 7].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 7].Resizable = false;
                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 7].Visible = false;

                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Text = Convert.ToString("Ext Total").Trim();
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Tag = Convert.ToString(li.Value).Trim();
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 6].VerticalAlign = VerticalAlign.Middle;
                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Resizable = false;
                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Visible = false;

                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Text = Convert.ToString("Ext Avg").Trim();
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Tag = Convert.ToString(li.Value).Trim();
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 5].VerticalAlign = VerticalAlign.Middle;
                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Resizable = false;
                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Visible = false;

                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 4].Text = Convert.ToString("Ext Mark").Trim();
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 4].Tag = Convert.ToString(li.Value).Trim();
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 4].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 4].VerticalAlign = VerticalAlign.Middle;
                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 4].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 4].Resizable = false;
                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 4].Visible = false;

                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 3].Text = Convert.ToString("Ext R").Trim();
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 3].Tag = Convert.ToString(li.Value).Trim();
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 3].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 3].VerticalAlign = VerticalAlign.Middle;
                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 3].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 3].Resizable = false;
                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 3].Visible = false;

                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 2].Text = Convert.ToString("INT(" + intMax + ")").Trim();
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 2].Tag = Convert.ToString(li.Value).Trim();
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 2].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;
                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 2].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 2].Resizable = false;
                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 2].Visible = false;

                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Text = Convert.ToString("Total").Trim();
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(li.Value).Trim();
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Locked = true;
                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Resizable = false;
                                            FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Visible = false;

                                            if (cblColumnOrder.Items.Count > 0)
                                            {
                                                bool hasOne = false;
                                                foreach (ListItem liOrder in cblColumnOrder.Items)
                                                {
                                                    string value = Convert.ToString(liOrder.Value).Trim();
                                                    if (liOrder.Selected)
                                                    {
                                                        switch (value)
                                                        {
                                                            case "0":
                                                                FpSpreadTabMarks.Sheets[0].Columns[0].Visible = true;
                                                                break;
                                                            case "1":
                                                                FpSpreadTabMarks.Sheets[0].Columns[1].Visible = true;
                                                                break;
                                                            case "2":
                                                                FpSpreadTabMarks.Sheets[0].Columns[2].Visible = true;
                                                                break;
                                                            case "3":
                                                                valuation1 = true;
                                                                valuationCount++;
                                                                totalValuationMarks += writtenMaxMark;
                                                                if (!hasOne)
                                                                {
                                                                    startSpan = 3;
                                                                    FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 9, 1, 9);
                                                                }
                                                                hasOne = true;
                                                                FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 9].Visible = true;
                                                                break;
                                                            case "4":
                                                                valuationCount++;
                                                                valuation2 = true;
                                                                totalValuationMarks += writtenMaxMark;
                                                                if (!hasOne)
                                                                {
                                                                    startSpan = 4;
                                                                    FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 8, 1, 8);
                                                                }
                                                                hasOne = true;
                                                                FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 8].Visible = true;
                                                                break;
                                                            case "5":
                                                                valuationCount++;
                                                                valuation3 = true;
                                                                totalValuationMarks += writtenMaxMark;
                                                                if (!hasOne)
                                                                {
                                                                    startSpan = 5;
                                                                    FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 7, 1, 7);
                                                                }
                                                                hasOne = true;
                                                                FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 7].Visible = true;
                                                                break;
                                                            case "6":
                                                                FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Text = Convert.ToString("Ext Total(" + Convert.ToString(totalValuationMarks).Trim() + ")").Trim();
                                                                FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Tag = Convert.ToString(li.Value).Trim();
                                                                if (!hasOne)
                                                                {
                                                                    startSpan = 6;
                                                                    FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 6, 1, 6);
                                                                }
                                                                hasOne = true;
                                                                FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 6].Visible = true;
                                                                break;
                                                            case "12":
                                                                FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Text = Convert.ToString("Ext Total/" + valuationCount + "(" + Convert.ToString(totalValuationMarks / valuationCount).Trim() + ")").Trim();
                                                                FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[1, FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Tag = Convert.ToString(li.Value).Trim();
                                                                if (!hasOne)
                                                                {
                                                                    startSpan = 7;
                                                                    FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 5, 1, 5);
                                                                }
                                                                hasOne = true;
                                                                FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 5].Visible = true;
                                                                break;
                                                            case "7":
                                                                if (!hasOne)
                                                                {
                                                                    startSpan = 8;
                                                                    FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 4, 1, 4);
                                                                }
                                                                hasOne = true;
                                                                FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 4].Visible = true;
                                                                break;
                                                            case "8":
                                                                if (!hasOne)
                                                                {
                                                                    startSpan = 9;
                                                                    FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 3, 1, 3);
                                                                }
                                                                hasOne = true;
                                                                FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 3].Visible = true;
                                                                break;
                                                            case "9":
                                                                if (!hasOne)
                                                                {
                                                                    startSpan = 10;
                                                                    FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 2, 1, 2);
                                                                }
                                                                hasOne = true;
                                                                FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 2].Visible = true;
                                                                break;
                                                            case "10":
                                                                if (!hasOne)
                                                                {
                                                                    startSpan = 11;
                                                                    FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1, 1, 1);
                                                                }
                                                                hasOne = true;
                                                                FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Visible = true;
                                                                break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    FpSpreadTabMarks.Sheets[0].ColumnCount++;
                                    FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Text = Convert.ToString("OVER ALL TOTAL").Trim() + "\n" + maxTotal;
                                    FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Locked = true;
                                    FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                                    FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Locked = true;
                                    FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Resizable = false;
                                    FpSpreadTabMarks.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpreadTabMarks.Sheets[0].ColumnCount - 1, 2, 1);
                                    FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Visible = false;
                                    if (cblColumnOrder.Items.Count > 0)
                                    {
                                        foreach (ListItem liOrder in cblColumnOrder.Items)
                                        {
                                            string value = Convert.ToString(liOrder.Value).Trim();
                                            if (liOrder.Selected)
                                            {
                                                switch (value)
                                                {
                                                    case "11":
                                                        FpSpreadTabMarks.Sheets[0].Columns[FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Visible = true;
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                    for (int j1 = 0; j1 < dtStudent.Rows.Count; j1++)
                                    {

                                        string rollno = Convert.ToString(dtStudent.Rows[j1]["roll_no"]);
                                        string regno = Convert.ToString(dtStudent.Rows[j1]["Reg_No"]);
                                        string studnam = Convert.ToString(dtStudent.Rows[j1]["Stud_Name"]);
                                       // string tot = Convert.ToString(dsmk.Tables[0].Rows[j1]["total"]);
                                        FpSpreadTabMarks.Sheets[0].RowCount++;
                                        FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(j1 + 1).Trim();
                                        FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                        FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 0].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                                        FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(regno).Trim();
                                        FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(rollno).Trim();
                                        FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                        FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 1].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                                        FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(studnam).Trim();
                                        FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                        FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 2].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                                        double overAllTotal = 0;
                                        int absentCount = 0;
                                        double internalMarks = 0;
                                        string internalMark = string.Empty;
                                        Boolean internalAbsent = false;
                                        for (int subcol = 3, spanCol = 0; subcol < FpSpreadTabMarks.Sheets[0].ColumnCount - 1; subcol += 9, spanCol++)
                                        {
                                           DataView dvStudSubMarks = new DataView();
                                            string subNo = Convert.ToString(FpSpreadTabMarks.Sheets[0].ColumnHeader.Cells[0, subcol].Tag).Trim();
                                            dsmk.Tables[0].DefaultView.RowFilter = "Roll_No='" + rollno + "' and subject_no='" + subNo + "'";
                                            dvStudSubMarks = dsmk.Tables[0].DefaultView;
                                            if (dvStudSubMarks.Count > 0)
                                            {
                                                string tot = Convert.ToString(dvStudSubMarks[0]["total"]).Trim();
                                                double.TryParse(tot, out internalMarks);
                                                if (internalMarks < 0)
                                                {
                                                    internalMark = "AAA";
                                                    internalAbsent = true;
                                                }
                                                if (!internalAbsent)
                                                {
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol].Text = " ";
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol].Font.Name = "Book Antiqua";
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol].Locked = true;
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol].VerticalAlign = VerticalAlign.Middle;

                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 1].Text = " ";
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 1].Font.Name = "Book Antiqua";
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 1].Locked = true;
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 1].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 1].VerticalAlign = VerticalAlign.Middle;

                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 2].Text = " ";
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 2].Font.Name = "Book Antiqua";
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 2].Locked = true;
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 2].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 2].VerticalAlign = VerticalAlign.Middle;

                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 3].Text = " ";
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 3].Font.Name = "Book Antiqua";
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 3].Locked = true;
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 3].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 3].VerticalAlign = VerticalAlign.Middle;

                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 4].Text = " ";
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 4].Font.Name = "Book Antiqua";
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 4].Locked = true;
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 4].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 4].VerticalAlign = VerticalAlign.Middle;

                                                   
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 5].Text = " ";
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 5].Font.Name = "Book Antiqua";
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 5].Locked = true;
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 5].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 5].VerticalAlign = VerticalAlign.Middle;

                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 6].Text = " ";
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 6].Font.Name = "Book Antiqua";
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 6].Locked = true;
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 6].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 6].VerticalAlign = VerticalAlign.Middle;

                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 7].Text = Convert.ToString(internalMarks).Trim();
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 7].Font.Name = "Book Antiqua";
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 7].Locked = true;
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 7].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 7].VerticalAlign = VerticalAlign.Middle;

                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 8].Text = " ";
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 8].Font.Name = "Book Antiqua";
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 8].Locked = true;
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 8].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, subcol + 8].VerticalAlign = VerticalAlign.Middle;
                                                    double calculateTotal = 0;
                                                    double.TryParse(Convert.ToString(internalMarks).Trim(), out calculateTotal);
                                                    overAllTotal += calculateTotal;
                                                }
                                                else
                                                {
                                                    absentCount++;
                                                    for (int columns = subcol, span = 9; columns < subcol + 9; columns++, span--)
                                                    {
                                                        FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, columns].Text = "Absent";
                                                        FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, columns].Font.Name = "Book Antiqua";
                                                        FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, columns].Locked = true;
                                                        FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, columns].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, columns].VerticalAlign = VerticalAlign.Middle;
                                                        if (startSpan == 12 - span)
                                                        {

                                                            FpSpreadTabMarks.Sheets[0].AddSpanCell(FpSpreadTabMarks.Sheets[0].RowCount - 1, columns, 1, span);
                                                        }
                                                    }
                                                }
                                            }
                                        
                                        }

                                        FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Text = (absentCount != selSubjects) ? Convert.ToString(overAllTotal).Trim() : Convert.ToString("").Trim();
                                        FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                        FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].Locked = true;
                                        FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpreadTabMarks.Sheets[0].Cells[FpSpreadTabMarks.Sheets[0].RowCount - 1, FpSpreadTabMarks.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                                        spreadHeight += FpSpreadTabMarks.Sheets[0].Rows[FpSpreadTabMarks.Sheets[0].RowCount - 1].Height;

                                    }


                                    for (int sh = 0; sh < FpSpreadTabMarks.Sheets[0].ColumnHeader.RowCount; sh++)
                                    {
                                        spreadHeight += FpSpreadTabMarks.Sheets[0].ColumnHeader.Rows[sh].Height;
                                    }
                                    FpSpreadTabMarks.Sheets[0].PageSize = FpSpreadTabMarks.Sheets[0].RowCount;
                                    FpSpreadTabMarks.Width = 900;
                                    FpSpreadTabMarks.Height = 500;
                                    FpSpreadTabMarks.SaveChanges();
                                    FpSpreadTabMarks.Visible = true;
                                    divMainContent.Visible = true;
                                    rptprint1.Visible = true;
                                }
                                else
                                {
                                    lblpopuperr.Text = "No Student Were Found";
                                    lblpopuperr.Visible = true;
                                    popupdiv.Visible = true;
                                    return;
                                }
                            }
                        }
                    }


                    if (norec == true)
                    {
                        lblpopuperr.Text = "Marks Were Not Found";
                        lblpopuperr.Visible = true;
                        popupdiv.Visible = true;
                        return;
                    }
                    
                }

                else
                {
                    lblpopuperr.Text = "Exam is Not Created";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
            }
            else
            {
                lblpopuperr.Text = "No Record(s) Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
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

    #endregion Go Click

    #region Close Popup

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Close Popup

    #region Print Excel

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = "";
            lblErrSearch.Visible = false;
            Printcontrol1.Visible = false;
            string reportname = txtexcelname1.Text;
            if (Convert.ToString(reportname).Trim() != "")
            {
                if (FpSpreadTabMarks.Visible == true)
                {
                    d2.printexcelreport(FpSpreadTabMarks, reportname);
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
            lblErrSearch.Text = "";
            lblErrSearch.Visible = false;
            string rptheadname = "Tabulated Marks/Results - Written Examination";
            string pagename = "TabulatedMarkResults.aspx";
            if (FpSpreadTabMarks.Visible == true)
            {
                Printcontrol1.loadspreaddetails(FpSpreadTabMarks, pagename, rptheadname.ToUpper());
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

    #region Print PDF Using iTextSharp Class

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpreadTabMarks.Sheets[0].FrozenColumnCount = 0;
            FpSpreadTabMarks.Sheets[0].AutoPostBack = true;
            FpSpreadTabMarks.SaveChanges();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Print PDF Using iTextSharp Class

    #endregion Button Events

    #region Column Order

    #region Added By Malang Raja on Oct 20 2016

    protected void chkColumnOrderAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContent.Visible = false;
            rptprint1.Visible = false;
            if (chkColumnOrderAll.Checked == true)
            {
                txtOrder.Text = string.Empty;
                ItemList.Clear();
                Itemindex.Clear();
                for (int i = 0; i < cblColumnOrder.Items.Count; i++)
                {
                    string si = Convert.ToString(i).Trim();
                    cblColumnOrder.Items[i].Selected = true;
                    lbtnRemoveAll.Visible = true;
                    ItemList.Add(Convert.ToString(cblColumnOrder.Items[i].Text).Trim());
                    Itemindex.Add(si);
                }
                lbtnRemoveAll.Visible = true;
                txtOrder.Visible = true;
                txtOrder.Text = string.Empty;
                int j = 0;
                string colname12 = string.Empty;
                for (int i = 0; i < ItemList.Count; i++)
                {
                    j = j + 1;
                    if (colname12 == "")
                    {
                        colname12 = Convert.ToString(ItemList[i]).Trim() + "(" + Convert.ToString(j).Trim() + ")";
                    }
                    else
                    {
                        colname12 = colname12 + "," + Convert.ToString(ItemList[i]).Trim() + "(" + Convert.ToString(j).Trim() + ")";
                    }
                }
                txtOrder.Text = colname12;
            }
            else
            {

                ItemList.Clear();
                Itemindex.Clear();
                for (int i = 0; i < cblColumnOrder.Items.Count; i++)
                {
                    cblColumnOrder.Items[i].Selected = false;
                }
                lbtnRemoveAll.Visible = false;
                txtOrder.Text = string.Empty;
                txtOrder.Visible = false;
            }
            txtOrder.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void lbtnRemoveAll_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContent.Visible = false;
            rptprint1.Visible = false;
            cblColumnOrder.ClearSelection();
            chkColumnOrderAll.Checked = false;
            lbtnRemoveAll.Visible = false;
            ItemList.Clear();
            Itemindex.Clear();
            txtOrder.Text = string.Empty;
            txtOrder.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void cblColumnOrder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContent.Visible = false;
            rptprint1.Visible = false;

            chkColumnOrderAll.Checked = false;
            string value = "";
            int index;
            //cblColumnOrder.Items[0].Selected = true;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index).Trim();
            if (cblColumnOrder.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    ItemList.Add(Convert.ToString(cblColumnOrder.Items[index].Text).Trim());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(Convert.ToString(cblColumnOrder.Items[index].Text).Trim());
                Itemindex.Remove(sindex);
            }
            for (int i = 0; i < cblColumnOrder.Items.Count; i++)
            {
                if (cblColumnOrder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i).Trim();
                    ItemList.Remove(Convert.ToString(cblColumnOrder.Items[i].Text).Trim());
                    Itemindex.Remove(sindex);
                }
            }

            lbtnRemoveAll.Visible = true;
            txtOrder.Visible = false;
            txtOrder.Text = "";
            string colname12 = "";
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (colname12 == "")
                {
                    colname12 = Convert.ToString(ItemList[i]).Trim() + "(" + Convert.ToString((i + 1)).Trim() + ")";
                }
                else
                {
                    colname12 = colname12 + "," + Convert.ToString(ItemList[i]).Trim() + "(" + Convert.ToString((i + 1)).Trim() + ")";
                }
            }
            txtOrder.Text = colname12;
            if (ItemList.Count == 14)
            {
                chkColumnOrderAll.Checked = true;
            }
            if (ItemList.Count == 0)
            {
                txtOrder.Visible = false;
                lbtnRemoveAll.Visible = false;
            }
            txtOrder.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion

    #endregion

}