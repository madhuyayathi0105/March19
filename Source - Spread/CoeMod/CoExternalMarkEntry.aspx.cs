using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System.Collections;
using InsproDataAccess;

public partial class CoExternalMarkEntry : System.Web.UI.Page
{
    #region variables Declaration


    InsproStoreAccess storAcc = new InsproStoreAccess();
    InsproDirectAccess dir = new InsproDirectAccess();
    static DAccess2 da = new DAccess2();
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string grouporusercode = string.Empty;
    string CollegeCode = string.Empty;
    DataSet ds2 = new DataSet();
    Hashtable hat = new Hashtable();
    string examMonth = string.Empty;
    string examyear = string.Empty;

    #endregion

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
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            CollegeCode = Session["collegecode"].ToString();
            if (!IsPostBack)
            {

                Save.Visible = false;
                Delete.Visible = false;
                lblaane.Visible = false;
                if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    grouporusercode = " group_code='" + Convert.ToString(Session["group_code"]).Trim().Split(';')[0] + "'";
                    usercode = Session["group_code"].ToString();
                }
                else
                {
                    grouporusercode = " usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
                    usercode = Convert.ToString(Session["usercode"]).Trim();
                }
                year1();
                loadtype();
            }
        }
        catch (Exception ex) { }
    }

    public void year1()
    {

        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {

            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            grouporusercode = " and group_code='" + group_user + "'";
        }
        else
        {
            grouporusercode = " and usercode='" + Session["usercode"].ToString().Trim() + "'";
        }
        Boolean setflag = false;
        ddlYear1.Items.Clear();//
        string settings = "Exam year and month for Mark" + Session["collegecode"].ToString();
        string getexamvalue = da.GetFunction("select value from master_settings where settings='" + settings.Trim() + "' " + grouporusercode + "");//Exam year and month Valuation
        if (getexamvalue.Trim() != null && getexamvalue.Trim() != "" && getexamvalue.Trim() != "0")
        {
            string[] spe = getexamvalue.Split(',');
            if (spe.GetUpperBound(0) == 1)
            {
                if (spe[0].Trim() != "0")
                {
                    ddlYear1.Items.Add(new ListItem(Convert.ToString(spe[0]), Convert.ToString(spe[0])));
                    setflag = true;
                }
            }
        }
        //setflag = false;
        if (setflag == false)
        {
            ds2.Clear();
            ds2 = da.select_method_wo_parameter(" select distinct Exam_year from exam_details order by Exam_year desc", "Text");
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                ddlYear1.DataSource = ds2;
                ddlYear1.DataTextField = "Exam_year";
                ddlYear1.DataValueField = "Exam_year";
                ddlYear1.DataBind();

            }
        }
        ddlYear1.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
    }

    protected void month1()
    {
        try
        {
            ddlMonth1.Items.Clear();
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {

                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                grouporusercode = " and group_code='" + group_user + "'";
            }
            else
            {
                grouporusercode = " and usercode='" + Session["usercode"].ToString().Trim() + "'";
            }
            Boolean setflag = false;
            string monthval = string.Empty;
            string settings = "Exam year and month for Mark" + Session["collegecode"].ToString();
            string getexamvalue = da.GetFunction("select value from master_settings where settings='" + settings.Trim() + "' " + grouporusercode + "");//Exam year and month Valuation
            if (getexamvalue.Trim() != null && getexamvalue.Trim() != "" && getexamvalue.Trim() != "0")
            {
                string[] spe = getexamvalue.Split(',');
                if (spe.GetUpperBound(0) == 1)
                {
                    if (spe[1].Trim() != "0")
                    {
                        string val = spe[1].ToString();
                        monthval = " and Exam_month='" + val + "'";
                    }
                }
            }
            ds2.Clear();
            string year1 = ddlYear1.SelectedValue;
            string strsql = "select distinct Exam_month,upper(convert(varchar(3),DateAdd(month,Exam_month,-1))) as monthName from exam_details where Exam_year='" + year1 + "'" + monthval + " order by Exam_month desc";
            ds2 = da.select_method_wo_parameter(strsql, "Text");
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                ddlMonth1.DataSource = ds2;
                ddlMonth1.DataTextField = "monthName";
                ddlMonth1.DataValueField = "Exam_month";
                ddlMonth1.DataBind();
                ddlMonth1.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void loadtype()
    {
        try
        {
            string collegecode = Session["collegecode"].ToString();
            ddltype.Items.Clear();
            string strquery = "select distinct type from course where college_code='" + collegecode + "' and type is not null and type<>''";
            DataSet dstype = da.select_method_wo_parameter(strquery, "Text");
            if (dstype.Tables.Count > 0 && dstype.Tables[0].Rows.Count > 0)
            {
                ddltype.DataSource = dstype;
                ddltype.DataTextField = "type";
                ddltype.DataBind();
                ddltype.Enabled = true;
                ddltype.Items.Insert(0, "");
                ddltype.Items.Insert(1, "All");
            }
            else
            {
                degree();
                ddltype.Enabled = false;
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void subjectbind()
    {
        try
        {
            ddlSubject.Items.Clear();
            ds2.Clear();
            string branc = Convert.ToString(ddlbranch1.SelectedValue).Trim();
            string semmv = Convert.ToString(ddlsem1.SelectedValue).Trim();

            string typeval = string.Empty;
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                if (Convert.ToString(ddltype.SelectedItem.Text).Trim().ToLower() != "" && Convert.ToString(ddltype.SelectedItem.Text).Trim().ToLower() != "all")
                {
                    typeval = " and C.Type='" + Convert.ToString(ddltype.SelectedItem.Text).Trim() + "'";
                }
            }
            string qeryss = "SELECT distinct ss.subject_type,s.subject_name,s.subject_code,s.subject_name+' - '+s.subject_code as subnamecode FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,syllabus_master sy,sub_sem ss,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and sy.semester='" + semmv + "' and  ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' " + typeval + "  and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "'";
            qeryss = qeryss + " union SELECT distinct ss.subject_type,s.subject_name,s.subject_code,s.subject_name+' - '+s.subject_code as subnamecode FROM subjectChooser sc,subject s,Registration r,Exam_Details ed,sub_sem ss,Degree d,Course c where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and sc.semester=ed.Current_Semester and s.subType_no=ss.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and sc.semester='" + semmv + "' and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' " + typeval + "  and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "' and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' order by s.subject_name,s.subject_code desc";
            ds2 = da.select_method(qeryss, hat, "Text");
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                ddlSubject.DataSource = ds2;
                ddlSubject.DataTextField = "subnamecode";
                ddlSubject.DataValueField = "subject_code";
                ddlSubject.DataBind();
            }
            ddlSubject.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));


        }
        catch (Exception ex)
        {
        }
    }

    protected void subjecttypebind()
    {
        try
        {
            ddlsubtype.Items.Clear();
            ds2.Clear();
            string branc = Convert.ToString(ddlbranch1.SelectedValue).Trim();
            string semmv = Convert.ToString(ddlsem1.SelectedValue).Trim();

            string typeval = string.Empty;
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                if (Convert.ToString(ddltype.SelectedItem.Text).Trim().ToLower() != "" && Convert.ToString(ddltype.SelectedItem.Text).Trim().ToLower() != "all")
                {
                    typeval = " and C.Type='" + Convert.ToString(ddltype.SelectedItem.Text).Trim() + "'";
                }
            }
            string qeryss = "SELECT distinct ss.subject_type FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,syllabus_master sy,sub_sem ss,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and sy.semester='" + semmv + "' and  ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' " + typeval + " ";
            qeryss = qeryss + " union SELECT distinct ss.subject_type FROM subjectChooser sc,subject s,Registration r,Exam_Details ed,sub_sem ss,Degree d,Course c where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and sc.semester=ed.Current_Semester and s.subType_no=ss.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and sc.semester='" + semmv + "' and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' " + typeval + " and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' order by ss.subject_type";


            ds2 = da.select_method(qeryss, hat, "Text");
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                ddlsubtype.DataSource = ds2;
                ddlsubtype.DataTextField = "subject_type";
                ddlsubtype.DataBind();
            }
            ddlsubtype.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
        }
        catch (Exception ex)
        {

        }
    }

    public void degree()
    {
        try
        {
            ddldegree1.Items.Clear();
            string usercode = Session["usercode"].ToString();
            string collegecode = Session["collegecode"].ToString();
            string singleuser = Session["single_user"].ToString();
            string group_user = Session["group_code"].ToString();
            string type = string.Empty;
            if (ddltype.Enabled == true)
            {
                if (ddltype.Items.Count > 0)
                {
                    if (Convert.ToString(ddltype.SelectedItem.Text).Trim().ToLower() != "" && Convert.ToString(ddltype.SelectedItem.Text).Trim().ToLower() != "all")
                    {
                        type = " and course.type='" + Convert.ToString(ddltype.SelectedItem.Text).Trim() + "'";
                    }
                }
            }
            string codevalues = string.Empty;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                codevalues = "and group_code='" + group_user + "'";
            }
            else
            {
                codevalues = "and user_code='" + usercode + "'";
            }
            string strquery = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where  course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code='" + collegecode + "' and deptprivilages.Degree_code=degree.Degree_code " + codevalues + " " + type + " ";
            ds2 = da.select_method_wo_parameter(strquery, "Text");
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                ddldegree1.DataSource = ds2;
                ddldegree1.DataTextField = "course_name";
                ddldegree1.DataValueField = "course_id";
                ddldegree1.DataBind();
            }
            ddldegree1.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));


        }
        catch (Exception ex)
        {

        }
    }

    public void bindbranch1()
    {
        try
        {
            ddlbranch1.Items.Clear();
            hat.Clear();
            string usercode = Session["usercode"].ToString();
            string collegecode = Session["collegecode"].ToString();
            string singleuser = Session["single_user"].ToString();
            string group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Add("single_user", singleuser.ToString());
            hat.Add("group_code", group_user);
            hat.Add("course_id", ddldegree1.SelectedValue);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);

            DataSet ds = da.select_method("bind_branch", hat, "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count2 = ds.Tables[0].Rows.Count;
                if (count2 > 0)
                {

                    ddlbranch1.DataSource = ds;
                    ddlbranch1.DataTextField = "dept_name";
                    ddlbranch1.DataValueField = "degree_code";
                    ddlbranch1.DataBind();
                }
            }
            ddlbranch1.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));


        }
        catch (Exception ex)
        {

        }
    }

    public void bindsem1()
    {
        try
        {
            ddlsem1.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            hat.Clear();
            string usercode = Session["usercode"].ToString();
            string collegecode = Session["collegecode"].ToString();
            string singleuser = Session["single_user"].ToString();
            string group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }

            int inclBatchYrRights = dir.selectScalarInt("select LinkValue from New_InsSettings where LinkName='IncludeBatchRightsInMarkEntry' and college_code ='" + collegecode + "' and user_code ='" + usercode + "'");

            if (inclBatchYrRights == 0)
            {
                DataSet ds = new DataSet();

                ds = da.BindSem(ddlbranch1.SelectedValue.ToString(), ddlYear1.SelectedValue.ToString(), collegecode);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    first_year = Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0][1]).ToString());
                    duration = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0][0]).ToString());
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlsem1.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlsem1.Items.Add(i.ToString());
                        }
                    }
                }
            }
            else
            {
                string selQ = "select distinct Current_Semester from Registration where Batch_Year in (select distinct batch_year from tbl_attendance_rights where user_id='" + usercode + "'  and college_code='" + collegecode + "') and college_code = " + collegecode + " and DelFlag='0' and Exam_Flag<>'debar' order by Current_Semester ";
                DataTable dtSem = dir.selectDataTable(selQ);
                if (dtSem.Rows.Count > 0)
                {
                    ddlsem1.DataSource = dtSem;
                    ddlsem1.DataTextField = "Current_Semester";
                    ddlsem1.DataValueField = "Current_Semester";
                    ddlsem1.DataBind();
                }
            }
            ddlsem1.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
        }
        catch (Exception ex)
        {

        }
    }

    public void clear()
    {
        //GridStudent.Visible = false;
        lblerr1.Visible = false;
        flstuRoll.Visible = false;
        GridView3.Visible = false;
        GridStudent.Visible = false;
        lblGrandTotal.Visible = false;
        lblTot.Visible = false;
        Save.Visible = false;
        Delete.Visible = false;
        Button1.Visible = false;
        Button2.Visible = false;
        lblmaxMark.Visible = false;
        lblmax.Visible = false;

    }

    protected void ddldegree1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            bindbranch1();
            bindsem1();
            ddlsubtype.Items.Clear();
            ddlSubject.Items.Clear();

        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlbranch1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            bindsem1();
            ddlsubtype.Items.Clear();
            ddlSubject.Items.Clear();

        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlsem1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            ddlSubject.Items.Clear();
            subjecttypebind();
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        ddlbranch1.Items.Clear();
        ddlsubtype.Items.Clear();
        ddlSubject.Items.Clear();

        degree();
        bindbranch1();
        bindsem1();
    }

    protected void ddlMonth1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            ddlsubtype.Items.Clear();
            ddlSubject.Items.Clear();
            examMonth = Convert.ToString(ddlMonth1.SelectedItem);
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlYear1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            month1();
            examyear = Convert.ToString(ddlYear1.SelectedItem);
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlsubtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            subjectbind();
        }
        catch (Exception ex)
        {

        }

    }

    protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            clear();
        }
        catch (Exception ex)
        {

        }

    }

    protected void btnviewre_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            GridStudent.Visible = false;
            #region visiblesetting
            string groupUserCode = string.Empty;
            string qryUserOrGroupCode = string.Empty;
            string userCode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string group = Convert.ToString(Session["group_code"]).Trim();
                if (group.Contains(";"))
                {
                    string[] group_semi = group.Split(';');
                    groupUserCode = Convert.ToString(group_semi[0]);
                }
                if (!string.IsNullOrEmpty(groupUserCode))
                {
                    qryUserOrGroupCode = " and group_code='" + groupUserCode + "'";
                }
            }
            else if (Session["usercode"] != null)
            {
                userCode = Convert.ToString(Session["usercode"]).Trim();
                if (!string.IsNullOrEmpty(userCode))
                {
                    qryUserOrGroupCode = " and usercode='" + userCode + "'";
                }
            }
            #endregion

            string valuationSettng =da.GetFunction("select value from Master_Settings where settings='Valuation Settings' " + qryUserOrGroupCode + "");

            lblaane.Visible = true;
            Save.Visible = false;
            Delete.Visible = false;
            Button2.Visible = false;
            Button1.Visible = false;
            if (ddlYear1.SelectedIndex == 0)
            {

                lblerr1.Visible = true;
                lblerr1.Text = "Please Select Year";
                return;
            }
            if (ddlMonth1.Items.Count == 0)
            {

                lblerr1.Visible = true;
                lblerr1.Text = "No Exam Month Found";
                return;
            }
            else if (ddlMonth1.SelectedValue.Trim() == "0")
            {

                lblerr1.Visible = true;
                lblerr1.Text = "Please Select Month";
                return;
            }
            if (ddltype.SelectedIndex == 0)
            {

                lblerr1.Visible = true;
                lblerr1.Text = "Please Select Type";
                return;
            }
            if (ddldegree1.SelectedIndex == 0)
            {

                lblerr1.Visible = true;
                lblerr1.Text = "Please Select Degree";
                return;
            }
            if (ddlbranch1.SelectedIndex == 0)
            {

                lblerr1.Visible = true;
                lblerr1.Text = "Please Select branch";
                return;
            }
            if (ddlsem1.SelectedIndex == 0)
            {

                lblerr1.Visible = true;
                lblerr1.Text = "Please Select Semester";
                return;
            }
            if (ddlSubject.SelectedIndex == 0)
            {

                lblerr1.Visible = true;
                lblerr1.Text = "Please Select Subject";
                return;
            }
            else
            {
                try
                {
                    clear();
                    flstuRoll.Visible = false;
                    GridStudent.Visible = false;
                    string degreeval = string.Empty;
                    string degreevalregmoder = string.Empty;
                    string degreevalttab = string.Empty;
                    string degreevalregis = string.Empty;
                    degreeval = " and ed.degree_code='" + ddlbranch1.SelectedValue + "'";
                    degreevalregmoder = " and M.degree_code='" + ddlbranch1.SelectedValue + "'";
                    degreevalttab = " and e.degree_code='" + ddlbranch1.SelectedValue + "'";
                    degreevalregis = " and r.degree_code='" + ddlbranch1.SelectedValue + "'";
                    string subjectCodeNew = Convert.ToString(ddlSubject.SelectedValue);
                    string qeryss = string.Empty;
                    qeryss = "SELECT ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,ead.subject_no,s.subject_name,subject_code,r.app_no,r.Roll_No,r.Reg_No,r.Stud_Name,ead.attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'REGULAR' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,Registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and r.Roll_No=ea.roll_no " + degreeval + " and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and s.subject_code='" + subjectCodeNew + "' and isnull(r.Reg_No,'') <>'' ";

                    qeryss = qeryss + " union SELECT ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,s.subject_no,s.subject_name,subject_code,r.app_no,r.Roll_No,r.Reg_No,r.Stud_Name,'' attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'NR' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag FROM Exam_Details ED,tbl_not_registred nt,subject s,Registration r,subjectChooser sc where ed.Exam_Month=nt.exam_month and ed.Exam_year=nt.exam_year and s.subject_no=nt.subject_no and r.Roll_No=nt.roll_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and sc.semester=ed.current_semester " + degreeval + " and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and s.subject_code='" + subjectCodeNew + "' and r.cc=0 and isnull(r.Reg_No,'') <>'' ";
                    qeryss = qeryss + " union SELECT ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,s.subject_no,s.subject_name,subject_code,r.app_no,r.Roll_No,r.Reg_No,r.Stud_Name,'' attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'NE' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag FROM Exam_Details ED,studentsemestersubjectdebar nt,subject s,Registration r where r.Roll_No=nt.roll_no and nt.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and (ed.current_semester=nt.semester or r.CC=1) " + degreeval + " and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and s.subject_code='" + subjectCodeNew + "' and isnull(r.Reg_No,'') <>'' and r.cc=0 order by ed.batch_year desc,ed.degree_code,ed.current_semester,r.reg_no,sts";

                    qeryss = qeryss + " select roll_no,regno,Course_Name,Dept_Name,dummy_no from  dummynumber du,Degree d,Department dt,Course c,subject s where du.degreecode =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and exam_month ='" + ddlMonth1.SelectedValue.ToString() + "' and exam_year ='" + ddlYear1.SelectedItem.Text.ToString() + "'  and s.subject_no=du.subject_no and s.subject_code='" + subjectCodeNew + "' and  (dummy_type ='1' or dummy_type ='0')";

                    DataSet ds = da.select_method_wo_parameter(qeryss, "text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        GridStudent.DataSource = ds.Tables[0];
                        GridStudent.DataBind();
                        GridStudent.Visible = true;
                        if (valuationSettng == "2")
                        {
                            Button1.Visible = true;
                            Button2.Visible = true;
                        }
                        lblmax.Visible = true;
                        lblmaxMark.Visible = true;
                        lblmaxMark.Text = Convert.ToString(ds.Tables[0].Rows[0]["max_ext_marks"]);
                    }
                    if (valuationSettng=="1")
                            lnkAttMark11(sender, e);
                }
                catch
                {
                }
            }
        }
        catch
        {

        }
    }
    protected void GridStudent_OnDataBound(object sender, EventArgs e)
    {
        try
        {
            string degreeval = string.Empty;
            string degreevalregmoder = string.Empty;
            string degreevalttab = string.Empty;
            string degreevalregis = string.Empty;
            degreeval = " and ed.degree_code='" + ddlbranch1.SelectedValue + "'";
            degreevalregmoder = " and M.degree_code='" + ddlbranch1.SelectedValue + "'";
            degreevalttab = " and e.degree_code='" + ddlbranch1.SelectedValue + "'";
            degreevalregis = " and r.degree_code='" + ddlbranch1.SelectedValue + "'";
            string subjectCodeNew = Convert.ToString(ddlSubject.SelectedValue);

            string strinternammark = "select m.roll_no,r.Reg_No,m.internal_mark,m.exam_code,ED.Exam_year,ED.Exam_Month,(ed.Exam_year*12+ed.Exam_Month) EXAMYEARMONTHVAL from mark_entry m,subject s,Registration r,Exam_Details ed where m.roll_no=r.Roll_No and m.subject_no=s.subject_no and ed.exam_code=m.exam_code and (ed.Exam_year*12+ed.Exam_Month)<=('" + ddlYear1.SelectedValue.ToString() + "'*12+'" + ddlMonth1.SelectedValue.ToString() + "') AND s.subject_code='" + subjectCodeNew + "' " + degreevalregis + " and m.internal_mark is not null order by m.roll_no,EXAMYEARMONTHVAL DESC,ed.Exam_year,ED.Exam_Month desc";
            DataSet dsinternal = da.select_method_wo_parameter(strinternammark, "Text");

            string getdetails = "select me.roll_no,me.external_mark,me.internal_mark,me.result,me.evaluation1,me.evaluation2,me.evaluation3,me.external_mark,me.total  from mark_entry me,Exam_Details ed,subject s where me.exam_code=ed.exam_code and me.subject_no=s.subject_no and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.ToString() + "'  " + degreeval + " and s.subject_code='" + subjectCodeNew + "'";
            getdetails = getdetails + "  select * from moderation m,subject s where m.subject_no=s.subject_no and s.subject_code='" + subjectCodeNew + "' " + degreevalregmoder + " and m.exam_year='" + ddlYear1.SelectedItem.ToString() + "'";
            getdetails = getdetails + " select convert(varchar(50),exam_date,105) as exam_date,exam_session from exmtt e,exmtt_det et,subject s where e.exam_code=et.exam_code and et.subject_no=s.subject_no and e.Exam_month='" + ddlMonth1.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear1.SelectedItem.ToString() + "' " + degreevalttab + " and s.subject_code='" + subjectCodeNew + "' ";
            ds2 = da.select_method_wo_parameter(getdetails, "Text");

            foreach (GridViewRow gr in GridStudent.Rows)
            {
                int attempts = 0;
                LinkButton lblint = (gr.FindControl("lblInt") as LinkButton);
                TextBox txtESE = (gr.FindControl("txtTotMark") as TextBox);
                Label lblatmp = (gr.FindControl("lblAttempts") as Label);
                Label lblev1 = (gr.FindControl("lblEv1") as Label);
                DataView dv1 = new DataView();
                string rollNo = (gr.FindControl("lblRollNO") as LinkButton).Text;
                int monthval = (Convert.ToInt32(ddlYear1.SelectedValue.ToString()) * 12) + Convert.ToInt32(ddlMonth1.SelectedValue.ToString());
                DataView dvattempts = new DataView();
                if (dsinternal.Tables.Count > 0 && dsinternal.Tables[0].Rows.Count > 0)
                {
                    dsinternal.Tables[0].DefaultView.RowFilter = "roll_no='" + rollNo + "' and EXAMYEARMONTHVAL<" + monthval + "";
                    dvattempts = dsinternal.Tables[0].DefaultView;
                }
                attempts = dvattempts.Count + 1;
                DataView dvintmark = new DataView();
                if (dsinternal.Tables.Count > 0 && dsinternal.Tables[0].Rows.Count > 0)
                {
                    dsinternal.Tables[0].DefaultView.RowFilter = "roll_no='" + rollNo + "'";
                    dvintmark = dsinternal.Tables[0].DefaultView;
                }
                ds2.Tables[0].DefaultView.RowFilter = "roll_no='" + rollNo + "'";
                dv1 = ds2.Tables[0].DefaultView;
                if (dv1.Count > 0)
                {
                    string resullts = dv1[0]["result"].ToString();
                    string intermarkf = dv1[0]["internal_mark"].ToString();
                    string ev1=Convert.ToString(dv1[0]["evaluation1"]);
                    double intermarkf223 = 0;
                    string intermarkforab = string.Empty;
                    if (intermarkf.Trim() != "" && intermarkf != null)
                    {
                        intermarkf223 = Convert.ToDouble(intermarkf);
                        intermarkforab = intermarkf;
                        if (intermarkf223 < 0)
                        {
                            intermarkforab = loadmarkat(intermarkf);
                        }
                        if (intermarkf223.ToString().Trim() == "-1")
                        {
                            intermarkforab = "AB";

                            if (resullts.ToString().Trim().ToLower() == "whd")
                            {

                            }
                            else
                            {
                                resullts = "NC";
                            }
                        }

                        if (intermarkforab.Trim().ToLower().Contains('a'))
                        {
                            intermarkforab = "-1";
                        }
                    }

                    string externn = dv1[0]["external_mark"].ToString();

                    if (externn == "-1")
                    {
                        externn = "AB";
                        resullts = "AB";
                    }
                    else if (externn == "-2")
                    {
                        externn = "NR";
                        resullts = "AB";
                    }
                    else if (externn == "-3")
                    {
                        externn = "NE";
                        resullts = "AB";
                    }
                    else if (externn == "-4")
                    {
                        externn = "LT";
                        resullts = "AB";
                    }
                    else if (externn == "")
                    {
                        externn = string.Empty;
                        resullts = string.Empty;
                    }
                    else
                    {
                        double getexmark = Convert.ToDouble(externn);
                        getexmark = Math.Round(getexmark, 0, MidpointRounding.AwayFromZero);
                        externn = Convert.ToString(externn);
                    }
                    txtESE.Text = externn;
                    lblint.Text = intermarkforab;
                    lblev1.Text=ev1;
                }
                lblatmp.Text = attempts.ToString();
                if (true)
                    txtESE.Enabled = true;
            }


        }
        catch
        {
        }
    }
    protected void OnDataBound(object sender, EventArgs e)
    {
        try
        {
        }
        catch
        {
        }
    }
    protected void lnkAttMark11(object sender, EventArgs e)
    {
        LinkButton lnkSelected = (LinkButton)sender;
        string rowIndxS = lnkSelected.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
        int rowIndx = Convert.ToInt32(rowIndxS) - 2;
        Session["Row"] = rowIndx;
        Save.Visible = false;
        Delete.Visible = false;
        string appNo = (GridStudent.Rows[rowIndx].FindControl("lblappno") as Label).Text;
        string studname = (GridStudent.Rows[rowIndx].FindControl("lblName") as LinkButton).Text;
        string rollNo = (GridStudent.Rows[rowIndx].FindControl("lblRollNO") as LinkButton).Text;
        string regNo = (GridStudent.Rows[rowIndx].FindControl("lblregno") as LinkButton).Text;
        string ExamCode = (GridStudent.Rows[rowIndx].FindControl("lblExamCode") as Label).Text;
        string subNo = (GridStudent.Rows[rowIndx].FindControl("lblSubNo") as Label).Text;
        string iCA = (GridStudent.Rows[rowIndx].FindControl("lblInt") as LinkButton).Text;
        lblInternal.Text = iCA;
        flstuRoll.Visible = true;
        txtRollOrReg.Text = regNo;
        lblStuName.Text = studname;

        loadMarkEntry(rollNo, appNo, ExamCode, subNo);
    }
    public void loadMarkEntry(string rollNo, string appNo, string Ecode, string subjectNo)
    {
        lblGrandTotal.Text = "0";
        DataTable dtMarks = new DataTable();
        dtMarks.Columns.Add("appNo");
        dtMarks.Columns.Add("PartNo");
        dtMarks.Columns.Add("PartName");
        dtMarks.Columns.Add("CourseOutComeNo");
        dtMarks.Columns.Add("QNo");
        dtMarks.Columns.Add("SubNo");
        dtMarks.Columns.Add("maxmrk");
        dtMarks.Columns.Add("criteria");
        dtMarks.Columns.Add("StuMark");//
        dtMarks.Columns.Add("MasterID");
        dtMarks.Columns.Add("examCode");
        dtMarks.Columns.Add("sub1");
        dtMarks.Columns.Add("sub2");
        DataRow drResult = null;
        DataTable dtCoSett = dir.selectDataTable("select * from Master_Settings where settings='COSettings'");
        if (!string.IsNullOrEmpty(subjectNo) && !string.IsNullOrEmpty(Ecode) && !string.IsNullOrEmpty(appNo) && appNo != "0")
        {
            Dictionary<string, string> ParametersDic = new Dictionary<string, string>();
            ParametersDic.Add("@subno", Convert.ToString(subjectNo));
            ParametersDic.Add("@CriteriaNO", Ecode);
            DataTable dtSettings = storAcc.selectDataTable("getCAQuesSettings", ParametersDic);

            string gT = string.Empty;
            gT = da.GetFunction("select total from mark_entry where roll_no='" + rollNo + "' and exam_code='" + Ecode + "' and subject_no='" + subjectNo + "'");
            double grandtot = 0;
            int QNO = 0;
            if (dtSettings.Rows.Count > 0)
            {
                foreach (DataRow dr in dtSettings.Rows)
                {
                    int Part = 0;
                    int QUes = 0;
                    string NoPart = Convert.ToString(dr["PartNo"]);
                    string NoQues = Convert.ToString(dr["NO_Ques"]);
                    string masterID = string.Empty;
                    int.TryParse(NoPart, out Part);
                    int.TryParse(NoQues, out QUes);
                    if (QUes > 0 && Part > 0)
                    {
                        drResult = dtMarks.NewRow();
                        QNO++;
                        string Qname = "Q" + QNO + "Mark";
                        string UnitNo = string.Empty;
                        string mark = " ";
                        string martxt = string.Empty;
                        string DescTotal = string.Empty;
                        string MaxMark = string.Empty;
                        string sub1 = Convert.ToString(dr["sub1"]);
                        string sub2 = Convert.ToString(dr["sub2"]);
                        string QVal = Convert.ToString(dr["Qno"]);
                        ParametersDic.Clear();
                        ParametersDic.Add("@Appno", appNo);
                        ParametersDic.Add("@ExamCode", Ecode);
                        DataTable dtStuMarks = storAcc.selectDataTable("getCAMarks", ParametersDic);

                        masterID = Convert.ToString(dr["MasterID"]);

                        if (dtStuMarks.Rows.Count > 0)
                        {
                            float markVal = 0;
                            dtStuMarks.DefaultView.RowFilter = "MasterID='" + masterID + "'";
                            DataView dvMark = dtStuMarks.DefaultView;
                            if (dvMark.Count > 0)
                            {

                                mark = loadmarkat(Convert.ToString(dvMark[0]["Marks"]));
                                martxt = Convert.ToString(dvMark[0]["Marks"]);
                            }
                            else
                            {

                            }
                            float.TryParse(mark, out markVal);
                            grandtot = grandtot + markVal;
                        }
                        MaxMark = Convert.ToString(dr["Mark"]);
                        UnitNo = Convert.ToString(dr["CourseOutComeNo"]);

                        if (!string.IsNullOrEmpty(UnitNo) && dtCoSett.Rows.Count > 0)
                        {
                            dtCoSett.DefaultView.RowFilter = " masterno='" + UnitNo + "'";
                            DataView dvCo = dtCoSett.DefaultView;
                            if (dvCo.Count > 0)
                                UnitNo = Convert.ToString(dvCo[0]["template"]);
                        }
                        else
                            UnitNo = "0";

                        drResult["appNo"] = appNo;
                        drResult["PartNo"] = NoPart;
                        drResult["PartName"] = getPartText(NoPart);
                        drResult["CourseOutComeNo"] = UnitNo;
                        drResult["QNo"] = QVal;
                        drResult["SubNo"] = subjectNo;
                        drResult["maxmrk"] = MaxMark;
                        drResult["criteria"] = Ecode;
                        drResult["StuMark"] = mark;
                        drResult["MasterID"] = masterID;
                        drResult["examCode"] = Ecode;
                        drResult["sub1"] = sub1;
                        drResult["sub2"] = sub2;
                        dtMarks.Rows.Add(drResult);
                    }
                }
            }

            if (dtMarks.Rows.Count > 0)
            {

                GridView3.Visible = true;
                double grandtotal = Math.Round(grandtot);
                GridView3.DataSource = dtMarks;
                GridView3.DataBind();//Setting Need
                Save.Visible = true;
                Delete.Visible = true;
                lblTot.Visible = true;
                lblGrandTotal.Visible = true;
                lblGrandTotal.Text = grandtotal.ToString();
            }

        }


    }
    protected void BtnPerv_Click(object sender, EventArgs e)
    {
        Save.Visible = false;
        Delete.Visible = false;
        if (GridStudent.Rows.Count > 0)
        {
            int rowPre = 0;
            if (Session["Row"] != null)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(Session["Row"])))
                {
                    rowPre = Convert.ToInt16(Convert.ToString(Session["Row"])) - 1;
                }
                if (rowPre >= 0)
                {
                    string appNo = (GridStudent.Rows[rowPre].FindControl("lblappno") as Label).Text;

                    string studname = (GridStudent.Rows[rowPre].FindControl("lblName") as LinkButton).Text;
                    string rollNo = (GridStudent.Rows[rowPre].FindControl("lblRollNO") as LinkButton).Text;
                    string ExamCode = (GridStudent.Rows[rowPre].FindControl("lblExamCode") as Label).Text;
                    string subNo = (GridStudent.Rows[rowPre].FindControl("lblSubNo") as Label).Text;
                    string iCA = (GridStudent.Rows[rowPre].FindControl("lblInt") as LinkButton).Text;
                    lblInternal.Text = iCA;
                    Session["Row"] = rowPre;
                    rblStatus.SelectedIndex = 0;
                    rblStatus_OnSelectedIndexChanged(sender, e);
                    txtRollOrReg.Text = rollNo.Trim();
                    lblStuName.Text = studname.Trim();
                    loadMarkEntry(rollNo, appNo, ExamCode, subNo);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Invalid Row')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Pls Select Student!!')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Invalid Row')", true);
        }
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        Save.Visible = false;
        Delete.Visible = false;
        if (GridStudent.Rows.Count > 0)
        {
            int rowPre = 0;
            if (Session["Row"] != null)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(Session["Row"])))
                {
                    rowPre = Convert.ToInt16(Convert.ToString(Session["Row"])) + 1;
                }
                if (rowPre >= 0 && rowPre < GridStudent.Rows.Count)
                {
                    string appNo = (GridStudent.Rows[rowPre].FindControl("lblappno") as Label).Text;
                    string studname = (GridStudent.Rows[rowPre].FindControl("lblName") as LinkButton).Text;
                    string rollNo = (GridStudent.Rows[rowPre].FindControl("lblRollNO") as LinkButton).Text;
                    string ExamCode = (GridStudent.Rows[rowPre].FindControl("lblExamCode") as Label).Text;
                    string subNo = (GridStudent.Rows[rowPre].FindControl("lblSubNo") as Label).Text;
                    string iCA = (GridStudent.Rows[rowPre].FindControl("lblInt") as LinkButton).Text;
                    lblInternal.Text = iCA;
                    Session["Row"] = rowPre;
                    rblStatus.SelectedIndex = 0;
                    rblStatus_OnSelectedIndexChanged(sender, e);
                    txtRollOrReg.Text = rollNo.Trim();
                    lblStuName.Text = studname.Trim();
                    loadMarkEntry(rollNo, appNo, ExamCode, subNo);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Invalid Row')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Pls Select Student!!')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Invalid Row')", true);
        }
    }
    protected void rblStatus_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            GridView3.Enabled = true;
            if (rblStatus.SelectedIndex.Equals(1) || rblStatus.SelectedIndex.Equals(2))
            {
                GridView3.Enabled = false;
            }
            else
            {
                GridView3.Enabled = true;
            }
        }
        catch { }
    }
    protected void Save_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtRollOrReg.Text != "")
            {
                int qVal = 0;
                double grandTotal = 0;
                string examCode = Convert.ToString((GridView3.Rows[0].FindControl("lblExamCode") as Label).Text);
                string subject_no = Convert.ToString((GridView3.Rows[0].FindControl("lblsubid") as Label).Text);
                foreach (GridViewRow grid in GridView3.Rows)
                {
                   
                    qVal++;
                    //drMark = dtInsert.NewRow();
                    string mark = string.Empty;
                    string subjectNo = Convert.ToString((grid.FindControl("lblsubid") as Label).Text);
                    string Qno = Convert.ToString((grid.FindControl("lblregno") as Label).Text);
                    string Cri = Convert.ToString((grid.FindControl("lblExamCode") as Label).Text);
                    string app = Convert.ToString((grid.FindControl("lblappno") as Label).Text);
                    string MasterId = Convert.ToString((grid.FindControl("lblMaterId") as Label).Text);
                    string exmCode = Convert.ToString((grid.FindControl("lblExamCode") as Label).Text);

                    if (rblStatus.SelectedIndex.Equals(0))
                    {
                        mark = Convert.ToString((grid.FindControl("txttest") as TextBox).Text);
                    }
                    else if (rblStatus.SelectedIndex.Equals(1))
                    {
                        mark = "-1";
                    }
                    else
                        mark = "-16";
                    int qryStatus = da.update_method_wo_parameter("if not exists(select * from NewInternalMarkEntry where ExamCode='" + exmCode + "' and app_no='" + app + "' and MasterID='" + MasterId + "' and examtype='1') insert into NewInternalMarkEntry (app_no,examcode,MasterID,Marks,examtype) values('" + app + "','" + exmCode + "','" + MasterId + "','" + mark + "','1') else update NewInternalMarkEntry SET Marks='" + mark + "' where ExamCode='" + exmCode + "' and app_no='" + app + "' and MasterID='" + MasterId + "'", "text");

                    float marva = 0;
                    float.TryParse(mark, out marva);
                    if (marva != -1 && marva != -16 && marva != -20)
                        grandTotal = grandTotal + marva;
                    grandTotal = Math.Round(grandTotal, 1, MidpointRounding.AwayFromZero);


                    lblGrandTotal.Text = grandTotal.ToString();
                }
                string icamark = lblInternal.Text;
                if(string.IsNullOrEmpty(icamark))
                    icamark="0";
                double CIA=0;
                double.TryParse(icamark,out CIA);
                double totalmarkvalu = grandTotal + CIA;
                int my = Convert.ToInt32(ddlMonth1.SelectedIndex.ToString()) + Convert.ToInt32(ddlYear1.SelectedValue.ToString()) * 12;

                string insupdquery = "if not exists(select * from mark_entry where exam_code='" + examCode + "' and roll_no='" + txtRollOrReg.Text + "' and subject_no='" + subject_no + "')insert into mark_entry (roll_no,subject_no,exam_code,internal_mark,attempts,MYData,evaluation1) values('" + txtRollOrReg.Text + "','" + subject_no + "','" + examCode + "','" + icamark + "','','" + my + "','" + grandTotal + "') else update mark_entry SET internal_mark='" + icamark + "',evaluation1='" + grandTotal + "',MYData='" + my + "' where roll_no='" + txtRollOrReg.Text + "' and subject_no='" + subject_no + "' and exam_code='" + examCode + "'";
               
               int insupdval = da.update_method_wo_parameter(insupdquery, "Text");
                if(insupdval!=0)
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                else
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not Saved')", true);
            }
        }
        catch
        {
        }
    }
    protected void Delete_Click(object sender, EventArgs e)
    {

    }

    public string loadmarkat(string mr)
    {
        string strgetval = string.Empty;
        if (mr == "-1")
        {
            strgetval = "AB";
        }
        else if (mr == "-2")
        {
            strgetval = "NR";
        }
        else if (mr == "-3")
        {
            strgetval = "RA";
        }
        else
            strgetval = mr;
        return strgetval;
    }
    private string getPartText(string mark)
    {
        try
        {
            mark = mark.Trim().ToLower();
            switch (mark)
            {
                case "1":
                    mark = "Part A";
                    break;
                case "2":
                    mark = "Part B";
                    break;
                case "3":
                    mark = "Part C";
                    break;
                case "4":
                    mark = "Part D";
                    break;
                case "5":
                    mark = "Part E";
                    break;
                case "6":
                    mark = "Part F";
                    break;
                case "7":
                    mark = "Part G";
                    break;
                case "8":
                    mark = "Part H";
                    break;
                case "9":
                    mark = "Part I";
                    break;
                case "10":
                    mark = "Part J";
                    break;
            }
        }
        catch
        {
        }
        return mark;
    }
    private string getSubText1(string mark)
    {
        try
        {
            mark = mark.Trim().ToLower();
            switch (mark)
            {
                case "1":
                    mark = "A";
                    break;
                case "2":
                    mark = "B";
                    break;
                case "3":
                    mark = "C";
                    break;
                case "4":
                    mark = "D";
                    break;
                case "5":
                    mark = "E";
                    break;
                case "6":
                    mark = "F";
                    break;
                case "7":
                    mark = "G";
                    break;
                case "8":
                    mark = "H";
                    break;
                case "9":
                    mark = "I";
                    break;
                case "10":
                    mark = "J";
                    break;

            }
        }
        catch
        {
        }
        return mark;
    }
    private string getSubText2(string mark)
    {
        try
        {
            mark = mark.Trim().ToLower();
            switch (mark)
            {
                case "1":
                    mark = "i";
                    break;
                case "2":
                    mark = "ii";
                    break;
                case "3":
                    mark = "iii";
                    break;
                case "4":
                    mark = "iv";
                    break;
                case "5":
                    mark = "v";
                    break;
                case "6":
                    mark = "vi";
                    break;
                case "7":
                    mark = "vii";
                    break;
                case "8":
                    mark = "viii";
                    break;
                case "9":
                    mark = "ix";
                    break;
                case "10":
                    mark = "x";
                    break;
            }
        }
        catch
        {
        }
        return mark;
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow gr in GridStudent.Rows)
            {

                string icamark = ((gr.FindControl("lblInt") as LinkButton).Text);
                string ese = (gr.FindControl("txtTotMark") as TextBox).Text;
                string lblatmp = (gr.FindControl("lblAttempts") as Label).Text;
                string evauation1 = (gr.FindControl("lblEv1") as Label).Text;
                string roll_no = (gr.FindControl("lblRollNO") as LinkButton).Text;
                string examcode = (gr.FindControl("lblExamCode") as Label).Text;
                string attempts = (gr.FindControl("lblAttempts") as Label).Text;
                string subject_no = (gr.FindControl("lblSubNo") as Label).Text;
                string creditpoint = (gr.FindControl("lblCredit") as Label).Text;
                string MinEXT = (gr.FindControl("lblminExt") as Label).Text;
                string MinInt = (gr.FindControl("lblminInt") as Label).Text;
                string MinTot = (gr.FindControl("lblmintTot") as Label).Text;


                int my = Convert.ToInt32(ddlMonth1.SelectedIndex.ToString()) + Convert.ToInt32(ddlYear1.SelectedValue.ToString()) * 12;
                string evauation2 = "";
                string evauation3 = "";
                double inmark = 0; double extMark = 0; double totMark = 0; double ev1 = 0; double minint = 0; double minext = 0;double mintot=0;
                double.TryParse(ese, out extMark);
                double.TryParse(icamark, out inmark);
                double.TryParse(evauation1, out ev1);
                double.TryParse(MinInt, out minint);
                double.TryParse(MinEXT, out minext);
                double.TryParse(MinTot, out mintot);
                string passorfail = "0";
                string result = string.Empty;
                string totalmarkvalu = string.Empty;
                if (ev1 == extMark)
                {
                    totMark = inmark + extMark;
                    totalmarkvalu = totMark.ToString();

                    if (evauation1.Trim().ToLower() == "aaa" || evauation1.Trim().ToLower() == "ab" || evauation1.Trim().ToLower() == "aa" || evauation1.Trim().ToLower() == "a")
                    {
                        evauation1 = "-1";
                        ese = "-1";
                        result = "AAA";
                        passorfail = "0";
                        totalmarkvalu = icamark;
                        
                    }
                    else if (evauation1.Trim().ToLower() == "ne" || evauation2.Trim().ToLower() == "ne")
                    {
                        evauation1 = "-2";
                        ese = "-2";
                        result = "Fail";
                        passorfail = "0";
                        totalmarkvalu = icamark;
                      
                    }
                    else if (evauation1.Trim().ToLower() == "nr" || evauation2.Trim().ToLower() == "nr")
                    {
                        evauation1 = "-3";
                        ese = "-3";
                        result = "Fail";
                        passorfail = "0";
                        totalmarkvalu = icamark;
                       
                    }
                    else if (evauation1.Trim().ToLower() == "m" || evauation2.Trim().ToLower() == "m")
                    {
                        evauation1 = "0";
                        ese = "0";
                        result = "WHD";
                        passorfail = "0";
                        totalmarkvalu = icamark;
                       
                    }
                    else if (evauation1.Trim().ToLower() == "lt" || evauation2.Trim().ToLower() == "lt")
                    {
                        evauation1 = "-4";
                        ese = "-4";
                        result = "Fail";
                        passorfail = "0";
                        totalmarkvalu = icamark;
                        
                    }

                    if (ese.Trim().ToLower() == "aaa" || ese.Trim().ToLower() == "ab" || ese.Trim().ToLower() == "aa" || ese.Trim().ToLower() == "a")
                    {
                        evauation1 = "-1";
                        ese = "-1";
                        result = "AAA";
                        passorfail = "0";
                        totalmarkvalu = icamark;

                    }
                    else if (ese.Trim().ToLower() == "ne" || ese.Trim().ToLower() == "ne")
                    {
                        evauation1 = "-2";
                        ese = "-2";
                        result = "Fail";
                        passorfail = "0";
                        totalmarkvalu = icamark;

                    }
                    else if (ese.Trim().ToLower() == "nr" || ese.Trim().ToLower() == "nr")
                    {
                        evauation1 = "-3";
                        ese = "-3";
                        result = "Fail";
                        passorfail = "0";
                        totalmarkvalu = icamark;

                    }
                    else if (ese.Trim().ToLower() == "m" || ese.Trim().ToLower() == "m")
                    {
                        evauation1 = "0";
                        ese = "0";
                        result = "WHD";
                        passorfail = "0";
                        totalmarkvalu = icamark;

                    }
                    else if (ese.Trim().ToLower() == "lt" || ese.Trim().ToLower() == "lt")
                    {
                        evauation1 = "-4";
                        ese = "-4";
                        result = "Fail";
                        passorfail = "0";
                        totalmarkvalu = icamark;

                    }

                   
                    if (inmark >= minint && extMark >= minext && totMark >= mintot)
                    {
                        passorfail = "1";
                        result = "Pass";
                    }
                    else
                    {
                        passorfail = "0";
                        result = "Fail";
                    }
                    string insupdquery = "if not exists(select * from mark_entry where exam_code='" + examcode + "' and roll_no='" + roll_no + "' and subject_no='" + subject_no + "')";
                    insupdquery = insupdquery + " insert into mark_entry (roll_no,subject_no,exam_code,internal_mark,external_mark,total,result,passorfail,attempts,MYData,rej_stat,cp,evaluation1,evaluation2,evaluation3)";
                    insupdquery = insupdquery + " values('" + roll_no + "','" + subject_no + "','" + examcode + "'," + icamark + "," + ese + "," + totalmarkvalu + ",'" + result + "','" + passorfail + "','" + attempts + "','" + my + "','0','" + creditpoint + "'," + evauation1 + "," + evauation2 + "," + evauation3 + ")";
                    insupdquery = insupdquery + " else";
                    insupdquery = insupdquery + " update mark_entry set internal_mark=" + icamark + ",external_mark=" + ese + ",total=" + totalmarkvalu + ",result='" + result + "',passorfail='" + passorfail + "',attempts='" + attempts + "',evaluation1=" + evauation1 + ",evaluation2=" + evauation2 + ",evaluation3=" + evauation3 + "";
                    insupdquery = insupdquery + " where exam_code='" + examcode + "' and roll_no='" + roll_no + "' and subject_no='" + subject_no + "'";

                    int insupdval = da.insert_method(insupdquery, hat, "Text");

                }
            }
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
        }
        catch
        {

        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {

    }

}