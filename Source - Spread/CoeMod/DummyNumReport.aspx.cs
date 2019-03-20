/*
 * Page created by Idhris 23-02-2017
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using InsproDataAccess;
using System.Web.UI.WebControls;
using System.Text;
using System.Linq;
using System.Drawing;
using System.Configuration;

public partial class CoeMod_DummyNumReport : System.Web.UI.Page
{
    InsproDirectAccess dirAccess = new InsproDirectAccess();
    ReuasableMethods reUse = new ReuasableMethods();

    string collegeCode = string.Empty;
    string userCode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

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
            collegeCode = Session["collegecode"].ToString();
            userCode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();

            if (!Page.IsPostBack)
            {
                try
                {
                    bindCollege();
                    cb_College_CheckedChanged(sender, e);

                    string StreamShift = string.Empty;
                    try
                    {
                        StreamShift = Convert.ToString(Session["streamcode"]);
                        if (StreamShift.Trim() == "")
                        {
                            StreamShift = "Stream";
                        }
                    }
                    catch { StreamShift = "Stream"; }
                    lbl_stream.Text = StreamShift;
                }
                catch
                {
                }
            }
            else
            {
                clearSpread();
            }
            collegeCode = reUse.GetSelectedItemsValue(cbl_College);
        }
        catch (Exception ex)
        {
        }
    }
    public void bindCollege()
    {
        try
        {
            txt_College.Text = "College";
            cb_College.Checked = true;
            cbl_College.Items.Clear();
            string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + userCode + " and cp.college_code=cf.college_code";
            DataTable dtCollege = dirAccess.selectDataTable(selectQuery);
            if (dtCollege.Rows.Count > 0)
            {
                cbl_College.DataSource = dtCollege;
                cbl_College.DataTextField = "collname";
                cbl_College.DataValueField = "college_code";
                cbl_College.DataBind();
            }
            reUse.CallCheckBoxChangedEvent(cbl_College, cb_College, txt_College, "College");
        }
        catch { }
    }
    protected void cb_College_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            reUse.CallCheckBoxChangedEvent(cbl_College, cb_College, txt_College, "College");
            collegeCode = reUse.GetSelectedItemsValue(cbl_College);
            bindyear();
            bindmonth();
            ddlYear_SelectedIndexChanged(sender, e);
            //typeChange();
            bindType();
            binddegree2();
            bindbranch1();
            chkIsDept_CheckedChange(sender, e);
        }
        catch { }
    }
    protected void cbl_College_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            reUse.CallCheckBoxListChangedEvent(cbl_College, cb_College, txt_College, "College");
            collegeCode = reUse.GetSelectedItemsValue(cbl_College);
            bindyear();
            bindmonth();
            ddlYear_SelectedIndexChanged(sender, e);
            //typeChange();
            bindType();
            binddegree2();
            bindbranch1();
            chkIsDept_CheckedChange(sender, e);
        }
        catch { }
    }
    public void bindyear()
    {
        try
        {
            ddlYear.Items.Clear();
            if (string.IsNullOrEmpty(collegeCode))
            {
                return;
            }

            DataSet ds = reUse.Examyear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlYear.DataSource = ds;
                ddlYear.DataTextField = "Exam_year";
                ddlYear.DataValueField = "Exam_year";
                ddlYear.DataBind();
            }
        }
        catch { }
    }
    public void bindmonth()
    {
        try
        {
            ddlMonth.Items.Clear();

            string year = ddlYear.SelectedItem.Text;
            DataTable dtMon = dirAccess.selectDataTable("select distinct Exam_month,upper(convert(varchar(3),DateAdd(month,Exam_month,-1))) as monthName from exam_details where Exam_year='" + year + "'");
            if (dtMon.Rows.Count > 0)
            {
                ddlMonth.DataSource = dtMon;
                ddlMonth.DataValueField = "Exam_month";
                ddlMonth.DataTextField = "monthName";
                ddlMonth.DataBind();
            }
        }
        catch { }
    }
    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadExamDate();
        loadSubject();
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindmonth();
        loadExamDate();
        loadSubject();
    }
    protected void ddlGenType_IndexChange(object sender, EventArgs e)
    {
        if (ddlGenType.SelectedIndex == 0)
        {
            trSubjectDet.Visible = false;
        }
        else
        {
            trSubjectDet.Visible = true;
            loadExamDate();
            loadSubject();
        }
    }
    private void loadExamDate()
    {
        try
        {
            ddlExDate.Items.Clear();
            string selQ = "select distinct convert(varchar(10),exdt.Exam_date,105) as exam_date,exdt.Exam_date from exmtt_det as exdt,exmtt as exm where exm.exam_code=exdt.exam_code and exm.exam_month=" + ddlMonth.SelectedItem.Value.ToString() + " and exm.exam_year=" + ddlYear.SelectedValue.ToString() + "  and exdt.coll_code in (" + collegeCode + ")  order by exdt.Exam_date";
            DataTable dtExDate = dirAccess.selectDataTable(selQ);
            if (dtExDate.Rows.Count > 0)
            {
                ddlExDate.DataSource = dtExDate;
                ddlExDate.DataTextField = "Exam_date";
                ddlExDate.DataValueField = "Exam_date";
                ddlExDate.DataBind();
            }
        }
        catch { }
    }
    protected void ddlExdate_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loadSubject();
        }
        catch { }
    }
    protected void ddlsession_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loadSubject();
        }
        catch { }
    }
    private void loadSubject()
    {
        ddlsubject.Items.Clear();
        try
        {
            string examMonth = ddlMonth.SelectedValue.Trim();
            string examYear = ddlYear.SelectedValue.Trim();

            //string subjectQ = "select distinct subject_name+'-'+subject_code as subjectNameCode,subject_code from exam_application ea, Exam_Details ed,exam_appl_details ead,subject s where s.subject_no=ead.subject_no and ead.appl_no=ea.appl_no and  ea.exam_code = ed.exam_code and ed.Exam_Month='" + examMonth + "' and ed.Exam_year='" + examYear + "'";
            string session = string.Empty;
            if (ddlsession.SelectedIndex != 0)
            {
                session = " and exam_session ='" + ddlsession.SelectedItem.Text + " ' ";
            }
            string subjectQ = "select distinct s.subject_Name+'-'+s.subject_code as subjectNameCode ,s.Subject_code  from subject s,exmtt e,exmtt_det ex,sub_sem where sub_sem.subtype_no=s.subtype_no  and s.subject_no=ex.subject_no and ex.coll_code in (" + collegeCode + ") and ex.exam_Date=convert(datetime,'" + ddlExDate.SelectedValue.ToString() + "',103)and ex.exam_code=e.exam_code and e.Exam_Month=" + ddlMonth.SelectedValue.ToString() + " and e.Exam_Year=" + ddlYear.SelectedValue.ToString() + " and e.exam_type='Univ' " + session;
            DataTable dtSubject = dirAccess.selectDataTable(subjectQ);
            if (dtSubject.Rows.Count > 0)
            {
                ddlsubject.DataSource = dtSubject;
                ddlsubject.DataTextField = "subjectNameCode";
                ddlsubject.DataValueField = "subject_code";
                ddlsubject.DataBind();

                ddlsubject.Items.Insert(0, "All");
            }
        }
        catch { }
    }
    //If Department wise report
    protected void chkIsDept_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (!chkIsDept.Checked)
            {
                ddl_strm.Enabled = false;
                txt_degree2.Enabled = false;
                txt_branch2.Enabled = false;
            }
            else
            {
                ddl_strm.Enabled = true;
                txt_degree2.Enabled = true;
                txt_branch2.Enabled = true;
            }
        }
        catch { }
    }
    public void bindType()
    {
        try
        {
            ddl_strm.Items.Clear();
            reUse.bindStreamToDropDown(ddl_strm, Convert.ToString(collegeCode));
            if (ddl_strm.Items.Count > 0)
                ddl_strm.Enabled = true;
            else
                ddl_strm.Enabled = false;
        }
        catch (Exception ex) { }
    }
    protected void ddl_strm_OnIndexChange(object sender, EventArgs e)
    {
        binddegree2();
        bindbranch1();
    }
    protected void cb_strm_OnCheckedChanged(object sender, EventArgs e)
    {
        // CallCheckBoxChangedEvent(cbl_strm, cb_strm, txt_strm, lbl_stream.Text);
        binddegree2();
        bindbranch1();

    }
    protected void cbl_strm_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //CallCheckBoxListChangedEvent(cbl_strm, cb_strm, txt_strm, lbl_stream.Text);
        binddegree2();
        bindbranch1();

    }
    protected void cbl_branch1_SelectedIndexChanged(object sender, EventArgs e)
    {
        reUse.CallCheckBoxListChangedEvent(cbl_branch1, cb_branch1, txt_branch2, "Branch");

    }
    protected void cb_branch1_ChekedChange(object sender, EventArgs e)
    {
        reUse.CallCheckBoxChangedEvent(cbl_branch1, cb_branch1, txt_branch2, "Branch");

    }
    protected void cbl_degree2_SelectedIndexChanged(object sender, EventArgs e)
    {
        reUse.CallCheckBoxListChangedEvent(cbl_degree2, cb_degree2, txt_degree2, "Degree");
        bindbranch1();

    }
    protected void cb_degree2_ChekedChange(object sender, EventArgs e)
    {
        reUse.CallCheckBoxChangedEvent(cbl_degree2, cb_degree2, txt_degree2, "Degree");
        bindbranch1();
    }
    public void binddegree2()
    {
        try
        {
            DataSet ds = new DataSet();
            cbl_degree2.Items.Clear();
            string stream = "";
            stream = ddl_strm.Items.Count > 0 ? ddl_strm.SelectedValue : "";


            txt_degree2.Text = "--Select--";

            string useCOdeSet = "select LinkValue from New_InsSettings where LinkName='MultipleCollegeUserRights' and user_code ='" + userCode + "' and college_code in ('" + collegeCode + "' )";
            string colleges = Convert.ToString(reUse.GetFunction(useCOdeSet)).Trim();
            if (colleges == "" || colleges == "0")
            {
                colleges = collegeCode;
            }

            string query = "select distinct degree.course_id,course.course_name from degree,course where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code in (" + collegeCode + ") ";
            if (ddl_strm.Enabled)//if (txt_strm.Enabled)
            {
                query += " and course.type in ('" + stream + "')";
            }
            ds = reUse.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree2.DataSource = ds;
                cbl_degree2.DataTextField = "course_name";
                cbl_degree2.DataValueField = "course_id";
                cbl_degree2.DataBind();
                if (cbl_degree2.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree2.Items.Count; i++)
                    {
                        cbl_degree2.Items[i].Selected = true;
                    }
                    txt_degree2.Text = "Degree(" + cbl_degree2.Items.Count + ")";
                    cb_degree2.Checked = true;
                }
                else
                {
                    txt_degree2.Text = "--Select--";
                }
            }
            else
            {
                txt_degree2.Text = "--Select--";
            }

        }
        catch (Exception ex) { }
    }
    public void bindbranch1()
    {
        try
        {
            cbl_branch1.Items.Clear();

            string branch = "";
            for (int i = 0; i < cbl_degree2.Items.Count; i++)
            {
                if (cbl_degree2.Items[i].Selected == true)
                {
                    if (branch == "")
                    {
                        branch = "" + cbl_degree2.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        branch = branch + "'" + "," + "" + "'" + cbl_degree2.Items[i].Value.ToString() + "";
                    }
                }
            }
            string commname = "";
            if (branch != "")
            {
                //commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code ";
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') ";
            }
            else
            {
                //commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code";
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code ";
            }
            if (branch.Trim() != "")
            {
                DataSet ds = reUse.select_method_wo_parameter(commname, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch1.DataSource = ds;
                    cbl_branch1.DataTextField = "dept_name";
                    cbl_branch1.DataValueField = "degree_code";
                    cbl_branch1.DataBind();



                    if (cbl_branch1.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_branch1.Items.Count; i++)
                        {
                            cbl_branch1.Items[i].Selected = true;
                        }
                        txt_branch2.Text = "Branch(" + cbl_branch1.Items.Count + ")";
                        cb_branch1.Checked = true;
                    }
                }
                else
                {
                    txt_branch2.Text = "--Select--";
                }
            }
            else
            {
                txt_branch2.Text = "--Select--";
            }
        }
        catch { }
    }
    //View Report
    protected void btnViewReport_Click(object sender, EventArgs e)
    {
        try
        {
            string examMonth = ddlMonth.SelectedValue.Trim();
            string examYear = ddlYear.SelectedValue.Trim();
            byte dummyType = (byte)ddlGenMethod.SelectedIndex;// 0 - Serial , 1 - Random

            if (ddlGenType.SelectedIndex == 0)
            {
                //Common
                showCommonReport(examMonth, examYear, dummyType);
            }
            else
            {
                //SubjectWise
                string subjectCode = ddlsubject.SelectedValue.ToString();
                showSubjectWiseReport(examMonth, examYear, dummyType, subjectCode);
            }
        }
        catch { }
    }
    //Common
    private void showCommonReport(string examMonth, string examYear, byte dummyType)
    {
        string degreeCode;
        if (chkIsDept.Checked)
        {
            StringBuilder sbDegCodes = new StringBuilder();
            for (int braI = 0; braI < cbl_branch1.Items.Count; braI++)
            {
                if (cbl_branch1.Items[braI].Selected)
                {
                    sbDegCodes.Append(cbl_branch1.Items[braI].Value + ",");
                }
            }
            if (sbDegCodes.Length > 1)
            {
                degreeCode = " and d.degree_code in (" + sbDegCodes.Remove(sbDegCodes.Length - 1, 1).ToString() + ") ";
            }
            else
            {
                degreeCode = " and d.degree_code in ('0') ";
            }
        }
        else
        {
            degreeCode = string.Empty;
        }

        string studQ = "select dn.dummy_no,r.reg_no,r.Roll_No,r.Stud_Name,r.degree_code,(c.Course_Name+'-'+dt.Dept_Name) as Branch,r.college_code,d.Dept_Priority  from dummynumber dn,Registration r,Course c, Degree d, Department dt where r.Reg_No=dn.regno and r.degree_code=d.degree_code and c.Course_Id=d.Course_Id and dt.Dept_Code=d.Dept_Code and dn.exam_month='" + examMonth + "' and dn.exam_year='" + examYear + "' and dn.DNCollegeCode in ('" + collegeCode + "')  and ISNULL(dn.subject,'') in ('')  and dn.dummy_type='" + dummyType + "' " + degreeCode + "  order by d.Dept_Priority ";

        DataTable dtCommonDet = dirAccess.selectDataTable(studQ);
        if (dtCommonDet.Rows.Count > 0)
            loadCommonSpread(dtCommonDet);
    }
    private void loadCommonSpread(DataTable dtSpreadData)
    {
         
        spreadReport.Sheets[0].RowCount = dtSpreadData.Rows.Count;
        spreadReport.Sheets[0].ColumnCount = 0;
        spreadReport.Sheets[0].ColumnHeader.RowCount = 1;
        spreadReport.CommandBar.Visible = false;
        spreadReport.Sheets[0].ColumnCount = 6;

        spreadReport.Sheets[0].RowHeader.Visible = false;
        spreadReport.Sheets[0].AutoPostBack = false;


        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        darkstyle.ForeColor = Color.Black;
        spreadReport.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

        spreadReport.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
        spreadReport.Sheets[0].Columns[0].Locked = true;
        spreadReport.Columns[0].Width = 50;

        spreadReport.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Reg No";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
        spreadReport.Sheets[0].Columns[1].Locked = true;
        spreadReport.Columns[1].Width = 150;
        spreadReport.Columns[1].CellType = txt;

        spreadReport.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
        spreadReport.Sheets[0].Columns[2].Locked = true;
        spreadReport.Columns[2].Width = 100;
        spreadReport.Columns[2].Visible = false;

        spreadReport.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Dummy No";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
        spreadReport.Sheets[0].Columns[3].Locked = true;
        spreadReport.Columns[3].Width = 100;

        spreadReport.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Name";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
        spreadReport.Sheets[0].Columns[4].Locked = true;
        spreadReport.Columns[4].Width = 250;

        spreadReport.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Branch";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
        spreadReport.Sheets[0].Columns[5].Locked = true;
        spreadReport.Columns[5].Width = 250;

        spreadReport.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        spreadReport.Sheets[0].Columns[0].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].Columns[0].Font.Name = "Book Antiqua";

        spreadReport.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
        spreadReport.Sheets[0].Columns[1].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].Columns[1].Font.Name = "Book Antiqua";

        spreadReport.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
        spreadReport.Sheets[0].Columns[2].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].Columns[2].Font.Name = "Book Antiqua";

        spreadReport.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
        spreadReport.Sheets[0].Columns[3].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].Columns[3].Font.Name = "Book Antiqua";

        spreadReport.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
        spreadReport.Sheets[0].Columns[4].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].Columns[4].Font.Name = "Book Antiqua";

        spreadReport.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
        spreadReport.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
        spreadReport.Sheets[0].Columns[5].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].Columns[5].Font.Name = "Book Antiqua";
        spreadReport.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);

        for (int rowI = 0; rowI < dtSpreadData.Rows.Count; rowI++)
        {
            spreadReport.Sheets[0].Cells[rowI, 0].Text = (rowI + 1).ToString();
            spreadReport.Sheets[0].Cells[rowI, 1].Text = Convert.ToString(dtSpreadData.Rows[rowI]["reg_no"]);
            spreadReport.Sheets[0].Cells[rowI, 1].Tag = Convert.ToString(dtSpreadData.Rows[rowI]["college_code"]);
            spreadReport.Sheets[0].Cells[rowI, 2].Text = Convert.ToString(dtSpreadData.Rows[rowI]["roll_no"]);
            spreadReport.Sheets[0].Cells[rowI, 3].Text = Convert.ToString(dtSpreadData.Rows[rowI]["dummy_no"]);
            spreadReport.Sheets[0].Cells[rowI, 4].Text = Convert.ToString(dtSpreadData.Rows[rowI]["stud_name"]);
            spreadReport.Sheets[0].Cells[rowI, 5].Text = Convert.ToString(dtSpreadData.Rows[rowI]["branch"]);
            spreadReport.Sheets[0].Cells[rowI, 5].Tag = Convert.ToString(dtSpreadData.Rows[rowI]["degree_code"]);
           
        }

        spreadReport.Sheets[0].PageSize = spreadReport.Sheets[0].RowCount;
        spreadReport.SaveChanges();
        spreadReport.Visible = true;
        rptprint.Visible = true;
    }
    //Subject wise
    private void showSubjectWiseReport(string examMonth, string examYear, byte dummyType, string subjectCode)
    {
        string[] examDateArr = ddlExDate.SelectedItem.Text.Split('-');
        string examDate = examDateArr[1] + "/" + examDateArr[0] + "/" + examDateArr[2];
        string subject_code;

        if (ddlsubject.SelectedIndex == 0)
        {
            StringBuilder sbSubjectCodes = new StringBuilder();
            for (int itemI = 1; itemI < ddlsubject.Items.Count; itemI++)
            {
                sbSubjectCodes.Append(ddlsubject.Items[itemI].Value + "','");
            }
            if (sbSubjectCodes.Length > 3)
                sbSubjectCodes.Remove(sbSubjectCodes.Length - 3, 3);
            subject_code = sbSubjectCodes.ToString();
        }
        else
        {
            subject_code = ddlsubject.SelectedValue.ToString();
        }

        string session = string.Empty;
        if (ddlsession.SelectedIndex != 0)
        {
            session = " and ed.exam_session='" + ddlsession.SelectedItem.Text + "' ";
        }

        string degreeCode;
        if (chkIsDept.Checked)
        {
            StringBuilder sbDegCodes = new StringBuilder();
            for (int braI = 0; braI < cbl_branch1.Items.Count; braI++)
            {
                if (cbl_branch1.Items[braI].Selected)
                {
                    sbDegCodes.Append(cbl_branch1.Items[braI].Value + ",");
                }
            }
            if (sbDegCodes.Length > 1)
            {
                degreeCode = " and d.degree_code in (" + sbDegCodes.Remove(sbDegCodes.Length - 1, 1).ToString() + ") ";
            }
            else
            {
                degreeCode = " and d.degree_code in ('0') ";
            }
        }
        else
        {
            degreeCode = string.Empty;
        }

        string studQ = "select dn.dummy_no,r.reg_no,r.Roll_No,r.Stud_Name,r.degree_code,(c.Course_Name+'-'+dt.Dept_Name) as Branch,r.college_code,dn.subject_no,dn.subject,convert(varchar(10),ed.exam_date,103) as exDate,s.subject_name,d.Dept_Priority  from dummynumber dn,Registration r,Course c, Degree d, Department dt,subject s,exmtt_det ed  where  ed.exam_date=dn.exam_date and ed.subject_no=dn.subject_no and s.subject_no=ed.subject_no and r.Reg_No=dn.regno and r.degree_code=d.degree_code and c.Course_Id=d.Course_Id and dt.Dept_Code=d.Dept_Code and s.subject_no=dn.subject_no and s.subject_code=dn.subject and dn.exam_month='" + examMonth + "' and dn.exam_year='" + examYear + "' and dn.DNCollegeCode in ('" + collegeCode + "') and ISNULL(dn.subject,'') in ('" + subject_code + "')  and dn.dummy_type='" + dummyType + "' and dn.exam_date='" + examDate + "' " + session + degreeCode + "  order by subject,d.Dept_Priority ";

        DataTable dtSubjectDet = dirAccess.selectDataTable(studQ);
        if (dtSubjectDet.Rows.Count > 0)
            loadSubjectSpread(dtSubjectDet);
    }
    private void loadSubjectSpread(DataTable dtSpreadData)
    {
       
        spreadReport.Sheets[0].RowCount = dtSpreadData.Rows.Count;
        spreadReport.Sheets[0].ColumnCount = 0;
        spreadReport.Sheets[0].ColumnHeader.RowCount = 1;
        spreadReport.CommandBar.Visible = false;
        spreadReport.Sheets[0].ColumnCount = 9;

        spreadReport.Sheets[0].RowHeader.Visible = false;
        spreadReport.Sheets[0].AutoPostBack = false;


        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        darkstyle.ForeColor = Color.Black;
        spreadReport.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

        spreadReport.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
        spreadReport.Sheets[0].Columns[0].Locked = true;
        spreadReport.Columns[0].Width = 50;

        spreadReport.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Exam Date";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
        spreadReport.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
        spreadReport.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
        spreadReport.Sheets[0].Columns[1].Locked = true;
        spreadReport.Columns[1].Width = 100;

        spreadReport.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
        spreadReport.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
        spreadReport.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
        spreadReport.Sheets[0].Columns[2].Locked = true;
        spreadReport.Columns[2].Width = 150;

        spreadReport.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Subject Code";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
        spreadReport.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
        spreadReport.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
        spreadReport.Sheets[0].Columns[3].Locked = true;
        spreadReport.Columns[3].Width = 100;

        spreadReport.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Reg No";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
        spreadReport.Sheets[0].Columns[4].Locked = true;
        spreadReport.Columns[4].Width = 150;

        spreadReport.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Roll No";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
        spreadReport.Sheets[0].Columns[5].Locked = true;
        spreadReport.Columns[5].Width = 100;
        spreadReport.Columns[5].Visible = false;

        spreadReport.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Dummy No";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
        spreadReport.Sheets[0].Columns[6].Locked = true;
        spreadReport.Columns[6].Width = 100;

        spreadReport.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Name";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
        spreadReport.Sheets[0].Columns[7].Locked = true;
        spreadReport.Columns[7].Width = 180;

        spreadReport.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Branch";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
        spreadReport.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;
        spreadReport.Sheets[0].SetColumnMerge(8, FarPoint.Web.Spread.Model.MergePolicy.Always);
        spreadReport.Sheets[0].Columns[8].Locked = true;
        spreadReport.Columns[8].Width = 150;

        spreadReport.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        spreadReport.Sheets[0].Columns[0].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].Columns[0].Font.Name = "Book Antiqua";

        spreadReport.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
        spreadReport.Sheets[0].Columns[1].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].Columns[1].Font.Name = "Book Antiqua";

        spreadReport.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
        spreadReport.Sheets[0].Columns[2].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].Columns[2].Font.Name = "Book Antiqua";

        spreadReport.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
        spreadReport.Sheets[0].Columns[3].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].Columns[3].Font.Name = "Book Antiqua";

        spreadReport.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
        spreadReport.Sheets[0].Columns[4].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].Columns[4].Font.Name = "Book Antiqua";

        spreadReport.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
        spreadReport.Sheets[0].Columns[5].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].Columns[5].Font.Name = "Book Antiqua";

        spreadReport.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
        spreadReport.Sheets[0].Columns[6].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].Columns[6].Font.Name = "Book Antiqua";

        spreadReport.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;
        spreadReport.Sheets[0].Columns[7].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].Columns[7].Font.Name = "Book Antiqua";

        spreadReport.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Left;
        spreadReport.Sheets[0].Columns[8].Font.Size = FontUnit.Medium;
        spreadReport.Sheets[0].Columns[8].Font.Name = "Book Antiqua";

        for (int rowI = 0; rowI < dtSpreadData.Rows.Count; rowI++)
        {
            spreadReport.Sheets[0].Cells[rowI, 0].Text = (rowI + 1).ToString();
            spreadReport.Sheets[0].Cells[rowI, 1].Text = Convert.ToString(dtSpreadData.Rows[rowI]["exDate"]);
            spreadReport.Sheets[0].Cells[rowI, 2].Text = Convert.ToString(dtSpreadData.Rows[rowI]["subject_name"]);
            spreadReport.Sheets[0].Cells[rowI, 3].Text = Convert.ToString(dtSpreadData.Rows[rowI]["subject"]);
            spreadReport.Sheets[0].Cells[rowI, 3].Tag = Convert.ToString(dtSpreadData.Rows[rowI]["subject_no"]);
            spreadReport.Sheets[0].Cells[rowI, 4].Text = Convert.ToString(dtSpreadData.Rows[rowI]["reg_no"]);
            spreadReport.Sheets[0].Cells[rowI, 4].Tag = Convert.ToString(dtSpreadData.Rows[rowI]["college_code"]);
            spreadReport.Sheets[0].Cells[rowI, 5].Text = Convert.ToString(dtSpreadData.Rows[rowI]["roll_no"]);
            spreadReport.Sheets[0].Cells[rowI, 6].Text = Convert.ToString(dtSpreadData.Rows[rowI]["dummy_no"]);
            spreadReport.Sheets[0].Cells[rowI, 7].Text = Convert.ToString(dtSpreadData.Rows[rowI]["stud_name"]);
            spreadReport.Sheets[0].Cells[rowI, 8].Text = Convert.ToString(dtSpreadData.Rows[rowI]["branch"]);
            spreadReport.Sheets[0].Cells[rowI, 8].Tag = Convert.ToString(dtSpreadData.Rows[rowI]["degree_code"]);
            
        }
       

        spreadReport.Sheets[0].PageSize = spreadReport.Sheets[0].RowCount;
        spreadReport.SaveChanges();
        spreadReport.Visible = true;
        rptprint.Visible = true;
    }
    private void clearSpread()
    {
        if (spreadReport.Visible)
        {
            Control ctrlid = GetPostBackControl(this.Page);
            string btnid = Convert.ToString(ctrlid.ClientID);

            if (btnid != "MainContent_btn_excel" && btnid != "MainContent_btn_printmaster" && !btnid.Contains("Printcontrol"))
            {
                spreadReport.Visible = false;
                rptprint.Visible = false;
            }
        }
    }
    public static Control GetPostBackControl(Page page)
    {
        Control control = null;
        string ctrlname = page.Request.Params.Get("__EVENTTARGET");
        if (ctrlname != null && ctrlname != string.Empty)
        {
            control = page.FindControl(ctrlname);
        }
        else
        {
            foreach (string ctl in page.Request.Form)
            {
                Control c = page.FindControl(ctl);
                if (c is System.Web.UI.WebControls.Button)
                {
                    control = c;
                    break;
                }
            }
        }
        return control;
    }
    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Challan Datewise Report";
            string pagename = "DummyNumReport.aspx";
            Printcontrol.loadspreaddetails(spreadReport, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch { }
    }
    protected void btn_excel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txt_excelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                reUse.printexcelreport(spreadReport, reportname);
            }
        }
        catch { }
    }
    //Last modified by Idhris 06-03-2017
}