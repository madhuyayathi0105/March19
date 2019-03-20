using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using InsproDataAccess;
using System.Drawing;


public partial class CoBasedInternalCalculation : System.Web.UI.Page
{
    InsproDirectAccess dir = new InsproDirectAccess();
    InsproStoreAccess storAcc = new InsproStoreAccess();
    DataTable dtHeader = new DataTable();
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string usercode = string.Empty;
    string group_code = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    ReuasableMethods rs = new ReuasableMethods();
    Dictionary<string, string> dicParameter = new Dictionary<string, string>();
    protected void Page_Load(object sender, EventArgs e)
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
        if (!IsPostBack)
        {
            btnPrint.Visible = false;
            btnprint1.Visible = false;
            GridView1.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            bindclg();
            BindBatch();
            BindDegree();
            bindbranch();
            bindsem();
            GetSubject();
        }
    }
    public void bindclg()
    {
        try
        {
            string columnfield = string.Empty;
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
            dicParameter.Clear();
            dicParameter.Add("column_field", columnfield.ToString());
            //dtHeader.Dispose();
            //dtHeader.Clear();
            dtHeader = storAcc.selectDataTable("bind_college", dicParameter);

            ddlCollege.Items.Clear();

            if (dtHeader.Rows.Count > 0)
            {
                ddlCollege.DataSource = dtHeader;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
            }

        }
        catch
        {
        }
    }

    public void BindBatch()
    {
        dicParameter.Clear();
        ddlbatch.Items.Clear();
        //dtHeader.Dispose();
        //dtHeader.Clear();
        dtHeader = storAcc.selectDataTable("bind_batch", dicParameter);
        int count = dtHeader.Rows.Count;
        if (count > 0)
        {
            ddlbatch.DataSource = dtHeader;
            ddlbatch.DataTextField = "batch_year";
            ddlbatch.DataValueField = "batch_year";
            ddlbatch.DataBind();
        }
        int count1 = dtHeader.Rows.Count;
        if (count > 0)
        {
            int max_bat = 0;
            max_bat = Convert.ToInt32(dtHeader.Rows[0][0].ToString());
            ddlbatch.SelectedValue = max_bat.ToString();
        }
    }

    public void BindDegree()
    {

        ddldegree.Items.Clear();
        collegecode = Convert.ToString(ddlCollege.SelectedValue);
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        //dtHeader.Dispose();
        //dtHeader.Clear();
        string sql_s = "select distinct course_name,course_id from course where college_code='" + collegecode + "'";
        dtHeader = dir.selectDataTable(sql_s);
        if (dtHeader.Rows.Count > 0)
        {
            ddldegree.DataSource = dtHeader;
            ddldegree.DataTextField = "course_name";
            ddldegree.DataValueField = "course_id";
            ddldegree.DataBind();
        }
    }

    public void bindbranch()
    {
        dicParameter.Clear();
        ddlbranch.Items.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Convert.ToString(ddlCollege.SelectedValue); ;
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        //dicParameter.Add("single_user", singleuser);
        //dicParameter.Add("group_code", group_user);
        //dicParameter.Add("course_id", ddldegree.SelectedValue);
        //dicParameter.Add("college_code", collegecode);
        //dicParameter.Add("user_code", usercode);
        ds.Dispose();
        ds.Reset();
        ds = da.BindBranchMultiple(singleuser, group_user, ddldegree.SelectedValue.ToString(), collegecode, usercode);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlbranch.DataSource = ds;
            ddlbranch.DataTextField = "dept_name";
            ddlbranch.DataValueField = "degree_code";
            ddlbranch.DataBind();
        }

    }

    public void bindsem()
    {
        ddlsem.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;

        if (ddlbranch.SelectedValue.ToString() != string.Empty && ddlbatch.SelectedItem.ToString() != string.Empty)
        {

            string SelectQ = ("select distinct ndurations,first_year_nonsemester from ndegree where degree_code='" + ddlbranch.SelectedValue.ToString() + "' and batch_year='" + ddlbatch.SelectedItem.ToString() + "' and college_code='" + Convert.ToString(ddlCollege.SelectedValue) + "'");
            //dtHeader.Dispose();
            //dtHeader.Reset();
            dtHeader = dir.selectDataTable(SelectQ);
            if (dtHeader.Rows.Count > 0)
            {
                foreach (DataRow dr in dtHeader.Rows)
                {
                    first_year = Convert.ToBoolean(dr[1].ToString());
                    duration = Convert.ToInt16(dr[0].ToString());
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlsem.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlsem.Items.Add(i.ToString());
                        }

                    }
                }

            }
            else
            {
                string selectQ = "select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlbranch.Text.ToString() + " and college_code=" + Convert.ToString(ddlCollege.SelectedValue) + "";

                ddlsem.Items.Clear();
                dtHeader = dir.selectDataTable(selectQ);
                if (dtHeader.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtHeader.Rows)
                    {
                        first_year = Convert.ToBoolean(dr[1].ToString());
                        duration = Convert.ToInt16(dr[0].ToString());

                        for (i = 1; i <= duration; i++)
                        {
                            if (first_year == false)
                            {
                                ddlsem.Items.Add(i.ToString());
                            }
                            else if (first_year == true && i != 2)
                            {
                                ddlsem.Items.Add(i.ToString());
                            }
                        }
                    }
                }
            }
        }
    }

    public void GetSubject()
    {

        //ddlsubject.Visible = true;
        //ddlsubject.Enabled = true;
        try
        {
            //ddlsubject.Items.Clear();
            cblsubject.Items.Clear();
            string subjectquery = string.Empty;
            string sems = "";
            if (ddlsem.SelectedValue != "")
            {
                if (ddlsem.SelectedValue == "")
                {
                    sems = "";
                }
                else
                {
                    sems = "and SM.semester=" + ddlsem.SelectedValue.ToString() + "";
                }

                bool staffSelector = false;
                string minimumabsentsms = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "'");
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
                string logstaffcode = "";
                if (staffSelector == true && !string.IsNullOrEmpty(Convert.ToString(Session["Staff_Code"])))
                {
                    logstaffcode = " and sc.staffcode like '%" + Convert.ToString(Session["Staff_Code"]) + "%'";
                }
               
                //if (Convert.ToString(Session["Staff_Code"]) != "")
                //{
                //    logstaffcode = " and st.staff_code='" + Convert.ToString(Session["Staff_Code"]) + "'";
                //}
                //==================================//
                subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and  st.subject_no=s.subject_no  and SM.degree_code=" + ddlbranch.SelectedValue.ToString() + " " + sems.ToString() + " and  S.subtype_no = Sem.subtype_no and promote_count=1 and SM.batch_year='" + ddlbatch.SelectedValue.ToString() + "' " + logstaffcode + " order by S.subject_no ";//rajasekar 12/07/2018


                if (subjectquery != "")
                {
                    //dtHeader.Dispose();
                    //dtHeader.Reset();
                    dtHeader = dir.selectDataTable(subjectquery);
                    if (dtHeader.Rows.Count > 0)
                    {
                        //ddlsubject.Visible = true;
                        //ddlsubject.Enabled = true;
                        //ddlsubject.DataSource = dtHeader;
                        //ddlsubject.DataValueField = "Subject_No";
                        //ddlsubject.DataTextField = "Subject_Name";
                        //ddlsubject.DataBind();

                        cblsubject.Visible = true;
                        cblsubject.Enabled = true;
                        cblsubject.DataSource = dtHeader;
                        cblsubject.DataValueField = "Subject_No";
                        cblsubject.DataTextField = "Subject_Name";
                        cblsubject.DataBind();

                    }
                    else
                    {

                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBatch();
        BindDegree();
        bindbranch();
        bindsem();
        GetSubject();
        btnPrint.Visible = false;
        btnprint1.Visible = false;
        GridView1.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetSubject();
        btnPrint.Visible = false;
        btnprint1.Visible = false;
        GridView1.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;

    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
        bindsem();
        GetSubject();
        btnPrint.Visible = false;
        btnprint1.Visible = false;
        GridView1.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;

    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsem();
        GetSubject();
        btnPrint.Visible = false;
        btnprint1.Visible = false;
        GridView1.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetSubject();
    }

    protected void ddlsubject_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btnSaveSettings_OnClick(object sender, EventArgs e)
    {
        int msave=0;
        if (GridView1.Rows.Count > 3)
        {
            for (int i = 3; i < GridView1.Rows.Count; i++)
            {

                string rollno = GridView1.Rows[i].Cells[1].Text;
                if (rollno != "" && rollno!="&nbsp;")
                {
                    for (int j = 4; j < GridView1.Rows[2].Cells.Count; j++)
                    {
                        string colname = GridView1.Rows[2].Cells[j].Text;
                        string[] splt = colname.Split('-');
                        if (splt.Length == 2)
                        {
                            if (splt[1] == "Mark")
                            {
                                //  string rollno = GridView1.Rows[0].Cells[1].Text;
                                string subj = GridView1.Rows[2].Cells[j - 2].Text;
                                string total = GridView1.Rows[i].Cells[j].Text;
                                string[] spl = subj.Split('-');
                                string save = "if exists (select * from camarks where roll_no='" + rollno + "' and subject_no='" + spl[0] + "') update camarks set total='" + total + "' where roll_no='" + rollno + "' and subject_no='" + spl[0] + "' else insert into camarks (roll_no,subject_no,total) values('" + rollno + "','" + spl[0] + "','" + total + "')";
                                msave = da.update_method_wo_parameter(save, "text");


                            }
                        }
                    }
                }
                else
                {
                    i = GridView1.Rows.Count;
                }

            }
            if (msave>0)
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Save Sucessfully')", true);
        }

    }
    public string filterfunction()
    {
        string strorder = "ORDER BY Registration.Roll_No";
        string serialno = da.GetFunction("select LinkValue from inssettings where college_code=" + Convert.ToString(ddlCollege.SelectedValue) + " and linkname='Student Attendance'");
        if (serialno == "1")
        {
            strorder = "   order by registration.serialno";
        }
        else
        {
            string orderby_Setting = da.GetFunction("select value from master_Settings where settings='order_by'");
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

    protected void GO_Click(object sender, EventArgs e)
    {
        try
        {
            btnPrint.Visible = false;
            btnprint1.Visible = false;
            GridView1.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            string clgcode = Convert.ToString(ddlCollege.SelectedValue);
            string degcode = Convert.ToString(ddlbranch.SelectedValue);
            string batchyear = Convert.ToString(ddlbatch.SelectedValue);
            string semester = Convert.ToString(ddlsem.SelectedValue).Trim();
         //   string subNo = Convert.ToString(ddlsubject.SelectedValue).Trim();
            DataTable dtMark = new DataTable();
            DataTable dtStudent = new DataTable();
            DataTable dtWeight = new DataTable();
            DataTable dtCoNames = new DataTable();
            DataTable dtheadNames = new DataTable();
            string strstaffselecotr = string.Empty;
            string staffCode = Convert.ToString(Session["staff_code"]);
            bool staffSelector = false;
            string minimumabsentsms = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "'");
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
            string strorder = filterfunction();

            string subjectno = string.Empty;
            if (cblsubject.Items.Count > 0)
            {
                for (int m = 0; m < cblsubject.Items.Count; m++)
                {
                    if (cblsubject.Items[m].Selected == true)
                    {
                        if (subjectno == "")
                            subjectno = "" + Convert.ToString(cblsubject.Items[m].Value) + "";
                        else
                            subjectno = subjectno + "'" + "," + "'" + Convert.ToString(cblsubject.Items[m].Value) + "";
                        
                    
                    }
                }
            }
            if (!string.IsNullOrEmpty(clgcode) && !string.IsNullOrEmpty(degcode) && !string.IsNullOrEmpty(batchyear) && !string.IsNullOrEmpty(semester) && !string.IsNullOrEmpty(subjectno))
            {
                // string stddet = "select  r.Roll_No,r.Reg_No,r.Roll_Admit,r.Stud_Name,r.App_no,r.Sections   from Registration r where r.batch_year='" + batchyear + "' and r.degree_code='" + degcode + "' and r.Current_Semester='" + semester + "'  and  RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR'  order by r.reg_no,r.roll_no;";

                string stddet = "Select  distinct registration.roll_no,registration.reg_no,registration.stud_name,registration.sections,registration.App_No,Registration.Sections,Roll_Admit from registration ,SubjectChooser where  registration.roll_no = subjectchooser.roll_no and registration.Degree_Code ='" + Convert.ToString(degcode) + "' and Semester = '" + Convert.ToString(semester) + "' and registration.Batch_Year = '" + Convert.ToString(batchyear) + "' and Subject_No in( '" + subjectno + "') and RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR' and Semester = '" + Convert.ToString(semester) + "'   " + strstaffselecotr + "  " + strorder + "";

                string strwei = "select criteria_no,criterianame,coname,cono,weightage_percentage,subject_No  from weightage_setting where  batch='" + batchyear + "' and degree_code='" + degcode + "' and semester='" + semester + "'   and subject_no  in( '" + subjectno + "')";

                string dicCo = "select distinct template,masterno from CAQuesSettingsParent c,Master_Settings m where settings='COSettings' and m.masterno=c.CourseOutComeNo and c.subjectNo  in( '" + subjectno + "') order by template";

                string stuMark = "select SUM(im.marks) as stumark,SUM(ca.Mark) as totMark,subjectno, examcode,app_no,ca.CourseOutComeNo,(select isnull(template,'') from Master_Settings where settings='COSettings' and masterno=ca.CourseOutComeNo) as CourseoutCome,c.criteria,c.criteria_no from CAQuesSettingsParent ca,NewInternalMarkEntry im,criteriaforInternal c where ca.MasterID=im.MasterID and subjectno in('" + subjectno + "')  and (marks<>-1 and marks<>-16 and marks<>-20) and c.criteria_no=ca.criteriano  group by examcode,app_no,subjectno,ca.CourseOutComeNo,ca.PartNo,c.criteria,c.criteria_no,ca.Mark";

                dtStudent = dir.selectDataTable(stddet);
                dtWeight = dir.selectDataTable(strwei);
                dtCoNames = dir.selectDataTable(dicCo);
                dtMark = dir.selectDataTable(stuMark);

                DataTable dtReport = new DataTable();
                 dtReport.Columns.Add("S.No");
                dtReport.Columns.Add("Reg No");
                dtReport.Columns.Add("Roll No");
                dtReport.Columns.Add("StudName");
            if (cblsubject.Items.Count > 0)
            {
                for (int m = 0; m < cblsubject.Items.Count; m++)
                {
                    if (cblsubject.Items[m].Selected == true)
                    {
                string dicColum = "select distinct template,masterno from CAQuesSettingsParent c,Master_Settings m where settings='COSettings' and m.masterno=c.CourseOutComeNo and c.subjectNo  in( '" + Convert.ToString(cblsubject.Items[m].Value) + "') order by template";
                dtheadNames = dir.selectDataTable(dicColum);
                   if(dtheadNames.Rows.Count>0)
                   {
                        foreach (DataRow drCo in dtheadNames.Rows)
                    {
                        string coName = Convert.ToString(drCo["template"]);
                        dtReport.Columns.Add(Convert.ToString(cblsubject.Items[m].Value)+'-'+coName.Trim().ToUpper());
                    }
                   }
                   dtReport.Columns.Add(Convert.ToString(cblsubject.Items[m].Value)+'-'+"Average");
                   dtReport.Columns.Add(Convert.ToString(cblsubject.Items[m].Value)+'-'+"Mark");
                    }
                    
                }
            }
                //if (dtCoNames.Rows.Count > 0)
                //{
                //    foreach (DataRow drCo in dtCoNames.Rows)
                //    {
                //        string coName = Convert.ToString(drCo["template"]);
                //        dtReport.Columns.Add(coName.Trim().ToUpper());
                //    }
                //}
               
                DataRow drm;


              


                drm = dtReport.NewRow();
             drm["S.No"]="S.No";
                drm["Reg No"] = "Reg No";
                drm["Roll No"] = "Roll No";
                drm["StudName"] = "StudName";

                if (cblsubject.Items.Count > 0)
                {
                    for (int m = 0; m < cblsubject.Items.Count; m++)
                    {
                        if (cblsubject.Items[m].Selected == true)
                        {

                            string subjcode = da.GetFunction("select subject_code from subject where subject_no='" + Convert.ToString(cblsubject.Items[m].Value) + "'");
                            string dicColum = "select distinct template,masterno from CAQuesSettingsParent c,Master_Settings m where settings='COSettings' and m.masterno=c.CourseOutComeNo and c.subjectNo  in( '" + Convert.ToString(cblsubject.Items[m].Value) + "') order by template";
                            dtheadNames = dir.selectDataTable(dicColum);
                            if (dtheadNames.Rows.Count > 0)
                            {
                                foreach (DataRow drCo in dtheadNames.Rows)
                                {
                                    string coName = Convert.ToString(drCo["template"]);
                                    drm[Convert.ToString(cblsubject.Items[m].Value) + '-' + coName.Trim().ToUpper()] = subjcode + '-' + Convert.ToString(cblsubject.Items[m].Text);
                                }
                            }
                            drm[Convert.ToString(cblsubject.Items[m].Value) + '-' + "Average"] = subjcode +'-'+ "Average";
                            drm[Convert.ToString(cblsubject.Items[m].Value) + '-' + "Mark"] = subjcode + '-' + "Mark";
                        }

                    }
                }
               
                dtReport.Rows.Add(drm);
              
                drm = dtReport.NewRow();
                 drm["S.No"]="S.No";
                drm["Reg No"] = "Reg No";
                drm["Roll No"] = "Roll No";
                drm["StudName"] = "StudName";

                if (cblsubject.Items.Count > 0)
                {
                    for (int m = 0; m < cblsubject.Items.Count; m++)
                    {
                        if (cblsubject.Items[m].Selected == true)
                        {
                            string subjcode = da.GetFunction("select subject_code from subject where subject_no='" + Convert.ToString(cblsubject.Items[m].Value) + "'");
                            string dicColum = "select distinct template,masterno from CAQuesSettingsParent c,Master_Settings m where settings='COSettings' and m.masterno=c.CourseOutComeNo and c.subjectNo  in( '" + Convert.ToString(cblsubject.Items[m].Value) + "') order by template";
                            dtheadNames = dir.selectDataTable(dicColum);
                            if (dtheadNames.Rows.Count > 0)
                            {
                                foreach (DataRow drCo in dtheadNames.Rows)
                                {
                                    string coName = Convert.ToString(drCo["template"]);
                                    drm[Convert.ToString(cblsubject.Items[m].Value) + '-' + coName.Trim().ToUpper()] = coName.Trim().ToUpper();
                                }
                            }
                            drm[Convert.ToString(cblsubject.Items[m].Value)+'-'+"Average"] = subjcode+'-'+"Average";
                            drm[Convert.ToString(cblsubject.Items[m].Value)+'-'+"Mark"] =subjcode+'-'+ "Mark";
                        }

                    }
                }
               
                dtReport.Rows.Add(drm);

                drm = dtReport.NewRow();
                drm["S.No"] = "S.No";
                drm["Reg No"] = "Reg No";
                drm["Roll No"] = "Roll No";
                drm["StudName"] = "StudName";

                if (cblsubject.Items.Count > 0)
                {
                    for (int m = 0; m < cblsubject.Items.Count; m++)
                    {
                        if (cblsubject.Items[m].Selected == true)
                        {
                            string dicColum = "select distinct template,masterno from CAQuesSettingsParent c,Master_Settings m where settings='COSettings' and m.masterno=c.CourseOutComeNo and c.subjectNo  in( '" + Convert.ToString(cblsubject.Items[m].Value) + "') order by template";
                            string subjcode = da.GetFunction("select subject_code from subject where subject_no='" + Convert.ToString(cblsubject.Items[m].Value) + "'");
                            dtheadNames = dir.selectDataTable(dicColum);
                            if (dtheadNames.Rows.Count > 0)
                            {
                                foreach (DataRow drCo in dtheadNames.Rows)
                                {
                                    string coName = Convert.ToString(drCo["template"]);
                                    drm[Convert.ToString(cblsubject.Items[m].Value) + '-' + coName.Trim().ToUpper()] = Convert.ToString(cblsubject.Items[m].Value) + '-' + Convert.ToString(cblsubject.Items[m].Text);
                                }
                            }
                            drm[Convert.ToString(cblsubject.Items[m].Value) + '-' + "Average"] =subjcode+'-'+ "Average";
                            drm[Convert.ToString(cblsubject.Items[m].Value) + '-' + "Mark"] = subjcode + '-' + "Mark";
                        }

                    }
                }

                dtReport.Rows.Add(drm);

                DataRow drNew = null;
                double d = 0;
                if (!string.IsNullOrEmpty(txtConvertVal.Text))
                    double.TryParse(txtConvertVal.Text, out d);
                else
                    d = 100;
                int cun=0;
                if (dtStudent.Rows.Count > 0)
                {
                    foreach (DataRow drStu in dtStudent.Rows)
                    {
                        string rollNo = Convert.ToString(drStu["Roll_No"]);
                        string Reg_No = Convert.ToString(drStu["Reg_No"]);
                        string Roll_Admit = Convert.ToString(drStu["Roll_Admit"]);
                        string Stud_Name = Convert.ToString(drStu["Stud_Name"]);
                        string App_no = Convert.ToString(drStu["App_no"]);
                        string Sections = Convert.ToString(drStu["Sections"]);
                        cun++;
                        drNew = dtReport.NewRow();
                        drNew["S.No"] = cun;
                        drNew["Reg No"] = Reg_No;
                        drNew["Roll No"] = rollNo;
                        drNew["StudName"] = Stud_Name;
                        double totPer = 0;
                        double per100 = 0;

                        bool fnvalflag = false;
                        if (dtReport.Columns.Count > 0)
                        {
                            int perCount = 0;
                            for (int col = 4; col < dtReport.Columns.Count; col++)
                            {
                                
                                //  fnvalflag = false;
                                string colName = Convert.ToString(dtReport.Columns[col]);

                                string[] spl = colName.Split('-');
                                if (spl[1].Trim() != "Average" && spl[1].Trim() != "Mark")
                                {
                                    //  perCount++;
                                    bool mkflag = false;
                                    double commonWei = 0;



                                    if (dtWeight.Rows.Count > 0)
                                    {
                                        dtWeight.DefaultView.RowFilter = "coname='" + spl[1] + "' and subject_no='" + spl[0] + "'";
                                        DataTable dtDicCo = dtWeight.DefaultView.ToTable();
                                        if (dtDicCo.Rows.Count > 0)
                                        {
                                            foreach (DataRow drW in dtDicCo.Rows)
                                            {

                                                double singleWei = 0;
                                                string SubNo = Convert.ToString(drW["subject_no"]);
                                                string CriNo = Convert.ToString(drW["criteria_no"]);
                                                string strWeight = Convert.ToString(drW["weightage_percentage"]);
                                                string CoNo = Convert.ToString(drW["coname"]);
                                                double weight = 0;
                                                double.TryParse(Convert.ToString(strWeight), out weight);
                                                double wei = 0;
                                                double sumwei = 0;
                                                if (dtMark.Rows.Count > 0)
                                                {
                                                    int cc = 0;
                                                    if (CriNo.Contains(','))
                                                    {
                                                        string[] sptc = CriNo.Split(',');
                                                        for (int j = 0; j < sptc.Length; j++)
                                                        {
                                                            cc = cc + 1;
                                                            string cNo = Convert.ToString(sptc[j]);
                                                            dtMark.DefaultView.RowFilter = "CourseoutCome='" + spl[1] + "' and subjectno='" + spl[0] + "' and criteria_no in(" + cNo + ") and app_no='" + App_no + "'";
                                                            DataTable dtStuMark = dtMark.DefaultView.ToTable();
                                                            if (dtStuMark.Rows.Count > 0)
                                                            {

                                                                mkflag = true;
                                                                double sst = 0;
                                                                double stTot = 0;
                                                                foreach (DataRow drs in dtStuMark.Rows)
                                                                {
                                                                    double sums = 0;
                                                                    double sumt = 0;
                                                                    string SMark = Convert.ToString(drs["stumark"]);
                                                                    string tMark = Convert.ToString(drs["totMark"]);
                                                                    double.TryParse(SMark, out sums);
                                                                    double.TryParse(tMark, out sumt);
                                                                    sst = sst + sums;
                                                                    stTot = stTot + sumt;
                                                                }
                                                                double totCal = (sst / stTot);
                                                                wei = totCal * 100;
                                                                wei = Math.Round(wei, 0, MidpointRounding.AwayFromZero);
                                                                sumwei = sumwei + wei;
                                                                //cc = cc + 1;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {

                                                        dtMark.DefaultView.RowFilter = "CourseoutCome='" + spl[1] + "' and subjectno='" + spl[0] + "' and criteria_no in(" + CriNo + ") and app_no='" + App_no + "'";
                                                        DataTable dtStuMark = dtMark.DefaultView.ToTable();
                                                        cc = cc + 1;
                                                        if (dtStuMark.Rows.Count > 0)
                                                        {
                                                            // fnvalflag = true;
                                                            mkflag = true;
                                                            double sst = 0;
                                                            double stTot = 0;
                                                            foreach (DataRow drs in dtStuMark.Rows)
                                                            {
                                                                double sums = 0;
                                                                double sumt = 0;
                                                                string SMark = Convert.ToString(drs["stumark"]);
                                                                string tMark = Convert.ToString(drs["totMark"]);
                                                                double.TryParse(SMark, out sums);
                                                                double.TryParse(tMark, out sumt);
                                                                sst = sst + sums;
                                                                stTot = stTot + sumt;
                                                            }
                                                            double totCal = (sst / stTot);
                                                            wei = totCal * 100;
                                                            wei = Math.Round(wei, 0, MidpointRounding.AwayFromZero);
                                                            sumwei = sumwei + wei;
                                                        }
                                                    }
                                                    if (sumwei > 0)
                                                    {

                                                        singleWei = sumwei / (cc * 100);
                                                        singleWei = singleWei * 100;
                                                        singleWei = Math.Round(singleWei, 0, MidpointRounding.AwayFromZero);
                                                        totPer = totPer + singleWei;
                                                        singleWei = singleWei / 100;
                                                        singleWei = singleWei * weight;
                                                        singleWei = Math.Round(singleWei, 0, MidpointRounding.AwayFromZero);
                                                        commonWei = commonWei + singleWei;
                                                    }
                                                    if (mkflag == true)
                                                        perCount++;
                                                }
                                            }
                                        }
                                        drNew[colName] = Convert.ToString(commonWei);
                                        
                                    }
                                }
                                else
                                {
                                    if (spl[1].Trim() == "Average")
                                    {
                                        if (totPer == 0)
                                            drNew[colName] = "0";
                                        else
                                        {
                                            per100 = totPer / perCount;
                                            per100 = Math.Round(per100, 0, MidpointRounding.AwayFromZero);
                                            drNew[colName] = Convert.ToString(per100);
                                        }
                                    }
                                    if (spl[1].Trim() == "Mark")
                                    {
                                        if (totPer == 0)
                                            drNew[colName] = "0";
                                        else
                                        {
                                            per100 = totPer / perCount;
                                            per100 = totPer / (perCount * 100);
                                            per100 = per100 * d;
                                            per100 = Math.Round(per100, 0, MidpointRounding.AwayFromZero);
                                            drNew[colName] = Convert.ToString(per100);
                                            drNew[colName] = Convert.ToString(per100);
                                        }
                                         totPer = 0;
                                         per100 = 0;
                                         perCount = 0;
                                    }
                                }
                            }


                            dtReport.Rows.Add(drNew);
                        }
                    }
                    drm = dtReport.NewRow();
                    dtReport.Rows.Add(drm);
                    drm = dtReport.NewRow();
                    drm["Reg No"] = "Strong CO ";
                    drm["Roll No"] = "Moderate CO";
                    drm["StudName"] = "Weak CO";
                    dtReport.Rows.Add(drm);
                    string posetting = "select MasterValue,MasterCode from CO_MasterValues where MasterCriteria='posettings' and CollegeCode='" + ddlCollege.SelectedValue + "'";
                    DataTable dtposetting = dir.selectDataTable(posetting);
                    if (dtposetting.Rows.Count > 0)
                    {
                        for (int m = 0; m < dtposetting.Rows.Count; m++)
                        {
                            Hashtable settotal = new Hashtable();
                            int cunt=0;
                            drm = dtReport.NewRow();
                            drm["S.No"] = dtposetting.Rows[m]["MasterValue"];
                            for (int col = 4; col < dtReport.Columns.Count; col++)
                            {
                                string scolname = Convert.ToString(dtReport.Columns[col]);
                                string[] spli = scolname.Split('-');

                                string sqlmark =da.GetFunction("select " + dtposetting.Rows[m]["MasterValue"] + " from COThresholdSettings c,Master_Settings m where m.settings='COSettings' and c.CourseID=m.masterno and c.Subject_NO in(" + spli[0] + ") and template='"+spli[1]+"'");
                                if (sqlmark != "" && sqlmark != "0")
                                {
                                    drm[scolname] = sqlmark;
                                    if (!settotal.ContainsKey(sqlmark))
                                    {
                                        cunt = 0;
                                        cunt++;
                                        settotal.Add(sqlmark, cunt);
                                    }
                                    else
                                    {
                                        string val =Convert.ToString(settotal[sqlmark]);
                                        int j = 0;
                                
                                        int.TryParse(val,out j);
                                        j++;
                                        settotal[sqlmark] = j;
                                    }
                                    if (sqlmark=="3")
                                        drm["Reg No"] = Convert.ToString(settotal[sqlmark]);
                                    else if (sqlmark == "2")
                                        drm["Roll No"] = Convert.ToString(settotal[sqlmark]);
                                    else if (sqlmark == "1")
                                        drm["StudName"] = Convert.ToString(settotal[sqlmark]);
                                    else
                                        drm["StudName"] = "0";
                                }
                         


                            }
                            dtReport.Rows.Add(drm);
                        }

                    }
                }
                else
                {

                }
                if (dtReport.Rows.Count > 0)
                {
                    GridView1.DataSource = dtReport;
                    GridView1.DataBind();
                    btnPrint.Visible = true;
                    btnprint1.Visible = true;
                    GridView1.Visible = true;
                    txtexcelname.Visible = true;
                    lblrptname.Visible = true;

                    GridView1.Rows[2].Visible = false;
                    GridViewRow row = GridView1.Rows[1];
                    GridViewRow previousRow = GridView1.Rows[0];
                    for (int j = 0; j < row.Cells.Count; j++)
                    {
                        string date = row.Cells[j].Text;
                        string predate = previousRow.Cells[j].Text;
                        if (date == predate)
                        {
                            if (previousRow.Cells[j].RowSpan == 0)
                            {
                                if (row.Cells[j].RowSpan == 0)
                                {
                                    previousRow.Cells[j].RowSpan += 2;
                                }
                                else
                                {
                                    previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                                }
                                row.Cells[j].Visible = false;
                            }
                        }

                    }

                    int rowcnt = GridView1.Rows.Count - 1;

                    for (int rowIndex = GridView1.Rows.Count - rowcnt; rowIndex >= 0; rowIndex--)
                    {


                        for (int cell = GridView1.Rows[rowIndex].Cells.Count - 1; cell > 0; cell--)
                        {
                            TableCell colum = GridView1.Rows[rowIndex].Cells[cell];
                            TableCell previouscol = GridView1.Rows[rowIndex].Cells[cell - 1];
                            if (colum.Text == previouscol.Text)
                            {
                                if (previouscol.ColumnSpan == 0)
                                {
                                    if (colum.ColumnSpan == 0)
                                    {
                                        previouscol.ColumnSpan += 2;

                                    }
                                    else
                                    {
                                        previouscol.ColumnSpan += colum.ColumnSpan + 1;

                                    }
                                    colum.Visible = false;
                                }
                            }
                        }
                    }
                    for (int j = 3; j < GridView1.Rows.Count; j++)
                    {
                        for (int m = 4; m < GridView1.Rows[j].Cells.Count; m++)
                        {
                            GridView1.Rows[j].Cells[m].HorizontalAlign = HorizontalAlign.Center;
                            GridView1.Rows[j].Cells[0].HorizontalAlign = HorizontalAlign.Center;

                        }
                    }

                        RowHead(GridView1);
                }
              //  btnPrint11();

            }
        }
        catch
        {
        }
    }
    protected void RowHead(GridView gview)
    {
        for (int head = 0; head < 3; head++)
        {
            gview.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            gview.Rows[head].Font.Bold = true;
            gview.Rows[head].HorizontalAlign = HorizontalAlign.Center;
            gview.Rows[head].Font.Name = "Book Antique";
        }
    }
    //public void btnPrint11()
    //{
    //    string college_code = Convert.ToString(ddlCollege.SelectedValue);
    //    string colQ = "select * from collinfo where college_code='" + college_code + "'";
    //    DataSet dsCol = new DataSet();
    //    dsCol = da.select_method_wo_parameter(colQ, "Text");
    //    string collegeName = string.Empty;
    //    string collegeCateg = string.Empty;
    //    string collegeAff = string.Empty;
    //    string collegeAdd = string.Empty;
    //    string collegePhone = string.Empty;
    //    string collegeFax = string.Empty;
    //    string collegeWeb = string.Empty;
    //    string collegeEmai = string.Empty;
    //    string collegePin = string.Empty;
    //    string acr = string.Empty;
    //    string City = string.Empty;
    //    if (dsCol.Tables.Count > 0 && dsCol.Tables[0].Rows.Count > 0)
    //    {
    //        collegeName = Convert.ToString(dsCol.Tables[0].Rows[0]["Collname"]);
    //        City = Convert.ToString(dsCol.Tables[0].Rows[0]["address3"]);
    //        collegeAff = "(Affiliated to " + Convert.ToString(dsCol.Tables[0].Rows[0]["university"]) + ")";
    //        collegeAdd = Convert.ToString(dsCol.Tables[0].Rows[0]["address1"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["address2"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["district"]) + " - " + Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
    //        collegePin = Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
    //        collegePhone = "OFFICE: " + Convert.ToString(dsCol.Tables[0].Rows[0]["phoneno"]);
    //        collegeFax = "FAX: " + Convert.ToString(dsCol.Tables[0].Rows[0]["faxno"]);
    //        collegeWeb = "Website: " + Convert.ToString(dsCol.Tables[0].Rows[0]["website"]);
    //        collegeEmai = "E-Mail: " + Convert.ToString(dsCol.Tables[0].Rows[0]["email"]);
    //        collegeCateg = "(" + Convert.ToString(dsCol.Tables[0].Rows[0]["category"]) + ")";
    //    }
    //    DateTime dt = DateTime.Now;
    //    int year = dt.Year;
    //    spCollegeName.InnerHtml = collegeName;
    //    spAddr.InnerHtml = collegeAdd;
    //    spRoomType.InnerHtml = "Batch : " + Convert.ToString(ddlbatch.SelectedItem.Text) + "<br/>" + "Degree : " + Convert.ToString(ddlbranch.SelectedItem.Text);
    //    spReportName.InnerHtml = "Internal Mark Calculation";
    //    // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);
    //    spRoomNo.InnerHtml = "Subject: " + Convert.ToString(ddlsubject.SelectedItem.Text);

    //}
    
    protected void cbsubj_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cblsubject, cbsubj, txtsubj, lblsubject.Text);
       
    }
    protected void cblsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cblsubject, cbsubj, txtsubj, lblsubject.Text);
       
    }


    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text.ToString().Trim();
            if (reportname != "")
            {
                da.printexcelreportgrid(GridView1, reportname);
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
      

        string degreedetails = "CoBasedInternalCalculation " +  '@' + "Degree : " + ddldegree.SelectedItem.Text+'-'+ ddlbranch.SelectedItem.Text + '@' + "Sem : " + ddlsem.SelectedItem.Text + '@' + "Batch : " + ddlbatch.SelectedItem.Text;
        Printcontrol.loadspreaddetails(GridView1, "CoBasedInternalCalculation.aspx", degreedetails, 0, ss);
        Printcontrol.Visible = true;
    }

}