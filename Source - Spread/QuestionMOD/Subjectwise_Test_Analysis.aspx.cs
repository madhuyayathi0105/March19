using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Configuration;

public partial class Subjectwise_Test_Analysis : System.Web.UI.Page
{
    Hashtable hat = new Hashtable();
    DAccess2 d2 = new DAccess2();
    DataSet studgradeds = new DataSet();
    DataSet ds = new DataSet();

    string imgperfm = "PerformanceChart.png";
    string imgavg = "avganalysischart.png";
    string group_user = string.Empty;
    string singleuser = string.Empty;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string group_code = string.Empty;
    string strorderby = string.Empty;
    string degreecode = string.Empty;
    string semester = string.Empty;
    string section = string.Empty;
    string subject = string.Empty;
    string subject_no = string.Empty;
    static string batchyear = string.Empty;

    bool serialflag = false;
    bool isSchool = false;

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
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

        string grouporusercode1 = string.Empty;
        if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode1 = " and group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else
        {
            grouporusercode1 = " and usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }

        DataSet schoolds = new DataSet();
        string sqlschool = "select * from Master_Settings where settings='schoolorcollege' " + grouporusercode1 + "";
        schoolds.Clear();
        schoolds.Dispose();
        schoolds = d2.select_method_wo_parameter(sqlschool, "Text");
        if (schoolds.Tables[0].Rows.Count > 0)
        {
            string schoolvalue = Convert.ToString(schoolds.Tables[0].Rows[0]["value"]).Trim();
            if (schoolvalue.Trim() == "0")
            {
                isSchool = true;
            }
        }
        if (!IsPostBack)
        {

            #region LoadFilters

            bindcollege();
            bindbatch();
            binddegree();
            bindbranch();
            bindsem();
            bindsec();
            bindtest();
            bindsub();
            ChangeHeaderName(isSchool);

            #endregion LoadFilters

            rptprint1.Visible = false;
            lblerrormsg.Visible = false;
            lblErr.Visible = false;
            gvTestPerfm.Visible = false;
            PerformanceChart.Visible = false;
            gvAvgcount.Visible = false;
            chartAvg.Visible = false;
            lblerrormsg.Text = string.Empty;
            lblErr.Text = string.Empty;

            //Initialize Spread

            FpSubTest.Visible = false;
            FpSubTest.CommandBar.Visible = false;
            FpSubTest.RowHeader.Visible = false;
            FpSubTest.Sheets[0].AutoPostBack = true;
            FpSubTest.Sheets[0].RowCount = 0;
            FpSubTest.Sheets[0].ColumnCount = 3;

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
            darkstyle.Border.BorderSize = 0;
            darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;

            #endregion SpreadStyles

            FpSubTest.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSubTest.Sheets[0].ColumnHeader.RowCount = 2;
            FpSubTest.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.NO";
            FpSubTest.Sheets[0].Columns[0].Width = 50;
            FpSubTest.Sheets[0].Columns[1].Width = 150;
            FpSubTest.Sheets[0].Columns[2].Width = 250;
            FpSubTest.Sheets[0].ColumnHeader.Cells[0, 1].Text = "ROLL NO";
            FpSubTest.Sheets[0].ColumnHeader.Cells[0, 2].Text = "NAME OF THE STUDENT";
            FpSubTest.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpSubTest.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            FpSubTest.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);

        }

        if (ddl_collage.Items.Count > 0)
        {
            collegecode = Convert.ToString(ddl_collage.SelectedValue).Trim();
        }
    }

    #endregion Page Load

    #region Logout

    protected void logout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    #endregion Logout

    #region Bind_Filter

    public void bindcollege()
    {
        try
        {
            string columnfield = string.Empty;
            if (Session["UserCode"] != null)
                usercode = Convert.ToString(Session["UserCode"]).Trim();
            if (Session["group_code"] != null)
                group_code = Convert.ToString(Session["group_code"]).Trim();
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = Convert.ToString(group_semi[0]).Trim();
            }
            if ((Convert.ToString(group_code).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]).Trim() != "1" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true" && Convert.ToString(Session["single_user"]).Trim().ToUpper() != "TRUE" && Convert.ToString(Session["single_user"]).Trim() != "True"))
            {
                columnfield = " and group_code='" + Convert.ToString(group_code).Trim() + "'";
            }
            else
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            hat.Clear();
            hat.Add("column_field", Convert.ToString(columnfield));
            studgradeds.Clear();
            studgradeds = d2.select_method("bind_college", hat, "sp");
            ddl_collage.Items.Clear();
            if (studgradeds.Tables.Count > 0 && studgradeds.Tables[0].Rows.Count > 0)
            {
                ddl_collage.DataSource = studgradeds;
                ddl_collage.DataTextField = "collname";
                ddl_collage.DataValueField = "college_code";
                ddl_collage.DataBind();
                ddl_collage.SelectedIndex = 0;
                collegecode = Convert.ToString(ddl_collage.SelectedValue).Trim();
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void bindbatch()
    {
        try
        {
            ddl_Batch.Items.Clear();
            studgradeds.Clear();
            studgradeds = d2.select_method_wo_parameter("bind_batch", "sp");

            if (studgradeds.Tables.Count > 0)
            {
                int count = studgradeds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    ddl_Batch.DataSource = studgradeds;
                    ddl_Batch.DataTextField = "batch_year";
                    ddl_Batch.DataValueField = "batch_year";
                    ddl_Batch.DataBind();
                }
                if (studgradeds.Tables.Count > 1 && studgradeds.Tables[1].Rows.Count > 0)
                {
                    int max_bat = 0;
                    int.TryParse(Convert.ToString(studgradeds.Tables[1].Rows[0][0]).Trim(), out max_bat);
                    ddl_Batch.SelectedValue = Convert.ToString(max_bat).Trim();
                    batchyear = Convert.ToString(ddl_Batch.SelectedValue).Trim();
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void binddegree()
    {
        try
        {
            ddl_degree.Items.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = ddl_collage.SelectedItem.Value;
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Clear();
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            studgradeds.Clear();
            studgradeds = d2.select_method("bind_degree", hat, "sp");
            if (studgradeds.Tables[0].Rows.Count > 0)
            {
                ddl_degree.DataSource = studgradeds;
                ddl_degree.DataTextField = "course_name";
                ddl_degree.DataValueField = "course_id";
                ddl_degree.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void bindbranch()
    {
        try
        {
            hat.Clear();
            usercode = Session["usercode"].ToString();
            //collegecode = collegecode.ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("course_id", ddl_degree.SelectedValue);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            studgradeds.Clear();
            studgradeds = d2.select_method("bind_branch", hat, "sp");
            if (studgradeds.Tables[0].Rows.Count > 0)
            {
                ddl_Branch.DataSource = studgradeds;
                ddl_Branch.DataTextField = "dept_name";
                ddl_Branch.DataValueField = "degree_code";
                ddl_Branch.DataBind();
                ddl_Branch.SelectedIndex = 0;
                degreecode = ddl_Branch.SelectedValue.ToString();
                bindsem();
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void bindsem()
    {
        try
        {
            ddl_Sem.Items.Clear();
            lblerrormsg.Text = string.Empty;
            lblerrormsg.Visible = false;
            cbl_sem.Items.Clear();
            cb_sem.Checked = false;
            txtSem.Text = "---Select---";
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;

            degreecode = string.Empty;
            collegecode = string.Empty;
            batchyear = string.Empty;
            semester = string.Empty;

            if (ddl_collage.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddl_collage.SelectedItem.Value).Trim();
            }
            if (ddl_Batch.Items.Count > 0)
            {
                batchyear = Convert.ToString(ddl_Batch.SelectedItem.Text).Trim();
            }
            if (ddl_Branch.Items.Count > 0)
            {
                degreecode = Convert.ToString(ddl_Branch.SelectedItem.Value).Trim();
            }
            DataSet ds = new DataSet();
            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(batchyear) && !string.IsNullOrEmpty(degreecode))
            {
                string sqlnew = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and college_code='" + collegecode + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(sqlnew, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]).Trim(), out first_year);
                    int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]).Trim(), out duration);
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddl_Sem.Items.Add(Convert.ToString(i).Trim());
                            cbl_sem.Items.Add(Convert.ToString(i).Trim());
                        }
                        else if (first_year == true && i == 2)
                        {
                            ddl_Sem.Items.Add(Convert.ToString(i).Trim());
                            cbl_sem.Items.Add(Convert.ToString(i).Trim());
                        }
                    }
                }
                else
                {
                    sqlnew = "select distinct duration,first_year_nonsemester  from degree where degree_code=" + degreecode + " and college_code=" + collegecode + "";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(sqlnew, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]).Trim(), out first_year);
                        int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]).Trim(), out duration);
                        for (i = 1; i <= duration; i++)
                        {
                            if (first_year == false)
                            {
                                ddl_Sem.Items.Add(Convert.ToString(i).Trim());
                                cbl_sem.Items.Add(Convert.ToString(i).Trim());
                            }
                            else if (first_year == true && i != 2)
                            {
                                ddl_Sem.Items.Add(Convert.ToString(i).Trim());
                                cbl_sem.Items.Add(Convert.ToString(i).Trim());
                            }
                        }
                    }
                }
            }
            if (ddl_Sem.Items.Count > 0)
            {
                ddl_Sem.SelectedIndex = 0;
                semester = Convert.ToString(ddl_Sem.SelectedValue).Trim();
                bindsec();
            }
            if (cbl_sem.Items.Count > 0)
            {
                for (i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = true;
                }
                txtSem.Text = lbl_Sem.Text.Trim() + "(" + cbl_sem.Items.Count + ")";// ((!isSchool) ? "Semester(" : "Term(") + 
                cb_sem.Checked = true;
                bindsec();
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void bindsec()
    {
        try
        {
            ddl_Sec.Enabled = false;
            ddl_Sec.Items.Clear();
            hat.Clear();
            studgradeds.Clear();
            studgradeds = d2.BindSectionDetail(ddl_Batch.SelectedValue, ddl_Branch.SelectedValue);
            int count5 = studgradeds.Tables[0].Rows.Count;
            if (count5 > 0)
            {
                ddl_Sec.DataSource = studgradeds;
                ddl_Sec.DataTextField = "sections";
                ddl_Sec.DataValueField = "sections";
                ddl_Sec.DataBind();
                ddl_Sec.Enabled = true;
                ddl_Sec.Items.Insert(0, "All");
            }
            else
            {
                ddl_Sec.Enabled = false;
            }
            if (ddl_Sec.Enabled == true)
            {
                if (ddl_Sec.Items.Count > 0)
                {
                    if (Convert.ToString(ddl_Sec.SelectedItem.Text).Trim().ToLower() != "all" && Convert.ToString(ddl_Sec.SelectedItem.Text).Trim().ToLower() != "" && Convert.ToString(ddl_Sec.SelectedItem.Text).Trim().ToLower() != "-1")
                    {
                        //for (int sc = 0; sc < ddl_Sec.Items.Count; sc++)
                        //{
                        section = Convert.ToString(ddl_Sec.SelectedItem.Text).Trim();
                        //}
                    }
                    else
                    {
                        section = string.Empty;
                    }
                }
            }
            else
            {
                section = string.Empty;
            }
        }
        catch (Exception ex)
        {

        }
    }

    //public void bindsub()
    //{
    //    try
    //    {
    //        ddl_subject.Items.Clear();
    //        //Query For all subjects with lab and other types and theories
    //        //string query = "select subType_no,subject_code,subject_name,subject_no,syll_code,acronym from subject  where syll_code in (select syll_code from syllabus_master where Batch_Year='" + ddl_Batch.SelectedValue.ToString() + "' and semester='" + ddl_Sem.SelectedValue.ToString() + "' and degree_code='" + ddl_Branch.SelectedValue.ToString() + "') order by subject_no";

    //        //Query For only theory subjects

    //        string query = "select s.subType_no,subject_code,subject_name,subject_no,s.syll_code,acronym from subject s,sub_sem ss where s.syll_code=ss.syll_code and ss.subType_no=s.subType_no and ss.Lab<>1 and promote_count=1 and s.syll_code in (select syll_code from syllabus_master where Batch_Year='" + ddl_Batch.SelectedValue.ToString() + "' and semester='" + ddl_Sem.SelectedValue.ToString() + "' and degree_code='" + ddl_Branch.SelectedValue.ToString() + "') order by subject_no";
    //        studgradeds = d2.select_method_wo_parameter(query, "Text");
    //        int count5 = studgradeds.Tables[0].Rows.Count;
    //        if (count5 > 0)
    //        {
    //            ddl_subject.DataSource = studgradeds;
    //            ddl_subject.DataTextField = "subject_name";
    //            ddl_subject.DataValueField = "subject_no";
    //            ddl_subject.DataBind();
    //            ddl_subject.SelectedIndex = 0;
    //            subject = ddl_subject.SelectedItem.Text.ToString();
    //            subject_no = ddl_subject.SelectedValue.ToString();
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}

    public void bindsub()
    {
        try
        {
            string subjectquery = string.Empty;
            ddl_subject.Items.Clear();
            string sections = string.Empty;
            string strsec = string.Empty;
            degreecode = string.Empty;
            collegecode = string.Empty;
            batchyear = string.Empty;
            string semester = string.Empty;

            if (ddl_collage.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddl_collage.SelectedItem.Value).Trim();
            }
            if (ddl_Batch.Items.Count > 0)
            {
                batchyear = Convert.ToString(ddl_Batch.SelectedItem.Text).Trim();
            }
            if (ddl_Branch.Items.Count > 0)
            {
                degreecode = Convert.ToString(ddl_Branch.SelectedItem.Value).Trim();
            }

            if (ddl_Sec.Items.Count > 0)
            {
                sections = Convert.ToString(ddl_Sec.SelectedValue).Trim();
                if (Convert.ToString(ddl_Sec.SelectedItem.Text).Trim().ToLower() == "all" || Convert.ToString(ddl_Sec.SelectedItem.Text).Trim().ToLower() == "" || Convert.ToString(ddl_Sec.SelectedItem.Text).Trim().ToLower() == "-1")
                {
                    strsec = "";
                }
                else
                {
                    strsec = " and st.Sections='" + Convert.ToString(sections).Trim() + "'";
                }
            }

            string sems = string.Empty;
            int selSem = 0;
            semester = string.Empty;
            if (cbl_sem.Items.Count > 0)
            {
                foreach (System.Web.UI.WebControls.ListItem liSem in cbl_sem.Items)
                {
                    if (liSem.Selected)
                    {
                        selSem++;
                        if (string.IsNullOrEmpty(semester))
                        {
                            semester = "'" + liSem.Value + "'";
                        }
                        else
                        {
                            semester += ",'" + liSem.Value + "'";
                        }
                    }
                }
                if (selSem > 0 && !string.IsNullOrEmpty(semester.Trim()) && !string.IsNullOrEmpty(batchyear) && !string.IsNullOrEmpty(degreecode))
                {
                    sems = " and SM.semester in(" + semester + ")";
                    if (Session["Staff_Code"] != null && Convert.ToString(Session["Staff_Code"]).Trim() == "")
                    {
                        subjectquery = "select distinct S.subject_Code,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and SM.degree_code=" + Convert.ToString(degreecode) + " " + Convert.ToString(sems) + " and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.batch_year='" + Convert.ToString(batchyear) + "' order by S.subject_Code ";
                    }
                    else if (Session["Staff_Code"] != null && Convert.ToString(Session["Staff_Code"]).Trim() != "")
                    {
                        subjectquery = "select distinct S.subject_Code,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and st.subject_no=s.subject_no and s.syll_code=SM.syll_code and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.degree_code='" + Convert.ToString(degreecode).Trim() + "' " + Convert.ToString(sems).Trim() + " and  SM.batch_year='" + Convert.ToString(batchyear).Trim() + "'  and SC.staff_code='" + Convert.ToString(Session["Staff_Code"]).Trim() + "' " + strsec + "  order by S.subject_Code ";
                    }
                    if (!string.IsNullOrEmpty(subjectquery))
                    {
                        studgradeds.Dispose();
                        studgradeds.Reset();
                        studgradeds = d2.select_method(subjectquery, hat, "Text");
                        if (studgradeds.Tables.Count > 0 && studgradeds.Tables[0].Rows.Count > 0)
                        {
                            ddl_subject.Enabled = true;
                            ddl_subject.DataSource = studgradeds;
                            ddl_subject.DataValueField = "subject_Code";
                            ddl_subject.DataTextField = "Subject_Name";
                            ddl_subject.DataBind();
                        }
                        else
                        {
                            ddl_subject.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void bindtest()
    {
        Txt_Test.Text = "--Select--";
        Cb_test.Checked = false;
        Cbl_test.Items.Clear();
        DataSet titles = new DataSet();
        string sems = string.Empty;
        int selSem = 0;
        string semester = string.Empty;
        string subjectCode = string.Empty;
        string sections = string.Empty;
        string strsec = string.Empty;

        degreecode = string.Empty;
        collegecode = string.Empty;
        batchyear = string.Empty;

        if (ddl_collage.Items.Count > 0)
        {
            collegecode = Convert.ToString(ddl_collage.SelectedItem.Value).Trim();
        }
        if (ddl_Batch.Items.Count > 0)
        {
            batchyear = Convert.ToString(ddl_Batch.SelectedItem.Text).Trim();
        }
        if (ddl_Branch.Items.Count > 0)
        {
            degreecode = Convert.ToString(ddl_Branch.SelectedItem.Value).Trim();
        }

        if (ddl_subject.Items.Count > 0)
        {
            subjectCode = Convert.ToString(ddl_subject.SelectedValue).Trim();
        }

        if (ddl_Sec.Items.Count > 0)
        {
            sections = Convert.ToString(ddl_Sec.SelectedValue).Trim();
            if (Convert.ToString(ddl_Sec.SelectedItem.Text).Trim().ToLower() == "all" || Convert.ToString(ddl_Sec.SelectedItem.Text).Trim().ToLower() == "" || Convert.ToString(ddl_Sec.SelectedItem.Text).Trim().ToLower() == "-1")
            {
                strsec = "";
            }
            else
            {
                strsec = " and isnull(ltrim(rtrim(r.Sections)),'')='" + Convert.ToString(sections).Trim() + "'";
            }
        }
        if (cbl_sem.Items.Count > 0)
        {
            foreach (System.Web.UI.WebControls.ListItem liSem in cbl_sem.Items)
            {
                if (liSem.Selected)
                {
                    selSem++;
                    if (string.IsNullOrEmpty(semester))
                    {
                        semester = "'" + liSem.Value + "'";
                    }
                    else
                    {
                        semester += ",'" + liSem.Value + "'";
                    }
                }
            }
            if (selSem > 0 && !string.IsNullOrEmpty(semester.Trim()) && !string.IsNullOrEmpty(subjectCode) && !string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(batchyear) && !string.IsNullOrEmpty(degreecode))
            {
                sems = " and s.semester in(" + semester + ")";
                string Sqlstr = "select distinct c.criteria,c.criteria_no from criteriaforinternal c,registration r,syllabus_master s,Exam_type et,subject sub where et.batch_year=r.Batch_Year and et.criteria_no=c.Criteria_no and et.subject_no=sub.subject_no and sub.syll_code=s.syll_code and sub.syll_code=c.syll_code and  r.degree_code=s.degree_code and r.batch_year=s.batch_year and c.syll_code=s.syll_code and cc=0 and delflag=0 and r.exam_flag<>'debar' and r.batch_year='" + batchyear + "' and r.college_code='" + collegecode + "' and sub.subject_code='" + subjectCode + "' and r.degree_code in(" + degreecode + ") " + sems + strsec + " order by c.criteria_no asc";
                titles.Clear();
                titles.Dispose();
                titles = d2.select_method_wo_parameter(Sqlstr, "Test");

                if (titles.Tables.Count > 0 && titles.Tables[0].Rows.Count > 0)
                {
                    Cbl_test.DataSource = titles;
                    Cbl_test.DataValueField = "criteria_no";
                    Cbl_test.DataTextField = "criteria";
                    Cbl_test.DataBind();
                }
            }
          
        }
        if (Cbl_test.Items.Count > 0)
        {
            for (int row = 0; row < Cbl_test.Items.Count; row++)
            {
                Cbl_test.Items[row].Selected = true;
                Cb_test.Checked = true;
            }
            Txt_Test.Text = "Test(" + Cbl_test.Items.Count + ")";
        }
        else
        {
            Txt_Test.Text = "--Select--";
        }

    }

    public void ChangeHeaderName(bool isschool)
    {
        try
        {
            lblerrormsg.Text = string.Empty;
            lblerrormsg.Visible = false;
            lblCollege.Text = ((!isschool) ? "College" : "School");
            lbl_Batch.Text = ((!isschool) ? "Batch" : "Year");
            lbl_Degree.Text = ((!isschool) ? "Degree" : "School Type");
            lbl_Branch.Text = ((!isschool) ? "Department" : "Standard");
            lbl_Sem.Text = ((!isschool) ? "Semester" : "Term");
            lbl_Sec.Text = ((!isschool) ? "Section" : "Section");
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    #endregion Bind_Filter

    #region DDL&Filter_Events

    protected void ddl_collage_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrormsg.Visible = false;
        lblErr.Visible = false;
        gvTestPerfm.Visible = false;
        PerformanceChart.Visible = false;
        gvAvgcount.Visible = false;
        chartAvg.Visible = false;
        rptprint1.Visible = false;
        lblerrormsg.Text = string.Empty;
        lblErr.Text = string.Empty;
        FpSubTest.Visible = false;
        collegecode = Convert.ToString(ddl_collage.SelectedValue).Trim();
        bindbatch();
        binddegree();
        bindbranch();
        bindsem();
        bindsec();
        bindsub();

    }

    protected void ddl_Batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrormsg.Visible = false;
        lblErr.Visible = false;
        gvTestPerfm.Visible = false;
        PerformanceChart.Visible = false;
        gvAvgcount.Visible = false;
        chartAvg.Visible = false;
        rptprint1.Visible = false;
        lblerrormsg.Text = string.Empty;
        lblErr.Text = string.Empty;
        FpSubTest.Visible = false;
        batchyear = Convert.ToString(ddl_Batch.SelectedValue).Trim();
        binddegree();
        bindbranch();
        bindsem();
        bindsec();
        bindsub();
        bindtest();
    }

    protected void ddl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrormsg.Visible = false;
        lblErr.Visible = false;
        gvTestPerfm.Visible = false;
        PerformanceChart.Visible = false;
        gvAvgcount.Visible = false;
        chartAvg.Visible = false;
        rptprint1.Visible = false;
        lblerrormsg.Text = string.Empty;
        lblErr.Text = string.Empty;
        FpSubTest.Visible = false;
        bindbranch();
        bindsem();
        bindsec();
        bindsub();
        bindtest();
    }

    protected void ddl_Branch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrormsg.Visible = false;
        lblErr.Visible = false;
        gvTestPerfm.Visible = false;
        PerformanceChart.Visible = false;
        gvAvgcount.Visible = false;
        chartAvg.Visible = false;
        rptprint1.Visible = false;
        lblerrormsg.Text = string.Empty;
        lblErr.Text = string.Empty;
        FpSubTest.Visible = false;
        degreecode = Convert.ToString(ddl_Branch.SelectedValue).Trim();
        bindsem();
        bindsec();
        bindsub();
        bindtest();
    }

    protected void ddl_Sem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrormsg.Visible = false;
        lblErr.Visible = false;
        gvTestPerfm.Visible = false;
        PerformanceChart.Visible = false;
        gvAvgcount.Visible = false;
        chartAvg.Visible = false;
        rptprint1.Visible = false;
        lblerrormsg.Text = string.Empty;
        lblErr.Text = string.Empty;
        FpSubTest.Visible = false;
        semester = Convert.ToString(ddl_Sem.SelectedValue).Trim();
        bindsec();
        bindsub();
        bindtest();
    }

    protected void ddl_Sec_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrormsg.Visible = false;
        lblErr.Visible = false;
        gvTestPerfm.Visible = false;
        PerformanceChart.Visible = false;
        gvAvgcount.Visible = false;
        chartAvg.Visible = false;
        rptprint1.Visible = false;
        lblerrormsg.Text = string.Empty;
        lblErr.Text = string.Empty;
        FpSubTest.Visible = false;
        if (ddl_Sec.Enabled == true)
        {
            if (ddl_Sec.Items.Count > 0)
            {
                if (Convert.ToString(ddl_Sec.SelectedItem.Text).Trim().ToLower() != "all" && Convert.ToString(ddl_Sec.SelectedItem.Text).Trim().ToLower() != "" && Convert.ToString(ddl_Sec.SelectedItem.Text).Trim().ToLower() != "-1")
                {
                    section = Convert.ToString(ddl_Sec.SelectedItem.Text).Trim();
                }
            }
            else
            {
                section = string.Empty;
            }
        }
        else
        {
            section = string.Empty;
        }
        bindsub();
    }

    protected void ddl_subject_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrormsg.Visible = false;
        lblErr.Visible = false;
        gvTestPerfm.Visible = false;
        PerformanceChart.Visible = false;
        gvAvgcount.Visible = false;
        chartAvg.Visible = false;
        rptprint1.Visible = false;
        lblerrormsg.Text = string.Empty;
        lblErr.Text = string.Empty;
        FpSubTest.Visible = false;
        subject = Convert.ToString(ddl_subject.SelectedItem.Text).Trim();
        subject_no = Convert.ToString(ddl_subject.SelectedValue).Trim();
        bindtest();
    }

    protected void Cb_test_CheckedChanged(object sender, EventArgs e)
    {
        int cout = 0;
        Txt_Test.Text = "--Select--";
        lblerrormsg.Visible = false;
        lblErr.Visible = false;
        gvTestPerfm.Visible = false;
        PerformanceChart.Visible = false;
        gvAvgcount.Visible = false;
        chartAvg.Visible = false;
        rptprint1.Visible = false;
        lblerrormsg.Text = string.Empty;
        lblErr.Text = string.Empty;
        FpSubTest.Visible = false;
        if (Cb_test.Checked == true)
        {
            cout++;
            for (int i = 0; i < Cbl_test.Items.Count; i++)
            {
                Cbl_test.Items[i].Selected = true;
            }
            Txt_Test.Text = "Test(" + (Cbl_test.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < Cbl_test.Items.Count; i++)
            {
                Cbl_test.Items[i].Selected = false;
            }
            Txt_Test.Text = "--Select--";
        }
    }

    protected void Cbl_test_SelectedIndexChanged(object sender, EventArgs e)
    {
        Txt_Test.Text = "--Select--";
        Cb_test.Checked = false;
        lblerrormsg.Visible = false;
        lblErr.Visible = false;
        gvTestPerfm.Visible = false;
        PerformanceChart.Visible = false;
        gvAvgcount.Visible = false;
        chartAvg.Visible = false;
        rptprint1.Visible = false;
        lblerrormsg.Text = string.Empty;
        lblErr.Text = string.Empty;
        FpSubTest.Visible = false;
        int commcount = 0;
        for (int i = 0; i < Cbl_test.Items.Count; i++)
        {
            if (Cbl_test.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                Cb_test.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == Cbl_test.Items.Count)
            {
                Cb_test.Checked = true;
            }
            Txt_Test.Text = "Test(" + commcount.ToString() + ")";

        }
    }

    protected void cb_sem_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Text = string.Empty;
            lblerrormsg.Visible = false;
            //popupdiv.Visible = false;
            //divViewSpread.Visible = false;
            int i = 0;
            txtSem.Text = "--Select--";
            if (cb_sem.Checked == true)
            {
                for (i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = true;
                }
                txtSem.Text = ((!isSchool) ? "Semester(" : "Term(") + (cbl_sem.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = false;
                }
            }
            bindsec();
            bindsub();
            bindtest();
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    protected void cbl_sem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Text = string.Empty;
            lblerrormsg.Visible = false;
            //popupdiv.Visible = false;
            //divViewSpread.Visible = false;

            int i = 0;
            cb_sem.Checked = false;
            int commcount = 0;
            txtSem.Text = "--Select--";
            for (i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_sem.Items.Count)
                {
                    cb_sem.Checked = true;
                }
                txtSem.Text = ((isSchool) ? "Term(" : "Semester(") + commcount.ToString() + ")";
            }
            bindsec();
            bindsub();
            bindtest();
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    #endregion DDL&Filter_Events

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            rptprint1.Visible = false;
            lblerrormsg.Visible = false;
            lblErr.Visible = false;
            gvTestPerfm.Visible = false;
            PerformanceChart.Visible = false;
            gvAvgcount.Visible = false;
            chartAvg.Visible = false;
            FpSubTest.Visible = false;

            lblerrormsg.Text = "";
            lblErr.Text = "";

            DataTable dtPerform = new DataTable();
            DataTable dtAvg = new DataTable();

            DataTable dtStudTest = new DataTable();
            DataColumn dcStudTest = new DataColumn();
            DataRow drStud;

            DataColumn dcPerf = new DataColumn();

            DataRow[] drPerf = new DataRow[3];
            DataRow drAvg;

            int testcount = 0;
            string testname = "", criteriano = "";
            string testno = "";

            int[] lpcount, spcount, impcount, belowcount50, bw50_59, bw60_74, bw75_89, gt90;

            FpSubTest.Sheets[0].RowCount = 0;
            FpSubTest.Sheets[0].ColumnCount = 3;

            dtStudTest.Columns.Add("S.No");
            dtStudTest.Columns.Add("Roll No");
            dtStudTest.Columns.Add("Name of The Student");

            if (ddl_collage.Items.Count > 0)
                collegecode = Convert.ToString(ddl_collage.SelectedValue).Trim();
            else
            {
                lblerrormsg.Text = "There are No " + lblCollege.Text.Trim() + " are Found";
                lblerrormsg.Visible = true;
                return;
            }

            if (ddl_Batch.Items.Count > 0)
                batchyear = Convert.ToString(ddl_Batch.SelectedValue).Trim();
            else
            {
                lblerrormsg.Text = "There are No " + lbl_Batch.Text.Trim() + " are Found";
                lblerrormsg.Visible = true;
                return;
            }

            if (ddl_Branch.Items.Count > 0)
                degreecode = Convert.ToString(ddl_Branch.SelectedValue).Trim();
            else
            {
                lblerrormsg.Text = "There are No " + lbl_Branch.Text.Trim() + " are Found!";
                lblerrormsg.Visible = true;
                return;
            }

            if (ddl_Sem.Items.Count > 0)
                semester = Convert.ToString(ddl_Sem.SelectedValue).Trim();
            else
            {
                lblerrormsg.Text = "There are No " + lbl_Sem.Text.Trim() + " are Found!";
                lblerrormsg.Visible = true;
                return;
            }
            if (ddl_Sec.Enabled == true)
            {
                if (ddl_Sec.Items.Count > 0)
                {
                    if (Convert.ToString(ddl_Sec.SelectedItem.Text).Trim().ToLower() != "all" && Convert.ToString(ddl_Sec.SelectedItem.Text).Trim().ToLower() != "" && Convert.ToString(ddl_Sec.SelectedItem.Text).Trim().ToLower() != "-1")
                    {
                        section = Convert.ToString(ddl_Sec.SelectedItem.Text).Trim();
                    }
                    else
                    {
                        section = "";
                    }
                }
            }
            else
            {
                section = "";
            }

            if (ddl_subject.Items.Count > 0)
            {
                subject = Convert.ToString(ddl_subject.SelectedItem.Text).Trim();
                subject_no = Convert.ToString(ddl_subject.SelectedValue).Trim();
            }
            else
            {
                lblerrormsg.Text = "There are No Subject are Found!";
                lblerrormsg.Visible = true;
                return;
            }

            if (Txt_Test.Text == "--Select--")
            {
                if (Cbl_test.Items.Count > 0)
                {
                    lblerrormsg.Text = "You Must Select The Test!";
                    lblerrormsg.Visible = true;
                    return;
                }
                else
                {
                    lblerrormsg.Text = "There are No Tests are Found!";
                    lblerrormsg.Visible = true;
                    return;
                }
            }
            else
            {
                if (Cbl_test.Items.Count > 0)
                {
                    for (int i = 0; i < Cbl_test.Items.Count; i++)
                    {
                        if (Cbl_test.Items[i].Selected == true)
                        {
                            testcount++;   // Count the selected tests
                            if (string.IsNullOrEmpty(testname.Trim()) && string.IsNullOrEmpty(testno.Trim()) && string.IsNullOrEmpty(criteriano.Trim()))
                            {
                                testname = Convert.ToString(Cbl_test.Items[i].Text).Trim();
                                testno = Convert.ToString(Cbl_test.Items[i].Value).Trim();
                                criteriano = Convert.ToString(Cbl_test.Items[i].Value).Trim();
                            }
                            else
                            {
                                testname += "," + Convert.ToString(Cbl_test.Items[i].Text).Trim();
                                testno += "," + Convert.ToString(Cbl_test.Items[i].Value).Trim();
                                criteriano += "','" + Convert.ToString(Cbl_test.Items[i].Value).Trim();
                            }
                        }
                    }
                }
                else
                {
                    lblerrormsg.Text = "There are No Tests are Found!";
                    lblerrormsg.Visible = true;
                    return;
                }
                if (testcount == 0)
                {
                    lblerrormsg.Text = "You Must Select The Test!";
                    lblerrormsg.Visible = true;
                    return;
                }
                if (testcount % 2 != 0)
                {
                    //Test Found! But Selected Test Count Must Be Even!
                    FpSubTest.Sheets[0].ColumnCount = 3;
                    lblerrormsg.Text = "You Must Select Even Number Of Tests(i.e 2 or 4,etc)!";
                    lblerrormsg.Visible = true;
                    lblerrormsg.Width = 550;
                    return;
                }
                else
                {
                    int cnt = 0;
                    if (Cbl_test.Items.Count > 0)
                    {

                        //Table  For Performance Chart 

                        dtPerform.Columns.Clear();
                        dtPerform.Rows.Clear();
                        dtPerform.Columns.Add(" ");
                        PerformanceChart.Series.Clear();

                        //Table For Average Chart & Add Columns To DataTable For Average

                        dtAvg.Columns.Clear();
                        dtAvg.Rows.Clear();
                        dtAvg.Columns.Add(" ");
                        chartAvg.Series.Clear();

                        dtAvg.Columns.Add("Below 50");
                        dtAvg.Columns.Add("50 - 59");
                        dtAvg.Columns.Add("60 - 74");
                        dtAvg.Columns.Add("75 - 89");
                        dtAvg.Columns.Add("90+");

                        for (int i = 0; i < Cbl_test.Items.Count; i++)
                        {
                            if (Cbl_test.Items[i].Selected == true)
                            {
                                FpSubTest.Sheets[0].ColumnCount++;
                                FpSubTest.Sheets[0].ColumnHeader.Cells[0, FpSubTest.Sheets[0].ColumnCount - 1].Text = Convert.ToString(Cbl_test.Items[i].Text).Trim();
                                dtStudTest.Columns.Add(Convert.ToString(Cbl_test.Items[i].Text).Trim());
                                FpSubTest.Sheets[0].Columns[FpSubTest.Sheets[0].ColumnCount - 1].Width = 150;
                                FpSubTest.Sheets[0].ColumnHeader.Cells[0, FpSubTest.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(Cbl_test.Items[i].Value).Trim();
                                FpSubTest.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSubTest.Sheets[0].ColumnCount - 1, 2, 1);

                                if ((cnt + 1) % 2 == 0 && cnt != 0)
                                {
                                    FpSubTest.Sheets[0].ColumnCount += 2;
                                    FpSubTest.Sheets[0].ColumnHeader.Cells[0, FpSubTest.Sheets[0].ColumnCount - 2].Text = FpSubTest.Sheets[0].ColumnHeader.Cells[0, FpSubTest.Sheets[0].ColumnCount - 3].Text.Trim() + " - " + FpSubTest.Sheets[0].ColumnHeader.Cells[0, FpSubTest.Sheets[0].ColumnCount - 4].Text.Trim();
                                    dtStudTest.Columns.Add(Convert.ToString(FpSubTest.Sheets[0].ColumnHeader.Cells[0, FpSubTest.Sheets[0].ColumnCount - 2].Text).Trim());
                                    FpSubTest.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSubTest.Sheets[0].ColumnCount - 2, 2, 1);
                                    FpSubTest.Sheets[0].Columns[FpSubTest.Sheets[0].ColumnCount - 2].Width = 150;

                                    dcPerf = new DataColumn();
                                    dcPerf.ColumnName = Convert.ToString(FpSubTest.Sheets[0].ColumnHeader.Cells[0, FpSubTest.Sheets[0].ColumnCount - 2].Text).Trim();
                                    PerformanceChart.Series.Add(Convert.ToString(dcPerf.ColumnName).Trim());
                                    dtPerform.Columns.Add(dcPerf);

                                    string name = FpSubTest.Sheets[0].ColumnHeader.Cells[0, FpSubTest.Sheets[0].ColumnCount - 4].Text.Trim() + " & " + FpSubTest.Sheets[0].ColumnHeader.Cells[0, FpSubTest.Sheets[0].ColumnCount - 3].Text.Trim();
                                    FpSubTest.Sheets[0].ColumnHeader.Cells[0, FpSubTest.Sheets[0].ColumnCount - 1].Text = "( " + name + ") AVG";
                                    FpSubTest.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSubTest.Sheets[0].ColumnCount - 1, 2, 1);
                                    FpSubTest.Sheets[0].Columns[FpSubTest.Sheets[0].ColumnCount - 1].Width = 150;
                                    dtStudTest.Columns.Add(Convert.ToString(FpSubTest.Sheets[0].ColumnHeader.Cells[0, FpSubTest.Sheets[0].ColumnCount - 1].Text).Trim());

                                    chartAvg.Series.Add(name);
                                    drAvg = dtAvg.NewRow();
                                    drAvg[0] = name;
                                    dtAvg.Rows.Add(drAvg);
                                }
                                cnt++;
                            }
                        }
                    }

                    //Performance Counts

                    lpcount = new int[testcount / 2];
                    spcount = new int[testcount / 2];
                    impcount = new int[testcount / 2];

                    // For Avearage Count

                    belowcount50 = new int[testcount / 2];
                    bw50_59 = new int[testcount / 2];
                    bw60_74 = new int[testcount / 2];
                    bw75_89 = new int[testcount / 2];
                    gt90 = new int[testcount / 2];

                    //Loop for inialize the counts to zero

                    for (int i = 0; i < testcount / 2; i++)
                    {
                        lpcount[i] = 0;
                        spcount[i] = 0;
                        impcount[i] = 0;

                        belowcount50[i] = 0;
                        bw50_59[i] = 0;
                        bw60_74[i] = 0;
                        bw75_89[i] = 0;
                        gt90[i] = 0;

                    }
                }
            }

            string q = string.Empty;

            DataSet dsStud = new DataSet();

            strorderby = d2.GetFunction("select LinkValue from inssettings where college_code=" + collegecode.ToString() + " and linkname='Student Attendance'");

            if (strorderby == "1")
            {
                serialflag = true;
            }
            else
            {
                serialflag = false;
            }

            strorderby = d2.GetFunction("select value from Master_Settings where settings='order_by'");
            if (strorderby == "")
            {
                strorderby = "";
            }
            else
            {
                if (strorderby == "0")
                {
                    strorderby = "ORDER BY Roll_No";
                }
                else if (strorderby == "1")
                {
                    strorderby = "ORDER BY Reg_No";
                }
                else if (strorderby == "2")
                {
                    strorderby = "ORDER BY Stud_Name";
                }
                else if (strorderby == "0,1,2")
                {
                    strorderby = "ORDER BY Roll_No,Reg_No,Stud_Name";
                }
                else if (strorderby == "0,1")
                {
                    strorderby = "ORDER BY Roll_No,Reg_No";
                }
                else if (strorderby == "1,2")
                {
                    strorderby = "ORDER BY Reg_No,Stud_Name";
                }
                else if (strorderby == "0,2")
                {
                    strorderby = "ORDER BY Roll_No,Stud_Name";
                }
            }

            if (serialflag == false)
            {
                if (section != "")
                {
                    q = "select Roll_No,Reg_No,Stud_Name,Stud_Type,Current_Semester,Current_Year from Registration where college_code='" + collegecode + "' and Batch_Year='" + batchyear + "' and degree_code='" + degreecode + "'  and Sections in('" + section + "')  and DelFlag=0 and CC=0 and Exam_Flag<>'debar'" + strorderby;//and Current_Semester='" + semester + "'
                }
                else
                {
                    q = "select Roll_No,Reg_No,Stud_Name,Stud_Type,Current_Semester,Current_Year from Registration where college_code='" + collegecode + "' and Batch_Year='" + batchyear + "' and degree_code='" + degreecode + "' and DelFlag=0 and CC=0 and Exam_Flag<>'debar'" + strorderby;// and Current_Semester='" + semester + "' 
                }
            }
            else
            {
                if (section != "")
                {
                    q = "select Roll_No,Reg_No,Stud_Name,Stud_Type,Current_Semester,Current_Year from Registration where college_code='" + collegecode + "' and Batch_Year='" + batchyear + "' and degree_code='" + degreecode + "' and Sections in('" + section + "')  and DelFlag=0 and CC=0 and Exam_Flag<>'debar' ORDER BY serialno";//and Current_Semester='" + semester + "' 
                }
                else
                {
                    q = "select Roll_No,Reg_No,Stud_Name,Stud_Type,Current_Semester,Current_Year from Registration where college_code='" + collegecode + "' and Batch_Year='" + batchyear + "' and degree_code='" + degreecode + "'  and DelFlag=0 and CC=0 and Exam_Flag<>'debar' ORDER BY serialno";//and Current_Semester='" + semester + "' 
                }
            }
            if (section != "")
            {
                q += "; select r.serialno,r.Reg_No,r.Roll_No,r.Stud_Name,R.Current_Semester,re.marks_obtained,c.Criteria_no,c.criteria,c.max_mark,c.min_mark,r.Sections from Registration r,Result re,CriteriaForInternal c,Exam_type e,subject s where s.subject_no=e.subject_no and r.Roll_No=re.roll_no and e.criteria_no=c.Criteria_no and r.Batch_Year=e.batch_year and re.exam_code=e.exam_code  and r.degree_code='" + degreecode + "' and r.college_code='" + collegecode + "' and r.Batch_Year='" + batchyear + "' and c.Criteria_no in ('" + criteriano + "') and s.subject_Code='" + subject_no + "'  and DelFlag=0 and CC=0 and Exam_Flag<>'debar' and r.Sections in('" + section + "')";//and Current_Semester='" + semester + "'
            }
            else
            {
                q += "; select r.serialno,r.Reg_No,r.Roll_No,r.Stud_Name,R.Current_Semester,re.marks_obtained,c.Criteria_no,c.criteria,c.max_mark,c.min_mark,r.Sections from Registration r,Result re,CriteriaForInternal c,Exam_type e,subject s where s.subject_no=e.subject_no and r.Roll_No=re.roll_no and e.criteria_no=c.Criteria_no and r.Batch_Year=e.batch_year and re.exam_code=e.exam_code  and r.degree_code='" + degreecode + "' and r.college_code='" + collegecode + "' and r.Batch_Year='" + batchyear + "' and c.Criteria_no in ('" + criteriano + "') and s.subject_Code='" + subject_no + "' and DelFlag=0 and CC=0 and Exam_Flag<>'debar' ";// and Current_Semester='" + semester + "'
            }
            dsStud.Clear();
            dsStud = d2.select_method_wo_parameter(q, "Text");

            if (dsStud.Tables.Count > 0 && dsStud.Tables[0].Rows.Count > 0)
            {
                FpSubTest.Visible = true;
                int cnt = 3;
                string mark = string.Empty;

                for (int i = 0; i < dsStud.Tables[0].Rows.Count; i++)
                {
                    FpSubTest.Sheets[0].RowCount++;
                    FpSubTest.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1).Trim();
                    FpSubTest.Sheets[0].Cells[i, 0].Font.Name = "Book Antiqua";
                    FpSubTest.Sheets[0].Cells[i, 0].Font.Size = FontUnit.Medium;
                    FpSubTest.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSubTest.Sheets[0].Cells[i, 0].VerticalAlign = VerticalAlign.Middle;
                    drStud = dtStudTest.NewRow();
                    drStud[0] = Convert.ToString(i + 1).Trim();

                    FpSubTest.Sheets[0].Cells[i, 1].Text = Convert.ToString(dsStud.Tables[0].Rows[i]["Roll_No"]).Trim();
                    FpSubTest.Sheets[0].Cells[i, 1].Font.Name = "Book Antiqua";
                    FpSubTest.Sheets[0].Cells[i, 1].CellType = new FarPoint.Web.Spread.TextCellType();
                    FpSubTest.Sheets[0].Cells[i, 1].Font.Size = FontUnit.Medium;
                    FpSubTest.Sheets[0].Cells[i, 1].HorizontalAlign = HorizontalAlign.Left;
                    FpSubTest.Sheets[0].Cells[i, 1].VerticalAlign = VerticalAlign.Middle;
                    drStud[1] = Convert.ToString(dsStud.Tables[0].Rows[i]["Roll_No"]).Trim();

                    FpSubTest.Sheets[0].Cells[i, 2].Text = Convert.ToString(dsStud.Tables[0].Rows[i]["Stud_Name"]).Trim();
                    FpSubTest.Sheets[0].Cells[i, 2].Font.Name = "Book Antiqua";
                    FpSubTest.Sheets[0].Cells[i, 2].Font.Size = FontUnit.Medium;
                    FpSubTest.Sheets[0].Cells[i, 2].HorizontalAlign = HorizontalAlign.Left;
                    FpSubTest.Sheets[0].Cells[i, 2].VerticalAlign = VerticalAlign.Middle;
                    drStud[2] = Convert.ToString(dsStud.Tables[0].Rows[i]["Stud_Name"]).Trim();

                    if (dsStud.Tables[1].Rows.Count > 0)
                    {
                        string[] test = testno.Trim().Split(',');
                        cnt = 3;
                        int pcnt = 0;

                        string maxmark = string.Empty;
                        string minmark = string.Empty;
                        string maxmark1 = string.Empty;
                        string minmark1 = string.Empty;

                        if (test.Length > 0)
                        {
                            for (int tst = 0; tst < test.Length; tst++)
                            {
                                Double avg = 0.0;
                                DataView dvtestMark = new DataView();
                                dsStud.Tables[1].DefaultView.RowFilter = "Roll_No='" + Convert.ToString(dsStud.Tables[0].Rows[i]["Roll_No"]).Trim() + "' and Criteria_no='" + Convert.ToString(test[tst]).Trim() + "'";
                                dvtestMark = dsStud.Tables[1].DefaultView;

                                if (dvtestMark.Count > 0)
                                {
                                    mark = Convert.ToString(dvtestMark[0]["marks_obtained"]).Trim();
                                    maxmark = Convert.ToString(dvtestMark[0]["max_mark"]).Trim();
                                    minmark = Convert.ToString(dvtestMark[0]["min_mark"]).Trim();

                                    string dummymark = mark;
                                    double dummymrk = 0;
                                    double.TryParse(dummymark.Trim(), out dummymrk);
                                    ConvertedMark("100", maxmark, ref mark, ref minmark);

                                    double mrk = 0;
                                    double.TryParse(mark.Trim(), out mrk);

                                    if (dummymrk < 0)
                                    {
                                        FpSubTest.Sheets[0].Cells[i, cnt].Text = findresult(Convert.ToString(dummymrk).Trim());
                                        drStud[cnt] = findresult(Convert.ToString(dummymrk).Trim());
                                    }
                                    else
                                    {
                                        FpSubTest.Sheets[0].Cells[i, cnt].Text = findresult(Convert.ToString(Math.Round(mrk, 1, MidpointRounding.AwayFromZero)).Trim());
                                        drStud[cnt] = findresult(Convert.ToString(Math.Round(mrk, 1, MidpointRounding.AwayFromZero)).Trim());
                                    }

                                    FpSubTest.Sheets[0].Cells[i, cnt].Font.Name = "Book Antiqua";
                                    FpSubTest.Sheets[0].Cells[i, cnt].Font.Size = FontUnit.Medium;
                                    FpSubTest.Sheets[0].Cells[i, cnt].HorizontalAlign = HorizontalAlign.Center;
                                    FpSubTest.Sheets[0].Cells[i, cnt].VerticalAlign = VerticalAlign.Middle;

                                    if (tst % 2 == 0)
                                    {
                                        maxmark = Convert.ToString(dvtestMark[0]["max_mark"]).Trim();
                                        minmark = Convert.ToString(dvtestMark[0]["min_mark"]).Trim();
                                    }
                                    else
                                    {
                                        maxmark1 = Convert.ToString(dvtestMark[0]["max_mark"]).Trim();
                                        minmark1 = Convert.ToString(dvtestMark[0]["min_mark"]).Trim();
                                    }

                                    //FpSubTest.Sheets[0].Cells[i, cnt].Text = dvtestMark[0]["marks_obtained"].ToString();
                                    cnt++;

                                    if ((tst + 1) % 2 == 0 && tst != 0)
                                    {
                                        drAvg = dtAvg.NewRow();
                                        string mark1 = Convert.ToString(FpSubTest.Sheets[0].Cells[i, cnt - 2].Text).Trim();
                                        string mark2 = Convert.ToString(FpSubTest.Sheets[0].Cells[i, cnt - 1].Text).Trim();
                                        double m1 = 0, m2 = 0;

                                        //ConvertedMark("100", maxmark, ref mark1, ref minmark);
                                        //ConvertedMark("100", maxmark1, ref mark2, ref minmark1);

                                        double.TryParse(mark1.Trim(), out m1);
                                        double.TryParse(mark2.Trim(), out m2);

                                        //Count BELOW 49
                                        //if (m1 <=49 || m2 <=49)
                                        //{
                                        //    belowcount50[pcnt]++;
                                        //}

                                        if (m1 < 50)
                                        {
                                            belowcount50[pcnt]++;
                                        }
                                        if (m2 < 50)
                                        {
                                            belowcount50[pcnt]++;
                                        }

                                        //Count BETWEEN 50 AND 64
                                        //if ((m1 >= 50 && m1 <= 64) || (m2 >= 50 && m2 <= 64))
                                        //{
                                        //    bw50_59[pcnt]++;
                                        //}

                                        if ((m1 >= 50 && m1 <= 59))
                                        {
                                            bw50_59[pcnt]++;
                                        }

                                        if ((m2 >= 50 && m2 <= 59))
                                        {
                                            bw50_59[pcnt]++;
                                        }

                                        //Count BETWEEN 65 AND 74
                                        //if ((m1 >= 65 && m1 <=74) || (m2 >= 65 && m2 <=74))
                                        //{
                                        //    bw60_74[pcnt]++;
                                        //}

                                        if ((m1 >= 60 && m1 <= 74))
                                        {
                                            bw60_74[pcnt]++;
                                        }
                                        if ((m2 >= 60 && m2 <= 74))
                                        {
                                            bw60_74[pcnt]++;
                                        }

                                        //Count BETWEEN 75 AND 89

                                        //if ((m1 >= 75 && m1 <= 89) || (m2 >= 75 && m2 <=89))
                                        //{
                                        //    bw75_89[pcnt]++;
                                        //}

                                        if ((m1 >= 75 && m1 <= 89))
                                        {
                                            bw75_89[pcnt]++;
                                        }

                                        if ((m2 >= 75 && m2 <= 89))
                                        {
                                            bw75_89[pcnt]++;
                                        }

                                        //Count 90+
                                        //if (m1 >= 90 || m2 >= 90)
                                        //{
                                        //    gt90[pcnt]++;
                                        //}

                                        if (m1 >= 90)
                                        {
                                            gt90[pcnt]++;
                                        }

                                        if (m2 >= 90)
                                        {
                                            gt90[pcnt]++;
                                        }

                                        double sum = m2 + m1;
                                        sum = Math.Round(sum, 2, MidpointRounding.AwayFromZero);

                                        avg = sum / 2;
                                        avg = Math.Round(avg, 2, MidpointRounding.AwayFromZero);

                                        double diff = m2 - m1;
                                        diff = Math.Round(diff, 2, MidpointRounding.AwayFromZero);

                                        if (diff == 0)
                                        {
                                            spcount[pcnt]++;
                                        }
                                        else if (diff < 0)
                                        {
                                            lpcount[pcnt]++;
                                        }
                                        else if (diff > 0)
                                        {
                                            impcount[pcnt]++;
                                        }

                                        //drAvg[pcnt - 1] = FpSubTest.Sheets[0].ColumnHeader.Cells[i, cnt].Text.ToString();
                                        FpSubTest.Sheets[0].Cells[i, cnt].Text = Convert.ToString(diff).Trim();
                                        FpSubTest.Sheets[0].Cells[i, cnt].Font.Name = "Book Antiqua";
                                        FpSubTest.Sheets[0].Cells[i, cnt].Font.Size = FontUnit.Medium;
                                        FpSubTest.Sheets[0].Cells[i, cnt].HorizontalAlign = HorizontalAlign.Center;
                                        FpSubTest.Sheets[0].Cells[i, cnt].VerticalAlign = VerticalAlign.Middle;
                                        drStud[cnt] = Convert.ToString(diff);
                                        FpSubTest.Sheets[0].Cells[i, cnt + 1].Text = Convert.ToString(Math.Round(avg, 2, MidpointRounding.AwayFromZero)).Trim();
                                        FpSubTest.Sheets[0].Cells[i, cnt + 1].Font.Name = "Book Antiqua";
                                        FpSubTest.Sheets[0].Cells[i, cnt + 1].Font.Size = FontUnit.Medium;
                                        FpSubTest.Sheets[0].Cells[i, cnt + 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSubTest.Sheets[0].Cells[i, cnt + 1].VerticalAlign = VerticalAlign.Middle;
                                        drStud[cnt + 1] = Convert.ToString(Math.Round(avg, 2)).Trim();
                                        //FpSubTest.Sheets[0].Cells[i, cnt].Text = Convert.ToString(Convert.ToInt16(FpSubTest.Sheets[0].Cells[i, cnt - 1].Text.ToString()) - Convert.ToInt16(FpSubTest.Sheets[0].Cells[i, cnt - 2].Text.ToString()));
                                        //FpSubTest.Sheets[0].Cells[i, cnt + 1].Text = Convert.ToString((Convert.ToInt16(FpSubTest.Sheets[0].Cells[i, cnt - 1].Text.ToString()) + Convert.ToInt16(FpSubTest.Sheets[0].Cells[i, cnt - 2].Text.ToString())) / 2);
                                        cnt += 2;
                                        pcnt++;

                                    }
                                }
                                else
                                {
                                    cnt++;
                                    if ((tst + 1) % 2 == 0 && tst != 0)
                                    {
                                        cnt += 2;
                                        pcnt++;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        pnlContents.Visible = false;
                        gvAvgcount.Visible = false;
                        gvStudTest.Visible = false;
                        gvTestPerfm.Visible = false;
                        chartAvg.Visible = false;
                        PerformanceChart.Visible = false;
                        FpSubTest.Visible = false;
                        rptprint1.Visible = false;
                        lblerrormsg.Text = "Marks Not Fount!!!";
                        lblerrormsg.Visible = true;
                        return;
                    }
                    dtStudTest.Rows.Add(drStud);
                }
                if (dtPerform.Columns.Count > 0)
                {
                    string[] Perform = new string[3] { "LP", "SP", "IMP" };

                    for (int r = 0; r < Perform.Length; r++)
                    {
                        drPerf[r] = dtPerform.NewRow();
                        drPerf[r][0] = Convert.ToString(Perform[r]).Trim();
                        dtPerform.Rows.Add(drPerf[r]);
                    }
                    for (int c = 0; c < lpcount.Length; c++)
                    {
                        dtPerform.Rows[0][c + 1] = Convert.ToString(lpcount[c]).Trim();
                        dtPerform.Rows[1][c + 1] = Convert.ToString(spcount[c]).Trim();
                        dtPerform.Rows[2][c + 1] = Convert.ToString(impcount[c]).Trim();
                    }

                    //dtPerform.Rows.Add(drPerf[0]);
                    //dtPerform.Rows.Add(drPerf[1]);
                    //dtPerform.Rows.Add(drPerf[2]);
                    //for (int r = 0; r < Perform.Length; r++)
                    //{
                    //    drPerf = dtPerform.NewRow();
                    //    drPerf[0] = Perform[r];
                    //    if (r == 0)
                    //    {
                    //        for (int c = 0; c < lpcount.Length; c++)
                    //            drPerf[c+1] = lpcount[c].ToString();
                    //    }
                    //    else if (r == 1)
                    //    {
                    //        for (int c = 0; c < spcount.Length; c++)
                    //            drPerf[c + 1] = spcount[c].ToString();
                    //    }
                    //    else if (r == 2)
                    //    {
                    //        for (int c = 0; c < impcount.Length; c++)
                    //            drPerf[c + 1] = impcount[c].ToString();
                    //    }
                    //    dtPerform.Rows.Add(drPerf);
                    //}

                    if (dtPerform.Rows.Count > 0)
                    {
                        gvTestPerfm.DataSource = dtPerform;
                        gvTestPerfm.DataBind();
                        gvTestPerfm.Visible = true;

                        for (int r = 0; r < dtPerform.Rows.Count; r++)
                        {
                            for (int c = 1; c < dtPerform.Columns.Count; c++)
                            {
                                PerformanceChart.Series[c - 1].Points.AddXY(Convert.ToString(dtPerform.Rows[r][0]).Trim(), Convert.ToString(dtPerform.Rows[r][c]).Trim());
                                PerformanceChart.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                                PerformanceChart.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;

                                PerformanceChart.Series[c - 1].IsValueShownAsLabel = true;
                                PerformanceChart.Series[c - 1].IsXValueIndexed = true;

                                PerformanceChart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                                PerformanceChart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;

                                PerformanceChart.ChartAreas[0].AxisY.Interval = 10;
                                PerformanceChart.ChartAreas[0].AxisY.Minimum = 0;
                                PerformanceChart.ChartAreas[0].AxisY.Maximum = 100;
                                //if (r != 0)
                                gvTestPerfm.Rows[r].Cells[c].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                        PerformanceChart.Visible = true;
                        //string imgPath = HttpContext.Current.Request.PhysicalApplicationPath + imgperfm;
                        //PerformanceChart.SaveImage(imgPath);
                    }
                }

                if (dtAvg.Rows.Count > 0)
                {
                    int val = 0;
                    for (int r = 0; r < dtAvg.Rows.Count; r++)
                    {
                        dtAvg.Rows[r][1] = Convert.ToString(belowcount50[val]).Trim();
                        dtAvg.Rows[r][2] = Convert.ToString(bw50_59[val]).Trim();
                        dtAvg.Rows[r][3] = Convert.ToString(bw60_74[val]).Trim();
                        dtAvg.Rows[r][4] = Convert.ToString(bw75_89[val]).Trim();
                        dtAvg.Rows[r][5] = Convert.ToString(gt90[val]).Trim();
                        val++;
                    }
                    if (dtAvg.Rows.Count > 0)
                    {
                        gvAvgcount.DataSource = dtAvg;
                        gvAvgcount.DataBind();
                        gvAvgcount.Visible = true;
                        for (int r = 0; r < dtAvg.Rows.Count; r++)
                        {
                            for (int c = 1; c < dtAvg.Columns.Count; c++)
                            {
                                chartAvg.Series[r].Points.AddXY(Convert.ToString(dtAvg.Columns[c]).Trim(), Convert.ToString(dtAvg.Rows[r][c]).Trim());
                                chartAvg.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                                chartAvg.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;

                                chartAvg.Series[r].IsValueShownAsLabel = true;
                                chartAvg.Series[r].IsXValueIndexed = true;

                                chartAvg.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                                chartAvg.ChartAreas[0].AxisX.LabelStyle.Interval = 1;

                                chartAvg.ChartAreas[0].AxisY.Interval = 20;
                                chartAvg.ChartAreas[0].AxisY.Minimum = 0;
                                chartAvg.ChartAreas[0].AxisY.Maximum = 100;
                                //if (c != 0)
                                gvAvgcount.Rows[r].Cells[c].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                        chartAvg.Visible = true;
                        //string imgPath = HttpContext.Current.Request.PhysicalApplicationPath + imgavg;
                        //chartAvg.SaveImage(imgPath);
                    }
                    if (dtStudTest.Rows.Count > 0)
                    {
                        gvStudTest.DataSource = dtStudTest;
                        gvStudTest.DataBind();
                        gvStudTest.HeaderRow.HorizontalAlign = HorizontalAlign.Center;
                        for (int i = 0; i < gvStudTest.HeaderRow.Cells.Count; i++)
                        {
                            if (i == 0)
                                gvStudTest.HeaderRow.Cells[i].Width = 50;
                            else if (i == 1)
                                gvStudTest.HeaderRow.Cells[i].Width = 100;
                            else if (i == 2)
                                gvStudTest.HeaderRow.Cells[i].Width = 250;
                            else
                                gvStudTest.HeaderRow.Cells[i].Width = 100;
                        }
                        //gvStudTest.Visible = true;
                        for (int r = 0; r < dtStudTest.Rows.Count; r++)
                        {
                            for (int c = 0; c < dtStudTest.Columns.Count; c++)
                            {
                                if (r == 0)
                                {
                                    //gvStudTest.Columns[c].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                                    gvStudTest.HeaderRow.HorizontalAlign = HorizontalAlign.Center;
                                }
                                if (c != 2 && c != 1)
                                    gvStudTest.Rows[r].Cells[c].HorizontalAlign = HorizontalAlign.Center;
                                else
                                    gvStudTest.Rows[r].Cells[c].HorizontalAlign = HorizontalAlign.Left;
                            }
                        }
                    }
                }
                FpSubTest.Sheets[0].PageSize = FpSubTest.Sheets[0].RowCount;
                pnlContents.Visible = true;
                rptprint1.Visible = true;
                FpSubTest.Visible = true;
                FpSubTest.SaveChanges();
            }
            else
            {
                rptprint1.Visible = false;
                lblerrormsg.Text = "No Records Found";
                lblerrormsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.StackTrace;
            lblerrormsg.Visible = true;
        }
    }

    public string findresult(string att)
    {
        string atten = att.Trim();
        switch (atten)
        {
            case "-1":
                atten = "AAA";
                break;
            case "-2":
                atten = "EL";
                break;
            case "-3":
                atten = "EOD";
                break;
            case "-4":
                atten = "ML";
                break;
            case "-5":
                atten = "SOD";
                break;
            case "-6":
                atten = "NSS";
                break;
            case "-7":
                atten = "NJ";
                break;
            case "-8":
                atten = "S";
                break;
            case "-9":
                atten = "L";
                break;
            case "-10":
                atten = "NCC";
                break;
            case "-11":
                atten = "HS";
                break;
            case "-12":
                atten = "PP";
                break;
            case "-13":
                atten = "SYOD";
                break;
            case "-14":
                atten = "COD";
                break;
            case "-15":
                atten = "OOD";
                break;
            case "-16":
                atten = "OD";
                break;
            case "-17":
                atten = "LA";
                break;
            //Added By subburaj 21.08.2014****//
            case "-18":
                atten = "RAA";
                break;
            //********End**********************//
        }
        return atten;
    }

    public string ConvertedMark(string txtConvertTo, string maxMark, ref string obtainedMark, ref string minMark)
    {
        int Mark, max;
        bool r = int.TryParse(obtainedMark.Trim(), out Mark);
        bool maxflag = int.TryParse(txtConvertTo.Trim(), out max);
        double multiply;
        if (maxflag)
        {
            if (r)
            {
                if (Mark > 0)
                {
                    switch (txtConvertTo.Trim())
                    {
                        default:
                            multiply = double.Parse(txtConvertTo) / int.Parse(maxMark);
                            obtainedMark = Convert.ToString(Mark * multiply).Trim();
                            break;
                    }
                }
            }
            minMark = Convert.ToString(max / 2).Trim();
            maxMark = txtConvertTo.Trim();
        }
        return obtainedMark;
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname1.Text.Trim().Replace(" ", "_").Trim();
            if (reportname.ToString().Trim() != "")
            {
                lbl_norec1.Text = string.Empty;
                lbl_norec1.Visible = false;
                string subjectName = string.Empty;
                string sections = string.Empty;
                string branch = string.Empty;
                string current_sem = string.Empty;

                if (ddl_collage.Items.Count > 0)
                {
                    collegecode = Convert.ToString(ddl_collage.SelectedItem.Value).Trim();
                }

                if (ddl_Batch.Items.Count > 0)
                {
                    batchyear = Convert.ToString(ddl_Batch.SelectedItem.Text).Trim();
                }

                if (ddl_Branch.Items.Count > 0)
                {
                    degreecode = Convert.ToString(ddl_Branch.SelectedItem.Value).Trim();
                    branch = Convert.ToString(ddl_Branch.SelectedItem.Text).Trim();
                }

                if (ddl_subject.Items.Count > 0)
                {
                    subjectName = Convert.ToString(ddl_subject.SelectedItem.Text).Trim();
                }

                if (ddl_Sec.Items.Count > 0)
                {
                    sections = Convert.ToString(ddl_Sec.SelectedValue).Trim();
                    if (Convert.ToString(ddl_Sec.SelectedItem.Text).Trim().ToLower() == "all" || Convert.ToString(ddl_Sec.SelectedItem.Text).Trim().ToLower() == "" || Convert.ToString(ddl_Sec.SelectedItem.Text).Trim().ToLower() == "-1")
                    {
                        sections = string.Empty;
                    }
                    else
                    {
                        section = "&nbsp;-&nbsp;" + Convert.ToString(ddl_Sec.SelectedValue).ToUpper().Trim();
                    }
                }

                foreach (System.Web.UI.WebControls.ListItem liSem in cbl_sem.Items)
                {
                    if (liSem.Selected)
                    {
                        if (string.IsNullOrEmpty(current_sem))
                        {
                            current_sem = liSem.Text;
                        }
                        else
                        {
                            current_sem += " & " + liSem.Text;
                        }
                    }
                }

                string degreedetails = "";
                reportname = reportname.Trim() + "SubjectW_Wise_Test_Analysis_Report";

                degreedetails = branch.ToUpper() + "&nbsp;" + section + "&nbsp;(BATCH&nbsp;" + Convert.ToString(batchyear).Trim() + ")&nbsp;SEM&nbsp;-&nbsp;" + Convert.ToString(current_sem).Trim();

                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;filename=" + reportname.Replace(" ", "_").Trim() + ".xls");
                Response.ContentType = "application/excel";

                StringWriter sw = new StringWriter(); ;
                HtmlTextWriter htm = new HtmlTextWriter(sw);

                DataSet dscol = d2.select_method_wo_parameter("Select collname,address1,address2,address3,category,university from Collinfo where college_code='" + collegecode + "' ", "Text");

                Label lb = new Label();
                htm.InnerWriter.WriteLine("<center>");
                if (dscol.Tables[0].Rows.Count > 0)
                {
                    lb.Text = Convert.ToString(dscol.Tables[0].Rows[0]["collname"]).Trim() + "<br> ";
                    lb.Style.Add("height", "100px");
                    lb.Style.Add("text-decoration", "none");
                    lb.Style.Add("font-family", "Book Antiqua;");
                    lb.Style.Add("font-size", "18px");
                    lb.Style.Add("font-weight", "bold");
                    lb.Style.Add("text-align", "center");
                    lb.RenderControl(htm);

                    lb.Text = Convert.ToString(dscol.Tables[0].Rows[0]["collname"]).Trim() + "<br> ";
                    string address = "";
                    if (Convert.ToString(dscol.Tables[0].Rows[0]["address1"]).Trim() != "")
                    {
                        address = Convert.ToString(dscol.Tables[0].Rows[0]["address1"]).Trim();
                    }
                    if (Convert.ToString(dscol.Tables[0].Rows[0]["address2"]).Trim() != "")
                    {
                        if (address.Trim() == "")
                        {
                            address = Convert.ToString(dscol.Tables[0].Rows[0]["address2"]).Trim();
                        }
                        else
                        {
                            address = address + ", " + Convert.ToString(dscol.Tables[0].Rows[0]["address2"]).Trim();
                        }
                    }
                    if (Convert.ToString(dscol.Tables[0].Rows[0]["address3"]).Trim() != "")
                    {
                        if (address == "")
                        {
                            address = Convert.ToString(dscol.Tables[0].Rows[0]["address3"]).Trim();
                        }
                        else
                        {
                            address = address + ", " + Convert.ToString(dscol.Tables[0].Rows[0]["address3"]).Trim();
                        }
                    }
                    if (address.Trim() != "")
                    {
                        lb.Text = address + "<br> ";
                        lb.Style.Add("height", "100px");
                        lb.Style.Add("text-decoration", "none");
                        lb.Style.Add("font-family", "Book Antiqua;");
                        lb.Style.Add("font-size", "12px");
                        lb.Style.Add("text-align", "center");
                        lb.RenderControl(htm);
                    }
                }

                Label lb2 = new Label();
                lb2.Text = degreedetails;
                lb2.Style.Add("height", "100px");
                lb2.Style.Add("text-decoration", "none");
                lb2.Style.Add("font-family", "Book Antiqua;");
                lb2.Style.Add("font-size", "10px");
                lb2.Style.Add("font-weight", "bold");
                lb2.Style.Add("text-align", "center");
                lb2.RenderControl(htm);

                Label lb3 = new Label();
                lb3.Text = "<br>";
                lb3.Style.Add("height", "200px");
                lb3.Style.Add("text-decoration", "none");
                lb3.Style.Add("font-family", "Book Antiqua;");
                lb3.Style.Add("font-size", "10px");
                lb3.Style.Add("text-align", "left");
                lb3.RenderControl(htm);

                Label lb4 = new Label();
                lb4.Text = "Subject Wise Test Analysis Report<br><br>";
                lb4.Style.Add("height", "200px");
                lb4.Style.Add("font-weight", "bold");
                lb4.Style.Add("text-decoration", "none");
                lb4.Style.Add("font-family", "Book Antiqua;");
                lb4.Style.Add("font-size", "10px");
                lb4.Style.Add("text-align", "center");
                lb4.RenderControl(htm);

                htm.InnerWriter.WriteLine("</center>");

                lb4.Text = "Subject Name : " + subjectName.Trim() + " <br><br/>";
                lb4.Style.Add("height", "200px");
                lb4.Style.Add("text-decoration", "none");
                lb4.Style.Add("font-family", "Book Antiqua;");
                lb4.Style.Add("font-size", "10px");
                lb4.Style.Add("font-weight", "bold");
                lb4.Style.Add("text-align", "left");
                lb4.RenderControl(htm);

                //lb4.Text = "<br><br>";
                //lb4.Style.Add("height", "200px");
                //lb4.Style.Add("text-decoration", "none");
                //lb4.Style.Add("font-family", "Book Antiqua;");
                //lb4.Style.Add("font-size", "10px");
                //lb4.Style.Add("text-align", "center");
                //lb4.RenderControl(htm);

                gvStudTest.Visible = true;
                gvStudTest.RenderControl(htm);
                gvStudTest.Visible = false;

                //htm.InnerWriter.WriteLine("<center>");
                lb2 = new Label();
                lb2.Text = "<br/><br/><br/>Student Subject Wise Test Performance Analysis Chart<br/>";
                lb2.Style.Add("height", "100px");
                lb2.Style.Add("text-decoration", "none");
                lb2.Style.Add("font-family", "Book Antiqua;");
                lb2.Style.Add("font-size", "10px");
                lb2.Style.Add("font-weight", "bold");
                lb2.Style.Add("text-align", "center");
                lb2.RenderControl(htm);

                btngo_Click(sender, e);

                htm.InnerWriter.WriteLine("<br/><center>");
                gvTestPerfm.RenderControl(htm);
                htm.InnerWriter.WriteLine("</center><br/>");

                //string imgPath2 = Request.Url.GetLeftPart(UriPartial.Authority) + VirtualPathUtility.ToAbsolute("~/" + imgperfm);
                //if (PerformanceChart.Visible == true)
                //{
                //    htm.InnerWriter.WriteLine("<br/><br/><center><Table><tr><td><img src='" + imgPath2 + @"' \></td></tr></Table><br/><br/><br/></center><br/><br/>");
                //    //PerformanceChart.RenderControl(htm);
                //}

                lb2 = new Label();
                lb2.Text = "<br/><br/><br/><br/><br/><br/>";
                lb2.Style.Add("height", "100px");
                lb2.Style.Add("text-decoration", "none");
                lb2.Style.Add("font-family", "Book Antiqua;");
                lb2.Style.Add("font-size", "10px");
                lb2.Style.Add("font-weight", "bold");
                lb2.Style.Add("text-align", "center");
                lb2.RenderControl(htm);

                lb2 = new Label();
                lb2.Text = "<br/><br/><br/><br/><br/><br/>";
                lb2.Style.Add("height", "100px");
                lb2.Style.Add("text-decoration", "none");
                lb2.Style.Add("font-family", "Book Antiqua;");
                lb2.Style.Add("font-size", "10px");
                lb2.Style.Add("font-weight", "bold");
                lb2.Style.Add("text-align", "center");
                lb2.RenderControl(htm);

                //imgPath2 = Request.Url.GetLeftPart(UriPartial.Authority) + VirtualPathUtility.ToAbsolute("~/" + imgavg);

                htm.InnerWriter.WriteLine("<br/><br/><b><span style='font-family:Book Antiqua; font-size:10px;font-weight:bold; text-align:center; '>Student Subject Wise Test Average Analysis Chart</span><br/><br/></b><center>");
                gvAvgcount.RenderControl(htm);
                htm.InnerWriter.WriteLine("</center><br/><br/>");

                lb2 = new Label();
                lb2.Text = "<br/><br/>";
                lb2.Style.Add("height", "100px");
                lb2.Style.Add("text-decoration", "none");
                lb2.Style.Add("font-family", "Book Antiqua;");
                lb2.Style.Add("font-size", "10px");
                lb2.Style.Add("text-align", "center");
                lb2.RenderControl(htm);

                //htm.InnerWriter.WriteLine("<br/><center>");
                ////imgPath2 = Request.Url.GetLeftPart(UriPartial.Authority) + VirtualPathUtility.ToAbsolute("~/" + imgperfm);
                //if (chartAvg.Visible == true)
                //{
                //    htm.InnerWriter.WriteLine("<br/><center><Table><tr><td><img src='" + imgPath2 + @"' \></td></tr></Table><br/><br/></center>");
                //    //PerformanceChart.RenderControl(htm);
                //}
                //htm.InnerWriter.WriteLine("</center><br/><br/>");

                lb2 = new Label();
                lb2.Text = "<br/><br/>";
                lb2.Style.Add("height", "100px");
                lb2.Style.Add("text-decoration", "none");
                lb2.Style.Add("font-family", "Book Antiqua;");
                lb2.Style.Add("font-size", "10px");
                lb2.Style.Add("text-align", "center");
                lb2.RenderControl(htm);

                htm.InnerWriter.WriteLine("</center>");

                Response.Write(sw.ToString());
                Response.End();
                Response.Clear();
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
            lblerrormsg.Text = ex.StackTrace;
            lblerrormsg.Visible = true;
        }
    }

    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = string.Empty;
            string subjectName = string.Empty;
            string sections = string.Empty;
            string branch = string.Empty;
            string current_sem = string.Empty;
            string collegename = string.Empty;

            if (ddl_collage.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddl_collage.SelectedItem.Value).Trim();
            }
            if (ddl_Batch.Items.Count > 0)
            {
                batchyear = Convert.ToString(ddl_Batch.SelectedItem.Text).Trim();
            }
            if (ddl_Branch.Items.Count > 0)
            {
                degreecode = Convert.ToString(ddl_Branch.SelectedItem.Value).Trim();
                branch = Convert.ToString(ddl_Branch.SelectedItem.Text).Trim();
            }

            if (ddl_subject.Items.Count > 0)
            {
                subjectName = Convert.ToString(ddl_subject.SelectedItem.Text).Trim();
            }

            if (ddl_Sec.Items.Count > 0)
            {
                sections = Convert.ToString(ddl_Sec.SelectedValue).Trim();
                if (Convert.ToString(ddl_Sec.SelectedItem.Text).Trim().ToLower() == "all" || Convert.ToString(ddl_Sec.SelectedItem.Text).Trim().ToLower() == "" || Convert.ToString(ddl_Sec.SelectedItem.Text).Trim().ToLower() == "-1")
                {
                    sections = string.Empty;
                }
                else
                {
                    section = "&nbsp;-&nbsp;" + Convert.ToString(ddl_Sec.SelectedValue).ToUpper().Trim();
                }
            }

            foreach (System.Web.UI.WebControls.ListItem liSem in cbl_sem.Items)
            {
                if (liSem.Selected)
                {
                    if (string.IsNullOrEmpty(current_sem))
                    {
                        current_sem = liSem.Text;
                    }
                    else
                    {
                        current_sem += " & " + liSem.Text;
                    }
                }
            }

            degreedetails = branch.ToUpper() + "&nbsp;" + section + "&nbsp;(BATCH&nbsp;" + Convert.ToString(batchyear).Trim() + ")&nbsp;SEM&nbsp;-&nbsp;" + Convert.ToString(current_sem).Trim();

            btngo_Click(sender, e);

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Subject_Wise_Test_analysis.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 5f, 0f);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();

            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            Label lb = new Label();

            DataSet dscol = d2.select_method_wo_parameter("Select collname,address1,address2,address3,category,university from Collinfo where college_code='" + collegecode + "' ", "Text");
            if (dscol.Tables[0].Rows.Count > 0)
            {
                lb.Text = Convert.ToString(dscol.Tables[0].Rows[0]["collname"]).Trim() + "<br> ";
                lb.Style.Add("height", "100px");
                lb.Style.Add("text-decoration", "none");
                lb.Style.Add("font-family", "Book Antiqua;");
                lb.Style.Add("font-size", "18px");
                lb.Style.Add("text-align", "center");
                lb.RenderControl(hw);

                string address = "";
                if (Convert.ToString(dscol.Tables[0].Rows[0]["address1"]).Trim() != "")
                {
                    address = Convert.ToString(dscol.Tables[0].Rows[0]["address1"]).Trim();
                }
                if (Convert.ToString(dscol.Tables[0].Rows[0]["address2"]).Trim() != "")
                {
                    if (address.Trim() == "")
                    {
                        address = Convert.ToString(dscol.Tables[0].Rows[0]["address2"]).Trim();
                    }
                    else
                    {
                        address = address + ", " + Convert.ToString(dscol.Tables[0].Rows[0]["address2"]).Trim();
                    }
                }
                if (Convert.ToString(dscol.Tables[0].Rows[0]["address3"]).Trim() != "")
                {
                    if (address == "")
                    {
                        address = Convert.ToString(dscol.Tables[0].Rows[0]["address3"]).Trim();
                    }
                    else
                    {
                        address = address + ", " + Convert.ToString(dscol.Tables[0].Rows[0]["address3"]).Trim();
                    }
                }
                if (address.Trim() != "")
                {
                    lb.Text = address + "<br> ";
                    lb.Style.Add("height", "100px");
                    lb.Style.Add("text-decoration", "none");
                    lb.Style.Add("font-family", "Book Antiqua;");
                    lb.Style.Add("font-size", "12px");
                    lb.Style.Add("text-align", "center");
                    lb.RenderControl(hw);
                }
            }

            Label lb2 = new Label();
            lb2.Text = degreedetails;
            lb2.Style.Add("height", "100px");
            lb2.Style.Add("text-decoration", "none");
            lb2.Style.Add("font-family", "Book Antiqua;");
            lb2.Style.Add("font-size", "10px");
            lb2.Style.Add("text-align", "center");
            lb2.RenderControl(hw);

            Label lb3 = new Label();
            lb3.Text = "<br>";
            lb3.Style.Add("height", "200px");
            lb3.Style.Add("text-decoration", "none");
            lb3.Style.Add("font-family", "Book Antiqua;");
            lb3.Style.Add("font-size", "10px");
            lb3.Style.Add("text-align", "left");
            lb3.RenderControl(hw);

            Label lb4 = new Label();
            lb4.Text = "Subject Wise Test Analysis Report<br><br>";
            lb4.Style.Add("height", "200px");
            lb4.Style.Add("text-decoration", "none");
            lb4.Style.Add("font-family", "Book Antiqua;");
            lb4.Style.Add("font-size", "10px");
            lb4.Style.Add("text-align", "center");
            lb4.RenderControl(hw);

            StringWriter sw00 = new StringWriter();
            HtmlTextWriter hw00 = new HtmlTextWriter(sw00);

            lb4.Text = "Subject Name : " + subjectName.Trim() + " <br><br/>";
            lb4.Style.Add("height", "200px");
            lb4.Style.Add("text-decoration", "none");
            lb4.Style.Add("font-family", "Book Antiqua;");
            lb4.Style.Add("font-size", "10px");
            lb4.Style.Add("text-align", "left");
            lb4.RenderControl(hw00);

            if (gvStudTest.Rows.Count > 0)
            {
                gvStudTest.Visible = true;
                gvStudTest.AllowPaging = false;
                gvStudTest.HeaderRow.Style.Add("width", "15%");
                gvStudTest.HeaderRow.Style.Add("font-size", "8px");
                gvStudTest.HeaderRow.Style.Add("text-align", "center");
                gvStudTest.Style.Add("font-family", "Book Antiqua;");
                gvStudTest.Style.Add("font-size", "6px");
                gvStudTest.DataBind();
                gvStudTest.Enabled = true;
                gvStudTest.RenderControl(hw00);
                gvStudTest.DataBind();
                gvStudTest.Visible = false;
            }

            StringReader sr = new StringReader(sw.ToString());
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            htmlparser.Parse(sr);

            sr = new StringReader(sw00.ToString());
            htmlparser = new HTMLWorker(pdfDoc);
            htmlparser.Parse(sr);

            StringWriter sw0 = new StringWriter();
            HtmlTextWriter hw0 = new HtmlTextWriter(sw0);

            lb4 = new Label();
            if (PerformanceChart.Visible == true)
            {
                lb4.Text = "<br>Student Subject Wise Test Performance Analysis Chart<br><br><br>";
                lb4.Style.Add("height", "100px");
                lb4.Style.Add("text-decoration", "none");
                lb4.Style.Add("font-family", "Book Antiqua;");
                lb4.Style.Add("font-size", "10px");
                lb4.Style.Add("font-weight", "bold");
                lb4.Style.Add("text-align", "left");
                lb4.RenderControl(hw0);
            }

            if (gvTestPerfm.Rows.Count > 0)
            {
                gvTestPerfm.AllowPaging = false;
                gvTestPerfm.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                gvTestPerfm.HeaderRow.Style.Add("width", "15%");
                gvTestPerfm.HeaderRow.Style.Add("font-size", "10px");
                gvTestPerfm.HeaderRow.Style.Add("text-align", "center");
                gvTestPerfm.HeaderRow.Style.Add("font-weight", "bold");
                gvTestPerfm.Style.Add("font-family", "Book Antiqua;");
                gvTestPerfm.Style.Add("font-size", "6px");
                gvTestPerfm.DataBind();
                gvTestPerfm.Enabled = true;
                gvTestPerfm.RenderControl(hw0);
                gvTestPerfm.DataBind();
            }

            sr = new StringReader(sw0.ToString());
            htmlparser = new HTMLWorker(pdfDoc);
            htmlparser.Parse(sr);

            if (PerformanceChart.Visible == true)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    PerformanceChart.SaveImage(stream, ChartImageFormat.Png);
                    iTextSharp.text.Image chartImage = iTextSharp.text.Image.GetInstance(stream.GetBuffer());
                    chartImage.ScalePercent(75f);
                    pdfDoc.Add(chartImage);
                }
            }

            StringWriter sw1 = new StringWriter();
            HtmlTextWriter hw1 = new HtmlTextWriter(sw1);

            lb4 = new Label();
            if (chartAvg.Visible == true)
            {
                lb4.Text = "<br>Student Subject Wise Test Average Analysis Chart<br><br><br>";
                lb4.Style.Add("height", "100px");
                lb4.Style.Add("text-decoration", "none");
                lb4.Style.Add("font-family", "Book Antiqua;");
                lb4.Style.Add("font-size", "10px");
                lb4.Style.Add("font-weight", "bold");
                lb4.Style.Add("text-align", "left");
                lb4.RenderControl(hw1);
            }

            if (gvAvgcount.Rows.Count > 0)
            {
                gvAvgcount.AllowPaging = false;
                gvAvgcount.HeaderRow.Style.Add("width", "15%");
                gvAvgcount.HeaderRow.Style.Add("font-size", "8px");
                gvAvgcount.HeaderRow.Style.Add("text-align", "center");
                gvAvgcount.Style.Add("font-family", "Book Antiqua;");
                gvAvgcount.Style.Add("font-size", "6px");
                gvAvgcount.DataBind();
                gvAvgcount.Enabled = true;
                gvAvgcount.RenderControl(hw1);
                gvAvgcount.DataBind();
            }

            lb3.Text = "<br><b><br><br><br><br><br><br>";
            lb3.Style.Add("height", "200px");
            lb3.Style.Add("text-decoration", "none");
            lb3.Style.Add("font-family", "Book Antiqua;");
            lb3.Style.Add("font-size", "10px");
            lb3.Style.Add("text-align", "left");
            lb3.RenderControl(hw1);

            sr = new StringReader(sw1.ToString());
            htmlparser = new HTMLWorker(pdfDoc);
            htmlparser.Parse(sr);

            if (chartAvg.Visible == true)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    chartAvg.SaveImage(stream, ChartImageFormat.Png);
                    iTextSharp.text.Image chartImage = iTextSharp.text.Image.GetInstance(stream.GetBuffer());
                    chartImage.ScalePercent(75f);
                    pdfDoc.Add(chartImage);
                }
            }

            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.StackTrace;
            lblerrormsg.Visible = true;
        }
    }

    protected void gvStudTest_rowbound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //gvStudTest.HeaderRow.HorizontalAlign = HorizontalAlign.Center;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int count = e.Row.Cells.Count;
                for (int i = 0; i < count; i++)
                {
                    if (i == 1 || i == 2)
                    {
                        e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Left;
                    }
                    else
                    {
                        e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
                //e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                //e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                //e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;
            }
        }
        catch (Exception ex)
        {

            lblerrormsg.Text = ex.StackTrace;
            lblerrormsg.Visible = true;
        }
    }

    protected void gvTestPerfm_rowbound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //gvTestPerfm.HeaderRow.HorizontalAlign = HorizontalAlign.Center;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int count = e.Row.Cells.Count;
                for (int i = 0; i < count; i++)
                {
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.StackTrace;
            lblerrormsg.Visible = true;
        }
    }

    protected void gvAvgcount_rowbound(object sender, GridViewRowEventArgs e)
    {

        try
        {
            //gvAvgcount.HeaderRow.HorizontalAlign = HorizontalAlign.Center;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int count = e.Row.Cells.Count;
                for (int i = 0; i < count; i++)
                {
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.StackTrace;
            lblerrormsg.Visible = true;
        }
    }

}