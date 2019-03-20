using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using InsproDataAccess;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.DataVisualization.Charting;

public partial class MarkMod_courseoutcomeattainment : System.Web.UI.Page
{
    string collegecode = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string group_code = string.Empty;
    string columnfield = string.Empty;

    ReuasableMethods rs = new ReuasableMethods();
    InsproDirectAccess dir = new InsproDirectAccess();
    Hashtable hat = new Hashtable();
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    int k = 0;
    double studcount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            if (!Request.FilePath.Contains("CAMHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/MarkMod/CAMHome.aspx");
                    return;
                }
            }
            collegecode = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
            usercode = (Session["userco,de"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
            singleuser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
            group_user = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";


            if (!IsPostBack)
            {
                bindclg();
                bindbatch();
                binddegree();
                bindbranch();
                bindsem();
                //BindSectionDetail();
                BindCourseoutcome();
                bindSubject();
                Bindtest();
            }
        }
        catch
        {
        }

    }


    #region bindheaders

    public void bindclg()
    {
        try
        {

            group_code = Session["group_code"].ToString();
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
            ds = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();

            }

        }
        catch
        {
        }
    }

    public void bindbatch()
    {
        try
        {

            ds.Dispose();
            ds.Reset();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "Batch_year";
                ddlbatch.DataValueField = "Batch_year";
                ddlbatch.DataBind();
            }
        }
        catch
        {
        }
    }

    public void binddegree()
    {
        try
        {

            collegecode = ddlcollege.SelectedValue.ToString();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            hat.Clear();
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds = d2.select_method("bind_degree", hat, "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }
        }
        catch
        {
        }
    }

    public void bindbranch()
    {
        try
        {
            string typ = ddldegree.SelectedValue.ToString();
            collegecode = ddlcollege.SelectedValue.ToString();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            hat.Clear();
            hat.Add("single_user", singleuser.ToString());
            hat.Add("group_code", group_user);
            hat.Add("course_id", typ);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds.Clear();
            ds = d2.select_method("bind_branch", hat, "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
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
            ddlsemester.Items.Clear();

            string degreecode = ddlbranch.SelectedValue.ToString();
            string strgetfuncuti = string.Empty;
            if (degreecode.Trim() != "")
            {
                strgetfuncuti = d2.GetFunction("select max(Duration) from Degree where Degree_Code in(" + degreecode + ")");
            }
            if (Convert.ToInt32(strgetfuncuti) > 0)
            {
                for (int loop_val = 1; loop_val <= Convert.ToInt32(strgetfuncuti); loop_val++)
                {
                    ddlsemester.Items.Add(loop_val.ToString());
                }
            }

        }
        catch
        {
        }
    }
    //public void BindSectionDetail()
    //{
    //    try
    //    {
    //        string batch = ddlbatch.SelectedValue.ToString();
    //        string branch = ddlbranch.SelectedValue.ToString();
    //        ddlsection.Items.Clear();
    //        ds.Dispose();
    //        ds.Reset();
    //        ds = d2.BindSectionDetail(batch, branch);
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            ddlsection.DataSource = ds;
    //            ddlsection.DataTextField = "sections";
    //            ddlsection.DataBind();
    //            ddlsection.Items.Insert(0, "All");
    //            if (Convert.ToString(ds.Tables[0].Columns["sections"]) == string.Empty)
    //            {
    //                ddlsection.Enabled = false;

    //            }
    //            else
    //            {
    //                ddlsection.Enabled = true;

    //            }
    //        }
    //        else
    //        {
    //            ddlsection.Enabled = false;
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}
    public void bindSubject()
    {


        string batchYear = Convert.ToString(ddlbatch.SelectedValue);
        string degCode = Convert.ToString(ddlbranch.SelectedValue);
        string sem = Convert.ToString(ddlsemester.SelectedValue);
        string colCode = Convert.ToString(ddlcollege.SelectedValue);

        string selectQ = string.Empty;

        //    selectQ = "select distinct subject_name,s.subject_code,CONVERT(nvarchar(max),subject_code)+'-'+CONVERT(nvarchar(max),subject_name) as subName,s.subject_no from registration r,subject s,syllabus_master sm,Sub_sem Sem,degree d,course c  where d.degree_code=r.degree_code and d.course_id=c.course_id and r.degree_code=sm.degree_code and r.batch_year=sm.batch_year and r.current_semester=sm.semester and sm.syll_code=s.syll_code  and  s.syll_code=SM.syll_code  and  S.subtype_no = Sem.subtype_no and promote_count=1 and c.college_code=" + colCode + "  and r.degree_code='" + degCode + "' and r.Batch_Year='" + batchYear + "' and r.Current_Semester='" + sem + "'  and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar'  order by subject_name";


        string logstaffcode = "";
        if (Convert.ToString(Session["Staff_Code"]) != "")
        {
            logstaffcode = " and st.staff_code='" + Convert.ToString(Session["Staff_Code"]) + "'";
        }
        //==================================//
        selectQ = "select distinct S.subject_no,subject_name,s.subject_code,CONVERT(nvarchar(max),subject_code)+'-'+CONVERT(nvarchar(max),subject_name) as subName from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and  st.subject_no=s.subject_no  and SM.degree_code=" + ddlbranch.SelectedValue.ToString() + " and SM.semester='" + sem.ToString() + "' and  S.subtype_no = Sem.subtype_no and promote_count=1 and SM.batch_year='" + ddlbatch.SelectedValue.ToString() + "' " + logstaffcode + " order by S.subject_no ";

        DataSet dss = d2.select_method_wo_parameter(selectQ, "text");
        if (dss.Tables.Count > 0 && dss.Tables[0].Rows.Count > 0)
        {
            ddl_subject.DataSource = dss;
            ddl_subject.DataTextField = "subName";
            ddl_subject.DataValueField = "subject_no";
            ddl_subject.DataBind();

        }

    }

    public void BindCourseoutcome()
    {
        try
        {
            ds.Clear();
            string course = "Select distinct template,masterno from  Master_Settings where settings='COSettings'";
            ds = d2.select_method_wo_parameter(course, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlconame.DataSource = ds;
                ddlconame.DataTextField = "template";
                ddlconame.DataValueField = "masterno";
                ddlconame.DataBind();

            }

        }
        catch (Exception ex)
        {

        }
    }

    #region Test
    public void Bindtest()
    {
        try
        {

            txttest.Text = "--Select--";
            chktest.Checked = false;
            cbltest.Items.Clear();
            DataSet titles = new DataSet();
            string sems = string.Empty;
            int selSem = 0;
            string semester = string.Empty;
            string subjectno = string.Empty;

            string sections = string.Empty;
            string strsec = string.Empty;

            string degreecode = string.Empty;
            string collegecode = string.Empty;
            string batchyear = string.Empty;

            if (ddlcollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value).Trim();
            }
            if (ddlbatch.Items.Count > 0)
            {
                batchyear = Convert.ToString(ddlbatch.SelectedItem.Text).Trim();
            }
            if (ddlbranch.Items.Count > 0)
            {
                degreecode = Convert.ToString(ddlbranch.SelectedItem.Value).Trim();
            }

            if (ddl_subject.Items.Count > 0)
            {
                subjectno = Convert.ToString(ddl_subject.SelectedValue).Trim();
            }

            //if (ddlsection.Items.Count > 0)
            //{
            //    sections = Convert.ToString(ddlsection.SelectedValue).Trim();
            //    if (Convert.ToString(ddlsection.SelectedItem.Text).Trim().ToLower() == "all" || Convert.ToString(ddlsection.SelectedItem.Text).Trim().ToLower() == "" || Convert.ToString(ddlsection.SelectedItem.Text).Trim().ToLower() == "-1")
            //    {
            //        strsec = "";
            //    }
            //    else
            //    {
            //        strsec = " and isnull(ltrim(rtrim(Section)),'')='" + Convert.ToString(sections).Trim() + "'";
            //    }
            //}
            if (ddlsemester.Items.Count > 0)
            {
                semester = Convert.ToString(ddlsemester.SelectedValue).Trim();
            }
            string coname = Convert.ToString(ddlconame.SelectedValue);

            if (!string.IsNullOrEmpty(semester.Trim()) && !string.IsNullOrEmpty(subjectno) && !string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(batchyear) && !string.IsNullOrEmpty(degreecode) && !string.IsNullOrEmpty(coname))
            {
                sems = " and s.semester in(" + semester + ")";
                string Sqlstr = "select criteria_no,criterianame from weightage_setting where cono='" + coname + "' and batch='" + batchyear + "' and degree_code='" + degreecode + "' and semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and subject_no='" + ddl_subject.SelectedValue.ToString() + "'";
                titles.Clear();
                titles.Dispose();
                titles = d2.select_method_wo_parameter(Sqlstr, "Test");
            }
            if (titles.Tables.Count > 0 && titles.Tables[0].Rows.Count > 0)
            {
                cbltest.DataSource = titles;
                cbltest.DataValueField = "criteria_no";
                cbltest.DataTextField = "criterianame";
                cbltest.DataBind();
            }

            if (cbltest.Items.Count > 0)
            {
                for (int row = 0; row < cbltest.Items.Count; row++)
                {
                    cbltest.Items[row].Selected = true;
                    chktest.Checked = true;
                }
                txttest.Text = "Test(" + cbltest.Items.Count + ")";
            }
            else
            {
                txttest.Text = "--Select--";
            }


        }
        catch (Exception ex)
        {

        }
    }

    protected void chktest_CheckedChanged(object sender, EventArgs e)
    {
        if (chktest.Checked == true)
        {
            for (int i = 0; i < cbltest.Items.Count; i++)
            {
                cbltest.Items[i].Selected = true;
            }
            txttest.Text = "Test(" + (cbltest.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbltest.Items.Count; i++)
            {
                cbltest.Items[i].Selected = false;
            }
            txttest.Text = "---Select---";
        }
        //  Bindtest();

    }

    protected void cbltest_SelectedIndexChanged(object sender, EventArgs e)
    {
        chktest.Checked = false;
        txttest.Text = "---Select---";

        int subjectcount = 0;
        string value = string.Empty;
        string code = string.Empty;
        for (int i = 0; i < cbltest.Items.Count; i++)
        {
            if (cbltest.Items[i].Selected == true)
            {
                value = cbltest.Items[i].Text;
                code = cbltest.Items[i].Value.ToString();
                subjectcount = subjectcount + 1;
            }
        }
        if (subjectcount > 0)
        {
            txttest.Text = "Test(" + subjectcount.ToString() + ")";
            if (subjectcount == cbltest.Items.Count)
            {
                chktest.Checked = true;
            }
        }

    }
    #endregion


    #endregion

    public string filterfunction()
    {
        string strorder = "ORDER BY Registration.Roll_No";
        string serialno = d2.GetFunction("select LinkValue from inssettings where college_code=" + Convert.ToString(ddlcollege.SelectedValue) + " and linkname='Student Attendance'");
        if (serialno == "1")
        {
            strorder = "   order by registration.serialno";
        }
        else
        {
            string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
            if (orderby_Setting == "0")
            {
                strorder = "  ORDER BY Registration.Roll_No";
            }
            else if (orderby_Setting == "1")
            {
                strorder = "   ORDER BY Registration.Reg_No";
            }
            else if (orderby_Setting == "2")
            {
                strorder = "  ORDER BY Registration.Stud_Name";
            }
            else if (orderby_Setting == "0,1,2")
            {
                strorder = "  ORDER BY Registration.Roll_No,Registration.Reg_No,Registration.Stud_Name";
            }
            else if (orderby_Setting == "0,1")
            {
                strorder = "   ORDER BY Registration.Roll_No,Registration.Reg_No";
            }
            else if (orderby_Setting == "1,2")
            {
                strorder = "  ORDER BY Registration.Reg_No,Registration.Stud_Name";
            }
            else if (orderby_Setting == "0,2")
            {
                strorder = "  ORDER BY Registration.Roll_No,Registration.Stud_Name";
            }
        }
        return strorder;
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            Hashtable hat1 = new Hashtable();
            Dictionary<string, string> dicval = new Dictionary<string, string>();
            DataTable dt = new DataTable();
            DataRow dr = null;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            double threshold = 0;
            double target = 0;

            string weightpercent = string.Empty;
            string clgcode = ddlcollege.SelectedValue.ToString();
            string degcode = ddlbranch.SelectedValue.ToString();
            string batchyear = ddlbatch.SelectedValue.ToString();
            string section = string.Empty;
            string section1 = string.Empty;

            string tstcode = string.Empty;
            string tstcode1 = string.Empty;
            string semester = Convert.ToString(ddlsemester.SelectedValue).Trim();
            string coname = Convert.ToString(ddlconame.SelectedValue);
            string coname1 = Convert.ToString(ddlconame.SelectedItem.Text);
            string subcode = Convert.ToString(ddl_subject.SelectedValue).Trim();
            for (int g = 0; g < cbltest.Items.Count; g++)
            {
                if (cbltest.Items[g].Selected == true)
                {
                    if (string.IsNullOrEmpty(tstcode))
                        tstcode = cbltest.Items[g].Value;
                    else
                        tstcode = tstcode + "," + cbltest.Items[g].Value;

                    if (string.IsNullOrEmpty(tstcode1))
                        tstcode1 = cbltest.Items[g].Value;
                    else
                        tstcode1 = tstcode1 + "','" + cbltest.Items[g].Value;
                }
            }
            string[] sptval = null;
            string testcode = string.Empty;
            if (tstcode.Contains(','))
            {
                sptval = tstcode.Split(',');
                for (int q = 0; q < sptval.Length; q++)
                {
                    if (string.IsNullOrEmpty(testcode))
                        testcode = "'" + Convert.ToString(sptval[q]) + "'";
                    else
                        testcode = testcode + ",'" + Convert.ToString(sptval[q]) + "'";
                }
            }
            else
                testcode = tstcode;

            string strstaffselecotr = string.Empty;
            string staffCode = Convert.ToString(Session["staff_code"]);
            bool staffSelector = false;
            string minimumabsentsms = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Convert.ToString(ddlcollege.SelectedValue).Trim() + "'");
            string[] splitminimumabsentsms = minimumabsentsms.Split('-');
            if (splitminimumabsentsms.Length == 2)
            {
                int batchyearsetting = 0;
                int.TryParse(Convert.ToString(splitminimumabsentsms[1]).Trim(), out batchyearsetting);
                if (splitminimumabsentsms[0].ToString() == "1")
                {
                    if (Convert.ToInt32(ddlbatch.SelectedItem.Text.ToString()) >= batchyearsetting)
                    {
                        staffSelector = true;
                    }
                }
            }
            else if (splitminimumabsentsms.Length > 0)
            {
                if (splitminimumabsentsms[0].ToString() == "1")
                {
                    staffSelector = true;
                }
            }
            if (staffSelector == true && !string.IsNullOrEmpty(staffCode))
            {
                strstaffselecotr = " and subjectchooser.staffcode like '%" + Convert.ToString(staffCode) + "%'";
            }

            DataSet dsst = new DataSet();
            Hashtable hat = new Hashtable();
            string strorder = filterfunction();
            string stddet = "Select  distinct registration.roll_no,registration.reg_no,registration.stud_name,registration.sections,registration.App_No,Registration.Sections from registration ,SubjectChooser where  registration.roll_no = subjectchooser.roll_no and registration.Degree_Code ='" + Convert.ToString(degcode) + "' and Semester = '" + Convert.ToString(semester) + "' and registration.Batch_Year = '" + Convert.ToString(ddlbatch.SelectedItem.Text) + "' and Subject_No = '" + Convert.ToString(ddl_subject.SelectedValue) + "' and RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR' and Semester = '" + Convert.ToString(semester) + "'   " + strstaffselecotr + "  " + strorder + ";  select criteria_no,criterianame,coname,cono,weightage_percentage  from weightage_setting where cono='" + coname + "' and batch='" + ddlbatch.SelectedItem.Text.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "' and semester='" + ddlsemester.SelectedItem.Text.ToString() + "'  and subject_no='" + ddl_subject.SelectedValue.ToString() + "' and criteria_no in ('" + tstcode1 + "'); select Threshold,Target from COThresholdSettings where subject_No='" + Convert.ToString(ddl_subject.SelectedValue) + "' and courseID='" + Convert.ToString(ddlconame.SelectedValue) + "'";


            stddet = stddet + " select Mark_Grade,Frange,Trange from grade_master_CamInternal where batch_year='" + Convert.ToString(ddlbatch.SelectedItem.Text) + "' and Semester='" + Convert.ToString(ddlsemester.SelectedItem.Text) + "' and Degree_Code='" + Convert.ToString(ddlbranch.SelectedValue) + "' and College_Code='" + Convert.ToString(ddlcollege.SelectedValue) + "'";
            stddet = stddet + " select Mark_Grade,Frange,Trange from grade_master_CamInternal where batch_year='" + Convert.ToString(ddlbatch.SelectedItem.Text) + "' and Degree_Code='" + Convert.ToString(ddlbranch.SelectedValue) + "' and College_Code='" + Convert.ToString(ddlcollege.SelectedValue) + "'";

            dsst = d2.select_method_wo_parameter(stddet, "Text");
            if (dsst.Tables.Count > 0 && dsst.Tables[0].Rows.Count > 0)
            {
                if (dsst.Tables[2].Rows.Count > 0)
                {
                    string thd = Convert.ToString(dsst.Tables[2].Rows[0]["Threshold"]);
                    double.TryParse(thd, out threshold);
                    string tgt = Convert.ToString(dsst.Tables[2].Rows[0]["Target"]);
                    double.TryParse(tgt, out target);
                }

                if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(coname) && !string.IsNullOrEmpty(subcode) && !string.IsNullOrEmpty(tstcode))
                {
                    string sqlqry = " select distinct im.app_no,im.ExamCode from CAQuesSettingsParent ca,NewInternalMarkEntry im,Exam_type et where ca.MasterID=im.MasterID and ca.subjectNo=et.subject_no and im.ExamCode=et.exam_code and ca.CourseOutComeNo in('" + coname + "') and ca.subjectno in('" + subcode + "') and ca.CriteriaNo in(" + testcode + ")";

                    sqlqry += " select distinct PartNo,CourseOutComeNo,subjectno,CriteriaNo from CAQuesSettingsParent  where CourseOutComeNo in('" + coname + "' )  and subjectno in('" + subcode + "') and CriteriaNo in(" + testcode + ")";
                    sqlqry += " select * from CAQuesSettingsParent  where CourseOutComeNo in('" + coname + "' )  and subjectno in('" + subcode + "') and CriteriaNo in(" + testcode + ")";

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(sqlqry, "text");
                    DataSet dstd = new DataSet();
                    string stdmark = "select SUM(im.marks) mark,examcode,app_no,ca.CourseOutComeNo,ca.PartNo,(select isnull(template,'') from Master_Settings where settings='COSettings' and masterno=ca.CourseOutComeNo) as CourseoutCome,c.criteria,c.criteria_no from CAQuesSettingsParent ca,NewInternalMarkEntry im,criteriaforInternal c where ca.MasterID=im.MasterID and subjectno in('" + subcode + "') and CriteriaNo in(" + testcode + ") and (marks<>-1 and marks<>-16 and marks<>-20) and c.criteria_no=ca.criteriano  group by examcode,app_no,ca.CourseOutComeNo,ca.PartNo,c.criteria,c.criteria_no";

                    dstd = d2.select_method_wo_parameter(stdmark, "Text");

                    DataTable dtQsettings = dir.selectDataTable("select (select isnull(template,'') from Master_Settings where settings='COSettings' and masterno=ca.CourseOutComeNo) as CourseoutCome,ca.partno,ca.qno,ca.mark,ca.sub1,c.criteria,c.criteria_no from CAQuesSettingsParent ca,criteriaforInternal c  where ca.CourseOutComeNo in('" + coname + "')  and ca.subjectno in('" + subcode + "') and ca.CriteriaNo in(" + testcode + ") and c.criteria_no=ca.criteriano ");

                    DataTable dicPart = dtQsettings.DefaultView.ToTable(true, "partNo", "qno");
                    DataTable dicPartCo = dtQsettings.DefaultView.ToTable(true, "CourseoutCome", "criteria_no");
                    DataTable dicQSub = dtQsettings.DefaultView.ToTable(true, "CourseoutCome", "criteria_no");

                    Dictionary<string, int> dicstud = new Dictionary<string, int>();
                    DataTable dt1 = new DataTable();
                    DataRow dr1 = null;
                    dt1.Columns.Add("Grade");
                    dt1.Columns.Add("StudentCount");

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        dt.Columns.Add("S.No");
                        dt.Columns.Add("Roll No");
                        dt.Columns.Add("Reg No");
                        dt.Columns.Add("Student Name");

                        for (int j = 0; j < cbltest.Items.Count; j++)
                        {
                            if (cbltest.Items[j].Selected == true)
                            {
                                k++;

                                string val1 = cbltest.Items[j].Value.ToString();
                                dt.Columns.Add("" + cbltest.Items[j].Text.ToString() + "");
                                dic.Add(val1, cbltest.Items[j].Text.ToString());
                            }
                        }
                        dt.Columns.Add("Total");
                        dt.Columns.Add("Grade");

                        dr = dt.NewRow();
                        dr["S.No"] = "S.No";
                        dr["Roll No"] = "Roll No";
                        dr["Reg No"] = "Reg No";
                        dr["Student Name"] = "Student Name";
                        double totheadval = 0;
                        if (dic.Count > 0)
                        {
                            foreach (KeyValuePair<string, string> dc1 in dic)
                            {
                                string val = dc1.Value;
                                string ky = dc1.Key;
                                dsst.Tables[1].DefaultView.RowFilter = " criteria_no='" + Convert.ToString(ky) + "'";
                                DataView dv2 = dsst.Tables[1].DefaultView;
                                if (dv2.Count > 0)
                                {
                                    weightpercent = Convert.ToString(dv2[0]["weightage_percentage"]);
                                    totheadval += Convert.ToDouble(weightpercent);
                                }

                                dr["" + val + ""] = "" + val + "(100)";
                            }
                        }
                        dr["Total"] = "Total(" + totheadval + ")";
                        dr["Grade"] = "Grade";
                        dt.Rows.Add(dr);

                        double totalmark = 0;
                        string partNo = string.Empty;
                        string testtxt = string.Empty;
                        totalmark = 0;
                        partNo = string.Empty;
                        testtxt = string.Empty;
                        int l = 0;

                        if (tstcode1.Contains(','))
                        {
                            for (int f = 0; f < sptval.Length; f++)
                            {
                                string val = Convert.ToString(sptval[f]);
                                dtQsettings.DefaultView.RowFilter = "CourseoutCome='" + coname1 + "'  and criteria_no='" + val + "' ";
                                DataTable dvipart = dtQsettings.DefaultView.ToTable();
                                if (dvipart.Rows.Count > 0)
                                {
                                    l++;
                                    dicQSub.DefaultView.RowFilter = "CourseoutCome='" + coname1 + "'  and criteria_no='" + val + "'";
                                    DataTable dicP = dicQSub.DefaultView.ToTable();
                                    object sum = dtQsettings.Compute("Sum(Mark)", "CourseoutCome='" + coname1 + "' and criteria_no='" + val + "'");

                                    totalmark = (Convert.ToDouble(sum) / dicP.Rows.Count);
                                    dicval.Add(val, Convert.ToString(totalmark));
                                }
                            }

                        }
                        else
                        {
                            string val = Convert.ToString(tstcode);
                            dtQsettings.DefaultView.RowFilter = "CourseoutCome='" + coname1 + "'  and criteria_no='" + val + "' ";
                            DataTable dvipart = dtQsettings.DefaultView.ToTable();
                            if (dvipart.Rows.Count > 0)
                            {
                                l++;
                                dicQSub.DefaultView.RowFilter = "CourseoutCome='" + coname1 + "'  and criteria_no='" + val + "'";
                                DataTable dicP = dicQSub.DefaultView.ToTable();
                                object sum = dtQsettings.Compute("Sum(Mark)", "CourseoutCome='" + coname1 + "' and criteria_no='" + val + "'");

                                totalmark = (Convert.ToDouble(sum) / dicP.Rows.Count);
                                dicval.Add(val, Convert.ToString(totalmark));
                            }
                        }

                        double thtotwg = 0;
                        double totthdattainmk = 0;
                        int thresholdattcount = 0;
                        double totmk = 0;
                        double totweightage = 0;
                        double totattain = 0;
                        double wg1 = 0;
                        double finalval = 0;
                        string studct = Convert.ToString(dsst.Tables[0].Rows.Count);

                        if (dstd.Tables.Count > 0 & dstd.Tables[0].Rows.Count > 0)
                        {
                            double totcrit1 = 0;
                            int sno = 0;
                            double per = 0;
                            for (int std = 0; std < dsst.Tables[0].Rows.Count; std++)
                            {
                                dr = dt.NewRow();
                                sno++;
                                string appno = Convert.ToString(dsst.Tables[0].Rows[std]["app_no"]);
                                string RollNo = Convert.ToString(dsst.Tables[0].Rows[std]["Roll_No"]);
                                string RegNo = Convert.ToString(dsst.Tables[0].Rows[std]["Reg_No"]);
                                string Name = Convert.ToString(dsst.Tables[0].Rows[std]["Stud_Name"]);
                                dr["S.No"] = sno.ToString();
                                dr["Roll No"] = RollNo;
                                dr["Reg No"] = RegNo;
                                dr["Student Name"] = Name;
                                totmk = 0;
                                totcrit1 = 0;
                                double totcrit2 = 0;
                                int perc1 = 0;
                                for (int j = 0; j < cbltest.Items.Count; j++)
                                {

                                    totcrit1 = 0;
                                    double wgtot = 0;
                                    if (cbltest.Items[j].Selected == true)
                                    {

                                        bool totctflag = false;
                                        dsst.Tables[1].DefaultView.RowFilter = " criteria_no='" + cbltest.Items[j].Value + "'";
                                        DataView dv1 = dsst.Tables[1].DefaultView;
                                        if (dv1.Count > 0)
                                        {
                                            weightpercent = Convert.ToString(dv1[0]["weightage_percentage"]);
                                        }
                                        else
                                        {
                                            div4.Visible = true;
                                            div5.Visible = true;
                                            Label4.Visible = true;
                                            Label4.Text = "Please Set Weightage Settings For The Selected Test";
                                            return;
                                        }
                                        bool sptflag = false;
                                        double sumVal = 0;
                                        per = 0;
                                        string val1 = cbltest.Items[j].Value.ToString();
                                        string tstname1 = cbltest.Items[j].Text.ToString();
                                        string[] sptval1 = null;
                                        if (val1.Contains(','))
                                        {
                                            sptval1 = val1.Split(',');
                                            sptflag = true;
                                        }
                                        string critno = string.Empty;
                                        object sum = new object();
                                        int perct = 0;
                                        bool finalflag = false;
                                        if (sptflag == true)
                                        {
                                            for (int z = 0; z < sptval1.Length; z++)
                                            {
                                                perct++;
                                                critno = Convert.ToString(sptval1[z]);
                                                sum = dstd.Tables[0].Compute("Sum(Mark)", "CourseoutCome='" + coname1 + "' and criteria_no='" + critno + "' and app_no='" + appno + "'");
                                                string sumv = Convert.ToString(sum);
                                                if (!string.IsNullOrEmpty(sumv))
                                                {
                                                    finalflag = true;
                                                    totctflag = true;
                                                    double.TryParse(Convert.ToString(sum), out sumVal);
                                                    double maxtot = 0;
                                                    foreach (KeyValuePair<string, string> val2 in dicval)
                                                    {
                                                        if (val2.Key.Trim() == critno.Trim())
                                                        {
                                                            maxtot = Convert.ToDouble(val2.Value);
                                                            break;
                                                        }
                                                    }
                                                    per = (sumVal / maxtot);
                                                    per = per * 100;
                                                    totcrit1 += per;
                                                }
                                            }
                                            perct = perct * 100;
                                            totcrit1 = totcrit1 / perct;
                                            totcrit1 = totcrit1 * 100;
                                            totcrit1 = Math.Round(totcrit1, 0, MidpointRounding.AwayFromZero);

                                        }
                                        else
                                        {
                                            critno = val1;
                                            sum = dstd.Tables[0].Compute("Sum(Mark)", "CourseoutCome='" + coname1 + "' and criteria_no='" + critno + "' and app_no='" + appno + "'");
                                            string sumv = Convert.ToString(sum);
                                            if (!string.IsNullOrEmpty(sumv))
                                            {
                                                finalflag = true;
                                                totctflag = true;
                                                double.TryParse(Convert.ToString(sum), out sumVal);
                                                double maxtot = 0;
                                                foreach (KeyValuePair<string, string> val2 in dicval)
                                                {
                                                    if (val2.Key.Trim() == critno.Trim())
                                                    {
                                                        maxtot = Convert.ToDouble(val2.Value);
                                                        break;
                                                    }
                                                }
                                                per = (sumVal / maxtot);
                                                per = per * 100;
                                                totcrit1 += per;
                                                totcrit1 = Math.Round(totcrit1, 0, MidpointRounding.AwayFromZero);
                                            }
                                        }
                                        if (totctflag == true)
                                            perc1++;
                                        if (finalflag == true)
                                        {
                                            dr["" + tstname1 + ""] = Convert.ToString(totcrit1);


                                            double tot = 0;

                                            double.TryParse(weightpercent, out wgtot);

                                            tot = totcrit1 / 100;
                                            tot = tot * wgtot;

                                            tot = Math.Round(tot, 0, MidpointRounding.AwayFromZero);

                                            totmk += tot;

                                            wg1 += wgtot;
                                            finalval += totcrit1;
                                        }
                                        else
                                            dr["" + tstname1 + ""] = "-";


                                    }
                                }
                                perc1 = perc1 * 100;
                                finalval = finalval / perc1;
                                finalval = finalval * wg1;
                                finalval = Math.Round(finalval, 0, MidpointRounding.AwayFromZero);
                                perc1 = 0;
                                dr["Total"] = Convert.ToString(totmk);
                                string qry6 = "if exists(select * from coattainmentmarks where app_no='" + appno + "' and cono='" + coname + "' and subject_no='" + subcode + "' and mark='" + totmk + "')update coattainmentmarks set mark='" + totmk + "'  where app_no='" + appno + "' and cono='" + coname + "' and subject_no='" + subcode + "' else insert into coattainmentmarks(app_no,cono,subject_no,mark) values('" + appno + "','" + coname + "','" + subcode + "','" + totmk + "')";
                                int inst = d2.update_method_wo_parameter(qry6, "text");
                                finalval = 0;
                                wg1 = 0;
                                //double percent1 = totmk / totheadval;
                                //percent1 = percent1 * 100;
                                //percent1 = Math.Round(percent1, 0, MidpointRounding.AwayFromZero);

                                DataView dv3 = new DataView();
                                if (dsst.Tables[3].Rows.Count > 0)
                                {
                                    dsst.Tables[3].DefaultView.RowFilter = "  Frange <= '" + totmk + "' and Trange>='" + totmk + "'";
                                    dv3 = dsst.Tables[3].DefaultView;
                                }
                                else if (dsst.Tables[4].Rows.Count > 0)
                                {
                                    dsst.Tables[4].DefaultView.RowFilter = "  Frange <= '" + totmk + "' and Trange>='" + totmk + "'";
                                    dv3 = dsst.Tables[4].DefaultView;
                                }

                                if (dv3.Count > 0)
                                {
                                    dr["Grade"] = Convert.ToString(dv3[0]["Mark_Grade"]);
                                    int val1 = 0;
                                    if (!dicstud.ContainsKey(Convert.ToString(dv3[0]["Mark_Grade"])))
                                    {
                                        val1 = 1;
                                        dicstud.Add(Convert.ToString(dv3[0]["Mark_Grade"]), val1);
                                    }
                                    else
                                    {
                                        string sval = Convert.ToString(dicstud[Convert.ToString(dv3[0]["Mark_Grade"])]);
                                        int mval = 0;
                                        int.TryParse(sval, out mval);
                                        mval = mval + 1;
                                        dicstud[Convert.ToString(dv3[0]["Mark_Grade"])] = mval;
                                    }
                                }


                                if (totmk >= threshold)
                                {
                                    //totthdattainmk += percent1;
                                    //thresholdattcount++;
                                    //thtotwg += 100;

                                    totthdattainmk += totmk;
                                    thresholdattcount++;
                                    thtotwg += totheadval;

                                }

                                totattain += totmk;
                                totweightage += totheadval;
                                dt.Rows.Add(dr);
                            }
                            double.TryParse(studct, out studcount);
                            double avgtotattain = totattain / totweightage;
                            avgtotattain = avgtotattain * 100;
                            avgtotattain = Math.Round(avgtotattain, 0, MidpointRounding.AwayFromZero);


                            double relativeattain = 0;
                            double attbasedpercent = 0;
                            if (dsst.Tables[2].Rows.Count > 0)
                            {
                                attbasedpercent = totthdattainmk / thtotwg;
                                attbasedpercent = attbasedpercent * 100;
                                attbasedpercent = Math.Round(attbasedpercent, 0, MidpointRounding.AwayFromZero);

                                relativeattain = avgtotattain / target;
                                relativeattain = relativeattain * 100;
                                relativeattain = Math.Round(relativeattain, 0, MidpointRounding.AwayFromZero);
                            }

                            dr = dt.NewRow();
                            dr["S.No"] = "Average Attainment";
                            dr["Total"] = Convert.ToString(avgtotattain);
                            dt.Rows.Add(dr);
                            if (dsst.Tables[2].Rows.Count > 0)
                            {
                                dr = dt.NewRow();
                                dr["S.No"] = "No. of students attained Thershold ";
                                dr["Total"] = Convert.ToString(thresholdattcount);
                                dt.Rows.Add(dr);
                                dr = dt.NewRow();
                                dr["S.No"] = "Attainment % based on Threshold";
                                if (thresholdattcount == 0)
                                    dr["Total"] = "-";
                                else
                                    dr["Total"] = Convert.ToString(attbasedpercent);
                                dt.Rows.Add(dr);
                                dr = dt.NewRow();
                                dr["S.No"] = "Relative attainment % based on Avg Attainment";
                                dr["Total"] = Convert.ToString(relativeattain);
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                dr = dt.NewRow();
                                dr["S.No"] = "No. of students attained Thershold ";
                                dr["Total"] = "-";
                                dt.Rows.Add(dr);
                                dr = dt.NewRow();
                                dr["S.No"] = "Attainment % based on Threshold";
                                dr["Total"] = "-";
                                dt.Rows.Add(dr);
                                dr = dt.NewRow();
                                dr["S.No"] = "Relative attainment % based on Avg Attainment";
                                dr["Total"] = "-";
                                dt.Rows.Add(dr);
                            }
                            if (dicstud.Count > 0)
                            {
                                string qry3 = "select distinct Mark_Grade,Frange from grade_master_CamInternal  where Degree_Code='" + ddlbranch.SelectedValue.ToString() + "' and College_Code='" + Convert.ToString(ddlcollege.SelectedValue) + "' and batch_year='" + Convert.ToString(ddlbatch.SelectedItem.Text) + "' order by Frange desc";
                                DataSet dss1 = d2.select_method_wo_parameter(qry3, "text");
                                if (dss1.Tables.Count > 0 && dss1.Tables[0].Rows.Count > 0)
                                {
                                    for (int u = 0; u < dss1.Tables[0].Rows.Count; u++)
                                    {
                                        dr1 = dt1.NewRow();

                                        string gd2 = Convert.ToString(dss1.Tables[0].Rows[u]["Mark_Grade"]);
                                        dr1["Grade"] = Convert.ToString(gd2);
                                        if (dicstud.ContainsKey(gd2))
                                        {
                                            string sval = Convert.ToString(dicstud[Convert.ToString(dss1.Tables[0].Rows[u]["Mark_Grade"])]);
                                            dr1["studentcount"] = sval;
                                        }
                                        else
                                            dr1["studentcount"] = "0";
                                        dt1.Rows.Add(dr1);
                                    }
                                }
                                Chart1.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                                Chart1.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;
                                Chart1.DataSource = dt1;
                                Chart1.Series[0].ChartType = SeriesChartType.Line;
                                Chart1.Series[0].BorderWidth = 1;
                                Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = true;
                                Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = true;
                                Chart1.Font.Bold = true;
                                Series vd = new Series();
                                Chart1.Series[0].XValueMember = "Grade";
                                Chart1.Series[0].YValueMembers = "StudentCount";
                                Chart1.DataBind();
                                GridViewchart.Visible = true;
                                GridViewchart.DataSource = dt1;
                                GridViewchart.DataBind();
                                GridViewchart.Rows[0].Font.Bold = true;
                            }

                            GridView1.DataSource = dt;
                            GridView1.DataBind();
                            GridView1.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");

                            Chart1.Visible = true;
                            btnxl.Visible = true;
                            btnmasterprint.Visible = true;
                            txtexcelname.Visible = true;
                            lblrptname.Visible = true;
                            GridView1.Visible = true;
                        }

                    }
                    else
                    {
                        // printtable.Visible = false;
                        btnxl.Visible = false;
                        btnmasterprint.Visible = false;
                        txtexcelname.Visible = false;
                        lblrptname.Visible = false;
                        GridView1.Visible = false;
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);

                    }
                }
                else
                {
                    // printtable.Visible = false;
                    btnxl.Visible = false;
                    btnmasterprint.Visible = false;
                    txtexcelname.Visible = false;
                    lblrptname.Visible = false;
                    GridView1.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Select All The Feild')", true);
                    return;
                }
            }
            else
            {
                btnxl.Visible = false;
                btnmasterprint.Visible = false;
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                GridView1.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Students')", true);
                return;
            }

        }
        catch
        {
        }
    }
    protected void ddltest_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        btnxl.Visible = false;
        btnmasterprint.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        GridView1.Visible = false;
        Chart1.Visible = false;
        GridViewchart.Visible = false;
    }
    protected void ddl_subject_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Bindtest();
        btnxl.Visible = false;
        btnmasterprint.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        GridView1.Visible = false;
        Chart1.Visible = false;
        GridViewchart.Visible = false;
    }
    protected void ddlconame_OnselectedIndexedChanged(object sender, EventArgs e)
    {
        btnxl.Visible = false;
        btnmasterprint.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        GridView1.Visible = false;
        Chart1.Visible = false;
        GridViewchart.Visible = false;
        bindSubject();
        Bindtest();
    }
    //protected void ddlsection_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    btnxl.Visible = false;
    //    btnmasterprint.Visible = false;
    //    txtexcelname.Visible = false;
    //    lblrptname.Visible = false;
    //    GridView1.Visible = false;
    //    BindCourseoutcome();
    //    bindSubject();
    //    Bindtest();
    //}
    protected void ddlsemester_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnxl.Visible = false;
        btnmasterprint.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        GridView1.Visible = false;
        Chart1.Visible = false;
        GridViewchart.Visible = false;
        // BindSectionDetail();
        BindCourseoutcome();
        bindSubject();
        Bindtest();
    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnxl.Visible = false;
        btnmasterprint.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        GridView1.Visible = false;
        Chart1.Visible = false;
        GridViewchart.Visible = false;
        bindsem();
        // BindSectionDetail();
        BindCourseoutcome();
        bindSubject();
        Bindtest();
    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnxl.Visible = false;
        btnmasterprint.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        GridView1.Visible = false;
        Chart1.Visible = false;
        GridViewchart.Visible = false;
        bindbranch();
        bindsem();
        //  BindSectionDetail();
        BindCourseoutcome();
        bindSubject();
        Bindtest();
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnxl.Visible = false;
        btnmasterprint.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        GridView1.Visible = false;
        Chart1.Visible = false;
        GridViewchart.Visible = false;
        binddegree();
        bindbranch();
        bindsem();
        // BindSectionDetail();
        BindCourseoutcome();
        bindSubject();
        Bindtest();
    }
    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnxl.Visible = false;
        btnmasterprint.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        GridView1.Visible = false;
        Chart1.Visible = false;
        GridViewchart.Visible = false;
        bindbatch();
        binddegree();
        bindbranch();
        bindsem();
        // BindSectionDetail();
        BindCourseoutcome();
        bindSubject();
        Bindtest();
    }

    protected void btnaltok_Click(object sender, EventArgs e)
    {
        div4.Visible = false;
        div5.Visible = false;
    }

    protected void GridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex == 0)
                {
                    e.Row.Cells[0].Font.Bold = true;
                    e.Row.Cells[1].Font.Bold = true;
                    e.Row.Cells[2].Font.Bold = true;
                    e.Row.Cells[3].Font.Bold = true;
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[0].Width = 30;
                    e.Row.Cells[1].Width = 150;
                    e.Row.Cells[2].Width = 150;
                    e.Row.Cells[3].Width = 200;
                    int h1 = 3;
                    for (int h = 0; h < k; h++)
                    {
                        h1++;
                        e.Row.Cells[h1].Font.Bold = true;
                        e.Row.Cells[h1].HorizontalAlign = HorizontalAlign.Center;
                        e.Row.Cells[h1].Width = 100;
                    }
                    e.Row.Cells[h1 + 1].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[h1 + 1].Font.Bold = true;
                    e.Row.Cells[h1 + 1].Width = 100;
                    e.Row.Cells[h1 + 2].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[h1 + 2].Font.Bold = true;
                    e.Row.Cells[h1 + 2].Width = 100;

                }
                else
                {
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                    int h1 = 3;
                    for (int h = 0; h < k; h++)
                    {
                        h1++;
                        e.Row.Cells[h1].HorizontalAlign = HorizontalAlign.Center;
                        e.Row.Cells[h1].Width = 100;
                    }
                    e.Row.Cells[h1 + 1].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[h1 + 1].Width = 100;
                    e.Row.Cells[h1 + 2].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[h1 + 2].Width = 100;

                    if (e.Row.RowIndex > studcount)
                    {
                        int r = 4 + k;
                        //  int j = 1;
                        e.Row.Cells[0].ColumnSpan = r;

                        for (int j = 1; j < r; j++)
                        {
                            e.Row.Cells[j].Visible = false;
                        }
                        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                        e.Row.Cells[0].Font.Bold = true;
                        e.Row.Cells[r].Font.Bold = true;
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void GridViewchart_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
        }
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text.ToString().Trim();
            if (reportname != "")
            {
                d2.printexcelreportgrid(GridView1, reportname);
                lbl_err.Visible = false;
            }
            else
            {
                lbl_err.Text = "Please Enter Your Report Name";
                lbl_err.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    protected void btnmasterprint_Click(object sender, EventArgs e)
    {
        string ss = Convert.ToString(Session["usercode"]);
        GridView1.Visible = true;

        string degreedetails = string.Empty;
        degreedetails = "Course Outcome Attainment Report @" + ddlbatch.SelectedItem.Text + " " + ddldegree.SelectedItem.Text + " " + ddlbranch.SelectedItem.Text + "     Semester-" + ddlsemester.SelectedItem.Text.ToString() + " @Subject - " + ddl_subject.SelectedItem.Text.ToString();
        Printcontrol.loadspreaddetails(GridView1, "courseoutcomeattainment.aspx", degreedetails, 0, ss);
        Printcontrol.Visible = true;
    }

}