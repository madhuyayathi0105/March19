using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using Farpoint = FarPoint.Web.Spread;
using System.Configuration;

public partial class Cam_Comparision_Settings_For_PerformanceMoniter : System.Web.UI.Page
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
                lblErrSearch.Text = "";
                lblErrSearch.Visible = false;
                popupdiv.Visible = false;
                lblpopuperr.Text = string.Empty;
                divMainContents.Visible = false;

                #region LoadHeader

                Bindcollege();
                BindBatch();
                BindDegree();
                bindbranch();
                bindsem();
                BindSectionDetail();
                GetSubject();
                GetSubject1();

                #endregion LoadHeader

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
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Page Load

    #region Logout

    protected void lbtnLogout_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Logout

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
            lblErrSearch.Text = Convert.ToString(ex);
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
            lblErrSearch.Text = Convert.ToString(ex);
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
            lblErrSearch.Text = Convert.ToString(ex);
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
            lblErrSearch.Text = Convert.ToString(ex);
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
            lblErrSearch.Text = Convert.ToString(ex);
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
                if (Convert.ToString(ddlsem.SelectedValue) != "")
                {
                    if (Convert.ToString(ddlsem.SelectedValue) == "")
                    {
                        sems = "";
                    }
                    else
                    {
                        sems = "and SM.semester=" + Convert.ToString(ddlsem.SelectedValue) + "";
                    }


                    if (Convert.ToString(Session["Staff_Code"]) == "")
                    {
                        //subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and  st.subject_no=s.subject_no  and SM.degree_code=" + Convert.ToString(ddlbranch.SelectedValue) + " " + Convert.ToString(sems) + " and  S.subtype_no = Sem.subtype_no and promote_count=1 and SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue) + "' order by S.subject_no ";
                        subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and SM.degree_code='" + Convert.ToString(ddlbranch.SelectedValue) + "' " + Convert.ToString(sems) + " and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue) + "' order by S.subject_no ";
                    }
                    else if (Convert.ToString(Session["Staff_Code"]) != "")
                    {
                        subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and st.subject_no=s.subject_no and s.syll_code=SM.syll_code and  S.subtype_no = Sem.subtype_no and promote_count=1 and SM.degree_code=" + Convert.ToString(ddlbranch.SelectedValue) + " " + Convert.ToString(sems) + " and  SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue) + "'  and SC.staff_code='" + Convert.ToString(Session["Staff_Code"]) + "' " + strsec + " order by S.subject_no ";
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
            if (Convert.ToString(ddlsem.SelectedValue) != "")
            {
                if (Convert.ToString(ddlsem.SelectedValue) == "")
                {
                    sems = "";
                }
                else
                {
                    sems = "and SM.semester=" + Convert.ToString(ddlsem.SelectedValue) + "";
                }


                if (Convert.ToString(Session["Staff_Code"]) == "")
                {
                    //subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and  st.subject_no=s.subject_no  and SM.degree_code='" + Convert.ToString(ddlbranch.SelectedValue) + "' " + Convert.ToString(sems) + " and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue) + "' order by S.subject_no ";
                    subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and SM.degree_code='" + Convert.ToString(ddlbranch.SelectedValue) + "' " + Convert.ToString(sems) + " and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue) + "' order by S.subject_no ";
                }
                else if (Convert.ToString(Session["Staff_Code"]) != "")
                {
                    subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and st.subject_no=s.subject_no and s.syll_code=SM.syll_code and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.degree_code='" + Convert.ToString(ddlbranch.SelectedValue) + "' " + Convert.ToString(sems) + " and  SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue) + "'  and SC.staff_code='" + Convert.ToString(Session["Staff_Code"]) + "' " + strsec + "  order by S.subject_no ";
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
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
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

    public void Init_Spread(Farpoint.FpSpread FpSpread1)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
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
            FpSpread1.Sheets[0].ColumnCount = 6;
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
            FpSpread1.Sheets[0].Columns[5].Width = 80;

            FpSpread1.Sheets[0].Columns[0].Locked = true;
            FpSpread1.Sheets[0].Columns[1].Locked = true;
            FpSpread1.Sheets[0].Columns[2].Locked = true;
            FpSpread1.Sheets[0].Columns[3].Locked = true;
            FpSpread1.Sheets[0].Columns[4].Locked = true;
            FpSpread1.Sheets[0].Columns[5].Locked = true;

            FpSpread1.Sheets[0].Columns[0].Resizable = false;
            FpSpread1.Sheets[0].Columns[1].Resizable = false;
            FpSpread1.Sheets[0].Columns[2].Resizable = false;
            FpSpread1.Sheets[0].Columns[3].Resizable = false;
            FpSpread1.Sheets[0].Columns[4].Resizable = false;
            FpSpread1.Sheets[0].Columns[5].Resizable = false;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Subject Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Name of Comparision";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Compare From Test";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Compare To Test";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Comparision Convert To(Max.Mark)";

            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);

            FpSpread1.Sheets[0].SetColumnMerge(0, Farpoint.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(1, Farpoint.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(2, Farpoint.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(3, Farpoint.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(4, Farpoint.Model.MergePolicy.Always);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
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
            divMainContents.Visible = false;
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
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContents.Visible = false;

            ////btnSave.Visible = false;
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

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContents.Visible = false;

            // //btnSave.Visible = false;
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

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContents.Visible = false;
            ////btnSave.Visible = false;
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

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContents.Visible = false;

            //btnSave.Visible = false;
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

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContents.Visible = false;

            //btnSave.Visible = false;
            GetSubject();
            GetSubject1();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlsubject_Selectchanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContents.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
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
            divMainContents.Visible = false;
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
            divMainContents.Visible = false;

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
            divMainContents.Visible = false;
            divSettings.Visible = false;
            divSavedSettings.Visible = false;
            int selSubjectCount = 0;
            int spreadHeight = 0;

            qrysec = string.Empty;
            string qrysecnew = string.Empty;
            string qrySecAll = string.Empty;
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
                    qrySecAll = string.Empty;
                }
                else if (ddlsec.Items.Count > 0 && ddlsec.SelectedItem.Text.ToLower().Trim() == "all")
                {
                    section = string.Empty;
                    qrysec = string.Empty;
                    qrySecAll = string.Empty;
                }
                else
                {
                    section = Convert.ToString(ddlsec.SelectedItem.Text).Trim();
                    qrysec = " and e.sections='" + section + "'";//and e.sections=''
                    qrysecnew = " and ca.section='" + section + "'";//and ca.section='A'
                    qrySecAll = " and ts.section='" + section + "'";
                }
            }
            else
            {
                section = string.Empty;
                qrysec = string.Empty;
                qrySecAll = string.Empty;
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
            if (subject_no != "")
            {
                //qry = "select distinct tc.comaparisionID,tc.comparisionName,tc.convertedTo from FromTest_Comparision ftc,ToTest_Comparision ttc,Test_Comparision_Settings tc,CriteriaForInternal c,CAM_Calculation_Make_New_Test ca,subject s where tc.comaparisionID=ftc.comaparisionID and ttc.comaparisionID=ftc.comaparisionID and tc.comaparisionID=ttc.comaparisionID and tc.subject_no=ca.subject_no and s.subject_no=ca.subject_no and s.subject_no=tc.subject_no and tc.section=ca.section and (c.Criteria_no=ftc.criteria_no or ca.caluationID=ftc.criteria_no_New) and (c.Criteria_no=ttc.criteria_no or ca.caluationID=ttc.criteria_no_New) and tc.subject_no='" + subject_no + "' " + qrySecAll + " order by tc.comaparisionID ; select tc.subject_no,s.subject_code,s.subject_name,tc.comaparisionID,tc.comparisionName,tc.convertedTo,case when ftc.type=1 then c.Criteria_no else ca.caluationID end as From_Test_No,case when ftc.type=1 then c.criteria else ca.New_Test_Name end as From_Test,case when ttc.type=1 then c.Criteria_no else ca.caluationID end as To_Test_No,case when ttc.type=1 then c.criteria else ca.New_Test_Name end as To_Test from FromTest_Comparision ftc,ToTest_Comparision ttc,Test_Comparision_Settings tc,CriteriaForInternal c,CAM_Calculation_Make_New_Test ca,subject s where tc.comaparisionID=ftc.comaparisionID and ttc.comaparisionID=ftc.comaparisionID and tc.comaparisionID=ttc.comaparisionID and tc.subject_no=ca.subject_no and s.subject_no=ca.subject_no and s.subject_no=tc.subject_no and tc.section=ca.section and (c.Criteria_no=ftc.criteria_no or ca.caluationID=ftc.criteria_no_New) and (c.Criteria_no=ttc.criteria_no or ca.caluationID=ttc.criteria_no_New) and tc.subject_no='" + subject_no + "' " + qrySecAll + " order by tc.subject_no,tc.comaparisionID";
                qry = "select distinct ts.comaparisionID,ts.comparisionName,ts.convertedTo from Test_Comparision_Settings ts,FromTest_Comparision ft,ToTest_Comparision tt,subject s where ts.comaparisionID=ft.comaparisionID and tt.comaparisionID=ft.comaparisionID and ts.comaparisionID=tt.comaparisionID and s.subject_no=ts.subject_no and ts.subject_no='" + subject_no + "' " + qrySecAll + " order by ts.comaparisionID ; select ts.subject_no,s.subject_code,s.subject_name,ts.comaparisionID,ts.comparisionName,ts.convertedTo,case when ft.type=1 then ft.Criteria_no else ft.criteria_no_New end as From_Test_No,ft.type as From_Type,case when tt.type=1 then tt.Criteria_no else tt.criteria_no_New end as To_Test_No, tt.type as To_Type from Test_Comparision_Settings ts,FromTest_Comparision ft,ToTest_Comparision tt,subject s where ts.comaparisionID=ft.comaparisionID and tt.comaparisionID=ft.comaparisionID and ts.comaparisionID=tt.comaparisionID and s.subject_no=ts.subject_no and ts.subject_no='" + subject_no + "' " + qrySecAll + " order by ts.subject_no,ts.comaparisionID ;";

                DataSet dsAllCopmarision = new DataSet();

                dsAllCopmarision = d2.select_method_wo_parameter(qry, "Text");

                if (dsAllCopmarision.Tables.Count > 0 && dsAllCopmarision.Tables[0].Rows.Count > 0)
                {
                    Init_Spread(FpSpreadSettings);
                    for (int row = 0; row < dsAllCopmarision.Tables[0].Rows.Count; row++)
                    {
                        string comparisionTestName = Convert.ToString(dsAllCopmarision.Tables[0].Rows[row]["comparisionName"]).Trim();
                        string comparisionID = Convert.ToString(dsAllCopmarision.Tables[0].Rows[row]["comaparisionID"]).Trim();
                        string convertedTo = Convert.ToString(dsAllCopmarision.Tables[0].Rows[row]["convertedTo"]).Trim();
                        int startrow = FpSpreadSettings.Sheets[0].RowCount++;

                        FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                        FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 0].Locked = true;
                        FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                        FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(comparisionTestName);
                        FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(comparisionID);
                        FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 2].Note = Convert.ToString(convertedTo);
                        FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 2].Locked = true;
                        FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                        if (dsAllCopmarision.Tables.Count >= 2 && dsAllCopmarision.Tables[1].Rows.Count > 0)
                        {
                            DataView dv = new DataView();
                            dsAllCopmarision.Tables[1].DefaultView.RowFilter = "comaparisionID='" + comparisionID + "' and comparisionName='" + comparisionTestName + "'";
                            dv = dsAllCopmarision.Tables[1].DefaultView;
                            if (dv.Count > 0)
                            {
                                for (int newrow = 0; newrow < dv.Count; newrow++)
                                {
                                    if (newrow != 0)
                                    {
                                        FpSpreadSettings.Sheets[0].RowCount++;
                                    }
                                    comparisionTestName = Convert.ToString(dsAllCopmarision.Tables[0].Rows[row]["comparisionName"]).Trim();
                                    comparisionID = Convert.ToString(dsAllCopmarision.Tables[0].Rows[row]["comaparisionID"]).Trim();
                                    convertedTo = Convert.ToString(dsAllCopmarision.Tables[0].Rows[row]["convertedTo"]).Trim();
                                    string subjectNo = Convert.ToString(dv[newrow]["subject_no"]);
                                    string subjectName = Convert.ToString(dv[newrow]["subject_name"]);
                                    string subjectCode = Convert.ToString(dv[newrow]["subject_code"]);
                                    string fromTestName = "";// Convert.ToString(dv[newrow]["From_Test"]);
                                    string fromTestNo = Convert.ToString(dv[newrow]["From_Test_No"]);
                                    string fromType = Convert.ToString(dv[newrow]["From_Type"]);
                                    FindTestName(fromTestNo.Trim(), fromType.Trim(), ref fromTestName);
                                    string toTestName = "";// Convert.ToString(dv[newrow]["To_Test"]);
                                    string toType = Convert.ToString(dv[newrow]["To_Type"]);
                                    string toTestNo = Convert.ToString(dv[newrow]["To_Test_No"]);
                                    FindTestName(toTestNo.Trim(), toType.Trim(), ref toTestName);
                                    //string con = Convert.ToString(dv[newrow]["criteria"]);

                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 0].Locked = true;
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(comparisionTestName);
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(comparisionID);
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 2].Note = Convert.ToString(convertedTo);
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

                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(fromTestName);
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(fromTestNo);
                                    //FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 3].Note = Convert.ToString(oldTestConvertTo);
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 3].Locked = true;
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(toTestName);
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(toTestNo);
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 4].Locked = true;
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(convertedTo);
                                    //FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(criteria_no);
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 5].Locked = true;
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpreadSettings.Sheets[0].Cells[FpSpreadSettings.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
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
                    divMainContents.Visible = true;
                }
                else
                {
                    if (!string.IsNullOrEmpty(batch_year) && !string.IsNullOrEmpty(degree_code) && !string.IsNullOrEmpty(semester) && !string.IsNullOrEmpty(subject_no))
                    {
                        //qry = "select * from internal_cam_calculation_master_setting camset,syllabus_master sy where  sy.syll_code=camset.syll_code and Conversion_value is not null   and sy.Batch_Year='" + batch_year + "' and sy.degree_code='" + degree_code + "' and semester='" + semester + "' and subject_no='" + subject_no + "'";
                        qry = "select DISTINCT c.criteria,c.Criteria_no,'1' AS TYPE,CONVERT(nvarchar(500),c.Criteria_no)+';'+'1' as Criteria_no_New from CriteriaForInternal c,syllabus_master sy,Exam_type e where c.syll_code=sy.syll_code and c.Criteria_no=e.criteria_no  and sy.Batch_Year='" + batch_year + "' and sy.degree_code='" + degree_code + "' and sy.semester='" + semester + "' " + qrysec + "  and e.subject_no IN ('" + subject_no + "') union select distinct New_Test_Name as criteria,ca.caluationID as Criteria_no,'2' AS TYPE,CONVERT(nvarchar(500),ca.caluationID)+';'+'2' as Criteria_no_New from CAM_Calculation_Make_New_Test ca,CAM_Calculation_Test_Settings se,CriteriaForInternal c,syllabus_master sy where ca.caluationID=se.caluationID and ca.subject_no=se.subject_no and ca.section=se.section and sy.syll_code=c.syll_code and se.criteria_no=c.Criteria_no and sy.Batch_Year='" + batch_year + "' and sy.degree_code='" + degree_code + "' and sy.semester='" + semester + "' and ca.subject_no IN('" + subject_no + "') " + qrysecnew + " order by TYPE,c.Criteria_no";
                        //select distinct New_Test_Name as criteria,ca.caluationID as Criteria_no,'2' AS TYPE from CAM_Calculation_Make_New_Test ca,CAM_Calculation_Test_Settings se where ca.caluationID=se.caluationID and ca.subject_no=se.subject_no and ca.section=se.section and ca.subject_no IN('" + subject_no + "') " + qrysecnew + " order by TYPE,c.Criteria_no";
                        ds.Clear();
                        ds.Reset();
                        ds = d2.select_method_wo_parameter(qry, "Text");
                        cblFromTest.Items.Clear();
                        cblToTest.Items.Clear();
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            cblFromTest.DataSource = ds;
                            cblFromTest.DataTextField = "criteria";
                            cblFromTest.DataValueField = "Criteria_no_New";
                            cblFromTest.DataBind();

                            cblToTest.DataSource = ds;
                            cblToTest.DataTextField = "criteria";
                            cblToTest.DataValueField = "Criteria_no_New";
                            cblToTest.DataBind();
                            divMainContents.Visible = true;
                            divSettings.Visible = true;
                            divSavedSettings.Visible = false;
                        }
                        else
                        {
                            divMainContents.Visible = false;
                            divSettings.Visible = false;
                            divSavedSettings.Visible = false;
                            lblpopuperr.Text = "No Test Were Conducted For " + Convert.ToString(ddlsubject.SelectedItem.Text);
                            lblpopuperr.Visible = true;
                            popupdiv.Visible = true;
                            return;
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblpopuperr.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;

            string[] fromTestNo = new string[0];
            string[] fromTestName = new string[0];
            string[] fromTestType = new string[0];

            string[] toTestNo = new string[0];
            string[] toTestName = new string[0];
            string[] toTestType = new string[0];

            bool isSuccess = false;
            bool issuc = false;

            double convertedTo = 0;

            string conversionName = string.Empty;

            int fromTestSelCount = 0;
            int toTestSelCount = 0;
            //divMainContents.Visible = false;
            int selSubjectCount = 0;

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
                    qrysec = " and section='" + section + "'";//and e.sections='' and section=''
                    qrysecnew = " and ca.section='" + section + "'";//and ca.section='A'
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

            if (cblFromTest.Items.Count > 0 && cblToTest.Items.Count > 0)
            {
                fromTestSelCount = 0;
                toTestSelCount = 0;
                foreach (ListItem li in cblFromTest.Items)
                {
                    if (li.Selected)
                    {
                        string testNoType = li.Value;
                        string[] testNoTypeSeperator = testNoType.Split(';');
                        fromTestSelCount++;
                        Array.Resize(ref fromTestName, fromTestSelCount);
                        Array.Resize(ref fromTestNo, fromTestSelCount);
                        Array.Resize(ref fromTestType, fromTestSelCount);
                        fromTestName[fromTestSelCount - 1] = li.Text;
                        fromTestNo[fromTestSelCount - 1] = ((testNoTypeSeperator.Length >= 1) ? testNoTypeSeperator[0] : "");
                        fromTestType[fromTestSelCount - 1] = ((testNoTypeSeperator.Length >= 2) ? testNoTypeSeperator[1] : "");
                    }
                }
                foreach (ListItem li in cblToTest.Items)
                {
                    if (li.Selected)
                    {
                        string testNoType = li.Value;
                        string[] testNoTypeSeperator = testNoType.Split(';');
                        toTestSelCount++;
                        Array.Resize(ref toTestName, toTestSelCount);
                        Array.Resize(ref toTestNo, toTestSelCount);
                        Array.Resize(ref toTestType, toTestSelCount);
                        toTestName[toTestSelCount - 1] = li.Text;
                        toTestNo[toTestSelCount - 1] = ((testNoTypeSeperator.Length >= 1) ? testNoTypeSeperator[0] : "");
                        toTestType[toTestSelCount - 1] = ((testNoTypeSeperator.Length >= 2) ? testNoTypeSeperator[1] : "");
                    }
                }

                if (txtNameCompare.Text.Trim() == "")
                {
                    lblpopuperr.Text = "Please Enter The Comparision Test Name";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
                else
                {
                    conversionName = txtNameCompare.Text.Trim();
                }
                bool isConvertedValid = double.TryParse(txtConvertedTo.Text.Trim(), out convertedTo);
                if (isConvertedValid && txtConvertedTo.Text.Trim() != "" && txtConvertedTo.Text.Trim() != "0")
                {

                }
                else
                {
                    lblpopuperr.Text = "Please Enter ConvertTo Marks of Test Comparision And ConvertTo Marks Should Be Greater Than Zero";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
                if (fromTestSelCount == 0)
                {
                    lblpopuperr.Text = "Please Select Atleast One From Test Comparision";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
                if (toTestSelCount == 0)
                {
                    lblpopuperr.Text = "Please Select Atleast One To Comparision";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
                if (conversionName.Trim() != "" && subject_no.Trim() != "")
                {
                    qry = "if exists (select * from Test_Comparision_Settings where comparisionName='" + conversionName + "' and subject_no='" + subject_no + "' " + qrysec + " ) update Test_Comparision_Settings set convertedTo='" + convertedTo + "' where comparisionName='" + conversionName + "' and subject_no='" + subject_no + "' " + qrysec + " else insert into Test_Comparision_Settings (comparisionName,subject_no,section,convertedTo) values ('" + conversionName + "','" + subject_no + "','" + section + "','" + convertedTo + "')";
                    int res = d2.update_method_wo_parameter(qry, "Text");
                    if (res > 0)
                    {
                        isSuccess = true;
                    }
                    string newqry = "select comaparisionID from Test_Comparision_Settings where comparisionName='" + conversionName + "' and subject_no='" + subject_no + "' " + qrysec;
                    string comparisionID = d2.GetFunctionv(newqry);
                    if (comparisionID.Trim() != "")
                    {
                        string qrydel = "delete from FromTest_Comparision where comaparisionID='" + comparisionID.Trim() + "'";// and subject_no='" + subject_no + "'" + qrysec + "";//and criteria_no='" + eachcriteia[sel] + "'
                        int resdel = d2.update_method_wo_parameter(qrydel, "Text");

                        qrydel = "delete from ToTest_Comparision where comaparisionID='" + comparisionID.Trim() + "'";//and criteria_no='" + eachcriteia[sel] + "'
                        resdel = d2.update_method_wo_parameter(qrydel, "Text");

                        for (int from = 0; from < fromTestSelCount; from++)
                        {
                            newqry = "if exists (select * from FromTest_Comparision where comaparisionID='" + comparisionID.Trim() + "'" + ((fromTestType[from].Trim() == "1") ? " and criteria_no='" + fromTestNo[from] + "' " : " and criteria_no_New='" + fromTestNo[from] + "'") + ") update FromTest_Comparision set convertedTo='100'," + ((fromTestType[from].Trim() == "1") ? "criteria_no='" + fromTestNo[from] + "'" : "criteria_no_New='" + fromTestNo[from] + "'") + "  where comaparisionID='" + comparisionID.Trim() + "'" + ((fromTestType[from].Trim() == "1") ? " and criteria_no='" + fromTestNo[from] + "' " : " and criteria_no_New='" + fromTestNo[from] + "'") + " else insert into FromTest_Comparision (" + ((fromTestType[from].Trim() == "1") ? "criteria_no," : "criteria_no_New,") + "comaparisionID,convertedTo,type) values ('" + fromTestNo[from] + "','" + comparisionID.Trim() + "','100','" + fromTestType[from].Trim() + "')";
                            int newres = d2.update_method_wo_parameter(newqry, "Text");
                            if (newres > 0)
                            {
                                issuc = true;
                            }
                        }
                        for (int to = 0; to < toTestSelCount; to++)
                        {
                            newqry = "if exists (select * from ToTest_Comparision where comaparisionID='" + comparisionID.Trim() + "'" + ((toTestType[to].Trim() == "1") ? " and criteria_no='" + toTestNo[to] + "' " : " and criteria_no_New='" + toTestNo[to] + "'") + ") update ToTest_Comparision set convertedTo='100'," + ((toTestType[to].Trim() == "1") ? "criteria_no='" + toTestNo[to] + "'" : "criteria_no_New='" + toTestNo[to] + "'") + "  where comaparisionID='" + comparisionID.Trim() + "'" + ((toTestType[to].Trim() == "1") ? " and criteria_no='" + toTestNo[to] + "' " : " and criteria_no_New='" + toTestNo[to] + "'") + " else insert into ToTest_Comparision (" + ((toTestType[to].Trim() == "1") ? "criteria_no," : "criteria_no_New,") + "comaparisionID,convertedTo,type) values ('" + toTestNo[to] + "','" + comparisionID.Trim() + "','100','" + toTestType[to].Trim() + "')";
                            int newres = d2.update_method_wo_parameter(newqry, "Text");
                            if (newres > 0)
                            {
                                issuc = true;
                            }
                        }
                    }
                }
                if (issuc && isSuccess)
                {
                    btngo_Click(sender, e);
                    lblpopuperr.Text = "Saved Successfully";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
                else
                {
                    lblpopuperr.Text = "Not Saved";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
            }
            else
            {
                lblpopuperr.Text = "No Test were Conducted";
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

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblpopuperr.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;

            //divMainContents.Visible = false;
            divSettings.Visible = false;
            //divSavedSettings.Visible = false;
            //divMainContents.Visible = false;
            int selSubjectCount = 0;

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
                    qrysec = " and e.sections='" + section + "'";//and e.sections=''
                    qrysecnew = " and ca.section='" + section + "'";//and ca.section='A'
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
            txtConvertedTo.Text = string.Empty;
            txtNameCompare.Text = string.Empty;
            if (!string.IsNullOrEmpty(batch_year) && !string.IsNullOrEmpty(degree_code) && !string.IsNullOrEmpty(semester) && !string.IsNullOrEmpty(subject_no))
            {
                //qry = "select * from internal_cam_calculation_master_setting camset,syllabus_master sy where  sy.syll_code=camset.syll_code and Conversion_value is not null   and sy.Batch_Year='" + batch_year + "' and sy.degree_code='" + degree_code + "' and semester='" + semester + "' and subject_no='" + subject_no + "'";
                qry = "select DISTINCT c.criteria,c.Criteria_no,'1' AS TYPE,CONVERT(nvarchar(500),c.Criteria_no)+';'+'1' as Criteria_no_New from CriteriaForInternal c,syllabus_master sy,Exam_type e where c.syll_code=sy.syll_code and c.Criteria_no=e.criteria_no  and sy.Batch_Year='" + batch_year + "' and sy.degree_code='" + degree_code + "' and sy.semester='" + semester + "' " + qrysec + "  and e.subject_no IN ('" + subject_no + "') union select distinct New_Test_Name as criteria,ca.caluationID as Criteria_no,'2' AS TYPE,CONVERT(nvarchar(500),ca.caluationID)+';'+'2' as Criteria_no_New from CAM_Calculation_Make_New_Test ca,CAM_Calculation_Test_Settings se,CriteriaForInternal c,syllabus_master sy where ca.caluationID=se.caluationID and ca.subject_no=se.subject_no and ca.section=se.section and sy.syll_code=c.syll_code and se.criteria_no=c.Criteria_no and sy.Batch_Year='" + batch_year + "' and sy.degree_code='" + degree_code + "' and sy.semester='" + semester + "' and ca.subject_no IN('" + subject_no + "') " + qrysecnew + " order by TYPE,c.Criteria_no";
                //select distinct New_Test_Name as criteria,ca.caluationID as Criteria_no,'2' AS TYPE from CAM_Calculation_Make_New_Test ca,CAM_Calculation_Test_Settings se where ca.caluationID=se.caluationID and ca.subject_no=se.subject_no and ca.section=se.section and ca.subject_no IN('" + subject_no + "') " + qrysecnew + " order by TYPE,c.Criteria_no";
                ds.Clear();
                ds.Reset();
                ds = d2.select_method_wo_parameter(qry, "Text");
                cblFromTest.Items.Clear();
                cblToTest.Items.Clear();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cblFromTest.DataSource = ds;
                    cblFromTest.DataTextField = "criteria";
                    cblFromTest.DataValueField = "Criteria_no_New";
                    cblFromTest.DataBind();

                    cblToTest.DataSource = ds;
                    cblToTest.DataTextField = "criteria";
                    cblToTest.DataValueField = "Criteria_no_New";
                    cblToTest.DataBind();
                    divMainContents.Visible = true;
                    divSettings.Visible = true;
                    //divSavedSettings.Visible = false;
                }
                else
                {
                    divSettings.Visible = false;
                    lblpopuperr.Text = "No Test Were Conducted For " + Convert.ToString(ddlsubject.SelectedItem.Text);
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = "";
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion  Button Events

    private void FindTestName(string testNo, string Type, ref string testName)
    {
        try
        {
            testNo = testNo.Trim();
            Type = Type.Trim();

            if (testNo != "")
            {
                switch (Type)
                {
                    case "1":
                        testName = d2.GetFunctionv("select criteria from CriteriaForInternal where Criteria_no='" + testNo + "'");
                        break;
                    case "2":
                        testName = d2.GetFunctionv("select New_Test_Name from CAM_Calculation_Make_New_Test where caluationID='" + testNo + "'");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

}