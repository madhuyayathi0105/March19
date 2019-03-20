using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Data;
using System.Collections;
using System.Net.Mail;
using System.Net;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using System.Reflection;

public partial class CoeMod_QuestionPaperSelector : System.Web.UI.Page
{

    #region Field Declaration

    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string selectQuery = string.Empty;
    Dictionary<string, string> dicStaffList = new Dictionary<string, string>();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet dsprint = new DataSet();
    ArrayList colord = new ArrayList();
    DAccess2 da = new DAccess2();
    DataView dvhead = new DataView();
    DataSet dscol = new DataSet();
    Hashtable ht = new Hashtable();
    DataTable dtCommon = new DataTable();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    SqlConnection ssql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    Dictionary<string, string> dicSQLParameter = new Dictionary<string, string>();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    string qry = string.Empty;
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    string collegeCode = string.Empty;
    string batchYear = string.Empty;
    string courseId = string.Empty;
    string degreeCode = string.Empty;
    string semester = string.Empty;
    string orderBy = string.Empty;
    string orderBySetting = string.Empty;
    //string qry = string.Empty;
    string qryCollegeCode = string.Empty;
    string qryCollegeCode1 = string.Empty;
    string qryBatchYear = string.Empty;
    string qryDegreeCode = string.Empty;
    string qrySemester = string.Empty;
    string examYear = string.Empty;
    string qryExamYear = string.Empty;
    string examMonth = string.Empty;
    string qryExamMonth = string.Empty;
    int ACTROW = 0;
    string internalorexternal = string.Empty;
    int countperdate = 0;
    string SenderID = string.Empty;
    string Password = string.Empty;
    string user_id = string.Empty;
    string send_mail = string.Empty;
    string send_pw = string.Empty;
    string to_mail = string.Empty;
    DataTable dtsendsms = new DataTable();

    #endregion

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
            else
            {
                userCollegeCode = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
                userCode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
                singleUser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
                groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
            }
            if (!IsPostBack)
            {
                Bindcollege();
                BindExamYear();
                BindExamMonth();
                College();
                BindAlterStaffDepartment(((ddlAlterFreeCollege.Items.Count > 0) ? Convert.ToString(ddlAlterFreeCollege.SelectedValue).Trim() : userCollegeCode));
                getdetailsSmsEmail();


            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectAllotment"); }

    }

    #region college

    public void Bindcollege()
    {
        try
        {
            ddlCollege.Items.Clear();
            dtCommon.Clear();
            ddlCollege.Enabled = false;
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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectAllotment"); }
    }

    #endregion

    #region ExamMonth

    private void BindExamMonth()
    {
        try
        {
            string collegeCode = string.Empty;
            string batchYear = string.Empty;
            string degreeCode = string.Empty;
            ddlExamMonth.Items.Clear();
            ds.Clear();
            collegeCode = string.Empty;
            batchYear = string.Empty;
            degreeCode = string.Empty;
            qryCollegeCode = string.Empty;
            qryDegreeCode = string.Empty;
            qryBatchYear = string.Empty;
            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode = " and dg.college_code in (" + collegeCode + ")";
                }
            }

            examYear = string.Empty;
            qryExamYear = string.Empty;
            if (ddlExamYear.Items.Count > 0)
            {
                foreach (System.Web.UI.WebControls.ListItem li in ddlExamYear.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(examYear))
                        {
                            examYear = "'" + li.Value + "'";
                        }
                        else
                        {
                            examYear += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(examYear))
                {
                    qryExamYear = " and Exam_year in (" + examYear + ")";
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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectAllotment"); }
    }

    #endregion

    #region ExamYear

    public void BindExamYear()
    {
        try
        {
            ddlExamYear.Items.Clear();
            ds.Clear();
            collegeCode = string.Empty;
            batchYear = string.Empty;
            degreeCode = string.Empty;
            qryCollegeCode = string.Empty;
            qryDegreeCode = string.Empty;
            qryBatchYear = string.Empty;
            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode = " and dg.college_code in (" + collegeCode + ")";
                }
            }

            if (!string.IsNullOrEmpty(qryCollegeCode))
            {
                string qry = "select distinct ed.Exam_year from exam_details ed,Degree dg,Course c,Department dt where dg.Degree_Code=ed.degree_code and  c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and ed.Exam_year<>'0' " + qryCollegeCode + " order by ed.Exam_year desc";
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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectAllotment"); }
    }

    #endregion

    #region subject search
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText, string contextKey)
    {
        string Flitervalues = contextKey;
        string[] Flitervalue = Flitervalues.Split('$');
        string collegecode = Convert.ToString(Flitervalue[0]);

        string exmon = Convert.ToString(Flitervalue[1]);
        string exyr = Convert.ToString(Flitervalue[2]);
        //string tapselect = Convert.ToString(Flitervalue[4]);
        WebService ws = new WebService();
        List<string> name = new List<string>();
        if (prefixText.Trim() != "")
        {
            //string query = "select distinct s.subject_name from  subject s,syllabus_master sm,sub_sem ss where ss.syll_code=s.syll_code and s.syll_code=sm.syll_code and sm.syll_code=ss.syll_code and s.subType_no=ss.subType_no and   s.subject_name  like 'prefixText%'";
            string query = "select distinct s.subject_code,s.subject_name,s.subject_no from subject s,sub_sem ss,syllabus_master sm,Exam_Details ed,exam_application ea,exam_appl_details ead,Degree d where ea.exam_code=ed.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and ed.degree_code=sm.degree_code and sm.Batch_Year=ed.batch_year and sm.syll_code=s.syll_code and s.syll_code=ss.syll_code and ss.syll_code=sm.syll_code and sm.degree_code=d.degree_code and  ss.subType_no=s.subType_no and ISNULL(ss.Lab,'0')='0' and ISNULL(ss.promote_count,'0')='1' and ISNULL(ss.projThe,'0')='0' and s.subject_name  like 'prefixText%'";
            name = ws.Getname(query);
        }
        return name;
    }
    #endregion

    #region Index Changed Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCollege.Items.Count > 0)
            {

                collegecode = Convert.ToString(ddlCollege.SelectedItem.Value);
                BindExamYear();
                BindExamMonth();
                ddlCollege.SelectedIndex = ddlCollege.Items.IndexOf(ddlCollege.Items.FindByValue(collegecode));
                Fpuser.Visible = false;
                fpstaff.Visible = false;
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectAllotment"); }
       
    }

    protected void ddlExamMonth_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            BindExamYear();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectAllotment"); }
        
    }

    protected void ddlExamYear_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {




        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectAllotment"); }
       
    }



    #endregion Index Changed Events

    #region SmsCredit
    protected void smscreditcountperdate()
    {
        string todaydate = DateTime.Now.ToString("yyyy-MM-dd");
        string datecreate = "select groupmessageid from smsdeliverytrackmaster where date='" + todaydate + "' and groupmessageid!='No Sufficient Credits'";
        DataSet creiddate = new DataSet();
        DAccess2 credite = new DAccess2();
        creiddate = credite.select_method_wo_parameter(datecreate, "text");
        if (creiddate.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < creiddate.Tables[0].Rows.Count; i++)
            {
                string date1 = creiddate.Tables[0].Rows[i]["groupmessageid"].ToString();
                string[] split = date1.Split(new Char[] { ' ' });
                for (int k = 0; k <= split.GetUpperBound(0); k++)
                {
                    if (split[k].ToString().Trim() != "")
                    {
                        countperdate++;
                    }
                }
            }
        }
        lblmsgused.Text = "Credits User Today:" + countperdate + "";
    }
    #endregion

    #region Go

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            lblmsgcredit.Text = "SMS Available Credits :0";
            Label2.Visible = true;
            smscreditcountperdate();
            //txtmessage.Visible = false;
            if (Convert.ToString(Session["QueryString"]) != "")
            {
                PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                // make collection editable
                isreadonly.SetValue(this.Request.QueryString, false, null);
                // remove
                this.Request.QueryString.Remove(Convert.ToString(Session["QueryString"]));
                Request.QueryString.Clear();
            }
            Session["InternalCollegeCode"] = ddlCollege.SelectedValue.ToString();
            string strsenderquery = "select SMS_User_ID,college_code from Track_Value where college_code = '" + ddlCollege.SelectedValue.ToString() + "'";
            ds1.Dispose();
            ds1.Reset();
            ds1 = d2.select_method(strsenderquery, ht, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                user_id = Convert.ToString(ds1.Tables[0].Rows[0]["SMS_User_ID"]);
            }
           
            string getval = d2.GetUserapi(user_id);
            string[] spret = getval.Split('-');
            if (spret.GetUpperBound(0) == 1)
            {
                SenderID = spret[0].ToString();
                Password = spret[1].ToString();
                Session["api"] = user_id;
                Session["senderid"] = SenderID;
            }
            if (SenderID != "" && Password != "")
            {
                lblmsgcredit.Visible = true;
                WebRequest request = WebRequest.Create("http://hp.dial4sms.com/balalert/main.php?uname=" + SenderID + "&pass=" + Password + "");
                WebResponse response = request.GetResponse();
                Stream data = response.GetResponseStream();
                StreamReader sr = new StreamReader(data);
                string strvel = sr.ReadToEnd();
                lblmsgcredit.Text = strvel.ToString();
                string[] strrrvel = strvel.Split(' ');
                int getuprbnd = strrrvel.GetUpperBound(0);
                lblmsgcredit.Text = "SMS Available Credits :" + strrrvel[getuprbnd];
            }
            DataTable dt = new DataTable();
            dt = getsubjectdetails();
            if (dt.Rows.Count > 0 && dt.Rows.Count > 0)
            {
                loadspread(dt);
            }
            else
            {

                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Found!";

            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectAllotment"); }

    }

    #endregion

    #region loadSubjectDetails
    private DataTable getsubjectdetails()
    {

        DataTable dtload = new DataTable();
        //DataTable dtdetails = new DataTable();
        try
        {
            #region get Value
            string collegecode = string.Empty;
            string batch = string.Empty;
            string examyear = string.Empty;
            string exammonth = string.Empty;
            string qrySubjectFilter = string.Empty;

            if (ddlCollege.Items.Count > 0)
                collegecode = Convert.ToString(ddlCollege.SelectedValue);
            if (ddlExamYear.Items.Count > 0)
                examyear = Convert.ToString(ddlExamYear.SelectedValue);
            if (ddlExamMonth.Items.Count > 0)
                exammonth = Convert.ToString(ddlExamMonth.SelectedValue);

            string selQ = string.Empty;

            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(examyear) && !string.IsNullOrEmpty(exammonth))
            {
                if (txt_subject.Text.Trim() != "")
                    qrySubjectFilter = "and s.subject_name like '" + txt_subject.Text + "%' ";
                selQ = "select distinct s.subject_code,s.subject_name,s.subject_no from subject s,sub_sem ss,syllabus_master sm,Exam_Details ed,exam_application ea,exam_appl_details ead,Degree d where ea.exam_code=ed.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and ed.degree_code=sm.degree_code and sm.Batch_Year=ed.batch_year and sm.syll_code=s.syll_code and s.syll_code=ss.syll_code and ss.syll_code=sm.syll_code and sm.degree_code=d.degree_code and  ss.subType_no=s.subType_no and ISNULL(ss.Lab,'0')='0' and ISNULL(ss.promote_count,'0')='1' and ISNULL(ss.projThe,'0')='0' and subject_name not like '%practical%' and subject_name not  like '%lab%' and ed.Exam_Month='" + exammonth + "' and ed.Exam_year='" + examyear + "'  and  d.college_code ='" + collegecode + "' " + qrySubjectFilter + "  order by  s.subject_code,s.subject_name";
                //selQ = "select distinct s.subject_code,s.subject_name from subject s,sub_sem ss,syllabus_master sm,Exam_Details ed,exam_application ea,exam_appl_details ead where ea.exam_code=ed.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and ed.degree_code=sm.degree_code and sm.Batch_Year=ed.batch_year and sm.syll_code=s.syll_code and s.syll_code=ss.syll_code and ss.syll_code=sm.syll_code and ss.subType_no=s.subType_no and ISNULL(ss.Lab,'0')='0' and ISNULL(ss.promote_count,'0')='1' and ISNULL(ss.projThe,'0')='0' and subject_name not like '%practical%' and subject_name not  like '%lab%' and ed.Exam_Month='" + exammonth + "' and ed.Exam_year='" + examyear + "'  and d.college_code ='" + collegecode + "' " + qrySubjectFilter + " order by  s.subject_code,s.subject_name";
                //selQ = "select distinct s.subject_name,s.subject_code,ss.subject_type,s.subType_no,s.subject_no,ed.batch_year,ed.degree_code,ed.current_semester,ed.Exam_Month,ed.Exam_year,c.type,c.Edu_Level,t.Equal_Subject_Code,t.Com_Subject_Code from Exam_Details ed,exam_application ea,exam_appl_details ead,sub_sem ss,Degree d,course c,subject s left join tbl_equal_paper_Matching t on t.Equal_Subject_Code=s.subject_code where ed.exam_code=ea.exam_code and ead.appl_no=ea.appl_no and ead.subject_no=s.subject_no and ss.subType_no=s.subType_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and t.Exam_Year=ed.Exam_year and t.Exam_month=ed.Exam_month and ed.Exam_Month='" + exammonth + "' and ed.Exam_year='" + examyear + "' and d.college_code ='" + collegecode + "' " + qrySubjectFilter + "order by s.subject_name,s.subject_code";

                dtload.Clear();
                dtload = dirAcc.selectDataTable(selQ);

            }

            #endregion
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "SubjectAllotment"); }
        return dtload;
    }

    private void loadspread(DataTable dt)
    {
        try
        {
            string ExYr = string.Empty;
            string Exmn = string.Empty;

            if (ddlExamYear.Items.Count > 0)
                ExYr = Convert.ToString(ddlExamYear.SelectedValue);
            if (ddlExamMonth.Items.Count > 0)
                Exmn = Convert.ToString(ddlExamMonth.SelectedValue);

            //Fpuser.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //Fpuser.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //Fpuser.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
            Fpuser.Sheets[0].ColumnCount = 6;
            Fpuser.Sheets[0].RowCount = 0;
            Fpuser.SheetCorner.ColumnCount = 0;

            Fpuser.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpuser.Sheets[0].Columns[0].Width = 40;

            Fpuser.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Subject Number";
            Fpuser.Sheets[0].Columns[1].Width = 300;

            Fpuser.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Code";
            Fpuser.Sheets[0].Columns[2].Width = 250;

            Fpuser.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Subject Name";
            Fpuser.Sheets[0].Columns[3].Width = 80;

            Fpuser.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Select";
            Fpuser.Sheets[0].Columns[4].Width = 40;

            Fpuser.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Staff Details";
            Fpuser.Sheets[0].Columns[5].Width = 40;
            Fpuser.Sheets[0].Columns[5].Visible = false;
            Fpuser.Sheets[0].AutoPostBack = false;
            Fpuser.CommandBar.Visible = false;

            FarPoint.Web.Spread.ButtonCellType btn = new FarPoint.Web.Spread.ButtonCellType();
            btn.Text = "Select Staff";
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

            int rowcount = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Fpuser.Sheets[0].RowCount++;

                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 0].Text = Fpuser.Sheets[0].RowCount.ToString();
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 0].Locked = true;

                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 1].CellType = txt;
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 1].Text = dt.Rows[i]["subject_no"].ToString();
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 1].Locked = true;
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dt.Rows[i]["subject_no"]).Trim();

                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 2].CellType = txt;
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 2].Text = dt.Rows[i]["subject_code"].ToString();
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 2].Locked = true;

                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 3].CellType = txt;
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 3].Text = dt.Rows[i]["subject_name"].ToString();
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 3].Locked = true;

                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 4].CellType = btn;
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                //FarPoint.Web.Spread.ImageCellType f = new FarPoint.Web.Spread.ImageCellType();

                //FarPoint.Web.Spread.ButtonCellType file = new FarPoint.Web.Spread.ButtonCellType();
                //Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 4].CellType = file.ButtonType;

            }

            Fpuser.Sheets[0].Columns[Fpuser.Sheets[0].ColumnCount - 2].Width = 150;
            Fpuser.Sheets[0].Columns[Fpuser.Sheets[0].ColumnCount - 3].Width = 250;
            Fpuser.Sheets[0].Columns[Fpuser.Sheets[0].ColumnCount - 4].Width = 150;
            Fpuser.Sheets[0].Columns[Fpuser.Sheets[0].ColumnCount - 5].Width = 100;
            Fpuser.Sheets[0].Columns[Fpuser.Sheets[0].ColumnCount - 6].Width = 50;
            Fpuser.Sheets[0].Columns[1].Visible = false;
            Fpuser.SaveChanges();
            Fpuser.Width = 650;
            Fpuser.Height = 400;
            divspread.Visible = true;
            Fpuser.Visible = true;
            Fpuser.Sheets[0].PageSize = Fpuser.Sheets[0].RowCount;


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectAllotment"); }
    }

    protected void Fpuser_OnButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        ACTROW = e.SheetView.ActiveRow;
        divAlterFreeStaffDetails.Visible = true;
        btnSearch_click(sender, e);
        getexistingfile();
    }
    #endregion

    #region saveandsend

    protected void btnSave_Click(Object sender, EventArgs e)
    {
        Fpuser.SaveChanges();
        try
        {
            if (cbsendSMS.Checked == false && chkSendEMail.Checked == false)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select SMS Or EMAIL";
            }
            else
            {
                bool sendsmsflag = cbsendSMS.Checked;
                bool sendemailflag = chkSendEMail.Checked;

                bool isSave = false;
                DataTable dtstaffsave = new DataTable();
                DataTable dtstaffinternalstaff = new DataTable();
                DataTable dtstaffExaternalstaff = new DataTable();
                string exammonth = string.Empty;
                string examyear = string.Empty;
                examyear = Convert.ToString(ddlExamYear.SelectedValue);
                exammonth = Convert.ToString(ddlExamMonth.SelectedValue);
                string selectStaffCode = string.Empty;
                string selectStaffApplId = string.Empty;
                string Saveqry = string.Empty;
                string DeleteQry = string.Empty;
                string staffcodecom = string.Empty;
                if (!string.IsNullOrEmpty(exammonth) && !string.IsNullOrEmpty(examyear))
                {
                    for (int row = 0; row < Fpuser.Sheets[0].RowCount; row++)
                    {
                        string SubjectNo = Convert.ToString(Fpuser.Sheets[0].Cells[row, 1].Tag).Trim();
                        string staffCode = Convert.ToString(Fpuser.Sheets[0].Cells[row, 4].Tag).Trim();
                        string staffId = Convert.ToString(Fpuser.Sheets[0].Cells[row, 4].Note).Trim();
                        string internalorexternal = Convert.ToString(Fpuser.Sheets[0].Cells[row, 3].Tag).Trim();


                        string[] split = staffCode.Split(';');
                        string[] split1 = staffId.Split(';');
                        if (split.Length > 0 && split1.Length > 0)
                        {
                            string stafcode = split[0];
                            string stafid = split1[0];
                            for (int i = 0; i < split.Length; i++)
                            {
                                stafcode = split[i];
                                stafid = split1[i];
                                if (!string.IsNullOrEmpty(stafcode) && !string.IsNullOrEmpty(stafid))
                                {
                                    if (internalorexternal.ToUpper() == "INTERNAL")
                                    {
                                        Saveqry = "if not exists(select * from qPaperSetterStaff where subjectNo='" + SubjectNo + "' and examYear='" + examyear + "' and examMonth='" + exammonth + "' and staffId='" + stafid + "' and staffCode='" + stafcode + "')  insert into qPaperSetterStaff (subjectNo,staffCode,staffId,examYear,examMonth,isExternal) values ('" + SubjectNo + "','" + stafcode + "','" + stafid + "','" + examyear + "','" + exammonth + "','0') else update qPaperSetterStaff set subjectNo='" + SubjectNo + "',staffCode='" + stafcode + "',staffId='" + stafid + "',examYear='" + examyear + "',examMonth='" + exammonth + "',isExternal='0'  where subjectNo='" + SubjectNo + "' and examYear='" + examyear + "' and examMonth='" + exammonth + "' and staffId='" + stafid + "' and staffCode='" + stafcode + "'";
                                        dtstaffsave.Clear();
                                        int res = dirAcc.insertData(Saveqry);
                                        if (res != 0)
                                            isSave = true;
                                    }
                                    //and staffId='" + stafid + "','" + stafid + "',staffId,
                                    if (internalorexternal.ToUpper() == "EXTERNAL")
                                    {
                                        Saveqry = "if not exists(select * from qPaperSetterStaff where subjectNo='" + SubjectNo + "' and examYear='" + examyear + "' and examMonth='" + exammonth + "'  and staffCode='" + stafcode + "')  insert into qPaperSetterStaff (subjectNo,staffCode,examYear,examMonth,isExternal) values ('" + SubjectNo + "','" + stafcode + "','" + examyear + "','" + exammonth + "','1')else update qPaperSetterStaff set subjectNo='" + SubjectNo + "',staffCode='" + stafcode + "',examYear='" + examyear + "',examMonth='" + exammonth + "',isExternal='1'  where subjectNo='" + SubjectNo + "' and examYear='" + examyear + "' and examMonth='" + exammonth + "' and staffCode='" + stafcode + "'";
                                        dtstaffsave.Clear();
                                        int res = dirAcc.insertData(Saveqry);
                                        if (res != 0)
                                            isSave = true;

                                    }
                                }
                                //sendSmsandEmail
                                if ((sendsmsflag && sendemailflag) || sendemailflag || sendsmsflag)
                                {
                                    if (isSave)
                                    {
                                        string user_id = string.Empty;
                                        string ssr = "select * from Track_Value where college_code='" + Convert.ToString(userCollegeCode) + "'";
                                        ds.Clear();
                                        ds = d2.select_method_wo_parameter(ssr, "Text");
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            user_id = Convert.ToString(ds.Tables[0].Rows[0]["SMS_User_ID"]);
                                        }
                                        if (internalorexternal.ToUpper() == "INTERNAL")
                                        {
                                            staffcodecom = staffCode.Replace(";", "','");
                                            string qryinternal = "select distinct sa.per_mobileno,sa.email,sj.subject_name,sj.subject_no,q.staffCode  from  staff_appl_master sa inner join staffmaster s on sa.appl_no=s.appl_no inner join qPaperSetterStaff q on s.staff_code=q.staffCode inner join subject sj on sj.subject_no=q.subjectNo where q.isExternal='0' and q.subjectNo in('" + SubjectNo + "') and q.staffCode in('" + stafcode + "') ; ";
                                            dtstaffinternalstaff.Clear();
                                            dtstaffinternalstaff = dirAcc.selectDataTable(qryinternal);
                                            for (int internalstaff = 0; internalstaff < dtstaffinternalstaff.Rows.Count; internalstaff++)
                                            {
                                                string staffCodee = dtstaffinternalstaff.Rows[internalstaff]["staffCode"].ToString();
                                                string StaffMobile = dtstaffinternalstaff.Rows[internalstaff]["per_mobileno"].ToString();
                                                string StaffEmail = dtstaffinternalstaff.Rows[internalstaff]["email"].ToString();
                                                string SubjectName = dtstaffinternalstaff.Rows[internalstaff]["subject_name"].ToString();
                                                string SubjectNum = dtstaffinternalstaff.Rows[internalstaff]["subject_no"].ToString();

                                                string Msg = " Dear Sir/Madam,We Have Alloted You SubjectName:" + SubjectName + " And SubjectNumber:" + SubjectNum + ",So Kindly Accept It";
                                                if (cbsendSMS.Checked == true)
                                                {
                                                    int d = d2.send_sms(user_id, userCollegeCode, userCode, StaffMobile, Msg, "1");

                                                }
                                                if (chkSendEMail.Checked == true)
                                                {
                                                    string send_mail = string.Empty;
                                                    string send_pw = string.Empty;
                                                    string strquery = "select massemail,masspwd from collinfo where college_code ='" + ddlCollege.SelectedItem.Value + "' ";
                                                    dtstaffinternalstaff.Dispose();
                                                    dtstaffinternalstaff.Reset();
                                                    dtstaffinternalstaff = dirAcc.selectDataTable(strquery);
                                                    {
                                                        send_mail = Convert.ToString(dtstaffinternalstaff.Rows[0]["massemail"]);
                                                        send_pw = Convert.ToString(dtstaffinternalstaff.Rows[0]["masspwd"]);
                                                        //send_mail = "palpaporange@gmail.com";
                                                        //send_pw = "palpap1234";
                                                    }

                                                    SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                                                    Mail.EnableSsl = true;
                                                    MailMessage mailmsg = new MailMessage();
                                                    MailAddress mfrom = new MailAddress(send_mail);
                                                    mailmsg.From = mfrom;
                                                    mailmsg.To.Add(StaffEmail);
                                                    mailmsg.Subject = "Subject Allotment For Staff";
                                                    mailmsg.IsBodyHtml = true;
                                                    mailmsg.Body = Msg;
                                                    Mail.EnableSsl = true;
                                                    Mail.UseDefaultCredentials = false;
                                                    NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                                                    Mail.Credentials = credentials;
                                                    Mail.Send(mailmsg);
                                                }
                                            }
                                        }
                                        if (internalorexternal.ToUpper() == "EXTERNAL")
                                        {
                                            string qryexternal = "select distinct es.per_mobileno,es.email,q.staffCode,sj.subject_name,sj.subject_no from external_staff es inner join qPaperSetterStaff q on es.staff_code=q.staffCode inner join subject sj on sj.subject_no=q.subjectNo where q.isExternal='1' and q.subjectNo in('" + SubjectNo + "') and q.staffCode in('" + stafcode + "')";
                                            dtstaffExaternalstaff.Clear();
                                            dtstaffExaternalstaff = dirAcc.selectDataTable(qryexternal);
                                            for (int external = 0; external < dtstaffExaternalstaff.Rows.Count; external++)
                                            {
                                                string ExternalstaffCodee = dtstaffExaternalstaff.Rows[external]["staffCode"].ToString();
                                                string ExternalStaffMobile = dtstaffExaternalstaff.Rows[external]["per_mobileno"].ToString();
                                                string ExternalStaffEmail = dtstaffExaternalstaff.Rows[external]["email"].ToString();
                                                string ExternalSubjectName = dtstaffExaternalstaff.Rows[external]["subject_name"].ToString();
                                                string ExternalSubjectNum = dtstaffExaternalstaff.Rows[external]["subject_no"].ToString();
                                                string Msg = " Dear Sir/Madam,We Have Alloted You SubjectName:" + ExternalSubjectName + " And SubjectNumber:" + ExternalSubjectNum + ",So Kindly Accept It";
                                                if (cbsendSMS.Checked == true)
                                                {
                                                    int d = d2.send_sms(user_id, userCollegeCode, userCode, ExternalStaffMobile, Msg, "1");
                                                }
                                                if (chkSendEMail.Checked == true)
                                                {
                                                    string send_mail = string.Empty;
                                                    string send_pw = string.Empty;
                                                    string strquery = "select massemail,masspwd from collinfo where college_code ='" + ddlCollege.SelectedItem.Value + "' ";
                                                    dtstaffExaternalstaff.Dispose();
                                                    dtstaffExaternalstaff.Reset();
                                                    dtstaffExaternalstaff = dirAcc.selectDataTable(strquery);
                                                    {
                                                        send_mail = Convert.ToString(dtstaffExaternalstaff.Rows[0]["massemail"]);
                                                        send_pw = Convert.ToString(dtstaffExaternalstaff.Rows[0]["masspwd"]);
                                                        //send_mail = "palpaporange@gmail.com";
                                                        //send_pw = "palpap1234";
                                                    }
                                                    SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                                                    Mail.EnableSsl = true;
                                                    MailMessage mailmsg = new MailMessage();
                                                    MailAddress mfrom = new MailAddress(send_mail);
                                                    mailmsg.From = mfrom;
                                                    mailmsg.To.Add(ExternalStaffEmail);
                                                    mailmsg.Subject = "Subject Allotment For Staff";
                                                    mailmsg.IsBodyHtml = true;
                                                    mailmsg.Body = Msg;
                                                    Mail.EnableSsl = true;
                                                    Mail.UseDefaultCredentials = false;
                                                    NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                                                    Mail.Credentials = credentials;
                                                    Mail.Send(mailmsg);
                                                }
                                            }
                                        }
                                        if (cbsendSMS.Checked == true)
                                        {
                                            alertpopwindow.Visible = true;
                                            lblalerterr.Text = "Save And SMS Send SuccessFully";
                                            isSave = false;
                                        }
                                        if (chkSendEMail.Checked == true)
                                        {
                                            alertpopwindow.Visible = true;
                                            lblalerterr.Text = "Save And Mail Send SuccessFully";
                                            isSave = false;
                                        }
                                        if (chkSendEMail.Checked == true && cbsendSMS.Checked == true)
                                        {
                                            alertpopwindow.Visible = true;
                                            lblalerterr.Text = "Save And Sms,Mail Send SuccessFully";
                                            isSave = false;
                                        }
                                    }
                                }
                            }

                        }
                    }
                    btnGo_Click(sender, e);
                }
            }

        }

        catch
        {

        }

    }

    #endregion

    #region Popup

    #region College
    private void College()
    {
        try
        {
            ddlAlterFreeCollege.Items.Clear();
            qry = "select collname,college_code from collinfo order by college_code";
            DataTable dtCollege = dirAcc.selectDataTable(qry);
            if (dtCollege.Rows.Count > 0)
            {
                ddlAlterFreeCollege.DataSource = dtCollege;
                ddlAlterFreeCollege.DataTextField = "collname";
                ddlAlterFreeCollege.DataValueField = "college_code";
                ddlAlterFreeCollege.DataBind();
            }

            if (External.Checked == true)
            {
                qry = "select distinct college_name,coll_code from external_staff order by coll_code";
                DataTable dtexternalcollege = dirAcc.selectDataTable(qry);
                if (dtexternalcollege.Rows.Count > 0)
                {
                    ddlAlterFreeCollege.DataSource = dtexternalcollege;
                    ddlAlterFreeCollege.DataTextField = "college_name";
                    ddlAlterFreeCollege.DataValueField = "coll_code";
                    ddlAlterFreeCollege.DataBind();
                }

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectAllotment"); }
      
    }
    #endregion

    #region Dept
    private void BindAlterStaffDepartment(string collegeCode)
    {
        try
        {
            ddlAlterFreeDepartment.Items.Clear();
            DataTable dtDept = new DataTable();
            string qry = string.Empty;
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

            if (External.Checked == true) //External Staff Department
            {
                ds = d2.select_method_wo_parameter("select textval,TextCode from TextValTable where TextCriteria='exdep' order by textval", "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlAlterFreeDepartment.DataSource = ds;
                    ddlAlterFreeDepartment.DataTextField = "textval";
                    ddlAlterFreeDepartment.DataValueField = "TextCode";
                    ddlAlterFreeDepartment.DataBind();
                    ddlAlterFreeDepartment.Items.Insert(0, "All");
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectAllotment"); }
       
    }
    #endregion

    #region Event
    protected void ddlAlterFreeCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlCollege.SelectedItem.Value);
                BindAlterStaffDepartment((ddlAlterFreeCollege.Items.Count > 0) ? Convert.ToString(ddlAlterFreeCollege.SelectedValue).Trim() : userCollegeCode);
                ddlCollege.SelectedIndex = ddlCollege.Items.IndexOf(ddlCollege.Items.FindByValue(collegecode));
                //Fpuser.Visible = false;
                //fpstaff.Visible = false;
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectAllotment"); }
      
    }

    protected void ddlAlterFreeDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectAllotment"); }
      
    }

    protected void ddlAlterFreeStaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //GetStaffDetails();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectAllotment"); }
       
    }

    protected void txtAlterFreeStaffSearch_TextChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectAllotment"); }
       
    }

    protected void ddl_desig_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindAlterStaffDepartment(collegeCode);
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectAllotment"); }
       
    }
    #endregion

    #region search

    protected void btnSearch_click(object sender, EventArgs e)
    {
        try
        {
            string val = txtAlterFreeStaffSearch.Text;
            txtAlterFreeStaffSearch.Text = "";
            Internal.Checked = false;
            External.Checked = false;
            ddlAlterFreeDepartment.Items.Clear();
            DataTable dtstaff = new DataTable();
            //string searchValue = txtAlterFreeStaffSearch.Text;
            dtstaff = getFreeStaffListNew("");
            if (dtstaff.Rows.Count > 0 && dtstaff.Rows.Count > 0)
            {
                loadspreadpopup(dtstaff);
                Internal.Checked = true;
                Internal_CheckedChanged(sender, e);
            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "No Record Found!";

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectAllotment"); }

    }

    protected void btnSearch_clickNEw(object sender, EventArgs e)
    {
        DataTable dtstaff = new DataTable();
        string searchValue = txtAlterFreeStaffSearch.Text;
        dtstaff = getFreeStaffListNew(searchValue);
        if (dtstaff.Rows.Count > 0 && dtstaff.Rows.Count > 0)
        {
            loadspreadpopup(dtstaff);
            Internal.Checked = true;
            //Internal_CheckedChanged(sender, e);
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
            if (External.Checked == true)
            {
                //if (ddlAlterFreeCollege.Items.Count > 0)
                //    qryCollegeFilter = " and sfm.college_code ='" + Convert.ToString(ddlAlterFreeCollege.SelectedValue).Trim() + "'";
                if (ddlAlterFreeDepartment.Items.Count > 0)
                    if (!string.IsNullOrEmpty(Convert.ToString(ddlAlterFreeDepartment.SelectedValue).Trim()) && Convert.ToString(ddlAlterFreeDepartment.SelectedValue).Trim().ToLower() != "all")
                        qryDeptFilter = " and hr.dept_code='" + Convert.ToString(ddlAlterFreeDepartment.SelectedValue).Trim() + "'";
                if (!string.IsNullOrEmpty(searchValue))
                    if (ddlAlterFreeStaff.Items.Count > 0)
                        if (ddlAlterFreeStaff.SelectedIndex == 0)
                            qryStaffFilter = "where es.staff_name like '" + searchValue + "%'";
                        else
                            qryStaffFilter = "where es.staff_code like '" + searchValue + "%'";
                qry = "select distinct  es.staff_code as appl_id,es.staff_code,es.staff_name+' [ '+Convert(varchar(10), es.staff_code)+' ]' as staff_name,isnull(es.totalexp,'-')as totalexp from external_staff es " + qryDeptFilter + qryStaffFilter + "";
                //select distinct  sm.staff_code as appl_id,sm.staff_code,sm.staff_name+' [ '+Convert(varchar(10), sm.staff_code)+' ]' as staff_name,'0' Experiance  from tbl_exam_valuatiuon_staff ev,external_staff sm,examstaffmaster c where ev.staff_code=Convert(nvarchar(50),sm.staff_code ) and c.staff_code=ev.staff_code and ev.isexternal='1'" + qryDeptFilter + qryStaffFilter + "
                dtFreeStaffList.Clear();
                dtFreeStaffList = dirAcc.selectDataTable(qry);
            }
            else
            {
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
                dtFreeStaffList.Clear();
                dtFreeStaffList = dirAcc.selectDataTable(qry);

            }
        }


            //sa.experience_info
        //qry = " select distinct sfm.staff_code,sfm.staff_name+' [ '+sfm.staff_code+' ]' as staff_name,'0' Experiance from staffmaster sfm inner join stafftrans sts on sts.staff_code=sfm.staff_code inner join hrdept_master hr on hr.dept_code=sts.dept_code where sts.latestrec='1' and sfm.resign=0 and sfm.settled=0 and sfm.college_code=hr.college_code " + qryCollegeFilter + qryDeptFilter + qryStaffFilter + " order by staff_name,sfm.staff_code";


        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectAllotment"); }
       
        return dtFreeStaffList;
    }

    private void loadspreadpopup(DataTable dtstaff)
    {
        try
        {
            fpstaff.Sheets[0].ColumnCount = 4;
            fpstaff.Sheets[0].RowCount = 0;
            fpstaff.SheetCorner.ColumnCount = 0;

            fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            fpstaff.Sheets[0].Columns[0].Width = 40;

            fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Name";
            fpstaff.Sheets[0].Columns[1].Width = 250;

            fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Exp";
            fpstaff.Sheets[0].Columns[2].Width = 80;

            fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Select";
            fpstaff.Sheets[0].Columns[3].Width = 40;
            fpstaff.Sheets[0].AutoPostBack = false;
            fpstaff.CommandBar.Visible = false;

            FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

            int rowcount = 0;
            for (int i = 0; i < dtstaff.Rows.Count; i++)
            {
                fpstaff.Sheets[0].RowCount++;
                fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 0].Text = fpstaff.Sheets[0].RowCount.ToString();
                fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 0].Locked = true;

                fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 1].CellType = txt;
                fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 1].Text = dtstaff.Rows[i]["staff_name"].ToString();
                fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 1].Locked = true;

                fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dtstaff.Rows[i]["staff_code"]).Trim();
                fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(dtstaff.Rows[i]["appl_id"]).Trim();
                fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                if (External.Checked == false)
                {
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
                    fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 2].CellType = txt;
                    fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 2].Text = totalexperience;
                }
                else
                {
                    fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 2].CellType = txt;
                    fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 2].Text = "year:" + dtstaff.Rows[i]["totalexp"].ToString() + "";
                }
                //dtstaff.Rows[i]["Experiance"].ToString();
                fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 2].Locked = true;

                fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 3].CellType = chk;
                fpstaff.Sheets[0].Cells[fpstaff.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

            }
            fpstaff.Sheets[0].Columns[fpstaff.Sheets[0].ColumnCount - 1].Width = 50;
            fpstaff.Sheets[0].Columns[fpstaff.Sheets[0].ColumnCount - 2].Width = 200;
            fpstaff.Sheets[0].Columns[fpstaff.Sheets[0].ColumnCount - 3].Width = 300;
            fpstaff.Sheets[0].Columns[fpstaff.Sheets[0].ColumnCount - 4].Width = 50;



            fpstaff.SaveChanges();
            fpstaff.Width = 600;
            fpstaff.Height = 300;
            divspreadpopup.Visible = true;
            fpstaff.Visible = true;
            btn_Save.Visible = false;
            fpstaff.Sheets[0].PageSize = fpstaff.Sheets[0].RowCount;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectAllotment"); }
    }

    protected void btnSelectStaff_Click(object sender, EventArgs e)
    {
        try
        {
            fpstaff.SaveChanges();
            fileuploadmodelquestionpaper();
            SmsEmail();
            string isinternalorexternal = (Internal.Checked) ? "internal" : (External.Checked) ? "external" : "";
            string selectStaffName = string.Empty;
            string selectStaffCode = string.Empty;
            string selectStaffApplId = string.Empty;
            int activeRow = Fpuser.ActiveSheetView.ActiveRow;
            int activeColumn = Fpuser.ActiveSheetView.ActiveColumn;
            for (int row = 0; row < fpstaff.Sheets[0].RowCount; row++)
            {
                int selected = 0;
                int.TryParse(Convert.ToString(fpstaff.Sheets[0].Cells[row, 3].Value).Trim(), out selected);

                if (selected == 1)
                {
                    string staffName = Convert.ToString(fpstaff.Sheets[0].Cells[row, 1].Text).Trim();
                    string staffCode = Convert.ToString(fpstaff.Sheets[0].Cells[row, 1].Tag).Trim();
                    string staffId = Convert.ToString(fpstaff.Sheets[0].Cells[row, 1].Note).Trim();

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

            Fpuser.Sheets[0].Cells[activeRow, activeColumn].Tag = selectStaffCode;
            Fpuser.Sheets[0].Cells[activeRow, activeColumn].Note = selectStaffApplId;

            Fpuser.Sheets[0].Columns[5].Visible = true;
            Fpuser.Sheets[0].Columns[5].Width = 200;
            Fpuser.Sheets[0].Cells[activeRow, 5].Text = selectStaffName;
            Fpuser.Sheets[0].Cells[activeRow, 5].Tag = selectStaffName;//isinternalorexternal
            Fpuser.Sheets[0].Cells[activeRow, 3].Tag = isinternalorexternal;
            //internalorexternal = Fpuser.Sheets[0].Cells[activeRow, 5].Tag.ToString();

            //Fpuser.Sheets[0].Cells[ACTROW, 3].Note = selectStaff;
            divAlterFreeStaffDetails.Visible = false;
            btn_Save.Visible = true;
            Fpuser.SaveChanges();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectAllotment"); }


    }

    protected void btnFreeStaffExit_Click(object sender, EventArgs e)
    {
        divAlterFreeStaffDetails.Visible = false;
    }

    protected void Internal_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            Fpuser.SaveChanges();
            txtAlterFreeStaffSearch.Text = "";
            College();
            BindAlterStaffDepartment(((ddlAlterFreeCollege.Items.Count > 0) ? Convert.ToString(ddlAlterFreeCollege.SelectedValue).Trim() : userCollegeCode));
            DataTable dtstaff = new DataTable();
            string searchValue = txtAlterFreeStaffSearch.Text;
            dtstaff = getFreeStaffListNew(searchValue);
            if (dtstaff.Rows.Count > 0 && dtstaff.Rows.Count > 0)
            {
                loadspreadpopup(dtstaff);
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Found!";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectAllotment"); }

    }

    protected void External_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            Fpuser.SaveChanges();
            txtAlterFreeStaffSearch.Text = "";
            College();
            BindAlterStaffDepartment(((ddlAlterFreeCollege.Items.Count > 0) ? Convert.ToString(ddlAlterFreeCollege.SelectedValue).Trim() : userCollegeCode));
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
                lblAlertMsg.Text = "No Record Found!";

            }


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectAllotment"); }
       

    }

    #endregion

    #region alertpopwindowclose
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }
    #endregion

    #region SmsEmailContent
    public void SmsEmail()
    {
        string smsmailqry = string.Empty;
        string sms = string.Empty;
        string email = string.Empty;
        try
        {
            sms = Convert.ToString(textarea_smscontent.InnerText);
            email = Convert.ToString(textarea_emailcontent.InnerText);
            string college = string.Empty;
            if (ddlCollege.Items.Count > 0)
                college = Convert.ToString(ddlCollege.SelectedValue);

            if (!string.IsNullOrEmpty(sms) || !string.IsNullOrEmpty(email) || (!string.IsNullOrEmpty(sms) && !string.IsNullOrEmpty(email)))
            {
                DataTable dtsmsmail = new DataTable();
                //smsmailqry = "insert into Tbl_SmsEmailContent (smscontent,emailcontent,college_code) values ('" + sms + "','" + email + "','" + Convert.ToString(ddlAlterFreeCollege.SelectedValue) + "')";
                smsmailqry = "if not exists(select * from Tbl_SmsEmailContent where college_code='" + college + "')insert into Tbl_SmsEmailContent (smscontent,emailcontent,college_code) values ('" + sms + "','" + email + "','" + college + "')else update Tbl_SmsEmailContent set smscontent='" + sms + "',emailcontent='" + email + "' where college_code='" + college + "'";
                dtsmsmail.Clear();
                int res = dirAcc.insertData(smsmailqry);
            }
        }
        catch
        {

        }

    }

    public void getdetailsSmsEmail()
    {
        string qrysmsmailcontent = string.Empty;
        try
        {

            DataTable dtsmsmail = new DataTable();
            qrysmsmailcontent = "  select top 1 tsm.smscontent,tsm.emailcontent from Tbl_SmsEmailContent tsm where college_code='" + Convert.ToString(ddlAlterFreeCollege.SelectedValue) + "'";
            dtsmsmail.Clear();
            dtsmsmail = dirAcc.selectDataTable(qrysmsmailcontent);
            if (dtsmsmail.Rows.Count > 0)
            {
                string smscontent = Convert.ToString(dtsmsmail.Rows[0]["smscontent"]).Trim();
                string mailcontent = Convert.ToString(dtsmsmail.Rows[0]["emailcontent"]).Trim();
                textarea_smscontent.InnerText = smscontent;
                textarea_emailcontent.InnerText = mailcontent;
            }
        }
        catch
        { }
    }

    #endregion

    #region TypeOfFile
    protected void fileuploadmodelquestionpaper()
    {
        try
        {
            Fpuser.SaveChanges();
            bool savnotsflag = false;
            string exammonth = string.Empty;
            string examyear = string.Empty;
            examyear = Convert.ToString(ddlExamYear.SelectedValue);
            exammonth = Convert.ToString(ddlExamMonth.SelectedValue);
            string Saveqry = string.Empty;
            string DeleteQry = string.Empty;
            string filenamesys = string.Empty;
            string filetypesys = string.Empty;
            if (!string.IsNullOrEmpty(exammonth) && !string.IsNullOrEmpty(examyear))
            {
                int activeRow = Fpuser.ActiveSheetView.ActiveRow;
                int activeColumn = Fpuser.ActiveSheetView.ActiveColumn;
                string SubjectNo = Convert.ToString(Fpuser.Sheets[0].Cells[activeRow, 1].Tag).Trim();
                string fileName = string.Empty;
                string documentType = string.Empty;
                if (fileupload.HasFile)
                {
                    //string fileName = string.Empty;
                    //string documentType = string.Empty;
                    string fileExtension = string.Empty;

                    bool FileFromat = false;
                    FileFromat = FileTypeCheck(fileupload, ref fileName, ref fileExtension, ref documentType);
                    //if (!(fileupload.HasFile && fileupload1.HasFile))
                    //{
                    //    DataTable dtfilesys = new DataTable();
                    //    string sysqry = "select ss.SubjectNo,ss.SysFileName,ss.SysFiletype from Subject_Syllabus_Modelquestionpaper ss where ExamMonth='" + exammonth + "' and ExamYear='" + examyear + "' and SubjectNo='" + SubjectNo + "'";
                    //    dtfilesys.Clear();
                    //    dtfilesys = dirAcc.selectDataTable(sysqry);
                    //    if (dtfilesys.Rows.Count > 0)
                    //    {
                    //        filenamesys = Convert.ToString(dtfilesys.Rows[0]["SysFileName"]).Trim();
                    //        filetypesys = Convert.ToString(dtfilesys.Rows[0]["SysFiletype"]).Trim();

                    //    }
                    //}
                    int fileSize = fileupload.PostedFile.ContentLength;
                    byte[] documentBinary = new byte[fileSize];
                    fileupload.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                    string datetime = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                    SqlCommand cmdnotes = new SqlCommand();

                    cmdnotes.CommandText = "if exists (select SubjectNo from Subject_Syllabus_Modelquestionpaper where ExamMonth='" + exammonth + "' and ExamYear='" + examyear + "' and SubjectNo='" + SubjectNo + "')  update Subject_Syllabus_Modelquestionpaper set ExamMonth=@exammonth,ExamYear=@examyear,SubjectNo=@subjectno,SysFileName=@DocName ,Syllabus=@DocData,SysFiletype=@Type,Date=@date where SubjectNo='" + SubjectNo + "' and ExamMonth='" + exammonth + "' and ExamYear='" + examyear + "'   else insert into Subject_Syllabus_Modelquestionpaper(ExamMonth,ExamYear,SubjectNo,SysFileName,Syllabus,SysFiletype,Date)" + " VALUES (@exammonth,@examyear,@subjectno,@DocName,@DocData,@Type,@date)";
                    cmdnotes.CommandType = CommandType.Text;
                    cmdnotes.Connection = ssql;
                    SqlParameter exammth = new SqlParameter("@exammonth", SqlDbType.Int, 100);
                    exammth.Value = exammonth;
                    cmdnotes.Parameters.Add(exammth);
                    SqlParameter examyr = new SqlParameter("@examyear", SqlDbType.Int, 100);
                    examyr.Value = examyear;
                    cmdnotes.Parameters.Add(examyr);
                    SqlParameter subno = new SqlParameter("@subjectno", SqlDbType.BigInt, 100);
                    subno.Value = SubjectNo;
                    cmdnotes.Parameters.Add(subno);
                    SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
                    DocName.Value = fileName.ToString();
                    cmdnotes.Parameters.Add(DocName);
                    SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
                    Type.Value = documentType.ToString();
                    cmdnotes.Parameters.Add(Type);
                    SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
                    uploadedDocument.Value = documentBinary;
                    cmdnotes.Parameters.Add(uploadedDocument);
                    SqlParameter uploadedDate = new SqlParameter("@date", SqlDbType.DateTime, 100);
                    uploadedDate.Value = datetime;
                    cmdnotes.Parameters.Add(uploadedDate);
                    ssql.Close();
                    ssql.Open();
                    int result = cmdnotes.ExecuteNonQuery();
                    if (result > 0)
                    {
                        savnotsflag = true;
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "File Uploaded SuccessFully";
                    }
                }
                if (fileupload1.HasFile)
                {
                    string fileName1 = string.Empty;
                    string documentType1 = string.Empty;
                    string fileExtension1 = string.Empty;
                    string filenamemodel = string.Empty;
                    string filetypesysmodel = string.Empty;
                    bool FileFromat1 = false;
                    FileFromat1 = FileTypeCheck1(fileupload1, ref fileName1, ref fileExtension1, ref documentType1);

                    //if (fileName != "")
                    //{
                    //    fileName1 = fileName + "$" + fileName1;
                    //}
                    //if (documentType != "")
                    //{
                    //    documentType1 = documentType + "$" + documentType1;
                    //}
                    //if (!(fileupload.HasFile && fileupload1.HasFile))
                    //{
                    //    DataTable dtfilesys = new DataTable();
                    //    string sysqry = "select ss.SubjectNo,ss.ModelFileName,ss.ModelFiletype from Subject_Syllabus_Modelquestionpaper ss where ExamMonth='" + exammonth + "' and ExamYear='" + examyear + "' and SubjectNo='" + SubjectNo + "'";
                    //    dtfilesys.Clear();
                    //    dtfilesys = dirAcc.selectDataTable(sysqry);
                    //    if (dtfilesys.Rows.Count > 0)
                    //    {
                    //        filenamemodel = Convert.ToString(dtfilesys.Rows[0]["ModelFileName"]).Trim();
                    //        filetypesysmodel = Convert.ToString(dtfilesys.Rows[0]["ModelFiletype"]).Trim();

                    //    }
                    //}
                    int fileSize = fileupload1.PostedFile.ContentLength;
                    byte[] documentBinary = new byte[fileSize];
                    fileupload1.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                    string datetime = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                    SqlCommand cmdnotes = new SqlCommand();

                    cmdnotes.CommandText = "if exists (select SubjectNo from Subject_Syllabus_Modelquestionpaper where ExamMonth='" + exammonth + "' and ExamYear='" + examyear + "' and SubjectNo='" + SubjectNo + "')  update Subject_Syllabus_Modelquestionpaper set ExamMonth=@exammonth,ExamYear=@examyear,SubjectNo=@subjectno,ModelFileName=@DocName,Modelquestionpaper=@DocData,ModelFiletype=@Type,Date=@date where SubjectNo='" + SubjectNo + "' and ExamMonth='" + exammonth + "' and ExamYear='" + examyear + "'   else insert into Subject_Syllabus_Modelquestionpaper(ExamMonth,ExamYear,SubjectNo,ModelFileName,Modelquestionpaper,ModelFiletype,Date)" + " VALUES (@exammonth,@examyear,@subjectno,@DocName,@DocData,@Type,@date)";
                    cmdnotes.CommandType = CommandType.Text;
                    cmdnotes.Connection = ssql;
                    SqlParameter exammth = new SqlParameter("@exammonth", SqlDbType.Int, 100);
                    exammth.Value = exammonth;
                    cmdnotes.Parameters.Add(exammth);
                    SqlParameter examyr = new SqlParameter("@examyear", SqlDbType.Int, 100);
                    examyr.Value = examyear;
                    cmdnotes.Parameters.Add(examyr);
                    SqlParameter subno = new SqlParameter("@subjectno", SqlDbType.BigInt, 100);
                    subno.Value = SubjectNo;
                    cmdnotes.Parameters.Add(subno);
                    SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
                    DocName.Value = fileName1.ToString();
                    cmdnotes.Parameters.Add(DocName);
                    SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
                    Type.Value = documentType1.ToString();
                    cmdnotes.Parameters.Add(Type);
                    SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
                    uploadedDocument.Value = documentBinary;
                    cmdnotes.Parameters.Add(uploadedDocument);
                    SqlParameter uploadedDate = new SqlParameter("@date", SqlDbType.DateTime, 100);
                    uploadedDate.Value = datetime;
                    cmdnotes.Parameters.Add(uploadedDate);
                    ssql.Close();
                    ssql.Open();
                    int result = cmdnotes.ExecuteNonQuery();
                    if (result > 0)
                    {
                        savnotsflag = true;
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "File Uploaded SuccessFully";
                    }
                }

            }
        }



        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "QuestionUpload"); }

    }
    #endregion

    #region alertclose
    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectAllotment"); }
    }
    #endregion

    #region SysFile
    protected bool FileTypeCheck(FileUpload UploadFile, ref string fileName, ref string fileExtension, ref string documentType)
    {
        bool fileBool = false;
        try
        {
            if (UploadFile.FileName.EndsWith(".jpg") || UploadFile.FileName.EndsWith(".gif") || UploadFile.FileName.EndsWith(".png") || UploadFile.FileName.EndsWith(".txt") || UploadFile.FileName.EndsWith(".doc") || UploadFile.FileName.EndsWith(".xls") || UploadFile.FileName.EndsWith(".docx") || UploadFile.FileName.EndsWith(".txt") || UploadFile.FileName.EndsWith(".document") || UploadFile.FileName.EndsWith(".xls") || UploadFile.FileName.EndsWith(".xlsx") || UploadFile.FileName.EndsWith(".pdf") || UploadFile.FileName.EndsWith(".ppt") || UploadFile.FileName.EndsWith(".pptx"))
            {
                fileName = Path.GetFileName(UploadFile.PostedFile.FileName);
                fileExtension = Path.GetExtension(UploadFile.PostedFile.FileName);
                documentType = string.Empty;
                switch (fileExtension)
                {
                    case ".pdf":
                        documentType = "application/pdf";
                        break;
                    case ".xls":
                        documentType = "application/vnd.ms-excel";
                        break;
                    case ".xlsx":
                        documentType = "application/vnd.ms-excel";
                        break;
                    case ".doc":
                        documentType = "application/vnd.ms-word";
                        break;
                    case ".docx":
                        documentType = "application/vnd.ms-word";
                        break;
                    case ".gif":
                        documentType = "image/gif";
                        break;
                    case ".png":
                        documentType = "image/png";
                        break;
                    case ".jpg":
                        documentType = "image/jpg";
                        break;
                    case ".ppt":
                        documentType = "application/vnd.ms-ppt";
                        break;
                    case ".pptx":
                        documentType = "application/vnd.ms-pptx";
                        break;
                    case ".txt":
                        documentType = "application/txt";
                        break;
                }
                if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(fileExtension) && !string.IsNullOrEmpty(documentType))
                    fileBool = true;
            }
        }
        catch { return fileBool; }
        return fileBool;
    }
    #endregion

    #region ModelFile
    protected bool FileTypeCheck1(FileUpload UploadFile1, ref string fileName1, ref string fileExtension1, ref string documentType1)
    {
        bool fileBool = false;
        try
        {
            if (UploadFile1.FileName.EndsWith(".jpg") || UploadFile1.FileName.EndsWith(".gif") || UploadFile1.FileName.EndsWith(".png") || UploadFile1.FileName.EndsWith(".txt") || UploadFile1.FileName.EndsWith(".doc") || UploadFile1.FileName.EndsWith(".xls") || UploadFile1.FileName.EndsWith(".docx") || UploadFile1.FileName.EndsWith(".txt") || UploadFile1.FileName.EndsWith(".document") || UploadFile1.FileName.EndsWith(".xls") || UploadFile1.FileName.EndsWith(".xlsx") || UploadFile1.FileName.EndsWith(".pdf") || UploadFile1.FileName.EndsWith(".ppt") || UploadFile1.FileName.EndsWith(".pptx"))
            {
                fileName1 = Path.GetFileName(UploadFile1.PostedFile.FileName);
                fileExtension1 = Path.GetExtension(UploadFile1.PostedFile.FileName);
                documentType1 = string.Empty;
                switch (fileExtension1)
                {
                    case ".pdf":
                        documentType1 = "application/pdf";
                        break;
                    case ".xls":
                        documentType1 = "application/vnd.ms-excel";
                        break;
                    case ".xlsx":
                        documentType1 = "application/vnd.ms-excel";
                        break;
                    case ".doc":
                        documentType1 = "application/vnd.ms-word";
                        break;
                    case ".docx":
                        documentType1 = "application/vnd.ms-word";
                        break;
                    case ".gif":
                        documentType1 = "image/gif";
                        break;
                    case ".png":
                        documentType1 = "image/png";
                        break;
                    case ".jpg":
                        documentType1 = "image/jpg";
                        break;
                    case ".ppt":
                        documentType1 = "application/vnd.ms-ppt";
                        break;
                    case ".pptx":
                        documentType1 = "application/vnd.ms-pptx";
                        break;
                    case ".txt":
                        documentType1 = "application/txt";
                        break;
                }
                if (!string.IsNullOrEmpty(fileName1) && !string.IsNullOrEmpty(fileExtension1) && !string.IsNullOrEmpty(documentType1))
                    fileBool = true;
            }
        }
        catch { return fileBool; }
        return fileBool;
    }
    #endregion

    #region ShowExistingFile

    protected void getexistingfile()
    {
        try
        {
            lnk_model.Visible = false;
            lnk_model.Text = string.Empty;
            lnk_Syllabus.Visible = false;
            lnk_Syllabus.Text = string.Empty;
            Fpuser.SaveChanges();
            string exammonth = string.Empty;
            string examyear = string.Empty;
            examyear = Convert.ToString(ddlExamYear.SelectedValue);
            exammonth = Convert.ToString(ddlExamMonth.SelectedValue);
            string Saveqry = string.Empty;
            string DeleteQry = string.Empty;
            DataSet dtfile = new DataSet();
            if (!string.IsNullOrEmpty(exammonth) && !string.IsNullOrEmpty(examyear))
            {
                int activeRow = Fpuser.ActiveSheetView.ActiveRow;
                int activeColumn = Fpuser.ActiveSheetView.ActiveColumn;
                string SubjectNo = Convert.ToString(Fpuser.Sheets[0].Cells[activeRow, 1].Tag).Trim();
                string Existingfile = " select s.ExamMonth,s.ExamYear,s.SubjectNo,s.SysFileName,s.ModelFileName,s.Syllabus,s.Modelquestionpaper from Subject_Syllabus_Modelquestionpaper s where SubjectNo in('" + SubjectNo + "')";
                dtfile.Clear();
                dtfile = d2.select_method_wo_parameter(Existingfile, "Text");
                if (dtfile.Tables.Count > 0 && dtfile.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dtfile.Tables[0].Rows.Count; i++)
                    {
                        string Sysfilename = Convert.ToString(dtfile.Tables[0].Rows[i]["SysFileName"]).Trim();
                        string Modelfilename = Convert.ToString(dtfile.Tables[0].Rows[i]["ModelFileName"]).Trim();
                        string SyllabusFile = Convert.ToString(dtfile.Tables[0].Rows[i]["Syllabus"]).Trim();
                        string Modelfile = Convert.ToString(dtfile.Tables[0].Rows[i]["Modelquestionpaper"]).Trim();
                        if (!string.IsNullOrEmpty(Sysfilename) && !string.IsNullOrEmpty(SyllabusFile))
                        {

                            lnk_Syllabus.Visible = true;
                            lnk_Syllabus.Text = Sysfilename;

                        }
                        if (!string.IsNullOrEmpty(Modelfilename) && !string.IsNullOrEmpty(Modelfile))
                        {

                            lnk_model.Visible = true;
                            lnk_model.Text = Modelfilename;

                        }
                    }
                }

            }

        }

        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "QuestionUpload"); }

    }

    #endregion

    #region syallbus
    protected void lnk_Syllabus_Click(object sender, EventArgs e)
    {
        try
        {
            Fpuser.SaveChanges();
            string exammonth = string.Empty;
            string examyear = string.Empty;
            examyear = Convert.ToString(ddlExamYear.SelectedValue);
            exammonth = Convert.ToString(ddlExamMonth.SelectedValue);
            string Saveqry = string.Empty;
            string DeleteQry = string.Empty;
            DataSet dssyllabus = new DataSet();
            if (!string.IsNullOrEmpty(exammonth) && !string.IsNullOrEmpty(examyear))
            {
                int activeRow = Fpuser.ActiveSheetView.ActiveRow;
                int activeColumn = Fpuser.ActiveSheetView.ActiveColumn;
                string SubjectNo = Convert.ToString(Fpuser.Sheets[0].Cells[activeRow, 1].Tag).Trim();

                string Sysfile = " select s.ExamMonth,s.ExamYear,s.SubjectNo,s.SysFileName,s.Syllabus,s.SysFiletype from Subject_Syllabus_Modelquestionpaper s where SubjectNo in('" + SubjectNo + "')";
                dssyllabus.Clear();
                dssyllabus = d2.select_method_wo_parameter(Sysfile, "Text");
                for (int i = 0; i < dssyllabus.Tables[0].Rows.Count; i++)
                {
                    Response.ContentType = dssyllabus.Tables[0].Rows[i]["SysFiletype"].ToString();
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + dssyllabus.Tables[0].Rows[i]["SysFileName"] + "\"");
                    Response.BinaryWrite((byte[])dssyllabus.Tables[0].Rows[i]["Syllabus"]);
                    Response.End();

                }
            }

        }

        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "QuestionUpload"); }

    }
    #endregion

    #region ModelQuestion
    protected void lnk_model_Click(object sender, EventArgs e)
    {
        try
        {
            Fpuser.SaveChanges();

            string exammonth = string.Empty;
            string examyear = string.Empty;
            examyear = Convert.ToString(ddlExamYear.SelectedValue);
            exammonth = Convert.ToString(ddlExamMonth.SelectedValue);
            string Saveqry = string.Empty;
            string DeleteQry = string.Empty;
            DataSet dsmodel = new DataSet();
            if (!string.IsNullOrEmpty(exammonth) && !string.IsNullOrEmpty(examyear))
            {
                int activeRow = Fpuser.ActiveSheetView.ActiveRow;
                int activeColumn = Fpuser.ActiveSheetView.ActiveColumn;
                string SubjectNo = Convert.ToString(Fpuser.Sheets[0].Cells[activeRow, 1].Tag).Trim();

                string Modelfile = " select s.ExamMonth,s.ExamYear,s.SubjectNo,s.ModelFileName,s.Modelquestionpaper,s.ModelFiletype from Subject_Syllabus_Modelquestionpaper s where SubjectNo in('" + SubjectNo + "')";
                dsmodel.Clear();
                dsmodel = d2.select_method_wo_parameter(Modelfile, "Text");
                for (int i = 0; i < dsmodel.Tables[0].Rows.Count; i++)
                {
                    Response.ContentType = dsmodel.Tables[0].Rows[i]["ModelFiletype"].ToString();
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + dsmodel.Tables[0].Rows[i]["ModelFileName"] + "\"");
                    Response.BinaryWrite((byte[])dsmodel.Tables[0].Rows[i]["Modelquestionpaper"]);
                    Response.End();

                }
            }

        }

        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "QuestionUpload"); }


    }
    #endregion
}