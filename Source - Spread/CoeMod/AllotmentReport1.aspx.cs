using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class CoeMod_AllotmentReport1 : System.Web.UI.Page
{

    string usercode = string.Empty;
    static string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string sessstream = string.Empty;
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
    string batch2 = "";
    string degree = "";

    string collegeCode = string.Empty;
    string eduLevel = string.Empty;
    string qryEduLevel = string.Empty;
    string batchYear = string.Empty;
    string courseId = string.Empty;
    string degreeCode = string.Empty;
    string semester = string.Empty;
    string examMonth = string.Empty;

    string orderBy = string.Empty;
    string orderBySetting = string.Empty;

    string qry = string.Empty;
    string qryCollegeCode = string.Empty;
    string qryCollegeCode1 = string.Empty;
    string qryBatchYear = string.Empty;
    string qryDegreeCode = string.Empty;
    string qrySemester = string.Empty;
    string examYear = string.Empty;
    string qryExamYear = string.Empty;
    string streamNames = string.Empty;
    string qryStream = string.Empty;
    string qryCourseId = string.Empty;
    string qryExamMonth = string.Empty;
    int ACTROW = 0;
    Boolean cellclick = false;

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
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                loadcollege();
                if (ddl_collegename.Items.Count > 0)
                    collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);

                BindExamYear();
                BindExamMonth();
                showreport1.Visible = false;
                showreport2.Visible = false;
                getPrintSettings2();
                getPrintSettings1();
            }
        }
        catch (Exception ex)
        {
        }
    }
    

    #region college
    public void loadcollege()
    {
        try
        {
            string strUser = d2.getUserCode(Convert.ToString(Session["group_code"]), Convert.ToString(Session["usercode"]), 1);
            ds.Clear();
            ddl_collegename.Items.Clear();
            string Query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where " + strUser + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegename.DataSource = ds;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
            }
        }
        catch
        { }
    }

    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }

          
            BindExamYear();
            BindExamMonth();
            showreport1.Visible = false;
            showreport2.Visible = false;

        }
        catch
        {
        }
    }
    #endregion

    #region examyear
    public void BindExamYear()
    {
        try
        {
            ddlExamYear.Items.Clear();
            ds.Clear();

            batchYear = string.Empty;
            collegeCode = string.Empty;
            streamNames = string.Empty;

            qryCollegeCode = string.Empty;
            qryBatchYear = string.Empty;
            qryDegreeCode = string.Empty;

            if (ddl_collegename.Items.Count > 0 && ddl_collegename.Visible)
            {
                collegeCode = Convert.ToString(ddl_collegename.SelectedValue).Trim();
            }

            if (!string.IsNullOrEmpty(collegeCode))
            {
                qryCollegeCode = " and dg.college_code in(" + collegeCode + ")";
            }

          
           
            if (!string.IsNullOrEmpty(qryCollegeCode))
            {
                string qry = "select distinct ed.Exam_year from exam_details ed,Degree dg,Course c,Department dt where dg.Degree_Code=ed.degree_code and  c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and ed.Exam_year<>'0' " + qryCollegeCode + qryDegreeCode + qryBatchYear + " order by ed.Exam_year desc";
                ds.Clear();
                ds.Reset();
                ds.Dispose();
                ds = da.select_method_wo_parameter(qry, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlExamYear.DataSource = ds;
                    ddlExamYear.DataTextField = "Exam_year";
                    ddlExamYear.DataValueField = "Exam_year";
                    ddlExamYear.DataBind();
                    ddlExamYear.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlExamYear_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            showreport1.Visible = false;
            showreport2.Visible = false;
            BindExamMonth();
            
        }
        catch (Exception ex)
        {

        }
    }

    #endregion

    #region exammonth
    private void BindExamMonth()
    {
        try
        {
            ddlExamMonth.Items.Clear();
            ds.Clear();
            examYear = string.Empty;
           
            collegeCode = string.Empty;
          

            qryCollegeCode = string.Empty;
            qryExamYear = string.Empty;

            if (ddl_collegename.Items.Count > 0 && ddl_collegename.Visible)
            {
                collegeCode = Convert.ToString(ddl_collegename.SelectedValue).Trim();
            }

            if (!string.IsNullOrEmpty(collegeCode))
            {
                qryCollegeCode = " and dg.college_code in(" + collegeCode + ")";
            }

            examYear = string.Empty;
            qryExamYear = string.Empty;
            if (ddlExamYear.Items.Count > 0)
            {
                examYear = Convert.ToString(ddlExamYear.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(examYear))
                {
                    qryExamYear = " and Exam_year in ('" + examYear + "')";
                }
            }
            if (!string.IsNullOrEmpty(qryExamYear) && !string.IsNullOrEmpty(qryCollegeCode))
            {
                string qry = "select distinct ed.Exam_Month,upper(convert(varchar(3),DateAdd(month,ed.Exam_Month,-1))) as Month_Name from exam_details ed,Degree dg,Course c,Department dt where dg.Degree_Code=ed.degree_code and  c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and ed.Exam_Month<>'0' " + qryCollegeCode + qryDegreeCode + qryBatchYear + qryExamYear + " order by Exam_Month";
                ds.Clear();
                ds.Reset();
                ds.Dispose();
                ds = da.select_method_wo_parameter(qry, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlExamMonth.DataSource = ds;
                    ddlExamMonth.DataTextField = "Month_Name";
                    ddlExamMonth.DataValueField = "Exam_Month";
                    ddlExamMonth.DataBind();
                    ddlExamMonth.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlExamMonth_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            showreport1.Visible = false;
            showreport2.Visible = false;
        }
        catch (Exception ex)






        {

        }
    }
    #endregion


    private void setLabelText()
    {
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        List<Label> lbl = new List<Label>();
        List<byte> fields = new List<byte>();

        lbl.Add(lbl_collegename);

      

        fields.Add(0);

        fields.Add(2);
        fields.Add(3);
        fields.Add(4);

        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

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

    #endregion


    #region Go
    protected void btn_go_Click(object sender, EventArgs e)
    {
        if (cellclick != true)
        {
            ds = getstaffdetailsCount();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                loadspreadCount(ds);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
            }
        }

        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);

        }

    }
    #endregion

    #region fpspread2
    private DataSet getstaffdetailsCount()
    {

        DataSet dsloaddetails = new DataSet();
        try
        {

            #region get Value
            string collegecode =Convert.ToString(Session["collegecode"]);
            string examyear = string.Empty;
            string exammonth = string.Empty;
            string view = string.Empty;
            if (ddlExamYear.Items.Count > 0)
                examyear = Convert.ToString(ddlExamYear.SelectedValue);
            if (ddlExamMonth.Items.Count > 0)
                exammonth = Convert.ToString(ddlExamMonth.SelectedValue);
            //if (ddl_view.Items.Count > 0)
            //    view = Convert.ToString(ddl_view.SelectedValue);


            string selQ = string.Empty;
            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(examyear) && !string.IsNullOrEmpty(exammonth))
            {
                //selQ = "select distinct  convert(varchar(10),sm.Batch_Year)+'-'+c.Course_Name+'-'+dt.Dept_Name as degree,s.subject_name,COUNT(qs.staffCode)TotalNoOfCount,case when isnull(Accept,0)=1 then COUNT(isnull(Accept,0)) else 0 end NoOfAccept,case when isnull(Accept,0)=1 then COUNT(isnull(Accept,0)) else 0 end NoOfUpload,qs.subjectno  from  qPaperSetterStaff qs left join QuestionAttachment qa on qa.examMonth=qs.examMonth and qs.examYear=qa.ExamYear and qa.StaffCode=qs.staffCode and qa.SubjectNo=qs.subjectNo,subject s,syllabus_master sm,sub_sem ss ,Degree d,course c,Department dt where s.subject_no=qs.SubjectNo and d.Course_Id=c.Course_Id and dt.Dept_Code=d.Dept_Code and d.Degree_Code=sm.degree_code and ss.syll_code=s.syll_code and s.syll_code=sm.syll_code and sm.syll_code=ss.syll_code and s.subType_no=ss.subType_no and c.college_code='" + collegecode + "' and sm.Batch_Year in('" + batch + "') and qs.ExamMonth='" + exammonth + "' and qs.ExamYear='" + examyear + "' and d.Degree_Code in('" + branch + "')  group by qs.subjectNo,Accept,sm.Batch_Year,c.Course_Name,dt.Dept_Name,s.subject_name ";
                //dsloaddetails.Clear();
                //dsloaddetails = d2.select_method_wo_parameter(selQ, "Text");
                selQ = "select distinct  convert(varchar(10),sm.Batch_Year)+'-'+c.Course_Name+'-'+dt.Dept_Name as degree,s.subject_name,COUNT(qs.staffCode)TotalNoOfCount,qs.subjectno  from  qPaperSetterStaff qs left join QuestionAttachment qa on qa.examMonth=qs.examMonth and qs.examYear=qa.ExamYear and qa.StaffCode=qs.staffCode and qa.SubjectNo=qs.subjectNo,subject s,syllabus_master sm,sub_sem ss ,Degree d,course c,Department dt where s.subject_no=qs.SubjectNo and d.Course_Id=c.Course_Id and dt.Dept_Code=d.Dept_Code and d.Degree_Code=sm.degree_code and ss.syll_code=s.syll_code and s.syll_code=sm.syll_code and sm.syll_code=ss.syll_code and s.subType_no=ss.subType_no and c.college_code='" + collegecode + "' and  qs.ExamMonth='" + exammonth + "' and qs.ExamYear='" + examyear + "'  group by qs.subjectNo,sm.Batch_Year,c.Course_Name,dt.Dept_Name,s.subject_name;";//Total No Of Count

                selQ += "select distinct case when isnull(Accept,0)=1 then COUNT(isnull(Accept,0)) else 0 end NoOfAccept,qs.subjectno from  qPaperSetterStaff qs left join QuestionAttachment qa on qa.examMonth=qs.examMonth and qs.examYear=qa.ExamYear and qa.StaffCode=qs.staffCode and qa.SubjectNo=qs.subjectNo,subject s,syllabus_master sm,sub_sem ss ,Degree d,course c,Department dt where s.subject_no=qs.SubjectNo and d.Course_Id=c.Course_Id and dt.Dept_Code=d.Dept_Code and d.Degree_Code=sm.degree_code and ss.syll_code=s.syll_code and s.syll_code=sm.syll_code and sm.syll_code=ss.syll_code and s.subType_no=ss.subType_no and c.college_code='" + collegecode + "' and  qs.ExamMonth='" + exammonth + "' and qs.ExamYear='" + examyear + "'   group by qs.subjectno,Accept,sm.Batch_Year,c.Course_Name,dt.Dept_Name,s.subject_name;";//No Of Accept


                selQ += "select distinct case when isnull(Accept,0)=1 then COUNT(isnull(Accept,0)) else 0 end NoOfUpload,qs.subjectno from  qPaperSetterStaff qs left join QuestionAttachment qa on qa.examMonth=qs.examMonth and qs.examYear=qa.ExamYear and qa.StaffCode=qs.staffCode and qa.SubjectNo=qs.subjectNo,subject s,syllabus_master sm,sub_sem ss ,Degree d,course c,Department dt where s.subject_no=qs.SubjectNo and d.Course_Id=c.Course_Id and dt.Dept_Code=d.Dept_Code and d.Degree_Code=sm.degree_code and ss.syll_code=s.syll_code and s.syll_code=sm.syll_code and sm.syll_code=ss.syll_code and s.subType_no=ss.subType_no and  c.college_code='" + collegecode + "' and   qs.ExamMonth='" + exammonth + "' and qs.ExamYear='" + examyear + "'   group by qs.subjectNo,Accept,sm.Batch_Year,c.Course_Name,dt.Dept_Name,s.subject_name;"; //No of Upload

                dsloaddetails.Clear();
                dsloaddetails = d2.select_method_wo_parameter(selQ, "Text");

            }

            #endregion
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegeCode, "AllotmentReport"); }
        return dsloaddetails;
    }

    private void loadspreadCount(DataSet ds)
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SNo");
            dt.Columns.Add("Degree Details");
            dt.Columns.Add("Subject Name");
            dt.Columns.Add("Total No Of Request");
            dt.Columns.Add("No Of Accept");
            dt.Columns.Add("No Of Upload");



            spreadDet1.Sheets[0].RowCount = 0;
            spreadDet1.Sheets[0].ColumnCount = 0;
            spreadDet1.CommandBar.Visible = false;
            spreadDet1.Sheets[0].AutoPostBack = true;
            spreadDet1.Sheets[0].ColumnHeader.RowCount = 1;
            spreadDet1.Sheets[0].RowHeader.Visible = false;
            spreadDet1.Sheets[0].ColumnCount = 0;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            //spreadDet.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dt.Columns.Count; row++)
                {

                    spreadDet1.Sheets[0].ColumnCount++;
                    string col = Convert.ToString(dt.Columns[row].ColumnName);
                    spreadDet1.Sheets[0].ColumnHeader.Cells[0, row].Text = col;
                    spreadDet1.Sheets[0].ColumnHeader.Cells[0, row].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet1.Sheets[0].ColumnHeader.Cells[0, row].Font.Bold = true;
                    spreadDet1.Sheets[0].ColumnHeader.Cells[0, row].Font.Name = "Book Antiqua";
                    spreadDet1.Sheets[0].ColumnHeader.Cells[0, row].Font.Size = FontUnit.Medium;
                    spreadDet1.Sheets[0].ColumnHeader.Cells[0, row].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

                }
            }

            FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();
            int sno = 0;
            DataTable dtnew = new DataTable();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    //foreach (DataRow drPerfRow in ds.Tables[0].Rows)
                    //{
                    spreadDet1.Sheets[0].RowCount++;
                    sno++;
                    string batch = Convert.ToString(ds.Tables[0].Rows[row]["degree"]).Trim();
                    string subjectname = Convert.ToString(ds.Tables[0].Rows[row]["subject_name"]).Trim();
                    string TotalnoofCount = Convert.ToString(ds.Tables[0].Rows[row]["TotalNoOfCount"]).Trim();
                    string subjectnumber = Convert.ToString(ds.Tables[0].Rows[row]["subjectNo"]).Trim();
                    //string NoofAccept = Convert.ToString(drPerfRow["NoOfAccept"]).Trim();
                    //string NoofUpload = Convert.ToString(drPerfRow["NoOfUpload"]).Trim();

                    //NoofAccept
                    int noofaccept = 0;
                    int noofupload = 0;
                    int accept = 0;
                    int upload = 0;
                    string subjectno = Convert.ToString(ds.Tables[0].Rows[row]["subjectNo"]).Trim();
                    ds.Tables[1].DefaultView.RowFilter = "subjectNo ='" + subjectno + "'";
                    dtnew = ds.Tables[1].DefaultView.ToTable();
                    for (int i = 0; i < dtnew.Rows.Count; i++)
                    {
                        noofaccept = Convert.ToInt32(dtnew.Rows[i]["NoOfAccept"]);
                        accept += noofaccept;
                    }
                    //NoofUpload
                    string subjectnum = Convert.ToString(ds.Tables[0].Rows[row]["subjectNo"]).Trim();
                    ds.Tables[2].DefaultView.RowFilter = "subjectNo ='" + subjectnum + "'";
                    dtnew = ds.Tables[2].DefaultView.ToTable();
                    //if (dtnew.Rows.Count > 0)
                    for (int j = 0; j < dtnew.Rows.Count; j++)
                    {
                        noofupload = Convert.ToInt32(dtnew.Rows[j]["NoOfUpload"]);
                        upload += noofupload;
                    }
                    //double consumtotal = Convert.ToDouble(ds1.Tables[1].Compute("Sum(Consumption_Value)", ""));
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].CellType = txtCell;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 1].CellType = txtCell;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].CellType = txtCell;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 4].CellType = txtCell;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 5].CellType = txtCell;

                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 1].Text = batch;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 2].Text = subjectname;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 2].Tag = subjectnumber;

                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].Text = TotalnoofCount;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(accept);
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(upload);
                    //string totalcount = Convert.ToString(ds.Tables[0].Compute("Sum(TotalNoOfCount)", "subjectNo in ('" + subjectno + "')"));
                    //string accept = Convert.ToString(ds.Tables[0].Compute("Sum(NoOfAccept)", "subjectNo in ('" + subjectno + "')"));
                    //string upload = Convert.ToString(ds.Tables[0].Compute("Sum(NoOfUpload)", "subjectNo in ('" + subjectno + "')"));
                    //spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].Text = totalcount;
                    //spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 4].Text = accept;
                    //spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 5].Text = upload;


                    //spreadDet.ActiveSheetView.Cells[spreadDet.Sheets[0].RowCount - 1, 1].CellType = new FarPoint.Web.Spread.ButtonCellType("OneCommand", FarPoint.Web.Spread.ButtonType.LinkButton, imagetext);


                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;

                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;

                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].Locked = true;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 1].Locked = true;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 2].Locked = true;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].Locked = true;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 4].Locked = true;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 5].Locked = true;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].ForeColor = Color.Blue;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].Font.Underline = true;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 4].ForeColor = Color.Blue;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 4].Font.Underline = true;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 5].ForeColor = Color.Blue;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 5].Font.Underline = true;

                    spreadDet1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    spreadDet1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    //spreadDet1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                }

                spreadDet1.Sheets[0].Columns[0].Width = 50;
                spreadDet1.Sheets[0].Columns[1].Width = 150;
                spreadDet1.Sheets[0].Columns[2].Width = 200;
                spreadDet1.Sheets[0].Columns[3].Width = 200;
                spreadDet1.Sheets[0].Columns[4].Width = 80;
                spreadDet1.Sheets[0].Columns[5].Width = 150;
                spreadDet1.Sheets[0].PageSize = spreadDet1.Sheets[0].RowCount;
                spreadDet1.SaveChanges();

                showreport1.Visible = true;
                showreport2.Visible = false;
                print1.Visible = true;
            }
        }

        catch (Exception ex) { d2.sendErrorMail(ex, collegeCode, "AllotmentReport"); }
      
    }


    public void spreadDet1_OnCellClick(object sender, EventArgs e)
    {
        try
        {
            cellclick = true;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegeCode, "AllotmentReport"); }
       

    }

    protected void spreadDet1_Selectedindexchange(object sender, EventArgs e)
    {
        try
        {
            if (cellclick == true)
            {
                ds = staffdetails();
               if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    loadspreadstaffdetails(ds);
                    spreadDet2.Visible = true;

                }
                else
                {
                    spreadDet2.Visible = false;
                    print2.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);

                }
            }


        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegeCode, "AllotmentReport"); }
       

    }


    #region Print
    protected void btnExcel_Click1(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname1.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(spreadDet1, reportname);
                // lblvalidation1.Visible = false;
            }
            else
            {
                // lblvalidation1.Text = "Please Enter Your  Report Name";
                //  lblvalidation1.Visible = true;
                txtexcelname1.Focus();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegeCode, "AllotmentReport"); }
       
    }

    public void btnprintmaster_Click1(object sender, EventArgs e)
    {
        try
        {
            lblvalidation2.Text = "";
            txtexcelname1.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Allotment Report";
            //+ '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "AllotmentReport.aspx";
            Printcontrolhed1.loadspreaddetails(spreadDet1, pagename, degreedetails);
            Printcontrolhed1.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegeCode, "AllotmentReport"); }
    }

    protected void getPrintSettings1()
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
                    lblrptname1.Visible = true;
                    txtexcelname1.Visible = true;
                    btnExcel1.Visible = true;
                }
                if (printset.Contains("P"))
                {
                    btnprintmasterhed1.Visible = true;
                }
                if (printset == "0")
                {
                    lblrptname1.Visible = true;
                    txtexcelname1.Visible = true;
                    btnExcel1.Visible = true;
                    btnprintmasterhed1.Visible = true;
                }
            }
            #endregion
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegeCode, "AllotmentReport"); }
    }

    #endregion
    #endregion

    #region fpspread3

    private DataSet staffdetails()
    {

        spreadDet1.SaveChanges();
        DataSet dtstaffDetails = new DataSet();
        try
        {

            string exammonth = string.Empty;
            string examyear = string.Empty;
            examyear = Convert.ToString(ddlExamYear.SelectedValue);
            exammonth = Convert.ToString(ddlExamMonth.SelectedValue);
            string collegecode = string.Empty;
            

            string view = string.Empty;
            if (ddl_collegename.Items.Count > 0)
                collegecode = Convert.ToString(ddl_collegename.SelectedValue);
           
            string SubjectNo = string.Empty;
            string selQ = string.Empty;
            string Saveqry = string.Empty;
            string DeleteQry = string.Empty;
            int actRow = 0;
            int actCol = 0;
            string activerow = spreadDet1.ActiveSheetView.ActiveRow.ToString();
            string activecol = spreadDet1.ActiveSheetView.ActiveColumn.ToString();
            int.TryParse(activerow, out actRow);
            int.TryParse(activecol, out actCol);
            if (actRow != -1 && actCol != -1)
            {
                SubjectNo = Convert.ToString(spreadDet1.Sheets[0].Cells[actRow, 2].Tag);
                if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(examyear) && !string.IsNullOrEmpty(exammonth) && !string.IsNullOrEmpty(SubjectNo))
                {
                    if (actCol == 3)
                    {
                        selQ = "  select  convert(varchar(10),sm.Batch_Year)+'-'+c.Course_Name+'-'+dt.Dept_Name as degree,s.subject_name,case when isnull(Accept,0)=1 then 'Accept' when isnull(Accept,0)=0 then 'Not Accept' end Accept,qa.FileName,ISNULL(isExternal,0)isExternal, case when ISNULL(isExternal,0)=1 then es.staff_name+' [ '+convert(varchar(30), es.staff_code)+' ]' else sf.staff_name+' [ '+sf.staff_code+' ]' end staff_name,qa.SubjectNo  from qPaperSetterStaff qs left join QuestionAttachment qa on qa.examMonth=qs.examMonth and qs.examYear=qa.ExamYear and qa.StaffCode=qs.staffCode and qa.SubjectNo=qs.subjectNo left join external_staff es on qs.staffcode =es.staff_code LEFT join staffmaster sf on sf.staff_code=qs.staffCode and sf.resign=0 and sf.settled=0 and sf.staff_code=qs.staffCode,subject s,syllabus_master sm,sub_sem ss ,Degree d,course c,Department dt where s.subject_no=qs.SubjectNo and d.Course_Id=c.Course_Id and dt.Dept_Code=d.Dept_Code and d.Degree_Code=sm.degree_code and ss.syll_code=s.syll_code and s.syll_code=sm.syll_code and sm.syll_code=ss.syll_code and s.subType_no=ss.subType_no and c.college_code='" + collegecode + "' and  qs.ExamMonth='" + exammonth + "' and qs.ExamYear='" + examyear + "' and  qs.subjectNo='" + SubjectNo + "'";
                        dtstaffDetails.Clear();
                        dtstaffDetails = d2.select_method_wo_parameter(selQ, "Text");
                    }
                    else if (actCol == 4 || actCol == 5)
                    {
                        selQ = "  select  convert(varchar(10),sm.Batch_Year)+'-'+c.Course_Name+'-'+dt.Dept_Name as degree,s.subject_name,case when isnull(Accept,0)=1 then 'Accept' when isnull(Accept,0)=0 then 'Not Accept' end Accept,qa.FileName,ISNULL(isExternal,0)isExternal, case when ISNULL(isExternal,0)=1 then es.staff_name+' [ '+convert(varchar(30), es.staff_code)+' ]' else sf.staff_name+' [ '+sf.staff_code+' ]' end staff_name,qa.SubjectNo  from qPaperSetterStaff qs left join QuestionAttachment qa on qa.examMonth=qs.examMonth and qs.examYear=qa.ExamYear and qa.StaffCode=qs.staffCode and qa.SubjectNo=qs.subjectNo left join external_staff es on qs.staffcode =es.staff_code LEFT join staffmaster sf on sf.staff_code=qs.staffCode and sf.resign=0 and sf.settled=0 and sf.staff_code=qs.staffCode,subject s,syllabus_master sm,sub_sem ss ,Degree d,course c,Department dt where s.subject_no=qs.SubjectNo and d.Course_Id=c.Course_Id and dt.Dept_Code=d.Dept_Code and d.Degree_Code=sm.degree_code and ss.syll_code=s.syll_code and s.syll_code=sm.syll_code and sm.syll_code=ss.syll_code and s.subType_no=ss.subType_no and c.college_code='" + collegecode + "' and qs.ExamMonth='" + exammonth + "' and qs.ExamYear='" + examyear + "' and qs.subjectNo='" + SubjectNo + "'  and qa.Accept='1'";
                        dtstaffDetails.Clear();
                        dtstaffDetails = d2.select_method_wo_parameter(selQ, "Text");
                    }
                }
            }
            //select subjectNo from qPaperSetterStaff qs where examMonth='11' and examYear='2017'

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegeCode, "AllotmentReport"); }
       
        return dtstaffDetails;
    }


    private void loadspreadstaffdetails(DataSet ds)
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SNo");
            dt.Columns.Add("Degree Details");
            dt.Columns.Add("Subject Name");
            dt.Columns.Add("Staff Name");
            dt.Columns.Add("Status");
            dt.Columns.Add("File Name");
            dt.Columns.Add("View");


            spreadDet2.Sheets[0].RowCount = 0;
            spreadDet2.Sheets[0].ColumnCount = 0;
            spreadDet2.CommandBar.Visible = false;
            spreadDet2.Sheets[0].AutoPostBack = true;
            spreadDet2.Sheets[0].ColumnHeader.RowCount = 1;
            spreadDet2.Sheets[0].RowHeader.Visible = false;
            spreadDet2.Sheets[0].ColumnCount = 0;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            spreadDet2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dt.Columns.Count; row++)
                {

                    spreadDet2.Sheets[0].ColumnCount++;
                    string col = Convert.ToString(dt.Columns[row].ColumnName);
                    spreadDet2.Sheets[0].ColumnHeader.Cells[0, row].Text = col;
                    spreadDet2.Sheets[0].ColumnHeader.Cells[0, row].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet2.Sheets[0].ColumnHeader.Cells[0, row].Font.Bold = true;
                    spreadDet2.Sheets[0].ColumnHeader.Cells[0, row].Font.Name = "Book Antiqua";
                    spreadDet2.Sheets[0].ColumnHeader.Cells[0, row].Font.Size = FontUnit.Medium;
                    spreadDet2.Sheets[0].ColumnHeader.Cells[0, row].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    int actRow = 0;
                    int actCol = 0;
                    string activerow = spreadDet1.ActiveSheetView.ActiveRow.ToString();
                    string activecol = spreadDet1.ActiveSheetView.ActiveColumn.ToString();
                    int.TryParse(activerow, out actRow);
                    int.TryParse(activecol, out actCol);
                    if (actCol == 3 && col == "Status")
                        spreadDet2.Sheets[0].Columns[4].Visible = false;
                    if (actCol == 3 && col == "File Name")
                        spreadDet2.Sheets[0].Columns[5].Visible = false;
                    if (actCol == 3 && col == "View")
                        spreadDet2.Sheets[0].Columns[6].Visible = false;
                    if (actCol == 4 && col == "Status")
                        spreadDet2.Sheets[0].Columns[4].Visible = false;
                    if (actCol == 4 && col == "File Name")
                        spreadDet2.Sheets[0].Columns[5].Visible = false;
                    if (actCol == 4 && col == "View")
                        spreadDet2.Sheets[0].Columns[6].Visible = false;
                    if (actCol == 5 && col == "Status")
                        spreadDet2.Sheets[0].Columns[4].Visible = false;
                    if (actCol == 5 && col == "File Name")
                        spreadDet2.Sheets[0].Columns[5].Visible = false;
                    if (actCol == 5 && col == "View")
                        spreadDet2.Sheets[0].Columns[6].Visible = true;


                }
            }

            FarPoint.Web.Spread.ButtonCellType btn = new FarPoint.Web.Spread.ButtonCellType();
            btn.Text = "Question paper";
            btn.CommandName = "btnView";
            FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();
            int sno = 0;

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow drPerfRow in ds.Tables[0].Rows)
                {
                    spreadDet2.Sheets[0].RowCount++;
                    sno++;
                    string batch = Convert.ToString(drPerfRow["degree"]).Trim();
                    string subjectname = Convert.ToString(drPerfRow["subject_name"]).Trim();
                    string staffname = Convert.ToString(drPerfRow["staff_name"]).Trim();
                    string status = Convert.ToString(drPerfRow["Accept"]).Trim();
                    string file = Convert.ToString(drPerfRow["FileName"]).Trim();
                    string subjectno = Convert.ToString(drPerfRow["SubjectNo"]).Trim();



                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 0].CellType = txtCell;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 1].CellType = txtCell;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 3].CellType = txtCell;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 4].CellType = txtCell;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 5].CellType = txtCell;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 6].CellType = btn;


                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 1].Text = batch;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 2].Text = subjectname;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 3].Text = staffname;
                    //spreaddet2.sheets[0].cells[spreaddet2.sheets[0].rowcount - 1, 4].text = status;
                    //spreaddet2.sheets[0].cells[spreaddet2.sheets[0].rowcount - 1, 5].text = file;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 6].Tag = subjectno;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 5].Tag = file;



                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;


                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;



                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 0].Locked = true;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 1].Locked = true;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 2].Locked = true;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 3].Locked = true;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 4].Locked = true;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 5].Locked = true;

                    spreadDet2.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    spreadDet2.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                }

                spreadDet2.Sheets[0].Columns[0].Width = 50;
                spreadDet2.Sheets[0].Columns[1].Width = 150;
                spreadDet2.Sheets[0].Columns[2].Width = 200;
                spreadDet2.Sheets[0].Columns[3].Width = 200;
                spreadDet2.Sheets[0].Columns[4].Width = 100;
                spreadDet2.Sheets[0].Columns[5].Width = 100;

                spreadDet2.Sheets[0].PageSize = spreadDet2.Sheets[0].RowCount;
                spreadDet2.SaveChanges();

                showreport1.Visible = true;
                showreport2.Visible = true;
                print2.Visible = true;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegeCode, "AllotmentReport"); }

    }


    #region Print
    protected void btnExcel_Click2(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname2.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(spreadDet2, reportname);
                // lblvalidation1.Visible = false;
            }
            else
            {
                // lblvalidation1.Text = "Please Enter Your  Report Name";
                //  lblvalidation1.Visible = true;
                txtexcelname2.Focus();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegeCode, "AllotmentReport"); }
      
    }

    public void btnprintmaster_Click2(object sender, EventArgs e)
    {
        try
        {
            lblvalidation3.Text = "";
            txtexcelname2.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Allotment Report";
            //+ '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "AllotmentReport.aspx";
            Printcontrolhed2.loadspreaddetails(spreadDet2, pagename, degreedetails);
            Printcontrolhed2.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegeCode, "AllotmentReport"); }
    }

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
        catch (Exception ex) { d2.sendErrorMail(ex, collegeCode, "AllotmentReport"); }
    }

    #endregion
    #endregion

    protected void spreadDet2_OnButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        spreadDet1.SaveChanges();
        if (e.CommandName == "btnView")
        {
            string Position = e.CommandArgument.ToString().Replace("}", "").Replace("{", "");
            string[] pos = Position.Split(',');

            int xpos = 0;
            int ypos = 0;

            if (pos.Length > 0)
            {
                string[] xVal = (pos.Length > 0) ? pos[0].Split('=') : new string[0];
                string[] yVal = (pos.Length > 1) ? pos[1].Split('=') : new string[0];
                if (xVal.Length > 1)
                {
                    int.TryParse(xVal[1], out xpos);
                    lblXpos.Text = xpos.ToString();
                }
                if (yVal.Length > 1)
                {
                    int.TryParse(yVal[1], out ypos);
                    lblYpos.Text = ypos.ToString();

                }
                int actrow = xpos;
                viewquestionpaper1();
            }
        }
    }

    public void viewquestionpaper()
    {

        try
        {

            string activerow = "";
            string activecol = "";
            activerow = lblXpos.Text;
            activecol = lblYpos.Text;

            if (Convert.ToInt32(activecol) == 6)
            {
                string fileName = string.Empty;
                string subjectno = spreadDet1.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Tag.ToString();
                string file = spreadDet1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Tag.ToString();

                string strquer = "select FileName,AttachDoc,Filetype FROM QuestionAttachment where SubjectNo='" + subjectno + "' and FileName='" + file + "'";

                DataSet dsquery = d2.select_method_wo_parameter(strquer, "Text");
                for (int i = 0; i < dsquery.Tables[0].Rows.Count; i++)
                {
                    Response.ContentType = dsquery.Tables[0].Rows[i]["Filetype"].ToString();
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + dsquery.Tables[0].Rows[i]["FileName"] + "\"");
                    Response.BinaryWrite((byte[])dsquery.Tables[0].Rows[i]["AttachDoc"]);
                    Response.End();
                    //Cellclick = false;
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegeCode, "AllotmentReport"); }
        
    }

    public void viewquestionpaper1()
    {

        try
        {
            spreadDet2.SaveChanges();
            string activerow = "";
            string activecol = "";
            activerow = lblXpos.Text;
            activecol = lblYpos.Text;

            if (Convert.ToInt32(activecol) == 6)
            {
                string fileName = string.Empty;
                string subjectno = spreadDet2.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Tag.ToString();
                string file = spreadDet2.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Tag.ToString();

                string strquer = "select FileName,AttachDoc,Filetype FROM QuestionAttachment where SubjectNo='" + subjectno + "' and FileName='" + file + "'";

                DataSet dsquery = d2.select_method_wo_parameter(strquer, "Text");
                for (int i = 0; i < dsquery.Tables[0].Rows.Count; i++)
                {
                    Response.ContentType = dsquery.Tables[0].Rows[i]["Filetype"].ToString();
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + dsquery.Tables[0].Rows[i]["FileName"] + "\"");
                    Response.BinaryWrite((byte[])dsquery.Tables[0].Rows[i]["AttachDoc"]);
                    Response.End();
                    //Cellclick = false;
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegeCode, "AllotmentReport"); }
     
    }
}