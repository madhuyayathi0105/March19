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


public partial class ExamValidatorMarksNew : System.Web.UI.Page
{
    string CollegeCode;

    bool yes_flag = false;

    DataView dv1 = new DataView();
    DataView dv2 = new DataView();
    DataView dv3 = new DataView();
    DAccess2 da = new DAccess2();
    DataSet ds2 = new DataSet();
    DataSet dsss = new DataSet();
    InsproDirectAccess dirAccess = new InsproDirectAccess();
    Hashtable hat = new Hashtable();

    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string grouporusercode = string.Empty;

    //protected void txtbundleno_TextChanged(object sender, EventArgs e)
    //{
    //    string bundle_no=Request

    //}
     

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
                //string getcodeba = da.GetFunctionv("select value from COE_Master_Settings where settings = 'Direct_CIA'");
                //if (getcodeba.Trim() != "")
                //{
                //    if (getcodeba.Trim() == "1")
                //    {
                //        chk_onlycia.Checked = true;
                //    }
                //    else
                //    {
                //        chk_onlycia.Checked = false;
                //    }
                //}
                UpdatePanel24.Visible = true;
                Page.MaintainScrollPositionOnPostBack = true;
                chk_onlycia.Visible = false;
                lblaane.Visible = false;
                fpspread.Visible = false;
                BindCollege();
                year1();
                loadtype();
                chkmoderation.Visible = false;
                btnexcelimport.Visible = false;
                fpmarkexcel.Visible = false;
                rbeval.Visible = false;
                rbcia.Visible = false;
                chkincluevel2.Visible = false;
                //degree();
                // month1();
                //month11();
                //subjectbind();
                Session["MarkEntrySave"] = 1;
                chkbundleno.Visible = true;
                chkbundleno.Checked = true;
                txtbundleno.Visible = true;

            }
        }

        catch (Exception ex)
        {
        }


    }

    public void BindCollege()
    {
        try
        {
            string group_code = Session["group_code"].ToString();
            string columnfield = string.Empty;
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            DataSet ds = da.select_method("bind_college", hat, "sp");
            ddlCollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlCollege.Enabled = true;
                ddlCollege.DataSource = ds;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
            }
        }
        catch (Exception ex)
        {
          
        }
    }

    public void year1()
    {
        try
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
            bool setflag = false;
            string collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            string settings = "Exam year and month for Mark" + collegeCode.Trim();
            ddlYear1.Items.Clear();
            DataTable dtsettings = dirAccess.selectDataTable("select distinct value from master_settings where settings='" + settings + "' " + grouporusercode + "");

            //string getexamvalue = da.GetFunction("select distinct value from master_settings where settings='" + settings + "' " + grouporusercode + "");//Exam year and month Valuation
            if (dtsettings.Rows.Count > 0)
            {
                foreach (DataRow dtyear in dtsettings.Rows)
                {
                    string getexamvalue = Convert.ToString(dtyear["value"]);
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
                }
            }
            //setflag = false;
            if (setflag == false)
            {
                //dsss.Clear();
                dsss = da.select_method_wo_parameter(" select distinct Exam_year from exam_details order by Exam_year desc", "Text");
                if (dsss.Tables[0].Rows.Count > 0)
                {
                    ddlYear1.DataSource = dsss;
                    ddlYear1.DataTextField = "Exam_year";
                    ddlYear1.DataValueField = "Exam_year";
                    ddlYear1.DataBind();

                }
            }
            ddlYear1.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
        }
        catch
        {

        }
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
            bool setflag = false;
            string monthval = string.Empty;
            string collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            string settings = "Exam year and month for Mark" + collegeCode.Trim();
           // string getexamvalue = da.GetFunction("select value from master_settings where settings='Exam year and month for Mark' " + grouporusercode + "");//Exam year and month Valuation
             DataTable dtsettings = dirAccess.selectDataTable("select distinct value from master_settings where settings='" + settings + "' " + grouporusercode + "");

            //string getexamvalue = da.GetFunction("select distinct value from master_settings where settings='" + settings + "' " + grouporusercode + "");//Exam year and month Valuation
             string val1=string.Empty;
             if (dtsettings.Rows.Count > 0)
             {
                 foreach (DataRow dtmonth in dtsettings.Rows)
                 {
                     string getexamvalue = Convert.ToString(dtmonth["value"]);
                     if (getexamvalue.Trim() != null && getexamvalue.Trim() != "" && getexamvalue.Trim() != "0")
                     {
                         string[] spe = getexamvalue.Split(',');
                         if (spe.GetUpperBound(0) == 1)
                         {
                             if (spe[1].Trim() != "0")
                             {
                                 string val = spe[1].ToString();
                                 if(string.IsNullOrEmpty(val1))
                                    val1 =val;
                                 else if(!val1.Contains(val))
                                     val1=val1+"','"+val;
                             }
                         }
                     }
                 }
             }
            if(!string.IsNullOrEmpty(val1))
                monthval = " and Exam_month in('" + val1 + "')";
            dsss.Clear();
            string year1 = ddlYear1.SelectedValue;
            string strsql = "select distinct Exam_month,upper(convert(varchar(3),DateAdd(month,Exam_month,-1))) as monthName from exam_details where Exam_year='" + year1 + "'" + monthval + " order by Exam_month desc";
            dsss = da.select_method_wo_parameter(strsql, "Text");
            if (dsss.Tables[0].Rows.Count > 0)
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
            string collegecode = Convert.ToString(ddlCollege.SelectedValue);
            ddltype.Items.Clear();
            string strquery = "select distinct type from course where college_code='" + collegecode + "' and type is not null and type<>''";
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

    public void degree()
    {
        try
        {
            ddldegree1.Items.Clear();
            string usercode = Session["usercode"].ToString();
            //string collegecode = Session["collegecode"].ToString();
            string collegecode = Convert.ToString(ddlCollege.SelectedValue);
            string singleuser = Session["single_user"].ToString();
            string group_user = Session["group_code"].ToString();
            string type = string.Empty;
            if (ddltype.Enabled == true)
            {
                if (ddltype.Items.Count > 0)
                {
                    if (Convert.ToString(ddltype.SelectedItem.Text).Trim().ToLower() != "all" && Convert.ToString(ddltype.SelectedItem.Text).Trim().ToLower() != "")
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
            //string collegecode = Session["collegecode"].ToString();
            string collegecode = Convert.ToString(ddlCollege.SelectedValue);
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
            bool first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            hat.Clear();
            string usercode = Session["usercode"].ToString();
            string collegecode = Convert.ToString(ddlCollege.SelectedValue);
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

    protected void subjectbind()
    {
        try
        {
            ddlSubject.Items.Clear();
            dsss.Clear();
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

            if (chksubwise.Checked == false)
            {
                qeryss = "SELECT distinct ss.subject_type,s.subject_name,s.subject_code,s.subject_name+' - '+s.subject_code as subnamecode FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,syllabus_master sy,sub_sem ss where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and ed.degree_code='" + branc + "' and sy.semester='" + semmv + "' and  ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "' ";
                qeryss = qeryss + " union SELECT distinct ss.subject_type,s.subject_name,s.subject_code,s.subject_name+' - '+s.subject_code as subnamecode FROM subjectChooser sc,subject s,Registration r,Exam_Details ed,sub_sem ss where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and sc.semester=ed.Current_Semester and s.subType_no=ss.subType_no and r.degree_code='" + branc + "'  and sc.semester='" + semmv + "' and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "' and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' order by s.subject_name,s.subject_code desc";
            }
            //if (chk_onlycia.Checked == true)
            //{
            //    qeryss = "SELECT distinct s.subject_name,s.subject_code,s.subject_name+' - '+s.subject_code as subnamecode FROM subjectChooser sc,subject s,Registration r,Exam_Details ed,sub_sem ss where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and sc.semester=ed.Current_Semester and ss.subType_no=s.subType_no and r.degree_code='" + branc + "' and sc.semester='" + semmv + "' and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "' order by s.subject_name,s.subject_code desc";
            //}
            if (chkmarkbased.Checked == true)
            {
                qeryss = "SELECT distinct s.subject_name,s.subject_code,s.subject_name+' - '+s.subject_code as subnamecode FROM Exam_Details ED,mark_entry m,subject s,syllabus_master sy,sub_sem ss where ed.exam_code=m.exam_code and s.subject_no=m.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and ed.degree_code='" + branc + "' and sy.semester='" + semmv + "' and  ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "'  order by s.subject_name,s.subject_code desc";
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
            if (chksubwise.Checked == false)
            {
                qeryss = "SELECT distinct ss.subject_type FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,syllabus_master sy,sub_sem ss where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and ed.degree_code='" + branc + "' and sy.semester='" + semmv + "' and  ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' ";
                qeryss = qeryss + " union SELECT distinct ss.subject_type FROM subjectChooser sc,subject s,Registration r,Exam_Details ed,sub_sem ss where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and sc.semester=ed.Current_Semester and s.subType_no=ss.subType_no and r.degree_code='" + branc + "'  and sc.semester='" + semmv + "' and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' order by ss.subject_type";
            }
            if (chk_onlycia.Checked == true)
            {
                qeryss = "SELECT distinct ss.subject_type FROM subjectChooser sc,subject s,Registration r,Exam_Details ed,sub_sem ss where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and sc.semester=ed.Current_Semester and ss.subType_no=s.subType_no and r.degree_code='" + branc + "' and sc.semester='" + semmv + "' and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' order by ss.subject_type";
            }
            if (chkmarkbased.Checked == true)
            {
                qeryss = "SELECT distinct ss.subject_type FROM Exam_Details ED,mark_entry m,subject s,syllabus_master sy,sub_sem ss where ed.exam_code=m.exam_code and s.subject_no=m.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and ed.degree_code='" + branc + "' and sy.semester='" + semmv + "' and  ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' order by ss.subject_type";
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

    public void clear()
    {
        lblaane.Visible = false;
        btnreset.Visible = false;
        btnprintt.Visible = false;
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
        Response.Redirect("default.aspx", false);
    }

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        year1();
        month1();
        loadtype();

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
            string collegename = string.Empty;


            string examexternalmark = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[2, 2].Tag);  // added by jairam 30-01-2016
            string maxexternalmark = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[2, 8].Tag);
            string maxinternalmark = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[2, 7].Tag);
            string totlmatrk = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[2, 9].Tag);
            string modrationva = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Tag) + " %";


            string catgory = string.Empty;
            string coll_name = string.Empty;
            string ugorpg = string.Empty;
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

    protected void btnviewre_Click(object sender, EventArgs e)
    {
        try
        {
            chkmoderation.Checked = false;
            if (ddlYear1.Items.Count == 0)
            {
                btnreset.Visible = false;
                lblaane.Visible = false;
                btnsave1.Visible = false;
                btnprintt.Visible = false;
                fpspread.Visible = false;
                lblerr1.Visible = true;
                lblerr1.Text = "No Exam Year Found";
                return;
            }
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
            if (!chkbundleno.Checked)
            {
                if (ddltype.Items.Count > 0)
                {
                    if (ddltype.SelectedIndex == 0)
                    {
                        btnreset.Visible = false;
                        lblaane.Visible = false;
                        btnsave1.Visible = false;
                        btnprintt.Visible = false;
                        fpspread.Visible = false;
                        lblerr1.Visible = true;
                        lblerr1.Text = "Please Select Type";
                        return;
                    }
                }
                if (ddldegree1.Items.Count > 0)
                {
                    if (ddldegree1.SelectedIndex == 0 && chksubwise.Checked == false)
                    {
                        btnreset.Visible = false;
                        lblaane.Visible = false;
                        btnsave1.Visible = false;
                        btnprintt.Visible = false;
                        fpspread.Visible = false;
                        lblerr1.Visible = true;
                        lblerr1.Text = "Please Select Degree";
                        return;
                    }
                }
                else
                {

                }
                if (ddlbranch1.Items.Count > 0)
                {
                    if (ddlbranch1.SelectedIndex == 0 && chksubwise.Checked == false)
                    {
                        btnreset.Visible = false;
                        lblaane.Visible = false;
                        btnsave1.Visible = false;
                        btnprintt.Visible = false;
                        fpspread.Visible = false;
                        lblerr1.Visible = true;
                        lblerr1.Text = "Please Select branch";
                        return;
                    }
                }
                if (ddlsem1.SelectedIndex == 0)
                {
                    btnreset.Visible = false;
                    lblaane.Visible = false;
                    btnsave1.Visible = false;
                    btnprintt.Visible = false;
                    fpspread.Visible = false;
                    lblerr1.Visible = true;
                    lblerr1.Text = "Please Select Semester";
                    return;
                }
                if (ddlsubtype.Items.Count == 0)
                {

                }
                if (ddlSubject.Items.Count > 0)
                {
                    if (ddlSubject.SelectedIndex == 0)
                    {
                        btnreset.Visible = false;
                        lblaane.Visible = false;
                        btnsave1.Visible = false;
                        btnprintt.Visible = false;
                        fpspread.Visible = false;
                        lblerr1.Visible = true;
                        lblerr1.Text = "Please Select Subject";
                        return;
                    }
                }
                if (ddlSubject.Items.Count == 0)
                {

                }
                else
                {
                    buttongo();
                }
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
            
            if (chkbundleno.Checked == true)//Rajkumar on 28-5-2018
            {
                string bundlno = txtbundleno.Text;
                if (string.IsNullOrEmpty(bundlno))
                {
                    imgdiv2.Visible = true;
                    pnl2.Visible = true;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "Enter the Bundle Number";
                    fpspread.Visible = false;
                    return;

                }
            }
            //*******
            

            Session["MarkEntrySave"] = 1;
            int markround = 0;
            string getmarkround = da.GetFunctionv("select value from COE_Master_Settings where settings='Mark Round of'").Trim();
            if (!string.IsNullOrEmpty(getmarkround.Trim()) && getmarkround.Trim() != "0")
            {
                int.TryParse(getmarkround, out markround);
            }
            clear();
            string passorfail = string.Empty;
            string maxintmark = string.Empty;
            string minextmark = string.Empty;
            string maxextmark = string.Empty;
            string minintmark = string.Empty;
            string mintotmark = string.Empty;
            string maxtotmark = string.Empty;
            bool temp=true;
            if (temp)//ddlMonth1.SelectedValue.Trim() != "" && ddlYear1.SelectedValue.Trim() != "" && ddlSubject.SelectedValue.Trim() != ""
            {
                #region Dummy Number Display

                byte dummyNumberMode = getDummyNumberMode();//0-serial , 1-random
                string dummyNumberType = string.Empty;
                if (DummyNumberType() == 1)
                {
                    dummyNumberType = " and subject='" + ddlSubject.SelectedValue + "' ";
                }
                else
                {
                    dummyNumberType = " and isnull(subject,'')='' ";
                }

                string selDummyQ = "select dummy_no,regno,roll_no from dummynumber where exam_month='" + ddlMonth1.SelectedValue.ToString() + "' and exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and DNCollegeCode='" + CollegeCode + "' and degreecode='" + ddlbranch1.SelectedValue + "' " + dummyNumberType + "  and dummy_type='" + dummyNumberMode + "' --  and semester='" + ddlsem1.SelectedValue.ToString() + "' and exam_date='11/01/2016' ";

                DataTable dtMappedNumbers = dirAccess.selectDataTable(selDummyQ);
                bool showDummyNumber = ShowDummyNumber();
                if (showDummyNumber)
                {
                    if (dtMappedNumbers.Rows.Count == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Dummy Numbers Generated')", true);
                        return;
                    }
                }

                #endregion

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

                double minicamoderation = 0;
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
                fpspread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Center;

                string degreeval = string.Empty;
                string degreevalregmoder = string.Empty;
                string degreevalttab = string.Empty;
                string degreevalregis = string.Empty;
                if (chksubwise.Checked == false)
                {
                    degreeval = " and ed.degree_code='" + ddlbranch1.SelectedValue + "'";
                    degreevalregmoder = " and M.degree_code='" + ddlbranch1.SelectedValue + "'";
                    degreevalttab = " and e.degree_code='" + ddlbranch1.SelectedValue + "'";
                    degreevalregis = " and r.degree_code='" + ddlbranch1.SelectedValue + "'";
                }
                string subjectCodeNew = Convert.ToString(ddlSubject.SelectedValue);
                if (chkbundleno.Checked && !string.IsNullOrEmpty(txtbundleno.Text))
                    subjectCodeNew = da.GetFunction("select distinct  s.subject_code from exam_seating es,Exam_Details ed,subject s where s.subject_no=es.subject_no and ed.Exam_Month='" + Convert.ToString(ddlMonth1.SelectedValue) + "' and ed.Exam_year='" + Convert.ToString(ddlYear1.SelectedValue) + "' and es.bundle_no='" + txtbundleno.Text + "' ");

                string subCode = string.Empty;
                if (!string.IsNullOrEmpty(Convert.ToString(ddlSubject.SelectedValue)) && !chkbundleno.Checked)
                {
                    subCode = "and s.subject_code='" + Convert.ToString(subjectCodeNew) + "'";
                }
                if (chkbundleno.Checked && !string.IsNullOrEmpty(txtbundleno.Text))//modified by Mullai
                {
                    degreeval = string.Empty;
                    degreevalregmoder = string.Empty;
                    degreevalttab = string.Empty;
                    degreevalregis = string.Empty;
                }
                Dictionary<string, int> dicContoNotPaid = new Dictionary<string, int>();
                Dictionary<string, int> dicNotEligible = new Dictionary<string, int>();
                DataSet dsCondoNotPaid = new DataSet();
                DataSet dsNotEligible = new DataSet();
                DataSet dsDegreeDetails = new DataSet();
                dsDegreeDetails = da.select_method_wo_parameter("select c.Edu_Level,LTRIM(RTRIM(isnull(c.type,''))) as type,c.Course_Name,dt.Dept_Name,dt.dept_acronym,c.Course_Id,dt.Dept_Code,dg.Degree_Code,c.Priority,c.college_code from Course c,Degree dg,Department dt where c.college_code=dt.college_code and dt.college_code=dg.college_code and dt.college_code=c.college_code and c.Course_Id=dg.Course_Id and dt.Dept_Code=dg.Dept_Code order by c.college_code,c.Edu_Level desc,dg.Degree_Code", "TEXT");

                if (!chkbundleno.Checked)
                {
                    if (!CheckIncludeCondoUnpaidForMarkEntry())
                    {
                        dsCondoNotPaid = da.select_method_wo_parameter("select len(r.reg_no),r.Batch_Year,r.Roll_No,r.Reg_No,r.Stud_Name,isnull(el.fine_amt,'0') total_fee,CASE WHEN isnull(el.isCondonationFee,'0')='1' THEN 'Paid' ELSE 'Unpaid' END status,el.semester,r.Current_Semester from Registration r,Eligibility_list el where el.Roll_no=r.Roll_No and r.Batch_Year=el.batch_year and el.degree_code=r.degree_code and r.App_No=el.app_no and el.is_eligible='2' and isnull(el.isCondonationFee,'0')='0' and el.semester='" + Convert.ToString(ddlsem1.SelectedItem.Text).Trim() + "' " + degreevalregis + "  order by len(r.reg_no),r.reg_no,r.stud_name", "Text");
                    }
                    if (dsCondoNotPaid.Tables.Count > 0 && dsCondoNotPaid.Tables[0].Rows.Count > 0)
                    {
                        dicContoNotPaid.Clear();
                        foreach (DataRow dr in dsCondoNotPaid.Tables[0].Rows)
                        {
                            string rollNo = string.Empty;
                            rollNo = Convert.ToString(dr["Roll_No"]).Trim();
                            if (!dicContoNotPaid.ContainsKey(rollNo))
                            {
                                dicContoNotPaid.Add(rollNo, 1);
                            }
                        }
                    }

                    dsNotEligible = da.select_method_wo_parameter("select len(r.reg_no),r.Batch_Year,r.Roll_No,r.Reg_No,r.Stud_Name,isnull(el.fine_amt,'0') total_fee,CASE WHEN isnull(el.isCondonationFee,'0')='1' THEN 'Paid' ELSE 'Unpaid' END status,el.semester,r.Current_Semester from Registration r,Eligibility_list el where el.Roll_no=r.Roll_No and r.Batch_Year=el.batch_year and el.degree_code=r.degree_code and r.App_No=el.app_no and el.is_eligible='3' " + degreevalregis + " and el.semester='" + Convert.ToString(ddlsem1.SelectedItem.Text).Trim() + "' and ISNULL(isCompleteRedo,'0')='0' and cc=0 order by len(r.reg_no),r.reg_no,r.stud_name", "Text");//and cc=0 added by rajkumar 1/2/2018

                    if (dsNotEligible.Tables.Count > 0 && dsNotEligible.Tables[0].Rows.Count > 0)
                    {
                        dicNotEligible.Clear();
                        foreach (DataRow dr in dsNotEligible.Tables[0].Rows)
                        {
                            string rollNo = string.Empty;
                            rollNo = Convert.ToString(dr["Roll_No"]).Trim();
                            if (!dicNotEligible.ContainsKey(rollNo))
                            {
                                dicNotEligible.Add(rollNo, 1);
                            }
                        }
                    }
                }

                bool isFinalYearModerationApplicable = false;
                string commonModerationApplicable = string.Empty;
                string qry = " select LinkValue from New_InsSettings where LinkName='MaximumModerationApplicable' and college_code ='" + CollegeCode + "'";
                commonModerationApplicable = da.GetFunctionv(qry);
                if (!string.IsNullOrEmpty(commonModerationApplicable) && commonModerationApplicable.Trim().ToLower() == "1")
                {
                    isFinalYearModerationApplicable = true;
                }

                DataSet dsCommonFinModerationBatch = new DataSet();
                DataSet dsCommonFinModerationSem = new DataSet();
                DataSet dscommonFinalYearModerationMark = new DataSet();
                double commonFinalYrModeration = 0;
                string commonFinalYearModerationMark = string.Empty;
                dsCommonFinModerationBatch = da.select_method_wo_parameter("select LinkValue,LinkName from New_InsSettings where LinkName like 'MaximumModerationAppBatch%' and college_code ='" + CollegeCode + "'", "text");
                dsCommonFinModerationSem = da.select_method_wo_parameter("select LinkValue,LinkName from New_InsSettings where LinkName like 'MaximumModerationAppSem%' and college_code ='" + CollegeCode + "'", "text");
                //commonFinalYearModerationMark = da.GetFunction("select LinkValue from New_InsSettings where LinkName like 'MaximumModerationAppMark%' and college_code ='" + CollegeCode + "'");
                dscommonFinalYearModerationMark = da.select_method_wo_parameter("select LinkValue,LinkName from New_InsSettings where LinkName like 'MaximumModerationAppMark%' and college_code ='" + CollegeCode + "'", "text");


                DataSet dsNewInternal = new DataSet();
                //dsNewInternal = da.select_method_wo_parameter("select sc.roll_no,s.subject_no,s.subject_code,total,actual_total,Exam_Year,Exam_Month from camarks ca,subject s,subjectChooser sc where ca.roll_no=sc.roll_no and sc.subject_no=s.subject_no and s.subject_no=ca.subject_no and ca.subject_no=sc.subject_no and s.subject_code='" + ddlSubject.SelectedValue + "'", "text");

                dsNewInternal = da.select_method_wo_parameter("select sc.roll_no,s.subject_no,s.subject_code,total,actual_total,ca.Exam_Year,ca.Exam_Month from camarks ca,subject s,subjectChooser sc,Exam_Details ed,Registration r where r.Roll_No=sc.roll_no  and ca.roll_no=r.Roll_No and ca.roll_no=sc.roll_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and sc.subject_no=s.subject_no and s.subject_no=ca.subject_no and ca.subject_no=sc.subject_no and s.subject_code='" + subjectCodeNew + "' and ed.Exam_Month='" + Convert.ToString(ddlMonth1.SelectedValue).Trim() + "' and ed.Exam_year='" + Convert.ToString(ddlYear1.SelectedItem.Text).Trim() + "' " + degreeval + "  and isnull(r.Reg_No,'') <>'' order by r.Reg_No", "Text");

                string qeryss = string.Empty;
                if (chkbundleno.Checked && !string.IsNullOrEmpty(txtbundleno.Text))
                {
                    qeryss = "SELECT distinct ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,ead.subject_no,s.subject_name,subject_code,r.Roll_No,ISNULL(es.bundle_no,'0') as bundleId,r.Reg_No,r.Stud_Name,ead.attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'REGULAR' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag,isnull(thirdValmarkDifferent,'0') as ThirdValuationMarkDifference FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,Registration r,exam_seating es where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and r.Roll_No=ea.roll_no and s.subject_no=es.subject_no and es.regno=r.Reg_No and  ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and es.bundle_no in('" + txtbundleno.Text + "') and isnull(r.Reg_No,'') <>''  and s.subject_code='" + subjectCodeNew + "' order by ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,ead.subject_no,s.subject_name,subject_code,r.Roll_No,ISNULL(es.bundle_no,'0'),r.Reg_No,r.Stud_Name,ead.attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,s.Moderation_Mark,s.min_int_moderation,r.delflag ";

                }
                else
                {
                    qeryss = "SELECT ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,ead.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,ead.attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'REGULAR' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag,isnull(thirdValmarkDifferent,'0') as ThirdValuationMarkDifference FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,Registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and r.degree_code=ed.degree_code and  r.Roll_No=ea.roll_no " + degreeval + " and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and s.subject_code='" + subjectCodeNew + "' and isnull(r.Reg_No,'') <>'' ";//r.Batch_Year=ed.batch_year removed by rajkumar 2018/02/01
                    // qeryss = qeryss + " union SELECT ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,s.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,'' attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'NR' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag FROM Exam_Details ED,tbl_not_registred nt,subject s,Registration r where ed.Exam_Month=nt.exam_month and ed.Exam_year=nt.exam_year and s.subject_no=nt.subject_no and r.Roll_No=nt.roll_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year " + degreeval + " and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and s.subject_code='" + ddlSubject.SelectedValue + "' and r.cc=0 and isnull(r.Reg_No,'') <>'' ";
                    qeryss = qeryss + " union SELECT ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,s.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,'' attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'NR' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag,isnull(thirdValmarkDifferent,'0') as ThirdValuationMarkDifference FROM Exam_Details ED,tbl_not_registred nt,subject s,Registration r,subjectChooser sc where ed.Exam_Month=nt.exam_month and ed.Exam_year=nt.exam_year and s.subject_no=nt.subject_no and r.Roll_No=nt.roll_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and sc.semester=ed.current_semester " + degreeval + " and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and s.subject_code='" + subjectCodeNew + "' and r.cc=0 and isnull(r.Reg_No,'') <>'' order by ed.batch_year desc,ed.degree_code,ed.current_semester,r.reg_no,sts";
                    //qeryss = qeryss + " union SELECT ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,s.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,'' attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'NE' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag,isnull(thirdValmarkDifferent,'0') as ThirdValuationMarkDifference FROM Exam_Details ED,studentsemestersubjectdebar nt,subject s,Registration r where r.Roll_No=nt.roll_no and nt.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and (ed.current_semester=nt.semester or r.CC=1) " + degreeval + " and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and s.subject_code='" + ddlSubject.SelectedValue + "' and isnull(r.Reg_No,'') <>'' and r.cc=0 order by ed.batch_year desc,ed.degree_code,ed.current_semester,r.reg_no,sts";
                    // qeryss = qeryss + " union SELECT ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,s.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,'0' attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'REGULAR' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag FROM subjectChooser sc,subject s,Registration r,Exam_Details ed where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and sc.semester=ed.Current_Semester " + degreeval + " and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and s.subject_code='" + ddlSubject.SelectedValue + "' and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' and isnull(r.Reg_No,'') <>'' order by ed.batch_year desc,ed.degree_code,ed.current_semester,r.reg_no,sts";
                    if (chk_onlycia.Checked == true)
                    {
                        //qeryss = "SELECT ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,sc.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,'0' attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,r.cc,s.writtenmaxmark,'REGULAR' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag,isnull(thirdValmarkDifferent,'0') as ThirdValuationMarkDifference FROM subjectChooser sc,subject s,Registration r,Exam_Details ed where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and sc.semester=ed.Current_Semester " + degreevalregis + " and sc.semester='" + ddlsem1.SelectedValue.ToString() + "' and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and s.subject_code='" + ddlSubject.SelectedValue + "' order by r.batch_year desc,r.degree_code,r.current_semester,sc.subject_no,r.reg_no ";
                    }
                    if (chkicaretake.Checked == true)
                    {
                        //qeryss = "select ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,isnull(thirdValmarkDifferent,'0') as ThirdValuationMarkDifference,s.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,'1' attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'REGULAR' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag,max(m.internal_mark) ICA,max(m.external_mark) EXE from mark_entry m,subject s,Registration r,Exam_Details ed,exam_application ea,exam_appl_details ead where m.subject_no=s.subject_no and r.Roll_No=m.roll_no and ed.batch_year=r.Batch_Year  AND M.subject_no=ead.subject_no and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.roll_no=r.Roll_No and ead.type='1' and ed.degree_code=r.degree_code " + degreeval + " and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and s.subject_code='" + ddlSubject.SelectedValue + "' group by ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,s.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,r.Batch_Year ,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,s.Moderation_Mark,s.min_int_moderation,r.delflag  order by ed.batch_year desc,ed.degree_code,ed.current_semester,s.subject_no,r.reg_no";
                        qeryss = "select ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,isnull(thirdValmarkDifferent,'0') as ThirdValuationMarkDifference,s.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,'1' attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'REGULAR' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag,(select max(m1.internal_mark) from mark_entry m1 where m1.subject_no=ead.subject_no and ea.roll_no=m1.roll_no) internal_mark,(select max(m1.external_mark) from mark_entry m1 where m1.subject_no=ead.subject_no and ea.roll_no=m1.roll_no) EXE from subject s,Registration r,Exam_Details ed,exam_application ea,exam_appl_details ead where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.roll_no=r.Roll_No and ead.subject_no=s.subject_no and ead.type='1' and ed.degree_code=r.degree_code " + degreeval + " and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and s.subject_code='" + subjectCodeNew + "'  order by r.batch_year desc,r.degree_code,ed.current_semester,ead.subject_no,r.reg_no";
                        //qeryss = "select ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,s.subject_no,isnull(thirdValmarkDifferent,'0') as ThirdValuationMarkDifference,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,'1' attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'Regular' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag,max(m.internal_mark) ICA,max(m.external_mark) EXE from mark_entry m,subject s,Registration r,Exam_Details ed,exam_application ea,exam_appl_details ead where m.subject_no=s.subject_no and r.Roll_No=m.roll_no and ed.batch_year=r.Batch_Year and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.roll_no=r.Roll_No and ead.type='1' and ed.degree_code=r.degree_code " + degreeval + " and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and s.subject_code='" + ddlSubject.SelectedValue + "' and m.roll_no not in (select m1.roll_no from mark_entry m1 where m1.roll_no=m.roll_no and m.subject_no=m1.subject_no and m1.result='Pass') group by ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,s.subject_no,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,r.Batch_Year ,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,s.Moderation_Mark,s.min_int_moderation,r.delflag  order by ed.batch_year desc,ed.degree_code,ed.current_semester,s.subject_no,r.reg_no";
                    }
                    if (chkmarkbased.Checked == true)
                    {
                        qeryss = "SELECT ed.degree_code,ed.batch_year,ed.current_semester,ed.exam_code,m.subject_no,isnull(thirdValmarkDifferent,'0') as ThirdValuationMarkDifference,s.subject_name,subject_code,r.Roll_No,r.Reg_No,r.Stud_Name,m.attempts,r.Batch_Year,s.min_int_marks,s.min_ext_marks,s.max_ext_marks,s.max_int_marks,s.subject_no,s.maxtotal,s.mintotal,s.credit_points,s.writtenmaxmark,r.cc,'REGULAR' as sts,s.Moderation_Mark,s.min_int_moderation,r.delflag,m.internal_mark,m.external_mark EXE FROM Exam_Details ED,mark_entry m,subject s,Registration r where ed.exam_code=m.exam_code and s.subject_no=m.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and r.Roll_No=m.roll_no and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' " + degreeval + " and s.subject_code='" + subjectCodeNew + "' and isnull(r.Reg_No,'') <>''   order by r.batch_year desc,r.degree_code,r.current_semester,m.subject_no,r.reg_no";
                    }
                }
                qeryss = qeryss + " select roll_no,regno,Course_Name,Dept_Name,dummy_no  from  dummynumber du,Degree d,Department dt,Course c,subject s where du.degreecode =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and exam_month ='" + ddlMonth1.SelectedValue.ToString() + "' and exam_year ='" + ddlYear1.SelectedItem.Text.ToString() + "'  and s.subject_no=du.subject_no and s.subject_code='" + subjectCodeNew + "' and  (dummy_type ='1' or dummy_type ='0')";
               
                DataSet ds = da.select_method_wo_parameter(qeryss, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 4, 1);
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Reg No";
                    if (showDummyNumber) fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Dummy No";
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
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "I.C.A";
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

                    if (!string.IsNullOrEmpty(subjectCodeNew))
                    {
                        string subject_no = Convert.ToString(ddlSubject.SelectedValue).Trim();
                        string exam_code = Convert.ToString(ds.Tables[0].Rows[0]["exam_code"]).Trim();
                        string sem = Convert.ToString(ddlsem1.SelectedValue).Trim();

                        string getdetails = "select me.roll_no,me.external_mark,me.internal_mark,me.result,me.evaluation1,me.evaluation2,me.evaluation3,me.external_mark,me.total,me.exam_code  from mark_entry me,Exam_Details ed,subject s where me.exam_code=ed.exam_code and me.subject_no=s.subject_no and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + Convert.ToString(ddlYear1.SelectedItem).Trim() + "'  " + degreeval + " and s.subject_code='" + subjectCodeNew + "'";
                        getdetails = getdetails + "  select * from moderation m,subject s where m.subject_no=s.subject_no and s.subject_code='" + subjectCodeNew + "' " + degreevalregmoder + " and m.exam_year='" + Convert.ToString(ddlYear1.SelectedItem).Trim() + "'";
                        getdetails = getdetails + " select convert(varchar(50),exam_date,105) as exam_date,exam_session from exmtt e,exmtt_det et,subject s where e.exam_code=et.exam_code and et.subject_no=s.subject_no and e.Exam_month='" + ddlMonth1.SelectedValue.ToString() + "' and e.Exam_year='" + Convert.ToString(ddlYear1.SelectedItem).Trim() + "' " + degreevalttab + " and s.subject_code='" + subjectCodeNew + "' ";
                        ds2 = da.select_method_wo_parameter(getdetails, "Text");

                        double ev11 = 0;
                        double ev21 = 0;
                        double ev31 = 0;
                        double external_mark = 0;
                        double modermarks = 0;
                        double difff1 = 0;
                        double difff2 = 0;

                        string ev1 = string.Empty;
                        string ev2 = string.Empty;
                        string ev3 = string.Empty;
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
                        FarPoint.Web.Spread.RegExpCellType rgex = new FarPoint.Web.Spread.RegExpCellType();
                        FarPoint.Web.Spread.RegExpCellType rgexInternal = new FarPoint.Web.Spread.RegExpCellType();

                        double min_int_marks1 = 0;
                        // Convert.ToDouble(ds.Tables[0].Rows[0]["min_int_marks"]);
                        double.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["min_int_marks"]).Trim(), out min_int_marks1);
                        min_int_marks1 = Math.Round(min_int_marks1, markround, MidpointRounding.AwayFromZero);

                        double mintolmarks1 = 0;
                        // Convert.ToDouble(ds.Tables[0].Rows[0]["mintotal"]);
                        double.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["mintotal"]).Trim(), out mintolmarks1);
                        mintolmarks1 = Math.Round(mintolmarks1, markround, MidpointRounding.AwayFromZero);

                        double min_ext_marks1 = 0;
                        //  Convert.ToDouble(ds.Tables[0].Rows[0]["min_ext_marks"]);
                        double.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["min_ext_marks"]).Trim(), out min_ext_marks1);
                        min_ext_marks1 = Math.Round(min_ext_marks1, markround, MidpointRounding.AwayFromZero);

                        double max_ext_marks1 = 0;
                        //  Convert.ToDouble(ds.Tables[0].Rows[0]["max_ext_marks"]);
                        double.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["max_ext_marks"]).Trim(), out max_ext_marks1);
                        max_ext_marks1 = Math.Round(max_ext_marks1, markround, MidpointRounding.AwayFromZero);

                        double max_int_marks1 = 0;
                        //  Convert.ToDouble(ds.Tables[0].Rows[0]["max_int_marks"]);
                        double.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["max_int_marks"]).Trim(), out max_int_marks1);
                        max_int_marks1 = Math.Round(max_int_marks1, markround, MidpointRounding.AwayFromZero);

                        double max_tol_marks1 = 0;
                        // Convert.ToDouble(ds.Tables[0].Rows[0]["maxtotal"]);
                        double.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["maxtotal"]).Trim(), out max_tol_marks1);
                        max_tol_marks1 = Math.Round(max_tol_marks1, markround, MidpointRounding.AwayFromZero);

                        //string subjectMarkDifference = string.Empty;
                        //double subjectMarkDifferent = 0;
                        //subjectMarkDifference = Convert.ToString(ds.Tables[0].Rows[0]["ThirdValuationMarkDifference"]).Trim();
                        //double.TryParse(Convert.ToString(subjectMarkDifference).Trim(), out subjectMarkDifferent);
                        //subjectMarkDifferent = Math.Round(subjectMarkDifferent, markround, MidpointRounding.AwayFromZero);

                        string regularNewRaja = @"^(AB)?$|^(Ab)?$|^(aB)?$|^(ab)?$|^(a)?$|^(A)?$|^(M)?$|^(m)?$|^(lt)?$|^(LT)?$|^(Lt)?$|^(lT)?$";
                        string regexpree = "AB|ab|a|A|M|m|LT|lt|00|01|02|03|04|05|06|07|08|09|";
                        string newExapressionRaja = string.Empty;
                        string roundValuesRaja = string.Empty;
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
                        //rgex.ValidationExpression = "(\\W|^)(" + regexpree + "|\\sdarnit|heck)(\\W|$)";

                        rgexInternal.ValidationExpression = regularNewRaja + newExapressionRaja;
                        rgexInternal.ErrorMessage = "Please Enter the Mark Less Than or Equal to (" + max_int_marks1 + ")";
                        fpspread.Sheets[0].Columns[7].CellType = rgexInternal;
                        string type = string.Empty;
                        string educationLevel = string.Empty;
                        if (chksubwise.Checked == false)
                        {
                            type = da.GetFunction("select edu_level from course where course_id=" + ddldegree1.SelectedValue + "");
                        }
                        else
                        {
                            type = da.GetFunction("select distinct c.Edu_Level from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.ToString() + "' and s.subject_code='" + subjectCodeNew + "'");
                        }
                        educationLevel = type;
                        if (type.Trim() != "" && type.Trim() != "0" && type != null)
                        {
                            string extexammaxmark = ds.Tables[0].Rows[0]["writtenmaxmark"].ToString();
                            if (extexammaxmark.Trim() == "" || extexammaxmark.Trim() == "0" || extexammaxmark == null)
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

                                regularNewRaja = @"^(AB)?$|^(Ab)?$|^(aB)?$|^(ab)?$|^(a)?$|^(A)?$|^(M)?$|^(m)?$|^(lt)?$|^(LT)?$|^(Lt)?$|^(lT)?$|^(nr)?$|^(Nr)?$|^(nR)?$|^(NR)?$|^(NE)?$|^(nE)?$|^(Ne)?$|^(ne)?$|^(RA)?$|^(rA)?$|^(Ra)?$|^(ra)?$";
                                regexpree = "AB|ab||NR|nr|NE|ne|ra||RA|a|A|M|m|LT|lt|00|01|02|03|04|05|06|07|08|09|";
                                newExapressionRaja = string.Empty;
                                for (int i = 0; i <= Convert.ToInt32(extexammaxmark); i++)
                                {

                                    regexpree = regexpree + "|" + "" + i + "";
                                    if (i != Convert.ToInt32(extexammaxmark))
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
                                //rgex.ValidationExpression = "(\\W|^)(" + regexpree + "|\\sdarnit|heck)(\\W|$)";

                                rgex.ValidationExpression = regularNewRaja + newExapressionRaja;
                                rgex.ErrorMessage = "Please Enter the Mark Less Than or Equal to (" + extexammaxmark + ")";
                                fpspread.Sheets[0].Columns[2].CellType = rgex;
                                fpspread.Sheets[0].Columns[3].CellType = rgex;
                                fpspread.Sheets[0].Columns[4].CellType = rgex;
                                fpspread.Sheets[0].Columns[5].CellType = rgex;
                            }
                            else
                            {
                                btnsave1.Visible = false;
                                btnprintt.Visible = false;
                                lblerr1.Text = "Please Set Max External Mark";
                                lblerr1.Visible = true;
                                chkmoderation.Visible = false;
                                return;
                            }
                            string minicamodeval = "0";
                            if (chksubwise.Checked == false)
                            {
                                minicamodeval = da.GetFunction("select distinct s.min_int_moderation from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.ToString() + "' and s.subject_code='" + subjectCodeNew + "'");
                            }
                            if (minicamodeval.Trim() == "" || minicamodeval.Trim() == "0")
                            {
                                minicamodeval = da.GetFunctionv("select value from COE_Master_Settings where settings = 'min Ica Moderation " + type + "'");
                            }
                            if (minicamodeval.Trim() == "")
                            {
                                minicamodeval = "0";
                            }
                            minicamoderation = 0;// Convert.ToDouble(minicamodeval);
                            double.TryParse(minicamodeval.Trim(), out minicamoderation);
                        }

                        fpspread.Sheets[0].ColumnHeader.Cells[2, 9].Text = "Max : " + (max_ext_marks1 + max_int_marks1).ToString();
                        fpspread.Sheets[0].ColumnHeader.Cells[2, 8].Text = "Max : " + max_ext_marks1.ToString();
                        fpspread.Sheets[0].ColumnHeader.Cells[2, 7].Text = "Max : " + max_int_marks1.ToString();
                        fpspread.Sheets[0].ColumnHeader.Cells[2, 7].Tag = max_int_marks1.ToString();
                        fpspread.Sheets[0].ColumnHeader.Cells[2, 8].Tag = max_ext_marks1.ToString();
                        fpspread.Sheets[0].ColumnHeader.Cells[2, 9].Tag = (max_ext_marks1 + max_int_marks1).ToString();

                        double passint = 0;// Math.Round((min_int_marks1 / max_int_marks1) * 100, markround);
                        double passext = 0;// Math.Round((min_ext_marks1 / max_ext_marks1) * 100, markround);

                        if (max_int_marks1 > 0)
                        {
                            passint = Math.Round(Convert.ToDouble(min_int_marks1 / max_int_marks1) * 100, markround);
                        }
                        if (max_ext_marks1 > 0)
                        {
                            passext = Math.Round(Convert.ToDouble(min_ext_marks1 / max_ext_marks1) * 100, markround);
                        }

                        double Mark_Difference1 = 0;
                        string Mark_Difference = da.GetFunction("select distinct isnull(value,'') as value from COE_Master_Settings where settings='Mark Difference'");
                        //if (string.IsNullOrEmpty(subjectMarkDifference) || subjectMarkDifference.Trim() == "0")
                        //{
                        Mark_Difference = da.GetFunction("select distinct isnull(value,'') as value from COE_Master_Settings where settings='Mark Difference'");
                        if (Mark_Difference != "")
                        {
                            Mark_Difference1 = 0;// Convert.ToDouble(Mark_Difference);
                            double.TryParse(Mark_Difference.Trim(), out Mark_Difference1);
                        }
                        else
                        {
                            Mark_Difference1 = 0;

                        }
                        //}
                        //else
                        //{
                        //    Mark_Difference1 = subjectMarkDifferent;
                        //}

                        double Mark_moderation1 = 0;

                        string Mark_moderation = string.Empty;
                        if (chksubwise.Checked == false)
                        {
                            Mark_moderation = da.GetFunction("select distinct s.Moderation_Mark from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.ToString() + "' and s.subject_code='" + subjectCodeNew + "'");
                        }
                        if (Mark_moderation.Trim() == "" || Mark_moderation.Trim() == "0")
                        {
                            Mark_moderation = da.GetFunction("select distinct isnull(value,'') as value from COE_Master_Settings where settings='Mark Moderation'").Trim();
                        }

                        if (Mark_moderation != "")
                        {
                            Mark_moderation1 = 0; // Convert.ToDouble(Mark_moderation);
                            double.TryParse(Mark_moderation.Trim(), out Mark_moderation1);
                        }
                        else
                        {
                            Mark_moderation1 = 0;
                        }
                    
                        fpspread.Sheets[0].ColumnHeader.Cells[2, 6].Text = "Max : " + Mark_moderation1.ToString() + " %";
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Tag = Mark_moderation1.ToString();
                        fpspread.Sheets[0].ColumnHeader.Cells[2, 6].Tag = minicamoderation;
                        fpspread.Sheets[0].ColumnHeader.Cells[2, 6].Note = Mark_Difference1.ToString();

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
                            chkincluevel2.Visible = true;
                            btnexcelimport.Visible = true;
                            fpmarkexcel.Visible = true;
                            rbeval.Visible = true;
                            rbcia.Visible = true;
                            rbcia.Checked = false;
                            rbeval.Checked = true;
                            DataSet dsstuatt = new DataSet();
                            btnsave1.Visible = true;
                            btnprintt.Visible = true;
                            lblaane.Visible = true;
                            string strsetval = da.GetFunction("select value from COE_Master_Settings where settings='Attendance Link mark'");
                            if (strsetval == "1")
                            {
                                if (ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0)
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
                                        string strattmarval = "select a.roll_no from attendance a,Exam_Details ed,exam_application ea,exam_appl_details ead where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.roll_no=a.roll_no  and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and ead.subject_no='" + subjectCodeNew + "' and a.month_year='" + monva + "' and a." + ath + " is not null and a." + ath + "<>'0' and a." + ath + "<>''";
                                        dsstuatt = da.select_method_wo_parameter(strattmarval, "Text");
                                        if (dsstuatt.Tables.Count > 0 && dsstuatt.Tables[0].Rows.Count == 0)
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
                                rollno = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]).Trim();
                                if (!hat.Contains(rollno))
                                {
                                    //dynamic iis = "sa";
                                    hat.Add(rollno, rollno);
                                    regno = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]).Trim();
                                    batchyerr = Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]).Trim();
                                    string examcode = Convert.ToString(ds.Tables[0].Rows[i]["exam_code"]).Trim();
                                    string subno = Convert.ToString(ds.Tables[0].Rows[i]["subject_no"]).Trim();
                                    string attempts = Convert.ToString(ds.Tables[0].Rows[i]["attempts"]).Trim();
                                    string cursem = Convert.ToString(ds.Tables[0].Rows[i]["current_semester"]).Trim();
                                    string status = Convert.ToString(ds.Tables[0].Rows[i]["sts"]).Trim();
                                    minintmark = Convert.ToString(ds.Tables[0].Rows[i]["min_int_marks"]).Trim();
                                    maxintmark = Convert.ToString(ds.Tables[0].Rows[i]["max_int_marks"]).Trim();
                                    minextmark = Convert.ToString(ds.Tables[0].Rows[i]["min_ext_marks"]).Trim();
                                    maxextmark = Convert.ToString(ds.Tables[0].Rows[i]["max_ext_marks"]).Trim();
                                    mintotmark = Convert.ToString(ds.Tables[0].Rows[i]["mintotal"]).Trim();
                                    maxtotmark = Convert.ToString(ds.Tables[0].Rows[i]["maxtotal"]).Trim();
                                    string degreecode = Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]).Trim();
                                    string crdeitpoints = Convert.ToString(ds.Tables[0].Rows[i]["credit_points"]).Trim();
                                    string submaoremark = Convert.ToString(ds.Tables[0].Rows[i]["Moderation_Mark"]).Trim();
                                    //string subjectMarkDiff = Convert.ToString(ds.Tables[0].Rows[i]["Moderation_Mark"]).Trim();
                                    string minintmodeallow = Convert.ToString(ds.Tables[0].Rows[i]["min_int_moderation"]).Trim();
                                    string dlflag = Convert.ToString(ds.Tables[0].Rows[i]["delflag"]).Trim();
                                    string subjectMarkDifference = string.Empty;
                                    double subjectMarkDifferent = 0;
                                    subjectMarkDifference = Convert.ToString(ds.Tables[0].Rows[i]["ThirdValuationMarkDifference"]).Trim();
                                    double.TryParse(Convert.ToString(subjectMarkDifference).Trim(), out subjectMarkDifferent);
                                    subjectMarkDifferent = Math.Round(subjectMarkDifferent, markround, MidpointRounding.AwayFromZero);

                                    bool setflag = false;
                                    if (string.IsNullOrEmpty(attempts) || attempts.Trim() == "0")
                                    {
                                        attempts = "1";
                                    }
                                    if (ds2.Tables.Count > 0 && ds2.Tables[2].Rows.Count > 0)
                                    {
                                        exandate = Convert.ToString(ds2.Tables[2].Rows[0]["exam_date"]).Trim();
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
                                        //if (ds.Tables[1].Rows.Count > 0)
                                        //{
                                        //    ds.Tables[1].DefaultView.RowFilter = "roll_no='" + regno + "'";
                                        //    dnew = ds.Tables[1].DefaultView;
                                        //    if (dnew.Count > 0)
                                        //    {
                                        //        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dnew[0]["dummy_no"]);
                                        //        fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Dummy No";
                                        //    }
                                        //}

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
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Tag = examcode;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].CellType = rgex;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Tag = subno;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Note = attempts;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].CellType = rgex;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Tag = exandate;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Note = cursem;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Tag = subjectMarkDifference.Trim();
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].CellType = rgex;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Note = degreecode;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Tag = crdeitpoints;

                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Note = minintmark;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Tag = maxintmark;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].CellType = rgexInternal;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Note = minextmark;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Tag = maxextmark;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 8].Note = mintotmark;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 8].Tag = maxtotmark;

                                        #region Final Year Moderation

                                        educationLevel = string.Empty;
                                        DataView dvDegree = new DataView();
                                        if (dsDegreeDetails.Tables.Count > 0 && dsDegreeDetails.Tables[0].Rows.Count > 0)
                                        {
                                            dsDegreeDetails.Tables[0].DefaultView.RowFilter = "degree_code='" + degreecode + "'";
                                            dvDegree = dsDegreeDetails.Tables[0].DefaultView;
                                        }
                                        if (dvDegree.Count > 0)
                                        {
                                            educationLevel = Convert.ToString(dvDegree[0]["edu_level"]).Trim();
                                        }

                                        DataTable dtFinModerBatch = new DataTable();
                                        DataTable dtFinModerSem = new DataTable();
                                        DataTable dtFinModerMarks = new DataTable();
                                        if (isFinalYearModerationApplicable)
                                        {
                                            if (dsCommonFinModerationBatch.Tables.Count > 0 && dsCommonFinModerationBatch.Tables[0].Rows.Count > 0)
                                            {
                                                dsCommonFinModerationBatch.Tables[0].DefaultView.RowFilter = "LinkValue='" + batchyerr + "' and LinkName='MaximumModerationAppBatch@" + educationLevel + "'";
                                                dtFinModerBatch = dsCommonFinModerationBatch.Tables[0].DefaultView.ToTable();
                                            }
                                            if (dsCommonFinModerationSem.Tables.Count > 0 && dsCommonFinModerationSem.Tables[0].Rows.Count > 0)
                                            {
                                                dsCommonFinModerationSem.Tables[0].DefaultView.RowFilter = "LinkValue='" + cursem + "' and LinkName='MaximumModerationAppSem@" + educationLevel + "'";
                                                dtFinModerSem = dsCommonFinModerationSem.Tables[0].DefaultView.ToTable();
                                            }
                                            if (dscommonFinalYearModerationMark.Tables.Count > 0 && dsCommonFinModerationSem.Tables[0].Rows.Count > 0)
                                            {
                                                dscommonFinalYearModerationMark.Tables[0].DefaultView.RowFilter = "LinkName='MaximumModerationAppMark@" + educationLevel + "'";
                                                dtFinModerMarks = dscommonFinalYearModerationMark.Tables[0].DefaultView.ToTable();
                                            }
                                            if (dtFinModerBatch.Rows.Count > 0 && dtFinModerSem.Rows.Count > 0 && dtFinModerMarks.Rows.Count > 0)
                                            {
                                                commonFinalYearModerationMark = Convert.ToString(dtFinModerMarks.Rows[0]["LinkValue"]).Trim();
                                                double.TryParse(commonFinalYearModerationMark, out commonFinalYrModeration);

                                                if (max_ext_marks1 < 60)
                                                {
                                                    commonFinalYrModeration = Math.Round(commonFinalYrModeration / 2, 0, MidpointRounding.AwayFromZero);
                                                }
                                                if (commonFinalYearModerationMark.Trim() != "" && commonFinalYearModerationMark.Trim() != "0" && commonFinalYrModeration > 0)
                                                {
                                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 9].Note = commonFinalYrModeration.ToString();
                                                }
                                                else if (submaoremark.Trim() != "" && submaoremark.Trim() != "0")
                                                {
                                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 9].Note = submaoremark;
                                                }
                                                else
                                                {
                                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 9].Note = Mark_moderation1.ToString();
                                                }
                                            }
                                            else if (submaoremark.Trim() != "" && submaoremark.Trim() != "0")
                                            {
                                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 9].Note = submaoremark;
                                            }
                                            else
                                            {
                                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 9].Note = Mark_moderation1.ToString();
                                            }
                                        }
                                        else
                                        {
                                            if (submaoremark.Trim() != "" && submaoremark.Trim() != "0")
                                            {
                                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 9].Note = submaoremark;
                                            }
                                            else
                                            {
                                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 9].Note = Mark_moderation1.ToString();
                                            }
                                        }

                                        #endregion  Final Year Moderation

                                        if (minintmodeallow.Trim() != "" && minintmodeallow.Trim() != "0")
                                        {
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 9].Tag = minintmodeallow.ToString();
                                        }
                                        else
                                        {
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 9].Tag = minicamoderation.ToString();
                                        }

                                        if (!string.IsNullOrEmpty(subjectMarkDifference.Trim()) && subjectMarkDifference.Trim() != "0")
                                        {
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Tag = subjectMarkDifference.Trim();
                                        }
                                        else
                                        {
                                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Tag = Mark_Difference1.ToString().Trim();
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
                        fpspread.Height = height + 200;
                        fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;

                        string strinternammark = "select m.roll_no,r.Reg_No,m.internal_mark,m.exam_code,ED.Exam_year,ED.Exam_Month,(ed.Exam_year*12+ed.Exam_Month) EXAMYEARMONTHVAL from mark_entry m,subject s,Registration r,Exam_Details ed where m.roll_no=r.Roll_No and m.subject_no=s.subject_no and ed.exam_code=m.exam_code and (ed.Exam_year*12+ed.Exam_Month)<=('" + ddlYear1.SelectedValue.ToString() + "'*12+'" + ddlMonth1.SelectedValue.ToString() + "') AND s.subject_code='" + subjectCodeNew + "' " + degreevalregis + " and m.internal_mark is not null order by m.roll_no,EXAMYEARMONTHVAL DESC,ed.Exam_year,ED.Exam_Month desc";
                        DataSet dsinternal = da.select_method_wo_parameter(strinternammark, "Text");
                        double evalmaxmark = 0;// Convert.ToDouble(fpspread.Sheets[0].ColumnHeader.Cells[2, 2].Tag);
                        double.TryParse(Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[2, 2].Tag).Trim(), out evalmaxmark);
                        for (int row_cnt = 0; row_cnt < fpspread.Sheets[0].RowCount; row_cnt++)
                        {
                            int attempts = 1;
                            string roll_noo = fpspread.Sheets[0].Cells[row_cnt, 1].Note.ToString();
                            string roll_noo2 = fpspread.Sheets[0].Cells[row_cnt, 1].Text.ToString();
                            string batchyr = fpspread.Sheets[0].Cells[row_cnt, 1].Tag.ToString();
                            string subjectNoNew = Convert.ToString(fpspread.Sheets[0].Cells[row_cnt, 2].Tag).Trim();
                            string previousinternalmark = string.Empty;
                            string strus = fpspread.Sheets[0].Cells[row_cnt, 0].Note.ToString();
                            double markmodeart = 0;
                            string modemarkval = fpspread.Sheets[0].Cells[row_cnt, 9].Note.ToString();
                            if (modemarkval.Trim() != "" && modemarkval.Trim() != "0")
                            {
                                markmodeart = 0;// Convert.ToDouble(modemarkval);
                                double.TryParse(modemarkval.Trim(), out markmodeart);
                            }
                            else
                            {
                                markmodeart = Mark_moderation1;
                            }
                           
                            double markdifference = 0;
                            string markdiff = Convert.ToString(fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Tag).Trim();
                            if (!string.IsNullOrEmpty(markdiff) && markdiff.Trim() != "" && markdiff.Trim() != "0")
                            {
                                markdifference = 0;
                                double.TryParse(markdiff.Trim(), out markdifference);
                            }
                            else
                            {
                                markdifference = Mark_Difference1;
                            }

                            bool isCaLock = false;
                            double minintmodeallmar = 0;
                            string minintmoderationallow = fpspread.Sheets[0].Cells[row_cnt, 9].Tag.ToString();
                            if (minintmoderationallow.Trim() != "" && minintmoderationallow.Trim() != "0")
                            {
                                minintmodeallmar = 0;// Convert.ToDouble(minintmoderationallow);
                                double.TryParse(minintmoderationallow.Trim(), out minintmodeallmar);
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
                                ds2.Tables[0].DefaultView.RowFilter = "roll_no='" + roll_noo + "' and exam_code ='" + batchyr + "'"; // Modify 04-02-2017 jai
                                dv1 = ds2.Tables[0].DefaultView;
                                DataView dvintmark = new DataView();
                                if (chkicaretake.Checked == true || chkmarkbased.Checked == true)
                                {
                                    ds.Tables[0].DefaultView.RowFilter = "roll_no='" + roll_noo + "'";
                                    dvintmark = ds.Tables[0].DefaultView;
                                }
                                else
                                {
                                    if (dsinternal.Tables.Count > 0 && dsinternal.Tables[0].Rows.Count > 0)
                                    {
                                        dsinternal.Tables[0].DefaultView.RowFilter = "roll_no='" + roll_noo + "'";
                                        dvintmark = dsinternal.Tables[0].DefaultView;
                                    }
                                }
                                int monthval = (Convert.ToInt32(ddlYear1.SelectedValue.ToString()) * 12) + Convert.ToInt32(ddlMonth1.SelectedValue.ToString());
                                DataView dvattempts = new DataView();
                                if (dsinternal.Tables.Count > 0 && dsinternal.Tables[0].Rows.Count > 0)
                                {
                                    dsinternal.Tables[0].DefaultView.RowFilter = "roll_no='" + roll_noo + "' and EXAMYEARMONTHVAL<" + monthval + "";
                                    dvattempts = dsinternal.Tables[0].DefaultView;
                                }
                                if (dvattempts.Count > 0)
                                {
                                    attempts = dvattempts.Count + 1;
                                }
                                DataView dvNewInternal = new DataView();
                                if (dsNewInternal.Tables.Count > 0 && dsNewInternal.Tables[0].Rows.Count > 0)
                                {
                                    dsNewInternal.Tables[0].DefaultView.RowFilter = "roll_no='" + roll_noo + "' and subject_no='" + subjectNoNew + "'";
                                    dvNewInternal = dsNewInternal.Tables[0].DefaultView;
                                }
                                fpspread.Sheets[0].Cells[row_cnt, 2].Note = attempts.ToString();

                                if (dvNewInternal.Count > 0)
                                {
                                    previousinternalmark = Convert.ToString(dvNewInternal[0]["total"]).Trim();
                                    if (previousinternalmark.Trim() != "" && previousinternalmark != null)
                                    {
                                        isCaLock = true;
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
                                    previousinternalmark = Convert.ToString(dvintmark[0]["internal_mark"]).Trim();
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
                                if (dv1.Count > 0)
                                {
                                    btnreset.Visible = true;
                                    chkmoderation.Visible = true;
                                    ev1 = dv1[0]["evaluation1"].ToString();
                                    ev2 = dv1[0]["evaluation2"].ToString();
                                    subjectcode = ddlSubject.SelectedValue.ToString();
                                    ev3 = dv1[0]["evaluation3"].ToString();
                                    intermarkf = dv1[0]["internal_mark"].ToString();
                                    resullts = dv1[0]["result"].ToString();
                                    //if (chkicaretake.Checked == true || chkmarkbased.Checked == true)
                                    //{
                                    //    ev1 = string.Empty;
                                    //    ev2 = string.Empty;
                                    //    ev3 = string.Empty;
                                    //}

                                    double intermarkf223 = 0;
                                    string intermarkforab = string.Empty;

                                    if (intermarkf.Trim() != "" && intermarkf != null)
                                    {
                                        //intermarkf223 = Convert.ToDouble(intermarkf);
                                        double.TryParse(intermarkf, out intermarkf223);
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
                                                resullts = "NC";
                                            }
                                        }
                                        fpspread.Sheets[0].Cells[row_cnt, 7].Text = intermarkforab;
                                        previousinternalmark = intermarkforab;
                                        if (intermarkforab.Trim().ToLower().Contains('a'))
                                        {
                                            intermarkforab = "-1";
                                        }
                                    }
                                    else
                                    {
                                        //intermarkf223 = Convert.ToDouble(previousinternalmark);
                                        double.TryParse(previousinternalmark, out intermarkf223);
                                        intermarkforab = previousinternalmark;
                                        if (intermarkf223 < 0)
                                        {
                                            intermarkforab = loadmarkat(previousinternalmark);
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
                                                resullts = "NC";
                                            }
                                        }
                                        fpspread.Sheets[0].Cells[row_cnt, 7].Text = previousinternalmark;
                                        intermarkf = previousinternalmark;
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
                                    string total = dv1[0]["total"].ToString();

                                    if (intermarkf == "" && resullts.Trim().ToLower() != "whd")
                                    {
                                        resullts = string.Empty;
                                        total = string.Empty;
                                    }

                                    if (ev2.Trim() != "" && ev2.Trim() != null && ev2.Trim().ToLower() != "m")
                                    {
                                        abse2 = Convert.ToDouble(ev2);
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
                                                            fpspread.Sheets[0].Cells[row_cnt, 2].Text = string.Empty;
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
                                                            fpspread.Sheets[0].Cells[row_cnt, 3].Text = string.Empty;
                                                        }

                                                        string avemark = string.Empty;
                                                        if (ev1 != "" && ev2 != "" && ev2 != "NE" && ev1 != "NE")
                                                        {
                                                            ds2.Tables[1].DefaultView.RowFilter = "roll_no='" + roll_noo + "'";
                                                            dv3 = ds2.Tables[1].DefaultView;
                                                            if (dv3.Count > 0)
                                                            {
                                                                modermarks = Convert.ToDouble(dv3[0]["passmark"].ToString());
                                                                modermarks = Math.Round(modermarks, markround, MidpointRounding.AwayFromZero);
                                                                fpspread.Sheets[0].Cells[row_cnt, 6].Text = modermarks.ToString();
                                                                externn = dv3[0]["af_moderation_extmrk"].ToString();
                                                                external_mark = Convert.ToDouble(externn.ToString());
                                                                external_mark = Math.Round(external_mark, markround, MidpointRounding.AwayFromZero);
                                                                externn = dv1[0]["external_mark"].ToString();
                                                                if (externn.Trim() != "")
                                                                {
                                                                    external_mark = Convert.ToDouble(externn.ToString());
                                                                    external_mark = Math.Round(external_mark, markround, MidpointRounding.AwayFromZero);
                                                                    fpspread.Sheets[0].Cells[row_cnt, 4].Text = string.Empty;

                                                                    fpspread.Sheets[0].Cells[row_cnt, 8].Text = Convert.ToString(Math.Round(external_mark, markround, MidpointRounding.AwayFromZero));
                                                                }
                                                            }
                                                            else
                                                            {
                                                                externn = dv1[0]["external_mark"].ToString();
                                                                if (externn.Trim() != "")
                                                                {
                                                                    external_mark = Convert.ToDouble(externn.ToString());
                                                                    external_mark = Math.Round(external_mark, markround, MidpointRounding.AwayFromZero);
                                                                    fpspread.Sheets[0].Cells[row_cnt, 4].Text = external_mark.ToString();
                                                                    fpspread.Sheets[0].Cells[row_cnt, 8].Text = Convert.ToString(Math.Round(external_mark, markround, MidpointRounding.AwayFromZero));
                                                                }
                                                            }
                                                            if (externn.Trim() != "")
                                                            {
                                                                fpspread.Sheets[0].Cells[row_cnt, 9].Text = total;
                                                                fpspread.Sheets[0].Cells[row_cnt, 10].Text = resullts;
                                                            }

                                                            ev11 = Convert.ToDouble(ev1);
                                                            ev21 = Convert.ToDouble(ev2);
                                                            difff1 = ev11 - ev21;
                                                            difff2 = ev21 - ev11;
                                                            double thirfddiff = 0;
                                                            if (ev11 > ev21)
                                                            {
                                                                thirfddiff = ev11 - ev21;
                                                            }
                                                            else
                                                            {
                                                                thirfddiff = ev21 - ev11;
                                                            }

                                                            if (thirfddiff >= markdifference)
                                                            {
                                                                fpspread.Sheets[0].Cells[row_cnt, 5].BackColor = Color.SkyBlue;
                                                                fpspread.Sheets[0].Cells[row_cnt, 5].Locked = false;
                                                                externn = string.Empty;
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

                                                                        ev31 = Convert.ToDouble(ev3);
                                                                        double devdif1 = 0;
                                                                        double devdif2 = 0;
                                                                        double finev3 = 0;

                                                                        double[] evaluationAll = new double[3];
                                                                        evaluationAll[0] = ev11;
                                                                        evaluationAll[1] = ev21;
                                                                        evaluationAll[2] = ev31;
                                                                        Array.Sort(evaluationAll);

                                                                        double difference1 = ev11 - ev21;
                                                                        double difference2 = ev21 - ev31;
                                                                        double difference3 = ev31 - ev11;
                                                                        difference1 = Math.Abs(difference1);
                                                                        difference2 = Math.Abs(difference2);
                                                                        difference3 = Math.Abs(difference3);

                                                                        if (difference1 < difference2 && difference1 < difference3)
                                                                        {
                                                                            finev3 = (ev11 + ev21) / 2;
                                                                        }
                                                                        else if (difference2 < difference1 && difference2 < difference3)
                                                                        {
                                                                            finev3 = (ev21 + ev31) / 2;
                                                                        }
                                                                        else if (difference3 < difference1 && difference3 < difference2)
                                                                        {
                                                                            finev3 = (ev31 + ev11) / 2;
                                                                        }

                                                                        if (difference1 == difference2)
                                                                        {
                                                                            finev3 = (evaluationAll[1] + evaluationAll[2]) / 2;
                                                                            if (difference3 < difference1)
                                                                            {
                                                                                finev3 = (ev31 + ev11) / 2;
                                                                            }
                                                                        }
                                                                        else if (difference2 == difference3)
                                                                        {
                                                                            finev3 = (evaluationAll[1] + evaluationAll[2]) / 2;
                                                                            if (difference1 < difference2)
                                                                            {
                                                                                finev3 = (ev11 + ev21) / 2;
                                                                            }
                                                                        }
                                                                        else if (difference1 == difference3)
                                                                        {
                                                                            finev3 = (evaluationAll[1] + evaluationAll[2]) / 2;
                                                                            if (difference2 < difference1)
                                                                            {
                                                                                finev3 = (ev21 + ev31) / 2;
                                                                            }
                                                                        }

                                                                        //if (ev11 > ev31)
                                                                        //{
                                                                        //    devdif1 = ev11 - ev31;
                                                                        //}
                                                                        //else
                                                                        //{
                                                                        //    devdif1 = ev31 - ev11;
                                                                        //}
                                                                        //if (ev21 > ev31)
                                                                        //{
                                                                        //    devdif2 = ev21 - ev31;
                                                                        //}
                                                                        //else
                                                                        //{
                                                                        //    devdif2 = ev31 - ev21;
                                                                        //}
                                                                        //if (devdif1 > devdif2)
                                                                        //{
                                                                        //    finev3 = (Convert.ToDouble(ev21) + Convert.ToDouble(ev31)) / 2;
                                                                        //}
                                                                        //else if (devdif1 == devdif2)
                                                                        //{
                                                                        //    if (ev11 > ev21)
                                                                        //    {
                                                                        //        finev3 = (Convert.ToDouble(ev11) + Convert.ToDouble(ev31)) / 2;
                                                                        //    }
                                                                        //    else
                                                                        //    {
                                                                        //        finev3 = (Convert.ToDouble(ev21) + Convert.ToDouble(ev31)) / 2;
                                                                        //    }
                                                                        //}
                                                                        //else
                                                                        //{
                                                                        //    finev3 = (Convert.ToDouble(ev11) + Convert.ToDouble(ev31)) / 2;
                                                                        //}

                                                                        #region  Added By Malang Raja For Jamal College Condition On Jan 12 2017

                                                                        //double diffEval1And2 = 0;
                                                                        //double diffEval2AndEval3 = 0; //43 30 17


                                                                        //diffEval1And2 = ev11 - ev21;
                                                                        //diffEval2AndEval3 = ev21 - ev31;
                                                                        //diffEval1And2 = Math.Abs(diffEval1And2);
                                                                        //diffEval2AndEval3 = Math.Abs(diffEval2AndEval3);
                                                                        //if (diffEval1And2 == diffEval2AndEval3 && diffEval1And2 > 0 && diffEval2AndEval3 > 0)
                                                                        //{
                                                                        //    double m1 = 0;
                                                                        //    double m2 = 0;
                                                                        //    m2 = ev11 - ev31;
                                                                        //    m2 = Math.Abs(m2);
                                                                        //    m1 = (evaluationAll[1] + evaluationAll[2]) / 2;
                                                                        //    if (m2 < diffEval2AndEval3)
                                                                        //        m1 = (ev11 + ev31) / 2;
                                                                        //    finev3 = m1;
                                                                        //    //m1 = ((ev11 + ev21) / 2);
                                                                        //    //m2 = ((ev21 + ev31) / 2);
                                                                        //    ////m1 = Math.Abs(m1);
                                                                        //    ////m2 = Math.Abs(m2);
                                                                        //    //if (m1 == m2)
                                                                        //    //{
                                                                        //    //    finev3 = m1;
                                                                        //    //}
                                                                        //    //else if (m1 < m2)
                                                                        //    //{
                                                                        //    //    finev3 = m2;
                                                                        //    //}
                                                                        //    //else if (m1 > m2)
                                                                        //    //{
                                                                        //    //    finev3 = m1;
                                                                        //    //}
                                                                        //}

                                                                        #endregion Added By Malang Raja For Jamal College Condition On Jan 12 2017

                                                                        finev3 = Math.Round(finev3, markround, MidpointRounding.AwayFromZero);
                                                                        avemark = finev3.ToString();
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    fpspread.Sheets[0].Cells[row_cnt, 5].Text = string.Empty;
                                                                    fpspread.Sheets[0].Cells[row_cnt, 6].Text = string.Empty;
                                                                    double bindav = (Convert.ToDouble(ev11) + Convert.ToDouble(ev21)) / 2;
                                                                    bindav = Math.Round(bindav, markround, MidpointRounding.AwayFromZero);
                                                                    avemark = bindav.ToString();
                                                                }
                                                                if (total.Trim() != "" && intermarkforab.Trim() != "")
                                                                {
                                                                    double getval = Convert.ToDouble(total);
                                                                    if (minintmodeallmar <= Convert.ToDouble(intermarkforab))
                                                                    {
                                                                        if (getval < mintolmarks1)
                                                                        {
                                                                            double markfaikl = mintolmarks1 - getval;
                                                                            markfaikl = markfaikl / max_ext_marks1 * papermaxexter;
                                                                            markfaikl = Math.Round(markfaikl, markround, MidpointRounding.AwayFromZero);
                                                                            double extmarkfail = 0;
                                                                            if (Convert.ToDouble(external_mark) < min_ext_marks1 && intermarkf223 >= min_int_marks1)
                                                                            {
                                                                                extmarkfail = min_ext_marks1 - Convert.ToDouble(external_mark);
                                                                                extmarkfail = extmarkfail / max_ext_marks1 * papermaxexter;
                                                                                if (extmarkfail < 0)
                                                                                {
                                                                                    extmarkfail = 0;
                                                                                }
                                                                            }

                                                                            //Uncommand by Rajkumar on 5-6-2018
                                                                            if (markfaikl <= markmodeart && extmarkfail <= markmodeart)//Rajkumar
                                                                            {
                                                                                if (markfaikl > extmarkfail)
                                                                                {
                                                                                    fpspread.Sheets[0].Cells[row_cnt, 6].Text = Convert.ToString(Math.Round(markfaikl)); // added 02-2016
                                                                                }
                                                                                else
                                                                                {
                                                                                    fpspread.Sheets[0].Cells[row_cnt, 6].Text = Convert.ToString(Math.Round(markfaikl));
                                                                                }
                                                                            }
                                                                        }
                                                                        else if (min_ext_marks1 > Convert.ToDouble(external_mark))//Rajkumar
                                                                        {
                                                                            double markfaikl = min_ext_marks1 - Convert.ToDouble(external_mark);
                                                                            markfaikl = markfaikl / max_ext_marks1 * papermaxexter;
                                                                            if (markfaikl <= markmodeart)
                                                                            {
                                                                                fpspread.Sheets[0].Cells[row_cnt, 6].Text = Convert.ToString(Math.Round(markfaikl));
                                                                            }
                                                                        }
                                                                        //-------------------------------------------
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                fpspread.Sheets[0].Cells[row_cnt, 5].Text = string.Empty;
                                                                double bindav = (ev11 + ev21) / 2;
                                                                bindav = Math.Round(bindav, markround, MidpointRounding.AwayFromZero);
                                                                avemark = bindav.ToString();

                                                                if (total.Trim() != "" && intermarkforab.Trim() != "")
                                                                {
                                                                    double getval = Convert.ToDouble(total);
                                                                    if (minintmodeallmar <= Convert.ToDouble(intermarkforab))
                                                                    {
                                                                        if (getval < mintolmarks1)
                                                                        {
                                                                            double markfaikl = mintolmarks1 - getval;
                                                                            markfaikl = markfaikl / max_ext_marks1 * papermaxexter;
                                                                            double extmarkfail = 0;
                                                                            if (Convert.ToDouble(external_mark) < min_ext_marks1 && intermarkf223 >= min_int_marks1)
                                                                            {
                                                                                extmarkfail = min_ext_marks1 - Convert.ToDouble(external_mark);
                                                                                extmarkfail = extmarkfail / max_ext_marks1 * papermaxexter;
                                                                                if (extmarkfail < 0)
                                                                                {
                                                                                    extmarkfail = 0;
                                                                                }
                                                                            }
                                                                            if (markfaikl <= markmodeart && extmarkfail <= markmodeart)//Rajkumar
                                                                            {
                                                                                if (markfaikl > extmarkfail)
                                                                                {
                                                                                    fpspread.Sheets[0].Cells[row_cnt, 6].Text = Convert.ToString(Math.Round(markfaikl));
                                                                                }
                                                                                else
                                                                                {
                                                                                    fpspread.Sheets[0].Cells[row_cnt, 6].Text = Convert.ToString(Math.Round(markfaikl));
                                                                                }
                                                                            }
                                                                        }
                                                                        else if (min_ext_marks1 > Convert.ToDouble(external_mark))//Rajkumar
                                                                        {
                                                                            double markfaikl = min_ext_marks1 - Convert.ToDouble(external_mark);
                                                                            markfaikl = markfaikl / max_ext_marks1 * papermaxexter;
                                                                            if (markfaikl <= markmodeart)
                                                                            {
                                                                                fpspread.Sheets[0].Cells[row_cnt, 6].Text = Convert.ToString(Math.Round(markfaikl));
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
                                                            if (chkicaretake.Checked == true || chkmarkbased.Checked == true)
                                                            {
                                                                ds2.Tables[0].DefaultView.RowFilter = "roll_no='" + roll_noo + "'";
                                                                dv3 = ds2.Tables[0].DefaultView;
                                                                if (dv3.Count > 0)
                                                                {
                                                                    resullts = dv3[0]["result"].ToString();
                                                                    ds.Tables[0].DefaultView.RowFilter = "roll_no='" + roll_noo + "'";
                                                                    DataView dvicra = ds.Tables[0].DefaultView;
                                                                    if (dvicra.Count > 0)
                                                                    {
                                                                        externn = dvicra[0]["EXE"].ToString();
                                                                    }
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
                                                                            externn = string.Empty;
                                                                            resullts = string.Empty;
                                                                        }
                                                                        else
                                                                        {
                                                                            double getexmark = Convert.ToDouble(externn);
                                                                            getexmark = Math.Round(getexmark, markround, MidpointRounding.AwayFromZero);
                                                                            externn = Convert.ToString(externn);
                                                                        }
                                                                        fpspread.Sheets[0].Cells[row_cnt, 8].Text = externn;
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
                                                        fpspread.Sheets[0].Cells[row_cnt, 5].Text = string.Empty;
                                                        fpspread.Sheets[0].Cells[row_cnt, 6].Text = string.Empty;
                                                        fpspread.Sheets[0].Cells[row_cnt, 4].Text = "A";
                                                        fpspread.Sheets[0].Cells[row_cnt, 5].BackColor = Color.White;
                                                        fpspread.Sheets[0].Cells[row_cnt, 7].Text = previousinternalmark;
                                                        fpspread.Sheets[0].Cells[row_cnt, 8].Text = "A";
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
                                                    fpspread.Sheets[0].Cells[row_cnt, 2].Text = "NE";
                                                    fpspread.Sheets[0].Cells[row_cnt, 3].Text = "NE";
                                                    fpspread.Sheets[0].Cells[row_cnt, 4].Text = "NE";
                                                    fpspread.Sheets[0].Cells[row_cnt, 5].Text = string.Empty;
                                                    fpspread.Sheets[0].Cells[row_cnt, 6].Text = string.Empty;
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
                                                fpspread.Sheets[0].Cells[row_cnt, 5].Text = string.Empty;
                                                fpspread.Sheets[0].Cells[row_cnt, 6].Text = string.Empty;
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
                                            fpspread.Sheets[0].Cells[row_cnt, 5].Text = string.Empty;
                                            fpspread.Sheets[0].Cells[row_cnt, 6].Text = string.Empty;
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
                                        fpspread.Sheets[0].Cells[row_cnt, 5].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 6].Text = string.Empty;
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
                                        fpspread.Sheets[0].Cells[row_cnt, 2].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 3].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 4].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 5].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 6].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 8].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 9].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 10].Text = string.Empty;

                                        ds.Tables[0].DefaultView.RowFilter = "roll_no='" + roll_noo + "'";
                                        DataView dvicaretake = ds.Tables[0].DefaultView;
                                        if (dvicaretake.Count > 0)
                                        {
                                            string esemark = dvicaretake[0]["EXE"].ToString();
                                            previousinternalmark = dvicaretake[0]["internal_mark"].ToString();
                                            if (previousinternalmark == "-1")
                                            {
                                                previousinternalmark = "A";
                                            }

                                            //double getvalmark = Convert.ToDouble(esemark) / max_ext_marks1 * evalmaxmark;
                                            double getvalmark = 0;// Convert.ToDouble(esemark) / max_ext_marks1 * evalmaxmark;
                                            if (esemark == "")
                                            {
                                                esemark = string.Empty;
                                                resullts = string.Empty;
                                            }
                                            else
                                            {
                                                double getexmark = Convert.ToDouble(externn);
                                                getexmark = Math.Round(getexmark, markround, MidpointRounding.AwayFromZero);
                                                externn = Convert.ToString(externn);
                                            }
                                            double externalMarksNew = 0;
                                            double.TryParse(esemark, out externalMarksNew);
                                            getvalmark = Convert.ToDouble(externalMarksNew) / max_ext_marks1 * evalmaxmark;
                                            getvalmark = Math.Round(getvalmark, markround, MidpointRounding.AwayFromZero);
                                            //fpspread.Sheets[0].Cells[row_cnt, 2].Text = getvalmark.ToString();
                                            //fpspread.Sheets[0].Cells[row_cnt, 3].Text = getvalmark.ToString();
                                            //fpspread.Sheets[0].Cells[row_cnt, 4].Text = getvalmark.ToString();
                                            fpspread.Sheets[0].Cells[row_cnt, 7].Text = previousinternalmark.ToString();
                                            if (esemark.Trim() == "-1")
                                            {
                                                esemark = "A";
                                            }
                                            fpspread.Sheets[0].Cells[row_cnt, 8].Text = esemark.ToString();
                                        }
                                    }
                                    else if (strus == "NE")
                                    {
                                        fpspread.Sheets[0].Cells[row_cnt, 2].Text = "NE";
                                        fpspread.Sheets[0].Cells[row_cnt, 3].Text = "NE";
                                        fpspread.Sheets[0].Cells[row_cnt, 4].Text = "NE";
                                        fpspread.Sheets[0].Cells[row_cnt, 5].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 6].Text = string.Empty;
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
                                        fpspread.Sheets[0].Cells[row_cnt, 5].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 6].Text = string.Empty;
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
                                        fpspread.Sheets[0].Cells[row_cnt, 5].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 6].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 5].BackColor = Color.White;
                                        fpspread.Sheets[0].Cells[row_cnt, 7].Text = previousinternalmark;
                                        fpspread.Sheets[0].Cells[row_cnt, 8].Text = "LT";
                                        fpspread.Sheets[0].Cells[row_cnt, 9].Text = "NC";
                                        fpspread.Sheets[0].Cells[row_cnt, 10].Text = "NC";
                                        fpspread.Sheets[0].Cells[row_cnt, 10].Note = passorfail.ToString();
                                    }
                                    else
                                    {
                                        fpspread.Sheets[0].Cells[row_cnt, 2].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 3].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 5].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 5].BackColor = Color.White;
                                        fpspread.Sheets[0].Cells[row_cnt, 7].Text = previousinternalmark;
                                        fpspread.Sheets[0].Cells[row_cnt, 4].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 8].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 9].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 10].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 10].Note = passorfail.ToString();
                                    }
                                }
                                if (dicContoNotPaid.ContainsKey(roll_noo))
                                {
                                    if (previousinternalmark == "-1")
                                    {
                                        previousinternalmark = "AB";
                                    }
                                    string GetText = Convert.ToString(fpspread.Sheets[0].Cells[row_cnt, 8].Text);
                                    if (GetText.Trim() == "NE" || GetText.Trim() == "-3" || GetText.Trim() == "")
                                    {
                                        fpspread.Sheets[0].Cells[row_cnt, 2].Text = "NE";
                                        fpspread.Sheets[0].Cells[row_cnt, 3].Text = "NE";
                                        fpspread.Sheets[0].Cells[row_cnt, 4].Text = "NE";
                                        fpspread.Sheets[0].Cells[row_cnt, 5].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 6].Text = string.Empty;
                                        fpspread.Sheets[0].Cells[row_cnt, 5].BackColor = Color.White;
                                        fpspread.Sheets[0].Cells[row_cnt, 7].Text = previousinternalmark;
                                        fpspread.Sheets[0].Cells[row_cnt, 8].Text = "NE";
                                        fpspread.Sheets[0].Cells[row_cnt, 9].Text = "NC";
                                        fpspread.Sheets[0].Cells[row_cnt, 10].Text = "NC";
                                        fpspread.Sheets[0].Cells[row_cnt, 10].Note = passorfail.ToString();
                                    }
                                    //fpspread.Sheets[0].Cells[row_cnt, 2].Locked = true;
                                    //fpspread.Sheets[0].Cells[row_cnt, 3].Locked = true;
                                    //fpspread.Sheets[0].Cells[row_cnt, 4].Locked = true;
                                    //fpspread.Sheets[0].Cells[row_cnt, 5].Locked = true;
                                    //fpspread.Sheets[0].Cells[row_cnt, 6].Locked = true;
                                    //fpspread.Sheets[0].Cells[row_cnt, 7].Locked = true;
                                    //fpspread.Sheets[0].Cells[row_cnt, 8].Locked = true;
                                    //fpspread.Sheets[0].Cells[row_cnt, 9].Locked = true;
                                    //fpspread.Sheets[0].Cells[row_cnt, 10].Locked = true;
                                }
                                if (dicNotEligible.ContainsKey(roll_noo))
                                {
                                    if (previousinternalmark == "-1")
                                    {
                                        previousinternalmark = "AB";
                                    }
                                    fpspread.Sheets[0].Cells[row_cnt, 2].Text = "NE";
                                    fpspread.Sheets[0].Cells[row_cnt, 3].Text = "NE";
                                    fpspread.Sheets[0].Cells[row_cnt, 4].Text = "NE";
                                    fpspread.Sheets[0].Cells[row_cnt, 5].Text = string.Empty;
                                    fpspread.Sheets[0].Cells[row_cnt, 6].Text = string.Empty;
                                    fpspread.Sheets[0].Cells[row_cnt, 5].BackColor = Color.White;
                                    fpspread.Sheets[0].Cells[row_cnt, 7].Text = previousinternalmark;
                                    fpspread.Sheets[0].Cells[row_cnt, 8].Text = "NE";
                                    fpspread.Sheets[0].Cells[row_cnt, 9].Text = "NC";
                                    fpspread.Sheets[0].Cells[row_cnt, 10].Text = "NC";
                                    fpspread.Sheets[0].Cells[row_cnt, 10].Note = passorfail.ToString();
                                    fpspread.Sheets[0].Cells[row_cnt, 2].Locked = true;
                                    fpspread.Sheets[0].Cells[row_cnt, 3].Locked = true;
                                    fpspread.Sheets[0].Cells[row_cnt, 4].Locked = true;
                                    fpspread.Sheets[0].Cells[row_cnt, 5].Locked = true;
                                    fpspread.Sheets[0].Cells[row_cnt, 6].Locked = true;
                                    fpspread.Sheets[0].Cells[row_cnt, 7].Locked = true;
                                    fpspread.Sheets[0].Cells[row_cnt, 8].Locked = true;
                                    fpspread.Sheets[0].Cells[row_cnt, 9].Locked = true;
                                    fpspread.Sheets[0].Cells[row_cnt, 10].Locked = true;
                                }
                                fpspread.Sheets[0].Cells[row_cnt, 7].Locked = isCaLock;
                            }
                            if (chkincluevel2.Checked == true)
                            {
                                fpspread.Sheets[0].Cells[row_cnt, 3].Text = string.Empty;
                                fpspread.Sheets[0].Cells[row_cnt, 5].Text = string.Empty;
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
            fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
            int hei = 300;
            for (int col = 0; col < fpspread.Sheets[0].RowCount; col++)
            {
                hei = hei + fpspread.Sheets[0].Rows[col].Height;
            }
            fpspread.Height = hei;
            fpspread.SaveChanges();

            string group_code = Session["group_code"].ToString();
            string columnfield = string.Empty;
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
            if (!chkincluevel2.Checked)
            {
                for (int i = 0; i < fpspread.Sheets[0].Rows.Count; i++)
                {
                    if (fpspread.Sheets[0].Cells[i, 5].BackColor == Color.SkyBlue)
                    {
                        if (fpspread.Sheets[0].Cells[i, 5].Text.ToString().Trim() == "")
                        {
                            fpspread.Sheets[0].Cells[i, 4].Text = string.Empty;
                            fpspread.Sheets[0].Cells[i, 6].Text = string.Empty;
                            fpspread.Sheets[0].Cells[i, 8].Text = string.Empty;
                            fpspread.Sheets[0].Cells[i, 9].Text = string.Empty;
                            fpspread.Sheets[0].Cells[i, 10].Text = string.Empty;
                        }
                    }
                }
            }

            if (chkonlyica.Checked == true)
            {
                for (int i = 0; i < fpspread.Sheets[0].Rows.Count; i++)
                {
                    fpspread.Sheets[0].Cells[i, 2].Text = string.Empty;
                    fpspread.Sheets[0].Cells[i, 3].Text = string.Empty;
                    fpspread.Sheets[0].Cells[i, 4].Text = string.Empty;
                    fpspread.Sheets[0].Cells[i, 8].Text = string.Empty;
                }
            }

            if (maxextmark == "0" || maxintmark == "0")
            {
                for (int i = 0; i < fpspread.Sheets[0].Rows.Count; i++)
                {
                    if (maxextmark == "0" && minextmark == "0")
                    {
                        fpspread.Sheets[0].Cells[i, 2].Text = string.Empty;
                        fpspread.Sheets[0].Cells[i, 3].Text = string.Empty;
                        fpspread.Sheets[0].Cells[i, 4].Text = string.Empty;
                        //fpspread.Sheets[0].Cells[i, 8].Text =string.Empty;
                    }
                    if (maxintmark == "0" && minintmark == "0")
                    {
                        fpspread.Sheets[0].Cells[i, 7].Text = string.Empty;
                    }
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

            if (chkmarkbased.Checked == true)
            {
                fpspread.Sheets[0].Columns[8].Locked = false;
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
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
            //if (chk_onlycia.Checked == true)
            //{
            //    saveexamapplicationonlycia();
            //    lblerr1.Visible = false;
            //    fpspread.SaveChanges();
            //    btnviewre_Click(sender, e);
            //    fpspread.Sheets[0].Columns[0].Locked = true;
            //    fpspread.Sheets[0].Columns[1].Locked = true;
            //    fpspread.Sheets[0].Columns[2].Locked = true;
            //    fpspread.Sheets[0].Columns[3].Locked = true;
            //    fpspread.Sheets[0].Columns[4].Locked = true;
            //    fpspread.Sheets[0].Columns[5].Locked = true;
            //    fpspread.Sheets[0].Columns[6].Locked = true;
            //    fpspread.Sheets[0].Columns[7].Locked = false;
            //    fpspread.Sheets[0].Columns[8].Locked = true;
            //    fpspread.Sheets[0].Columns[9].Locked = true;
            //    fpspread.Sheets[0].Columns[10].Locked = true;
            //}
            //else
            //{
            //fpspread.SaveChanges();
            //saveexamapplication();
            //lblerr1.Visible = false;
            //fpspread.SaveChanges();
            //btnviewre_Click(sender, e);
            //}
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
                    string notexistsroll = string.Empty;
                    bool importflag = false;
                    for (int i = 1; i < fpmarkimport.Sheets[0].RowCount; i++)
                    {
                        string rollno = fpmarkimport.Sheets[0].Cells[i, 0].Text.ToString().Trim().ToLower();
                        bool rolfalg = false;
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
                int.TryParse(getmarkround.Trim(), out markround);
                //int num = 0;
                //if (int.TryParse(getmarkround, out num))
                //{
                //    markround = Convert.ToInt32(getmarkround);
                //}
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
            if (chksubwise.Checked == false)
            {
                degreeval = " and ed.degree_code='" + ddlbranch1.SelectedValue + "'";
                degreevalsub = " and s.degree_code='" + ddlbranch1.SelectedValue + "'";
                degreevalttab = " and m.degree_code='" + ddlbranch1.SelectedValue + "'";
                degreevalregis = " and r.degree_code='" + ddlbranch1.SelectedValue + "'";
            }

            string[] spmaxsp = fpspread.Sheets[0].ColumnHeader.Cells[2, 8].Text.ToString().Split(':');
            double maxexternal = Convert.ToDouble(spmaxsp[1]);
            spmaxsp = fpspread.Sheets[0].ColumnHeader.Cells[2, 2].Text.ToString().Split(':');
            // double entmaxexternal = Convert.ToDouble(spmaxsp[1]); Added by jairam 02-02-2016
            double entmaxexternal = 0;
            if (spmaxsp.Length > 1)
            {
                //double.TryParse(spmaxsp[1], out entmaxexternal);
                if (spmaxsp[1].Trim() != "")
                {
                    entmaxexternal = Convert.ToDouble(spmaxsp[1]);
                }
            }

            double moderationtot = 0;

            //string Mark_moderation = string.Empty;
            //if (chksubwise.Checked == false)
            //{
            //    Mark_moderation = da.GetFunction("select distinct s.Moderation_Mark from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.ToString() + "' and s.subject_code='" + subjectCodeNew + "'");
            //}
            //if (Mark_moderation.Trim() == "" || Mark_moderation.Trim() == "0")
            //{
            //    Mark_moderation = da.GetFunction("select distinct isnull(value,'') as value from COE_Master_Settings where settings='Mark Moderation'").Trim();
            //}

            //if (Mark_moderation != "")
            //{
            //    moderationtot = 0; // Convert.ToDouble(Mark_moderation);
            //    double.TryParse(Mark_moderation.Trim(), out moderationtot);
            //}
            //else
            //{
            //    moderationtot = 0;
            //}


            string modtotal = da.GetFunction("select distinct isnull(value,'') as value from COE_Master_Settings where settings='Mark Moderation'");
            //double.TryParse(modtotal, out moderationtot);
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

            double evalmaxmark = 0;// Convert.ToDouble(fpspread.Sheets[0].ColumnHeader.Cells[2, 2].Tag);
            double evalmaxmarkNEw = 0;// Convert.ToDouble(fpspread.Sheets[0].ColumnHeader.Cells[2, 8].Tag);
            double markdifffmoderer = 0;// Convert.ToDouble(fpspread.Sheets[0].ColumnHeader.Cells[2, 6].Note.ToString());

            double.TryParse(Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[2, 2].Tag).Trim(), out evalmaxmark);
            double.TryParse(Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[2, 8].Tag).Trim(), out evalmaxmarkNEw);
            double.TryParse(Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[2, 6].Note).Trim(), out markdifffmoderer);

            //if (evalmaxmark > evalmaxmarkNEw)
            //{
            //    evalmaxmark = evalmaxmarkNEw;
            //}


            double minextmarks = 0;
            double manextmarks = 0;
            double minintmarks = 0;
            double mintotalv = 0;
            double maxtotalv = 0;
            double minexternaleva = 0;
            double mintotaleva = 0;
            double maxinternalmarkvalue = 0;
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
                string maderonmark = string.Empty;
                result = fpspread.Sheets[0].Cells[r, 10].Text.ToString();
                string cursem = fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Note.ToString();
                double minicamoderatio = Convert.ToDouble(fpspread.Sheets[0].Cells[r, 9].Tag.ToString());

                string modemarkval = fpspread.Sheets[0].Cells[r, 9].Note.ToString();
                //double maxmarkmoderation = Convert.ToDouble(modemarkval);
                double maxmarkmoderation = 0;
                double.TryParse(modemarkval, out maxmarkmoderation);
         
                minintmarks = 0;// Convert.ToDouble(fpspread.Sheets[0].Cells[r, 6].Note.ToString());
                minextmarks = 0;// Convert.ToDouble(fpspread.Sheets[0].Cells[r, 7].Note.ToString());
                manextmarks = 0;// Convert.ToDouble(fpspread.Sheets[0].Cells[r, 7].Tag.ToString());
                maxtotalv = 0;//Convert.ToDouble(fpspread.Sheets[0].Cells[r, 8].Tag.ToString());
                mintotalv = 0;// Convert.ToDouble(fpspread.Sheets[0].Cells[r, 8].Note.ToString());
                maxinternalmarkvalue = 0;// Convert.ToDouble(fpspread.Sheets[0].Cells[r, 6].Tag.ToString());

                double.TryParse(Convert.ToString(fpspread.Sheets[0].Cells[r, 6].Note).Trim(), out minintmarks);
                double.TryParse(Convert.ToString(fpspread.Sheets[0].Cells[r, 7].Note).Trim(), out minextmarks);
                double.TryParse(Convert.ToString(fpspread.Sheets[0].Cells[r, 7].Tag).Trim(), out manextmarks);
                double.TryParse(Convert.ToString(fpspread.Sheets[0].Cells[r, 8].Tag).Trim(), out maxtotalv);
                double.TryParse(Convert.ToString(fpspread.Sheets[0].Cells[r, 8].Note).Trim(), out mintotalv);
                double.TryParse(Convert.ToString(fpspread.Sheets[0].Cells[r, 6].Tag).Trim(), out maxinternalmarkvalue);

                minexternaleva = (minextmarks / manextmarks) * evalmaxmark;
                minexternaleva = (minexternaleva / evalmaxmark) * manextmarks;
                mintotaleva = mintotalv / maxtotalv * evalmaxmark;

                if (manextmarks == 0 && minextmarks == 0)
                {
                    evauation1 = "0";
                    evauation2 = "0";
                }
                if (maxinternalmarkvalue == 0 && minintmarks == 0)
                {
                    icamark = "0";
                }

                double avg = 0;
                double extemod = 0;
                double moderat = 0;
                double intark = 0;
                if (icamark == "")
                {
                    if (chkonlyica.Checked == true)
                    {
                        evauation1 = string.Empty;
                        evauation2 = string.Empty;
                    }
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
                    else
                    {
                        intark = Convert.ToDouble(icamark);
                    }
                }
                if (chk_onlycia.Checked == false)
                {
                    double ev1 = 0;
                    double ev2 = 0;
                    double ev3 = 0;
                    double totalmarkvalue = 0;
                    double extmark = 0;
                    double moderationmark = 0;

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
                            avg = Math.Round(avg, markround, MidpointRounding.AwayFromZero);
                            intark = Convert.ToDouble(intark);
                            extmark = (avg / entmaxexternal) * maxexternal;
                            extmark = Math.Round(extmark, markround, MidpointRounding.AwayFromZero); // added by jairam 02-02-2016
                            extmark = Math.Round(extmark, markround, MidpointRounding.AwayFromZero);
                            ese = extmark.ToString();
                            totalmarkvalue = extmark + intark;
                            totalmarkvalu = totalmarkvalue.ToString();
                            ev1 = Convert.ToDouble(evauation1);
                            ev2 = Convert.ToDouble(evauation2);

                            double avg1 = 0;
                            avg1 = ev2 - ev1;
                            avg1 = Math.Abs(avg1);

                            if (icamark.Trim() != "")
                            {
                                if (!icamark.ToLower().Contains('a') && !icamark.ToLower().Contains("-1"))
                                {
                                    double getval = Convert.ToDouble(totalmarkvalue);
                                    if ((minicamoderatio <= Convert.ToDouble(intark)) || (getval < mintotalv && minextmarks <= Convert.ToDouble(extmark)))
                                    {
                                        if ((getval < mintotalv || minextmarks > Convert.ToDouble(extmark) || avg < minexternaleva) && intark >= minintmarks)
                                        {
                                            extemod = mintotalv - getval;
                                            moderat = minexternaleva - extmark;
                                            if (moderat <= maxmarkmoderation)
                                            {
                                                if (moderat > 0)
                                                {
                                                    extmark = Math.Round(extmark, markround, MidpointRounding.AwayFromZero);
                                                    double maxrk = moderat + extmark;
                                                    double extcheck = maxrk / maxexternal * maxexternal;
                                                    extcheck = Math.Round(extcheck, markround, MidpointRounding.AwayFromZero);
                                                    double mintotcheck = extcheck + intark;
                                                    double extrenmoder = moderat;
                                                    if (mintotcheck >= mintotalv)
                                                    {
                                                        if (moderat <= maxmarkmoderation)
                                                        {
                                                            maderonmark = moderat.ToString();
                                                            extemod = (moderat) / minexternaleva * minextmarks;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        moderat = (extemod / evalmaxmark) * evalmaxmark;
                                                        double modesetva = Math.Round(moderat, markround, MidpointRounding.AwayFromZero);
                                                        if (modesetva < moderat)
                                                        {
                                                            moderat = Math.Round(moderat, markround, MidpointRounding.AwayFromZero);
                                                            moderat++;
                                                        }

                                                        //if ((extmark % 2) > 0)
                                                        //{
                                                        //    moderat++;
                                                        //    if (moderat == maxmarkmoderation + 1)
                                                        //    {
                                                        //        moderat = maxmarkmoderation;
                                                        //    }
                                                        //    if (extrenmoder > moderat)
                                                        //    {
                                                        //        moderat = extrenmoder;
                                                        //    }
                                                        //}

                                                        if (moderat <= maxmarkmoderation)
                                                        {
                                                            maderonmark = moderat.ToString();
                                                        }
                                                    }
                                                }
                                                else if (extemod > 0)
                                                {
                                                    moderat = (extemod / evalmaxmark) * evalmaxmark;
                                                    double modesetva = Math.Round(moderat, markround, MidpointRounding.AwayFromZero);
                                                    if (modesetva < moderat)
                                                    {
                                                        moderat = Math.Round(moderat, markround, MidpointRounding.AwayFromZero);
                                                        moderat++;
                                                    }

                                                    //if ((avg % 2) > 0)
                                                    //{
                                                    //    moderat++;
                                                    //    if (moderat == maxmarkmoderation + 1)
                                                    //    {
                                                    //        moderat = maxmarkmoderation;
                                                    //    }
                                                    //}

                                                    if (moderat <= maxmarkmoderation)
                                                    {
                                                        maderonmark = moderat.ToString();
                                                    }
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

                        double[] evaluationAll = new double[3];
                        evaluationAll[0] = ev1;
                        evaluationAll[1] = ev2;
                        evaluationAll[2] = ev3;
                        Array.Sort(evaluationAll);

                        double difference1 = ev1 - ev2;
                        double difference2 = ev2 - ev3;
                        double difference3 = ev3 - ev1;
                        difference1 = Math.Abs(difference1);
                        difference2 = Math.Abs(difference2);
                        difference3 = Math.Abs(difference3);

                        if (difference1 < difference2 && difference1 < difference3)
                        {
                            avg = (ev1 + ev2) / 2;
                        }
                        else if (difference2 < difference1 && difference2 < difference3)
                        {
                            avg = (ev2 + ev3) / 2;
                        }
                        else if (difference3 < difference1 && difference3 < difference2)
                        {
                            avg = (ev3 + ev1) / 2;
                        }

                        if (difference1 == difference2)
                        {
                            avg = (evaluationAll[1] + evaluationAll[2]) / 2;
                            if (difference3 < difference1)
                            {
                                avg = (ev3 + ev1) / 2;
                            }
                        }
                        else if (difference2 == difference3)
                        {
                            avg = (evaluationAll[1] + evaluationAll[2]) / 2;
                            if (difference1 < difference2)
                            {
                                avg = (ev1 + ev2) / 2;
                            }
                        }
                        else if (difference1 == difference3)
                        {
                            avg = (evaluationAll[1] + evaluationAll[2]) / 2;
                            if (difference2 < difference1)
                            {
                                avg = (ev2 + ev3) / 2;
                            }
                        }

                        #region  Added By Malang Raja For Jamal College Condition On Jan 12 2017

                        //double diffEval1And2 = 0;
                        //double diffEval2AndEval3 = 0; //43 30 17

                        //diffEval1And2 = ev1 - ev2;
                        //diffEval2AndEval3 = ev2 - ev3;
                        //diffEval1And2 = Math.Abs(diffEval1And2);
                        //diffEval2AndEval3 = Math.Abs(diffEval2AndEval3);
                        //if (diffEval1And2 == diffEval2AndEval3 && diffEval1And2 > 0 && diffEval2AndEval3 > 0)
                        //{
                        //    double m1 = 0;
                        //    double m2 = 0;
                        //    m2 = ev1 - ev3;
                        //    m2 = Math.Abs(m2);
                        //    m1 = (evaluationAll[1] + evaluationAll[2]) / 2;
                        //    if (m2 < diffEval2AndEval3)
                        //        m1 = (ev1 + ev3) / 2;
                        //    avg = m1;
                        //    //m1 = ((ev1 + ev2) / 2);
                        //    //m2 = ((ev2 + ev3) / 2);
                        //    ////m1 = Math.Abs(m1);
                        //    ////m2 = Math.Abs(m2);
                        //    //if (m1 == m2)
                        //    //{
                        //    //    avg = m1;
                        //    //}
                        //    //else if (m1 < m2)
                        //    //{
                        //    //    avg = m2;
                        //    //}
                        //    //else if (m1 > m2)
                        //    //{
                        //    //    avg = m1;
                        //    //}
                        //}

                        #endregion Added By Malang Raja For Jamal College Condition On Jan 12 2017

                        avg = Math.Round(avg, markround, MidpointRounding.AwayFromZero);
                        intark = Convert.ToDouble(intark);
                        extmark = avg / entmaxexternal * maxexternal;
                        extmark = Math.Round(extmark, markround, MidpointRounding.AwayFromZero);
                        extmark = Math.Round(extmark, markround, MidpointRounding.AwayFromZero);
                        ese = extmark.ToString();
                        totalmarkvalue = extmark + intark;
                        totalmarkvalu = totalmarkvalue.ToString();

                        if (icamark.Trim() != "")
                        {
                            if (!icamark.ToLower().Contains('a') && !icamark.ToLower().Contains("-1"))
                            {
                                double getval = Convert.ToDouble(totalmarkvalue);
                                if ((minicamoderatio <= Convert.ToDouble(intark)) || (getval < mintotalv && minextmarks <= Convert.ToDouble(extmark)))
                                {
                                    if ((getval < mintotalv || minextmarks > Convert.ToDouble(extmark)) && intark >= minintmarks)
                                    {
                                        extemod = mintotalv - getval;
                                        moderat = minexternaleva - extmark;
                                        if (moderat <= maxmarkmoderation)
                                        {
                                            if (moderat > 0)
                                            {
                                                double maxrk = moderat + extmark;
                                                double extcheck = maxrk / maxexternal * maxexternal;
                                                double mintotcheck = extcheck + intark;
                                                double extrenmoder = moderat;
                                                if (mintotcheck >= mintotalv)
                                                {
                                                    if (moderat <= maxmarkmoderation)
                                                    {
                                                        maderonmark = moderat.ToString();
                                                        extemod = moderat / minexternaleva * minextmarks;
                                                    }
                                                }
                                                else
                                                {
                                                    moderat = (extemod / evalmaxmark) * evalmaxmark;
                                                    double modesetva = Math.Round(moderat, markround, MidpointRounding.AwayFromZero);
                                                    if (modesetva < moderat)
                                                    {
                                                        moderat = Math.Round(moderat, markround, MidpointRounding.AwayFromZero);
                                                        moderat++;
                                                    }

                                                    //if ((extmark % 2) > 0)
                                                    //{
                                                    //    moderat++;
                                                    //    if (moderat == maxmarkmoderation + 1)
                                                    //    {
                                                    //        moderat = maxmarkmoderation;
                                                    //    }
                                                    //    if (extrenmoder > moderat)
                                                    //    {
                                                    //        moderat = extrenmoder;
                                                    //    }
                                                    //}

                                                    if (moderat <= maxmarkmoderation)
                                                    {
                                                        maderonmark = moderat.ToString();
                                                    }
                                                }
                                            }
                                            else if (extemod > 0)
                                            {
                                                //moderat = (extemod / manextmarks) * evalmaxmark;
                                                moderat = (extemod / evalmaxmark) * evalmaxmark;
                                                double modesetva = Math.Round(moderat, markround, MidpointRounding.AwayFromZero);
                                                if (modesetva < moderat)
                                                {
                                                    moderat = Math.Round(moderat, markround, MidpointRounding.AwayFromZero);
                                                    moderat++;
                                                }
                                                //if ((avg % 2) > 0)
                                                //{
                                                //    moderat++;
                                                //    if (moderat == maxmarkmoderation + 1)
                                                //    {
                                                //        moderat = maxmarkmoderation;
                                                //    }
                                                //}
                                                if (moderat <= maxmarkmoderation)
                                                {
                                                    maderonmark = moderat.ToString();
                                                }
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
                    if (chkicaretake.Checked == true || chkmarkbased.Checked == true)
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
                            //evauation1 = "Null";
                            //evauation2 = "Null";
                        }
                    }

                    if (maderonmark != "" && chkmoderation.Checked == true)
                    {
                        moderationmark = extmark + Convert.ToDouble(Math.Round(Convert.ToDouble(maderonmark)));
                        moderationmark = moderationmark / maxexternal * maxexternal;
                        moderationmark = Math.Round(moderationmark, markround, MidpointRounding.AwayFromZero);
                        moderationmark = Math.Round(moderationmark, markround, MidpointRounding.AwayFromZero);
                        remaim = moderationtot - Convert.ToDouble(maderonmark);
                        remaim = Math.Round(Convert.ToDouble(remaim));
                        maderonmark = Convert.ToString(Math.Round(Convert.ToDouble(maderonmark)));
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
                        if (chkonlyica.Checked == true) // added by jairam 31-01-2015
                        {
                            if (icamark.Trim() != "NaN" && icamark.Trim() != "" && icamark.Trim() != "Null")
                            {
                                //if (ese.Trim() == "NaN" || ese.Trim() == "" || ese.Trim() == "Null")
                                //{
                                //    ese = "0";
                                //}
                                //if (totalmarkvalu.Trim() == "NaN" || totalmarkvalu.Trim() == "" || totalmarkvalu.Trim() == "Null")
                                //{
                                if (Convert.ToDouble(icamark) < 0)
                                {
                                    // totalmarkvalu = icamark;
                                    evauation1 = "-1";
                                    evauation2 = "-1";
                                    ese = "-1";
                                    result = "AAA";
                                    totalmarkvalu = icamark;
                                    evauation3 = "Null";
                                }
                                else
                                {
                                    totalmarkvalu = icamark;
                                    ese = "0";
                                    if (minintmarks <= intark)
                                    {
                                        result = "Pass";
                                    }
                                }

                                //}
                            }
                        }

                        insupdquery = "if not exists(select * from mark_entry where exam_code='" + examcode + "' and roll_no='" + roll_no + "' and subject_no='" + subject_no + "')";
                        insupdquery = insupdquery + " insert into mark_entry (roll_no,subject_no,exam_code,internal_mark,external_mark,total,result,passorfail,attempts,MYData,rej_stat,cp,evaluation1,evaluation2,evaluation3)";
                        insupdquery = insupdquery + " values('" + roll_no + "','" + subject_no + "','" + examcode + "'," + icamark + "," + ese + "," + totalmarkvalu + ",'" + result + "','" + passorfail + "','" + attempts + "','" + my + "','0','" + creditpoint + "'," + evauation1 + "," + evauation2 + "," + evauation3 + ")";
                        insupdquery = insupdquery + " else";
                        insupdquery = insupdquery + " update mark_entry set internal_mark=" + icamark + ",external_mark=" + ese + ",total=" + totalmarkvalu + ",result='" + result + "',passorfail='" + passorfail + "',attempts='" + attempts + "',evaluation1=" + evauation1 + ",evaluation2=" + evauation2 + ",evaluation3=" + evauation3 + "";
                        insupdquery = insupdquery + " where exam_code='" + examcode + "' and roll_no='" + roll_no + "' and subject_no='" + subject_no + "'";

                        insupdval = da.update_method_wo_parameter(insupdquery, "Text");
                        if (maderonmark == "")
                        {
                            insupdquery = "delete from moderation where exam_code=" + examcode + " and subject_no=" + subject_no + " and roll_no='" + roll_no + "' and degree_code=" + degreecode + " and exam_month=" + exammonth + " and exam_year=" + examyear + "";
                            insupdval = da.update_method_wo_parameter(insupdquery, "Text");
                        }
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

                        insupdquery = "if not exists(select * from mark_entry where exam_code='" + examcode + "' and roll_no='" + roll_no + "' and subject_no='" + subject_no + "')";
                        insupdquery = insupdquery + " insert into mark_entry (roll_no,subject_no,exam_code,internal_mark,external_mark,total,result,passorfail,attempts,MYData,rej_stat,cp,evaluation1,evaluation2,evaluation3)";
                        insupdquery = insupdquery + " values('" + roll_no + "','" + subject_no + "','" + examcode + "'," + icamark + "," + ese + "," + totalmarkvalu + ",'" + result + "','" + passorfail + "','" + attempts + "','" + my + "','1','" + creditpoint + "'," + evauation1 + "," + evauation2 + "," + evauation3 + ")";
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
            lblerr1.Text = ex.ToString();
        }
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

    private bool CheckIncludeCondoUnpaidForMarkEntry()
    {
        bool isResult = false;
        try
        {
            string grouporusercode1 = string.Empty;
            if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode1 = " and group_code='" + Convert.ToString(Session["group_code"]).Trim() + "'";
            }
            else
            {
                grouporusercode1 = " and usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            string checkValue = da.GetFunctionv("select value from Master_Settings where settings='IncludeCondonationUnpaidStudentsInCoeMarkEntry' " + grouporusercode1 + "");
            if (string.IsNullOrEmpty(checkValue.Trim()) || checkValue.Trim() == "0")
            {
                isResult = false;
            }
            else if (checkValue.Trim() == "1")
            {
                isResult = true;
            }
            else
            {
                isResult = false;
            }
            return isResult;
        }
        catch (Exception ex)
        {
            return false;
        }
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
   
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getbundleno(string prefixtext)
    {
         
        List<string> name = new List<string>();
        {
            try
            {
                string query = "";
                WebService ws = new WebService();

                query = "select distinct ISNULL(es.bundle_no,'0') as bundleNo from exam_seating es  where bundle_no like '" + prefixtext + "%' order by bundleNo asc";

                name = ws.Getname(query);

                return name;
            }
            catch
            {
                return name;
            }
        }
        

    }

    protected void chkbundleno_CheckedChanged(object sender, EventArgs e)
    {
        
        if (chkbundleno.Checked == true)
        {
            UpdatePanel24.Visible = true;
            txtbundleno.Visible = true;
        }
        else
        {

            ddlsem1.Enabled = true;
            ddlsubtype.Enabled = true;
            ddlSubject.Enabled = true;
            UpdatePanel24.Visible = false;
        }

        
       
    }

    protected void txtBundleNo_TextChanged(object sender, EventArgs e)
    {
        
        //txtbundleno = "select distinct ISNULL(es.bundle_no,'0') as bundleNo from exam_seating es  where bundle_no like '" + prefixtext1 + "%' order by bundleNo asc";

    }

    
}
