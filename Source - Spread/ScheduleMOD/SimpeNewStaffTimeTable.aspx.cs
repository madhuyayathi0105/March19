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
using System.Text;

public partial class ScheduleMOD_SimpeNewStaffTimeTable : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    InsproDirectAccess dir = new InsproDirectAccess();
    ReuasableMethods rs = new ReuasableMethods();
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
    static int curRow = 0;
    static int curCol = 1;
    static int cbVal1 = 0;
    bool isused = false;
    double tothr = 0;
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
                selectedDept = Convert.ToString(ddlDept.SelectedValue);
                collegecode = ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedItem.Value) : Convert.ToString(Session["collegecode"]);
                btnPrint.Visible = false;
                bindEdulevel();
                //btndelete.Visible = false;
                string date = DateTime.Now.ToString("dd/MM/yyyy");
                string[] dsplit1 = date.Split(new Char[] { '/' });
                txtFromDate.Text = dsplit1[0].ToString().PadLeft(2, '0') + "/" + dsplit1[1].ToString().PadLeft(2, '0') + "/" + dsplit1[2].ToString();
                if (Session["StaffCode"] != "")
                    btnGo_OnClick(sender, e);
                this.BindGrid();
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
            ddlsem.Items.Insert(0, "ODD");
            ddlsem.Items.Insert(1, "EVEN");
        }
        catch
        {

        }
    }

    private void bindSubject()
    {
        try
        {
            // string sql = "select max(No_of_hrs_per_day)HoursPerDay,MAX(nodays)NoOfDays from PeriodAttndSchedule where semester in(" + sem + ")";

            string collegeCode = Convert.ToString(ddlcollege.SelectedValue);
            string sem = Convert.ToString(ddlsem.SelectedValue);
            string staffCode = Convert.ToString(ddlSearchOption.SelectedValue);
            string degCode = string.Empty;
            string batchYear = string.Empty;
            string sec = string.Empty;
            if (sem.ToLower() == "odd")
            {
                sem = "1,3,5,7";
            }
            else if (sem.ToLower() == "even")
            {
                sem = "2,4,6,8";
            }
            if (cblBranch.Items.Count > 0)
            {
                for (int cb = 0; cb < cblBranch.Items.Count; cb++)
                {
                    if (cblBranch.Items[cb].Selected)
                    {
                        string deg = Convert.ToString(cblBranch.Items[cb].Value);
                        string[] val = deg.Split('-');

                        if (val.Length > 1)
                        {
                            if (string.IsNullOrEmpty(degCode))
                                degCode = Convert.ToString(val[1]);
                            else
                                degCode = degCode + "," + Convert.ToString(val[1]);
                            if (string.IsNullOrEmpty(batchYear))
                                batchYear = Convert.ToString(val[0]);
                            else
                                batchYear = batchYear + "," + Convert.ToString(val[0]);
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(staffCode))
            {
                string SelectSubject = "select distinct s.subject_code,(s.subject_code+'-'+s.subject_name) as subjectval  from Registration r,subject s,syllabus_master sm,Department de,course c,Degree d where  c.Course_Id=d.Course_Id and r.degree_code=d.Degree_Code and de.Dept_Code=d.Dept_Code and r.Batch_Year=sm.Batch_Year and r.degree_code=sm.degree_code and r.Current_Semester=sm.semester and s.syll_code=sm.syll_code and r.Current_Semester in(" + sem + ") and r.Batch_Year in(" + batchYear + ") and r.degree_code in(" + degCode + ")";
                DataTable dtSubject = dir.selectDataTable(SelectSubject);
                if (dtSubject.Rows.Count > 0)
                {
                    for (int hr = 1; hr <= 10; hr++)
                    {
                        CheckBoxList cbl = new CheckBoxList();
                        cbl.ID = "cblPeriod" + hr;
                        TextBox txt = new TextBox();
                        txt.ID = "txtPeriod" + hr;
                        CheckBox chk = new CheckBox();
                        chk.ID = "chkPeriod" + hr;
                        cbl.DataSource = dtSubject;
                        cbl.DataTextField = "subjectval";
                        cbl.DataValueField = "subject_code";
                        cbl.DataBind();
                        checkBoxListselectOrDeselect(cbl, true);
                        CallCheckboxListChange(chk, cbl, txt, "Branch", "--Select--");
                    }
                }

            }
        }
        catch
        {

        }
    }

    private void bindbatchInfo()
    {

        cblBranch.Items.Clear();
        string collegeCode = Convert.ToString(ddlcollege.SelectedValue);
        string sem = Convert.ToString(ddlsem.SelectedItem.Text);
        string staffCode = Convert.ToString(ddlSearchOption.SelectedValue);
        //string subNo = Convert.ToString(ddlSubject.SelectedValue);
        if (!string.IsNullOrEmpty(sem))
        {
            if (sem.ToLower() == "odd")
            {
                sem = "1,3,5,7";
            }
            else if (sem.ToLower() == "even")
            {
                sem = "2,4,6,8";
            }

            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(sem))
            {

                string Selectbatch = "select distinct convert(nvarchar(max),(Convert(nvarchar,(r.Batch_Year))+'-'+Convert(nvarchar,(r.degree_code))+'-'+Convert(nvarchar,(r.Current_Semester))+'-'+LTRIM(RTRIM(ISNULL(r.Sections,''))))) as val,(ci.coll_acronymn+'/'+Convert(nvarchar,(r.Batch_Year))+'/'+convert(nvarchar,(c.Course_Name+'/'+ de.dept_acronym+'/'+Convert(nvarchar,(r.Current_Semester))+'/'+LTRIM(RTRIM(ISNULL(r.Sections,''))))))as ccc    from Registration r,subject s,syllabus_master sm,Department de,course c,Degree d,collinfo ci where  c.Course_Id=d.Course_Id and r.degree_code=d.Degree_Code and de.Dept_Code=d.Dept_Code and r.Batch_Year=sm.Batch_Year and r.degree_code=sm.degree_code and r.Current_Semester=sm.semester and s.syll_code=sm.syll_code and r.Current_Semester in(" + sem + ")   and r.college_code=ci.college_code  and CC=0 and isnull(delflag,0)<>1 and r.Exam_Flag<>'DEBAR'  order by ccc,val";

                DataTable dtbatchinfo = dir.selectDataTable(Selectbatch);
                if (dtbatchinfo.Rows.Count > 0)
                {
                    cblBranch.DataSource = dtbatchinfo;
                    cblBranch.DataTextField = "ccc";
                    cblBranch.DataValueField = "val";
                    cblBranch.DataBind();
                    checkBoxListselectOrDeselect(cblBranch, true);
                    CallCheckboxListChange(chkBranch, cblBranch, txtBranch, "Branch", "--Select--");
                }

            }
        }

    }

    private void CheckUser()
    {
        DataTable dtStaff = new DataTable();
        string staffCode = Session["StaffCode"].ToString();
        ddlDept.Enabled = true;
        ddlSearchOption.Enabled = true;
        if (ddlDept.Items.Count > 0 && ddlSearchOption.Items.Count > 0)
        {
            string staffName = d2.GetFunction("select staff_name from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_code like '%" + staffCode + "%' and college_code='" + collegecode + "'");
            if (staffCode != "")
            {
                string qry = "select * from stafftrans where staff_code='" + staffCode + "' and latestrec=1";
                dtStaff = dir.selectDataTable(qry);
                if (dtStaff.Rows.Count > 0)
                {
                    ddlSearchOption.Items.FindByValue(Convert.ToString(dtStaff.Rows[0]["staff_code"])).Selected = true;
                    ddlDept.Items.FindByValue(Convert.ToString(dtStaff.Rows[0]["dept_code"])).Selected = true;
                    ddlDept.Enabled = false;
                    ddlSearchOption.Enabled = false;

                }
            }
        }
    }

    private void bindstaffCode()
    {
        try
        {
            DataTable dtStaff = new DataTable();
            selectedDept = Convert.ToString(ddlDept.SelectedValue);
            collegecode = Convert.ToString(ddlcollege.SelectedValue);
            if (!string.IsNullOrEmpty(selectedDept))
            {
                string query = "select distinct st.staff_code,(st.staff_code+'-'+sm.staff_name) as staff from stafftrans st, staffmaster sm where st.staff_code=sm.staff_code and resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and   dept_code in ('" + selectedDept + "')  and college_code='" + collegecode + "'";
                dtStaff = dir.selectDataTable(query);
                if (dtStaff.Rows.Count > 0)
                {
                    ddlSearchOption.DataSource = dtStaff;
                    ddlSearchOption.DataValueField = "staff_code";
                    ddlSearchOption.DataTextField = "staff";
                    ddlSearchOption.DataBind();
                }
            }
            CheckUser();

        }
        catch
        {

        }
    }

    private void bindDate()
    {
        try
        {
            txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlcollege_change(object sender, EventArgs e)
    {

        collegecode = ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedItem.Value) : Convert.ToString(Session["collegecode"]);
        bindDept();
        bindstaffCode();
        bindSem();
        bindSubject();
        bindbatchInfo();

        //tdStfCodeAuto.Visible = true;
        //tdStfNameAuto.Visible = false;

    }

    protected void ddlDept_change(object sender, EventArgs e)
    {
        bindstaffCode();
        bindSem();
        bindbatchInfo();
        bindDate();
    }

    protected void ddlsem_change(object sender, EventArgs e)
    {
        bindbatchInfo();
        bindDate();
    }

    protected void ddlSchinfo_change(object sender, EventArgs e)
    {
        bindDate();
    }

    protected void ddlSearchOption_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindSem();
            bindbatchInfo();
            bindDate();
        }
        catch
        {
        }
    }

    protected void chkBranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(chkBranch, cblBranch, txtBranch, "Branch", "--Select--");

        }
        catch (Exception ex)
        {
        }
    }
    protected void cblBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chkBranch, cblBranch, txtBranch, "Branch", "--Select--");

        }
        catch (Exception ex)
        {
        }
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

            string qry = "select distinct (c.Course_Name+' ('+de.dept_acronym+')-'+'Sem'+CONVERT(varchar, r.Current_Semester)+' '+ISNULL(r.Sections,''))Degree,r.Current_Semester  from Registration r,Degree d,Department de,course c where r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0  and r.degree_code='" + Convert.ToString(arr[0]).Trim() + "' and r.Batch_Year='" + Convert.ToString(arr[2]).Trim() + "' and r.Current_Semester='" + Convert.ToString(arr[1]).Trim() + "'" + strsec + " ";//and r.college_code='" + Convert.ToString(collegecode).Trim() + "'
            DataTable dDeg = dir.selectDataTable(qry);
            string str1 = string.Empty;
            if (dDeg.Rows.Count > 0)
            {
                string deg = Convert.ToString(dDeg.Rows[0]["Degree"]);
                string semD = Convert.ToString(dDeg.Rows[0]["Current_Semester"]);
                string sems = string.Empty;
                if (semD.Trim() == "1" || semD.Trim() == "2")
                    sems = "I Year ";
                if (semD.Trim() == "3" || semD.Trim() == "4")
                    sems = "II Year ";
                if (semD.Trim() == "5" || semD.Trim() == "6")
                    sems = "III Year ";
                if (semD.Trim() == "7" || semD.Trim() == "8")
                    sems = "IV Year ";
                if (semD.Trim() == "9" || semD.Trim() == "10")
                    sems = "V Year ";

                str1 = sems + "-" + " " + deg;
            }

            string subtype =d2.GetFunction("select ss.subject_type from subject s,sub_sem ss where s.subtype_no=ss.subtype_no and s.subject_no=" + Convert.ToString(subjectNo) + "");
            //textValue = d2.GetFunction(qry);
            string Points = d2.GetFunction("select m.points  from degree d,department de,course c,SubjectPointMaster m where d.dept_code=de.dept_code and d.course_id=c.course_id and d.degree_code='" + Convert.ToString(arr[0]).Trim() + "' and c.edu_level=m.Edulevel  and m.SubTypeName='" + subtype +"'");
            double point=0;
            double.TryParse(Points, out point);
            tothr = tothr + point;
            strSubName = (d2.GetFunction("select subject_name from subject where subject_no=" + Convert.ToString(subjectNo) + " "));
            noteValue = Convert.ToString(strScheduledHour) + "," + strSemSchedule;
            string room = string.Empty;
            room = d2.GetFunction("select rd.room_name from subject s,Room_detail rd where s.roompk=rd.roompk and s.subject_no='" + Convert.ToString(subjectNo) + "'");
            if (!string.IsNullOrEmpty(room) && room != "0")
                room = " R:" + room;
            else
                room = string.Empty;
            return str1 + "<br/>" + " " + room + "<br/>" + strSubName + "#" + noteValue;
            //return strSubName + "-" + textValue + room + "#" + noteValue;
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
            tothr = 0;
            btnPrint11();
            DataTable dtTTDisp = new DataTable();
            dtTTDisp.Columns.Add("DateDisp");
            dtTTDisp.Columns.Add("DateVal");
            dtTTDisp.Columns.Add("P1Val");
            dtTTDisp.Columns.Add("TT_1");
            dtTTDisp.Columns.Add("P2Val");
            dtTTDisp.Columns.Add("TT_2");
            dtTTDisp.Columns.Add("P3Val");
            dtTTDisp.Columns.Add("TT_3");
            dtTTDisp.Columns.Add("P4Val");
            dtTTDisp.Columns.Add("TT_4");
            dtTTDisp.Columns.Add("P5Val");
            dtTTDisp.Columns.Add("TT_5");
            dtTTDisp.Columns.Add("P6Val");
            dtTTDisp.Columns.Add("TT_6");
            dtTTDisp.Columns.Add("P7Val");
            dtTTDisp.Columns.Add("TT_7");
            dtTTDisp.Columns.Add("P8Val");
            dtTTDisp.Columns.Add("TT_8");
            dtTTDisp.Columns.Add("P9Val");
            dtTTDisp.Columns.Add("TT_9");
            dtTTDisp.Columns.Add("P10Val");
            dtTTDisp.Columns.Add("TT_10");
            gridTimeTable.Visible = false;
            DataRow drNew = null;
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

            string sql = "select max(No_of_hrs_per_day)HoursPerDay,MAX(nodays)NoOfDays from PeriodAttndSchedule";
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
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Please Select Branch!";
                btnGo_OnClick(sender, e);
            }

            string SchOrder = d2.GetFunction("select distinct top 1 schOrder from PeriodAttndSchedule");
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

            //string qryGetDegDetails = "select distinct r.Batch_Year,r.degree_code,sm.semester,r.Sections,si.end_date  from Registration r,subject s,syllabus_master sm,Department de,course c,Degree d,collinfo ci,seminfo si where si.batch_year=r.Batch_Year and si.degree_code=r.degree_code and si.semester=r.Current_Semester and sm.Batch_Year=si.batch_year and si.degree_code=sm.degree_code and si.semester=sm.semester and  c.Course_Id=d.Course_Id and r.degree_code=d.Degree_Code and de.Dept_Code=d.Dept_Code and r.Batch_Year=sm.Batch_Year and r.degree_code=sm.degree_code and r.Current_Semester=sm.semester and s.syll_code=sm.syll_code and  r.college_code=ci.college_code  and CC=0 and isnull(delflag,0)<>1 and r.Exam_Flag<>'DEBAR'";


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
                                                            spreadCellValue = spreadCellValue + ";" + oldValue;
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
                drNew = dtTTDisp.NewRow();
                string r = row.ToString();
                string dayName = DaysName[row];
                string dayAcronym = DaysAcronym[row];

                if (SchOrder == "1")
                {
                    drNew["DateDisp"] = dayName;
                    drNew["DateVal"] = dayAcronym;
                }
                else
                {
                    int dayNo = row + 1;
                    drNew["DateDisp"] = "Day " + dayNo;
                    drNew["DateVal"] = dayNo;
                }

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
                                    if (!string.IsNullOrEmpty(cellValue))
                                        cellValue = cellValue + ";" + "<br/><br/>" + val[0];
                                    else
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


                        string lbl1 = "P" + col + "Val";
                        string lbl2 = "TT_" + col;

                        drNew[lbl1] = cellValue;
                        drNew[lbl2] = cellNoteValue;

                    }

                }
                dtTTDisp.Rows.Add(drNew);
            }


            if (dtTTDisp.Rows.Count > 0)
            {
                gridTimeTable.DataSource = dtTTDisp;
                gridTimeTable.DataBind();
                gridTimeTable.Visible = true;
                GridView1.DataSource = dtTTDisp;
                GridView1.DataBind();
                GridView1.Visible = true;
                btnPrint.Visible = true;
            }
            if (noOfHrs != 0)
            {
                for (int i = 1; i <= noOfHrs; i++)
                {
                    gridTimeTable.Columns[i].Visible = true;
                    GridView1.Columns[i].Visible = true;
                }

            }
            //int HRCount = 0;
            //foreach (GridViewRow gr in GridView1.Rows)
            //{
            //    string p1 = (gr.FindControl("lnkPeriod_1") as LinkButton).Text;
            //    if (!string.IsNullOrEmpty(p1))
            //        HRCount = HRCount + 1;
            //    string p2 = (gr.FindControl("lnkPeriod_2") as LinkButton).Text;
            //    if (!string.IsNullOrEmpty(p2))
            //        HRCount = HRCount + 1;
            //    string p3 = (gr.FindControl("lnkPeriod_3") as LinkButton).Text;
            //    if (!string.IsNullOrEmpty(p3))
            //        HRCount = HRCount + 1;
            //    string p4 = (gr.FindControl("lnkPeriod_4") as LinkButton).Text;
            //    if (!string.IsNullOrEmpty(p4))
            //        HRCount = HRCount + 1;
            //    string p5 = (gr.FindControl("lnkPeriod_5") as LinkButton).Text;
            //    if (!string.IsNullOrEmpty(p5))
            //        HRCount = HRCount + 1;
            //    string p6 = (gr.FindControl("lnkPeriod_6") as LinkButton).Text;
            //    if (!string.IsNullOrEmpty(p6))
            //        HRCount = HRCount + 1;
            //    string p7 = (gr.FindControl("lnkPeriod_7") as LinkButton).Text;
            //    if (!string.IsNullOrEmpty(p7))
            //        HRCount = HRCount + 1;
            //    string p8 = (gr.FindControl("lnkPeriod_8") as LinkButton).Text;
            //    if (!string.IsNullOrEmpty(p8))
            //        HRCount = HRCount + 1;
            //    string p9 = (gr.FindControl("lnkPeriod_9") as LinkButton).Text;
            //    if (!string.IsNullOrEmpty(p9))
            //        HRCount = HRCount + 1;
            //    string p10 = (gr.FindControl("lnkPeriod_10") as LinkButton).Text;
            //    if (!string.IsNullOrEmpty(p10))
            //        HRCount = HRCount + 1;
            //}
            tothr = Math.Round(tothr, 0, MidpointRounding.AwayFromZero);
            spSection.InnerHtml = "Total No.of.Hours: " + tothr;//Deepali on 5.10.18
        }
        catch
        {

        }
    }

    protected void btnAddNew_OnClick(object sender, EventArgs e)
    {
        div3.Visible = true;
        Label4.Visible = true;
        Label4.Text = "Do you want Confirm to Save !";
    }

    protected void Button5_Click(object sender, EventArgs e)
    {
        div3.Visible = false;
        Label4.Visible = false;
        Label4.Text = "";
    }

    protected void btnAdd_OnClick(object sender, EventArgs e)
    {

        try
        {
            string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
            string splval = string.Empty;
            string subno_staff = string.Empty;
            string subno_staffnote = string.Empty;
            string dt = DateTime.Now.ToString("dd/MM/yyyy");
            string[] date = txtFromDate.Text.Split('/');
            string fromdate = date[1] + '/' + date[0] + '/' + date[2];
            string staffName = "";
            string staffCode = "";
            string qry = string.Empty;
            string tablevalue = string.Empty;
            string ttName = "";
            Hashtable hatdegree = new Hashtable();
            string history_data = string.Empty;
            string SubCode = string.Empty;
            string[] arr = null;
            string SchOrder = d2.GetFunction("select distinct top 1 schOrder from PeriodAttndSchedule");
            int day = 0;
            int i = 1;
            string sec = string.Empty;
            int rowCount = 0;
            int colCount = 0;
            int cb = 0;

            if (curRow == 0 && curCol == 0 && cbVal1 == 0)
            {
                rowCount = gridTimeTable.Rows.Count;
                colCount = gridTimeTable.Columns.Count;
                cbVal1 = 0;
            }
            else
            {
                day = curRow;
                i = curCol;
                cb = cbVal1;
            }

            if (curRow > gridTimeTable.Rows.Count)
                curRow = 0;
            if (curCol > gridTimeTable.Columns.Count)
                curCol = 1;
            if (curCol == 0)
                curCol = 1;
        lableNext:
            for (day = curRow; day < gridTimeTable.Rows.Count; day++)
            {
                for (i = curCol; i < gridTimeTable.Columns.Count; i++)
                {
                    if (gridTimeTable.Columns[i].Visible == true)
                    {
                        string cblist = "cblPeriod" + i;
                        string ckBox = "chkPeriod" + i;
                        string txt = "txtPeriod" + i;

                        string dayOrder = Convert.ToString(day);
                        string col = Convert.ToString(i);
                        int row = 0;
                        int.TryParse(dayOrder, out row);
                        string Daycoulmn = string.Empty;
                        string Daycoulmnvalue = string.Empty;
                        string dayofweek = Days[row];
                        Daycoulmn = dayofweek + Convert.ToString(col);

                        CheckBoxList atttype = (gridTimeTable.Rows[day].FindControl(cblist) as CheckBoxList);
                        CheckBox chk = (gridTimeTable.Rows[day].FindControl(ckBox) as CheckBox);
                        TextBox txtBox = (gridTimeTable.Rows[day].FindControl(txt) as TextBox);
                        string DayOrder = string.Empty;
                        string Hour = string.Empty;
                        if (SchOrder == "1")
                        {
                            DayOrder = Convert.ToString(Days[row]);
                            Hour = i.ToString();
                        }
                        else
                        {
                            int dayNo = day + 1;
                            DayOrder = "Day " + dayNo;
                            Hour = i.ToString();
                        }


                        if (atttype.Items.Count > 0)
                        {
                            for (cb = cbVal1; cb < atttype.Items.Count; cb++)
                            {

                                if (atttype.Items[cb].Selected)
                                {
                                    string cbVal = Convert.ToString(atttype.Items[cb].Value);//13-2017-55-1-A-1788

                                    if (!string.IsNullOrEmpty(cbVal))
                                    {
                                        string[] input = cbVal.Split('-');
                                        string selectedCollCode = Convert.ToString(input[0]);
                                        string selectedBatch = Convert.ToString(input[1]);
                                        string selectedDegCode = Convert.ToString(input[2]);
                                        string selectedSem = Convert.ToString(input[3]);
                                        string selectedSec = Convert.ToString(input[4]);
                                        string selectedsubNo = Convert.ToString(input[5]);

                                        if (!string.IsNullOrEmpty(selectedSec))
                                        {
                                            sec = selectedSec;
                                            // selectedSec = "  and Sections='" + selectedSec + "'";

                                        }
                                        //string timetabelname = d2.GetFunction("select TTName from Semester_Schedule where batch_year='" + selectedBatch + "' and degree_code='" + selectedDegCode + "' and semester='" + selectedSem + "' " + selectedSec + "  and FromDate>='" + fromdate + "' order by FromDate desc");
                                        string timetabelname = string.Empty;
                                        if (!string.IsNullOrEmpty(ddlSearchOption.SelectedValue))
                                        {
                                            staffCode = Convert.ToString(ddlSearchOption.SelectedValue).Trim();
                                            staffName = d2.GetFunction("select staff_name from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_code like '%" + staffCode + "%' and college_code='" + collegecode + "'");
                                        }

                                        Session["StaffCode"] = staffCode;

                                        if (!string.IsNullOrEmpty(timetabelname))
                                        {
                                            ttName = timetabelname;
                                        }
                                        else
                                        {
                                            ttName = selectedBatch + "/" + selectedDegCode + "/" + selectedSem + "/" + sec + "/" + fromdate;
                                        }
                                        if (!string.IsNullOrEmpty(ttName))
                                        {

                                            string subTypeNo = d2.GetFunction("select ss.Lab from subject s,syllabus_master sm,sub_sem ss where ss.syll_code=sm.syll_code and s.subType_no=ss.subType_no and s.syll_code=sm.syll_code and sm.Batch_Year='" + selectedBatch + "' and sm.degree_code='" + selectedDegCode + "' and sm.semester='" + selectedSem + "'' and s.subject_no='" + selectedsubNo + "'");
                                            //string subNo = d2.GetFunction("select s.subject_no from subject s,syllabus_master sm,sub_sem ss where ss.syll_code=sm.syll_code and s.subType_no=ss.subType_no and s.syll_code=sm.syll_code and sm.Batch_Year='" + selectedBatch + "' and sm.degree_code='" + selectedDegCode + "' and sm.semester='" + selectedSem + "' and s.subject_code='" + SubCode + "'");
                                            string subNo = selectedsubNo;

                                            if (subTypeNo == "1")
                                                subTypeNo = "L";
                                            else
                                                subTypeNo = "S";

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
                                                        curCol = i;
                                                        curRow = day;
                                                        cbVal1 = cb;

                                                        Label8.Text = "Already Selected this :" + staff + " Staff";//+ " for Day " + DayOrder + " Hour "+Hour
                                                        ModalPopupExtender1.Show();
                                                        goto lable1;
                                                    }
                                                }
                                            }

                                            string IncStaffSel = "  if not exists(select * from staff_selector where subject_no='" + subNo + "' and staff_code='" + staffCode + "' and batch_year='" + selectedBatch + "' and isnull(Sections,'')='" + sec + "')  insert into  staff_selector (subject_no,staff_code,batch_year,dailyflag,Sections) values ('" + subNo + "','" + staffCode + "','" + selectedBatch + "','0','" + sec + "')";
                                            int d = d2.update_method_wo_parameter(IncStaffSel, "text");

                                            if (!string.IsNullOrEmpty(Daycoulmnvalue) && !string.IsNullOrEmpty(selectedDegCode) && !string.IsNullOrEmpty(selectedSem) && !string.IsNullOrEmpty(selectedBatch) && !string.IsNullOrEmpty(Daycoulmn) && !string.IsNullOrEmpty(Daycoulmnvalue) && !string.IsNullOrEmpty(ttName) && !string.IsNullOrEmpty(fromdate))
                                            {


                                                string existingColValue = d2.GetFunction("select " + Daycoulmn + " from Semester_Schedule where degree_code='" + Convert.ToString(selectedDegCode) + "' and batch_year='" + Convert.ToString(selectedBatch) + "' and semester='" + Convert.ToString(selectedSem) + "' " + secval + " and TTName='" + ttName + "' and FromDate='" + fromdate + "'");
                                                bool isNot = false;
                                                if (Daycoulmnvalue == existingColValue)
                                                    isNot = true;

                                                if (!string.IsNullOrEmpty(existingColValue) && existingColValue != "0")
                                                    appndColumn = existingColValue + ";" + Daycoulmnvalue;
                                                else
                                                    appndColumn = Daycoulmnvalue;



                                                if (!isNot)
                                                {
                                                    string staffsubject = getstaffStatus(Daycoulmn);

                                                    if (existingColValue != "" && existingColValue != "0" && existingColValue != null)
                                                    {
                                                        string oldsch = getSemesterSch(existingColValue, Daycoulmn, selectedBatch, selectedDegCode, selectedSem, selectedSec);
                                                        if (!isChanged)
                                                        {
                                                            curCol = i;
                                                            curRow = day;
                                                            cbVal1 = cb;
                                                            lblErrmsg.Text = "Already Exists:  " + oldsch + " for Day " + DayOrder + " Hour " + Hour;
                                                            alert2PopUp.Show();
                                                            goto lable1;
                                                        }
                                                        if (replace)
                                                        {
                                                            status = insertRecord(selectedDegCode, selectedSem, selectedBatch, Daycoulmn, Daycoulmnvalue, ttName, fromdate, selectedSec);

                                                            replace = false;
                                                            isChanged = false;
                                                            goto lableNext;
                                                        }
                                                        if (appand)
                                                        {
                                                            status = insertRecord(selectedDegCode, selectedSem, selectedBatch, Daycoulmn, appndColumn, ttName, fromdate, selectedSec);
                                                            appand = false;
                                                            isChanged = false;
                                                        }
                                                    }
                                                    else if (!string.IsNullOrEmpty(staffsubject) && staffsubject != "-")
                                                    {
                                                        if (!isChanged)
                                                        {
                                                            curCol = i;
                                                            curRow = day;
                                                            cbVal1 = cb;
                                                            lblErrmsg.Text = "Staff is Busy: " + staffsubject + " for Day " + DayOrder + " Hour " + Hour;
                                                            alert2PopUp.Show();
                                                            goto lable1;
                                                        }
                                                        if (allowCombineClass)
                                                        {
                                                            string selectQ = d2.GetFunction("select " + Daycoulmn + " from Semester_Schedule where " + Daycoulmn + " like '%" + Convert.ToString(Session["StaffCode"]) + "%'  order by FromDate desc");
                                                            if (!string.IsNullOrEmpty(selectQ))
                                                                appndColumn = Daycoulmnvalue;

                                                            status = insertRecord(selectedDegCode, selectedSem, selectedBatch, Daycoulmn, appndColumn, ttName, fromdate, selectedSec);
                                                            isChanged = false;
                                                            allowCombineClass = false;
                                                            //curCol = i;
                                                            //curRow = day;
                                                            //cbVal1 = cb;
                                                            //goto lableNext;
                                                        }
                                                    }
                                                    if (!replace && !appand && !allowCombineClass)
                                                    {
                                                        status = insertRecord(selectedDegCode, selectedSem, selectedBatch, Daycoulmn, Daycoulmnvalue, ttName, fromdate, selectedSec);
                                                        isChanged = false;
                                                        //curCol = i;
                                                        //curRow = day;
                                                        //cbVal1 = cb;
                                                        //goto lableNext;
                                                    }
                                                }
                                                isChanged = false;
                                                allowCombineClass = false;
                                                appand = false;
                                                replace = false;
                                            }
                                        }
                                    }
                                }
                            }
                            cbVal1 = 0;
                        }
                    }
                }
                curCol = 1;
            }
            if (status != 0)
            {
                Label4.Text = "";

                curRow = 0;
                curCol = 0;
                cbVal1 = 0;

                gridTimeTable.Visible = true;
                GridView1.Visible = true;
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Saved Sucessfully";
                div3.Visible = false;
                Label4.Visible = false;
                return;
                //btnGo_OnClick(sender, e);
            }
            else
            {
                curRow = 0;
                curCol = 0;
                cbVal1 = 0;
                gridTimeTable.Visible = true;
                GridView1.Visible = true;
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Not Saved";
                div3.Visible = false;
                Label4.Visible = false;
                Label4.Text = "";
                return;
                //btnGo_OnClick(sender, e);
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
            btnGo_OnClick(sender, e);
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

    protected int deleteRecord(string degCode, string sem, string batch, string colName, string ttName, string ttDate, string sec, string subNo)
    {
        try
        {
            int status = 0;
            string columnValue = "";

            string existingColValue = d2.GetFunction("select " + colName + " from Semester_Schedule where degree_code='" + Convert.ToString(degCode) + "' and batch_year='" + Convert.ToString(batch) + "' and semester='" + Convert.ToString(sem) + "' and isnull(Sections,'')='" + Convert.ToString(sec) + "' and TTName='" + ttName + "' and FromDate='" + ttDate + "'");

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

                            if (val.Contains(Session["StaffCode"].ToString()))
                            {
                                string sub = Convert.ToString(val.Split('-')[0]);
                                if (sub != subNo)
                                {
                                    if (temp == "")
                                        temp = val;
                                    else
                                        temp = temp + ";" + val;
                                }
                            }
                            else
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
            try
            {
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
                                    string subNo = Convert.ToString(info[7]);
                                    count = deleteRecord(deg, sem, batch, colva, TTname, FDAte, Section, subNo);
                                }
                            }
                        }
                    }
                }

                if (count > 0)
                {
                    div1.Visible = true;
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.Text = "Deleted.!";
                    btnGo_OnClick(sender, e);
                }
                else
                {
                    lblErrorMsg.Visible = true;
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
        catch
        {

        }
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

    protected void gridTimeTable_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string collegeCode = Convert.ToString(ddlcollege.SelectedValue);
                string sem = Convert.ToString(ddlsem.SelectedValue);
                string staffCode = Convert.ToString(ddlSearchOption.SelectedValue);
                string degCode = string.Empty;
                string batchYear = string.Empty;
                string sec = string.Empty;
                string semester = string.Empty;
                string sections = string.Empty;
                DataTable dtSubject = new DataTable();
                if (sem.ToLower() == "odd")
                {
                    sem = "1,3,5,7";
                }
                else if (sem.ToLower() == "even")
                {
                    sem = "2,4,6,8";
                }
                string SelectSubject = string.Empty;
                if (cblBranch.Items.Count > 0)
                {
                    DataTable dttemp = new DataTable();
                    for (int cb = 0; cb < cblBranch.Items.Count; cb++)
                    {
                        if (cblBranch.Items[cb].Selected)
                        {
                            string deg = Convert.ToString(cblBranch.Items[cb].Value);
                            string[] val = deg.Split('-');

                            if (val.Length > 1)
                            {
                                degCode = string.Empty;
                                batchYear = string.Empty;
                                semester = string.Empty;
                                sections = string.Empty;
                                if (!string.IsNullOrEmpty(Convert.ToString(val[1])))
                                    degCode = Convert.ToString(val[1]);
                                if (!string.IsNullOrEmpty(Convert.ToString(val[0])))
                                    batchYear = Convert.ToString(val[0]);
                                if (!string.IsNullOrEmpty(Convert.ToString(val[2])))
                                    semester = Convert.ToString(val[2]);
                                if (!string.IsNullOrEmpty(Convert.ToString(val[3])))
                                    sections = " and r.Sections in('" + Convert.ToString(val[3]) + "') ";
                                if (string.IsNullOrEmpty(SelectSubject))
                                {
                                    //SelectSubject = "select distinct  CONVERT(nvarchar(max),convert(nvarchar(50),r.college_code)+'-'+convert(nvarchar(50),r.Batch_Year)+'-'+convert(nvarchar(50),isnull(d.Degree_Code,''))+'-'+CONVERT(nvarchar(5),r.current_semester)+'-'+isnull(r.Sections,'')+'-'+convert(nvarchar(100), s.subject_no)) as val,   CONVERT(nvarchar(max),convert(nvarchar(50),cc.coll_acronymn)+'-'+convert(nvarchar(50),r.Batch_Year)+'-'+convert(nvarchar(50),c.Course_Name)+'-'+convert(nvarchar(50),d.Acronym)+'-'+CONVERT(nvarchar(5),r.current_semester)+'-'+isnull(r.Sections,'')+'-'+s.subject_code) as text  from collinfo cc, Registration r,subject s,syllabus_master sm,Department de,course c,Degree d where  c.Course_Id=d.Course_Id and r.degree_code=d.Degree_Code and de.Dept_Code=d.Dept_Code and r.Batch_Year=sm.Batch_Year and r.degree_code=sm.degree_code and r.Current_Semester=sm.semester and s.syll_code=sm.syll_code and cc.college_code=r.college_code and r.Batch_Year in(" + batchYear + ") and r.degree_code in(" + degCode + ") and r.CC=0 and r.Current_Semester in(" + semester + ") " + sections + "  and ISNULL(r.DelFlag,0)=0 and r.Exam_Flag<>'Debar'";

                                    SelectSubject = "select distinct  CONVERT(nvarchar(max),convert(nvarchar(50),r.college_code)+'-'+convert(nvarchar(50),r.Batch_Year)+'-'+convert(nvarchar(50),isnull(d.Degree_Code,''))+'-'+CONVERT(nvarchar(5),r.current_semester)+'-'+isnull(r.Sections,'')+'-'+convert(nvarchar(100), s.subject_no)) as val,   CONVERT(nvarchar(max),convert(nvarchar(50),cc.coll_acronymn)+'-'+convert(nvarchar(50),r.Batch_Year)+'-'+convert(nvarchar(50),c.Course_Name)+'-'+convert(nvarchar(50),d.Acronym)+'-'+CONVERT(nvarchar(5),r.current_semester)+'-'+isnull(r.Sections,'')+'-'+s.subject_code+convert(nvarchar(max),isnull((select isnull('_'+rd.Room_Name,'') as room from Room_Detail rd where rd.Roompk=s.roompk ),''))) as text  from collinfo cc, Registration r,subject s,syllabus_master sm,Department de,course c,Degree d where  c.Course_Id=d.Course_Id and r.degree_code=d.Degree_Code and de.Dept_Code=d.Dept_Code and r.Batch_Year=sm.Batch_Year and r.degree_code=sm.degree_code and r.Current_Semester=sm.semester and s.syll_code=sm.syll_code and cc.college_code=r.college_code and r.Batch_Year in(" + batchYear + ") and r.degree_code in(" + degCode + ")  and r.Current_Semester in(" + semester + ") " + sections + "  and ISNULL(r.DelFlag,0)=0 and r.Exam_Flag<>'Debar' and r.CC=0";
                                }
                                else
                                {
                                    SelectSubject = SelectSubject + " union all select distinct  CONVERT(nvarchar(max),convert(nvarchar(50),r.college_code)+'-'+convert(nvarchar(50),r.Batch_Year)+'-'+convert(nvarchar(50),isnull(d.Degree_Code,''))+'-'+CONVERT(nvarchar(5),r.current_semester)+'-'+isnull(r.Sections,'')+'-'+convert(nvarchar(100), s.subject_no)) as val,   CONVERT(nvarchar(max),convert(nvarchar(50),cc.coll_acronymn)+'-'+convert(nvarchar(50),r.Batch_Year)+'-'+convert(nvarchar(50),c.Course_Name)+'-'+convert(nvarchar(50),d.Acronym)+'-'+CONVERT(nvarchar(5),r.current_semester)+'-'+isnull(r.Sections,'')+'-'+s.subject_code+convert(nvarchar(max),isnull((select isnull('_'+rd.Room_Name,'') as room from Room_Detail rd where rd.Roompk=s.roompk ),''))) as text  from collinfo cc, Registration r,subject s,syllabus_master sm,Department de,course c,Degree d where  c.Course_Id=d.Course_Id and r.degree_code=d.Degree_Code and de.Dept_Code=d.Dept_Code and r.Batch_Year=sm.Batch_Year and r.degree_code=sm.degree_code and r.Current_Semester=sm.semester and s.syll_code=sm.syll_code and cc.college_code=r.college_code and r.Batch_Year in(" + batchYear + ") and r.degree_code in(" + degCode + ") and r.CC=0 and r.Current_Semester in(" + semester + ") " + sections + "  and ISNULL(r.DelFlag,0)=0 and r.Exam_Flag<>'Debar'";
                                }
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(SelectSubject))
                    dtSubject = dir.selectDataTable(SelectSubject);

                if (dtSubject.Rows.Count > 0)
                {
                    for (int hr = 1; hr <= 10; hr++)
                    {
                        string cblist = "cblPeriod" + hr;
                        string ckBox = "chkPeriod" + hr;
                        string txt = "txtPeriod" + hr;
                        CheckBoxList atttype = (e.Row.FindControl(cblist) as CheckBoxList);
                        CheckBox chk = (e.Row.FindControl(ckBox) as CheckBox);
                        TextBox txtBox = (e.Row.FindControl(txt) as TextBox);
                        atttype.Items.Clear();
                        atttype.DataSource = dtSubject;
                        atttype.DataTextField = "text";
                        atttype.DataValueField = "val";
                        atttype.DataBind();
                        checkBoxListselectOrDeselect(atttype, false);
                        CallCheckboxListChange(chk, atttype, txtBox, "Subjects", "--Select--");
                    }
                }
            }

        }
        catch
        {
        }
    }

    protected void chkPeriod_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox ddlLabTest = (CheckBox)sender;
            var row = ddlLabTest.NamingContainer;
            string rowIndxS = ddlLabTest.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
            int rowIndx = Convert.ToInt32(rowIndxS) - 2;
            string colIndxS = ddlLabTest.UniqueID.ToString().Split('$')[4].Replace("chkPeriod", string.Empty);
            int colIndx = Convert.ToInt32(colIndxS);
            CheckBox ddlAddLabTestShortName = (CheckBox)row.FindControl("chkPeriod" + colIndx);
            CheckBoxList cbl = (CheckBoxList)row.FindControl("cblPeriod" + colIndx);
            TextBox txtB = (TextBox)row.FindControl("txtPeriod" + colIndx);
            CallCheckboxChange(ddlAddLabTestShortName, cbl, txtPeriod1, "Subjects", "--Select--");
        }
        catch (Exception ex)
        {
        }
    }

    protected void cblPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chkPeriod1, cblPeriod1, txtPeriod1, "SubjectS", "--Select--");
        }
        catch (Exception ex)
        {
        }
    }

    protected void lnkAttMark(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkSelected = (LinkButton)sender;
            string rowIndxS = lnkSelected.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
            int rowIndx = Convert.ToInt32(rowIndxS) - 2;
            string colIndxS = lnkSelected.UniqueID.ToString().Split('$')[4].Replace("lnkPeriod_", string.Empty);
            int colIndx = Convert.ToInt32(colIndxS);

            string staffCode = Convert.ToString(ddlSearchOption.SelectedValue).Trim();
            string activerow = rowIndx.ToString();
            string activecol = colIndx.ToString();
            string colVal = string.Empty;
            int row = 0;

            string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
            string Daycoulmn = string.Empty;
            string Daycoulmnvalue = string.Empty;

            string dayOrder = Convert.ToString(rowIndx);
            int colV = 0;

            int.TryParse(activecol, out colV);
            string col1 = Convert.ToString(colV + 1);
            int row1 = 0;
            int.TryParse(dayOrder, out row1);
            string dayofweek = Days[rowIndx];
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
                Label lblNot = (GridView1.Rows[rowIndx].FindControl("lblTT_" + colV) as Label);
                string NoteVal = lblNot.Text;

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
                                dr2["Colval"] = Batch + "-" + deg + "-" + SEM + "-" + Daycoulmn + "-" + TTname + "-" + FDAte + "-" + Section + "-" + subNo;
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
                            dr2["Colval"] = Batch + "-" + deg + "-" + SEM + "-" + Daycoulmn + "-" + TTname + "-" + FDAte + "-" + Section + "-" + subNo;
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
        catch
        {

        }

    }

    public void btnPrint11()
    {
        string college_code = Convert.ToString(ddlcollege.SelectedValue);
        string colQ = "select * from collinfo where college_code='" + college_code + "'";
        DataSet dsCol = new DataSet();
        dsCol = d2.select_method_wo_parameter(colQ, "Text");
        string collegeName = string.Empty;
        string collegeCateg = string.Empty;
        string collegeAff = string.Empty;
        string collegeAdd = string.Empty;
        string collegePhone = string.Empty;
        string collegeFax = string.Empty;
        string collegeWeb = string.Empty;
        string collegeEmai = string.Empty;
        string collegePin = string.Empty;
        string acr = string.Empty;
        string City = string.Empty;
        if (dsCol.Tables.Count > 0 && dsCol.Tables[0].Rows.Count > 0)
        {
            collegeName = Convert.ToString(dsCol.Tables[0].Rows[0]["Collname"]);
            City = Convert.ToString(dsCol.Tables[0].Rows[0]["address3"]);
            collegeAff = "(Affiliated to " + Convert.ToString(dsCol.Tables[0].Rows[0]["university"]) + ")";
            collegeAdd = Convert.ToString(dsCol.Tables[0].Rows[0]["address1"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["address2"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["district"]) + " - " + Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePin = Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePhone = "OFFICE: " + Convert.ToString(dsCol.Tables[0].Rows[0]["phoneno"]);
            collegeFax = "FAX: " + Convert.ToString(dsCol.Tables[0].Rows[0]["faxno"]);
            collegeWeb = "Website: " + Convert.ToString(dsCol.Tables[0].Rows[0]["website"]);
            collegeEmai = "E-Mail: " + Convert.ToString(dsCol.Tables[0].Rows[0]["email"]);
            collegeCateg = "(" + Convert.ToString(dsCol.Tables[0].Rows[0]["category"]) + ")";
        }
        DateTime dt = DateTime.Now;
        int year = dt.Year;
        spCollegeName.InnerHtml = collegeName;
        spAddr.InnerHtml = collegeAdd;
        spDegreeName.InnerHtml = acr;
        spReportName.InnerHtml = "STAFF TIME TABLE FOR THE ACADEMIC YEAR  " + year + "-" + (year + 1);
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);
        spProgremme.InnerHtml = "Satff: " + Convert.ToString(ddlSearchOption.SelectedItem.Text) + "<br/>" + "Department: " + Convert.ToString(ddlDept.SelectedItem.Text);

    }

    protected void btnpopup_clcik(object sender, EventArgs e)
    {
        //poperrjs.Visible = false;
    }

    public void bindEdulevel()
    {
        string coCode = Convert.ToString(ddlcollege.SelectedItem.Value);
        string Qry = "select distinct Edu_Level from course where college_code='" + coCode + "' order by Edu_Level asc";
        DataTable dt = dir.selectDataTable(Qry);
        if (dt.Rows.Count > 0)
        {
            DropDownList1.DataSource = dt;
            DropDownList1.DataTextField = "Edu_Level";
            DropDownList1.DataValueField = "Edu_Level";
            DropDownList1.DataBind();
        }
        DropDownList1.Items.Insert(0," ");
    }

    protected void gridPoint_OnDataBound(object sender, EventArgs e)//DropDownList1_change
    {

    }
    protected void DropDownList1_change(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void btn_SaveMandfee_Click(object sender, EventArgs e)
    {
        lblErrMsg11.Visible = false;
        string coCode = Convert.ToString(ddlcollege.SelectedItem.Value);
        string edu = Convert.ToString(DropDownList1.SelectedItem.Text);
        int cout = 0;
        if (!string.IsNullOrEmpty(edu) && edu != "" && edu != null)
        {
            foreach (GridViewRow gr in gridPoint.Rows)
            {
                string subtype = (gr.FindControl("lblSubjectType") as Label).Text;
                string point = (gr.FindControl("txtPont") as TextBox).Text;
                if (!string.IsNullOrEmpty(subtype) && !string.IsNullOrEmpty(coCode) && !string.IsNullOrEmpty(edu))
                {
                    string InsertQ = "if exists(select * from SubjectPointMaster where CollegeCode='" + coCode + "' and Edulevel='" + edu + "' and SubTypeName='" + subtype + "') update SubjectPointMaster SET  Points='" + point + "' where CollegeCode='" + coCode + "' and Edulevel='" + edu + "' and SubTypeName='" + subtype + "'  else insert into SubjectPointMaster(CollegeCode,Edulevel,SubTypeName,Points) values('" + coCode + "','" + edu + "','" + subtype + "','" + point + "')";
                    cout = d2.update_method_wo_parameter(InsertQ,"text");
                }
            }
        }
        if (cout > 0)
        {
            lblErrMsg11.Visible = true;
            lblErrMsg11.Text = "Saved Successfully.!";
        }
        else
        {
            lblErrMsg11.Visible = true;
            lblErrMsg11.Text = "Select Edu Level";
        }
    }
    protected void btn_ResetMandFee_Click(object sender, EventArgs e)
    {
    }
    protected void btn_CloseMandFee_Click(object sender, EventArgs e)
    {
        divMandFee.Visible =false;
    }
    protected void lnksetting_click(object sender, EventArgs e)
    {
        divMandFee.Visible = true;
        lblErrMsg11.Visible = false;
        DropDownList1.ClearSelection();
        gridPoint.Visible = false;

    }


    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridPoint.PageIndex = e.NewPageIndex;
        gridPoint.DataBind();
    }
    private void BindGrid()
    {
        lblErrMsg11.Visible = false;
        string coCode = Convert.ToString(ddlcollege.SelectedItem.Value);
        string edu = Convert.ToString(DropDownList1.SelectedItem.Text);
        if (!string.IsNullOrEmpty(edu.Trim()))
        {
            string SelectQ = "select distinct isnull(ss.subject_type,'') as subject_type,(select isnull(Points,' ') from SubjectPointMaster sm where ss.subject_type=sm.SubTypeName and  sm.Edulevel='" + edu + "') as points  from subject s,sub_sem ss,syllabus_master sy,Registration r where r.degree_code=sy.degree_code and r.Batch_Year=sy.Batch_Year and r.Current_Semester=sy.semester and s.syll_code=ss.syll_code and s.syll_code=sy.syll_code and r.CC=0 and r.DelFlag<>1 and r.Exam_Flag<>'Debar' and r.college_code='" + coCode + "' order by subject_type asc";
            DataTable dtSubjectInfo = dir.selectDataTable(SelectQ);

            if (dtSubjectInfo.Rows.Count > 0)
            {
                gridPoint.DataSource = dtSubjectInfo;
                gridPoint.DataBind();
                gridPoint.Visible = true;
            }
            else
            {
                gridPoint.Visible = false;
            }
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

    #endregion


}