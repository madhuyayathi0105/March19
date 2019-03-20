using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using Farpoint = FarPoint.Web.Spread;
using System.Configuration;

public partial class CAM_Calculation_Settings_New : System.Web.UI.Page
{

    #region Field Declaration

    Hashtable hat = new Hashtable();

    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    string batch_year = string.Empty;
    string degree_code = string.Empty;
    string semester = string.Empty;
    string section = string.Empty;

    string test_name = string.Empty;
    string test_no = string.Empty;
    string subject_no = string.Empty;

    string exam_type = string.Empty;
    string exam_code = string.Empty;

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
            ////****************************************************//
            //string PageLogOut = "";
            //string sess = Convert.ToString(Session["IsLogin"]);
            //PageLogOut = Convert.ToString(Session["PageLogout"]);
            //if (sess == "")
            //{
            //}
            //else
            //{
            //    if (!Request.FilePath.Contains("QuesM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["Question"].Trim();
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

            if (!Request.FilePath.Contains("QuestionsMasterHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/QuestionMOD/QuestionsMasterHome.aspx");
                    return;
                }
            }
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
                divSettings.Visible = false;
                divSavedSettings.Visible = false;
                //txtConvertedTo.Attributes.Add("type", "number");
                //txtConvertedTo.Attributes.Add("max", "100");
                //txtConvertedTo.Attributes.Add("min", "1");
                //txtConvertedTo.Attributes.Add("maxlength", "3");

                #region LoadHeader

                Bindcollege();
                BindBatch();
                BindDegree();
                bindbranch();
                bindsem();
                BindSectionDetail();
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
                //Init_Spread();
            }
        }
        catch (Exception ex) { }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        List<string> keys = Request.Form.AllKeys.Where(key => key.Contains("txtConvertTo")).ToList();
        int i = 1;
        //tblMax.Rows.Clear();
        //tblMax.Controls.Clear();
        foreach (string key in keys)
        {
            this.CreateTextBox("txtConvertTo" + i, tblMax, i);
            i++;
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
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
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
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
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
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

    public void BindSectionDetail()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            string strbatch = Convert.ToString(ddlbatch.SelectedValue);
            string strbranch = Convert.ToString(ddlbranch.SelectedValue);

            ddlsec.Items.Clear();
            ds.Dispose();
            ds.Reset();
            ds = d2.BindSectionDetail(strbatch, strbranch);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsec.DataSource = ds;
                ddlsec.DataTextField = "sections";
                ddlsec.DataBind();
                if (Convert.ToString(ds.Tables[0].Columns["sections"]) == string.Empty)
                {
                    ddlsec.Enabled = false;
                }
                else
                {
                    ddlsec.Enabled = true;
                }
            }
            else
            {
                ddlsec.Enabled = false;
            }
            GetSubject();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
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
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

    public void GetSubject()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            string subjectquery = string.Empty;
            string sections = string.Empty;
            string strsec = "";
            cblSubjects.Items.Clear();
            if (ddlsec.Items.Count > 0)
            {
                sections = Convert.ToString(ddlsec.SelectedValue).Trim();
                if (Convert.ToString(ddlsec.SelectedItem.Text).Trim().ToLower() == "all" || Convert.ToString(ddlsec.SelectedItem.Text).Trim().ToLower() == "")
                {
                    strsec = "";
                }
                else
                {
                    strsec = " and st.Sections='" + Convert.ToString(sections) + "'";
                }
            }

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


                    if (Convert.ToString(Session["Staff_Code"]) == "")
                    {
                        //subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and  st.subject_no=s.subject_no  and SM.degree_code='" + Convert.ToString(ddlbranch.SelectedValue) + "' " + Convert.ToString(sems) + " and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue) + "' order by S.subject_no ";
                        subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and SM.degree_code='" + Convert.ToString(ddlbranch.SelectedValue) + "' " + Convert.ToString(sems) + " and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue) + "' order by S.subject_no ";
                    }
                    else if (Convert.ToString(Session["Staff_Code"]) != "")
                    {
                        subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and st.subject_no=s.subject_no and s.syll_code=SM.syll_code and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.degree_code='" + Convert.ToString(ddlbranch.SelectedValue) + "' " + Convert.ToString(sems) + " and  SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue) + "'  and staff_code='" + Convert.ToString(Session["Staff_Code"]) + "' " + strsec + " order by S.subject_no ";
                    }
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
        }
    }

    public void GetSubject1()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            string subjectquery = string.Empty;
            ddlsubject.Items.Clear();
            string sections = string.Empty;// Convert.ToString(ddlsec.SelectedValue);
            string strsec = "";
            if (ddlsec.Items.Count > 0)
            {
                sections = Convert.ToString(ddlsec.SelectedValue);
                if (Convert.ToString(ddlsec.SelectedItem.Text).Trim().ToLower() == "all" || Convert.ToString(ddlsec.SelectedItem.Text).Trim().ToLower() == "")
                {
                    strsec = "";
                }
                else
                {
                    strsec = " and st.Sections='" + Convert.ToString(sections) + "'";
                }
            }

            string sems = "";
            if (ddlsem.Items.Count > 0)
            {
                if (Convert.ToString(ddlsem.SelectedValue) != "")
                {
                    if (Convert.ToString(ddlsem.SelectedValue) == "")
                    {
                        sems = "";
                    }
                    else
                    {
                        sems = "and SM.semester='" + Convert.ToString(ddlsem.SelectedValue) + "'";
                    }


                    if (Convert.ToString(Session["Staff_Code"]).Trim() == "")
                    {
                        //subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and  st.subject_no=s.subject_no  and SM.degree_code=" + Convert.ToString(ddlbranch.SelectedValue) + " " + Convert.ToString(sems) + " and  S.subtype_no = Sem.subtype_no and promote_count=1 and SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue) + "' order by S.subject_no ";
                        subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and SM.degree_code='" + Convert.ToString(ddlbranch.SelectedValue) + "' " + Convert.ToString(sems) + " and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue).Trim() + "' order by S.subject_no ";
                    }
                    else if (Convert.ToString(Session["Staff_Code"]).Trim() != "")
                    {
                        subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and st.subject_no=s.subject_no and s.syll_code=SM.syll_code and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.degree_code='" + Convert.ToString(ddlbranch.SelectedValue) + "' " + Convert.ToString(sems) + " and  SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue) + "'  and sc.staff_code='" + Convert.ToString(Session["Staff_Code"]).Trim() + "' " + strsec + "  order by S.subject_no ";
                    }
                    if (subjectquery != "")
                    {
                        ds.Dispose();
                        ds.Reset();
                        ds = d2.select_method(subjectquery, hat, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlsubject.Enabled = true;
                            ddlsubject.DataSource = ds;
                            ddlsubject.DataValueField = "Subject_No";
                            ddlsubject.DataTextField = "Subject_Name";
                            ddlsubject.DataBind();
                        }
                        else
                        {
                            ddlsubject.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

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
            lblsec.Text = ((!isschool) ? "Section" : "Section");
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void BindTest()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            cblChooseTests.Items.Clear();
            ds.Clear();
            ds.Reset();
            ds.Dispose();
            batch_year = Convert.ToString(ddlbatch.SelectedItem.Text).Trim();
            degree_code = Convert.ToString(ddlbranch.SelectedValue).Trim();
            semester = Convert.ToString(ddlsem.SelectedItem.Text).Trim();
            qry = "";
            if (!string.IsNullOrEmpty(batch_year) && !string.IsNullOrEmpty(degree_code) && !string.IsNullOrEmpty(semester))
            {
                qry = "select c.criteria,c.Criteria_no from CriteriaForInternal c, syllabus_master sy where c.syll_code=sy.syll_code and sy.Batch_Year='" + batch_year + "' and sy.degree_code='" + degree_code + "' and sy.semester='" + semester + "'";
                ds = d2.select_method_wo_parameter(qry, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cblChooseTests.DataSource = ds;
                    cblChooseTests.DataTextField = "criteria";
                    cblChooseTests.DataValueField = "Criteria_no";
                    cblChooseTests.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void Init_Spread(Farpoint.FpSpread FpSpread1)
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
        FpSpread1.Sheets[0].AutoPostBack = true;
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Sheets[0].ColumnCount = 5;
        //FpSpread1.Sheets[0].FrozenColumnCount = 4;

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
        FpSpread1.Sheets[0].Columns[1].Width = 150;
        FpSpread1.Sheets[0].Columns[2].Width = 40;
        FpSpread1.Sheets[0].Columns[3].Width = 80;
        FpSpread1.Sheets[0].Columns[4].Width = 80;

        FpSpread1.Sheets[0].Columns[0].Locked = true;
        FpSpread1.Sheets[0].Columns[1].Locked = true;
        FpSpread1.Sheets[0].Columns[2].Locked = true;
        FpSpread1.Sheets[0].Columns[3].Locked = true;
        FpSpread1.Sheets[0].Columns[4].Locked = true;

        FpSpread1.Sheets[0].Columns[0].Resizable = false;
        FpSpread1.Sheets[0].Columns[1].Resizable = false;
        FpSpread1.Sheets[0].Columns[2].Resizable = false;
        FpSpread1.Sheets[0].Columns[3].Resizable = false;
        FpSpread1.Sheets[0].Columns[4].Resizable = false;

        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Subject Name";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "New Test Name";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Calculate Test Names";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Convert To(Max.Mark)";

        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);

        FpSpread1.Sheets[0].SetColumnMerge(0, Farpoint.Model.MergePolicy.Always);
        FpSpread1.Sheets[0].SetColumnMerge(1, Farpoint.Model.MergePolicy.Always);
        FpSpread1.Sheets[0].SetColumnMerge(2, Farpoint.Model.MergePolicy.Always);

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
            divSettings.Visible = false;
            divSavedSettings.Visible = false;

            BindDegree();
            bindbranch();
            bindsem();
            BindSectionDetail();
            GetSubject();
            GetSubject1();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblErrSearch.Text = string.Empty;
        lblErrSearch.Visible = false;
        popupdiv.Visible = false;
        lblpopuperr.Text = string.Empty;
        divSettings.Visible = false;
        divSavedSettings.Visible = false;

        ////btnSave.Visible = false;
        BindDegree();
        bindbranch();
        bindsem();
        BindSectionDetail();
        GetSubject();
        GetSubject1();
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblErrSearch.Text = string.Empty;
        lblErrSearch.Visible = false;
        popupdiv.Visible = false;
        lblpopuperr.Text = string.Empty;
        divSettings.Visible = false;
        divSavedSettings.Visible = false;


        bindbranch();
        bindsem();
        BindSectionDetail();
        GetSubject();
        GetSubject1();
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblErrSearch.Text = string.Empty;
        lblErrSearch.Visible = false;
        popupdiv.Visible = false;
        lblpopuperr.Text = string.Empty;
        divSettings.Visible = false;
        divSavedSettings.Visible = false;

        bindsem();
        BindSectionDetail();
        GetSubject();
        GetSubject1();
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblErrSearch.Text = string.Empty;
        lblErrSearch.Visible = false;
        popupdiv.Visible = false;
        lblpopuperr.Text = string.Empty;
        divSettings.Visible = false;
        divSavedSettings.Visible = false;

        BindSectionDetail();
        GetSubject();
        GetSubject1();
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {

        lblErrSearch.Text = string.Empty;
        lblErrSearch.Visible = false;
        popupdiv.Visible = false;
        lblpopuperr.Text = string.Empty;
        divSettings.Visible = false;
        divSavedSettings.Visible = false;

        GetSubject();
        GetSubject1();
    }

    protected void ddlsubject_Selectchanged(object sender, EventArgs e)
    {
        lblErrSearch.Text = string.Empty;
        lblErrSearch.Visible = false;
        popupdiv.Visible = false;
        lblpopuperr.Text = string.Empty;
        divSettings.Visible = false;
        divSavedSettings.Visible = false;
    }

    protected void chkSubjects_CheckedChanged(object sender, EventArgs e)
    {

        lblErrSearch.Text = string.Empty;
        lblErrSearch.Visible = false;
        popupdiv.Visible = false;
        lblpopuperr.Text = string.Empty;
        divSettings.Visible = false;
        divSavedSettings.Visible = false;

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

    protected void cblSubjects_SelectedIndexChanged(object sender, EventArgs e)
    {

        lblErrSearch.Text = string.Empty;
        lblErrSearch.Visible = false;
        chkSubjects.Checked = false;
        popupdiv.Visible = false;
        lblpopuperr.Text = string.Empty;
        divSettings.Visible = false;
        divSavedSettings.Visible = false;

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

    #endregion DropDown Events

    #region Button Events

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;

            divSettings.Visible = false;
            divSavedSettings.Visible = false;

            int selSubjectCount = 0;
            int spreadHeight = 0;
            qrysec = string.Empty;
            string qrysecnew = string.Empty;
            string cblSubjectNo = string.Empty;

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

            if (ddlsec.Items.Count > 0)
            {
                if (ddlsec.Enabled == false)
                {
                    section = string.Empty;
                    qrysec = string.Empty;
                }
                else if (ddlsec.Items.Count > 0 && ddlsec.SelectedItem.Text.ToLower().Trim() == "all")
                {
                    section = string.Empty;
                    qrysec = string.Empty;
                }
                else
                {
                    section = Convert.ToString(ddlsec.SelectedItem.Text).Trim();
                    qrysec = " and e.sections='" + section + "'";
                    qrysecnew = " and ca.section='" + section + "'";
                }
            }
            else
            {
                section = string.Empty;
                qrysec = string.Empty;
            }

            if (ddlsubject.Items.Count != 0)
            {
                subject_no = Convert.ToString(ddlsubject.SelectedValue).Trim();
            }
            else
            {
                lblpopuperr.Text = "No Subject were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            //selSubjectCount = 0;
            //if (cblSubjects.Items.Count == 0)
            //{
            //    lblpopuperr.Text = "No Subject were Found";
            //    lblpopuperr.Visible = true;
            //    popupdiv.Visible = true;
            //    return;
            //}
            //else
            //{
            //    selSubjectCount = 0;
            //    cblSubjectNo = string.Empty;
            //    foreach (ListItem li in cblSubjects.Items)
            //    {
            //        if (li.Selected)
            //        {
            //            selSubjectCount++;
            //            if (string.IsNullOrEmpty(cblSubjectNo))
            //            {
            //                cblSubjectNo = li.Value + "'";
            //            }
            //            else
            //            {
            //                cblSubjectNo = ",'"+li.Value + "";
            //            }
            //        }
            //    }
            //    if (selSubjectCount == 0)
            //    {
            //        lblpopuperr.Text = "Please Select Atleast One Subject";
            //        lblpopuperr.Visible = true;
            //        popupdiv.Visible = true;
            //        return;
            //    }
            //}

            //BindTest();

            if (subject_no != "")
            {
                qry = "select distinct New_Test_Name,ca.caluationID,ca.convertedTo as New_Test_Max  from CAM_Calculation_Make_New_Test ca,CAM_Calculation_Test_Settings se,CriteriaForInternal c where c.Criteria_no=se.criteria_no and  ca.subject_no=se.subject_no   and ca.caluationID=se.caluationID and ca.section=se.section " + qrysecnew + " and ca.subject_no='" + subject_no + "' order by ca.caluationID; select  New_Test_Name,ca.caluationID,ca.subject_no,s.subject_name,s.subject_code,ca.convertedTo as New_Test_Max,se.convertedTo as Old_Test_Max,c.Criteria_no,c.criteria from CAM_Calculation_Make_New_Test ca,CAM_Calculation_Test_Settings se,CriteriaForInternal c, subject s where c.Criteria_no=se.criteria_no and  ca.subject_no=se.subject_no and s.subject_no=ca.subject_no and s.subject_no=se.subject_no and ca.caluationID=se.caluationID and ca.section=se.section  " + qrysecnew + " and ca.subject_no='" + subject_no + "' order by ca.subject_no,ca.caluationID,c.Criteria_no";
                DataSet dsAllTests = new DataSet();
                dsAllTests = d2.select_method_wo_parameter(qry, "Text");

                if (dsAllTests.Tables.Count > 0 && dsAllTests.Tables[0].Rows.Count > 0)
                {
                    Init_Spread(FpSpreadSettings);
                    for (int row = 0; row < dsAllTests.Tables[0].Rows.Count; row++)
                    {
                        string newTestName = Convert.ToString(dsAllTests.Tables[0].Rows[row]["New_Test_Name"]).Trim();
                        string CalculatedID = Convert.ToString(dsAllTests.Tables[0].Rows[row]["caluationID"]).Trim();
                        string convertTo = Convert.ToString(dsAllTests.Tables[0].Rows[row]["New_Test_Max"]).Trim();

                        int startrow = FpSpreadSettings.Sheets[0].RowCount++;//New_Test_Max

                        FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                        FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 0].Locked = true;
                        FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                        FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(newTestName);
                        FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(CalculatedID);
                        FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 2].Note = Convert.ToString(convertTo);
                        FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 2].Locked = true;
                        FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                        if (dsAllTests.Tables.Count >= 2 && dsAllTests.Tables[1].Rows.Count > 0)
                        {
                            DataView dv = new DataView();
                            dsAllTests.Tables[1].DefaultView.RowFilter = "caluationID='" + CalculatedID + "' and New_Test_Name='" + newTestName + "'";
                            dv = dsAllTests.Tables[1].DefaultView;
                            if (dv.Count > 0)
                            {
                                for (int newrow = 0; newrow < dv.Count; newrow++)
                                {
                                    if (newrow != 0)
                                    {
                                        FpSpreadSettings.Sheets[0].RowCount++;
                                    }

                                    newTestName = Convert.ToString(dv[newrow]["New_Test_Name"]).Trim();
                                    CalculatedID = Convert.ToString(dv[newrow]["caluationID"]).Trim();
                                    convertTo = Convert.ToString(dv[newrow]["New_Test_Max"]).Trim();
                                    string subjectNo = Convert.ToString(dv[newrow]["subject_no"]);
                                    string subjectName = Convert.ToString(dv[newrow]["subject_name"]);
                                    string subjectCode = Convert.ToString(dv[newrow]["subject_code"]);
                                    string oldTestConvertTo = Convert.ToString(dv[newrow]["Old_Test_Max"]);
                                    string criteria_no = Convert.ToString(dv[newrow]["Criteria_no"]);
                                    string criteria = Convert.ToString(dv[newrow]["criteria"]);

                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 0].Locked = true;
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(newTestName);
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(CalculatedID);
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 2].Note = Convert.ToString(convertTo);
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 2].Locked = true;
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(subjectName);
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(subjectNo);
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(subjectCode);
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 2].Locked = true;
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(criteria);
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(criteria_no);
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 3].Note = Convert.ToString(oldTestConvertTo);
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 3].Locked = true;
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(oldTestConvertTo);
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(criteria_no);
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 4].Locked = true;
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                                    spreadHeight += FpSpreadSettings.Sheets[0].Rows[FpSpreadSettings.Sheets[0].RowCount - 1].Height;
                                }
                            }
                        }
                    }

                    for (int sh = 0; sh < FpSpreadSettings.Sheets[0].ColumnHeader.RowCount; sh++)
                    {
                        spreadHeight += FpSpreadSettings.Sheets[0].ColumnHeader.Rows[sh].Height;
                    }

                    FpSpreadSettings.Sheets[0].PageSize = FpSpreadSettings.Sheets[0].RowCount;
                    FpSpreadSettings.Width = 900;
                    FpSpreadSettings.Height = (spreadHeight) + 45;
                    FpSpreadSettings.SaveChanges();
                    FpSpreadSettings.Visible = true;
                    divSavedSettings.Visible = true;

                }
                else
                {
                    if (!string.IsNullOrEmpty(batch_year) && !string.IsNullOrEmpty(degree_code) && !string.IsNullOrEmpty(semester) && !string.IsNullOrEmpty(subject_no))
                    {
                        qry = "select distinct c.Criteria_no,c.criteria from CriteriaForInternal c, Exam_type e,syllabus_master sy where c.Criteria_no=e.criteria_no and sy.syll_code=c.syll_code and sy.Batch_Year='" + batch_year + "' and sy.degree_code='" + degree_code + "' and sy.semester='" + semester + "' and e.subject_no='" + subject_no + "' " + qrysec + " ";
                        ds.Reset();
                        ds.Dispose();
                        ds = d2.select_method_wo_parameter(qry, "Text");
                    }
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        cblChooseTests.Items.Clear();
                        tblMax.Rows.Clear();
                        tblMax.Controls.Clear();
                        cblChooseTests.DataSource = ds;
                        cblChooseTests.DataTextField = "criteria";
                        cblChooseTests.DataValueField = "Criteria_no";
                        cblChooseTests.DataBind();
                        TextBox txtConvertTo = new TextBox();
                        TableRow trTest = new TableRow();
                        TableCell tcTest = new TableCell();
                        for (int test = 0; test < ds.Tables[0].Rows.Count; test++)
                        {
                            txtConvertTo = new TextBox();
                            txtConvertTo.ID = "txtConvertTo" + (test + 1);
                            txtConvertTo.Text = "";
                            txtConvertTo.MaxLength = 3;
                            //txtConvertTo.Width = 50;
                            AjaxControlToolkit.FilteredTextBoxExtender filter = new FilteredTextBoxExtender();
                            filter.ID = "filter" + (test + 1);
                            filter.TargetControlID = txtConvertTo.ID;
                            filter.FilterType = FilterTypes.Numbers;

                            trTest = new TableRow();
                            tcTest = new TableCell();
                            tcTest.Controls.Add(txtConvertTo);
                            tcTest.Controls.Add(filter);
                            trTest.Cells.Add(tcTest);
                            tblMax.Rows.Add(trTest);
                        }
                        tblMax.Caption = "Tests Max Marks";
                        tblMax.Width = tblMax.Caption.Length * 10;
                        divSettings.Visible = true;
                        divSavedSettings.Visible = false;
                    }
                    else
                    {
                        divSettings.Visible = false;
                        divSavedSettings.Visible = false;
                        lblpopuperr.Text = "Test is Not Conducted For Selected Subject " + Convert.ToString(ddlsubject.SelectedItem.Text);
                        lblpopuperr.Visible = true;
                        popupdiv.Visible = true;
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            qrysec = string.Empty;
            string qrysecnew = string.Empty;
            divSettings.Visible = false;
            txtConvertedTo.Text = string.Empty;
            txtTypeTestName.Text = string.Empty;

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
            else
            {
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
            if (ddlsec.Items.Count > 0)
            {
                if (ddlsec.Enabled == false)
                {
                    section = string.Empty;
                    qrysec = string.Empty;
                }
                else if (ddlsec.Items.Count > 0 && ddlsec.SelectedItem.Text.ToLower().Trim() == "all")
                {
                    section = string.Empty;
                    qrysec = string.Empty;
                }
                else
                {
                    section = Convert.ToString(ddlsec.SelectedItem.Text).Trim();
                    qrysec = " and e.sections='" + section + "'";
                    qrysecnew = " and ca.section='" + section + "'";
                }
            }
            else
            {
                section = string.Empty;
                qrysec = string.Empty;
            }
            if (ddlsubject.Items.Count != 0)
            {
                subject_no = Convert.ToString(ddlsubject.SelectedValue).Trim();
            }
            else
            {
                lblpopuperr.Text = "No Subject were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (!string.IsNullOrEmpty(batch_year) && !string.IsNullOrEmpty(degree_code) && !string.IsNullOrEmpty(semester) && !string.IsNullOrEmpty(subject_no))
            {
                qry = "select distinct c.Criteria_no,c.criteria from CriteriaForInternal c, Exam_type e,syllabus_master sy where c.Criteria_no=e.criteria_no and sy.syll_code=c.syll_code and sy.Batch_Year='" + batch_year + "' and sy.degree_code='" + degree_code + "' and sy.semester='" + semester + "' and e.subject_no='" + subject_no + "' " + qrysec + " ";
                ds.Reset();
                ds.Dispose();
                ds = d2.select_method_wo_parameter(qry, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblChooseTests.Items.Clear();
                tblMax.Rows.Clear();
                tblMax.Controls.Clear();
                cblChooseTests.DataSource = ds;
                cblChooseTests.DataTextField = "criteria";
                cblChooseTests.DataValueField = "Criteria_no";
                cblChooseTests.DataBind();
                TextBox txtConvertTo = new TextBox();
                TableRow trTest = new TableRow();
                TableCell tcTest = new TableCell();
                for (int test = 0; test < ds.Tables[0].Rows.Count; test++)
                {
                    txtConvertTo = new TextBox();
                    txtConvertTo.ID = "txtConvertTo" + (test + 1);
                    txtConvertTo.Attributes.Add("autocomplete", "off");
                    txtConvertTo.Text = "";
                    txtConvertTo.MaxLength = 3;
                    //txtConvertTo.Width = 50;
                    AjaxControlToolkit.FilteredTextBoxExtender filter = new FilteredTextBoxExtender();
                    filter.ID = "filter" + (test + 1);
                    filter.TargetControlID = txtConvertTo.ID;
                    filter.FilterType = FilterTypes.Numbers;

                    trTest = new TableRow();
                    tcTest = new TableCell();
                    tcTest.Controls.Add(txtConvertTo);
                    tcTest.Controls.Add(filter);
                    trTest.Cells.Add(tcTest);
                    tblMax.Rows.Add(trTest);
                }
                tblMax.Caption = "Tests Max Marks";
                tblMax.Width = tblMax.Caption.Length * 10;
                divSettings.Visible = true;
            }
            else
            {
                divSettings.Visible = false;
                lblpopuperr.Text = "Test is Not Conducted For Selected Subject " + Convert.ToString(ddlsubject.SelectedItem.Text);
                lblpopuperr.Visible = true;
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            int row = cblChooseTests.Items.Count;
            string criteria_no = string.Empty;
            string[] eachcriteia = new string[1];
            double[] maxMarks = new double[1];
            string newTestName = string.Empty;
            string newTestConvertTo = string.Empty;

            bool isSuccess = false;
            bool issuc = false;

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
            else
            {
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
            if (ddlsec.Items.Count > 0)
            {
                if (ddlsec.Enabled == false)
                {
                    section = string.Empty;
                    qrysec = string.Empty;
                }
                else if (ddlsec.Items.Count > 0 && ddlsec.SelectedItem.Text.ToLower().Trim() == "all")
                {
                    section = string.Empty;
                    qrysec = string.Empty;
                }
                else
                {
                    section = Convert.ToString(ddlsec.SelectedItem.Text).Trim();
                    qrysec = " and section='" + section + "'";
                }
            }
            else
            {
                section = string.Empty;
                qrysec = string.Empty;
            }
            if (ddlsubject.Items.Count != 0)
            {
                subject_no = Convert.ToString(ddlsubject.SelectedValue).Trim();
            }
            else
            {
                lblpopuperr.Text = "No Subject were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (txtTypeTestName.Text.Trim() != "")
            {
                newTestName = txtTypeTestName.Text.Trim();
            }
            else
            {
                lblpopuperr.Text = "Please Type The Criteria Name";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            if (txtConvertedTo.Text.Trim() != "")
            {
                double value = 0;
                if (double.TryParse(txtConvertedTo.Text.Trim(), out value))
                {
                    if (value == 0)
                    {
                        lblpopuperr.Text = "Please Enter The Max Mark for Test " + txtTypeTestName.Text + " Should Be Greater Than 0";
                        lblpopuperr.Visible = true;
                        popupdiv.Visible = true;
                        return;
                    }
                    newTestConvertTo = txtConvertedTo.Text.Trim();
                }
            }
            else
            {
                lblpopuperr.Text = "Please Type Convert To Marks For Criteria";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            int selTest = 0;
            int newcount = 0;
            foreach (ListItem li in cblChooseTests.Items)
            {
                newcount++;
                if (li.Selected)
                {
                    if (selTest != 0)
                    {
                        Array.Resize(ref eachcriteia, selTest + 1);
                        Array.Resize(ref maxMarks, selTest + 1);
                    }
                    selTest++;
                    eachcriteia[selTest - 1] = li.Value;
                    if (criteria_no == "")
                    {
                        criteria_no = "'" + li.Value + "'";
                    }
                    else
                    {
                        criteria_no += ",'" + li.Value + "'";
                    }
                    string crimenameid = "txtConvertTo" + (newcount); ;
                    TextBox tbcrimename = (TextBox)tblMax.Rows[selTest - 1].FindControl(crimenameid);
                    if (tbcrimename != null)
                    {
                        if (tbcrimename.Text.Trim() != "")
                        {
                            double value = 0;
                            if (double.TryParse(tbcrimename.Text.Trim(), out value))
                            {
                                if (value == 0)
                                {
                                    lblpopuperr.Text = "Please Enter The Max Mark for Test " + li.Text + " Should Be Greater Than 0";
                                    lblpopuperr.Visible = true;
                                    popupdiv.Visible = true;
                                    return;
                                }
                                maxMarks[selTest - 1] = value;
                            }
                        }
                        else
                        {
                            lblpopuperr.Text = "Please Enter The Max Mark for Test " + li.Text;
                            lblpopuperr.Visible = true;
                            popupdiv.Visible = true;
                            return;
                        }
                    }
                }
            }

            if (selTest == 0)
            {
                lblpopuperr.Text = "Please Select Atleast One Test";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            if (newTestName.Trim() != "" && newTestConvertTo.Trim() != "" && subject_no.Trim() != "")
            {
                //,convertedMin=''
                qry = "if exists (select * from CAM_Calculation_Make_New_Test where New_Test_Name='" + newTestName + "' and subject_no='" + subject_no + "' " + qrysec + " ) update CAM_Calculation_Make_New_Test set convertedTo='" + newTestConvertTo + "'  where New_Test_Name='" + newTestName + "' and subject_no='" + subject_no + "' " + qrysec + "  else insert into CAM_Calculation_Make_New_Test (New_Test_Name,subject_no,section,convertedTo) values('" + newTestName + "','" + subject_no + "','" + section + "','" + newTestConvertTo + "')";
                int res = d2.update_method_wo_parameter(qry, "Text");
                if (res > 0)
                {
                    isSuccess = true;

                }
                string caluationID = d2.GetFunctionv("select caluationID from CAM_Calculation_Make_New_Test where New_Test_Name='" + newTestName + "' and subject_no='" + subject_no + "' " + qrysec + "");
                if (caluationID.Trim() != "")
                {
                    string nee = "delete from CAM_Calculation_Test_Settings where caluationID='" + caluationID.Trim() + "'  and subject_no='" + subject_no + "'" + qrysec + "";//and criteria_no='" + eachcriteia[sel] + "'
                    int swe = d2.update_method_wo_parameter(nee, "Text");

                    for (int sel = 0; sel < selTest; sel++)
                    {
                        if (maxMarks[sel] != 0)
                        {
                            string newqry = "if exists (select * from CAM_Calculation_Test_Settings where caluationID='" + caluationID.Trim() + "' and criteria_no='" + eachcriteia[sel] + "' and subject_no='" + subject_no + "'" + qrysec + ") update CAM_Calculation_Test_Settings set convertedTo='" + maxMarks[sel] + "' where  caluationID='" + caluationID.Trim() + "' and criteria_no='" + eachcriteia[sel] + "' and subject_no='" + subject_no + "'" + qrysec + " else insert into CAM_Calculation_Test_Settings (caluationID,criteria_no,subject_no,section,convertedTo) values('" + caluationID.Trim() + "','" + eachcriteia[sel] + "','" + subject_no + "','" + section + "','" + maxMarks[sel] + "')";
                            int newres = d2.update_method_wo_parameter(newqry, "Text");
                            if (newres > 0)
                            {
                                issuc = true;
                            }
                        }
                    }
                }
            }
            if (isSuccess)
            {
                btngo_Click(sender, e);
                lblpopuperr.Text = "Saved Successfully";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            else
            {
                btngo_Click(sender, e);
                lblpopuperr.Text = "Not Saved";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        popupdiv.Visible = false;
    }

    private void CreateTextBox(int rows, params double[] values)
    {
        if (rows > 0)
        {
            int rows1 = tblMax.Rows.Count;
            tblMax.Rows.Clear();
            TextBox txtConvertTo = new TextBox();
            TableRow trTest = new TableRow();
            TableCell tcTest = new TableCell();
            for (int test = 0; test < rows; test++)
            {
                txtConvertTo = new TextBox();
                txtConvertTo.ID = "txtConvertTo" + (test + 1);
                txtConvertTo.MaxLength = 3;
                
                //txtConvertTo.Text = "";
                //txtConvertTo.Width = 50;
                trTest = new TableRow();
                tcTest = new TableCell();
                AjaxControlToolkit.FilteredTextBoxExtender filter = new FilteredTextBoxExtender();
                filter.ID = "filter" + (test + 1);
                filter.TargetControlID = txtConvertTo.ID;
                filter.FilterType = FilterTypes.Numbers;
                //tcTest.Controls.Add(txtConvertTo);
                CreateTextBox(txtConvertTo.ID, tcTest);
                CreateTextBox(filter.ID, tcTest);
                trTest.Cells.Add(tcTest);
                tblMax.Rows.Add(trTest);
                txtConvertedTo.Attributes.Add("autocomplete", "off");
            }
            tblMax.Caption = "Tests Max Marks";
            //tblMax.Width = tblMax.Caption.Length * 10;
        }
    }

    private void CreateTextBox(string id, Control pnlTextBoxes, int row = 0)
    {
        if (row == 0)
        {
            TextBox txt = new TextBox();
            txt.ID = id;
            txt.MaxLength = 3;
            AjaxControlToolkit.FilteredTextBoxExtender filter = new FilteredTextBoxExtender();
            filter.ID = "filter" + (id.Substring(id.Length - 1));
            filter.TargetControlID = id;
            filter.FilterType = FilterTypes.Numbers;
            pnlTextBoxes.Controls.Add(txt);
            pnlTextBoxes.Controls.Add(filter);
            txt.Attributes.Add("autocomplete", "off");
            
        }
        else
        {
            TextBox txt = new TextBox();
            txt.ID = id;
            txt.MaxLength = 3;
            
            TableCell tcTest = new TableCell();
            TableRow trTest = new TableRow();
            trTest.Cells.Add(tcTest);            
            AjaxControlToolkit.FilteredTextBoxExtender filter = new FilteredTextBoxExtender();
            filter.ID = "filter" + (id.Substring(id.Length - 1));
            filter.TargetControlID = id;
            filter.FilterType = FilterTypes.Numbers;
            tcTest.Controls.Add(txt);
            tcTest.Controls.Add(filter);
            pnlTextBoxes.Controls.Add(trTest);
            txt.Attributes.Add("autocomplete", "off");
        }
    }

    #endregion  Button Events

}