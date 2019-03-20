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


public partial class COESubtypePartSettings : System.Web.UI.Page
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
                Page.MaintainScrollPositionOnPostBack = true;
                BindCollege();
                year1();
                degree();
                Session["MarkEntrySave"] = 1;
            }
        }
        catch(Exception ex)
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
            DataTable dtsettings = dirAccess.selectDataTable("select distinct value from master_settings where settings='" + settings + "' " + grouporusercode + "");
            string val1 = string.Empty;
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
                                if (string.IsNullOrEmpty(val1))
                                    val1 = val;
                                else if (!val1.Contains(val))
                                    val1 = val1 + "','" + val;
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(val1))
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
                ds = da.BindSem(ddlbranch1.SelectedValue.ToString(), ddlYear1.SelectedValue.ToString(), collegecode);
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


            string qeryss = "SELECT distinct ss.subject_type,s.subject_name,s.subject_code,s.subject_name+' - '+s.subject_code as subnamecode FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,syllabus_master sy,sub_sem ss,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and sy.semester='" + semmv + "' and  ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "'  and sy.degree_code='" + branc + "'  and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "'";
            qeryss = qeryss + " union SELECT distinct ss.subject_type,s.subject_name,s.subject_code,s.subject_name+' - '+s.subject_code as subnamecode FROM subjectChooser sc,subject s,Registration r,Exam_Details ed,sub_sem ss,Degree d,Course c where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and sc.semester=ed.Current_Semester and s.subType_no=ss.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and sc.semester='" + semmv + "' and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "'  and r.degree_code='" + branc + "'   and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "' and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' order by s.subject_name,s.subject_code desc";
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
            string qeryss = "SELECT distinct ss.subject_type FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,syllabus_master sy,sub_sem ss,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and sy.semester='" + semmv + "' and  ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' " + typeval + " ";
            qeryss = qeryss + " union SELECT distinct ss.subject_type FROM subjectChooser sc,subject s,Registration r,Exam_Details ed,sub_sem ss,Degree d,Course c where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and sc.semester=ed.Current_Semester and s.subType_no=ss.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and sc.semester='" + semmv + "' and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' " + typeval + " and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' order by ss.subject_type";


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

    }

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        year1();
        month1();
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

    protected void btnviewre_Click(object sender, EventArgs e)
    {
        try
        {
            lblMin.Visible = false;
            lblminMark.Visible = false;
            lblMax.Visible = false;
            lblMaxMark.Visible = false;
            btnSave.Visible = false;
            lblerr1.Visible = false;
          
            DataTable dtPartAlloc = new DataTable();
            dtPartAlloc.Columns.Add("PartNo");
            dtPartAlloc.Columns.Add("PartName");
            dtPartAlloc.Columns.Add("Part");
            dtPartAlloc.Columns.Add("maxmark");
            DataRow dr = null;
            int noPart = 0;
            lblerr1.Visible = false;
            if (ddlYear1.Items.Count == 0)
            {
                lblerr1.Visible = true;
                lblerr1.Text = "No Exam Year Found";
                return;
            }
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
            if (ddldegree1.Items.Count > 0)
            {
                if (ddldegree1.SelectedIndex == 0)
                {
                    lblerr1.Visible = true;
                    lblerr1.Text = "Please Select Degree";
                    return;
                }
            }
            if (ddlbranch1.Items.Count > 0)
            {
                if (ddlbranch1.SelectedIndex == 0)
                {
                    lblerr1.Visible = true;
                    lblerr1.Text = "Please Select branch";
                    return;
                }
            }
            if (ddlsem1.SelectedIndex == 0)
            {
                lblerr1.Visible = true;
                lblerr1.Text = "Please Select Semester";
                return;
            }
            if (ddlSubject.Items.Count > 0)
            {
                if (ddlSubject.SelectedIndex == 0)
                {
                    lblerr1.Visible = true;
                    lblerr1.Text = "Please Select Subject";
                    return;
                }
            }
            string DegCode = Convert.ToString(ddlbranch1.SelectedValue);
            string Sem = Convert.ToString(ddlsem1.SelectedItem.Text);
            string subCode = Convert.ToString(ddlSubject.SelectedValue);
            string examMonth = Convert.ToString(ddlMonth1.SelectedValue);
            string examYear = Convert.ToString(ddlYear1.SelectedValue);
            string suCode = Convert.ToString(ddlSubject.SelectedValue);
            bool isData = false;
            string SelectQ = "select id from COESubSubjectPartMater where ExamYear='" + examYear + "' and ExamMonth='" + examMonth + "' and DegreeCode='" + DegCode + "' and Semester='" + Sem + "'";
            string Id = da.GetFunction(SelectQ);
            if (!string.IsNullOrEmpty(Id) && Id != "0")
            {
                string SQl = "select * from COESubSubjectPartSettings  where id='" + Id + "' and subCode='" + suCode + "'";
                DataTable dtSuSubject = dirAccess.selectDataTable(SQl);
                if (dtSuSubject.Rows.Count > 0)
                {
                    txtNoPart.Text = dtSuSubject.Rows.Count.ToString();
                    int.TryParse(txtNoPart.Text, out noPart);
                    if (noPart > 0)
                    {
                        for (int i = 1; i <= noPart; i++)
                        {
                            dr = dtPartAlloc.NewRow();
                            dr["PartNo"] = i;
                            dr["PartName"] = getPartText(i.ToString());
                            dr["Part"] = Convert.ToString(dtSuSubject.Rows[i - 1]["subpart"]);//
                            dr["maxmark"] = Convert.ToString(dtSuSubject.Rows[i - 1]["maxmark"]);
                            dtPartAlloc.Rows.Add(dr);
                        }
                        isData = true;
                    }
                }

            }
             if (!isData)
            {
                if (!string.IsNullOrEmpty(txtNoPart.Text))
                {
                    int.TryParse(txtNoPart.Text, out noPart);
                    if (noPart > 0)
                    {
                        for (int i = 1; i <= noPart; i++)
                        {
                            dr = dtPartAlloc.NewRow();
                            dr["PartNo"] = i;
                            dr["PartName"] = getPartText(i.ToString());
                            dr["Part"] = "";
                            dr["maxmark"] = "";
                            dtPartAlloc.Rows.Add(dr);
                        }
                    }
                }
                else
                {
                    lblerr1.Visible = true;
                    lblerr1.Text = "Enter No.of sub-Subject";
                }
            }
            if (dtPartAlloc.Rows.Count > 0)
            {
                lblMin.Visible = true;
                lblminMark.Visible = true;
                lblMax.Visible = true;
                lblMaxMark.Visible = true;
                DataTable txtMinMax = dirAccess.selectDataTable("select distinct min_ext_marks,max_ext_marks from subject where subject_code='" + subCode + "'");
                if (txtMinMax.Rows.Count > 0)
                {
                    lblminMark.Text = Convert.ToString(txtMinMax.Rows[0]["min_ext_marks"]);
                    lblMaxMark.Text = Convert.ToString(txtMinMax.Rows[0]["max_ext_marks"]);
                }
                GridView1.DataSource = dtPartAlloc;
                GridView1.DataBind();
                GridView1.Visible = true;
                GridView1.Visible = true;
                btnSave.Visible = true;
            }

        }
        catch
        {
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            lblerr1.Visible = false;
            if (ddlYear1.Items.Count == 0)
            {
                lblerr1.Visible = true;
                lblerr1.Text = "No Exam Year Found";
                return;
            }
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
            if (ddldegree1.Items.Count > 0)
            {
                if (ddldegree1.SelectedIndex == 0)
                {
                    lblerr1.Visible = true;
                    lblerr1.Text = "Please Select Degree";
                    return;
                }
            }
            if (ddlbranch1.Items.Count > 0)
            {
                if (ddlbranch1.SelectedIndex == 0)
                {
                    lblerr1.Visible = true;
                    lblerr1.Text = "Please Select branch";
                    return;
                }
            }
            if (ddlsem1.SelectedIndex == 0)
            {
                lblerr1.Visible = true;
                lblerr1.Text = "Please Select Semester";
                return;
            }
            if (ddlSubject.Items.Count > 0)
            {
                if (ddlSubject.SelectedIndex == 0)
                {
                    lblerr1.Visible = true;
                    lblerr1.Text = "Please Select Subject";
                    return;
                }
            }

            string DegCode = Convert.ToString(ddlbranch1.SelectedValue);
            string Sem = Convert.ToString(ddlsem1.SelectedItem.Text);
            string subCode = Convert.ToString(ddlSubject.SelectedValue);
            string examMonth = Convert.ToString(ddlMonth1.SelectedValue);
            string examYear = Convert.ToString(ddlYear1.SelectedValue);
            string suCode = Convert.ToString(ddlSubject.SelectedValue);
            double Chkmax = 0;
            double Mark = 0;
            double.TryParse(lblMaxMark.Text,out Mark);
            foreach (GridViewRow grid in GridView1.Rows)
            {
                string maxMark = (grid.FindControl("txtMaxMark") as TextBox).Text;
                double maxMa=0;
                double.TryParse(maxMark, out maxMa);
                Chkmax=Chkmax+maxMa;
            }
            if (Chkmax != Mark)
            {
                lblerr1.Visible = true;
                lblerr1.Text = "Enter Max mark equal to Subject Max Mark!";
                return;
            }

            if (!string.IsNullOrEmpty(DegCode) && !string.IsNullOrEmpty(Sem) && !string.IsNullOrEmpty(subCode))
            {
                string UpSelectQ = "if not exists (select * from COESubSubjectPartMater where ExamYear='" + examYear + "' and ExamMonth='" + examMonth + "' and DegreeCode='" + DegCode + "' and Semester='" + Sem + "') insert into COESubSubjectPartMater (ExamYear , ExamMonth , DegreeCode , Semester) values('" + examYear + "','" + examMonth + "','" + DegCode + "','" + Sem + "')";
                int update = da.update_method_wo_parameter(UpSelectQ, "text");
                string ID = da.GetFunction("select id from COESubSubjectPartMater where ExamYear='" + examYear + "' and ExamMonth='" + examMonth + "' and DegreeCode='" + DegCode + "' and Semester='" + Sem + "'");
                if (!string.IsNullOrEmpty(ID) && ID != "0")
                {
                    string delQ = "delete From COESubSubjectPartSettings where id='" + ID + "'";
                    int del = da.update_method_wo_parameter(delQ, "text");
                    foreach (GridViewRow grid in GridView1.Rows)
                    {
                        string part = (grid.FindControl("txtgMarks") as TextBox).Text;
                        string maxMark = (grid.FindControl("txtMaxMark") as TextBox).Text;
                        string insQ = "insert into COESubSubjectPartSettings (id,SubCode,SubPart,maxmark) values('" + ID + "','" + suCode + "','" + part + "'," + maxMark + ")";
                        int inserQ = da.update_method_wo_parameter(insQ, "text");
                    }
                }

                lblerr1.Visible = true;
                lblerr1.Text = "Saved";
                return;

            }
        }
        catch
        {
        }
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

}