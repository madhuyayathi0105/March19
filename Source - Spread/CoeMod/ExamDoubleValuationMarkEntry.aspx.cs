using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gios.Pdf;
using InsproDataAccess;
using System.Text;
using wc = System.Web.UI.WebControls;
using System.Configuration;

public partial class CoeMod_ExamDoubleValuationMarkEntry : System.Web.UI.Page
{
    string CollegeCode = string.Empty;
    Boolean yes_flag = false;
    DataView dv1 = new DataView();
    DataView dv2 = new DataView();
    DataView dv3 = new DataView();
    DAccess2 da = new DAccess2();
    DataSet ds2 = new DataSet();
    DataSet dsss = new DataSet();
    Hashtable hat = new Hashtable();
    ReuasableMethods rs = new ReuasableMethods();
    InsproDirectAccess dirAccess = new InsproDirectAccess();
    static string examMonth = string.Empty;
    static string examyear = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string grouporusercode = string.Empty;

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
            lblerr1.Visible = false;
            if (!IsPostBack)
            {
                lblaane.Visible = false;

                fpspread.Visible = false;
                year1();
                loadtype();
                Session["MarkEntrySave"] = 1;
            }
        }
        catch (Exception ex)
        {
        }
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
        ddlYear1.Items.Clear();
        string getexamvalue = da.GetFunction("select value from master_settings where settings='Exam year and month for Mark' " + grouporusercode + "");//Exam year and month Valuation
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
            //dsss.Clear();
            dsss = da.select_method_wo_parameter(" select distinct Exam_year from exam_details order by Exam_year desc", "Text");
            if (dsss.Tables.Count > 0 && dsss.Tables[0].Rows.Count > 0)
            {
                ddlYear1.DataSource = dsss;
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
            string getexamvalue = da.GetFunction("select value from master_settings where settings='Exam year and month for Mark' " + grouporusercode + "");//Exam year and month Valuation
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
            dsss.Clear();
            string year1 = ddlYear1.SelectedValue;
            string strsql = "select distinct Exam_month,upper(convert(varchar(3),DateAdd(month,Exam_month,-1))) as monthName from exam_details where Exam_year='" + year1 + "'" + monthval + " order by Exam_month desc";
            dsss = da.select_method_wo_parameter(strsql, "Text");
            if (dsss.Tables.Count > 0 && dsss.Tables[0].Rows.Count > 0)
            {
                ddlMonth1.DataSource = dsss;
                ddlMonth1.DataTextField = "monthName";
                ddlMonth1.DataValueField = "Exam_month";
                ddlMonth1.DataBind();
                ddlMonth1.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
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
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    protected void subjectbind()
    {
        try
        {
            ddlSubject.Items.Clear();
            dsss.Clear();
            string branc = Convert.ToString(ddlbranch1.SelectedValue).Trim();
            string semmv = Convert.ToString(ddlsem1.SelectedValue).Trim();
            string degCode = Convert.ToString(ddlbranch1.SelectedValue).Trim();
            string typeval = string.Empty;
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                if (Convert.ToString(ddltype.SelectedItem.Text).Trim().ToLower() != "" && Convert.ToString(ddltype.SelectedItem.Text).Trim().ToLower() != "all")
                {
                    typeval = " and C.Type='" + Convert.ToString(ddltype.SelectedItem.Text).Trim() + "'";
                }
            }
            string qeryss = "SELECT distinct ss.subject_type,s.subject_name,s.subject_code,s.subject_name+' - '+s.subject_code as subnamecode FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,syllabus_master sy,sub_sem ss,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and sy.semester='" + semmv + "' and  ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and ed.degree_code='" + degCode + "' " + typeval + "  and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "'";

            qeryss = qeryss + " union SELECT distinct ss.subject_type,s.subject_name,s.subject_code,s.subject_name+' - '+s.subject_code as subnamecode FROM subjectChooser sc,subject s,Registration r,Exam_Details ed,sub_sem ss,Degree d,Course c where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and sc.semester=ed.Current_Semester and s.subType_no=ss.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and sc.semester='" + semmv + "' and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and ed.degree_code='" + degCode + "' " + typeval + "  and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "' and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' order by s.subject_name,s.subject_code desc";

            dsss = da.select_method(qeryss, hat, "Text");
            if (dsss.Tables.Count > 0 && dsss.Tables[0].Rows.Count > 0)
            {
                ddlSubject.DataSource = dsss;
                ddlSubject.DataTextField = "subnamecode";
                ddlSubject.DataValueField = "subject_code";
                ddlSubject.DataBind();
            }
            ddlSubject.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));


        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    protected void subjecttypebind()
    {
        try
        {
            ddlsubtype.Items.Clear();
            dsss.Clear();
            string branc = Convert.ToString(ddlbranch1.SelectedValue).Trim();
            string semmv = Convert.ToString(ddlsem1.SelectedValue).Trim();
            string degCode = Convert.ToString(ddlbranch1.SelectedValue).Trim();
            string typeval = string.Empty;
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                if (Convert.ToString(ddltype.SelectedItem.Text).Trim().ToLower() != "" && Convert.ToString(ddltype.SelectedItem.Text).Trim().ToLower() != "all")
                {
                    typeval = " and C.Type='" + Convert.ToString(ddltype.SelectedItem.Text).Trim() + "'";
                }
            }
            string qeryss = "SELECT distinct ss.subject_type FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,syllabus_master sy,sub_sem ss,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and sy.semester='" + semmv + "' and  ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.degree_code='" + degCode + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' " + typeval + " ";
            qeryss = qeryss + " union SELECT distinct ss.subject_type FROM subjectChooser sc,subject s,Registration r,Exam_Details ed,sub_sem ss,Degree d,Course c where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and sc.semester=ed.Current_Semester and s.subType_no=ss.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and sc.semester='" + semmv + "' and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.degree_code='" + degCode + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' " + typeval + " and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' order by ss.subject_type";

            dsss = da.select_method(qeryss, hat, "Text");
            if (dsss.Tables.Count > 0 && dsss.Tables[0].Rows.Count > 0)
            {
                ddlsubtype.DataSource = dsss;
                ddlsubtype.DataTextField = "subject_type";
                ddlsubtype.DataBind();
            }
            ddlsubtype.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
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
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
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
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
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

            int inclBatchYrRights = dirAccess.selectScalarInt("select LinkValue from New_InsSettings where LinkName='IncludeBatchRightsInMarkEntry' and college_code ='" + collegecode + "' and user_code ='" + usercode + "'");

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
                DataTable dtSem = dirAccess.selectDataTable(selQ);
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
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    public void subSubject()
    {
        try
        {
            ddlSubSub.Items.Clear();
            string semmv = Convert.ToString(ddlsem1.SelectedValue).Trim();
            string degCode = Convert.ToString(ddlbranch1.SelectedValue).Trim();
            string subjectCode = Convert.ToString(ddlSubject.SelectedValue).Trim();
            string SelectQ = "select SubPart,subsubjectid from COESubSubjectPartMater sm,COESubSubjectPartSettings sp where sm.id=sp.id and sm.ExamMonth='" + ddlMonth1.SelectedValue.ToString() + "' and sm.ExamYear='" + ddlYear1.SelectedItem.Text.ToString() + "' and sm.DegreeCode='" + degCode + "' and sm.Semester='" + semmv + "' and sp.SubCode='" + subjectCode + "'";
            DataTable dtSubSu = dirAccess.selectDataTable(SelectQ);
            if (dtSubSu.Rows.Count > 0)
            {
                ddlSubSub.DataSource = dtSubSu;
                ddlSubSub.DataTextField = "SubPart";
                ddlSubSub.DataValueField = "subsubjectid";
                ddlSubSub.DataBind();
            }
            ddlSubSub.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
        }
        catch
        {
        }
    }

    public void BindSubBatch()
    {
        ddlBatch.Items.Clear();
        string selectQ = "select distinct batch from examtheorybatch  where  subsubjectid='"+Convert.ToString(ddlSubSub.SelectedValue)+"'";
        DataTable dtBatch = dirAccess.selectDataTable(selectQ);
        if (dtBatch.Rows.Count > 0)
        {
            ddlBatch.DataSource = dtBatch;
            ddlBatch.DataTextField = "batch";
            ddlBatch.DataValueField = "batch";
            ddlBatch.DataBind();
        }
        ddlBatch.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        string intime = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
        if (Session["Entry_Code"] == null)
        {
            Session["Entry_Code"] = 0;
        }
        int a = da.update_method_wo_parameter("update UserEELog  set Out_Time='" + intime + "',LogOff='1' where entry_code='" + Session["Entry_Code"] + "'", "Text");

        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);
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
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
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
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
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
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlbranch1.Items.Clear();
        ddlsubtype.Items.Clear();
        ddlSubject.Items.Clear();
        clear();
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
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
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
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
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
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }

    }

    protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            subSubject();
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }

    }

    protected void ddlSubSub_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        BindSubBatch();
    }


    protected void btnreset_print(object sender, EventArgs e)
    {
        try
        {
            fpspread.SaveChanges();
            for (int r = 0; r < fpspread.Sheets[0].RowCount; r++)
            {
                fpspread.Sheets[0].Cells[r, 2].Text = string.Empty;
                fpspread.Sheets[0].Cells[r, 3].Text = string.Empty;
                fpspread.Sheets[0].Cells[r, 4].Text = string.Empty;
                fpspread.Sheets[0].Cells[r, 5].Text = string.Empty;
                fpspread.Sheets[0].Cells[r, 6].Text = string.Empty;
                fpspread.Sheets[0].Cells[r, 8].Text = string.Empty;
                fpspread.Sheets[0].Cells[r, 9].Text = string.Empty;
                fpspread.Sheets[0].Cells[r, 10].Text = string.Empty;
            }
            marksavefunction();
            lblAlertMsg.Visible = true;
            lblAlertMsg.Text = "Marks Deleted Successfully";
            divPopAlert.Visible = true;
            return;
            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Marks Deleted Successfully')", true);
            lblerr1.Visible = false;
            buttongo();
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    protected void btnprintt_print(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Exam Mark ";
            string pagename = "ExamDoubleValuationMarkEntry.aspx";
            Printcontrol.loadspreaddetails(fpspread, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }
        //try
        //{
        //    fpspread.SaveChanges();
        //    string[] spmaxsp = fpspread.Sheets[0].ColumnHeader.Cells[2, 8].Text.ToString().Split(':');
        //    double maxexternal = Convert.ToDouble(spmaxsp[1]);
        //    spmaxsp = fpspread.Sheets[0].ColumnHeader.Cells[2, 2].Text.ToString().Split(':');
        //    // double entmaxexternal = Convert.ToDouble(spmaxsp[1]); 
        //    double entmaxexternal = 0;
        //    if (spmaxsp.Length > 1)
        //    {
        //        if (spmaxsp[1].Trim() != "")
        //        {
        //            entmaxexternal = Convert.ToDouble(spmaxsp[1]);
        //        }
        //    }

        //    #region visiblesetting
        //    string groupUserCode = string.Empty;
        //    string qryUserOrGroupCode = string.Empty;
        //    string userCode = string.Empty;
        //    if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        //    {
        //        string group = Convert.ToString(Session["group_code"]).Trim();
        //        if (group.Contains(";"))
        //        {
        //            string[] group_semi = group.Split(';');
        //            groupUserCode = Convert.ToString(group_semi[0]);
        //        }
        //        if (!string.IsNullOrEmpty(groupUserCode))
        //        {
        //            qryUserOrGroupCode = " and group_code='" + groupUserCode + "'";
        //        }
        //    }
        //    else if (Session["usercode"] != null)
        //    {
        //        userCode = Convert.ToString(Session["usercode"]).Trim();
        //        if (!string.IsNullOrEmpty(userCode))
        //        {
        //            qryUserOrGroupCode = " and usercode='" + userCode + "'";
        //        }
        //    }
        //    #endregion

        //    string valuationSettng = string.Empty;
        //    valuationSettng = "3";
        //    valuationSettng = da.GetFunction("select value from Master_Settings where settings='Valuation Settings' " + qryUserOrGroupCode + "");
        //    if (valuationSettng == "1" || valuationSettng == "2")
        //    {
        //        print1(valuationSettng);
        //    }
        //    else
        //    {
        //        Font Fontbold123 = new Font("Times New Roman", 15, FontStyle.Bold);
        //        Font Fontbold = new Font("Times New Roman", 15, FontStyle.Bold);
        //        Font Fontsmall = new Font("Times New Roman", 10, FontStyle.Regular);
        //        Font Fontbold1 = new Font("Times New Roman", 10, FontStyle.Bold);
        //        Gios.Pdf.PdfDocument myprovdoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
        //        Gios.Pdf.PdfPage myprov_pdfpage = myprovdoc.NewPage();

        //        int coltop = 5;
        //        string collegename = string.Empty;
        //        string examexternalmark = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[2, 9].Tag);
        //        string maxexternalmark = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[2, 9].Tag);
        //        string maxinternalmark = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[2, 8].Tag);
        //        string totlmatrk = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[2, 10].Tag);
        //        // string modrationva = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Tag) + " %";


        //        string catgory = string.Empty;
        //        string coll_name = string.Empty;
        //        string ugorpg = string.Empty;
        //        string str = "select isnull(collname,'') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1,'') as address1,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code='" + Session["collegecode"] + "'";
        //        ds2 = da.select_method_wo_parameter(str, "Text");
        //        if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
        //        {
        //            catgory = ds2.Tables[0].Rows[0]["category"].ToString();
        //            coll_name = Convert.ToString(ds2.Tables[0].Rows[0]["collname"]);
        //        }
        //        ugorpg = da.GetFunction("select distinct c.Edu_Level from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.ToString() + "' and s.subject_code='" + ddlSubject.SelectedValue.ToString() + "'");

        //        string edusheet = "MARK SHEET / AVERAGE SHEET [" + ugorpg + "]";
        //        collegename = coll_name + " (" + catgory + ")";

        //        PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
        //                                                         new PdfArea(myprovdoc, 20, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleCenter, collegename);

        //        coltop = coltop + 20;
        //        PdfTextArea pts = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
        //                                                          new PdfArea(myprovdoc, 20, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, edusheet);
        //        PdfTextArea ptss = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
        //                             new PdfArea(myprovdoc, 200, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "PASSING MINIMUM");

        //        coltop = coltop + 15;
        //        PdfTextArea ptss1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
        //                             new PdfArea(myprovdoc, 20, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Month & Year : " + ddlMonth1.SelectedItem.Text.ToString() + "-" + ddlYear1.SelectedItem.Text.ToString());


        //        coltop = coltop + 15;
        //        PdfTextArea ptss2 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
        //                                             new PdfArea(myprovdoc, 20, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, " ");

        //        ptss2 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
        //                                         new PdfArea(myprovdoc, 20, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Class & Group : " + ddldegree1.SelectedItem.Text.ToString() + "-" + ddlbranch1.SelectedItem.Text.ToString());

        //        ptss2 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
        //                                           new PdfArea(myprovdoc, 20, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Class & Group : All Groups");

        //        int pasmintopval = coltop - 15;
        //        string gettext = da.GetFunction("select value from COE_Master_Settings where settings='" + ugorpg + " Passing Minimum'");
        //        string[] stva = gettext.Split('~');
        //        if (gettext.Trim() != "" && gettext.Trim() != "0")
        //        {
        //            for (int c = 0; c <= stva.GetUpperBound(0); c++)
        //            {
        //                PdfTextArea ptsspassmin = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
        //                               new PdfArea(myprovdoc, 350, pasmintopval + (c * 15), 400, 30), System.Drawing.ContentAlignment.MiddleLeft, stva[c].ToString());

        //                myprov_pdfpage.Add(ptsspassmin);
        //            }
        //        }
        //        if (coltop < pasmintopval + (stva.GetUpperBound(0) * 15))
        //        {
        //            coltop = pasmintopval + (stva.GetUpperBound(0) * 15);
        //        }

        //        string subjectname = da.GetFunction("Select subject_name from subject where subject_code='" + ddlSubject.SelectedValue.ToString() + "'");
        //        coltop = coltop + 15;
        //        PdfTextArea ptss22 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
        //                             new PdfArea(myprovdoc, 20, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Paper Name : " + subjectname);

        //        PdfTextArea ptss3 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
        //                                         new PdfArea(myprovdoc, 350, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Paper Code : " + ddlSubject.SelectedValue.ToString());

        //        PdfTextArea ptss31 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
        //                                         new PdfArea(myprovdoc, 350, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "");

        //        coltop = coltop + 30;

        //        PdfTextArea ptadate = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
        //                                         new PdfArea(myprovdoc, 10, 780, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Date");

        //        PdfTextArea ptadatecur = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
        //                                         new PdfArea(myprovdoc, 10, 800, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, DateTime.Now.ToString("dd/MM/yyyy"));

        //        PdfTextArea ptadepratment = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
        //                                        new PdfArea(myprovdoc, 275, 780, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Department");

        //        PdfTextArea ptachairman = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
        //                                        new PdfArea(myprovdoc, 450, 780, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Chairman");

        //        myprov_pdfpage.Add(ptc);
        //        myprov_pdfpage.Add(pts);
        //        myprov_pdfpage.Add(ptss);
        //        myprov_pdfpage.Add(ptss1);
        //        myprov_pdfpage.Add(ptss2);
        //        myprov_pdfpage.Add(ptss22);
        //        myprov_pdfpage.Add(ptss3);
        //        myprov_pdfpage.Add(ptss31);

        //        myprov_pdfpage.Add(ptadepratment);
        //        myprov_pdfpage.Add(ptadatecur);
        //        myprov_pdfpage.Add(ptadate);
        //        myprov_pdfpage.Add(ptachairman);

        //        Gios.Pdf.PdfTable table1;
        //        int val = 3;
        //        int noofrowpertable = 0;
        //        if (fpspread.Sheets[0].RowCount > 25)
        //        {
        //            noofrowpertable = 29;
        //        }
        //        else
        //        {
        //            noofrowpertable = fpspread.Sheets[0].RowCount + 4;
        //        }

        //        table1 = myprovdoc.NewTable(Fontsmall, noofrowpertable, 7, 1);
        //        table1.VisibleHeaders = false;

        //        table1.VisibleHeaders = false;
        //        table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
        //        table1.Columns[0].SetWidth(100);
        //        table1.Columns[1].SetWidth(200);
        //        table1.Columns[2].SetWidth(80);
        //        table1.Columns[3].SetWidth(80);
        //        table1.Columns[4].SetWidth(80);
        //        //table1.Columns[5].SetWidth(80);
        //        //table1.Columns[6].SetWidth(80);
                
        //        table1.CellRange(0, 0, 0, 5).SetFont(Fontbold1);
        //        //table1.CellRange(0, 1, 0, 5).SetFont(Fontbold1);
        //        table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
        //        if (ShowDummyNumber())
        //        {
        //            table1.Cell(0, 0).SetContent("Dummy No");
        //        }
        //        else
        //        {
        //            table1.Cell(0, 0).SetContent("Reg No");
        //        }
        //        table1.Cell(0, 0).SetFont(Fontbold1);
        //        //string subCode=Convert
        //        table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
        //        table1.Cell(0, 1).SetFont(Fontbold1);
        //        table1.Cell(0, 1).SetContent("Valuation");

        //        string val1 = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[1, 2].Text);

        //        table1.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
        //        table1.Cell(1, 1).SetFont(Fontbold1);
        //        table1.Cell(1, 1).SetContent(val1);

        //        table1.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
        //        table1.Cell(2, 1).SetFont(Fontbold1);
        //        table1.Cell(2, 1).SetContent("Max");

        //        string val2 = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[1, 3].Text);
        //        table1.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
        //        table1.Cell(1, 2).SetFont(Fontbold1);
        //        table1.Cell(1, 2).SetContent(val2);

        //        table1.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
        //        table1.Cell(2, 2).SetFont(Fontbold1);
        //        table1.Cell(2, 2).SetContent("Max");

        //        table1.Cell(2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
        //        table1.Cell(2, 3).SetFont(Fontbold1);

        //        table1.Cell(3, 1).SetContent(entmaxexternal);
        //        table1.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
        //        table1.Cell(3, 2).SetFont(Fontbold1);
        //        table1.Cell(3, 2).SetContent(entmaxexternal);

        //        //table1.Cell(2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
        //        //table1.Cell(2, 3).SetFont(Fontbold1);
        //        //table1.Cell(2, 3).SetContent("Max");

        //        table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
        //        table1.Cell(0, 3).SetFont(Fontbold1);
        //        table1.Cell(0, 3).SetContent("I.C.A");

        //        table1.Cell(2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
        //        table1.Cell(2, 3).SetFont(Fontbold1);
        //        table1.Cell(2, 3).SetContent("Max");

        //        table1.Cell(3, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
        //        table1.Cell(3, 3).SetFont(Fontbold1);
        //        table1.Cell(3, 3).SetContent(maxinternalmark);

        //        table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
        //        table1.Cell(0, 4).SetFont(Fontbold1);
        //        table1.Cell(0, 4).SetContent("E.S.E	");

        //        table1.Cell(2, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
        //        table1.Cell(2, 4).SetFont(Fontbold1);
        //        table1.Cell(2, 4).SetContent("Max");

        //        table1.Cell(3, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
        //        table1.Cell(3, 4).SetFont(Fontbold1);
        //        table1.Cell(3, 4).SetContent(maxexternalmark);

        //        table1.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
        //        table1.Cell(0, 5).SetFont(Fontbold1);
        //        table1.Cell(0, 5).SetContent("Total");

        //        table1.Cell(2, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
        //        table1.Cell(2, 5).SetFont(Fontbold1);
        //        table1.Cell(2, 5).SetContent("Max");

        //        table1.Cell(3, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
        //        table1.Cell(3, 5).SetFont(Fontbold1);
        //        table1.Cell(3, 5).SetContent(totlmatrk);

        //        table1.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
        //        table1.Cell(0, 6).SetFont(Fontbold1);
        //        table1.Cell(0, 6).SetContent("Result");

        //        table1.Rows[0].SetCellPadding(5);
        //        table1.Rows[1].SetCellPadding(5);
        //        table1.Rows[2].SetCellPadding(5);
        //        table1.Rows[3].SetCellPadding(5);

        //        //if (chkincluevel2.Checked == true)
        //        //{
        //        //    table1.Columns[2].SetWidth(1);
        //        //    table1.Cell(1, 2).SetContent("");
        //        //    table1.Cell(2, 2).SetContent("");
        //        //    table1.Cell(3, 2).SetContent("");

        //        //    table1.Columns[4].SetWidth(1);
        //        //    table1.Cell(1, 4).SetContent("");
        //        //    table1.Cell(2, 4).SetContent("");
        //        //    table1.Cell(3, 4).SetContent("");
        //        //    table1.Cell(0, 4).SetContent("");
        //        //}


        //        foreach (PdfCell pr in table1.CellRange(0, 0, 0, 0).Cells)
        //        {
        //            pr.RowSpan = 4;
        //        }
        //        foreach (PdfCell pr in table1.CellRange(0, 1, 0, 1).Cells)
        //        {
        //            pr.ColSpan = 2;
        //        }
        //        foreach (PdfCell pr in table1.CellRange(0, 3, 0, 3).Cells)
        //        {
        //            pr.RowSpan = 2;
        //        }
        //        foreach (PdfCell pr in table1.CellRange(0, 5, 0, 5).Cells)
        //        {
        //            pr.RowSpan = 2;
        //        }
        //        foreach (PdfCell pr in table1.CellRange(0, 6, 0, 6).Cells)
        //        {
        //            pr.RowSpan = 2;
        //        }

        //        Gios.Pdf.PdfTablePage myprov_pdfpage1;

        //        for (int row_cnt = 0; row_cnt < fpspread.Sheets[0].RowCount; row_cnt++)
        //        {
        //            if ((row_cnt % 25) == 0 && row_cnt > 0)
        //            {
        //                myprov_pdfpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(myprovdoc, 20, coltop, 570, 650));
        //                myprov_pdfpage.Add(myprov_pdfpage1);

        //                myprov_pdfpage.SaveToDocument();
        //                myprov_pdfpage = myprovdoc.NewPage();
        //                myprov_pdfpage.Add(ptc);
        //                myprov_pdfpage.Add(pts);
        //                myprov_pdfpage.Add(ptss);
        //                myprov_pdfpage.Add(ptss1);
        //                if (gettext.Trim() != "" && gettext.Trim() != "0")
        //                {
        //                    for (int c = 0; c <= stva.GetUpperBound(0); c++)
        //                    {
        //                        PdfTextArea ptsspassmin = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
        //                                       new PdfArea(myprovdoc, 350, pasmintopval + (c * 15), 400, 30), System.Drawing.ContentAlignment.MiddleLeft, stva[c].ToString());

        //                        myprov_pdfpage.Add(ptsspassmin);
        //                    }
        //                }
        //                myprov_pdfpage.Add(ptss2);
        //                myprov_pdfpage.Add(ptss22);
        //                myprov_pdfpage.Add(ptss3);
        //                myprov_pdfpage.Add(ptss31);

        //                myprov_pdfpage.Add(ptadepratment);
        //                myprov_pdfpage.Add(ptadatecur);
        //                myprov_pdfpage.Add(ptadate);
        //                myprov_pdfpage.Add(ptachairman);

        //                noofrowpertable = 0;
        //                if (fpspread.Sheets[0].RowCount > row_cnt + 25)
        //                {
        //                    noofrowpertable = 25;
        //                }
        //                else
        //                {
        //                    noofrowpertable = fpspread.Sheets[0].RowCount - row_cnt;
        //                }

        //                val = 3;
        //                table1.VisibleHeaders = false;
        //                table1 = myprovdoc.NewTable(Fontsmall, noofrowpertable + 4, 7, 1);

        //                table1.VisibleHeaders = false;
        //                table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
        //                table1.Columns[0].SetWidth(100);
        //                table1.Columns[1].SetWidth(80);
        //                table1.Columns[2].SetWidth(80);
        //                table1.Columns[3].SetWidth(80);
        //                table1.Columns[4].SetWidth(80);
        //                table1.Columns[5].SetWidth(80);
        //                table1.Columns[6].SetWidth(80);
        //                //table1.Columns[7].SetWidth(80);


        //                table1.Cell(0, 0).SetFont(Fontbold1);

        //                table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
        //                table1.Cell(0, 1).SetFont(Fontbold1);
        //                table1.Cell(0, 1).SetContent("Valuation");

        //                table1.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
        //                table1.Cell(1, 1).SetFont(Fontbold1);
        //                table1.Cell(1, 1).SetContent(val1);

        //                table1.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
        //                table1.Cell(2, 1).SetFont(Fontbold1);
        //                table1.Cell(2, 1).SetContent("Max");

        //                table1.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
        //                table1.Cell(1, 2).SetFont(Fontbold1);
        //                table1.Cell(1, 2).SetContent(val2);

        //                table1.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
        //                table1.Cell(2, 2).SetFont(Fontbold1);
        //                table1.Cell(2, 2).SetContent("Max");

        //                //table1.Cell(2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
        //                //table1.Cell(2, 3).SetFont(Fontbold1);
        //                //table1.Cell(2, 3).SetContent("Max");

        //                table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
        //                table1.Cell(0, 3).SetFont(Fontbold1);
        //                table1.Cell(0, 3).SetContent("I.C.A");

        //                table1.Cell(2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
        //                table1.Cell(2, 3).SetFont(Fontbold1);
        //                table1.Cell(2, 3).SetContent("Max");

        //                table1.Cell(3, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
        //                table1.Cell(3, 3).SetFont(Fontbold1);
        //                table1.Cell(3, 3).SetContent(maxinternalmark);

        //                table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
        //                table1.Cell(0, 4).SetFont(Fontbold1);
        //                table1.Cell(0, 4).SetContent("E.S.E	");

        //                table1.Cell(2, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
        //                table1.Cell(2, 4).SetFont(Fontbold1);
        //                table1.Cell(2, 4).SetContent("Max");

        //                table1.Cell(3, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
        //                table1.Cell(3, 4).SetFont(Fontbold1);
        //                table1.Cell(3, 4).SetContent(maxexternalmark);

        //                table1.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
        //                table1.Cell(0, 5).SetFont(Fontbold1);
        //                table1.Cell(0, 5).SetContent("Total");

        //                table1.Cell(2, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
        //                table1.Cell(2, 5).SetFont(Fontbold1);
        //                table1.Cell(2, 5).SetContent("Max");

        //                table1.Cell(3, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
        //                table1.Cell(3, 5).SetFont(Fontbold1);
        //                table1.Cell(3, 5).SetContent(totlmatrk);

        //                table1.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
        //                table1.Cell(0, 6).SetFont(Fontbold1);
        //                table1.Cell(0, 6).SetContent("Result");

        //                table1.Rows[0].SetCellPadding(5);
        //                table1.Rows[1].SetCellPadding(5);
        //                table1.Rows[2].SetCellPadding(5);
        //                table1.Rows[3].SetCellPadding(5);

        //                foreach (PdfCell pr in table1.CellRange(0, 0, 0, 0).Cells)
        //                {
        //                    pr.RowSpan = 4;
        //                }
        //                foreach (PdfCell pr in table1.CellRange(0, 1, 0, 1).Cells)
        //                {
        //                    pr.ColSpan = 2;
        //                }
        //                foreach (PdfCell pr in table1.CellRange(0, 3, 0, 3).Cells)
        //                {
        //                    pr.RowSpan = 2;
        //                }
        //                foreach (PdfCell pr in table1.CellRange(0, 5, 0, 5).Cells)
        //                {
        //                    pr.RowSpan = 2;
        //                }
        //                foreach (PdfCell pr in table1.CellRange(0, 6, 0, 6).Cells)
        //                {
        //                    pr.RowSpan = 2;
        //                }
        //                //foreach (PdfCell pr in table1.CellRange(0, 7, 0, 7).Cells)
        //                //{
        //                //    pr.RowSpan = 2;
        //                //}

        //            }

        //            string sno = fpspread.Sheets[0].Cells[row_cnt, 0].Text.ToString();
        //            string roll_noo = fpspread.Sheets[0].Cells[row_cnt, 1].Note.ToString();
        //            string roll_noo2 = fpspread.Sheets[0].Cells[row_cnt, 1].Text.ToString();
        //            string batchyr = fpspread.Sheets[0].Cells[row_cnt, 0].Tag.ToString();
        //            string e1 = fpspread.Sheets[0].Cells[row_cnt, 2].Text.ToString();
        //            string e2 = fpspread.Sheets[0].Cells[row_cnt, 3].Text.ToString();
        //            string ca = fpspread.Sheets[0].Cells[row_cnt, 7].Text.ToString();
        //            string ese = fpspread.Sheets[0].Cells[row_cnt, 8].Text.ToString();
        //            string totall = fpspread.Sheets[0].Cells[row_cnt, 9].Text.ToString();
        //            string results = fpspread.Sheets[0].Cells[row_cnt, 10].Text.ToString();

        //            val++;
        //            table1.Rows[val].SetCellPadding(5);
        //            table1.Cell(val, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
        //            table1.Cell(val, 0).SetContent(roll_noo2);

        //            table1.Cell(val, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
        //            table1.Cell(val, 1).SetContent(e1);

        //            table1.Cell(val, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
        //            if (true)
        //            {
        //                table1.Cell(val, 2).SetContent(e2);
        //            }

        //            table1.Cell(val, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
        //            table1.Cell(val, 3).SetContent(ca);

        //            table1.Cell(val, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
        //            table1.Cell(val, 4).SetContent(ese);

        //            table1.Cell(val, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
        //            table1.Cell(val, 5).SetContent(totall);

        //            table1.Cell(val, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
        //            table1.Cell(val, 6).SetContent(results);
        //        }

        //        myprov_pdfpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(myprovdoc, 20, coltop, 570, 650));
        //        myprov_pdfpage.Add(myprov_pdfpage1);

        //        myprov_pdfpage.Add(ptadepratment);
        //        myprov_pdfpage.Add(ptadatecur);
        //        myprov_pdfpage.Add(ptadate);
        //        myprov_pdfpage.Add(ptachairman);
        //        myprov_pdfpage.SaveToDocument();

        //        string appPath = HttpContext.Current.Server.MapPath("~");
        //        if (appPath != "")
        //        {
        //            string szPath = appPath + "/Report/";
        //            string szFile = "MARKSHEERTPRINT.pdf";

        //            myprovdoc.SaveToFile(szPath + szFile);
        //            Response.ClearHeaders();
        //            Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
        //            Response.ContentType = "application/pdf";
        //            Response.WriteFile(szPath + szFile);
        //        }
        //    }

        //}
        //catch (Exception ex)
        //{
        //    lblerr1.Visible = true;
        //    lblerr1.Text = ex.ToString();
        //}
    }

    protected void btnviewre_Click(object sender, EventArgs e)
    {
        try
        {

            if (ddlYear1.SelectedIndex == 0)
            {
                btnreset.Visible = false;
                lblaane.Visible = false;

                btnsave1.Visible = false;
                btnprintt.Visible = false;
                fpspread.Visible = false;
                lblerr1.Visible = true;
                lblerr1.Text = "Please Select Year";
                return;
            }
            if (ddlMonth1.Items.Count == 0)
            {
                btnreset.Visible = false;
                lblaane.Visible = false;

                btnsave1.Visible = false;
                btnprintt.Visible = false;
                fpspread.Visible = false;
                lblerr1.Visible = true;
                lblerr1.Text = "No Exam Month Found";
                return;
            }
            else if (ddlMonth1.SelectedValue.Trim() == "0")
            {
                btnreset.Visible = false;
                lblaane.Visible = false;

                btnsave1.Visible = false;
                btnprintt.Visible = false;
                fpspread.Visible = false;
                lblerr1.Visible = true;
                lblerr1.Text = "Please Select Month";
                return;
            }
            else
            {
                buttongo();
            }

        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    protected void buttongo()
    {
        try
        {
            Session["MarkEntrySave"] = 1;
            int markround = 0;
            string getmarkround = da.GetFunctionv("select value from COE_Master_Settings where settings='Mark Round of'");
            if (getmarkround.Trim() != "" && getmarkround.Trim() != "0")
            {
                int num = 0;
                if (int.TryParse(getmarkround, out num))
                {
                    markround = Convert.ToInt32(getmarkround);
                }
            }

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

            string valuationSettng = "3";

            // valuationSettng = da.GetFunction("select value from Master_Settings where settings='Valuation Settings' " + qryUserOrGroupCode + "");

            clear();
            string passorfail = string.Empty;
            string maxintmark = "", minextmark = "", maxextmark = "", minintmark = "", mintotmark = "", maxtotmark = string.Empty;
            DataTable dtSubSubject = new DataTable();
            if (ddlMonth1.SelectedValue != "" && ddlYear1.SelectedValue != "")//&& ddlSubject.SelectedValue != ""
            {
                string subjectCodeNew = Convert.ToString(ddlSubject.SelectedValue);


                string subCode = string.Empty;
                if (!string.IsNullOrEmpty(Convert.ToString(ddlSubject.SelectedValue)))
                {
                    subCode = "and s.subject_code='" + Convert.ToString(subjectCodeNew) + "'";
                }

                #region Dummy Number Display
                byte dummyNumberMode = getDummyNumberMode();//0-serial , 1-random
                string dummyNumberType = string.Empty;

                if (DummyNumberType() == 1)
                {
                    dummyNumberType = " and subject='" + subjectCodeNew + "' ";
                }
                else
                {
                    dummyNumberType = " and isnull(subject,'')='' ";
                }
                string selDummyQ = string.Empty;
                selDummyQ = "select dummy_no,regno,roll_no from dummynumber where exam_month='" + ddlMonth1.SelectedValue.ToString() + "' and exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and DNCollegeCode='" + CollegeCode + "' and degreecode='" + ddlbranch1.SelectedValue + "' " + dummyNumberType + "  and dummy_type='" + dummyNumberMode + "' --  and semester='" + ddlsem1.SelectedValue.ToString() + "' and exam_date='11/01/2016' ";


                DataTable dtMappedNumbers = dirAccess.selectDataTable(selDummyQ);
                bool showDummyNumber = ShowDummyNumber();
                if (showDummyNumber)
                {
                    if (dtMappedNumbers.Rows.Count == 0)
                    {
                        lblAlertMsg.Visible = true;
                        lblAlertMsg.Text = "No Dummy Numbers Generated";
                        divPopAlert.Visible = true;
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Dummy Numbers Generated')", true);
                        return;
                    }
                }
                #endregion

                fpspread.Width = 520;
                fpspread.Visible = false;
                fpspread.Sheets[0].RowCount = 0;
                fpspread.Sheets[0].ColumnCount = 0;
                fpspread.Sheets[0].ColumnCount = 12;
                string regularNewRaja = @"^(AB)?$|^(Ab)?$|^(aB)?$|^(ab)?$|^(a)?$|^(A)?$|^(M)?$|^(m)?$|^(lt)?$|^(LT)?$|^(Lt)?$|^(lT)?$|^(w)?$|^(W)?$";
                string regexpree = "AB|ab|a|A|M|m|LT|lt|w|W|00|01|02|03|04|05|06|07|08|09|";
                string newExapressionRaja = string.Empty;
                string roundValuesRaja = string.Empty;
                FarPoint.Web.Spread.RegExpCellType rgex = new FarPoint.Web.Spread.RegExpCellType();

                fpspread.Sheets[0].RowHeader.Visible = false;
                FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
                MyStyle.Font.Size = FontUnit.Medium;
                MyStyle.Font.Name = "Book Antiqua";
                MyStyle.Font.Bold = true;
                MyStyle.HorizontalAlign = HorizontalAlign.Center;
                MyStyle.ForeColor = Color.Black;
                MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                fpspread.Sheets[0].DefaultStyle.Font.Bold = false;
                fpspread.Sheets[0].ColumnHeader.RowCount = 3;
                fpspread.Sheets[0].AutoPostBack = false;
                fpspread.CommandBar.Visible = false;

                double minicamoderation = 0;
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

                fpspread.Sheets[0].ColumnHeader.Columns[0].Width = 70;
                fpspread.Sheets[0].ColumnHeader.Columns[1].Width = 100;
                fpspread.Sheets[0].ColumnHeader.Columns[2].Width = 200;
                fpspread.Sheets[0].ColumnHeader.Columns[3].Width = 80;
                fpspread.Sheets[0].ColumnHeader.Columns[4].Width = 80;
                fpspread.Sheets[0].ColumnHeader.Columns[5].Width = 80;
                fpspread.Sheets[0].ColumnHeader.Columns[6].Width = 80;
                fpspread.Sheets[0].ColumnHeader.Columns[7].Width = 80;
                fpspread.Sheets[0].ColumnHeader.Columns[8].Width = 80;
                fpspread.Sheets[0].ColumnHeader.Columns[9].Width = 80;
                fpspread.Sheets[0].ColumnHeader.Columns[10].Width = 80;
                fpspread.Sheets[0].ColumnHeader.Columns[11].Width = 80;

                fpspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
               
                fpspread.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;

                fpspread.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[11].HorizontalAlign = HorizontalAlign.Center;

                fpspread.Sheets[0].ColumnHeader.Columns[0].Visible = false;
                fpspread.Sheets[0].ColumnHeader.Columns[1].Visible = false;
                fpspread.Sheets[0].ColumnHeader.Columns[2].Visible = false;
                fpspread.Sheets[0].ColumnHeader.Columns[3].Visible = false;
                fpspread.Sheets[0].ColumnHeader.Columns[4].Visible = false;
                fpspread.Sheets[0].ColumnHeader.Columns[5].Visible = false;
                fpspread.Sheets[0].ColumnHeader.Columns[6].Visible = false;
                fpspread.Sheets[0].ColumnHeader.Columns[7].Visible = false;
                fpspread.Sheets[0].ColumnHeader.Columns[8].Visible = false;
                fpspread.Sheets[0].ColumnHeader.Columns[9].Visible = false;
                fpspread.Sheets[0].ColumnHeader.Columns[10].Visible = false;
                fpspread.Sheets[0].ColumnHeader.Columns[11].Visible = false;

                string degreeval = string.Empty;
                string degreevalregmoder = string.Empty;
                string degreevalttab = string.Empty;
                string degreevalregis = string.Empty;
                string degCode = string.Empty;
                if (!string.IsNullOrEmpty(Convert.ToString(ddlbranch1.SelectedValue)) && Convert.ToString(ddlbranch1.SelectedValue) != "0")
                {
                    degreeval = " and ed.degree_code='" + ddlbranch1.SelectedValue + "'";
                    degreevalregmoder = " and M.degree_code='" + ddlbranch1.SelectedValue + "'";
                    degreevalttab = " and e.degree_code='" + ddlbranch1.SelectedValue + "'";
                    degreevalregis = " and r.degree_code='" + ddlbranch1.SelectedValue + "'";
                    degCode = "   and  DegreeCode='" + ddlbranch1.SelectedValue + "'";
                }

                string buldleNo = string.Empty;
                string qeryss = string.Empty;
                DataSet dsNewInternal = new DataSet();
                dsNewInternal = da.select_method_wo_parameter("select sc.roll_no,s.subject_no,s.subject_code,total,actual_total,ca.Exam_Year,ca.Exam_Month from camarks ca,subject s,subjectChooser sc,Exam_Details ed,Registration r where r.Roll_No=sc.roll_no  and ca.roll_no=r.Roll_No and ca.roll_no=sc.roll_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and sc.subject_no=s.subject_no and s.subject_no=ca.subject_no and ca.subject_no=sc.subject_no and s.subject_code='" + subjectCodeNew + "' and ed.Exam_Month='" + Convert.ToString(ddlMonth1.SelectedValue).Trim() + "' and ed.Exam_year='" + Convert.ToString(ddlYear1.SelectedItem.Text).Trim() + "' " + degreeval + "  and isnull(r.Reg_No,'') <>'' order by r.Reg_No", "Text");



                qeryss = "SELECT ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,ead.subject_no,s.subject_name,subject_code,r.app_no,r.Roll_No,r.Reg_No,r.Stud_Name,ead.attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'REGULAR' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,Registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and r.Roll_No=ea.roll_no " + degreeval + " and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and s.subject_code='" + subjectCodeNew + "' and isnull(r.Reg_No,'') <>'' ";

                qeryss = qeryss + " union SELECT ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,s.subject_no,s.subject_name,subject_code,r.app_no,r.Roll_No,r.Reg_No,r.Stud_Name,'' attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'NR' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag FROM Exam_Details ED,tbl_not_registred nt,subject s,Registration r,subjectChooser sc where ed.Exam_Month=nt.exam_month and ed.Exam_year=nt.exam_year and s.subject_no=nt.subject_no and r.Roll_No=nt.roll_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and sc.semester=ed.current_semester " + degreeval + " and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and s.subject_code='" + subjectCodeNew + "' and r.cc=0 and isnull(r.Reg_No,'') <>'' ";
                qeryss = qeryss + " union SELECT ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,s.subject_no,s.subject_name,subject_code,r.app_no,r.Roll_No,r.Reg_No,r.Stud_Name,'' attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'NE' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag FROM Exam_Details ED,studentsemestersubjectdebar nt,subject s,Registration r where r.Roll_No=nt.roll_no and nt.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and (ed.current_semester=nt.semester or r.CC=1) " + degreeval + " and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and s.subject_code='" + subjectCodeNew + "' and isnull(r.Reg_No,'') <>'' and r.cc=0 order by ed.batch_year desc,ed.degree_code,ed.current_semester,r.reg_no,sts";


                qeryss = qeryss + " select roll_no,regno,Course_Name,Dept_Name,dummy_no from  dummynumber du,Degree d,Department dt,Course c,subject s where du.degreecode =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and exam_month ='" + ddlMonth1.SelectedValue.ToString() + "' and exam_year ='" + ddlYear1.SelectedItem.Text.ToString() + "'  and s.subject_no=du.subject_no and s.subject_code='" + subjectCodeNew + "' and  (dummy_type ='1' or dummy_type ='0')";

                DataSet ds = da.select_method_wo_parameter(qeryss, "text");

                //string strinternammark = "select m.roll_no,r.Reg_No,m.internal_mark,m.exam_code,ED.Exam_year,ED.Exam_Month,(ed.Exam_year*12+ed.Exam_Month) EXAMYEARMONTHVAL from mark_entry m,subject s,Registration r,Exam_Details ed where m.roll_no=r.Roll_No and m.subject_no=s.subject_no and ed.exam_code=m.exam_code and (ed.Exam_year*12+ed.Exam_Month)<=('" + ddlYear1.SelectedValue.ToString() + "'*12+'" + ddlMonth1.SelectedValue.ToString() + "') AND s.subject_code='" + subjectCodeNew + "' " + degreevalregis + " and m.internal_mark is not null order by m.roll_no,EXAMYEARMONTHVAL DESC,ed.Exam_year,ED.Exam_Month desc";
                //DataSet dsinternal = da.select_method_wo_parameter(strinternammark, "Text");
                string subSubject = string.Empty;
                string batch = string.Empty;
                if (!string.IsNullOrEmpty(Convert.ToString(ddlSubSub.SelectedItem.Text)) && ddlSubSub.SelectedIndex!=0)
                    subSubject = "  and es.SubPart='" + Convert.ToString(ddlSubSub.SelectedItem.Text) + "'";
                if (!string.IsNullOrEmpty(Convert.ToString(ddlBatch.SelectedValue)) && ddlBatch.SelectedIndex!=0)
                    batch = "  and Batch='" + Convert.ToString(ddlBatch.SelectedValue) + "'";

                string SelectSub = "SELECT es.SubPart,es.maxmark FROM COESubSubjectPartSettings ES,COESubSubjectPartMater Em WHERE em.id=es.id and em.ExamMonth='" + ddlMonth1.SelectedValue.ToString() + "' and em.ExamYear='" + ddlYear1.SelectedItem.Text.ToString() + "' and es.SubCode='" + subjectCodeNew + "'   " + degCode + " ";//" + subSubject + "
                string batchStudent = "select e.AppNo,s.subject_code,e.SubNo,e.SubSubjectID,e.Batch from examtheorybatch e,Exam_Details ed,subject s where s.subject_no=e.SubNo and  ed.exam_code=e.ExamCode and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and s.subject_code='" + subjectCodeNew + "'";
                DataTable dtBatchStudent = dirAccess.selectDataTable(batchStudent);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    dtSubSubject = dirAccess.selectDataTable(SelectSub);
                    DataRow drRow = null;
                    if (dtSubSubject.Rows.Count == 0)
                    {
                        drRow = dtSubSubject.NewRow();
                        drRow["SubPart"] = subjectCodeNew;
                        drRow["maxmark"] = Convert.ToDouble(ds.Tables[0].Rows[0]["max_ext_marks"]);
                        dtSubSubject.Rows.Add(drRow);
                    }

                    fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 4, 1);
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Reg No";
                    if (showDummyNumber) fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Dummy No";
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 4, 1);
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Name";
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 4, 1);

                    fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = subjectCodeNew;
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 1, 4);
                    if (dtSubSubject.Rows.Count > 0 && dtSubSubject.Rows.Count <= 4)
                    {
                        regularNewRaja = @"^(AB)?$|^(Ab)?$|^(aB)?$|^(ab)?$|^(a)?$|^(A)?$|^(M)?$|^(m)?$|^(lt)?$|^(LT)?$|^(Lt)?$|^(lT)?$|^(nr)?$|^(Nr)?$|^(nR)?$|^(NR)?$|^(NE)?$|^(nE)?$|^(Ne)?$|^(ne)?$|^(RA)?$|^(rA)?$|^(Ra)?$|^(ra)?$|^(w)?$|^(W)?$";
                        regexpree = "AB|ab||NR|nr|NE|ne|ra||RA|a|A|M|m|LT|lt|w|W|00|01|02|03|04|05|06|07|08|09|";
                        newExapressionRaja = string.Empty;

                        int i = 0;
                        foreach (DataRow dr in dtSubSubject.Rows)
                        {
                            i++;
                            string ParName = Convert.ToString(dr["subPart"]);
                            string MaxMark = Convert.ToString(dr["maxMark"]);
                            if (!string.IsNullOrEmpty(ParName) && i == 1)
                            {
                                fpspread.Sheets[0].ColumnHeader.Cells[1, 3].Text = ParName;
                                fpspread.Sheets[0].ColumnHeader.Cells[2, 3].Text = "Max : " + MaxMark;
                                if (!string.IsNullOrEmpty(MaxMark))
                                {
                                    fpspread.Sheets[0].ColumnHeader.Cells[2, 3].Tag = Convert.ToDouble(MaxMark);

                                    for (int j = 0; j <= Convert.ToInt32(MaxMark); j++)
                                    {
                                        regexpree = regexpree + "|" + "" + j + "";
                                        if (j != Convert.ToInt32(MaxMark))
                                        {
                                            newExapressionRaja += @"|" + "^(" + j + ")(\\.[0-9]{" + roundValuesRaja + "})?$";
                                            for (int d = 0; d < 100; d++)
                                            {
                                                regexpree = regexpree + "|" + "" + j + "." + d;
                                            }
                                        }
                                        else
                                        {
                                            newExapressionRaja += @"|" + "^(" + j + ")(\\.[0]{" + roundValuesRaja + "})?$";
                                        }
                                    }
                                    rgex.ValidationExpression = regularNewRaja + newExapressionRaja;
                                    rgex.ErrorMessage = "Please Enter the Mark Less Than or Equal to (" + MaxMark + ")";
                                    fpspread.Sheets[0].Columns[3].CellType = rgex;
                                }
                            }
                            if (!string.IsNullOrEmpty(ParName) && i == 2)
                            {
                                fpspread.Sheets[0].ColumnHeader.Cells[1, 4].Text = ParName;
                                fpspread.Sheets[0].ColumnHeader.Cells[2, 4].Text = "Max : " + MaxMark;
                                if (!string.IsNullOrEmpty(MaxMark))
                                {
                                    fpspread.Sheets[0].ColumnHeader.Cells[2, 4].Tag = Convert.ToDouble(MaxMark);
                                    for (int j = 0; j <= Convert.ToInt32(MaxMark); j++)
                                    {
                                        regexpree = regexpree + "|" + "" + j + "";
                                        if (j != Convert.ToInt32(MaxMark))
                                        {
                                            newExapressionRaja += @"|" + "^(" + j + ")(\\.[0-9]{" + roundValuesRaja + "})?$";
                                            for (int d = 0; d < 100; d++)
                                            {
                                                regexpree = regexpree + "|" + "" + j + "." + d;
                                            }
                                        }
                                        else
                                        {
                                            newExapressionRaja += @"|" + "^(" + j + ")(\\.[0]{" + roundValuesRaja + "})?$";
                                        }
                                    }
                                    rgex.ValidationExpression = regularNewRaja + newExapressionRaja;
                                    rgex.ErrorMessage = "Please Enter the Mark Less Than or Equal to (" + MaxMark + ")";
                                    fpspread.Sheets[0].Columns[4].CellType = rgex;
                                }
                            }
                            if (!string.IsNullOrEmpty(ParName) && i == 3)
                            {
                                fpspread.Sheets[0].ColumnHeader.Cells[1, 5].Text = ParName;
                                fpspread.Sheets[0].ColumnHeader.Cells[2, 5].Text = "Max : " + MaxMark;
                                if (!string.IsNullOrEmpty(MaxMark))
                                {
                                    fpspread.Sheets[0].ColumnHeader.Cells[2, 5].Tag = Convert.ToDouble(MaxMark);
                                    for (int j = 0; j <= Convert.ToInt32(MaxMark); j++)
                                    {
                                        regexpree = regexpree + "|" + "" + j + "";
                                        if (j != Convert.ToInt32(MaxMark))
                                        {
                                            newExapressionRaja += @"|" + "^(" + j + ")(\\.[0-9]{" + roundValuesRaja + "})?$";
                                            for (int d = 0; d < 100; d++)
                                            {
                                                regexpree = regexpree + "|" + "" + j + "." + d;
                                            }
                                        }
                                        else
                                        {
                                            newExapressionRaja += @"|" + "^(" + j + ")(\\.[0]{" + roundValuesRaja + "})?$";
                                        }
                                    }
                                    rgex.ValidationExpression = regularNewRaja + newExapressionRaja;
                                    rgex.ErrorMessage = "Please Enter the Mark Less Than or Equal to (" + MaxMark + ")";
                                    fpspread.Sheets[0].Columns[5].CellType = rgex;
                                }
                            }
                            if (!string.IsNullOrEmpty(ParName) && i == 4)
                            {
                                fpspread.Sheets[0].ColumnHeader.Cells[1, 6].Text = ParName;
                                fpspread.Sheets[0].ColumnHeader.Cells[2, 6].Text = "Max : " + MaxMark;
                                if (!string.IsNullOrEmpty(MaxMark))
                                {
                                    fpspread.Sheets[0].ColumnHeader.Cells[2, 6].Tag = Convert.ToDouble(MaxMark);
                                    for (int j = 0; j <= Convert.ToInt32(MaxMark); j++)
                                    {
                                        regexpree = regexpree + "|" + "" + j + "";
                                        if (j != Convert.ToInt32(MaxMark))
                                        {
                                            newExapressionRaja += @"|" + "^(" + j + ")(\\.[0-9]{" + roundValuesRaja + "})?$";
                                            for (int d = 0; d < 100; d++)
                                            {
                                                regexpree = regexpree + "|" + "" + j + "." + d;
                                            }
                                        }
                                        else
                                        {
                                            newExapressionRaja += @"|" + "^(" + j + ")(\\.[0]{" + roundValuesRaja + "})?$";
                                        }
                                    }
                                    rgex.ValidationExpression = regularNewRaja + newExapressionRaja;
                                    rgex.ErrorMessage = "Please Enter the Mark Less Than or Equal to (" + MaxMark + ")";
                                    fpspread.Sheets[0].Columns[6].CellType = rgex;
                                }
                            }
                        }
                    }

                    fpspread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Sub Total";
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 4, 1);
                    fpspread.Sheets[0].ColumnHeader.Cells[2, 7].Text = "Total";

                    fpspread.Sheets[0].ColumnHeader.Cells[0, 8].Text = "I.C.A";
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);
                    fpspread.Sheets[0].ColumnHeader.Cells[2, 8].Text = "Max";

                    fpspread.Sheets[0].ColumnHeader.Cells[0, 9].Text = "E.S.E";
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);
                    fpspread.Sheets[0].ColumnHeader.Cells[2, 9].Text = "Max";
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Total";
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 10, 2, 1);
                    fpspread.Sheets[0].ColumnHeader.Cells[2, 10].Text = "Max";
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Result";
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 11, 4, 1);
                    fpspread.Sheets[0].Columns[0].Visible = true;

                    if ((Convert.ToString(subjectCodeNew) != "") && !string.IsNullOrEmpty(subjectCodeNew))
                    {
                        
                        string subject_no = subjectCodeNew;
                        string exam_code = ds.Tables[0].Rows[0]["exam_code"].ToString();
                        string sem = ddlsem1.SelectedValue.ToString();
                        string getdetails = "select me.roll_no,me.external_mark,me.internal_mark,me.result,me.evaluation1,me.evaluation2,me.evaluation3,evaluation4,me.external_mark,me.total  from mark_entry me,Exam_Details ed,subject s where me.exam_code=ed.exam_code and me.subject_no=s.subject_no and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.ToString() + "'  " + degreeval + " and s.subject_code='" + subjectCodeNew + "'";
                        getdetails = getdetails + "  select * from moderation m,subject s where m.subject_no=s.subject_no and s.subject_code='" + subjectCodeNew + "' " + degreevalregmoder + " and m.exam_year='" + ddlYear1.SelectedItem.ToString() + "'";
                        getdetails = getdetails + " select convert(varchar(50),exam_date,105) as exam_date,exam_session from exmtt e,exmtt_det et,subject s where e.exam_code=et.exam_code and et.subject_no=s.subject_no and e.Exam_month='" + ddlMonth1.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear1.SelectedItem.ToString() + "' " + degreevalttab + " and s.subject_code='" + subjectCodeNew + "' ";
                        ds2 = da.select_method_wo_parameter(getdetails, "Text");

                        double ev11 = 0;
                        double ev21 = 0;
                        double ev31 = 0;
                        double ev41 = 0;
                        double external_mark = 0;
                        double modermarks = 0;
                        string ev1 = string.Empty;
                        string ev2 = string.Empty;
                        string ev3 = string.Empty;
                        string ev4 = string.Empty;
                        string rollno = string.Empty;
                        string regno = string.Empty;
                        string batchyerr = string.Empty;
                        string resullts = string.Empty;
                        string externn = string.Empty;
                        string intermarkf = string.Empty;
                        string subjectcode = string.Empty;

                        int sno = 1;

                        double papermaxexter = 0;
                        FarPoint.Web.Spread.DoubleCellType intgrcel = new FarPoint.Web.Spread.DoubleCellType();
                        FarPoint.Web.Spread.DoubleCellType intgrcel1 = new FarPoint.Web.Spread.DoubleCellType();


                        double min_int_marks1 = Convert.ToDouble(ds.Tables[0].Rows[0]["min_int_marks"]);
                        min_int_marks1 = Math.Round(min_int_marks1, markround);

                        double mintolmarks1 = Convert.ToDouble(ds.Tables[0].Rows[0]["mintotal"]);
                        mintolmarks1 = Math.Round(mintolmarks1, markround);

                        double min_ext_marks1 = Convert.ToDouble(ds.Tables[0].Rows[0]["min_ext_marks"]);
                        min_ext_marks1 = Math.Round(min_ext_marks1, markround);

                        double max_ext_marks1 = Convert.ToDouble(ds.Tables[0].Rows[0]["max_ext_marks"]);
                        max_ext_marks1 = Math.Round(max_ext_marks1, markround);

                        double max_int_marks1 = Convert.ToDouble(ds.Tables[0].Rows[0]["max_int_marks"]);
                        max_int_marks1 = Math.Round(max_int_marks1, markround);

                        double max_tol_marks1 = Convert.ToDouble(ds.Tables[0].Rows[0]["maxtotal"]);
                        max_tol_marks1 = Math.Round(max_tol_marks1, markround);

                        regularNewRaja = @"^(AB)?$|^(Ab)?$|^(aB)?$|^(ab)?$|^(a)?$|^(A)?$|^(M)?$|^(m)?$|^(lt)?$|^(LT)?$|^(Lt)?$|^(lT)?$|^(w)?$|^(W)?$";
                        regexpree = "AB|ab|a|A|M|m|LT|lt|w|W|00|01|02|03|04|05|06|07|08|09|";
                        newExapressionRaja = string.Empty;
                        roundValuesRaja = string.Empty;

                        if (markround == 0)
                        {
                            roundValuesRaja = "1,2";
                        }
                        for (int round = 1; round <= markround; round++)
                        {
                            if (string.IsNullOrEmpty(roundValuesRaja))
                            {
                                roundValuesRaja = Convert.ToString(round).Trim();
                            }
                            else
                            {
                                roundValuesRaja += "," + Convert.ToString(round).Trim();
                            }
                        }
                        for (int i = 0; i <= max_int_marks1; i++)
                        {
                            newExapressionRaja += "|" + "^(" + i + ")?$";
                            regexpree = regexpree + "|" + "" + i + "";
                            if (i != max_int_marks1)
                            {
                                newExapressionRaja += @"|" + "^(" + i + ")(\\.[0-9]{" + roundValuesRaja + "})?$";
                                for (int d = 0; d < 100; d++)
                                {
                                    regexpree = regexpree + "|" + "" + i + "." + d;
                                }
                            }
                            else
                            {
                                newExapressionRaja += @"|" + "^(" + i + ")(\\.[0]{" + roundValuesRaja + "})?$";
                            }
                        }
                        rgex.ValidationExpression = regularNewRaja + newExapressionRaja;

                        rgex.ErrorMessage = "Please Enter the Mark Less Than or Equal to (" + max_int_marks1 + ")";
                        fpspread.Sheets[0].Columns[8].CellType = rgex;

                        string type = string.Empty;



                        type = da.GetFunction("select edu_level from course where course_id=" + ddldegree1.SelectedValue + "");


                        if (type.Trim() != "" && type.Trim() != "0" && type != null)
                        {
                            string extexammaxmark = ds.Tables[0].Rows[0]["writtenmaxmark"].ToString();
                            //papermaxexter = Convert.ToDouble(extexammaxmark);
                            double.TryParse(extexammaxmark, out papermaxexter);

                            string minicamodeval = "0";

                            minicamodeval = da.GetFunction("select distinct s.min_int_moderation from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.ToString() + "' and s.subject_code='" + subjectCodeNew + "'");

                            if (minicamodeval.Trim() == "" || minicamodeval.Trim() == "0")
                            {
                                minicamodeval = da.GetFunctionv("select value from COE_Master_Settings where settings = 'min Ica Moderation " + type + "'");
                            }
                            if (minicamodeval.Trim() == "")
                            {
                                minicamodeval = "0";
                            }
                            minicamoderation = Convert.ToDouble(minicamodeval);
                        }

                        fpspread.Sheets[0].ColumnHeader.Cells[2, 10].Text = "Max : " + (max_ext_marks1 + max_int_marks1).ToString();
                        fpspread.Sheets[0].ColumnHeader.Cells[2, 9].Text = "Max : " + max_ext_marks1.ToString();
                        fpspread.Sheets[0].ColumnHeader.Cells[2, 8].Text = "Max : " + max_int_marks1.ToString();
                        fpspread.Sheets[0].ColumnHeader.Cells[2, 8].Tag = max_int_marks1.ToString();
                        fpspread.Sheets[0].ColumnHeader.Cells[2, 9].Tag = max_ext_marks1.ToString();
                        fpspread.Sheets[0].ColumnHeader.Cells[2, 10].Tag = (max_ext_marks1 + max_int_marks1).ToString();

                        double passint = Math.Round((min_int_marks1 / max_int_marks1) * 100, markround);
                        double passext = Math.Round((min_ext_marks1 / max_ext_marks1) * 100, markround);

                        string exandate = string.Empty;
                        int height = 50;
                        lblerr1.Visible = false;
                        fpspread.Visible = true;

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
                        {
                            clear();
                            lblerr1.Visible = true;
                            lblerr1.Text = "No Records Found";
                            return;
                        }
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            //chkincluevel2.Visible = true;
                            DataView dnew = new DataView();
                            hat.Clear();
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                rollno = ds.Tables[0].Rows[i]["Roll_No"].ToString();
                                if (!hat.Contains(rollno))
                                {
                                    hat.Add(rollno, rollno);
                                    regno = ds.Tables[0].Rows[i]["Reg_No"].ToString();
                                    batchyerr = ds.Tables[0].Rows[i]["Batch_Year"].ToString();
                                    string StuName = ds.Tables[0].Rows[i]["Stud_Name"].ToString();
                                    string examcode = ds.Tables[0].Rows[i]["exam_code"].ToString();
                                    string subno = ds.Tables[0].Rows[i]["subject_no"].ToString();
                                    string attempts = ds.Tables[0].Rows[i]["attempts"].ToString();
                                    string cursem = ds.Tables[0].Rows[i]["current_semester"].ToString();
                                    string status = ds.Tables[0].Rows[i]["sts"].ToString();
                                    string AppNo = Convert.ToString(ds.Tables[0].Rows[i]["App_no"]);
                                    minintmark = ds.Tables[0].Rows[i]["min_int_marks"].ToString();
                                    maxintmark = ds.Tables[0].Rows[i]["max_int_marks"].ToString();
                                    minextmark = ds.Tables[0].Rows[i]["min_ext_marks"].ToString();
                                    maxextmark = ds.Tables[0].Rows[i]["max_ext_marks"].ToString();
                                    mintotmark = ds.Tables[0].Rows[i]["mintotal"].ToString();
                                    maxtotmark = ds.Tables[0].Rows[i]["maxtotal"].ToString();
                                    string degreecode = ds.Tables[0].Rows[i]["degree_code"].ToString();
                                    string crdeitpoints = ds.Tables[0].Rows[i]["credit_points"].ToString();
                                    string submaoremark = ds.Tables[0].Rows[i]["Moderation_Mark"].ToString();
                                    string minintmodeallow = ds.Tables[0].Rows[i]["min_int_moderation"].ToString();
                                    string dlflag = ds.Tables[0].Rows[i]["delflag"].ToString();

                                    string batchalloc = Convert.ToString(ddlBatch.SelectedValue);
                                    Boolean setflag = false;
                                    if (dtBatchStudent.Rows.Count > 0 && !string.IsNullOrEmpty(batchalloc) && batchalloc!="0") 
                                    {
                                        dtBatchStudent.DefaultView.RowFilter = "appno='" + AppNo + "' and Batch='" + batchalloc + "'";
                                        DataView dvStudent = dtBatchStudent.DefaultView;
                                        if (dvStudent.Count > 0)
                                        {
                                            setflag = false;
                                        }
                                        else
                                            setflag = true;
                                    }

                                    
                                    if (string.IsNullOrEmpty(attempts) || attempts.Trim() == "0")
                                    {
                                        attempts = "1";
                                    }
                                    if (ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0)
                                    {
                                        exandate = ds2.Tables[2].Rows[0]["exam_date"].ToString();
                                    }
                                    else
                                    {
                                        string dummy_date = DateTime.Today.ToString();
                                        string[] dummy_date_split = dummy_date.Split(' ');
                                        string[] final_date_string = dummy_date_split[0].Split('/');
                                        dummy_date = final_date_string[1].ToString() + "-" + final_date_string[0].ToString() + "-" + final_date_string[2].ToString();
                                        exandate = dummy_date;
                                    }
                                    string cc = ds.Tables[0].Rows[i]["cc"].ToString().Trim();

                                    if (setflag == false)
                                    {
                                        fpspread.Sheets[0].RowCount = fpspread.Sheets[0].RowCount + 1;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = sno + "";
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Tag = batchyerr;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Note = status;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Locked = true;

                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].CellType = txt;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Text = regno;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Locked = true;


                                        if (showDummyNumber)
                                        {
                                            dtMappedNumbers.DefaultView.RowFilter = "regno='" + regno + "'";
                                            DataView dvDumNo = dtMappedNumbers.DefaultView;
                                            if (dvDumNo.Count > 0)
                                            {
                                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Text = dvDumNo[0]["dummy_no"].ToString();
                                            }
                                        }


                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Note = rollno;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = StuName;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Tag = examcode;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].CellType = rgex;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Tag = subno;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Note = attempts;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].CellType = rgex;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].CellType = rgex;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].CellType = rgex;

                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Tag = exandate;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 10].Locked = true;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 9].Note = minextmark;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 9].Tag = maxextmark;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 10].Note = mintotmark;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 10].Tag = maxtotmark;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Locked = true;

                                        //Rajkumar
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Note = degreecode;
                                        //fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Locked = true;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Tag = crdeitpoints;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Note = minintmark;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 8].Locked = true;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 8].Tag = maxintmark;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Note = cursem;
                                        //
                                        if (dlflag.Trim().ToLower() == "1" || dlflag.Trim().ToLower() == "true")
                                        {
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Text = "LT";
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Text = "LT";
                                            //fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Text = "LT";
                                        }

                                        sno++;
                                    }
                                }
                            }
                        }
                        fpspread.Height = height + 30;
                        fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;

                        string strinternammark = "select  m.roll_no,r.Reg_No,m.internal_mark,m.exam_code,ED.Exam_year,ED.Exam_Month,(ed.Exam_year*12+ed.Exam_Month) EXAMYEARMONTHVAL from mark_entry m,subject s,Registration r,Exam_Details ed where m.roll_no=r.Roll_No and m.subject_no=s.subject_no and ed.exam_code=m.exam_code and (ed.Exam_year*12+ed.Exam_Month)<=('" + ddlYear1.SelectedValue.ToString() + "'*12+'" + ddlMonth1.SelectedValue.ToString() + "') AND s.subject_code='" + subjectCodeNew + "' " + degreevalregis + " and m.internal_mark is not null order by m.roll_no,EXAMYEARMONTHVAL DESC,ed.Exam_year,ED.Exam_Month desc";
                        DataSet dsinternal = da.select_method_wo_parameter(strinternammark, "Text");
                        double evalmaxmark = Convert.ToDouble(fpspread.Sheets[0].ColumnHeader.Cells[2, 2].Tag);
                        for (int row_cnt = 0; row_cnt < fpspread.Sheets[0].RowCount; row_cnt++)
                        {   //317CO0417
                            int attempts = 0;
                            string roll_noo = fpspread.Sheets[0].Cells[row_cnt, 1].Note.ToString();
                            string roll_noo2 = fpspread.Sheets[0].Cells[row_cnt, 1].Text.ToString();
                            string batchyr = fpspread.Sheets[0].Cells[row_cnt, 1].Tag.ToString();
                            string subjectNo = fpspread.Sheets[0].Cells[row_cnt, 3].Tag.ToString();

                            string previousinternalmark = string.Empty;
                            string strus = fpspread.Sheets[0].Cells[row_cnt, 0].Note.ToString();

                            min_ext_marks1 = Convert.ToDouble(fpspread.Sheets[0].Cells[row_cnt, 9].Note.ToString());
                            max_ext_marks1 = Convert.ToDouble(fpspread.Sheets[0].Cells[row_cnt, 9].Tag.ToString());
                            mintolmarks1 = Convert.ToDouble(fpspread.Sheets[0].Cells[row_cnt, 10].Note.ToString());

                            if (roll_noo2 != "" && roll_noo != "")
                            {
                                ds2.Tables[0].DefaultView.RowFilter = "roll_no='" + roll_noo + "'";
                                dv1 = ds2.Tables[0].DefaultView;
                                DataView dvintmark = new DataView();

                                if (dsinternal.Tables.Count > 0 && dsinternal.Tables[0].Rows.Count > 0)
                                {
                                    dsinternal.Tables[0].DefaultView.RowFilter = "roll_no='" + roll_noo + "'";
                                    dvintmark = dsinternal.Tables[0].DefaultView;
                                }

                                int monthval = (Convert.ToInt32(ddlYear1.SelectedValue.ToString()) * 12) + Convert.ToInt32(ddlMonth1.SelectedValue.ToString());
                                DataView dvattempts = new DataView();
                                if (dsinternal.Tables.Count > 0 && dsinternal.Tables[0].Rows.Count > 0)
                                {
                                    dsinternal.Tables[0].DefaultView.RowFilter = "roll_no='" + roll_noo + "' and EXAMYEARMONTHVAL<" + monthval + "";
                                    dvattempts = dsinternal.Tables[0].DefaultView;
                                }
                                if (dvattempts.Count > 0)
                                    attempts = dvattempts.Count + 1;

                                fpspread.Sheets[0].Cells[row_cnt, 3].Note = attempts.ToString();
                                DataView dvNewInternal = new DataView();
                                if (dsNewInternal.Tables.Count > 0 && dsNewInternal.Tables[0].Rows.Count > 0)
                                {
                                    dsNewInternal.Tables[0].DefaultView.RowFilter = "roll_no='" + roll_noo + "' and subject_no='" + subjectNo + "'";
                                    dvNewInternal = dsNewInternal.Tables[0].DefaultView;
                                }

                                fpspread.Sheets[0].Cells[row_cnt, 3].Note = attempts.ToString();
                                if (dvNewInternal.Count > 0)
                                {
                                    previousinternalmark = Convert.ToString(dvNewInternal[0]["total"]).Trim();
                                    if (previousinternalmark.Trim() != "" && previousinternalmark != null)
                                    {
                                        if (previousinternalmark.Trim() != "-1" && previousinternalmark.Trim() != "0")
                                        {
                                            double setmark = 0;// Convert.ToDouble(previousinternalmark);
                                            double.TryParse(previousinternalmark.Trim(), out setmark);
                                            setmark = Math.Round(setmark, markround, MidpointRounding.AwayFromZero);
                                            previousinternalmark = setmark.ToString();
                                        }
                                        else
                                        {
                                            previousinternalmark = "A";
                                        }
                                    }
                                }
                                if (dvintmark.Count > 0)
                                {
                                    previousinternalmark = dvintmark[0]["internal_mark"].ToString();
                                    if (previousinternalmark.Trim() != "" && previousinternalmark != null)
                                    {
                                        if (previousinternalmark.Trim() != "-1" && previousinternalmark.Trim() != "0")
                                        {
                                            double setmark = Convert.ToDouble(previousinternalmark);
                                            setmark = Math.Round(setmark, markround, MidpointRounding.AwayFromZero);
                                            previousinternalmark = setmark.ToString();
                                        }
                                        else
                                        {
                                            previousinternalmark = "A";
                                        }
                                    }
                                }
                                if (dv1.Count > 0)
                                {
                                    btnreset.Visible = true;

                                    ev1 = dv1[0]["evaluation1"].ToString();
                                    ev2 = dv1[0]["evaluation2"].ToString();
                                    ev3 = dv1[0]["evaluation3"].ToString();
                                    ev4 = dv1[0]["evaluation4"].ToString();
                                    subjectcode = ddlSubject.SelectedValue.ToString();
                                    intermarkf = dv1[0]["internal_mark"].ToString();
                                    resullts = dv1[0]["result"].ToString();

                                    if (string.IsNullOrEmpty(intermarkf))
                                        intermarkf = previousinternalmark;

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
                                            previousinternalmark = "AB";
                                            if (resullts.ToString().Trim().ToLower() == "whd")
                                            {
                                                ev1 = "M";
                                                ev2 = "M";
                                                ev3 = "M";
                                                ev4 = "M";
                                            }
                                            else
                                            {
                                                resullts = "NC";
                                            }
                                        }
                                        if (intermarkf223.ToString().Trim() == "-19")
                                        {
                                            intermarkforab = "W";
                                            previousinternalmark = "E";
                                            if (resullts.ToString().Trim().ToLower() == "W")
                                            {
                                                ev1 = "W";
                                                ev2 = "W";
                                                ev3 = "W";
                                                ev4 = "W";
                                            }
                                            else
                                            {
                                                resullts = "W";
                                            }
                                        }
                                        fpspread.Sheets[0].Cells[row_cnt, 8].Text = intermarkforab;
                                        previousinternalmark = intermarkforab;
                                        if (intermarkforab.Trim().ToLower().Contains('a'))
                                        {
                                            intermarkforab = "-1";
                                        }
                                    }

                                    double abse1 = 0;
                                    if (ev1.Trim() != "" && ev1.Trim() != null && ev1.Trim().ToLower() != "m")
                                    {
                                        abse1 = Convert.ToDouble(ev1); ;
                                    }

                                    if (ev1.Trim() != "" || ev2.Trim() != "" || intermarkf.Trim() != "")
                                    {
                                        Session["MarkEntrySave"] = 2;
                                    }
                                    double abse2 = 0;
                                    double abse3 = 0;
                                    double abse4 = 0;
                                    string total = dv1[0]["total"].ToString();

                                    if (intermarkf == "" && resullts.Trim().ToLower() != "whd")
                                    {
                                        resullts = string.Empty;
                                        total = string.Empty;
                                    }

                                    if (ev2.Trim() != "" && ev2.Trim() != null && ev2.Trim().ToLower() != "m")
                                    {
                                        abse2 = Convert.ToDouble(ev2);

                                    }
                                    if (ev1.Trim() == "" && strus.Trim().ToUpper() != "REGULAR")
                                    {
                                        if (strus == "NR")
                                        {
                                            abse1 = -3;
                                            abse2 = -3;
                                        }
                                        else
                                        {
                                            abse1 = -2;
                                            abse2 = -2;
                                        }
                                    }
                                    if (resullts.Trim().ToLower() != "whd")
                                    {
                                        if (abse1 != -4 && abse2 != -4 && abse4 != -4 && abse3 != -4)
                                        {
                                            if (abse1 != -3 && abse2 != -3 && abse4 != -3 && abse3 != -3)
                                            {
                                                if (abse1 != -2 && abse2 != -2 && abse4 != -2 && abse3 != -2)
                                                {
                                                    if (abse1 != -1 && abse2 != -1 && abse4 != -1 && abse3 != -1)
                                                    {
                                                        if (ev1 == "-1")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 3].Text = "AAA";
                                                        }
                                                        else if (ev1 == "-2")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 3].Text = "NE";
                                                        }
                                                        else if (ev1 == "-3")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 3].Text = "RA";
                                                        }
                                                        else if (ev1 == "-19")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 3].Text = "W";
                                                        }
                                                        else if (ev1.Trim() != "" && ev1.Trim() != "0")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 3].Text = ev1;
                                                        }
                                                        else
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 3].Text = string.Empty;
                                                        }

                                                        if (ev2 == "-1")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 4].Text = "AAA";
                                                        }
                                                        else if (ev2 == "-2")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 4].Text = "NE";
                                                        }
                                                        else if (ev2 == "-3")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 4].Text = "RA";
                                                        }
                                                        else if (ev2 == "-19")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 4].Text = "W";
                                                        }
                                                        else if (ev2.Trim() != "" && ev2.Trim() != "0")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 4].Text = ev2;
                                                        }
                                                        else
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 4].Text = string.Empty;
                                                        }


                                                        if (ev3 == "-1")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 5].Text = "AAA";
                                                        }
                                                        else if (ev3 == "-2")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 5].Text = "NE";
                                                        }
                                                        else if (ev3 == "-3")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 5].Text = "RA";
                                                        }
                                                        else if (ev3 == "-19")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 5].Text = "W";
                                                        }
                                                        else if (ev3.Trim() != "" && ev3.Trim() != "0")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 5].Text = ev3;
                                                        }
                                                        else
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 5].Text = string.Empty;
                                                        }

                                                        if (ev4 == "-1")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 6].Text = "AAA";
                                                        }
                                                        else if (ev4 == "-2")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 6].Text = "NE";
                                                        }
                                                        else if (ev4 == "-3")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 6].Text = "RA";
                                                        }
                                                        else if (ev4 == "-19")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 6].Text = "W";
                                                        }
                                                        else if (ev4.Trim() != "" && ev4.Trim() != "0")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 6].Text = ev4;
                                                        }
                                                        else
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 6].Text = string.Empty;
                                                        }

                                                        string avemark = string.Empty;

                                                        if (ev2 != "NE" && ev1 != "NE")
                                                        {
                                                            ds2.Tables[1].DefaultView.RowFilter = "roll_no='" + roll_noo + "'";
                                                            dv3 = ds2.Tables[1].DefaultView;
                                                            if (dv3.Count > 0)
                                                            {
                                                                modermarks = Convert.ToDouble(dv3[0]["passmark"].ToString());
                                                                modermarks = Math.Round(modermarks, markround, MidpointRounding.AwayFromZero);
                                                                //fpspread.Sheets[0].Cells[row_cnt, 6].Text = modermarks.ToString();

                                                                externn = dv3[0]["af_moderation_extmrk"].ToString();

                                                                external_mark = Convert.ToDouble(externn.ToString());
                                                                external_mark = Math.Round(external_mark, markround, MidpointRounding.AwayFromZero);
                                                                externn = dv1[0]["external_mark"].ToString();

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
                                                                else if (externn == "-19")
                                                                {
                                                                    externn = "W";
                                                                    resullts = "W";
                                                                    total = "W";
                                                                }
                                                                else if (externn == "")
                                                                {
                                                                    externn = string.Empty;
                                                                    resullts = string.Empty;
                                                                }
                                                                else
                                                                {
                                                                    double getexmark = Convert.ToDouble(externn);
                                                                    getexmark = Math.Round(getexmark, markround, MidpointRounding.AwayFromZero);
                                                                    externn = Convert.ToString(externn);
                                                                    external_mark = Convert.ToDouble(externn.ToString());
                                                                    external_mark = Math.Round(external_mark, markround, MidpointRounding.AwayFromZero);
                                                                }
                                                                //fpspread.Sheets[0].Cells[row_cnt, 4].Text = external_mark.ToString();
                                                                if(externn!="0")
                                                                    fpspread.Sheets[0].Cells[row_cnt, 9].Text = Convert.ToString(externn);
                                                               
                                                            }
                                                            else
                                                            {
                                                                externn = dv1[0]["external_mark"].ToString();
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
                                                                else if (externn == "-19")
                                                                {
                                                                    externn = "W";
                                                                    resullts = "W";
                                                                    total = "W";
                                                                }
                                                                else if (externn == "")
                                                                {
                                                                    externn = string.Empty;
                                                                    resullts = string.Empty;
                                                                }
                                                                else
                                                                {
                                                                    double getexmark = Convert.ToDouble(externn);
                                                                    getexmark = Math.Round(getexmark, markround, MidpointRounding.AwayFromZero);
                                                                    externn = Convert.ToString(getexmark);//externn
                                                                    external_mark = Convert.ToDouble(externn.ToString());
                                                                    //external_mark = Math.Round(external_mark, markround, MidpointRounding.AwayFromZero);
                                                                    external_mark = Math.Round(external_mark, markround, MidpointRounding.AwayFromZero);
                                                                }

                                                                //fpspread.Sheets[0].Cells[row_cnt, 4].Text = external_mark.ToString();
                                                                if (externn!="0")
                                                                    fpspread.Sheets[0].Cells[row_cnt, 9].Text = Convert.ToString(externn);

                                                                //double e1 = 0; double e2 = 0; double e3 = 0; double e4 = 0;
                                                                //double.TryParse(ev1, out e1);
                                                                //double.TryParse(ev2, out e2);
                                                                //double.TryParse(ev3, out e3);
                                                                //double.TryParse(ev4, out e4);
                                                              
                                                            }
                                                            if (externn.Trim() != "")
                                                            {
                                                                if(total!="0")
                                                                    fpspread.Sheets[0].Cells[row_cnt, 10].Text = total;

                                                                fpspread.Sheets[0].Cells[row_cnt, 11].Text = resullts;
                                                            }
                                                            double.TryParse(ev1, out ev11);
                                                            double.TryParse(ev2, out ev21);
                                                            double.TryParse(ev3, out ev31);
                                                            double.TryParse(ev4, out ev41);
                                                            double bindav = (ev11 + ev21 + ev31 + ev41);

                                                            bindav = Math.Round(bindav, 0, MidpointRounding.AwayFromZero);
                                                            avemark = bindav.ToString();
                                                            if (bindav != 0)
                                                                fpspread.Sheets[0].Cells[row_cnt, 7].Text = Convert.ToString(avemark);

                                                            if (intermarkf223 == -1 || intermarkf223 == -2 || intermarkf223 == -3 || intermarkf223 == -19)
                                                            {
                                                                intermarkf = "0";
                                                                if (intermarkf223 == -1)
                                                                {
                                                                    fpspread.Sheets[0].Cells[row_cnt, 8].Text = "AB";
                                                                }
                                                                else if (intermarkf223 == -19)
                                                                {
                                                                    fpspread.Sheets[0].Cells[row_cnt, 8].Text = "W";
                                                                }
                                                                else
                                                                {
                                                                    fpspread.Sheets[0].Cells[row_cnt, 8].Text = "0";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                fpspread.Sheets[0].Cells[row_cnt, 8].Text = intermarkf;
                                                            }
                                                        }
                                                        else
                                                        {

                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (previousinternalmark == "-1")
                                                        {
                                                            previousinternalmark = "AB";
                                                        }
                                                        fpspread.Sheets[0].Cells[row_cnt, 3].Text = "A";
                                                        fpspread.Sheets[0].Cells[row_cnt, 4].Text = "A";
                                                        fpspread.Sheets[0].Cells[row_cnt, 5].Text = "A";
                                                        fpspread.Sheets[0].Cells[row_cnt, 6].Text = "A";

                                                        fpspread.Sheets[0].Cells[row_cnt, 8].Text = previousinternalmark;
                                                        fpspread.Sheets[0].Cells[row_cnt, 9].Text = "A";
                                                        fpspread.Sheets[0].Cells[row_cnt, 10].Text = "NC";
                                                        fpspread.Sheets[0].Cells[row_cnt, 11].Text = "NC";
                                                        fpspread.Sheets[0].Cells[row_cnt, 11].Note = passorfail.ToString();
                                                    }
                                                }
                                                else
                                                {
                                                    if (previousinternalmark == "-1")
                                                    {
                                                        previousinternalmark = "AB";
                                                    }
                                                    fpspread.Sheets[0].Cells[row_cnt, 3].Text = "NE";
                                                    fpspread.Sheets[0].Cells[row_cnt, 4].Text = "NE";
                                                    fpspread.Sheets[0].Cells[row_cnt, 5].Text = "NE";
                                                    fpspread.Sheets[0].Cells[row_cnt, 6].Text = "NE";
                                                    fpspread.Sheets[0].Cells[row_cnt, 8].Text = previousinternalmark;
                                                    fpspread.Sheets[0].Cells[row_cnt, 9].Text = "NE";
                                                    fpspread.Sheets[0].Cells[row_cnt, 10].Text = "NC";
                                                    fpspread.Sheets[0].Cells[row_cnt, 11].Text = "NC";
                                                    fpspread.Sheets[0].Cells[row_cnt, 11].Note = passorfail.ToString();
                                                }
                                            }
                                            else
                                            {
                                                if (previousinternalmark == "-1")
                                                {
                                                    previousinternalmark = "AB";
                                                }
                                                fpspread.Sheets[0].Cells[row_cnt, 3].Text = "NR";
                                                fpspread.Sheets[0].Cells[row_cnt, 4].Text = "NR";
                                                fpspread.Sheets[0].Cells[row_cnt, 5].Text = "NR";
                                                fpspread.Sheets[0].Cells[row_cnt, 6].Text = "NR";
                                                fpspread.Sheets[0].Cells[row_cnt, 8].Text = previousinternalmark;
                                                fpspread.Sheets[0].Cells[row_cnt, 9].Text = "NR";
                                                fpspread.Sheets[0].Cells[row_cnt, 10].Text = "NC";
                                                fpspread.Sheets[0].Cells[row_cnt, 11].Text = "NC";
                                                fpspread.Sheets[0].Cells[row_cnt, 11].Note = passorfail.ToString();
                                            }
                                        }
                                        else
                                        {
                                            if (previousinternalmark == "-1")
                                            {
                                                previousinternalmark = "AB";
                                            }
                                            fpspread.Sheets[0].Cells[row_cnt, 3].Text = "LT";
                                            fpspread.Sheets[0].Cells[row_cnt, 4].Text = "LT";
                                            fpspread.Sheets[0].Cells[row_cnt, 5].Text = "LT";
                                            fpspread.Sheets[0].Cells[row_cnt, 6].Text = "LT";
                                            fpspread.Sheets[0].Cells[row_cnt, 8].Text = previousinternalmark;
                                            fpspread.Sheets[0].Cells[row_cnt, 9].Text = "LT";
                                            fpspread.Sheets[0].Cells[row_cnt, 10].Text = "NC";
                                            fpspread.Sheets[0].Cells[row_cnt, 11].Text = "NC";
                                            fpspread.Sheets[0].Cells[row_cnt, 11].Note = passorfail.ToString();
                                        }
                                    }
                                    else
                                    {
                                        if (previousinternalmark == "-1")
                                        {
                                            previousinternalmark = "AB";
                                        }
                                        fpspread.Sheets[0].Cells[row_cnt, 3].Text = "NR";
                                        fpspread.Sheets[0].Cells[row_cnt, 4].Text = "NR";
                                        fpspread.Sheets[0].Cells[row_cnt, 5].Text = "NR";
                                        fpspread.Sheets[0].Cells[row_cnt, 6].Text = "NR";
                                        fpspread.Sheets[0].Cells[row_cnt, 8].Text = previousinternalmark;
                                        fpspread.Sheets[0].Cells[row_cnt, 9].Text = "NR";
                                        fpspread.Sheets[0].Cells[row_cnt, 10].Text = "NC";
                                        fpspread.Sheets[0].Cells[row_cnt, 11].Text = "NC";
                                        fpspread.Sheets[0].Cells[row_cnt, 11].Note = passorfail.ToString();
                                    }
                                }
                                else
                                {

                                    if (strus == "NE")
                                    {
                                        fpspread.Sheets[0].Cells[row_cnt, 3].Text = "NE";
                                        fpspread.Sheets[0].Cells[row_cnt, 4].Text = "NE";
                                        fpspread.Sheets[0].Cells[row_cnt, 5].Text = "NE";
                                        fpspread.Sheets[0].Cells[row_cnt, 6].Text = "NE";
                                        fpspread.Sheets[0].Cells[row_cnt, 8].Text = previousinternalmark;
                                        fpspread.Sheets[0].Cells[row_cnt, 9].Text = "NE";
                                        fpspread.Sheets[0].Cells[row_cnt, 10].Text = "NC";
                                        fpspread.Sheets[0].Cells[row_cnt, 11].Text = "NC";
                                        fpspread.Sheets[0].Cells[row_cnt, 11].Note = passorfail.ToString();
                                    }
                                    else if (strus == "NR")
                                    {
                                        fpspread.Sheets[0].Cells[row_cnt, 3].Text = "NR";
                                        fpspread.Sheets[0].Cells[row_cnt, 4].Text = "NR";
                                        fpspread.Sheets[0].Cells[row_cnt, 5].Text = "NR";
                                        fpspread.Sheets[0].Cells[row_cnt, 6].Text = "NR";
                                        fpspread.Sheets[0].Cells[row_cnt, 8].Text = previousinternalmark;
                                        fpspread.Sheets[0].Cells[row_cnt, 9].Text = "NR";
                                        fpspread.Sheets[0].Cells[row_cnt, 10].Text = "NC";
                                        fpspread.Sheets[0].Cells[row_cnt, 11].Text = "NC";
                                        fpspread.Sheets[0].Cells[row_cnt, 11].Note = passorfail.ToString();
                                    }
                                    else if (fpspread.Sheets[0].Cells[row_cnt, 3].Text == "LT")
                                    {
                                        fpspread.Sheets[0].Cells[row_cnt, 3].Text = "LT";
                                        fpspread.Sheets[0].Cells[row_cnt, 4].Text = "LT";
                                        fpspread.Sheets[0].Cells[row_cnt, 5].Text = "LT";
                                        fpspread.Sheets[0].Cells[row_cnt, 6].Text = "LT";
                                        fpspread.Sheets[0].Cells[row_cnt, 8].Text = previousinternalmark;
                                        fpspread.Sheets[0].Cells[row_cnt, 9].Text = "LT";
                                        fpspread.Sheets[0].Cells[row_cnt, 10].Text = "NC";
                                        fpspread.Sheets[0].Cells[row_cnt, 11].Text = "NC";
                                        fpspread.Sheets[0].Cells[row_cnt, 11].Note = passorfail.ToString();
                                    }
                                    else if (fpspread.Sheets[0].Cells[row_cnt, 3].Text == "W")
                                    {
                                        fpspread.Sheets[0].Cells[row_cnt, 3].Text = "W";
                                        fpspread.Sheets[0].Cells[row_cnt, 4].Text = "W";
                                        fpspread.Sheets[0].Cells[row_cnt, 5].Text = "W";
                                        fpspread.Sheets[0].Cells[row_cnt, 6].Text = "W";
                                        fpspread.Sheets[0].Cells[row_cnt, 8].Text = previousinternalmark;
                                        fpspread.Sheets[0].Cells[row_cnt, 9].Text = "W";
                                        fpspread.Sheets[0].Cells[row_cnt, 10].Text = "NC";
                                        fpspread.Sheets[0].Cells[row_cnt, 11].Text = "NC";
                                        fpspread.Sheets[0].Cells[row_cnt, 11].Note = passorfail.ToString();
                                    }
                                    else
                                    {
                                        fpspread.Sheets[0].Cells[row_cnt, 3].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 4].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 5].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 6].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 8].Text = previousinternalmark;
                                        fpspread.Sheets[0].Cells[row_cnt, 9].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 10].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 11].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 11].Note = passorfail.ToString();
                                    }
                                }
                            }
                           

                        }
                        lblaane.Visible = true;

                        fpspread.Height = height;
                        fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
                        fpspread.Visible = false;
                        fpspread.Visible = true;
                        lblerr1.Visible = false;
                        btnprintt.Visible = true;
                        btnsave1.Visible = true;
                    }

                }
                else
                {
                    btnreset.Visible = false;
                    lblaane.Visible = false;

                    btnsave1.Visible = false;
                    btnprintt.Visible = false;
                    fpspread.Visible = false;
                    lblerr1.Visible = true;
                    lblerr1.Text = "No Records Found";
                }
            }
            if (valuationSettng == "3")
            {
                fpspread.Sheets[0].ColumnHeader.Columns[0].Visible = true;
                fpspread.Sheets[0].ColumnHeader.Columns[1].Visible = true;
                fpspread.Sheets[0].ColumnHeader.Columns[2].Visible = true; 
                if (ddlSubSub.SelectedIndex != 0)
                {
                    if (dtSubSubject.Rows.Count > 0 && Convert.ToString(ddlSubSub.SelectedItem.Text) == fpspread.Sheets[0].ColumnHeader.Cells[1, 3].Text)
                    {
                        fpspread.Width = 550 + 50;
                        fpspread.Sheets[0].ColumnHeader.Columns[3].Visible = true;
                    }

                    if (dtSubSubject.Rows.Count > 1 && Convert.ToString(ddlSubSub.SelectedItem.Text) == fpspread.Sheets[0].ColumnHeader.Cells[1, 4].Text)
                    {
                        fpspread.Width = 550 + 80;
                        fpspread.Sheets[0].ColumnHeader.Columns[4].Visible = true;
                    }
                    if (dtSubSubject.Rows.Count > 2 && Convert.ToString(ddlSubSub.SelectedItem.Text) == fpspread.Sheets[0].ColumnHeader.Cells[1, 5].Text)
                    {
                        fpspread.Width = 550 + 160;
                        fpspread.Sheets[0].ColumnHeader.Columns[5].Visible = true;
                    }
                    if (dtSubSubject.Rows.Count > 3 && Convert.ToString(ddlSubSub.SelectedItem.Text) == fpspread.Sheets[0].ColumnHeader.Cells[1, 6].Text)
                    {
                        fpspread.Width = 550 + 240;
                        fpspread.Sheets[0].ColumnHeader.Columns[6].Visible = true;
                    }
                }
                else
                {
                    if (dtSubSubject.Rows.Count > 0)
                    {
                        fpspread.Width = 550 + 50;
                        fpspread.Sheets[0].ColumnHeader.Columns[3].Visible = true;
                    }

                    if (dtSubSubject.Rows.Count > 1)
                    {
                        fpspread.Width = 550 + 80;
                        fpspread.Sheets[0].ColumnHeader.Columns[4].Visible = true;
                    }
                    if (dtSubSubject.Rows.Count > 2)
                    {
                        fpspread.Width = 550 + 160;
                        fpspread.Sheets[0].ColumnHeader.Columns[5].Visible = true;
                    }
                    if (dtSubSubject.Rows.Count > 3)
                    {
                        fpspread.Width = 550 + 240;
                        fpspread.Sheets[0].ColumnHeader.Columns[6].Visible = true;
                    }
                }

                fpspread.Sheets[0].ColumnHeader.Columns[7].Visible = true;
                fpspread.Sheets[0].ColumnHeader.Columns[2].Visible = true;
                fpspread.Sheets[0].ColumnHeader.Columns[8].Visible = false;
                fpspread.Sheets[0].ColumnHeader.Columns[9].Visible = false;
                fpspread.Sheets[0].ColumnHeader.Columns[10].Visible = false;
                fpspread.Sheets[0].ColumnHeader.Columns[11].Visible = false;
            }
            else
            {
                fpspread.Visible = false;
                lblerr1.Visible = true;
                lblerr1.Text = "Pls Check Valuation Settings";
            }

            fpspread.Sheets[0].ColumnHeader.Columns[0].Locked = true;
            fpspread.Sheets[0].ColumnHeader.Columns[1].Locked = true;
            fpspread.Sheets[0].ColumnHeader.Columns[2].Locked = true;
            //fpspread.Sheets[0].ColumnHeader.Columns[6].Locked = true;
            fpspread.Sheets[0].ColumnHeader.Columns[5].Locked = true;
            fpspread.Sheets[0].ColumnHeader.Columns[7].Locked = true;
            fpspread.Sheets[0].ColumnHeader.Columns[8].Locked = true;
            fpspread.Sheets[0].ColumnHeader.Columns[9].Locked = true;
            fpspread.Sheets[0].ColumnHeader.Columns[10].Locked = true;
            fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
            int hei = 300;
            for (int col = 0; col < fpspread.Sheets[0].RowCount; col++)
            {
                hei = hei + fpspread.Sheets[0].Rows[col].Height;
            }
            fpspread.Height = hei;
            fpspread.Visible = true;
            fpspread.SaveChanges();

        }

        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    protected void btnsavel1_click(object sender, EventArgs e)
    {
        try
        {
            fpspread.SaveChanges();
            marksavefunction();
            lblAlertMsg.Visible = true;
            lblAlertMsg.Text = "Saved Sucessfully";
            divPopAlert.Visible = true;
            return;
            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
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
        else if (mr == "-19")
        {
            strgetval = "W";
        }
        return strgetval;
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    public void marksavefunction()
    {
        try
        {
            Dictionary<string, string> dicdegreedetails = new Dictionary<string, string>();
            int markround = 0;
            string getmarkround = da.GetFunctionv("select value from COE_Master_Settings where settings='Mark Round of'");
            if (getmarkround.Trim() != "" && getmarkround.Trim() != "0")
            {
                int num = 0;
                if (int.TryParse(getmarkround, out num))
                {
                    markround = Convert.ToInt32(getmarkround);
                }
            }

            fpspread.SaveChanges();
            string batchyear = string.Empty;
            string degreecode = string.Empty;
            string subject_no = string.Empty;
            string examcode = string.Empty;
            double remaim = 0;
            string roll_no = string.Empty;
            string result = string.Empty;
            int passorfail = 0;
            string insupdquery = string.Empty;
            int insupdval = 0;
            string exammonth = ddlMonth1.SelectedValue.ToString();
            string examyear = ddlYear1.SelectedValue.ToString();
            int my = Convert.ToInt32(ddlMonth1.SelectedIndex.ToString()) + Convert.ToInt32(ddlYear1.SelectedValue.ToString()) * 12;

            string degreeval = string.Empty;
            string degreevalsub = string.Empty;
            string degreevalttab = string.Empty;
            string degreevalregis = string.Empty;

            string subjectCodeNew = Convert.ToString(ddlSubject.SelectedValue);

            degreeval = " and ed.degree_code='" + ddlbranch1.SelectedValue + "'";
            degreevalsub = " and s.degree_code='" + ddlbranch1.SelectedValue + "'";
            degreevalttab = " and m.degree_code='" + ddlbranch1.SelectedValue + "'";
            degreevalregis = " and r.degree_code='" + ddlbranch1.SelectedValue + "'";
            string degCode = "   and  DegreeCode='" + ddlbranch1.SelectedValue + "'";

            string pmaxsp = fpspread.Sheets[0].ColumnHeader.Cells[0, 10].Text;
            string[] spmaxsp = fpspread.Sheets[0].ColumnHeader.Cells[2, 9].Text.ToString().Split(':');
            double maxexternal = Convert.ToDouble(spmaxsp[1]);
            spmaxsp = fpspread.Sheets[0].ColumnHeader.Cells[2, 9].Text.ToString().Split(':');
            // double entmaxexternal = Convert.ToDouble(spmaxsp[1]); 
            double entmaxexternal = 0;
            if (spmaxsp.Length > 1)
            {
                if (spmaxsp[1].Trim() != "")
                {
                    entmaxexternal = Convert.ToDouble(spmaxsp[1]);
                }
            }

            Hashtable hatgrade = new Hashtable();
            string grdaemaster = "select batch_year,grade_flag,degree_code from grademaster m where m.exam_month='" + exammonth + "' and m.exam_year='" + examyear + "' and m.grade_flag='3' " + degreevalttab + "";
            DataSet dsgrademaster = da.select_method_wo_parameter(grdaemaster, "Text");
            if (dsgrademaster.Tables.Count > 0 && dsgrademaster.Tables[0].Rows.Count > 0)
            {
                for (int d = 0; d < dsgrademaster.Tables[0].Rows.Count; d++)
                {
                    if (!hatgrade.Contains(dsgrademaster.Tables[0].Rows[d]["batch_year"].ToString() + '-' + dsgrademaster.Tables[0].Rows[d]["degree_code"].ToString()))
                    {
                        hatgrade.Add(dsgrademaster.Tables[0].Rows[d]["batch_year"].ToString() + '-' + dsgrademaster.Tables[0].Rows[d]["degree_code"].ToString(), dsgrademaster.Tables[0].Rows[d]["batch_year"].ToString());
                    }
                }
            }

            string SelectSub = "SELECT * FROM COESubSubjectPartSettings ES,COESubSubjectPartMater Em WHERE em.id=es.id and em.ExamMonth='" + ddlMonth1.SelectedValue.ToString() + "' and em.ExamYear='" + ddlYear1.SelectedItem.Text.ToString() + "' and es.SubCode='" + subjectCodeNew + "'  " + degCode + " ";
            DataTable dtSubSubject = dirAccess.selectDataTable(SelectSub);

            dtSubSubject = dirAccess.selectDataTable(SelectSub);
            DataRow drRow = null;
            if (dtSubSubject.Rows.Count == 0)
            {
                drRow = dtSubSubject.NewRow();
                drRow["SubPart"] = subjectCodeNew;
                drRow["maxmark"] = maxexternal;
                dtSubSubject.Rows.Add(drRow);
            }

            double evalmaxmark = Convert.ToDouble(fpspread.Sheets[0].ColumnHeader.Cells[2, 3].Tag);
            double minextmarks = 0;
            double manextmarks = 0;
            double minintmarks = 0;
            double mintotalv = 0;
            double maxtotalv = 0;
            double minexternaleva = 0;
            double mintotaleva = 0;
            double maxinternalmarkvalue = 0;
            bool saveflag = false;
            for (int r = 0; r < fpspread.Sheets[0].RowCount; r++)
            {
                saveflag = true;
                roll_no = fpspread.Sheets[0].Cells[r, 1].Note.ToString();
                batchyear = fpspread.Sheets[0].Cells[r, 0].Tag.ToString();
                degreecode = fpspread.Sheets[0].Cells[r, 5].Note.ToString();
                string creditpoint = fpspread.Sheets[0].Cells[r, 6].Tag.ToString();

                examcode = fpspread.Sheets[0].Cells[r, 1].Tag.ToString();

                string attempts = fpspread.Sheets[0].Cells[r, 3].Note.ToString();
                subject_no = fpspread.Sheets[0].Cells[r, 3].Tag.ToString();

                string evauation1 = fpspread.Sheets[0].Cells[r, 3].Text.ToString();
                string evauation2 = fpspread.Sheets[0].Cells[r, 4].Text.ToString();
                string evauation3 = fpspread.Sheets[0].Cells[r, 5].Text.ToString();
                string evauation4 = fpspread.Sheets[0].Cells[r, 6].Text.ToString();

                string icamark = fpspread.Sheets[0].Cells[r, 8].Text.ToString();
                string ese = fpspread.Sheets[0].Cells[r, 9].Text.ToString();
                string totalmarkvalu = fpspread.Sheets[0].Cells[r, 10].Text.ToString();
                string maderonmark = string.Empty;
                result = fpspread.Sheets[0].Cells[r, 11].Text.ToString();

                string cursem = fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Note.ToString();

                //double minicamoderatio = Convert.ToDouble(fpspread.Sheets[0].Cells[r, 6].Note.ToString());
                //string modemarkval = fpspread.Sheets[0].Cells[r, 6].Tag.ToString();
                //double maxmarkmoderation = Convert.ToDouble(modemarkval);

                minintmarks = Convert.ToDouble(fpspread.Sheets[0].Cells[r, 6].Note.ToString());
                minextmarks = Convert.ToDouble(fpspread.Sheets[0].Cells[r, 9].Note.ToString());
                manextmarks = Convert.ToDouble(fpspread.Sheets[0].Cells[r, 9].Tag.ToString());
                maxtotalv = Convert.ToDouble(fpspread.Sheets[0].Cells[r, 10].Tag.ToString());
                mintotalv = Convert.ToDouble(fpspread.Sheets[0].Cells[r, 10].Note.ToString());
                maxinternalmarkvalue = Convert.ToDouble(fpspread.Sheets[0].Cells[r, 8].Tag.ToString());

                minexternaleva = minextmarks / manextmarks * evalmaxmark;
                mintotaleva = mintotalv / maxtotalv * evalmaxmark;

                if (manextmarks == 0 && minextmarks == 0)
                {
                    evauation1 = "0";
                    evauation2 = "0";
                    evauation3 = "0";
                    evauation4 = "0";
                }
                if (maxinternalmarkvalue == 0 && minintmarks == 0)
                {
                    icamark = "0";
                }

                double avg = 0;
                double intark = 0;
                if (icamark == "")
                {
                    icamark = "Null";
                    result = "Null";
                    intark = 0;
                }
                else
                {
                    if (icamark.Trim().ToLower().Contains('a'))// == "ab")
                    {
                        icamark = "-1";
                    }
                    if (icamark.Trim().ToLower().Contains('w'))// == "ab")
                    {
                        icamark = "-19";
                    }
                    else
                    {
                        intark = Convert.ToDouble(icamark);
                    }
                }

                if (!string.IsNullOrEmpty(icamark))
                {
                    double ev1 = 0;
                    double ev2 = 0;
                    double ev3 = 0;
                    double ev4 = 0;
                    ;
                    double totalmarkvalue = 0;
                    double extmark = 0;

                    if (evauation1.Trim() == "")
                    {
                        evauation1 = "Null";
                        evauation2 = "Null";
                        evauation3 = "Null";
                        evauation4 = "Null";
                        ese = "Null";
                        result = "Null";
                        totalmarkvalu = "Null";
                    }

                    else if (evauation1.Trim() != "" || evauation2.Trim() != "" || evauation3.Trim() != "" || evauation4.Trim() != "")
                    {
                        if (evauation1.Trim().ToLower() == "aaa" || evauation1.Trim().ToLower() == "ab" || evauation1.Trim().ToLower() == "aa" || evauation1.Trim().ToLower() == "a" || evauation3.Trim().ToLower() == "aaa" || evauation3.Trim().ToLower() == "ab" || evauation4.Trim().ToLower() == "aaa" || evauation4.Trim().ToLower() == "ab")
                        {
                            evauation1 = "-1";
                            evauation2 = "-1";
                            evauation3 = "-1";
                            evauation4 = "-1";
                            ese = "-1";
                            result = "AAA";
                            totalmarkvalu = icamark;
                        }
                        else if (evauation1.Trim().ToLower() == "ne" || evauation2.Trim().ToLower() == "ne" || evauation3.Trim().ToLower() == "ne" || evauation4.Trim().ToLower() == "ne")
                        {
                            evauation1 = "-2";
                            evauation3 = "-2";
                            evauation4 = "-2";
                            ese = "-2";
                            result = "Fail";
                            passorfail = 0;
                            totalmarkvalu = icamark;
                            evauation2 = "-2";
                        }
                        else if (evauation1.Trim().ToLower() == "nr" || evauation2.Trim().ToLower() == "nr" || evauation3.Trim().ToLower() == "nr" || evauation4.Trim().ToLower() == "nr")
                        {
                            evauation1 = "-3";
                            ese = "-3";
                            evauation3 = "-3";
                            evauation4 = "-3";
                            result = "Fail";
                            passorfail = 0;
                            totalmarkvalu = icamark;
                            evauation2 = "-3";

                        }
                        else if (evauation1.Trim().ToLower() == "lt" || evauation2.Trim().ToLower() == "lt" || evauation3.Trim().ToLower() == "lt" || evauation4.Trim().ToLower() == "lt")
                        {
                            evauation1 = "-4";
                            ese = "-4";
                            evauation3 = "-4";
                            evauation4 = "-4";
                            result = "Fail";
                            passorfail = 0;
                            totalmarkvalu = icamark;
                            evauation2 = "-4";
                        }
                        else if (evauation1.Trim().ToLower() == "w" || evauation2.Trim().ToLower() == "w" || evauation3.Trim().ToLower() == "w" || evauation4.Trim().ToLower() == "w")
                        {
                            evauation1 = "-19";
                            ese = "-19";
                            evauation3 = "-19";
                            evauation4 = "-19";
                            result = "W";
                            passorfail = 0;
                            totalmarkvalu = icamark;
                            evauation2 = "-19";
                        }
                        else if (evauation1.Trim().ToLower() == "m" || evauation2.Trim().ToLower() == "m" || evauation3.Trim().ToLower() == "m" || evauation4.Trim().ToLower() == "m")
                        {
                            evauation1 = "0";
                            evauation3 = "0";
                            evauation4 = "0";
                            ese = "0";
                            result = "WHD";
                            passorfail = 0;
                            totalmarkvalu = icamark;
                            evauation2 = "0";
                        }
                        else 
                        {
                            bool isSaved = false;
                            if (dtSubSubject.Rows.Count == 1 && !string.IsNullOrEmpty(evauation1))
                                isSaved = true;
                            if (dtSubSubject.Rows.Count == 2 && !string.IsNullOrEmpty(evauation1) && !string.IsNullOrEmpty(evauation2))
                                isSaved = true;
                            if (dtSubSubject.Rows.Count == 3 && !string.IsNullOrEmpty(evauation1) && !string.IsNullOrEmpty(evauation2) &&  !string.IsNullOrEmpty(evauation3))
                                isSaved = true;
                            if (dtSubSubject.Rows.Count == 4 && !string.IsNullOrEmpty(evauation1) && !string.IsNullOrEmpty(evauation2) && !string.IsNullOrEmpty(evauation3) && !string.IsNullOrEmpty(evauation4))
                                isSaved = true;
                            if (isSaved)
                            {

                                double.TryParse(evauation1, out ev1);
                                double.TryParse(evauation2, out ev2);
                                double.TryParse(evauation3, out ev3);
                                double.TryParse(evauation4, out ev4);

                                avg = (ev1 + ev2 + ev3 + ev4);
                                avg = Math.Round(avg, 0, MidpointRounding.AwayFromZero);
                                intark = Convert.ToDouble(intark);
                                extmark = (ev1 + ev2 + ev3 + ev4);
                                //extmark = Math.Round(extmark, markround, MidpointRounding.AwayFromZero); //Rajkumar
                                extmark = extmark / entmaxexternal * maxexternal;
                                extmark = Math.Round(extmark, 2, MidpointRounding.AwayFromZero);
                                extmark = Math.Round(extmark, MidpointRounding.AwayFromZero);

                                ese = extmark.ToString();
                                totalmarkvalue = extmark + intark;
                                totalmarkvalu = totalmarkvalue.ToString();
                                //ev1 = Convert.ToDouble(evauation1);
                                //ev2 = Convert.ToDouble(evauation2);

                                //double avg1 = 0;
                                //avg1 = ev2 - ev1;
                                //avg1 = Math.Abs(avg1);

                                result = "Fail";
                                if (minintmarks <= intark && minextmarks <= extmark && mintotalv <= totalmarkvalue)
                                {
                                    result = "Pass";
                                }
                            }
                        }
                    }

                    passorfail = 0;
                    if (result.Trim().ToLower() == "pass")
                    {
                        passorfail = 1;
                    }
                    if (icamark.ToLower().Contains('a') || icamark.ToLower().Contains("-1"))
                    {
                        passorfail = 0;
                        if (result.Trim().ToLower() == "whd")
                        {
                            result = "WHD";
                        }
                        else if (result.Trim().ToLower() == "w")
                        {
                            result = "W";
                        }
                        else
                        {
                            result = "Fail";
                        }
                    }
                    if (icamark.ToLower().Contains('w') || icamark.ToLower().Contains("-19"))
                    {
                        passorfail = 0;
                        if (result.Trim().ToLower() == "w")
                        {
                            result = "W";
                        }
                        else
                        {
                            result = "Fail";
                        }
                    }

                    if (saveflag)
                    {
                        if (string.IsNullOrEmpty(evauation1))
                            evauation1="''";
                        if (string.IsNullOrEmpty(evauation2))
                            evauation2 = "''";
                        if (string.IsNullOrEmpty(evauation3))
                            evauation3 = "''";
                        if (string.IsNullOrEmpty(evauation4))
                            evauation4 = "''";
                        if (string.IsNullOrEmpty(ese))
                            ese = "''";
                        if (string.IsNullOrEmpty(totalmarkvalu))
                            totalmarkvalu = "''";


                        insupdquery = "if not exists(select * from mark_entry where exam_code='" + examcode + "' and roll_no='" + roll_no + "' and subject_no='" + subject_no + "')";
                        insupdquery = insupdquery + " insert into mark_entry (roll_no,subject_no,exam_code,internal_mark,external_mark,total,result,passorfail,attempts,MYData,rej_stat,cp,evaluation1,evaluation2,evaluation3,evaluation4)";
                        insupdquery = insupdquery + " values('" + roll_no + "','" + subject_no + "','" + examcode + "'," + icamark + "," + ese + "," + totalmarkvalu + ",'" + result + "','" + passorfail + "','" + attempts + "','" + my + "','0','" + creditpoint + "'," + evauation1 + "," + evauation2 + "," + evauation3 + "," + evauation4 + ")";
                        insupdquery = insupdquery + " else";
                        insupdquery = insupdquery + " update mark_entry set internal_mark=" + icamark + ",external_mark=" + ese + ",total=" + totalmarkvalu + ",result='" + result + "',passorfail='" + passorfail + "',attempts='" + attempts + "',evaluation1=" + evauation1 + ",evaluation2=" + evauation2 + ",evaluation3=" + evauation3 + ",evaluation4=" + evauation4 + "";
                        insupdquery = insupdquery + " where exam_code='" + examcode + "' and roll_no='" + roll_no + "' and subject_no='" + subject_no + "'";

                        insupdval = da.update_method_wo_parameter(insupdquery, "Text");
                    }
                }

                string degreedetails = batchyear + '-' + degreecode;
                if (!dicdegreedetails.ContainsKey(degreedetails))
                {
                    dicdegreedetails.Add(degreedetails, degreedetails);
                    string entrycode = Session["Entry_Code"].ToString();
                    string formname = "Exam Mark Entry";
                    string toa = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                    string doa = DateTime.Now.ToString("MM/dd/yyy");
                    int savevalue = Convert.ToInt32(Session["MarkEntrySave"].ToString());
                    string details = "" + degreecode + ":Batch Year -" + batchyear + ":Exam Month - " + ddlMonth1.SelectedValue.ToString() + ":Exam Year -" + ddlYear1.SelectedValue.ToString() + ":Subject Code -" + ddlSubject.SelectedValue.ToString();
                    string modules = "0";
                    string act_diff = " ";
                    string ctsname = "Update The Exam Mark Entry Information";
                    if (savevalue == 1)
                    {
                        ctsname = "Save The Exam Mark Entry Information";
                    }

                    string strlogdetails = "insert into UserLog (Entry_Code,Form_Name,UsrAction,TOA,DOA,Details,Module,Act_Diff,ctrNam) values ('" + entrycode + "','" + formname + "','" + savevalue + "','" + toa + "','" + doa + "','" + details + "','" + modules + "','" + act_diff + "','" + ctsname + "')";
                    int a = da.update_method_wo_parameter(strlogdetails, "Text");
                }
            }
            buttongo();
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    public void clear()
    {
        lblaane.Visible = false;

        btnreset.Visible = false;
        btnprintt.Visible = false;
        fpspread.Visible = false;
        fpspread.Visible = false;
        btnsave1.Visible = false;

        //chkincluevel2.Visible = false;
    }

    //Added bY iDhris 22-02-2017
    private bool ShowDummyNumber()
    {
        bool retval = false;
        string saveDummy = da.GetFunction("select LinkValue from New_InsSettings where LinkName='ShowDummyNumberOnMarkEntryCOE' and college_code ='" + CollegeCode + "' and user_code ='" + usercode + "'  ").Trim();
        if (saveDummy == "1")
        {
            retval = true;
        }
        return retval;
    }

    private byte DummyNumberType()
    {
        byte retval = 0;//0-common , 1- subjectwise
        string typeDummy = da.GetFunction("select LinkValue from New_InsSettings where LinkName='DummyNumberTypeOnMarkEntryCOE' and college_code ='" + CollegeCode + "' and user_code ='" + usercode + "'  ").Trim();
        if (typeDummy == "1")
        {
            retval = 1;
        }
        return retval;
    }

    private byte getDummyNumberMode()
    {
        byte retval = 0;//0-Serial , 1- Random
        string modeDummy = da.GetFunction("select LinkValue from New_InsSettings where LinkName='DummyNumberModeOnMarkEntryCOE' and college_code ='" + CollegeCode + "' and user_code ='" + usercode + "'  ").Trim();
        if (modeDummy == "1")
        {
            retval = 1;
        }
        return retval;
    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
        }
        catch (Exception ex)
        {
            //da.sendErrorMail(ex, collegecode, "Exam Application");
        }
    }

    public void print1(string valuationSettng)
    {
        try
        {
            Font Fontbold123 = new Font("Times New Roman", 15, FontStyle.Bold);
            Font Fontbold = new Font("Times New Roman", 15, FontStyle.Bold);
            Font Fontsmall = new Font("Times New Roman", 10, FontStyle.Regular);
            Font Fontbold1 = new Font("Times New Roman", 10, FontStyle.Bold);
            Gios.Pdf.PdfDocument myprovdoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage myprov_pdfpage = myprovdoc.NewPage();

            int coltop = 5;
            string collegename = string.Empty;
            string examexternalmark = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[2, 4].Tag);
            string maxexternalmark = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[2, 5].Tag);
            string maxinternalmark = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[2, 4].Tag);
            string totlmatrk = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[2, 6].Tag);
            // string modrationva = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Tag) + " %";
            string[] spmaxsp = fpspread.Sheets[0].ColumnHeader.Cells[2, 5].Text.ToString().Split(':');
            double maxexternal = Convert.ToDouble(spmaxsp[1]);
            spmaxsp = fpspread.Sheets[0].ColumnHeader.Cells[2, 2].Text.ToString().Split(':');
            // double entmaxexternal = Convert.ToDouble(spmaxsp[1]); 
            double entmaxexternal = 0;
            if (spmaxsp.Length > 1)
            {
                if (spmaxsp[1].Trim() != "")
                {
                    entmaxexternal = Convert.ToDouble(spmaxsp[1]);
                }
            }

            string catgory = string.Empty;
            string coll_name = string.Empty;
            string ugorpg = string.Empty;
            string str = "select isnull(collname,'') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1,'') as address1,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code='" + Session["collegecode"] + "'";
            ds2 = da.select_method_wo_parameter(str, "Text");
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                catgory = ds2.Tables[0].Rows[0]["category"].ToString();
                coll_name = Convert.ToString(ds2.Tables[0].Rows[0]["collname"]);
            }
            ugorpg = da.GetFunction("select distinct c.Edu_Level from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.ToString() + "' and s.subject_code='" + ddlSubject.SelectedValue.ToString() + "'");

            string edusheet = "MARK SHEET / AVERAGE SHEET [" + ugorpg + "]";
            collegename = coll_name + " (" + catgory + ")";

            PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                             new PdfArea(myprovdoc, 20, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleCenter, collegename);

            coltop = coltop + 20;
            PdfTextArea pts = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(myprovdoc, 20, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, edusheet);
            PdfTextArea ptss = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                 new PdfArea(myprovdoc, 200, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "PASSING MINIMUM");

            coltop = coltop + 15;
            PdfTextArea ptss1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                 new PdfArea(myprovdoc, 20, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Month & Year : " + ddlMonth1.SelectedItem.Text.ToString() + "-" + ddlYear1.SelectedItem.Text.ToString());


            coltop = coltop + 15;
            PdfTextArea ptss2 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                 new PdfArea(myprovdoc, 20, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, " ");

            ptss2 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                               new PdfArea(myprovdoc, 20, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Class & Group : All Groups");

            int pasmintopval = coltop - 15;
            string gettext = da.GetFunction("select value from COE_Master_Settings where settings='" + ugorpg + " Passing Minimum'");
            string[] stva = gettext.Split('~');
            if (gettext.Trim() != "" && gettext.Trim() != "0")
            {
                for (int c = 0; c <= stva.GetUpperBound(0); c++)
                {
                    PdfTextArea ptsspassmin = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                   new PdfArea(myprovdoc, 350, pasmintopval + (c * 15), 400, 30), System.Drawing.ContentAlignment.MiddleLeft, stva[c].ToString());

                    myprov_pdfpage.Add(ptsspassmin);
                }
            }
            if (coltop < pasmintopval + (stva.GetUpperBound(0) * 15))
            {
                coltop = pasmintopval + (stva.GetUpperBound(0) * 15);
            }

            string subjectname = da.GetFunction("Select subject_name from subject where subject_code='" + ddlSubject.SelectedValue.ToString() + "'");
            coltop = coltop + 15;
            PdfTextArea ptss22 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                 new PdfArea(myprovdoc, 20, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Paper Name : " + subjectname);

            PdfTextArea ptss3 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                             new PdfArea(myprovdoc, 350, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Paper Code : " + ddlSubject.SelectedValue.ToString());

            PdfTextArea ptss31 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                             new PdfArea(myprovdoc, 350, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "");

            coltop = coltop + 30;

            PdfTextArea ptadate = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                             new PdfArea(myprovdoc, 10, 780, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Date");

            PdfTextArea ptadatecur = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                             new PdfArea(myprovdoc, 10, 800, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, DateTime.Now.ToString("dd/MM/yyyy"));

            PdfTextArea ptadepratment = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                            new PdfArea(myprovdoc, 275, 780, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Department");

            PdfTextArea ptachairman = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                            new PdfArea(myprovdoc, 450, 780, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Chairman");

            myprov_pdfpage.Add(ptc);
            myprov_pdfpage.Add(pts);
            myprov_pdfpage.Add(ptss);
            myprov_pdfpage.Add(ptss1);
            myprov_pdfpage.Add(ptss2);
            myprov_pdfpage.Add(ptss22);
            myprov_pdfpage.Add(ptss3);
            myprov_pdfpage.Add(ptss31);

            myprov_pdfpage.Add(ptadepratment);
            myprov_pdfpage.Add(ptadatecur);
            myprov_pdfpage.Add(ptadate);
            myprov_pdfpage.Add(ptachairman);

            Gios.Pdf.PdfTable table1;
            int val = 3;
            int noofrowpertable = 0;
            if (fpspread.Sheets[0].RowCount > 25)
            {
                noofrowpertable = 29;
            }
            else
            {
                noofrowpertable = fpspread.Sheets[0].RowCount + 4;
            }

            table1 = myprovdoc.NewTable(Fontsmall, noofrowpertable, 2, 1);
            table1.VisibleHeaders = false;

            table1.VisibleHeaders = false;
            table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
            table1.Columns[0].SetWidth(100);
            table1.Columns[1].SetWidth(80);
            //table1.Columns[2].SetWidth(80);
            //table1.Columns[3].SetWidth(80);
            //table1.Columns[4].SetWidth(80);
            //table1.Columns[5].SetWidth(80);
            //table1.Columns[6].SetWidth(80);
            //table1.Columns[7].SetWidth(80);
            //table1.Columns[8].SetWidth(80);
            //table1.Columns[9].SetWidth(80);


            table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
            if (ShowDummyNumber())
            {
                table1.Cell(0, 0).SetContent("Dummy No");
            }
            else
            {
                table1.Cell(0, 0).SetContent("Reg No");
            }
            table1.Cell(0, 0).SetFont(Fontbold1);

            table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(0, 1).SetFont(Fontbold1);
            table1.Cell(0, 1).SetContent("Valuation");

            table1.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(1, 1).SetFont(Fontbold1);
            if (valuationSettng == "1")
                table1.Cell(1, 1).SetContent("I");
            else
                table1.Cell(1, 1).SetContent("II");

            table1.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(2, 1).SetFont(Fontbold1);
            table1.Cell(2, 1).SetContent("Max");

            table1.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(3, 1).SetFont(Fontbold1);
            table1.Cell(3, 1).SetContent(entmaxexternal);
            table1.Rows[0].SetCellPadding(5);
            table1.Rows[1].SetCellPadding(5);

            foreach (PdfCell pr in table1.CellRange(0, 0, 0, 0).Cells)
            {
                pr.RowSpan = 4;
            }
            foreach (PdfCell pr in table1.CellRange(0, 1, 0, 1).Cells)
            {
                pr.ColSpan = 1;
            }


            Gios.Pdf.PdfTablePage myprov_pdfpage1;

            for (int row_cnt = 0; row_cnt < fpspread.Sheets[0].RowCount; row_cnt++)
            {
                if ((row_cnt % 25) == 0 && row_cnt > 0)
                {
                    myprov_pdfpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(myprovdoc, 150, coltop, 300, 650));
                    myprov_pdfpage.Add(myprov_pdfpage1);

                    myprov_pdfpage.SaveToDocument();
                    myprov_pdfpage = myprovdoc.NewPage();
                    myprov_pdfpage.Add(ptc);
                    myprov_pdfpage.Add(pts);
                    myprov_pdfpage.Add(ptss);
                    myprov_pdfpage.Add(ptss1);
                    if (gettext.Trim() != "" && gettext.Trim() != "0")
                    {
                        for (int c = 0; c <= stva.GetUpperBound(0); c++)
                        {
                            PdfTextArea ptsspassmin = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                           new PdfArea(myprovdoc, 350, pasmintopval + (c * 15), 400, 30), System.Drawing.ContentAlignment.MiddleLeft, stva[c].ToString());

                            myprov_pdfpage.Add(ptsspassmin);
                        }
                    }
                    myprov_pdfpage.Add(ptss2);
                    myprov_pdfpage.Add(ptss22);
                    myprov_pdfpage.Add(ptss3);
                    myprov_pdfpage.Add(ptss31);

                    myprov_pdfpage.Add(ptadepratment);
                    myprov_pdfpage.Add(ptadatecur);
                    myprov_pdfpage.Add(ptadate);
                    myprov_pdfpage.Add(ptachairman);

                    noofrowpertable = 0;
                    if (fpspread.Sheets[0].RowCount > row_cnt + 25)
                    {
                        noofrowpertable = 25;
                    }
                    else
                    {
                        noofrowpertable = fpspread.Sheets[0].RowCount - row_cnt;
                    }

                    val = 3;
                    table1.VisibleHeaders = false;
                    table1 = myprovdoc.NewTable(Fontsmall, noofrowpertable + 4, 2, 1);

                    table1.VisibleHeaders = false;
                    table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                    table1.Columns[0].SetWidth(100);
                    table1.Columns[1].SetWidth(80);

                    //table1.Columns[7].SetWidth(80);


                    table1.Cell(0, 0).SetFont(Fontbold1);

                    table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(0, 1).SetFont(Fontbold1);
                    table1.Cell(0, 1).SetContent("Valuation");

                    table1.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(1, 1).SetFont(Fontbold1);
                    if (valuationSettng == "1")
                        table1.Cell(1, 1).SetContent("I");
                    else
                        table1.Cell(1, 1).SetContent("II");

                    table1.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(2, 1).SetFont(Fontbold1);
                    table1.Cell(2, 1).SetContent("Max");

                    table1.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(3, 1).SetFont(Fontbold1);
                    table1.Cell(3, 1).SetContent(maxexternalmark);

                    table1.Rows[0].SetCellPadding(5);
                    table1.Rows[1].SetCellPadding(5);


                    foreach (PdfCell pr in table1.CellRange(0, 0, 0, 0).Cells)
                    {
                        pr.RowSpan = 4;
                    }
                    foreach (PdfCell pr in table1.CellRange(0, 1, 0, 1).Cells)
                    {
                        pr.ColSpan = 1;
                    }


                }

                string sno = fpspread.Sheets[0].Cells[row_cnt, 0].Text.ToString();
                string roll_noo = fpspread.Sheets[0].Cells[row_cnt, 1].Note.ToString();
                string roll_noo2 = fpspread.Sheets[0].Cells[row_cnt, 1].Text.ToString();
                string batchyr = fpspread.Sheets[0].Cells[row_cnt, 0].Tag.ToString();
                string e1 = fpspread.Sheets[0].Cells[row_cnt, 2].Text.ToString();
                string e2 = fpspread.Sheets[0].Cells[row_cnt, 3].Text.ToString();
                string ca = fpspread.Sheets[0].Cells[row_cnt, 4].Text.ToString();
                string ese = fpspread.Sheets[0].Cells[row_cnt, 5].Text.ToString();
                string totall = fpspread.Sheets[0].Cells[row_cnt, 6].Text.ToString();
                string results = fpspread.Sheets[0].Cells[row_cnt, 7].Text.ToString();

                val++;
                table1.Rows[val].SetCellPadding(5);
                table1.Cell(val, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                table1.Cell(val, 0).SetContent(roll_noo2);

                table1.Cell(val, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                if (valuationSettng == "1")
                    table1.Cell(val, 1).SetContent(e1);
                else
                    table1.Cell(val, 1).SetContent(e2);

            }

            myprov_pdfpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(myprovdoc, 150, coltop, 300, 650));
            myprov_pdfpage.Add(myprov_pdfpage1);

            myprov_pdfpage.Add(ptadepratment);
            myprov_pdfpage.Add(ptadatecur);
            myprov_pdfpage.Add(ptadate);
            myprov_pdfpage.Add(ptachairman);
            myprov_pdfpage.SaveToDocument();

            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "MARKSHEERTPRINT.pdf";

                myprovdoc.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    #region Common Checkbox and Checkboxlist Event

    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        StringBuilder selectedvalue = new StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append("'" + Convert.ToString(cblSelected.Items[sel].Value) + "'");
                    }
                    else
                    {
                        selectedvalue.Append(",'" + Convert.ToString(cblSelected.Items[sel].Value) + "'");
                    }
                }
            }
        }
        catch { }
        return selectedvalue.ToString();
    }

    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        StringBuilder selectedText = new StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append("'" + Convert.ToString(cblSelected.Items[sel].Text) + "'");
                    }
                    else
                    {
                        selectedText.Append(",'" + Convert.ToString(cblSelected.Items[sel].Text) + "'");
                    }
                }
            }
        }
        catch { }
        return selectedText.ToString();
    }

    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = string.Empty;
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
            string name = string.Empty;
            cb.Checked = false;
            txt.Text = deft;
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

    private void checkBoxListselectOrDeselect(CheckBoxList cbl, bool selected = true)
    {
        try
        {
            foreach (wc.ListItem li in cbl.Items)
            {
                li.Selected = selected;
            }
        }
        catch
        {
        }
    }

    private bool getSelectedCheckBoxListCount(CheckBoxList cbl, out int selectedCount)
    {
        selectedCount = 0;
        try
        {
            foreach (wc.ListItem li in cbl.Items)
            {
                if (li.Selected)
                {
                    selectedCount++;
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Developed By Malang Raja T
    /// </summary>
    /// <param name="c">Only Data Bound Controls eg.DropDownList,RadioButtonList,CheckBoxList </param>
    /// <param name="selectedValue"></param>
    /// <param name="selectedText"></param>
    /// <param name="type">0 - Index; 1 - Text; 2 - Value;</param>
    private void SelectDataBound(Control c, string selectedValue, string selectedText)
    {
        try
        {
            bool isDataBoundControl = false;
            if (c is DataBoundControl)
            {
                if (c is CheckBoxList || c is DropDownList || c is RadioButtonList)
                {
                    isDataBoundControl = true;
                }
                if (isDataBoundControl)
                {
                    ListControl lstControls = (ListControl)c;
                    if (lstControls.Items.Count > 0)
                    {
                        ListItem[] listItem = new ListItem[lstControls.Items.Count];
                        lstControls.Items.CopyTo(listItem, 0);
                        if (listItem.Contains(new ListItem(selectedText, selectedValue)))
                        {
                            lstControls.SelectedValue = selectedValue;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
}

    #endregion