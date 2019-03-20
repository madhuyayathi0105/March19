using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Drawing;
using InsproDataAccess;
using System.Collections;
using Farpoint = FarPoint.Web.Spread;
using System.Globalization;
using wc = System.Web.UI.WebControls;

public partial class ScheduleMOD_NewAlternateSchedule : System.Web.UI.Page
{
    #region Variable Declaration

    DataSet ds = new DataSet();
    ReuasableMethods rs = new ReuasableMethods();
    DataSet degreeDataset = new DataSet();
    DataTable dtCommon = new DataTable();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    DAccess2 dacess = new DAccess2();
    SqlConnection getcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection cona = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con1a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con4a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection tempcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlCommand cmd;
    SqlCommand cmda;
    SqlCommand cmd1a;
    Hashtable hat = new Hashtable();
    Dictionary<string, Dictionary<int, string>> dicScheduledData = new Dictionary<string, Dictionary<int, string>>();
    Dictionary<string, Dictionary<int, string>> dicAlternateScheduledData = new Dictionary<string, Dictionary<int, string>>();
    Dictionary<string, Dictionary<string, string[]>> dicAlternateSubjectList = new Dictionary<string, Dictionary<string, string[]>>();
    ArrayList allotedstaff = new ArrayList();
    string start_dayorder = string.Empty;
    Boolean spreadDetCellClick = false;
    Boolean semspreadCellClick = false;
    string cellTagValue = string.Empty;
    static int noOfHoursPerDay = 0;
    int noScheduleCnt = 0;
    int checkSemDateCnt = 0;
    bool holiday = false;
    string holidayDescription = "";

    #endregion

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
            if (!IsPostBack)
            {
                Bindcollege();
                bindbatch();
                bindBranch();
                txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtDate.Attributes.Add("readonly", "readonly");
                BindCollege();
                BindAlterStaffDepartment((ddlAlterFreeCollege.Items.Count > 0) ? Convert.ToString(ddlAlterFreeCollege.SelectedValue).Trim() : "");
                InitSpread(FpAlterFreeStaffList);
                sem_schedule.Visible = false;
                //semspread.Visible = false;
                subDiv.Visible = false;
                semspread.Sheets[0].AutoPostBack = true;
                semspread.Attributes.Add("onmouseup", "__doPostBack('semspread','CellClick,' + semspread.ActiveRow + ',' + semspread.ActiveCol)");
            }
            semspread.Visible = true;
        }
        catch (Exception ex) { }
    }

    #region college

    public void Bindcollege()
    {
        try
        {
            ddlCollege.Items.Clear();
            dtCommon.Clear();
            ddlCollege.Enabled = false;
            DataSet dsprint = new DataSet();
            dsprint.Clear();
            string qryUserCodeOrGroupCode = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["group_code"] != null && Session["single_user"] != null && Convert.ToString(Session["single_user"]).Trim() != "1" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true")
            {
                qryUserCodeOrGroupCode = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["usercode"]).Trim()))
            {
                qryUserCodeOrGroupCode = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            if (!string.IsNullOrEmpty(qryUserCodeOrGroupCode))
            {
                dicQueryParameter.Clear();
                dicQueryParameter.Add("column_field", Convert.ToString(qryUserCodeOrGroupCode));
                dtCommon = storeAcc.selectDataTable("bind_college", dicQueryParameter);
            }
            if (dtCommon.Rows.Count > 0)
            {
                ddlCollege.DataSource = dtCommon;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
                ddlCollege.SelectedIndex = 0;
                ddlCollege.Enabled = true;
            }
        }
        catch (Exception ex)
        {

        }
    }

    #endregion

    #region Batch
    public void bindbatch()
    {
        cbl_batch.Items.Clear();
        ds = dacess.select_method_wo_parameter("bind_batch", "sp");
        int count = ds.Tables[0].Rows.Count;
        if (count > 0)
        {
            cbl_batch.DataSource = ds;
            cbl_batch.DataTextField = "batch_year";
            cbl_batch.DataValueField = "batch_year";
            cbl_batch.DataBind();
        }
        if (cbl_batch.Items.Count > 0)
        {
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                cbl_batch.Items[i].Selected = true;
            }
            txt_batch.Text = lblbatch.Text + "(" + cbl_batch.Items.Count + ")";
            cb_batch.Checked = true;
        }

    }

    public void bindBranch()
    {
        try
        {

            ds.Clear();
            txtBranch.Text = "---Select---";
            string batchCode = string.Empty;
            chkBranch.Checked = false;
            cblBranch.Items.Clear();
            string collegeCode = string.Empty;
            if (ddlCollege.Items.Count > 0)
                collegeCode = ddlCollege.SelectedValue.ToString().Trim();

            string columnfield = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            string valBatch = string.Empty;
            if (cbl_batch.Items.Count > 0)
                valBatch = rs.GetSelectedItemsValueAsString(cbl_batch);

            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch))
            {
                string selBranch = "SELECT DISTINCT dg.Degree_Code,(c.Course_Name+'-'+dt.Dept_Name) as Dept_Name,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "') AND r.Batch_Year in('" + valBatch + "')" + columnfield + " ORDER BY dg.Degree_Code, CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";
                ds = dacess.select_method_wo_parameter(selBranch, "Text");

            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblBranch.DataSource = ds;
                cblBranch.DataTextField = "Dept_Name";
                cblBranch.DataValueField = "Degree_Code";
                cblBranch.DataBind();
                checkBoxListselectOrDeselect(cblBranch, true);
                CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            }
        }
        catch
        {
        }
    }

    protected void cb_batch_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_batch, cbl_batch, txt_batch, lblbatch.Text, "--Select--");
        bindBranch();
    }
    protected void cbl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_batch, cbl_batch, txt_batch, lblbatch.Text, "--Select--");
        bindBranch();
    }
    protected void chkBranch_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
    }
    protected void cblBranch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");

    }
    #endregion

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            chkPerDAySched.Visible = true;
            chkPerDAySched.Checked = false;
            chkPerDAySched_OnCheckedChanged(sender, e);
            btnsave.Visible = false;
            semmsglbl.Text = "Select any cell!";
            semmsglbl.Visible = true;
            semspread.Visible = false;

            string selectedBatchYears = Convert.ToString(getCblSelectedValue(cbl_batch));
            if (selectedBatchYears != "")
            {
                norecordlbl.Visible = false;
                //int noOfHours = Convert.ToInt32(getSpreadData());
                getSpreadData();
                if (holiday == false)
                {
                    if ((dicScheduledData.Count != 0 || dicAlternateScheduledData.Count != 0) && (noScheduleCnt != dicScheduledData.Count && checkSemDateCnt != dicScheduledData.Count))//
                    {
                        subDiv.Visible = true;
                        //loadspreadDetails(noOfHours);
                        loadspreadDetails();
                    }
                    else
                    {

                        if (noScheduleCnt != 0)
                            norecordlbl.Text = "No Schedule for selected date";
                        else
                            norecordlbl.Text = "The Selected date must be between Semester date";
                        norecordlbl.Visible = true;
                        subDiv.Visible = false;
                        divSpreadDet.Visible = false;
                        spreadDet.Visible = false;
                        chkPerDAySched.Visible = false;
                    }
                }
                else
                {

                    norecordlbl.Text = holidayDescription;
                    norecordlbl.Visible = true;
                    subDiv.Visible = false;
                    divSpreadDet.Visible = false;
                    spreadDet.Visible = false;
                    chkPerDAySched.Visible = false;
                }
            }
            else
            {
                norecordlbl.Visible = true;
                norecordlbl.Text = "Select Batch ";
                divSpreadDet.Visible = false;
                spreadDet.Visible = false;
                chkPerDAySched.Visible = false;
            }
        }
        catch (Exception ex) { }
    }

    protected void chkPerDAySched_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (!chkPerDAySched.Checked)
                sem_schedule.Visible = false;
            else
            {
                // btnAsPerDaySchedule_Click();
                semspread.Visible = false;
                //  semmsglbl.Visible = false;
                sem_schedule.Visible = true;
            }
        }
        catch (Exception ex) { }
    }

    protected void getSpreadData()
    {

        string StaffCode = string.Empty;
        degreeDataset.Clear();
        dicScheduledData.Clear();
        dicAlternateScheduledData.Clear();
        string valDegree = string.Empty;
        int hrsPerDay = 0;

        if (cblBranch.Items.Count == 0)
        {
            Label12.Visible = true;
            Label12.Text = "No " + lblBranch.Text + " Found";
            Div3.Visible = true;
            return;
        }
        else
        {
            valDegree = rs.GetSelectedItemsValueAsString(cblBranch);
            if (string.IsNullOrEmpty(valDegree))
            {
                Label12.Visible = true;
                Label12.Text = "Select Atleast One " + lblBranch.Text + "";
                Div3.Visible = true;
                return;
            }
        }

        try
        {
            StaffCode = Session["staff_code"].ToString().Trim();
            string selectedBatchYears = Convert.ToString(getCblSelectedValue(cbl_batch));
            string qry = " select distinct (CONVERT(varchar,r.Batch_Year)+' - '+c.Course_Name+' ('+de.dept_acronym+') - '+CONVERT(varchar, r.Current_Semester)+' '+ISNULL(r.Sections,''))Degree ,r.Batch_Year,r.degree_code,r.Current_Semester,ISNULL(r.Sections,'')Section,(CONVERT(varchar,r.Batch_Year)+' - '+CONVERT(varchar,r.degree_code)+' - '+CONVERT(varchar, r.Current_Semester)+' - '+ISNULL(r.Sections,''))Code from Registration r,Degree d,Department de,course c where r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0 and r.Batch_Year in ('" + selectedBatchYears + "') and r.degree_code in('" + valDegree + "') and r.college_code='" + ddlCollege.SelectedValue + "' order by r.Batch_Year desc";
            degreeDataset = dacess.select_method_wo_parameter(qry, "Text");

            //get No_of_hrs_per_day,schorder,nodays
            if (degreeDataset.Tables.Count > 0 && degreeDataset.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < degreeDataset.Tables[0].Rows.Count; i++)
                {
                    Dictionary<int, string> dic_scheduleddata = new Dictionary<int, string>();
                    Dictionary<int, string> dic_alterScheduleddata = new Dictionary<int, string>();
                    int schOrder = 0;
                    int noOfDays = 0;
                    int frstHlfHour = 0;
                    string semStartdate = string.Empty;
                    string semEnddate = string.Empty;
                    string start_dayorder = string.Empty;
                    string selectedDate_day = string.Empty;
                    string holidyreasn = string.Empty;
                    Boolean noflag = false;
                    string splValNew = string.Empty;
                    string splval = string.Empty;
                    string setcellnote = string.Empty;
                    Boolean alterflag = true;
                    int rowval = 0;

                    string selectedDate = txtDate.Text.ToString();
                    string[] splitSelectedDate = selectedDate.Split(new Char[] { '/' });
                    selectedDate = splitSelectedDate[1].ToString() + "/" + splitSelectedDate[0].ToString() + "/" + splitSelectedDate[2].ToString();
                    DateTime dtSelectedDate = Convert.ToDateTime(selectedDate.ToString());


                    string printDegree = Convert.ToString(degreeDataset.Tables[0].Rows[i]["Degree"]);
                    string degCode = Convert.ToString(degreeDataset.Tables[0].Rows[i]["degree_code"]);
                    string sem = Convert.ToString(degreeDataset.Tables[0].Rows[i]["Current_Semester"]);
                    string batchYear = Convert.ToString(degreeDataset.Tables[0].Rows[i]["Batch_Year"]);
                    string sec = Convert.ToString(degreeDataset.Tables[0].Rows[i]["Section"]);
                    string str_sec;

                    if (sec == "" || sec == "-1")
                    {
                        str_sec = string.Empty;
                    }
                    else
                    {
                        str_sec = " and sections='" + sec + "'";
                    }

                    string qry1 = "Select No_of_hrs_per_day,schorder,nodays,no_of_hrs_I_half_day from periodattndschedule where degree_code=" + degCode + " and semester = " + sem + "";
                    DataSet dataSet = dacess.select_method_wo_parameter(qry1, "Text");
                    if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    {
                        hrsPerDay = Convert.ToInt32(dataSet.Tables[0].Rows[0]["No_of_hrs_per_day"]);
                        schOrder = Convert.ToInt32(dataSet.Tables[0].Rows[0]["schorder"]);
                        noOfDays = Convert.ToInt32(dataSet.Tables[0].Rows[0]["nodays"]);
                        frstHlfHour = Convert.ToInt32(dataSet.Tables[0].Rows[0]["no_of_hrs_I_half_day"]);

                        noOfHoursPerDay = hrsPerDay;

                    }
                    //--------------get semester information ie schedule
                    string semInfoQry = "select * from seminfo where degree_code='" + degCode + "' and semester='" + sem + "' and batch_year='" + batchYear + "'";
                    DataSet semInfoDataSet = dacess.select_method_wo_parameter(semInfoQry, "Text");
                    if (semInfoDataSet.Tables.Count > 0 && semInfoDataSet.Tables[0].Rows.Count > 0)
                    {
                        if ((semInfoDataSet.Tables[0].Rows[0]["start_date"].ToString()) != "" && (semInfoDataSet.Tables[0].Rows[0]["start_date"].ToString()) != "\0")
                        {
                            string[] tmpdate = Convert.ToString(semInfoDataSet.Tables[0].Rows[0]["start_date"]).Split(new char[] { ' ' });
                            semStartdate = tmpdate[0].ToString();
                            string[] enddate = Convert.ToString(semInfoDataSet.Tables[0].Rows[0]["end_date"]).Split(new char[] { ' ' });
                            semEnddate = enddate[0].ToString();

                            if (Convert.ToString(semInfoDataSet.Tables[0].Rows[0]["starting_dayorder"]) != "")
                            {
                                start_dayorder = Convert.ToString(semInfoDataSet.Tables[0].Rows[0]["starting_dayorder"]);
                            }
                            else
                            {
                                start_dayorder = "1";
                            }
                        }
                        else
                        {
                            //norecordlbl.Visible = true;
                            norecordlbl.Text = "Update semester Information";
                            norecordlbl.ForeColor = Color.Red;
                            //return null;
                        }
                    }
                    else
                    {
                        // norecordlbl.Visible = true;
                        norecordlbl.Text = "Update semester Information";
                        norecordlbl.ForeColor = Color.Red;
                        //return null;
                    }
                    // Day Order Change=======Start====================
                    string dayorderQry = "Select * from tbl_consider_day_order where degree_code='" + degCode + "' and semester='" + sem + "' and batch_year='" + batchYear + "'  and ((From_Date between '" + dtSelectedDate.ToString("yyyy-MM-dd") + "' and '" + dtSelectedDate.ToString("yyyy-MM-dd") + "') or (To_Date between '" + dtSelectedDate.ToString("yyyy-MM-dd") + "' and '" + dtSelectedDate.ToString("yyyy-MM-dd") + "'))";

                    DataSet dayorderDataSet = dacess.select_method_wo_parameter(dayorderQry, "Text");
                    Hashtable hatDayOrderChange = new Hashtable();
                    for (int doc = 0; doc < dayorderDataSet.Tables[0].Rows.Count; doc++)
                    {
                        DateTime dtFromDate = Convert.ToDateTime(dayorderDataSet.Tables[0].Rows[doc]["from_date"].ToString());
                        DateTime dtEndDate = Convert.ToDateTime(dayorderDataSet.Tables[0].Rows[doc]["to_date"].ToString());
                        string reason = dayorderDataSet.Tables[0].Rows[doc]["Reason"].ToString();
                        for (DateTime dtChangeDate = dtFromDate; dtChangeDate <= dtEndDate; dtChangeDate = dtChangeDate.AddDays(1))
                        {
                            if (!hatDayOrderChange.Contains(dtChangeDate))
                            {
                                hatDayOrderChange.Add(dtChangeDate, reason);
                            }
                        }
                    }
                    //=================================End======================================          
                    //------------find schedule order type
                    if (hrsPerDay > 0)
                    {
                        if (schOrder != 0)
                        {
                            selectedDate_day = dtSelectedDate.ToString("ddd");
                        }
                        else
                        {
                            selectedDate_day = dacess.findday(selectedDate.ToString(), degCode, sem, batchYear, semStartdate.ToString(), noOfDays.ToString(), start_dayorder.ToString());
                        }
                    }
                    if (semStartdate != "" && semEnddate != "")
                    {
                        if ((dtSelectedDate >= Convert.ToDateTime(semStartdate) && dtSelectedDate <= Convert.ToDateTime(semEnddate)))
                        {
                            if (selectedDate_day != "Sun")
                            {
                                string sqlsrt = "select top 1 ";
                                string noOfAlterQry = "select no_of_alter,";

                                for (int j = 1; j <= hrsPerDay; j++)
                                {
                                    sqlsrt = sqlsrt + selectedDate_day + j.ToString() + ",";
                                    noOfAlterQry = noOfAlterQry + selectedDate_day + j.ToString() + ",";
                                }

                                string holidayStudentsQry = "select * from holidaystudents  where degree_code=" + degCode + " and semester=" + sem + " and holiday_date ='" + selectedDate.ToString() + "'";
                                string holidayReason = string.Empty;
                                Boolean morleave = false;
                                Boolean eveleave = false;
                                DataSet holidayStudentsDataSet = dacess.select_method_wo_parameter(holidayStudentsQry, "Text");
                                if (holidayStudentsDataSet.Tables.Count > 0 && holidayStudentsDataSet.Tables[0].Rows.Count > 0)
                                {
                                    holidayReason = Convert.ToString(holidayStudentsDataSet.Tables[0].Rows[0]["holiday_desc"]);
                                    holidayDescription = Convert.ToString(holidayStudentsDataSet.Tables[0].Rows[0]["holiday_desc"]);
                                    string hlfOrFull = Convert.ToString(holidayStudentsDataSet.Tables[0].Rows[0]["halforfull"]);
                                    if (hlfOrFull.Trim() == "1" || hlfOrFull.Trim().ToLower() == "true")
                                    {
                                        if (Convert.ToString(holidayStudentsDataSet.Tables[0].Rows[0]["morning"]).Trim() == "1" || Convert.ToString(holidayStudentsDataSet.Tables[0].Rows[0]["morning"]).Trim().ToLower() == "true")
                                        {
                                            morleave = true;
                                        }
                                        if (Convert.ToString(holidayStudentsDataSet.Tables[0].Rows[0]["evening"]).Trim() == "1" || Convert.ToString(holidayStudentsDataSet.Tables[0].Rows[0]["evening"]).Trim().ToLower() == "true")
                                        {
                                            eveleave = true;
                                        }
                                    }
                                    else
                                    {
                                        morleave = true;
                                        eveleave = true;
                                        holiday = true;
                                    }
                                }

                                string sqlQry = sqlsrt + " degree_code,semester,batch_year from semester_schedule where batch_year=" + batchYear + " and degree_code = " + degCode + " and semester = " + sem + " and FromDate<= ' " + selectedDate + " ' " + str_sec + " order by fromdate desc";

                                DataSet sqlQryDataSet = dacess.select_method_wo_parameter(sqlQry, "Text");

                                string alternateValueQry = sqlsrt + " degree_code , semester , batch_year from Alternate_schedule where batch_year=" + batchYear + " and degree_code = " + degCode + " and semester = " + sem + " and FromDate= '" + selectedDate + "' " + str_sec + "";
                                DataSet alternateValueDataSet = dacess.select_method(alternateValueQry, hat, "Text");

                                if (sqlQryDataSet.Tables.Count > 0 && sqlQryDataSet.Tables[0].Rows.Count > 0)
                                {
                                    if (holidyreasn == "")
                                    {
                                        holidyreasn = dtSelectedDate.ToString("dd/MM/yyyy") + " is Holiday- " + holidayReason;
                                    }
                                    else
                                    {
                                        holidyreasn = holidyreasn + ',' + dtSelectedDate.ToString("dd/MM/yyyy") + " is Holiday- " + holidayReason;
                                    }
                                    for (int hr = 1; hr <= hrsPerDay; hr++)
                                    {

                                        Boolean leavefa = false;
                                        if (morleave == true)
                                        {
                                            if (hr < frstHlfHour + 1)
                                            {
                                                leavefa = true;
                                            }
                                        }
                                        if (eveleave == true)
                                        {
                                            if (hr > frstHlfHour)
                                            {
                                                leavefa = true;
                                            }
                                        }
                                        if (leavefa == true)
                                        {
                                            if (holidayReason != "" && holidayReason != null)
                                            {
                                                if ((Convert.ToString(sqlQryDataSet.Tables[0].Rows[0][hr - 1])) != "" && (Convert.ToString(sqlQryDataSet.Tables[0].Rows[0][hr - 1])) != "\0")
                                                {

                                                    spreadDet.Sheets[0].Cells[hr - 1, 0].Text = holidayReason + " Holiday";
                                                    spreadDet.Sheets[0].Cells[hr - 1, 1].Text = holidayReason + " Holiday";
                                                    spreadDet.Sheets[0].Cells[hr - 1, 0].Locked = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            noflag = true;
                                            if ((Convert.ToString(sqlQryDataSet.Tables[0].Rows[0][hr - 1])) != "" && (Convert.ToString(sqlQryDataSet.Tables[0].Rows[0][hr - 1])) != "\0")
                                            {
                                                //============Day Order Change =================
                                                if (hatDayOrderChange.Contains(dtSelectedDate))
                                                {
                                                    splValNew = hatDayOrderChange[dtSelectedDate].ToString();
                                                    spreadDet.Sheets[0].Cells[hr - 1, 0].Locked = true;
                                                }
                                                else
                                                {
                                                    string[] subjnew = (Convert.ToString(sqlQryDataSet.Tables[0].Rows[0][hr - 1])).Split(new Char[] { ';' });
                                                    for (int l = 0; l <= subjnew.GetUpperBound(0); l++)
                                                    {
                                                        if (subjnew.GetUpperBound(0) >= 0)
                                                        {
                                                            string[] subjstr = subjnew[l].Split(new Char[] { '-' });
                                                            if (subjstr.GetUpperBound(0) >= 2)
                                                            {
                                                                if (!string.IsNullOrEmpty(StaffCode))
                                                                {
                                                                    for (int m = 0; m < subjstr.Length; m++)
                                                                    {
                                                                        if (StaffCode == subjstr[hr].ToString().Trim())
                                                                        {
                                                                            string strsub = GetFunction("select subject_name from subject where subject_no=" + subjstr[0] + " ");
                                                                            getcon.Close();
                                                                            if (!splValNew.Contains(StaffCode))
                                                                                splValNew = splValNew + ((strsub.ToString()) + "-" + subjstr[1] + "-" + subjstr[2]) + ";";
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    string strsub = GetFunction("select subject_name from subject where subject_no=" + subjstr[0] + " ");
                                                                    getcon.Close();
                                                                    splValNew = splValNew + ((strsub.ToString()) + "-" + subjstr[1] + "-" + subjstr[2]) + ";";
                                                                }

                                                            }
                                                        }
                                                    }
                                                    dic_scheduleddata.Add(hr, splValNew);
                                                }
                                            }
                                        }
                                        splValNew = string.Empty;

                                        if (alterflag == true) //Alternate Schedule Details
                                        {

                                            alterflag = false;
                                            string alternateDetailsQry = alternateValueQry;
                                            int noaltval = 1;
                                            if (noaltval > 1)
                                            {
                                                alternateDetailsQry = noOfAlterQry + "degree_code,semester,batch_year from tbl_alter_schedule_Details where batch_year=" + batchYear + " and degree_code = " + degCode + " and semester = " + sem + " and FromDate= ' " + selectedDate.ToString() + " ' " + str_sec + " order by no_of_alter, fromdate desc";
                                            }
                                            DataSet alternateDetailsDataSet = dacess.select_method(alternateDetailsQry, hat, "Text");
                                            if (alternateDetailsDataSet.Tables[0].Rows.Count > 0)
                                            {
                                                for (int hour = 1; hour <= hrsPerDay; hour++)
                                                {
                                                    for (int alternateHour = 0; alternateHour < alternateDetailsDataSet.Tables[0].Rows.Count; alternateHour++)
                                                    {
                                                        if (alternateHour + 1 <= 1)
                                                        {
                                                            string column = selectedDate_day + hour;
                                                            string value = alternateDetailsDataSet.Tables[0].Rows[alternateHour]["" + column + ""].ToString().Trim();
                                                            splval = string.Empty;
                                                            leavefa = false;
                                                            if (morleave == true)
                                                            {
                                                                if (hour < frstHlfHour + 1)
                                                                {
                                                                    leavefa = true;
                                                                }
                                                            }
                                                            if (eveleave == true)
                                                            {
                                                                if (hour > frstHlfHour)
                                                                {
                                                                    leavefa = true;
                                                                }
                                                            }
                                                            if (leavefa == true)
                                                            {
                                                                if (holidayReason != "" && holidayReason != null)
                                                                {
                                                                    if (value != "" && value != "\0")
                                                                    {
                                                                        spreadDet.Sheets[0].Cells[hour - 1, alternateHour + 1].Text = holidayReason + " Holiday";
                                                                        spreadDet.Sheets[0].Cells[hour - 1, alternateHour + 1].Note = holidayReason + " Holiday";
                                                                        spreadDet.Sheets[0].Cells[hour - 1, alternateHour + 1].Locked = true;
                                                                        splval = string.Empty;
                                                                        //batchbtn.Visible = true;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (value != "" && value != "\0")
                                                                {
                                                                    if (hatDayOrderChange.Contains(dtSelectedDate))
                                                                    {
                                                                        spreadDet.Sheets[0].Cells[hour - 1, alternateHour + 1].Locked = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        //  spreadDet.Sheets[0].Cells[hour - 1, alternateHour + 1].Locked = false;
                                                                        setcellnote = value;
                                                                        string[] sple = (value).Split(new Char[] { ';' });
                                                                        for (int ii = 0; ii <= sple.GetUpperBound(0); ii++)
                                                                        {
                                                                            if (sple.GetUpperBound(0) >= 0)
                                                                            {
                                                                                string[] sp1 = (sple[ii].ToString()).Split(new Char[] { '-' });

                                                                                if (sp1.GetUpperBound(0) >= 2)
                                                                                {
                                                                                    if (!string.IsNullOrEmpty(StaffCode))
                                                                                    {

                                                                                        splval = splval + (GetFunction("select subject_name from subject where subject_no=" + sp1[0].ToString() + " ") + "-" + sp1[1].ToString() + "-" + sp1[2].ToString()) + ";";


                                                                                    }


                                                                                    else
                                                                                    {
                                                                                        if (sp1.GetUpperBound(0) >= 2)
                                                                                        {
                                                                                            splval = splval + (GetFunction("select subject_name from subject where subject_no=" + sp1[0].ToString() + " ") + "-" + sp1[1].ToString() + "-" + sp1[2].ToString()) + ";";

                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    dic_alterScheduleddata.Add(hour, (splval + "#" + value));
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                    }
                                }
                                else
                                {

                                    dic_scheduleddata.Add(1, "No Schedule");
                                    noScheduleCnt++;


                                }
                            }
                            else
                            {
                                //for (intNCtr = 1; intNCtr <= intNHrs; intNCtr++)
                                //{

                                //    SpdInfo.Sheets[0].Cells[intNCtr - 1, 0].Text = "Sunday";
                                //    SpdInfo.Sheets[0].Cells[intNCtr - 1, 1].Text = "Sunday";
                                //    SpdInfo.Sheets[0].Cells[intNCtr - 1, 1].Note = "Holiday";
                                //    SpdInfo.Sheets[0].Cells[intNCtr - 1, 0].Note = "Holiday";
                                //}
                            }
                        }
                        else
                        {
                            dic_scheduleddata.Add(1, "The selected date must be between Semester date");

                            checkSemDateCnt++;
                            spreadDet.Visible = false;
                            chkPerDAySched.Visible = false;
                            //return "";
                        }
                    }
                    else
                    {
                        dic_scheduleddata.Add(1, "Update semester Information");

                        checkSemDateCnt++;
                        spreadDet.Visible = false;
                        chkPerDAySched.Visible = false;
                    }
                    rowval = 0;
                    splValNew = string.Empty;
                    splval = string.Empty;
                    if (dic_scheduleddata.Count != 0)
                        dicScheduledData.Add(printDegree, dic_scheduleddata);
                    if (dic_alterScheduleddata.Count != 0)
                        dicAlternateScheduledData.Add(printDegree, dic_alterScheduleddata);

                }
            }

            //return hrsPerDay.ToString();
        }
        catch (Exception ex)
        {
            //return null;
        }
    }

    protected void loadspreadDetails()
    {
        try
        {

            Dictionary<int, string> dic = new Dictionary<int, string>();
            Dictionary<int, string> alterDic = new Dictionary<int, string>();

            #region design
            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 2;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            spreadDet.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            spreadDet.ActiveSheetView.SelectionBackColor = Color.Transparent;


            #endregion

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].Columns[0].Width = 100;


            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Degree";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            spreadDet.Sheets[0].Columns[1].Width = 1500;

            int sno = 0;

            for (int i = 0; i < noOfHoursPerDay; i++)
            {
                int col = i + 1;
                spreadDet.Sheets[0].ColumnCount++;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = "Period " + col;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                spreadDet.Sheets[0].Columns[1].Width = 150;

            }

            for (int cnt = 0; cnt < dicScheduledData.Count; cnt++)
            {
                dic.Clear();
                alterDic.Clear();
                sno++;
                spreadDet.Sheets[0].RowCount++;

                if (dicScheduledData.ContainsKey(Convert.ToString(degreeDataset.Tables[0].Rows[cnt]["Degree"])))
                    dic = dicScheduledData[Convert.ToString(degreeDataset.Tables[0].Rows[cnt]["Degree"])];

                if (dicAlternateScheduledData.ContainsKey(Convert.ToString(degreeDataset.Tables[0].Rows[cnt]["Degree"])))
                    alterDic = dicAlternateScheduledData[Convert.ToString(degreeDataset.Tables[0].Rows[cnt]["Degree"])];

                spreadDet.Sheets[0].Cells[cnt, 0].Text = Convert.ToString(sno);
                spreadDet.Sheets[0].Cells[cnt, 1].Text = Convert.ToString(degreeDataset.Tables[0].Rows[cnt]["Degree"]);

                foreach (KeyValuePair<int, string> dicKeyValue in dic)
                {
                    int dicKey = dicKeyValue.Key;
                    string dicValue = dicKeyValue.Value;

                    bool hasAlternateValue = false;
                    string alterDicValue = "";
                    foreach (KeyValuePair<int, string> entry in alterDic)
                    {
                        if (dicKey == entry.Key)
                        {
                            hasAlternateValue = true;
                            alterDicValue = entry.Value;
                            break;
                        }
                    }

                    if (hasAlternateValue)
                    {
                        string str = Convert.ToString(alterDicValue);
                        string[] arr = str.Split('#');
                        spreadDet.Sheets[0].Cells[cnt, (1 + dicKey)].Text = Convert.ToString(arr[0]);
                        spreadDet.Sheets[0].Cells[cnt, (1 + dicKey)].Note = Convert.ToString(arr[1]);
                        spreadDet.Sheets[0].Cells[cnt, (1 + dicKey)].Tag = Convert.ToString(degreeDataset.Tables[0].Rows[cnt]["Code"]);
                        spreadDet.Sheets[0].Cells[cnt, (1 + dicKey)].ForeColor = Color.Green;
                    }
                    else
                    {
                        if (Convert.ToString(dicValue) != "No Schedule" && Convert.ToString(dicValue) != "The selected date must be between Semester date" && Convert.ToString(dicValue) != "Update semester Information")
                        {
                            spreadDet.Sheets[0].Cells[cnt, (1 + dicKey)].Text = Convert.ToString(dicValue);
                            spreadDet.Sheets[0].Cells[cnt, (1 + dicKey)].Tag = Convert.ToString(degreeDataset.Tables[0].Rows[cnt]["Code"]);
                        }
                        else
                        {
                            spreadDet.Sheets[0].Cells[cnt, (1 + dicKey)].Text = Convert.ToString(dic[1]);
                            spreadDet.ActiveSheetView.SpanModel.Add(cnt, 2, 1, noOfHoursPerDay);
                            spreadDet.Sheets[0].Cells[cnt, (1 + dicKey)].ForeColor = Color.Red;
                            spreadDet.Sheets[0].Cells[cnt, (1 + dicKey)].HorizontalAlign = HorizontalAlign.Center;
                            spreadDet.Sheets[0].Cells[cnt, (1 + dicKey)].VerticalAlign = VerticalAlign.Middle;
                        }
                    }
                }

            }
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;

            spreadDet.SaveChanges();
            spreadDet.Height = 200;
            //spreadDet.Width = 600;
            divSpreadDet.Visible = true;
            spreadDet.Visible = true;
            chkPerDAySched.Visible = true;
            chkPerDAySched.Checked = false;
            //semspread.Visible = true;
            // semspread.Visible = false;
        }
        catch (Exception ex) { }

    }

    protected void spreadDet_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {

        try
        {
            spreadDetCellClick = true;
            btnFreeStaffList.Visible = true;
            spcellClickPopup.Visible = false;
            subjtree.Visible = false;
            altersp_td.Visible = false;
            btnOk.Visible = false;
            chkForAlternateStaff.Checked = false;
            chkappend.Checked = false;
            InitSpread(FpAlterFreeStaffList);
            btnsave.Enabled = true;
            fpSpreadTreeNode.Sheets[0].Columns[3].Visible = false;

            Session["batch"] = "";
            Session["degcode"] = "";
            Session["sem"] = "";
            Session["sec"] = "";
            Session["period"] = "";
            Session["spreadDetcellTagValue"] = "";

            string Position = e.CommandArgument.ToString().Replace("}", "").Replace("{", "");
            string[] pos = Position.Split(',');

            int xpos = 0;
            int ypos = 0;
            int actRow = 0;
            int actCol = 0;

            if (pos.Length > 0)
            {
                string[] xVal = (pos.Length > 0) ? pos[0].Split('=') : new string[0];
                string[] yVal = (pos.Length > 1) ? pos[1].Split('=') : new string[0];
                if (xVal.Length > 1)
                {
                    int.TryParse(xVal[1], out xpos);
                }
                if (yVal.Length > 1)
                {
                    int.TryParse(yVal[1], out ypos);

                }
                actRow = xpos;
                actCol = ypos;



                cellTagValue = Convert.ToString(spreadDet.Sheets[0].Cells[actRow, actCol].Tag);

                if (cellTagValue != "")
                {
                    string[] splitTagValue = cellTagValue.Split(new Char[] { '-' });

                    Session["batch"] = Convert.ToString(splitTagValue[0]);
                    Session["degcode"] = Convert.ToString(splitTagValue[1]);
                    Session["sem"] = Convert.ToString(splitTagValue[2]);
                    Session["sec"] = "";
                    if (Convert.ToString(splitTagValue[3]) != "")
                        Session["sec"] = Convert.ToString(splitTagValue[3]).Trim();
                    Session["period"] = Convert.ToString(spreadDet.Sheets[0].ColumnHeader.Cells[0, actCol].Text).Split(' ')[1];
                    Session["spreadDetcellTagValue"] = cellTagValue;
                    free_staff();

                }
            }

        }
        catch (Exception ex) { }
    }

    protected void spreadDet_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {

            if (spreadDetCellClick)
            {
                if (cellTagValue != "")
                {
                    if (!chkPerDAySched.Checked)
                    {
                        loadTree();
                        //btnAsPerDaySchedule_Click();

                    }
                    else
                    {
                        sem_schedule.Visible = true;
                        btnAsPerDaySchedule_Click();
                        semspread.Visible = true;
                        semmsglbl.Visible = false;
                        semspread.SaveChanges();
                    }
                }
                else
                {
                    spcellClickPopup.Visible = false;
                }
                if (cellTagValue == "" && sem_schedule.Visible == true)
                {
                    semspread.Visible = false;
                    semmsglbl.Visible = true;
                    semmsglbl.Text = "Please select valid cell!";
                }


            }

        }
        catch (Exception ex) { }
    }

    protected void spcellClickPopupclose_Click(object sender, EventArgs e)
    {
        spcellClickPopup.Visible = false;


    }

    #region TreeView

    private void loadTree()
    {
        try
        {
            spcellClickPopup.Visible = true;
            subjtree.Visible = true;
            altersp_td.Visible = false;
            btnOk.Visible = false;
            chkForAlternateStaff.Checked = false;
            chkappend.Checked = false;

            fpSpreadTreeNode.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            fpSpreadTreeNode.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            fpSpreadTreeNode.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
            fpSpreadTreeNode.ActiveSheetView.RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
            fpSpreadTreeNode.ActiveSheetView.RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            fpSpreadTreeNode.ActiveSheetView.RowHeader.DefaultStyle.Font.Bold = true;
            fpSpreadTreeNode.Sheets[0].ColumnHeader.Columns[0].Width = 200;
            fpSpreadTreeNode.Sheets[0].ColumnHeader.Columns[1].Width = 200;
            fpSpreadTreeNode.Sheets[0].ColumnHeader.Columns[2].Width = 100;
            fpSpreadTreeNode.CommandBar.Visible = false;

            fpSpreadTreeNode.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
            FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
            style.Font.Size = 13;
            style.Font.Bold = true;
            fpSpreadTreeNode.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
            fpSpreadTreeNode.Sheets[0].AllowTableCorner = true;
            fpSpreadTreeNode.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;


            fpSpreadTreeNode.Sheets[0].RowCount = 0;
            fpSpreadTreeNode.Sheets[0].ColumnCount = 5;
            fpSpreadTreeNode.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Subject Name";

            fpSpreadTreeNode.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Name";
            FarPoint.Web.Spread.ButtonCellType staf_butt1 = new FarPoint.Web.Spread.ButtonCellType("OneCommand", FarPoint.Web.Spread.ButtonType.PushButton, "Remove");
            fpSpreadTreeNode.Sheets[0].Columns[2].CellType = staf_butt1;
            staf_butt1.Text = "Remove";

            FarPoint.Web.Spread.ButtonCellType btnfreestaff = new Farpoint.ButtonCellType();
            btnfreestaff.CommandName = "ButtonFreeStaff";
            btnfreestaff.Text = "Select";
            fpSpreadTreeNode.Sheets[0].Columns[4].CellType = btnfreestaff;

            fpSpreadTreeNode.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Remove";
            fpSpreadTreeNode.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            fpSpreadTreeNode.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            fpSpreadTreeNode.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            fpSpreadTreeNode.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            fpSpreadTreeNode.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            fpSpreadTreeNode.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            fpSpreadTreeNode.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            fpSpreadTreeNode.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            fpSpreadTreeNode.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;

            fpSpreadTreeNode.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Alternate Staff Name";
            fpSpreadTreeNode.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            fpSpreadTreeNode.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            fpSpreadTreeNode.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            fpSpreadTreeNode.Sheets[0].Columns[3].Visible = false;

            fpSpreadTreeNode.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Choose Free Staff";
            fpSpreadTreeNode.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            fpSpreadTreeNode.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            fpSpreadTreeNode.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            fpSpreadTreeNode.Sheets[0].Columns[4].Width = 75;
            fpSpreadTreeNode.Sheets[0].Columns[4].Visible = false;

            subjtree.Nodes.Clear();
            //---------alternate subj shouldnt be same as subject

            //  string subjname_staffcode = string.Empty;
            //  string subjname = string.Empty;


            //subjname_staffcode = spreadDet.Sheets[0].Cells[actRow, actCol].Text;
            //string[] splitsubj = subjname_staffcode.Split(new Char[] { '-' });
            //subjname = splitsubj[0].ToString();


            string batch = Convert.ToString(Session["batch"]);
            string degcode = Convert.ToString(Session["degcode"]);
            string sem = Convert.ToString(Session["sem"]);
            string sec = "";
            if (Convert.ToString(Session["sec"]) != "")
                sec = Convert.ToString(Session["sec"]);

            //-------------------
            string Syllabus_year = string.Empty;
            Syllabus_year = GetSyllabusYear(degcode, batch, sem);
            if (Syllabus_year != "-1")
            {
                //--------------get subject type and subjects
                cona.Close();
                cona.Open();
                SqlDataReader subTypeRs;
                cmda = new SqlCommand("select distinct subject.subtype_no,subject_type from subject,sub_sem where sub_sem.subtype_no=subject.subtype_no and subject.syll_code=(select syll_code from syllabus_master where degree_code=" + degcode + " and semester=" + sem + " and syllabus_year = " + Syllabus_year + " and batch_year = " + batch + ") order by subject.subtype_no", cona);
                subTypeRs = cmda.ExecuteReader();
                TreeNode node;
                int rec_count = 0;
                while (subTypeRs.Read())
                {
                    if ((subTypeRs["subject_type"].ToString()) != "0")
                    {
                        SqlDataReader subTypeRs1;
                        con1a.Close();
                        con1a.Open();
                        cmd1a = new SqlCommand("select subject.subtype_no,subject_type,subject_no,subject_name,subject_code from subject,sub_sem where sub_sem.subtype_no=subject.subtype_no and subject.syll_code=(select syll_code from syllabus_master where degree_code=" + degcode + " and semester=" + sem + " and syllabus_year = " + Syllabus_year + " and batch_year = " + batch + ") and subject.subtype_no=" + subTypeRs["subtype_no"] + " order by subject.subtype_no,subject.subject_no", con1a);
                        subTypeRs1 = cmd1a.ExecuteReader();
                        node = new TreeNode(subTypeRs["subject_type"].ToString(), rec_count.ToString());
                        while (subTypeRs1.Read())//-------------set to tree
                        {
                            node.ChildNodes.Add(new TreeNode(subTypeRs1["subject_name"].ToString(), subTypeRs1["subject_no"].ToString()));
                            rec_count = rec_count + 1;

                        }
                        subjtree.Nodes.Add(node);
                    }
                }
                cona.Close();
                con1a.Close();
            }
        }
        catch
        {
        }
    }

    protected void subjtree_SelectedNodeChanged(object sender, EventArgs e) //Event called when TreeView node is selected
    {
        try
        {
            chkmulstaff_ChekedChange(sender, e);
            chkmullsstaff_SelectedIndexChanged(sender, e);
            fpSpreadTreeNode.ActiveSheetView.AutoPostBack = false;

            if (!chkForAlternateStaff.Checked)
            {
                fpSpreadTreeNode.Sheets[0].Columns[3].Visible = false;
                fpSpreadTreeNode.Sheets[0].Columns[4].Visible = false;
            }
            tr_mulstaff.Visible = false;
            chkmullsstaff.Items.Clear();
            int rowa = 0;
            int rowval = 0;
            int staf_cnt = 0;
            string staff_code = "", staff_name_code = string.Empty;
            int staffval = 0;

            string strsec;
            string batch = Convert.ToString(Session["batch"]);
            string degcode = Convert.ToString(Session["degcode"]);
            string sem = Convert.ToString(Session["sem"]);
            string sec = "";
            if (Convert.ToString(Session["sec"]) != "")
                sec = Convert.ToString(Session["sec"]);

            if (sec != "0" && sec != "\0")
            {
                strsec = string.Empty;
            }
            else
            {
                strsec = " and sections='" + sec + "'";
            }

            int parent_count = subjtree.Nodes.Count;//----------TreeView Nodes count
            for (int parentNodeCnt = 0; parentNodeCnt < parent_count; parentNodeCnt++)
            {
                for (int childNodeCnt = 0; childNodeCnt < subjtree.Nodes[parentNodeCnt].ChildNodes.Count; childNodeCnt++)//-------count child node
                {
                    if (subjtree.Nodes[parentNodeCnt].ChildNodes[childNodeCnt].Selected == true)
                    {
                        string temp_sec = string.Empty;
                        if (sec == "")
                        {
                            temp_sec = string.Empty;
                        }
                        else
                        {
                            temp_sec = " and Sections='" + sec + "'";
                        }

                        if (chkappend.Checked == true)
                        {
                            bool subj = false;
                            string subno = subjtree.Nodes[parentNodeCnt].ChildNodes[childNodeCnt].Value;
                            if (fpSpreadTreeNode.Sheets[0].RowCount > 0)
                            {
                                rowa = fpSpreadTreeNode.Sheets[0].RowCount - 1;
                                while (rowa >= 0)
                                {
                                    string rows = Convert.ToString(fpSpreadTreeNode.Sheets[0].Cells[rowa, 0].Tag);

                                    if (subno == rows)
                                    {

                                        subj = true;

                                    }
                                    rowa--;
                                }
                            }
                            if (subj == false)
                            {
                                fpSpreadTreeNode.Sheets[0].RowCount++;
                                //-------set selected subject name into the spread
                                rowval = fpSpreadTreeNode.Sheets[0].RowCount - 1;
                                fpSpreadTreeNode.Sheets[0].Rows[rowval].Font.Name = "Book Antiqua";
                                fpSpreadTreeNode.Sheets[0].Rows[rowval].Font.Size = FontUnit.Medium;
                                fpSpreadTreeNode.Sheets[0].RowHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                                fpSpreadTreeNode.Sheets[0].RowHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                                fpSpreadTreeNode.Sheets[0].RowHeader.Cells[0, 0].Font.Bold = true;

                                fpSpreadTreeNode.Sheets[0].SetText(rowval, 0, subjtree.Nodes[parentNodeCnt].ChildNodes[childNodeCnt].Text);
                                fpSpreadTreeNode.Sheets[0].Cells[rowval, 0].Tag = subjtree.Nodes[parentNodeCnt].ChildNodes[childNodeCnt].Value;


                                string child_index = subjtree.Nodes[parentNodeCnt].ChildNodes[childNodeCnt].Value;
                                //--------------bind staff name into the spread
                                string strstaffquery = "select staff_code,staff_name from staffmaster where staff_code in (select staff_code from staff_selector where subject_no = " + Convert.ToInt32(child_index) + " and batch_year=" + batch + "  " + temp_sec + ")";

                                DataSet staf_set = dacess.select_method_wo_parameter(strstaffquery, "Text");

                                string[] staff_list = new string[staf_set.Tables[0].Rows.Count + 1];
                                for (staf_cnt = 0; staf_cnt < staf_set.Tables[0].Rows.Count; staf_cnt++)
                                {
                                    staff_list[staf_cnt] = staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                    chkmullsstaff.Items.Add(staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString());

                                    if (staff_code == "")
                                    {
                                        staff_code = staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                        staff_name_code = staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                    }
                                    else
                                    {
                                        staff_code = staff_code + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                        staff_name_code = staff_name_code + ";" + staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                    }
                                }
                                if (staff_list.GetUpperBound(0) > 0)
                                {
                                    staff_list[staf_cnt] = "All";
                                }
                                if (staf_set.Tables.Count > 0 && staf_set.Tables[0].Rows.Count > 1)
                                {
                                    tr_mulstaff.Visible = true;
                                    //btnAddRow.Visible = true;
                                    //chkSelectAlterStaff.Visible = true;
                                    //btnFreeStaff.Visible = true;
                                    //ddlAlternamteStaff.Items.Clear();
                                    //ddlAlternamteStaff.Visible = true;
                                    //ddlAlternamteStaff.Items.Insert(0, new ListItem("--Select--", ""));
                                    pmulstaff.Visible = true;
                                }
                                FarPoint.Web.Spread.ComboBoxCellType staf_combo = new FarPoint.Web.Spread.ComboBoxCellType(staff_list);
                                fpSpreadTreeNode.Sheets[0].Cells[rowval, 1].CellType = staf_combo;
                                fpSpreadTreeNode.Sheets[0].Cells[rowval, 1].Tag = staff_code;
                                fpSpreadTreeNode.Sheets[0].Cells[rowval, 1].Value = staff_name_code;
                                fpSpreadTreeNode.SaveChanges();

                            }
                        }
                        else if (chkForAlternateStaff.Checked)
                        {
                            spreadDet.SaveChanges();
                            int actRow = spreadDet.Sheets[0].ActiveRow;
                            int actColumn = spreadDet.Sheets[0].ActiveColumn;
                            string staffcode = string.Empty;
                            string[] stafflist = spreadDet.Sheets[0].Cells[actRow, actColumn].Text.Split('-');
                            int forvariable = 0;
                            for (int j = 0; j < stafflist.Length; j++)
                            {
                                if (forvariable != 0)
                                {
                                    if (stafflist[forvariable].Contains(';'))
                                    {
                                        stafflist[forvariable] = stafflist[forvariable].Remove(stafflist[forvariable].Length - 1);
                                    }
                                    if (!allotedstaff.Contains(stafflist[forvariable]))
                                        allotedstaff.Add(stafflist[forvariable]);
                                }
                                forvariable++;
                            }

                            fpSpreadTreeNode.Sheets[0].RowCount = 0;

                            rowval = 0;
                            //-------set selected subject name into the spread
                            string chile_index = subjtree.Nodes[parentNodeCnt].ChildNodes[childNodeCnt].Value;
                            //--------------bind staff name into the spread

                            string staffNamesQry = "select staff_code,staff_name from staffmaster where staff_code in (select staff_code from staff_selector where subject_no = " + Convert.ToInt32(chile_index) + " and batch_year=" + batch + " " + temp_sec + ")";

                            DataSet staf_set = dacess.select_method_wo_parameter(staffNamesQry, "Text");

                            string[] staff_list = new string[staf_set.Tables[0].Rows.Count + 1];

                            for (staf_cnt = 0; staf_cnt < staf_set.Tables[0].Rows.Count; staf_cnt++)
                            {

                                staff_list[staf_cnt] = staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                chkmullsstaff.Items.Add(staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString());
                                if (staff_code == "")
                                {
                                    staff_code = staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                    staff_name_code = staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                }
                                else
                                {
                                    staff_code = staff_code + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                    staff_name_code = staff_name_code + ";" + staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                }
                            }
                            if (staff_list.GetUpperBound(0) > 0)
                            {
                                // staff_list[staf_cnt] = "All";
                            }
                            if (staf_set.Tables[0].Rows.Count > 1)
                            {
                                tr_mulstaff.Visible = true;
                                //btnAddRow.Visible = true;
                                //chkSelectAlterStaff.Visible = true;
                                //btnFreeStaff.Visible = true;
                                //ddlAlternamteStaff.Items.Clear();
                                //ddlAlternamteStaff.Visible = true;
                                //ddlAlternamteStaff.Items.Insert(0, new ListItem("--Select--", ""));
                                pmulstaff.Visible = true;
                            }
                            int rowvalnew = 0;
                            for (int row = 0; row < staf_set.Tables[0].Rows.Count; row++)
                            {
                                if (String.IsNullOrEmpty(staff_list[staffval]) || staff_list[staffval] == "")
                                {

                                }
                                else
                                {
                                    fpSpreadTreeNode.Sheets[0].RowCount++;
                                    fpSpreadTreeNode.Sheets[0].SetText(rowvalnew, 0, subjtree.Nodes[parentNodeCnt].ChildNodes[childNodeCnt].Text);
                                    fpSpreadTreeNode.Sheets[0].Cells[rowvalnew, 0].Tag = subjtree.Nodes[parentNodeCnt].ChildNodes[childNodeCnt].Value;

                                    fpSpreadTreeNode.Sheets[0].Rows[rowvalnew].Font.Name = "Book Antiqua";
                                    fpSpreadTreeNode.Sheets[0].Rows[rowvalnew].Font.Size = FontUnit.Medium;

                                    FarPoint.Web.Spread.TextCellType txtbx = new Farpoint.TextCellType();
                                    fpSpreadTreeNode.Sheets[0].Cells[rowvalnew, 1].CellType = txtbx;
                                    fpSpreadTreeNode.Sheets[0].Cells[rowvalnew, 1].Tag = staff_list[staffval];
                                    fpSpreadTreeNode.Sheets[0].Cells[rowvalnew, 1].Text = staff_list[staffval];
                                    fpSpreadTreeNode.Sheets[0].Columns[4].Visible = true;
                                    // fpSpreadTreeNode.Sheets[0].Columns[5].Visible = false;
                                    fpSpreadTreeNode.Sheets[0].Columns[3].Visible = false;
                                    staffval++;
                                    rowvalnew++;

                                }
                            }

                            btnOk.Visible = true;
                            chkappend.Visible = true;
                            fpSpreadTreeNode.Visible = true;

                            fpSpreadTreeNode.SaveChanges();
                            fpSpreadTreeNode.Sheets[0].PageSize = fpSpreadTreeNode.Sheets[0].RowCount;
                        }
                        else
                        {
                            fpSpreadTreeNode.Sheets[0].RowCount = 0;
                            fpSpreadTreeNode.Sheets[0].RowCount = 1;
                            rowval = 0;
                            //-------set selected subject name into the spread
                            fpSpreadTreeNode.Sheets[0].SetText(rowval, 0, subjtree.Nodes[parentNodeCnt].ChildNodes[childNodeCnt].Text);
                            fpSpreadTreeNode.Sheets[0].Cells[rowval, 0].Tag = subjtree.Nodes[parentNodeCnt].ChildNodes[childNodeCnt].Value;
                            string child_index = subjtree.Nodes[parentNodeCnt].ChildNodes[childNodeCnt].Value;
                            fpSpreadTreeNode.Sheets[0].Rows[rowval].Font.Name = "Book Antiqua";
                            fpSpreadTreeNode.Sheets[0].Rows[rowval].Font.Size = FontUnit.Medium;
                            //--------------bind staff name into the spread

                            string staffNamesQry = "select staff_code,staff_name from staffmaster where staff_code in (select staff_code from staff_selector where subject_no = " + Convert.ToInt32(child_index) + " and batch_year=" + batch + " " + temp_sec + ")";

                            DataSet stafDataSet = dacess.select_method_wo_parameter(staffNamesQry, "Text");
                            string[] staff_list = new string[stafDataSet.Tables[0].Rows.Count + 1];
                            for (staf_cnt = 0; staf_cnt < stafDataSet.Tables[0].Rows.Count; staf_cnt++)
                            {

                                staff_list[staf_cnt] = stafDataSet.Tables[0].Rows[staf_cnt][1].ToString() + "-" + stafDataSet.Tables[0].Rows[staf_cnt][0].ToString();
                                chkmullsstaff.Items.Add(stafDataSet.Tables[0].Rows[staf_cnt][1].ToString() + "-" + stafDataSet.Tables[0].Rows[staf_cnt][0].ToString());
                                if (staff_code == "")
                                {
                                    staff_code = stafDataSet.Tables[0].Rows[staf_cnt][0].ToString();
                                    staff_name_code = stafDataSet.Tables[0].Rows[staf_cnt][1].ToString() + "-" + stafDataSet.Tables[0].Rows[staf_cnt][0].ToString();
                                }
                                else
                                {
                                    staff_code = staff_code + "-" + stafDataSet.Tables[0].Rows[staf_cnt][0].ToString();
                                    staff_name_code = staff_name_code + ";" + stafDataSet.Tables[0].Rows[staf_cnt][1].ToString() + "-" + stafDataSet.Tables[0].Rows[staf_cnt][0].ToString();
                                }
                            }
                            if (staff_list.GetUpperBound(0) > 0)
                            {
                                staff_list[staf_cnt] = "All";
                            }
                            if (stafDataSet.Tables[0].Rows.Count > 1)
                            {
                                lblmulstaff.Visible = true;
                                tr_mulstaff.Visible = true;
                                // btnAddRow.Visible = true;
                                // chkSelectAlterStaff.Visible = true;
                                // btnFreeStaff.Visible = true;
                                //ddlAlternamteStaff.Items.Clear();
                                //ddlAlternamteStaff.Visible = true;
                                //ddlAlternamteStaff.Items.Insert(0, new ListItem("--Select--", ""));
                                pmulstaff.Visible = true;
                            }
                            FarPoint.Web.Spread.ComboBoxCellType staf_combo = new FarPoint.Web.Spread.ComboBoxCellType(staff_list);
                            staf_combo.AutoPostBack = true;
                            fpSpreadTreeNode.Sheets[0].Cells[rowval, 1].CellType = staf_combo;
                            fpSpreadTreeNode.Sheets[0].Cells[rowval, 1].Tag = staff_code;
                            fpSpreadTreeNode.Sheets[0].Cells[rowval, 1].Value = staff_name_code;
                            fpSpreadTreeNode.Visible = true;


                        }
                        btnOk.Visible = true;
                        chkappend.Visible = true;
                        altersp_td.Visible = true;
                        fpSpreadTreeNode.Sheets[0].PageSize = fpSpreadTreeNode.Sheets[0].RowCount;
                        fpSpreadTreeNode.SaveChanges();
                    }

                }
            }

        }
        catch (Exception ex) { }
    }

    protected void fpSpreadTreeNode_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "ButtonFreeStaff") //On "Choose free staff" button clicked
            {
                try
                {
                    divAlterFreeStaffDetails.Visible = true;
                    lblAlterFreeStaffError.Text = string.Empty;
                    lblAlterFreeStaffError.Visible = false;
                    txtAlterFreeStaffSearch.Text = string.Empty;
                    int ActiveRow = e.SheetView.ActiveRow;
                    GetStaffDetailsNEW(ActiveRow);
                    //  fpSpreadTreeNode.Sheets[0].Columns[3].Visible = true;
                }
                catch
                {

                }
            }
            else//On "Remove" button clicked
            {
                subjtree.Visible = true;
                fpSpreadTreeNode.Visible = true;
                chkappend.Visible = true;
                btnOk.Visible = true;
                //   treepanel.Visible = true;
                int ar = 0;
                ar = fpSpreadTreeNode.ActiveSheetView.ActiveRow;
                int col = fpSpreadTreeNode.ActiveSheetView.ActiveColumn;
                fpSpreadTreeNode.Sheets[0].RemoveRows(ar, 1);
            }
        }
        catch (Exception ex) { }
    }

    protected void fpSpreadTreeNode_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        // cellclick1 = true;
    }

    private void GetStaffDetailsNEW(int ActRow)
    {
        try
        {
            DataTable dtStaffDetails = new DataTable();
            DateTime dtAlterDate = new DateTime();
            DateTime.TryParseExact(txtDate.Text, "dd/MM/yyyy", null, DateTimeStyles.None, out dtAlterDate);
            int period = 0;
            int.TryParse(Convert.ToString(Session["period"]), out period);
            dtStaffDetails = getFreeStaffListNew(dtAlterDate: dtAlterDate, period: period, type: 1, searchValue: txtAlterFreeStaffSearch.Text.Trim());

            InitSpread(FpAlterFreeStaffList);
            Farpoint.CheckBoxCellType chkCellAll = new Farpoint.CheckBoxCellType();
            chkCellAll.AutoPostBack = true;
            Farpoint.TextCellType txtCell = new Farpoint.TextCellType();
            Farpoint.CheckBoxCellType chkOneByOne = new Farpoint.CheckBoxCellType();
            chkOneByOne.AutoPostBack = false;
            int sno = 0;
            if (dtStaffDetails.Rows.Count > 0)
            {
                foreach (DataRow drStaffDetails in dtStaffDetails.Rows)
                {
                    string staffCode = Convert.ToString(drStaffDetails["staff_code"]).Trim();
                    string staffName = Convert.ToString(drStaffDetails["staff_name"]).Trim();
                    sno++;
                    FpAlterFreeStaffList.Sheets[0].RowCount++;
                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno).Trim();
                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 0].Locked = true;
                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 1].CellType = chkOneByOne;
                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 1].Locked = false;
                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(staffCode).Trim();
                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 2].Locked = true;
                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;

                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(staffName).Trim();
                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 3].Tag = ActRow.ToString();
                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 3].Locked = true;
                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;

                }
                FpAlterFreeStaffList.SaveChanges();
                FpAlterFreeStaffList.Sheets[0].PageSize = FpAlterFreeStaffList.Sheets[0].RowCount;
                FpAlterFreeStaffList.Height = 500;
                FpAlterFreeStaffList.SaveChanges();
                FpAlterFreeStaffList.Visible = true;
            }
            else
            {
            }
        }
        catch
        {
        }
    }

    public DataTable getFreeStaffListNew(DateTime dtAlterDate, int period, byte type = 0, string searchValue = null)
    {
        DataTable dtFreeStaffList = new DataTable();
        string qry = string.Empty;
        try
        {
            string qryStaffFilter = string.Empty;
            string qryDeptFilter = string.Empty;
            string qryCollegeFilter = string.Empty;

            string qryStaffFilter1 = string.Empty;
            string qryDeptFilter1 = string.Empty;
            string qryCollegeFilter1 = string.Empty;
            if (type == 0)
            {

            }
            else
            {
                if (ddlAlterFreeCollege.Items.Count > 0)
                {
                    qryCollegeFilter = " and sfm.college_code ='" + Convert.ToString(ddlAlterFreeCollege.SelectedValue).Trim() + "'";
                    qryCollegeFilter1 = " and sfm1.college_code='" + Convert.ToString(ddlAlterFreeCollege.SelectedValue).Trim() + "'";
                }
                if (ddlAlterFreeDepartment.Items.Count > 0)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(ddlAlterFreeDepartment.SelectedValue).Trim()) && Convert.ToString(ddlAlterFreeDepartment.SelectedValue).Trim().ToLower() != "all")
                    {
                        qryDeptFilter = " and hr.dept_code='" + Convert.ToString(ddlAlterFreeDepartment.SelectedValue).Trim() + "'";
                        qryDeptFilter1 = "  and hr1.dept_code='" + Convert.ToString(ddlAlterFreeDepartment.SelectedValue).Trim() + "'";
                    }
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    if (ddlAlterFreeStaff.Items.Count > 0)
                    {
                        if (ddlAlterFreeStaff.SelectedIndex == 0)
                        {
                            qryStaffFilter = " and sfm.staff_name like '%" + searchValue + "%'";
                            qryStaffFilter1 = " and sfm.staff_name like '%" + searchValue + "%'";
                        }
                        else
                        {
                            qryStaffFilter = " and sfm.staff_code like '%" + searchValue + "%'";
                            qryStaffFilter1 = " and sfm.staff_code like '%" + searchValue + "%'";
                        }
                    }
                }
            }

            if (period != 0)
            {
                qry = " select distinct sfm.staff_code,sfm.staff_name from staffmaster sfm inner join stafftrans sts on sts.staff_code=sfm.staff_code inner join hrdept_master hr on hr.dept_code=sts.dept_code where sts.latestrec='1' and sfm.resign=0 and sfm.settled=0 and sfm.college_code=hr.college_code " + qryCollegeFilter + qryDeptFilter + " and sfm.staff_code not in( select distinct sfm1.staff_code from Semester_Schedule sch,Registration r,seminfo si,staffmaster sfm1 inner join stafftrans sts1 on sts1.staff_code=sfm1.staff_code inner join hrdept_master hr1 on hr1.dept_code=sts1.dept_code where sts1.latestrec='1' and sfm1.resign=0 and sfm1.settled=0 and sfm1.college_code=hr1.college_code " + qryCollegeFilter1 + qryDeptFilter1 + " and r.Batch_Year=sch.batch_year and r.degree_code=sch.degree_code and r.Current_Semester=sch.semester and r.Batch_Year=si.batch_year and r.degree_code=si.degree_code and r.Current_Semester=si.semester and si.batch_year=sch.batch_year and sch.degree_code=si.degree_code and si.semester=sch.semester and LTRIM(RTRIM(ISNULL(sch.Sections,'')))=LTRIM(RTRIM(ISNULL(r.Sections,'')))  and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and sch.FromDate between si.start_date and si.end_date and '" + dtAlterDate.ToString("MM/dd/yyyy") + "' between si.start_date and si.end_date and sch.FromDate<='" + dtAlterDate.ToString("MM/dd/yyyy") + "' and sch." + Convert.ToString((DayOfWeek)dtAlterDate.DayOfWeek).Substring(0, 3) + period.ToString() + " like '%'+sfm1.staff_code+'%') and sfm.staff_code not in( select distinct sfm1.staff_code from Alternate_Schedule sch,Registration r,seminfo si,staffmaster sfm1 inner join stafftrans sts1 on sts1.staff_code=sfm1.staff_code inner join hrdept_master hr1 on hr1.dept_code=sts1.dept_code where sts1.latestrec='1' and sfm1.resign=0 and sfm1.settled=0 and sfm1.college_code=hr1.college_code " + qryCollegeFilter1 + qryDeptFilter1 + " and r.Batch_Year=sch.batch_year and r.degree_code=sch.degree_code and r.Current_Semester=sch.semester and r.Batch_Year=si.batch_year and r.degree_code=si.degree_code and r.Current_Semester=si.semester and si.batch_year=sch.batch_year and sch.degree_code=si.degree_code and si.semester=sch.semester and LTRIM(RTRIM(ISNULL(sch.Sections,'')))=LTRIM(RTRIM(ISNULL(r.Sections,'')))  and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and sch.FromDate between si.start_date and si.end_date and '" + dtAlterDate.ToString("MM/dd/yyyy") + "' between si.start_date and si.end_date and sch.FromDate='" + dtAlterDate.ToString("MM/dd/yyyy") + "' and sch." + Convert.ToString((DayOfWeek)dtAlterDate.DayOfWeek).Substring(0, 3) + period.ToString() + " like '%'+sfm1.staff_code+'%') " + qryStaffFilter + " order by sfm.staff_name,sfm.staff_code";
                dtFreeStaffList = dirAcc.selectDataTable(qry);

                DataTable dtAlterScheduleFreeStaff = new DataTable();
                qry = "select distinct sfm.staff_code,sfm.staff_name from staffmaster sfm inner join stafftrans sts on sts.staff_code=sfm.staff_code inner join hrdept_master hr on hr.dept_code=sts.dept_code where sts.latestrec='1' and sfm.resign=0 and sfm.settled=0 and sfm.college_code=hr.college_code " + qryCollegeFilter + qryDeptFilter + " and sfm.staff_code not in( select distinct sfm1.staff_code from Alternate_Schedule sch,Registration r,seminfo si,staffmaster sfm1 inner join stafftrans sts1 on sts1.staff_code=sfm1.staff_code inner join hrdept_master hr1 on hr1.dept_code=sts1.dept_code where sts1.latestrec='1' and sfm1.resign=0 and sfm1.settled=0 and sfm1.college_code=hr1.college_code " + qryCollegeFilter1 + qryDeptFilter1 + " and r.Batch_Year=sch.batch_year and r.degree_code=sch.degree_code and r.Current_Semester=sch.semester and r.Batch_Year=si.batch_year and r.degree_code=si.degree_code and r.Current_Semester=si.semester and si.batch_year=sch.batch_year and sch.degree_code=si.degree_code and si.semester=sch.semester and LTRIM(RTRIM(ISNULL(sch.Sections,'')))=LTRIM(RTRIM(ISNULL(r.Sections,'')))  and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and sch.FromDate between si.start_date and si.end_date and '" + dtAlterDate.ToString("MM/dd/yyyy") + "' between si.start_date and si.end_date and sch.FromDate='" + dtAlterDate.ToString("MM/dd/yyyy") + "' and sch." + Convert.ToString((DayOfWeek)dtAlterDate.DayOfWeek).Substring(0, 3) + period.ToString() + " like '%'+sfm1.staff_code+'%') " + qryStaffFilter + " order by sfm.staff_name,sfm.staff_code";

            }

        }
        catch
        {
        }
        return dtFreeStaffList;
    }

    public void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            int x = spreadDet.ActiveSheetView.ActiveRow;
            int y = spreadDet.ActiveSheetView.ActiveColumn;
            if (y + 1 < spreadDet.Sheets[0].ColumnCount && x > -1)
            {
                string subj_number = string.Empty;
                string splval = string.Empty, splval_temp = string.Empty;
                string subno_staff = string.Empty;
                string staffname = string.Empty, staff_name_code = string.Empty, staffcode = string.Empty;
                bool isChangedAlterStaff = false;
                string textValue = string.Empty;
                string noteValue = string.Empty;
                string tagValue = "0";
                string alterStaffCode = string.Empty, alterStaffName = string.Empty, alterStaffNameCode = string.Empty;
                string alterSplval = string.Empty, alterSplval_temp = string.Empty;

                //if (ddlAlternamteStaff.Items.Count > 0 && ddlAlternamteStaff.SelectedIndex != 0)
                //{
                //    isChangedAlterStaff = true;
                //    tagValue = "1";
                //}
                string alterTextValue = string.Empty;
                string alterNotValue = string.Empty;
                //if (chk_multisubj.Checked == false)
                //{
                for (int rowcnt = 0; rowcnt <= Convert.ToInt32(fpSpreadTreeNode.Sheets[0].RowCount) - 1; rowcnt++)
                {
                    fpSpreadTreeNode.SaveChanges();
                    staff_name_code = Convert.ToString(fpSpreadTreeNode.Sheets[0].GetText(rowcnt, 1));
                    string getstaff = Convert.ToString(fpSpreadTreeNode.Sheets[0].Cells[rowcnt, 1].Tag);
                    string getAlterStaff = Convert.ToString(fpSpreadTreeNode.Sheets[0].Cells[rowcnt, 3].Text);
                    if (staff_name_code == "" || staff_name_code == "System.Object")//-----------check wether the staff name selected or not
                    {
                        subjtree.Visible = true;
                        fpSpreadTreeNode.Visible = true;
                        chkappend.Visible = true;
                        btnOk.Visible = true;
                        errmsg.Visible = true;

                        errmsg.ForeColor = Color.Red;
                        errmsg.Text = "Select Staff name";
                        return;
                    }
                    else
                    {
                        //btnsave.Enabled = true;
                        subjtree.Visible = false;
                        fpSpreadTreeNode.Visible = false;
                        chkappend.Visible = false;
                        btnOk.Visible = false;

                        // btnsave.Visible = true;
                        errmsg.Visible = false;
                        lblmulstaff.Visible = false;
                        // btnFreeStaff.Visible = false;
                        //  ddlAlternamteStaff.Items.Clear();
                        // ddlAlternamteStaff.Visible = false;
                        tr_mulstaff.Visible = false;
                        //  btnAddRow.Visible = false;

                    }
                }
                if (Convert.ToInt32(fpSpreadTreeNode.Sheets[0].RowCount) == 0)//------------message for select the subject from the tree
                {
                    subjtree.Visible = true;
                    fpSpreadTreeNode.Visible = true;
                    chkappend.Visible = true;
                    btnOk.Visible = true;
                    errmsg.Visible = true;
                    errmsg.Text = "Select Subject name for alternate schedule from tree view";
                    errmsg.ForeColor = Color.Red;
                    return;
                }
                //-----------------set the selected subject name and staff name into the spread
                for (int row_cnt = 0; row_cnt <= Convert.ToInt32(fpSpreadTreeNode.Sheets[0].RowCount) - 1; row_cnt++)
                {
                    staffname = string.Empty;
                    staffcode = string.Empty;
                    alterStaffCode = string.Empty;
                    alterStaffName = string.Empty;

                    alterStaffNameCode = Convert.ToString(fpSpreadTreeNode.Sheets[0].Cells[row_cnt, 3].Text);
                    staff_name_code = Convert.ToString(fpSpreadTreeNode.Sheets[0].Cells[row_cnt, 1].Text).Trim();
                    string staffCodeList = Convert.ToString(fpSpreadTreeNode.Sheets[0].Cells[row_cnt, 1].Tag);

                    string alterStaffCodeList = Convert.ToString(fpSpreadTreeNode.Sheets[0].Cells[row_cnt, 3].Tag);
                    string getAlterStaff = Convert.ToString(fpSpreadTreeNode.Sheets[0].Cells[row_cnt, 3].Text);
                    subj_number = fpSpreadTreeNode.Sheets[0].Cells[row_cnt, 0].Tag.ToString();

                    string alternateDate = string.Empty;
                    string alternateHour = string.Empty;
                    DateTime dtAlterDate = new DateTime();
                    int period = 0;

                    dtAlterDate = new DateTime();
                    alternateDate = Convert.ToString(txtDate.Text).Trim();
                    alternateHour = Convert.ToString(Session["period"]);
                    period = 0;
                    int.TryParse(alternateHour, out period);
                    lblAlterDate.Text = alternateDate;
                    lblAlterHour.Text = period.ToString();
                    DateTime.TryParseExact(alternateDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtAlterDate);
                    Dictionary<string, string[]> dicSubjectStaffList = new Dictionary<string, string[]>();
                    string keyList = alternateDate.Trim() + "@" + alternateHour;
                    string[] subjectWiseStaffList = new string[0];
                    if (!string.IsNullOrEmpty(alterStaffNameCode))
                    {
                        if (!dicAlternateSubjectList.ContainsKey(keyList))
                        {
                            if (!dicSubjectStaffList.ContainsKey(subj_number.Trim()))
                                dicSubjectStaffList.Add(subj_number, subjectWiseStaffList);
                            dicAlternateSubjectList.Add(keyList.Trim(), dicSubjectStaffList);
                        }
                        else
                        {
                            dicSubjectStaffList = dicAlternateSubjectList[keyList.Trim()];
                            dicAlternateSubjectList[keyList.Trim()] = dicSubjectStaffList;
                        }
                        if (dicSubjectStaffList.ContainsKey(subj_number.Trim()))
                            subjectWiseStaffList = dicSubjectStaffList[subj_number.Trim()];
                    }
                    if (staff_name_code != "" && staff_name_code != null)
                    {
                        if (staff_name_code.ToLower().Trim() != "all")
                        {
                            string[] staff_name_code_spt = staff_name_code.Split('-');
                            for (int st = 0; st <= staff_name_code_spt.GetUpperBound(0); st = st + 2)
                            {
                                if (staffcode == "")
                                {
                                    staffname = staff_name_code_spt[st].ToString();
                                    staffcode = staff_name_code_spt[st + 1].ToString();
                                }
                                else
                                {
                                    staffname = staffname + "-" + staff_name_code_spt[st].ToString();
                                    staffcode = staffcode + "-" + staff_name_code_spt[st + 1].ToString();
                                }

                            }
                        }
                        else
                        {
                            staffcode = fpSpreadTreeNode.Sheets[0].Cells[row_cnt, 1].Tag.ToString();
                        }
                    }

                    if (alterStaffNameCode != "" && alterStaffNameCode != null)
                    {
                        if (alterStaffNameCode.ToLower().Trim() != "all")
                        {
                            string[] staff_name_code_spt = alterStaffNameCode.Split('-');
                            for (int st = 0; st <= staff_name_code_spt.GetUpperBound(0); st = st + 2)
                            {
                                if (alterStaffCode == "")
                                {
                                    alterStaffName = staff_name_code_spt[st].ToString();
                                    alterStaffCode = staff_name_code_spt[st + 1].ToString();
                                }
                                else
                                {
                                    alterStaffName = alterStaffName + "-" + staff_name_code_spt[st].ToString();
                                    alterStaffCode = alterStaffCode + "-" + staff_name_code_spt[st + 1].ToString();
                                }

                            }
                        }
                        else
                        {
                            alterStaffName = fpSpreadTreeNode.Sheets[0].Cells[row_cnt, 3].Text.ToString();
                            alterStaffCode = fpSpreadTreeNode.Sheets[0].Cells[row_cnt, 3].Tag.ToString();
                        }
                    }
                    if (!string.IsNullOrEmpty(alterStaffNameCode))
                    {
                        Array.Resize(ref subjectWiseStaffList, subjectWiseStaffList.Length + 1);
                        subjectWiseStaffList[subjectWiseStaffList.Length - 1] = staffcode + "@" + alterStaffCode;
                        subjectWiseStaffList = subjectWiseStaffList.Distinct().ToArray();

                        if (dicSubjectStaffList.ContainsKey(subj_number.Trim()))
                            dicSubjectStaffList[subj_number.Trim()] = subjectWiseStaffList;
                        if (dicAlternateSubjectList.ContainsKey(keyList))
                        {
                            dicAlternateSubjectList[keyList.Trim()] = dicSubjectStaffList;
                        }
                    }

                    string parenttext = subjtree.SelectedNode.Parent.Text;
                    string theory_lab = dacess.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subj_number + "'");
                    if (theory_lab.Trim() == "1" || theory_lab.Trim().ToLower() == "true")
                    {
                        theory_lab = "L";
                    }
                    else
                    {
                        theory_lab = "S";
                    }

                    string displaySubjectName = GetFunction("select subject_name from subject where subject_no=" + subj_number.ToString() + " ");
                    if (staff_name_code.Trim().ToLower() != "all")
                    {
                        if (splval == "")
                        {
                            splval = (displaySubjectName + "-" + staffcode + "-" + theory_lab);
                            subno_staff = subj_number + "-" + staffcode + "-" + theory_lab;
                        }
                        else
                        {
                            splval = splval + ";" + (displaySubjectName + "-" + staffcode + "-" + theory_lab);
                            subno_staff = subno_staff + ";" + subj_number + "-" + staffcode + "-" + theory_lab;
                        }
                    }

                    if (!string.IsNullOrEmpty(alterStaffNameCode))
                    {
                        if (alterStaffNameCode.Trim().ToLower() != "all")
                        {
                            if (string.IsNullOrEmpty(alterSplval))
                            {
                                alterSplval = (displaySubjectName + "-" + alterStaffCode + "-" + theory_lab);
                                alterSplval_temp = subj_number + "-" + alterStaffCode + "-" + theory_lab;
                            }
                            else
                            {
                                alterSplval = alterSplval + ";" + (displaySubjectName + "-" + alterStaffCode + "-" + theory_lab);
                                alterSplval_temp = alterSplval_temp + ";" + subj_number + "-" + alterStaffCode + "-" + theory_lab;
                            }
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(alterSplval))
                        {
                            alterSplval = (displaySubjectName + "-" + staffcode + "-" + theory_lab);
                            alterSplval_temp = subj_number + "-" + staffcode + "-" + theory_lab;
                        }
                        else
                        {
                            alterSplval = alterSplval + ";" + (displaySubjectName + "-" + staffcode + "-" + theory_lab);
                            alterSplval_temp = alterSplval_temp + ";" + subj_number + "-" + staffcode + "-" + theory_lab;
                        }
                    }
                    textValue = alterSplval;
                    noteValue = alterSplval_temp;

                }

                spreadDet.Sheets[0].Cells[x, y].Text = textValue.ToString();
                spreadDet.Sheets[0].Cells[x, y].Note = noteValue.ToString();
                spreadDet.Sheets[0].Cells[x, y].Tag = Session["spreadDetcellTagValue"];

                //FarPoint.Web.Spread.SheetView sv = spreadDet.ActiveSheetView;
                //sv.ActiveColumn = y;
                //sv.ActiveRow = x;
                spreadDet.Sheets[0].Cells[x, y].ForeColor = Color.Green;
                spcellClickPopup.Visible = false;
                btnsave.Visible = true;
            }
        }
        catch (Exception ex)
        {
            norecordlbl.Text = ex.ToString();
            norecordlbl.ForeColor = Color.Red;
            norecordlbl.Visible = true;
        }
    }


    #endregion

    #region Multiple staff selection
    protected void chkmulstaff_ChekedChange(object sender, EventArgs e)
    {
        txtmulstaff.Text = "---Select---";
        if (chkmulstaff.Checked == true)
        {
            if (chkmullsstaff.Items.Count > 0)
            {
                for (int i = 0; i < chkmullsstaff.Items.Count; i++)
                {
                    chkmullsstaff.Items[i].Selected = true;
                }
                txtmulstaff.Text = "Staff (" + chkmullsstaff.Items.Count + ")";
            }
        }
        else
        {
            for (int i = 0; i < chkmullsstaff.Items.Count; i++)
            {
                chkmullsstaff.Items[i].Selected = false;
            }
        }
    }

    protected void chkmullsstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtmulstaff.Text = "---Select---";
        chkmulstaff.Checked = false;
        int cou = 0;
        for (int i = 0; i < chkmullsstaff.Items.Count; i++)
        {
            if (chkmullsstaff.Items[i].Selected == true)
            {
                cou++;
            }
        }
        if (cou > 0)
        {
            txtmulstaff.Text = "Staff (" + cou + ")";
            if (chkmullsstaff.Items.Count == cou)
            {
                chkmulstaff.Checked = true;
            }
        }
    }

    protected void btnmulstaff_Click(object sender, EventArgs e)
    {
        try
        {
            string strsec = string.Empty;
            //string a = Convert.ToString(Session["sec"]);
            if (Convert.ToString(Session["sec"]) != " " && Convert.ToString(Session["sec"]) != "-1" && Convert.ToString(Session["sec"]) != null)
            {
                strsec = " and sections='" + Convert.ToString(Session["sec"]) + "'";
            }

            string strbatchyear = Convert.ToString(Session["batch"]);
            string strbranch = Convert.ToString(Session["degcode"]);
            string strsem = Convert.ToString(Session["sem"]);

            int activerow = fpSpreadTreeNode.Sheets[0].RowCount - 1;
            if (activerow != -1)
            {
                int rowval = Convert.ToInt32(activerow);
                if (chkmullsstaff.Items.Count > 0)
                {
                    string stafftext = string.Empty;
                    string stafftag = string.Empty;

                    string alterStaffText = string.Empty;
                    string alterStaffTag = string.Empty;
                    for (int i = 0; i < chkmullsstaff.Items.Count; i++)
                    {
                        if (chkmullsstaff.Items[i].Selected == true)
                        {
                            string stte = chkmullsstaff.Items[i].Text.ToString();
                            string[] stcode = stte.Split('-');
                            if (stafftext == "")
                            {
                                stafftext = chkmullsstaff.Items[i].Text.ToString();
                                stafftag = stcode[stcode.GetUpperBound(0)].ToString();
                            }
                            else
                            {
                                stafftext = stafftext + "-" + chkmullsstaff.Items[i].Text.ToString();
                                stafftag = stafftag + '-' + stcode[stcode.GetUpperBound(0)].ToString();
                            }
                        }
                    }

                    bool isHasAlternateStaff = false;
                    string[] alterStaffDetails = new string[1];
                    string[] alterStaffCodeDetails = new string[1];
                    alterStaffDetails[0] = string.Empty;
                    alterStaffCodeDetails[0] = string.Empty;
                    //if (ddlAlternamteStaff.Items.Count > 0)
                    //{
                    //    string alterStaffCode = string.Empty;
                    //    string alterStaffNameCode = string.Empty;
                    //    int selectedCount = 0;
                    //    if (chkSelectAlterStaff.Checked)
                    //        isHasAlternateStaff = true;
                    //    foreach (ListItem li in ddlAlternamteStaff.Items)
                    //    {
                    //        alterStaffCode = li.Value;
                    //        alterStaffNameCode = li.Text;
                    //        if (!string.IsNullOrEmpty(alterStaffCode) && !string.IsNullOrEmpty(alterStaffNameCode) && alterStaffNameCode.Trim().ToLower() != "all" && alterStaffNameCode.Trim().ToLower() != "--select--")
                    //        {
                    //            Array.Resize(ref alterStaffDetails, alterStaffDetails.Length + 1);
                    //            Array.Resize(ref alterStaffCodeDetails, alterStaffCodeDetails.Length + 1);
                    //            alterStaffDetails[alterStaffDetails.Length - 1] = alterStaffNameCode;
                    //            alterStaffCodeDetails[alterStaffCodeDetails.Length - 1] = alterStaffCode;
                    //            if (li.Selected)
                    //            {
                    //                selectedCount++;

                    //                if (!string.IsNullOrEmpty(alterStaffText))
                    //                {
                    //                    alterStaffText += "-" + alterStaffNameCode;
                    //                    alterStaffTag += "-" + alterStaffCode;
                    //                }
                    //                else
                    //                {
                    //                    alterStaffText = alterStaffNameCode;
                    //                    alterStaffTag = alterStaffCode;
                    //                }
                    //            }
                    //        }
                    //    }
                    //    if (!string.IsNullOrEmpty(alterStaffText) && selectedCount > 1)
                    //    {
                    //        Array.Resize(ref alterStaffDetails, alterStaffDetails.Length + 1);
                    //        Array.Resize(ref alterStaffCodeDetails, alterStaffCodeDetails.Length + 1);
                    //        alterStaffDetails[alterStaffDetails.Length - 1] = alterStaffText;
                    //        alterStaffCodeDetails[alterStaffCodeDetails.Length - 1] = alterStaffTag;
                    //    }
                    //}
                    int staf_cnt = 0;
                    string staff_code = string.Empty;
                    string staff_name_code = string.Empty;
                    int parent_count = subjtree.Nodes.Count;//----------count parent node value
                    for (int i = 0; i < parent_count; i++)
                    {
                        for (int node_count = 0; node_count < subjtree.Nodes[i].ChildNodes.Count; node_count++)//-------count child node
                        {
                            if (subjtree.Nodes[i].ChildNodes[node_count].Selected == true)//-------check checked condition
                            {
                                fpSpreadTreeNode.Visible = true;
                                subjtree.Visible = true;
                                chkappend.Visible = true;
                                btnOk.Visible = true;
                                //  treepanel.Visible = true;
                                //FpSpread1.Sheets[0].Cells[rowval, 0].CellType=new ch
                                fpSpreadTreeNode.Sheets[0].SetText(rowval, 0, subjtree.Nodes[i].ChildNodes[node_count].Text);
                                fpSpreadTreeNode.Sheets[0].Cells[rowval, 0].Tag = subjtree.Nodes[i].ChildNodes[node_count].Value;
                                string chile_index = subjtree.Nodes[i].ChildNodes[node_count].Value;
                                fpSpreadTreeNode.Sheets[0].Rows[rowval].Font.Name = "Book Antiqua";
                                fpSpreadTreeNode.Sheets[0].Rows[rowval].Font.Size = FontUnit.Medium;
                                DataSet staf_set = dacess.select_method("select staff_code,staff_name from staffmaster where staff_code in (select staff_code from staff_selector where subject_no = " + Convert.ToInt32(chile_index) + " and batch_year=" + strbatchyear.ToString() + " " + strsec + ")", hat, "Text");
                                if (staf_set.Tables[0].Rows.Count > 1)
                                {
                                    txtmulstaff.Visible = true;
                                    lblmulstaff.Visible = true;
                                    // btnFreeStaff.Visible = true;
                                    string[] staff_list = new string[staf_set.Tables[0].Rows.Count + 2];
                                    for (staf_cnt = 0; staf_cnt < staf_set.Tables[0].Rows.Count; staf_cnt++)
                                    {
                                        staff_list[staf_cnt] = staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                        if (staff_code == "")
                                        {
                                            staff_code = staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                            staff_name_code = staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                        }
                                        else
                                        {
                                            staff_code = staff_code + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                            staff_name_code = staff_name_code + ";" + staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                        }
                                    }
                                    if (staff_list.GetUpperBound(0) > 0)
                                    {
                                        staff_list[staf_cnt] = stafftext;
                                        staff_list[staf_cnt + 1] = "All";
                                    }
                                    FarPoint.Web.Spread.ComboBoxCellType staf_combo = new FarPoint.Web.Spread.ComboBoxCellType(staff_list);
                                    staf_combo.AutoPostBack = true;
                                    fpSpreadTreeNode.Sheets[0].Cells[rowval, 1].CellType = staf_combo;
                                    fpSpreadTreeNode.Sheets[0].Cells[rowval, 1].Locked = false;
                                }
                                fpSpreadTreeNode.Sheets[0].Cells[rowval, 1].Text = stafftext;
                                fpSpreadTreeNode.Sheets[0].Cells[rowval, 1].Tag = stafftag;

                                FarPoint.Web.Spread.ComboBoxCellType chkAlterStaff = new FarPoint.Web.Spread.ComboBoxCellType(alterStaffDetails, alterStaffCodeDetails);
                                fpSpreadTreeNode.Sheets[0].Cells[rowval, 3].CellType = chkAlterStaff;
                                fpSpreadTreeNode.Sheets[0].Cells[rowval, 3].Text = alterStaffText;
                                fpSpreadTreeNode.Sheets[0].Cells[rowval, 3].Tag = alterStaffTag;
                                fpSpreadTreeNode.Sheets[0].Columns[3].Visible = isHasAlternateStaff;
                                fpSpreadTreeNode.Visible = true;
                            }
                            fpSpreadTreeNode.SaveChanges();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    #region Free staff list PopUp
    private void BindCollege()
    {
        try
        {
            ddlAlterFreeCollege.Items.Clear();
            string qry = "select collname,college_code from collinfo";
            DataTable dtCollege = dirAcc.selectDataTable(qry);
            if (dtCollege.Rows.Count > 0)
            {
                ddlAlterFreeCollege.DataSource = dtCollege;
                ddlAlterFreeCollege.DataTextField = "collname";
                ddlAlterFreeCollege.DataValueField = "college_code";
                ddlAlterFreeCollege.DataBind();
            }
        }
        catch
        {
        }
    }

    private void BindAlterStaffDepartment(string collegeCode)
    {
        try
        {
            ddlAlterFreeDepartment.Items.Clear();
            DataTable dtDept = new DataTable();
            string qry = "";
            if (!string.IsNullOrEmpty(collegeCode))
            {
                qry = "select distinct dept_name,dept_code from hrdept_master where college_code='" + collegeCode + "'";
                dtDept = dirAcc.selectDataTable(qry);
            }
            if (dtDept.Rows.Count > 0)
            {
                ddlAlterFreeDepartment.DataSource = dtDept;
                ddlAlterFreeDepartment.DataTextField = "dept_name";
                ddlAlterFreeDepartment.DataValueField = "dept_code";
                ddlAlterFreeDepartment.DataBind();
                ddlAlterFreeDepartment.Items.Insert(0, new ListItem("All", ""));
            }
        }
        catch
        {
        }
    }

    protected void ddlAlterFreeCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindAlterStaffDepartment((ddlAlterFreeCollege.Items.Count > 0) ? Convert.ToString(ddlAlterFreeCollege.SelectedValue).Trim() : "");
            GetStaffDetails();
        }
        catch
        {

        }
    }

    protected void ddlAlterFreeDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GetStaffDetails();
        }
        catch
        {

        }
    }

    protected void ddlAlterFreeStaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GetStaffDetails();
        }
        catch
        {

        }
    }

    protected void txtAlterFreeStaffSearch_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GetStaffDetails();
        }
        catch
        {

        }
    }

    private void GetStaffDetails()
    {
        try
        {
            DataTable dtStaffDetails = new DataTable();
            DateTime dtAlterDate = new DateTime();
            DateTime.TryParseExact(txtDate.Text, "dd/MM/yyyy", null, DateTimeStyles.None, out dtAlterDate);
            int period = 0;
            int.TryParse(Convert.ToString(Session["period"]), out period);
            dtStaffDetails = getFreeStaffListNew(dtAlterDate: dtAlterDate, period: period, type: 1, searchValue: txtAlterFreeStaffSearch.Text.Trim());
            //dtStaffDetails = getFreeStaffListNew(dtAlterDate, period, type, searchValue);
            InitSpread(FpAlterFreeStaffList);
            Farpoint.CheckBoxCellType chkCellAll = new Farpoint.CheckBoxCellType();
            chkCellAll.AutoPostBack = true;
            Farpoint.TextCellType txtCell = new Farpoint.TextCellType();
            Farpoint.CheckBoxCellType chkOneByOne = new Farpoint.CheckBoxCellType();
            chkOneByOne.AutoPostBack = false;
            int sno = 0;
            if (dtStaffDetails.Rows.Count > 0)
            {
                foreach (DataRow drStaffDetails in dtStaffDetails.Rows)
                {
                    string staffCode = Convert.ToString(drStaffDetails["staff_code"]).Trim();
                    string staffName = Convert.ToString(drStaffDetails["staff_name"]).Trim();
                    sno++;
                    FpAlterFreeStaffList.Sheets[0].RowCount++;
                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno).Trim();
                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 0].Locked = true;
                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 1].CellType = chkOneByOne;
                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 1].Locked = false;
                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(staffCode).Trim();
                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 2].Locked = true;
                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;

                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(staffName).Trim();
                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 3].Locked = true;
                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                    FpAlterFreeStaffList.Sheets[0].Cells[FpAlterFreeStaffList.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;

                }
                FpAlterFreeStaffList.SaveChanges();
                FpAlterFreeStaffList.Sheets[0].PageSize = FpAlterFreeStaffList.Sheets[0].RowCount;
                FpAlterFreeStaffList.Height = 500;
                FpAlterFreeStaffList.SaveChanges();
                FpAlterFreeStaffList.Visible = true;
            }
            else
            {
            }
        }
        catch
        {
        }
    }

    protected void btnSelectStaff_Click(object sender, EventArgs e)
    {
        if (chkForAlternateStaff.Checked)
        {
            try
            {
                bool isHasAlternateStaff = false;
                //if (chkSelectAlterStaff.Checked || chkForAlternateStaff.Checked)
                if (chkForAlternateStaff.Checked)
                    isHasAlternateStaff = true;

                FpAlterFreeStaffList.SaveChanges();
                fpSpreadTreeNode.SaveChanges();
                int activeROw = -1;
                Dictionary<string, string> dicselectedstaff = new Dictionary<string, string>();
                //ddlAlternamteStaff.Items.Clear();
                //if (ddlAlternamteStaff.Items.Count == 0)
                //    ddlAlternamteStaff.Items.Insert(0, new ListItem("--Select--", ""));
                for (int row = 1; row < FpAlterFreeStaffList.Sheets[0].RowCount; row++)
                {
                    int selected = 0;
                    int.TryParse(Convert.ToString(FpAlterFreeStaffList.Sheets[0].Cells[row, 1].Value).Trim(), out selected);
                    string staffName = Convert.ToString(FpAlterFreeStaffList.Sheets[0].Cells[row, 3].Text).Trim();
                    string staffCode = Convert.ToString(FpAlterFreeStaffList.Sheets[0].Cells[row, 2].Text).Trim();

                    if (selected == 1)
                    {
                        activeROw = Convert.ToInt32(FpAlterFreeStaffList.Sheets[0].Cells[row, 3].Tag);
                        //ddlAlternamteStaff.Items.Add(new ListItem(staffName + "-" + staffCode, staffCode));
                        if (!dicselectedstaff.ContainsKey(staffCode))
                            dicselectedstaff.Add(staffName + "-" + staffCode, staffCode);
                    }
                }

                divAlterFreeStaffDetails.Visible = false;
                lblAlterFreeStaffError.Text = string.Empty;
                lblAlterFreeStaffError.Visible = false;
                txtAlterFreeStaffSearch.Text = string.Empty;
                // chkSelectAlterStaff_CheckedChanged(sender, e);

                if (activeROw != -1)
                {
                    string[] staffarr = new string[dicselectedstaff.Count];
                    string[] staffcodearr = new string[dicselectedstaff.Count];
                    int arrval = 0;
                    foreach (KeyValuePair<string, string> item in dicselectedstaff)
                    {
                        staffarr[arrval] = item.Key;
                        staffcodearr[arrval] = item.Value;
                        arrval++;
                    }
                    FarPoint.Web.Spread.ComboBoxCellType staf_combo = new FarPoint.Web.Spread.ComboBoxCellType(staffarr);
                    fpSpreadTreeNode.Sheets[0].Cells[activeROw, 3].CellType = staf_combo;
                    fpSpreadTreeNode.Sheets[0].Cells[activeROw, 3].Tag = staffcodearr;
                    fpSpreadTreeNode.Sheets[0].Cells[activeROw, 3].Value = staffarr;
                    fpSpreadTreeNode.Sheets[0].Columns[3].Visible = isHasAlternateStaff;

                    fpSpreadTreeNode.SaveChanges();
                }
                if (dicselectedstaff.Count > 0)
                    fpSpreadTreeNode.Sheets[0].Columns[3].Visible = isHasAlternateStaff;
            }
            catch
            {

            }
        }
        else
        {
            try
            {
                FpAlterFreeStaffList.SaveChanges();
                //ddlAlternamteStaff.Items.Clear();
                //if (ddlAlternamteStaff.Items.Count == 0)
                //    ddlAlternamteStaff.Items.Insert(0, new ListItem("--Select--", ""));
                for (int row = 1; row < FpAlterFreeStaffList.Sheets[0].RowCount; row++)
                {
                    int selected = 0;
                    int.TryParse(Convert.ToString(FpAlterFreeStaffList.Sheets[0].Cells[row, 1].Value).Trim(), out selected);
                    string staffName = Convert.ToString(FpAlterFreeStaffList.Sheets[0].Cells[row, 3].Text).Trim();
                    string staffCode = Convert.ToString(FpAlterFreeStaffList.Sheets[0].Cells[row, 2].Text).Trim();
                    //if (selected == 1)
                    //{
                    //    ddlAlternamteStaff.Items.Add(new ListItem(staffName + "-" + staffCode, staffCode));
                    //}
                }
                divAlterFreeStaffDetails.Visible = false;
                lblAlterFreeStaffError.Text = string.Empty;
                lblAlterFreeStaffError.Visible = false;
                txtAlterFreeStaffSearch.Text = string.Empty;
            }
            catch
            {

            }
        }
    }

    protected void btnFreeStaffExit_Click(object sender, EventArgs e)
    {
        try
        {
            divAlterFreeStaffDetails.Visible = false;
            lblAlterFreeStaffError.Text = string.Empty;
            lblAlterFreeStaffError.Visible = false;
            txtAlterFreeStaffSearch.Text = string.Empty;
        }
        catch
        {

        }
    }

    protected void FpAlterFreeStaffList_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            FpAlterFreeStaffList.SaveChanges();
            int r = FpAlterFreeStaffList.Sheets[0].ActiveRow;
            int j = FpAlterFreeStaffList.Sheets[0].ActiveColumn;
            if (r == 0 && j == 1)
            {
                int val = 0;
                int.TryParse(Convert.ToString(FpAlterFreeStaffList.Sheets[0].Cells[r, j].Value).Trim(), out val);
                for (int row = 1; row < FpAlterFreeStaffList.Sheets[0].RowCount; row++)
                {
                    if (FpAlterFreeStaffList.Sheets[0].Cells[row, 0].Text != string.Empty)
                    {
                        if (val == 1)
                            FpAlterFreeStaffList.Sheets[0].Cells[row, j].Value = 1;
                        else
                            FpAlterFreeStaffList.Sheets[0].Cells[row, j].Value = 0;
                    }
                }
            }
        }
        catch
        {
        }
    }


    #endregion

    #region As per day schedule button
    public void btnAsPerDaySchedule_Click()
    {
        try
        {

            string Syllabus_year = string.Empty;
            Syllabus_year = GetSyllabusYear(Convert.ToString(Session["degcode"]), Convert.ToString(Session["batch"]), Convert.ToString(Session["sem"]));
            if ((Syllabus_year).ToString() != "0" && cellTagValue != "")
            {
                loadschedule();
                semspread.Visible = true;
                semmsglbl.Visible = false;
            }
            if (cellTagValue == "")
            {
                semspread.Visible = false;
                semmsglbl.Visible = true;
                semmsglbl.Text = "Please select valid cell!";
            }
        }
        catch (Exception ex) { }
    }

    public void loadschedule()
    {
        try
        {
            string strsec = string.Empty;
            int intNHrs = 0;
            int SchOrder = 0;
            int nodays = 0;
            string srt_day = string.Empty;
            int order = 0;
            int insert_val = 0;
            string sunjno_staffno = string.Empty;
            int subj_no = 0;
            string acronym_val = string.Empty;
            int day_list = 0;
            string day_order = string.Empty;
            int ind_subj = 0;
            string sunjno_staffno_s = string.Empty;
            string acro = string.Empty;
            string acronym = string.Empty;
            string alt_sched = string.Empty;
            string shed_list = string.Empty;
            int spreadDet_ac = 0;
            string todate = string.Empty;

            semspread.Sheets[0].AutoPostBack = true;

            #region spreadsheet design
            semspread.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            semspread.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            semspread.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
            semspread.ActiveSheetView.RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
            semspread.ActiveSheetView.RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            semspread.ActiveSheetView.RowHeader.DefaultStyle.Font.Bold = true;
            semspread.CommandBar.Visible = false;
            FarPoint.Web.Spread.StyleInfo style3 = new FarPoint.Web.Spread.StyleInfo();
            style3.Font.Size = 13;
            style3.Font.Bold = true;
            semspread.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style3);
            semspread.Sheets[0].AllowTableCorner = true;
            semspread.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;


            #endregion

            semspread.Sheets[0].RowCount = 0;
            semspread.Sheets[0].ColumnCount = 0;
            spreadDet_ac = spreadDet.ActiveSheetView.ActiveColumn;
            semspread.Sheets[0].SheetCorner.Cells[0, 0].Text = "Day/Week Order";
            //-------------date
            string date1;
            string selectedDate;
            date1 = txtDate.Text.ToString();
            string[] split = date1.Split(new Char[] { '/' });
            selectedDate = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();

            DateTime dtSelectedDate = Convert.ToDateTime(selectedDate.ToString());

            string semStartdate = string.Empty;
            //-------------start date

            string qry = "select start_date from seminfo where degree_code=" + Convert.ToString(Session["degcode"]) + " and semester=" + Convert.ToString(Session["sem"]) + " and batch_year=" + Convert.ToString(Session["batch"]) + " ";

            DataSet qryDataSet = dacess.select_method_wo_parameter(qry, "Text");

            if (qryDataSet.Tables.Count > 0 && qryDataSet.Tables[0].Rows.Count > 0)
            {
                semStartdate = Convert.ToString(qryDataSet.Tables[0].Rows[0]["start_date"]);
            }
            //-------section
            if (Convert.ToString(Session["sec"]) == " ")
            {
                strsec = string.Empty;
            }
            else
            {
                if (Convert.ToString(Session["sec"]) == "-1")
                {
                    strsec = string.Empty;
                }
                else
                {
                    strsec = " and sections='" + Convert.ToString(Session["sec"]) + "'";
                }
            }
            semspread.Sheets[0].ColumnCount = 0;
            semspread.Sheets[0].RowCount = 0;

            string periodDetailsQry = "Select No_of_hrs_per_day,schorder,nodays from periodattndschedule where degree_code=" + Convert.ToString(Session["degcode"]) + " and semester = " + Convert.ToString(Session["sem"]) + "";
            DataSet periodDetailsDataSet = dacess.select_method_wo_parameter(periodDetailsQry, "Text");
            if (periodDetailsDataSet.Tables.Count > 0 && periodDetailsDataSet.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(periodDetailsDataSet.Tables[0].Rows[0]["No_of_hrs_per_day"]) != "")
                {
                    intNHrs = Convert.ToInt32(periodDetailsDataSet.Tables[0].Rows[0]["No_of_hrs_per_day"]);
                    SchOrder = Convert.ToInt32(periodDetailsDataSet.Tables[0].Rows[0]["schorder"]);
                    nodays = Convert.ToInt32(periodDetailsDataSet.Tables[0].Rows[0]["nodays"]);

                }
            }
            //------------------------dayorder
            if (intNHrs > 0)
            {
                if (SchOrder != 0)
                {
                    srt_day = dtSelectedDate.ToString("ddd");
                    semspread.Sheets[0].RowCount = nodays;
                    if (nodays >= 1)
                    {
                        semspread.Sheets[0].RowHeader.Cells[0, 0].Text = "Monday";
                        semspread.Sheets[0].RowHeader.Cells[0, 0].Tag = "mon";
                        semspread.Sheets[0].RowHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Left;
                    }
                    if (nodays >= 2)
                    {
                        semspread.Sheets[0].RowHeader.Cells[1, 0].Text = "Tueday";
                        semspread.Sheets[0].RowHeader.Cells[1, 0].Tag = "tue";
                        semspread.Sheets[0].RowHeader.Cells[1, 0].HorizontalAlign = HorizontalAlign.Left;
                    }
                    if (nodays >= 3)
                    {
                        semspread.Sheets[0].RowHeader.Cells[2, 0].Text = "Wednesday";
                        semspread.Sheets[0].RowHeader.Cells[2, 0].Tag = "wed";
                        semspread.Sheets[0].RowHeader.Cells[2, 0].HorizontalAlign = HorizontalAlign.Left;
                    }
                    if (nodays >= 4)
                    {
                        semspread.Sheets[0].RowHeader.Cells[3, 0].Text = "Thursday";
                        semspread.Sheets[0].RowHeader.Cells[3, 0].Tag = "thu";
                        semspread.Sheets[0].RowHeader.Cells[3, 0].HorizontalAlign = HorizontalAlign.Left;
                    }
                    if (nodays >= 5)
                    {
                        semspread.Sheets[0].RowHeader.Cells[4, 0].Text = "Friday";
                        semspread.Sheets[0].RowHeader.Cells[4, 0].Tag = "fri";
                        semspread.Sheets[0].RowHeader.Cells[4, 0].HorizontalAlign = HorizontalAlign.Left;
                    }
                    if (nodays >= 6)
                    {
                        semspread.Sheets[0].RowHeader.Cells[5, 0].Text = "Saturday";
                        semspread.Sheets[0].RowHeader.Cells[5, 0].Tag = "sat";
                        semspread.Sheets[0].RowHeader.Cells[5, 0].HorizontalAlign = HorizontalAlign.Left;
                    }
                }
                else
                {
                    todate = Convert.ToString(txtDate.Text);

                    string[] sps = todate.ToString().Split('/');
                    string curdate = sps[1] + '/' + sps[0] + '/' + sps[2];
                    srt_day = dacess.findday(curdate.ToString(), Convert.ToString(Session["degcode"]), Convert.ToString(Session["sem"]), Convert.ToString(Session["batch"]), semStartdate.ToString(), nodays.ToString(), start_dayorder.ToString());
                    for (order = 1; order <= nodays; order++)
                    {
                        semspread.Sheets[0].RowCount = semspread.Sheets[0].RowCount + 1;
                        semspread.Sheets[0].RowHeader.Cells[order - 1, 0].Text = "Dayorder" + order;
                        semspread.Sheets[0].RowHeader.Cells[order - 1, 0].Tag = srt_day;
                    }
                }
            }
            string[] daylist = { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };

            string semScheduleQry = "select top 1 * from semester_schedule where batch_year=" + Convert.ToString(Session["batch"]) + " and degree_code = " + Convert.ToString(Session["degcode"]) + " and semester = " + Convert.ToString(Session["sem"]) + " and FromDate<= ' " + Convert.ToString(selectedDate) + " ' " + strsec + " order by fromdate desc";
            DataSet semScheduleDataSet = dacess.select_method_wo_parameter(semScheduleQry, "Text");

            if (semScheduleDataSet.Tables.Count > 0 && semScheduleDataSet.Tables[0].Rows.Count > 0)
            {
                semspread.Sheets[0].ColumnCount = intNHrs;
                for (day_list = 0; day_list < nodays; day_list++)
                {
                    for (insert_val = 1; insert_val <= intNHrs; insert_val++)
                    {
                        semspread.Sheets[0].ColumnHeader.Cells[0, insert_val - 1].Text = "Period " + insert_val.ToString();
                        acro = string.Empty;
                        shed_list = string.Empty;
                        day_order = daylist[day_list] + insert_val.ToString();
                        sunjno_staffno = Convert.ToString(semScheduleDataSet.Tables[0].Rows[0][day_order]);

                        //---------------getupper bound for many subject
                        string[] many_subj = sunjno_staffno.Split(new Char[] { ';' });
                        for (ind_subj = 0; ind_subj <= many_subj.GetUpperBound(0); ind_subj++)
                        {
                            if (many_subj.GetUpperBound(0) >= 0)
                            {
                                sunjno_staffno_s = many_subj[ind_subj];
                                if (sunjno_staffno_s.Trim() != "")
                                {
                                    //---------------------------
                                    string[] subjno_staffno_splt = sunjno_staffno_s.Split(new Char[] { '-' });
                                    subj_no = Convert.ToInt32(subjno_staffno_splt[0].ToString());
                                    //---------tag
                                    SqlDataReader sub_dr;
                                    SqlCommand sub_cmd;
                                    con2a.Close();
                                    con2a.Open();
                                    sub_cmd = new SqlCommand("select subject_name from subject where subject_no=" + subj_no.ToString() + "", con2a);
                                    sub_dr = sub_cmd.ExecuteReader();
                                    sub_dr.Read();
                                    if (sub_dr.HasRows == true)
                                    {
                                        alt_sched = sub_dr[0].ToString() + "-" + subjno_staffno_splt[1].ToString() + "-" + subjno_staffno_splt[2].ToString();
                                    }
                                    //------------------
                                    cona.Close();
                                    cona.Open();
                                    acronym_val = "select isnull(acronym,subject_code) acronym from subject where subject_no=" + subj_no.ToString() + " ";
                                    SqlCommand ac_cmd = new SqlCommand(acronym_val, cona);
                                    SqlDataReader ac_dr;
                                    ac_dr = ac_cmd.ExecuteReader();
                                    ac_dr.Read();
                                    if (ac_dr.HasRows == true)
                                    {
                                        acronym = ac_dr["acronym"].ToString();
                                        if (acro == "")
                                        {
                                            acro = acro + acronym;
                                        }
                                        else
                                        {
                                            acro = acro + "," + acronym;
                                        }
                                        if (shed_list == "")
                                        {
                                            shed_list = shed_list + alt_sched;
                                        }
                                        else
                                        {
                                            shed_list = shed_list + ";" + alt_sched;
                                        }
                                    }
                                }
                            }
                        }
                        semspread.Sheets[0].Cells[day_list, insert_val - 1].Text = acro.ToString();
                        semspread.Sheets[0].Cells[day_list, insert_val - 1].Font.Name = "Book Antiqua";
                        semspread.Sheets[0].Cells[day_list, insert_val - 1].Font.Size = FontUnit.Medium;
                        semspread.Sheets[0].Cells[day_list, insert_val - 1].Tag = shed_list;
                        semspread.Sheets[0].Cells[day_list, insert_val - 1].Note = sunjno_staffno;
                    }
                }
                semspread.SaveChanges();
            }

        }


        catch (Exception ex)
        {

        }
    }

    protected void semspread_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            if (spreadDetCellClick && chkPerDAySched.Checked)
            {
                if (cellTagValue != "")
                    semspread.Visible = true;
                //semmsglbl.Visible = false;

                //btnAsPerDaySchedule_Click();
                semspread.SaveChanges();
            }
            if (semspreadCellClick)
            {
                assignAlternateData();
                btnsave.Enabled = true;
                btnsave.Visible = true;
            }
        }
        catch (Exception ex) { }
    }

    protected void semspread_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            semspreadCellClick = true;

        }
        catch
        {
        }
    }

    protected void assignAlternateData()
    {
        try
        {
            string actRow = string.Empty;
            string ar = string.Empty;
            int spreadDet_actRow = 0;
            int semspread_actRow = 0;
            string sqlstr = string.Empty;
            int noofhrs = 0;
            int periodCnt = 0;

            //------------------set the day schedule alter into the main spread from popup semester schedule spread
            actRow = spreadDet.ActiveSheetView.ActiveRow.ToString();
            spreadDet_actRow = Convert.ToInt32(actRow);

            ar = semspread.ActiveSheetView.ActiveRow.ToString();
            semspread_actRow = Convert.ToInt32(ar.ToString());

            string periodSchedQry = "Select No_of_hrs_per_day,schorder,nodays from periodattndschedule where degree_code=" + Convert.ToString(Session["degcode"]) + " and semester = " + Convert.ToString(Session["sem"]) + "";
            DataSet periodSchedDataSet = dacess.select_method_wo_parameter(periodSchedQry, "Text");
            if (periodSchedDataSet.Tables.Count > 0 && periodSchedDataSet.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(periodSchedDataSet.Tables[0].Rows[0]["No_of_hrs_per_day"]) != "")
                {
                    noofhrs = Convert.ToInt32(periodSchedDataSet.Tables[0].Rows[0]["No_of_hrs_per_day"]);
                }
            }
            sqlstr = "select No_of_hrs_per_day from PeriodAttndSchedule where degree_code='" + Convert.ToString(Session["degcode"]) + "' and semester=" + Convert.ToString(Session["sem"]) + " ";
            noofhrs = Convert.ToInt32(GetFunction(sqlstr));
            for (periodCnt = 0; periodCnt < noofhrs; periodCnt++)//----set value
            {
                string getvalu = semspread.Sheets[0].Cells[semspread_actRow, periodCnt].Note.ToString();
                string[] splitsub = getvalu.Split(';');
                for (int i = 0; i <= splitsub.GetUpperBound(0); i++)
                {
                    string[] spitsublab = splitsub[i].Split('-');
                    if (spitsublab.GetUpperBound(0) >= 0)
                    {
                        string subcode = spitsublab[0].ToString();
                        if (subcode.Trim() != "" && subcode != null)
                        {
                            string chklab = dacess.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subcode.ToString() + "'");
                            if (chklab.Trim() == "1" || chklab.ToLower().Trim() == "true")
                            {
                                string getday = semspread.Sheets[0].RowHeader.Cells[semspread_actRow, 0].Tag.ToString();
                                string setval = getday + ',' + periodCnt;
                                spreadDet.Sheets[0].ColumnHeader.Cells[0, periodCnt + 2].Tag = setval;
                            }
                        }
                    }
                }
                spreadDet.Sheets[0].Cells[spreadDet_actRow, periodCnt + 2].Text = semspread.Sheets[0].Cells[semspread_actRow, periodCnt].Tag.ToString();
                spreadDet.Sheets[0].Cells[spreadDet_actRow, periodCnt + 2].Note = semspread.Sheets[0].Cells[semspread_actRow, periodCnt].Note.ToString();
                spreadDet.SaveChanges();
                spreadDet.Sheets[0].Cells[spreadDet_actRow, periodCnt + 2].ForeColor = Color.Green;
                spreadDet.SaveChanges();
            }

        }
        catch (Exception ex) { }
    }
    #endregion

    protected void btnsave_Click(object sender, EventArgs e)
    {
        getAlert();
    }

    protected void getAlert()
    {
        try
        {


            string Dateval;
            int jj = 0;
            int j = 0;
            string strsec = string.Empty;
            int intNHrs = 0;
            int SchOrder = 0;
            string strDay = string.Empty;
            string Strsql = string.Empty;
            int nodays = 0;
            string strinsert = string.Empty;
            string VarSch = string.Empty;
            string dateval;
            string Strsqlval = string.Empty;
            string startdate = string.Empty;
            string todate = string.Empty;
            string batch = Convert.ToString(Session["batch"]);
            string degree = Convert.ToString(Session["degcode"]);
            string sem = Convert.ToString(Session["sem"]);
            int spreadDetActRow = Convert.ToInt32(spreadDet.ActiveSheetView.ActiveRow);

            DataSet dssetbatch = new DataSet();
            Boolean aeperflag = false;
            SqlConnection con7 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
            string smssend = dacess.GetFunction("select value from Master_Settings where settings='Alternatesms'");
            if (spreadDet.Sheets[0].RowCount > 0)
            {
                if (Convert.ToString(Session["sec"]) == "")
                {
                    strsec = string.Empty;
                }
                else
                {
                    if (Convert.ToString(Session["sec"]) == "-1")
                    {
                        strsec = string.Empty;
                    }
                    else
                    {
                        strsec = " and sections='" + Convert.ToString(Session["sec"]) + "'";
                    }
                }


                Hashtable hataltersc = new Hashtable();

                for (jj = 0; jj <= 0; jj++)//----------incement column value
                {
                    Dateval = Convert.ToString(txtDate.Text);
                    string[] split = Dateval.Split(new Char[] { '/' });
                    dateval = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                    DateTime head_date = Convert.ToDateTime(dateval.ToString());

                    string periodSchedQry = "Select No_of_hrs_per_day,schorder,nodays from periodattndschedule where degree_code=" + degree + " and semester = " + sem + "";
                    DataSet periodSchedDataSet = dacess.select_method_wo_parameter(periodSchedQry, "Text");
                    if (periodSchedDataSet.Tables.Count > 0 && periodSchedDataSet.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToString(periodSchedDataSet.Tables[0].Rows[0]["No_of_hrs_per_day"]) != "")
                        {
                            intNHrs = Convert.ToInt32(periodSchedDataSet.Tables[0].Rows[0]["No_of_hrs_per_day"]);
                            SchOrder = Convert.ToInt32(periodSchedDataSet.Tables[0].Rows[0]["schorder"]);
                            nodays = Convert.ToInt32(periodSchedDataSet.Tables[0].Rows[0]["nodays"]);

                        }
                    }

                    string semDetailsQry = "select * from seminfo where degree_code=" + degree + " and semester=" + sem + " and batch_year=" + batch + " ";
                    DataSet semDetailsDataSet = dacess.select_method_wo_parameter(semDetailsQry, "Text");
                    if (semDetailsDataSet.Tables.Count > 0 && semDetailsDataSet.Tables[0].Rows.Count > 0)
                    {
                        if ((Convert.ToString(semDetailsDataSet.Tables[0].Rows[0]["start_date"])) != "" && (Convert.ToString(semDetailsDataSet.Tables[0].Rows[0]["start_date"])) != "\0")
                        {
                            string[] tmpdate = Convert.ToString(semDetailsDataSet.Tables[0].Rows[0]["start_date"]).Split(new char[] { ' ' });
                            startdate = tmpdate[0].ToString();
                            if (Convert.ToString(semDetailsDataSet.Tables[0].Rows[0]["starting_dayorder"]) != "")
                            {
                                start_dayorder = Convert.ToString(semDetailsDataSet.Tables[0].Rows[0]["starting_dayorder"]);
                            }
                            else
                            {
                                start_dayorder = "1";
                            }
                        }
                        else
                        {
                            norecordlbl.Visible = true;
                            norecordlbl.Text = "Update semester Information";
                            norecordlbl.ForeColor = Color.Red;
                        }
                    }
                    else
                    {
                        norecordlbl.Visible = true;
                        norecordlbl.Text = "Update semester Information";
                        norecordlbl.ForeColor = Color.Red;
                    }
                    if (intNHrs > 0)
                    {
                        if (SchOrder != 0)
                        {
                            strDay = head_date.ToString("ddd");
                        }
                        else
                        {
                            todate = Convert.ToString(txtDate.Text);

                            string[] sps = todate.ToString().Split('/');
                            string curdate = sps[1] + '/' + sps[0] + '/' + sps[2];
                            strDay = dacess.findday(curdate.ToString(), degree, sem, batch, startdate.ToString(), nodays.ToString(), start_dayorder.ToString());
                        }
                        string ttname = dacess.GetFunction("select  top 1 ttname from Semester_Schedule where batch_year=" + batch + " and degree_code = " + degree + " and semester = " + sem + " and FromDate <='" + dateval + "'" + strsec + " order by FromDate desc");
                        if (ttname.Trim() != "" && ttname != null && ttname.Trim() != "0")
                        {
                            ttname = " and Timetablename='" + ttname + "'";
                        }
                        Strsqlval = string.Empty;
                        Strsqlval = "select top 1 ";
                        string getday = string.Empty;
                        for (int intNCtr = 1; intNCtr <= intNHrs; intNCtr++)
                        {
                            Strsqlval = Strsqlval + strDay + intNCtr.ToString() + ",";
                            if (getday == "")
                            {
                                getday = strDay + intNCtr.ToString();
                            }
                            else
                            {
                                getday = getday + "," + strDay + intNCtr.ToString();
                            }
                        }
                        //---------------------check the record in alternate schedule for update

                        Strsql = Strsqlval + " degree_code , semester , batch_year,lastrec,sections,TTName,Fromdate from Alternate_schedule where batch_year=" + batch + " and degree_code = " + degree + " and semester = " + sem + " and FromDate ='" + dateval + "'" + strsec + " order by FromDate";

                        DataSet StrsqlvalDataSet = dacess.select_method_wo_parameter(Strsql, "Text");

                        string code_value = string.Empty;
                        string cellnote = string.Empty;
                        string sectionval = string.Empty;
                        Boolean isaltflaf = false;
                        if ((Convert.ToString(Session["sec"])) != "" && (Convert.ToString(Session["sec"]) != "All"))
                        {
                            sectionval = Convert.ToString(Session["sec"]);
                        }
                        for (int alt = 1; alt <= 1; alt++)
                        {
                            string selnoofalter = Strsqlval + " degree_code , semester , batch_year,lastrec,sections,TTName,Fromdate,No_of_Alter from tbl_alter_schedule_Details where batch_year=" + batch + " and degree_code = " + degree + " and semester = " + sem + " and FromDate ='" + dateval + "'" + strsec + " and No_of_Alter='" + alt + "' order by FromDate";
                            DataSet dsnoofalter = dacess.select_method(selnoofalter, hat, "Text");
                            if (dsnoofalter.Tables[0].Rows.Count > 0)
                            {
                                string deltequery = "delete from tbl_alter_schedule_Details where batch_year=" + batch + " and degree_code = " + degree + " and semester = " + sem + " and FromDate ='" + dateval + "'" + strsec + " and No_of_Alter='" + alt + "'";
                                int del = dacess.update_method_wo_parameter(deltequery, "Text");
                            }
                            string getStaffCode = string.Empty;
                            int colCnt = 0;
                            string altersubdeta = string.Empty;
                            string stfName = string.Empty;
                            int periodNo = 0;
                            for (j = 2; j < spreadDet.Sheets[0].ColumnCount; j++)//---------------loop for col value
                            {
                                periodNo++;
                                string substaffname = spreadDet.Sheets[0].Cells[spreadDetActRow, j].Text;
                                string substaffcode = spreadDet.Sheets[0].Cells[spreadDetActRow, j].Note;
                                string dayval = strDay + Convert.ToInt32(periodNo).ToString();
                                string getlabdetails = string.Empty;
                                if (substaffname != "" && substaffcode != "" && substaffname != "Sunday")
                                {
                                    if (spreadDet.Sheets[0].Cells[spreadDetActRow, j].Locked == false)
                                    {
                                        if (!hataltersc.Contains(dayval))
                                        {
                                            if (spreadDet.Sheets[0].ColumnHeader.Cells[0, j].Tag != null)
                                            {
                                                getlabdetails = "/" + spreadDet.Sheets[0].ColumnHeader.Cells[0, j].Tag.ToString();
                                            }
                                            hataltersc.Add(dayval, substaffcode + getlabdetails);
                                        }
                                        else
                                        {
                                            if (spreadDet.Sheets[0].ColumnHeader.Cells[0, j].Tag != null)
                                            {
                                                getlabdetails = "/" + spreadDet.Sheets[0].ColumnHeader.Cells[0, j].Tag.ToString();
                                            }
                                            hataltersc[dayval] = substaffcode + getlabdetails;
                                        }
                                        if (altersubdeta == "")
                                        {
                                            altersubdeta = "'" + substaffcode + "'";
                                        }
                                        else
                                        {
                                            altersubdeta = altersubdeta + ",'" + substaffcode + "'";
                                        }
                                        colCnt = j + 1;
                                        getStaffCode = substaffcode;
                                        stfName = substaffname;
                                    }
                                    else
                                    {
                                        if (altersubdeta == "")
                                        {
                                            altersubdeta = "''";
                                        }
                                        else
                                        {
                                            altersubdeta = altersubdeta + ",''";
                                        }
                                    }
                                }
                                else
                                {
                                    if (altersubdeta == "")
                                    {
                                        altersubdeta = "''";
                                    }
                                    else
                                    {
                                        altersubdeta = altersubdeta + ",''";
                                    }
                                }
                            }
                            string alertStr = string.Empty;

                            bool check = getAlternateScheduleCheck(getStaffCode, strDay, colCnt, dateval, ref alertStr, stfName);
                            if (check)
                            {
                                Div3.Visible = true;
                                Label12.Text = alertStr;
                                return;
                            }
                            else
                            {
                                Save();
                            }
                        }
                    }
                }
            }
        }
        catch { }
    }

    protected void Save()
    {
        try
        {
            string Dateval;
            int jj = 0;
            int j = 0;
            string strsec = string.Empty;
            int intNHrs = 0;
            int SchOrder = 0;
            string strDay = string.Empty;
            string Strsql = string.Empty;
            int nodays = 0;
            string strinsert = string.Empty;
            string VarSch = string.Empty;
            string dateval;
            string Strsqlval = string.Empty;
            string startdate = string.Empty;
            string todate = string.Empty;
            string batch = Convert.ToString(Session["batch"]);
            string degree = Convert.ToString(Session["degcode"]);
            string sem = Convert.ToString(Session["sem"]);
            int spreadDetActRow = Convert.ToInt32(spreadDet.ActiveSheetView.ActiveRow);
            spreadDet.SaveChanges();
            DataSet dssetbatch = new DataSet();
            Boolean aeperflag = false;
            SqlConnection con7 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
            string smssend = dacess.GetFunction("select value from Master_Settings where settings='Alternatesms'");
            if (spreadDet.Sheets[0].RowCount > 0)
            {
                if (Convert.ToString(Session["sec"]) == "")
                {
                    strsec = string.Empty;
                }
                else
                {
                    if (Convert.ToString(Session["sec"]) == "-1")
                    {
                        strsec = string.Empty;
                    }
                    else
                    {
                        strsec = " and sections='" + Convert.ToString(Session["sec"]) + "'";
                    }
                }

                Hashtable hataltersc = new Hashtable();

                for (jj = 0; jj <= 0; jj++)//----------increment column value
                {
                    Dateval = Convert.ToString(txtDate.Text);
                    string[] split = Dateval.Split(new Char[] { '/' });
                    dateval = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                    DateTime head_date = Convert.ToDateTime(dateval.ToString());

                    string periodSchedQry = "Select No_of_hrs_per_day,schorder,nodays from periodattndschedule where degree_code=" + degree + " and semester = " + sem + "";
                    DataSet periodSchedDataSet = dacess.select_method_wo_parameter(periodSchedQry, "Text");
                    if (periodSchedDataSet.Tables.Count > 0 && periodSchedDataSet.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToString(periodSchedDataSet.Tables[0].Rows[0]["No_of_hrs_per_day"]) != "")
                        {
                            intNHrs = Convert.ToInt32(periodSchedDataSet.Tables[0].Rows[0]["No_of_hrs_per_day"]);
                            SchOrder = Convert.ToInt32(periodSchedDataSet.Tables[0].Rows[0]["schorder"]);
                            nodays = Convert.ToInt32(periodSchedDataSet.Tables[0].Rows[0]["nodays"]);
                        }
                    }

                    string semDetailsQry = "select * from seminfo where degree_code=" + degree + " and semester=" + sem + " and batch_year=" + batch + " ";
                    DataSet semDetailsDataSet = dacess.select_method_wo_parameter(semDetailsQry, "Text");
                    if (semDetailsDataSet.Tables.Count > 0 && semDetailsDataSet.Tables[0].Rows.Count > 0)
                    {
                        if ((Convert.ToString(semDetailsDataSet.Tables[0].Rows[0]["start_date"])) != "" && (Convert.ToString(semDetailsDataSet.Tables[0].Rows[0]["start_date"])) != "\0")
                        {
                            string[] tmpdate = Convert.ToString(semDetailsDataSet.Tables[0].Rows[0]["start_date"]).Split(new char[] { ' ' });
                            startdate = tmpdate[0].ToString();
                            if (Convert.ToString(semDetailsDataSet.Tables[0].Rows[0]["starting_dayorder"]) != "")
                            {
                                start_dayorder = Convert.ToString(semDetailsDataSet.Tables[0].Rows[0]["starting_dayorder"]);
                            }
                            else
                            {
                                start_dayorder = "1";
                            }
                        }
                        else
                        {
                            norecordlbl.Visible = true;
                            norecordlbl.Text = "Update semester Information";
                            norecordlbl.ForeColor = Color.Red;
                        }
                    }
                    else
                    {
                        norecordlbl.Visible = true;
                        norecordlbl.Text = "Update semester Information";
                        norecordlbl.ForeColor = Color.Red;
                    }
                    if (intNHrs > 0)
                    {
                        if (SchOrder != 0)
                        {
                            strDay = head_date.ToString("ddd");
                        }
                        else
                        {
                            todate = Convert.ToString(txtDate.Text);

                            string[] sps = todate.ToString().Split('/');
                            string curdate = sps[1] + '/' + sps[0] + '/' + sps[2];
                            strDay = dacess.findday(curdate.ToString(), degree, sem, batch, startdate.ToString(), nodays.ToString(), start_dayorder.ToString());
                        }
                        string ttname = dacess.GetFunction("select  top 1 ttname from Semester_Schedule where batch_year=" + batch + " and degree_code = " + degree + " and semester = " + sem + " and FromDate <='" + dateval + "'" + strsec + " order by FromDate desc");
                        if (ttname.Trim() != "" && ttname != null && ttname.Trim() != "0")
                        {
                            ttname = " and Timetablename='" + ttname + "'";
                        }
                        Strsqlval = string.Empty;
                        Strsqlval = "select top 1 ";
                        string getday = string.Empty;
                        for (int intNCtr = 1; intNCtr <= intNHrs; intNCtr++)
                        {
                            string alternateDate = string.Empty;
                            string alternateHour = string.Empty;
                            DateTime dtAlterDate = new DateTime();
                            int period = 0;

                            dtAlterDate = new DateTime();
                            alternateDate = Convert.ToString(Dateval).Trim();

                            period = intNCtr;

                            DateTime.TryParseExact(alternateDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtAlterDate);
                            Dictionary<string, string[]> dicSubjectStaffList = new Dictionary<string, string[]>();
                            string keyList = alternateDate.Trim() + "@" + period.ToString();
                            string[] subjectWiseStaffList = new string[0];
                            Dictionary<string, string> dicParam = new Dictionary<string, string>();
                            Dictionary<string, Dictionary<string, string[]>> dicAlterSubjectStaff = new Dictionary<string, Dictionary<string, string[]>>();

                            //if (!string.IsNullOrEmpty(Convert.ToString(spreadDet.Sheets[0].Cells[jj, intNCtr + 2].Tag)) && spreadDet.Sheets[0].Cells[jj, intNCtr + 2].Tag != null)
                               // dicAlterSubjectStaff = (Dictionary<string, Dictionary<string, string[]>>)(spreadDet.Sheets[0].Cells[jj, intNCtr + 2].Tag);

                            if (dicAlternateSubjectList.ContainsKey(keyList.Trim()))
                            {
                                dicParam.Clear();
                                dicParam.Add("alternateDate", dtAlterDate.ToString("MM/dd/yyyy"));
                                dicParam.Add("alternateHour", intNCtr.ToString());
                                int del = storeAcc.deleteData("uspDeleteAlternateStaffByDateHour", dicParam);
                                dicSubjectStaffList = dicAlternateSubjectList[keyList.Trim()];
                                foreach (KeyValuePair<string, string[]> keySub in dicSubjectStaffList)
                                {
                                    string keySubject = keySub.Key;
                                    subjectWiseStaffList = new string[0];
                                    subjectWiseStaffList = keySub.Value;
                                    string[] actualStaffList = new string[0];
                                    string[] alterStaffList = new string[0];
                                    foreach (string staff in subjectWiseStaffList)
                                    {
                                        string[] actualAlterStaffList = new string[0];
                                        actualAlterStaffList = staff.Split('@');
                                        actualStaffList = actualAlterStaffList[0].Split('-');
                                        alterStaffList = actualAlterStaffList[1].Split('-');
                                        foreach (string actualStaff in actualStaffList)
                                        {
                                            foreach (string alterStaff in alterStaffList)
                                            {
                                                dicParam.Clear();
                                                dicParam.Add("alternateDate", dtAlterDate.ToString("MM/dd/yyyy"));
                                                dicParam.Add("alternateHour", intNCtr.ToString());
                                                dicParam.Add("subjectNo", keySubject.ToString());
                                                dicParam.Add("actualStaffCode", actualStaff.ToString());
                                                dicParam.Add("alterStaffCode", alterStaff.ToString());
                                                int insAlter = storeAcc.insertData("uspInsertAlternateStaffDetail", dicParam);
                                            }
                                        }
                                    }
                                }
                            }
                            Strsqlval = Strsqlval + strDay + intNCtr.ToString() + ",";
                            if (getday == "")
                            {
                                getday = strDay + intNCtr.ToString();
                            }
                            else
                            {
                                getday = getday + "," + strDay + intNCtr.ToString();
                            }

                        }
                        //---------------------check the record in alternate schedule for update

                        Strsql = Strsqlval + " degree_code , semester , batch_year,lastrec,sections,TTName,Fromdate from Alternate_schedule where batch_year=" + batch + " and degree_code = " + degree + " and semester = " + sem + " and FromDate ='" + dateval + "'" + strsec + " order by FromDate";
                        DataSet StrsqlvalDataSet = dacess.select_method_wo_parameter(Strsql, "Text");
                        string code_value = string.Empty;
                        string cellnote = string.Empty;
                        string sectionval = string.Empty;
                        Boolean isaltflaf = false;
                        if ((Convert.ToString(Session["sec"])) != "" && (Convert.ToString(Session["sec"]) != "All"))
                        {
                            sectionval = Convert.ToString(Session["sec"]);
                        }
                        for (int alt = 1; alt <= 1; alt++)
                        {
                            string selnoofalter = Strsqlval + " degree_code , semester , batch_year,lastrec,sections,TTName,Fromdate,No_of_Alter from tbl_alter_schedule_Details where batch_year=" + batch + " and degree_code = " + degree + " and semester = " + sem + " and FromDate ='" + dateval + "'" + strsec + " and No_of_Alter='" + alt + "' order by FromDate";
                            DataSet dsnoofalter = dacess.select_method(selnoofalter, hat, "Text");
                            if (dsnoofalter.Tables[0].Rows.Count > 0)
                            {
                                string deltequery = "delete from tbl_alter_schedule_Details where batch_year=" + batch + " and degree_code = " + degree + " and semester = " + sem + " and FromDate ='" + dateval + "'" + strsec + " and No_of_Alter='" + alt + "'";
                                int del = dacess.update_method_wo_parameter(deltequery, "Text");
                            }
                            string getStaffCode = string.Empty;
                            int colCnt = 0;
                            string altersubdeta = string.Empty;
                            // int altercolu = alt + jj - 1;
                            int periodNo = 0;
                            for (j = 2; j < spreadDet.Sheets[0].ColumnCount; j++)//---------------loop for col value
                            {
                                periodNo++;
                                string substaffname = spreadDet.Sheets[0].Cells[spreadDetActRow, j].Text;
                                string substaffcode = spreadDet.Sheets[0].Cells[spreadDetActRow, j].Note;
                                string dayval = strDay + Convert.ToInt32(periodNo).ToString();
                                string getlabdetails = string.Empty;
                                if (substaffname != "" && substaffcode != "" && substaffname != "Sunday")
                                {
                                    if (spreadDet.Sheets[0].Cells[spreadDetActRow, j].Locked == false)
                                    {
                                        if (!hataltersc.Contains(dayval))
                                        {
                                            if (spreadDet.Sheets[0].ColumnHeader.Cells[0, j].Tag != null)
                                            {
                                                getlabdetails = "/" + spreadDet.Sheets[0].ColumnHeader.Cells[0, j].Tag.ToString();
                                            }
                                            hataltersc.Add(dayval, substaffcode + getlabdetails);
                                        }
                                        else
                                        {
                                            if (spreadDet.Sheets[0].ColumnHeader.Cells[0, j].Tag != null)
                                            {
                                                getlabdetails = "/" + spreadDet.Sheets[0].ColumnHeader.Cells[0, j].Tag.ToString();
                                            }
                                            hataltersc[dayval] = substaffcode + getlabdetails;
                                        }
                                        if (altersubdeta == "")
                                        {
                                            altersubdeta = "'" + substaffcode + "'";
                                        }
                                        else
                                        {
                                            altersubdeta = altersubdeta + ",'" + substaffcode + "'";
                                        }
                                        colCnt = (periodNo - 1) + 1;
                                        getStaffCode = substaffcode;
                                    }
                                    else
                                    {
                                        if (altersubdeta == "")
                                        {
                                            altersubdeta = "''";
                                        }
                                        else
                                        {
                                            altersubdeta = altersubdeta + ",''";
                                        }
                                    }
                                }
                                else
                                {
                                    if (altersubdeta == "")
                                    {
                                        altersubdeta = "''";
                                    }
                                    else
                                    {
                                        altersubdeta = altersubdeta + ",''";
                                    }
                                }
                            }

                            if (altersubdeta != "" && intNHrs > 0)
                            {
                                string insertnoofalter = "insert into tbl_alter_schedule_Details(degree_code,semester,batch_year,fromdate,lastrec,sections,No_of_Alter," + getday + ") values(" + degree + "," + sem + "," + batch + ",'" + dateval + "',0,'" + sectionval + "','" + alt + "'," + altersubdeta + ")";
                                int ins = dacess.update_method_wo_parameter(insertnoofalter, "Text");
                                if (ins > 0)
                                {
                                    if (Convert.ToString(Session["leavereqstatus"]) == "LeaveRequest")
                                    {
                                        Session["alter_done"] = "1";
                                        Session["tbl_alter_qry"] = insertnoofalter;
                                        Session["deg"] = degree;
                                        Session["semester"] = sem;
                                        Session["batch_year"] = batch;
                                        Session["fromdates"] = dateval;
                                        Session["sections"] = sectionval;
                                        Session["No_of_Alter"] = alt;
                                        Session["getday"] = altersubdeta;
                                        Session["getdays"] = getday;

                                    }
                                }
                                else
                                {
                                    Session["alter_done"] = "0";
                                    Session["tbl_alter_qry"] = string.Empty;
                                }
                            }
                        }
                        //---------------delete the record from the alternate schedule for insert the updated record
                        if (StrsqlvalDataSet.Tables.Count > 0 && StrsqlvalDataSet.Tables[0].Rows.Count > 0)
                        {
                            string delsql = string.Empty;
                            con7.Close();
                            con7.Open();
                            delsql = "delete from Alternate_schedule where batch_year=" + batch + " and degree_code = " + degree + " and semester = " + sem + " and FromDate ='" + dateval + "'" + strsec + string.Empty;
                            SqlCommand delcmd = new SqlCommand(delsql, con7);
                            SqlDataReader del_dr;
                            del_dr = delcmd.ExecuteReader();
                            del_dr.Read();
                        }
                        string rsec = string.Empty;
                        string getrsec = string.Empty;
                        if (sectionval != "")
                        {
                            rsec = Convert.ToString(Session["sec"]);
                            getrsec = "and sections='" + rsec + "'";
                        }
                        int prdNo = 0;
                        for (j = 2; j < (intNHrs + 2); j++)//---------------loop for col value
                        {
                            prdNo++;
                            VarSch = string.Empty;
                            cellnote = string.Empty;
                            string daygetval = strDay + Convert.ToInt32(prdNo).ToString();
                            if (hataltersc.Contains(daygetval))
                            {
                                VarSch = GetCorrespondingKey(daygetval, hataltersc).ToString();
                                cellnote = GetCorrespondingKey(daygetval, hataltersc).ToString();
                            }
                            if (VarSch != "" && cellnote != "")
                            {
                                string setcode = string.Empty;
                                try
                                {
                                    string[] spitlabhour = cellnote.Split('/');
                                    setcode = spitlabhour[0].ToString();
                                    string[] spitsubject = setcode.Split(';');
                                    for (int subalter = 0; subalter <= spitsubject.GetUpperBound(0); subalter++)
                                    {
                                        string[] splitcode = spitsubject[subalter].Split('-');
                                        if (spitlabhour.GetUpperBound(0) > 0)
                                        {
                                            aeperflag = true;
                                            string getdayhour = spitlabhour[1].ToString();
                                            string[] spitgetdayhour = getdayhour.Split(',');
                                            string strquery = string.Empty;
                                            int insert = 0;
                                            string dayvalue = spitgetdayhour[0].ToString();
                                            int hourvalue = int.Parse(spitgetdayhour[1]) + 1;
                                            if (subalter == 0)
                                            {
                                                strquery = "delete from subjectChooser_New where subject_no='" + splitcode[0].ToString() + "' and semester='" + sem + "' and fromdate='" + dateval + "' and roll_no in( select roll_no from Registration where  batch_year='" + batch + "' and degree_code='" + degree + "' and semester='" + sem + "' " + getrsec + " and cc=0 and delflag=0 and exam_flag<>'debar' )";
                                                insert = dacess.update_method_wo_parameter(strquery, "Text");
                                                strquery = "delete from laballoc_new where  batch_year='" + batch + "' and degree_code='" + degree + "' and semester='" + sem + "' " + getrsec + " and day_value='" + strDay + "' and hour_value='" + hourvalue + "' and fdate='" + dateval + "'";
                                                insert = dacess.update_method_wo_parameter(strquery, "Text");
                                            }
                                            strquery = "select distinct s.subtype_no,s.Batch,r.roll_no from Registration r ,subjectChooser s where r.Roll_No=s.roll_no and r.Current_Semester=s.semester and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'Debar' and s.subject_no='" + splitcode[0].ToString() + "' and batch_year='" + batch + "' and r.degree_code='" + degree + "' and s.semester='" + sem + "' " + getrsec + string.Empty;
                                            dssetbatch = dacess.select_method_wo_parameter(strquery, "Text");
                                            if (dssetbatch.Tables[0].Rows.Count > 0)
                                            {
                                                strquery = "insert into subjectChooser_New (semester,roll_no,subject_no,subtype_no,Batch,fromdate,todate) ";
                                                strquery = strquery + "(select s.semester,s.roll_no,s.subject_no,s.subtype_no,s.Batch,'" + dateval + "' as fromdate ,'" + dateval + "' as todate from Registration r ,subjectChooser s where r.Roll_No=s.roll_no and r.Current_Semester=s.semester and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'Debar' and s.subject_no='" + splitcode[0].ToString() + "' and batch_year='" + batch + "' and r.degree_code='" + degree + "' and s.semester='" + sem + "' " + getrsec + ")";
                                                insert = dacess.update_method_wo_parameter(strquery, "Text");
                                            }
                                            strquery = "select distinct Stu_Batch,Day_Value,Hour_Value from laballoc where batch_year='" + batch + "' and degree_code='" + degree + "' and semester='" + sem + "' " + getrsec + " and day_value='" + dayvalue + "' and hour_value='" + hourvalue + "' and subject_no='" + splitcode[0].ToString() + "' " + ttname + string.Empty;
                                            dssetbatch = dacess.select_method_wo_parameter(strquery, "Text");
                                            for (int b = 0; b < dssetbatch.Tables[0].Rows.Count; b++)
                                            {
                                                string subtype = dssetbatch.Tables[0].Rows[b]["Stu_Batch"].ToString();
                                                string day = dssetbatch.Tables[0].Rows[b]["Day_Value"].ToString();
                                                string hour = dssetbatch.Tables[0].Rows[b]["Day_Value"].ToString();
                                                strquery = "insert into laballoc_new (Batch_Year,Degree_Code,Semester,Sections,Subject_No,Stu_Batch,Day_Value,Hour_Value,fdate,tdate) ";
                                                strquery = strquery + "values('" + batch + "','" + degree + "','" + sem + "','" + rsec + "','" + splitcode[0].ToString() + "','" + subtype + "','" + strDay + "','" + hourvalue + "','" + dateval + "','" + dateval + "')";
                                                insert = dacess.update_method_wo_parameter(strquery, "Text");
                                            }
                                        }
                                        else
                                        {
                                            int hours = j + 1;
                                            string strquery = "delete from subjectChooser_New where subject_no='" + splitcode[0].ToString() + "' and semester='" + sem + "' and fromdate='" + dateval + "' and roll_no in( select roll_no from Registration where  batch_year='" + batch + "' and degree_code='" + degree + "' and semester='" + sem + "' " + getrsec + " and cc=0 and delflag=0 and exam_flag<>'debar' )";
                                            int insert = dacess.update_method_wo_parameter(strquery, "Text");
                                            strquery = "delete from laballoc_new where  batch_year='" + batch + "' and degree_code='" + degree + "' and semester='" + sem + "' " + getrsec + " and day_value='" + strDay + "' and hour_value='" + hours + "' and fdate='" + dateval + "'";
                                            insert = dacess.update_method_wo_parameter(strquery, "Text");
                                            strquery = "select distinct s.subtype_no,s.Batch,r.roll_no from Registration r ,subjectChooser s where r.Roll_No=s.roll_no and r.Current_Semester=s.semester and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'Debar' and s.subject_no='" + splitcode[0].ToString() + "' and batch_year='" + batch + "' and r.degree_code='" + degree + "' and s.semester='" + sem + "' " + getrsec + string.Empty;
                                            dssetbatch = dacess.select_method_wo_parameter(strquery, "Text");
                                            if (dssetbatch.Tables[0].Rows.Count > 0)
                                            {
                                                strquery = "insert into subjectChooser_New (semester,roll_no,subject_no,subtype_no,Batch,fromdate,todate) ";
                                                strquery = strquery + "(select s.semester,s.roll_no,s.subject_no,s.subtype_no,s.Batch,'" + dateval + "' as fromdate ,'" + dateval + "' as todate from Registration r ,subjectChooser s where r.Roll_No=s.roll_no and r.Current_Semester=s.semester and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'Debar' and s.subject_no='" + splitcode[0].ToString() + "' and batch_year='" + batch + "' and r.degree_code='" + degree + "' and s.semester='" + sem + "' " + getrsec + ")";
                                                insert = dacess.update_method_wo_parameter(strquery, "Text");
                                                aeperflag = true;
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                                if (code_value == "")
                                {
                                    code_value = "'" + setcode + "'";
                                    isaltflaf = true;
                                }
                                else
                                {
                                    code_value = code_value + ",'" + setcode + "'";
                                    isaltflaf = true;
                                }
                                if (smssend.Trim() != "" && smssend != null && smssend.Trim() != "0")
                                {
                                    //sendsms(daygetval, ddlbatch.SelectedValue.ToString(), ddlbranch.SelectedValue.ToString(), ddlduration.SelectedValue.ToString(), sectionval, dateval, setcode);
                                }
                            }
                            else
                            {
                                if (code_value == "")
                                {
                                    code_value = "''";
                                }
                                else
                                {
                                    code_value = code_value + ",''";
                                }
                            }
                        }
                        if (aeperflag == false)
                        {
                            string strquery = "delete from subjectChooser_New where semester='" + sem + "' and fromdate='" + dateval + "'  and roll_no in(Select roll_no from registration where batch_year='" + batch + "' and degree_code='" + degree + "' and semester='" + sem + "' " + getrsec + " and cc=0 and delflag=0 and exam_flag<>'debar'  )";
                            int insert = dacess.update_method_wo_parameter(strquery, "Text");
                        }
                        //---------------save the record into altenate schedule
                        if (code_value != "" && isaltflaf == true)
                        {
                            strinsert = "insert into Alternate_schedule(degree_code,semester,batch_year,fromdate,lastrec,sections," + getday + ") values(" + degree + "," + sem + "," + batch + ",'" + dateval + "',0,'" + sectionval + "'," + code_value + ")";
                            con1a.Close();
                            con1a.Open();
                            SqlCommand savecmd = new SqlCommand(strinsert, con1a);
                            SqlDataReader save_dr;
                            save_dr = savecmd.ExecuteReader();
                            save_dr.Read();
                            btnsave.Enabled = false;
                            while (save_dr.Read())
                            {
                                if (Convert.ToString(Session["leavereqstatus"]) == "LeaveRequest")
                                {
                                    Session["alter_done"] = "1";
                                    Session["alternate_schedule_qry"] = strinsert;
                                    Session["code_val"] = code_value;
                                }
                            }

                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
                        }
                    }
                }
                // batchbtn.Visible = true;
            }
        }
        catch
        {
        }
    }

    protected void btnOKsave_Clik(object sender, EventArgs e)
    {
        Div3.Visible = false;
        Save();
    }

    protected void bt_closedalter_Clik(object sender, EventArgs e)
    {
        btnGo_Click(sender, e);
        Div3.Visible = false;
    }

    protected void free_staff()
    {
        try
        {
            freestaff.Sheets[0].ColumnCount = 0;
            freestaff.Sheets[0].RowCount = 0;
            string date1;
            string date2;
            string datefrom;
            string dateto;
            string strDay = string.Empty;
            string detail_no = string.Empty;
            string staff_code = string.Empty;
            string subj_no = string.Empty;
            string sub_staff = string.Empty;
            string asql = string.Empty;
            string Staff_Code = string.Empty;
            string sqlstr;
            int noofhrs;
            string date_change;
            Boolean isstafffree = false;
            int SchOrder = 0, nodays = 0;
            int intNHrs = 0;
            //---------------------------------------------               
            freestaff.Sheets[0].SheetCorner.Cells[0, 0].Text = "Date";
            if (txtDate.Text.ToString() != "" && txtDate.Text.ToString() != "\0")
            {

                date1 = txtDate.Text.ToString();
                string[] split = date1.Split(new Char[] { '/' });
                datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                date2 = txtDate.Text.ToString();
                string[] split1 = date2.Split(new Char[] { '/' });
                dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
                DateTime dt2 = Convert.ToDateTime(dateto.ToString());
                TimeSpan t = dt2.Subtract(dt1);
                long days = t.Days;
                string[] differ_days = new string[days];
                sqlstr = "select No_of_hrs_per_day from PeriodAttndSchedule where degree_code='" + Convert.ToString(Session["degcode"]) + "' and semester=" + Convert.ToString(Session["sem"]) + " ";
                noofhrs = Convert.ToInt32(GetFunction(sqlstr));
                if (days >= 0)
                {
                    if (noofhrs != 0)
                    {
                        for (int i = 1; i <= noofhrs; i++)
                        {
                            freestaff.Sheets[0].ColumnCount = freestaff.Sheets[0].ColumnCount + 1;
                            freestaff.Sheets[0].ColumnHeader.Cells[0, freestaff.Sheets[0].ColumnCount - 1].Text = "Period " + Convert.ToString(i);
                            freestaff.Sheets[0].Columns[freestaff.Sheets[0].ColumnCount - 1].Width = 100;
                            freestaff.Sheets[0].Columns[freestaff.Sheets[0].ColumnCount - 1].Locked = true;
                            freestaff.Sheets[0].Columns[i - 1].Font.Name = "Book Antiqua";
                            freestaff.Sheets[0].Columns[i - 1].Font.Size = FontUnit.Medium;
                        }
                        string[] split_1 = date1.Split(new Char[] { '/' });
                        for (int k = 0; k <= days; k++)
                        {
                            DateTime split_plus = dt1.AddDays(k);
                            string split_str = string.Empty;
                            split_str = split_plus.ToString();
                            string[] split_1_str = split_str.Split(' ');
                            string[] split_dt = split_1_str[0].Split('/');

                            date_change = split_dt[1].ToString() + "/" + split_dt[0].ToString() + "/" + split_dt[2].ToString();//Modified by Manikandan 15/08/2013 from above Line
                            freestaff.Sheets[0].RowCount = freestaff.Sheets[0].RowCount + 1;
                            freestaff.Sheets[0].RowHeader.Cells[freestaff.Sheets[0].RowCount - 1, 0].Text = date_change;
                            con1a.Close();
                            con1a.Open();
                            SqlCommand cmd_holi = new SqlCommand("select holiday_desc from holidaystudents where holiday_date='" + split_1_str[0].ToString() + "' and degree_code=" + Convert.ToString(Session["degcode"]) + " and semester=" + Convert.ToString(Session["sem"]) + "", con1a);
                            string str_holiday = (string)cmd_holi.ExecuteScalar();
                            con1a.Close();
                            con1a.Open();
                            SqlCommand cmd1a;
                            SqlDataReader staff_list;
                            cmd1a = new SqlCommand("select distinct st.subject_no,s.subject_name,st.staff_code from subject s,staff_selector st,syllabus_master sy where s.syll_code= sy.syll_code  and sy.degree_code=" + Convert.ToString(Session["degcode"]) + " and semester=" + Convert.ToString(Session["sem"]) + " and sy.batch_year=" + Convert.ToString(Session["batch"]) + " and st.batch_year=" + Convert.ToString(Session["batch"]) + " and st.sections='" + Convert.ToString(Session["sec"]) + "' and s.subject_no=st.subject_no order by st.subject_no,s.subject_name,st.staff_code ", con1a);
                            staff_list = cmd1a.ExecuteReader();
                            while (staff_list.Read())
                            {
                                isstafffree = false;
                                if (staff_list.HasRows == true)
                                {
                                    Staff_Code = staff_list[2].ToString();
                                    Session["Staff_Code_Temp"] = Staff_Code.ToString();
                                    if (noofhrs > 0)
                                    {
                                        string sql_s = string.Empty;
                                        string sql1 = string.Empty;
                                        string day_change;
                                        string SqlBatchYear = string.Empty;
                                        string SqlPrefinal1 = string.Empty;
                                        string SqlPrefinal2 = string.Empty;
                                        string SqlPrefinal3 = string.Empty;
                                        string SqlPrefinal4 = string.Empty;
                                        string SqlFinal = string.Empty;
                                        string SqlBatchYear1 = string.Empty;
                                        string SqlPrefinal11 = string.Empty;
                                        string SqlPrefinal22 = string.Empty;
                                        string SqlPrefinal33 = string.Empty;
                                        string SqlPrefinal44 = string.Empty;
                                        string SqlFinal1 = string.Empty;
                                        string Schedule_string = string.Empty;
                                        string staff_name = string.Empty;
                                        string[] split_1a = date1.Split(new Char[] { '/' });

                                        string Strsql = string.Empty;

                                        strDay = split_plus.ToString("ddd");

                                        DateTime startdate = Convert.ToDateTime(GetFunction("select start_date from seminfo where degree_code='" + Convert.ToString(Session["degcode"]) + "' and semester='" + Convert.ToString(Session["sem"]) + "' and batch_year='" + Convert.ToString(Session["batch"]) + "' "));
                                        if (startdate.ToString() != "" && startdate.ToString() != null)
                                        {
                                            strDay = startdate.ToString("ddd");
                                        }
                                        con.Close();
                                        con.Open();
                                        SqlDataReader dr;
                                        cmd = new SqlCommand("Select No_of_hrs_per_day,schorder,nodays from periodattndschedule where degree_code='" + Convert.ToString(Session["degcode"]) + "' and semester ='" + Convert.ToString(Session["sem"]) + "'", con);
                                        dr = cmd.ExecuteReader();
                                        dr.Read();
                                        if (dr.HasRows == true)
                                        {
                                            if ((dr["No_of_hrs_per_day"].ToString()) != "")
                                            {
                                                intNHrs = Convert.ToInt32(dr["No_of_hrs_per_day"]);
                                                SchOrder = Convert.ToInt32(dr["schorder"]);
                                                nodays = Convert.ToInt32(dr["nodays"]);
                                            }
                                        }
                                        if (intNHrs > 0)
                                        {
                                            if (SchOrder != 0)
                                            {
                                                strDay = (Convert.ToDateTime(split_1_str[0].ToString())).ToString("ddd");
                                            }
                                            else
                                            {

                                                string[] sps = date_change.ToString().Split('/');
                                                string curdate = sps[1] + '/' + sps[0] + '/' + sps[2];
                                                strDay = dacess.findday(curdate.ToString(), Convert.ToString(Session["degcode"]), Convert.ToString(Session["sem"]), Convert.ToString(Session["batch"]), startdate.ToString(), nodays.ToString(), start_dayorder.ToString());
                                            }
                                        }

                                        con2a.Close();
                                        con2a.Open();
                                        sql_s = "select semester_schedule.degree_code,(select No_of_hrs_per_day from PeriodAttndSchedule where degree_code=semester_schedule.degree_code and semester=semester_schedule.semester), ";
                                        asql = "select Alternate_schedule.degree_code,(select No_of_hrs_per_day from PeriodAttndSchedule where degree_code=Alternate_schedule.degree_code and semester=Alternate_schedule.semester), ";
                                        SqlCommand cmdasql = new SqlCommand(asql, con4a);
                                        for (int i_loop = 1; i_loop <= noofhrs; i_loop++)
                                        {
                                            Strsql = Strsql + strDay + Convert.ToString(i_loop) + ",";
                                            if (sql1 == "")
                                            {
                                                sql1 = sql1 + strDay + Convert.ToString(i_loop) + " like '%" + (string)Session["Staff_Code_Temp"] + "%'";
                                            }
                                            else
                                            {
                                                sql1 = sql1 + " or " + strDay + Convert.ToString(i_loop) + " like '%" + (string)Session["Staff_Code_Temp"] + "%'";
                                            }
                                        }
                                        sql1 = "(" + sql1 + ")";
                                        sql_s = sql_s + Strsql + string.Empty;
                                        asql = asql + Strsql + string.Empty;
                                        string day_from;
                                        date1 = txtDate.Text.ToString();
                                        string[] split_su = date1.Split(new Char[] { '/' });
                                        day_from = split_su[1].ToString() + "/" + split_su[0].ToString() + "/" + split_su[2].ToString();
                                        DateTime date_from = Convert.ToDateTime(day_from.ToString());
                                        SqlBatchYear = "(select distinct(registration.batch_year) from registration,semester_schedule where registration.degree_code=semester_schedule.degree_code and registration.cc=0 and delflag=0 and registration.exam_flag<>'DEBAR' AND registration.current_Semester = semester_schedule.semester)";
                                        SqlPrefinal1 = sql_s + " semester,sections,batch_year from semester_schedule where lastrec=1 and batch_year in " + SqlBatchYear + " and " + sql1 + " and semester=1 and degree_code in (select degree_code from degree where Exam_System= 'Semester' and First_Year_Nonsemester<>0)";
                                        SqlPrefinal2 = sql_s + " semester,sections,batch_year from semester_schedule where lastrec=1 and batch_year in " + SqlBatchYear + " and " + sql1 + " and degree_code in (select degree_code from degree where Exam_System='Semester' and First_Year_Nonsemester=0)";
                                        SqlPrefinal3 = sql_s + " semester,sections,batch_year from semester_schedule where lastrec=1 and batch_year in " + SqlBatchYear + " and " + sql1 + " and degree_code in (select degree_code from degree where Exam_System='Non Semester') ";
                                        SqlPrefinal4 = sql_s + " semester,sections,batch_year from semester_schedule where lastrec=1 and batch_year in " + SqlBatchYear + " and " + sql1 + " and semester<>1 and semester<>-1  and degree_code in (select degree_code from degree where Exam_System= 'Semester' and First_Year_Nonsemester<>0)";
                                        SqlFinal = "(" + SqlPrefinal1 + ") union all (" + SqlPrefinal4 + ") union all (" + SqlPrefinal2 + ") union all (" + SqlPrefinal3 + ")";

                                        SqlBatchYear1 = "(select distinct(registration.batch_year) from registration,Alternate_schedule where registration.degree_code=Alternate_schedule.degree_code and registration.cc=0 and delflag=0 and registration.exam_flag<>'DEBAR' AND registration.current_Semester = Alternate_schedule.semester)";
                                        SqlPrefinal11 = asql + " semester,sections from Alternate_schedule where batch_year in " + SqlBatchYear1 + " and " + sql1 + " and semester=1 and degree_code in (select degree_code from degree where Exam_System= 'Semester' and First_Year_Nonsemester<>0)";
                                        SqlPrefinal22 = asql + " semester,sections from Alternate_schedule where  FromDate ='" + split_1_str[0] + "'  and batch_year in " + SqlBatchYear1 + " and " + sql1 + " and degree_code in (select degree_code from degree where Exam_System='Semester' and First_Year_Nonsemester=0)";
                                        SqlPrefinal33 = asql + " semester,sections from Alternate_schedule where  FromDate ='" + split_1_str[0] + "' and batch_year in " + SqlBatchYear1 + " and " + sql1 + " and degree_code in (select degree_code from degree where Exam_System='Non Semester') ";
                                        SqlPrefinal44 = asql + " semester,sections from Alternate_schedule where  FromDate ='" + split_1_str[0] + "'  and batch_year in " + SqlBatchYear1 + " and " + sql1 + " and semester<>1  and degree_code in (select degree_code from degree where Exam_System= 'Semester' and First_Year_Nonsemester<>0)";
                                        SqlFinal1 = "(" + SqlPrefinal11 + ") union all (" + SqlPrefinal44 + ") union all (" + SqlPrefinal22 + ") union all (" + SqlPrefinal33 + ")";
                                        con4a.Close();
                                        con4a.Open();
                                        SqlDataAdapter da_alternate = new SqlDataAdapter("select degree_code,semester," + Strsql + " sections from Alternate_schedule where fromdate='" + split_1_str[0] + "'", con4a);
                                        DataTable dt_alternate = new DataTable();
                                        da_alternate.Fill(dt_alternate);
                                        //Semester Schedule
                                        con4a.Close();
                                        con4a.Open();
                                        SqlCommand cmd_1 = new SqlCommand(SqlFinal, con4a);
                                        SqlDataAdapter da_1 = new SqlDataAdapter(cmd_1);
                                        DataTable dt_1 = new DataTable();
                                        da_1.Fill(dt_1);
                                        //Alternate Schedule
                                        SqlCommand cmd_2 = new SqlCommand(SqlFinal1, con4a);
                                        SqlDataAdapter da_2 = new SqlDataAdapter(cmd_2);
                                        DataTable dt_2 = new DataTable();
                                        da_2.Fill(dt_2);
                                        string staffavail = string.Empty;
                                        int rowcount = 0;
                                        string freestaffname = string.Empty;
                                        DateTime? curfromsem = null;
                                        DateTime? curtosem = null;
                                        for (int col_cnt = 1; col_cnt <= noofhrs; col_cnt++)
                                        {
                                            int a = 0;
                                            int b = 0;
                                            if (!string.IsNullOrEmpty(str_holiday))
                                            {
                                                freestaff.Sheets[0].Cells[k, col_cnt - 1].Text = str_holiday + " Holiday";
                                            }
                                            else
                                            {
                                                for (int row_cnt = 0; row_cnt < dt_1.Rows.Count; row_cnt++)
                                                {
                                                    con4a.Close();
                                                    con4a.Open();
                                                    string cmd_semdate = "select start_date,end_date from seminfo where degree_code=" + dt_1.Rows[row_cnt]["degree_code"].ToString() + " and semester=" + dt_1.Rows[row_cnt]["semester"].ToString() + " and batch_year=" + dt_1.Rows[row_cnt]["batch_year"].ToString() + string.Empty;
                                                    SqlDataAdapter da_semdate = new SqlDataAdapter(cmd_semdate, con4a);
                                                    DataTable dt_semdate = new DataTable();
                                                    da_semdate.Fill(dt_semdate);
                                                    if (dt_semdate.Rows.Count > 0)
                                                    {
                                                        curfromsem = Convert.ToDateTime(dt_semdate.Rows[0]["Start_date"].ToString());
                                                        curtosem = Convert.ToDateTime(dt_semdate.Rows[0]["end_date"].ToString());
                                                    }
                                                    if (Convert.ToDateTime(day_from) >= curfromsem && Convert.ToDateTime(day_from) <= curtosem)
                                                    {
                                                        string staffcode = dt_1.Rows[row_cnt][col_cnt + 1].ToString();
                                                        if (staffcode.Contains(Staff_Code) == true)
                                                        {
                                                            a++;
                                                        }
                                                        //Check alternate
                                                        for (int row_cnt_1 = 0; row_cnt_1 < dt_alternate.Rows.Count; row_cnt_1++)
                                                        {
                                                            staffcode = dt_alternate.Rows[row_cnt_1][col_cnt + 1].ToString();
                                                            if (!string.IsNullOrEmpty(staffcode))
                                                            {
                                                                b = 1;
                                                                a = 0;
                                                            }
                                                        }
                                                        //alternate end
                                                    }
                                                }

                                                for (int row_cnt_1 = 0; row_cnt_1 < dt_2.Rows.Count; row_cnt_1++)
                                                {
                                                    string staffcode = dt_2.Rows[row_cnt_1][col_cnt + 1].ToString();
                                                    if (staffcode.Contains(Staff_Code) == true)
                                                    {
                                                        a++;
                                                        b = 1;
                                                    }
                                                }

                                                //free staff name
                                                if (dt_2.Rows.Count == 0)
                                                {
                                                    b = 1;
                                                }
                                                if (a == 0 && b == 0)
                                                {
                                                    b = 1;
                                                }
                                                if (a == 0 && b == 1)
                                                {
                                                    freestaffname = freestaff.Sheets[0].Cells[rowcount, col_cnt - 1].Text;
                                                    rowcount = Convert.ToInt32(freestaff.Sheets[0].RowCount) - 1;
                                                    staff_name = GetFunction("select staff_name from staffmaster where staff_code='" + Staff_Code + "'");
                                                    if (freestaffname.Contains(staff_name) == false)
                                                    {
                                                        freestaff.Sheets[0].Cells[k, col_cnt - 1].Text += staff_name + ";";
                                                    }
                                                }//free staff name end
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }

        }
        catch
        {
        }
    }

    protected void btnBatchAllocation_Click(object sender, EventArgs e)
    {
        try
        {
            Session["Batch_ReDir"] = "FromNewAlternateSchedule";
            string redrURI = ConfigurationManager.AppSettings["SchBatch"].Trim();
             Response.Redirect(redrURI);
            //Response.Redirect("~/ScheduleMOD/Batchallocation.aspx");
        }
        catch { }
    }

    protected bool getAlternateScheduleCheck(string staffcode, string day, int col, string fromdate, ref string alertStr, string stfName)
    {
        bool checkbool = false;
        try
        {
            string[] staffcode_check = staffcode.Split('-');
            string staffname = dacess.GetFunction("select s.staff_name from staffmaster s,staff_appl_master sm where s.appl_no=sm.appl_no and s.staff_code='" + staffcode_check[1] + "'");
            string tablevalue = string.Empty;
            if (day == "Mon")
                tablevalue = "mon" + col + string.Empty;
            else if (day == "Tue")
                tablevalue = "tue" + col + string.Empty;
            else if (day == "Wed")
                tablevalue = "wed" + col + string.Empty;
            else if (day == "Thu")
                tablevalue = "thu" + col + string.Empty;
            else if (day == "Fri")
                tablevalue = "fri" + col + string.Empty;
            else if (day == "Sat")
                tablevalue = "sat" + col + string.Empty;
            string SqlFinal = string.Empty;
            string history_data = string.Empty;
            for (int i = 0; i <= staffcode_check.Length - 1; i++)
            {
                string staff_code = staffcode_check[i].ToString();
                Hashtable hatdegree = new Hashtable();
                SqlFinal = " select cc.Course_Name, de.Acronym, r.Batch_Year,r.degree_code,sy.semester,r.Sections,si.end_date from staff_selector ss,Registration r,";
                SqlFinal = SqlFinal + " subject s,sub_sem sm,syllabus_master sy,seminfo si,Degree de,COURSE cc where sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code";
                SqlFinal = SqlFinal + " and sy.semester=r.Current_Semester and sy.syll_code=sm.syll_code and sm.subType_no=s.subType_no ";
                SqlFinal = SqlFinal + " and s.subject_no=ss.subject_no and isnull(r.sections,'')=isnull(ss.sections,'') and ss.batch_year=r.Batch_Year";
                SqlFinal = SqlFinal + " and si.Batch_Year=r.Batch_Year and si.degree_code=r.degree_code and si.semester=r.Current_Semester and ";
                SqlFinal = SqlFinal + " si.Batch_Year=sy.Batch_Year and sy.degree_code=r.degree_code and si.semester=sy.Semester and r.CC=0 and r.Exam_Flag<>'debar'";
                SqlFinal = SqlFinal + " and r.DelFlag=0 and ss.staff_code='" + staff_code + "' and de.Degree_Code=si.degree_code and de.Course_Id=cc.Course_Id and '" + fromdate + "' between si.start_date and si.end_date";

                DataSet srids = dacess.select_method_wo_parameter(SqlFinal, "Text");
                for (int j = 0; j < srids.Tables[0].Rows.Count; j++)
                {
                    string btch = srids.Tables[0].Rows[j]["batch_year"].ToString();
                    string dgre = srids.Tables[0].Rows[j]["degree_code"].ToString();
                    string ster = srids.Tables[0].Rows[j]["semester"].ToString();
                    string sctn = srids.Tables[0].Rows[j]["Sections"].ToString();
                    string acrnym = srids.Tables[0].Rows[j]["Acronym"].ToString();
                    string coursename = srids.Tables[0].Rows[j]["Course_Name"].ToString();
                    if (!hatdegree.ContainsKey(btch + '-' + dgre + '-' + ster + '-' + sctn))
                    {
                        hatdegree.Add(btch + '-' + dgre + '-' + ster + '-' + sctn, btch + '-' + dgre + '-' + ster + '-' + sctn);
                        string slq = "select top 1 * from Semester_Schedule where batch_year='" + btch + "' and semester ='" + ster + "' and degree_code='" + dgre + "' and Sections='" + sctn + "' and FromDate <= '" + fromdate + "' ORDER BY FromDate desc";
                        string rept = string.Empty;

                        DataSet ds = dacess.select_method_wo_parameter(slq, "Text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            string strsetval = "" + tablevalue + " like '%" + staff_code + "%'";
                            ds.Tables[0].DefaultView.RowFilter = strsetval;
                            DataView dvfils = ds.Tables[0].DefaultView;
                            if (dvfils.Count > 0)
                            {
                                if (history_data == "")
                                {
                                    if (ster == "1")
                                    {
                                        history_data = btch + "-" + coursename + "-" + acrnym + "-" + ster + "st Sem";
                                        if (sctn != null && sctn != "")
                                        {
                                            history_data = history_data + "-" + sctn + "  Sec";
                                        }
                                    }
                                    if (ster == "2")
                                    {
                                        history_data = btch + "-" + coursename + "-" + acrnym + "-" + ster + "nd Sem";
                                        if (sctn != null && sctn != "")
                                        {
                                            history_data = history_data + "-" + sctn + "  Sec";
                                        }
                                    }
                                    if (ster == "3")
                                    {
                                        history_data = btch + "-" + coursename + "-" + acrnym + "-" + ster + "rd Sem";
                                        if (sctn != null && sctn != "")
                                        {
                                            history_data = history_data + "-" + sctn + "  Sec";
                                        }
                                    }
                                    else
                                    {
                                        history_data = btch + "-" + coursename + "-" + acrnym + "-" + ster + "th Sem";
                                        if (sctn != null && sctn != "")
                                        {
                                            history_data = history_data + "-" + sctn + "  Sec";
                                        }
                                    }
                                }
                                else
                                {
                                    if (ster == "1")
                                    {
                                        history_data = history_data + " ; " + btch + "-" + coursename + "-" + acrnym + "-" + ster + "st Sem";
                                        if (sctn != null && sctn != "")
                                        {
                                            history_data = history_data + "-" + sctn + "  Sec";
                                        }
                                    }
                                    else if (ster == "2")
                                    {
                                        history_data = history_data + " ; " + btch + "-" + coursename + "-" + acrnym + "-" + ster + "nd Sem";
                                        if (sctn != null && sctn != "")
                                        {
                                            history_data = history_data + "-" + sctn + "-Sec";
                                        }
                                    }
                                    else if (ster == "3")
                                    {
                                        history_data = history_data + " ; " + btch + "-" + coursename + "-" + acrnym + "-" + ster + "rd Sem";
                                        if (sctn != null && sctn != "")
                                        {
                                            history_data = history_data + "-" + sctn + "  Sec";
                                        }
                                    }
                                    else
                                    {
                                        history_data = history_data + " ; " + btch + "-" + coursename + "-" + acrnym + "-" + ster + "th Sem";
                                        if (sctn != null && sctn != "")
                                        {
                                            history_data = "-" + history_data + "-" + sctn + "  Sec";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (history_data != "")
            {
                string getRights = dacess.GetFunction("select value from Master_Settings where  settings='Time Table Alert Rights'");
                if (getRights.Trim() == "0" || String.IsNullOrEmpty(getRights))
                {
                    checkbool = true;
                    alertStr = "The Staff " + staffname + " is BUSY in " + history_data + " - Do you want to Schedule the Class Anyway?";
                }
            }
        }
        catch { }
        return checkbool;
    }

    private string GetSyllabusYear(string degree_code, string batch_year, string sem)
    {
        try
        {
            string syl_year = string.Empty;
            con2a.Close();
            con2a.Open();
            SqlCommand cmd2a;
            SqlDataReader get_syl_year;
            cmd2a = new SqlCommand("select syllabus_year from syllabus_master where degree_code=" + degree_code + " and semester =" + sem + " and batch_year=" + batch_year + " ", con2a);
            get_syl_year = cmd2a.ExecuteReader();
            get_syl_year.Read();
            if (get_syl_year.HasRows == true)
            {
                if (get_syl_year[0].ToString() == "\0")
                {
                    syl_year = "-1";
                }
                else
                {
                    syl_year = get_syl_year[0].ToString();
                }
            }
            else
            {
                syl_year = "-1";
            }
            return syl_year;
            con2a.Close();
        }
        catch
        {
            return string.Empty;
        }
    }

    public string GetFunction(string Att_strqueryst)
    {
        try
        {
            string sqlstr;
            sqlstr = Att_strqueryst;
            getcon.Close();
            getcon.Open();
            SqlDataReader drnew;
            SqlCommand cmd = new SqlCommand(sqlstr, getcon);
            drnew = cmd.ExecuteReader();
            drnew.Read();
            if (drnew.HasRows == true)
            {
                return drnew[0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        catch
        {
            return string.Empty;
        }
    }

    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {
        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Key.ToString() == key.ToString())
            {
                return e.Value;
            }
        }
        return null;
    }

    private void InitSpread(Farpoint.FpSpread FpSpread1)
    {
        try
        {
            #region FpSpread Style

            FpSpread1.Visible = true;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;

            #endregion FpSpread Style

            #region SpreadStyles

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.Font.Bold = true;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.VerticalAlign = VerticalAlign.Middle;
            darkstyle.ForeColor = Color.Black;
            darkstyle.Border.BorderSize = 1;
            darkstyle.Border.BorderColor = Color.Black;
            FarPoint.Web.Spread.StyleInfo sheetstyle = new FarPoint.Web.Spread.StyleInfo();
            sheetstyle.Font.Name = "Book Antiqua";
            sheetstyle.Font.Size = FontUnit.Medium;
            sheetstyle.Font.Bold = true;
            sheetstyle.HorizontalAlign = HorizontalAlign.Center;
            sheetstyle.VerticalAlign = VerticalAlign.Middle;
            sheetstyle.ForeColor = Color.Black;
            sheetstyle.Border.BorderSize = 1;
            sheetstyle.Border.BorderColor = Color.Black;

            #endregion SpreadStyles

            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.Sheets[0].DefaultStyle = sheetstyle;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
            FpSpread1.HorizontalScrollBarPolicy = Farpoint.ScrollBarPolicy.AsNeeded;
            FpSpread1.VerticalScrollBarPolicy = Farpoint.ScrollBarPolicy.AsNeeded;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.RowHeader.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = false;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.ActiveSheetView.SelectionBackColor = Color.Transparent;

            FpSpread1.Sheets[0].ColumnCount = 6;
            FpSpread1.Sheets[0].FrozenRowCount = 1;


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Code";
            FpSpread1.Sheets[0].Columns[2].Width = 100;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff Name";
            FpSpread1.Sheets[0].Columns[3].Width = 200;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Choose Free Staff";
            FpSpread1.Sheets[0].Columns[4].Width = 200;

            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Alternate Staff";
            //FpSpread1.Sheets[0].Columns[5].Width = 200;

            FpSpread1.Sheets[0].Columns[0].Width = 40;
            FpSpread1.Sheets[0].Columns[0].Locked = true;
            FpSpread1.Sheets[0].Columns[0].Resizable = false;
            FpSpread1.Sheets[0].Columns[0].Visible = true;
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);

            FpSpread1.Sheets[0].Columns[1].Width = 65;
            FpSpread1.Sheets[0].Columns[1].Locked = false;
            FpSpread1.Sheets[0].Columns[1].Resizable = false;
            FpSpread1.Sheets[0].Columns[1].Visible = true;
            FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);

            FpSpread1.Sheets[0].Columns[2].Locked = true;
            FpSpread1.Sheets[0].Columns[2].Resizable = false;
            FpSpread1.Sheets[0].Columns[2].Visible = true;
            FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);

            FpSpread1.Sheets[0].Columns[3].Locked = true;
            FpSpread1.Sheets[0].Columns[3].Resizable = false;
            FpSpread1.Sheets[0].Columns[3].Visible = true;
            FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);


            FpSpread1.Sheets[0].Columns[4].Locked = false;
            FpSpread1.Sheets[0].Columns[4].Resizable = false;
            FpSpread1.Sheets[0].Columns[4].Visible = false;
            FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);

            //FpSpread1.Sheets[0].Columns[5].Locked = false;
            //FpSpread1.Sheets[0].Columns[5].Resizable = false;
            //FpSpread1.Sheets[0].Columns[5].Visible = false;
            //FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            //FpSpread1.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
            //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);

            FarPoint.Web.Spread.CheckBoxCellType chkOneByOne = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.CheckBoxCellType chkSelectAll = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.ButtonCellType btnclType = new Farpoint.ButtonCellType();
            FarPoint.Web.Spread.ListBoxCellType lstbx = new Farpoint.ListBoxCellType();

            chkSelectAll.AutoPostBack = true;
            FpSpread1.Visible = true;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].RowCount++;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chkSelectAll;
            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 2, 1, 3);

        }
        catch
        {
        }
    }

    protected void chkSelectAlterStaff_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            //divAlterFreeStaffDetails.Visible = false;
            //lblAlterFreeStaffError.Text = string.Empty;
            //lblAlterFreeStaffError.Visible = false;
            //txtAlterFreeStaffSearch.Text = string.Empty;
            //chkSelectAlterStaff_CheckedChanged(sender, e);
            fpSpreadTreeNode.SaveChanges();
            if (chkForAlternateStaff.Checked)
            {
                try
                {
                    bool isHasAlternateStaff1 = false;
                    if (chkForAlternateStaff.Checked)
                        isHasAlternateStaff1 = true;
                    FpAlterFreeStaffList.SaveChanges();
                    fpSpreadTreeNode.SaveChanges();

                    int activeROw = fpSpreadTreeNode.Sheets[0].ActiveRow;
                    int j = fpSpreadTreeNode.Sheets[0].ActiveColumn;
                    Dictionary<string, string> dicselectedstaff = new Dictionary<string, string>();
                    //ddlAlternamteStaff.Items.Clear();
                    //if (ddlAlternamteStaff.Items.Count == 0)
                    //    ddlAlternamteStaff.Items.Insert(0, new ListItem("--Select--", ""));

                    int parent_count = subjtree.Nodes.Count;//----------count parent node value
                    for (int i = 0; i < parent_count; i++)
                    {
                        for (int node_count = 0; node_count < subjtree.Nodes[i].ChildNodes.Count; node_count++)//-------count child node
                        {
                            if (subjtree.Nodes[i].ChildNodes[node_count].Selected == true)//-------check checked condition
                            {
                                string subno = subjtree.Nodes[i].ChildNodes[node_count].Value;
                                string localsatff = "select distinct (sm.staff_name+'-'+ss.staff_code) as satff,ss.staff_code from staff_selector ss,staffmaster sm where  subject_no='" + subno + "' and sm.staff_code=ss.staff_code ";
                                DataTable dtlocalstaff = dirAcc.selectDataTable(localsatff);
                                foreach (DataRow dt in dtlocalstaff.Rows)
                                {
                                    string satff = Convert.ToString(dt["satff"]);
                                    string stCode = Convert.ToString(dt["staff_code"]);
                                    if (!dicselectedstaff.ContainsKey(satff))
                                        dicselectedstaff.Add(satff, stCode);

                                }
                            }
                        }
                    }

                    for (int rowC = 0; rowC < fpSpreadTreeNode.Rows.Count; rowC++)
                    {
                        string[] staffarr = new string[dicselectedstaff.Count];
                        string[] staffcodearr = new string[dicselectedstaff.Count];
                        int arrval = 0;
                        foreach (KeyValuePair<string, string> item in dicselectedstaff)
                        {
                            staffarr[arrval] = item.Key;
                            staffcodearr[arrval] = item.Value;
                            arrval++;
                        }
                        FarPoint.Web.Spread.ComboBoxCellType staf_combo = new FarPoint.Web.Spread.ComboBoxCellType(staffarr);
                        fpSpreadTreeNode.Sheets[0].Cells[rowC, 3].CellType = staf_combo;
                        fpSpreadTreeNode.Sheets[0].Cells[rowC, 3].Tag = staffcodearr;
                        fpSpreadTreeNode.Sheets[0].Cells[rowC, 3].Value = staffarr;
                        fpSpreadTreeNode.Sheets[0].Columns[3].Visible = isHasAlternateStaff1;
                    }
                    fpSpreadTreeNode.SaveChanges();
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

    #region Common Checkbox and Checkboxlist Event

    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedvalue = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        selectedvalue.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedvalue.ToString();
    }
    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        selectedText.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = "";
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
            string name = "";
            cb.Checked = false;
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

    #endregion
}