using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using InsproDataAccess;
using System.Drawing;
using System.Configuration;

public partial class MarkMod_InvigilationAlter : System.Web.UI.Page
{

    string usercode = string.Empty;
    static string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string sessstream = string.Empty;
    string staff_code = string.Empty;
    string selectQuery = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet dsprint = new DataSet();
    ArrayList colord = new ArrayList();
    DAccess2 queryObject = new DAccess2();
    DAccess2 da = new DAccess2();
    DataView dvhead = new DataView();
    DataSet dscol = new DataSet();
    Hashtable grandtotal = new Hashtable();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    string selQ = string.Empty;
    string CycleTestname = string.Empty;
    string CycleTestno = string.Empty;
    int ACTROW = 0;

   static DataTable dt = new DataTable();
    DataRow dr;
    DataTable dtstaff1 = new DataTable();
    DataRow drstaff;


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
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        staff_code = (string)Session["Staff_Code"];

        if (!IsPostBack)
        {
            if (staff_code == "" || staff_code == null)
            {
                Response.Write("You Are not a Valid Staff");
                return;
            }
            ExamDate();
            HallNo();
            College();
            BindAlterStaffDepartment(((ddlAlterFreeCollege.Items.Count > 0) ? Convert.ToString(ddlAlterFreeCollege.SelectedValue).Trim() : collegecode));
            showreport1.Visible = false;
            btn_save.Visible = false;


        }

    }

    #region ExamDate

    public void ExamDate()
    {
        try
        {

            cbl_date.Items.Clear();
            cb_date.Checked = false;
            txt_date.Text = "---Select---";
            ds.Clear();
            ds = d2.BindBatch();
            string Query = "select distinct Convert(varchar(10),e.exam_date,103) as exam_date from CriteriaForInternal ci,Registration r,syllabus_master sm,Exam_type e where ci.syll_code=sm.syll_code and ci.Criteria_no=e.criteria_no and sm.semester=r.Current_Semester and sm.Batch_Year=r.Batch_Year and r.degree_code=sm.degree_code and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_date.DataSource = ds;
                cbl_date.DataTextField = "exam_date";
                cbl_date.DataValueField = "exam_date";
                cbl_date.DataBind();
                if (cbl_date.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_date.Items.Count; i++)
                    {
                        cbl_date.Items[i].Selected = true;
                    }
                    txt_date.Text = "Date(" + cbl_date.Items.Count + ")";
                    cb_date.Checked = true;
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationAlterStaff"); }
    }

    protected void cb_date_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_date, cbl_date, txt_date, "Date", "--Select--");
            showreport1.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationAlterStaff"); }
    }

    protected void cbl_date_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_date, cbl_date, txt_date, "Date", "--Select--");

            showreport1.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationAlterStaff"); }
    }

    #endregion

    #region HallNo

    public void HallNo()
    {
        try
        {

            cbl_hall.Items.Clear();
            cb_hall.Checked = false;
            txt_hall.Text = "---Select---";
            ds1.Clear();
            ds1 = d2.BindBatch();
            string Query = "select distinct e.exam_date as exam_date,i.hallNo  from  CriteriaForInternal ci,Registration r,syllabus_master sm,Exam_type e,internalSeatingArragement i where ci.syll_code=sm.syll_code and ci.Criteria_no=e.criteria_no and sm.semester=r.Current_Semester and sm.Batch_Year=r.Batch_Year and r.degree_code=sm.degree_code and  i.examCode=e.exam_code and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar'";
            ds1.Clear();
            ds1 = d2.select_method_wo_parameter(Query, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                cbl_hall.DataSource = ds1;
                cbl_hall.DataTextField = "hallNo";
                cbl_hall.DataValueField = "hallNo";
                cbl_hall.DataBind();
                if (cbl_hall.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_hall.Items.Count; i++)
                    {
                        cbl_hall.Items[i].Selected = true;
                    }
                    txt_hall.Text = "HallNo(" + cbl_hall.Items.Count + ")";
                    cb_hall.Checked = true;
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationAlterStaff"); }
    }

    protected void cb_hall_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_hall, cbl_hall, txt_hall, "cycletest", "--Select--");
            showreport1.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationAlterStaff"); }
    }

    protected void cbl_hall_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_hall, cbl_hall, txt_hall, "cycletest", "--Select--");
            showreport1.Visible = false;


        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationAlterStaff"); }
    }

    #endregion

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

    #endregion

    #region Go

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsStaffAlter = new DataSet();
            dsStaffAlter = Alterstaffselect();
            if (dsStaffAlter.Tables.Count > 0 && dsStaffAlter.Tables[0].Rows.Count > 0)
            {
                loadspreadCount(dsStaffAlter);
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Found!";
                showreport1.Visible = false;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationAlterStaff"); }
    }

    #endregion

    #region fpspread

    private DataSet Alterstaffselect()
    {
        DataSet dsloaddetails = new DataSet();
        try
        {
            #region get Value
            string TestDate = string.Empty;
            string TestHall = string.Empty;
            if (cbl_date.Items.Count > 0)
                TestDate = Convert.ToString(getCblSelectedText(cbl_date));

            if (cbl_hall.Items.Count > 0)
                TestHall = Convert.ToString(getCblSelectedText(cbl_hall));
            staff_code = (string)Session["Staff_Code"];

            if (!string.IsNullOrEmpty(TestDate) && !string.IsNullOrEmpty(TestHall) && !string.IsNullOrEmpty(staff_code))
            {
                //selQ = "select distinct c.course_name +' - '+CONVERT(varchar(10),r.batch_year)+' - '+dt.dept_acronym+' - '+ CONVERT(varchar(10),r.Current_semester)+' - '+r.Sections as degree from degree d,course c,department dt,registration r,internalSeatingArragement i where d.course_id=c.course_id and d.dept_code=dt.dept_code and i.appno=r.App_no and r.degree_code=d.Degree_Code and CONVERT(varchar(20),i.examDate,103) in ('" + TestDate + "') and i.hallNo in('" + TestHall + "') and i.staff_code='" + staff_code + "';";//Degree Details

                //selQ += "select distinct CONVERT(varchar(20),e.examFromTime,103) InDate,CONVERT(varchar(8),e.examFromTime,108) ExamInTime,e.examToTime,CONVERT(varchar(8),e.examToTime,103) OutDate,CONVERT(varchar(5),e.examToTime,108) ExamOutTime,e.examToTime,CONVERT(varchar(20),i.examDate,103) ExamDate,CONVERT(varchar(8),i.examDate,108) ExamTime,i.examDate, ci.criteria,i.hallNo,i.criteriaNo,i.staff_code  from CriteriaForInternal ci,Registration r,syllabus_master sm,Exam_type e,internalSeatingArragement i where ci.syll_code=sm.syll_code and ci.Criteria_no=e.criteria_no  and sm.semester=r.Current_Semester and sm.Batch_Year=r.Batch_Year and r.degree_code=sm.degree_code and i.examCode=e.exam_code  and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and CONVERT(varchar(20),i.examDate,103) in('" + TestDate + "') and i.hallNo in('" + TestHall + "') and i.staff_code='" + staff_code + "';";//Exam Details

                //selQ = "select distinct c.course_name +' - '+CONVERT(varchar(10),r.batch_year)+' - '+dt.dept_acronym+' - '+ CONVERT(varchar(10),r.Current_semester)+' - '+r.Sections as degree, CONVERT(varchar(20),e.examFromTime,103) InDate,CONVERT(varchar(8),e.examFromTime,108) ExamInTime,e.examToTime,CONVERT(varchar(8),e.examToTime,103) OutDate,CONVERT(varchar(5),e.examToTime,108) ExamOutTime,e.examToTime,CONVERT(varchar(20),i.examDate,103) ExamDate,CONVERT(varchar(8),i.examDate,108) ExamTime,i.examDate, ci.criteria,i.hallNo,i.criteriaNo,i.staff_code  from degree d,course c,department dt,CriteriaForInternal ci,Registration r,syllabus_master sm,Exam_type e,internalSeatingArragement i where ci.syll_code=sm.syll_code and ci.Criteria_no=e.criteria_no  and sm.semester=r.Current_Semester and sm.Batch_Year=r.Batch_Year and r.degree_code=sm.degree_code and d.course_id=c.course_id and d.dept_code=dt.dept_code and i.appno=r.App_no and r.degree_code=d.Degree_Code and i.examCode=e.exam_code  and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and CONVERT(varchar(20),i.examDate,103) in('" + TestDate + "') and i.hallNo in('" + TestHall + "') and i.staff_code='" + staff_code + "';";


                //selQ += "select distinct sa.appl_id,i.staff_code,sfm.staff_name,s.subject_name,s.subject_code,s.subject_no from staffmaster sfm inner join staff_appl_master sa on sa.appl_no=sfm.appl_no inner join stafftrans sts on sts.staff_code=sfm.staff_code inner join hrdept_master hr on hr.dept_code=sts.dept_code inner join internalSeatingArragement i on sts.staff_code=i.staff_code inner join subject s on i.subjectNo=s.subject_no where sts.latestrec='1' and sfm.resign=0 and sfm.settled=0 and sfm.college_code=hr.college_code  and CONVERT(varchar(20),i.examDate,103) in('" + TestDate + "') and i.hallNo in('" + TestHall + "') and i.staff_code='" + staff_code + "' order by staff_name,i.staff_code,s.subject_name,s.subject_code;";//Subject And Staff Details

                selQ = "select distinct c.course_name +' - '+CONVERT(varchar(10),r.batch_year)+' - '+dt.dept_acronym+' - '+ CONVERT(varchar(10),r.Current_semester)+' - '+r.Sections as degree, CONVERT(varchar(20),e.examFromTime,103) InDate,CONVERT(varchar(8),e.examFromTime,108) ExamInTime,e.examToTime,CONVERT(varchar(8),e.examToTime,103) OutDate,CONVERT(varchar(5),e.examToTime,108) ExamOutTime,e.examToTime,CONVERT(varchar(20),i.examDate,103) ExamDate,CONVERT(varchar(8),i.examDate,108) ExamTime,i.examDate, ci.criteria,i.hallNo,i.criteriaNo,i.staff_code,sa.appl_id,i.staff_code,sfm.staff_name,s.subject_name,s.subject_code,s.subject_no from degree d,course c,department dt,CriteriaForInternal ci,Registration r,syllabus_master sm,Exam_type e,staffmaster sfm inner join staff_appl_master sa on sa.appl_no=sfm.appl_no inner join stafftrans sts on sts.staff_code=sfm.staff_code inner join hrdept_master hr on hr.dept_code=sts.dept_code inner join internalSeatingArragement i on sts.staff_code=i.staff_code inner join subject s on i.subjectNo=s.subject_no where ci.syll_code=sm.syll_code and ci.Criteria_no=e.criteria_no  and sm.semester=r.Current_Semester and sm.Batch_Year=r.Batch_Year and r.degree_code=sm.degree_code and d.course_id=c.course_id and d.dept_code=dt.dept_code and i.appno=r.App_no and r.degree_code=d.Degree_Code and i.examCode=e.exam_code and sts.latestrec='1' and sfm.resign=0 and sfm.settled=0   and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and CONVERT(varchar(20),i.examDate,103) in('" + TestDate + "') and i.hallNo in('" + TestHall + "') and i.staff_code='" + staff_code + "';";
                dsloaddetails.Clear();
                dsloaddetails = d2.select_method_wo_parameter(selQ, "Text");

            }

            #endregion
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationAlterStaff"); }
        return dsloaddetails;
    }

    private void loadspreadCount(DataSet ds)
    {
        try
        {
            dt.Columns.Clear();
            dt.Clear();
            dt.Columns.Add("Degree_details");
            dt.Columns.Add("Date");
            dt.Columns.Add("HallNo");
            dt.Columns.Add("examdate");
            dt.Columns.Add("criteriano");
            dt.Columns.Add("Session");
            dt.Columns.Add("SubjectName");
            dt.Columns.Add("subjectno");
            dt.Columns.Add("StaffName");
            dt.Columns.Add("Staffcode");
            dt.Columns.Add("AlterStaffName");
            dt.Columns.Add("AlterStaffcode");
            dt.Columns.Add("AlterStaffapplid");

          
           // int sno = 0;
            DateTime an = new DateTime();
            DateTime fn = new DateTime();
            string examtime = string.Empty;
            string Antime = string.Empty;
            string Fntime = string.Empty;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                   // spreadDet1.Sheets[0].RowCount++;
                   // sno++;
                    string batch = Convert.ToString(ds.Tables[0].Rows[row]["degree"]).Trim();
                    string examtestdate = Convert.ToString(ds.Tables[0].Rows[row]["ExamDate"]).Trim();
                    string hallnumber = Convert.ToString(ds.Tables[0].Rows[row]["hallNo"]).Trim();
                    string session = Convert.ToString(ds.Tables[0].Rows[row]["ExamInTime"]).Trim();
                    string subjectname = Convert.ToString(ds.Tables[0].Rows[row]["subject_name"]).Trim();
                    string Staffname = Convert.ToString(ds.Tables[0].Rows[row]["staff_name"]).Trim();
                    an = Convert.ToDateTime("09:00:00");
                    fn = Convert.ToDateTime("12:00:00");
                    examtime = Convert.ToString(session);
                    Antime = Convert.ToString(an);
                    Fntime = Convert.ToString(fn);

                    dr = dt.NewRow();
                    dr["Degree_details"] = batch;
                    dr["Date"] = examtestdate;
                    dr["HallNo"] = hallnumber;
                    dr["examdate"] = Convert.ToString(ds.Tables[0].Rows[row]["examdate"]).Trim();
                    dr["criteriano"] = Convert.ToString(ds.Tables[0].Rows[row]["criteriaNo"]).Trim();

                    if ((!string.IsNullOrEmpty(examtime) && !string.IsNullOrEmpty(Antime)) || (!string.IsNullOrEmpty(examtime) && !string.IsNullOrEmpty(Fntime)))
                    {
                        
                        string[] split1 = Antime.Split(' ');
                        string[] split2 = Fntime.Split(' ');

                        if (split1.Length > 0 ||  split2.Length > 0)
                        {
                           
                            string AnTime = split1[1];
                            string FnTime = split2[1];
                            if (Convert.ToDateTime(examtime) < Convert.ToDateTime(AnTime))
                            {
                                dr["Session"] = "AN";

                            }
                            else if (Convert.ToDateTime(examtime) <= Convert.ToDateTime(FnTime))
                            {
                                dr["Session"] = "FN";

                            }

                        }
                    }

                    dr["SubjectName"] = subjectname;
                    dr["subjectno"] = Convert.ToString(ds.Tables[0].Rows[row]["subject_no"]).Trim();
                    dr["StaffName"] = Staffname;
                    dr["Staffcode"] = Convert.ToString(ds.Tables[0].Rows[row]["staff_code"]).Trim();
                    dt.Rows.Add(dr);
                }
                GridView1.DataSource = dt;
                GridView1.DataBind();
                GridView1.Columns[8].Visible = false;
                showreport1.Visible = true;
                btn_save.Visible = false;

            }
        }

        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationAlterStaff"); }
    }

    protected void Pagingindexchanged(object sender, EventArgs e)
    {

        divAlterFreeStaffDetails.Visible = true;
        btnSearch_click(sender, e);
    }

    protected void btnstaff_OnClick(object sender, EventArgs e)
    {
        Button selectstaf = (Button)sender;
        string rowindex = selectstaf.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
        int rowindexs = Convert.ToInt32(rowindex) - 2;
        Session["rowIndex"] = rowindexs.ToString();
        divAlterFreeStaffDetails.Visible = true;
        btnSearch_click(sender, e);
    }

    protected void gridview1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
   
    #endregion

    #region Popup

    private void College()
    {
        try
        {
            ddlAlterFreeCollege.Items.Clear();
            string qry = "select collname,college_code from collinfo order by college_code";
            DataTable dtCollege = dirAcc.selectDataTable(qry);
            if (dtCollege.Rows.Count > 0)
            {
                ddlAlterFreeCollege.DataSource = dtCollege;
                ddlAlterFreeCollege.DataTextField = "collname";
                ddlAlterFreeCollege.DataValueField = "college_code";
                ddlAlterFreeCollege.DataBind();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationAlterStaff"); }
    }

    private void BindAlterStaffDepartment(string collegeCode)
    {
        try
        {
            ddlAlterFreeDepartment.Items.Clear();
            DataTable dtDept = new DataTable();
            string qry = string.Empty;
            if (!string.IsNullOrEmpty(collegeCode))
            {
                qry = "select distinct dept_name,dept_code from hrdept_master where college_code='" + collegecode + "'";
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
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationAlterStaff"); }
    }

    #region Event
    protected void ddlAlterFreeCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlAlterFreeCollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlAlterFreeCollege.SelectedItem.Value);
                BindAlterStaffDepartment((ddlAlterFreeCollege.Items.Count > 0) ? Convert.ToString(ddlAlterFreeCollege.SelectedValue).Trim() : collegecode);
                ddlAlterFreeCollege.SelectedIndex = ddlAlterFreeCollege.Items.IndexOf(ddlAlterFreeCollege.Items.FindByValue(collegecode));
                //Fpuser.Visible = false;
                //fpstaff.Visible = false;
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationAlterStaff"); }
    }

    protected void ddlAlterFreeDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationAlterStaff"); }
    }

    protected void ddlAlterFreeStaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //GetStaffDetails();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationAlterStaff"); }
    }

    protected void txtAlterFreeStaffSearch_TextChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationAlterStaff"); }
    }

    protected void ddl_desig_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindAlterStaffDepartment(collegecode);
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationAlterStaff"); }
    }
    #endregion

    #region search
    protected void btnSearch_click(object sender, EventArgs e)
    {
        try
        {
            string val = txtAlterFreeStaffSearch.Text;
            txtAlterFreeStaffSearch.Text = "";
            BindAlterStaffDepartment(((ddlAlterFreeCollege.Items.Count > 0) ? Convert.ToString(ddlAlterFreeCollege.SelectedValue).Trim() : collegecode));
            DataTable dtstaff = new DataTable();
            dtstaff = getFreeStaffListNew("");
            if (dtstaff.Rows.Count > 0 && dtstaff.Rows.Count > 0)
            {
                loadspreadpopup(dtstaff);

            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "No Record Found!";

            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationAlterStaff"); }
    }

    protected void btnSearch_clickNEw(object sender, EventArgs e)
    {
        DataTable dtstaff = new DataTable();
        string searchValue = txtAlterFreeStaffSearch.Text;
        dtstaff = getFreeStaffListNew(searchValue);
        if (dtstaff.Rows.Count > 0 && dtstaff.Rows.Count > 0)
        {
            loadspreadpopup(dtstaff);

        }
        else
        {
            divPopAlert.Visible = true;
            txtAlterFreeStaffSearch.Text = "";
            lblAlertMsg.Text = "No Record Found!";
        }
    }
    #endregion


    public DataTable getFreeStaffListNew(string searchValue = null)
    {
        DataTable dtFreeStaffList = new DataTable();
        string qry = string.Empty;
        try
        {
            string qryStaffFilter = string.Empty;
            string qryDeptFilter = string.Empty;
            string qryCollegeFilter = string.Empty;

            if (ddlAlterFreeCollege.Items.Count > 0)
                qryCollegeFilter = " and sfm.college_code ='" + Convert.ToString(ddlAlterFreeCollege.SelectedValue).Trim() + "'";
            if (ddlAlterFreeDepartment.Items.Count > 0)
                if (!string.IsNullOrEmpty(Convert.ToString(ddlAlterFreeDepartment.SelectedValue).Trim()) && Convert.ToString(ddlAlterFreeDepartment.SelectedValue).Trim().ToLower() != "all")
                    qryDeptFilter = " and hr.dept_code='" + Convert.ToString(ddlAlterFreeDepartment.SelectedValue).Trim() + "'";
            if (!string.IsNullOrEmpty(searchValue))
                if (ddlAlterFreeStaff.Items.Count > 0)
                    if (ddlAlterFreeStaff.SelectedIndex == 0)
                        qryStaffFilter = " and sfm.staff_name like '" + searchValue + "%'";
                    else
                        qryStaffFilter = " and sfm.staff_code like '" + searchValue + "%'";
            qry = "select distinct sa.appl_id,sfm.staff_code,sfm.staff_name+' [ '+sfm.staff_code+' ]' as staff_name,'0' Experiance,sfm.join_date,sa.experience_info,convert(nvarchar(15),sfm.join_date,101) as jdate from staffmaster sfm inner join staff_appl_master sa on sa.appl_no=sfm.appl_no inner join stafftrans sts on sts.staff_code=sfm.staff_code inner join hrdept_master hr on hr.dept_code=sts.dept_code where sts.latestrec='1' and sfm.resign=0 and sfm.settled=0 and sfm.college_code=hr.college_code  and sfm.college_code =hr.college_code " + qryCollegeFilter + qryDeptFilter + qryStaffFilter + " order by staff_name,sfm.staff_code";
            //qry = " select distinct sfm.staff_code,sfm.staff_name+' [ '+sfm.staff_code+' ]' as staff_name,'0' Experiance from staffmaster sfm inner join stafftrans sts on sts.staff_code=sfm.staff_code inner join hrdept_master hr on hr.dept_code=sts.dept_code where sts.latestrec='1' and sfm.resign=0 and sfm.settled=0 and sfm.college_code=hr.college_code " + qryCollegeFilter + qryDeptFilter + qryStaffFilter + " order by staff_name,sfm.staff_code";
            dtFreeStaffList.Clear();
            dtFreeStaffList = dirAcc.selectDataTable(qry);
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationAlterStaff"); }
        return dtFreeStaffList;
    }

    private void loadspreadpopup(DataTable dtstaff)
    {
        try
        {
            dtstaff1.Clear();
            dtstaff1.Columns.Clear();
            dtstaff1.Columns.Add("StaffName");
            dtstaff1.Columns.Add("staffcode");
            dtstaff1.Columns.Add("applid");
            dtstaff1.Columns.Add("exp");

           // int rowcount = 0;
            for (int i = 0; i < dtstaff.Rows.Count; i++)
            {
                drstaff = dtstaff1.NewRow();
                drstaff["StaffName"] = dtstaff.Rows[i]["staff_name"].ToString();
                drstaff["staffcode"] = Convert.ToString(dtstaff.Rows[i]["staff_code"]).Trim();
                drstaff["applid"] = Convert.ToString(dtstaff.Rows[i]["appl_id"]).Trim();
                //fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 0].Text = fpstaff.Sheets[0].RowCount.ToString();
                //fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                //fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 0].Locked = true;

               // fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 1].CellType = txt;
                //fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 1].Text = dtstaff.Rows[i]["staff_name"].ToString();
                //fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 1].Locked = true;

                //fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dtstaff.Rows[i]["staff_code"]).Trim();
                //fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(dtstaff.Rows[i]["appl_id"]).Trim();
                //fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;

                

                //fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 3].CellType = chk;
                //fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;


                string perexp = dtstaff.Rows[i]["experience_info"].ToString();
                string joindate = dtstaff.Rows[i]["jdate"].ToString();

                Boolean valflag = false;

                int expyear = 0;
                int expmon = 0;
                string previousexperience = "";
                string[] spit = perexp.Split('\\');
                for (int s = 0; s <= spit.GetUpperBound(0); s++)
                {
                    if (spit[s].Trim().ToString() != "" && spit[s] != "")
                    {
                        string[] sporg = spit[s].Split(';');
                        if (sporg.GetUpperBound(0) > 10)
                        {
                            string yer = sporg[6].ToString();
                            if (yer.ToString().Trim() != "" && yer != null)
                            {
                                expyear = expyear + Convert.ToInt32(yer);
                            }
                            string mon = sporg[7].ToString();
                            if (mon.ToString().Trim() != "" && mon != null)
                            {
                                expmon = expmon + Convert.ToInt32(mon);
                            }
                        }
                    }
                }
                int exy = 0;
                int exaxcm = 0;
                if (expmon.ToString().Trim() != "" && expmon != null)
                {
                    if (expmon > 11)
                    {
                        exy = expmon / 12;
                        exaxcm = expmon % 12;
                    }
                    else
                    {
                        exaxcm = expmon;
                    }
                }
                expyear = expyear + exy;
                if (expyear > 0 || exaxcm > 0)
                {
                    if (expyear > 0)
                    {
                        previousexperience = " Years :" + expyear + "";
                    }
                    if (exaxcm > 0)
                    {
                        if (previousexperience.Trim() != "")
                        {
                            previousexperience = previousexperience + " Months :" + exaxcm + "";
                        }
                        else
                        {
                            previousexperience = " Months :" + exaxcm + "";
                        }
                    }


                }
                else
                {
                    previousexperience = "-";
                }

                int cureyear = 0;
                int curemonth = 0;
                string collexperience = "";
                string joindatestaff = "-";
                if (joindate.Trim() != "" && joindate != null)
                {
                    DateTime dtexp = Convert.ToDateTime(joindate);
                    joindatestaff = dtexp.ToString("dd/MM/yyyy");
                }
                if (joindate.Trim() != "" && joindate != null)
                {
                    DateTime dt = DateTime.Now;
                    DateTime dtexp = Convert.ToDateTime(joindate);
                    int cury = Convert.ToInt32(dt.ToString("yyyy"));
                    int jyear = Convert.ToInt32(dtexp.ToString("yyyy"));
                    cureyear = cury - jyear;

                    int curmon = Convert.ToInt32(dt.ToString("MM"));
                    int jmon = Convert.ToInt32(dtexp.ToString("MM"));
                    if (curmon < jmon)
                    {
                        curemonth = (curmon + 12) - jmon;
                        cureyear--;
                    }
                    else
                    {
                        curemonth = curmon - jmon;
                    }

                    if (cureyear > 0 || curemonth > 0)
                    {
                        collexperience = "";
                        if (cureyear > 0)
                        {
                            collexperience = " Years :" + cureyear + "";
                        }
                        if (curemonth > 0)
                        {
                            if (collexperience.Trim() != "")
                            {
                                collexperience = collexperience + " Months :" + curemonth + "";
                            }
                            else
                            {
                                collexperience = " Months :" + curemonth + "";
                            }
                        }

                    }
                }
                else
                {
                    collexperience = "-";
                }
                int totalexpyear = cureyear + expyear;
                int totalexpmonth = curemonth + exaxcm;
                string totalexperience = "";
                if (totalexpmonth > 11)
                {
                    totalexpmonth = totalexpmonth - 12;
                    totalexpyear++;
                }
                if (totalexpyear > 0 || totalexpmonth > 0)
                {
                    totalexperience = "";
                    if (totalexpyear > 0)
                    {
                        totalexperience = " Years :" + totalexpyear + "";
                    }
                    if (totalexpmonth > 0)
                    {
                        if (totalexperience.Trim() != "")
                        {
                            totalexperience = totalexperience + " Months :" + totalexpmonth + "";
                        }
                        else
                        {
                            totalexperience = " Months :" + totalexpmonth + "";
                        }
                    }

                }
                else
                {
                    totalexperience = "-";
                }
                drstaff["exp"] = totalexperience;
                dtstaff1.Rows.Add(drstaff);

                //fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 2].CellType = txt;
                //fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 2].Text = totalexperience;
            }
            GridView2.DataSource = dtstaff1;
            GridView2.DataBind();
            divspreadpopup.Visible = true;
            GridView2.Visible = true;
            btn_save.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationAlterStaff"); }
    }

    protected void btnSelectStaff_Click(object sender, EventArgs e)
    {
        try
        {
           
            string Row = Convert.ToString(Session["rowIndex"]);
            int ActiveRow = 0;
            int.TryParse(Row, out ActiveRow);

            string selectStaffName = string.Empty;
            string selectStaffCode = string.Empty;
            string selectStaffApplId = string.Empty;
            int selectCount = 0;
            foreach (GridViewRow gr in GridView2.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gr.FindControl("cbcheck");
                if (chk.Checked == true)
                {
                    selectCount++;
                }
            }
           //// for (int staffcount = 0; staffcount < fpstaff.Sheets[0].RowCount; staffcount++)
           // {
           //     int selected = 0;
           //     // int.TryParse(Convert.ToString(fpstaff.Sheets[0].Cells[staffcount, 3].Value).Trim(), out selected);

           //     if (selected == 1)
           //     {
           //         selectCount++;
           //     }
           // }
            if (selectCount > 1)
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Please Select Any One";
                divspreadpopup.Visible = true;
               // fpstaff.Visible = true;
               

            }
            else
            {
                foreach (GridViewRow gr1 in GridView2.Rows)
               // for (int row = 0; row < fpstaff.Sheets[0].RowCount; row++)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gr1.FindControl("cbcheck");
                    if (chk.Checked == true)
                   // int selected = 0;
                    //int.TryParse(Convert.ToString(fpstaff.Sheets[0].Cells[row, 3].Value).Trim(), out selected);

                    //if (selected == 1)
                    {
                        Label stafnam = (Label)gr1.FindControl("lblsatfname");
                        string staffName = stafnam.Text.Trim();
                        Label stafcode = (Label)gr1.FindControl("lblstafcode");
                        string staffCode = stafcode.Text.Trim();
                        Label stafappid = (Label)gr1.FindControl("lblapplid");
                        string staffId = stafappid.Text.Trim();

                        if (String.IsNullOrEmpty(selectStaffName))
                        {
                            selectStaffName = staffName;
                        }
                        else
                        {
                            selectStaffName += ";" + staffName;
                        }

                        if (String.IsNullOrEmpty(selectStaffCode))
                        {
                            selectStaffCode = staffCode;
                        }
                        else
                        {
                            selectStaffCode += ";" + staffCode;
                        }

                        if (String.IsNullOrEmpty(selectStaffApplId))
                        {
                            selectStaffApplId = staffId;
                        }
                        else
                        {
                            selectStaffApplId += ";" + staffId;
                        }
                    }

                }
                btngo_Click(sender, e);
                dt.Rows[ActiveRow]["AlterStaffName"] = selectStaffName;
                dt.Rows[ActiveRow]["AlterStaffcode"] = selectStaffCode;
                dt.Rows[ActiveRow]["AlterStaffapplid"] = selectStaffApplId;
               
                GridView1.DataSource = dt;
                GridView1.DataBind();
                GridView1.Columns[8].Visible = true;
               // GridView2.Rows[ActiveRow].FindControl("lblalterstaff") = selectStaffName;
                //spreadDet1.Sheets[0].Cells[activeRow, activeColumn].Tag = selectStaffCode;
                //spreadDet1.Sheets[0].Cells[activeRow, activeColumn].Note = selectStaffApplId;

                //spreadDet1.Sheets[0].Columns[8].Visible = true;
                //spreadDet1.Sheets[0].Columns[8].Width = 200;
                //spreadDet1.Sheets[0].Cells[activeRow, 8].Text = selectStaffName;
                //spreadDet1.Sheets[0].Cells[activeRow, 8].Tag = selectStaffName;
                //spreadDet1.Sheets[0].Cells[activeRow, 8].Tag = selectStaffCode;
                divAlterFreeStaffDetails.Visible = false;
                btn_save.Visible = true;
               // spreadDet1.SaveChanges();

            }
           
        }

        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationAlterStaff"); }

    }

    protected void btnFreeStaffExit_Click(object sender, EventArgs e)
    {
        divAlterFreeStaffDetails.Visible = false;
    }

    protected void btn_save_Click(Object sender, EventArgs e)
    {

        //spreadDet1.SaveChanges();
        try
        {

            bool isSave = false;
            DataTable dtstaffsave = new DataTable();
            string testname = string.Empty;
            string hallnumber = string.Empty;
            testname = Convert.ToString(cbl_date.SelectedValue);
            hallnumber = Convert.ToString(cbl_hall.SelectedValue);
            string selectStaffCode = string.Empty;
            string selectStaffApplId = string.Empty;
            string Saveqry = string.Empty;
            string DeleteQry = string.Empty;
            if (!string.IsNullOrEmpty(testname) && !string.IsNullOrEmpty(hallnumber))
            {
                foreach(GridViewRow gr in GridView1.Rows)
               // for (int row = 0; row < spreadDet1.Sheets[0].RowCount; row++)
                {
                    Label subno = (Label)gr.FindControl("lblsubno");
                    string SubjectNo = subno.Text.Trim();
                    Label alterstafcod = (Label)gr.FindControl("lblalterstafcode");
                    string AlterstaffCode = alterstafcod.Text.Trim();
                    Label criteria = (Label)gr.FindControl("lblcriteriano");
                    string CriteriaNum = criteria.Text.Trim();
                    Label stafcod = (Label)gr.FindControl("lblstaffcode");
                    string staffCode = stafcod.Text.Trim();
                    Label exmdt = (Label)gr.FindControl("lblexmdate");
                    string examdatetime = exmdt.Text.Trim();


                    string[] split = AlterstaffCode.Split(';');

                    if (split.Length > 0)
                    {
                        string stafcode = split[0];

                        for (int i = 0; i < split.Length; i++)
                        {
                            stafcode = split[i];

                            if (!string.IsNullOrEmpty(stafcode))
                            {
                                Saveqry = "if exists(select * from internalSeatingArragement where subjectNo='" + SubjectNo + "'  and staff_code='" + staffCode + "' and criteriaNo='" + CriteriaNum + "')  update internalSeatingArragement set staff_code='" + stafcode + "' where subjectNo='" + SubjectNo + "' and staff_code='" + staffCode + "' and criteriaNo='" + CriteriaNum + "' ";
                                dtstaffsave.Clear();
                                int res = dirAcc.insertData(Saveqry);
                                if (res != 0)
                                    isSave = true;
                            }

                        }
                    }

                }
                //btnGo_Click(sender, e);

            }

            if (isSave)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Saved Successfully";
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationAlterStaff"); }

    }



    #endregion

    #region alertclose
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        try
        {
            lblalerterr.Text = string.Empty;
            alertpopwindow.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationAlterStaff"); }
    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationAlterStaff"); }
    }
    #endregion
}