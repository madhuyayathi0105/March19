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

public partial class MarkMod_InvigilationReport : System.Web.UI.Page
{
    string usercode = string.Empty;
    static string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string sessstream = string.Empty;
    string staff_code = string.Empty;
    string selectQuery = "";
    string testDate;
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
    DataRow drinvig;
    DataTable dt = new DataTable();

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
        //staff_code = (string)Session["Staff_Code"];

        if (!IsPostBack)
        {
            //if (staff_code == "" || staff_code == null)
            //{
            //    Response.Write("You Are not a Valid Staff");
            //    return;
            //}
            bindhall();
            CycleTest();
            SessionBind();
            StaffBind();
            getPrintSettings2();
            showreport2.Visible = false;
             
        }
    }

    #region bindhall

    public void bindhall()
    {
        try
        {

            cbl_hall.Items.Clear();
            txt_hall.Text = "---Select---";
            ds.Clear();
            ds = d2.BindBatch();
            string Query = "select distinct e.exam_date as exam_date,i.hallNo  from  CriteriaForInternal ci,Registration r,syllabus_master sm,Exam_type e,internalSeatingArragement i where ci.syll_code=sm.syll_code and ci.Criteria_no=e.criteria_no and sm.semester=r.Current_Semester and sm.Batch_Year=r.Batch_Year and r.degree_code=sm.degree_code and  i.examCode=e.exam_code and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar'  ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hall.DataSource = ds;
                cbl_hall.DataTextField = "hallNo";
                cbl_hall.DataValueField = "hallNo";
                cbl_hall.DataBind();
                if (cbl_hall.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_hall.Items.Count; i++)
                    {
                        cbl_hall.Items[i].Selected = true;
                    }
                    txt_hall.Text = "Hall(" + cbl_hall.Items.Count + ")";
                    cb_hall.Checked = true;
                }
            }
        }
        catch { }
    }

    #endregion

    #region CycleTest

    public void CycleTest()
    {
        try
        {

            cbl_cycletest.Items.Clear();
            cb_cycletest.Checked = false;
            txt_cycletest.Text = "---Select---";
            ds1.Clear();
            ds1 = d2.BindBatch();
            string Query = "select distinct ci.criteria,ci.Criteria_no from CriteriaForInternal ci,Registration r,syllabus_master sm,Exam_type e where ci.syll_code=sm.syll_code and ci.Criteria_no=e.criteria_no  and sm.semester=r.Current_Semester and sm.Batch_Year=r.Batch_Year and r.degree_code=sm.degree_code and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar'";
            ds1.Clear();
            ds1 = d2.select_method_wo_parameter(Query, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                cbl_cycletest.DataSource = ds1;
                cbl_cycletest.DataTextField = "criteria";
                cbl_cycletest.DataValueField = "Criteria_no";
                cbl_cycletest.DataBind();
                if (cbl_cycletest.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_cycletest.Items.Count; i++)
                    {
                        cbl_cycletest.Items[i].Selected = true;
                    }
                    txt_cycletest.Text = "CycleTest(" + cbl_cycletest.Items.Count + ")";
                    cb_cycletest.Checked = true;
                }
            }
        }
        catch { }
    }

    #endregion

    #region Session
    public void SessionBind()
    {
        ddlSession.Items.Clear();
        string testName = string.Empty;
        string hallno = string.Empty;

        if (cbl_cycletest.Items.Count > 0)
            testName = Convert.ToString(getCblSelectedText(cbl_cycletest).ToUpper());

        if (cbl_hall.Items.Count > 0)
            hallno = Convert.ToString(getCblSelectedText(cbl_hall));
        DataTable dtSession = new DataTable();
        if (!string.IsNullOrEmpty(testName) && !string.IsNullOrEmpty(hallno))
        {
            string dicSession = "select distinct es.examSession from internalSeatingArragement es,CriteriaForInternal ci,Exam_type e ,class_master cs,syllabus_master sm where sm.syll_code=ci.syll_code and ci.Criteria_no=e.criteria_no and es.hallNo=cs.rno  and e.exam_date=es.examDate  and ci.criteria in('" + testName + "') and es.hallNo in('" + hallno + "')";
            //"select distinct examSession from internalSeatingArragement where examDate='" + testDate.ToString() + "'"; //and hallNo='" + ddlHallNo.SelectedItem.ToString().Trim() + "'";
            dtSession.Clear();
            dtSession = dirAcc.selectDataTable(dicSession);
        }
        if (dtSession.Rows.Count > 0)
        {
            ddlSession.DataSource = dtSession;
            ddlSession.DataTextField = "examSession";
            ddlSession.DataValueField = "examSession";
            ddlSession.DataBind();
            ddlSession.SelectedIndex = 0;
            ddlSession.Enabled = true;
        }
        else
        {
            //lblAlertMsg.Visible = true;
            //lblAlertMsg.Text = "No Session were Found";
            //divPopAlert.Visible = true;
            //return;
        }
    }

   
    #endregion

    #region Staffname
    public void StaffBind()
    {
        ddlStaff.Items.Clear();
        string testName = string.Empty;
        string hallno = string.Empty;


        if (cbl_cycletest.Items.Count > 0)
            testName = Convert.ToString(getCblSelectedText(cbl_cycletest).ToUpper());

        if (cbl_hall.Items.Count > 0)
            hallno = Convert.ToString(getCblSelectedText(cbl_hall));
        DataTable dtStaff = new DataTable();
        if (!string.IsNullOrEmpty(testName) && !string.IsNullOrEmpty(hallno))
        {
            string dicSession = "select distinct sfm.staff_name,i.staff_code from staffmaster sfm inner join staff_appl_master sa on sa.appl_no=sfm.appl_no inner join stafftrans sts on sts.staff_code=sfm.staff_code inner join hrdept_master hr on hr.dept_code=sts.dept_code inner join internalSeatingArragement i on sts.staff_code=i.staff_code inner join subject s on i.subjectNo=s.subject_no  inner join CriteriaForInternal ci on i.criteriaNo=ci.Criteria_no where sts.latestrec='1' and sfm.resign=0 and sfm.settled=0 and sfm.college_code=hr.college_code  and ci.criteria in('" + testName + "') and i.hallNo in('" + hallno + "')";
            //"select distinct examSession from internalSeatingArragement where examDate='" + testDate.ToString() + "'"; //and hallNo='" + ddlHallNo.SelectedItem.ToString().Trim() + "'";
            dtStaff.Clear();
            dtStaff = dirAcc.selectDataTable(dicSession);
        }
        if (dtStaff.Rows.Count > 0)
        {
            ddlStaff.DataSource = dtStaff;
            ddlStaff.DataTextField = "staff_name";
            ddlStaff.DataValueField = "staff_code";
            ddlStaff.DataBind();
            ddlStaff.SelectedIndex = 0;
            ddlStaff.Enabled = true;
        }
        else
        {
            //lblAlertMsg.Visible = true;
            //lblAlertMsg.Text = "No Session were Found";
            //divPopAlert.Visible = true;
            //return;
        }
    }

    #endregion

    # region loading part

    protected void cb_hall_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_hall, cbl_hall, txt_hall, "hall", "--Select--");
            showreport2.Visible = false;

        }
        catch { }

    }

    protected void cbl_hall_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_hall, cbl_hall, txt_hall, "hall", "--Select--");
            showreport2.Visible = false;
        }
        catch { }
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        showreport2.Visible = false;
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        showreport2.Visible = false;
    }

    protected void cb_cycletest_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
           
            CallCheckboxChange(cb_cycletest, cbl_cycletest, txt_cycletest, "cycletest", "--Select--");
            showreport2.Visible = false;
        }
        catch { }
    }

    protected void cbl_cycletest_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_cycletest, cbl_cycletest, txt_cycletest, "cycletest", "--Select--");
            showreport2.Visible = false;
        }
        catch { }
    }

    protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindhall();
        CycleTest();
        showreport2.Visible = false;
        print2.Visible = false;
        
    }

    protected void ddlStaff_SelectedIndexChanged(object sender, EventArgs e)
    {
     
        showreport2.Visible = false;
        print2.Visible = false;
     
    }
    # endregion

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

    protected void gridview1_onselectedindexchanged(object sender, EventArgs e)
    {
    }

    protected void gridview1_onpageindexchanged(object sender, GridViewPageEventArgs e)
    {
        gridview1.PageIndex = e.NewPageIndex;
        btnGo_Click(sender, e);
    }

    #region go
    protected void btnGo_Click(object sender, EventArgs e)
    {
        DataSet dsStaffAlter = new DataSet();
        dsStaffAlter = invigilation();
        if (dsStaffAlter.Tables.Count > 0 && dsStaffAlter.Tables[0].Rows.Count > 0)
        {
            loadspreadCount(dsStaffAlter);
        }
        else
        {
            alertpopwindow.Visible = true;
            lblalerterr.Text = "No Record Found!";
        }
    }
    #endregion

    #region fpspread2

    private DataSet invigilation()
    {
        DataSet dsloaddetails = new DataSet();
        try
        {
            #region get Value
            string CycleTestname = string.Empty;
            string hallno = string.Empty;
            string session = string.Empty;
            string staffcode = string.Empty;
            if (cbl_cycletest.Items.Count > 0)
                CycleTestname = Convert.ToString(getCblSelectedText(cbl_cycletest).ToUpper());
            if (cbl_hall.Items.Count > 0)
                hallno = Convert.ToString(getCblSelectedText(cbl_hall));
            if (ddlSession.Items.Count > 0)
            {
                session = Convert.ToString(ddlSession.SelectedValue).Trim();
            }
            if (ddlStaff.Items.Count > 0)
            {
                staffcode = Convert.ToString(ddlStaff.SelectedValue);
            }
            string fromDate = txtFromDate.Text;
            string toDate = txtToDate.Text;
            string[] frdate = fromDate.Split('/');
            if (frdate.Length == 3)
                fromDate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = toDate.Split('/');
            if (tdate.Length == 3)
                toDate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            string selQ = string.Empty;



            if (!string.IsNullOrEmpty(CycleTestname) && !string.IsNullOrEmpty(hallno) && !string.IsNullOrEmpty(session) && !string.IsNullOrEmpty(staffcode))
            {
                selQ = "select distinct CONVERT(varchar(20),i.examDate,103) examDate,i.hallNo,i.examSession,ci.criteria,sm.staff_name,i.staff_code from internalSeatingArragement i,CriteriaForInternal ci,staffmaster sm where i.criteriaNo=ci.Criteria_no and sm.staff_code=i.staff_code and ci.criteria in('" + CycleTestname + "') and  i.hallNo in('" + hallno + "') and i.staff_code='" + staffcode + "' and i.examSession='" + session + "' and i.examDate between'" + fromDate + "' and '" + toDate + "'";

                dsloaddetails.Clear();
                dsloaddetails = d2.select_method_wo_parameter(selQ, "Text");

            }

            #endregion
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationReport"); }
        return dsloaddetails;
    }

    private void loadspreadCount(DataSet ds)
    {
        try
        {
            dt.Columns.Add("SNo");
            dt.Columns.Add("Staff");
            dt.Columns.Add("Test");
            dt.Columns.Add("Date");
            dt.Columns.Add("Hall");
            dt.Columns.Add("Session");

            drinvig = dt.NewRow();
            drinvig["SNo"] = "S.No";
            drinvig["Staff"] = "Staff";
            drinvig["Test"] = "Test";
            drinvig["Date"] = "Date";
            drinvig["Hall"] = "Hall";
            drinvig["Session"] = "Session";
           

            dt.Rows.Add(drinvig);
            gridview1.Visible = true;
            int sno = 0;
            DateTime an = new DateTime();
            DateTime fn = new DateTime();
            string examtime = string.Empty;
            string Antime = string.Empty;
            string Fntime = string.Empty;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {

                    sno++;
                    drinvig = dt.NewRow();
                    string staff_name = Convert.ToString(ds.Tables[0].Rows[row]["Staff_Name"]).Trim();
                    string test = Convert.ToString(ds.Tables[0].Rows[row]["criteria"]).Trim();
                    string date = Convert.ToString(ds.Tables[0].Rows[row]["examDate"]).Trim();
                    string hall = Convert.ToString(ds.Tables[0].Rows[row]["hallNo"]).Trim();
                    string session = Convert.ToString(ds.Tables[0].Rows[row]["examSession"]).Trim();
                    an = Convert.ToDateTime("09:00:00");
                    fn = Convert.ToDateTime("12:00:00");
                    examtime = Convert.ToString(session);
                    Antime = Convert.ToString(an);
                    Fntime = Convert.ToString(fn);

                    drinvig["SNo"] = sno;
                    drinvig["Staff"] = staff_name;
                    drinvig["Test"] = test;
                    drinvig["Date"] = date;
                    drinvig["Hall"] = hall;
                   
                 
                    if ((!string.IsNullOrEmpty(examtime) && !string.IsNullOrEmpty(Antime)) || (!string.IsNullOrEmpty(examtime) && !string.IsNullOrEmpty(Fntime)))
                    {

                        string[] split = examtime.Split('-');
                        string[] split1 = Antime.Split(' ');
                        string[] split2 = Fntime.Split(' ');

                        if ((split.Length > 0 && split1.Length > 0) || (split.Length > 0 && split2.Length > 0))
                        {
                            string ExamInTime = split[0];
                            string AnTime = split1[1];
                            string FnTime = split2[1];
                            if (Convert.ToDateTime(ExamInTime) < Convert.ToDateTime(AnTime))
                            {
                             
                                drinvig["Session"] = "AN";
                            }
                            else if (Convert.ToDateTime(ExamInTime) <= Convert.ToDateTime(FnTime))
                            {
                                 drinvig["Session"] = "FN";

                            }

                        }
                    }
                    dt.Rows.Add(drinvig);

                }
                gridview1.DataSource = dt;
                gridview1.DataBind();
                gridview1.Visible = true;
                RowHead(gridview1);
                showreport2.Visible = true;
                print2.Visible = true;

            }
        }

        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationReport"); }
    }

    protected void RowHead(GridView Gridview1)
    {
        for (int head = 0; head < 1; head++)
        {
            Gridview1.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            Gridview1.Rows[head].Font.Bold = true;
            Gridview1.Rows[head].HorizontalAlign = HorizontalAlign.Center;

        }
    }

    #region Print
    protected void btnExcel_Click2(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname2.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(gridview1, reportname);
                lblvalidation3.Visible = false;
            }
            else
            {
                lblvalidation3.Text = "Please Enter Your  Report Name";
                lblvalidation3.Visible = true;
                txtexcelname2.Focus();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationReport"); }

    }

    public void btnprintmaster_Click2(object sender, EventArgs e)
    {
        try
        {
            lblvalidation3.Text = "";
            txtexcelname2.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "InvigilationReport ";
            //+ '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "InvigilationReport.aspx";
            Printcontrolhed2.loadspreaddetails(gridview1, pagename, degreedetails);
            Printcontrolhed2.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationReport"); }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void getPrintSettings2()
    {
        try
        {

            #region Excel print settings
            string usertype = "";
            if (usercode.Trim() != "")
                usertype = " and usercode='" + usercode + "'";
            else if (group_user.Trim() != "")
                usertype = " and group_code='" + group_user + "'";
            string printset = d2.GetFunction("select value from Master_Settings where settings='Excel and Pdf Print Settings' " + usertype + " ");
            if (printset != "")
            {
                if (printset.Contains("E"))
                {
                    lblrptname2.Visible = true;
                    txtexcelname2.Visible = true;
                    btnExcel2.Visible = true;
                }
                if (printset.Contains("P"))
                {
                    btnprintmasterhed2.Visible = true;
                }
                if (printset == "0")
                {
                    lblrptname2.Visible = true;
                    txtexcelname2.Visible = true;
                    btnExcel2.Visible = true;
                    btnprintmasterhed2.Visible = true;

                }
            }
            #endregion
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationReport"); }
    }

    #endregion

    #endregion


    #region alertclose
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        try
        {
            lblalerterr.Text = string.Empty;
            alertpopwindow.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationReport"); }
    }

   
    #endregion
}