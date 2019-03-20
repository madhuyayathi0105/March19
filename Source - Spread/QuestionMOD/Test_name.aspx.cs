using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class Test_name : System.Web.UI.Page
{
    #region Field Declaration

    bool cellclick = false;

    DAccess2 d2 = new DAccess2();

    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();

    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    bool isSchool = false;

    Hashtable hat = new Hashtable();

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
                divMainContent.Visible = false;
                bindcollege();
                BindBatch();
                BindDegree();
                bindbranch();
                bindsem();
                BindSectionDetail();
                GetSubject();
                ChangeHeaderName(isSchool);
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Page Load

    #region  Bind Header

    protected void bindcollege()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ds.Clear();
            ddl_collegename.Items.Clear();
            string clgname = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(clgname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegename.DataSource = ds;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
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
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlbatch.DataSource = ds;
                    ddlbatch.DataTextField = "Batch_year";
                    ddlbatch.DataValueField = "Batch_year";
                    ddlbatch.DataBind();
                    ddlbatch.SelectedIndex = ddlbatch.Items.Count - 1;
                }
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
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddldegree.DataSource = ds;
                    ddldegree.DataTextField = "course_name";
                    ddldegree.DataValueField = "course_id";
                    ddldegree.DataBind();

                }
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
            string course_id = Convert.ToString(ddldegree.SelectedValue).Trim();
            ddlbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlbranch.DataSource = ds;
                    ddlbranch.DataTextField = "dept_name";
                    ddlbranch.DataValueField = "degree_code";
                    ddlbranch.DataBind();
                }
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
                //first_year = Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0][1]));
                //duration = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0][0]));
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
            ddlsubject.Items.Clear();
            string sections = string.Empty;
            string strsec = "";
            if (ddlsec.Items.Count > 0)
            {
                sections = Convert.ToString(ddlsec.SelectedValue).Trim();
                if (Convert.ToString(ddlsec.SelectedItem.Text).Trim().ToLower() == "all" || Convert.ToString(ddlsec.SelectedItem.Text).Trim() == "")
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
                    if (Convert.ToString(Session["Staff_Code"]).Trim() == "")
                    {
                        //subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and   SM.degree_code=" + Convert.ToString(ddlbranch.SelectedValue).Trim() + " " + Convert.ToString(sems) + " and  S.subtype_no = Sem.subtype_no and promote_count=1 and SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue) + "' order by S.subject_no ";
                        subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and SM.degree_code='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' " + Convert.ToString(sems) + " and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue).Trim() + "' order by S.subject_no ";
                    }
                    else if (Convert.ToString(Session["Staff_Code"]).Trim() != "")
                    {
                        subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and st.subject_no=s.subject_no and s.syll_code=SM.syll_code and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.degree_code='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' " + Convert.ToString(sems) + " and  SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue).Trim() + "'  and sc.staff_code='" + Convert.ToString(Session["Staff_Code"]).Trim() + "' " + strsec + " order by S.subject_no ";
                    }
                    if (subjectquery != "")
                    {
                        ds.Dispose();
                        ds.Reset();
                        ds = d2.select_method(subjectquery, hat, "Text");
                        if (ds.Tables.Count > 0)
                        {
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
                else
                {
                    ddlsubject.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    private DataTable GetMonth()
    {
        DataTable dtMon = new DataTable();
        DataRow drMon;
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            dtMon.Rows.Clear();
            dtMon.Columns.Clear();
            dtMon.Columns.Add("Month_Name");
            dtMon.Columns.Add("Month_Value");
            var mon = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
            for (int m = 0; m < mon.Length; m++)
            {
                if (mon[m] != "")
                {
                    drMon = dtMon.NewRow();
                    drMon["Month_Name"] = mon[m];
                    drMon["Month_Value"] = m + 1;
                    dtMon.Rows.Add(drMon);
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }

        return dtMon;
    }

    public void ChangeHeaderName(bool isschool)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lbl_clg.Text = ((!isschool) ? "College" : "School");
            lblbatch.Text = ((!isschool) ? "Batch" : "Year");
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

    #endregion Bind Header

    #region DropDownList Events

    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContent.Visible = false;
            FpSpread1.Visible = false;
            BindBatch();
            bindbranch();
            bindsem();
            BindSectionDetail();
            GetSubject();
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
            divMainContent.Visible = false;
            FpSpread1.Visible = false;
            BindDegree();
            bindbranch();
            bindsem();
            BindSectionDetail();
            GetSubject();
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
            divMainContent.Visible = false;
            FpSpread1.Visible = false;
            bindsem();
            BindSectionDetail();
            GetSubject();
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
            divMainContent.Visible = false;
            FpSpread1.Visible = false;
            bindbranch();
            bindsem();
            BindSectionDetail();
            GetSubject();
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
            divMainContent.Visible = false;
            FpSpread1.Visible = false;
            BindSectionDetail();
            GetSubject();
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
            divMainContent.Visible = false;
            GetSubject();
            FpSpread1.Visible = false;
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
            divMainContent.Visible = false;
            FpSpread1.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion DropDownList Events

    #region Internal_External Changed Event

    protected void rb_internel_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblsubject.Visible = true;
            ddlsubject.Visible=true;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            FpSpread1.Sheets[0].Columns[1].Visible = false;
            FpSpread1.Sheets[0].Columns[2].Visible = false;
            FpSpread1.Sheets[0].Columns[3].Visible = true;
            FpSpread1.Visible = false;
           // format1();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void rb_external_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            lblsubject.Visible = true;
            ddlsubject.Visible = true;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            FpSpread1.Sheets[0].Columns[1].Visible = true;
            FpSpread1.Sheets[0].Columns[2].Visible = true;
            FpSpread1.Sheets[0].Columns[3].Visible = false;
            //format1();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void rb_General_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            lblsubject.Visible = false;
            ddlsubject.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            FpSpread1.Sheets[0].Columns[1].Visible = false;
            FpSpread1.Sheets[0].Columns[2].Visible = false;
            FpSpread1.Sheets[0].Columns[3].Visible = true;
           // format1();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Internal_External Changed Event

    #region Popup Close

    protected void btn_errorclose1_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            imgdiv3.Visible = false;
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
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            imgdiv2.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Popup Close

    #region Add Click

    protected void btn_add_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            string batch = Convert.ToString(ddlbatch.SelectedItem.Text).Trim();
            string degreecod = Convert.ToString(ddlbranch.SelectedItem.Value).Trim();
            string sem = Convert.ToString(ddlsem.SelectedItem.Text).Trim();
            string section = string.Empty;
            if (ddlsec.Items.Count > 0)
            {
                section = Convert.ToString(ddlsec.SelectedItem.Text).Trim();
            }

            string getquery = " select c.criteria,c.Criteria_no from CriteriaForInternal c, syllabus_master sy where c.syll_code=sy.syll_code and sy.Batch_Year='" + batch + "' and sy.degree_code='" + degreecod + "' and sy.semester='" + sem + "'";
            ds1 = d2.select_method_wo_parameter(getquery, "Text");

            string[] arraysize = new string[ds1.Tables[0].Rows.Count];
            if (rb_internel.Checked == true)
            {
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    for (int jk = 0; jk < ds1.Tables[0].Rows.Count; jk++)
                    {
                        arraysize[jk] = Convert.ToString(ds1.Tables[0].Rows[jk]["criteria"]);
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No Test(s) Were Found.Please Create Test First And Then Proceed.";
                    FpSpread1.Visible = false;
                    divMainContent.Visible = false;
                    btn_add.Visible = false;
                    btn_save.Visible = false;
                    return;
                }
            }
            FpSpread1.Sheets[0].AutoPostBack = false;
            string[] arraysect = new string[3];
            arraysect[0] = "Numeric";
            arraysect[1] = "Alpha";
            arraysect[2] = "Roman";
            FarPoint.Web.Spread.ComboBoxCellType cb = new FarPoint.Web.Spread.ComboBoxCellType(arraysize);
            FarPoint.Web.Spread.ComboBoxCellType cbsec = new FarPoint.Web.Spread.ComboBoxCellType(arraysect);
            FarPoint.Web.Spread.IntegerCellType intnoofsec = new FarPoint.Web.Spread.IntegerCellType();
            intnoofsec.ErrorMessage = "Please Enter Number Only";
            cb.UseValue = true;
            cb.ShowButton = false;
            cbsec.UseValue = true;

            string max_yr = d2.GetFunction("select  max(Batch_Year) from Registration");
            string min_yr = d2.GetFunction("select min(Batch_Year) from Registration");
            int year = Convert.ToInt32(max_yr);
            int s = 0;
            string[] cbyear = new string[1];
            if (min_yr != "0")
            {
                for (int r = year + 1; r > Convert.ToInt32(min_yr); r--)
                {
                    if (s != 0)
                    {
                        Array.Resize(ref cbyear, cbyear.Length + 1);
                    }
                    cbyear[s] = Convert.ToString(r);
                    s++;
                }

                if (min_yr == max_yr)
                {
                    if (s != 0)
                    {
                        Array.Resize(ref cbyear, cbyear.Length + 1);
                    }
                    cbyear[s] = Convert.ToString(max_yr);
                }

            }

            var cbstrmonth = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;

            string[] cbstrmonths = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
            string[] cbday = new string[31];
            //ArrayList cbday = new ArrayList();
            string[] cbmin = new string[60];
            string[] cbhrs = new string[13];
            for (int r = 0; r < 31; r++)
            {
                cbday[r] = Convert.ToString(r + 1).PadLeft(2, '0');

            }
            for (int min = 0; min < 60; min++)
            {
                cbmin[min] = Convert.ToString(min).PadLeft(2, '0');

            }
            for (int hr = 0; hr < 13; hr++)
            {
                cbhrs[hr] = Convert.ToString(hr).PadLeft(2, '0');
            }
            FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.ComboBoxCellType cmbcel1 = new FarPoint.Web.Spread.ComboBoxCellType();

            //    cmbcel1.va = cbstrmonth;

            FarPoint.Web.Spread.ComboBoxCellType cmbcel2 = new FarPoint.Web.Spread.ComboBoxCellType(cbyear);
            FarPoint.Web.Spread.ComboBoxCellType cmmonnum = new FarPoint.Web.Spread.ComboBoxCellType(cbstrmonths);
            FarPoint.Web.Spread.ComboBoxCellType cmbday = new FarPoint.Web.Spread.ComboBoxCellType(cbday);
            FarPoint.Web.Spread.ComboBoxCellType cmbmin = new FarPoint.Web.Spread.ComboBoxCellType(cbmin);
            FarPoint.Web.Spread.ComboBoxCellType cmbhr = new FarPoint.Web.Spread.ComboBoxCellType(cbhrs);
            cmbcel2.ShowButton = false;
            cmmonnum.ShowButton = false;
            cmbday.ShowButton = false;
            cmbmin.ShowButton = false;
            cbsec.ShowButton = false;

            FpSpread1.Sheets[0].RowCount++;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].RowCount);
            if (rb_external.Checked == true)
            {
                FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = cmbcel1;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = cmbcel2;
            }

            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = cb;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = intnoofsec;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].CellType = cbsec;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].CellType = cmbday;

            FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].CellType = cmmonnum;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].CellType = cmbcel2;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].CellType = cmbhr;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].CellType = cmbmin;

            FpSpread1.Sheets[0].Columns[11].CellType = chk;
            FpSpread1.Sheets[0].Columns[11].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[11].VerticalAlign = VerticalAlign.Middle;

            if (rb_internel.Checked == true)
            {
                FpSpread1.Sheets[0].Columns[1].Visible = false;
                FpSpread1.Sheets[0].Columns[2].Visible = false;
                FpSpread1.Sheets[0].Columns[3].Visible = true;
            }
            else
            {
                FpSpread1.Sheets[0].Columns[1].Visible = true;
                FpSpread1.Sheets[0].Columns[2].Visible = true;
                FpSpread1.Sheets[0].Columns[3].Visible = false;
            }

            FpSpread1.Columns[0].Locked = true;
            FpSpread1.SaveChanges();
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            divMainContent.Visible = true;
            FpSpread1.Visible = true;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Add Click

    #region Save Click

    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            FpSpread1.SaveChanges();
            bool inschek = false;
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                string date = "";
                if (FpSpread1.Sheets[0].Cells[i, 6].Text.Trim() != "" && FpSpread1.Sheets[0].Cells[i, 7].Text.Trim() != "" && FpSpread1.Sheets[0].Cells[i, 8].Text.Trim() != "")
                {
                    date = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 6].Text.Trim() + "/" + FpSpread1.Sheets[0].Cells[i, 7].Text.Trim() + "/" + FpSpread1.Sheets[0].Cells[i, 8].Text.Trim());

                }

                string testname = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text).Trim();
                string subno = Convert.ToString(ddlsubject.SelectedItem.Value).Trim();
                if (subno.Trim() == "")
                {
                    imgdiv3.Visible = true;
                    lbl_alert.Text = "Please Select Subject";
                    return;
                }

                if (testname.Trim() != "")
                {
                    if (!hat.ContainsKey(testname.Trim()))
                    {
                        hat.Add(testname.Trim(), date);
                    }
                    else
                    {
                        imgdiv3.Visible = true;
                        lbl_alert.Text = "Test Name Already Existed";
                        return;
                    }
                }

                int spvalue = 0;
                int.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 14].Value).Trim(), out spvalue);
                if (spvalue == 1)
                {
                    string totalsec = string.Empty;
                    string totals_sec = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Text).Trim();
                    string duration = string.Empty;
                    if (FpSpread1.Sheets[0].Cells[i, 9].Text.Trim() != "" && FpSpread1.Sheets[0].Cells[i, 10].Text.Trim() != "")
                    {
                        duration = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 9].Text).Trim() + ":" + Convert.ToString(FpSpread1.Sheets[0].Cells[i, 10].Text).Trim();
                    }

                    string Examtime = string.Empty;
                    string StartTime = string.Empty;
                    if (FpSpread1.Sheets[0].Cells[i, 11].Text.Trim() != "" && FpSpread1.Sheets[0].Cells[i, 12].Text.Trim() != "")
                    {
                        Examtime = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 11].Text).Trim() + ":" + Convert.ToString(FpSpread1.Sheets[0].Cells[i, 12].Text).Trim() + " " + Convert.ToString(FpSpread1.Sheets[0].Cells[i, 13].Text).Trim();
                        DateTime dt = Convert.ToDateTime(Examtime);
                        StartTime = dt.ToString("hh:mmtt");
                    }

                    string updatmonth_year = string.Empty;
                    string creatmonth_year = string.Empty;
                    string valusins = string.Empty;

                    string month = string.Empty;
                    string year = string.Empty;
                    if (rb_external.Checked == true)
                    {
                        if (FpSpread1.Sheets[0].Cells[i, 1].Text.Trim() != "" && FpSpread1.Sheets[0].Cells[i, 2].Text.Trim() != "")
                        {
                            month = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Value).Trim();
                            year = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text).Trim();
                            creatmonth_year = "  exam_type, exam_month, exam_year,";
                            valusins = " '1','" + month + "', '" + year + "' ";
                            updatmonth_year = "   exam_type='1', exam_month='" + month + "', exam_year='" + year + "' ";
                        }
                        else
                        {
                            imgdiv3.Visible = true;
                            lbl_alert.Text = "Please Select Exam Month and Exam Year";
                            return;
                        }
                    }
                    else if (rb_internel.Checked == true)
                    {
                        if (FpSpread1.Sheets[0].Cells[i, 3].Text.Trim() != "")
                        {
                            creatmonth_year = "  exam_type,";
                            valusins = " '2' ";
                            updatmonth_year = "   exam_type='2' ";
                        }
                        else
                        {
                            imgdiv3.Visible = true;
                            lbl_alert.Text = "Please Select Test Name";
                            return;
                        }
                    }
                    else
                    {
                        if (FpSpread1.Sheets[0].Cells[i, 3].Text.Trim() != "")
                        {
                            creatmonth_year = "  exam_type,";
                            valusins = " '0' ";
                            updatmonth_year = "   exam_type='0' ";
                        }
                        else
                        {
                            imgdiv3.Visible = true;
                            lbl_alert.Text = "Please Select Test Name";
                            return;
                        }


                    }

                    int t_sec = 0;
                    int.TryParse(totals_sec, out t_sec);
                    if (t_sec != 0)
                    {
                        totalsec = totals_sec;
                        string sectyp = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 5].Text).Trim();
                        if (date.Contains('/'))
                        {
                            DateTime dt = new DateTime();
                            bool dat = true;
                            dat = DateTime.TryParseExact(date, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dt);
                            if (dat == true)
                            {
                                string batch = Convert.ToString(ddlbatch.SelectedItem.Text).Trim();
                                string degreecod = Convert.ToString(ddlbranch.SelectedItem.Value).Trim();
                                string sem = Convert.ToString(ddlsem.SelectedItem.Text).Trim();
                                string section = string.Empty;
                                string qrysec = string.Empty;
                                if (ddlsec.Items.Count > 0)
                                {
                                    section = Convert.ToString(ddlsec.SelectedItem.Text).Trim();
                                    qrysec = " and  Sections='" + section + "'";
                                }
                                date = dt.ToString("MM/dd/yyyy");

                                string testvalu = d2.GetFunction(" select Criteria_no from CriteriaForInternal c, syllabus_master sy where c.syll_code=sy.syll_code and sy.Batch_Year='" + batch + "' and sy.degree_code='" + degreecod + "' and sy.semester='" + sem + "' and criteria='" + testname.Trim() + "'");

                                string qryExternal = string.Empty;
                                if (rb_external.Checked == true)
                                {
                                    testvalu = "Regular";
                                    qryExternal = "  and exam_month='" + month + "' and exam_year='" + year + "' and  exam_type='2'";
                                }

                                string checktestname = d2.GetFunction("select  *  from tbl_question_bank_master where  Batch_year='" + batch + "'and Degree_Code='" + degreecod + "' and  Semester='" + sem + "' " + qrysec + "  and Exam= '" + testvalu + "' ");
                                if (testvalu.Trim() != "0" && testvalu.Trim() != "")
                                {
                                    string insertqry = "";
                                    if (!rb_General.Checked)
                                    {
                                        insertqry = "if exists (select  *  from tbl_question_bank_master where  Batch_year='" + batch + "'and Degree_Code='" + degreecod + "' and  Semester='" + sem + "' " + qrysec + " and Exam= '" + testvalu.Trim() + "'  " + qryExternal + " ) update tbl_question_bank_master set  exam_date='" + dt.ToString("MM/dd/yyyy") + "', " + updatmonth_year + ", No_Sections='" + totalsec + "', Section_Type='" + sectyp + "' ,Duration='" + duration + "',Subject_no='" + subno + "', ExamTime ='" + StartTime + "'  where Batch_year='" + batch + "'and Degree_Code='" + degreecod + "' and  Semester='" + sem + "' " + qrysec + " and Exam= '" + testvalu.Trim() + "' " + qryExternal + "   else  insert into tbl_question_bank_master ( Batch_year,Degree_Code, Semester, Sections, " + creatmonth_year + " Exam,exam_date,No_Sections,Section_Type,Duration,Subject_no,ExamTime) values('" + batch + "','" + degreecod + "','" + sem + "','" + section + "'," + valusins + ",'" + testvalu + "','" + dt.ToString("MM/dd/yyyy") + "','" + totalsec + "','" + sectyp + "','" + duration + "','" + subno + "','" + StartTime + "')";
                                    }
                                    else
                                    {
                                        insertqry = "if exists (select  *  from tbl_question_bank_master where  Batch_year='" + batch + "'and Degree_Code='" + degreecod + "' and  Semester='" + sem + "' " + qrysec + " and Exam= '" + testvalu.Trim() + "'  " + qryExternal + " and exam_type='0' ) update tbl_question_bank_master set  exam_date='" + dt.ToString("MM/dd/yyyy") + "', " + updatmonth_year + ", No_Sections='" + totalsec + "', Section_Type='" + sectyp + "' ,Duration='" + duration + "', ExamTime ='" + StartTime + "'  where Batch_year='" + batch + "'and Degree_Code='" + degreecod + "' and  Semester='" + sem + "' " + qrysec + " and Exam= '" + testvalu.Trim() + "' " + qryExternal + "   else  insert into tbl_question_bank_master ( Batch_year,Degree_Code, Semester, Sections, " + creatmonth_year + " Exam,exam_date,No_Sections,Section_Type,Duration,ExamTime) values('" + batch + "','" + degreecod + "','" + sem + "','" + section + "'," + valusins + ",'" + testvalu + "','" + dt.ToString("MM/dd/yyyy") + "','" + totalsec + "','" + sectyp + "','" + duration + "','" + StartTime + "')";

                                    }

                                    int insert = d2.update_method_wo_parameter(insertqry, "Text");
                                    if (insert != 0)
                                    {
                                        inschek = true;
                                    }
                                }
                            }
                            else
                            {
                                imgdiv3.Visible = true;
                                lbl_alert.Text = "Date Should Be in The Format dd/MM/yyyy.";
                                return;
                            }
                        }
                        else
                        {
                            imgdiv3.Visible = true;
                            lbl_alert.Text = "Please Select The Date.";
                            return;
                        }
                    }
                    else
                    {
                        imgdiv3.Visible = true;
                        lbl_alert.Text = "Total Section Should Be in Numeric"; //"Enter Total Section Numeric Format";
                        return;
                    }
                }
            }
            if (inschek == true)
            {
                format1();
                imgdiv3.Visible = true;
                lbl_alert.Text = "Saved Successfully";
            }
            else
            {
                imgdiv3.Visible = true;
                lbl_alert.Text = "Not Saved";
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Save Click

    #region Go Click

    public void btn_go_Click(object sender, EventArgs e)
    {
        try
        {

            divMainContent.Visible = true;
            FpSpread1.Visible = true;
            if (ddl_collegename.Items.Count == 0)
            {
                divMainContent.Visible = false;
                lbl_alert1.Text = "No " + ((isSchool) ? "School" : "College") + " were Found";
                lbl_alert1.Visible = true;
                imgdiv2.Visible = true;
                return;
            }
            else
            {
                collegecode = Convert.ToString(ddl_collegename.SelectedValue).Trim();
            }
            if (ddlbatch.Items.Count == 0)
            {
                divMainContent.Visible = false;
                lbl_alert1.Text = "No " + ((isSchool) ? "Year" : " Batch") + " were Found";
                lbl_alert1.Visible = true;
                imgdiv2.Visible = true;
                return;
            }
            if (ddldegree.Items.Count == 0)
            {
                divMainContent.Visible = false;
                lbl_alert1.Text = "No " + ((isSchool) ? "School Type" : "Degree") + " were Found";
                lbl_alert1.Visible = true;
                imgdiv2.Visible = true;
                return;
            }

            if (ddlbranch.Items.Count == 0)
            {
                divMainContent.Visible = false;
                lbl_alert1.Text = "No " + ((isSchool) ? "Standard" : "Department") + " were Found";
                lbl_alert1.Visible = true;
                imgdiv2.Visible = true;
                return;
            }
            if (ddlsem.Items.Count == 0)
            {
                divMainContent.Visible = false;
                lbl_alert1.Text = "No " + ((isSchool) ? "Term" : " Semester") + " were Found";
                lbl_alert1.Visible = true;
                imgdiv2.Visible = true;
                return;
            }
            if (ddlsubject.Items.Count == 0)
            {
                divMainContent.Visible = false;
                lbl_alert1.Text = "No Subject were Found";
                lbl_alert1.Visible = true;
                imgdiv2.Visible = true;
                return;
            }
            format1();
            rb_external.Visible = true;
            rb_internel.Visible = true;
            rb_General.Visible = true;
            // FpSpread1.Sheets[0].Columns[3].Visible = true;
        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }
    }

    #endregion Go Click

    public void format1()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            FpSpread1.Visible = true;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 15;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.Visible = true;
            FpSpread1.SaveChanges();
            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Date";
            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Month";
            ////string[] cbstrmonth;
            var cbstrmonth = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;

            string[] cbstrmonths = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
            string[] cbday = new string[31];
            //ArrayList cbday = new ArrayList();
            string[] cbmin = new string[60];
            string[] cbhrs = new string[13];
            string[] cbmin1 = new string[60];
            string[] cbhrs1 = new string[13];
            string[] cbampm = new string[2];
            for (int r = 0; r < 31; r++)
            {
                cbday[r] = Convert.ToString(r + 1).PadLeft(2, '0');
            }
            for (int min = 0; min < 60; min++)
            {
                cbmin[min] = Convert.ToString(min).PadLeft(2, '0');
            }

            for (int hr = 0; hr < 13; hr++)
            {
                cbhrs[hr] = Convert.ToString(hr).PadLeft(2, '0');
            }
            for (int min1 = 0; min1 < 60; min1++)
            {
                cbmin1[min1] = Convert.ToString(min1).PadLeft(2, '0');
            }

            for (int hr1 = 0; hr1 < 13; hr1++)
            {
                cbhrs1[hr1] = Convert.ToString(hr1).PadLeft(2, '0');
            }

            cbampm[0] = "AM";
            cbampm[1] = "PM";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpSpread1.Sheets[0].Columns[0].Resizable = false;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Month";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            FpSpread1.Sheets[0].Columns[1].Resizable = false;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Year";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            FpSpread1.Sheets[0].Columns[2].Resizable = false;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Test Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[3].Resizable = false;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total Section";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[0].Resizable = false;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Section Type";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Exam Date";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 1, 3);

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].Text = "Date";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].Text = "Month";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 8].Text = "Year";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 8].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 8].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 8].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 8].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Duration in Minutes";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 1, 2);

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 9].Text = "Hrs";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 9].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 9].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 9].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 9].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 10].Text = "Min";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 10].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 10].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 10].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 10].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Start Time";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 11, 1, 3);

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 11].Text = "Hrs";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 11].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 11].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 11].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 11].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 12].Text = "Min";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 12].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 12].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 12].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 12].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 13].Text = "AM/PM";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 13].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 13].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 13].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 13].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 14].Text = "Select";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 14].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 14].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 14].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 14].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 14, 2, 1);

            string batch = Convert.ToString(ddlbatch.SelectedItem.Text).Trim();
            string degreecod = Convert.ToString(ddlbranch.SelectedItem.Value).Trim();
            string sem = Convert.ToString(ddlsem.SelectedItem.Text).Trim();
            string subjectNo = string.Empty;
            string section = string.Empty;
            string strsec = string.Empty;
            if (ddlbranch.Items.Count == 0)
            {
                divMainContent.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Department Were Found";
                return;
            }
            if (ddlsubject.Items.Count == 0)
            {
                divMainContent.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Subject Were Found";
                return;
            }
            else
            {
                subjectNo = Convert.ToString(ddlsubject.SelectedValue).Trim();
            }
            if (ddlsec.Items.Count > 0)
            {
                section = Convert.ToString(ddlsec.SelectedItem.Text).Trim();

                if (Convert.ToString(ddlsec.SelectedItem.Text).ToLower().Trim() == "all" || Convert.ToString(ddlsec.SelectedItem.Text).Trim() == "")
                {
                    strsec = string.Empty;
                }
                else
                {
                    strsec = " and Sections='" + section + "'";
                }
            }

            string typ = "";
            if (rb_internel.Checked == true)
            {
                typ = "2";
            }
            else if (rb_external.Checked == true)
            {
                typ = "1";
            }
            else
            {
                typ = "0";
            }


            string getquery = "";
            ds.Clear();
            if (!rb_General.Checked)
            {
                getquery = " select No_Sections,Section_Type,Duration, exam_type,(select criteria from CriteriaForInternal where Convert(nvarchar(25),Criteria_no) = Exam) as [Exam] ,CONVERT(varchar(10), exam_date,103) as exam_date ,exam_month,exam_year,RIGHT(Convert(VARCHAR(20), ExamTime,100),7) as ExamTime from tbl_question_bank_master where exam_type='" + typ + "'  and Batch_year='" + batch + "'and Degree_Code='" + degreecod + "' and  Semester='" + sem + "' and Subject_no='" + subjectNo + "'  " + strsec + " ";
                getquery = getquery + " select c.criteria,c.Criteria_no from CriteriaForInternal c, syllabus_master sy where c.syll_code=sy.syll_code and sy.Batch_Year='" + batch + "' and sy.degree_code='" + degreecod + "' and sy.semester='" + sem + "'";
                ds = d2.select_method_wo_parameter(getquery, "Text");
            }
            else 
            {
                getquery = " select No_Sections,Section_Type,Duration, exam_type,(select criteria from CriteriaForInternal where Convert(nvarchar(25),Criteria_no) = Exam) as [Exam] ,CONVERT(varchar(10), exam_date,103) as exam_date ,exam_month,exam_year,RIGHT(Convert(VARCHAR(20), ExamTime,100),7) as ExamTime from tbl_question_bank_master where exam_type='" + typ + "'  and Batch_year='" + batch + "'and Degree_Code='" + degreecod + "' and  Semester='" + sem + "'  " + strsec + " ";
                getquery = getquery + " select c.criteria,c.Criteria_no from CriteriaForInternal c, syllabus_master sy where c.syll_code=sy.syll_code and sy.Batch_Year='" + batch + "' and sy.degree_code='" + degreecod + "' and sy.semester='" + sem + "'";
                ds = d2.select_method_wo_parameter(getquery, "Text");
            
            }
            if (ds.Tables.Count > 0)
            {
                string[] arraysize = new string[ds.Tables[1].Rows.Count];
                if (rb_internel.Checked == true || rb_General.Checked == true)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            for (int jk = 0; jk < ds.Tables[1].Rows.Count; jk++)
                            {
                                arraysize[jk] = Convert.ToString(ds.Tables[1].Rows[jk]["criteria"]);
                            }
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert1.Text = "No Record(s) Were Found";
                        FpSpread1.Visible = false;
                        //divMainContent.Visible = false;
                        btn_add.Visible = false;
                        btn_save.Visible = false;
                        //return;
                    }
                }

                FpSpread1.Sheets[0].AutoPostBack = false;

                string[] arraysect = new string[3];
                arraysect[0] = "Numeric";
                arraysect[1] = "Alpha";
                arraysect[2] = "Roman";


                FarPoint.Web.Spread.ComboBoxCellType cbsec = new FarPoint.Web.Spread.ComboBoxCellType(arraysect);
                cbsec.UseValue = true;

                FarPoint.Web.Spread.IntegerCellType intnoofsec = new FarPoint.Web.Spread.IntegerCellType();
                intnoofsec.ErrorMessage = "Please Enter Number Only";
                FarPoint.Web.Spread.ComboBoxCellType cb = new FarPoint.Web.Spread.ComboBoxCellType(arraysize);
                cb.UseValue = true;
                cb.AutoPostBack = true;
                cb.ShowButton = false;

                string max_yr = d2.GetFunctionv("select  max(Batch_Year) from Registration").Trim();
                string min_yr = d2.GetFunctionv("select min(Batch_Year) from Registration").Trim();
                int year = Convert.ToInt32(max_yr);
                int s = 0;
                string[] cbyear = new string[1];
                if (min_yr.Trim() != "")
                {
                    for (int r = year + 1; r > Convert.ToInt32(min_yr); r--)
                    {
                        if (s != 0)
                        {
                            Array.Resize(ref cbyear, cbyear.Length + 1);
                        }
                        cbyear[s] = Convert.ToString(r);
                        s++;
                    }
                    if (min_yr == max_yr)
                    {
                        if (s != 0)
                        {
                            Array.Resize(ref cbyear, cbyear.Length + 1);
                        }
                        cbyear[s] = Convert.ToString(max_yr);
                    }
                }

                FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
                FarPoint.Web.Spread.ComboBoxCellType cmbcel1 = new FarPoint.Web.Spread.ComboBoxCellType();
                FarPoint.Web.Spread.ComboBoxCellType cmbcel2 = new FarPoint.Web.Spread.ComboBoxCellType(cbyear);
                FarPoint.Web.Spread.ComboBoxCellType cmmonnum = new FarPoint.Web.Spread.ComboBoxCellType(cbstrmonths);
                FarPoint.Web.Spread.ComboBoxCellType cmbday = new FarPoint.Web.Spread.ComboBoxCellType(cbday);
                FarPoint.Web.Spread.ComboBoxCellType cmbmin = new FarPoint.Web.Spread.ComboBoxCellType(cbmin);
                FarPoint.Web.Spread.ComboBoxCellType cmbhr = new FarPoint.Web.Spread.ComboBoxCellType(cbhrs);
                FarPoint.Web.Spread.ComboBoxCellType cmbmin1 = new FarPoint.Web.Spread.ComboBoxCellType(cbmin1);
                FarPoint.Web.Spread.ComboBoxCellType cmbhr1 = new FarPoint.Web.Spread.ComboBoxCellType(cbhrs1);
                FarPoint.Web.Spread.ComboBoxCellType cmbap = new FarPoint.Web.Spread.ComboBoxCellType(cbampm);

                cmbcel1.DataSource = GetMonth();
                cmbcel1.DataTextField = "Month_Name";
                cmbcel1.DataValueField = "Month_Value";
                //   cmbcel1.DataSource = cbstrmonth;

                //combocol.DataTextField = "TextVal";
                //combocol.DataValueField = "TextCode";
                cmbcel2.ShowButton = false;
                cmmonnum.ShowButton = false;
                cmbday.ShowButton = false;
                cmbmin.ShowButton = false;
                cbsec.ShowButton = false;
                chk.AutoPostBack = true;
                int he = 100;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);

                    FpSpread1.Sheets[0].Columns[1].CellType = cmbcel1;
                    // exam_month,exam_year 
                    FpSpread1.Sheets[0].Columns[2].CellType = cmbcel2;

                    int mont = 0;
                    int.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["exam_month"]), out mont);

                    if (mont != 0)
                    {
                        string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(mont);
                        FpSpread1.Sheets[0].Cells[i, 1].Value = Convert.ToString(ds.Tables[0].Rows[i]["exam_month"]);
                    }

                    FpSpread1.Sheets[0].Cells[i, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["exam_year"]);
                    FpSpread1.Sheets[0].Cells[i, 3].CellType = cb;
                    FpSpread1.Sheets[0].Cells[i, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Exam"]);
                    FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread1.Sheets[0].Cells[i, 4].CellType = intnoofsec;
                    FpSpread1.Sheets[0].Cells[i, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["No_Sections"]);
                    FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;

                    FpSpread1.Sheets[0].Columns[5].CellType = cbsec;
                    FpSpread1.Sheets[0].Cells[i, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["Section_Type"]);
                    FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread1.Sheets[0].Columns[6].CellType = cmbday;
                    FpSpread1.Sheets[0].Columns[7].CellType = cmmonnum;
                    // exam_month,exam_year 
                    FpSpread1.Sheets[0].Columns[8].CellType = cmbcel2;

                    string exdate = Convert.ToString(ds.Tables[0].Rows[i]["exam_date"]);
                    string[] split = new string[0];

                    if (exdate.Contains('/'))
                    {
                        split = exdate.Split('/');
                        if (split.Length > 0)
                        {
                            FpSpread1.Sheets[0].Cells[i, 6].Text = Convert.ToString(split[0]);
                            FpSpread1.Sheets[0].Cells[i, 7].Text = Convert.ToString(split[1]);
                            FpSpread1.Sheets[0].Cells[i, 8].Text = Convert.ToString(split[2]);
                        }
                    }

                    FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread1.Sheets[0].Columns[9].CellType = cmbhr;
                    FpSpread1.Sheets[0].Columns[10].CellType = cmbmin;
                    FpSpread1.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Center;
                    string durate = Convert.ToString(ds.Tables[0].Rows[i]["Duration"]);
                    string[] split1 = new string[0];
                    if (durate.Contains(':'))
                    {
                        split1 = durate.Split(':');
                        if (split1.Length > 0)
                        {
                            FpSpread1.Sheets[0].Cells[i, 9].Text = Convert.ToString(split1[0]);
                            FpSpread1.Sheets[0].Cells[i, 10].Text = Convert.ToString(split1[1]);
                        }
                    }

                    FpSpread1.Sheets[0].Columns[11].CellType = cmbhr1;
                    FpSpread1.Sheets[0].Columns[12].CellType = cmbmin1;
                    FpSpread1.Sheets[0].Columns[13].CellType = cmbap;
                    FpSpread1.Sheets[0].Columns[11].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[12].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[13].HorizontalAlign = HorizontalAlign.Center;
                    string extime = Convert.ToString(ds.Tables[0].Rows[i]["ExamTime"]);
                    string[] split2 = new string[0];
                    if (extime.Contains(':'))
                    {
                        split2 = extime.Split(':');
                        if (split2.Length > 0)
                        {
                            string time = split2[1].Contains("AM") ? "AM" : "PM";
                            string aa = Convert.ToString(time.ElementAt(0));
                            string hr = Convert.ToString(split2[0].Trim());
                            string min = Convert.ToString(split2[1].Split(new string[] { aa }, StringSplitOptions.None)[0]);
                            if (hr.Length != 2)
                                hr = "0" + hr;
                            FpSpread1.Sheets[0].Cells[i, 11].Text = Convert.ToString(hr);
                            FpSpread1.Sheets[0].Cells[i, 12].Text = min;
                            FpSpread1.Sheets[0].Cells[i, 13].Text = Convert.ToString(time);
                        }
                    }
                    he = he + 20;
                    FpSpread1.Sheets[0].Cells[i, 14].CellType = chk;
                    FpSpread1.Sheets[0].Columns[14].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[14].VerticalAlign = VerticalAlign.Middle;
                    FpSpread1.Sheets[0].Cells[i, 0].Locked = true;
                }

                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].RowCount);
                if (rb_external.Checked == true)
                {
                    FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = cmbcel1;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = cmbcel2;
                }

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = cb;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = intnoofsec;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].CellType = cbsec;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].CellType = cmbday;
                FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].CellType = cmmonnum;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].CellType = cmbcel2;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].CellType = cmbhr;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].CellType = cmbmin;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].CellType = cmbhr1;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 12].CellType = cmbmin1;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 13].CellType = cmbap;

                FpSpread1.Sheets[0].Columns[14].CellType = chk;

                FpSpread1.Sheets[0].Columns[13].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[13].VerticalAlign = VerticalAlign.Middle;
                if (rb_internel.Checked == true)
                {
                    FpSpread1.Sheets[0].Columns[1].Visible = false;
                    FpSpread1.Sheets[0].Columns[2].Visible = false;
                    FpSpread1.Sheets[0].Columns[3].Visible = true;
                }
                else if (rb_external.Checked == true)
                {
                    FpSpread1.Sheets[0].Columns[1].Visible = true;
                    FpSpread1.Sheets[0].Columns[2].Visible = true;
                    FpSpread1.Sheets[0].Columns[3].Visible = false;
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[1].Visible = false;
                    FpSpread1.Sheets[0].Columns[2].Visible = false;
                    FpSpread1.Sheets[0].Columns[3].Visible = true;
                }

                FpSpread1.Sheets[0].Columns[0].Width = 40;
                FpSpread1.Sheets[0].Columns[1].Width = 60;
                FpSpread1.Sheets[0].Columns[2].Width = 60;
                FpSpread1.Sheets[0].Columns[4].Width = 60;
                FpSpread1.Sheets[0].Columns[5].Width = 65;
                FpSpread1.Sheets[0].Columns[6].Width = 40;
                FpSpread1.Sheets[0].Columns[7].Width = 60;
                FpSpread1.Sheets[0].Columns[8].Width = 50;
                FpSpread1.Sheets[0].Columns[9].Width = 48;
                FpSpread1.Sheets[0].Columns[10].Width = 48;
                FpSpread1.Sheets[0].Columns[11].Width = 60;
                FpSpread1.Sheets[0].Columns[3].Width = 180;

                FpSpread1.Columns[0].Locked = true;
                FpSpread1.Height = he + 40;
                FpSpread1.SaveChanges();
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                btn_add.Visible = true;
                btn_save.Visible = true;
                divMainContent.Visible = true;
            }
            else
            {
                divMainContent.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Records Found";
                FpSpread1.Visible = false;
                btn_add.Visible = false;
                btn_save.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void FpSpread1_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        FpSpread1.SaveChanges();
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            int row = FpSpread1.ActiveSheetView.ActiveRow;
            int col = FpSpread1.ActiveSheetView.ActiveColumn;
            if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[row, 11].Value) == 1)
            {
                FpSpread1.Sheets[0].Cells[row, 11].Value = 1;
            }
            else if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[row, 11].Value) == 0)
            {
                FpSpread1.Sheets[0].Cells[row, 11].Value = 0;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    private bool IsValidDuration(int hour, int miniute)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            DateTime dt = new DateTime(1, 1, 1, 0, 30, 00);
            TimeSpan tt = new TimeSpan(hour, miniute, 0);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

}