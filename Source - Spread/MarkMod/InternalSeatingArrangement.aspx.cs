using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gios.Pdf;
using InsproDataAccess;
using wc = System.Web.UI.WebControls;


public partial class MarkMod_InternalSeatingArrangement : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    DAccess2 dt = new DAccess2();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    Hashtable ht = new Hashtable();
    DataTable dtCommon = new DataTable();
    ReuasableMethods rs = new ReuasableMethods();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    string collegeCode = string.Empty;
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    string build = "", buildvalue = string.Empty;
    string qryCollege = string.Empty;
    string qry = string.Empty;
    string qryBatch = string.Empty;
    string testDate;
    string[] arrang;
    string[] arran;
    string norow = string.Empty;
    string nocol = string.Empty;
    string allotseat = string.Empty;
    string[] spcel;
    int hss = 0;
    int a = 0;

    DataTable dtfor2 = new DataTable();
    DataRow drfor2;
    DataTable dtfor1 = new DataTable();
    DataRow drfor1;
    DataTable dtformat1 = new DataTable();
    DataRow drformat1;
    DataTable dtform1 = new DataTable();
    DataRow drform1;

    Dictionary<int, string> dicformat1 = new Dictionary<int, string>();

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

            userCollegeCode = Convert.ToString(Session["collegecode"]).Trim();
            userCode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
            singleUser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
            groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";


            if (!IsPostBack)
            {
                divFormat1.Visible = false;
                divFormat2.Visible = false;
                chkReport.Checked = false;
                Multiple.Visible = false;
                Single.Visible = false;
                btnMissingStudent.Visible = false;
                Radioformat1.Checked = true;
                Radioformat2.Checked = true;
                Bindcollege();
                BindRightsBaseBatch();
                binddegree();
                bindbranch();
                bindTest();
                bindTestDate();
                //SessionBind();
                //hallBind();
                btnGo.Visible = false;
                lblHall.Visible = false;
                ddlHallNo.Visible = false;
                lblTestSession.Visible = false;
                cblhall.Visible = false;
                chkhall.Visible = false;
                txthall.Visible = false;
                // Multiple.Checked = false;
                //Single.Checked=false;
                ddlSession.Visible = false;
                btn_directprint.Visible = false;
                pnlhall.Visible = false;
            }

        }
        catch (Exception ex)
        {
        }
    }

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

    public void BindRightsBaseBatch()
    {
        try
        {
            DataSet dsBatch = new DataSet();
            userCode = string.Empty;
            groupUserCode = string.Empty;
            qryUserOrGroupCode = string.Empty;
            collegeCode = string.Empty;
            ds.Clear();
            chkBatch.Checked = false;
            cblBatch.Items.Clear();
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string group = Convert.ToString(Session["group_code"]).Trim();
                if (group.Contains(";"))
                {
                    string[] group_semi = group.Split(';');
                    groupUserCode = Convert.ToString(group_semi[0]);
                }
                if (!string.IsNullOrEmpty(groupUserCode))
                {
                    qryUserOrGroupCode = " and user_id='" + groupUserCode + "'";
                }
            }
            else if (Session["usercode"] != null)
            {
                userCode = Convert.ToString(Session["usercode"]).Trim();
                if (!string.IsNullOrEmpty(userCode))
                {
                    qryUserOrGroupCode = " and user_id='" + userCode + "'";
                }
            }
            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollege = " and r.college_code in(" + collegeCode + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCode))
            {
                qryCollege = " and r.college_code in(" + collegeCode + ")";
            }

            dsBatch.Clear();
            if (!string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                string qry = "select distinct batch_year from tbl_attendance_rights r where batch_year<>'' " + qryUserOrGroupCode + " order by batch_year desc";
                dsBatch = da.select_method_wo_parameter(qry, "Text");
            }
            qryBatch = string.Empty;
            if (dsBatch.Tables.Count > 0 && dsBatch.Tables[0].Rows.Count > 0)
            {
                List<int> lstBatch = dsBatch.Tables[0].AsEnumerable().Select(r => r.Field<int>("batch_year")).ToList();
                if (lstBatch.Count > 0)
                    qryBatch = " and r.Batch_Year in('" + string.Join("','", lstBatch.ToArray()) + "')";
            }
            string batchquery = string.Empty;
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryCollege))
            {
                batchquery = "select distinct r.Batch_Year from Registration r,Course c,Degree dg,Department dt where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=r.college_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and r.degree_code=dg.Degree_Code  and r.Batch_Year<>'0' and r.Batch_Year<>-1 and r.cc='0' and delflag='0' and exam_flag<>'debar' " + qryCollege + qryBatch + " order by r.Batch_Year desc";
                //ds.Clear();
                ds = da.select_method_wo_parameter(batchquery, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cblBatch.DataSource = ds;
                    cblBatch.DataTextField = "Batch_Year";
                    cblBatch.DataValueField = "Batch_Year";
                    cblBatch.DataBind();

                    checkBoxListselectOrDeselect(cblBatch, true);
                    CallCheckboxListChange(chkBatch, cblBatch, txtBatch, lblBatch.Text, "--Select--");
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void binddegree()
    {
        try
        {
            ds.Clear();
            txtDegree.Text = "---Select---";
            string batchCode = string.Empty;
            chkDegree.Checked = false;
            cblDegree.Items.Clear();
            //userCode = Session["usercode"].ToString();
            //singleUser = Session["single_user"].ToString();
            //groupUserCode = Session["group_code"].ToString();
            collegeCode = string.Empty;
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
            if (cblBatch.Items.Count > 0)
                valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch))
            {
                string selDegree = "SELECT DISTINCT c.course_id,c.course_name,c.Priority,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "') AND r.Batch_Year in('" + valBatch + "') " + columnfield + " ORDER BY CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";
                ds = da.select_method_wo_parameter(selDegree, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblDegree.DataSource = ds;
                cblDegree.DataTextField = "course_name";
                cblDegree.DataValueField = "course_id";
                cblDegree.DataBind();
                checkBoxListselectOrDeselect(cblDegree, true);
                CallCheckboxListChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void bindbranch()
    {
        try
        {
            string degreecode = string.Empty;
            //collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            txtBranch.Text = "---Select---";
            chkBranch.Checked = false;
            cblBranch.Items.Clear();
            ds.Clear();
            collegeCode = string.Empty;
            if (ddlCollege.Items.Count > 0)
                collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            string selBranch = string.Empty;
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
            //string valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
            //string valDegree = rs.GetSelectedItemsValueAsString(cblDegree);

            string valBatch = string.Empty;// rs.GetSelectedItemsValueAsString(cblBatch);
            string valDegree = string.Empty;//rs.GetSelectedItemsValueAsString(cblBranch);
            if (cblBatch.Items.Count > 0)
                valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
            if (cblDegree.Items.Count > 0)
                valDegree = rs.GetSelectedItemsValueAsString(cblDegree);

            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree))
            {
                selBranch = "SELECT DISTINCT dg.Degree_Code,dt.Dept_Name,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "') AND r.Batch_Year in('" + valBatch + "') AND c.Course_Id in('" + valDegree + "') " + columnfield + " ORDER BY dg.Degree_Code, CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";
                ds = da.select_method_wo_parameter(selBranch, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblBranch.DataSource = ds;
                cblBranch.DataTextField = "dept_name";
                cblBranch.DataValueField = "degree_code";
                cblBranch.DataBind();
                checkBoxListselectOrDeselect(cblBranch, true);
                CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            }

        }
        catch (Exception ex)
        {

        }
    }

    public void bindTest()
    {
        ddlTest.Items.Clear();
        string valBatch = string.Empty; //rs.GetSelectedItemsValueAsString(cblBatch);
        string valDegree = string.Empty; //rs.GetSelectedItemsValueAsString(cblBranch);
        if (cblBatch.Items.Count > 0)
            valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
        if (cblBranch.Items.Count > 0)
            valDegree = rs.GetSelectedItemsValueAsString(cblBranch);
        string selTest = string.Empty;

        if (!string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree))
        {
            selTest = "select distinct ci.criteria from CriteriaForInternal ci,Registration r,syllabus_master sm,Exam_type e where ci.syll_code=sm.syll_code and ci.Criteria_no=e.criteria_no  and sm.semester=r.Current_Semester and sm.Batch_Year=r.Batch_Year and r.degree_code=sm.degree_code and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and sm.Batch_Year in ('" + valBatch + "') and sm.degree_code in ('" + valDegree + "') order by ci.criteria";
            ds = da.select_method_wo_parameter(selTest, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlTest.DataSource = ds;
                ddlTest.DataTextField = "criteria";
                ddlTest.DataValueField = "criteria";
                ddlTest.DataBind();
                ddlTest.SelectedIndex = 0;
                ddlTest.Enabled = true;
            }

        }
        else
        {
            //lblErrmsg.Visible = true;
            //lblErrmsg.Text = "Invalid to select";
        }
    }

    public void bindTestDate()
    {
        ddlTestDate.Items.Clear();
        string valBatch = string.Empty;// rs.GetSelectedItemsValueAsString(cblBatch);
        string valDegree = string.Empty;//rs.GetSelectedItemsValueAsString(cblBranch);
        string strTestName = string.Empty;// Convert.ToString(ddlTest.SelectedItem.Text.Trim());
        if (cblBatch.Items.Count > 0)
            valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
        if (cblBranch.Items.Count > 0)
            valDegree = rs.GetSelectedItemsValueAsString(cblBranch);
        if (ddlTest.Items.Count > 0)
            strTestName = Convert.ToString(ddlTest.SelectedItem.Text.Trim());

        if (!string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree) && !string.IsNullOrEmpty(strTestName))
        {
            string selTest = string.Empty;
            selTest = "select distinct e.exam_date as exam_date,CONVERT(varchar(20),e.exam_date,103) as examDate,DATEPART(year,exam_date) Year from  CriteriaForInternal ci,Registration r,syllabus_master sm,Exam_type e where ci.syll_code=sm.syll_code and ci.Criteria_no=e.criteria_no and sm.semester=r.Current_Semester and sm.Batch_Year=r.Batch_Year and r.degree_code=sm.degree_code and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and sm.Batch_Year in('" + valBatch + "') and sm.degree_code in('" + valDegree + "') and ci.criteria in('" + strTestName + "') order by Year desc, e.exam_date";
            ds = da.select_method_wo_parameter(selTest, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlTestDate.DataSource = ds;
                ddlTestDate.DataTextField = "examDate";
                ddlTestDate.DataValueField = "exam_date";
                ddlTestDate.DataBind();
                ddlTestDate.SelectedIndex = 0;
                ddlTestDate.Enabled = true;
            }
        }
        else
        {
            //lblErrmsg.Visible = true;
            //lblErrmsg.Text = "Invalid to select";
        }
    }

    public void hallBind()
    {
        if (Multiple.Checked == true)
        {
            string testName1 = string.Empty;
            string qrytestName1 = string.Empty;
            testDate = string.Empty;
            if (ddlTestDate.Items.Count > 0)
            {
                string ldate1 = ddlTestDate.SelectedItem.ToString();
                if (ldate1.Trim() != "")
                {
                    string[] spl = ldate1.Split('/');
                    DateTime dtl = Convert.ToDateTime(spl[1] + '/' + spl[0] + '/' + spl[2]);
                    testDate = dtl.ToString("yyyy-MM-dd");
                }
            }
            else
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Select Exam Date";
                divPopAlert.Visible = true;
                return;
            }
            if (ddlSession.Items.Count > 0)
            {

            }
            else
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Select Exam Sessions";
                divPopAlert.Visible = true;
                return;
            }
            string session1 = string.Empty;// ddlSession.SelectedItem.ToString().Trim();
            if (ddlSession.Items.Count > 0)
                session1 = ddlSession.SelectedItem.ToString().Trim();
            if (ddlTest.Items.Count > 0)
            {
                testName1 = Convert.ToString(ddlTest.SelectedItem.Text).Trim();
            }
            DataTable dtHallNo1 = new DataTable();
            if (!string.IsNullOrEmpty(testName1) && !string.IsNullOrEmpty(testDate) && !string.IsNullOrEmpty(session1))
            {
                //string dichallno = "select distinct hallNo from internalSeatingArragement where examDate='" + testDate.ToString() + "' and examSession='" + session + "'";
                string dichallno1 = "  select distinct cs.rno hallNo ,cs.block,cs.priority from internalSeatingArragement es,CriteriaForInternal ci,Exam_type e ,class_master cs,syllabus_master sm where sm.syll_code=ci.syll_code and ci.Criteria_no=e.criteria_no and es.hallNo=cs.rno  and es.examDate='" + testDate + "' and e.exam_date=es.examDate and es.examSession='" + session1 + "' and ci.criteria='" + testName1 + "' order by cs.priority ";
                dtHallNo1 = dirAcc.selectDataTable(dichallno1);
            }
            if (dtHallNo1.Rows.Count > 0)
            {
                cblhall.DataSource = dtHallNo1;
                cblhall.DataTextField = "hallNo";
                cblhall.DataValueField = "hallNo";
                cblhall.DataBind();
                cblhall.SelectedIndex = 0;
                cblhall.Enabled = true;
            }
        }
        else
        {
            ddlHallNo.Items.Clear();
            string testName = string.Empty;
            string qrytestName = string.Empty;
            testDate = string.Empty;
            if (ddlTestDate.Items.Count > 0)
            {
                string ldate = ddlTestDate.SelectedItem.ToString();
                if (ldate.Trim() != "")
                {
                    string[] spl = ldate.Split('/');
                    DateTime dtl = Convert.ToDateTime(spl[1] + '/' + spl[0] + '/' + spl[2]);
                    testDate = dtl.ToString("yyyy-MM-dd");
                }
            }
            else
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Select Exam Date";
                divPopAlert.Visible = true;
                return;
            }
            if (ddlSession.Items.Count > 0)
            {

            }
            else
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Select Exam Sessions";
                divPopAlert.Visible = true;
                return;
            }

            string session = string.Empty;// ddlSession.SelectedItem.ToString().Trim();
            if (ddlSession.Items.Count > 0)
                session = ddlSession.SelectedItem.ToString().Trim();
            if (ddlTest.Items.Count > 0)
            {
                testName = Convert.ToString(ddlTest.SelectedItem.Text).Trim();
            }
            DataTable dtHallNo = new DataTable();
            if (!string.IsNullOrEmpty(testName) && !string.IsNullOrEmpty(testDate) && !string.IsNullOrEmpty(session))
            {
                //string dichallno = "select distinct hallNo from internalSeatingArragement where examDate='" + testDate.ToString() + "' and examSession='" + session + "'";
                string dichallno = "  select distinct cs.rno hallNo ,cs.block,cs.priority from internalSeatingArragement es,CriteriaForInternal ci,Exam_type e ,class_master cs,syllabus_master sm where sm.syll_code=ci.syll_code and ci.Criteria_no=e.criteria_no and es.hallNo=cs.rno  and es.examDate='" + testDate + "' and e.exam_date=es.examDate and es.examSession='" + session + "' and ci.criteria='" + testName + "' order by cs.priority ";
                dtHallNo = dirAcc.selectDataTable(dichallno);
            }
            if (dtHallNo.Rows.Count > 0)
            {
                ddlHallNo.DataSource = dtHallNo;
                ddlHallNo.DataTextField = "hallNo";
                ddlHallNo.DataValueField = "hallNo";
                ddlHallNo.DataBind();
                ddlHallNo.SelectedIndex = 0;
                ddlHallNo.Enabled = true;
            }
        }

    }

    public void SessionBind()
    {
        ddlSession.Items.Clear();
        string testName = string.Empty;
        string qrytestName = string.Empty;
        if (ddlTestDate.Items.Count > 0)
        {
            string ldate = ddlTestDate.SelectedItem.ToString();
            if (ldate.Trim() != "")
            {
                string[] spl = ldate.Split('/');
                DateTime dtl = Convert.ToDateTime(spl[1] + '/' + spl[0] + '/' + spl[2]);
                testDate = dtl.ToString("yyyy-MM-dd");
            }
        }
        else
        {
            lblAlertMsg.Visible = true;
            lblAlertMsg.Text = "Select Exam Date";
            divPopAlert.Visible = true;
            return;
        }
        if (ddlTest.Items.Count > 0)
        {
            testName = Convert.ToString(ddlTest.SelectedItem.Text).Trim();
        }
        DataTable dtSession = new DataTable();
        if (!string.IsNullOrEmpty(testName) && !string.IsNullOrEmpty(testDate))
        {
            string dicSession = "select distinct es.examSession from internalSeatingArragement es,CriteriaForInternal ci,Exam_type e ,class_master cs,syllabus_master sm where sm.syll_code=ci.syll_code and ci.Criteria_no=e.criteria_no and es.hallNo=cs.rno  and e.exam_date=es.examDate and es.examDate='" + testDate.ToString() + "' and ci.criteria='" + testName + "'";
            //"select distinct examSession from internalSeatingArragement where examDate='" + testDate.ToString() + "'"; //and hallNo='" + ddlHallNo.SelectedItem.ToString().Trim() + "'";
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
            lblAlertMsg.Visible = true;
            lblAlertMsg.Text = "No Session were Found";
            divPopAlert.Visible = true;
            return;
        }
    }

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            divFormat1.Visible = false;
            divFormat2.Visible = false;
            chkReport.Checked = false;
            BindRightsBaseBatch();
            binddegree();
            bindbranch();
            bindTest();
            bindTestDate();
        }
        catch (Exception ex)
        {
        }
    }

    protected void chkBatch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            divFormat1.Visible = false;
            divFormat2.Visible = false;
            chkReport.Checked = false;
            CallCheckboxChange(chkBatch, cblBatch, txtBatch, lblBatch.Text, "--Select--");
            binddegree();
            bindbranch();
            bindTest();
            bindTestDate();
        }
        catch (Exception ex)
        {
        }
    }

    protected void cblBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            divFormat1.Visible = false;
            divFormat2.Visible = false;
            chkReport.Checked = false;
            CallCheckboxListChange(chkBatch, cblBatch, txtBatch, lblBatch.Text, "--Select--");
            binddegree();
            bindbranch();
            bindTest();
            bindTestDate();

        }
        catch (Exception ex)
        {
        }
    }

    protected void chkDegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            divFormat1.Visible = false;
            divFormat2.Visible = false;
            chkReport.Checked = false;
            CallCheckboxChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
            bindbranch();
            bindTest();
            bindTestDate();
        }
        catch (Exception ex)
        {
        }
    }

    protected void cblDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            divFormat1.Visible = false;
            divFormat2.Visible = false;
            chkReport.Checked = false;
            CallCheckboxListChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
            bindbranch();
            bindTest();
            bindTestDate();
        }
        catch (Exception ex)
        {
        }
    }

    protected void chkBranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            divFormat1.Visible = false;
            divFormat2.Visible = false;
            chkReport.Checked = false;
            CallCheckboxChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            bindTest();
            bindTestDate();
        }
        catch (Exception ex)
        {
        }
    }

    protected void cblBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            divFormat1.Visible = false;
            divFormat2.Visible = false;
            chkReport.Checked = false;
            CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            bindTest();
            bindTestDate();
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlTest_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            divFormat1.Visible = false;
            divFormat2.Visible = false;
            chkReport.Checked = false;
            bindTestDate();
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlTestDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            divFormat1.Visible = false;
            divFormat2.Visible = false;
            chkReport.Checked = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        divFormat1.Visible = false;
        divFormat2.Visible = false;
        hallBind();
    }

    protected void ddlHall_SelectedIndexChanged(object sender, EventArgs e)
    {
        divFormat1.Visible = false;
        divFormat2.Visible = false;
        GridView2.Visible = false;
        // Fpspread.Visible = false;
        btn_directprint.Visible = false;
        //SessionBind();
    }

    protected void chkReport_CheckedChanged(object sender, EventArgs e)
    {
        divFormat1.Visible = false;
        divFormat2.Visible = false;
        if (chkReport.Checked == true)
        {
            Single.Visible = true;
            Multiple.Visible = true;
            btnGo.Visible = false;
            lblHall.Visible = false;
            ddlHallNo.Visible = false;
            lblTestSession.Visible = false;
            //ddlHallNo.Enabled = false;
            ddlSession.Visible = false;
            SessionBind();
            hallBind();
            //btnMissingStudent.Visible = true;
            Radioformat1.Checked = true;
            ////Fpspread.Visible = true;
            //chkmultihall1.Visible = true;
            ////pnlhall.Visible = true;
            //// txthall.Visible = true;
            ////cblhall.Visible = true;
            //// chkhall.Visible = true;


        }
        else
        {
            Single.Visible = false;
            Multiple.Visible = false;
            btnGo.Visible = false;
            lblHall.Visible = false;
            ddlHallNo.Visible = false;
            lblTestSession.Visible = false;
            ddlSession.Visible = false;
            btnGenerate.Enabled = true;
            GridView2.Visible = false;
            //Fpspread.Visible = false;
            btnMissingStudent.Visible = false;
            btn_directprint.Visible = false;
            Radioformat1.Checked = false;
            txthall.Visible = false;
            chkhall.Visible = false;
            cblhall.Visible = false;
            pnlhall.Visible = false;

        }
    }
    protected void Single_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Single.Checked == true)
            {
                btnGo.Visible = true;
                lblHall.Visible = true;
                ddlHallNo.Visible = true;
                lblTestSession.Visible = true;
                hallBind();
                SessionBind();
                //ddlHallNo.Enabled = false;
                ddlSession.Visible = true;
                txthall.Visible = false;
                chkhall.Visible = false;
                cblhall.Visible = false;
                pnlhall.Visible = false;
            }
            else
            {
                lblHall.Visible = false;
                ddlHallNo.Visible = false;
                lblTestSession.Visible = false;
                //ddlHallNo.Enabled = false;
                ddlSession.Visible = false;
            }

        }
        catch
        {
        }
    }
    protected void Multiple_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Multiple.Checked == true)
            {
                btnGo.Visible = true;
                hallBind();
                SessionBind();
                txthall.Visible = true;
                chkhall.Visible = true;
                cblhall.Visible = true;
                lblHall.Visible = false;
                ddlHallNo.Visible = false;
                lblTestSession.Visible = true;
                //ddlHallNo.Enabled = false;
                ddlSession.Visible = true;
                pnlhall.Visible = true;
            }
            else
            {
                txthall.Visible = false;
                chkhall.Visible = false;
                cblhall.Visible = false;
                pnlhall.Visible = false;
            }
        }
        catch { }
    }
    //protected void chkmultihall1_CheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (chkmultihall1.Checked == true)
    //        {
    //            txthall.Visible = true;
    //            ddlHallNo.Enabled = false;
    //            cblhall.Visible = true;
    //            chkhall.Visible = true;
    //            hallBind();
    //            pnlhall.Visible = true;
    //        }
    //        else
    //        {
    //            txthall.Visible = false;
    //            cblhall.Visible = false;
    //            chkhall.Visible = false;
    //            ddlHallNo.Enabled = true;
    //            pnlhall.Visible = false;
    //        }
    //    }
    //    catch
    //    {
    //    }

    //}
    protected void chkhall_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkhall.Checked == true)
            {
                for (int i = 0; i < cblhall.Items.Count; i++)
                {
                    cblhall.Items[i].Selected = true;
                }
                txthall.Text = "hall(" + (cblhall.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cblhall.Items.Count; i++)
                {
                    cblhall.Items[i].Selected = false;
                }
                txthall.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void cblhall_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txthall.Text = "--Select--";
            chkhall.Checked = false;
            for (int i = 0; i < cblhall.Items.Count; i++)
            {
                if (cblhall.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txthall.Text = "hall(" + commcount.ToString() + ")";
                if (commcount == cblhall.Items.Count)
                {
                    chkhall.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    /*protected void chkhall_CheckedChanged(object sender, EventArgs e)
     {
         try
         {
             divFormat1.Visible = false;
             divFormat2.Visible = false;
             hallBind();
         }
         catch
         {

         }
     }
     protected void cblhall_SelectedIndexChanged(object sender, EventArgs e)
     {
         try
         {
             divFormat1.Visible = false;
             divFormat2.Visible = false;
             //hallBind();
         }
         catch
         {
         }
     }*/
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        divFormat1.Visible = false;
        divFormat2.Visible = false;
        string valBatch = string.Empty;
        string valDegree = string.Empty;
        string valBranch = string.Empty;
        //string valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
        string colllegeCode = string.Empty;
        string testName = string.Empty;
        //List<string> liSubjectNo;
        List<string> liSession;
        List<string> liExamDate;
        int totAllotedStudents = 0;
        int totAllotedStudentsNew = 0;
        int totActualStudents = 0;
        int totActualStudentsNew = 0;
        int spcn = 0;
        int OddIndex = 0;
        int EvenIndex = 1;
        bool Evenflag = false;
        bool Oddflag = false;
        bool isGenerated = false;
        try
        {
            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            }
            else
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No " + lblCollege.Text + " Found";
                divPopAlert.Visible = true;
                return;
            }
            if (cblBatch.Items.Count == 0)
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No " + lblBatch.Text + " Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
                if (string.IsNullOrEmpty(valBatch))
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Select Atleast One " + lblBatch.Text + "";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            if (cblDegree.Items.Count == 0)
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No " + lblDegree.Text + " Found";
                divPopAlert.Visible = true;
                return;
            }

            else
            {
                valDegree = rs.GetSelectedItemsValueAsString(cblDegree);
                if (string.IsNullOrEmpty(valDegree))
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Select Atleast One " + lblDegree.Text + "";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            if (cblBranch.Items.Count == 0)
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No " + lblBranch.Text + " Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                valBranch = rs.GetSelectedItemsValueAsString(cblBranch);
                if (string.IsNullOrEmpty(valBranch))
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Select Atleast One " + lblBranch.Text + "";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            if (ddlTest.Items.Count > 0)
            {
                testName = ddlTest.SelectedItem.ToString().Trim();
            }
            else
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No " + lblTest.Text + " Found";
                divPopAlert.Visible = true;
                return;
            }
            if (ddlTestDate.Items.Count > 0)
            {
                string ldate = ddlTestDate.SelectedItem.ToString();
                if (ldate.Trim() != "")
                {
                    string[] spl = ldate.Split('/');
                    DateTime dtl = Convert.ToDateTime(spl[1] + '/' + spl[0] + '/' + spl[2]);
                    testDate = dtl.ToString("yyyy-MM-dd");
                }
            }
            else
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No " + lblTestDate.Text + " Found";
                divPopAlert.Visible = true;
                return;
            }

            if (!string.IsNullOrEmpty(collegeCode))
            {
                DataTable dtRoomSeatingsArrange = new DataTable();
                DataTable dtHallPriority = new DataTable();
                DataTable dtStudentInfo = new DataTable();
                DataTable dtHallDefinition = new DataTable();
                DataTable dtTestSession = new DataTable();
                DataTable dtSubjectNo = new DataTable();
                DataTable dtSubCodeTotalStudent = new DataTable();
                DataTable dtExamDate = new DataTable();

                string room = "select * from tbl_room_seats where coll_code in (" + collegeCode + ") ";
                dtRoomSeatingsArrange = dirAcc.selectDataTable(room);
                if (dtRoomSeatingsArrange.Rows.Count == 0)
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "No Hall Definition Found";
                    divPopAlert.Visible = true;
                    return;
                }
                //magesh 13/2/18
                // string Priority = "select * from class_master where coll_code in (" + collegeCode + ")  order by priority";
                string Priority = "select *  from class_master where priority is not null  and coll_code in (" + collegeCode + ")  order by priority";
                dtHallPriority = dirAcc.selectDataTable(Priority);
                if (dtHallPriority.Rows.Count == 0)
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "No Hall Were Found";
                    divPopAlert.Visible = true;
                    return;
                }

                string studentDetails = "select distinct r.Roll_No,r.Reg_No,r.App_No,r.Roll_Admit,r.Stud_Name,r.degree_code,r.Batch_Year,LTRIM(RTRIM(ISNULL(r.sections,''))) as Sections,ci.criteria,e.exam_code,e.criteria_no,e.exam_date,s.subject_code,s.subject_no,e.examFromTime as fromTime,e.examToTime as toTime,CONVERT(varchar(5),e.examFromTime,108)+'-'+CONVERT(varchar(5),e.examToTime,108) as examSession,CONVERT(varchar(20),e.exam_date,101) as examDate from Registration r,Exam_type e,CriteriaForInternal ci,syllabus_master sm,subject s,subjectChooser sc where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and sc.subject_no=e.subject_no and e.subject_no=s.subject_no and s.syll_code=sm.syll_code and s.syll_code=ci.syll_code and sm.syll_code=ci.syll_code and sm.Batch_Year=r.Batch_Year and sm.degree_code=r.degree_code and r.Current_Semester=sm.semester and e.criteria_no=ci.Criteria_no and LTRIM(RTRIM(ISNULL(e.sections,'')))=LTRIM(RTRIM(ISNULL(r.sections,''))) and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and r.ProlongAbsent='0' and r.college_code='" + collegeCode + "' and ci.criteria='" + testName + "' and e.exam_date='" + testDate.ToString() + "'  order by e.exam_date,examSession,r.degree_code,r.Batch_Year,sections,r.Reg_No"; // and r.Batch_Year in ('" + valBatch + "') and r.degree_code in ('" + valBranch + "')

                DataSet dsStudentDetails = da.select_method_wo_parameter(studentDetails, "text");
                DataTable dtAllStud = new DataTable();
                if (dsStudentDetails.Tables[0].Rows.Count > 0)
                {
                    dtAllStud = dsStudentDetails.Tables[0].DefaultView.ToTable();
                }
                if (dtAllStud.Rows.Count > 0)
                {
                    dtAllStud.DefaultView.RowFilter = "Batch_Year in ('" + valBatch + "') and degree_code in ('" + valBranch + "') ";
                    dtAllStud.DefaultView.Sort = "exam_date,examSession,degree_code,Batch_Year,sections,Reg_No";
                    //e.exam_date,examSession,r.degree_code,r.Batch_Year,sections,r.Reg_No
                    dtStudentInfo = dtAllStud.DefaultView.ToTable();
                }


                if (dtStudentInfo.Rows.Count == 0)
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "No Student Were Found";
                    divPopAlert.Visible = true;
                    return;
                }

                string examDate = "select distinct e.exam_date as exam_date,CONVERT(varchar(20),e.exam_date,101) as examDate from CriteriaForInternal ci,Registration r,syllabus_master sm,Exam_type e where ci.syll_code=sm.syll_code and ci.Criteria_no=e.criteria_no and r.Current_Semester=sm.semester and sm.Batch_Year=r.Batch_Year and sm.degree_code=r.degree_code and sm.Batch_Year in('" + valBatch + "') and sm.degree_code in('" + valBranch + "') and ci.criteria in('" + testName + "') and e.exam_date='" + testDate.ToString() + "' order by  e.exam_date asc";
                dtExamDate = dirAcc.selectDataTable(examDate);
                if (dtExamDate.Rows.Count == 0)
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "No Exam Date were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                else
                {
                    liExamDate = new List<string>(dtExamDate.Rows.Count);
                    foreach (DataRow row in dtExamDate.Rows)
                        liExamDate.Add(Convert.ToString(row["examDate"]));
                }

                string testSession = "select distinct e.examFromTime as fromTime,e.examToTime as toTime,CONVERT(varchar(5),e.examFromTime,108)+'-'+CONVERT(varchar(5),e.examToTime,108) as examSession from CriteriaForInternal ci,Registration r,syllabus_master sm,Exam_type e where ci.syll_code=sm.syll_code and ci.Criteria_no=e.criteria_no and r.Current_Semester=sm.semester and sm.Batch_Year=r.Batch_Year and sm.degree_code=r.degree_code and sm.Batch_Year in('" + valBatch + "') and sm.degree_code in('" + valBranch + "') and ci.criteria in('" + testName + "') and e.exam_date='" + testDate.ToString() + "' and CONVERT(varchar(5),e.examFromTime,108)+'-'+CONVERT(varchar(5),e.examToTime,108)<>'' order by  examSession";// --and LTRIM(RTRIM(ISNULL(r.Sections,'')))=LTRIM(RTRIM(ISNULL(e.sections,'')))
                dtTestSession = dirAcc.selectDataTable(testSession);
                if (dtTestSession.Rows.Count == 0)
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "No Session Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                else
                {
                    liSession = new List<string>(dtTestSession.Rows.Count);
                    foreach (DataRow row in dtTestSession.Rows)
                        liSession.Add(Convert.ToString(row["examSession"]));
                }

                //string subjectNo = "select distinct s.subject_no from CriteriaForInternal ci,Registration r,syllabus_master sm,Exam_type e,subject s where ci.syll_code=sm.syll_code and ci.Criteria_no=e.criteria_no and r.Current_Semester=sm.semester and sm.Batch_Year=r.Batch_Year and s.syll_code=sm.syll_code and s.syll_code=ci.syll_code and s.subject_no=e.subject_no and sm.degree_code=r.degree_code and sm.Batch_Year in('" + valBatch + "') and sm.degree_code in('" + valBranch + "') and ci.criteria in('" + testName + "') and e.exam_date='" + testDate.ToString() + "' order by  s.subject_no";//and LTRIM(RTRIM(ISNULL(r.Sections,'')))=LTRIM(RTRIM(ISNULL(e.sections,'')))
                //dtSubjectNo = dirAcc.selectDataTable(subjectNo);
                //if (dtSubjectNo.Rows.Count == 0)
                //{
                //    lblAlertMsg.Visible = true;
                //    lblAlertMsg.Text = "No SubjectNo were Found";
                //    divPopAlert.Visible = true;
                //    return;
                //}
                //else
                //{
                //    liSubjectNo = new List<string>(dtSubjectNo.Rows.Count);
                //    foreach (DataRow row in dtSubjectNo.Rows)
                //        liSubjectNo.Add(Convert.ToString(row["subject_no"]));
                //    //liSubjectNo.Add((string)row["subject_no"]);
                //}
                //string subjectCodeTotalStudent = "select distinct s.subject_code,ci.criteria,e.exam_date,e.examFromTime as fromTime,e.examToTime as toTime,CONVERT(varchar(20),e.examFromTime,108)+'-'+CONVERT(varchar(20),e.examToTime,108) as examSession,Count(distinct sc.roll_no) as TotalStudent from CriteriaForInternal ci,Registration r,syllabus_master sm,Exam_type e,subject s,subjectChooser sc where ci.syll_code=sm.syll_code and ci.Criteria_no=e.criteria_no and r.Current_Semester=sm.semester and sm.Batch_Year=r.Batch_Year and s.syll_code=sm.syll_code and s.syll_code=ci.syll_code and s.subject_no=e.subject_no and sm.degree_code=r.degree_code and sc.subject_no=e.subject_no and sc.subject_no=s.subject_no and sc.roll_no=r.Roll_No and sm.Batch_Year in('" + valBatch + "') and sm.degree_code in('" + valBranch + "') and e.exam_date='" + testDate.ToString() + "'  and ci.criteria in('" + testName + "')  group by s.subject_code,ci.criteria,e.exam_date,e.examFromTime,e.examToTime order by  TotalStudent desc";//---and LTRIM(RTRIM(ISNULL(r.Sections,'')))=LTRIM(RTRIM(ISNULL(e.sections,'')))

                string subjectCodeTotalStudent = "select distinct r.Batch_Year,r.degree_code,s.subject_no,s.subject_code,Count(distinct sc.roll_no) as TotalStudent,ci.criteria,e.examFromTime as fromTime,e.examToTime as toTime,CONVERT(varchar(5),e.examFromTime,108)+'-'+CONVERT(varchar(5),e.examToTime,108) as examSession,e.exam_date as exam_date,CONVERT(varchar(20),e.exam_date,101) as examDate,e.exam_code,e.criteria_no from CriteriaForInternal ci,Registration r,syllabus_master sm,Exam_type e,subject s,subjectChooser sc where ci.syll_code=sm.syll_code and ci.Criteria_no=e.criteria_no and r.Current_Semester=sm.semester and sm.Batch_Year=r.Batch_Year and s.syll_code=sm.syll_code and s.syll_code=ci.syll_code and s.subject_no=e.subject_no and sm.degree_code=r.degree_code and sc.subject_no=e.subject_no and sc.subject_no=s.subject_no and sc.roll_no=r.Roll_No and e.exam_date='" + testDate.ToString() + "'  and ci.criteria in('" + testName + "')  group by r.Batch_Year,r.degree_code,s.subject_code,s.subject_no,e.examFromTime,e.examToTime,e.exam_date,ci.criteria,e.exam_code,e.criteria_no order by  TotalStudent,examDate,examSession desc ";//and sm.Batch_Year in('" + valBatch + "') and sm.degree_code in('" + valBranch + "')
                //dtSubCodeTotalStudent = dirAcc.selectDataTable(subjectCodeTotalStudent);
                DataSet dsStudentDetails1 = da.select_method_wo_parameter(subjectCodeTotalStudent, "text");
                DataTable dtSubCodeTotalStudent1 = new DataTable();
                if (dsStudentDetails1.Tables[0].Rows.Count > 0)
                {
                    dtSubCodeTotalStudent1 = dsStudentDetails1.Tables[0].DefaultView.ToTable();
                }
                if (dtSubCodeTotalStudent1.Rows.Count > 0)
                {
                    dtSubCodeTotalStudent1.DefaultView.RowFilter = "Batch_Year in ('" + valBatch + "') and degree_code in ('" + valBranch + "') ";
                    dtSubCodeTotalStudent = dtSubCodeTotalStudent1.DefaultView.ToTable();
                }

                if (dtSubCodeTotalStudent.Rows.Count == 0)
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "No Student were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                if (dtRoomSeatingsArrange.Rows.Count > 0)
                {
                    object total = dtRoomSeatingsArrange.Compute("SUM(allocted_seats)", string.Empty);
                    int.TryParse(Convert.ToString(total).Trim(), out totAllotedStudents);

                    total = dtRoomSeatingsArrange.Compute("sum(allotedSeatsNew)", string.Empty);
                    int.TryParse(Convert.ToString(total).Trim(), out totAllotedStudentsNew);

                    total = dtRoomSeatingsArrange.Compute("sum(actual_seats)", string.Empty);
                    int.TryParse(Convert.ToString(total).Trim(), out totActualStudents);

                    total = dtRoomSeatingsArrange.Compute("sum(actualSeatsNew)", string.Empty);
                    int.TryParse(Convert.ToString(total).Trim(), out totActualStudentsNew);

                }

                //---------------------------------------- old flow
                Dictionary<string, int> dicAllsubjects = new Dictionary<string, int>();
                DataTable dtAllDistinctSubjects = new DataTable();
                DataTable dtAllDistinctSubjectsList = new DataTable();
                Dictionary<string, int> dicInCompleteSubjects = new Dictionary<string, int>();
                Dictionary<string, int> dicTotalStudentsForSubjects = new Dictionary<string, int>();
                Dictionary<string, int> dicdubcount = new Dictionary<string, int>();
                int totalStudent = 0;
                int seatno;
                bool flag = false;
                //if (dtExamDate.Rows.Count == 0)
                //{

                //}
                //if (dtTestSession.Rows.Count == 0)
                //{

                //}
                if (dtExamDate.Rows.Count > 0)
                {
                    spcn = dtExamDate.Rows.Count - 1;
                    for (int sp = 0; sp < dtTestSession.Rows.Count; sp++)
                    {
                        OddIndex = 0;
                        EvenIndex = 1;
                        Evenflag = false;
                        Oddflag = false;
                        bool isAlternate = false;
                        bool isOne = false;
                        int seatingNo = 0;
                        Dictionary<string, string> dicStudentsHall = new Dictionary<string, string>();
                        Dictionary<string, int> dicHallMaxSeatNo = new Dictionary<string, int>();
                        Dictionary<string, int> dicStudentsAlloted = new Dictionary<string, int>();

                        string examSession = Convert.ToString(dtTestSession.Rows[sp]["examSession"]).Trim();
                        string delstr = string.Empty;
                        delstr = "delete from internalSeatingArragement where examDate='" + Convert.ToString(dtExamDate.Rows[0]["exam_date"]).Trim() + "' and examSession='" + examSession + "'";
                        int delQ = dirAcc.deleteData(delstr);
                        if (dtSubCodeTotalStudent.Rows.Count > 0 && dtStudentInfo.Rows.Count > 0)
                        {
                            dicAllsubjects.Clear();
                            dicInCompleteSubjects.Clear();
                            dicTotalStudentsForSubjects.Clear();
                            dtSubCodeTotalStudent.DefaultView.RowFilter = "examSession='" + examSession + "'";
                            dtAllDistinctSubjects = dtSubCodeTotalStudent.DefaultView.ToTable(true, "subject_code", "examDate", "examSession", "exam_date", "TotalStudent");
                            int index = 0;
                            foreach (DataRow dr in dtAllDistinctSubjects.Rows)
                            {
                                string subjectCode = Convert.ToString(dr["subject_code"]).Trim();
                                string studentCounts = Convert.ToString(dr["TotalStudent"]).Trim();
                                int studentsCount = 0;
                                int.TryParse(studentCounts, out studentsCount);
                                totalStudent += studentsCount;
                                if (!dicAllsubjects.ContainsKey(Convert.ToString(subjectCode).Trim().ToLower()))
                                {
                                    dicAllsubjects.Add(Convert.ToString(subjectCode).Trim().ToLower(), index);
                                }
                                if (!dicInCompleteSubjects.ContainsKey(Convert.ToString(subjectCode).Trim().ToLower()))
                                {
                                    dicInCompleteSubjects.Add(Convert.ToString(subjectCode).Trim().ToLower(), 0);
                                }
                                if (!dicTotalStudentsForSubjects.ContainsKey(Convert.ToString(subjectCode).Trim().ToLower()))
                                {
                                    dicTotalStudentsForSubjects.Add(Convert.ToString(subjectCode).Trim().ToLower(), studentsCount);
                                }
                                else
                                {
                                    int countValue = dicTotalStudentsForSubjects[subjectCode.Trim().ToLower()];
                                    dicTotalStudentsForSubjects[subjectCode.Trim().ToLower()] += studentsCount;
                                }
                                index++;
                            }
                            dtAllDistinctSubjects = dtSubCodeTotalStudent.DefaultView.ToTable(true, "subject_code", "examDate", "examSession", "exam_date");
                        }


                        if (dtAllDistinctSubjects.Rows.Count > 0)
                        {
                            if (dtHallPriority.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtHallPriority.Rows.Count; i++)
                                {
                                    seatno = 0;
                                    string halno = Convert.ToString(dtHallPriority.Rows[i]["rno"]).Trim();
                                    //string halno = "AH";
                                    room = "select * from tbl_room_seats where Hall_No='" + halno + "' and coll_code in (" + collegeCode + ") ";
                                    DataTable dtRoomSeating = new DataTable();
                                    if (dtRoomSeatingsArrange.Rows.Count > 0)
                                    {
                                        dtRoomSeatingsArrange.DefaultView.RowFilter = "Hall_No='" + halno + "' " + ((isAlternate) ? " and hasAlternate ='1'" : "");
                                        dtRoomSeating = dtRoomSeatingsArrange.DefaultView.ToTable();
                                    }

                                    int tempOdd = OddIndex;
                                    bool tempOddFlag = Oddflag;
                                    if (dicInCompleteSubjects.Count == 1)
                                    {
                                        if (OddIndex > EvenIndex)
                                        {
                                            OddIndex = EvenIndex;
                                            Oddflag = Evenflag;
                                            Evenflag = tempOddFlag;
                                            EvenIndex = tempOdd;
                                        }
                                    }
                                    Dictionary<string, int> dicHallSubject = new Dictionary<string, int>();
                                    //DataTable dtrommdet = new DataTable();
                                    //dtrommdet.Clear();
                                    //dtrommdet.Add(dtRoomSeating);
                                    if (dtRoomSeating.Rows.Count > 0)
                                    {
                                        string floor = Convert.ToString(dtRoomSeating.Rows[0]["Floor_Name"]).Trim();
                                        norow = Convert.ToString(dtRoomSeating.Rows[0]["no_of_rows"]).Trim();
                                        string arrangeview = Convert.ToString(dtRoomSeating.Rows[0]["arranged_view"]).Trim();
                                        nocol = Convert.ToString(dtRoomSeating.Rows[0]["no_of_columns"]).Trim();
                                        //string mode = Convert.ToString(dsrommdet.Tables[0].Rows[0]["mode"]).Trim();
                                        string acseat = Convert.ToString(dtRoomSeating.Rows[0]["actual_seats"]).Trim();
                                        allotseat = Convert.ToString(dtRoomSeating.Rows[0]["allocted_seats"]).Trim();
                                        string seattype = Convert.ToString(dtRoomSeating.Rows[0]["is_single"]).Trim();
                                        //string month = Convert.ToString(dsrommdet.Tables[0].Rows[0]["exm_month"]).Trim();
                                        //string year = Convert.ToString(dsrommdet.Tables[0].Rows[0]["exm_year"]).Trim();
                                        arrang = arrangeview.Split(';');
                                        string arrangeViewNew = Convert.ToString(dtRoomSeating.Rows[0]["arrangedViewNew"]).Trim();
                                        string actualSeats = Convert.ToString(dtRoomSeating.Rows[0]["actualSeatsNew"]).Trim();
                                        string allotedSeats = Convert.ToString(dtRoomSeating.Rows[0]["allotedSeatsNew"]).Trim();
                                        string defaultViewNew = Convert.ToString(dtRoomSeating.Rows[0]["defaultViewNew"]).Trim();
                                        if (isAlternate)
                                        {
                                            arrang = arrangeViewNew.Split(';');
                                            if (dicHallMaxSeatNo.ContainsKey(halno.Trim().ToLower()))
                                            {
                                                int seatVal = dicHallMaxSeatNo[halno.Trim().ToLower()];
                                                if (seatno >= seatVal)
                                                {
                                                    dicHallMaxSeatNo[halno.Trim().ToLower()] = seatno;
                                                }
                                                else
                                                {
                                                    dicHallMaxSeatNo[halno.Trim().ToLower()] = seatVal;
                                                }
                                                seatno = dicHallMaxSeatNo[halno.Trim().ToLower()];
                                            }
                                        }
                                        //--------------------------------------- //--------- COE seating ------------------------------------------------//
                                        Dictionary<int, int> dicsubcol = new Dictionary<int, int>();
                                        Dictionary<string, int> dicsubcolcount = new Dictionary<string, int>();
                                        for (int spr = 0; spr <= arrang.GetUpperBound(0); spr++)
                                        {
                                            string colsp = arrang[spr].ToString();
                                            if (colsp.Trim() != "" && colsp != null)
                                            {
                                                spcel = colsp.Split('-');
                                                for (int spc = 0; spc <= spcel.GetUpperBound(0); spc++)
                                                {
                                                    int colsn = Convert.ToInt32(spcel[spc]);
                                                    string strrow = "C" + spc + "R" + spr;
                                                    if (!dicsubcolcount.ContainsKey(strrow))
                                                    {
                                                        dicsubcolcount.Add(strrow, colsn);
                                                    }
                                                    if (dicsubcol.ContainsKey(spc))
                                                    {
                                                        int valc = dicsubcol[spc];
                                                        if (valc < colsn)
                                                        {
                                                            dicsubcol[spc] = colsn;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        dicsubcol.Add(spc, colsn);
                                                    }
                                                }
                                            }
                                        }
                                        if (dtAllDistinctSubjects.Rows.Count > 0)
                                        {
                                            string sub = string.Empty;
                                            string Session = string.Empty;
                                            string ses = string.Empty;
                                            string emd = string.Empty;
                                            string examcode = string.Empty;
                                            string criteria_no = string.Empty;
                                            string degcd = string.Empty;
                                            string subcode = string.Empty;
                                            string seatValue = string.Empty;
                                            int autoChar = 97;
                                            int newSeatNo = 0;
                                            for (int col = 0; col < Convert.ToInt32(nocol); col++)
                                            {
                                                tempOdd = OddIndex;
                                                tempOddFlag = Oddflag;
                                                //tempEven = EvenIndex;
                                                seatValue = string.Empty;
                                                if (dicInCompleteSubjects.Count == 1)
                                                {
                                                    if (OddIndex > EvenIndex)
                                                    {
                                                        OddIndex = EvenIndex;
                                                        Oddflag = Evenflag;
                                                        Evenflag = tempOddFlag;
                                                        EvenIndex = tempOdd;
                                                    }
                                                }
                                                int rowSeat = 0;
                                                for (int row = 0; row < Convert.ToInt32(norow); row++)
                                                {
                                                    string strrow = "C" + col + "R" + row;
                                                    if (dicsubcolcount.ContainsKey(strrow))
                                                    {
                                                        int getcouv = dicsubcolcount[strrow];
                                                        int sucol = dicsubcol[col];
                                                        int recaldept = 0;
                                                        string subjectCodeOld = subcode;
                                                        for (int subcol = 0; subcol < Convert.ToInt32(sucol); subcol++)
                                                        {
                                                            newSeatNo++;
                                                            seatValue = Convert.ToString((row + 1) + (Convert.ToInt32(norow) * subcol)) + Convert.ToString((char)autoChar);
                                                            seatingNo = (row + 1) + (Convert.ToInt32(norow) * subcol);
                                                            string keyValue1 = Convert.ToString(halno.Trim() + "@" + seatValue.Trim()).Trim().ToLower();
                                                            //rowSeat++;
                                                            if (!dicStudentsHall.ContainsKey(keyValue1))
                                                            {
                                                                int scl = 0;
                                                                int oldscl = subcol;
                                                                subjectCodeOld = subcode;
                                                                DataView dvSub = new DataView();
                                                                DataView dvStudent = new DataView();
                                                                subcode = string.Empty;
                                                                tempOdd = OddIndex;
                                                                tempOddFlag = Oddflag;
                                                                //tempEven = EvenIndex;
                                                                if (dicInCompleteSubjects.Count == 1)
                                                                {
                                                                    if (OddIndex > EvenIndex)
                                                                    {
                                                                        OddIndex = EvenIndex;
                                                                        Oddflag = Evenflag;
                                                                        Evenflag = tempOddFlag;
                                                                        EvenIndex = tempOdd;
                                                                    }
                                                                }
                                                            //Rajkumar 12/16/2017

                                                               raja: if (chkCommonSeating.Checked == true)
                                                                {
                                                                    if (dtAllDistinctSubjects.Rows.Count > EvenIndex)
                                                                    {
                                                                        subcode = Convert.ToString(dtAllDistinctSubjects.Rows[EvenIndex]["subject_code"]).Trim();
                                                                        flag = true;
                                                                        scl = EvenIndex;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (Oddflag == true)
                                                                        {
                                                                            if (OddIndex >= dtAllDistinctSubjects.Rows.Count)
                                                                            {
                                                                                if (subcol < getcouv)
                                                                                {
                                                                                    seatno++;
                                                                                    if (!dicHallMaxSeatNo.ContainsKey(halno.Trim().ToLower()))
                                                                                    {
                                                                                        dicHallMaxSeatNo.Add(halno.Trim().ToLower(), seatno);
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        int seatVal = dicHallMaxSeatNo[halno.Trim().ToLower()];
                                                                                        if (seatno >= seatVal)
                                                                                        {
                                                                                            dicHallMaxSeatNo[halno.Trim().ToLower()] = seatno;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            dicHallMaxSeatNo[halno.Trim().ToLower()] = seatVal;
                                                                                        }
                                                                                        seatno = dicHallMaxSeatNo[halno.Trim().ToLower()];
                                                                                    }
                                                                                }
                                                                            }
                                                                            continue;
                                                                        }
                                                                        if (dtAllDistinctSubjects.Rows.Count > OddIndex)
                                                                        {
                                                                            flag = false;
                                                                            subcode = Convert.ToString(dtAllDistinctSubjects.Rows[OddIndex]["subject_code"]).Trim();
                                                                            scl = OddIndex;
                                                                        }
                                                                    }
                                                                }

                                                                //Rajkumar
                                                                if (chkCommonSeating.Checked == false)
                                                                {
                                                                    if (subcol % 2 != 0)
                                                                    {
                                                                        if (Evenflag == true)
                                                                        {
                                                                            if (EvenIndex >= dtAllDistinctSubjects.Rows.Count)
                                                                            {
                                                                                if (subcol < getcouv)
                                                                                {
                                                                                    seatno++;
                                                                                    if (!dicHallMaxSeatNo.ContainsKey(halno.Trim().ToLower()))
                                                                                    {
                                                                                        dicHallMaxSeatNo.Add(halno.Trim().ToLower(), seatno);
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        int seatVal = dicHallMaxSeatNo[halno.Trim().ToLower()];
                                                                                        if (seatno >= seatVal)
                                                                                        {
                                                                                            dicHallMaxSeatNo[halno.Trim().ToLower()] = seatno;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            dicHallMaxSeatNo[halno.Trim().ToLower()] = seatVal;
                                                                                        }
                                                                                        seatno = dicHallMaxSeatNo[halno.Trim().ToLower()];
                                                                                    }
                                                                                }
                                                                            }
                                                                            continue;
                                                                        }
                                                                        if (dtAllDistinctSubjects.Rows.Count > EvenIndex)
                                                                        {
                                                                            subcode = Convert.ToString(dtAllDistinctSubjects.Rows[EvenIndex]["subject_code"]).Trim();
                                                                            flag = true;
                                                                            scl = EvenIndex;
                                                                        }
                                                                    }

                                                                    else
                                                                    {
                                                                        if (Oddflag == true)
                                                                        {
                                                                            if (OddIndex >= dtAllDistinctSubjects.Rows.Count)
                                                                            {
                                                                                if (subcol < getcouv)
                                                                                {
                                                                                    seatno++;
                                                                                    if (!dicHallMaxSeatNo.ContainsKey(halno.Trim().ToLower()))
                                                                                    {
                                                                                        dicHallMaxSeatNo.Add(halno.Trim().ToLower(), seatno);
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        int seatVal = dicHallMaxSeatNo[halno.Trim().ToLower()];
                                                                                        if (seatno >= seatVal)
                                                                                        {
                                                                                            dicHallMaxSeatNo[halno.Trim().ToLower()] = seatno;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            dicHallMaxSeatNo[halno.Trim().ToLower()] = seatVal;
                                                                                        }
                                                                                        seatno = dicHallMaxSeatNo[halno.Trim().ToLower()];
                                                                                    }
                                                                                }
                                                                            }
                                                                            continue;
                                                                        }
                                                                        if (dtAllDistinctSubjects.Rows.Count > OddIndex)
                                                                        {
                                                                            flag = false;
                                                                            subcode = Convert.ToString(dtAllDistinctSubjects.Rows[OddIndex]["subject_code"]).Trim();
                                                                            scl = OddIndex;
                                                                        }
                                                                    }
                                                                }

                                                                emd = Convert.ToString(dtAllDistinctSubjects.Rows[scl]["exam_date"]).Trim();
                                                                string examDateF = Convert.ToString(dtAllDistinctSubjects.Rows[scl]["examDate"]).Trim();
                                                                ses = Convert.ToString(dtAllDistinctSubjects.Rows[scl]["examSession"]).Trim();
                                                                criteria_no = Convert.ToString(dtSubCodeTotalStudent.Rows[scl]["criteria_no"]).Trim();
                                                                examcode = Convert.ToString(dtSubCodeTotalStudent.Rows[scl]["exam_code"]).Trim();

                                                                if (dtAllDistinctSubjects.Rows.Count > 0 && dtStudentInfo.Rows.Count > 0)
                                                                {
                                                                    dtAllDistinctSubjects.DefaultView.RowFilter = "subject_code='" + subcode + "'";
                                                                    dtStudentInfo.DefaultView.RowFilter = "subject_code='" + subcode + "' and examSession='" + ses + "' and examDate='" + examDateF + "'";
                                                                    dtStudentInfo.DefaultView.Sort = "exam_date,examSession,degree_code,Batch_Year,sections,Reg_No";
                                                                    //dtSubCodeTotalStudent.DefaultView.RowFilter = "subject_code='" + subcode + "' and examDate='" + emd + "' and examSession='" + ses + "' ";
                                                                    //dtStudentInfo.DefaultView.RowFilter = "subject_code='" + subcode + "' and examDate='" + emd + "' and examSession='" + ses + "' ";
                                                                    dvStudent = dtStudentInfo.DefaultView;
                                                                    dvSub = dtAllDistinctSubjects.DefaultView;
                                                                }
                                                                if (dvSub.Count > 0 && dvStudent.Count > 0)
                                                                {
                                                                    if (subcol < getcouv)
                                                                    {
                                                                        int stuco = 0;
                                                                        if (dicdubcount.ContainsKey(subcode.ToString().Trim().ToLower()))
                                                                        {
                                                                            stuco = dicdubcount[subcode.ToString().Trim().ToLower()];
                                                                        }
                                                                        else
                                                                        {
                                                                            dicdubcount.Add(subcode.ToString().Trim().ToLower(), 0);
                                                                        }
                                                                        //if (stuco != dvSub.Count)
                                                                        if (stuco != dvStudent.Count)
                                                                        {
                                                                            //if (dvSub.Count > stuco)
                                                                            if (dvStudent.Count > stuco)
                                                                            {
                                                                                seatno++;
                                                                                if (!dicHallMaxSeatNo.ContainsKey(halno.Trim().ToLower()))
                                                                                {
                                                                                    dicHallMaxSeatNo.Add(halno.Trim().ToLower(), seatno);
                                                                                }
                                                                                else
                                                                                {
                                                                                    int seatVal = dicHallMaxSeatNo[halno.Trim().ToLower()];
                                                                                    if (seatno >= seatVal)
                                                                                    {
                                                                                        dicHallMaxSeatNo[halno.Trim().ToLower()] = seatno;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        dicHallMaxSeatNo[halno.Trim().ToLower()] = seatVal;
                                                                                    }
                                                                                    seatno = dicHallMaxSeatNo[halno.Trim().ToLower()];
                                                                                }
                                                                                // btngen = 1;
                                                                                string roll1 = Convert.ToString(dvStudent[stuco]["App_No"]).Trim();
                                                                                //degcd = Convert.ToString(dvSub[stuco]["Degree_Code"]).Trim();
                                                                                sub = Convert.ToString(dvStudent[stuco]["subject_no"]).Trim();
                                                                                string examCode1 = Convert.ToString(dvStudent[stuco]["exam_code"]).Trim();//exam_code,e.criteria_no
                                                                                string criteriaNo1 = Convert.ToString(dvStudent[stuco]["criteria_no"]).Trim();

                                                                                //Session = Convert.ToString(dvSub[stuco]["examSession"]).Trim();
                                                                                string keyValue = Convert.ToString(halno.Trim() + "@" + seatValue.Trim()).Trim().ToLower();
                                                                                if (!dicStudentsHall.ContainsKey(keyValue))
                                                                                {
                                                                                    dicStudentsHall.Add(keyValue, roll1);
                                                                                }

                                                                                if (!dicStudentsAlloted.ContainsKey(roll1.Trim().ToLower()))
                                                                                {
                                                                                    dicStudentsAlloted.Add(roll1.Trim().ToLower(), 1);
                                                                                }
                                                                                //to be changed 
                                                                                //string seatarrange = "if exists(select * from exam_seating where edate='" + emd + "' and ses_sion='" + ses + "' and subject_no='" + sub + "' and roomno='" + halno + "' and seat_no='" + seatno + "')delete from exam_seating where edate='" + emd + "' and ses_sion='" + ses + "' and subject_no='" + sub + "' and roomno='" + halno + "' and seat_no='" + seatno + "' insert into exam_seating (roomno,regno,subject_no,edate,ses_sion,block,seat_no,degree_code,ArrangementType,Floorid,seatCode)values('" + halno + "','" + roll1 + "','" + sub + "','" + emd + "','" + ses + "','" + floor + "','" + seatno + "','" + degcd + "',0,'" + floor + "','" + seatValue + "')  ";

                                                                                string seatarrange = "if exists(select * from internalSeatingArragement where examDate='" + emd + "' and examSession='" + ses + "'  and hallNo='" + halno + "' and seatNo='" + seatno + "' and subjectNo='" + sub + "' and appNo='" + roll1 + "') delete from internalSeatingArragement where examSession='" + ses + "' and hallNo='" + halno + "' and seatNo='" + seatno + "' and subjectNo='" + sub + "' and appNo='" + roll1 + "' insert into internalSeatingArragement(appNo,hallNo,seatno,examDate,examSession,examCode,criteriaNo,seatCode,subjectNo) values ('" + roll1 + "','" + halno + "','" + seatno + "','" + emd + "','" + ses + "','" + examCode1 + "','" + criteriaNo1 + "','" + seatValue + "','" + sub + "')";
                                                                                a = da.update_method_wo_parameter(seatarrange, "text");
                                                                                if (a != 0)
                                                                                {
                                                                                    isGenerated = true;
                                                                                }
                                                                                stuco++;
                                                                                dicdubcount[subcode.ToString().Trim().ToLower()] = stuco;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (dicInCompleteSubjects.ContainsKey(subcode.ToString().Trim().ToLower()))
                                                                            {
                                                                                dicInCompleteSubjects.Remove(subcode.ToString().Trim().ToLower());
                                                                            }
                                                                            if (flag == true)
                                                                            {
                                                                                if (OddIndex > EvenIndex)
                                                                                {
                                                                                    EvenIndex = OddIndex + 1;
                                                                                }
                                                                                else if (OddIndex < EvenIndex)
                                                                                {
                                                                                    EvenIndex = EvenIndex + 1;
                                                                                }
                                                                                if (EvenIndex >= dtAllDistinctSubjects.Rows.Count)
                                                                                {
                                                                                    //EvenIndex--;
                                                                                    Evenflag = true;
                                                                                }
                                                                                goto raja;
                                                                                //seatno-=1;
                                                                            }
                                                                            else if (flag == false)
                                                                            {
                                                                                if (OddIndex > EvenIndex)
                                                                                {
                                                                                    OddIndex = OddIndex + 1;
                                                                                }
                                                                                else if (OddIndex < EvenIndex)
                                                                                {
                                                                                    OddIndex = EvenIndex + 1;
                                                                                }
                                                                                if (OddIndex >= dtAllDistinctSubjects.Rows.Count)
                                                                                {
                                                                                    //OddIndex--;
                                                                                    Oddflag = true;
                                                                                }
                                                                                goto raja;
                                                                                //seatno -= 1;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                autoChar++;
                                            }

                                        }
                                    }
                                    if (dtHallPriority.Rows.Count - 1 == i)
                                    {
                                        if (!isOne)
                                        {
                                            if (totalStudent > totAllotedStudents)
                                            {
                                                if (dicStudentsAlloted.Count < totalStudent)
                                                {
                                                    i = -1;
                                                    isAlternate = true;
                                                    isOne = true;
                                                    continue;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //lblAlertMsg.Visible = true;
                                //lblAlertMsg.Text = "No Hall were Found";
                                //divPopAlert.Visible = true;
                                //return;
                            }
                        }
                        else
                        {
                            //lblAlertMsg.Visible = true;
                            //lblAlertMsg.Text = "Student Not Write Exam this Date";
                            //divPopAlert.Visible = true;
                            //return;
                        }
                    }
                }
                else
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "No Exam Time Table Found";
                    return;
                }

                //-------------------------
            }
            lblAlertMsg.Visible = true;
            lblAlertMsg.Text = "Seating " + (((isGenerated) ? "" : "Not")) + " Genarated";
            divPopAlert.Visible = true;
            return;
        }
        catch (Exception ex)
        {

            lblAlertMsg.Visible = true;
            lblAlertMsg.Text = ex.ToString();
            return;

        }
    }

    private bool ColumnHeaderVisiblity(int type, DataSet dsSettingsOptional = null)
    {
        //r.Reg_No,r.Roll_No,r.Roll_Admit
        bool hasValues = false;
        try
        {
            DataSet dsSettings = new DataSet();
            if (dsSettingsOptional == null)
            {
                string grouporusercode = string.Empty;
                if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    string groupCode = Convert.ToString(Session["group_code"]).Trim();
                    string[] groupUser = Convert.ToString(groupCode).Trim().Split(';');
                    if (groupUser.Length > 0)
                    {
                        groupCode = groupUser[0].Trim();
                    }
                    if (!string.IsNullOrEmpty(groupCode.Trim()))
                    {
                        grouporusercode = " and  group_code=" + Convert.ToString(groupCode).Trim() + "";
                    }
                }
                else if (Session["usercode"] != null)
                {
                    grouporusercode = " and usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
                }
                if (!string.IsNullOrEmpty(grouporusercode))
                {
                    string Master1 = "select * from Master_Settings where settings in('Roll No','Register No','Admission No','Application No') and value='1' " + grouporusercode + "";
                    dsSettings = dirAcc.selectDataSet(Master1);
                }
            }
            else
            {
                dsSettings = dsSettingsOptional;
            }
            if (dsSettings.Tables.Count > 0 && dsSettings.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drSettings in dsSettings.Tables[0].Rows)
                {
                    switch (type)
                    {
                        case 0:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "roll no")
                            {
                                hasValues = true;
                            }
                            break;
                        case 1:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "register no")
                            {
                                hasValues = true;
                            }
                            break;
                        case 2:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "admission no")
                            {
                                hasValues = true;
                            }
                            break;

                        case 3:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "application no")
                            {
                                hasValues = true;
                            }
                            break;
                    }
                    if (hasValues)
                        break;
                }

            }
            return hasValues;
        }
        catch (Exception ex)
        {
            return false;
        }

    }

    public void report()
    {

        try
        {

            string rl = string.Empty;
            int p = 0;
            int l1 = 0;
            int t = 0;
            string nrow = string.Empty;
            int vl = 0;
            int v = 0;
            int vrow = 0;
            int flag = 0;
            int ncol2 = 0;
            string dat = string.Empty;
            string arrangeview1 = string.Empty;
            string arrangeviewNew = string.Empty;
            string allotSeat = string.Empty;
            int allotedSeats = 0;
            int allotedSeatsNew = 0;
            DataSet dsCollege = new DataSet();
            collegeCode = string.Empty;
            collegeCode = ddlCollege.SelectedItem.Value.ToString();
            string orderType = string.Empty;
            DataTable dtStudentInfo = new DataTable();
            TableCell tcellRow = new TableCell();
            bool isRollNoVisible = ColumnHeaderVisiblity(0);
            bool isRegNoVisible = ColumnHeaderVisiblity(1);
            bool isAdmissionNoVisible = ColumnHeaderVisiblity(2);
            bool isAppNo = ColumnHeaderVisiblity(3);
            string sqlry = string.Empty;
            string frdate = string.Empty;
            string todate = string.Empty;
            string fotodate = string.Empty;
            int m = 0;
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
            Gios.Pdf.PdfPage mypdfpage1 = mydocument.NewPage();
            Font header = new Font("Arial", 15, FontStyle.Bold);
            Font header1 = new Font("Arial", 14, FontStyle.Bold);
            Font Fonthead = new Font("Arial", 12, FontStyle.Bold);
            Font Fontbold = new Font("Times New Roman", 10, FontStyle.Bold);
            Font Fontbold1 = new Font("Times New Roman", 12, FontStyle.Bold);
            Font Fontbold2 = new Font("Times New Roman", 9, FontStyle.Bold);
            Font Fonttimes = new Font("Times New Roman", 10, FontStyle.Regular);
            Font Fontsmall = new Font("Arial", 9, FontStyle.Regular);
            Font FontsmallBold = new Font("Arial", 10, FontStyle.Bold);
            Font fontitalic = new Font("Arial", 9, FontStyle.Italic);
            Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);



            if (!string.IsNullOrEmpty(collegeCode.Trim()))
            {
                string qrynew = "select *,district+' - '+pincode  as districtpin from collinfo where college_code in (" + collegeCode + ")";
                dsCollege = dt.select_method_wo_parameter(qrynew, "Text");
            }

            //string Examdate = string.Empty;
            string hallNo = string.Empty;
            string examSession = string.Empty;

            //DataTable dtExamSession = new DataTable();
            //DataTable dtHallNo = new DataTable();
            if (ddlTestDate.Items.Count > 0)
            {
                string ldate = ddlTestDate.SelectedItem.ToString();
                if (ldate.Trim() != "")
                {
                    string[] spl = ldate.Split('/');
                    DateTime dtl = Convert.ToDateTime(spl[1] + '/' + spl[0] + '/' + spl[2]);
                    testDate = dtl.ToString("yyyy-MM-dd");
                }
            }
            if (Multiple.Checked == true)
            {

                multihall();
            }
            else
            {
                hallNo = ddlHallNo.SelectedItem.ToString().Trim();
                examSession = ddlSession.SelectedItem.ToString().Trim();
                //ds.Clear();                
                rl = "select * from tbl_room_seats where Hall_No ='" + hallNo + "'";
                ds = dt.select_method_wo_parameter(rl, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    flag = 1;
                    nrow = Convert.ToString(ds.Tables[0].Rows[0]["no_of_rows"]).Trim();
                    arrangeview1 = Convert.ToString(ds.Tables[0].Rows[0]["arranged_view"]).Trim();
                    arrangeviewNew = Convert.ToString(ds.Tables[0].Rows[0]["arrangedViewNew"]).Trim();
                    allotseat = Convert.ToString(ds.Tables[0].Rows[0]["allocted_seats"]).Trim();
                    allotSeat = Convert.ToString(ds.Tables[0].Rows[0]["allotedSeatsNew"]).Trim();
                    int.TryParse(allotseat, out allotedSeats);
                    int.TryParse(allotSeat, out allotedSeatsNew);
                }
                //string sql = "select * from internalSeatingArragement es,subject s where es.subjectNo=s.subject_no and es.hallNo='" + hallNo + "' and  es.examDate='" + testDate.ToString() + "' and  es.examSession='" + examSession + "' order by seatNo";
                string sql = "select r.Reg_No,r.Roll_No,d.Acronym,r.Roll_Admit,r.Stud_Name,a.app_formno as ApplicationNo,es.*,s.subject_code from internalSeatingArragement es,subject s,Registration r,applyn a,Degree d where r.App_No=es.appNo and a.app_no=r.App_No and a.app_no=es.appNo and es.subjectNo=s.subject_no and d.Degree_Code=r.degree_code and es.hallNo='" + hallNo + "' and  es.examDate='" + testDate.ToString() + "' and  es.examSession='" + examSession + "' order by seatNo";
                ds1 = dt.select_method_wo_parameter(sql, "text");
                if (flag == 1)
                {
                    if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        if (isRollNoVisible)
                        {
                            orderType = "Roll_No";
                        }
                        else if (isRegNoVisible)
                        {
                            orderType = "Reg_No";
                        }
                        else if (isAdmissionNoVisible)
                        {
                            orderType = "Roll_Admit";

                        }
                        else if (isAppNo)
                        {
                            orderType = "ApplicationNo";
                        }
                        else
                        {
                            orderType = "Roll_No";
                        }
                        pnlContent1.Visible = true;

                        if (allotedSeats < ds1.Tables[0].Rows.Count)
                        {
                            arrang = arrangeviewNew.Split(';');
                        }
                        else //if (allotedSeatsNew <= ds1.Tables[0].Rows.Count)
                        {
                            arrang = arrangeview1.Split(';');
                        }
                        Dictionary<int, int> dicsubcol = new Dictionary<int, int>();
                        Dictionary<string, int> dicsubcolcount = new Dictionary<string, int>();

                        //print setting
                        if (dsCollege.Tables.Count > 0 && dsCollege.Tables[0].Rows.Count > 0)
                        {
                            //maha
                            string frdatenew = string.Empty;
                            string todatenew = string.Empty;
                            string fotodatenew = string.Empty;
                            sqlry = "SELECT CONVERT(VARCHAR(50),min(exam_date),106) as fromdate,CONVERT(VARCHAR(50),max(exam_date),106) as todate FROM  CriteriaForInternal c,Exam_type e where c.Criteria_no=e.criteria_no and  c.criteria='" + ddlTest.SelectedItem.ToString() + "'";
                            DataTable dtTestDateNew = new DataTable();
                            dtTestDateNew = dirAcc.selectDataTable(sqlry);
                            if (dtTestDateNew.Rows.Count > 0)
                            {
                                frdatenew = Convert.ToString(dtTestDateNew.Rows[0]["fromdate"].ToString());

                                todatenew = Convert.ToString(dtTestDateNew.Rows[0]["todate"].ToString());
                                if (frdatenew == todatenew)
                                {
                                    fotodatenew = frdatenew;
                                }
                                else
                                {
                                    fotodatenew = frdatenew + '-' + todatenew;
                                }
                            }
                            sqlry = string.Empty;
                            sqlry = "select upper(convert(varchar(3),DateAdd(month,CONVERT(int, min(datepart(m,exam_date))),-1)))fromdate,upper(convert(varchar(3),DateAdd(month,CONVERT(int, max(datepart(m,exam_date))),-1)))todate from CriteriaForInternal c,Exam_type e where c.Criteria_no=e.criteria_no and  c.criteria='" + ddlTest.SelectedItem.ToString() + "'";
                            DataTable dtTestDate = new DataTable();
                            dtTestDate = dirAcc.selectDataTable(sqlry);
                            if (dtTestDate.Rows.Count > 0)
                            {
                                frdate = Convert.ToString(dtTestDate.Rows[0]["fromdate"].ToString());

                                todate = Convert.ToString(dtTestDate.Rows[0]["todate"].ToString());
                                if (todate == frdate)
                                {
                                    fotodate = frdate;
                                }
                                else
                                {
                                    fotodate = frdate + '/' + todate;
                                }
                            }
                            //maha

                            //string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(Convert.ToString(ddlMonth.SelectedItem.Value).Trim()));
                            // string[] strpa = Convert.ToString(dsCollege.Tables[0].Rows[0]["affliatedby"]).Trim().Split(',');
                            spF1College.InnerText = Convert.ToString(dsCollege.Tables[0].Rows[0]["Collname"]).Trim();
                            //spF1Controller.InnerText = fotodatenew;
                            spF1Seating.InnerText = "SEATING ARRANGEMENT";
                            // spF1Aff.InnerText = (strpa.Length > 0) ? Convert.ToString(strpa[0]).Trim() : Convert.ToString(dsCollege.Tables[0].Rows[0]["affliatedby"]).Trim();
                            spF1Date.InnerText = "Date & Session : " + Convert.ToString(ddlTestDate.SelectedItem.Text).Trim() + " & " + Convert.ToString(ddlSession.SelectedItem.Text).Trim();
                            spExamination.InnerText = "Examination - " + ddlTest.SelectedItem.ToString().ToUpper() + "-" + fotodatenew + " "; //+ Convert.ToString(ddlYear.SelectedItem.Text);
                            spHallNo.InnerText = "Hall No : " + Convert.ToString(ddlHallNo.SelectedItem.Text).Trim();

                            Span2.InnerText = txtsignature.Text;


                        }

                        dtfor1.Columns.Add("row");
                        for (int spr = 0; spr <= arrang.GetUpperBound(0); spr++)
                        {
                            string colsp = arrang[spr].ToString();
                            if (colsp.Trim() != "" && colsp != null)
                            {
                                spcel = colsp.Split('-');
                                for (int spc = 0; spc <= spcel.GetUpperBound(0); spc++)
                                {
                                    int colsn = Convert.ToInt32(spcel[spc]);
                                    string strrow = "C" + spc + "R" + spr;
                                    if (!dicsubcolcount.ContainsKey(strrow))
                                    {
                                        dicsubcolcount.Add(strrow, colsn);
                                    }
                                    if (dicsubcol.ContainsKey(spc))
                                    {
                                        int valc = dicsubcol[spc];
                                        if (valc < colsn)
                                        {
                                            dicsubcol[spc] = colsn;
                                        }
                                    }
                                    else
                                    {
                                        dicsubcol.Add(spc, colsn);
                                    }
                                }
                            }
                        }

                        int count = 0;
                        int add = 0;
                        int getcouv = 0;
                        ArrayList addarr = new ArrayList();
                        //TableRow trRow1 = new TableRow();
                        TableCell tcell = new TableCell();

                        int autoChar = 97;

                        for (int h1 = 0; h1 < dicsubcol.Count; h1++)
                        {
                            int sucol = dicsubcol[h1];
                            for (int l = 1; l <= sucol; l++)
                            {
                                dtfor1.Columns.Add("C" + h1 + l + "");
                            }
                        }



                        for (int h1 = 0; h1 < dicsubcol.Count; h1++)
                        {
                            t++;
                            int sucol = dicsubcol[h1];
                            for (int l = sucol - 1; l < sucol; l++)
                            {
                                l1++;

                                TableCell tcellnew = new TableCell();

                                addarr.Clear();
                                for (int j = 0; j < Convert.ToInt32(nrow); j++)
                                {
                                    drfor1 = dtfor1.NewRow();
                                    vl++;
                                    string strrow = "C" + h1 + "R" + j;
                                    string seatValue = string.Empty;
                                    if (dicsubcolcount.ContainsKey(strrow))
                                    {
                                        getcouv = dicsubcolcount[strrow];
                                        count = add;
                                        addarr.Add(getcouv);
                                        for (int g = 0; g < getcouv; g++)
                                        {
                                            hss++;
                                            //seatValue = Convert.ToString("");
                                            seatValue = Convert.ToString((j + 1) + (Convert.ToInt32(nrow) * g)) + Convert.ToString((char)autoChar);
                                            //string seatNo = Convert.ToString((h1 + 1) * ((j + 1) + (Convert.ToInt32(nrow) * g)));
                                            tcellnew = new TableCell();
                                            tcellnew.Width = 86;
                                            tcellnew.Text = Convert.ToString(g + 1);
                                            //tcellnew.BorderWidth = 1;
                                            //tcellnew.BorderColor = Color.Black;
                                            if (dtfor1.Columns.Count - 1 >= tblHeader2.Cells.Count)
                                            {
                                                if (tblHeader2.Cells.Count == count)
                                                    tblHeader2.Cells.AddAt(count, tcellnew);
                                                else
                                                    tblHeader2.Cells.Add(tcellnew);
                                            }

                                            drfor1["row"] = "Row " + (j + 1);

                                            DataView dvStudent = new DataView();
                                            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                                            {
                                                ds1.Tables[0].DefaultView.RowFilter = " seatCode='" + seatValue + "'";// ((chkNewSeating.Checked) ? " seatCode='" + seatValue + "'" : " seat_no='" + hss + "' ");
                                                dvStudent = ds1.Tables[0].DefaultView;
                                            }


                                            if (dvStudent.Count > 0)
                                            {

                                                int g2 = g + 1;
                                                drfor1["C" + h1 + g2 + ""] = dvStudent[0][orderType].ToString() + "  -[" + dvStudent[0]["seatNo"].ToString() + "]- " + dvStudent[0]["Acronym"].ToString();
                                            }
                                            else
                                            {
                                                int g2 = g + 1;
                                                drfor1["C" + h1 + g2 + ""] = "[" + hss + "]";
                                            }

                                            count++;
                                        }

                                    }
                                    dtfor1.Rows.Add(drfor1);
                                }
                                if (addarr.Count > 0)
                                {
                                    addarr.Sort();
                                }
                                add = add + Convert.ToInt32(addarr[addarr.Count - 1]);
                                //h++;
                                if (v < dtfor1.Columns.Count)
                                {
                                    tcellnew = new TableCell();
                                    //tcellnew.Width = 90;
                                    tcellnew.Text = "Column" + (t);
                                    tcellnew.BorderWidth = 0;
                                    tcellnew.ColumnSpan = sucol;
                                    tblHeader1.Cells.Add(tcellnew);
                                    v = v + sucol;
                                }


                            }
                            autoChar++;
                        }
                        //tblFormat1.Width = tblHeader1.Cells.Count * 80;
                    }
                    else
                    {
                        divFormat1.Visible = false;
                        lblAlertMsg.Visible = true;
                        lblAlertMsg.Text = "No Record Found";
                        divPopAlert.Visible = true;
                        return;
                    }
                    string rwname = string.Empty;
                    for (int i1 = 0; i1 < dtfor1.Columns.Count; i1++)
                    {
                        dtformat1.Columns.Add(Convert.ToString(dtfor1.Columns[i1].ColumnName));
                    }
                    int count1 = Convert.ToInt32(dtformat1.Columns.Count);
                    int ct1 = 0;
                    for (int j1 = 0; j1 < Convert.ToInt32(nrow); j1++)
                    {
                        dicformat1.Clear();
                        drformat1 = dtformat1.NewRow();
                        string rw = "Row " + (j1 + 1) + "";
                        dtfor1.DefaultView.RowFilter = " row='" + rw + "'";
                        rwname = rw;
                        DataView dv = dtfor1.DefaultView;
                        if (dv.Count > 0)
                        {
                            ct1++;
                            for (int ct = 0; ct < dv.Count; ct++)
                            {
                                for (int i2 = 0; i2 < dv.Table.Columns.Count; i2++)
                                {
                                    string str = Convert.ToString(dv[ct][i2]);
                                    if (!string.IsNullOrEmpty(str))
                                    {
                                        if (!dicformat1.ContainsValue(str))
                                        {

                                            dicformat1.Add(i2, str);
                                        }
                                    }

                                }
                            }

                        }
                        if (dicformat1.Count > 0)
                        {
                            foreach (KeyValuePair<int, string> dic in dicformat1)
                            {
                                int val = dic.Key;
                                string strval = dic.Value;
                                string colnam = Convert.ToString(dtformat1.Columns[val].ColumnName);
                                drformat1[colnam] = strval;

                            }

                        }
                        dtformat1.Rows.Add(drformat1);
                    }

                    int totalcount = 0;
                    string sal = " select distinct s.subject_name,s.subject_code,COUNT(e.subjectNo) as num from internalSeatingArragement e,subject s where e.subjectNo=s.subject_no and e.hallNo='" + ddlHallNo.SelectedItem.Text + "' and e.examDate='" + testDate.ToString() + "' and e.examSession='" + ddlSession.SelectedItem.Text + "' group by s.subject_name,s.subject_code order by COUNT(e.subjectNo) desc,s.subject_code";
                    ds = dt.select_method_wo_parameter(sal, "text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {
                            drformat1 = dtformat1.NewRow();

                            string colname = Convert.ToString(dtformat1.Columns[1].ColumnName);
                            drformat1[colname] = Convert.ToString(ds.Tables[0].Rows[j]["subject_code"]) + " - " + Convert.ToString(ds.Tables[0].Rows[j]["subject_name"]);

                            string colcount = Convert.ToString(dtformat1.Columns.Count);
                            int clct = Convert.ToInt32(colcount) - 1;
                            string colnam = Convert.ToString(dtformat1.Columns[Convert.ToInt32(clct)].ColumnName);
                            drformat1[colnam] = Convert.ToString(" Count : " + ds.Tables[0].Rows[j]["num"].ToString() + "");

                            totalcount = totalcount + Convert.ToInt32(ds.Tables[0].Rows[j]["num"].ToString());
                            dtformat1.Rows.Add(drformat1);
                        }
                        drformat1 = dtformat1.NewRow();

                        string colcount1 = Convert.ToString(dtformat1.Columns.Count);
                        int clct1 = Convert.ToInt32(colcount1) - 1;
                        string colnam1 = Convert.ToString(dtformat1.Columns[Convert.ToInt32(clct1)].ColumnName);
                        drformat1[colnam1] = "Total:" + totalcount + "";
                        divFormat1.Visible = true;
                        GridView2.Visible = true;
                        //Fpspread.Visible = true;
                        dtformat1.Rows.Add(drformat1);
                    }
                    GridView2.DataSource = dtformat1;
                    GridView2.DataBind();
                    int colcout = dtformat1.Columns.Count;
                    string coutnum = rwname.Substring(4, 1);
                    for (int l = 3; l < dtformat1.Columns.Count; l++)
                    {

                        GridView2.Rows[Convert.ToInt32(coutnum)].Cells[l - 1].Visible = false;
                        GridView2.Rows[Convert.ToInt32(coutnum) + 1].Cells[l - 1].Visible = false;
                        GridView2.Rows[Convert.ToInt32(coutnum) + 2].Cells[l - 1].Visible = false;
                    }
                    GridView2.Rows[Convert.ToInt32(coutnum)].Cells[1].ColumnSpan = colcout - 2;
                    GridView2.Rows[Convert.ToInt32(coutnum) + 1].Cells[1].ColumnSpan = colcout - 2;
                    GridView2.Rows[Convert.ToInt32(coutnum) + 2].Cells[1].ColumnSpan = colcout - 2;
                    GridView2.Rows[Convert.ToInt32(coutnum)].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                    GridView2.Rows[Convert.ToInt32(coutnum) + 1].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                    GridView2.Visible = true;
                }

                else
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "No Session were Found";
                    divPopAlert.Visible = true;
                    return;
                }

                string studentInfo1 = "select r.Reg_No,r.Roll_No,r.Roll_Admit,r.App_No,r.Stud_Name,r.Stud_Type,r.Current_Semester,LTRIM(RTRIM(ISNULL(r.Sections,''))) Sections,r.degree_code,r.Batch_Year,case when r.mode='1' then 'Regular' when r.mode='2' then 'Transfer' when r.mode='3' then 'Lateral' end as Mode,r.mode as ModeVal,r.isRedo,Convert(int,DATEPART(year,r.Adm_Date)) AS tempBatch,c.Course_Name+' - '+dt.dept_acronym as DeptName,c.Course_Name,dt.Dept_Name,dt.dept_acronym,s.subject_code,s.subject_name,ci.criteria,es.examDate,es.examSession,es.hallNo,es.seatNo from internalSeatingArragement es,CriteriaForInternal ci,Exam_type e ,class_master cs,syllabus_master sm,Course c,Degree dg,Department dt,Registration r,subject s where r.degree_code=sm.degree_code and r.Batch_Year=sm.Batch_Year and r.Current_Semester=sm.semester and r.App_No=es.appNo and c.Course_Id=dg.Course_Id and dt.Dept_Code=dg.Dept_Code and r.degree_code=sm.degree_code and sm.degree_code=dg.Degree_Code and ci.syll_code=sm.syll_code and s.syll_code=sm.syll_code and s.syll_code=ci.syll_code and s.subject_no=es.subjectNo and ci.Criteria_no=e.criteria_no and s.subject_no=e.subject_no and e.exam_code=es.examCode  and cs.rno=es.hallNo and es.examDate='" + testDate.ToString() + "' and es.examSession='" + ddlSession.SelectedItem.ToString().Trim() + "'  and ci.criteria='" + ddlTest.SelectedValue.ToString().Trim() + "' and es.hallNo='" + ddlHallNo.SelectedItem.Text + "' order by es.examDate,es.examSession,es.hallNo,es.seatNo,r.Reg_No,r.Current_Semester";
                //DataSet dsStudentInfo=da.select_method_wo_parameter(studentInfo,'text');
                dtStudentInfo = dirAcc.selectDataTable(studentInfo1);
                DataTable dtRoomSeating = new DataTable();
                DataTable dtdicStudent = new DataTable();
                DataTable dicStuCount = new DataTable();
                DataTable dtBatchWiseDetail = new DataTable();
                if (dtStudentInfo.Rows.Count > 0)
                {
                    // dtStudentInfo.DefaultView.RowFilter = "hallNo='" + roomno + "'";
                    dtRoomSeating = dtStudentInfo.DefaultView.ToTable();
                    dtBatchWiseDetail = dtStudentInfo.DefaultView.ToTable(true, "degree_code", "Batch_Year", "Current_Semester", "Sections", "DeptName", "Course_Name", "subject_code", "dept_acronym", "subject_name");//Sections,r.degree_code,r.Batch_Year
                }
                int headercount = dtBatchWiseDetail.Rows.Count;
                string det = "details";
                dtform1.Columns.Add("Header");
                int ct3 = 0;
                if (headercount > 0)
                {
                    for (int ct = 0; ct < headercount; ct++)
                    {
                        ct3++;
                        dtform1.Columns.Add(det);
                        det = "details" + ct3 + "";
                    }
                }
                else
                {
                    dtform1.Columns.Add("details");
                }

                drform1 = dtform1.NewRow();
                drform1["Header"] = "Date";
                dtform1.Rows.Add(drform1);
                drform1 = dtform1.NewRow();
                drform1["Header"] = "Dept";
                dtform1.Rows.Add(drform1);
                drform1 = dtform1.NewRow();
                drform1["Header"] = "No of Students Registered";
                dtform1.Rows.Add(drform1);
                drform1 = dtform1.NewRow();
                drform1["Header"] = "No.of Student Present";
                dtform1.Rows.Add(drform1);
                drform1 = dtform1.NewRow();
                drform1["Header"] = "No.of Student Absent";
                dtform1.Rows.Add(drform1);
                drform1 = dtform1.NewRow();
                drform1["Header"] = "Name of Hall Superintendent";
                dtform1.Rows.Add(drform1);
                drform1 = dtform1.NewRow();
                drform1["Header"] = "H.S Signature";
                dtform1.Rows.Add(drform1);

                //magesh 3/1/18
                GridView3.Visible = true;
                if (dtRoomSeating.Rows.Count > 0)
                {
                    int total = 1;
                    int dept2 = 0;
                    int dpt2 = 0;
                    string det1 = "details";
                    for (int dept = 0; dept < dtBatchWiseDetail.Rows.Count; dept++)
                    {
                        dept2 = 0;
                        dpt2++;
                        // drform1 = dtform1.NewRow();
                        string year = string.Empty;
                        string regno = string.Empty;
                        m++;
                        string degreeCode = dtBatchWiseDetail.Rows[dept]["degree_code"].ToString();
                        string batchYear = dtBatchWiseDetail.Rows[dept]["Batch_Year"].ToString();
                        string sem = dtBatchWiseDetail.Rows[dept]["Current_Semester"].ToString();
                        string sections = dtBatchWiseDetail.Rows[dept]["Sections"].ToString();
                        string CourceId = dtBatchWiseDetail.Rows[dept]["Course_Name"].ToString();
                        string acroymn = dtBatchWiseDetail.Rows[dept]["dept_acronym"].ToString();
                        string subjectNo = dtBatchWiseDetail.Rows[dept]["subject_code"].ToString();
                        string subjectName = dtBatchWiseDetail.Rows[dept]["subject_name"].ToString();
                        if (sem == "1" || sem == "2")
                        {
                            year = "1 Year";
                        }
                        else if (sem == "3" || sem == "4")
                        {
                            year = "2 Year";
                        }
                        else if (sem == "5" || sem == "6")
                        {
                            year = "3 Year";
                        }
                        else if (sem == "7" || sem == "8")
                        {
                            year = "4 Year";
                        }
                        else
                        {
                            year = "";
                        }
                        string yearde = acroymn.ToString();
                        string deyear = year.ToString();
                        string depte = yearde + "-" + deyear;
                        int tol = Convert.ToInt32(ds.Tables[0].Rows[m - 1]["num"].ToString());
                        dtform1.Rows[dept2][det1] = Convert.ToString(ddlTestDate.SelectedItem.Text).Trim();
                        dept2++;
                        dtform1.Rows[dept2][det1] = Convert.ToString(depte);
                        dept2++;
                        dtform1.Rows[dept2][det1] = tol;
                        det1 = "details" + dpt2 + "";
                    }
                    GridView3.DataSource = dtform1;
                    GridView3.DataBind();
                    GridView3.Visible = true;
                }
            }
        }


        catch (Exception ex)
        {
            lblAlertMsg.Visible = true;
            lblAlertMsg.Text = ex.ToString();
            divPopAlert.Visible = true;
            da.sendErrorMail(ex, collegeCode, "InternalSeatingArrangement");
            return;
        }
    }

    protected void gridview2_DataBound(object sender, GridViewRowEventArgs e)
    {

        GridView2.ShowHeader = false;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].BackColor = Color.LightBlue;
            e.Row.Cells[0].Width = 60;
            e.Row.Cells[1].Width = 90;
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
        }

    }
    protected void gridview3_DataBound(object sender, GridViewRowEventArgs e)
    {
        GridView3.ShowHeader = false;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            e.Row.Cells[0].Width = 120;
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
        }
    }

    //magesh 2/1/2018
    protected void multihall()
    {
        try
        {
            string rl = string.Empty;
            int l1 = 0;
            int nrow = 0;
            int nrows = 0;
            int flag = 0;
            string roomno = string.Empty;
            string dat = string.Empty;
            string arrangeview1 = string.Empty;
            string arrangeviewNew = string.Empty;
            string allotSeat = string.Empty;
            int allotedSeats = 0;
            int allotedSeatsNew = 0;
            int ncol1 = 0;
            DataSet dsCollege = new DataSet();
            collegeCode = string.Empty;
            collegeCode = ddlCollege.SelectedItem.Value.ToString();
            GridView2.Visible = true;
            //Fpspread.Visible = true;
            string orderType = string.Empty;
            TableCell tcellRow = new TableCell();
            bool isRollNoVisible = ColumnHeaderVisiblity(0);
            bool isRegNoVisible = ColumnHeaderVisiblity(1);
            bool isAdmissionNoVisible = ColumnHeaderVisiblity(2);
            bool isAppNo = ColumnHeaderVisiblity(3);
            string sqlry = string.Empty;
            string frdate = string.Empty;
            string todate = string.Empty;
            string fotodate = string.Empty;
            DAccess2 da = new DAccess2();
            DataTable dtStudentInfo = new DataTable();

            contentDiv.InnerHtml = "";
            StringBuilder html = new StringBuilder();

            if (!string.IsNullOrEmpty(collegeCode.Trim()))
            {
                string qrynew = "select *,district+' - '+pincode  as districtpin from collinfo where college_code in (" + collegeCode + ")";
                dsCollege = dt.select_method_wo_parameter(qrynew, "Text");
            }
            string hallNo = string.Empty;
            string examSession = string.Empty;


            string clname = Convert.ToString(ddlCollege.SelectedItem.Text);


            html.Append("<center> <div style='height: 990px; width: 100%; border: 0px solid black; margin-left: 5px; margin: 0px; page-break-after: always;'> <center><div style='border: 0px solid black'>  <center>");

            html.Append("<table cellspacing='0' cellpadding='0' style='width: 700px; ' border='0'>");
            // html.Append("<tr><td>" + clname + "</td></tr>");
            html.Append("<tr><td style='width: 50px;'></td><td style='text-align: right;' > <img src=~/college/Left_Logo(" + Convert.ToString(Session["collegecode"]) + ").jpeg alt='' style='height: 100px; width: 120px;' /></td><td style='font-size: 12px; font-family: Book Antiqua;  border: 0px solid black; text-align: center;'><span style='font-size: 30px;font-weight:bold;'>" + clname + "</span> </td><td style='text-align: right;' > <img src=~/college/Right_Logo(" + Convert.ToString(Session["collegecode"]) + ").jpeg alt='' style='height: 100px; width: 120px;' /></td></tr> ");


            html.Append(" </table>");


            if (ddlTestDate.Items.Count > 0)
            {
                string ldate = ddlTestDate.SelectedItem.ToString();
                if (ldate.Trim() != "")
                {
                    string[] spl = ldate.Split('/');
                    DateTime dtl = Convert.ToDateTime(spl[1] + '/' + spl[0] + '/' + spl[2]);
                    testDate = dtl.ToString("yyyy-MM-dd");
                }
            }
            for (int i = 0; i < cblhall.Items.Count; i++)
            {

                if (cblhall.Items[i].Selected == true)
                {
                    int clum = 0;
                    int t = 0;
                    int v = 0;

                    hallNo = cblhall.Items[i].ToString().Trim();
                    examSession = ddlSession.SelectedItem.ToString().Trim();
                    //ds.Clear();                
                    rl = "select * from tbl_room_seats where Hall_No ='" + hallNo + "'";
                    ds = dt.select_method_wo_parameter(rl, "text");
                    int ncol = 0;
                    int m = 0;
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        flag = 1;
                        int.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["no_of_rows"]).Trim(), out nrow);
                        string sal = " select distinct s.subject_name,s.subject_code,COUNT(e.subjectNo) as num from internalSeatingArragement e,subject s where e.subjectNo=s.subject_no and e.hallNo='" + cblhall.Items[i].Text + "' and e.examDate='" + testDate.ToString() + "' and e.examSession='" + ddlSession.SelectedItem.Text + "' group by s.subject_name,s.subject_code order by COUNT(e.subjectNo) desc,s.subject_code";
                        DataTable dts = new DataTable();
                        dts = dirAcc.selectDataTable(sal);
                        nrows = nrow + dts.Rows.Count;
                        int.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["no_of_columns"]).Trim(), out ncol);

                        arrangeview1 = Convert.ToString(ds.Tables[0].Rows[0]["arranged_view"]).Trim();
                        arrangeviewNew = Convert.ToString(ds.Tables[0].Rows[0]["arrangedViewNew"]).Trim();

                        arran = arrangeview1.Replace(';', '-').Split('-');
                        Array.Sort(arran);
                        Array.Reverse(arran);
                        string myIndex = arran[0];
                        int colum = 0;
                        int.TryParse(Convert.ToInt64(myIndex).ToString(), out colum);
                        ncol *= colum;
                        ncol += 1;
                        allotseat = Convert.ToString(ds.Tables[0].Rows[0]["allocted_seats"]).Trim();
                        allotSeat = Convert.ToString(ds.Tables[0].Rows[0]["allotedSeatsNew"]).Trim();
                        int.TryParse(allotseat, out allotedSeats);
                        int.TryParse(allotSeat, out allotedSeatsNew);
                    }

                    int cols = ncol;

                    if (dsCollege.Tables.Count > 0 && dsCollege.Tables[0].Rows.Count > 0)
                    {
                        //maha
                        string frdatenew = string.Empty;
                        string todatenew = string.Empty;
                        string fotodatenew = string.Empty;
                        sqlry = "SELECT CONVERT(VARCHAR(50),min(exam_date),106) as fromdate,CONVERT(VARCHAR(50),max(exam_date),106) as todate FROM  CriteriaForInternal c,Exam_type e where c.Criteria_no=e.criteria_no and  c.criteria='" + ddlTest.SelectedItem.ToString() + "'";
                        DataTable dtTestDateNew = new DataTable();
                        dtTestDateNew = dirAcc.selectDataTable(sqlry);
                        if (dtTestDateNew.Rows.Count > 0)
                        {
                            frdatenew = Convert.ToString(dtTestDateNew.Rows[0]["fromdate"].ToString());

                            todatenew = Convert.ToString(dtTestDateNew.Rows[0]["todate"].ToString());
                            if (frdatenew == todatenew)
                            {
                                fotodatenew = frdatenew;
                            }
                            else
                            {
                                fotodatenew = frdatenew + '-' + todatenew;
                            }
                        }
                        sqlry = string.Empty;
                        sqlry = "select upper(convert(varchar(3),DateAdd(month,CONVERT(int, min(datepart(m,exam_date))),-1)))fromdate,upper(convert(varchar(3),DateAdd(month,CONVERT(int, max(datepart(m,exam_date))),-1)))todate from CriteriaForInternal c,Exam_type e where c.Criteria_no=e.criteria_no and  c.criteria='" + ddlTest.SelectedItem.ToString() + "'";
                        DataTable dtTestDate = new DataTable();
                        dtTestDate = dirAcc.selectDataTable(sqlry);
                        if (dtTestDate.Rows.Count > 0)
                        {
                            frdate = Convert.ToString(dtTestDate.Rows[0]["fromdate"].ToString());

                            todate = Convert.ToString(dtTestDate.Rows[0]["todate"].ToString());
                            if (todate == frdate)
                            {
                                fotodate = frdate;
                            }
                            else
                            {
                                fotodate = frdate + '/' + todate;
                            }
                        }
                        int coltop = 15;
                        string collegename = dsCollege.Tables[0].Rows[0]["Collname"].ToString();
                        string exam = "Examination - " + ddlTest.SelectedItem.ToString().ToUpper() + "-" + fotodatenew + " ";
                        string datesession = "Date & Session : " + Convert.ToString(ddlTestDate.SelectedItem.Text).Trim() + " & " + Convert.ToString(ddlSession.SelectedItem.Text).Trim();
                        string sign = Convert.ToString(txtsignature.Text).Trim();
                        string hallno = "Hall No : " + Convert.ToString(cblhall.Items[i].ToString()).Trim();
                        html.Append("<table style='margin-left:80px'  ><tr><td style='text-align: center;'>" + "SEATING ARRANGEMENT" + "</td></tr><tr><td style='text-align: left;'>" + datesession + "</td></tr><tr><td style='text-align: center;' >" + exam + "</td></tr><tr><td style='text-align: center;' >" + hallno + "</td></tr>");
                        html.Append("</table>");



                        arran = arrangeview1.Replace(';', '-').Split('-');
                        Array.Sort(arran);
                        Array.Reverse(arran);
                        string myIndex = arran[0];
                        int columnn = 0;
                        int.TryParse(Convert.ToInt64(myIndex).ToString(), out columnn);
                        #region setting arrangement
                        string sql1 = string.Empty;
                        sql1 = "select r.Reg_No,r.Roll_No,d.Acronym,r.Roll_Admit,r.Stud_Name,a.app_formno as ApplicationNo,es.*,s.subject_code from internalSeatingArragement es,subject s,Registration r,applyn a,Degree d where r.App_No=es.appNo and a.app_no=r.App_No and a.app_no=es.appNo and es.subjectNo=s.subject_no and d.Degree_Code=r.degree_code and es.hallNo='" + hallNo + "' and  es.examDate='" + testDate.ToString() + "' and  es.examSession='" + examSession + "' order by seatNo";
                        DataTable dts = new DataTable();
                        dts = dirAcc.selectDataTable(sql1);
                        if (flag == 1)
                        {
                            if (dts.Rows.Count > 0)
                            {
                                if (isRollNoVisible)
                                {
                                    orderType = "Roll_No";
                                }
                                else if (isRegNoVisible)
                                {
                                    orderType = "Reg_No";
                                }
                                else if (isAdmissionNoVisible)
                                {
                                    orderType = "Roll_Admit";

                                }
                                else if (isAppNo)
                                {
                                    orderType = "ApplicationNo";
                                }
                                else
                                {
                                    orderType = "Roll_No";
                                }
                                pnlContent1.Visible = true;

                                if (allotedSeats < dts.Rows.Count)
                                {
                                    arrang = arrangeviewNew.Split(';');
                                }
                                else
                                {
                                    arrang = arrangeview1.Split(';');
                                }
                                Dictionary<int, int> dicsubcol = new Dictionary<int, int>();
                                Dictionary<string, int> dicsubcolcount = new Dictionary<string, int>();
                                for (int spr = 0; spr <= arrang.GetUpperBound(0); spr++)
                                {
                                    string colsp = arrang[spr].ToString();
                                    if (colsp.Trim() != "" && colsp != null)
                                    {
                                        spcel = colsp.Split('-');
                                        for (int spc = 0; spc <= spcel.GetUpperBound(0); spc++)
                                        {
                                            int colsn = Convert.ToInt32(spcel[spc]);
                                            string strrow = "C" + spc + "R" + spr;
                                            if (!dicsubcolcount.ContainsKey(strrow))
                                            {
                                                dicsubcolcount.Add(strrow, colsn);
                                            }
                                            if (dicsubcol.ContainsKey(spc))
                                            {
                                                int valc = dicsubcol[spc];
                                                if (valc < colsn)
                                                {
                                                    dicsubcol[spc] = colsn;
                                                }
                                            }
                                            else
                                            {
                                                dicsubcol.Add(spc, colsn);
                                            }
                                        }
                                    }
                                }
                                dtfor1.Clear();
                                dtfor1.Columns.Add("row");

                                for (int h1 = 0; h1 < dicsubcol.Count; h1++)
                                {
                                    int sucol = dicsubcol[h1];
                                    for (int l = 1; l <= sucol; l++)
                                    {
                                        dtfor1.Columns.Add("C" + h1 + l + "");
                                    }

                                }

                                int count = 0;
                                int add = 0;
                                int getcouv = 0;
                                ArrayList addarr = new ArrayList();
                                TableCell tcell = new TableCell();
                                int autoChar = 97;
                                TableCell tcellnew = new TableCell();
                                tcellnew = new TableCell();
                                tcellnew.Width = 86;
                                //for (ncol = 0; ncol < cols; ncol++)
                                //{
                                //    //table2.Columns[ncol].SetWidth(500);
                                //}
                                html.Append("<table style='width:1120px;margin-left:190px'><tr>");
                                for (int h1 = 0; h1 < dicsubcol.Count; h1++)
                                {
                                    t++;

                                    int sucol = dicsubcol[h1];
                                    for (int l = sucol - 1; l < sucol; l++)
                                    {
                                        l1++;
                                        addarr.Clear();
                                        for (int j = 0; j < Convert.ToInt32(nrow); j++)
                                        {
                                            drfor1 = dtfor1.NewRow();
                                            string strrow = "C" + h1 + "R" + j;
                                            string seatValue = string.Empty;
                                            if (dicsubcolcount.ContainsKey(strrow))
                                            {
                                                getcouv = dicsubcolcount[strrow];
                                                count = add;
                                                addarr.Add(getcouv);
                                                for (int g = 0; g < getcouv; g++)
                                                {
                                                    hss++;
                                                    seatValue = Convert.ToString((j + 1) + (Convert.ToInt32(nrow) * g)) + Convert.ToString((char)autoChar);
                                                    // table1.Cell(1, count).SetContent(Convert.ToString(g + 1));
                                                    int countrow = j + 1;
                                                    // table2.Cell(j, 0).SetContent(Convert.ToString("Row:" + countrow + ""));
                                                    drfor1["row"] = "Row " + (countrow);
                                                    //table2.Cell(j, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    DataView dvStudent = new DataView();
                                                    if (dts.Rows.Count > 0)
                                                    {
                                                        dts.DefaultView.RowFilter = " seatCode='" + seatValue + "'";
                                                        dvStudent = dts.DefaultView;
                                                    }
                                                    if (dvStudent.Count > 0)
                                                    {
                                                        int g2 = g + 1;
                                                        drfor1["C" + h1 + g2 + ""] = dvStudent[0][orderType].ToString() + "  -[" + dvStudent[0]["seatNo"].ToString() + "]- " + dvStudent[0]["Acronym"].ToString();

                                                    }
                                                    count++;
                                                }
                                            }
                                            dtfor1.Rows.Add(drfor1);
                                        }
                                        if (addarr.Count > 0)
                                        {
                                            addarr.Sort();
                                        }
                                        add = add + Convert.ToInt32(addarr[addarr.Count - 1]);
                                        DataTable dtcol = new DataTable();
                                        dtcol.Columns.Add("column");
                                        dtcol.Columns.Add("colno");
                                        DataRow drcol;

                                        if (v < ncol + 1)
                                        {
                                            drcol = dtcol.NewRow();
                                            drcol["column"] = Convert.ToString(("Column" + (t)));

                                            v = v + sucol;
                                            html.Append("<td>" + Convert.ToString(("Column" + (t))) + "</td>");
                                            dtcol.Rows.Add(drcol);
                                        }
                                    }
                        #endregion
                                    clum += columnn;
                                    autoChar++;
                                }
                                html.Append("</tr></table>");

                            }
                            else
                            {
                                lblAlertMsg.Visible = true;
                                lblAlertMsg.Text = "No Record Found";
                                return;
                            }
                            html.Append("<table border='1px' style= 'border-collapse:collapse;border:1px solid black width:1500px; margin-left:60px' ;><tr><td></td>");
                            for (int i1 = 0; i1 < dtfor1.Columns.Count; i1++)
                            {
                                string col = Convert.ToString(dtfor1.Columns[i1].ColumnName);
                                string colval = col.Substring(2);
                                if (colval.ToUpper() != "W")
                                {
                                    html.Append("<td>" + colval + "</td>");
                                }
                                dtformat1.Columns.Add(Convert.ToString(dtfor1.Columns[i1].ColumnName));
                            }
                            html.Append("</tr>");
                            int count1 = Convert.ToInt32(dtformat1.Columns.Count);
                            int ct1 = 0;
                            for (int j1 = 0; j1 < Convert.ToInt32(nrow); j1++)
                            {
                                dicformat1.Clear();
                                drformat1 = dtformat1.NewRow();
                                string rw = "Row " + (j1 + 1) + "";
                                dtfor1.DefaultView.RowFilter = " row='" + rw + "'";
                                DataView dv = dtfor1.DefaultView;
                                if (dv.Count > 0)
                                {
                                    ct1++;
                                    for (int ct = 0; ct < dv.Count; ct++)
                                    {
                                        for (int i2 = 0; i2 < dv.Table.Columns.Count; i2++)
                                        {
                                            string str = Convert.ToString(dv[ct][i2]);
                                            if (!string.IsNullOrEmpty(str))
                                            {
                                                if (!dicformat1.ContainsValue(str))
                                                {

                                                    dicformat1.Add(i2, str);
                                                }
                                            }

                                        }
                                    }

                                }
                                if (dicformat1.Count > 0)
                                {
                                    html.Append("<tr>");
                                    foreach (KeyValuePair<int, string> dic in dicformat1)
                                    {
                                        int val = dic.Key;
                                        string strval = dic.Value;
                                        string colnam = Convert.ToString(dtformat1.Columns[val].ColumnName);
                                        drformat1[colnam] = strval;
                                        html.Append("<td>" + strval + "</td>");

                                    }

                                }
                                html.Append("</tr>");
                                dtformat1.Rows.Add(drformat1);


                            }




                            int totalcount = 0;
                            string sal = " select distinct s.subject_name,s.subject_code,COUNT(e.subjectNo) as num from internalSeatingArragement e,subject s where e.subjectNo=s.subject_no and e.hallNo='" + cblhall.Items[i].Text + "' and e.examDate='" + testDate.ToString() + "' and e.examSession='" + ddlSession.SelectedItem.Text + "' group by s.subject_name,s.subject_code order by COUNT(e.subjectNo) desc,s.subject_code";
                            dts = dirAcc.selectDataTable(sal);
                            int cerow = nrows - dts.Rows.Count;
                            if (dts.Rows.Count > 0)
                            {

                                for (int j = 0; j < dts.Rows.Count; j++)
                                {
                                    html.Append("<tr>");
                                    drformat1 = dtformat1.NewRow();
                                    string colname = Convert.ToString(dtformat1.Columns[1].ColumnName);
                                    drformat1[colname] = Convert.ToString(dts.Rows[j]["subject_code"]) + " - " + Convert.ToString(dts.Rows[j]["subject_name"]);

                                    string colcount = Convert.ToString(dtformat1.Columns.Count);
                                    int clct = Convert.ToInt32(colcount) - 1;
                                    string colnam = Convert.ToString(dtformat1.Columns[Convert.ToInt32(clct)].ColumnName);
                                    drformat1[colnam] = Convert.ToString(" Count : " + dts.Rows[j]["num"].ToString() + "");
                                    int clct2 = clct - 1;
                                    totalcount = totalcount + Convert.ToInt32(dts.Rows[j]["num"].ToString());
                                    html.Append("<td></td><td colspan=" + clct2 + ">" + Convert.ToString(dts.Rows[j]["subject_code"]) + " - " + Convert.ToString(dts.Rows[j]["subject_name"]) + "</td><td>" + " Count : " + dts.Rows[j]["num"].ToString() + "" + "</td></tr>");
                                    dtformat1.Rows.Add(drformat1);
                                    cerow++;
                                }
                                drformat1 = dtformat1.NewRow();

                                string colcount1 = Convert.ToString(dtformat1.Columns.Count);
                                int clct1 = Convert.ToInt32(colcount1) - 1;
                                string colnam1 = Convert.ToString(dtformat1.Columns[Convert.ToInt32(clct1)].ColumnName);
                                drformat1[colnam1] = "Total:" + totalcount + "";
                                int cout = dtformat1.Columns.Count - 1;
                                html.Append("<tr><td colspan=" + cout + "></td><td >" + "Total:" + totalcount + "" + "</td></tr>");
                                dtformat1.Rows.Add(drformat1);



                            }
                            html.Append("</table>");

                            string studentInfo1 = "select r.Reg_No,r.Roll_No,r.Roll_Admit,r.App_No,r.Stud_Name,r.Stud_Type,r.Current_Semester,LTRIM(RTRIM(ISNULL(r.Sections,''))) Sections,r.degree_code,r.Batch_Year,case when r.mode='1' then 'Regular' when r.mode='2' then 'Transfer' when r.mode='3' then 'Lateral' end as Mode,r.mode as ModeVal,r.isRedo,Convert(int,DATEPART(year,r.Adm_Date)) AS tempBatch,c.Course_Name+' - '+dt.dept_acronym as DeptName,c.Course_Name,dt.Dept_Name,dt.dept_acronym,s.subject_code,s.subject_name,ci.criteria,es.examDate,es.examSession,es.hallNo,es.seatNo from internalSeatingArragement es,CriteriaForInternal ci,Exam_type e ,class_master cs,syllabus_master sm,Course c,Degree dg,Department dt,Registration r,subject s where r.degree_code=sm.degree_code and r.Batch_Year=sm.Batch_Year and r.Current_Semester=sm.semester and r.App_No=es.appNo and c.Course_Id=dg.Course_Id and dt.Dept_Code=dg.Dept_Code and r.degree_code=sm.degree_code and sm.degree_code=dg.Degree_Code and ci.syll_code=sm.syll_code and s.syll_code=sm.syll_code and s.syll_code=ci.syll_code and s.subject_no=es.subjectNo and ci.Criteria_no=e.criteria_no and s.subject_no=e.subject_no and e.exam_code=es.examCode  and cs.rno=es.hallNo and es.examDate='" + testDate.ToString() + "' and es.examSession='" + ddlSession.SelectedItem.ToString().Trim() + "'  and ci.criteria='" + ddlTest.SelectedValue.ToString().Trim() + "' and es.hallNo='" + cblhall.Items[i].Text + "' order by es.examDate,es.examSession,es.hallNo,es.seatNo,r.Reg_No,r.Current_Semester";
                            //DataSet dsStudentInfo=da.select_method_wo_parameter(studentInfo,'text');
                            dtStudentInfo = dirAcc.selectDataTable(studentInfo1);
                            DataTable dtRoomSeating = new DataTable();
                            DataTable dtdicStudent = new DataTable();
                            DataTable dicStuCount = new DataTable();
                            DataTable dtBatchWiseDetail = new DataTable();
                            if (dtStudentInfo.Rows.Count > 0)
                            {
                                // dtStudentInfo.DefaultView.RowFilter = "hallNo='" + roomno + "'";
                                dtRoomSeating = dtStudentInfo.DefaultView.ToTable();
                                dtBatchWiseDetail = dtStudentInfo.DefaultView.ToTable(true, "degree_code", "Batch_Year", "Current_Semester", "Sections", "DeptName", "Course_Name", "subject_code", "dept_acronym", "subject_name");//Sections,r.degree_code,r.Batch_Year
                            }
                            Dictionary<string, string> dicval = new Dictionary<string, string>();
                            int.TryParse(Convert.ToString(dtBatchWiseDetail.Rows.Count).Trim(), out ncol1);
                            if (dtRoomSeating.Rows.Count > 0)
                            {
                                int total = 1;
                                string dat1 = string.Empty;
                                string noof_stud = string.Empty;
                                string deptm = string.Empty;
                                if (dtBatchWiseDetail.Rows.Count > 0)
                                {
                                    for (int dept = 0; dept < dtBatchWiseDetail.Rows.Count; dept++)
                                    {
                                        string year = string.Empty;
                                        string regno = string.Empty;
                                        m++;
                                        string degreeCode = dtBatchWiseDetail.Rows[dept]["degree_code"].ToString();
                                        string batchYear = dtBatchWiseDetail.Rows[dept]["Batch_Year"].ToString();
                                        string sem = dtBatchWiseDetail.Rows[dept]["Current_Semester"].ToString();
                                        string sections = dtBatchWiseDetail.Rows[dept]["Sections"].ToString();
                                        string CourceId = dtBatchWiseDetail.Rows[dept]["Course_Name"].ToString();
                                        string acroymn = dtBatchWiseDetail.Rows[dept]["dept_acronym"].ToString();
                                        string subjectNo = dtBatchWiseDetail.Rows[dept]["subject_code"].ToString();
                                        string subjectName = dtBatchWiseDetail.Rows[dept]["subject_name"].ToString();
                                        if (sem == "1" || sem == "2")
                                        {
                                            year = "1 Year";
                                        }
                                        else if (sem == "3" || sem == "4")
                                        {
                                            year = "2 Year";
                                        }
                                        else if (sem == "5" || sem == "6")
                                        {
                                            year = "3 Year";
                                        }
                                        else if (sem == "7" || sem == "8")
                                        {
                                            year = "4 Year";
                                        }
                                        else
                                        {
                                            year = "";
                                        }
                                        string yearde = acroymn.ToString();
                                        string deyear = year.ToString();
                                        string depte = yearde + "-" + deyear;
                                        int tol = Convert.ToInt32(dts.Rows[m - 1]["num"].ToString());
                                        if (string.IsNullOrEmpty(dat1))
                                            dat1 = Convert.ToString(ddlTestDate.SelectedItem.Text).Trim();
                                        else
                                            dat1 = dat1 + "," + Convert.ToString(ddlTestDate.SelectedItem.Text).Trim();
                                        if (string.IsNullOrEmpty(noof_stud))
                                            noof_stud = Convert.ToString(tol);
                                        else
                                            noof_stud = noof_stud + "," + Convert.ToString(tol);
                                        if (string.IsNullOrEmpty(deptm))
                                            deptm = Convert.ToString(depte);
                                        else
                                            deptm = deptm + "," + Convert.ToString(depte);
                                        if (dtBatchWiseDetail.Rows.Count == 1)
                                        {
                                            html.Append("<table border='1px' style= 'border-collapse:collapse;border:1px solid black;margin-top:30px'  > <tr><td style='text-align: left;'>" + "Date:" + "</td><td>" + Convert.ToString(ddlTestDate.SelectedItem.Text).Trim() + "</td></tr>   <tr><td style='text-align: left;'>" + "Dept:" + "</td><td>" + depte + "</td></tr>    <tr><td style='text-align: left;'>" + "No of Student Registered:" + "</td><td>" + tol + "</td></tr>  <tr><td style='text-align: left;'>" + "No of Student Present" + "</td><td></td></tr> <tr><td style='text-align: left;'>" + "No of Student Absent" + "</td><td></td></tr> <tr><td style='text-align: left;'>" + "Name of Hall Superitendent" + "</td><td></td></tr> <tr><td style='text-align: left;'>" + "H.S Signature" + "</td><td></td></tr> </table>");
                                        }

                                    }
                                    html.Append("<table border='1px' style= 'border-collapse:collapse;border:1px solid black;margin-top:30px'  > <tr><td style='text-align: left;'>Date</td>");
                                    string[] dt3 = dat1.Split(',');
                                    for (int k = 0; k < dt3.Length; k++)
                                    {
                                        html.Append("<td>" + dt3[k].ToString() + "</td>");
                                    }
                                    html.Append("</tr> <tr><td style='text-align: left;'>Dept</td>");
                                    string[] dt4 = deptm.Split(',');
                                    for (int k1 = 0; k1 < dt4.Length; k1++)
                                    {
                                        html.Append("<td>" + dt4[k1].ToString() + "</td>");
                                    }
                                    html.Append("</tr> <tr><td style='text-align: left;'>No of Student Registered</td>");
                                    string[] dt5 = noof_stud.Split(',');
                                    for (int k2 = 0; k2 < dt5.Length; k2++)
                                    {
                                        html.Append("<td>" + dt5[k2].ToString() + "</td>");
                                    }
                                    html.Append("</tr> <tr><td style='text-align: left;'>No of Student Present</td><td></td><td></td></tr> <tr><td style='text-align: left;'>No of Student Absent</td><td></td></td></td></tr> <tr><td style='text-align: left;'>Name of Hall Superitendent</td><td></td><td></td></tr> <tr><td style='text-align: left;'>H.S Signature</td><td>" + txtsignature.Text + "</td><td>" + txtsignature.Text + "</td></tr> </table>");
                                }
                            }


                        }
                    }

                }
            }
            html.Append("</center></div></center></div></center>");


            contentDiv.InnerHtml = html.ToString();
            contentDiv.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "btnprint", "PrintPanel2();", true);


        }
        catch (Exception ex) { da.sendErrorMail(ex, collegeCode, "InternalSeatingArrangement"); }

    }

    public void go()
    {
        try
        {
            string valBatch = string.Empty;
            string valDegree = string.Empty;
            string valBranch = string.Empty;
            string testName = string.Empty;
            string roomno = string.Empty;
            string regNo = string.Empty;
            string degreeCode = string.Empty;
            string CourceId = string.Empty;
            string subjectNo = string.Empty;
            string subjectName = string.Empty;
            string acroymn = string.Empty;
            string totalStudent = string.Empty;
            string sqlry = string.Empty;
            string frdate = string.Empty;
            string todate = string.Empty;
            string fotodate = string.Empty;

            btnDirectPrintF2.Visible = true;
            Hashtable hat = new Hashtable();
            DataTable dtExamDate = new DataTable();
            DataTable dtHall = new DataTable();
            DataTable dtStudentInfo = new DataTable();
            DataTable dtStuCount = new DataTable();
            ArrayList regStudUniq = new ArrayList();
            DataSet dsCollege = new DataSet();
            int b = 0;
            int add = 0;

            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            }
            else
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No " + lblCollege.Text + " Found";
                divPopAlert.Visible = true;
                return;
            }

            if (cblBatch.Items.Count == 0)
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No " + lblBatch.Text + " Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
                if (string.IsNullOrEmpty(valBatch))
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Select Atleast One " + lblBatch.Text + "";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            if (cblDegree.Items.Count == 0)
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No " + lblDegree.Text + " Found";
                divPopAlert.Visible = true;
                return;
            }

            else
            {
                valDegree = rs.GetSelectedItemsValueAsString(cblDegree);
                if (string.IsNullOrEmpty(valDegree))
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Select Atleast One " + lblDegree.Text + "";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            if (cblBranch.Items.Count == 0)
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No " + lblBranch.Text + " Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                valBranch = rs.GetSelectedItemsValueAsString(cblBranch);
                if (string.IsNullOrEmpty(valBranch))
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Select Atleast One " + lblBranch.Text + "";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            if (ddlTest.Items.Count > 0)
            {
                testName = ddlTest.SelectedItem.ToString().Trim();
            }
            else
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No " + lblTest.Text + " Found";
                divPopAlert.Visible = true;
                return;
            }
            if (ddlTestDate.Items.Count > 0)
            {
                string ldate = ddlTestDate.SelectedItem.ToString();
                if (ldate.Trim() != "")
                {
                    string[] spl = ldate.Split('/');
                    DateTime dtl = Convert.ToDateTime(spl[1] + '/' + spl[0] + '/' + spl[2]);
                    testDate = dtl.ToString("MM-dd-yyyy");
                }
            }
            else
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No " + lblTestDate.Text + " Found";
                divPopAlert.Visible = true;
                return;
            }
            if (!string.IsNullOrEmpty(collegeCode.Trim()))
            {
                string qrynew = "select *,district+' - '+pincode  as districtpin from collinfo where college_code in (" + collegeCode + ")";
                dsCollege = dt.select_method_wo_parameter(qrynew, "Text");
            }
            //print Setting
            if (dsCollege.Tables.Count > 0 && dsCollege.Tables[0].Rows.Count > 0)
            {
                string frdatenew = string.Empty;
                string todatenew = string.Empty;
                string fotodatenew = string.Empty;
                sqlry = "SELECT CONVERT(VARCHAR(50),min(exam_date),106) as fromdate,CONVERT(VARCHAR(50),max(exam_date),106) as todate FROM  CriteriaForInternal c,Exam_type e where c.Criteria_no=e.criteria_no and  c.criteria='" + ddlTest.SelectedItem.ToString() + "'";
                DataTable dtTestDateNew = new DataTable();
                dtTestDateNew = dirAcc.selectDataTable(sqlry);
                if (dtTestDateNew.Rows.Count > 0)
                {
                    frdatenew = Convert.ToString(dtTestDateNew.Rows[0]["fromdate"].ToString());

                    todatenew = Convert.ToString(dtTestDateNew.Rows[0]["todate"].ToString());
                    if (frdatenew == todatenew)
                    {
                        fotodatenew = frdatenew;
                    }
                    else
                    {
                        fotodatenew = frdatenew + '-' + todatenew;
                    }
                }
                sqlry = string.Empty;
                sqlry = "select upper(convert(varchar(3),DateAdd(month,CONVERT(int, min(datepart(m,exam_date))),-1)))fromdate,upper(convert(varchar(3),DateAdd(month,CONVERT(int, max(datepart(m,exam_date))),-1)))todate from CriteriaForInternal c,Exam_type e where c.Criteria_no=e.criteria_no and  c.criteria='" + ddlTest.SelectedItem.ToString() + "'";
                DataTable dtTestDate = new DataTable();
                dtTestDate = dirAcc.selectDataTable(sqlry);
                if (dtTestDate.Rows.Count > 0)
                {
                    frdate = Convert.ToString(dtTestDate.Rows[0]["fromdate"].ToString());

                    todate = Convert.ToString(dtTestDate.Rows[0]["todate"].ToString());
                    if (frdate == todate)
                    {
                        fotodate = frdate;
                    }
                    else
                    {
                        fotodate = frdate + '/' + todate;
                    }
                }
                //string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(Convert.ToString(ddlMonth.SelectedItem.Value).Trim()));
                //string[] strpa = Convert.ToString(dsCollege.Tables[0].Rows[0]["affliatedby"]).Trim().Split(',');
                spCollege.InnerText = Convert.ToString(dsCollege.Tables[0].Rows[0]["Collname"]).Trim();
                spController.InnerText = fotodatenew;
                spSeating.InnerText = "SEATING ARRANGEMENT";
                //Span4.InnerText = fotodatenew;
                // spAffBy.InnerText = (strpa.Length > 0) ? Convert.ToString(strpa[0]).Trim() : Convert.ToString(dsCollege.Tables[0].Rows[0]["affliatedby"]).Trim();
                spDateSession.InnerText = "Date & Session : " + Convert.ToString(ddlTestDate.SelectedItem.Text).Trim() + " & " + Convert.ToString(ddlSession.SelectedItem.Text).Trim();
                Span1.InnerText = "Examination - " + ddlTest.SelectedItem.ToString().ToUpper() + "-" + fotodate.ToString() + ""; //+ Convert.ToString(ddlYear.SelectedItem.Text);
                spHallNo.InnerText = "Hall No : " + Convert.ToString(ddlHallNo.SelectedItem.Text).Trim();

                Span3.InnerText = txtsignature.Text;
            }
            string examDate = "select distinct e.exam_date as exam_date,CONVERT(varchar(20),e.exam_date,101) as examDate from CriteriaForInternal ci,Registration r,syllabus_master sm,Exam_type e where ci.syll_code=sm.syll_code and ci.Criteria_no=e.criteria_no and r.Current_Semester=sm.semester and sm.Batch_Year=r.Batch_Year and sm.degree_code=r.degree_code and sm.Batch_Year in('" + valBatch + "') and sm.degree_code in('" + valBranch + "') and ci.criteria in('" + testName + "') and e.exam_date='" + testDate.ToString() + "' order by  e.exam_date asc";
            dtExamDate = dirAcc.selectDataTable(examDate);
            if (dtExamDate.Rows.Count > 0)
            {

            }
            else
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Please Set Time Table";
                divPopAlert.Visible = true;
                return;
            }
            //string dicHall = "select es.hallNo,cs.block,cs.priority from internalSeatingArragement es,class_master cs  where cs.rno=es.hallNo and es.examDate='" + testDate.ToString() + "' and es.examSession='" + ddlSession.SelectedItem.ToString().Trim() + "' and cs.coll_code in('" + collegeCode + "') order by  cs.priority";
            string dicHall = "select distinct cs.rno,cs.block,cs.priority from internalSeatingArragement es,CriteriaForInternal ci,Exam_type e ,class_master cs,syllabus_master sm where sm.syll_code=ci.syll_code and ci.Criteria_no=e.criteria_no and es.hallNo=cs.rno  and es.examDate='" + testDate.ToString() + "' and es.examSession='" + ddlSession.SelectedItem.ToString().Trim() + "' and ci.criteria='" + ddlTest.SelectedValue.ToString().Trim() + "' order by cs.priority ";
            dtHall = dirAcc.selectDataTable(dicHall);
            if (dtHall.Rows.Count > 0)
            {

            }
            else
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No Hall were Found";
                divPopAlert.Visible = true;
                return;
            }
            string studentInfo = "select r.Reg_No,r.Roll_No,r.Roll_Admit,r.App_No,r.Stud_Name,r.Stud_Type,r.Current_Semester,LTRIM(RTRIM(ISNULL(r.Sections,''))) Sections,r.degree_code,r.Batch_Year,case when r.mode='1' then 'Regular' when r.mode='2' then 'Transfer' when r.mode='3' then 'Lateral' end as Mode,r.mode as ModeVal,r.isRedo,Convert(int,DATEPART(year,r.Adm_Date)) AS tempBatch,c.Course_Name+' - '+dt.dept_acronym as DeptName,c.Course_Name,dt.Dept_Name,dt.dept_acronym,s.subject_code,s.subject_name,ci.criteria,es.examDate,es.examSession,es.hallNo,es.seatNo from internalSeatingArragement es,CriteriaForInternal ci,Exam_type e ,class_master cs,syllabus_master sm,Course c,Degree dg,Department dt,Registration r,subject s where r.degree_code=sm.degree_code and r.Batch_Year=sm.Batch_Year and r.Current_Semester=sm.semester and r.App_No=es.appNo and c.Course_Id=dg.Course_Id and dt.Dept_Code=dg.Dept_Code and r.degree_code=sm.degree_code and sm.degree_code=dg.Degree_Code and ci.syll_code=sm.syll_code and s.syll_code=sm.syll_code and s.syll_code=ci.syll_code and s.subject_no=es.subjectNo and ci.Criteria_no=e.criteria_no and s.subject_no=e.subject_no and e.exam_code=es.examCode  and cs.rno=es.hallNo and es.examDate='" + testDate.ToString() + "' and es.examSession='" + ddlSession.SelectedItem.ToString().Trim() + "'  and ci.criteria='" + ddlTest.SelectedValue.ToString().Trim() + "' order by es.examDate,es.examSession,es.hallNo,es.seatNo,r.Reg_No,r.Current_Semester";
            //DataSet dsStudentInfo=da.select_method_wo_parameter(studentInfo,'text');
            dtStudentInfo = dirAcc.selectDataTable(studentInfo);
            if (dtStudentInfo.Rows.Count > 0)
            {

            }
            else
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No student were Found";
                divPopAlert.Visible = true;
                return;
            }
            string StudCount = "select Count(distinct r.App_No) as TotalStudent,ci.criteria,es.examDate,es.examSession,es.hallNo,r.degree_code,r.Batch_Year,LTRIM(RTRIM(ISNULL(r.Sections,''))) Sections,s.subject_code from internalSeatingArragement es,CriteriaForInternal ci,Exam_type e ,class_master cs,syllabus_master sm,Course c,Degree dg,Department dt,Registration r,subject s where r.degree_code=sm.degree_code and r.Batch_Year=sm.Batch_Year and r.Current_Semester=sm.semester and r.App_No=es.appNo and c.Course_Id=dg.Course_Id and dt.Dept_Code=dg.Dept_Code and r.degree_code=sm.degree_code and sm.degree_code=dg.Degree_Code and ci.syll_code=sm.syll_code and s.syll_code=sm.syll_code and s.syll_code=ci.syll_code and s.subject_no=es.subjectNo and s.subject_no=e.subject_no and ci.Criteria_no=e.criteria_no and e.exam_code=es.examCode  and cs.rno=es.hallNo and es.examDate='" + testDate.ToString() + "' and es.examSession='" + ddlSession.SelectedItem.ToString().Trim() + "' and ci.criteria='" + ddlTest.SelectedValue.ToString().Trim() + "' group by ci.criteria,es.examDate,es.examSession,es.hallNo,r.degree_code,r.Batch_Year,s.subject_code,LTRIM(RTRIM(ISNULL(r.Sections,'')))";
            dtStuCount = dirAcc.selectDataTable(StudCount);
            if (dtStuCount.Rows.Count > 0)
            {

            }
            else
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No student were Found";
                divPopAlert.Visible = true;
                return;
            }
            dtfor2.Columns.Add("department");
            dtfor2.Columns.Add("subjectcode");
            dtfor2.Columns.Add("regnum");
            dtfor2.Columns.Add("hallno");
            dtfor2.Columns.Add("totalstud");

            if (dtHall.Rows.Count > 0)
            {
                for (int f = 0; f < dtHall.Rows.Count; f++)
                {
                    int rowcount = 0;
                    roomno = Convert.ToString(dtHall.Rows[f]["rno"]);
                    int total = 1;
                    DataTable dtRoomSeating = new DataTable();
                    DataTable dtdicStudent = new DataTable();
                    DataTable dicStuCount = new DataTable();

                    DataTable dtBatchWiseDetail = new DataTable();
                    if (dtStudentInfo.Rows.Count > 0)
                    {
                        dtStudentInfo.DefaultView.RowFilter = "hallNo='" + roomno + "'";
                        dtRoomSeating = dtStudentInfo.DefaultView.ToTable();
                        dtBatchWiseDetail = dtStudentInfo.DefaultView.ToTable(true, "degree_code", "Batch_Year", "Current_Semester", "Sections", "DeptName", "Course_Name", "subject_code", "dept_acronym", "subject_name");//Sections,r.degree_code,r.Batch_Year
                    }

                    if (dtRoomSeating.Rows.Count > 0)
                    {
                        total = 1;
                        for (int dept = 0; dept < dtBatchWiseDetail.Rows.Count; dept++)
                        {
                            drfor2 = dtfor2.NewRow();
                            string year = string.Empty;
                            string regno = string.Empty;
                            degreeCode = dtBatchWiseDetail.Rows[dept]["degree_code"].ToString();
                            string batchYear = dtBatchWiseDetail.Rows[dept]["Batch_Year"].ToString();
                            string sem = dtBatchWiseDetail.Rows[dept]["Current_Semester"].ToString();
                            string sections = dtBatchWiseDetail.Rows[dept]["Sections"].ToString();
                            CourceId = dtBatchWiseDetail.Rows[dept]["Course_Name"].ToString();
                            acroymn = dtBatchWiseDetail.Rows[dept]["dept_acronym"].ToString();
                            subjectNo = dtBatchWiseDetail.Rows[dept]["subject_code"].ToString();
                            subjectName = dtBatchWiseDetail.Rows[dept]["subject_name"].ToString();
                            if (sem == "1" || sem == "2")
                            {
                                year = "1 Year";
                            }
                            else if (sem == "3" || sem == "4")
                            {
                                year = "2 Year";
                            }
                            else if (sem == "5" || sem == "6")
                            {
                                year = "3 Year";
                            }
                            else if (sem == "7" || sem == "8")
                            {
                                year = "4 Year";
                            }
                            else
                            {
                                year = "";
                            }

                            drfor2["department"] = CourceId + " - " + acroymn.ToString() + "-" + "(" + year + ")";
                            drfor2["subjectcode"] = subjectNo + "-" + subjectName;
                            drfor2["hallno"] = roomno.ToString();


                            dtStudentInfo.DefaultView.RowFilter = "degree_code='" + degreeCode + "' and Batch_Year='" + batchYear + "' and Sections='" + sections + "' and hallNo='" + roomno + "' and subject_code='" + subjectNo + "'";
                            dtdicStudent = dtStudentInfo.DefaultView.ToTable();
                            if (dtdicStudent.Rows.Count > 0)
                            {
                                for (b = 0; b < dtdicStudent.Rows.Count; b++)
                                {
                                    if (!regStudUniq.Contains(dtdicStudent.Rows[b]["Reg_No"].ToString()))
                                    {
                                        if (regno == "")
                                        {
                                            regno = dtdicStudent.Rows[b]["Reg_No"].ToString();
                                        }
                                        else
                                        {
                                            regno = regno + ",  " + dtdicStudent.Rows[b]["Reg_No"].ToString();
                                            total++;
                                        }
                                        regStudUniq.Add(dtdicStudent.Rows[b]["Reg_No"].ToString());
                                    }
                                }
                            }

                            if (dtStuCount.Rows.Count > 0)
                            {
                                dtStuCount.DefaultView.RowFilter = "degree_code='" + degreeCode + "'  and Batch_Year='" + batchYear + "' and Sections='" + sections + "' and hallNo='" + roomno + "'  and subject_code='" + subjectNo + "'";
                                dicStuCount = dtStuCount.DefaultView.ToTable();
                            }
                            totalStudent = dicStuCount.Rows[0]["TotalStudent"].ToString();
                            if (!hat.ContainsKey(roomno))
                            {
                                hat.Add(roomno, totalStudent);
                                add = 0;
                                add = add + Convert.ToInt32(totalStudent);
                            }
                            else
                            {
                                add = add + Convert.ToInt32(totalStudent);
                            }

                            //from to formate
                            DataTable dicBatchwiseStudent = new DataTable();
                            DataTable dtStroll = new DataTable();
                            dtStroll = dtdicStudent.DefaultView.ToTable(true, "tempBatch", "degree_code");
                            DataTable dtBatch = dtdicStudent.DefaultView.ToTable(true, "batch_year");
                            if (dtStroll.Rows.Count > 0)
                            {
                                int max = 0;
                                List<object> lstBatch = dtBatch.AsEnumerable().Select(r => r.Field<object>("batch_year")).ToList();
                                dtStudentInfo.DefaultView.RowFilter = "ModeVal='1'  and Batch_Year='" + batchYear + "' and Sections='" + sections + "' " + ((lstBatch.Count > 0) ? " and tempBatch in('" + string.Join("','", lstBatch.ToArray()) + "')" : "");
                                DataTable dtMaxBatch = dtStudentInfo.DefaultView.ToTable();

                                if (dtMaxBatch.Rows.Count == 0)
                                {
                                    dtStudentInfo.DefaultView.RowFilter = "ModeVal='1'  and Batch_Year='" + batchYear + "' and Sections='" + sections + "' and degree_code='" + degreeCode + "'";
                                    dtMaxBatch = dtStudentInfo.DefaultView.ToTable();
                                }
                                if (dtMaxBatch.Rows.Count > 0)
                                {
                                    List<int> studentList = dtMaxBatch.AsEnumerable().Select(r => r.Field<int>("tempBatch")).ToList();
                                    max = studentList.Max();
                                }
                                DataTable dtStrollNew = new DataTable();
                                bool currentBatch = false;
                                if (dtStroll.Rows.Count > 0)
                                {
                                    string finalRegNo = string.Empty;
                                    for (int i = 0; i < dtStroll.Rows.Count; i++)
                                    {
                                        string listBatch = Convert.ToString(dtStroll.Rows[i]["tempBatch"]).Trim();
                                        dtStudentInfo.DefaultView.RowFilter = "tempBatch='" + listBatch + "'  and Batch_Year='" + batchYear + "' and Sections='" + sections + "' and degree_code='" + degreeCode + "' and hallNo='" + roomno + "'";
                                        dtStudentInfo.DefaultView.Sort = "Reg_No asc";
                                        dicBatchwiseStudent = dtStudentInfo.DefaultView.ToTable();
                                        if (dicBatchwiseStudent.Rows.Count > 0)
                                        {
                                            int batch = Convert.ToInt32(dicBatchwiseStudent.Rows[0]["tempBatch"]);
                                            regno = dicBatchwiseStudent.Rows[0]["Reg_No"].ToString();
                                            if (batch == max)
                                            {
                                                if (currentBatch == false)
                                                {
                                                    string rollCount = string.Empty;
                                                    dicBatchwiseStudent.DefaultView.RowFilter = "Batch_Year='" + batch + "' and degree_code='" + degreeCode + "'";

                                                    currentBatch = true;
                                                    if (currentBatch == true)
                                                    {
                                                        DataRow dr = (DataRow)dicBatchwiseStudent.Rows[dicBatchwiseStudent.Rows.Count - 1];
                                                        string latReg = dr["Reg_No"].ToString();
                                                        if (dicBatchwiseStudent.Rows.Count > 1)
                                                            finalRegNo += "  " + regno.ToString() + "-" + latReg;
                                                        else
                                                            finalRegNo += "  " + regno.ToString();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                for (int j = 0; j < dicBatchwiseStudent.Rows.Count; j++)
                                                {
                                                    regno = dicBatchwiseStudent.Rows[j]["Reg_No"].ToString();
                                                    finalRegNo += "  " + regno.ToString() + ",";
                                                }
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(finalRegNo))
                                        {
                                            drfor2["regnum"] = finalRegNo;
                                        }

                                    }
                                }
                            }
                            drfor2["totalstud"] = b.ToString();
                            dtfor2.Rows.Add(drfor2);
                            pnlContents.Visible = true;
                            divFormat2.Visible = true;
                            total = 0;
                            hat.Clear();
                        }
                        GridView1.DataSource = dtfor2;
                        GridView1.DataBind();


                    }
                }
                GridView1.Visible = true;
                divFormat2.Visible = true;

            }

            else
            {

            }
        }
        catch (Exception ex)
        {

        }
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

    protected void btnGo_Click(object sender, EventArgs e)
    {
        divFormat1.Visible = false;
        divFormat2.Visible = false;
        btnDirectPrintF2.Visible = false;
        Span2.Attributes.Add("style", "display:none;");
        Span3.Attributes.Add("style", "display:none;");
        Span4.Attributes.Add("style", "display:none;");
        if (Radioformat1.Checked == true)
        {
            if (Multiple.Checked == true)
            {

                multihall();
            }
            else
            {
                ddlHallNo.Enabled = true;
                report();
                btn_directprint.Visible = true;

                GridView2.Visible = true;
                //Fpspread.Visible = true;
            }
        }
        else if (Radioformat2.Checked == true)
        {
            ddlHallNo.Enabled = false;
            go();
        }
    }
    protected void directprint_Click()
    {

        Span2.Attributes.Add("style", "display:block;");
        imgLeftLogo2.Visible = true;
        DataSet dsCollege = new DataSet();
        Font Fontbold1 = new Font("Algerian", 15, FontStyle.Bold);
        Font font2bold = new Font("Palatino Linotype", 11, FontStyle.Bold);
        Font font2small = new Font("Palatino Linotype", 11, FontStyle.Regular);
        Font font3bold = new Font("Palatino Linotype", 9, FontStyle.Bold);
        Font font3small = new Font("Palatino Linotype", 9, FontStyle.Regular);
        Font font4bold = new Font("Palatino Linotype", 7, FontStyle.Bold);
        Font font4small = new Font("Palatino Linotype", 7, FontStyle.Regular);
        Boolean flag = true;
        System.Drawing.Font Fontboldhead = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
        System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold);
        System.Drawing.Font Fontbolda = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
        System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Regular);
        System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
        System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
        System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
        System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
        System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);
        System.Drawing.Font Fontmediumv = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
        System.Drawing.Font Fontmedium1V = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
        System.Drawing.Font f1 = new System.Drawing.Font("Book Antiqua", 7, FontStyle.Regular);
        System.Drawing.Font f2 = new System.Drawing.Font("Book Antiqua", 8, FontStyle.Regular);
        System.Drawing.Font f3 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
        System.Drawing.Font f4 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
        System.Drawing.Font f5 = new System.Drawing.Font("Book Antiqua", 11, FontStyle.Regular);
        System.Drawing.Font f6 = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
        System.Drawing.Font f7 = new System.Drawing.Font("Book Antiqua", 7, FontStyle.Bold);
        System.Drawing.Font f8 = new System.Drawing.Font("Book Antiqua", 8, FontStyle.Bold);
        System.Drawing.Font f9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold);
        System.Drawing.Font f10 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
        System.Drawing.Font f11 = new System.Drawing.Font("Book Antiqua", 11, FontStyle.Bold);
        System.Drawing.Font f12 = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
        string coename = string.Empty;
        string strquery = "select *,district+' - '+pincode  as districtpin from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
        ds.Dispose();
        ds.Reset();
        ds = dt.select_method_wo_parameter(strquery, "Text");
        string Collegename = string.Empty;
        string aff = string.Empty;
        string collacr = string.Empty;
        string dispin = string.Empty;
        string sqlry = string.Empty;
        string frdate = string.Empty;
        string todate = string.Empty;
        string fotodate = string.Empty;

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            sqlry = "select upper(convert(varchar(3),DateAdd(month,CONVERT(int, min(datepart(m,exam_date))),-1)))fromdate,upper(convert(varchar(3),DateAdd(month,CONVERT(int, max(datepart(m,exam_date))),-1)))todate from CriteriaForInternal c,Exam_type e where c.Criteria_no=e.criteria_no and  c.criteria='" + ddlTest.SelectedItem.ToString() + "'";
            DataTable dtTestDate = new DataTable();
            dtTestDate = dirAcc.selectDataTable(sqlry);
            if (dtTestDate.Rows.Count > 0)
            {
                frdate = Convert.ToString(dtTestDate.Rows[0]["fromdate"].ToString());

                todate = Convert.ToString(dtTestDate.Rows[0]["todate"].ToString());
                fotodate = frdate + '/' + todate;
            }

            //string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(Convert.ToString(ddlMonth.SelectedItem.Value).Trim()));
            // string[] strpa = Convert.ToString(ds.Tables[0].Rows[0]["affliatedby"]).Trim().Split(',');
            spF1College.InnerText = Convert.ToString(ds.Tables[0].Rows[0]["Collname"]).Trim();
            // spF1Controller.InnerText = "OFFICE OF THE CONTROLLER OF EXAMINATIONS";
            spF1Seating.InnerText = "SEATING ARRANGEMENT";
            // spF1Aff.InnerText = (strpa.Length > 0) ? Convert.ToString(strpa[0]).Trim() : Convert.ToString(ds.Tables[0].Rows[0]["affliatedby"]).Trim();
            spF1Date.InnerText = "Date & Session : " + Convert.ToString(ddlTestDate.SelectedItem.Text).Trim() + " & " + Convert.ToString(ddlSession.SelectedItem.Text).Trim();
            spExamination.InnerText = "Examination - " + ddlTest.SelectedItem.ToString().ToUpper() + " "; //+ Convert.ToString(ddlYear.SelectedItem.Text);
            spHallNo.InnerText = "Hall No : " + Convert.ToString(ddlHallNo.SelectedItem.Text).Trim();
            Span2.InnerText = "Coordinator of Examination";
        }


    }

    protected void btn_directprint_Click(object sender, EventArgs e)
    {
        Span2.Attributes.Add("style", "display:block;");
        imgLeftLogo2.Visible = true;
        DataSet dsCollege = new DataSet();
        Font Fontbold1 = new Font("Algerian", 15, FontStyle.Bold);
        Font font2bold = new Font("Palatino Linotype", 11, FontStyle.Bold);
        Font font2small = new Font("Palatino Linotype", 11, FontStyle.Regular);
        Font font3bold = new Font("Palatino Linotype", 9, FontStyle.Bold);
        Font font3small = new Font("Palatino Linotype", 9, FontStyle.Regular);
        Font font4bold = new Font("Palatino Linotype", 7, FontStyle.Bold);
        Font font4small = new Font("Palatino Linotype", 7, FontStyle.Regular);
        Boolean flag = true;
        System.Drawing.Font Fontboldhead = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
        System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
        System.Drawing.Font Fontbolda = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
        System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Regular);
        System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
        System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
        System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
        System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
        System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);
        System.Drawing.Font Fontmediumv = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
        System.Drawing.Font Fontmedium1V = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
        System.Drawing.Font f1 = new System.Drawing.Font("Book Antiqua", 7, FontStyle.Regular);
        System.Drawing.Font f2 = new System.Drawing.Font("Book Antiqua", 8, FontStyle.Regular);
        System.Drawing.Font f3 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
        System.Drawing.Font f4 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
        System.Drawing.Font f5 = new System.Drawing.Font("Book Antiqua", 11, FontStyle.Regular);
        System.Drawing.Font f6 = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
        System.Drawing.Font f7 = new System.Drawing.Font("Book Antiqua", 7, FontStyle.Bold);
        System.Drawing.Font f8 = new System.Drawing.Font("Book Antiqua", 8, FontStyle.Bold);
        System.Drawing.Font f9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold);
        System.Drawing.Font f10 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
        System.Drawing.Font f11 = new System.Drawing.Font("Book Antiqua", 11, FontStyle.Bold);
        System.Drawing.Font f12 = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
        string coename = string.Empty;
        string strquery = "select *,district+' - '+pincode  as districtpin from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
        ds.Dispose();
        ds.Reset();
        ds = dt.select_method_wo_parameter(strquery, "Text");
        string Collegename = string.Empty;
        string aff = string.Empty;
        string collacr = string.Empty;
        string dispin = string.Empty;
        string sqlry = string.Empty;
        string frdate = string.Empty;
        string todate = string.Empty;
        string fotodate = string.Empty;

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            sqlry = "select upper(convert(varchar(3),DateAdd(month,CONVERT(int, min(datepart(m,exam_date))),-1)))fromdate,upper(convert(varchar(3),DateAdd(month,CONVERT(int, max(datepart(m,exam_date))),-1)))todate from CriteriaForInternal c,Exam_type e where c.Criteria_no=e.criteria_no and  c.criteria='" + ddlTest.SelectedItem.ToString() + "'";
            DataTable dtTestDate = new DataTable();
            dtTestDate = dirAcc.selectDataTable(sqlry);
            if (dtTestDate.Rows.Count > 0)
            {
                frdate = Convert.ToString(dtTestDate.Rows[0]["fromdate"].ToString());

                todate = Convert.ToString(dtTestDate.Rows[0]["todate"].ToString());
                fotodate = frdate + '/' + todate;
            }

            //string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(Convert.ToString(ddlMonth.SelectedItem.Value).Trim()));
            // string[] strpa = Convert.ToString(ds.Tables[0].Rows[0]["affliatedby"]).Trim().Split(',');
            spF1College.InnerText = Convert.ToString(ds.Tables[0].Rows[0]["Collname"]).Trim();
            // spF1Controller.InnerText = "OFFICE OF THE CONTROLLER OF EXAMINATIONS";
            spF1Seating.InnerText = "SEATING ARRANGEMENT";
            // spF1Aff.InnerText = (strpa.Length > 0) ? Convert.ToString(strpa[0]).Trim() : Convert.ToString(ds.Tables[0].Rows[0]["affliatedby"]).Trim();
            spF1Date.InnerText = "Date & Session : " + Convert.ToString(ddlTestDate.SelectedItem.Text).Trim() + " & " + Convert.ToString(ddlSession.SelectedItem.Text).Trim();
            spExamination.InnerText = "Examination - " + ddlTest.SelectedItem.ToString().ToUpper() + " "; //+ Convert.ToString(ddlYear.SelectedItem.Text);
            spHallNo.InnerText = "Hall No : " + Convert.ToString(ddlHallNo.SelectedItem.Text).Trim();
            Span2.InnerText = "Coordinator of Examination";
        }

    }

    protected void Radioformat1_CheckedChanged(object sender, EventArgs e)
    {
        divFormat1.Visible = false;
        divFormat2.Visible = false;
        ddlHallNo.Enabled = true;
    }

    protected void Radioformat2_CheckedChanged(object sender, EventArgs e)
    {
        divFormat1.Visible = false;
        divFormat2.Visible = false;
        ddlHallNo.Enabled = false;
    }

    protected void btnF2_directprint_Click(object sender, EventArgs e)
    {
        imgLeftLogo2.Visible = true;
        DataSet dsCollege = new DataSet();
        Font Fontbold1 = new Font("Algerian", 15, FontStyle.Bold);
        Font font2bold = new Font("Palatino Linotype", 11, FontStyle.Bold);
        Font font2small = new Font("Palatino Linotype", 11, FontStyle.Regular);
        Font font3bold = new Font("Palatino Linotype", 9, FontStyle.Bold);
        Font font3small = new Font("Palatino Linotype", 9, FontStyle.Regular);
        Font font4bold = new Font("Palatino Linotype", 7, FontStyle.Bold);
        Font font4small = new Font("Palatino Linotype", 7, FontStyle.Regular);
        Boolean flag = true;
        System.Drawing.Font Fontboldhead = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
        System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
        System.Drawing.Font Fontbolda = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
        System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Regular);
        System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
        System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
        System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
        System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
        System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);
        System.Drawing.Font Fontmediumv = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
        System.Drawing.Font Fontmedium1V = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
        System.Drawing.Font f1 = new System.Drawing.Font("Book Antiqua", 7, FontStyle.Regular);
        System.Drawing.Font f2 = new System.Drawing.Font("Book Antiqua", 8, FontStyle.Regular);
        System.Drawing.Font f3 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
        System.Drawing.Font f4 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
        System.Drawing.Font f5 = new System.Drawing.Font("Book Antiqua", 11, FontStyle.Regular);
        System.Drawing.Font f6 = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
        System.Drawing.Font f7 = new System.Drawing.Font("Book Antiqua", 7, FontStyle.Bold);
        System.Drawing.Font f8 = new System.Drawing.Font("Book Antiqua", 8, FontStyle.Bold);
        System.Drawing.Font f9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold);
        System.Drawing.Font f10 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
        System.Drawing.Font f11 = new System.Drawing.Font("Book Antiqua", 11, FontStyle.Bold);
        System.Drawing.Font f12 = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
        string coename = string.Empty;
        string strquery = "select *,district+' - '+pincode  as districtpin from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
        ds.Dispose();
        ds.Reset();
        ds = dt.select_method_wo_parameter(strquery, "Text");
        string Collegename = string.Empty;
        string aff = string.Empty;
        string collacr = string.Empty;
        string dispin = string.Empty;
        string sqlry = string.Empty;
        string frdate = string.Empty;
        string todate = string.Empty;
        string fotodate = string.Empty;

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            sqlry = "select upper(convert(varchar(3),DateAdd(month,CONVERT(int, min(datepart(m,exam_date))),-1)))fromdate,upper(convert(varchar(3),DateAdd(month,CONVERT(int, max(datepart(m,exam_date))),-1)))todate from CriteriaForInternal c,Exam_type e where c.Criteria_no=e.criteria_no and  c.criteria='" + ddlTest.SelectedItem.ToString() + "'";
            DataTable dtTestDate = new DataTable();
            dtTestDate = dirAcc.selectDataTable(sqlry);
            if (dtTestDate.Rows.Count > 0)
            {
                frdate = Convert.ToString(dtTestDate.Rows[0]["fromdate"].ToString());

                todate = Convert.ToString(dtTestDate.Rows[0]["todate"].ToString());
                fotodate = frdate + '/' + todate;
            }


            spCollege.InnerText = Convert.ToString(ds.Tables[0].Rows[0]["Collname"]).Trim();
            spSeating.InnerText = "SEATING ARRANGEMENT";
            Span1.InnerText = "Examination - " + ddlTest.SelectedItem.ToString().ToUpper() + "";
            Span3.InnerText = "Coordinator of Examination";
        }
    }

    protected void btnMissingStudent_Click(object sender, EventArgs e)
    {
    }

    protected void chkCommonSeating_CheckedChanged(object sender, EventArgs e)
    {
        //if (chkCommonSeating.Checked == true)
        //{
        //    btnMissingStudent.Visible = true;
        //    lblHall.Visible = false;
        //}
    }
}