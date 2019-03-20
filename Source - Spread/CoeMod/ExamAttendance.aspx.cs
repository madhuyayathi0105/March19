/*
 * Page Reconstructed by Idhris 
 * start date : 17-02-2017
 * */

using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using InsproDataAccess;
using System.Text;
using System.Configuration;

public partial class ExamAttendance : System.Web.UI.Page
{
    Boolean flag_true = false;
    Boolean yes_flag = false;
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    ReuasableMethods reUse = new ReuasableMethods();
    InsproDirectAccess dirAccess = new InsproDirectAccess();
    string userCode = string.Empty;
    string collegeCode = string.Empty;

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
            userCode = Session["usercode"].ToString();
            lblerror.Visible = false;
            if (!Page.IsPostBack)
            {
                bindCollege();
                cb_College_CheckedChanged(sender, e);

                string Master = "select * from Master_Settings where usercode=" + userCode + "";
                DataSet dsma = d2.select_method_wo_parameter(Master, "text");
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                for (int i = 0; i < dsma.Tables[0].Rows.Count; i++)
                {
                    if (dsma.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && dsma.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (dsma.Tables[0].Rows[i]["settings"].ToString() == "Register No" && dsma.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                }
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
            typeChange();
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
            typeChange();
        }
        catch { }
    }
    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        Savebtn.Visible = false;
        Subjectspread.Visible = false;
        ddlfrmdate.Items.Clear();
        //string getexamdate = "select distinct convert(varchar(10),exdt.Exam_date,105) as exam_date,exdt.Exam_date  as exdate from exmtt_det as exdt,exmtt as exm where exm.exam_code=exdt.exam_code and exm.exam_month=" + ddlMonth.SelectedItem.Value + " and exm.exam_year=" + ddlYear.SelectedValue.ToString() + " ";
        //getexamdate = getexamdate + "   union all";
        //getexamdate = getexamdate + "  select distinct  convert(varchar(10),e.ExamDate,105) as exam_date,e.ExamDate as exdate from examtheorybatch e,subject su where e.subno=su.subject_no  and DATEPART(year,ExamDate)=" + ddlYear.SelectedValue.ToString() + " order by exam_date";

        string getexamdate = "select distinct convert(varchar(10),exdt.Exam_date,105) as exam_date ,datepart(day ,exam_date),datepart(month,exam_date),datepart(year,exam_date) from exmtt_det as exdt,exmtt as exm,degree d,Course c where exm.exam_code=exdt.exam_code and exm.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id  and exm.exam_month=" + ddlMonth.SelectedValue.ToString() + " and exm.exam_year=" + ddlYear.SelectedValue.ToString() + " ";
        getexamdate = getexamdate + "   union all";
        getexamdate = getexamdate + "   select distinct  convert(varchar(10),e.ExamDate,105) as exam_date,datepart(day ,ExamDate),datepart(month,ExamDate),datepart(year,ExamDate) from examtheorybatch e,subject su where e.subno=su.subject_no   order by datepart(year,exam_date),datepart(month,exam_date),datepart(day ,exam_date),exam_date";
        DataSet ds1 = d2.select_method_wo_parameter(getexamdate, "text");
        if (ds1.Tables[0].Rows.Count > 0)
        {
            ddlfrmdate.DataSource = ds1;
            ddlfrmdate.DataValueField = "Exam_date";
            ddlfrmdate.DataBind();
        }
        typeChange();
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Savebtn.Visible = false;
            Subjectspread.Visible = false;
            ddlfrmdate.Items.Clear();
            //string getexamdate = "select distinct convert(varchar(10),exdt.Exam_date,105) as exam_date from exmtt_det as exdt,exmtt as exm where exm.exam_code=exdt.exam_code and exm.exam_month=" + ddlMonth.SelectedItem.Value + " and exm.exam_year=" + ddlYear.SelectedValue.ToString() + "";
            //getexamdate = getexamdate + "   union all";
            //getexamdate = getexamdate + "    select distinct  convert(varchar(10),e.ExamDate,105) as exam_date from examtheorybatch e,subject su where e.subno=su.subject_no  and DATEPART(year,ExamDate)=" + ddlYear.SelectedValue.ToString() + " order by exam_date";


            string getexamdate = "select distinct convert(varchar(10),exdt.Exam_date,105) as exam_date ,datepart(day ,exam_date),datepart(month,exam_date),datepart(year,exam_date) from exmtt_det as exdt,exmtt as exm,degree d,Course c where exm.exam_code=exdt.exam_code and exm.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id  and exm.exam_month=" + ddlMonth.SelectedValue.ToString() + " and exm.exam_year=" + ddlYear.SelectedValue.ToString() + " ";
            getexamdate = getexamdate + "   union all";
            getexamdate = getexamdate + "   select distinct  convert(varchar(10),e.ExamDate,105) as exam_date,datepart(day ,ExamDate),datepart(month,ExamDate),datepart(year,ExamDate) from examtheorybatch e,subject su where e.subno=su.subject_no  order by datepart(year,exam_date),datepart(month,exam_date),datepart(day ,exam_date),exam_date";
            DataSet ds1 = d2.select_method_wo_parameter(getexamdate, "text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                ddlfrmdate.DataSource = ds1;
                ddlfrmdate.DataValueField = "Exam_date";
                ddlfrmdate.DataBind();

            }
        }
        catch { }
        typeChange();
    }
    protected void Subjectspread_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        string actrow = e.SheetView.ActiveRow.ToString();
        if (flag_true == false && actrow == "0")
        {
            for (int j = 0; j < Convert.ToInt16(Subjectspread.Sheets[0].RowCount); j++)
            {
                string actcol = e.SheetView.ActiveColumn.ToString();
                string seltext = e.EditValues[Convert.ToInt16(actcol)].ToString();
                if (seltext != "System.Object")
                {
                    Subjectspread.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = seltext.ToString();
                    Subjectspread.Sheets[0].Cells[j, Convert.ToInt16(actcol)].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            flag_true = true;
        }

    }
    protected void btnGo_Click(object sender, EventArgs e)
    {

        if (ddlType.SelectedIndex == 0)
        {
            //Subject Wise Attendance
            loadSubjectStudents();
        }
        else
        {
            //Hall Wise Attendance
            loadHallStudents();
        }

    }
    public void loadSubjectStudents()
    {
        try
        {
            Subjectspread.SaveChanges();
            Subjectspread.Visible = true;
            Subjectspread.Sheets[0].ColumnCount = 8;
            Subjectspread.RowHeader.Visible = false;
            Subjectspread.Sheets[0].RowCount = 0;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            Subjectspread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            Subjectspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            Subjectspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].DefaultStyle.Font.Bold = false;
            Subjectspread.Sheets[0].AutoPostBack = false;
            Subjectspread.CommandBar.Visible = false;



            FarPoint.Web.Spread.ComboBoxCellType objintcell = new FarPoint.Web.Spread.ComboBoxCellType();
            FarPoint.Web.Spread.ButtonCellType img = new FarPoint.Web.Spread.ButtonCellType();
            img.CssClass = "submit";
            img.OnClientClick = "return chagevalue(this)";
            img.Text = "";
            string[] strcomo1 = new string[] { "Select for All ", " ", "P", "A" };
            string[] strcomo = new string[] { " ", "P", "A" };
            objintcell = new FarPoint.Web.Spread.ComboBoxCellType(strcomo1);
            objintcell.AutoPostBack = true;
            objintcell.ShowButton = true;
            objintcell.UseValue = true;
            FarPoint.Web.Spread.ComboBoxCellType objcom = new FarPoint.Web.Spread.ComboBoxCellType();
            objcom = new FarPoint.Web.Spread.ComboBoxCellType(strcomo);
            Subjectspread.Sheets[0].AutoPostBack = true;

            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Degree";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Branch";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Attendance";

            Subjectspread.Columns[0].Width = 50;
            Subjectspread.Columns[0].Locked = true;
            Subjectspread.Columns[1].Width = 100;
            Subjectspread.Columns[1].Locked = true;
            Subjectspread.Columns[2].Width = 150;
            Subjectspread.Columns[2].Locked = true;
            Subjectspread.Columns[3].Width = 200;
            Subjectspread.Columns[3].Locked = true;
            Subjectspread.Columns[4].Width = 100;
            Subjectspread.Columns[4].Locked = true;
            Subjectspread.Columns[5].Width = 50;
            Subjectspread.Columns[5].Locked = true;
            Subjectspread.Columns[6].Width = 150;
            Subjectspread.Columns[6].Locked = true;
            Subjectspread.Columns[7].Width = 100;

            string subjectno1 = "";
            if (ddlsubject.Items.Count > 0)
            {
                string session = Convert.ToString(ddlsession.SelectedItem.Text);
                string maindegree = "";
                string spreadbind = "";
                if (ddlpart.SelectedIndex == 0 && ddlbatch.SelectedIndex == 0)
                {
                    spreadbind = "select  distinct s.subject_code, s.subject_Name as SubjectName,d.degree_code from exmtt e,exmtt_det ex,Course c,Department dpt,degree d,subject s where c.Course_Id=d.Course_Id and  d.Degree_Code=e.degree_code and dpt.Dept_Code=d.dept_code and s.subject_no=ex.subject_no and ex.exam_code=e.exam_code  and e.exam_type='Univ' and convert(varchar(10),ex.exam_date,105) in( '" + ddlfrmdate.SelectedValue.ToString() + "' ) and exam_session like '%" + ddlsession.SelectedItem.Text + "%' and s.subject_code ='" + Convert.ToString(ddlsubject.SelectedItem.Value) + "'  and c.college_code in (" + collegeCode + ") ";
                }
                else
                {

                    spreadbind = "select distinct degreecode as degree_code from COESubSubjectPartMater co,examtheorybatch eth,COESubSubjectPartSettings cs where examyear=" + ddlYear.SelectedValue + " and exammonth='" + ddlMonth.SelectedValue + "' and convert(varchar(10),eth.examdate,105) in( '" + ddlfrmdate.SelectedValue.ToString() + "' ) and examsession like '%" + ddlsession.SelectedItem.Text + "%' and cs.id=co.id and cs.subsubjectid=eth.subsubjectid  ";
                }

                DataSet ds2 = d2.select_method_wo_parameter(spreadbind, "text");
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    for (int row = 0; row < ds2.Tables[0].Rows.Count; row++)
                    {
                        if (maindegree == "")
                        {
                            maindegree = Convert.ToString(ds2.Tables[0].Rows[row]["degree_code"]);
                        }
                        else
                        {
                            maindegree = maindegree + "," + Convert.ToString(ds2.Tables[0].Rows[row]["degree_code"]);
                        }
                    }
                }

                string[] examDateArr = ddlfrmdate.SelectedItem.Text.Split('-');
                string examDate = examDateArr[1] + "/" + examDateArr[0] + "/" + examDateArr[2];
                string subject_code = Convert.ToString(ddlsubject.SelectedItem.Value);
                string examMonth = ddlMonth.SelectedValue;
                string examYear = ddlYear.SelectedValue;
                //string studforsubject = "select distinct r.reg_no as regno,r.stud_name as stuname,r.roll_no as roll,r.stud_type as typeofstud,r.current_semester as sem,r.degree_code ,r.Batch_Year,s.subject_no,ead.appl_no from exam_application ea, Exam_Details ed,exam_appl_details ead,subject s,Registration r,exmtt et,exmtt_det etd  where ea.roll_no=r.Roll_No and s.subject_no=ead.subject_no and ead.appl_no=ea.appl_no and  ea.exam_code = ed.exam_code  and et.exam_code=etd.exam_code and et.degree_code=ed.degree_code and et.batchTo=ed.batch_year and et.Exam_month=ed.Exam_Month and et.Exam_year=ed.Exam_year and etd.subject_no= ead.subject_no and s.subject_no=etd.subject_no  and ed.Exam_Month='" + examMonth + "' and ed.Exam_year='" + examYear + "' and subject_code='" + subject_code + "'  and etd.exam_date='" + examDate + "'  and etd.exam_session ='" + ddlsession.SelectedItem.Text + " '  and r.degree_code in (" + maindegree + ")  and r.college_code in (" + collegeCode + ")  order by degree_code,batch_year desc,reg_no asc ";
                //magesh 4/1/18
                string studforsubject = string.Empty;
                if (ddlpart.SelectedIndex == 0 && ddlbatch.SelectedIndex == 0)
                {
                    studforsubject = "select distinct r.reg_no as regno,r.stud_name as stuname,r.roll_no as roll,r.stud_type as typeofstud,r.current_semester as sem,r.degree_code ,r.Batch_Year,s.subject_no,ead.appl_no from exam_application ea, Exam_Details ed,exam_appl_details ead,subject s,Registration r,exmtt et,exmtt_det etd  where ea.roll_no=r.Roll_No and s.subject_no=ead.subject_no and ead.appl_no=ea.appl_no and  ea.exam_code = ed.exam_code  and et.exam_code=etd.exam_code and et.degree_code=ed.degree_code and et.batchTo=ed.batch_year and et.Exam_month=ed.Exam_Month and et.Exam_year=ed.Exam_year and etd.subject_no= ead.subject_no and s.subject_no=etd.subject_no  and ed.Exam_Month='" + examMonth + "' and ed.Exam_year='" + examYear + "' and subject_code='" + subject_code + "'  and etd.exam_date='" + examDate + "'  and etd.exam_session  like '%" + ddlsession.SelectedItem.Text + "%'  and r.degree_code in (" + maindegree + ")  and r.college_code in (" + collegeCode + ")  order by r.degree_code,r.batch_year desc,r.reg_no asc ";


                }
                else
                {
                    if (ddlbatch.SelectedIndex == 0)
                    {
                        studforsubject = " select distinct r.reg_no as regno,r.stud_name as stuname,r.roll_no as roll,r.stud_type as typeofstud,r.current_semester as sem,r.degree_code ,r.Batch_Year,s.subject_no,ead.appl_no from exam_application ea, Exam_Details ed,exam_appl_details ead,subject s,Registration r,examtheorybatch eth,COESubSubjectPartMater co,COESubSubjectPartSettings cs  where ea.roll_no=r.Roll_No and s.subject_no=ead.subject_no and ead.appl_no=ea.appl_no and  ea.exam_code = ed.exam_code       and ed.Exam_Month='" + examMonth + "' and ed.Exam_year='" + examYear + "' and subject_code='" + subject_code + "'   and     convert(varchar(10),eth.examdate,105) in ( '" + ddlfrmdate.SelectedValue.ToString() + "' )   and r.degree_code in (" + maindegree + ") and eth.appno=r.app_no   and r.college_code in (" + collegeCode + ")  and eth.SubNo=s.subject_no and cs.SubCode=s.subject_code and eth.SubSubjectID=cs.SubSubjectID and co.id=cs.id and ed.Exam_Month=co.ExamMonth and ed.Exam_year=co.ExamYear and eth.ExamCode=ea.exam_code  and eth.examsession like '%" + ddlsession.SelectedItem.Text + "%'   order by r.degree_code,r.batch_year desc,r.reg_no asc ";
                    }
                    else
                        studforsubject = " select distinct r.reg_no as regno,r.stud_name as stuname,r.roll_no as roll,r.stud_type as typeofstud,r.current_semester as sem,r.degree_code ,r.Batch_Year,s.subject_no,ead.appl_no from exam_application ea, Exam_Details ed,exam_appl_details ead,subject s,Registration r,examtheorybatch eth,COESubSubjectPartMater co,COESubSubjectPartSettings cs  where ea.roll_no=r.Roll_No and s.subject_no=ead.subject_no and ead.appl_no=ea.appl_no and  ea.exam_code = ed.exam_code       and ed.Exam_Month='" + examMonth + "' and ed.Exam_year='" + examYear + "' and subject_code='" + subject_code + "'   and     convert(varchar(10),eth.examdate,105) in ( '" + ddlfrmdate.SelectedValue.ToString() + "' )   and r.degree_code in (" + maindegree + ") and eth.appno=r.app_no   and r.college_code in (" + collegeCode + ")  and eth.SubNo=s.subject_no and cs.SubCode=s.subject_code and eth.SubSubjectID=cs.SubSubjectID and co.id=cs.id and ed.Exam_Month=co.ExamMonth and ed.Exam_year=co.ExamYear and eth.ExamCode=ea.exam_code  and eth.examsession like '%" + ddlsession.SelectedItem.Text + "%'  and eth.Batch='" + ddlbatch.SelectedItem.Text + "' order by r.degree_code,r.batch_year desc,r.reg_no asc ";
                    //  studforsubject = "select distinct r.reg_no as regno,r.stud_name as stuname,r.roll_no as roll,r.stud_type as typeofstud,r.current_semester as sem,r.degree_code ,r.Batch_Year,s.subject_no,ead.appl_no from exam_application ea, Exam_Details ed,exam_appl_details ead,subject s,Registration r,exmtt et,exmtt_det etd  where ea.roll_no=r.Roll_No and s.subject_no=ead.subject_no and ead.appl_no=ea.appl_no and  ea.exam_code = ed.exam_code  and et.exam_code=etd.exam_code and et.degree_code=ed.degree_code and et.batchTo=ed.batch_year and et.Exam_month=ed.Exam_Month and et.Exam_year=ed.Exam_year and etd.subject_no= ead.subject_no and s.subject_no=etd.subject_no  and ed.Exam_Month='" + examMonth + "' and ed.Exam_year='" + examYear + "' and subject_code='" + subject_code + "'  and etd.exam_date='" + examDate + "'  and etd.exam_session  like '%" + ddlsession.SelectedItem.Text + "%'  and r.degree_code in (" + maindegree + ")  and r.college_code in (" + collegeCode + ")  order by r.degree_code,r.batch_year desc,r.reg_no asc ";


                }
                DataSet ds3 = d2.select_method_wo_parameter(studforsubject, "text");
                DataSet dsCourseDegDet = d2.select_method_wo_parameter(" select C.Course_Name,dt.Dept_Name,d.Degree_Code from  Degree d,Department dt,Course c  where  d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and d.Degree_Code in (" + maindegree + ")  and c.college_code in (" + collegeCode + ") ", "Text");

                string regno = "";
                string studname = "";
                string rollno = "";
                string studtype = "";
                string sem = "";
                int sno = 1;
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                if (ds3.Tables[0].Rows.Count > 0)
                {
                    Subjectspread.Sheets[0].RowCount = Subjectspread.Sheets[0].RowCount + 1;
                    Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 1, 0, 1, 5);
                    Subjectspread.Sheets[0].Cells[0, 7].CellType = objintcell;
                    for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                    {
                        string batchyear = Convert.ToString(ds3.Tables[0].Rows[i]["Batch_Year"]);
                        string degreecode1 = Convert.ToString(ds3.Tables[0].Rows[i]["degree_code"]);
                        sem = Convert.ToString(ds3.Tables[0].Rows[i]["sem"]);

                        string courseName = string.Empty;
                        string deptname = string.Empty;

                        if (dsCourseDegDet.Tables.Count > 0 && dsCourseDegDet.Tables[0].Rows.Count > 0)
                        {
                            dsCourseDegDet.Tables[0].DefaultView.RowFilter = "Degree_Code='" + degreecode1 + "'";
                            DataView dv = dsCourseDegDet.Tables[0].DefaultView;
                            if (dv.Count > 0)
                            {
                                courseName = dv[0]["Course_Name"].ToString();
                                deptname = dv[0]["Dept_Name"].ToString();
                            }
                        }

                        Savebtn.Visible = true;
                        Subjectspread.Sheets[0].RowCount = Subjectspread.Sheets[0].RowCount + 1;
                        regno = ds3.Tables[0].Rows[i]["regno"].ToString();
                        studname = ds3.Tables[0].Rows[i]["stuname"].ToString();
                        rollno = ds3.Tables[0].Rows[i]["roll"].ToString();
                        studtype = ds3.Tables[0].Rows[i]["typeofstud"].ToString();
                        string subjectcode = Convert.ToString(ddlsubject.SelectedItem.Value);
                        subjectno1 = Convert.ToString(ds3.Tables[0].Rows[i]["subject_no"]);

                        //sem = ds3.Tables[0].Rows[i]["sem"].ToString();
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 7].CellType = objcom;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 0].Text = sno + "";
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 1].CellType = txt;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 1].Text = rollno;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 2].CellType = txt;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 2].Text = regno;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 3].Text = studname;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 4].Text = studtype;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 5].Text = courseName;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 6].Text = deptname;

                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 0].Note = session;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds3.Tables[0].Rows[i]["appl_no"]);
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 1].Note = subjectno1;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 2].Note = batchyear;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 3].Note = degreecode1;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 4].Note = sem;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 5].Note = subjectcode;
                        sno++;
                        //========================

                        int totalrows = Subjectspread.Sheets[0].RowCount;
                        Subjectspread.Sheets[0].PageSize = totalrows * 10;
                        Subjectspread.Height = 400;// (totalrows + 30) * 10;
                        if (Session["Rollflag"] == "0")
                        {
                            Subjectspread.Width = 580;
                            Subjectspread.Sheets[0].Columns[1].Visible = false;
                        }
                        if (Session["Regflag"] == "0")
                        {
                            Subjectspread.Width = 560;
                            Subjectspread.Sheets[0].Columns[2].Visible = false;
                        }

                        //=========
                        string noofperiods = "select No_of_hrs_per_day as tothrs,No_of_hrs_I_half_day as FNhrs,No_of_hrs_II_half_day as ANhrs  from PeriodAttndSchedule where degree_code=" + degreecode1 + " and semester=" + sem + "";
                        DataSet ds11 = d2.select_method_wo_parameter(noofperiods, "text");
                        string totalhrs = "";
                        string fsthalfhrs = "";
                        string scndhalfhrs = "";
                        string leavecode = "";
                        string value = "";
                        string reqdate = "";
                        int reqdatenew = 0;

                        if (ds11.Tables[0].Rows.Count > 0)
                        {
                            totalhrs = ds11.Tables[0].Rows[0]["tothrs"].ToString();
                            fsthalfhrs = ds11.Tables[0].Rows[0]["FNhrs"].ToString();
                            scndhalfhrs = ds11.Tables[0].Rows[0]["ANhrs"].ToString();
                            string examdate = ddlfrmdate.SelectedValue.ToString();
                            string[] splitdate = examdate.Split(new Char[] { '-' });
                            reqdate = splitdate[0].ToString();
                            reqdatenew = Convert.ToInt32(reqdate);
                            string reqmonth = splitdate[1].ToString();


                            //for F.N
                            string attvalue = "";
                            string obtainedattenance = "";
                            if (session == "F.N")
                            {
                                value = ("d" + reqdatenew + "d" + Convert.ToInt32(fsthalfhrs));

                                //string rollno = Subjectspread.Sheets[0].Cells[i1, 5].Note;
                                int my = Convert.ToInt32(ddlMonth.SelectedItem.Value.ToString()) + Convert.ToInt32(ddlYear.SelectedValue.ToString()) * 12;
                                string selectattend = "select " + value + " from attendance where roll_no='" + rollno + "' and month_year=" + my + "";
                                DataSet ds12 = d2.select_method_wo_parameter(selectattend, "text");
                                if (ds12.Tables[0].Rows.Count > 0)
                                {
                                    attvalue = ds12.Tables[0].Rows[0][value].ToString();
                                    obtainedattenance = Attmark(attvalue);
                                    Subjectspread.Sheets[0].SetText(Subjectspread.Sheets[0].RowCount - 1, 7, obtainedattenance);
                                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;

                                }
                                else
                                {
                                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 7].Text = "P";
                                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                }

                            }
                            //for A.F
                            if (session == "A.N")
                            {

                                int secondhalfour = Convert.ToInt32(fsthalfhrs) + 1;
                                value = ("d" + reqdatenew + "d" + secondhalfour);
                                int my = Convert.ToInt32(ddlMonth.SelectedItem.Value.ToString()) + Convert.ToInt32(ddlYear.SelectedValue.ToString()) * 12;
                                string selectattend = "select " + value + " from attendance where roll_no='" + rollno + "' and month_year=" + my + "";
                                DataSet ds13 = d2.select_method_wo_parameter(selectattend, "text");
                                if (ds13.Tables[0].Rows.Count > 0)
                                {
                                    attvalue = ds13.Tables[0].Rows[0][value].ToString();
                                    obtainedattenance = Attmark(attvalue);
                                    Subjectspread.Sheets[0].SetText(Subjectspread.Sheets[0].RowCount - 1, 7, obtainedattenance);
                                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                }
                                else
                                {
                                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 7].Text = "P";
                                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                        }
                        //==

                    }
                }
                Subjectspread.Sheets[0].AutoPostBack = false;
                if (Subjectspread.Rows.Count > 1)
                {
                    for (int cbl = 0; cbl < cblColumnOrder.Items.Count; cbl++)
                    {
                        if (cblColumnOrder.Items[cbl].Selected)
                            Subjectspread.Columns[cbl].Visible = true;
                        else
                            Subjectspread.Columns[cbl].Visible = false;
                    }
                    Subjectspread.Visible = true;
                    div_report.Visible = true;
                    Subjectspread.Sheets[0].FrozenColumnCount = 5;
                    Subjectspread.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Subjectspread.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Subjectspread.Sheets[0].SetColumnMerge(6, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Subjectspread.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
                    Subjectspread.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
                    Subjectspread.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
                    Subjectspread.Width = 850;
                    Subjectspread.Sheets[0].PageSize = Subjectspread.Sheets[0].RowCount;
                    Subjectspread.SaveChanges();
                }
                else
                {
                    Subjectspread.Visible = false;
                    div_report.Visible = false;
                    lblerror.Visible = true;
                    lblerror.Text = "No Records Found";
                }
            }
            else
            {
                Subjectspread.Visible = false;
                div_report.Visible = false;
                lblerror.Visible = true;
                lblerror.Text = "Please Select Subject";
            }
        }
        catch
        {
            Subjectspread.Visible = false;
            div_report.Visible = false;
            lblerror.Visible = true;
            lblerror.Text = "No Records Found";
        }
    }
    public void loadHallStudents()
    {
        try
        {
            Subjectspread.SaveChanges();
            Subjectspread.Visible = true;
            Subjectspread.Sheets[0].ColumnCount = 8;
            Subjectspread.RowHeader.Visible = false;
            Subjectspread.Sheets[0].RowCount = 0;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            Subjectspread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            Subjectspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            Subjectspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].DefaultStyle.Font.Bold = false;
            Subjectspread.Sheets[0].AutoPostBack = false;
            Subjectspread.CommandBar.Visible = false;



            FarPoint.Web.Spread.ComboBoxCellType objintcell = new FarPoint.Web.Spread.ComboBoxCellType();
            FarPoint.Web.Spread.ButtonCellType img = new FarPoint.Web.Spread.ButtonCellType();
            img.CssClass = "submit";
            img.OnClientClick = "return chagevalue(this)";
            img.Text = "";
            string[] strcomo1 = new string[] { "Select for All ", " ", "P", "A" };
            string[] strcomo = new string[] { " ", "P", "A" };
            objintcell = new FarPoint.Web.Spread.ComboBoxCellType(strcomo1);
            objintcell.AutoPostBack = true;
            objintcell.ShowButton = true;
            objintcell.UseValue = true;
            FarPoint.Web.Spread.ComboBoxCellType objcom = new FarPoint.Web.Spread.ComboBoxCellType();
            objcom = new FarPoint.Web.Spread.ComboBoxCellType(strcomo);
            Subjectspread.Sheets[0].AutoPostBack = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Degree";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Branch";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Attendance";

            Subjectspread.Columns[0].Width = 50;
            Subjectspread.Columns[0].Locked = true;
            Subjectspread.Columns[1].Width = 100;
            Subjectspread.Columns[1].Locked = true;
            Subjectspread.Columns[2].Width = 150;
            Subjectspread.Columns[2].Locked = true;
            Subjectspread.Columns[3].Width = 200;
            Subjectspread.Columns[3].Locked = true;
            Subjectspread.Columns[4].Width = 100;
            Subjectspread.Columns[4].Locked = true;
            Subjectspread.Columns[5].Width = 50;
            Subjectspread.Columns[5].Locked = true;
            Subjectspread.Columns[6].Width = 150;
            Subjectspread.Columns[6].Locked = true;
            Subjectspread.Columns[7].Width = 100;

            string subjectno1 = "";
            if (ddlHall.Items.Count > 0)
            {
                string session = Convert.ToString(ddlsession.SelectedItem.Text);

                string[] examDateArr = ddlfrmdate.SelectedItem.Text.Split('-');
                string examDate = examDateArr[1] + "/" + examDateArr[0] + "/" + examDateArr[2];
                string subject_code = Convert.ToString(ddlsubject.SelectedItem.Value);
                string examMonth = ddlMonth.SelectedValue;
                string examYear = ddlYear.SelectedValue;
                //magesh 4.1.18
                string studforsubject = "select  distinct r.reg_no as regno,r.stud_name as stuname,r.roll_no as roll,r.stud_type as typeofstud,r.current_semester as sem,r.degree_code ,r.Batch_Year,s.subject_no,ead.appl_no from exmtt e,exmtt_det ex,Course c,Department dpt,degree d,subject s,exam_seating es,class_master cm,Registration r,exam_appl_details ead,exam_application ea where ea.appl_no = ead.appl_no and ead.subject_no=ex.subject_no and ead.subject_no=es.subject_no and s.subject_no=ead.subject_no and ea.roll_no=r.Roll_No and es.regno=r.Reg_No  and es.subject_no =ex.subject_no  and es.roomno=cm.rno and es.edate  = ex.exam_date and c.Course_Id=d.Course_Id and  d.Degree_Code=e.degree_code and dpt.Dept_Code=d.dept_code and s.subject_no=ex.subject_no and ex.exam_code=e.exam_code  and e.exam_type='Univ' and e.Exam_month='" + examMonth + "' and e.Exam_year='" + examYear + "' and convert(varchar(10),ex.exam_date,105) in( '" + ddlfrmdate.SelectedItem.Text + "' ) and exam_session like '%" + session + "%' and cm.coll_code in(" + collegeCode + ") and roomno = '" + ddlHall.SelectedValue + "' order by r.degree_code,r.batch_year desc,r.reg_no asc ";

                DataSet ds3 = d2.select_method_wo_parameter(studforsubject, "text");

                DataSet dsCourseDegDet = d2.select_method_wo_parameter(" select C.Course_Name,dt.Dept_Name,d.Degree_Code from  Degree d,Department dt,Course c  where  d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and c.college_code in (" + collegeCode + ")  ", "Text");

                string regno = "";
                string studname = "";
                string rollno = "";
                string studtype = "";
                string sem = "";
                int sno = 1;
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                if (ds3.Tables[0].Rows.Count > 0)
                {
                    Subjectspread.Sheets[0].RowCount = Subjectspread.Sheets[0].RowCount + 1;
                    Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 1, 0, 1, 5);
                    Subjectspread.Sheets[0].Cells[0, 7].CellType = objintcell;
                    for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                    {
                        string batchyear = Convert.ToString(ds3.Tables[0].Rows[i]["Batch_Year"]);
                        string degreecode1 = Convert.ToString(ds3.Tables[0].Rows[i]["degree_code"]);
                        sem = Convert.ToString(ds3.Tables[0].Rows[i]["sem"]);

                        string courseName = string.Empty;
                        string deptname = string.Empty;

                        if (dsCourseDegDet.Tables.Count > 0 && dsCourseDegDet.Tables[0].Rows.Count > 0)
                        {
                            dsCourseDegDet.Tables[0].DefaultView.RowFilter = "Degree_Code='" + degreecode1 + "'";
                            DataView dv = dsCourseDegDet.Tables[0].DefaultView;
                            if (dv.Count > 0)
                            {
                                courseName = dv[0]["Course_Name"].ToString();
                                deptname = dv[0]["Dept_Name"].ToString();
                            }
                        }

                        Savebtn.Visible = true;
                        Subjectspread.Sheets[0].RowCount = Subjectspread.Sheets[0].RowCount + 1;
                        regno = ds3.Tables[0].Rows[i]["regno"].ToString();
                        studname = ds3.Tables[0].Rows[i]["stuname"].ToString();
                        rollno = ds3.Tables[0].Rows[i]["roll"].ToString();
                        studtype = ds3.Tables[0].Rows[i]["typeofstud"].ToString();
                        string subjectcode = Convert.ToString(ddlsubject.SelectedItem.Value);
                        subjectno1 = Convert.ToString(ds3.Tables[0].Rows[i]["subject_no"]);

                        //sem = ds3.Tables[0].Rows[i]["sem"].ToString();
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 7].CellType = objcom;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 0].Text = sno + "";
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 1].CellType = txt;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 1].Text = rollno;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 2].CellType = txt;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 2].Text = regno;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 3].Text = studname;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 4].Text = studtype;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 5].Text = courseName;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 6].Text = deptname;

                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 0].Note = session;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds3.Tables[0].Rows[i]["appl_no"]);
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 1].Note = subjectno1;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 2].Note = batchyear;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 3].Note = degreecode1;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 4].Note = sem;
                        Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 5].Note = subjectcode;
                        sno++;
                        //========================

                        int totalrows = Subjectspread.Sheets[0].RowCount;
                        Subjectspread.Sheets[0].PageSize = totalrows * 10;
                        Subjectspread.Height = 400;// (totalrows + 30) * 10;
                        if (Session["Rollflag"] == "0")
                        {
                            Subjectspread.Width = 580;
                            Subjectspread.Sheets[0].Columns[1].Visible = false;
                        }
                        if (Session["Regflag"] == "0")
                        {
                            Subjectspread.Width = 560;
                            Subjectspread.Sheets[0].Columns[2].Visible = false;
                        }

                        //=========
                        string noofperiods = "select No_of_hrs_per_day as tothrs,No_of_hrs_I_half_day as FNhrs,No_of_hrs_II_half_day as ANhrs  from PeriodAttndSchedule where degree_code=" + degreecode1 + " and semester=" + sem + "";
                        DataSet ds11 = d2.select_method_wo_parameter(noofperiods, "text");
                        string totalhrs = "";
                        string fsthalfhrs = "";
                        string scndhalfhrs = "";
                        string leavecode = "";
                        string value = "";
                        string reqdate = "";
                        int reqdatenew = 0;

                        if (ds11.Tables[0].Rows.Count > 0)
                        {
                            totalhrs = ds11.Tables[0].Rows[0]["tothrs"].ToString();
                            fsthalfhrs = ds11.Tables[0].Rows[0]["FNhrs"].ToString();
                            scndhalfhrs = ds11.Tables[0].Rows[0]["ANhrs"].ToString();
                            string examdate = ddlfrmdate.SelectedValue.ToString();
                            string[] splitdate = examdate.Split(new Char[] { '-' });
                            reqdate = splitdate[0].ToString();
                            reqdatenew = Convert.ToInt32(reqdate);
                            string reqmonth = splitdate[1].ToString();


                            //for F.N
                            string attvalue = "";
                            string obtainedattenance = "";
                            if (session == "F.N")
                            {
                                value = ("d" + reqdatenew + "d" + Convert.ToInt32(fsthalfhrs));

                                //string rollno = Subjectspread.Sheets[0].Cells[i1, 5].Note;
                                int my = Convert.ToInt32(ddlMonth.SelectedItem.Value.ToString()) + Convert.ToInt32(ddlYear.SelectedValue.ToString()) * 12;
                                string selectattend = "select " + value + " from attendance where roll_no='" + rollno + "' and month_year=" + my + "";
                                DataSet ds12 = d2.select_method_wo_parameter(selectattend, "text");
                                if (ds12.Tables[0].Rows.Count > 0)
                                {
                                    attvalue = ds12.Tables[0].Rows[0][value].ToString();
                                    obtainedattenance = Attmark(attvalue);
                                    Subjectspread.Sheets[0].SetText(Subjectspread.Sheets[0].RowCount - 1, 7, obtainedattenance);
                                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;

                                }
                                else
                                {
                                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 7].Text = "P";
                                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                }

                            }
                            //for A.F
                            if (session == "A.N")
                            {

                                int secondhalfour = Convert.ToInt32(fsthalfhrs) + 1;
                                value = ("d" + reqdatenew + "d" + secondhalfour);
                                int my = Convert.ToInt32(ddlMonth.SelectedItem.Value.ToString()) + Convert.ToInt32(ddlYear.SelectedValue.ToString()) * 12;
                                string selectattend = "select " + value + " from attendance where roll_no='" + rollno + "' and month_year=" + my + "";
                                DataSet ds13 = d2.select_method_wo_parameter(selectattend, "text");
                                if (ds13.Tables[0].Rows.Count > 0)
                                {
                                    attvalue = ds13.Tables[0].Rows[0][value].ToString();
                                    obtainedattenance = Attmark(attvalue);
                                    Subjectspread.Sheets[0].SetText(Subjectspread.Sheets[0].RowCount - 1, 7, obtainedattenance);
                                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                }
                                else
                                {
                                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 7].Text = "P";
                                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                        }
                        //==

                    }
                }
                Subjectspread.Sheets[0].AutoPostBack = false;
                if (Subjectspread.Rows.Count > 1)
                {
                    for (int cbl = 0; cbl < cblColumnOrder.Items.Count; cbl++)
                    {
                        if (cblColumnOrder.Items[cbl].Selected)
                            Subjectspread.Columns[cbl].Visible = true;
                        else
                            Subjectspread.Columns[cbl].Visible = false;
                    }
                    Subjectspread.Visible = true;
                    Subjectspread.Sheets[0].FrozenColumnCount = 5;
                    Subjectspread.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Subjectspread.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Subjectspread.Sheets[0].SetColumnMerge(6, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Subjectspread.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
                    Subjectspread.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
                    Subjectspread.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
                    Subjectspread.Width = 850;
                    Subjectspread.Sheets[0].PageSize = Subjectspread.Sheets[0].RowCount;
                    Subjectspread.SaveChanges();
                }
                else
                {
                    Subjectspread.Visible = false;
                    lblerror.Visible = true;
                    lblerror.Text = "No Records Found";
                }
            }
            else
            {
                Subjectspread.Visible = false;
                lblerror.Visible = true;
                lblerror.Text = "Please Select Hall";
            }
        }
        catch
        {
            Subjectspread.Visible = false;
            lblerror.Visible = true;
            lblerror.Text = "No Records Found";
        }
    }
    public string Attmark(string Attstr_mark)
    {

        string Att_mark;
        Att_mark = "";
        if (Attstr_mark == "1")
        {
            Att_mark = "P";
        }

        else if (Attstr_mark == "2")
        {
            Att_mark = "A";

        }
        else if (Attstr_mark == "3")
        {
            Att_mark = "OD";

        }
        else if (Attstr_mark == "4")
        {
            Att_mark = "ML";

        }
        else if (Attstr_mark == "5")
        {
            Att_mark = "SOD";

        }
        else if (Attstr_mark == "6")
        {
            Att_mark = "NSS";

        }
        else if (Attstr_mark == "7")
        {
            Att_mark = "H";

        }
        if (Attstr_mark == "8")
        {
            Att_mark = "NJ";

        }
        else if (Attstr_mark == "9")
        {
            Att_mark = "S";

        }
        else if (Attstr_mark == "10")
        {
            Att_mark = "L";

        }
        else if (Attstr_mark == "11")
        {
            Att_mark = "NCC";

        }
        else if (Attstr_mark == "12")
        {
            Att_mark = "HS";

        }
        else if (Attstr_mark == "13")
        {
            Att_mark = "PP";
        }
        else if (Attstr_mark == "14")
        {
            Att_mark = "SYOD";
        }
        else if (Attstr_mark == "15")
        {
            Att_mark = "COD";
        }
        else if (Attstr_mark == "16")
        {
            Att_mark = "OOD";
        }
        else if (Attstr_mark == "17")
        {
            Att_mark = "LA";
        }
        else
        {
            // Att_mark = "";
        }
        //return Convert.ToInt32(Att_mark);
        return Att_mark;
    }
    public string Attvalues(string Att_str1)
    {
        string Attvalue;

        Attvalue = "";
        if (Att_str1 == "P")
        {
            Attvalue = "1";

        }
        else if (Att_str1 == "A")
        {
            Attvalue = "2";

        }
        else if (Att_str1 == "OD")
        {
            Attvalue = "3";

        }
        else if (Att_str1 == "ML")
        {
            Attvalue = "4";

        }
        else if (Att_str1 == "SOD")
        {
            Attvalue = "5";
        }

        else if (Att_str1 == "NSS")
        {
            Attvalue = "6";

        }
        else if (Att_str1 == "H")
        {
            Attvalue = "7";

        }

        else if (Att_str1 == "NJ")
        {
            Attvalue = "8";

        }
        else if (Att_str1 == "S")
        {
            Attvalue = "9";

        }
        else if (Att_str1 == "L")
        {
            Attvalue = "10";

        }
        else if (Att_str1 == "NCC")
        {
            Attvalue = "11";

        }
        else if (Att_str1 == "HS")
        {
            Attvalue = "12";
        }

        else if (Att_str1 == "PP")
        {
            Attvalue = "13";
        }
        else if (Att_str1 == "SYOD")
        {
            Attvalue = "14";
        }
        else if (Att_str1 == "COD")
        {
            Attvalue = "15";
        }
        else if (Att_str1 == "OOD")
        {
            Attvalue = "16";
        }
        else if (Att_str1 == "LA")
        {
            Attvalue = "17";
        }
        else
        {
            Attvalue = "";
        }
        return Attvalue;

    }
    protected void ddlfrmdate_SelectedIndexChanged(object sender, EventArgs e)
    {
        Savebtn.Visible = false;
        Subjectspread.Visible = false;
        typeChange();
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        typeChange();
    }
    protected void Savebtn_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(collegeCode))
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select College')", true);
            return;
        }
        try
        {
            Subjectspread.SaveChanges();
            string exammonth = ddlMonth.SelectedValue.ToString();
            string sem2 = Subjectspread.Sheets[0].Cells[1, 4].Note;
            string degreecode2 = Subjectspread.Sheets[0].Cells[1, 3].Note;
            string session = Subjectspread.Sheets[0].Cells[1, 0].Note;
            string batch = Subjectspread.Sheets[0].Cells[1, 2].Note;
            string subjectno = Subjectspread.Sheets[0].Cells[1, 1].Note;
            string v = Convert.ToString(Subjectspread.Sheets[0].Cells[1, 7].Value);
            string noofperiods = "select No_of_hrs_per_day as tothrs,No_of_hrs_I_half_day as FNhrs,No_of_hrs_II_half_day as ANhrs  from PeriodAttndSchedule where degree_code=" + degreecode2 + " and semester=" + sem2 + "";
            DataSet ds4 = d2.select_method_wo_parameter(noofperiods, "text");
            string totalhrs = "";
            string fsthalfhrs = "";
            string scndhalfhrs = "";
            if (ds4.Tables[0].Rows.Count > 0)
            {
                totalhrs = ds4.Tables[0].Rows[0]["tothrs"].ToString();
                fsthalfhrs = ds4.Tables[0].Rows[0]["FNhrs"].ToString();
                scndhalfhrs = ds4.Tables[0].Rows[0]["ANhrs"].ToString();

                string examdate = ddlfrmdate.SelectedValue.ToString();
                string[] splitdate = examdate.Split(new Char[] { '-' });
                string reqdate = splitdate[0].ToString();
                int reqdatenew = Convert.ToInt32(reqdate);
                string reqmonth = splitdate[1].ToString();
                string leavecode = "";
                string value = "";

                string exmcode = d2.GetFunction("select exam_code from Exam_Details where Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and Exam_year='" + ddlYear.SelectedValue.ToString() + "' and batch_year='" + batch + "' and degree_code=" + degreecode2 + "");

                for (int i1 = 1; i1 <= Subjectspread.Sheets[0].RowCount - 1; i1++)
                {
                    byte examAttValue = 0;
                    string Att_mark = "";
                    string Att_value = "";
                    Att_mark = Convert.ToString(Subjectspread.Sheets[0].GetText(i1, 7));

                    if (Att_mark == null)
                    {
                        Att_mark = Subjectspread.GetEditValue(i1, 7).ToString();
                    }
                    Att_value = Attvalues(Att_mark);
                    if (Att_value == "")
                    {
                        Att_value = "''";
                    }
                    //string rollno = Subjectspread.Sheets[0].Cells[i1, 5].Note;//Rajkumar 5/1/2018
                    string rollno = Subjectspread.Sheets[0].Cells[i1, 1].Text;

                    string strsetval = d2.GetFunction("select value from COE_Master_Settings where settings='Attendance Link mark'");
                    if (strsetval == "1")
                    {
                        if (Att_value == "2")
                        {
                            string strquerymark = "if not exists(select * from mark_entry where roll_no='" + rollno + "'  and exam_code='" + exmcode + "' and subject_no='" + subjectno + "')";
                            strquerymark = strquerymark + " insert into mark_entry (exam_code,subject_no,roll_no,external_mark,evaluation1,evaluation2,passorfail,result) values ('" + exmcode + "','" + subjectno + "','" + rollno + "','-1','-1','-1','0','Fail')";
                            strquerymark = strquerymark + " else";
                            strquerymark = strquerymark + " update mark_entry set external_mark='-1',evaluation1='-1',evaluation2='-1',evaluation3='',passorfail='0',result='Fail' where roll_no='" + rollno + "'  and exam_code='" + exmcode + "' and subject_no='" + subjectno + "'";
                            int insm = d2.update_method_wo_parameter(strquerymark, "Text");
                        }
                        else
                        {
                            string strquerymark = "if exists(select * from mark_entry where roll_no='" + rollno + "'  and exam_code='" + exmcode + "' and subject_no='" + subjectno + "' and external_mark='-1')";
                            strquerymark = strquerymark + " update mark_entry set external_mark='',result=null where roll_no='" + rollno + "'  and exam_code='" + exmcode + "' and subject_no='" + subjectno + "'";
                            int insm = d2.update_method_wo_parameter(strquerymark, "Text");
                        }
                    }
                    //Exam attendance entry Attendance
                    if (Att_value == "1")
                    {
                        examAttValue = 1;
                    }
                    string subjectNo = Convert.ToString(Subjectspread.Sheets[0].Cells[i1, 1].Note);
                    string appl_No = Convert.ToString(Subjectspread.Sheets[0].Cells[i1, 0].Tag);
                    string insUpdQ = " update exam_appl_details set ExAttendance=" + examAttValue + " where  subject_no='" + subjectNo + "' and appl_no='" + appl_No + "'";
                    d2.update_method_wo_parameter(insUpdQ, "Text");
                    //Exam Attendance entry for subject
                    //for F.N
                    int insupdattendance = 0;
                    if (session == "F.N")
                    {
                        for (int i = 1; i <= Convert.ToInt32(fsthalfhrs); i++)
                        {
                            value = ("d" + reqdatenew + "d" + i);
                            int my = Convert.ToInt32(ddlMonth.SelectedItem.Value.ToString()) + Convert.ToInt32(ddlYear.SelectedValue.ToString()) * 12;
                            string selectattend = "select * from attendance where roll_no='" + rollno + "' and month_year=" + my + "";
                            DataSet ds5 = d2.select_method_wo_parameter(selectattend, "text");
                            if (ds5.Tables[0].Rows.Count > 0)
                            {
                                string updateattend = "update Attendance set " + value + "=" + Att_value + " where  Roll_no='" + rollno + "' and month_year=" + my + "";
                                insupdattendance = d2.update_method_wo_parameter(updateattend, "text");
                                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Attendance Saved')", true);
                            }
                            else
                            {
                                string insertattend = "insert into attendance (roll_no,month_year," + value + ")values('" + rollno + "'," + my + "," + Att_value + ")";
                                insupdattendance = d2.update_method_wo_parameter(insertattend, "text");
                                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Attendance Saved')", true);
                            }
                        }
                    }
                    //for A.F
                    if (session == "A.N")
                    {
                        for (int i = Convert.ToInt32(fsthalfhrs) + 1; i <= Convert.ToInt32(totalhrs); i++)
                        {

                            value = ("d" + reqdatenew + "d" + i);
                            //string rollno = Subjectspread.Sheets[0].Cells[i1, 5].Note;
                            int my = Convert.ToInt32(ddlMonth.SelectedItem.Value.ToString()) + Convert.ToInt32(ddlYear.SelectedValue.ToString()) * 12;
                            string selectattend = "select * from attendance where roll_no='" + rollno + "' and month_year=" + my + "";
                            DataSet ds5 = d2.select_method_wo_parameter(selectattend, "text");
                            if (ds5.Tables[0].Rows.Count > 0)
                            {
                                string updateattend = "update Attendance set " + value + "=" + Att_value + " where  Roll_no='" + rollno + "' and month_year=" + my + "";
                                insupdattendance = d2.update_method_wo_parameter(updateattend, "text");
                                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Attendance Saved')", true);
                            }
                            else
                            {
                                string insertattend = "insert into attendance (roll_no,month_year," + value + ")values('" + rollno + "'," + my + "," + Att_value + ")";
                                insupdattendance = d2.update_method_wo_parameter(insertattend, "text");
                                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Attendance Saved')", true);
                            }
                        }
                    }
                }
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Update Attendance Parameters!!!";
            }
        }
        catch (Exception ex)
        {
            string collegeCode1 = Convert.ToString(Session["collegecode"]);
            d2.sendErrorMail(ex, collegeCode1, "Exam Attendance");
        }
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
            ds.Clear();
            ds = d2.Examyear();
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
            ds.Clear();
            string year = ddlYear.SelectedItem.Text;
            ds = d2.Exammonth(year);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlMonth.DataSource = ds;
                ddlMonth.DataValueField = "Exam_month";
                ddlMonth.DataTextField = "monthName";
                ddlMonth.DataBind();
            }
        }
        catch { }
    }
    protected void ddlsession_SelectedIndexChanged(object sender, EventArgs e)
    {
        typeChange();
    }
    public void bindsubject()
    {
        try
        {
            ddlsubject.Items.Clear();
            string spreadbind = "select  distinct s.subject_code, (s.subject_code+' - '+s.subject_Name) as SubjectName from exmtt e,exmtt_det ex,Course c,Department dpt,degree d,subject s where c.Course_Id=d.Course_Id and  d.Degree_Code=e.degree_code and dpt.Dept_Code=d.dept_code and s.subject_no=ex.subject_no and ex.exam_code=e.exam_code  and e.exam_type='Univ' and convert(varchar(10),ex.exam_date,105) in ( '" + ddlfrmdate.SelectedValue.ToString() + "' ) and exam_session like '%" + ddlsession.SelectedItem.Text + "%' and e.Exam_Month=" + ddlMonth.SelectedItem.Value + " and e.Exam_Year=" + ddlYear.SelectedItem.Text + " and c.college_code in (" + collegeCode + ") ";

            spreadbind = spreadbind + "  union all select distinct s.subject_code,(s.subject_code+' - '+s.subject_Name) as SubjectName from examtheorybatch eth,subject s where s.subject_no=eth.SubNo and  examsession like '%" + ddlsession.SelectedItem.Text + "%' and convert(varchar(10),eth.ExamDate,105) in ( '" + ddlfrmdate.SelectedValue.ToString() + "' )";
            DataSet ds2 = d2.select_method_wo_parameter(spreadbind, "text");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddlsubject.DataSource = ds2;
                ddlsubject.DataTextField = "SubjectName";
                ddlsubject.DataValueField = "subject_code";
                ddlsubject.DataBind();
            }
            bindsubpart();
        }
        catch { }

    }
    protected void ddlsubject_Change(object sender, EventArgs e)
    {
        Savebtn.Visible = false;
        Subjectspread.Visible = false;
        bindsubpart();
    }
    public void loadHall()
    {
        try
        {
            ddlHall.Items.Clear();
            collegeCode = reUse.getCblSelectedValue(cbl_College);
            string selQ = "select distinct roomno,cm.priority from exam_seating es, exmtt_det ed,exmtt e,class_master cm where es.roomno=cm.rno and es.edate  = ed.exam_date and e.exam_code=ed.exam_code and e.Exam_month='" + ddlMonth.SelectedItem.Value + "' and e.Exam_year='" + ddlYear.SelectedItem.Text + "' and convert(varchar(10),ed.exam_date,105) = '" + ddlfrmdate.SelectedValue + "' and es.ses_sion like '%" + ddlsession.SelectedItem.Text + "%' and cm.coll_code in('" + collegeCode + "') order by priority asc";
            DataTable dtHall = dirAccess.selectDataTable(selQ);
            if (dtHall.Rows.Count > 0)
            {
                ddlHall.DataSource = dtHall;
                ddlHall.DataTextField = "roomno";
                ddlHall.DataValueField = "roomno";
                ddlHall.DataBind();
            }
        }
        catch { }
    }
    protected void ddlHall_Change(object sender, EventArgs e)
    {
        Savebtn.Visible = false;
        Subjectspread.Visible = false;
    }
    public void typeChange()
    {
        if (ddlType.SelectedIndex == 0)
        {
            divSubject.Visible = true;
            divHall.Visible = false;
            part.Visible = true;
            batch.Visible = true;
            bindsubject();
        }
        else
        {
            divSubject.Visible = false;
            divHall.Visible = true;
            part.Visible = false;
            batch.Visible = false;
            loadHall();
        }
    }
    protected void ddlpart_Change(object sender, EventArgs e)
    {
        try
        {
            bindsubbatch();
        }
        catch
        {
        }

    }
    protected void ddlbatch_Change(object sender, EventArgs e)
    {
        try
        {

        }
        catch
        {
        }

    }
    public void bindsubpart()
    {
        try
        {
            ddlpart.Items.Clear();
            string sql = "select distinct SubPart  from COESubSubjectPartMater co,COESubSubjectPartSettings cs where co.id=cs.id and co.ExamYear='" + Convert.ToString(ddlYear.SelectedItem.Text) + "' and co.ExamMonth='" + Convert.ToString(ddlMonth.SelectedItem.Value) + "' and SubCode='" + Convert.ToString(ddlsubject.SelectedItem.Value) + "'";
            DataSet ds2 = d2.select_method_wo_parameter(sql, "text");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddlpart.DataSource = ds2;
                ddlpart.DataTextField = "SubPart";
                ddlpart.DataValueField = "SubPart";
                ddlpart.DataBind();
            }
            ddlpart.Items.Insert(0, "");
            bindsubbatch();
        }
        catch
        {

        }
    }
    public void bindsubbatch()
    {
        try
        {
            ddlbatch.Items.Clear();

            string sql = "   select distinct e.Batch from examtheorybatch e,Exam_Details ed,COESubSubjectPartMater CM,COESubSubjectPartSettings CP where ed.exam_code=e.ExamCode and cm.ExamMonth=ed.Exam_Month and cm.ExamYear=ed.Exam_year and cm.DegreeCode=ed.degree_code  and ed.Exam_year='" + Convert.ToString(ddlYear.SelectedItem.Text) + "' and ed.Exam_Month='" + Convert.ToString(ddlMonth.SelectedItem.Value) + "' and cp.SubCode='" + Convert.ToString(ddlsubject.SelectedItem.Value) + "' and cp.SubPart='" + Convert.ToString(ddlpart.SelectedItem.Text) + "' and e.SubSubjectID=cp.SubSubjectID";
            DataSet ds2 = d2.select_method_wo_parameter(sql, "text");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds2;
                ddlbatch.DataTextField = "Batch";
                ddlbatch.DataValueField = "Batch";
                ddlbatch.DataBind();
            }
            ddlbatch.Items.Insert(0, "");
        }
        catch
        {

        }
    }
    public void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string date = "@" + "Date :" + System.DateTime.Now.ToString("dd/MM/yyy");
            string batch = "";

            string pagename = "ExamAttendance.aspx";
            string degreedetails = "Exam Attendance" + batch + date;
            Printcontrol.loadspreaddetails(Subjectspread, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
        }
    }
    //Code Ended by Idhris -- Last Modified by idhris 22-02-2017
    protected void chkColumnOrderAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkColumnOrderAll.Checked == true)
        {
            foreach (System.Web.UI.WebControls.ListItem liOrder in cblColumnOrder.Items)
            {
                liOrder.Selected = true;
            }
        }
        else
        {
            foreach (System.Web.UI.WebControls.ListItem liOrder in cblColumnOrder.Items)
            {
                liOrder.Selected = false;
            }
        }
    }
    protected void lbtnRemoveAll_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch
        {
        }
    }
    protected void cblColumnOrder_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
}