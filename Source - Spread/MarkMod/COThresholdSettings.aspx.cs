using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using InsproDataAccess;

public partial class COThresholdSettings : System.Web.UI.Page
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

        ddlsubject.Visible = true;
        ddlsubject.Enabled = true;
        try
        {
            ddlsubject.Items.Clear();
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


                string logstaffcode = "";
                if (Convert.ToString(Session["Staff_Code"]) != "")
                {
                    logstaffcode = " and st.staff_code='" + Convert.ToString(Session["Staff_Code"]) + "'";
                }
                //==================================//
                subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and  st.subject_no=s.subject_no  and SM.degree_code=" + ddlbranch.SelectedValue.ToString() + " " + sems.ToString() + " and  S.subtype_no = Sem.subtype_no and promote_count=1 and SM.batch_year='" + ddlbatch.SelectedValue.ToString() + "' " + logstaffcode + " order by S.subject_no ";//rajasekar 12/07/2018


                if (subjectquery != "")
                {
                    //dtHeader.Dispose();
                    //dtHeader.Reset();
                    dtHeader = dir.selectDataTable(subjectquery);
                    if (dtHeader.Rows.Count > 0)
                    {
                        ddlsubject.Visible = true;
                        ddlsubject.Enabled = true;
                        ddlsubject.DataSource = dtHeader;
                        ddlsubject.DataValueField = "Subject_No";
                        ddlsubject.DataTextField = "Subject_Name";
                        ddlsubject.DataBind();

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
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetSubject();

    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
        bindsem();
        GetSubject();

    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsem();
        GetSubject();
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
        try
        {
            string subNo = Convert.ToString(ddlsubject.SelectedValue);
            if (!string.IsNullOrEmpty(subNo))
            {
                int del = dir.deleteData("delete COThresholdSettings where Subject_NO='" + subNo + "'");
                int count = 0;

                foreach (GridViewRow grid in GridView1.Rows)
                {
                    string coId = Convert.ToString((grid.FindControl("CoId") as Label).Text);
                    string ddlKo = Convert.ToString((grid.FindControl("ddlknw") as DropDownList).SelectedValue);
                    string ddlpo1 = Convert.ToString((grid.FindControl("ddlpo1") as DropDownList).SelectedValue);
                    if (string.IsNullOrEmpty(ddlpo1))
                        ddlpo1 = "null";
                    string ddlpo2 = Convert.ToString((grid.FindControl("ddlpo2") as DropDownList).SelectedValue);
                    if (string.IsNullOrEmpty(ddlpo2))
                        ddlpo2 = "null";
                    string ddlpo3 = Convert.ToString((grid.FindControl("ddlpo3") as DropDownList).SelectedValue);
                    if (string.IsNullOrEmpty(ddlpo3))
                        ddlpo3 = "null";
                    string ddlpo4 = Convert.ToString((grid.FindControl("ddlpo4") as DropDownList).SelectedValue);
                    if (string.IsNullOrEmpty(ddlpo4))
                        ddlpo4 = "null";
                    string ddlpo5 = Convert.ToString((grid.FindControl("ddlpo5") as DropDownList).SelectedValue);
                    if (string.IsNullOrEmpty(ddlpo5))
                        ddlpo5 = "null";
                    string ddlpo6 = Convert.ToString((grid.FindControl("ddlpo6") as DropDownList).SelectedValue);
                    if (string.IsNullOrEmpty(ddlpo6))
                        ddlpo6 = "null";
                    string ddlpo7 = Convert.ToString((grid.FindControl("ddlpo7") as DropDownList).SelectedValue);
                    if (string.IsNullOrEmpty(ddlpo7))
                        ddlpo7 = "null";
                    string ddlpo8 = Convert.ToString((grid.FindControl("ddlpo8") as DropDownList).SelectedValue);
                    if (string.IsNullOrEmpty(ddlpo8))
                        ddlpo8 = "null";
                    string ddlpo9 = Convert.ToString((grid.FindControl("ddlpo9") as DropDownList).SelectedValue);
                    if (string.IsNullOrEmpty(ddlpo9))
                        ddlpo9 = "null";
                    string ddlpo10 = Convert.ToString((grid.FindControl("ddlpo10") as DropDownList).SelectedValue);
                    if (string.IsNullOrEmpty(ddlpo10))
                        ddlpo10 = "null";
                    string ddlpo11 = Convert.ToString((grid.FindControl("ddlpo11") as DropDownList).SelectedValue);
                    if (string.IsNullOrEmpty(ddlpo11))
                        ddlpo11 = "null";
                    string ddlpo12 = Convert.ToString((grid.FindControl("ddlpo12") as DropDownList).SelectedValue);
                    if (string.IsNullOrEmpty(ddlpo12))
                        ddlpo12 ="null";
                    string ddlpo13 = Convert.ToString((grid.FindControl("ddlpo13") as DropDownList).SelectedValue);
                    if (string.IsNullOrEmpty(ddlpo13))
                        ddlpo13 = "null";
                    string ddlpo14 = Convert.ToString((grid.FindControl("ddlpo14") as DropDownList).SelectedValue);
                    if (string.IsNullOrEmpty(ddlpo14))
                        ddlpo14 = "null";
                    string ddlpo15 = Convert.ToString((grid.FindControl("ddlpo15") as DropDownList).SelectedValue);
                    if (string.IsNullOrEmpty(ddlpo15))
                        ddlpo15 = "null";
                    string txtDes = Convert.ToString((grid.FindControl("txtDesc") as TextBox).Text);
                    string txtTarget = Convert.ToString((grid.FindControl("txtTarget") as TextBox).Text);
                    string textThres = Convert.ToString((grid.FindControl("txtThreshold") as TextBox).Text);
                    if (!string.IsNullOrEmpty(coId) && !string.IsNullOrEmpty(ddlKo) && !string.IsNullOrEmpty(txtDes) && !string.IsNullOrEmpty(txtTarget) && !string.IsNullOrEmpty(textThres))
                    {

                        double Target = 0;
                        double Thres = 0;
                        double.TryParse(txtTarget, out Target);
                        double.TryParse(textThres, out Thres);
                        if (Thres < Target)
                        {
                            string insert = "insert into  COThresholdSettings (Subject_NO,CourseID,Knowledgeid,Description,Target,Threshold,po1,po2,po3,po4,po5,po6,po7,po8,po9,po10,po11,po12,po13,po14,po15) values ('" + subNo + "','" + coId + "','" + ddlKo + "','" + txtDes + "','" + Target + "','" + Thres + "'," + ddlpo1 + "," + ddlpo2 + "," + ddlpo3 + "," + ddlpo4 + "," + ddlpo5 + "," + ddlpo6 + "," + ddlpo7 + "," + ddlpo8 + "," + ddlpo9 + "," + ddlpo10 + "," + ddlpo11 + "," + ddlpo12 + "," + ddlpo13 + "," + ddlpo14 + "," + ddlpo15 + ")";
                            int val = dir.insertData(insert);
                            count = val + count;
                        }
                    }
                   
                }
                if (count > 0)
                {
                    divpopalter.Visible = true;
                    lblaltermsgs.Visible = true;
                    lblaltermsgs.Text = "Saved Successfuly!";
                }
                else
                {
                    divpopalter.Visible = true;
                    lblaltermsgs.Visible = true;
                    lblaltermsgs.Text = "Not Saved!";
                }
            }
            else
            {
                divpopalter.Visible = true;
                lblaltermsgs.Visible = true;
                lblaltermsgs.Text = "Subject Not found!";

            }
        }
        catch
        {
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
            string poval = da.GetFunction("select count(MasterValue) from CO_MasterValues where MasterCriteria='POsettings' and collegecode='" + ddlCollege.SelectedValue.ToString() + "'");
            string selectQ = "select masterno,template from Master_Settings where settings like 'COSettings'";
            DataTable dtSettings = dir.selectDataTable(selectQ);
            if (dtSettings.Rows.Count > 0)
            {
                GridView1.DataSource = dtSettings;
                GridView1.DataBind();
                divspreadpopup.Visible = true;
                int va = 9+Convert.ToInt32(poval);
                for (int g = va; g < 24; g++)
                {
                    GridView1.HeaderRow.Cells[g].Visible = false;
                }
            }
            string clgcode = Convert.ToString(ddlCollege.SelectedValue);
            string degcode = Convert.ToString(ddlbranch.SelectedValue);
            string batchyear = Convert.ToString(ddlbatch.SelectedValue);
            string semester = Convert.ToString(ddlsem.SelectedValue).Trim();
            string subNo = Convert.ToString(ddlsubject.SelectedValue);
            string SelectQ = "select * from COThresholdSettings where Subject_NO='" + subNo + "'";
            DataTable dtSetting = dir.selectDataTable(SelectQ);

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

            string stuMark = "select SUM(im.marks) as stumark,SUM(ca.Mark) as totMark, examcode,app_no,ca.CourseOutComeNo,(select isnull(template,'') from Master_Settings where settings='COSettings' and masterno=ca.CourseOutComeNo) as CourseoutCome,c.criteria,c.criteria_no from CAQuesSettingsParent ca,NewInternalMarkEntry im,criteriaforInternal c where ca.MasterID=im.MasterID and subjectno in('" + subNo + "')  and (marks<>-1 and marks<>-16 and marks<>-20) and c.criteria_no=ca.criteriano  group by examcode,app_no,ca.CourseOutComeNo,ca.PartNo,c.criteria,c.criteria_no,ca.Mark";
            DataTable dtMark = dir.selectDataTable(stuMark);
            //string dicCo = "select distinct template,masterno from CAQuesSettingsParent c,Master_Settings m where settings='COSettings' and m.masterno=c.CourseOutComeNo and c.subjectNo='" + subNo + "' order by template";
            //DataTable dtCoNames = dir.selectDataTable(dicCo);
            string strwei = "select criteria_no,criterianame,coname,cono,weightage_percentage,subject_No  from weightage_setting where  batch='" + batchyear + "' and degree_code='" + degcode + "' and semester='" + semester + "'   and subject_no='" + subNo + "'";
            DataTable dtWeight = dir.selectDataTable(strwei);

            string strorder = filterfunction();
            string stddet = "Select  distinct registration.roll_no,registration.reg_no,registration.stud_name,registration.sections,registration.App_No,Registration.Sections,Roll_Admit from registration ,SubjectChooser where  registration.roll_no = subjectchooser.roll_no and registration.Degree_Code ='" + Convert.ToString(degcode) + "' and Semester = '" + Convert.ToString(semester) + "' and registration.Batch_Year = '" + Convert.ToString(batchyear) + "' and Subject_No = '" + Convert.ToString(ddlsubject.SelectedValue) + "' and RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR' and Semester = '" + Convert.ToString(semester) + "'   " + strstaffselecotr + "  " + strorder + "";

            //  string stddet = "select  r.Roll_No,r.Reg_No,r.Roll_Admit,r.Stud_Name,r.App_no,r.Sections   from Registration r where r.batch_year='" + batchyear + "' and r.degree_code='" + degcode + "' and r.Current_Semester='" + semester + "'  and  RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR'  order by r.reg_no,r.roll_no;";
            DataTable dtStudent = dir.selectDataTable(stddet);
            double tar = 0;
            double th = 0;
            int minTar = 0;
            int minthr = 0;
            double mintarPer = 0;
            double minthrper = 0;
            double totPer = 0;

            if (GridView1.Rows.Count > 0)
            {
                foreach (GridViewRow dr in GridView1.Rows)
                {
                    string target = Convert.ToString((dr.FindControl("txtTarget") as TextBox).Text);
                    string thres = Convert.ToString((dr.FindControl("txtThreshold") as TextBox).Text);
                    string course = Convert.ToString((dr.FindControl("lblCoName") as Label).Text);
                    double.TryParse(target, out tar);
                    double.TryParse(thres, out th);
                    double colper = 0;
                    minTar = 0;
                    minthr = 0;
                    mintarPer = 0;
                    minthrper = 0;
                    totPer = 0;
                    foreach (DataRow drStu in dtStudent.Rows)
                    {
                        string rollNo = Convert.ToString(drStu["Roll_No"]);
                        string Reg_No = Convert.ToString(drStu["Reg_No"]);
                        string Roll_Admit = Convert.ToString(drStu["Roll_Admit"]);
                        string Stud_Name = Convert.ToString(drStu["Stud_Name"]);
                        string App_no = Convert.ToString(drStu["App_no"]);
                        string Sections = Convert.ToString(drStu["Sections"]);
                        double commonWei = 0;
                        if (dtWeight.Rows.Count > 0)
                        {
                            dtWeight.DefaultView.RowFilter = "coname='" + course + "'";
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
                                    int cc = 0;
                                    if (dtMark.Rows.Count > 0)
                                    {
                                        if (CriNo.Contains(','))
                                        {
                                            string[] sptc = CriNo.Split(',');
                                            for (int j = 0; j < sptc.Length; j++)
                                            {
                                                cc = cc + 1;
                                                string cNo = Convert.ToString(sptc[j]);
                                                dtMark.DefaultView.RowFilter = "CourseoutCome='" + course.Trim() + "' and criteria_no in(" + cNo + ") and app_no='" + App_no + "'";
                                                DataTable dtStuMark = dtMark.DefaultView.ToTable();
                                                if (dtStuMark.Rows.Count > 0)
                                                {

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
                                            dtMark.DefaultView.RowFilter = "CourseoutCome='" + course.Trim() + "' and criteria_no in(" + CriNo + ") and app_no='" + App_no + "'";
                                            DataTable dtStuMark = dtMark.DefaultView.ToTable();
                                            cc = cc + 1;
                                            if (dtStuMark.Rows.Count > 0)
                                            {

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
                                    }
                                    if (sumwei > 0)
                                    {
                                        singleWei = sumwei / (cc * 100);
                                        singleWei = singleWei * 100;
                                        singleWei = Math.Round(singleWei, 0, MidpointRounding.AwayFromZero);

                                        singleWei = singleWei / 100;
                                        singleWei = singleWei * weight;
                                        singleWei = Math.Round(singleWei, 0, MidpointRounding.AwayFromZero);
                                        commonWei = commonWei + singleWei;
                                        // totPer = totPer + commonWei;
                                    }
                                }
                            }
                            if (th <= commonWei)
                            {
                                minthr = minthr + 1;
                                minthrper = minthrper + commonWei;
                            }
                            if (tar <= commonWei)
                            {
                                minTar = minTar + 1;
                                mintarPer = mintarPer + commonWei;
                            }
                            totPer = totPer + commonWei;
                        }
                    }
                    Label lbl = (dr.FindControl("lblAvgAtt") as Label);
                    Label lbl2 = (dr.FindControl("lblAtt") as Label);
                    Label lbl3 = (dr.FindControl("lblRelAvgAtt") as Label);
                    int stuCount = dtStudent.Rows.Count;
                    double cal1 = 0;
                    double cal2 = 0;
                    double cal3 = 0;
                    if (!string.IsNullOrEmpty(target) && !string.IsNullOrEmpty(thres))
                    {
                        if (totPer > 0 && stuCount > 0)
                        {
                            cal1 = totPer / (stuCount * 100);
                            cal1 = cal1 * 100;
                            cal1 = Math.Round(cal1, 0, MidpointRounding.AwayFromZero);
                            lbl.Text = Convert.ToString(cal1);
                        }
                        if (minthrper > 0 && minthr > 0)
                        {
                            cal2 = minthrper / (minthr * 100);
                            cal2 = cal2 * 100;
                            cal2 = Math.Round(cal2, 0, MidpointRounding.AwayFromZero);
                            lbl2.Text = Convert.ToString(cal2);
                        }
                        if (cal1 > 0 && tar > 0)
                        {
                            cal3 = cal1 / tar;
                            cal3 = cal3 * 100;
                            cal3 = Math.Round(cal3, 0, MidpointRounding.AwayFromZero);
                            lbl3.Text = Convert.ToString(cal3);
                        }
                    }
                }


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
            string subNo = Convert.ToString(ddlsubject.SelectedValue);
            string SelectQ = "select * from COThresholdSettings where Subject_NO='" + subNo + "'";
            DataTable dtTable = dir.selectDataTable(SelectQ);
            if (dtTable.Rows.Count > 0)
            {
                foreach (GridViewRow grid in GridView1.Rows)
                {
                    string coId = Convert.ToString((grid.FindControl("CoId") as Label).Text);
                    DropDownList ddlKo = (grid.FindControl("ddlknw") as DropDownList);
                    DropDownList ddlpo1 = (grid.FindControl("ddlpo1") as DropDownList);
                    DropDownList ddlpo2 = (grid.FindControl("ddlpo2") as DropDownList);
                    DropDownList ddlpo3 = (grid.FindControl("ddlpo3") as DropDownList);
                    DropDownList ddlpo4 = (grid.FindControl("ddlpo4") as DropDownList);
                    DropDownList ddlpo5 = (grid.FindControl("ddlpo5") as DropDownList);
                    DropDownList ddlpo6 = (grid.FindControl("ddlpo6") as DropDownList);
                    DropDownList ddlpo7 = (grid.FindControl("ddlpo7") as DropDownList);
                    DropDownList ddlpo8 = (grid.FindControl("ddlpo8") as DropDownList);
                    DropDownList ddlpo9 = (grid.FindControl("ddlpo9") as DropDownList);
                    DropDownList ddlpo10 = (grid.FindControl("ddlpo10") as DropDownList);
                    DropDownList ddlpo11 = (grid.FindControl("ddlpo11") as DropDownList);
                    DropDownList ddlpo12 = (grid.FindControl("ddlpo12") as DropDownList);
                    DropDownList ddlpo13 = (grid.FindControl("ddlpo13") as DropDownList);
                    DropDownList ddlpo14 = (grid.FindControl("ddlpo14") as DropDownList);
                    DropDownList ddlpo15 = (grid.FindControl("ddlpo15") as DropDownList);
                   
                    TextBox txtDes = (grid.FindControl("txtDesc") as TextBox);
                    TextBox txtTarget = (grid.FindControl("txtTarget") as TextBox);
                    TextBox textThres = (grid.FindControl("txtThreshold") as TextBox);
                    dtTable.DefaultView.RowFilter = "CourseID='" + coId + "'";
                    DataView dvData = dtTable.DefaultView;
                    string poval=string.Empty;
                    if (dvData.Count > 0)
                    {
                        txtDes.Text = Convert.ToString(dvData[0]["Description"]);
                        txtTarget.Text = Convert.ToString(dvData[0]["Target"]);
                        textThres.Text = Convert.ToString(dvData[0]["Threshold"]);
                        string kid = Convert.ToString(dvData[0]["Knowledgeid"]);

                        string po1 = Convert.ToString(dvData[0]["po1"]);
                        if (po1 == "0")
                            po1 = string.Empty;
                        string po2 = Convert.ToString(dvData[0]["po2"]);
                        if (po2 == "0")
                            po2 = string.Empty;
                        string po3 = Convert.ToString(dvData[0]["po3"]);
                        if (po3 == "0")
                            po3 = string.Empty;
                        string po4 = Convert.ToString(dvData[0]["po4"]);
                        if (po4 == "0")
                            po4 = string.Empty;
                        string po5 = Convert.ToString(dvData[0]["po5"]);
                        if (po5 == "0")
                            po5 = string.Empty;
                        string po6 = Convert.ToString(dvData[0]["po6"]);
                        if (po6 == "0")
                            po6 = string.Empty;
                        string po7 = Convert.ToString(dvData[0]["po7"]);
                        if (po7 == "0")
                            po7 = string.Empty;
                        string po8 = Convert.ToString(dvData[0]["po8"]);
                        if (po8 == "0")
                            po8 = string.Empty;
                        string po9 = Convert.ToString(dvData[0]["po9"]);
                        if (po9 == "0")
                            po9 = string.Empty;
                        string po10 = Convert.ToString(dvData[0]["po10"]);
                        if (po10 == "0")
                            po10 = string.Empty;
                        string po11 = Convert.ToString(dvData[0]["po11"]);
                        if (po11 == "0")
                            po11 = string.Empty;
                        string po12 = Convert.ToString(dvData[0]["po12"]);
                        if (po12 == "0")
                            po12 = string.Empty;
                        string po13 = Convert.ToString(dvData[0]["po13"]);
                        if (po13 == "0")
                            po13 = string.Empty;
                        string po14 = Convert.ToString(dvData[0]["po14"]);
                        if (po14 == "0")
                            po14 = string.Empty;
                        string po15 = Convert.ToString(dvData[0]["po15"]);
                        if (po15 == "0")
                            po15 = string.Empty;
                        ddlKo.ClearSelection();
                        ddlKo.Items.FindByValue(kid).Selected = true;
                        //if(po1.Trim().ToLower()=="s")
                        //    poval="Strong";
                        //else if(po1.Trim().ToLower()=="w")
                        //    poval="Weak";
                        //else if(po1.Trim().ToLower()=="m")
                        //     poval="Medium";

                        ddlpo1.ClearSelection();
                        ddlpo1.Items.FindByValue(po1).Selected = true;
                        ddlpo2.ClearSelection();
                        ddlpo2.Items.FindByValue(po2).Selected = true;
                        ddlpo3.ClearSelection();
                        ddlpo3.Items.FindByValue(po3).Selected = true;
                        ddlpo4.ClearSelection();
                        ddlpo4.Items.FindByValue(po4).Selected = true;
                        ddlpo5.ClearSelection();
                        ddlpo5.Items.FindByValue(po5).Selected = true;
                        ddlpo6.ClearSelection();
                        ddlpo6.Items.FindByValue(po6).Selected = true;
                        ddlpo7.ClearSelection();
                        ddlpo7.Items.FindByValue(po7).Selected = true;
                        ddlpo8.ClearSelection();
                        ddlpo8.Items.FindByValue(po8).Selected = true;
                        ddlpo9.ClearSelection();
                        ddlpo9.Items.FindByValue(po9).Selected = true;
                        ddlpo10.ClearSelection();
                        ddlpo10.Items.FindByValue(po10).Selected = true;
                        ddlpo11.ClearSelection();
                        ddlpo11.Items.FindByValue(po11).Selected = true;
                        ddlpo12.ClearSelection();
                        ddlpo12.Items.FindByValue(po12).Selected = true;
                        ddlpo13.ClearSelection();
                        ddlpo13.Items.FindByValue(po13).Selected = true;
                        ddlpo14.ClearSelection();
                        ddlpo14.Items.FindByValue(po14).Selected = true;
                        ddlpo15.ClearSelection();
                        ddlpo15.Items.FindByValue(po15).Selected = true;


                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlKo = (e.Row.FindControl("ddlknw") as DropDownList);
                string status = "select * from CO_MasterValues where MasterCriteria='Knowledgesettings' and collegecode='" + Convert.ToString(ddlCollege.SelectedValue) + "'";
                DataSet dtKo = da.select_method_wo_parameter(status, "text");
                ddlKo.Items.Clear();
                if (dtKo.Tables.Count > 0 && dtKo.Tables[0].Rows.Count > 0)
                {
                    ddlKo.DataSource = dtKo.Tables[0];
                    ddlKo.DataTextField = "MasterValue";
                    ddlKo.DataValueField = "MasterCode";
                    ddlKo.DataBind();
                    ddlKo.Items.Insert(0, "");
                }

                DataTable dt1 = new DataTable();
                DataRow dr1 = null;
                dt1.Columns.Add("name");
                dt1.Columns.Add("value");
                dr1 = dt1.NewRow();
                dr1["name"] = "Strong";
                dr1["value"] = "3";
                dt1.Rows.Add(dr1);
                dr1 = dt1.NewRow();
                dr1["name"] = "Moderate";
                dr1["value"] = "2";
                dt1.Rows.Add(dr1);
                dr1 = dt1.NewRow();
                dr1["name"] = "Weak";
                dr1["value"] = "1";
                dt1.Rows.Add(dr1);
                for (int j = 1; j < 16; j++)
                {
                    string ddlname = "ddlpo" + j + "";
                    DropDownList ddl1 = (e.Row.FindControl("" + ddlname + "") as DropDownList);

                    ddl1.DataSource = dt1;
                    ddl1.DataTextField = "name";
                    ddl1.DataValueField = "value";
                    ddl1.DataBind();
                    ddl1.Items.Insert(0, "");
                }

                string poval = da.GetFunction("select count(MasterValue) from CO_MasterValues where MasterCriteria='POsettings' and collegecode='" + ddlCollege.SelectedValue.ToString() + "'");
                    int va = 9 + Convert.ToInt32(poval);
                    for (int g = va; g < 24; g++)
                    {
                        e.Row.Cells[g].Visible = false;
                    }
                

            }
        }
        catch
        {
        }
    }

    protected void btnokclk_Click(object sender, EventArgs e)
    {
        divpopalter.Visible = false;
        lblaltermsgs.Visible = false;
    }

}