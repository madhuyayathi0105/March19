using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Configuration;
using wc = System.Web.UI.WebControls;
using InsproDataAccess;

public partial class ScheduleMOD_NewSemesterTimetable : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    InsproDirectAccess dir = new InsproDirectAccess();
    static string collegecode = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string strstaffcode = string.Empty;
    bool cellClick = false;
    static string code = "";
    Boolean allowCombineClass = false;
    static string selectedSubjectNo = "";
    static string selectedDept = string.Empty;
    static string selectedDesig = string.Empty;
    static string selectedCategory = string.Empty;
    Hashtable htData = new Hashtable();
    bool replace = false;
    bool appand = false;
    bool isChanged = false;
    int status = 0;
    bool staffApnd = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("ScheduleHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/ScheduleMOD/ScheduleHome.aspx");
                    return;
                }
            }
            collegecode = ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedItem.Value) : Convert.ToString(Session["collegecode"]);
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            strstaffcode = Session["Staff_Code"].ToString();
            Session["StaffCode"] = Session["Staff_Code"].ToString();
            if (!IsPostBack)
            {

                bindCollege();
                bindDept();
                bindstaffCode();
                bindSem();
                bindbatchInfo();
                bindSubject();
                selectedDept = Convert.ToString(ddlDept.SelectedValue);
                collegecode = ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedItem.Value) : Convert.ToString(Session["collegecode"]);
                btnPrint.Visible = false;
                //btndelete.Visible = false;
                string date = DateTime.Now.ToString("dd/MM/yyyy");
                string[] dsplit1 = date.Split(new Char[] { '/' });
                txtFromDate.Text = dsplit1[0].ToString().PadLeft(2, '0') + "/" + dsplit1[1].ToString().PadLeft(2, '0') + "/" + dsplit1[2].ToString();

            }
        }
        catch (Exception ex) { }
    }
    private void bindCollege()
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
                columnfield = " and group_code='" + group_code + "'";
            else
                columnfield = " and user_code='" + Session["usercode"] + "'";

            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.Enabled = true;
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();

            }
        }
        catch (Exception e) { }
    }

    private void bindDept()
    {
        try
        {

            ds.Clear();
            string group_user = string.Empty;
            string cmd = string.Empty;
            ddlDept.Items.Clear();
            string singleuser = Session["single_user"].ToString();
            if (singleuser == "True")
            {
                cmd = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code=" + usercode + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + collegecode + "') order by dept_name";
            }
            else
            {
                group_user = Session["group_code"].ToString();
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                cmd = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr where group_code='" + group_user + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + collegecode + "') order by dept_name";

            }
            ds = d2.select_method_wo_parameter(cmd, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlDept.DataSource = ds;
                    ddlDept.DataValueField = "dept_code";
                    ddlDept.DataTextField = "dept_name";
                    ddlDept.DataBind();

                }

            }
        }
        catch { }
    }

    private void bindSem()
    {
        try
        {
            ddlsem.Items.Clear();
            string satffCode = Convert.ToString(ddlSearchOption.SelectedValue);
            string selectSem = " select distinct r.Current_Semester from Registration r where   CC=0 and isnull(delflag,0)<>1 and r.Exam_Flag<>'DEBAR' order by r.Current_Semester";
            DataTable dtsem = dir.selectDataTable(selectSem);
            if (dtsem.Rows.Count > 0)
            {
                ddlsem.DataSource = dtsem;
                ddlsem.DataValueField = "Current_Semester";
                ddlsem.DataTextField = "Current_Semester";
                ddlsem.DataBind();
            }

        }
        catch
        {

        }
    }

    private void bindHour()
    {
        try
        {
            string degCode = string.Empty;
            string deg = Convert.ToString(ddlSchinfo.SelectedValue);
            string sem = Convert.ToString(ddlsem.SelectedValue);
            string[] val = deg.Split('-');
            if (val.Length > 1)
                degCode = Convert.ToString(val[1]);
            ddlHour.Items.Clear();
            if (!string.IsNullOrEmpty(degCode))
            {
                string noofhour = d2.GetFunction("select max(No_of_hrs_per_day)HoursPerDay  from PeriodAttndSchedule where degree_code='" + degCode + "' and  semester='" + sem + "'");
                int hour = 0;
                int.TryParse(noofhour, out hour);
                if (hour != 0)
                {
                    for (int i = 1; i <= hour; i++)
                    {
                        ddlHour.Items.Add(i.ToString());
                    }
                }
            }
        }
        catch
        {

        }


    }

    private void bindSubject()
    {
        try
        {
            ddlSubject.Items.Clear();
            string collegeCode = Convert.ToString(ddlcollege.SelectedValue);
            string sem = Convert.ToString(ddlsem.SelectedValue);
            string staffCode = Convert.ToString(ddlSearchOption.SelectedValue);
            string degCode = string.Empty;
            string batchYear = string.Empty;
            string sec = string.Empty;
            string deg = Convert.ToString(ddlSchinfo.SelectedValue);
            //string sem = Convert.ToString(ddlsem.SelectedValue);
            string[] val = deg.Split('-');
            if (val.Length > 1)
            {
                degCode = Convert.ToString(val[1]);
                batchYear = Convert.ToString(val[0]);
                sec = Convert.ToString(val[3]);
            }
            string section = string.Empty;
            if (!string.IsNullOrEmpty(sec))
            {
                section = "and sections='" + sec + "'";
            }
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(staffCode))
            {
                //string SelectSubject = "select distinct s.subject_no,(s.subject_code+'-'+s.subject_name) as subjectval,r.Current_Semester from Registration r,subject s,syllabus_master sm,staff_selector ss where r.Batch_Year=sm.Batch_Year and r.degree_code=sm.degree_code and r.Current_Semester=sm.semester and s.syll_code=sm.syll_code and s.subject_no=ss.subject_no and ss.staff_code='" + staffCode + "' and r.Current_Semester='" + sem + "' and r.college_code='" + collegeCode + "'";

                //string SelectSubject = "select distinct s.subject_code,(s.subject_code+'-'+s.subject_name) as subjectval  from Registration r,subject s,syllabus_master sm,Department de,course c,Degree d,staff_selector ss where ss.subject_no=s.subject_no and c.Course_Id=d.Course_Id and r.degree_code=d.Degree_Code and de.Dept_Code=d.Dept_Code and r.Batch_Year=sm.Batch_Year and r.degree_code=sm.degree_code and r.Current_Semester=sm.semester and s.syll_code=sm.syll_code and r.Current_Semester='" + sem + "'   and LTRIM(RTRIM(ISNULL(r.Sections,'')))=LTRIM(RTRIM(ISNULL(ss.Sections,''))) and ss.staff_code='" + staffCode + "'";//and r.college_code='" + collegeCode + "'

                string SelectSubject = "select distinct s.subject_code,(s.subject_code+'-'+s.subject_name) as subjectval  from Registration r,subject s,syllabus_master sm,Department de,course c,Degree d where  c.Course_Id=d.Course_Id and r.degree_code=d.Degree_Code and de.Dept_Code=d.Dept_Code and r.Batch_Year=sm.Batch_Year and r.degree_code=sm.degree_code and r.Current_Semester=sm.semester and s.syll_code=sm.syll_code and r.Current_Semester='" + sem + "' and r.Batch_Year='" + batchYear + "' and r.degree_code='" + degCode + "'";

                DataTable dtSubject = dir.selectDataTable(SelectSubject);
                if (dtSubject.Rows.Count > 0)
                {
                    ddlSubject.DataSource = dtSubject;
                    ddlSubject.DataValueField = "subject_code";
                    ddlSubject.DataTextField = "subjectval";
                    ddlSubject.DataBind();
                }

            }
        }
        catch
        {

        }
    }

    private void bindbatchInfo()
    {

        ddlSchinfo.Items.Clear();
        string collegeCode = Convert.ToString(ddlcollege.SelectedValue);
        string sem = Convert.ToString(ddlsem.SelectedValue);
        string staffCode = Convert.ToString(ddlSearchOption.SelectedValue);
        //string subNo = Convert.ToString(ddlSubject.SelectedValue);

        if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(sem))
        {

            //string Selectbatch = "select distinct convert(nvarchar(max),(Convert(nvarchar,(r.Batch_Year))+'-'+Convert(nvarchar,(r.degree_code))+'-'+Convert(nvarchar,(r.Current_Semester))+'-'+LTRIM(RTRIM(ISNULL(r.Sections,''))))) as val,(ci.acr+'-'+Convert(nvarchar,(r.Batch_Year))+'-'+convert(nvarchar,(c.Course_Name+'-'+ de.dept_acronym+'-'+Convert(nvarchar,(r.Current_Semester))+'-'+LTRIM(RTRIM(ISNULL(r.Sections,''))))))as ccc   from Registration r,subject s,syllabus_master sm,Department de,course c,Degree d,staff_selector ss,collinfo ci where ss.subject_no=s.subject_no and c.Course_Id=d.Course_Id and r.degree_code=d.Degree_Code and de.Dept_Code=d.Dept_Code and r.Batch_Year=sm.Batch_Year and r.degree_code=sm.degree_code and r.Current_Semester=sm.semester and s.syll_code=sm.syll_code and r.Current_Semester='" + sem + "'   and r.college_code=ci.college_code and LTRIM(RTRIM(ISNULL(r.Sections,'')))=LTRIM(RTRIM(ISNULL(ss.Sections,''))) and ss.staff_code='" + staffCode + "'";//and r.college_code='" + collegeCode + "'// and s.subject_code='" + subNo + "'
            string Selectbatch = "select distinct convert(nvarchar(max),(Convert(nvarchar,(r.Batch_Year))+'-'+Convert(nvarchar,(r.degree_code))+'-'+Convert(nvarchar,(r.Current_Semester))+'-'+LTRIM(RTRIM(ISNULL(r.Sections,''))))) as val,(ci.acr+'/'+Convert(nvarchar,(r.Batch_Year))+'/'+convert(nvarchar,(c.Course_Name+'/'+ de.dept_acronym+'/'+Convert(nvarchar,(r.Current_Semester))+'/'+LTRIM(RTRIM(ISNULL(r.Sections,''))))))as ccc    from Registration r,subject s,syllabus_master sm,Department de,course c,Degree d,collinfo ci where  c.Course_Id=d.Course_Id and r.degree_code=d.Degree_Code and de.Dept_Code=d.Dept_Code and r.Batch_Year=sm.Batch_Year and r.degree_code=sm.degree_code and r.Current_Semester=sm.semester and s.syll_code=sm.syll_code and r.Current_Semester='" + sem + "'   and r.college_code=ci.college_code  and CC=0 and isnull(delflag,0)<>1 and r.Exam_Flag<>'DEBAR'  order by ccc,val";

            DataTable dtbatchinfo = dir.selectDataTable(Selectbatch);
            if (dtbatchinfo.Rows.Count > 0)
            {
                ddlSchinfo.DataSource = dtbatchinfo;
                ddlSchinfo.DataValueField = "val";
                ddlSchinfo.DataTextField = "ccc";
                ddlSchinfo.DataBind();
            }

        }

    }

    private void bindstaffCode()
    {
        try
        {
            ddlSearchOption.Items.Clear();
            if (!string.IsNullOrEmpty(selectedDept))
            {
                string query = "select distinct st.staff_code,(st.staff_code+'-'+sm.staff_name) as staff from stafftrans st, staffmaster sm where st.staff_code=sm.staff_code and resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and   dept_code in ('" + selectedDept + "')  and college_code='" + collegecode + "'";
                DataTable dtStaff = dir.selectDataTable(query);
                if (dtStaff.Rows.Count > 0)
                {
                    ddlSearchOption.DataSource = dtStaff;
                    ddlSearchOption.DataValueField = "staff_code";
                    ddlSearchOption.DataTextField = "staff";
                    ddlSearchOption.DataBind();
                }
            }

        }
        catch
        {

        }
    }

    private void bindTimeTable()
    {
        try
        {
            ddltimetable.Items.Clear();
            string degCode = string.Empty;
            string batchYear = string.Empty;
            string sec = string.Empty;
            string deg = Convert.ToString(ddlSchinfo.SelectedValue);
            string sem = Convert.ToString(ddlsem.SelectedValue);
            string[] val = deg.Split('-');
            if (val.Length > 1)
            {
                degCode = Convert.ToString(val[1]);
                batchYear = Convert.ToString(val[0]);
                sec = Convert.ToString(val[3]);
            }
            string section = string.Empty;
            if (!string.IsNullOrEmpty(sec))
            {
                section = "and sections='" + sec + "'";
            }
            ds.Dispose();
            ds.Reset();
            string strtimetable = "Select DISTINCT TTname from semester_schedule where batch_year=" + batchYear + " and degree_code=" + degCode + " and semester=" + sem + " " + section + "";
            ds = d2.select_method(strtimetable, hat, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddltimetable.DataSource = ds;
                ddltimetable.DataTextField = "TTname";
                ddltimetable.DataBind();
                bindDate(batchYear, degCode, sem, sec);
            }
            ddltimetable.Items.Insert(0, "");
            ddltimetable.Items.Insert(1, "New");

        }
        catch
        {
        }
    }

    private void bindDate(string batch, string degCode, string sem, string sec)
    {
        try
        {
            txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            string section = string.Empty;
            if (sec != "" && sec != "-1" && sec != "All")
            {
                section = "and sections='" + sec + "'";
            }
            string date = d2.GetFunction("Select convert(nvarchar(15),Fromdate,103) as date from semester_schedule where batch_year=" + batch + " and degree_code=" + degCode + " and semester=" + sem + " " + section + " and ttname='" + Convert.ToString(ddltimetable.SelectedItem.Text) + "' ");
            if (date != "" && date != null && date != "0" && ddlSchinfo.Enabled == true)
            {
                //StartDate 
                DateTime dt1 = new DateTime();// Convert.ToDateTime(datefrom.ToString());
                bool isValidDate = DateTime.TryParseExact(date, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dt1);
                if (isValidDate)
                    txtFromDate.Text = date;
            }

            if (Convert.ToString(ddltimetable.SelectedItem.Text).ToLower() == "new")
            {
                string deg = Convert.ToString(ddlSchinfo.SelectedItem.Text);
                tdTime.Visible = true;
                txttimetable.Text = deg;
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlcollege_change(object sender, EventArgs e)
    {
        spreadTimeTable.Visible = false;
        collegecode = ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedItem.Value) : Convert.ToString(Session["collegecode"]);
        bindDept();
        bindstaffCode();
        bindSem();
        bindSubject();
        bindbatchInfo();
        tdTime.Visible = false;
        txttimetable.Text = "";
        //tdStfCodeAuto.Visible = true;
        //tdStfNameAuto.Visible = false;

    }

    protected void ddlDept_change(object sender, EventArgs e)
    {
        tdTime.Visible = false;
        txttimetable.Text = "";
        spreadTimeTable.Visible = false;
        selectedDept = Convert.ToString(ddlDept.SelectedValue);
        bindstaffCode();
        bindSem();
        bindbatchInfo();
        bindSubject();
        string degCode = string.Empty;
        string batchYear = string.Empty;
        string sec = string.Empty;
        string deg = Convert.ToString(ddlSchinfo.SelectedValue);
        string sem = Convert.ToString(ddlsem.SelectedValue);
        string[] val = deg.Split('-');
        if (val.Length > 1)
        {
            degCode = Convert.ToString(val[1]);
            batchYear = Convert.ToString(val[0]);
            sec = Convert.ToString(val[3]);
        }
        bindTimeTable();
        bindDate(batchYear, degCode, sem, sec);
        bindTimeTableGrid(degCode, sem);
        bindHour();
    }

    protected void ddlSubject_change(object sender, EventArgs e)
    {
        try
        {
            tdTime.Visible = false;
            txttimetable.Text = "";
            //bindbatchInfo();
            string degCode = string.Empty;
            string batchYear = string.Empty;
            string sec = string.Empty;
            string deg = Convert.ToString(ddlSchinfo.SelectedValue);
            string sem = Convert.ToString(ddlsem.SelectedValue);
            string[] val = deg.Split('-');
            if (val.Length > 1)
            {
                degCode = Convert.ToString(val[1]);
                batchYear = Convert.ToString(val[0]);
                sec = Convert.ToString(val[3]);
            }
            //bindSubject();
            bindTimeTable();
            bindDate(batchYear, degCode, sem, sec);
            bindTimeTableGrid(degCode, sem);
            bindHour();
        }
        catch
        {
        }
    }

    protected void ddlsem_change(object sender, EventArgs e)
    {
        bindbatchInfo();
        bindSubject();
        tdTime.Visible = false;
        txttimetable.Text = "";
        string batchYear = string.Empty;
        string sec = string.Empty;
        string degCode = string.Empty;
        string deg = Convert.ToString(ddlSchinfo.SelectedValue);
        string sem = Convert.ToString(ddlsem.SelectedValue);
        string[] val = deg.Split('-');
        if (val.Length > 1)
        {
            degCode = Convert.ToString(val[1]);
            batchYear = Convert.ToString(val[0]);
            sec = Convert.ToString(val[3]);
        }

        bindTimeTable();
        bindDate(batchYear, degCode, sem, sec);
        bindTimeTableGrid(degCode, sem);
        bindHour();
    }

    protected void ddlSchinfo_change(object sender, EventArgs e)
    {
        tdTime.Visible = false;
        txttimetable.Text = "";
        string degCode = string.Empty;
        string batchYear = string.Empty;
        string sec = string.Empty;
        string deg = Convert.ToString(ddlSchinfo.SelectedValue);
        string sem = Convert.ToString(ddlsem.SelectedValue);
        string[] val = deg.Split('-');
        if (val.Length > 1)
        {
            degCode = Convert.ToString(val[1]);
            batchYear = Convert.ToString(val[0]);
            sec = Convert.ToString(val[3]);
        }
        bindSubject();
        bindTimeTable();
        bindDate(batchYear, degCode, sem, sec);
        bindTimeTableGrid(degCode, sem);
        bindHour();
    }

    protected void ddlDayOrder_change(object sender, EventArgs e)
    {
    }

    protected void ddlSearchOption_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            spreadTimeTable.Visible = false;
            bindSem();
            bindSubject();
            bindbatchInfo();
            string batchYear = string.Empty;
            string sec = string.Empty;
            string degCode = string.Empty;
            string deg = Convert.ToString(ddlSchinfo.SelectedValue);
            string sem = Convert.ToString(ddlsem.SelectedValue);
            string[] val = deg.Split('-');
            if (val.Length > 1)
            {
                degCode = Convert.ToString(val[1]);
                batchYear = Convert.ToString(val[0]);
                sec = Convert.ToString(val[3]);
            }
            bindTimeTableGrid(degCode, sem);
            bindHour();
            bindTimeTable();
            bindDate(batchYear, degCode, sem, sec);

        }
        catch
        {
        }
    }

    private void bindTimeTableGrid(string degCode, string sem)
    {
        try
        {
            string dayvalue = string.Empty;
            int dayorder = 0;
            int hourPerDay = 0;
            int noofdays = 0;
            string holiday = string.Empty;

            DataTable dtTTSel = new DataTable();

            string day = string.Empty;
            DataSet dsDay = new DataSet();
            int date = 0;

            string strpriodquery = "Select No_of_hrs_per_day,schorder,nodays,holiday from PeriodAttndSchedule where degree_code = '" + degCode + "' and semester = " + sem + "";
            dsDay = d2.select_method(strpriodquery, hat, "Text");
            if (dsDay.Tables.Count > 0 && dsDay.Tables[0].Rows.Count > 0)
            {
                // dayorder = Convert.ToInt32(dsDay.Tables[0].Rows[0]["schorder"]);
                hourPerDay = Convert.ToInt32(dsDay.Tables[0].Rows[0]["No_of_hrs_per_day"]);
                noofdays = Convert.ToInt32(dsDay.Tables[0].Rows[0]["nodays"]);
                holiday = Convert.ToString(dsDay.Tables[0].Rows[0]["holiday"]);
                // Session["dayorder"] = Convert.ToString(dayorder);
            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Please Check Sem Info";
                return;
            }

            string SchOrder = d2.GetFunction("select distinct top 1 schOrder from PeriodAttndSchedule");
            dayorder = Convert.ToInt32(SchOrder);

            if (dayorder == 1)
            {
                dtTTSel.Columns.Add("Day");
                dtTTSel.Columns.Add("DayVal");
                for (int i = 1; i <= noofdays; i++)
                {
                    switch (i)
                    {
                        case 1:
                            day = "Monday";
                            break;
                        case 2:
                            day = "Tuesday";
                            break;
                        case 3:
                            day = "Wednesday";
                            break;
                        case 4:
                            day = "Thursday";
                            break;
                        case 5:
                            day = "Friday";
                            break;
                        case 6:
                            day = "Saturday";
                            break;
                        case 7:
                            day = "Sunday";
                            break;
                    }
                    DataRow dr = dtTTSel.NewRow();
                    dr["Day"] = day;
                    dr["DayVal"] = i;
                    dtTTSel.Rows.Add(dr);
                }

                ddlDayOrder.DataSource = dtTTSel;
                ddlDayOrder.DataTextField = "Day";
                ddlDayOrder.DataValueField = "DayVal";
                ddlDayOrder.DataBind();
            }
            else
            {
                dtTTSel.Columns.Add("Day");
                dtTTSel.Columns.Add("DayVal");

                for (int day1 = 0; day1 < noofdays; day1++)
                {
                    DataRow dr = dtTTSel.NewRow();

                    int daysetweek = day1 + 2;

                    if (day1 == noofdays)
                    {
                        daysetweek = 1;
                    }
                    if (!holiday.Contains(daysetweek.ToString()))
                    {
                        if (dayorder == 1)
                        {
                        }
                        else
                        {
                            date = day1 + 1;
                            dr["Day"] = "Day" + " " + date;
                            dr["DayVal"] = date;
                            dtTTSel.Rows.Add(dr);
                        }
                    }
                }
                ddlDayOrder.DataSource = dtTTSel;
                ddlDayOrder.DataValueField = "DayVal";
                ddlDayOrder.DataTextField = "Day";
                ddlDayOrder.DataBind();
            }
        }
        catch
        { }
    }

    protected string getSpreadCellValue(string strScheduledHour, string strSemSchedule)
    {
        try
        {
            string strSubName = "";
            string textValue = "";
            string noteValue = "";
            string subjectNo = strScheduledHour.Split('-')[0];
            string[] arr = strSemSchedule.Split(',');

            string sec = Convert.ToString(arr[5]).Trim();
            string strsec = "";

            if (sec != "" && sec != "-1" && sec != "all" && sec != null)
            {

                strsec = "and r.sections='" + sec + "'";
            }

            string subType = "S";
            string subj_type = d2.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + Convert.ToString(subjectNo) + "'");
            if (subj_type == "1" || subj_type.ToLower().Trim() == "true")
            {
                subType = "L";
            }

            string qry = "select distinct (CONVERT(varchar,r.Batch_Year)+'-'+c.Course_Name+' ('+de.dept_acronym+')-'+'Sem'+CONVERT(varchar, r.Current_Semester)+' '+ISNULL(r.Sections,''))Degree  from Registration r,Degree d,Department de,course c where r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0  and r.degree_code='" + Convert.ToString(arr[0]).Trim() + "' and r.Batch_Year='" + Convert.ToString(arr[2]).Trim() + "' and r.Current_Semester='" + Convert.ToString(arr[1]).Trim() + "'" + strsec + " ";//and r.college_code='" + Convert.ToString(collegecode).Trim() + "'

            textValue = d2.GetFunction(qry);

            strSubName = (d2.GetFunction("select subject_name from subject where subject_no=" + Convert.ToString(subjectNo) + " "));
            noteValue = Convert.ToString(strScheduledHour) + "," + strSemSchedule;

            return strSubName + "-" + subType + "-" + textValue + "#" + noteValue;
        }
        catch
        {
            return null;
        }
    }

    protected void btnGo_OnClick(object sender, EventArgs e)
    {
        try
        {
            #region spread design
            spreadTimeTable.Sheets[0].AutoPostBack = true;
            spreadTimeTable.Sheets[0].ColumnHeader.RowCount = 1;
            spreadTimeTable.Sheets[0].ColumnCount = 1;
            spreadTimeTable.Sheets[0].RowCount = 0;
            spreadTimeTable.CommandBar.Visible = false;
            spreadTimeTable.Sheets[0].RowHeader.Visible = false;
            spreadTimeTable.Columns.Default.Font.Name = "Book Antiqua";
            spreadTimeTable.Columns.Default.Font.Size = FontUnit.Medium;
            FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
            style.Font.Size = FontUnit.Medium;
            style.Font.Bold = true;
            style.Font.Name = "Book Antiqua";
            style.HorizontalAlign = HorizontalAlign.Center;
            style.ForeColor = Color.Black;
            style.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            spreadTimeTable.Sheets[0].ColumnHeader.DefaultStyle = style;
            spreadTimeTable.Sheets[0].Columns.Default.Font.Name = "Book Antiqua";
            spreadTimeTable.Sheets[0].Columns.Default.Font.Size = FontUnit.Medium;
            spreadTimeTable.Sheets[0].Rows.Default.Font.Name = "Book Antiqua";
            spreadTimeTable.Sheets[0].Rows.Default.Font.Size = FontUnit.Medium;
            spreadTimeTable.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Day/Week";
            spreadTimeTable.Sheets[0].Columns[0].ForeColor = Color.Black;
            spreadTimeTable.Sheets[0].Columns[0].Locked = true;
            #endregion

            string degCode = string.Empty;
            string deg = Convert.ToString(ddlSchinfo.SelectedValue);
            string sem = Convert.ToString(ddlsem.SelectedValue);
            string[] val1 = deg.Split('-');
            if (val1.Length > 1)
                degCode = Convert.ToString(val1[1]);

            if (Convert.ToString(Session["Staff_Code"]) == "")
            {
                if (Convert.ToString(ddlSearchOption.SelectedValue).Trim() != "")
                    Session["StaffCode"] = Convert.ToString(ddlSearchOption.SelectedValue).Trim();
                else
                {
                    string staff_Name = Convert.ToString(Convert.ToString(ddlSearchOption.SelectedValue)).Trim();
                    if (staff_Name != "")
                    {
                        string staff_Code = d2.GetFunction("select staff_code from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_name like '%" + staff_Name + "%' and college_code='" + collegecode + "'");
                        Session["StaffCode"] = staff_Code.Trim();
                        ddlSearchOption.SelectedValue = staff_Code.Trim();
                    }
                }
            }

            htData.Clear();
            string[] DaysAcronym = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
            string[] DaysName = new string[7] { "Monday", "Tuesday", "wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            string sql = "select max(No_of_hrs_per_day)HoursPerDay,MAX(nodays)NoOfDays from PeriodAttndSchedule where degree_code='" + degCode + "' and  semester='" + sem + "'";
            DataSet ds = d2.select_method_wo_parameter(sql, "Text");
            int noOfHrs = 0;
            int noOfDays = 0;
            string dayvalue = string.Empty;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["HoursPerDay"].ToString().Trim() != "" && ds.Tables[0].Rows[0]["HoursPerDay"].ToString().Trim() != null && ds.Tables[0].Rows[0]["HoursPerDay"].ToString().Trim() != "0")
                {
                    noOfHrs = Convert.ToInt32(ds.Tables[0].Rows[0]["HoursPerDay"].ToString());
                    noOfDays = Convert.ToInt32(ds.Tables[0].Rows[0]["NoOfDays"].ToString());
                }
            }
            else
            {

            }
            if (noOfHrs != 0)
            {
                for (int i = 1; i <= noOfHrs; i++)
                {
                    spreadTimeTable.Sheets[0].ColumnCount = spreadTimeTable.Sheets[0].ColumnCount + 1;
                    spreadTimeTable.Sheets[0].ColumnHeader.Cells[0, spreadTimeTable.Sheets[0].ColumnCount - 1].Text = "Period " + Convert.ToString(i);
                }

            }

            string SchOrder = d2.GetFunction("select distinct top 1 schOrder from PeriodAttndSchedule");
            for (int day = 0; day < noOfDays; day++)
            {
                string dayName = DaysName[day];
                string dayAcronym = DaysAcronym[day];
                spreadTimeTable.Sheets[0].RowCount++;
                if (SchOrder == "1")
                {
                    spreadTimeTable.Sheets[0].Cells[spreadTimeTable.Sheets[0].RowCount - 1, 0].Text = dayName;
                    spreadTimeTable.Sheets[0].Cells[spreadTimeTable.Sheets[0].RowCount - 1, 0].Note = dayAcronym;
                }
                else
                {
                    int dayNo = day + 1;
                    spreadTimeTable.Sheets[0].Cells[spreadTimeTable.Sheets[0].RowCount - 1, 0].Text = "Day " + dayNo;
                }
            }
            DateTime dt1 = new DateTime();
            string fDate = string.Empty;
            bool isval = DateTime.TryParseExact(txtFromDate.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dt1);
            if (isval)
                fDate = "  and FromDate>='" + dt1.ToString("MM/dd/yyyy") + "' ";

            DateTime cur_date = DateTime.Now;
            string strCurrDate = Convert.ToString(cur_date).Split(new Char[] { ' ' })[0];
            DataSet dsAllDetails = new DataSet();
            string qryGetDegDetails = "";
            qryGetDegDetails = " select distinct r.Batch_Year,r.degree_code,sy.semester,r.Sections,si.end_date from staff_selector ss,Registration r,";
            qryGetDegDetails = qryGetDegDetails + " subject s,sub_sem sm,syllabus_master sy,seminfo si where sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code";
            qryGetDegDetails = qryGetDegDetails + " and sy.semester=r.Current_Semester and sy.syll_code=sm.syll_code and sm.subType_no=s.subType_no ";
            qryGetDegDetails = qryGetDegDetails + " and s.subject_no=ss.subject_no and isnull(r.sections,'')=isnull(ss.sections,'') and ss.batch_year=r.Batch_Year";
            qryGetDegDetails = qryGetDegDetails + " and si.Batch_Year=r.Batch_Year and si.degree_code=r.degree_code and si.semester=r.Current_Semester and ";
            qryGetDegDetails = qryGetDegDetails + " si.Batch_Year=sy.Batch_Year and sy.degree_code=r.degree_code and si.semester=sy.Semester and r.CC=0 and r.Exam_Flag<>'debar'";
            qryGetDegDetails = qryGetDegDetails + " and r.DelFlag=0 and ss.staff_code='" + Convert.ToString(Session["StaffCode"]) + "' union select distinct r.Batch_Year,r.degree_code,sy.semester,r.Sections,si.end_date from alternateStaffDetails asd,Registration r,sub_sem sm,syllabus_master sy,seminfo si, subject s  where sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and sy.semester=r.Current_Semester and sy.syll_code=sm.syll_code and sm.subType_no=s.subType_no  and s.subject_no=asd.subjectNo and si.Batch_Year=r.Batch_Year and si.degree_code=r.degree_code and si.semester=r.Current_Semester and  si.Batch_Year=sy.Batch_Year and sy.degree_code=r.degree_code and si.semester=sy.Semester and r.CC=0 and r.Exam_Flag<>'debar' and r.DelFlag=0 and asd.alterStaffCode='" + Convert.ToString(Session["StaffCode"]) + "'";
            DataSet dsDegreeDetails = d2.select_method_wo_parameter(qryGetDegDetails, "Text");

            // string qryAllDetails = "select * from Semester_Schedule order by FromDate desc;";
            string qryAllDetails = " select * from Semester_Schedule where (mon1 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or mon2 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or mon3 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or mon4 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or mon5 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or mon6 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or mon7 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or mon8 like '%" + Convert.ToString(Session["StaffCode"]) + "%') or (tue1 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or tue2 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or tue3 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or tue4 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or tue5 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or tue6 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or tue7 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or tue8 like '%" + Convert.ToString(Session["StaffCode"]) + "%') or (wed1 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or wed2 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or wed3 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or wed4 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or wed5 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or wed6 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or wed7 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or wed8 like '%" + Convert.ToString(Session["StaffCode"]) + "%') or (thu1 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or thu2 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or thu3 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or thu4 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or thu5 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or thu6 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or thu7 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or thu8 like '%" + Convert.ToString(Session["StaffCode"]) + "%') or (fri1 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or fri2 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or fri3 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or fri4 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or fri5 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or fri6 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or fri7 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or fri8 like '%" + Convert.ToString(Session["StaffCode"]) + "%') or (sat1 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sat2 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sat3 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sat4 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sat5 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sat6 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sat7 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sat8 like '%" + Convert.ToString(Session["StaffCode"]) + "%') or (sun1 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sun2 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sun3 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sun4 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sun5 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sun6 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sun7 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sun8 like '%" + Convert.ToString(Session["StaffCode"]) + "%')" + fDate + " order by FromDate desc";

            // qryAllDetails = qryAllDetails + "select * from Alternate_Schedule order by FromDate desc;";

            dsAllDetails = d2.select_method_wo_parameter(qryAllDetails, "Text");


            DataView dvSemTT = new DataView();
            DataView dvAlternateSemTT = new DataView();
            Hashtable hat = new Hashtable();
            if (dsDegreeDetails.Tables.Count > 0 && dsDegreeDetails.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsDegreeDetails.Tables[0].Rows.Count; i++)
                {
                    string strSec = string.Empty;
                    if (dsDegreeDetails.Tables[0].Rows[i]["sections"].ToString() != "-1" && dsDegreeDetails.Tables[0].Rows[i]["sections"].ToString() != null && dsDegreeDetails.Tables[0].Rows[i]["sections"].ToString().Trim() != "")
                    {
                        strSec = "and Sections='" + dsDegreeDetails.Tables[0].Rows[i]["sections"].ToString() + "'";
                    }

                    if (dsAllDetails.Tables.Count > 0)
                    {
                        bool checkRow = false;
                        if (dsAllDetails.Tables[0].Rows.Count > 0)
                        {
                            string strDegDetails = "";
                            dsAllDetails.Tables[0].DefaultView.RowFilter = "batch_year='" + dsDegreeDetails.Tables[0].Rows[i]["batch_year"].ToString() + "' and degree_code='" + dsDegreeDetails.Tables[0].Rows[i]["degree_code"].ToString() + "' and semester='" + dsDegreeDetails.Tables[0].Rows[i]["semester"].ToString() + "' " + strSec + " and FromDate<='" + strCurrDate.ToString() + "'";
                            dvSemTT = dsAllDetails.Tables[0].DefaultView;
                            checkRow = false;
                            if (!hat.ContainsKey((dsDegreeDetails.Tables[0].Rows[i]["batch_year"].ToString() + "-" + dsDegreeDetails.Tables[0].Rows[i]["degree_code"].ToString() + "-" + dsDegreeDetails.Tables[0].Rows[i]["semester"].ToString() + "-" + strSec)))
                            {
                                hat.Add(dsDegreeDetails.Tables[0].Rows[i]["batch_year"].ToString() + "-" + dsDegreeDetails.Tables[0].Rows[i]["degree_code"].ToString() + "-" + dsDegreeDetails.Tables[0].Rows[i]["semester"].ToString() + "-" + strSec, dsDegreeDetails.Tables[0].Rows[i]["degree_code"].ToString());

                                if (dvSemTT.Count > 0)
                                {
                                    strDegDetails = Convert.ToString(dvSemTT[0]["degree_code"]) + "," + Convert.ToString(dvSemTT[0]["semester"]) + "," + Convert.ToString(dvSemTT[0]["batch_year"]) + "," + Convert.ToString(dvSemTT[0]["ttname"]) + "," + Convert.ToString(dvSemTT[0]["fromdate"]).Split(' ')[0] + "," + Convert.ToString(dvSemTT[0]["sections"]);

                                    if (checkRow == false)
                                    {
                                        for (int day = 0; day < noOfDays; day++)
                                        {
                                            for (int hr = 1; hr <= noOfHrs; hr++)
                                            {
                                                string str = DaysAcronym[day].ToString() + hr;

                                                string val = Convert.ToString(dvSemTT[0][str]);
                                                if (!string.IsNullOrEmpty(val))
                                                {
                                                    if (val.Contains(Convert.ToString((Session["StaffCode"]))))
                                                    {
                                                        string row = "";
                                                        switch (DaysAcronym[day].ToString())
                                                        {
                                                            case "mon":
                                                                row = "0";
                                                                break;
                                                            case "tue":
                                                                row = "1";
                                                                break;
                                                            case "wed":
                                                                row = "2";
                                                                break;
                                                            case "thu":
                                                                row = "3"; break;
                                                            case "fri":
                                                                row = "4"; break;
                                                            case "sat":
                                                                row = "5"; break;
                                                            case "sun":
                                                                row = "6";
                                                                break;

                                                        }
                                                        string spreadCellValue = "";
                                                        if (val.Contains(';'))
                                                        {
                                                            string[] arr = val.Split(';');
                                                            for (int k = 0; k < arr.Length; k++)
                                                            {
                                                                if (arr[k].Contains(Convert.ToString((Session["StaffCode"]))))
                                                                {
                                                                    if (spreadCellValue == "")
                                                                        //spreadCellValue = Convert.ToString(arr[k]);
                                                                        spreadCellValue = getSpreadCellValue(Convert.ToString(arr[k]), strDegDetails);
                                                                    else
                                                                        spreadCellValue = spreadCellValue + ";" + getSpreadCellValue(Convert.ToString(arr[k]), strDegDetails);
                                                                }
                                                            }

                                                        }
                                                        else
                                                        {
                                                            //spreadCellValue = val;
                                                            spreadCellValue = getSpreadCellValue(val, strDegDetails);
                                                        }

                                                        if (!htData.ContainsKey(row + hr))
                                                        {
                                                            htData.Add(row + hr, spreadCellValue);
                                                        }
                                                        else
                                                        {
                                                            string oldValue = Convert.ToString(htData[row + hr]);
                                                            //if (oldValue.Split('#')[1] != spreadCellValue.Split('#')[1])
                                                            //{
                                                            spreadCellValue = spreadCellValue + ";" + oldValue;
                                                            //  }
                                                            htData.Remove(row + hr);
                                                            htData.Add(row + hr, spreadCellValue);
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        checkRow = true;
                                    }
                                }

                            }

                        }

                    }
                }
            }

            for (int row = 0; row < noOfDays; row++)
            {
                string r = row.ToString();
                for (int col = 1; col <= noOfHrs; col++)
                {
                    string cellValue = "";
                    string cellNoteValue = "";
                    string c = col.ToString();
                    if (htData.ContainsKey(r + c))
                    {
                        if (Convert.ToString(htData[r + c]).Contains(';'))
                        {
                            string[] arr = Convert.ToString(htData[r + c]).Split(';');
                            for (int k = 0; k < arr.Length; k++)
                            {
                                string[] val = Convert.ToString(arr[k]).Split('#');

                                if (cellValue == "")
                                {
                                    cellValue = val[0];
                                    cellNoteValue = val[1];
                                }
                                else
                                {
                                    cellValue = cellValue + ";" + val[0];
                                    cellNoteValue = cellNoteValue + ";" + val[1];
                                }
                            }
                        }
                        else
                        {
                            string[] val = Convert.ToString(htData[r + c]).Split('#');
                            cellValue = val[0];
                            cellNoteValue = val[1];
                        }

                        spreadTimeTable.Sheets[0].Cells[row, col].Text = cellValue;
                        spreadTimeTable.Sheets[0].Cells[row, col].Note = cellNoteValue;
                        spreadTimeTable.Sheets[0].Cells[row, col].HorizontalAlign = HorizontalAlign.Left;
                    }
                }
            }
            spreadTimeTable.SaveChanges();
            spreadTimeTable.Visible = true;
            btnPrint.Visible = true;
            //btndelete.Visible = true;

        }
        catch
        {

        }
    }

    protected void btnAdd_OnClick(object sender, EventArgs e)
    {
        try
        {
            spreadTimeTable.SaveChanges();
            string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
            string splval = string.Empty;
            string subno_staff = string.Empty;
            string subno_staffnote = string.Empty;
            //string activerow = spreadTimeTable.ActiveSheetView.ActiveRow.ToString();
            //string activecol = spreadTimeTable.ActiveSheetView.ActiveColumn.ToString();
            string dt = DateTime.Now.ToString("dd/MM/yyyy");
            string[] date = txtFromDate.Text.Split('/');
            string fromdate = date[1] + '/' + date[0] + '/' + date[2];
            string staffName = "";
            string staffCode = "";
            string qry = "";
            string tablevalue = string.Empty;
            string ttName = "";
            Hashtable hatdegree = new Hashtable();
            string history_data = string.Empty;
            string SubCode = Convert.ToString(ddlSubject.SelectedValue);
            string[] arr = Convert.ToString(ddlSchinfo.SelectedValue).Split('-');

            string selectedBatch = Convert.ToString(arr[0]).Trim();
            string selectedDegCode = Convert.ToString(arr[1]).Trim();
            string selectedSem = Convert.ToString(arr[2]).Trim();
            string selectedSec = Convert.ToString(arr[3]).Trim();

            if (Convert.ToString(ddltimetable.SelectedItem.Text).ToLower().Trim() == "new" && string.IsNullOrEmpty(Convert.ToString(txttimetable.Text)))
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Pls Enter time table Name";
                return;
            }
            if (string.IsNullOrEmpty(Convert.ToString(ddltimetable.SelectedItem.Text).Trim()))
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Pls select time table";
                return;
            }
            if (!string.IsNullOrEmpty(ddlSearchOption.SelectedValue))
            {
                staffCode = Convert.ToString(ddlSearchOption.SelectedValue).Trim();
                staffName = d2.GetFunction("select staff_name from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_code like '%" + staffCode + "%' and college_code='" + collegecode + "'");
            }
            //else if (ddlSearchOption.SelectedIndex == 1)
            //{
            //    staffName = Convert.ToString(ddlSearchOption.SelectedValue).Trim();
            //    staffCode = d2.GetFunction("select staff_code from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_name like '%" + staffName + "%' and college_code='" + collegecode + "'");
            //}
            Session["StaffCode"] = staffCode;
            if (!string.IsNullOrEmpty(ddlSchinfo.SelectedItem.Text))
            {
                if (!string.IsNullOrEmpty(Convert.ToString(txttimetable.Text)))
                    ttName = Convert.ToString(txttimetable.Text);
                else
                    ttName = ddltimetable.SelectedItem.Text.Trim();
            }
            if (!string.IsNullOrEmpty(ttName))
            {

                string subTypeNo = d2.GetFunction("select ss.Lab from subject s,syllabus_master sm,sub_sem ss where ss.syll_code=sm.syll_code and s.subType_no=ss.subType_no and s.syll_code=sm.syll_code and sm.Batch_Year='" + selectedBatch + "' and sm.degree_code='" + selectedDegCode + "' and sm.semester='" + selectedSem + "'' and s.subject_code='" + SubCode + "'");
                string subNo = d2.GetFunction("select s.subject_no from subject s,syllabus_master sm,sub_sem ss where ss.syll_code=sm.syll_code and s.subType_no=ss.subType_no and s.syll_code=sm.syll_code and sm.Batch_Year='" + selectedBatch + "' and sm.degree_code='" + selectedDegCode + "' and sm.semester='" + selectedSem + "' and s.subject_code='" + SubCode + "'");

                if (subTypeNo == "1")
                    subTypeNo = "L";
                else
                    subTypeNo = "S";

                string dayOrder = Convert.ToString(ddlDayOrder.SelectedValue);
                string col = Convert.ToString(ddlHour.SelectedValue);
                int row = 0;
                int.TryParse(dayOrder, out row);
                string Daycoulmn = string.Empty;
                string Daycoulmnvalue = string.Empty;//995-CSET009-CSET028-CSET503-L
                string dayofweek = Days[row - 1];
                Daycoulmn = dayofweek + Convert.ToString(col);
                Daycoulmnvalue = subNo + "-" + staffCode + "-" + subTypeNo;
                string appndColumn = string.Empty;
                string secval = string.Empty;
                if (!string.IsNullOrEmpty(selectedSec))
                    secval = "  and Sections='" + Convert.ToString(selectedSec) + "'";

                string StaffChk = "select s.staff_name,s.staff_code from staff_selector sm,staffmaster s where  s.staff_code=sm.staff_code and sm.subject_no='" + subNo + "'  and sm.batch_year='" + selectedBatch + "'" + secval;
                DataTable dtEx = dir.selectDataTable(StaffChk);
                string staff = string.Empty;
                if (!staffApnd)
                {
                    if (dtEx.Rows.Count > 0)
                    {

                        foreach (DataRow dr in dtEx.Rows)
                        {
                            string sCode = Convert.ToString(dr["staff_code"]);
                            string Sname = Convert.ToString(dr["staff_name"]);
                            if (sCode != staffCode)
                            {
                                if (string.IsNullOrEmpty(staff))
                                    staff = sCode + "-" + Sname;
                                else
                                    staff = staff + " & " + sCode + "-" + Sname;
                            }
                        }
                        if (!string.IsNullOrEmpty(staff))
                        {
                            Label8.Text = "Already Exists:" + staff;
                            ModalPopupExtender1.Show();
                            goto lable1;
                        }
                    }
                }
                string IncStaffSel = "  if not exists(select * from staff_selector where subject_no='" + subNo + "' and staff_code='" + staffCode + "' and batch_year='" + selectedBatch + "' and isnull(Sections,'')='" + selectedSec + "')  insert into  staff_selector (subject_no,staff_code,batch_year,dailyflag,Sections) values ('" + subNo + "','" + staffCode + "','" + selectedBatch + "','0','" + selectedSec + "')";
                int d = d2.update_method_wo_parameter(IncStaffSel, "text");

                if (!string.IsNullOrEmpty(Daycoulmnvalue) && !string.IsNullOrEmpty(selectedDegCode) && !string.IsNullOrEmpty(selectedSem) && !string.IsNullOrEmpty(selectedBatch) && !string.IsNullOrEmpty(Daycoulmn) && !string.IsNullOrEmpty(Daycoulmnvalue) && !string.IsNullOrEmpty(ttName) && !string.IsNullOrEmpty(fromdate))
                {


                    string existingColValue = d2.GetFunction("select " + Daycoulmn + " from Semester_Schedule where degree_code='" + Convert.ToString(selectedDegCode) + "' and batch_year='" + Convert.ToString(selectedBatch) + "' and semester='" + Convert.ToString(selectedSem) + "' " + secval + " and TTName='" + ttName + "' and FromDate='" + fromdate + "'");
                    if (!string.IsNullOrEmpty(existingColValue) && existingColValue != "0")
                        appndColumn = existingColValue + ";" + Daycoulmnvalue;
                    else
                        appndColumn = Daycoulmnvalue;

                    string staffsubject = getstaffStatus(Daycoulmn);

                    if (existingColValue != "" && existingColValue != "0" && existingColValue != null)
                    {
                        string oldsch = getSemesterSch(existingColValue, Daycoulmn, selectedBatch, selectedDegCode, selectedSem, selectedSec);
                        if (!isChanged)
                        {
                            lblErrmsg.Text = "Already Exists:  " + oldsch;
                            alert2PopUp.Show();
                            goto lable1;
                        }
                        if (replace)
                        {
                            status = insertRecord(selectedDegCode, selectedSem, selectedBatch, Daycoulmn, Daycoulmnvalue, ttName, fromdate, selectedSec);
                        }
                        if (appand)
                        {
                            status = insertRecord(selectedDegCode, selectedSem, selectedBatch, Daycoulmn, appndColumn, ttName, fromdate, selectedSec);
                        }
                    }
                    else if (!string.IsNullOrEmpty(staffsubject) && staffsubject != "-")
                    {
                        if (!isChanged)
                        {
                            lblErrmsg.Text = "Staff is Busy: " + staffsubject;
                            alert2PopUp.Show();
                            goto lable1;
                        }
                        if (allowCombineClass)
                        {
                            string selectQ = d2.GetFunction("select " + Daycoulmn + " from Semester_Schedule where " + Daycoulmn + " like '%" + Convert.ToString(Session["StaffCode"]) + "%'  order by FromDate desc");
                            if (!string.IsNullOrEmpty(selectQ))
                                appndColumn = Daycoulmnvalue;
                            status = insertRecord(selectedDegCode, selectedSem, selectedBatch, Daycoulmn, appndColumn, ttName, fromdate, selectedSec);

                        }
                    }
                    if (!replace && !appand && !allowCombineClass)
                        status = insertRecord(selectedDegCode, selectedSem, selectedBatch, Daycoulmn, Daycoulmnvalue, ttName, fromdate, selectedSec);
                }
                else
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Input Not Valid";
                    return;
                }
                if (status != 0)
                {
                    spreadTimeTable.Visible = true;
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Saved Sucessfully";
                    btnGo_OnClick(sender, e);
                }
                else
                {
                    spreadTimeTable.Visible = true;
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Not Saved";
                    btnGo_OnClick(sender, e);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Enter Time Table Name')", true);
                return;
            }
        lable1: ;
        }
        catch { }
    }

    protected void btnUpdate_OnClick(object sender, EventArgs e)
    {
        alert2PopUp.Hide();
        try
        {
            isChanged = true;
            replace = false;
            appand = true;
            allowCombineClass = true;
            staffApnd = true;
            btnAdd_OnClick(sender, e);

        }
        catch
        {
        }
    }

    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        try
        {
            alert2PopUp.Hide();
            return;
        }
        catch
        {
        }
    }

    protected void btnReplace_OnClick(object sender, EventArgs e)
    {
        alert2PopUp.Hide();
        try
        {
            isChanged = true;
            replace = true;
            appand = false;
            staffApnd = true;
            btnAdd_OnClick(sender, e);
        }
        catch { }
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

        }
    }

    protected void btnPrint_OnClick(object sender, EventArgs e)
    {

    }

    protected int insertRecord(string degCode, string sem, string batch, string colName, string colVal, string ttName, string ttDate, string sec)
    {
        try
        {
            int status = 0;
            string columnValue = "";
            string secval = string.Empty;
            if (!string.IsNullOrEmpty(sec))
                secval = "  and Sections='" + Convert.ToString(sec) + "'";




            string existingColValue = d2.GetFunction("select " + colName + " from Semester_Schedule where degree_code='" + Convert.ToString(degCode) + "' and batch_year='" + Convert.ToString(batch) + "' and semester='" + Convert.ToString(sem) + "'  and TTName='" + ttName + "'" + secval + " and FromDate='" + ttDate + "'");

            if (existingColValue != "" && existingColValue != "0" && existingColValue != null)
            {
                if (existingColValue.Contains(Session["StaffCode"].ToString()))
                {
                    columnValue = colVal;
                    //if (existingColValue.Contains(';'))
                    //{
                    //    string temp = "";
                    //    string[] arrVal = existingColValue.Split(';');
                    //    for (int i = 0; i < arrVal.Length; i++)
                    //    {
                    //        string val = arrVal[i];
                    //        if (val.Contains(Session["StaffCode"].ToString()))
                    //        {
                    //            if (temp == "")
                    //                temp = colVal;
                    //            else
                    //                temp = temp + ";" + colVal;
                    //        }
                    //        else
                    //        {
                    //            if (temp == "")
                    //                temp = val;
                    //            else
                    //                temp = temp + ";" + val;
                    //        }
                    //    }
                    //    columnValue = temp;
                    //}
                    //else
                    //{
                    //    columnValue = colVal;
                    //}
                }
                else
                {
                    columnValue = existingColValue + ";" + colVal;
                }
            }
            else
            {
                columnValue = colVal;
            }
            string insertQuery = "";

            if (sec != "")
            {
                insertQuery = " if exists(select * from Semester_Schedule where degree_code='" + Convert.ToString(degCode) + "' and batch_year='" + Convert.ToString(batch) + "' and semester='" + Convert.ToString(sem) + "' and Sections='" + Convert.ToString(sec) + "' and TTName='" + ttName + "' and FromDate='" + ttDate + "'  ) update Semester_Schedule set " + colName + "='" + columnValue + "' where degree_code='" + Convert.ToString(degCode) + "' and batch_year='" + Convert.ToString(batch) + "' and semester='" + Convert.ToString(sem) + "' and Sections='" + Convert.ToString(sec) + "' and TTName='" + ttName + "' and FromDate='" + ttDate + "' else  insert into Semester_Schedule (degree_code,batch_year,semester,sections,TTName,FromDate," + colName + ",lastrec) values(" + Convert.ToString(degCode) + "," + Convert.ToString(batch) + "," + Convert.ToString(sem) + ",'" + Convert.ToString(sec) + "','" + ttName + "','" + ttDate + "','" + columnValue + "',1)";
            }
            else
            {
                insertQuery = " if exists(select * from Semester_Schedule where degree_code='" + Convert.ToString(degCode) + "' and batch_year='" + Convert.ToString(batch) + "' and semester='" + Convert.ToString(sem) + "' and TTName='" + ttName + "' and FromDate='" + ttDate + "'  ) update Semester_Schedule set " + colName + "='" + columnValue + "' where degree_code='" + Convert.ToString(degCode) + "' and batch_year='" + Convert.ToString(batch) + "' and semester='" + Convert.ToString(sem) + "' and TTName='" + ttName + "' and FromDate='" + ttDate + "' else  insert into Semester_Schedule (degree_code,batch_year,semester,TTName,FromDate," + colName + ",lastrec) values(" + Convert.ToString(degCode) + "," + Convert.ToString(batch) + "," + Convert.ToString(sem) + ",'" + ttName + "','" + ttDate + "','" + columnValue + "',1)";
            }
            status = d2.update_method_wo_parameter(insertQuery, "Text");
            return status;
        }
        catch
        {
            return 0;
        }
    }

    protected string getSemesterSch(string strScheduledHour, string day, string batch, string degcod, string sem, string sec)
    {
        try
        {
            string strSubName = "";
            string textValue = "";
            string noteValue = "";
            string subjectNo = strScheduledHour.Split('-')[0];
            string strsec = "";

            if (sec != "" && sec != "-1" && sec != "all" && sec != null)
            {

                strsec = "and r.sections='" + sec + "'";
            }

            string subType = "S";
            string subj_type = d2.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + Convert.ToString(subjectNo) + "'");
            if (subj_type == "1" || subj_type.ToLower().Trim() == "true")
            {
                subType = "L";
            }

            string qry = "select distinct (CONVERT(varchar,r.Batch_Year)+'-'+c.Course_Name+' ('+de.dept_acronym+')-'+'Sem'+CONVERT(varchar, r.Current_Semester)+' '+ISNULL(r.Sections,''))Degree  from Registration r,Degree d,Department de,course c where r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0  and r.degree_code='" + Convert.ToString(degcod).Trim() + "' and r.Batch_Year='" + Convert.ToString(batch).Trim() + "' and r.Current_Semester='" + Convert.ToString(sem).Trim() + "'" + strsec + " and r.college_code='" + Convert.ToString(collegecode).Trim() + "'";

            textValue = d2.GetFunction(qry);
            strSubName = (d2.GetFunction("select subject_name from subject where subject_no=" + Convert.ToString(subjectNo) + " "));
            noteValue = Convert.ToString(strScheduledHour) + "," + sem;

            return strSubName + "-" + subType + "-" + textValue;//"#" + noteValue
        }
        catch
        {
            return null;
        }
    }

    protected int deleteRecord(string degCode, string sem, string batch, string colName, string ttName, string ttDate, string sec)
    {
        try
        {
            int status = 0;
            string columnValue = "";

            string existingColValue = d2.GetFunction("select " + colName + " from Semester_Schedule where degree_code='" + Convert.ToString(degCode) + "' and batch_year='" + Convert.ToString(batch) + "' and semester='" + Convert.ToString(sem) + "' and Sections='" + Convert.ToString(sec) + "' and TTName='" + ttName + "' and FromDate='" + ttDate + "'");

            if (existingColValue != "" && existingColValue != "0" && existingColValue != null)
            {
                if (existingColValue.Contains(Session["StaffCode"].ToString()))
                {
                    if (existingColValue.Contains(';'))
                    {
                        string temp = "";
                        string[] arrVal = existingColValue.Split(';');
                        for (int i = 0; i < arrVal.Length; i++)
                        {
                            string val = arrVal[i];
                            if (!val.Contains(Session["StaffCode"].ToString()))
                            {
                                if (temp == "")
                                    temp = val;
                                else
                                    temp = temp + ";" + val;
                            }
                        }
                        columnValue = temp;
                    }
                    else
                    {
                        columnValue = null;
                    }
                }
                else
                {
                    columnValue = existingColValue;
                }
            }
            string query = "";
            if (sec != "")
            {
                query = " if exists(select * from Semester_Schedule where degree_code='" + Convert.ToString(degCode) + "' and batch_year='" + Convert.ToString(batch) + "' and semester='" + Convert.ToString(sem) + "' and Sections='" + Convert.ToString(sec) + "' and TTName='" + ttName + "' and FromDate='" + ttDate + "'  ) update Semester_Schedule set " + colName + "='" + columnValue + "' where degree_code='" + Convert.ToString(degCode) + "' and batch_year='" + Convert.ToString(batch) + "' and semester='" + Convert.ToString(sem) + "' and Sections='" + Convert.ToString(sec) + "' and TTName='" + ttName + "' and FromDate='" + ttDate + "'";
            }
            else
            {
                query = " if exists(select * from Semester_Schedule where degree_code='" + Convert.ToString(degCode) + "' and batch_year='" + Convert.ToString(batch) + "' and semester='" + Convert.ToString(sem) + "' and TTName='" + ttName + "' and FromDate='" + ttDate + "'  ) update Semester_Schedule set " + colName + "='" + columnValue + "' where degree_code='" + Convert.ToString(degCode) + "' and batch_year='" + Convert.ToString(batch) + "' and semester='" + Convert.ToString(sem) + "' and TTName='" + ttName + "' and FromDate='" + ttDate + "'";
            }
            status = d2.update_method_wo_parameter(query, "Text");

            return status;
        }
        catch { return 0; }
    }

    protected string getstaffStatus(string dayval)
    {
        try
        {
            string subjectDegree = string.Empty;
            string textValue = string.Empty;
            string strSubName = string.Empty;
            string selectQ1 = "select * from Semester_Schedule where " + dayval + " like '%" + Convert.ToString(Session["StaffCode"]) + "%'  order by FromDate desc";
            string selectQ = d2.GetFunction("select " + dayval + " from Semester_Schedule where " + dayval + " like '%" + Convert.ToString(Session["StaffCode"]) + "%'  order by FromDate desc");
            DataTable dtstaffSub = dir.selectDataTable(selectQ1);

            if (dtstaffSub.Rows.Count > 0)
            {
                string degcode = Convert.ToString(dtstaffSub.Rows[0]["degree_code"]);
                string batch_year = Convert.ToString(dtstaffSub.Rows[0]["batch_year"]);
                string semester = Convert.ToString(dtstaffSub.Rows[0]["semester"]);
                string sections = Convert.ToString(dtstaffSub.Rows[0]["sections"]);
                string strsec = "";

                if (sections != "" && sections != "-1" && sections != "all" && sections != null)
                {

                    strsec = "and r.sections='" + sections + "'";
                }
                string qry = "select distinct (CONVERT(varchar,r.Batch_Year)+'-'+c.Course_Name+' ('+de.dept_acronym+')-'+'Sem'+CONVERT(varchar, r.Current_Semester)+' '+ISNULL(r.Sections,''))Degree  from Registration r,Degree d,Department de,course c where r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0  and r.degree_code='" + Convert.ToString(degcode).Trim() + "' and r.Batch_Year='" + Convert.ToString(batch_year).Trim() + "' and r.Current_Semester='" + Convert.ToString(semester).Trim() + "'" + strsec + " ";//and r.college_code='" + Convert.ToString(collegecode).Trim() + "'

                textValue = d2.GetFunction(qry);

                if (!string.IsNullOrEmpty(textValue) && textValue != "0")
                {
                    if (!string.IsNullOrEmpty(selectQ) && selectQ != "0")
                    {
                        if (selectQ.Contains(";"))
                        {
                            string temp = "";
                            string[] arrVal = selectQ.Split(';');
                            for (int i = 0; i < arrVal.Length; i++)
                            {
                                string val = arrVal[i];
                                if (val.Contains(Session["StaffCode"].ToString()))
                                {
                                    string[] subNo = val.Split('-');
                                    subjectDegree = Convert.ToString(subNo[0]);
                                    strSubName = (d2.GetFunction("select subject_name from subject where subject_no=" + Convert.ToString(subjectDegree) + " "));
                                }
                            }
                        }
                        else
                        {
                            string[] subNo = selectQ.Split('-');
                            subjectDegree = Convert.ToString(subNo[0]);
                            strSubName = (d2.GetFunction("select subject_name from subject where subject_no=" + Convert.ToString(subjectDegree) + " "));
                        }

                    }
                }
                else
                {
                    textValue = string.Empty;
                }
            }

            return strSubName + "-" + textValue;

        }
        catch
        {
            return null;
        }
    }

    protected void btndelete_OnClick(object sender, EventArgs e)
    {
        try
        {
            #region Old
            //int count = 0;
            //string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
            //string degCode = string.Empty;
            //string batchYear = string.Empty;
            //string sec = string.Empty;
            //string dayOrder = Convert.ToString(ddlDayOrder.SelectedValue);
            //string col = Convert.ToString(ddlHour.SelectedValue);
            //int row = 0;
            //int.TryParse(dayOrder, out row);
            //string deg = Convert.ToString(ddlSchinfo.SelectedValue);
            //string ttname = string.Empty;
            //string sem = Convert.ToString(ddlsem.SelectedValue);
            //string[] val = deg.Split('-');
            //if (val.Length > 1)
            //{
            //    degCode = Convert.ToString(val[1]);
            //    batchYear = Convert.ToString(val[0]);
            //    sec = Convert.ToString(val[3]);
            //}
            //if (Convert.ToString(ddltimetable.SelectedItem.Text).ToLower().Trim() == "new" && string.IsNullOrEmpty(Convert.ToString(txttimetable.Text)))
            //{
            //    divPopAlert.Visible = true;
            //    lblAlertMsg.Text = "Pls Enter time table Name";
            //    return;
            //}
            //if (string.IsNullOrEmpty(Convert.ToString(ddltimetable.SelectedItem.Text).Trim()))
            //{
            //    divPopAlert.Visible = true;
            //    lblAlertMsg.Text = "Pls select time table";
            //    return;
            //}
            //if (!string.IsNullOrEmpty(ddlSchinfo.SelectedItem.Text))
            //{
            //    if (!string.IsNullOrEmpty(Convert.ToString(txttimetable.Text)))
            //        ttname = Convert.ToString(txttimetable.Text);
            //    else
            //        ttname = ddltimetable.SelectedItem.Text.Trim();
            //}
            //string Daycoulmn = string.Empty;
            //string Daycoulmnvalue = string.Empty;//995-CSET009-CSET028-CSET503-L
            //string dayofweek = Days[row - 1];
            //Daycoulmn = dayofweek + Convert.ToString(col);
            //string date1 = txtFromDate.Text.ToString();
            //string[] date_fm = date1.Split(new Char[] { '/' });
            //string fmdate = date_fm[2].ToString() + "/" + date_fm[1].ToString() + "/" + date_fm[0].ToString();

            //if (!string.IsNullOrEmpty(degCode) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(ttname) && !string.IsNullOrEmpty(fmdate))
            //{
            //    count = deleteRecord(degCode, sem, batchYear, Daycoulmn, ttname, fmdate, sec);
            //}
            #endregion
            //if (cellClick == true)
            //{
            int count = 0;
            if (cblTime.Items.Count > 0)
            {
                for (int i = 0; i < cblTime.Items.Count; i++)
                {
                    if (cblTime.Items[i].Selected)
                    {
                        //dr2["Colval"] = Batch + "-" + deg + "-" + SEM + "-" + Daycoulmn + "-" + TTname + "-" + FDAte + "-" + Section;
                        string cblval = Convert.ToString(cblTime.Items[i].Value);
                        if (!string.IsNullOrEmpty(cblval))
                        {
                            string[] info = cblval.Split('-');
                            if (info.Count() > 0)
                            {
                                string batch = Convert.ToString(info[0]);
                                string deg = Convert.ToString(info[1]);
                                string sem = Convert.ToString(info[2]);
                                string colva = Convert.ToString(info[3]);
                                string TTname = Convert.ToString(info[4]);
                                string FDAte = Convert.ToString(info[5]);
                                string Section = Convert.ToString(info[6]);
                                count = deleteRecord(deg, sem, batch, colva, TTname, FDAte, Section);
                            }
                        }
                    }
                }
            }

            if (count > 0)
            {
                spreadTimeTable.Visible = true;
                div1.Visible = true;
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "Deleted.!";
                btnGo_OnClick(sender, e);
            }
            else
            {
                lblErrorMsg.Visible = true;
                spreadTimeTable.Visible = true;
                div1.Visible = true;
                lblErrorMsg.Text = "Not Deleted.!";
                btnGo_OnClick(sender, e);
            }
            //}

        }
        catch
        {

        }
    }
    protected void ddltimetable_SelectedIndexChanged(object sender, EventArgs e)
    {
        tdTime.Visible = false;
        txttimetable.Text = "";
        string degCode = string.Empty;
        string batchYear = string.Empty;
        string sec = string.Empty;
        string deg = Convert.ToString(ddlSchinfo.SelectedValue);
        string sem = Convert.ToString(ddlsem.SelectedValue);
        string[] val = deg.Split('-');
        if (val.Length > 1)
        {
            degCode = Convert.ToString(val[1]);
            batchYear = Convert.ToString(val[0]);
            sec = Convert.ToString(val[3]);
        }
        bindDate(batchYear, degCode, sem, sec);
    }

    protected string getCellValue(string strScheduledHour, string strSemSchedule)
    {
        try
        {
            string strSubName = "";
            string textValue = "";
            string noteValue = "";
            string subjectNo = strScheduledHour.Split('-')[0];
            string[] arr = strSemSchedule.Split(',');

            string sec = Convert.ToString(arr[5]).Trim();
            string strsec = "";

            if (sec != "" && sec != "-1" && sec != "all" && sec != null)
            {

                strsec = "and r.sections='" + sec + "'";
            }

            string subType = "S";
            string subj_type = d2.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + Convert.ToString(subjectNo) + "'");
            if (subj_type == "1" || subj_type.ToLower().Trim() == "true")
            {
                subType = "L";
            }

            string qry = "select distinct (CONVERT(varchar,r.Batch_Year)+'-'+c.Course_Name+' ('+de.dept_acronym+')-'+'Sem'+CONVERT(varchar, r.Current_Semester)+' '+ISNULL(r.Sections,''))Degree  from Registration r,Degree d,Department de,course c where r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0  and r.degree_code='" + Convert.ToString(arr[0]).Trim() + "' and r.Batch_Year='" + Convert.ToString(arr[2]).Trim() + "' and r.Current_Semester='" + Convert.ToString(arr[1]).Trim() + "'" + strsec + "";

            textValue = d2.GetFunction(qry);
            strSubName = (d2.GetFunction("select subject_name from subject where subject_no=" + Convert.ToString(subjectNo) + " "));
            noteValue = Convert.ToString(strScheduledHour) + "," + strSemSchedule;
            return strSubName + "-" + subType + "-" + textValue + "#" + noteValue;
        }
        catch
        {
            return null;
        }
    }

    protected void spreadTimeTable_OnCellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        spreadTimeTable.SaveChanges();
        cellClick = true;
    }
    protected void spreadTimeTable_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            if (cellClick == true)
            {
                spreadTimeTable.SaveChanges();
                string staffCode = Convert.ToString(ddlSearchOption.SelectedValue).Trim();
                string activerow = spreadTimeTable.ActiveSheetView.ActiveRow.ToString();
                string activecol = spreadTimeTable.ActiveSheetView.ActiveColumn.ToString();
                string colVal = string.Empty;
                int row = 0;

                string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
                string Daycoulmn = string.Empty;
                string Daycoulmnvalue = string.Empty;
                string dayOrder = Convert.ToString(ddlDayOrder.SelectedValue);
                int colV = 0;
                int.TryParse(activecol, out colV);
                string col1 = Convert.ToString(colV + 1);
                int row1 = 0;
                int.TryParse(dayOrder, out row1);
                string dayofweek = Days[row1 - 1];
                Daycoulmn = dayofweek + Convert.ToString(activecol);

                int.TryParse(activerow, out row);//9765-CSET031-C;10151-CSET538-C;10484-PHYT502-CHET509-C;2340-121110501-S,53,3,2017,2017-B.E-EEE-3-A,8/1/2018,;2262-121110501-S,54,7,2015,2015 TT 4Year VIISem CSE,7/2/2018,A
                int col = 0;
                DataTable dtDel = new DataTable();
                dtDel.Columns.Add("Colname");
                dtDel.Columns.Add("Colval");
                DataRow dr2 = null;
                int.TryParse(activecol, out col);
                int count = 0;
                if (activecol != "0")
                {
                    string NoteVal = Convert.ToString(spreadTimeTable.Sheets[0].Cells[row, col].Note);

                    if (!string.IsNullOrEmpty(NoteVal))
                    {
                        if (NoteVal.Contains(";"))
                        {
                            string[] FIR = NoteVal.Split(';');
                            for (int a = 0; a < FIR.Count(); a++)
                            {
                                string[] SEC = Convert.ToString(FIR[a]).Split(',');
                                if (SEC.Count() > 0)
                                {
                                    string SubSem = Convert.ToString(SEC[0]);
                                    string deg = Convert.ToString(SEC[1]);
                                    string SEM = Convert.ToString(SEC[2]);
                                    string Batch = Convert.ToString(SEC[3]);
                                    string TTname = Convert.ToString(SEC[4]);
                                    string FDAte = Convert.ToString(SEC[5]);
                                    string Section = Convert.ToString(SEC[6]);
                                    string subNo = string.Empty;

                                    if (!string.IsNullOrEmpty(SubSem))
                                    {
                                        string[] StaffCod = SubSem.Split('-');
                                        if (StaffCod.Count() > 0)
                                        {
                                            subNo = Convert.ToString(StaffCod[0]);
                                        }
                                    }
                                    dr2 = dtDel.NewRow();
                                    string degInfo = getDegree(deg);
                                    string subName = getSubject(subNo);
                                    dr2["Colname"] = Batch + "-" + degInfo + "-" + SEM + "-" + Section + "-" + subName;
                                    dr2["Colval"] = Batch + "-" + deg + "-" + SEM + "-" + Daycoulmn + "-" + TTname + "-" + FDAte + "-" + Section;
                                    dtDel.Rows.Add(dr2);
                                    //count = deleteRecord(deg, SEM, Batch, Daycoulmn, TTname, FDAte, Section);
                                }

                            }
                        }
                        else
                        {

                            string[] SEC = Convert.ToString(NoteVal).Split(',');
                            if (SEC.Count() > 0)
                            {
                                string SubSem = Convert.ToString(SEC[0]);
                                string deg = Convert.ToString(SEC[1]);
                                string SEM = Convert.ToString(SEC[2]);
                                string Batch = Convert.ToString(SEC[3]);
                                string TTname = Convert.ToString(SEC[4]);
                                string FDAte = Convert.ToString(SEC[5]);
                                string Section = Convert.ToString(SEC[6]);
                                string subNo = string.Empty;
                                if (!string.IsNullOrEmpty(SubSem))
                                {
                                    string[] StaffCod = SubSem.Split('-');
                                    if (StaffCod.Count() > 0)
                                    {
                                        subNo = Convert.ToString(StaffCod[0]);
                                    }
                                }
                                dr2 = dtDel.NewRow();
                                string degInfo = getDegree(deg);
                                string subName = getSubject(subNo);
                                dr2["Colname"] = Batch + "-" + degInfo + "-" + SEM + "-" + Section + "-" + subName;
                                dr2["Colval"] = Batch + "-" + deg + "-" + SEM + "-" + Daycoulmn + "-" + TTname + "-" + FDAte + "-" + Section;
                                dtDel.Rows.Add(dr2);
                                //count = deleteRecord(deg, SEM, Batch, Daycoulmn, TTname, FDAte, Section);
                            }
                        }
                    }
                }

                if (dtDel.Rows.Count > 0)
                {
                    cblTime.DataSource = dtDel;
                    cblTime.DataTextField = "Colname";
                    cblTime.DataValueField = "Colval";
                    cblTime.DataBind();
                    cblTime.Visible = true;
                    div1.Visible = true;
                }


            }

        }
        catch
        {

        }
    }

    private string getDegree(string degCode)
    {
        string degACR = string.Empty;
        degACR = d2.GetFunction("select Acronym from Degree d	where Degree_Code='" + degCode + "'");
        return degACR;

    }

    private string getSubject(string SubNo)
    {
        string SubACR = string.Empty;
        SubACR = d2.GetFunction("select subject_name from subject where subject_no='" + SubNo + "'");
        return SubACR;

    }

    protected void btnColse_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrorMsg.Text = string.Empty;
            div1.Visible = false;
            cblTime.Items.Clear();
        }

        catch (Exception ex)
        {

        }
    }

    protected void Button2_OnClick(object sender, EventArgs e)
    {
        ModalPopupExtender1.Hide();
        try
        {
            staffApnd = true;
            btnAdd_OnClick(sender, e);
        }
        catch
        {
        }
    }

    protected void Button4_OnClick(object sender, EventArgs e)
    {
        try
        {
            ModalPopupExtender1.Hide();
            return;
        }
        catch
        {
        }
    }

}