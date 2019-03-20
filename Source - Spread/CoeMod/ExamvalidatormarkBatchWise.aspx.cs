using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.IO;
using Gios.Pdf;
using System.Globalization;
using System.Configuration;

public partial class ExamvalidatormarkBatchWise : System.Web.UI.Page
{
    string CollegeCode;
    Boolean yes_flag = false;
    DataView dv1 = new DataView();
    DataView dv2 = new DataView();
    DataView dv3 = new DataView();
    DAccess2 da = new DAccess2();
    DataSet ds2 = new DataSet();
    DataSet dsss = new DataSet();
    Hashtable hat = new Hashtable();

    DataSet dvget = new DataSet();

    string usercode = "", singleuser = "", group_user = "";

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
                chkmergrecol.Checked = true;
                string getcodeba = da.GetFunctionv("select value from COE_Master_Settings where settings = 'Direct_CIA'");
                if (getcodeba.Trim() != "")
                {
                    if (getcodeba.Trim() == "1")
                    {
                        chk_onlycia.Checked = true;
                    }
                    else
                    {
                        chk_onlycia.Checked = false;
                    }
                }

                lblaane.Visible = false;
                fpspread.Visible = false;
                ddlMonth1.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
                ddlMonth1.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Jan", "1"));
                ddlMonth1.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Feb", "2"));
                ddlMonth1.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Mar", "3"));
                ddlMonth1.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Apr", "4"));
                ddlMonth1.Items.Insert(5, new System.Web.UI.WebControls.ListItem("May", "5"));
                ddlMonth1.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jun", "6"));
                ddlMonth1.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Jul", "7"));
                ddlMonth1.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Aug", "8"));
                ddlMonth1.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Sep", "9"));
                ddlMonth1.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Oct", "10"));
                ddlMonth1.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Nov", "11"));
                ddlMonth1.Items.Insert(12, new System.Web.UI.WebControls.ListItem("Dec", "12"));

                int year1;
                year1 = Convert.ToInt16(DateTime.Today.Year);
                ddlYear1.Items.Clear();
                for (int l = 0; l <= 10; l++)
                {
                    ddlYear1.Items.Add(Convert.ToString(year1 - l));
                }
                ddlYear1.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
                loadtype();
                chkmoderation.Visible = false;
                btnexcelimport.Visible = false;
                fpmarkexcel.Visible = false;
                rbeval.Visible = false;
                rbcia.Visible = false;
                chkincluevel2.Visible = false;
                Session["MarkEntrySave"] = 1;
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void year1()
    {
        dsss.Clear();
        dsss = da.Examyear();
        if (dsss.Tables[0].Rows.Count > 0)
        {
            ddlYear1.DataSource = dsss;
            ddlYear1.DataTextField = "Exam_year";
            ddlYear1.DataValueField = "Exam_year";
            ddlYear1.DataBind();

        }
        ddlYear1.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
    }
    protected void month1()
    {
        try
        {
            dsss.Clear();
            string year1 = ddlYear1.SelectedValue;
            dsss = da.Exammonth(year1);
            if (dsss.Tables[0].Rows.Count > 0)
            {
                ddlMonth1.DataSource = dsss;
                ddlMonth1.DataTextField = "monthName";
                ddlMonth1.DataValueField = "Exam_month";
                ddlMonth1.DataBind();
            }
            ddlMonth1.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
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
        bindbatch();
        degree();
        bindbranch1();
        //  bindsem1();
    }
    public void loadtype()
    {
        try
        {
            string collegecode = Session["collegecode"].ToString();
            ddltype.Items.Clear();
            string strquery = "select distinct type from course where college_code='" + collegecode + "' and type is not null and type<>''";
            if (chkmergrecol.Checked == true)
            {
                strquery = "select distinct type from course where type is not null and type<>''";
            }
            DataSet dstype = da.select_method_wo_parameter(strquery, "Text");
            if (dstype.Tables[0].Rows.Count > 0)
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
            string branc = ddlbranch1.SelectedValue.ToString();
            string semmv = ddlsem1.SelectedValue.ToString();


            string typeval = "";
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                if (ddltype.Text != "")
                {
                    typeval = " and C.Type='" + ddltype.Text.ToString() + "'";
                }
            }

            string yearval = "", yearvalr = "";
            if (ddlbatch.Text != "All")
            {
                yearval = " and ed.batch_year='" + ddlbatch.Text.ToString() + "'";
                yearvalr = "and r.batch_year='" + ddlbatch.Text.ToString() + "'";
            }
            string qeryss = "SELECT distinct ss.subject_type,s.subject_name,s.subject_code,s.subject_name+' - '+s.subject_code as subnamecode FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,syllabus_master sy,sub_sem ss,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and sy.semester='" + semmv + "' and  ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' " + typeval + "  and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "' " + yearval + "";
            qeryss = qeryss + " union SELECT distinct ss.subject_type,s.subject_name,s.subject_code,s.subject_name+' - '+s.subject_code as subnamecode FROM subjectChooser sc,subject s,Registration r,Exam_Details ed,sub_sem ss,Degree d,Course c where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and sc.semester=ed.Current_Semester and s.subType_no=ss.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and sc.semester='" + semmv + "' and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' " + typeval + "  and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "'" + yearvalr + " and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' order by s.subject_name,s.subject_code desc";
            if (chksubwise.Checked == false)
            {
                qeryss = "SELECT distinct ss.subject_type,s.subject_name,s.subject_code,s.subject_name+' - '+s.subject_code as subnamecode FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,syllabus_master sy,sub_sem ss where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and ed.degree_code='" + branc + "' and sy.semester='" + semmv + "' and  ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "' " + yearval + "";
                qeryss = qeryss + " union SELECT distinct ss.subject_type,s.subject_name,s.subject_code,s.subject_name+' - '+s.subject_code as subnamecode FROM subjectChooser sc,subject s,Registration r,Exam_Details ed,sub_sem ss where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and sc.semester=ed.Current_Semester and s.subType_no=ss.subType_no and r.degree_code='" + branc + "'  and sc.semester='" + semmv + "' and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "'" + yearvalr + " and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' order by s.subject_name,s.subject_code desc";
            }
            if (chk_onlycia.Checked == true)
            {
                qeryss = "SELECT distinct s.subject_name,s.subject_code,s.subject_name+' - '+s.subject_code as subnamecode FROM subjectChooser sc,subject s,Registration r,Exam_Details ed,sub_sem ss where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and sc.semester=ed.Current_Semester and ss.subType_no=s.subType_no and r.degree_code='" + branc + "' and sc.semester='" + semmv + "' and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "'" + yearvalr + " order by s.subject_name,s.subject_code desc";
            }
            dsss = da.select_method(qeryss, hat, "Text");
            if (dsss.Tables[0].Rows.Count > 0)
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
            string branc = ddlbranch1.SelectedValue.ToString();
            string semmv = ddlsem1.SelectedValue.ToString();
            string typeval = "";
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                if (ddltype.Text != "")
                {
                    typeval = " and C.Type='" + ddltype.Text.ToString() + "'";
                }
            }
            string yearval = "";
            if (ddlbatch.Text != "All")
            {
                yearval = " and ed.batch_year='" + ddlbatch.Text.ToString() + "'";
            }
            string qeryss = "SELECT distinct ss.subject_type FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,syllabus_master sy,sub_sem ss,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and sy.semester='" + semmv + "' and  ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' " + typeval + "" + yearval + " ";
            qeryss = qeryss + " union SELECT distinct ss.subject_type FROM subjectChooser sc,subject s,Registration r,Exam_Details ed,sub_sem ss,Degree d,Course c where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and sc.semester=ed.Current_Semester and s.subType_no=ss.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and sc.semester='" + semmv + "' and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' " + typeval + "" + yearval + " and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' order by ss.subject_type";
            if (chksubwise.Checked == false)
            {
                qeryss = "SELECT distinct ss.subject_type FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,syllabus_master sy,sub_sem ss where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and ed.degree_code='" + branc + "' and sy.semester='" + semmv + "' and  ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "'" + yearval + " ";
                qeryss = qeryss + " union SELECT distinct ss.subject_type FROM subjectChooser sc,subject s,Registration r,Exam_Details ed,sub_sem ss where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and sc.semester=ed.Current_Semester and s.subType_no=ss.subType_no and r.degree_code='" + branc + "'  and sc.semester='" + semmv + "' and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "'" + yearval + " and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' order by ss.subject_type";
            }
            if (chk_onlycia.Checked == true)
            {
                qeryss = "SELECT distinct ss.subject_type FROM subjectChooser sc,subject s,Registration r,Exam_Details ed,sub_sem ss where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and sc.semester=ed.Current_Semester and ss.subType_no=s.subType_no and r.degree_code='" + branc + "' and sc.semester='" + semmv + "' and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "'" + yearval + " order by ss.subject_type";
            }
            dsss = da.select_method(qeryss, hat, "Text");
            if (dsss.Tables[0].Rows.Count > 0)
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
    protected void lb2_Click(object sender, EventArgs e)
    {
        string intime = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
        if (Session["Entry_Code"].ToString() == null)
        {
            Session["Entry_Code"] = 0;
        }
        int a = da.update_method_wo_parameter("update UserEELog  set Out_Time='" + intime + "',LogOff='1' where entry_code='" + Session["Entry_Code"] + "'", "Text");

        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
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
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            degree();
            bindbranch1();
            // bindsem1();
            ddlsubtype.Items.Clear();
            ddlSubject.Items.Clear();
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
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
    public void bindbatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            ds2 = da.BindBatch();
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds2;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();
                ddlbatch.Items.Insert(0, "All");
            }
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
            string type = "";
            if (ddltype.Enabled == true)
            {
                if (ddltype.Items.Count > 0)
                {
                    if (ddltype.SelectedItem.ToString() != "All" && ddltype.SelectedItem.ToString() != "")
                    {
                        type = " and course.type='" + ddltype.SelectedItem.ToString() + "'";
                    }
                }
            }
            string codevalues = "";
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
            if (chkmergrecol.Checked == true)
            {
                strquery = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where  course.course_id=degree.course_id and course.college_code = degree.college_code and   deptprivilages.Degree_code=degree.Degree_code " + codevalues + " " + type + " ";
            }
            ds2 = da.select_method_wo_parameter(strquery, "Text");
            if (ds2.Tables[0].Rows.Count > 0)
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
            if (chkmergrecol.Checked == true)
            {
                collegecode = da.GetFunctionv("select college_code from Course where Course_Id='" + ddldegree1.SelectedValue + "'");
            }
            hat.Add("single_user", singleuser.ToString());
            hat.Add("group_code", group_user);
            hat.Add("course_id", ddldegree1.SelectedValue);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);

            DataSet ds = da.select_method("bind_branch", hat, "sp");
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {

                ddlbranch1.DataSource = ds;
                ddlbranch1.DataTextField = "dept_name";
                ddlbranch1.DataValueField = "degree_code";
                ddlbranch1.DataBind();
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
            if (chkmergrecol.Checked == true)
            {
                collegecode = da.GetFunctionv("select college_code from Course where Course_Id='" + ddldegree1.SelectedValue + "'");
            }
            DataSet ds = new DataSet();
            if (chksubwise.Checked == true)
            {
                string strsql = "select Max(ndurations),first_year_nonsemester from ndegree where college_code=" + collegecode + " group by first_year_nonsemester order by Max(ndurations) ";
                ds = da.select_method_wo_parameter(strsql, "TExt");
            }
            else
            {
                ds = da.BindSem(ddlbranch1.SelectedValue.ToString(), ddlYear1.SelectedValue.ToString(), collegecode);
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0][1]).ToString());
                duration = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0][0]).ToString());
                for (i = 1; i <= duration; i++)
                {
                    //if (first_year == false)
                    //{
                    ddlsem1.Items.Add(i.ToString());
                    //}
                    //else if (first_year == true && i != 2)
                    //{
                    //    ddlsem1.Items.Add(i.ToString());
                    //}
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
    protected void ddlMonth1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            ddlsubtype.Items.Clear();
            ddlSubject.Items.Clear();
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
            //month1();
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
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }

    }

    protected void btnreset_print(object sender, EventArgs e)
    {
        try
        {
            fpspread.SaveChanges();
            for (int r = 0; r < fpspread.Sheets[0].RowCount; r++)
            {
                fpspread.Sheets[0].Cells[r, 2].Text = "";
                fpspread.Sheets[0].Cells[r, 3].Text = "";
                fpspread.Sheets[0].Cells[r, 4].Text = "";
                fpspread.Sheets[0].Cells[r, 5].Text = "";
                fpspread.Sheets[0].Cells[r, 6].Text = "";
                fpspread.Sheets[0].Cells[r, 8].Text = "";
                fpspread.Sheets[0].Cells[r, 9].Text = "";
                fpspread.Sheets[0].Cells[r, 10].Text = "";
            }
            marksavefunction();
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Marks Deleted Successfully')", true);
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
            Font Fontbold123 = new Font("Times New Roman", 15, FontStyle.Bold);
            Font Fontbold = new Font("Times New Roman", 15, FontStyle.Bold);
            Font Fontsmall = new Font("Times New Roman", 10, FontStyle.Regular);
            Font Fontbold1 = new Font("Times New Roman", 10, FontStyle.Bold);
            Gios.Pdf.PdfDocument myprovdoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage myprov_pdfpage = myprovdoc.NewPage();

            int coltop = 5;
            string collegename = "";

            string examexternalmark = fpspread.Sheets[0].ColumnHeader.Cells[2, 2].Tag.ToString();
            string maxexternalmark = fpspread.Sheets[0].ColumnHeader.Cells[2, 8].Tag.ToString();
            string maxinternalmark = fpspread.Sheets[0].ColumnHeader.Cells[2, 7].Tag.ToString();
            string totlmatrk = fpspread.Sheets[0].ColumnHeader.Cells[2, 9].Tag.ToString();
            string modrationva = fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Tag.ToString() + " %";


            string catgory = "";
            string coll_name = "";
            string ugorpg = "";
            string str = "select isnull(collname,'') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1,'') as address1,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code='" + Session["collegecode"] + "'";
            ds2 = da.select_method_wo_parameter(str, "Text");
            if (ds2.Tables[0].Rows.Count > 0)
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
            if (chksubwise.Checked == false)
            {
                ptss2 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                 new PdfArea(myprovdoc, 20, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Class & Group : " + ddldegree1.SelectedItem.Text.ToString() + "-" + ddlbranch1.SelectedItem.Text.ToString());
            }
            else
            {
                ptss2 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                   new PdfArea(myprovdoc, 20, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Class & Group : All Groups");
            }
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

            table1 = myprovdoc.NewTable(Fontsmall, noofrowpertable, 10, 1);
            table1.VisibleHeaders = false;

            table1.VisibleHeaders = false;
            table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
            table1.Columns[0].SetWidth(100);
            table1.Columns[1].SetWidth(80);
            table1.Columns[2].SetWidth(80);
            table1.Columns[3].SetWidth(80);
            table1.Columns[4].SetWidth(80);
            table1.Columns[5].SetWidth(80);
            table1.Columns[6].SetWidth(80);
            table1.Columns[7].SetWidth(80);
            table1.Columns[8].SetWidth(80);
            table1.Columns[9].SetWidth(80);



            table1.CellRange(0, 0, 0, 5).SetFont(Fontsmall);
            table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(0, 0).SetContent("Reg No");
            table1.Cell(0, 0).SetFont(Fontbold1);

            table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(0, 1).SetFont(Fontbold1);
            table1.Cell(0, 1).SetContent("Valuation");

            table1.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(1, 1).SetFont(Fontbold1);
            table1.Cell(1, 1).SetContent("I");



            table1.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(2, 1).SetFont(Fontbold1);
            table1.Cell(2, 1).SetContent("Max");

            table1.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(1, 2).SetFont(Fontbold1);
            table1.Cell(1, 2).SetContent("II");

            table1.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(2, 2).SetFont(Fontbold1);
            table1.Cell(2, 2).SetContent("Max");

            table1.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(3, 1).SetFont(Fontbold1);
            table1.Cell(3, 1).SetContent(examexternalmark);

            table1.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(3, 2).SetFont(Fontbold1);
            table1.Cell(3, 2).SetContent(examexternalmark);

            table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(0, 3).SetFont(Fontbold1);
            table1.Cell(0, 3).SetContent("Average");

            table1.Cell(2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(2, 3).SetFont(Fontbold1);
            table1.Cell(2, 3).SetContent("Max");

            table1.Cell(3, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(3, 3).SetFont(Fontbold1);
            table1.Cell(3, 3).SetContent(examexternalmark);

            table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(0, 4).SetFont(Fontbold1);
            table1.Cell(0, 4).SetContent("Valuation	");

            table1.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(1, 4).SetFont(Fontbold1);
            table1.Cell(1, 4).SetContent("III");

            table1.Cell(2, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(2, 4).SetFont(Fontbold1);
            table1.Cell(2, 4).SetContent("Max	");

            table1.Cell(3, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(3, 4).SetFont(Fontbold1);
            table1.Cell(3, 4).SetContent(examexternalmark);

            table1.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(0, 5).SetFont(Fontbold1);
            table1.Cell(0, 5).SetContent("Moderation");

            table1.Cell(2, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(2, 5).SetFont(Fontbold1);
            table1.Cell(2, 5).SetContent("Max  ");

            table1.Cell(3, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(3, 5).SetFont(Fontbold1);
            table1.Cell(3, 5).SetContent(modrationva);

            table1.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(0, 6).SetFont(Fontbold1);
            table1.Cell(0, 6).SetContent("I.C.A");

            table1.Cell(2, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(2, 6).SetFont(Fontbold1);
            table1.Cell(2, 6).SetContent("Max");

            table1.Cell(3, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(3, 6).SetFont(Fontbold1);
            table1.Cell(3, 6).SetContent(maxinternalmark);

            table1.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(0, 7).SetFont(Fontbold1);
            table1.Cell(0, 7).SetContent("E.S.E	");

            table1.Cell(2, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(2, 7).SetFont(Fontbold1);
            table1.Cell(2, 7).SetContent("Max");

            table1.Cell(3, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(3, 7).SetFont(Fontbold1);
            table1.Cell(3, 7).SetContent(maxexternalmark);

            table1.Cell(0, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(0, 8).SetFont(Fontbold1);
            table1.Cell(0, 8).SetContent("Total");

            table1.Cell(2, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(2, 8).SetFont(Fontbold1);
            table1.Cell(2, 8).SetContent("Max");

            table1.Cell(3, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(3, 8).SetFont(Fontbold1);
            table1.Cell(3, 8).SetContent(totlmatrk);

            table1.Cell(0, 9).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(0, 9).SetFont(Fontbold1);
            table1.Cell(0, 9).SetContent("Result");

            table1.Rows[0].SetCellPadding(5);
            table1.Rows[1].SetCellPadding(5);
            table1.Rows[2].SetCellPadding(5);
            table1.Rows[3].SetCellPadding(5);

            if (chkincluevel2.Checked == true)
            {
                table1.Columns[2].SetWidth(1);
                table1.Cell(1, 2).SetContent("");
                table1.Cell(2, 2).SetContent("");
                table1.Cell(3, 2).SetContent("");

                table1.Columns[4].SetWidth(1);
                table1.Cell(1, 4).SetContent("");
                table1.Cell(2, 4).SetContent("");
                table1.Cell(3, 4).SetContent("");
                table1.Cell(0, 4).SetContent("");
            }


            foreach (PdfCell pr in table1.CellRange(0, 0, 0, 0).Cells)
            {
                pr.RowSpan = 4;
            }
            foreach (PdfCell pr in table1.CellRange(0, 1, 0, 1).Cells)
            {
                pr.ColSpan = 2;
            }
            foreach (PdfCell pr in table1.CellRange(0, 3, 0, 3).Cells)
            {
                pr.RowSpan = 2;
            }
            foreach (PdfCell pr in table1.CellRange(0, 5, 0, 5).Cells)
            {
                pr.RowSpan = 2;
            }
            foreach (PdfCell pr in table1.CellRange(0, 6, 0, 6).Cells)
            {
                pr.RowSpan = 2;
            }
            foreach (PdfCell pr in table1.CellRange(0, 7, 0, 7).Cells)
            {
                pr.RowSpan = 2;
            }
            foreach (PdfCell pr in table1.CellRange(0, 8, 0, 8).Cells)
            {
                pr.RowSpan = 2;
            }
            foreach (PdfCell pr in table1.CellRange(0, 9, 0, 9).Cells)
            {
                pr.RowSpan = 4;
            }

            Gios.Pdf.PdfTablePage myprov_pdfpage1;

            for (int row_cnt = 0; row_cnt < fpspread.Sheets[0].RowCount; row_cnt++)
            {
                if ((row_cnt % 25) == 0 && row_cnt > 0)
                {
                    myprov_pdfpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(myprovdoc, 20, coltop, 570, 650));
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
                    table1 = myprovdoc.NewTable(Fontsmall, noofrowpertable + 4, 10, 1);

                    table1.VisibleHeaders = false;
                    table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                    table1.Columns[0].SetWidth(100);
                    table1.Columns[1].SetWidth(80);
                    table1.Columns[2].SetWidth(80);
                    table1.Columns[3].SetWidth(80);
                    table1.Columns[4].SetWidth(80);
                    table1.Columns[5].SetWidth(80);
                    table1.Columns[6].SetWidth(80);
                    table1.Columns[7].SetWidth(80);
                    table1.Columns[8].SetWidth(80);
                    table1.Columns[9].SetWidth(80);

                    table1.CellRange(0, 0, 0, 5).SetFont(Fontsmall);
                    table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(0, 0).SetContent("Reg No");
                    table1.Cell(0, 0).SetFont(Fontbold1);

                    table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(0, 1).SetFont(Fontbold1);
                    table1.Cell(0, 1).SetContent("Valuation");

                    table1.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(1, 1).SetFont(Fontbold1);
                    table1.Cell(1, 1).SetContent("I");

                    table1.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(2, 1).SetFont(Fontbold1);
                    table1.Cell(2, 1).SetContent("Max");

                    table1.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(1, 2).SetFont(Fontbold1);
                    table1.Cell(1, 2).SetContent("II");

                    table1.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(2, 2).SetFont(Fontbold1);
                    table1.Cell(2, 2).SetContent("Max");

                    table1.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(3, 1).SetFont(Fontbold1);
                    table1.Cell(3, 1).SetContent(examexternalmark);

                    table1.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(3, 2).SetFont(Fontbold1);
                    table1.Cell(3, 2).SetContent(examexternalmark);

                    table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(0, 3).SetFont(Fontbold1);
                    table1.Cell(0, 3).SetContent("Average");

                    table1.Cell(2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(2, 3).SetFont(Fontbold1);
                    table1.Cell(2, 3).SetContent("Max");

                    table1.Cell(3, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(3, 3).SetFont(Fontbold1);
                    table1.Cell(3, 3).SetContent(examexternalmark);

                    table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(0, 4).SetFont(Fontbold1);
                    table1.Cell(0, 4).SetContent("Valuation	");

                    table1.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(1, 4).SetFont(Fontbold1);
                    table1.Cell(1, 4).SetContent("III");

                    table1.Cell(2, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(2, 4).SetFont(Fontbold1);
                    table1.Cell(2, 4).SetContent("Max	");

                    table1.Cell(3, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(3, 4).SetFont(Fontbold1);
                    table1.Cell(3, 4).SetContent(examexternalmark);

                    table1.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(0, 5).SetFont(Fontbold1);
                    table1.Cell(0, 5).SetContent("Moderation");

                    table1.Cell(2, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(2, 5).SetFont(Fontbold1);
                    table1.Cell(2, 5).SetContent("Max  ");

                    table1.Cell(3, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(3, 5).SetFont(Fontbold1);
                    table1.Cell(3, 5).SetContent(modrationva);

                    table1.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(0, 6).SetFont(Fontbold1);
                    table1.Cell(0, 6).SetContent("I.C.A");

                    table1.Cell(2, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(2, 6).SetFont(Fontbold1);
                    table1.Cell(2, 6).SetContent("Max");

                    table1.Cell(3, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(3, 6).SetFont(Fontbold1);
                    table1.Cell(3, 6).SetContent(maxinternalmark);

                    table1.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(0, 7).SetFont(Fontbold1);
                    table1.Cell(0, 7).SetContent("E.S.E	");

                    table1.Cell(2, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(2, 7).SetFont(Fontbold1);
                    table1.Cell(2, 7).SetContent("Max");

                    table1.Cell(3, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(3, 7).SetFont(Fontbold1);
                    table1.Cell(3, 7).SetContent(maxexternalmark);

                    table1.Cell(0, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(0, 8).SetFont(Fontbold1);
                    table1.Cell(0, 8).SetContent("Total");

                    table1.Cell(2, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(2, 8).SetFont(Fontbold1);
                    table1.Cell(2, 8).SetContent("Max");

                    table1.Cell(3, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(3, 8).SetFont(Fontbold1);
                    table1.Cell(3, 8).SetContent(totlmatrk);

                    table1.Cell(0, 9).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(0, 9).SetFont(Fontbold1);
                    table1.Cell(0, 9).SetContent("Result");

                    table1.Rows[0].SetCellPadding(5);
                    table1.Rows[1].SetCellPadding(5);
                    table1.Rows[2].SetCellPadding(5);
                    table1.Rows[3].SetCellPadding(5);

                    if (chkincluevel2.Checked == true)
                    {
                        table1.Columns[2].SetWidth(1);
                        table1.Cell(1, 2).SetContent("");
                        table1.Cell(2, 2).SetContent("");
                        table1.Cell(3, 2).SetContent("");

                        table1.Columns[4].SetWidth(1);
                        table1.Cell(1, 4).SetContent("");
                        table1.Cell(2, 4).SetContent("");
                        table1.Cell(3, 4).SetContent("");
                        table1.Cell(0, 4).SetContent("");
                    }

                    foreach (PdfCell pr in table1.CellRange(0, 0, 0, 0).Cells)
                    {
                        pr.RowSpan = 4;
                    }
                    foreach (PdfCell pr in table1.CellRange(0, 1, 0, 1).Cells)
                    {
                        pr.ColSpan = 2;
                    }
                    foreach (PdfCell pr in table1.CellRange(0, 3, 0, 3).Cells)
                    {
                        pr.RowSpan = 2;
                    }
                    foreach (PdfCell pr in table1.CellRange(0, 5, 0, 5).Cells)
                    {
                        pr.RowSpan = 2;
                    }
                    foreach (PdfCell pr in table1.CellRange(0, 6, 0, 6).Cells)
                    {
                        pr.RowSpan = 2;
                    }
                    foreach (PdfCell pr in table1.CellRange(0, 7, 0, 7).Cells)
                    {
                        pr.RowSpan = 2;
                    }
                    foreach (PdfCell pr in table1.CellRange(0, 8, 0, 8).Cells)
                    {
                        pr.RowSpan = 2;
                    }
                    foreach (PdfCell pr in table1.CellRange(0, 9, 0, 9).Cells)
                    {
                        pr.RowSpan = 4;
                    }
                }

                string sno = fpspread.Sheets[0].Cells[row_cnt, 0].Text.ToString();
                string roll_noo = fpspread.Sheets[0].Cells[row_cnt, 1].Note.ToString();
                string roll_noo2 = fpspread.Sheets[0].Cells[row_cnt, 1].Text.ToString();
                string batchyr = fpspread.Sheets[0].Cells[row_cnt, 1].Tag.ToString();
                string e1 = fpspread.Sheets[0].Cells[row_cnt, 2].Text.ToString();
                string e2 = fpspread.Sheets[0].Cells[row_cnt, 3].Text.ToString();
                string avreage = fpspread.Sheets[0].Cells[row_cnt, 4].Text.ToString();
                string e3 = fpspread.Sheets[0].Cells[row_cnt, 5].Text.ToString();
                string moderon = fpspread.Sheets[0].Cells[row_cnt, 6].Text.ToString();
                string ca = fpspread.Sheets[0].Cells[row_cnt, 7].Text.ToString();
                string ese = fpspread.Sheets[0].Cells[row_cnt, 8].Text.ToString();
                string totall = fpspread.Sheets[0].Cells[row_cnt, 9].Text.ToString();
                string results = fpspread.Sheets[0].Cells[row_cnt, 10].Text.ToString();

                val++;
                table1.Rows[val].SetCellPadding(5);
                table1.Cell(val, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                table1.Cell(val, 0).SetContent(roll_noo2);

                table1.Cell(val, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                table1.Cell(val, 1).SetContent(e1);

                table1.Cell(val, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                if (chkincluevel2.Checked == false)
                {
                    table1.Cell(val, 2).SetContent(e2);
                }

                table1.Cell(val, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                table1.Cell(val, 3).SetContent(avreage);

                table1.Cell(val, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                if (chkincluevel2.Checked == false)
                {
                    table1.Cell(val, 4).SetContent(e3);
                }

                table1.Cell(val, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                table1.Cell(val, 5).SetContent(moderon);

                table1.Cell(val, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                table1.Cell(val, 6).SetContent(ca);

                table1.Cell(val, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                table1.Cell(val, 7).SetContent(ese);

                table1.Cell(val, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                table1.Cell(val, 8).SetContent(totall);

                table1.Cell(val, 9).SetContentAlignment(ContentAlignment.MiddleCenter);
                table1.Cell(val, 9).SetContent(results);
            }

            myprov_pdfpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(myprovdoc, 20, coltop, 570, 650));
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

    protected void btnprintt_t1_print(object sender, EventArgs e)
    {
        try
        {
            int colinc = 0;
            int snoo = 0;
            string passingminmax = "0";
            string totalpassingminmax = "0";
            Font Fontbold123 = new Font("Times New Roman", 15, FontStyle.Bold);
            Font Fontbold = new Font("Times New Roman", 15, FontStyle.Bold);
            Font Fontsmall = new Font("Times New Roman", 10, FontStyle.Regular);
            Font Fontbold1 = new Font("Times New Roman", 10, FontStyle.Bold);
            Gios.Pdf.PdfDocument myprovdoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage myprov_pdfpage = myprovdoc.NewPage();

            int coltop = 5;
            string collegename = "";

            string examexternalmark = fpspread.Sheets[0].ColumnHeader.Cells[2, 2].Tag.ToString();
            string maxexternalmark = fpspread.Sheets[0].ColumnHeader.Cells[2, 8].Tag.ToString();
            string maxinternalmark = fpspread.Sheets[0].ColumnHeader.Cells[2, 7].Tag.ToString();
            string totlmatrk = fpspread.Sheets[0].ColumnHeader.Cells[2, 9].Tag.ToString();
            string modrationva = fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Tag.ToString() + " %";


            string catgory = "";
            string coll_name = "";
            string ugorpg = "";
            string aff = "";
            string str = "select isnull(collname,'') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1,'') as address1,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code='" + Session["collegecode"] + "'";
            ds2 = da.select_method_wo_parameter(str, "Text");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                catgory = ds2.Tables[0].Rows[0]["category"].ToString();
                coll_name = Convert.ToString(ds2.Tables[0].Rows[0]["collname"]);
                aff = ds2.Tables[0].Rows[0]["affliated"].ToString();
            }
            ugorpg = da.GetFunction("select distinct c.Edu_Level from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.ToString() + "' and s.subject_code='" + ddlSubject.SelectedValue.ToString() + "'");

            collegename = coll_name + " (" + catgory + ")";




            PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                             new PdfArea(myprovdoc, 20, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleCenter, coll_name);

            coltop = coltop + 15;
            PdfTextArea pts = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(myprovdoc, 20, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleCenter, aff);
            coltop = coltop + 15;
            PdfTextArea ptss = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                 new PdfArea(myprovdoc, 20, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleCenter, "OFFICE OF THE CONTROLLER OF EXAMINATIONS");


            coltop = coltop + 15;
            string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(ddlMonth1.SelectedIndex.ToString()));
            PdfTextArea ptss1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                 new PdfArea(myprovdoc, 20, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleCenter, "Passing Board Report for Semester Examination  - " + strMonthName + " - " + ddlYear1.SelectedItem.Text.ToString());


            coltop = coltop + 20;
            PdfTextArea ptss2 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                 new PdfArea(myprovdoc, 20, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, " ");
            if (chksubwise.Checked == false)
            {
                ptss2 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                 new PdfArea(myprovdoc, 20, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Class & Group : " + ddldegree1.SelectedItem.Text.ToString() + "-" + ddlbranch1.SelectedItem.Text.ToString());
            }
            else
            {
                ptss2 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                   new PdfArea(myprovdoc, 20, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Class & Group : All Groups");
            }
            PdfTextArea ptss2aa = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                             new PdfArea(myprovdoc, 350, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Batch / Semester : " + ddlbatch.SelectedItem.Text.ToString() + " / " + ddlsem1.SelectedItem.Text.ToString());

            int pasmintopval = coltop - 15;

            string gettext = da.GetFunction("select value from COE_Master_Settings where settings='" + ugorpg + " Passing Minimum'");
            string[] stva = gettext.Split('~');
            if (gettext.Trim() != "" && gettext.Trim() != "0")
            {
                for (int c = 0; c <= stva.GetUpperBound(0); c++)
                {
                    PdfTextArea ptsspassmin = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
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
            PdfTextArea ptss22 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                 new PdfArea(myprovdoc, 20, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Paper Name : " + subjectname);


            PdfTextArea ptss3 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                             new PdfArea(myprovdoc, 350, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Paper Code : " + ddlSubject.SelectedValue.ToString());

            PdfTextArea ptss31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                             new PdfArea(myprovdoc, 350, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "");

            coltop = coltop + 30;


            PdfTextArea ptadate = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                             new PdfArea(myprovdoc, 10, 780, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "");

            PdfTextArea ptadatecur = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                             new PdfArea(myprovdoc, 10, 800, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "");

            PdfTextArea ptadepratment = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                            new PdfArea(myprovdoc, 275, 780, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "");

            PdfTextArea ptachairman = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                            new PdfArea(myprovdoc, 380, 780, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "CONTROLLER OF EXAMINATIONS");


            string data = "";

            myprov_pdfpage.Add(ptc);
            myprov_pdfpage.Add(pts);
            myprov_pdfpage.Add(ptss);
            myprov_pdfpage.Add(ptss1);
            myprov_pdfpage.Add(ptss2);
            myprov_pdfpage.Add(ptss2aa);
            #region Left Logo
            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
            {
                PdfImage LogoImage = myprovdoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                myprov_pdfpage.Add(LogoImage, 20, 5, 400);
            }
            #endregion
            myprov_pdfpage.Add(ptss22);
            myprov_pdfpage.Add(ptss3);
            myprov_pdfpage.Add(ptss31);

            myprov_pdfpage.Add(ptadepratment);
            myprov_pdfpage.Add(ptadatecur);
            myprov_pdfpage.Add(ptadate);
            myprov_pdfpage.Add(ptachairman);

            Gios.Pdf.PdfTable table1;
            int val = 0;
            int noofrowpertable = 0;
            if (fpspread.Sheets[0].RowCount > 25)
            {
                noofrowpertable = 35 - 3;
            }
            else
            {
                noofrowpertable = fpspread.Sheets[0].RowCount + 1;
            }

            table1 = myprovdoc.NewTable(Fontsmall, noofrowpertable, 10, 3);
            table1.VisibleHeaders = false;

            table1.VisibleHeaders = false;
            table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

            table1.Columns[0].SetWidth(80);
            colinc = 1;
            table1.Columns[colinc + 0].SetWidth(100);
            table1.Columns[colinc + 1].SetWidth(80);
            table1.Columns[colinc + 2].SetWidth(80);
            table1.Columns[colinc + 3].SetWidth(80);
            table1.Columns[colinc + 4].SetWidth(80);
            table1.Columns[colinc + 5].SetWidth(80);
            table1.Columns[colinc + 6].SetWidth(80);
            table1.Columns[colinc + 7].SetWidth(80);
            table1.Columns[colinc + 8].SetWidth(80);
            table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(0, 0).SetContent("S.No.");
            table1.Cell(0, 0).SetFont(Fontbold1);
            colinc = 1;
            table1.Cell(0, colinc + 0).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(0, colinc + 0).SetContent("Reg No.");
            table1.Cell(0, colinc + 0).SetFont(Fontbold1);

            table1.Cell(0, colinc + 1).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(0, colinc + 1).SetFont(Fontbold1);
            table1.Cell(0, colinc + 1).SetContent("VAL I");


            table1.Cell(0, colinc + 2).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(0, colinc + 2).SetFont(Fontbold1);
            table1.Cell(0, colinc + 2).SetContent("VAL II");


            table1.Cell(0, colinc + 3).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(0, colinc + 3).SetFont(Fontbold1);
            table1.Cell(0, colinc + 3).SetContent("VAL III");
            table1.Cell(0, colinc + 4).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(0, colinc + 4).SetFont(Fontbold1);
            if (fpspread.Sheets[0].RowCount > 0)
            {
                passingminmax = fpspread.Sheets[0].Cells[0, 7].Note.ToString();
                totalpassingminmax = fpspread.Sheets[0].Cells[0, 8].Tag.ToString();
                data = da.GetFunctionv("select distinct min_ext_marks from subject where subject_code='" + ddlSubject.SelectedItem.Value.ToString() + "'");
                data = data + " / " + da.GetFunctionv("select distinct max_ext_marks from subject where subject_code='" + ddlSubject.SelectedItem.Value.ToString() + "'");
                table1.Cell(0, colinc + 4).SetContent("Final Marks (Passing Minimum " + ugorpg + "  - " + data + ")");
            }
            table1.Cell(0, colinc + 5).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(0, colinc + 5).SetFont(Fontbold1);
            table1.Cell(0, colinc + 5).SetContent("CIA Marks");

            table1.Cell(0, colinc + 6).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(0, colinc + 6).SetFont(Fontbold1);
            if (fpspread.Sheets[0].RowCount > 0)
            {
                passingminmax = fpspread.Sheets[0].Cells[0, 7].Tag.ToString();
                totalpassingminmax = fpspread.Sheets[0].Cells[0, 8].Tag.ToString();
                data = da.GetFunctionv("select distinct mintotal from subject where subject_code='" + ddlSubject.SelectedItem.Value.ToString() + "'");
                data = data + " / " + da.GetFunctionv("select distinct maxtotal from subject where subject_code='" + ddlSubject.SelectedItem.Value.ToString() + "'");
                table1.Cell(0, colinc + 6).SetContent("Total (Passing Minimum " + ugorpg + "  - " + data + ")");
            }
            table1.Cell(0, colinc + 7).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(0, colinc + 7).SetFont(Fontbold1);
            table1.Cell(0, colinc + 7).SetContent("Result Pass / Fail / RA");

            table1.Cell(0, colinc + 8).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1.Cell(0, colinc + 8).SetFont(Fontbold1);
            table1.Cell(0, colinc + 8).SetContent("Deficit");


            if (chkincluevel2.Checked == true)
            {
                table1.Columns[colinc + 2].SetWidth(1);
                table1.Cell(1, colinc + 2).SetContent("");
                table1.Cell(2, colinc + 2).SetContent("");
                table1.Cell(3, colinc + 2).SetContent("");

                table1.Columns[colinc + 4].SetWidth(1);
                table1.Cell(1, colinc + 4).SetContent("");
                table1.Cell(2, colinc + 4).SetContent("");
                table1.Cell(3, colinc + 4).SetContent("");
                table1.Cell(0, colinc + 4).SetContent("");
            }


            Gios.Pdf.PdfTablePage myprov_pdfpage1;

            for (int row_cnt = 0; row_cnt < fpspread.Sheets[0].RowCount; row_cnt++)
            {
                if ((row_cnt % 29) == 0 && row_cnt > 0)
                {
                    myprov_pdfpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(myprovdoc, 20, coltop, 570, 650));
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
                    #region Left Logo
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                    {
                        PdfImage LogoImage = myprovdoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                        myprov_pdfpage.Add(LogoImage, 20, 5, 400);
                    }
                    #endregion
                    myprov_pdfpage.Add(ptss2);
                    myprov_pdfpage.Add(ptss2aa);
                    #region Left Logo
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                    {
                        PdfImage LogoImage = myprovdoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                        myprov_pdfpage.Add(LogoImage, 20, 5, 400);
                    }
                    #endregion
                    myprov_pdfpage.Add(ptss22);
                    myprov_pdfpage.Add(ptss3);
                    myprov_pdfpage.Add(ptss31);

                    myprov_pdfpage.Add(ptadepratment);
                    myprov_pdfpage.Add(ptadatecur);
                    myprov_pdfpage.Add(ptadate);
                    myprov_pdfpage.Add(ptachairman);

                    noofrowpertable = 0;
                    if (fpspread.Sheets[0].RowCount > row_cnt + 30)
                    {
                        noofrowpertable = 30;
                    }
                    else
                    {
                        noofrowpertable = fpspread.Sheets[0].RowCount - row_cnt;
                    }

                    val = 0;
                    table1.VisibleHeaders = false;
                    table1 = myprovdoc.NewTable(Fontsmall, noofrowpertable + 1, 10, 3);

                    table1.VisibleHeaders = false;
                    table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                    table1.Columns[0].SetWidth(80);
                    colinc = 1;
                    table1.Columns[colinc + 0].SetWidth(100);
                    table1.Columns[colinc + 1].SetWidth(80);
                    table1.Columns[colinc + 2].SetWidth(80);
                    table1.Columns[colinc + 3].SetWidth(80);
                    table1.Columns[colinc + 4].SetWidth(80);
                    table1.Columns[colinc + 5].SetWidth(80);
                    table1.Columns[colinc + 6].SetWidth(80);
                    table1.Columns[colinc + 7].SetWidth(80);
                    table1.Columns[colinc + 8].SetWidth(80);

                    table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(0, 0).SetContent("S.No.");
                    table1.Cell(0, 0).SetFont(Fontbold1);
                    colinc = 1;
                    table1.Cell(0, colinc + 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(0, colinc + 0).SetContent("Reg No");
                    table1.Cell(0, colinc + 0).SetFont(Fontbold1);

                    table1.Cell(0, colinc + 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(0, colinc + 1).SetFont(Fontbold1);
                    table1.Cell(0, colinc + 1).SetContent("Int. Eval.");

                    table1.Cell(0, colinc + 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(0, colinc + 2).SetFont(Fontbold1);
                    table1.Cell(0, colinc + 2).SetContent("Ext. Eval.");


                    table1.Cell(0, colinc + 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(0, colinc + 3).SetFont(Fontbold1);
                    table1.Cell(0, colinc + 3).SetContent("Third Eval.");


                    table1.Cell(0, colinc + 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(0, colinc + 4).SetFont(Fontbold1);
                    if (fpspread.Sheets[0].RowCount > 0)
                    {
                        passingminmax = fpspread.Sheets[0].Cells[0, 7].Note.ToString();
                        totalpassingminmax = fpspread.Sheets[0].Cells[0, 8].Tag.ToString();
                        data = da.GetFunctionv("select distinct min_ext_marks from subject where subject_code='" + ddlSubject.SelectedItem.Value.ToString() + "'");
                        data = data + " / " + da.GetFunctionv("select distinct max_ext_marks from subject where subject_code='" + ddlSubject.SelectedItem.Value.ToString() + "'");
                        table1.Cell(0, colinc + 4).SetContent("Final Marks (Passing Minimum " + ugorpg + "  - " + data + ")");
                    }


                    table1.Cell(0, colinc + 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(0, colinc + 5).SetFont(Fontbold1);
                    table1.Cell(0, colinc + 5).SetContent("CIA Marks");

                    table1.Cell(0, colinc + 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(0, colinc + 6).SetFont(Fontbold1);
                    if (fpspread.Sheets[0].RowCount > 0)
                    {
                        passingminmax = fpspread.Sheets[0].Cells[0, 7].Tag.ToString();
                        totalpassingminmax = fpspread.Sheets[0].Cells[0, 8].Tag.ToString();

                        data = da.GetFunctionv("select distinct mintotal from subject where subject_code='" + ddlSubject.SelectedItem.Value.ToString() + "'");
                        data = data + " / " + da.GetFunctionv("select distinct maxtotal from subject where subject_code='" + ddlSubject.SelectedItem.Value.ToString() + "'");
                        table1.Cell(0, colinc + 6).SetContent("Total (Passing Minimum " + ugorpg + "  - " + data + ")");
                    }


                    table1.Cell(0, colinc + 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(0, colinc + 7).SetFont(Fontbold1);
                    table1.Cell(0, colinc + 7).SetContent("Result Pass / Fail / RA");



                    table1.Cell(0, colinc + 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(0, colinc + 8).SetFont(Fontbold1);
                    table1.Cell(0, colinc + 8).SetContent("Deficit");


                    //table1.Rows[0].SetCellPadding(5);
                    //table1.Rows[1].SetCellPadding(5);
                    //table1.Rows[2].SetCellPadding(5);
                    //table1.Rows[3].SetCellPadding(5);

                    if (chkincluevel2.Checked == true)
                    {
                        table1.Columns[colinc + 2].SetWidth(1);
                        table1.Cell(1, colinc + 2).SetContent("");
                        table1.Cell(2, colinc + 2).SetContent("");
                        table1.Cell(3, colinc + 2).SetContent("");

                        table1.Columns[colinc + 4].SetWidth(1);
                        table1.Cell(1, colinc + 4).SetContent("");
                        table1.Cell(2, colinc + 4).SetContent("");
                        table1.Cell(3, colinc + 4).SetContent("");
                        table1.Cell(0, colinc + 4).SetContent("");
                    }


                }

                string sno = fpspread.Sheets[0].Cells[row_cnt, 0].Text.ToString();
                string roll_noo = fpspread.Sheets[0].Cells[row_cnt, 1].Note.ToString();
                string roll_noo2 = fpspread.Sheets[0].Cells[row_cnt, 1].Text.ToString();
                string batchyr = fpspread.Sheets[0].Cells[row_cnt, 1].Tag.ToString();
                string e1 = fpspread.Sheets[0].Cells[row_cnt, 2].Text.ToString();
                string e2 = fpspread.Sheets[0].Cells[row_cnt, 3].Text.ToString();
                string avreage = fpspread.Sheets[0].Cells[row_cnt, 4].Text.ToString();
                string e3 = fpspread.Sheets[0].Cells[row_cnt, 5].Text.ToString();
                string moderon = fpspread.Sheets[0].Cells[row_cnt, 6].Text.ToString();
                string ca = fpspread.Sheets[0].Cells[row_cnt, 7].Text.ToString();
                string ese = fpspread.Sheets[0].Cells[row_cnt, 8].Text.ToString();
                string totall = fpspread.Sheets[0].Cells[row_cnt, 9].Text.ToString();
                string results = fpspread.Sheets[0].Cells[row_cnt, 10].Text.ToString();



                val++;
                snoo++;
                table1.Rows[val].SetCellPadding(5);
                table1.Cell(val, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                table1.Cell(val, 0).SetContent(snoo);
                colinc = 1;
                table1.Cell(val, colinc + 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                table1.Cell(val, colinc + 0).SetContent(roll_noo2);

                table1.Cell(val, colinc + 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                table1.Cell(val, colinc + 1).SetContent(e1);

                table1.Cell(val, colinc + 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                if (chkincluevel2.Checked == false)
                {
                    table1.Cell(val, colinc + 2).SetContent(e2);
                }


                table1.Cell(val, colinc + 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                if (chkincluevel2.Checked == false)
                {
                    table1.Cell(val, colinc + 3).SetContent(e3);
                }


                table1.Cell(val, colinc + 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                table1.Cell(val, colinc + 4).SetContent(ese);

                table1.Cell(val, colinc + 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                table1.Cell(val, colinc + 5).SetContent(ca);

                table1.Cell(val, colinc + 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                table1.Cell(val, colinc + 6).SetContent(totall);

                table1.Cell(val, colinc + 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                table1.Cell(val, colinc + 7).SetContent(results);


                double markget = 0;
                double markgetdummy = 0;
                if (results.ToUpper() == "FAIL")
                {
                    if (double.TryParse(totall, out markget))
                    {
                        markget = Convert.ToDouble(totall);
                        double passingm = Convert.ToDouble(fpspread.Sheets[0].Cells[row_cnt, 8].Note.ToString());
                        double passingext = Convert.ToDouble(fpspread.Sheets[0].Cells[row_cnt, 8].Text.ToString());
                        if (passingext < passingm)
                        {
                            markgetdummy = Convert.ToDouble(passingm) - passingext;
                            if (double.TryParse(fpspread.Sheets[0].Cells[row_cnt, 7].Text.ToString(), out markget))
                            {
                                passingext = passingext + Convert.ToDouble(fpspread.Sheets[0].Cells[row_cnt, 7].Text.ToString());
                            }
                            if (passingext < passingm)
                            {
                                markget = Convert.ToDouble(passingm) - passingext;

                            }
                            else
                            {
                                markget = Convert.ToDouble(fpspread.Sheets[0].Cells[row_cnt, 7].Note.ToString()) - Convert.ToDouble(fpspread.Sheets[0].Cells[row_cnt, 8].Text.ToString());

                            }


                        }
                        else
                        {
                            passingm = Convert.ToDouble(fpspread.Sheets[0].Cells[row_cnt, 8].Note.ToString());
                            passingext = Convert.ToDouble(fpspread.Sheets[0].Cells[row_cnt, 9].Text.ToString());
                            markget = Convert.ToDouble(passingm) - passingext;
                        }


                    }
                    else
                    {
                        markget = 0;
                    }
                    table1.Cell(val, colinc + 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1.Cell(val, colinc + 8).SetContent(markget);
                }



            }

            myprov_pdfpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(myprovdoc, 20, coltop, 570, 650));
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
    protected void btnviewre_Click(object sender, EventArgs e)
    {
        try
        {
            chkmoderation.Checked = false;
            if (ddlYear1.SelectedIndex == 0)
            {
                btnreset.Visible = false; btnPrint.Visible = false;
                lblaane.Visible = false;
                btnsave1.Visible = false;
                btnprintt.Visible = false; btnprintt_t1.Visible = false;
                fpspread.Visible = false;
                lblerr1.Visible = true;
                lblerr1.Text = "Please Select Year";
                return;
            }
            if (ddlMonth1.SelectedIndex == 0)
            {
                btnreset.Visible = false; btnPrint.Visible = false;
                lblaane.Visible = false;
                btnsave1.Visible = false;
                btnprintt.Visible = false; btnprintt_t1.Visible = false;
                fpspread.Visible = false;
                lblerr1.Visible = true;
                lblerr1.Text = "Please Select Month";
                return;
            }
            if (ddltype.SelectedIndex == 0)
            {
                btnreset.Visible = false; btnPrint.Visible = false;
                lblaane.Visible = false;
                btnsave1.Visible = false;
                btnprintt.Visible = false; btnprintt_t1.Visible = false;
                fpspread.Visible = false;
                lblerr1.Visible = true;
                lblerr1.Text = "Please Select Type";
                return;
            }
            if (ddldegree1.SelectedIndex == 0 && chksubwise.Checked == false)
            {
                btnreset.Visible = false; btnPrint.Visible = false;
                lblaane.Visible = false;
                btnsave1.Visible = false;
                btnprintt.Visible = false; btnprintt_t1.Visible = false;
                fpspread.Visible = false;
                lblerr1.Visible = true;
                lblerr1.Text = "Please Select Degree";
                return;
            }
            if (ddlbranch1.SelectedIndex == 0 && chksubwise.Checked == false)
            {
                btnreset.Visible = false; btnPrint.Visible = false;
                lblaane.Visible = false;
                btnsave1.Visible = false;
                btnprintt.Visible = false; btnprintt_t1.Visible = false;
                fpspread.Visible = false;
                lblerr1.Visible = true;
                lblerr1.Text = "Please Select branch";
                return;
            }
            if (ddlsem1.SelectedIndex == 0)
            {
                btnreset.Visible = false; btnPrint.Visible = false;
                lblaane.Visible = false;
                btnsave1.Visible = false;
                btnprintt.Visible = false; btnprintt_t1.Visible = false;
                fpspread.Visible = false;
                lblerr1.Visible = true;
                lblerr1.Text = "Please Select Semester";
                return;
            }
            if (ddlSubject.SelectedIndex == 0)
            {
                btnreset.Visible = false; btnPrint.Visible = false;
                lblaane.Visible = false;
                btnsave1.Visible = false;
                btnprintt.Visible = false; btnprintt_t1.Visible = false;
                fpspread.Visible = false;
                lblerr1.Visible = true;
                lblerr1.Text = "Please Select Subject";
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
            clear();
            string passorfail = "";
            if (ddlMonth1.SelectedValue != "" && ddlYear1.SelectedValue != "" && ddlSubject.SelectedValue != "")
            {
                fpspread.Width = 906;
                fpspread.Visible = false;
                fpspread.Sheets[0].RowCount = 0;
                fpspread.Sheets[0].ColumnCount = 0;
                fpspread.Sheets[0].ColumnCount = 11;

                fpspread.Sheets[0].RowHeader.Visible = false;
                fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                fpspread.Sheets[0].DefaultStyle.Font.Bold = false;
                fpspread.Sheets[0].ColumnHeader.RowCount = 3;
                fpspread.Sheets[0].AutoPostBack = false;
                fpspread.CommandBar.Visible = false;


                Double minicamoderation = 0;
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

                fpspread.Sheets[0].ColumnHeader.Columns[0].Width = 70;
                fpspread.Sheets[0].ColumnHeader.Columns[1].Width = 100;
                fpspread.Sheets[0].ColumnHeader.Columns[2].Width = 80;
                fpspread.Sheets[0].ColumnHeader.Columns[3].Width = 80;
                fpspread.Sheets[0].ColumnHeader.Columns[4].Width = 80;
                fpspread.Sheets[0].ColumnHeader.Columns[5].Width = 80;
                fpspread.Sheets[0].ColumnHeader.Columns[6].Width = 90;
                fpspread.Sheets[0].ColumnHeader.Columns[7].Width = 80;
                fpspread.Sheets[0].ColumnHeader.Columns[8].Width = 80;
                fpspread.Sheets[0].ColumnHeader.Columns[9].Width = 80;
                fpspread.Sheets[0].ColumnHeader.Columns[10].Width = 80;

                fpspread.Sheets[0].ColumnHeader.Columns[0].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].ColumnHeader.Columns[1].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].ColumnHeader.Columns[2].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].ColumnHeader.Columns[3].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].ColumnHeader.Columns[4].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].ColumnHeader.Columns[5].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].ColumnHeader.Columns[6].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].ColumnHeader.Columns[7].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].ColumnHeader.Columns[8].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].ColumnHeader.Columns[9].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].ColumnHeader.Columns[10].HorizontalAlign = HorizontalAlign.Center;


                fpspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                fpspread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Center;

                FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                style2.Font.Size = 13;
                style2.Font.Name = "Book Antiqua";
                style2.Font.Bold = true;
                style2.HorizontalAlign = HorizontalAlign.Center;
                style2.ForeColor = System.Drawing.Color.White;
                style2.BackColor = System.Drawing.Color.Teal;
                fpspread.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);

                fpspread.ShowHeaderSelection = false;
                fpspread.Sheets[0].SheetName = " ";
                fpspread.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
                fpspread.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
                fpspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].DefaultStyle.Font.Bold = false;

                string type = "";
                if (chksubwise.Checked == false)
                {
                    type = da.GetFunction("select edu_level from course where course_id=" + ddldegree1.SelectedValue + "");
                }
                else
                {
                    type = da.GetFunction("select distinct c.Edu_Level from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.ToString() + "' and s.subject_code='" + ddlSubject.SelectedValue.ToString() + "'");
                }

                string degreeval = "";
                string degreevalregmoder = "";
                string degreevalttab = "";
                string degreevalregis = "";
                if (chksubwise.Checked == false)
                {
                    degreeval = " and ed.degree_code='" + ddlbranch1.SelectedValue + "'";
                    degreevalregmoder = " and M.degree_code='" + ddlbranch1.SelectedValue + "'";
                    degreevalttab = " and e.degree_code='" + ddlbranch1.SelectedValue + "'";
                    degreevalregis = " and r.degree_code='" + ddlbranch1.SelectedValue + "'";
                }

                string yearval = "";
                if (ddlbatch.Text != "All")
                {
                    yearval = " and r.batch_year='" + ddlbatch.Text.ToString() + "'";
                }

                int Mark_Difference1 = 0;
                string Mark_Difference = da.GetFunction("select distinct isnull(value,'') as value from COE_Master_Settings where settings='Mark Difference'");
                if (Mark_Difference != "")
                {
                    Mark_Difference1 = Convert.ToInt32(Mark_Difference);
                }
                else
                {
                    Mark_Difference1 = 0;
                }


                int Mark_moderation1 = 0;
                string Mark_moderation = "";
                if (chksubwise.Checked == false)
                {
                    Mark_moderation = da.GetFunction("select distinct s.Moderation_Mark from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.ToString() + "' and s.subject_code='" + ddlSubject.SelectedValue.ToString() + "'");
                }
                if (Mark_moderation.Trim() == "" || Mark_moderation.Trim() == "0" && ddlbatch.Text != "All")
                {
                    Mark_moderation = da.GetFunction("select distinct isnull(value,'') as value from COE_Master_Settings where settings='" + ddlbatch.Text.ToString() + "/" + type + "/Moderation'");
                }
                if (Mark_moderation.Trim() == "" || Mark_moderation.Trim() == "0")
                {
                    Mark_moderation = da.GetFunction("select distinct isnull(value,'') as value from COE_Master_Settings where settings='Mark Moderation'");
                }

                if (Mark_Difference != "")
                {
                    Mark_moderation1 = Convert.ToInt16(Mark_moderation);
                }
                else
                {
                    Mark_moderation1 = 0;
                }

                string qeryss = "SELECT ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,ead.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,ead.attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'REGULAR' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,Registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and r.Roll_No=ea.roll_no " + degreeval + " and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and s.subject_code='" + ddlSubject.SelectedValue + "' " + yearval + " and isnull(r.Reg_No,'') <>'' ";
                // qeryss = qeryss + " union SELECT ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,s.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,'' attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'NR' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag FROM Exam_Details ED,tbl_not_registred nt,subject s,Registration r where ed.Exam_Month=nt.exam_month and ed.Exam_year=nt.exam_year and s.subject_no=nt.subject_no and r.Roll_No=nt.roll_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year " + degreeval + " and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and s.subject_code='" + ddlSubject.SelectedValue + "' and r.cc=0 and isnull(r.Reg_No,'') <>'' ";
                qeryss = qeryss + " union SELECT ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,s.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,'' attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'NR' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag FROM Exam_Details ED,tbl_not_registred nt,subject s,Registration r,subjectChooser sc where ed.Exam_Month=nt.exam_month and ed.Exam_year=nt.exam_year and s.subject_no=nt.subject_no and r.Roll_No=nt.roll_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and sc.semester=ed.current_semester " + degreeval + " and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and s.subject_code='" + ddlSubject.SelectedValue + "' " + yearval + "  and r.cc=0 and isnull(r.Reg_No,'') <>'' ";
                qeryss = qeryss + " union SELECT ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,s.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,'' attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'NE' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag FROM Exam_Details ED,studentsemestersubjectdebar nt,subject s,Registration r where r.Roll_No=nt.roll_no and nt.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and (ed.current_semester=nt.semester or r.CC=1) " + degreeval + " and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and s.subject_code='" + ddlSubject.SelectedValue + "' and isnull(r.Reg_No,'') <>'' and r.cc=0";
                // qeryss = qeryss + " union SELECT ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,s.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,'0' attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'REGULAR' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag FROM subjectChooser sc,subject s,Registration r,Exam_Details ed where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and sc.semester=ed.Current_Semester " + degreeval + " and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and s.subject_code='" + ddlSubject.SelectedValue + "' and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' and isnull(r.Reg_No,'') <>'' " + yearval + "  order by ed.batch_year desc,ed.degree_code,ed.current_semester,r.reg_no,sts";
                if (chk_onlycia.Checked == true)
                {
                    qeryss = "SELECT ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,sc.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,'0' attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,r.cc,s.writtenmaxmark,'REGULAR' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag FROM subjectChooser sc,subject s,Registration r,Exam_Details ed where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and sc.semester=ed.Current_Semester and r.degree_code='" + ddlbranch1.SelectedValue + "'  and sc.semester='" + ddlsem1.SelectedValue.ToString() + "' and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and s.subject_code='" + ddlSubject.SelectedValue + "' " + yearval + "  order by r.batch_year desc,r.degree_code,r.current_semester,sc.subject_no,r.reg_no ";
                }
                if (chkicaretake.Checked == true)
                {
                    qeryss = "select ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,s.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,'1' attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'REGULAR' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag,max(m.internal_mark) ICA,max(m.external_mark) EXE from mark_entry m,subject s,Registration r,Exam_Details ed,exam_application ea,exam_appl_details ead where m.subject_no=s.subject_no and r.Roll_No=m.roll_no and ed.batch_year=r.Batch_Year  AND M.subject_no=ead.subject_no and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.roll_no=r.Roll_No and ead.type='1' and ed.degree_code=r.degree_code " + degreeval + " and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and s.subject_code='" + ddlSubject.SelectedValue + "' " + yearval + "  group by ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,s.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,r.Batch_Year ,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,s.Moderation_Mark,s.min_int_moderation,r.delflag  order by ed.batch_year desc,ed.degree_code,ed.current_semester,s.subject_no,r.reg_no";
                    //qeryss = "select ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,s.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,'1' attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'Regular' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag,max(m.internal_mark) ICA,max(m.external_mark) EXE from mark_entry m,subject s,Registration r,Exam_Details ed,exam_application ea,exam_appl_details ead where m.subject_no=s.subject_no and r.Roll_No=m.roll_no and ed.batch_year=r.Batch_Year and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.roll_no=r.Roll_No and ead.type='1' and ed.degree_code=r.degree_code " + degreeval + " and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and s.subject_code='" + ddlSubject.SelectedValue + "' and m.roll_no not in (select m1.roll_no from mark_entry m1 where m1.roll_no=m.roll_no and m.subject_no=m1.subject_no and m1.result='Pass') group by ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,s.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,r.Batch_Year ,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,s.Moderation_Mark,s.min_int_moderation,r.delflag  order by ed.batch_year desc,ed.degree_code,ed.current_semester,s.subject_no,r.reg_no";
                }
                if (chkIIIvaluation.Checked == true)
                {
                    qeryss = "SELECT ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,m.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,m.attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'REGULAR' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag FROM Exam_Details ED,mark_entry m,subject s,Registration r where ed.exam_code=m.exam_code and s.subject_no=m.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and r.Roll_No=m.roll_no  and ed.degree_code='" + ddlbranch1.SelectedValue + "' and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and s.subject_code='" + ddlSubject.SelectedValue + "'  " + yearval + " and isnull(r.Reg_No,'') <>''  and (evaluation1-evaluation2>=" + Mark_Difference1 + " or evaluation2-evaluation1>=" + Mark_Difference1 + ") and evaluation1>=0 and evaluation2>=0 and evaluation1 is not null and evaluation2 is not null order by r.batch_year desc,r.degree_code,r.current_semester,m.subject_no,r.reg_no";
                }
                qeryss = qeryss + " select roll_no,regno,Course_Name,Dept_Name,dummy_no  from  dummynumber du,Degree d,Department dt,Course c,subject s where du.degreecode =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and exam_month ='" + ddlMonth1.SelectedValue.ToString() + "' and exam_year ='" + ddlYear1.SelectedItem.Text.ToString() + "'  and s.subject_no=du.subject_no and s.subject_code='" + ddlSubject.SelectedValue + "' and  (dummy_type ='1' or dummy_type ='0')";
                DataSet ds = da.select_method_wo_parameter(qeryss, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 4, 1);
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Reg No";
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 4, 1);
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Valuation";
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 1, 2);
                    fpspread.Sheets[0].ColumnHeader.Cells[1, 2].Text = "I";
                    fpspread.Sheets[0].ColumnHeader.Cells[1, 3].Text = "II";
                    fpspread.Sheets[0].ColumnHeader.Cells[2, 2].Text = "Max";
                    fpspread.Sheets[0].ColumnHeader.Cells[2, 3].Text = "Max";
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Average";
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                    fpspread.Sheets[0].ColumnHeader.Cells[2, 4].Text = "Max";
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Valuation";
                    fpspread.Sheets[0].ColumnHeader.Cells[1, 5].Text = "III";
                    fpspread.Sheets[0].ColumnHeader.Cells[2, 5].Text = "Max";
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Moderation";
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                    fpspread.Sheets[0].ColumnHeader.Cells[2, 6].Text = "Max";
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "C.I.A";
                    fpspread.Sheets[0].Columns[7].Locked = true;
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
                    fpspread.Sheets[0].ColumnHeader.Cells[2, 7].Text = "Max";
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 8].Text = "E.S.E";
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);
                    fpspread.Sheets[0].ColumnHeader.Cells[2, 8].Text = "Max";
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Total";
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);
                    fpspread.Sheets[0].ColumnHeader.Cells[2, 9].Text = "Max";
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Result";
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 10, 4, 1);
                    fpspread.Sheets[0].Columns[0].Visible = true;

                    if ((ddlSubject.SelectedItem.ToString() != "") && (ddlSubject.SelectedItem.ToString() != "--Select--"))
                    {
                        string subject_no = ddlSubject.SelectedValue.ToString();
                        string exam_code = ds.Tables[0].Rows[0]["exam_code"].ToString();
                        string sem = ddlsem1.SelectedValue.ToString();

                        string getdetails = "select me.roll_no,me.external_mark,me.internal_mark,me.result,me.evaluation1,me.evaluation2,me.evaluation3,me.external_mark,me.total  from mark_entry me,Exam_Details ed,subject s where me.exam_code=ed.exam_code and me.subject_no=s.subject_no and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.ToString() + "'  " + degreeval + " and s.subject_code='" + ddlSubject.SelectedValue.ToString() + "'";
                        getdetails = getdetails + "  select * from moderation m,subject s where m.subject_no=s.subject_no and s.subject_code='" + ddlSubject.SelectedValue.ToString() + "' " + degreevalregmoder + " and m.exam_year='" + ddlYear1.SelectedItem.ToString() + "'";
                        getdetails = getdetails + " select convert(varchar(50),exam_date,105) as exam_date,exam_session from exmtt e,exmtt_det et,subject s where e.exam_code=et.exam_code and et.subject_no=s.subject_no and e.Exam_month='" + ddlMonth1.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear1.SelectedItem.ToString() + "' " + degreevalttab + " and s.subject_code='" + ddlSubject.SelectedValue.ToString() + "' ";
                        ds2 = da.select_method_wo_parameter(getdetails, "Text");

                        Double ev11 = 0;
                        Double ev21 = 0;
                        Double ev31 = 0;
                        Double external_mark = 0;
                        Double modermarks = 0;
                        Double difff1 = 0;
                        Double difff2 = 0;

                        string ev1 = "";
                        string ev2 = "";
                        string ev3 = "";
                        string rollno = "";
                        string regno = "";
                        string batchyerr = "";
                        string resullts = "";
                        string externn = "";
                        string intermarkf = "";
                        string subjectcode = "";

                        int sno = 1;

                        Double papermaxexter = 0;
                        FarPoint.Web.Spread.DoubleCellType intgrcel = new FarPoint.Web.Spread.DoubleCellType();
                        FarPoint.Web.Spread.DoubleCellType intgrcel1 = new FarPoint.Web.Spread.DoubleCellType();
                        FarPoint.Web.Spread.RegExpCellType rgex = new FarPoint.Web.Spread.RegExpCellType();

                        Double min_int_marks1 = Convert.ToDouble(ds.Tables[0].Rows[0]["min_int_marks"]);
                        min_int_marks1 = Math.Round(min_int_marks1, 0);

                        Double mintolmarks1 = Convert.ToDouble(ds.Tables[0].Rows[0]["mintotal"]);
                        mintolmarks1 = Math.Round(mintolmarks1, 0);

                        Double min_ext_marks1 = Convert.ToDouble(ds.Tables[0].Rows[0]["min_ext_marks"]);
                        min_ext_marks1 = Math.Round(min_ext_marks1, 0);

                        Double max_ext_marks1 = Convert.ToDouble(ds.Tables[0].Rows[0]["max_ext_marks"]);
                        max_ext_marks1 = Math.Round(max_ext_marks1, 0);

                        Double max_int_marks1 = Convert.ToDouble(ds.Tables[0].Rows[0]["max_int_marks"]);
                        max_int_marks1 = Math.Round(max_int_marks1, 0);

                        Double max_tol_marks1 = Convert.ToDouble(ds.Tables[0].Rows[0]["maxtotal"]);
                        max_tol_marks1 = Math.Round(max_tol_marks1, 0);

                        string regexpree = "AB|ab|a|A|M|m|LT|lt|00|01|02|03|04|05|06|07|08|09|";
                        for (int i = 0; i <= max_int_marks1; i++)
                        {
                            regexpree = regexpree + "|" + "" + i + "";

                        }
                        rgex.ValidationExpression = "(\\W|^)(" + regexpree + "|\\sdarnit|heck)(\\W|$)";
                        rgex.ErrorMessage = "Please Enter the Mark Less Than or Equal to (" + max_int_marks1 + ")";
                        fpspread.Sheets[0].Columns[7].CellType = rgex;

                        if (type.Trim() != "" && type.Trim() != "0" && type != null)
                        {
                            string extexammaxmark = ds.Tables[0].Rows[0]["writtenmaxmark"].ToString();
                            if (extexammaxmark.Trim() == "" || extexammaxmark.Trim() == "0" || extexammaxmark == null || extexammaxmark.Trim() == "0.0" || extexammaxmark.Trim() == "0.00")
                            {
                                extexammaxmark = da.GetFunction("select value from COE_Master_Settings where settings='MaxExternalMark " + type + "'");
                            }
                            if (extexammaxmark.Trim() != "" && extexammaxmark.Trim() != "0" && extexammaxmark != null)
                            {
                                papermaxexter = Convert.ToDouble(extexammaxmark);
                                fpspread.Sheets[0].ColumnHeader.Cells[2, 2].Text = "Max : " + extexammaxmark;
                                fpspread.Sheets[0].ColumnHeader.Cells[2, 3].Text = "Max : " + extexammaxmark;
                                fpspread.Sheets[0].ColumnHeader.Cells[2, 5].Text = "Max : " + extexammaxmark;
                                fpspread.Sheets[0].ColumnHeader.Cells[2, 4].Text = "Max : " + extexammaxmark;
                                fpspread.Sheets[0].ColumnHeader.Cells[2, 2].Tag = extexammaxmark;

                                regexpree = "AB|ab||NR|nr|NE|ne|ra||RA|a|A|M|m|LT|lt|00|01|02|03|04|05|06|07|08|09|";
                                for (int i = 0; i <= Convert.ToDouble(extexammaxmark); i++)
                                {
                                    regexpree = regexpree + "|" + "" + i + "";
                                }
                                rgex.ValidationExpression = "(\\W|^)(" + regexpree + "|\\sdarnit|heck)(\\W|$)";
                                rgex.ErrorMessage = "Please Enter the Mark Less Than or Equal to (" + extexammaxmark + ")";
                                fpspread.Sheets[0].Columns[2].CellType = rgex;
                                fpspread.Sheets[0].Columns[3].CellType = rgex;
                                fpspread.Sheets[0].Columns[4].CellType = rgex;
                                fpspread.Sheets[0].Columns[5].CellType = rgex;
                            }
                            else
                            {
                                btnsave1.Visible = false;
                                btnprintt.Visible = false; btnprintt_t1.Visible = false;
                                lblerr1.Text = "Please Set Max External Mark";
                                lblerr1.Visible = true;
                                chkmoderation.Visible = false;
                                return;
                            }
                            string minicamodeval = "0";
                            if (chksubwise.Checked == false)
                            {
                                minicamodeval = da.GetFunction("select distinct s.min_int_moderation from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.ToString() + "' and s.subject_code='" + ddlSubject.SelectedValue.ToString() + "'");
                            }
                            if (minicamodeval.Trim() == "" || minicamodeval.Trim() == "0")
                            {
                                minicamodeval = da.GetFunctionv("select value from COE_Master_Settings where settings = 'min Ica Moderation " + type + "'");
                            }
                            if (minicamodeval.Trim() == "")
                            {
                                minicamodeval = "0";
                            }
                            minicamoderation = Convert.ToInt32(minicamodeval);
                        }

                        fpspread.Sheets[0].ColumnHeader.Cells[2, 9].Text = "Max : " + (max_ext_marks1 + max_int_marks1).ToString();
                        fpspread.Sheets[0].ColumnHeader.Cells[2, 8].Text = "Max : " + max_ext_marks1.ToString();
                        fpspread.Sheets[0].ColumnHeader.Cells[2, 7].Text = "Max : " + max_int_marks1.ToString();
                        fpspread.Sheets[0].ColumnHeader.Cells[2, 7].Tag = max_int_marks1.ToString();
                        fpspread.Sheets[0].ColumnHeader.Cells[2, 8].Tag = max_ext_marks1.ToString();
                        fpspread.Sheets[0].ColumnHeader.Cells[2, 9].Tag = (max_ext_marks1 + max_int_marks1).ToString();

                        Double passint = Math.Round((min_int_marks1 / max_int_marks1) * 100, 0);
                        Double passext = Math.Round((min_ext_marks1 / max_ext_marks1) * 100, 0);



                        fpspread.Sheets[0].ColumnHeader.Cells[2, 6].Text = "Max : " + Mark_moderation1.ToString() + " ";
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Tag = Mark_moderation1.ToString();
                        fpspread.Sheets[0].ColumnHeader.Cells[2, 6].Tag = minicamoderation;
                        fpspread.Sheets[0].ColumnHeader.Cells[2, 6].Note = Mark_Difference1.ToString();

                        string exandate = "";
                        int height = 50;
                        lblerr1.Visible = false;
                        fpspread.Visible = true;

                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            clear();
                            lblerr1.Visible = true;
                            lblerr1.Text = "No Records Found";
                            return;
                        }
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            chkincluevel2.Visible = true;
                            btnexcelimport.Visible = true;
                            fpmarkexcel.Visible = true;
                            rbeval.Visible = true;
                            rbcia.Visible = true;
                            rbcia.Checked = false;
                            rbeval.Checked = true;
                            DataSet dsstuatt = new DataSet();
                            btnsave1.Visible = true;
                            btnprintt.Visible = true; btnprintt_t1.Visible = true;
                            lblaane.Visible = true;
                            string strsetval = da.GetFunction("select value from COE_Master_Settings where settings='Attendance Link mark'");
                            if (strsetval == "1")
                            {
                                if (ds2.Tables[2].Rows.Count > 0)
                                {
                                    string getdate = ds2.Tables[2].Rows[0]["exam_date"].ToString();
                                    string[] spd = getdate.Split('-');

                                    if (spd.GetUpperBound(0) == 2)
                                    {
                                        DateTime dtval = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
                                        string ath = "d" + dtval.Day + "d1";
                                        int monva = Convert.ToInt32(spd[2]) * 12 + Convert.ToInt32(spd[1]);
                                        string ses = ds2.Tables[2].Rows[0]["exam_session"].ToString();
                                        if (ses.Trim().ToLower() == "a.n")
                                        {
                                            ath = "d" + dtval.Day + "d5";
                                        }
                                        string strattmarval = "select a.roll_no from attendance a,Exam_Details ed,exam_application ea,exam_appl_details ead where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.roll_no=a.roll_no  and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and ead.subject_no='" + ddlSubject.SelectedValue.ToString() + "' and a.month_year='" + monva + "' and a." + ath + " is not null and a." + ath + "<>'0' and a." + ath + "<>''";
                                        dsstuatt = da.select_method_wo_parameter(strattmarval, "Text");
                                        if (dsstuatt.Tables[0].Rows.Count == 0)
                                        {
                                            string cc = ds.Tables[0].Rows[0]["cc"].ToString().Trim();
                                            if (cc.ToLower() != "true" && cc.ToLower() != "0")
                                            {
                                                clear();
                                                lblerr1.Visible = true;
                                                lblerr1.Text = "Please Mark Exam Attendance";
                                                return;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    clear();
                                    lblerr1.Visible = true;
                                    lblerr1.Text = "Please Set Time Table";
                                    return;
                                }
                            }
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
                                    string examcode = ds.Tables[0].Rows[i]["exam_code"].ToString();
                                    string subno = ds.Tables[0].Rows[i]["subject_no"].ToString();
                                    string attempts = ds.Tables[0].Rows[i]["attempts"].ToString();
                                    string cursem = ds.Tables[0].Rows[i]["current_semester"].ToString();
                                    string status = ds.Tables[0].Rows[i]["sts"].ToString();
                                    string minintmark = ds.Tables[0].Rows[i]["min_int_marks"].ToString();
                                    string maxintmark = ds.Tables[0].Rows[i]["max_int_marks"].ToString();
                                    string minextmark = ds.Tables[0].Rows[i]["min_ext_marks"].ToString();
                                    string maxextmark = ds.Tables[0].Rows[i]["max_ext_marks"].ToString();
                                    string mintotmark = ds.Tables[0].Rows[i]["mintotal"].ToString();
                                    string maxtotmark = ds.Tables[0].Rows[i]["maxtotal"].ToString();
                                    string degreecode = ds.Tables[0].Rows[i]["degree_code"].ToString();
                                    string crdeitpoints = ds.Tables[0].Rows[i]["credit_points"].ToString();
                                    string submaoremark = ds.Tables[0].Rows[i]["Moderation_Mark"].ToString();
                                    string minintmodeallow = ds.Tables[0].Rows[i]["min_int_moderation"].ToString();
                                    string dlflag = ds.Tables[0].Rows[i]["delflag"].ToString();
                                    Boolean setflag = false;

                                    if (ds2.Tables[2].Rows.Count > 0)
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
                                    if (strsetval == "1" && cc.Trim().ToLower() != "true" && cc.ToLower() != "0")
                                    {
                                        dsstuatt.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "'";
                                        DataView dvst = dsstuatt.Tables[0].DefaultView;
                                        if (dvst.Count == 0)
                                        {
                                            setflag = true;
                                        }
                                    }
                                    if (setflag == false)
                                    {
                                        fpspread.Sheets[0].RowCount = fpspread.Sheets[0].RowCount + 1;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = sno + "";
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Tag = batchyerr;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Note = status;

                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].CellType = txt;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Text = regno;
                                        if (ds.Tables[1].Rows.Count > 0)
                                        {
                                            ds.Tables[1].DefaultView.RowFilter = "roll_no='" + regno + "'";
                                            dnew = ds.Tables[1].DefaultView;
                                            if (dnew.Count > 0)
                                            {
                                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dnew[0]["dummy_no"]);
                                                fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Dummy No";
                                            }
                                        }
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Note = rollno;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Tag = examcode;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].CellType = rgex;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Tag = subno;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Note = attempts;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].CellType = rgex;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Tag = exandate;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Note = cursem;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].CellType = rgex;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Note = degreecode;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Tag = crdeitpoints;

                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Note = minintmark;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Tag = maxintmark;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Note = minextmark;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Tag = maxextmark;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 8].Note = mintotmark;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 8].Tag = maxtotmark;

                                        if (submaoremark.Trim() != "" && submaoremark.Trim() != "0")
                                        {
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 9].Note = submaoremark;
                                        }
                                        else
                                        {
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 9].Note = Mark_moderation1.ToString();
                                        }
                                        if (minintmodeallow.Trim() != "" && minintmodeallow.Trim() != "0")
                                        {
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 9].Tag = minintmodeallow.ToString();
                                        }
                                        else
                                        {
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 9].Tag = minicamoderation.ToString();
                                        }

                                        if (dlflag.Trim().ToLower() == "1" || dlflag.Trim().ToLower() == "true")
                                        {
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = "LT";
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Text = "LT";
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Text = "LT";
                                        }

                                        sno++;
                                    }
                                }
                            }
                        }
                        fpspread.Height = height + 30;
                        fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;

                        string strinternammark = "select distinct m.roll_no,m.internal_mark,m.exam_code from mark_entry m,subject s,Registration r where m.roll_no=r.Roll_No and m.subject_no=s.subject_no and s.subject_code='" + ddlSubject.SelectedValue.ToString() + "' " + degreevalregis + "  and m.internal_mark is not null order by m.roll_no,m.exam_code desc";
                        DataSet dsinternal = da.select_method_wo_parameter(strinternammark, "Text");
                        Double evalmaxmark = Convert.ToDouble(fpspread.Sheets[0].ColumnHeader.Cells[2, 2].Tag);
                        for (int row_cnt = 0; row_cnt < fpspread.Sheets[0].RowCount; row_cnt++)
                        {
                            string roll_noo = fpspread.Sheets[0].Cells[row_cnt, 1].Note.ToString();
                            string roll_noo2 = fpspread.Sheets[0].Cells[row_cnt, 1].Text.ToString();
                            string batchyr = fpspread.Sheets[0].Cells[row_cnt, 1].Tag.ToString();
                            string previousinternalmark = "";
                            string strus = fpspread.Sheets[0].Cells[row_cnt, 0].Note.ToString();

                            int markmodeart = 0;
                            string modemarkval = fpspread.Sheets[0].Cells[row_cnt, 9].Note.ToString();
                            if (modemarkval.Trim() != "" && modemarkval.Trim() != "0")
                            {
                                markmodeart = Convert.ToInt32(modemarkval);
                            }
                            else
                            {
                                markmodeart = Mark_moderation1;
                            }

                            Double minintmodeallmar = 0;
                            string minintmoderationallow = fpspread.Sheets[0].Cells[row_cnt, 9].Tag.ToString();
                            if (minintmoderationallow.Trim() != "" && minintmoderationallow.Trim() != "0")
                            {
                                minintmodeallmar = Convert.ToInt32(minintmoderationallow);
                            }
                            else
                            {
                                minintmodeallmar = minicamoderation;
                            }
                            min_int_marks1 = Convert.ToDouble(fpspread.Sheets[0].Cells[row_cnt, 6].Note.ToString());
                            max_int_marks1 = Convert.ToDouble(fpspread.Sheets[0].Cells[row_cnt, 6].Tag.ToString());
                            min_ext_marks1 = Convert.ToDouble(fpspread.Sheets[0].Cells[row_cnt, 7].Note.ToString());
                            max_ext_marks1 = Convert.ToDouble(fpspread.Sheets[0].Cells[row_cnt, 7].Tag.ToString());
                            mintolmarks1 = Convert.ToDouble(fpspread.Sheets[0].Cells[row_cnt, 8].Note.ToString());

                            if (roll_noo2 != "" && roll_noo != "")
                            {
                                ds2.Tables[0].DefaultView.RowFilter = "roll_no='" + roll_noo + "'";
                                dv1 = ds2.Tables[0].DefaultView;

                                dsinternal.Tables[0].DefaultView.RowFilter = "roll_no='" + roll_noo + "'";
                                DataView dvintmark = dsinternal.Tables[0].DefaultView;





                                if (previousinternalmark.Trim() == "" || previousinternalmark == null)
                                {
                                    previousinternalmark = da.GetFunctionv("select total from camarks where Roll_No='" + roll_noo + "' and subject_no in (select subject_no from subject  where syll_code in (select syll_code from syllabus_master where  degree_code='" + ddlbranch1.SelectedItem.Value.ToString() + "' and batch_year='" + ddlbatch.SelectedItem.Text.ToString() + "' and semester='" + ddlsem1.SelectedItem.Text.ToString() + "') and subject_code='" + ddlSubject.SelectedItem.Value.ToString() + "')");
                                }
                                if (previousinternalmark.Trim() != "" && previousinternalmark != null)
                                {
                                    if (previousinternalmark.Trim() != "")
                                    {
                                        Double setmark = Convert.ToDouble(previousinternalmark);
                                        setmark = Math.Round(setmark, 0, MidpointRounding.AwayFromZero);
                                        previousinternalmark = setmark.ToString();
                                    }
                                    else
                                    {
                                        previousinternalmark = "";
                                    }
                                }

                                if (dvintmark.Count > 0 && previousinternalmark.Trim() == "")
                                {
                                    previousinternalmark = dvintmark[0]["internal_mark"].ToString();
                                    if (previousinternalmark.Trim() != "" && previousinternalmark != null)
                                    {
                                        if (previousinternalmark.Trim() != "")
                                        {
                                            Double setmark = Convert.ToDouble(previousinternalmark);
                                            setmark = Math.Round(setmark, 0, MidpointRounding.AwayFromZero);
                                            previousinternalmark = setmark.ToString();
                                        }
                                        else
                                        {
                                            previousinternalmark = "";
                                        }
                                    }
                                }
                                if (dv1.Count > 0)
                                {
                                    btnreset.Visible = true; btnPrint.Visible = true;
                                    chkmoderation.Visible = true;
                                    ev1 = dv1[0]["evaluation1"].ToString();
                                    ev2 = dv1[0]["evaluation2"].ToString();
                                    subjectcode = ddlSubject.SelectedValue.ToString();
                                    ev3 = dv1[0]["evaluation3"].ToString();

                                    intermarkf = "";
                                    if (intermarkf.Trim() == "" || intermarkf == null)
                                    {
                                        // intermarkf = da.GetFunction("select total from camarks where Roll_No='" + roll_noo + "' and subject_no='" + ddlSubject.SelectedValue.ToString() + "'");

                                        intermarkf = da.GetFunctionv("select total from camarks where Roll_No='" + roll_noo + "' and subject_no in (select subject_no from subject  where syll_code in (select syll_code from syllabus_master where  degree_code='" + ddlbranch1.SelectedItem.Value.ToString() + "' and batch_year='" + ddlbatch.SelectedItem.Text.ToString() + "' and semester='" + ddlsem1.SelectedItem.Text.ToString() + "') and subject_code='" + ddlSubject.SelectedItem.Value.ToString() + "')");

                                    }
                                    if (intermarkf.Trim() == "" || intermarkf == null)
                                    {
                                        intermarkf = dv1[0]["internal_mark"].ToString();
                                    }

                                    resullts = dv1[0]["result"].ToString();


                                    Double intermarkf223 = 0;
                                    string intermarkforab = "";


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
                                            }
                                            else
                                            {
                                                resullts = "AB";
                                            }
                                        }
                                        fpspread.Sheets[0].Cells[row_cnt, 7].Text = intermarkforab;
                                        if (intermarkforab.Trim().ToLower().Contains('a'))
                                        {
                                            intermarkforab = "-1";
                                        }
                                    }
                                    int abse1 = 0;
                                    if (ev1.Trim() != "" && ev1.Trim() != null && ev1.Trim().ToLower() != "m")
                                    {
                                        abse1 = Convert.ToInt32(ev1); ;
                                    }

                                    if (ev1.Trim() != "" || ev2.Trim() != "" || intermarkf.Trim() != "")
                                    {
                                        Session["MarkEntrySave"] = 2;
                                    }

                                    int abse2 = 0;
                                    string total = dv1[0]["total"].ToString();

                                    if (intermarkf == "" && resullts.Trim().ToLower() != "whd")
                                    {
                                        resullts = "";
                                        total = "";
                                    }

                                    if (ev2.Trim() != "" && ev2.Trim() != null && ev2.Trim().ToLower() != "m")
                                    {
                                        abse2 = Convert.ToInt16(ev2);
                                        chkmoderation.Visible = true;
                                    }
                                    if (ev1.Trim() == "" && strus.Trim().ToUpper() != "REGULAR" && chkicaretake.Checked == false)
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
                                        if (abse1 != -4 && abse2 != -4)
                                        {
                                            if (abse1 != -3 && abse2 != -3)
                                            {
                                                if (abse1 != -2 && abse2 != -2)
                                                {
                                                    if (abse1 != -1 && abse2 != -1)
                                                    {
                                                        if (ev1 == "-1")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 2].Text = "AAA";
                                                        }
                                                        else if (ev1 == "-2")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 2].Text = "NE";
                                                        }
                                                        else if (ev1 == "-3")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 2].Text = "RA";
                                                        }
                                                        else if (ev1.Trim() != "")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 2].Text = ev1;
                                                        }
                                                        else
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 2].Text = "";
                                                        }

                                                        if (ev2 == "-1")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 3].Text = "AAA";
                                                        }
                                                        else if (ev2 == "-2")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 3].Text = "NE";
                                                        }
                                                        else if (ev2 == "-3")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 3].Text = "RA";
                                                        }
                                                        else if (ev2.Trim() != "")
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 3].Text = ev2;
                                                        }
                                                        else
                                                        {
                                                            fpspread.Sheets[0].Cells[row_cnt, 3].Text = "";
                                                        }


                                                        string avemark = "";
                                                        if (ev1 != "" && ev2 != "" && ev2 != "NE" && ev1 != "NE")
                                                        {
                                                            ds2.Tables[1].DefaultView.RowFilter = "roll_no='" + roll_noo + "'";
                                                            dv3 = ds2.Tables[1].DefaultView;
                                                            if (dv3.Count > 0)
                                                            {
                                                                modermarks = Convert.ToInt16(dv3[0]["passmark"].ToString());

                                                                fpspread.Sheets[0].Cells[row_cnt, 6].Text = modermarks.ToString();
                                                                fpspread.Sheets[0].Cells[row_cnt, 9].BackColor = Color.LightSeaGreen;
                                                                externn = dv3[0]["af_moderation_extmrk"].ToString();
                                                                external_mark = Convert.ToDouble(externn.ToString());
                                                                external_mark = Math.Round(external_mark, 0);

                                                                externn = dv1[0]["external_mark"].ToString();
                                                                if (externn.Trim() != "")
                                                                {
                                                                    external_mark = Convert.ToDouble(externn.ToString());
                                                                    external_mark = Math.Round(external_mark, 0);

                                                                    fpspread.Sheets[0].Cells[row_cnt, 4].Text = "";
                                                                    fpspread.Sheets[0].Cells[row_cnt, 8].Text = external_mark.ToString();
                                                                }
                                                            }
                                                            else
                                                            {
                                                                externn = dv1[0]["external_mark"].ToString();
                                                                if (externn.Trim() != "")
                                                                {
                                                                    external_mark = Convert.ToDouble(externn.ToString());
                                                                    external_mark = Math.Round(external_mark, 0);
                                                                    fpspread.Sheets[0].Cells[row_cnt, 4].Text = external_mark.ToString();
                                                                    fpspread.Sheets[0].Cells[row_cnt, 8].Text = external_mark.ToString();
                                                                }
                                                            }
                                                            if (externn.Trim() != "")
                                                            {
                                                                fpspread.Sheets[0].Cells[row_cnt, 9].Text = total;
                                                                fpspread.Sheets[0].Cells[row_cnt, 10].Text = resullts;
                                                            }

                                                            ev11 = Convert.ToInt16(ev1);
                                                            ev21 = Convert.ToInt16(ev2);
                                                            difff1 = ev11 - ev21;
                                                            difff2 = ev21 - ev11;
                                                            Double thirfddiff = 0;
                                                            if (ev11 > ev21)
                                                            {
                                                                thirfddiff = ev11 - ev21;
                                                            }
                                                            else
                                                            {
                                                                thirfddiff = ev21 - ev11;
                                                            }
                                                            if (thirfddiff >= Mark_Difference1)
                                                            {
                                                                fpspread.Sheets[0].Cells[row_cnt, 5].BackColor = Color.SkyBlue;
                                                                fpspread.Sheets[0].Cells[row_cnt, 5].Locked = false;
                                                                externn = "";
                                                                if (ev3.Trim() != "")
                                                                {
                                                                    if (ev3.Trim() == "-1")
                                                                    {
                                                                        fpspread.Sheets[0].Cells[row_cnt, 5].Text = "AAA";
                                                                    }
                                                                    else if (ev3.Trim() == "-2")
                                                                    {
                                                                        fpspread.Sheets[0].Cells[row_cnt, 5].Text = "NE";
                                                                    }
                                                                    else if (ev3.Trim() == "-3")
                                                                    {
                                                                        fpspread.Sheets[0].Cells[row_cnt, 5].Text = "RA";
                                                                    }
                                                                    else if (ev3.Trim() != "")
                                                                    {
                                                                        fpspread.Sheets[0].Cells[row_cnt, 5].Text = ev3;

                                                                        ev31 = Convert.ToInt32(ev3);
                                                                        Double devdif1 = 0;
                                                                        Double devdif2 = 0;
                                                                        Double finev3 = 0;
                                                                        if (ev11 > ev31)
                                                                        {
                                                                            devdif1 = ev11 - ev31;
                                                                        }
                                                                        else
                                                                        {
                                                                            devdif1 = ev31 - ev11;
                                                                        }
                                                                        if (ev21 > ev31)
                                                                        {
                                                                            devdif2 = ev21 - ev31;
                                                                        }
                                                                        else
                                                                        {
                                                                            devdif2 = ev31 - ev21;
                                                                        }
                                                                        if (devdif1 > devdif2)
                                                                        {
                                                                            finev3 = (Convert.ToDouble(ev21) + Convert.ToDouble(ev31)) / 2;
                                                                        }
                                                                        else if (devdif1 == devdif2)
                                                                        {
                                                                            if (ev11 > ev21)
                                                                            {
                                                                                finev3 = (Convert.ToDouble(ev11) + Convert.ToDouble(ev31)) / 2;
                                                                            }
                                                                            else
                                                                            {
                                                                                finev3 = (Convert.ToDouble(ev21) + Convert.ToDouble(ev31)) / 2;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            finev3 = (Convert.ToDouble(ev11) + Convert.ToDouble(ev31)) / 2;
                                                                        }
                                                                        finev3 = Math.Round(finev3, 0, MidpointRounding.AwayFromZero);
                                                                        avemark = finev3.ToString();
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    fpspread.Sheets[0].Cells[row_cnt, 5].Text = "";
                                                                    fpspread.Sheets[0].Cells[row_cnt, 6].Text = "";
                                                                    Double bindav = (Convert.ToDouble(ev11) + Convert.ToDouble(ev21)) / 2;
                                                                    bindav = Math.Round(bindav, 0, MidpointRounding.AwayFromZero);
                                                                    avemark = bindav.ToString();
                                                                }
                                                                if (total.Trim() != "" && intermarkforab.Trim() != "")
                                                                {
                                                                    //  Double getval = Convert.ToDouble(total);
                                                                    if (intermarkf223 < 0)
                                                                    {
                                                                        intermarkf223 = 0;
                                                                    }
                                                                    Double getval = Convert.ToDouble(avemark) + Convert.ToDouble(intermarkf223);
                                                                    if (minintmodeallmar <= Convert.ToDouble(intermarkforab))
                                                                    {
                                                                        if (getval < mintolmarks1)
                                                                        {
                                                                            Double markfaikl = mintolmarks1 - getval;
                                                                            markfaikl = markfaikl / max_ext_marks1 * papermaxexter;
                                                                            markfaikl = Math.Round(markfaikl, 0, MidpointRounding.AwayFromZero);
                                                                            Double extmarkfail = 0;
                                                                            if (Convert.ToDouble(avemark) < min_ext_marks1)
                                                                            {
                                                                                extmarkfail = min_ext_marks1 - Convert.ToDouble(avemark);
                                                                                extmarkfail = extmarkfail / max_ext_marks1 * papermaxexter;
                                                                                if (extmarkfail < 0)
                                                                                {
                                                                                    extmarkfail = 0;
                                                                                }
                                                                            }
                                                                            if (markfaikl <= markmodeart && extmarkfail <= markmodeart)
                                                                            {
                                                                                if (markfaikl > extmarkfail)
                                                                                {
                                                                                    fpspread.Sheets[0].Cells[row_cnt, 6].Text = markfaikl.ToString();

                                                                                }
                                                                                else
                                                                                {
                                                                                    fpspread.Sheets[0].Cells[row_cnt, 6].Text = extmarkfail.ToString();

                                                                                }
                                                                            }

                                                                        }
                                                                        else if (min_ext_marks1 > Convert.ToDouble(avemark))
                                                                        {
                                                                            Double markfaikl = min_ext_marks1 - Convert.ToDouble(avemark);
                                                                            markfaikl = markfaikl / max_ext_marks1 * papermaxexter;
                                                                            if (markfaikl <= markmodeart)
                                                                            {
                                                                                fpspread.Sheets[0].Cells[row_cnt, 6].Text = markfaikl.ToString();

                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                fpspread.Sheets[0].Cells[row_cnt, 5].Text = "";
                                                                Double bindav = (ev11 + ev21) / 2;
                                                                bindav = Math.Round(bindav, 0, MidpointRounding.AwayFromZero);
                                                                avemark = bindav.ToString();

                                                                if (total.Trim() != "" && intermarkforab.Trim() != "")
                                                                {
                                                                    if (intermarkf223 < 0)
                                                                    {
                                                                        intermarkf223 = 0;
                                                                    }
                                                                    Double getval = Convert.ToDouble(avemark) + Convert.ToDouble(intermarkf223);
                                                                    if (minintmodeallmar <= Convert.ToDouble(intermarkforab))
                                                                    {
                                                                        if (getval < mintolmarks1)
                                                                        {
                                                                            Double markfaikl = mintolmarks1 - getval;
                                                                            markfaikl = markfaikl / max_ext_marks1 * papermaxexter;
                                                                            Double extmarkfail = 0;
                                                                            if (Convert.ToDouble(avemark) < min_ext_marks1)
                                                                            {
                                                                                extmarkfail = min_ext_marks1 - Convert.ToDouble(avemark);
                                                                                extmarkfail = extmarkfail / max_ext_marks1 * papermaxexter;
                                                                                if (extmarkfail < 0)
                                                                                {
                                                                                    extmarkfail = 0;
                                                                                }
                                                                            }
                                                                            if (markfaikl <= markmodeart && extmarkfail <= markmodeart)
                                                                            {
                                                                                if (markfaikl > extmarkfail)
                                                                                {
                                                                                    fpspread.Sheets[0].Cells[row_cnt, 6].Text = markfaikl.ToString();

                                                                                }
                                                                                else
                                                                                {
                                                                                    fpspread.Sheets[0].Cells[row_cnt, 6].Text = extmarkfail.ToString();

                                                                                }
                                                                            }
                                                                        }
                                                                        else if (min_ext_marks1 > Convert.ToDouble(avemark))
                                                                        {
                                                                            Double markfaikl = min_ext_marks1 - Convert.ToDouble(avemark);
                                                                            markfaikl = markfaikl / max_ext_marks1 * papermaxexter;
                                                                            if (markfaikl <= markmodeart)
                                                                            {
                                                                                fpspread.Sheets[0].Cells[row_cnt, 6].Text = markfaikl.ToString();

                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }


                                                            fpspread.Sheets[0].Cells[row_cnt, 4].Text = avemark;
                                                            fpspread.Sheets[0].Cells[row_cnt, 4].HorizontalAlign = HorizontalAlign.Center;

                                                            if (intermarkf223 == -1 || intermarkf223 == -2 || intermarkf223 == -3)
                                                            {
                                                                intermarkf = "0";
                                                                if (intermarkf223 == -1)
                                                                {
                                                                    fpspread.Sheets[0].Cells[row_cnt, 7].Text = "AB";
                                                                }
                                                                else
                                                                {
                                                                    fpspread.Sheets[0].Cells[row_cnt, 7].Text = "0";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                fpspread.Sheets[0].Cells[row_cnt, 7].Text = intermarkf;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (chkicaretake.Checked == true)
                                                            {
                                                                ds2.Tables[0].DefaultView.RowFilter = "roll_no='" + roll_noo + "'";
                                                                dv3 = ds2.Tables[0].DefaultView;
                                                                if (dv3.Count > 0)
                                                                {
                                                                    externn = dv3[0]["external_mark"].ToString();
                                                                    resullts = dv3[0]["result"].ToString();
                                                                    if (externn.Trim() != "")
                                                                    {
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
                                                                            externn = "";
                                                                            resullts = "";
                                                                        }
                                                                        fpspread.Sheets[0].Cells[row_cnt, 8].Text = externn.ToString();
                                                                        fpspread.Sheets[0].Cells[row_cnt, 9].Text = total.ToString();
                                                                        fpspread.Sheets[0].Cells[row_cnt, 10].Text = resullts;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (previousinternalmark == "-1")
                                                        {
                                                            previousinternalmark = "AB";
                                                        }
                                                        fpspread.Sheets[0].Cells[row_cnt, 2].Text = "A";
                                                        fpspread.Sheets[0].Cells[row_cnt, 3].Text = "A";
                                                        fpspread.Sheets[0].Cells[row_cnt, 5].Text = "";
                                                        fpspread.Sheets[0].Cells[row_cnt, 6].Text = "";
                                                        fpspread.Sheets[0].Cells[row_cnt, 4].Text = "A";
                                                        fpspread.Sheets[0].Cells[row_cnt, 5].BackColor = Color.White;
                                                        fpspread.Sheets[0].Cells[row_cnt, 7].Text = previousinternalmark;
                                                        fpspread.Sheets[0].Cells[row_cnt, 8].Text = "A";
                                                        fpspread.Sheets[0].Cells[row_cnt, 9].Text = "AB";
                                                        fpspread.Sheets[0].Cells[row_cnt, 10].Text = "AB";
                                                        fpspread.Sheets[0].Cells[row_cnt, 10].Note = passorfail.ToString();
                                                    }
                                                }
                                                else
                                                {
                                                    if (previousinternalmark == "-1")
                                                    {
                                                        previousinternalmark = "AB";
                                                    }
                                                    fpspread.Sheets[0].Cells[row_cnt, 2].Text = "NE";
                                                    fpspread.Sheets[0].Cells[row_cnt, 3].Text = "NE";
                                                    fpspread.Sheets[0].Cells[row_cnt, 4].Text = "NE";
                                                    fpspread.Sheets[0].Cells[row_cnt, 5].Text = "";
                                                    fpspread.Sheets[0].Cells[row_cnt, 6].Text = "";
                                                    fpspread.Sheets[0].Cells[row_cnt, 5].BackColor = Color.White;
                                                    fpspread.Sheets[0].Cells[row_cnt, 7].Text = previousinternalmark;
                                                    fpspread.Sheets[0].Cells[row_cnt, 8].Text = "NE";
                                                    fpspread.Sheets[0].Cells[row_cnt, 9].Text = "NC";
                                                    fpspread.Sheets[0].Cells[row_cnt, 10].Text = "NC";
                                                    fpspread.Sheets[0].Cells[row_cnt, 10].Note = passorfail.ToString();
                                                }
                                            }
                                            else
                                            {
                                                if (previousinternalmark == "-1")
                                                {
                                                    previousinternalmark = "AB";
                                                }
                                                fpspread.Sheets[0].Cells[row_cnt, 2].Text = "NR";
                                                fpspread.Sheets[0].Cells[row_cnt, 3].Text = "NR";
                                                fpspread.Sheets[0].Cells[row_cnt, 4].Text = "NR";
                                                fpspread.Sheets[0].Cells[row_cnt, 5].Text = "";
                                                fpspread.Sheets[0].Cells[row_cnt, 6].Text = "";
                                                fpspread.Sheets[0].Cells[row_cnt, 5].BackColor = Color.White;
                                                fpspread.Sheets[0].Cells[row_cnt, 7].Text = previousinternalmark;
                                                fpspread.Sheets[0].Cells[row_cnt, 8].Text = "NR";
                                                fpspread.Sheets[0].Cells[row_cnt, 9].Text = "NC";
                                                fpspread.Sheets[0].Cells[row_cnt, 10].Text = "NC";
                                                fpspread.Sheets[0].Cells[row_cnt, 10].Note = passorfail.ToString();
                                            }
                                        }
                                        else
                                        {
                                            if (previousinternalmark == "-1")
                                            {
                                                previousinternalmark = "AB";
                                            }
                                            fpspread.Sheets[0].Cells[row_cnt, 2].Text = "LT";
                                            fpspread.Sheets[0].Cells[row_cnt, 3].Text = "LT";
                                            fpspread.Sheets[0].Cells[row_cnt, 4].Text = "LT";
                                            fpspread.Sheets[0].Cells[row_cnt, 5].Text = "";
                                            fpspread.Sheets[0].Cells[row_cnt, 6].Text = "";
                                            fpspread.Sheets[0].Cells[row_cnt, 5].BackColor = Color.White;
                                            fpspread.Sheets[0].Cells[row_cnt, 7].Text = previousinternalmark;
                                            fpspread.Sheets[0].Cells[row_cnt, 8].Text = "LT";
                                            fpspread.Sheets[0].Cells[row_cnt, 9].Text = "NC";
                                            fpspread.Sheets[0].Cells[row_cnt, 10].Text = "NC";
                                            fpspread.Sheets[0].Cells[row_cnt, 10].Note = passorfail.ToString();
                                        }
                                    }
                                    else
                                    {
                                        if (previousinternalmark == "-1")
                                        {
                                            previousinternalmark = "AB";
                                        }
                                        fpspread.Sheets[0].Cells[row_cnt, 2].Text = "M";
                                        fpspread.Sheets[0].Cells[row_cnt, 3].Text = "M";
                                        fpspread.Sheets[0].Cells[row_cnt, 4].Text = "M";
                                        fpspread.Sheets[0].Cells[row_cnt, 5].Text = "";
                                        fpspread.Sheets[0].Cells[row_cnt, 6].Text = "";
                                        fpspread.Sheets[0].Cells[row_cnt, 5].BackColor = Color.White;
                                        fpspread.Sheets[0].Cells[row_cnt, 7].Text = previousinternalmark;
                                        fpspread.Sheets[0].Cells[row_cnt, 8].Text = "M";
                                        fpspread.Sheets[0].Cells[row_cnt, 9].Text = "NC";
                                        fpspread.Sheets[0].Cells[row_cnt, 10].Text = "NC";
                                        fpspread.Sheets[0].Cells[row_cnt, 10].Note = passorfail.ToString();
                                    }
                                }
                                else
                                {
                                    if (chkicaretake.Checked == true)
                                    {
                                        fpspread.Sheets[0].Cells[row_cnt, 2].Text = "";
                                        fpspread.Sheets[0].Cells[row_cnt, 3].Text = "";
                                        fpspread.Sheets[0].Cells[row_cnt, 4].Text = "";
                                        fpspread.Sheets[0].Cells[row_cnt, 5].Text = "";
                                        fpspread.Sheets[0].Cells[row_cnt, 6].Text = "";
                                        fpspread.Sheets[0].Cells[row_cnt, 8].Text = "";
                                        fpspread.Sheets[0].Cells[row_cnt, 9].Text = "";
                                        fpspread.Sheets[0].Cells[row_cnt, 10].Text = "";

                                        ds.Tables[0].DefaultView.RowFilter = "roll_no='" + roll_noo + "'";
                                        DataView dvicaretake = ds.Tables[0].DefaultView;
                                        if (dvicaretake.Count > 0)
                                        {
                                            string esemark = dvicaretake[0]["EXE"].ToString();
                                            previousinternalmark = dvicaretake[0]["ICA"].ToString();
                                            if (previousinternalmark == "-1")
                                            {
                                                previousinternalmark = "AB";
                                            }

                                            Double getvalmark = Convert.ToDouble(esemark) / max_ext_marks1 * evalmaxmark;
                                            getvalmark = Math.Round(getvalmark, 0, MidpointRounding.AwayFromZero);
                                            //fpspread.Sheets[0].Cells[row_cnt, 2].Text = getvalmark.ToString();
                                            //fpspread.Sheets[0].Cells[row_cnt, 3].Text = getvalmark.ToString();
                                            //fpspread.Sheets[0].Cells[row_cnt, 4].Text = getvalmark.ToString();
                                            fpspread.Sheets[0].Cells[row_cnt, 7].Text = previousinternalmark.ToString();
                                            fpspread.Sheets[0].Cells[row_cnt, 8].Text = esemark.ToString();
                                        }
                                    }
                                    else if (strus == "NE")
                                    {
                                        fpspread.Sheets[0].Cells[row_cnt, 2].Text = "NE";
                                        fpspread.Sheets[0].Cells[row_cnt, 3].Text = "NE";
                                        fpspread.Sheets[0].Cells[row_cnt, 4].Text = "NE";
                                        fpspread.Sheets[0].Cells[row_cnt, 5].Text = "";
                                        fpspread.Sheets[0].Cells[row_cnt, 6].Text = "";
                                        fpspread.Sheets[0].Cells[row_cnt, 5].BackColor = Color.White;
                                        fpspread.Sheets[0].Cells[row_cnt, 7].Text = previousinternalmark;
                                        fpspread.Sheets[0].Cells[row_cnt, 8].Text = "NE";
                                        fpspread.Sheets[0].Cells[row_cnt, 9].Text = "NC";
                                        fpspread.Sheets[0].Cells[row_cnt, 10].Text = "NC";
                                        fpspread.Sheets[0].Cells[row_cnt, 10].Note = passorfail.ToString();
                                    }
                                    else if (strus == "NR")
                                    {
                                        fpspread.Sheets[0].Cells[row_cnt, 2].Text = "NR";
                                        fpspread.Sheets[0].Cells[row_cnt, 3].Text = "NR";
                                        fpspread.Sheets[0].Cells[row_cnt, 4].Text = "NR";
                                        fpspread.Sheets[0].Cells[row_cnt, 5].Text = "";
                                        fpspread.Sheets[0].Cells[row_cnt, 6].Text = "";
                                        fpspread.Sheets[0].Cells[row_cnt, 5].BackColor = Color.White;
                                        fpspread.Sheets[0].Cells[row_cnt, 7].Text = previousinternalmark;
                                        fpspread.Sheets[0].Cells[row_cnt, 8].Text = "NR";
                                        fpspread.Sheets[0].Cells[row_cnt, 9].Text = "NC";
                                        fpspread.Sheets[0].Cells[row_cnt, 10].Text = "NC";
                                        fpspread.Sheets[0].Cells[row_cnt, 10].Note = passorfail.ToString();
                                    }
                                    else if (fpspread.Sheets[0].Cells[row_cnt, 2].Text == "LT")
                                    {
                                        fpspread.Sheets[0].Cells[row_cnt, 2].Text = "LT";
                                        fpspread.Sheets[0].Cells[row_cnt, 3].Text = "LT";
                                        fpspread.Sheets[0].Cells[row_cnt, 4].Text = "LT";
                                        fpspread.Sheets[0].Cells[row_cnt, 5].Text = "";
                                        fpspread.Sheets[0].Cells[row_cnt, 6].Text = "";
                                        fpspread.Sheets[0].Cells[row_cnt, 5].BackColor = Color.White;
                                        fpspread.Sheets[0].Cells[row_cnt, 7].Text = previousinternalmark;
                                        fpspread.Sheets[0].Cells[row_cnt, 8].Text = "LT";
                                        fpspread.Sheets[0].Cells[row_cnt, 9].Text = "NC";
                                        fpspread.Sheets[0].Cells[row_cnt, 10].Text = "NC";
                                        fpspread.Sheets[0].Cells[row_cnt, 10].Note = passorfail.ToString();
                                    }
                                    else
                                    {
                                        fpspread.Sheets[0].Cells[row_cnt, 2].Text = "";
                                        fpspread.Sheets[0].Cells[row_cnt, 3].Text = "";
                                        fpspread.Sheets[0].Cells[row_cnt, 5].Text = "";
                                        fpspread.Sheets[0].Cells[row_cnt, 5].BackColor = Color.White;
                                        fpspread.Sheets[0].Cells[row_cnt, 7].Text = previousinternalmark;
                                        fpspread.Sheets[0].Cells[row_cnt, 4].Text = "";
                                        fpspread.Sheets[0].Cells[row_cnt, 8].Text = "";
                                        fpspread.Sheets[0].Cells[row_cnt, 9].Text = "";
                                        fpspread.Sheets[0].Cells[row_cnt, 10].Text = "";
                                        fpspread.Sheets[0].Cells[row_cnt, 10].Note = passorfail.ToString();
                                    }
                                }
                            }
                            if (chkincluevel2.Checked == true)
                            {
                                fpspread.Sheets[0].Cells[row_cnt, 3].Text = "";
                                fpspread.Sheets[0].Cells[row_cnt, 5].Text = "";
                            }
                        }
                        lblaane.Visible = true;
                        fpspread.Height = height;
                        fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
                        fpspread.Visible = false;
                        fpspread.Visible = true;
                        lblerr1.Visible = false;
                        btnprintt.Visible = true; btnprintt_t1.Visible = true;
                        btnsave1.Visible = true;
                    }
                }
                else
                {
                    btnreset.Visible = false; btnPrint.Visible = false;
                    lblaane.Visible = false;
                    btnsave1.Visible = false;
                    btnprintt.Visible = false; btnprintt_t1.Visible = false;
                    fpspread.Visible = false;
                    lblerr1.Visible = true;
                    lblerr1.Text = "No Records Found";
                }
            }
            fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
            int hei = 300;
            for (int col = 0; col < fpspread.Sheets[0].RowCount; col++)
            {
                hei = hei + fpspread.Sheets[0].Rows[col].Height;
            }
            fpspread.Height = hei;
            fpspread.SaveChanges();

            string group_code = Session["group_code"].ToString();
            string columnfield = "";
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString().ToLower() != "true"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and usercode='" + Session["usercode"] + "'";
            }

            string thtrdevese = da.GetFunction("select value from Master_Settings where settings='III Evaluation'  " + columnfield + "");
            if (thtrdevese.Trim() == "1")
            {
                for (int i = 0; i < fpspread.Sheets[0].Rows.Count; i++)
                {
                    if (fpspread.Sheets[0].Cells[i, 5].BackColor == Color.SkyBlue)
                    {
                        fpspread.Sheets[0].Cells[i, 5].Locked = false;
                    }
                    else
                    {
                        fpspread.Sheets[0].Cells[i, 5].Locked = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < fpspread.Sheets[0].Rows.Count; i++)
                {
                    fpspread.Sheets[0].Cells[i, 5].Locked = true;
                }
            }

            for (int i = 0; i < fpspread.Sheets[0].Rows.Count; i++)
            {
                if (fpspread.Sheets[0].Cells[i, 5].BackColor == Color.SkyBlue)
                {
                    if (fpspread.Sheets[0].Cells[i, 5].Text.ToString().Trim() == "")
                    {
                        fpspread.Sheets[0].Cells[i, 4].Text = "";
                        fpspread.Sheets[0].Cells[i, 6].Text = "";
                        fpspread.Sheets[0].Cells[i, 8].Text = "";
                        fpspread.Sheets[0].Cells[i, 9].Text = "";
                        fpspread.Sheets[0].Cells[i, 10].Text = "";
                    }
                }
            }

            if (chkonlyica.Checked == true)
            {
                for (int i = 0; i < fpspread.Sheets[0].Rows.Count; i++)
                {
                    fpspread.Sheets[0].Cells[i, 2].Text = "";
                    fpspread.Sheets[0].Cells[i, 3].Text = "";
                    fpspread.Sheets[0].Cells[i, 4].Text = "";
                    fpspread.Sheets[0].Cells[i, 8].Text = "";
                }
            }

            fpspread.Sheets[0].Columns[0].Locked = true;
            fpspread.Sheets[0].Columns[1].Locked = true;
            fpspread.Sheets[0].Columns[4].Locked = true;
            fpspread.Sheets[0].Columns[6].Locked = true;
            fpspread.Sheets[0].Columns[8].Locked = true;
            fpspread.Sheets[0].Columns[9].Locked = true;
            fpspread.Sheets[0].Columns[10].Locked = true;

            if (chk_onlycia.Checked == true)
            {
                fpspread.Sheets[0].Columns[2].Locked = true;
                fpspread.Sheets[0].Columns[3].Locked = true;
                fpspread.Sheets[0].Columns[5].Locked = true;
                fpspread.Sheets[0].Columns[7].Locked = false;
            }

            thtrdevese = da.GetFunction("select value from Master_Settings where settings='cia_security' " + columnfield + "");
            if (thtrdevese.Trim() == "1")
            {
                fpspread.Sheets[0].Columns[7].Locked = false;
            }
            else
            {
                fpspread.Sheets[0].Columns[7].Locked = true;
            }
            thtrdevese = da.GetFunction("select value from COE_Master_Settings where settings='Moderation Automatic'");
            if (thtrdevese.Trim() == "1")
            {
                chkmoderation.Checked = true;
                chkmoderation.Visible = false;
            }
            else
            {
                chkmoderation.Checked = false;
                chkmoderation.Visible = true;
            }
            fpspread.SaveChanges();

            if (chkIIIvaluation.Checked == true)
            {
                string collname = da.GetFunctionv("select collname from collinfo where college_code='" + Session["collegecode"].ToString() + "'");
                lblcolname.Text = collname;
                lblbatchshow.Text = ddlbatch.SelectedItem.Text.ToString();
                lblbranchshow.Text = ddldegree1.SelectedItem.Text.ToString() + " " + ddlbranch1.SelectedItem.Text.ToString();
                lblsemestershow.Text = ddlsem1.SelectedItem.Text.ToString();
                lblsubject_nameshow.Text = "Subject Code : " + ddlSubject.SelectedItem.Value.ToString();

                int e_month = Convert.ToInt32(ddlMonth1.SelectedItem.Value.ToString());
                string e_year = ddlYear1.SelectedItem.Text.ToString();
                string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(e_month);
                strMonthName = strMonthName + "  " + e_year;
                lblexamm_y_show.Text = strMonthName;
                Table tblhtml = new Table();
                tblhtml.CssClass = "style1in";
                TableRow row;
                TableCell cell;
                Label lbl;
                for (int i = 0; i < fpspread.Sheets[0].Rows.Count; i++)
                {
                    row = new TableRow();

                    cell = new TableCell();
                    cell.Width = 50;
                    lbl = new Label();
                    lbl.ID = "lblsno" + i + "";
                    //lbl.Attributes.Add("style", "font-size:14px;");
                    //lbl.CssClass = "requiredfild";
                    lbl.Text = fpspread.Sheets[0].Cells[i, 0].Text.ToString();
                    cell.Controls.Add(lbl);
                    row.Controls.Add(cell);

                    cell = new TableCell();
                    cell.Width = 100;
                    lbl = new Label();
                    lbl.ID = "lblrolno" + i + "";
                    // lbl.Attributes.Add("style", "font-size:14px;");

                    lbl.Text = fpspread.Sheets[0].Cells[i, 1].Text.ToString();
                    cell.Controls.Add(lbl);
                    row.Controls.Add(cell);

                    cell = new TableCell();
                    cell.Width = 100;
                    lbl = new Label();
                    lbl.ID = "lblval1" + i + "";
                    // lbl.Attributes.Add("style", "font-size:14px;");
                    // lbl.CssClass = "requiredfild";
                    lbl.Text = fpspread.Sheets[0].Cells[i, 2].Text.ToString();
                    cell.Controls.Add(lbl);
                    row.Controls.Add(cell);

                    cell = new TableCell();
                    cell.Width = 100;
                    lbl = new Label();
                    lbl.ID = "lblval2" + i + "";
                    //  lbl.Attributes.Add("style", "font-size:14px;");
                    // lbl.CssClass = "requiredfild";
                    lbl.Text = fpspread.Sheets[0].Cells[i, 3].Text.ToString();
                    cell.Controls.Add(lbl);
                    row.Controls.Add(cell);

                    cell = new TableCell();
                    cell.Width = 100;
                    lbl = new Label();
                    lbl.ID = "lbldiff" + i + "";
                    // lbl.Attributes.Add("style", "font-size:14px;");
                    // lbl.CssClass = "requiredfild";
                    int val1 = 0;
                    int val2 = 0;
                    int diff = 0;
                    if (fpspread.Sheets[0].Cells[i, 3].Text.ToString().Trim() != "" && fpspread.Sheets[0].Cells[i, 2].Text.ToString().Trim() != "")
                    {
                        val1 = Convert.ToInt32(fpspread.Sheets[0].Cells[i, 2].Text.ToString());
                        val2 = Convert.ToInt32(fpspread.Sheets[0].Cells[i, 3].Text.ToString());
                        if (val2 >= val1)
                        {
                            diff = val2 - val1;
                        }
                        else if (val2 <= val1)
                        {
                            diff = val1 - val2;
                        }

                    }

                    if (diff != 0)
                    {
                        lbl.Text = Convert.ToString(diff);
                    }
                    else
                    {
                        lbl.Text = "";
                    }

                    cell.Controls.Add(lbl);
                    row.Controls.Add(cell);

                    cell = new TableCell();
                    cell.Width = 100;
                    lbl = new Label();
                    lbl.ID = "lblremark" + i + "";
                    // lbl.Attributes.Add("style", "font-size:14px;");
                    //lbl.CssClass = "requiredfild";
                    lbl.Text = "";
                    cell.Controls.Add(lbl);
                    row.Controls.Add(cell);

                    tblhtml.Controls.Add(row);

                }

                inerttablethirdval.Controls.Add(tblhtml);

            }

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
            Boolean IIIvalmarkflag = false;
            fpspread.SaveChanges();
            if (chkIIIvaluation.Checked == true)
            {
                for (int i = 0; i < fpspread.Sheets[0].Rows.Count; i++)
                {
                    string IIImark = Convert.ToString(fpspread.Sheets[0].Cells[i, 5].Text);
                    if (IIImark.Trim() == "")
                    {
                        IIIvalmarkflag = true;
                        i = fpspread.Sheets[0].Rows.Count;
                    }
                }
                marksavefunction();
                if (IIIvalmarkflag == true)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully. But Some Students Marks Not Enterd')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                }
            }
            else
            {
                marksavefunction();
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
            }
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    protected void btnexcelimport_click(object sender, EventArgs e)
    {
        try
        {
            fpmarkimport.Visible = false;
            if (fpmarkexcel.FileName != "" && fpmarkexcel.FileName != null)
            {
                if (fpmarkexcel.FileName.EndsWith(".xls") || fpmarkexcel.FileName.EndsWith(".xlsx"))
                {
                    using (Stream stream = this.fpmarkexcel.FileContent as Stream)
                    {
                        stream.Position = 0;
                        this.fpmarkimport.OpenExcel(stream);
                        fpmarkimport.OpenExcel(stream);
                        fpmarkimport.SaveChanges();
                    }
                    string notexistsroll = "";
                    Boolean importflag = false;
                    for (int i = 1; i < fpmarkimport.Sheets[0].RowCount; i++)
                    {
                        string rollno = fpmarkimport.Sheets[0].Cells[i, 0].Text.ToString().Trim().ToLower();
                        Boolean rolfalg = false;
                        for (int j = 0; j < fpspread.Sheets[0].RowCount; j++)
                        {
                            string getroll = fpspread.Sheets[0].Cells[j, 1].Text.ToString().Trim().ToLower();
                            if (rollno == getroll)
                            {
                                rolfalg = true;
                                importflag = true;
                                if (rbeval.Checked == true)
                                {
                                    fpspread.Sheets[0].Cells[j, 2].Text = fpmarkimport.Sheets[0].Cells[i, 1].Text.ToString();
                                    fpspread.Sheets[0].Cells[j, 3].Text = fpmarkimport.Sheets[0].Cells[i, 2].Text.ToString();
                                }
                                else
                                {
                                    fpspread.Sheets[0].Cells[j, 7].Text = fpmarkimport.Sheets[0].Cells[i, 1].Text.ToString();
                                }
                                j = fpspread.Sheets[0].RowCount;
                            }
                        }
                        if (rolfalg == false)
                        {
                            if (notexistsroll == "")
                            {
                                notexistsroll = rollno;
                            }
                            else
                            {
                                notexistsroll = notexistsroll + " , " + rollno;
                            }
                        }
                    }

                    if (importflag == false)
                    {
                        lblerr1.Visible = true;
                        lblerr1.Text = "Regno's Not Found That File";
                    }
                    else
                    {
                        if (notexistsroll == "")
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully ')", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully But " + notexistsroll + " Regno(s) are  Not Found')", true);
                        }
                    }
                }
                else
                {
                    lblerr1.Visible = true;
                    lblerr1.Text = "Please Import Only Excel File only";
                }
            }
            else
            {
                lblerr1.Visible = true;
                lblerr1.Text = "Please Select The File and Then Proceed";
            }
            fpmarkimport.Visible = false;
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }
    public string loadmarkat(string mr)
    {
        string strgetval = "";
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
        return strgetval;
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    public void marksavefunction()
    {
        string insupdquery = "";
        try
        {
            Dictionary<string, string> dicdegreedetails = new Dictionary<string, string>();
            fpspread.SaveChanges();
            string batchyear = "";
            string degreecode = "";
            string subject_no = "";
            string examcode = "";
            Double remaim = 0;
            string roll_no = "";
            string result = "";
            int passorfail = 0;
            int insupdval = 0;
            string exammonth = ddlMonth1.SelectedValue.ToString();
            string examyear = ddlYear1.SelectedValue.ToString();
            int my = Convert.ToInt32(ddlMonth1.SelectedIndex.ToString()) + Convert.ToInt32(ddlYear1.SelectedValue.ToString()) * 12;

            string degreeval = "";
            string degreevalsub = "";
            string degreevalttab = "";
            string degreevalregis = "";
            if (chksubwise.Checked == false)
            {
                degreeval = " and ed.degree_code='" + ddlbranch1.SelectedValue + "'";
                degreevalsub = " and s.degree_code='" + ddlbranch1.SelectedValue + "'";
                degreevalttab = " and m.degree_code='" + ddlbranch1.SelectedValue + "'";
                degreevalregis = " and r.degree_code='" + ddlbranch1.SelectedValue + "'";
            }

            string[] spmaxsp = fpspread.Sheets[0].ColumnHeader.Cells[2, 8].Text.ToString().Split(':');
            Double maxexternal = Convert.ToDouble(spmaxsp[1]);
            spmaxsp = fpspread.Sheets[0].ColumnHeader.Cells[2, 2].Text.ToString().Split(':');
            Double entmaxexternal = Convert.ToDouble(spmaxsp[1]);

            Double moderationtot = 0;
            string modtotal = da.GetFunction("select distinct isnull(value,'') as value from COE_Master_Settings where settings='Mark Moderation'");
            if (modtotal.Trim() != "")
            {
                moderationtot = Convert.ToDouble(modtotal);
            }

            Hashtable hatgrade = new Hashtable();
            string grdaemaster = "select batch_year,grade_flag,degree_code from grademaster m where m.exam_month='" + exammonth + "' and m.exam_year='" + examyear + "' and m.grade_flag='3' " + degreevalttab + "";
            DataSet dsgrademaster = da.select_method_wo_parameter(grdaemaster, "Text");
            for (int d = 0; d < dsgrademaster.Tables[0].Rows.Count; d++)
            {
                if (!hatgrade.Contains(dsgrademaster.Tables[0].Rows[d]["batch_year"].ToString() + '-' + dsgrademaster.Tables[0].Rows[d]["degree_code"].ToString()))
                {
                    hatgrade.Add(dsgrademaster.Tables[0].Rows[d]["batch_year"].ToString() + '-' + dsgrademaster.Tables[0].Rows[d]["degree_code"].ToString(), dsgrademaster.Tables[0].Rows[d]["batch_year"].ToString());
                }
            }


            Double evalmaxmark = Convert.ToDouble(fpspread.Sheets[0].ColumnHeader.Cells[2, 2].Tag);
            int markdifffmoderer = Convert.ToInt32(fpspread.Sheets[0].ColumnHeader.Cells[2, 6].Note.ToString());

            Double minextmarks = 0;
            Double manextmarks = 0;
            Double minintmarks = 0;
            Double mintotalv = 0;
            Double maxtotalv = 0;
            Double minexternaleva = 0;
            Double mintotaleva = 0;

            for (int r = 0; r < fpspread.Sheets[0].RowCount; r++)
            {
                roll_no = fpspread.Sheets[0].Cells[r, 1].Note.ToString();
                batchyear = fpspread.Sheets[0].Cells[r, 0].Tag.ToString();
                degreecode = fpspread.Sheets[0].Cells[r, 5].Note.ToString();
                string creditpoint = fpspread.Sheets[0].Cells[r, 5].Tag.ToString();
                examcode = fpspread.Sheets[0].Cells[r, 1].Tag.ToString();
                string attempts = fpspread.Sheets[0].Cells[r, 2].Note.ToString();
                subject_no = fpspread.Sheets[0].Cells[r, 2].Tag.ToString();
                string evauation1 = fpspread.Sheets[0].Cells[r, 2].Text.ToString();
                string evauation2 = fpspread.Sheets[0].Cells[r, 3].Text.ToString();

                string icaevauation1 = fpspread.Sheets[0].Cells[r, 2].Text.ToString();
                string icaevauation2 = fpspread.Sheets[0].Cells[r, 2].Text.ToString();

                if (chkincluevel2.Checked == true)
                {
                    evauation2 = evauation1;
                }

                if (chkonlyica.Checked == true)
                {
                    evauation1 = "0";
                    evauation2 = "0";
                }

                string evauation3 = fpspread.Sheets[0].Cells[r, 5].Text.ToString();
                string icamark = fpspread.Sheets[0].Cells[r, 7].Text.ToString();
                string ese = fpspread.Sheets[0].Cells[r, 8].Text.ToString();
                string totalmarkvalu = fpspread.Sheets[0].Cells[r, 9].Text.ToString();
                string maderonmark = "";
                result = fpspread.Sheets[0].Cells[r, 10].Text.ToString();
                string cursem = fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Note.ToString();
                Double minicamoderatio = Convert.ToDouble(fpspread.Sheets[0].Cells[r, 9].Tag.ToString());

                string modemarkval = fpspread.Sheets[0].Cells[r, 9].Note.ToString();
                Double maxmarkmoderation = Convert.ToDouble(modemarkval);

                minintmarks = Convert.ToDouble(fpspread.Sheets[0].Cells[r, 6].Note.ToString());
                minextmarks = Convert.ToDouble(fpspread.Sheets[0].Cells[r, 7].Note.ToString());
                manextmarks = Convert.ToDouble(fpspread.Sheets[0].Cells[r, 7].Tag.ToString());
                maxtotalv = Convert.ToDouble(fpspread.Sheets[0].Cells[r, 8].Tag.ToString());
                mintotalv = Convert.ToDouble(fpspread.Sheets[0].Cells[r, 8].Note.ToString());

                minexternaleva = minextmarks / manextmarks * evalmaxmark;
                mintotaleva = mintotalv / maxtotalv * evalmaxmark;

                Double avg = 0;
                Double extemod = 0;
                Double moderat = 0;

                Double intark = 0;
                if (icamark == "")
                {
                    if (chkonlyica.Checked == true)
                    {
                        evauation1 = "";
                        evauation2 = "";
                    }
                    icamark = "Null";
                    result = "Null";
                    intark = 0;
                }
                else
                {
                    if (icamark.Trim().ToLower() == "ab")
                    {
                        icamark = "-1";
                    }
                    else
                    {
                        intark = Convert.ToDouble(icamark);
                    }
                }
                if (chk_onlycia.Checked == false)
                {
                    Double ev1 = 0;
                    Double ev2 = 0;
                    Double ev3 = 0;
                    Double totalmarkvalue = 0;
                    Double extmark = 0;
                    Double moderationmark = 0;



                    if (evauation1.Trim() == "")
                    {
                        evauation1 = "Null";
                        evauation2 = "Null";
                        evauation3 = "Null";
                        ese = "Null";
                        result = "Null";
                        totalmarkvalu = "Null";
                    }
                    else if (evauation1.Trim() != "" && evauation2.Trim() == "")
                    {
                        if (evauation1.Trim().ToLower() == "aaa" || evauation1.Trim().ToLower() == "ab" || evauation1.Trim().ToLower() == "aa" || evauation1.Trim().ToLower() == "a")
                        {
                            evauation1 = "-1";
                            ese = "-1";
                            result = "AAA";
                            passorfail = 0;
                            totalmarkvalu = icamark;
                            evauation2 = "-1";
                            evauation3 = "Null";
                        }
                        else if (evauation1.Trim().ToLower() == "ne" || evauation2.Trim().ToLower() == "ne")
                        {
                            evauation1 = "-2";
                            ese = "-2";
                            result = "Fail";
                            passorfail = 0;
                            totalmarkvalu = icamark;
                            evauation2 = "-2";
                            evauation3 = "Null";
                        }
                        else if (evauation1.Trim().ToLower() == "nr" || evauation2.Trim().ToLower() == "nr")
                        {
                            evauation1 = "-3";
                            ese = "-3";
                            result = "Fail";
                            passorfail = 0;
                            totalmarkvalu = icamark;
                            evauation2 = "-3";
                            evauation3 = "Null";
                        }
                        else if (evauation1.Trim().ToLower() == "m" || evauation2.Trim().ToLower() == "m")
                        {
                            evauation1 = "0";
                            ese = "0";
                            result = "WHD";
                            passorfail = 0;
                            totalmarkvalu = icamark;
                            evauation2 = "0";
                            evauation3 = "Null";
                        }
                        else if (evauation1.Trim().ToLower() == "lt" || evauation2.Trim().ToLower() == "lt")
                        {
                            evauation1 = "-4";
                            ese = "-4";
                            result = "Fail";
                            passorfail = 0;
                            totalmarkvalu = icamark;
                            evauation2 = "-4";
                            evauation3 = "Null";
                        }
                        else
                        {
                            ev1 = Convert.ToDouble(evauation1);
                            evauation2 = "Null";
                            evauation3 = "Null";
                            ese = "Null";
                            totalmarkvalu = "Null";
                            result = "Null";
                        }
                    }
                    else if (evauation1.Trim() != "" && evauation2.Trim() != "" && evauation3.Trim() == "")
                    {
                        if (evauation1.Trim().ToLower() == "aaa" || evauation1.Trim().ToLower() == "ab" || evauation1.Trim().ToLower() == "aa" || evauation1.Trim().ToLower() == "a")
                        {
                            evauation1 = "-1";
                            evauation2 = "-1";
                            ese = "-1";
                            result = "AAA";
                            totalmarkvalu = icamark;
                            evauation3 = "Null";
                        }
                        else if (evauation1.Trim().ToLower() == "ne" || evauation2.Trim().ToLower() == "ne")
                        {
                            evauation1 = "-2";
                            ese = "-2";
                            result = "Fail";
                            passorfail = 0;
                            totalmarkvalu = icamark;
                            evauation2 = "-2";
                            evauation3 = "Null";
                        }
                        else if (evauation1.Trim().ToLower() == "nr" || evauation2.Trim().ToLower() == "nr")
                        {
                            evauation1 = "-3";
                            ese = "-3";
                            result = "Fail";
                            passorfail = 0;
                            totalmarkvalu = icamark;
                            evauation2 = "-3";
                            evauation3 = "Null";
                        }
                        else if (evauation1.Trim().ToLower() == "lt" || evauation2.Trim().ToLower() == "lt")
                        {
                            evauation1 = "-4";
                            ese = "-4";
                            result = "Fail";
                            passorfail = 0;
                            totalmarkvalu = icamark;
                            evauation2 = "-4";
                            evauation3 = "Null";
                        }
                        else if (evauation1.Trim().ToLower() == "m" || evauation2.Trim().ToLower() == "m")
                        {
                            evauation1 = "0";
                            ese = "0";
                            result = "WHD";
                            passorfail = 0;
                            totalmarkvalu = icamark;
                            evauation2 = "0";
                            evauation3 = "Null";
                        }
                        else
                        {
                            ev1 = Convert.ToDouble(evauation1);
                            ev2 = Convert.ToDouble(evauation2);
                            avg = (ev1 + ev2) / 2;
                            avg = Math.Round(avg, 0, MidpointRounding.AwayFromZero);
                            intark = Convert.ToDouble(intark);
                            extmark = avg / entmaxexternal * maxexternal;
                            extmark = Math.Round(extmark, 0, MidpointRounding.AwayFromZero);
                            ese = extmark.ToString();
                            totalmarkvalue = extmark + intark;
                            totalmarkvalu = totalmarkvalue.ToString();
                            ev1 = Convert.ToDouble(evauation1);
                            ev2 = Convert.ToDouble(evauation2);

                            Double avg1 = 0;
                            avg1 = ev2 - ev1;
                            avg1 = Math.Abs(avg1);

                            if (icamark.Trim() != "")
                            {
                                if (!icamark.ToLower().Contains('a') && !icamark.ToLower().Contains("-1"))
                                {
                                    Double getval = Convert.ToDouble(totalmarkvalue);
                                    if ((minicamoderatio <= Convert.ToDouble(intark)) || (getval < mintotalv && minextmarks <= Convert.ToDouble(extmark)))
                                    {
                                        if (getval < mintotalv || minextmarks > Convert.ToDouble(extmark) || avg < minexternaleva)
                                        {
                                            extemod = mintotalv - getval;
                                            moderat = minexternaleva - avg;
                                            if (moderat > 0)
                                            {

                                                avg = Math.Round(avg, 0, MidpointRounding.AwayFromZero);
                                                Double maxrk = moderat + avg;
                                                Double extcheck = maxrk / entmaxexternal * maxexternal;
                                                extcheck = Math.Round(extcheck, 0, MidpointRounding.AwayFromZero);
                                                Double mintotcheck = extcheck + intark;
                                                Double extrenmoder = moderat;
                                                if (mintotcheck >= mintotalv)
                                                {
                                                    if (moderat == maxmarkmoderation + 1 && manextmarks != evalmaxmark)
                                                    {
                                                        moderat = maxmarkmoderation;
                                                    }
                                                    if (moderat <= maxmarkmoderation)
                                                    {
                                                        maderonmark = moderat.ToString();
                                                        extemod = (moderat) / minexternaleva * minextmarks;
                                                    }
                                                }
                                                else
                                                {
                                                    moderat = (extemod / manextmarks) * evalmaxmark;
                                                    Double modesetva = Math.Round(moderat, 0, MidpointRounding.AwayFromZero);
                                                    if (modesetva < moderat)
                                                    {
                                                        moderat = Math.Round(moderat, 0, MidpointRounding.AwayFromZero);
                                                        moderat++;
                                                    }
                                                    if ((avg % 2) > 0 && manextmarks != evalmaxmark)
                                                    {
                                                        moderat++;
                                                        if (moderat == maxmarkmoderation + 1 && manextmarks != evalmaxmark)
                                                        {
                                                            moderat = maxmarkmoderation;
                                                        }
                                                        if (extrenmoder > moderat)
                                                        {
                                                            moderat = extrenmoder;
                                                        }
                                                    }
                                                    if (moderat <= maxmarkmoderation)
                                                    {
                                                        maderonmark = moderat.ToString();
                                                    }
                                                }
                                            }
                                            else if (extemod > 0)
                                            {
                                                moderat = (extemod / manextmarks) * evalmaxmark;
                                                Double modesetva = Math.Round(moderat, 0, MidpointRounding.AwayFromZero);
                                                if (modesetva < moderat)
                                                {
                                                    moderat = Math.Round(moderat, 0, MidpointRounding.AwayFromZero);
                                                    moderat++;
                                                }
                                                if ((avg % 2) > 0 && manextmarks != evalmaxmark)
                                                {
                                                    moderat++;
                                                    if (moderat == maxmarkmoderation + 1)
                                                    {
                                                        moderat = maxmarkmoderation;
                                                    }
                                                }
                                                if (moderat <= maxmarkmoderation)
                                                {
                                                    maderonmark = moderat.ToString();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            evauation3 = "Null";
                            result = "Fail";
                            if (minintmarks <= intark && minextmarks <= extmark && mintotalv <= totalmarkvalue)
                            {
                                result = "Pass";
                            }
                        }
                    }
                    else if (evauation1.Trim() != "" && evauation2.Trim() != "" && evauation3.Trim() != "")
                    {
                        ev1 = Convert.ToDouble(evauation1);
                        ev2 = Convert.ToDouble(evauation2);
                        ev3 = Convert.ToDouble(evauation3);

                        Double avg1 = ev3 - ev1;
                        Double avg2 = ev3 - ev2;
                        avg1 = Math.Abs(avg1);
                        avg2 = Math.Abs(avg2);

                        if (avg1 < avg2)
                        {
                            avg = (ev1 + ev3) / 2;
                        }
                        else if (avg1 == avg2)
                        {
                            if (ev1 > ev2)
                            {
                                avg = (ev1 + ev3) / 2;
                            }
                            else
                            {
                                avg = (ev2 + ev3) / 2;
                            }
                        }
                        else
                        {
                            avg = (ev2 + ev3) / 2;
                        }

                        avg = Math.Round(avg, 0, MidpointRounding.AwayFromZero);
                        intark = Convert.ToDouble(intark);
                        extmark = avg / entmaxexternal * maxexternal;
                        extmark = Math.Round(extmark, 0, MidpointRounding.AwayFromZero);
                        ese = extmark.ToString();
                        totalmarkvalue = extmark + intark;
                        totalmarkvalu = totalmarkvalue.ToString();

                        if (icamark.Trim() != "")
                        {
                            if (!icamark.ToLower().Contains('a') && !icamark.ToLower().Contains("-1"))
                            {
                                Double getval = Convert.ToDouble(totalmarkvalue);
                                if ((minicamoderatio <= Convert.ToDouble(intark)) || (getval < mintotalv && minextmarks <= Convert.ToDouble(extmark)))
                                {
                                    if (getval < mintotalv || minextmarks > Convert.ToDouble(extmark))
                                    {
                                        extemod = mintotalv - getval;
                                        moderat = minexternaleva - avg;
                                        if (moderat > 0)
                                        {
                                            Double maxrk = moderat + avg;
                                            Double extcheck = maxrk / entmaxexternal * maxexternal;
                                            Double mintotcheck = extcheck + intark;
                                            Double extrenmoder = moderat;
                                            if (mintotcheck >= mintotalv)
                                            {
                                                if (moderat == maxmarkmoderation + 1 && manextmarks != evalmaxmark)
                                                {
                                                    moderat = maxmarkmoderation;
                                                }
                                                if (moderat <= maxmarkmoderation)
                                                {
                                                    maderonmark = moderat.ToString();
                                                    extemod = moderat / minexternaleva * minextmarks;
                                                }
                                            }
                                            else
                                            {
                                                moderat = (extemod / manextmarks) * evalmaxmark;
                                                Double modesetva = Math.Round(moderat, 0, MidpointRounding.AwayFromZero);
                                                if (modesetva < moderat)
                                                {
                                                    moderat = Math.Round(moderat, 0, MidpointRounding.AwayFromZero);
                                                    moderat++;
                                                }
                                                if ((avg % 2) > 0 && manextmarks != evalmaxmark)
                                                {

                                                    moderat++;
                                                    if (moderat == maxmarkmoderation + 1 && manextmarks != evalmaxmark)
                                                    {
                                                        moderat = maxmarkmoderation;
                                                    }
                                                    if (extrenmoder > moderat)
                                                    {
                                                        moderat = extrenmoder;
                                                    }
                                                }
                                                if (moderat <= maxmarkmoderation)
                                                {
                                                    maderonmark = moderat.ToString();
                                                }
                                            }
                                        }
                                        else if (extemod > 0)
                                        {
                                            moderat = (extemod / manextmarks) * evalmaxmark;
                                            Double modesetva = Math.Round(moderat, 0, MidpointRounding.AwayFromZero);
                                            if (modesetva < moderat)
                                            {
                                                moderat = Math.Round(moderat, 0, MidpointRounding.AwayFromZero);
                                                moderat++;
                                            }

                                            if ((avg % 2) > 0 && manextmarks != evalmaxmark)
                                            {
                                                moderat++;
                                                if (moderat == maxmarkmoderation + 1 && manextmarks != evalmaxmark)
                                                {
                                                    moderat = maxmarkmoderation;
                                                }
                                            }
                                            if (moderat <= maxmarkmoderation)
                                            {
                                                maderonmark = moderat.ToString();
                                            }
                                        }
                                    }
                                }
                            }
                        }


                        result = "Fail";
                        if (minintmarks <= intark && minextmarks <= extmark && mintotalv <= totalmarkvalue)
                        {
                            result = "Pass";
                        }

                    }
                    if (chkicaretake.Checked == true)
                    {
                        ese = fpspread.Sheets[0].Cells[r, 8].Text.ToString();
                        if (ese.Trim().ToLower().Contains('a'))
                        {
                            ev1 = -1;
                            ev2 = ev1;
                            ese = ev1.ToString();
                        }
                        else if (ese.Trim().ToLower() == "ne")
                        {
                            ev1 = -2;
                            ev2 = ev1;
                            ese = ev1.ToString();
                        }
                        else if (ese.Trim().ToLower() == "nr")
                        {
                            ev1 = -3;
                            ev2 = ev1;
                            ese = ev1.ToString();
                        }
                        else if (ese.Trim().ToLower() == "lt")
                        {
                            ev1 = -4;
                            ev2 = ev1;
                            ese = ev1.ToString();
                        }
                        else
                        {
                            if (ese.Trim() != "")
                            {
                                ev1 = Convert.ToDouble(ese) / Convert.ToDouble(manextmarks) * Convert.ToDouble(evalmaxmark);
                                ev2 = ev1;
                            }
                        }
                        totalmarkvalue = 0;
                        if (ese != "")
                        {
                            extmark = Convert.ToDouble(ese);
                        }
                        else
                        {
                            ese = "Null";
                            extmark = 0;
                        }
                        if (intark > 0)
                        {
                            if (extmark > 0)
                            {
                                totalmarkvalue = Convert.ToDouble(extmark) + Convert.ToDouble(intark);
                            }
                            else
                            {
                                totalmarkvalue = Convert.ToDouble(intark);
                            }
                        }
                        else if (extmark > 0)
                        {
                            totalmarkvalue = Convert.ToDouble(extmark);
                        }
                        totalmarkvalu = totalmarkvalue.ToString();
                        result = "Fail";
                        if (minintmarks <= intark && minextmarks <= extmark && mintotalv <= totalmarkvalue)
                        {
                            result = "Pass";
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
                        else
                        {
                            result = "Fail";
                        }
                    }

                    if (chkicaretake.Checked == true)
                    {
                        if (icaevauation1.Trim() == "" || icaevauation2.Trim() == "")
                        {
                            evauation1 = "Null";
                            evauation2 = "Null";
                        }
                    }

                    if (maderonmark != "" && chkmoderation.Checked == true)
                    {
                        moderationmark = avg + Convert.ToDouble(maderonmark);
                        moderationmark = moderationmark / evalmaxmark * maxexternal;
                        moderationmark = Math.Round(moderationmark, 0, MidpointRounding.AwayFromZero);
                        remaim = moderationtot - Convert.ToDouble(maderonmark);
                        insupdquery = "if exists(select * from moderation where exam_code=" + examcode + " and subject_no=" + subject_no + " and roll_no='" + roll_no + "' and degree_code=" + degreecode + " and exam_month=" + exammonth + " and exam_year=" + examyear + ")";
                        insupdquery = insupdquery + " update moderation set roll_no='" + roll_no + "',bf_moderation_extmrk=" + ese + ",af_moderation_extmrk=" + moderationmark + ",passmark='" + maderonmark + "',remainingmark='" + remaim + "',moderation_mark=" + moderationtot + " where exam_code=" + examcode + " and subject_no=" + subject_no + " and roll_no='" + roll_no + "' and degree_code=" + degreecode + " and semester=" + ddlsem1.SelectedValue.ToString() + " and exam_month=" + exammonth + " and exam_year=" + examyear + "";
                        insupdquery = insupdquery + "  else insert into moderation(batch_year,degree_code,exam_code,subject_no,Semester,roll_no,bf_moderation_extmrk,af_moderation_extmrk,passmark,remainingmark,moderation_mark,exam_month,exam_year) values (" + batchyear + "," + degreecode + ",'" + examcode + "'," + subject_no + "," + ddlsem1.SelectedValue.ToString() + ",'" + roll_no + "'," + ese + "," + moderationmark + ",'" + maderonmark + "','" + remaim + "'," + moderationtot + "," + exammonth + "," + examyear + ")";
                        int save1 = da.insert_method(insupdquery, hat, "Text");
                        if (save1 == 1)
                        {
                            if (fpspread.Sheets[0].RowCount > 0)
                            {
                                fpspread.Visible = true;
                            }
                        }

                        ese = moderationmark.ToString();
                        totalmarkvalue = moderationmark + intark;
                        totalmarkvalu = totalmarkvalue.ToString();

                        extmark = moderationmark;
                        totalmarkvalue = extmark + intark;

                        result = "Fail";
                        if (minintmarks <= intark && minextmarks <= extmark && mintotalv <= totalmarkvalue)
                        {
                            result = "Pass";
                        }
                        passorfail = 0;
                        if (result.Trim().ToLower() == "pass")
                        {
                            passorfail = 1;
                        }
                        if (icamark.ToLower().Contains('a'))
                        {
                            passorfail = 0;
                            result = "Fail";
                        }
                        //insupdquery = "update mark_entry set internal_mark=" + icamark + ",external_mark=" + ese + ",total=" + totalmarkvalu + ",result='" + result + "',passorfail='" + passorfail + "',attempts='" + attempts + "',evaluation1=" + evauation1 + ",evaluation2=" + evauation2 + ",evaluation3=" + evauation3 + "";
                        //insupdquery = insupdquery + " where exam_code='" + examcode + "' and roll_no='" + roll_no + "' and subject_no='" + subject_no + "'";
                        if (evauation1 == "0" && evauation2 == "0")
                        {
                            ese = "0";
                            totalmarkvalu = "0";
                        }

                        insupdquery = "if not exists(select * from mark_entry where exam_code='" + examcode + "' and roll_no='" + roll_no + "' and subject_no='" + subject_no + "')";
                        insupdquery = insupdquery + " insert into mark_entry (roll_no,subject_no,exam_code,internal_mark,external_mark,total,result,passorfail,attempts,MYData,rej_stat,cp,evaluation1,evaluation2,evaluation3)";
                        insupdquery = insupdquery + " values('" + roll_no + "','" + subject_no + "','" + examcode + "'," + icamark + "," + ese + "," + totalmarkvalu + ",'" + result + "','" + passorfail + "','" + attempts + "','" + my + "','0','" + creditpoint + "'," + evauation1 + "," + evauation2 + "," + evauation3 + ")";
                        insupdquery = insupdquery + " else";
                        insupdquery = insupdquery + " update mark_entry set internal_mark=" + icamark + ",external_mark=" + ese + ",total=" + totalmarkvalu + ",result='" + result + "',passorfail='" + passorfail + "',attempts='" + attempts + "',evaluation1=" + evauation1 + ",evaluation2=" + evauation2 + ",evaluation3=" + evauation3 + "";
                        insupdquery = insupdquery + " where exam_code='" + examcode + "' and roll_no='" + roll_no + "' and subject_no='" + subject_no + "'";
                        insupdval = da.insert_method(insupdquery, hat, "Text");

                    }
                    else
                    {
                        if (evauation1 == "0" && evauation2 == "0")
                        {
                            ese = "0";
                            totalmarkvalu = "0";
                        }
                        insupdquery = "if not exists(select * from mark_entry where exam_code='" + examcode + "' and roll_no='" + roll_no + "' and subject_no='" + subject_no + "')";
                        insupdquery = insupdquery + " insert into mark_entry (roll_no,subject_no,exam_code,internal_mark,external_mark,total,result,passorfail,attempts,MYData,rej_stat,cp,evaluation1,evaluation2,evaluation3)";
                        insupdquery = insupdquery + " values('" + roll_no + "','" + subject_no + "','" + examcode + "'," + icamark + "," + ese + "," + totalmarkvalu + ",'" + result + "','" + passorfail + "','" + attempts + "','" + my + "','0','" + creditpoint + "'," + evauation1 + "," + evauation2 + "," + evauation3 + ")";
                        insupdquery = insupdquery + " else";
                        insupdquery = insupdquery + " update mark_entry set internal_mark=" + icamark + ",external_mark=" + ese + ",total=" + totalmarkvalu + ",result='" + result + "',passorfail='" + passorfail + "',attempts='" + attempts + "',evaluation1=" + evauation1 + ",evaluation2=" + evauation2 + ",evaluation3=" + evauation3 + "";
                        insupdquery = insupdquery + " where exam_code='" + examcode + "' and roll_no='" + roll_no + "' and subject_no='" + subject_no + "'";

                        insupdval = da.update_method_wo_parameter(insupdquery, "Text");
                        //if (maderonmark == "")
                        //{
                        insupdquery = "delete from moderation where exam_code=" + examcode + " and subject_no=" + subject_no + " and roll_no='" + roll_no + "' and degree_code=" + degreecode + " and exam_month=" + exammonth + " and exam_year=" + examyear + "";
                        insupdval = da.update_method_wo_parameter(insupdquery, "Text");
                        // }
                    }
                }
                else
                {
                    evauation1 = "Null";
                    evauation2 = "Null";
                    evauation3 = "Null";
                    ese = "Null";
                    result = "Null";
                    totalmarkvalu = "Null";

                    if (examcode == "")
                    {
                        examcode = da.GetFunction("select exam_code from Exam_Details where batch_year='" + batchyear + "' and degree_code='" + degreecode + "' and Exam_Month='" + exammonth + "' and Exam_year='" + examyear + "'");
                        fpspread.Sheets[0].Cells[r, 1].Tag = examcode;
                    }

                    if (!hatgrade.Contains(batchyear + '-' + degreecode))
                    {
                        hatgrade.Add(batchyear + '-' + degreecode, batchyear);
                        insupdquery = "if not exists(select batch_year,grade_flag from grademaster where batch_year='" + batchyear + "' and degree_code='" + degreecode + "' and exam_month='" + exammonth + "' and exam_year='" + batchyear + "')";
                        insupdquery = insupdquery + " insert into grademaster (batch_year,degree_code,exam_month,exam_year,grade_flag)";
                        insupdquery = insupdquery + " values('" + batchyear + "','" + degreecode + "','" + exammonth + "','" + examyear + "','3')";
                        insupdquery = insupdquery + " update grademaster set grade_flag='3' where degree_code='" + degreecode + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "' and batch_year='" + batchyear + "'";
                        insupdval = da.update_method_wo_parameter(insupdquery, "Text");
                    }

                    if (examcode != "")
                    {
                        if (minextmarks == 0)
                        {
                            ese = "0";
                            result = "Pass";
                            passorfail = 1;
                            totalmarkvalu = icamark;
                        }
                        if (icamark.ToLower().Contains('a'))
                        {
                            passorfail = 0;
                            result = "Fail";
                        }
                        if (evauation1 == "0" && evauation2 == "0")
                        {
                            ese = "0";
                            totalmarkvalu = "0";
                        }
                        insupdquery = "if not exists(select * from mark_entry where exam_code='" + examcode + "' and roll_no='" + roll_no + "' and subject_no='" + subject_no + "')";
                        insupdquery = insupdquery + " insert into mark_entry (roll_no,subject_no,exam_code,internal_mark,external_mark,total,result,passorfail,attempts,MYData,rej_stat,cp,evaluation1,evaluation2,evaluation3)";
                        insupdquery = insupdquery + " values('" + roll_no + "','" + subject_no + "','" + examcode + "'," + icamark + "," + ese + "," + totalmarkvalu + ",'" + result + "','" + passorfail + "','" + attempts + "','" + my + "','0','" + creditpoint + "'," + evauation1 + "," + evauation2 + "," + evauation3 + ")";
                        insupdquery = insupdquery + " else";
                        insupdquery = insupdquery + " update mark_entry set internal_mark=" + icamark + ",external_mark=" + ese + ",total=" + totalmarkvalu + ",result='" + result + "',passorfail='" + passorfail + "',attempts='" + attempts + "',evaluation1=" + evauation1 + ",evaluation2=" + evauation2 + ",evaluation3=" + evauation3 + "";
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
            lblerr1.Text = "Query :" + insupdquery.ToString();
        }
    }
    public void clear()
    {
        lblaane.Visible = false;
        btnreset.Visible = false; btnPrint.Visible = false;
        btnprintt.Visible = false; btnprintt_t1.Visible = false;
        fpspread.Visible = false;
        fpspread.Visible = false;
        btnsave1.Visible = false;
        chkmoderation.Visible = false;
        btnexcelimport.Visible = false;
        fpmarkexcel.Visible = false;
        rbeval.Visible = false;
        rbcia.Visible = false;
        chkincluevel2.Visible = false;
    }
    protected void chksubwise_CheckedChanged(object sender, EventArgs e)
    {

        if (chk_onlycia.Checked == false)
        {
            clear();
            ddldegree1.Enabled = true;
            ddlbranch1.Enabled = true;
            if (chksubwise.Checked == true)
            {
                ddldegree1.Enabled = false;
                ddlbranch1.Enabled = false;
            }
            bindsem1();
            subjecttypebind();
            subjectbind();
        }
        else
        {
            chksubwise.Checked = false;
            lblerr1.Visible = true;
            lblerr1.Text = "Please Update The Only I.C.A Mark Settings";
        }
    }
}