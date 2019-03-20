using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;

public partial class ExamTimeTable : System.Web.UI.Page
{
    int existsubject = 0;
    DataSet ds = new DataSet();
    DAccess2 dt = new DAccess2();
    SqlConnection con;
    int days;
    int days1;
    string CollegeCode;
    int j = 0;
    string vl = "";
    static int val = 0;
    Boolean flag_true = false;
    Boolean Cellclick = false;
    int n = 0;
    static int arow = 0;
    static int acol = 0;
    //string sem = "";
    FarPoint.Web.Spread.CheckBoxCellType chk1 = new FarPoint.Web.Spread.CheckBoxCellType();

    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }

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
            chk1.AutoPostBack = true;
            lblmsg.Visible = false;
            errmsg.Visible = false;
            lblbatcherror.Visible = false;
            if (!IsPostBack)
            {
                pnlvisible.Visible = true;
                LoadSettings();
                pnlHolidays.Visible = false;
                lblerror.Visible = false;

                ddltheoryStartTimeamHrs.Text = "9";
                ddltheoryStartTimeamMin.Text = "30";
                ddltheoryendtimeamHrs.Text = "12";
                ddltheoryendtimeamMin.Text = "30";
                ddltheoryendtimeampm.Text = "1";
                ddlTheoryStartTimeHrsPm.Text = "1";
                ddlTheoryStartTimeMinPm.Text = "30";
                ddlTheoryEndTimeHrsPm.Text = "4";
                ddlTheoryEndTimeMinPm.Text = "30";
                ddlTheoryendtimePmam.Text = "1";
                txtTheoryDurationpm.Text = "3:0";
                txtTheoryDurationam.Text = "3:0";
            }
        }
        catch (Exception ex)
        {
        }

    }
    protected void btnc_click(object sender, EventArgs e)
    {
        panel4.Visible = false;
    }
    protected void btngenerateclick(object sender, EventArgs e)
    {
        try
        {
            string arrearyear = "";
            int smarrva = 0;
            Hashtable ht = new Hashtable();
            Boolean subflag = false;
            int flagg = 0;
            int count = 0;
            int p = 0;
            string sm = "";
            string session = "";
            string[] fnan = new string[n];

            //int noofstucoun=
            string noofallow = txtnoofstudent.Text.ToString();
            if (noofallow.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "PLEASE SET THE MAXIMIUM STUDENT PER SESSION";
                return;
            }
            if (txtTheoryDurationam.Text.ToString().Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "PLEASE SET THE FORE NOON TIME IN THEORY";
                return;
            }
            if (txtTheoryDurationpm.Text.ToString().Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "PLEASE SET THE AFTER NOON TIME IN THEORY";
                return;
            }


            string startdate = ddltheoryStartTimeamHrs.SelectedItem.ToString() + ":" + ddltheoryStartTimeamMin.SelectedItem.ToString() + "AM";
            string enddate = ddltheoryendtimeamHrs.SelectedItem.ToString() + ":" + ddltheoryendtimeamMin.SelectedItem.ToString() + "" + ddltheoryendtimeampm.SelectedItem.ToString();

            string strval = "select * from tbl_exam_time_table_batch";
            DataSet dsbatchyse = dt.select_method_wo_parameter(strval, "Text");



            //string deletexam = "delete from exmtt_det where exam_code in(select exam_code from exmtt where Exam_month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "')";
            //need to update raj 3/10/2017
            string deletexam = "delete et from exmtt_det et,tbl_exam_time_table_batch t,degree d,course c,exmtt e where e.exam_code=et.exam_code and e.degree_code=d.degree_code and c.course_id=d.course_id and c.edu_level=t.edu_level and e.batchfrom=t.batch_year and e.Exam_month='" + ddlMonth.SelectedValue + "' and e.Exam_year='" + ddlYear.SelectedItem.Text + "'";
            int delexam = dt.update_method_wo_parameter(deletexam, "text");

            string deletholiday = "delete from examholiday where Exammonth='" + ddlMonth.SelectedValue.ToString() + "' and Examyear='" + ddlYear.SelectedValue.ToString() + "'";
            delexam = dt.update_method_wo_parameter(deletholiday, "text");
            //
            //string strqualsubject = "select distinct s.subject_name,s.subject_code,ss.subject_type,s.subType_no,s.subject_no,ed.batch_year,ed.degree_code,ed.current_semester,ed.Exam_Month,ed.Exam_year,c.type,c.Edu_Level,t.Equal_Subject_Code,t.Com_Subject_Code from Exam_Details ed,exam_application ea,exam_appl_details ead,sub_sem ss,Degree d,course c,subject s left join tbl_equal_paper_Matching t on t.Equal_Subject_Code=s.subject_code where ed.exam_code=ea.exam_code and ead.appl_no=ea.appl_no and ead.subject_no=s.subject_no and ss.subType_no=s.subType_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and t.Exam_Year=ed.Exam_year and t.Exam_month=ed.Exam_month and ed.Exam_Month='" + ddlMonth.SelectedValue + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text + "' order by s.subject_name,s.subject_code";
            string strqualsubject = "select distinct s.subject_name,s.subject_code,ss.subject_type,s.subType_no,s.subject_no,ed.batch_year,ed.degree_code,ed.current_semester,ed.Exam_Month,ed.Exam_year,c.type,c.Edu_Level,t.Equal_Subject_Code,t.Com_Subject_Code from Exam_Details ed,exam_application ea,exam_appl_details ead,tbl_exam_time_table_batch tb,sub_sem ss,Degree d,course c,subject s left join tbl_equal_paper_Matching t on t.Equal_Subject_Code=s.subject_code where ed.exam_code=ea.exam_code and ead.appl_no=ea.appl_no and tb.batch_year=ed.batch_year and tb.edu_level=c.edu_level and ead.subject_no=s.subject_no and isnull(s.sub_lab,'0')=0 and ss.subType_no=s.subType_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and ed.Exam_Month='" + ddlMonth.SelectedValue + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text + "' order by s.subject_name,s.subject_code";
            DataSet dsequalsubject = dt.select_method_wo_parameter(strqualsubject, "Text");

            // string strallarrearpaper = "select distinct s.subject_no,ed.batch_year,ed.degree_code,ed.current_semester,s.subject_name,s.subType_no,ss.subject_type,s.subject_code,ss.ElectivePap,ead.attempts from Exam_Details ed,exam_application ea,exam_appl_details ead,sub_sem ss,subject s where  ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subType_no=ss.subType_no and s.subject_no=ead.subject_no and ss.promote_count=1  and ead.attempts>0 and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text + "' order by s.subject_code";
            string strallarrearpaper = "select distinct s.subject_no,ed.batch_year,ed.degree_code,ed.current_semester,s.subject_name,s.subType_no,ss.subject_type,s.subject_code,ss.ElectivePap,ead.attempts from Exam_Details ed,exam_application ea,exam_appl_details ead,sub_sem ss,subject s,degree d,course c,tbl_exam_time_table_batch t where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and d.degree_code=ed.degree_code and c.course_id=d.course_id and c.edu_level=t.edu_level and ed.batch_year=t.batch_year and s.subType_no=ss.subType_no and s.subject_no=ead.subject_no and ss.promote_count=1 and isnull(s.sub_lab,'0')=0  and ead.attempts>0 and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text + "' order by s.subject_code";
            DataSet dsallarrear = dt.select_method_wo_parameter(strallarrearpaper, "text");

            //string strarrestulit = "select ea.roll_no,s.subject_code from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Degree d,course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and d.Degree_Code=ed.degree_code and c.Course_Id=d.Course_Id and ead.attempts>0 and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text + "' order by s.subcourse_code";
            string strarrestulit = "select ea.roll_no,s.subject_code from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Degree d,course c,tbl_exam_time_table_batch t  where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and d.Degree_Code=ed.degree_code and c.Course_Id=d.Course_Id and c.edu_level=t.edu_level and ed.batch_year=t.batch_year and ead.attempts>0 and isnull(s.sub_lab,'0')=0 and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text + "' order by s.subcourse_code";
            DataSet dsarrstulist = dt.select_method_wo_parameter(strarrestulit, "Text");

            string sl = "select * from examtimetablesetting where mode='" + ddlexammode.SelectedItem.ToString() + "'";
            ds = dt.select_method_wo_parameter(sl, "text");

            string sl11 = " select ElectivePap,subType_no from sub_sem";
            DataSet ds4 = dt.select_method_wo_parameter(sl11, "text");

            string strquerymajorelective = "select distinct subject_code,sy.degree_code,sy.semester from subject s,sub_sem ss,syllabus_master sy where sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and s.Elective=1 and isnull(s.sub_lab,'0')=0";
            DataSet dsmajorelective = dt.select_method_wo_parameter(strquerymajorelective, "text");

            Hashtable hatmaele = new Hashtable();
            for (int mae = 0; mae < dsmajorelective.Tables[0].Rows.Count; mae++)
            {
                string strsubelem = dsmajorelective.Tables[0].Rows[mae]["subject_code"].ToString().Trim().ToLower();
                if (!hatmaele.Contains(strsubelem))
                {
                    hatmaele.Add(strsubelem, strsubelem);
                }
            }
            //string strequlsubquery = "select * from tbl_equal_paper_Matching where Exam_month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "'";
            string strequlsubquery = "select * from tbl_equal_paper_Matching ";
            DataSet dsequalgetsubject = dt.select_method_wo_parameter(strequlsubquery, "text");
            //string strquery = "select * from course";
            //DataSet dscourse = dt.select_method_wo_parameter(strquery, "Text");

            Hashtable hatelesubtype = new Hashtable();
            // Hashtable hattypeyear = new Hashtable();

            //  Dictionary<string, string> diccurrdegree = new Dictionary<string, string>();

            for (int i = 0; i < dsequalgetsubject.Tables[0].Rows.Count; i++)
            {
                if (!hatelesubtype.Contains(dsequalgetsubject.Tables[0].Rows[i]["Equal_Subject_Code"].ToString().Trim().ToLower()))
                {
                    hatelesubtype.Add(dsequalgetsubject.Tables[0].Rows[i]["Equal_Subject_Code"].ToString().Trim().ToLower(), dsequalgetsubject.Tables[0].Rows[i]["Equal_Subject_Code"].ToString());
                }
            }

            string[] getfdate = txtexamstartdate.Text.ToString().Split('-');
            DateTime dtfrom = Convert.ToDateTime(getfdate[1] + '/' + getfdate[0] + '/' + getfdate[2]);
            string[] gettdate = txtExamFinishDate.Text.ToString().Split('-');
            DateTime dtto = Convert.ToDateTime(gettdate[1] + '/' + gettdate[0] + '/' + gettdate[2]);


            Hashtable hatdegreesuborder = new Hashtable();
            Hashtable hatdateyer = new Hashtable();
            //===========================Date Loop========================================
            for (DateTime dtt = dtfrom; dtt <= dtto; dtt = dtt.AddDays(1))
            {
                Hashtable hattypeyear = new Hashtable();
                int flag = 0;
                if (cblHolidays.Items[p].Selected == true)//Holidates Check
                {
                    string holy = cblHolidays.Items[p].Text;
                    string[] holl = holy.Split('-');
                    string holid = holl[1].ToString() + "-" + holl[0].ToString() + "-" + holl[2].ToString();
                    string holi = "if exists(select * from examholiday where exammonth='" + ddlMonth.SelectedValue + "' and examyear='" + ddlYear.SelectedItem.Text + "' and holiday_date='" + holid + "' and startdate='" + dtfrom.ToString("MM-dd-yyyy") + "' and enddate='" + dtto.ToString("MM-dd-yyyy") + "' and college_code='" + Session["collegecode"] + "')delete from examholiday where exammonth='" + ddlMonth.SelectedValue + "' and examyear='" + ddlYear.SelectedItem.Text + "' and holiday_date='" + holid + "' and startdate='" + dtfrom.ToString("MM-dd-yyyy") + "' and enddate='" + dtto.ToString("MM-dd-yyyy") + "' and college_code='" + Session["collegecode"] + "' insert into examholiday(exammonth,examyear,startdate,enddate,holiday_date,college_code) values('" + ddlMonth.SelectedValue + "','" + ddlYear.SelectedItem.Text + "','" + dtfrom.ToString("MM-dd-yyyy") + "','" + dtto.ToString("MM-dd-yyyy") + "','" + holid + "','" + Session["collegecode"] + "')";
                    int h = dt.update_method_wo_parameter(holi, "text");
                    p++;
                }
                else
                {
                    p++;
                    count++;
                    string day = "DAY" + " " + count;
                    DataView dv1 = new DataView();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Tables[0].DefaultView.RowFilter = "day='" + day + "'";//Exam day Filter
                        dv1 = ds.Tables[0].DefaultView;
                        if (dv1.Count > 0)
                        {
                            for (int l = 0; l < 2; l++)
                            {
                                for (int cty = 0; cty < dv1.Count; cty++)//Type Spilt Loop
                                {
                                    string strtype = dv1[cty]["type"].ToString();
                                    string strmodoeva = "";
                                    if (strtype != "" && strtype != null)
                                    {
                                        strmodoeva = "and mode='" + strtype + "' ";
                                    }

                                    Hashtable hatallarrear = new Hashtable();

                                    if (l == 0)
                                    {
                                        flag = 1;
                                        session = "F.N";
                                        fnan = dv1[cty]["FN"].ToString().Split(',');
                                        startdate = ddltheoryStartTimeamHrs.SelectedItem.ToString() + ":" + ddltheoryStartTimeamMin.SelectedItem.ToString() + "AM";
                                        enddate = ddltheoryendtimeamHrs.SelectedItem.ToString() + ":" + ddltheoryendtimeamMin.SelectedItem.ToString() + "" + ddltheoryendtimeampm.SelectedItem.ToString();

                                    }
                                    else if (l == 1)
                                    {
                                        flag = 1;
                                        session = "A.N";
                                        fnan = dv1[cty]["AN"].ToString().Split(',');

                                        startdate = ddlTheoryStartTimeHrsPm.SelectedItem.ToString() + ":" + ddlTheoryStartTimeMinPm.SelectedItem.ToString() + "PM";
                                        enddate = ddlTheoryEndTimeHrsPm.SelectedItem.ToString() + ":" + ddlTheoryEndTimeMinPm.SelectedItem.ToString() + "PM";

                                    }
                                    Boolean allareare = true;//Only Arrear Chciking
                                    for (int ac = 0; ac <= fnan.GetUpperBound(0); ac++)
                                    {
                                        if (fnan[ac] != null && fnan[ac].Trim() != "")
                                        {
                                            string[] subf = fnan[ac].Split('/');
                                            if (subf.Length != 3)
                                            {
                                                allareare = false;
                                            }
                                        }
                                    }
                                    if (flag == 1)
                                    {
                                        Hashtable hatdaysession = new Hashtable();
                                        for (int g = 0; g <= fnan.GetUpperBound(0); g++)//SPILTING UG/PG
                                        {
                                            if (fnan[g].Trim() != "")
                                            {
                                                string[] sub = fnan[g].Split('/');
                                                string subject = "select * from exampriority where year='" + sub[0].ToString() + "' and education='" + sub[1].ToString() + "' " + strmodoeva + " order by priority ";
                                                DataSet ds1 = new DataSet();
                                                ds1 = dt.select_method_wo_parameter(subject, "text");

                                                string typeyear = dv1[cty]["type"].ToString().Trim().ToLower() + "-" + sub[1].ToString().Trim().ToLower() + "-" + sub[0].ToString().Trim().ToLower();
                                                string datesee = dtt.ToString("MM/dd/yyyy") + "-" + session;
                                                if (hatdateyer.Contains(datesee))
                                                {
                                                    string getva = hatdateyer[datesee].ToString();
                                                    getva = typeyear + "&" + getva;
                                                    hatdateyer[datesee] = getva;
                                                }
                                                else
                                                {
                                                    hatdateyer.Add(datesee, typeyear);
                                                }

                                                if (ds1.Tables[0].Rows.Count > 0)
                                                {
                                                    if (sub[0].ToString().Trim().ToLower() == "1 year")
                                                    {
                                                        if (dv1[cty]["mode"].ToString().Trim().ToUpper() == "ODD")
                                                        {
                                                            sm = Convert.ToString("1");
                                                        }
                                                        else
                                                        {
                                                            sm = Convert.ToString("2");
                                                        }
                                                    }
                                                    else if (sub[0].ToString().Trim().ToLower() == "2 year")
                                                    {
                                                        if (dv1[cty]["mode"].ToString().Trim().ToUpper() == "ODD")
                                                        {
                                                            sm = Convert.ToString("3");
                                                        }
                                                        else
                                                        {
                                                            sm = Convert.ToString("4");
                                                        }
                                                    }
                                                    else if (sub[0].ToString().Trim().ToLower() == "3 year")
                                                    {
                                                        if (dv1[cty]["mode"].ToString().Trim().ToUpper() == "ODD")
                                                        {
                                                            sm = Convert.ToString("5");
                                                        }
                                                        else
                                                        {
                                                            sm = Convert.ToString("6");
                                                        }
                                                    }
                                                    else if (sub[0].ToString().Trim().ToLower() == "4 year")
                                                    {
                                                        if (dv1[cty]["mode"].ToString().Trim().ToUpper() == "ODD")
                                                        {
                                                            sm = Convert.ToString("7");
                                                        }
                                                        else
                                                        {
                                                            sm = Convert.ToString("8");
                                                        }
                                                    }
                                                    else if (sub[0].ToString().Trim().ToLower() == "5 year")
                                                    {
                                                        if (dv1[cty]["mode"].ToString().Trim().ToUpper() == "ODD")
                                                        {
                                                            sm = Convert.ToString("9");
                                                        }
                                                        else
                                                        {
                                                            sm = Convert.ToString("10");
                                                        }
                                                    }
                                                    Boolean arrearflag = false;
                                                    string subjec = "";
                                                    if (sub.Length != 3)//Current SUbject
                                                    {
                                                        //subjec = "select distinct s.subject_no,ed.batch_year,ed.degree_code,ed.current_semester,s.subject_name,s.subType_no,ss.subject_type,s.subject_code,ep.priority,ss.ElectivePap,ead.attempts from Exam_Details ed,exam_application ea,exam_appl_details ead,course c,Degree d,sub_sem ss,subject s,exampriority ep,syllabus_master sy where  ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and s.subType_no=ss.subType_no and s.subject_no=ead.subject_no and ss.promote_count=1 and sy.syll_code=ss.syll_code and ss.subject_type=ep.subject_type and ep.education=c.Edu_Level and ep.mode = c.type and sy.semester='" + sm.ToString() + "' and c.Edu_Level='" + sub[1].ToString() + "' and c.type='" + strtype + "' and ed.Exam_Month='" + ddlMonth.SelectedValue + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text + "' and ep.year='" + sub[0].ToString() + "' and sy.semester='" + sm + "' order by ep.priority,ed.degree_code,ed.batch_year desc,ed.current_semester,s.subject_code";
                                                        if (strtype != "" && strtype != null)
                                                        {
                                                            subjec = "select distinct s.subject_no,ed.batch_year,ed.degree_code,ed.current_semester,s.subject_name,s.subType_no,ss.subject_type,s.subject_code,ep.priority,ss.ElectivePap,ead.attempts,REVERSE(s.subject_code) as subjectorder from Exam_Details ed,exam_application ea,exam_appl_details ead,course c,Degree d,sub_sem ss,subject s,exampriority ep,syllabus_master sy,tbl_exam_time_table_batch tb where  ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and tb.batch_year=ed.batch_year and tb.edu_level=c.edu_level and s.subType_no=ss.subType_no and s.subject_no=ead.subject_no and ss.promote_count=1 and sy.syll_code=ss.syll_code and ss.subject_type=ep.subject_type and ep.education=c.Edu_Level and ep.mode = c.type and ead.attempts=0 and isnull(s.sub_lab,'0')=0 and sy.semester='" + sm.ToString() + "' and c.Edu_Level='" + sub[1].ToString() + "' and c.type='" + strtype + "' and ed.Exam_Month='" + ddlMonth.SelectedValue + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text + "' and ep.year='" + sub[0].ToString() + "' and s.subject_no not in (select et.subject_no from exmtt e,exmtt_det et where e.exam_code=et.exam_code and e.Exam_Month='" + ddlMonth.SelectedValue + "' and e.Exam_year='" + ddlYear.SelectedItem.Text + "' ) order by ep.priority,subjectorder,ed.degree_code,ed.batch_year desc,ed.current_semester";
                                                        }
                                                        else
                                                        {
                                                            subjec = "select distinct s.subject_no,ed.batch_year,ed.degree_code,ed.current_semester,s.subject_name,s.subType_no,ss.subject_type,s.subject_code,ep.priority,ss.ElectivePap,ead.attempts,REVERSE(s.subject_code) as subjectorder from Exam_Details ed,exam_application ea,exam_appl_details ead,course c,Degree d,sub_sem ss,subject s,exampriority ep,syllabus_master sytbl_exam_time_table_batch tb where  ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and tb.batch_year=ed.batch_year and tb.edu_level=c.edu_level and s.subType_no=ss.subType_no and s.subject_no=ead.subject_no and ss.promote_count=1 and isnull(s.sub_lab,'0')=0 and sy.syll_code=ss.syll_code and ss.subject_type=ep.subject_type and ep.education=c.Edu_Level and ead.attempts=0 and sy.semester='" + sm.ToString() + "' and c.Edu_Level='" + sub[1].ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text + "' and ep.year='" + sub[0].ToString() + "' and s.subject_no not in (select et.subject_no from exmtt e,exmtt_det et where e.exam_code=et.exam_code and e.Exam_Month='" + ddlMonth.SelectedValue + "' and e.Exam_year='" + ddlYear.SelectedItem.Text + "' ) order by ep.priority,subjectorder,ed.degree_code,ed.batch_year desc,ed.current_semester";
                                                        }
                                                        arrearyear = " and sy.semester ='" + sm.ToString() + "'";
                                                    }
                                                    else if (sub.Length == 3)//Arrear Subject
                                                    {
                                                        smarrva = Convert.ToInt32(sm);
                                                        if (dv1[0]["mode"].ToString() == "ODD")
                                                        {
                                                            smarrva = smarrva + 1;
                                                        }
                                                        else
                                                        {
                                                            smarrva = smarrva - 1;
                                                        }
                                                        arrearyear = " and sy.semester in('" + sm.ToString() + "')";
                                                        if (allareare == true)
                                                        {
                                                            arrearyear = " and sy.semester in('" + smarrva + "')";
                                                        }
                                                        if (strtype != "" && strtype != null)
                                                        {
                                                            // subjec = "select distinct s.subject_no,ed.batch_year,ed.degree_code,ed.current_semester,s.subject_name,s.subType_no,ss.subject_type,s.subject_code,ep.priority,ss.ElectivePap,REVERSE(s.subject_code)  as subjectorder from Exam_Details ed,exam_application ea,exam_appl_details ead,course c,Degree d,sub_sem ss,subject s,exampriority ep,syllabus_master sy where  ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and s.subType_no=ss.subType_no and s.subject_no=ead.subject_no and ss.promote_count=1 and ead.attempts>0 and sy.syll_code=ss.syll_code and ss.subject_type=ep.subject_type and ep.education=c.Edu_Level and ep.mode = c.type and c.Edu_Level='" + sub[1].ToString() + "' and c.type='" + strtype + "' and ed.Exam_Month='" + ddlMonth.SelectedValue + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text + "' and ep.year='" + sub[0].ToString() + "' " + arrearyear + " and s.subject_no not in (select et.subject_no from exmtt e,exmtt_det et where e.exam_code=et.exam_code and e.Exam_Month='" + ddlMonth.SelectedValue + "' and e.Exam_year='" + ddlYear.SelectedItem.Text + "' ) order by ep.priority,ed.degree_code,ed.batch_year desc,ed.current_semester,subjectorder";                                                       
                                                            subjec = "select distinct s.subject_no,ed.batch_year,ed.degree_code,ed.current_semester,s.subject_name,s.subType_no,ss.subject_type,s.subject_code,ep.priority,ss.ElectivePap,REVERSE(s.subject_code)  as subjectorder from Exam_Details ed,exam_application ea,exam_appl_details ead,course c,Degree d,sub_sem ss,subject s,exampriority ep,syllabus_master sy,tbl_exam_time_table_batch tb where  ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=d.Degree_Code and tb.batch_year=ed.batch_year and tb.edu_level=c.edu_level and d.Course_Id=c.Course_Id and s.subType_no=ss.subType_no and s.subject_no=ead.subject_no and ss.promote_count=1 and isnull(s.sub_lab,'0')=0 and ead.attempts>0 and sy.syll_code=ss.syll_code and ss.subject_type=ep.subject_type and ep.education=c.Edu_Level and ep.mode = c.type and c.Edu_Level='" + sub[1].ToString() + "' and c.type='" + strtype + "' and ed.Exam_Month='" + ddlMonth.SelectedValue + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text + "' and ep.year='" + sub[0].ToString() + "' " + arrearyear + " order by ep.priority,ed.degree_code,ed.batch_year desc,ed.current_semester,subjectorder";
                                                        }
                                                        else
                                                        {
                                                            //subjec = "select distinct s.subject_no,ed.batch_year,ed.degree_code,ed.current_semester,s.subject_name,s.subType_no,ss.subject_type,s.subject_code,ep.priority,ss.ElectivePap,REVERSE(s.subject_code)  as subjectorder from Exam_Details ed,exam_application ea,exam_appl_details ead,course c,Degree d,sub_sem ss,subject s,exampriority ep,syllabus_master sy where  ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and s.subType_no=ss.subType_no and s.subject_no=ead.subject_no and ss.promote_count=1 and ead.attempts>0 and sy.syll_code=ss.syll_code and ss.subject_type=ep.subject_type and ep.education=c.Edu_Level and c.Edu_Level='" + sub[1].ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text + "' and ep.year='" + sub[0].ToString() + "' " + arrearyear + " and s.subject_no not in (select et.subject_no from exmtt e,exmtt_det et where e.exam_code=et.exam_code and e.Exam_Month='" + ddlMonth.SelectedValue + "' and e.Exam_year='" + ddlYear.SelectedItem.Text + "' ) order by ep.priority,ed.degree_code,ed.batch_year desc,ed.current_semester,subjectorder";
                                                            subjec = "select distinct s.subject_no,ed.batch_year,ed.degree_code,ed.current_semester,s.subject_name,s.subType_no,ss.subject_type,s.subject_code,ep.priority,ss.ElectivePap,REVERSE(s.subject_code)  as subjectorder from Exam_Details ed,exam_application ea,exam_appl_details ead,course c,Degree d,sub_sem ss,subject s,exampriority ep,syllabus_master sy,tbl_exam_time_table_batch tb where  ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=d.Degree_Code and tb.batch_year=ed.batch_year and tb.edu_level=c.edu_level and d.Course_Id=c.Course_Id and s.subType_no=ss.subType_no and s.subject_no=ead.subject_no and ss.promote_count=1 and isnull(s.sub_lab,'0')=0 and ead.attempts>0 and sy.syll_code=ss.syll_code and ss.subject_type=ep.subject_type and ep.education=c.Edu_Level and c.Edu_Level='" + sub[1].ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text + "' and ep.year='" + sub[0].ToString() + "' " + arrearyear + " order by ep.priority,ed.degree_code,ed.batch_year desc,ed.current_semester,subjectorder";
                                                        }
                                                        arrearflag = true;
                                                    }
                                                    if (!hattypeyear.Contains(strtype.Trim().ToLower() + '-' + sub[0].ToString().ToLower().Trim() + '-' + sub[0].ToString().ToLower().Trim()))
                                                    {
                                                        DataSet ds3 = dt.select_method_wo_parameter(subjec, "text");
                                                        subflag = true;
                                                        DataView dv = new DataView();
                                                        string ha = "";
                                                        if (ds3.Tables[0].Rows.Count > 0)
                                                        {
                                                            //******************ARRER PAPER ***************************************************
                                                            if (arrearflag == true)
                                                            {
                                                                if (allareare == true)
                                                                {
                                                                    Hashtable hatequalgetsubjectcode = new Hashtable();
                                                                    Hashtable hatdegree = new Hashtable();
                                                                    for (int u = 0; u < ds1.Tables[0].Rows.Count; u++)
                                                                    {
                                                                        ds3.Tables[0].DefaultView.RowFilter = "subject_type='" + ds1.Tables[0].Rows[u]["subject_type"].ToString() + "' ";
                                                                        dv = ds3.Tables[0].DefaultView;
                                                                        if (dv.Count > 0)
                                                                        {
                                                                            for (int a = 0; a < dv.Count; a++)
                                                                            {
                                                                                string degedetails = dv[a]["degree_code"].ToString();
                                                                                string eleval = dv[a]["ElectivePap"].ToString();
                                                                                string actualsubjecode = dv[a]["subject_code"].ToString();
                                                                                string getsubjcode = " subject_code='" + dv[a]["subject_code"].ToString() + "'";
                                                                                Boolean rollexistflag = false;
                                                                                Boolean alreaducheck = false;
                                                                                Boolean checktype = false;
                                                                                if (!hatequalgetsubjectcode.Contains(actualsubjecode))
                                                                                {
                                                                                    if (hatdegree.Contains(degedetails))
                                                                                    {
                                                                                        if (eleval.Trim().ToLower() == "true" || eleval.Trim() == "1")
                                                                                        {
                                                                                            string getsubtype = hatdegree[degedetails].ToString();
                                                                                            if (getsubtype.Trim().ToLower() != ds1.Tables[0].Rows[u]["subject_type"].ToString().Trim().ToLower())
                                                                                            {
                                                                                                checktype = true;
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            checktype = true;
                                                                                        }

                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (hatdegree.Contains(degedetails))
                                                                                    {
                                                                                        if (eleval.Trim().ToLower() == "true" || eleval.Trim() == "1")
                                                                                        {
                                                                                            string getsubtype = hatdegree[degedetails].ToString();
                                                                                            if (getsubtype.Trim().ToLower() != ds1.Tables[0].Rows[u]["subject_type"].ToString().Trim().ToLower())
                                                                                            {
                                                                                                checktype = true;
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            checktype = true;
                                                                                        }
                                                                                    }
                                                                                    if (hatequalgetsubjectcode.Contains(actualsubjecode))
                                                                                    {
                                                                                        string getval = hatequalgetsubjectcode[actualsubjecode].ToString();
                                                                                        if (getval.Trim() != "0")
                                                                                        {
                                                                                            rollexistflag = true;
                                                                                            checktype = true;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            checktype = false;
                                                                                        }
                                                                                        alreaducheck = true;
                                                                                    }
                                                                                }
                                                                                if (checktype == false)
                                                                                {
                                                                                    if (hatequalgetsubjectcode.Contains(actualsubjecode))
                                                                                    {
                                                                                        string getval = hatequalgetsubjectcode[actualsubjecode].ToString();
                                                                                        if (getval.Trim() != "0")
                                                                                        {
                                                                                            rollexistflag = true;
                                                                                        }
                                                                                        alreaducheck = true;
                                                                                    }

                                                                                    if (alreaducheck == false)
                                                                                    {
                                                                                        string getequalsubval = "select Equal_Subject_Code as subjectcode from tbl_equal_paper_Matching where Com_Subject_Code in(select Com_Subject_Code from tbl_equal_paper_Matching where Equal_Subject_Code='" + dv[a]["subject_code"].ToString() + "')";
                                                                                        string checkfincode = " s1.subject_code='" + dv[a]["subject_code"].ToString() + "'";
                                                                                        if (eleval.Trim().ToLower() == "true" || eleval.Trim() == "1")
                                                                                        {
                                                                                            checkfincode = "";
                                                                                            Hashtable hatcheckval = new Hashtable();
                                                                                            for (int cheall = 0; cheall < dv.Count; cheall++)
                                                                                            {
                                                                                                string getecode = dv[cheall]["subject_code"].ToString();
                                                                                                if (!hatcheckval.Contains(getecode))
                                                                                                {
                                                                                                    if (checkfincode == "")
                                                                                                    {
                                                                                                        checkfincode = "'" + getecode + "'";
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        checkfincode = checkfincode + ",'" + getecode + "'";
                                                                                                    }
                                                                                                    hatcheckval.Add(getecode, getecode);
                                                                                                }
                                                                                            }
                                                                                            if (checkfincode.Trim() != "")
                                                                                            {
                                                                                                checkfincode = " s1.subject_code in(" + checkfincode + ")";
                                                                                                getequalsubval = "select distinct s.subject_code as subjectcode from subject s left join  tbl_equal_paper_Matching t on t.Equal_Subject_Code=s.subject_code where subject_code in(select s1.subject_code  from subject s1 where " + checkfincode + ")";
                                                                                            }
                                                                                        }
                                                                                        DataSet dsequalsub = dt.select_method_wo_parameter(getequalsubval, "text"); ;
                                                                                        DataView dvvale = dsequalsub.Tables[0].DefaultView;
                                                                                        if (dvvale.Count > 0)
                                                                                        {
                                                                                            string getequalsub = "";
                                                                                            for (int esc = 0; esc < dvvale.Count; esc++)
                                                                                            {
                                                                                                if (getequalsub == "")
                                                                                                {
                                                                                                    getequalsub = "'" + dvvale[esc]["subjectcode"].ToString() + "'";
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    getequalsub = getequalsub + ",'" + dvvale[esc]["subjectcode"].ToString() + "'";
                                                                                                }
                                                                                            }
                                                                                            getsubjcode = " subject_code in(" + getequalsub + ")";
                                                                                        }

                                                                                        //string strgetsubroll = "select ea.roll_no from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Degree d,course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and d.Degree_Code=ed.degree_code and c.Course_Id=d.Course_Id and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' " + getsubjcode + "";
                                                                                        //DataSet dsroll = dt.select_method_wo_parameter(strgetsubroll, "text");
                                                                                        string binroll = "";
                                                                                        dsarrstulist.Tables[0].DefaultView.RowFilter = "" + getsubjcode + "";
                                                                                        DataView dvarrearstucount = dsarrstulist.Tables[0].DefaultView;
                                                                                        for (int subr = 0; subr < dvarrearstucount.Count; subr++)
                                                                                        {
                                                                                            if (binroll == "")
                                                                                            {
                                                                                                binroll = "'" + dvarrearstucount[subr]["roll_no"].ToString() + "'";
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                binroll = binroll + ",'" + dvarrearstucount[subr]["roll_no"].ToString() + "'";
                                                                                            }
                                                                                        }

                                                                                        string checkroll = "select count(ea.roll_no) from Exam_Details ed,exam_application ea,exam_appl_details ead,exmtt e,exmtt_det et where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and e.exam_code=et.exam_code and ead.subject_no=et.subject_no and ed.Exam_Month=e.Exam_month and ed.Exam_year=e.Exam_year and ed.degree_code=e.degree_code and ed.batch_year=e.batchFrom and ed.current_semester=e.Semester and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and et.exam_date='" + dtt.ToString("MM/dd/yyyy") + "' and et.exam_session='" + session + "' and ea.roll_no in ( " + binroll + ")";
                                                                                        int getsubjectstucount = Convert.ToInt32(dt.GetFunction(checkroll));
                                                                                        if (getsubjectstucount > 0)
                                                                                        {
                                                                                            rollexistflag = true;
                                                                                        }
                                                                                        if (!hatequalgetsubjectcode.Contains(actualsubjecode))
                                                                                        {
                                                                                            hatequalgetsubjectcode.Add(actualsubjecode, getsubjectstucount);
                                                                                        }
                                                                                        for (int esc = 0; esc < dvvale.Count; esc++)
                                                                                        {
                                                                                            if (!hatequalgetsubjectcode.Contains(dvvale[esc]["subjectcode"].ToString()))
                                                                                            {
                                                                                                hatequalgetsubjectcode.Add(dvvale[esc]["subjectcode"].ToString(), getsubjectstucount);
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    if (rollexistflag == false)
                                                                                    {
                                                                                        ha = dv[a]["subject_no"].ToString() + "-" + dv[a]["degree_code"].ToString();
                                                                                        if (!ht.ContainsKey(ha))
                                                                                        {
                                                                                            if (!hatdegree.Contains(degedetails))
                                                                                            {
                                                                                                hatdegree.Add(degedetails, ds1.Tables[0].Rows[u]["subject_type"].ToString());
                                                                                            }
                                                                                            string exm = "if not exists(select * from exmtt where degree_code='" + dv[a]["degree_code"].ToString() + "' and batchFrom='" + dv[a]["batch_year"].ToString() + "' and Semester='" + dv[a]["current_semester"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ') insert into exmtt(degree_code,batchFrom,batchTo,Exam_month,Exam_year,exam_type,Semester) values('" + dv[a]["degree_code"].ToString() + "','" + dv[a]["batch_year"].ToString() + "','" + dv[a]["batch_year"].ToString() + "','" + ddlMonth.SelectedValue + "','" + ddlYear.SelectedItem.Text + "','univ','" + dv[a]["current_semester"].ToString() + "') select * from exmtt where degree_code='" + dv[a]["degree_code"].ToString() + "' and batchFrom='" + dv[a]["batch_year"].ToString() + "' and Semester='" + dv[a]["current_semester"].ToString() + "'  and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ'";
                                                                                            int s = dt.update_method_wo_parameter(exm, "text");
                                                                                            DataSet ds2 = new DataSet();
                                                                                            ds2 = dt.select_method_wo_parameter(exm, "text");
                                                                                            if (ds2.Tables[0].Rows.Count > 0)
                                                                                            {
                                                                                                flagg = 1;
                                                                                                string save = "if exists(select * from exmtt_det where subject_no='" + dv[a]["subject_no"].ToString() + "' and exam_date='" + dtt + "'and exam_session='" + session + "' and exam_code='" + ds2.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + Session["collegecode"] + "' and exam_type='Univ') update exmtt_det set subject_no='" + dv[a]["subject_no"].ToString() + "' , start_time='" + startdate + "' , end_time='" + enddate + "' , exam_date='" + dtt + "', exam_session='" + session + "' , exam_code='" + ds2.Tables[0].Rows[0]["exam_code"].ToString() + "' , coll_code='" + Session["collegecode"] + "' , exam_type='Univ' where subject_no='" + dv[a]["subject_no"].ToString() + "' and exam_date='" + dtt + "'and exam_session='" + session + "' and exam_code='" + ds2.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + Session["collegecode"] + "' and exam_type='Univ' else insert into exmtt_det(subject_no, start_time, end_time, exam_date, exam_session, exam_code, coll_code, exam_type) values('" + dv[a]["subject_no"].ToString() + "' ,'" + startdate + "' ,'" + enddate + "' ,'" + dtt + "','" + session + "' ,'" + ds2.Tables[0].Rows[0]["exam_code"].ToString() + "' ,'" + Session["collegecode"] + "' ,'Univ')";
                                                                                                int v = dt.update_method_wo_parameter(save, "text");
                                                                                            }
                                                                                            ht.Add(ha, dv[a]["degree_code"].ToString());

                                                                                            ////**********All Arrear Papre Move Same Date Start*********************************
                                                                                            //dsequalsubject.Tables[0].DefaultView.RowFilter = "Com_Subject_Code='" + dv[a]["subject_code"].ToString() + "' and degree_code='" + dv[a]["degree_code"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "'";
                                                                                            dsequalsubject.Tables[0].DefaultView.RowFilter = "Com_Subject_Code='" + dv[a]["subject_code"].ToString() + "' and degree_code='" + dv[a]["degree_code"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and Edu_Level='" + sub[1].ToString() + "' and type='" + strtype + "'";
                                                                                            DataView dvequalsubject = dsequalsubject.Tables[0].DefaultView;
                                                                                            if (dvequalsubject.Count > 0)
                                                                                            {
                                                                                                for (int dve = 0; dve < dvequalsubject.Count; dve++)
                                                                                                {
                                                                                                    string getsubject = dvequalsubject[dve]["subject_no"].ToString() + "-" + dvequalsubject[dve]["degree_code"].ToString();
                                                                                                    if (!ht.Contains(getsubject))
                                                                                                    {
                                                                                                        exm = "if not exists(select * from exmtt where degree_code='" + dvequalsubject[dve]["degree_code"].ToString() + "' and batchFrom='" + dvequalsubject[dve]["batch_year"].ToString() + "' and Semester='" + dvequalsubject[dve]["current_semester"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ') insert into exmtt(degree_code,batchFrom,batchTo,Exam_month,Exam_year,exam_type,Semester) values('" + dvequalsubject[dve]["degree_code"].ToString() + "','" + dvequalsubject[dve]["batch_year"].ToString() + "','" + dvequalsubject[dve]["batch_year"].ToString() + "','" + ddlMonth.SelectedValue + "','" + ddlYear.SelectedItem.Text + "','univ','" + dvequalsubject[dve]["current_semester"].ToString() + "') select * from exmtt where degree_code='" + dvequalsubject[dve]["degree_code"].ToString() + "' and batchFrom='" + dvequalsubject[dve]["batch_year"].ToString() + "' and Semester='" + dvequalsubject[dve]["current_semester"].ToString() + "'  and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ'";
                                                                                                        s = dt.update_method_wo_parameter(exm, "text");
                                                                                                        ds2.Dispose();
                                                                                                        ds2.Reset();
                                                                                                        ds2 = dt.select_method_wo_parameter(exm, "text");
                                                                                                        if (ds2.Tables[0].Rows.Count > 0)
                                                                                                        {
                                                                                                            flagg = 1;
                                                                                                            string save = "if exists(select * from exmtt_det where subject_no='" + dvequalsubject[dve]["subject_no"].ToString() + "' and exam_date='" + dtt + "'and exam_session='" + session + "' and exam_code='" + ds2.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + Session["collegecode"] + "' and exam_type='Univ') update exmtt_det set subject_no='" + dvequalsubject[dve]["subject_no"].ToString() + "' , start_time='" + startdate + "' , end_time='" + enddate + "' , exam_date='" + dtt + "', exam_session='" + session + "' , exam_code='" + ds2.Tables[0].Rows[0]["exam_code"].ToString() + "' , coll_code='" + Session["collegecode"] + "' , exam_type='Univ' where subject_no='" + dvequalsubject[dve]["subject_no"].ToString() + "' and exam_date='" + dtt + "'and exam_session='" + session + "' and exam_code='" + ds2.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + Session["collegecode"] + "' and exam_type='Univ' else insert into exmtt_det(subject_no, start_time, end_time, exam_date, exam_session, exam_code, coll_code, exam_type) values('" + dvequalsubject[dve]["subject_no"].ToString() + "' ,'" + startdate + "' ,'" + enddate + "' ,'" + dtt + "','" + session + "' ,'" + ds2.Tables[0].Rows[0]["exam_code"].ToString() + "' ,'" + Session["collegecode"] + "' ,'Univ')";
                                                                                                            int v = dt.update_method_wo_parameter(save, "text");
                                                                                                        }
                                                                                                        ht.Add(getsubject, dvequalsubject[dve]["degree_code"].ToString());
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
                                                            }
                                                            //*******************ARREAR PAPER END******************************************************************
                                                            if (arrearflag == false)
                                                            {
                                                                Hashtable hatcheckdegree = new Hashtable();
                                                                Hashtable hatdegreesubtype = new Hashtable();
                                                                Boolean setflag = false;
                                                                for (int u = 0; u < ds1.Tables[0].Rows.Count; u++)
                                                                {
                                                                    ds3.Tables[0].DefaultView.RowFilter = "subject_type='" + ds1.Tables[0].Rows[u]["subject_type"].ToString() + "' ";
                                                                    dv = ds3.Tables[0].DefaultView;
                                                                    if (dv.Count > 0)
                                                                    {
                                                                        for (int a = 0; a < dv.Count; a++)
                                                                        {
                                                                            Boolean alreadyset = false;
                                                                            Boolean samesubtype = false;
                                                                            string chechdegreedetails = dv[a]["batch_year"].ToString() + '-' + dv[a]["degree_code"].ToString() + '-' + dv[a]["current_semester"].ToString();
                                                                            string eleetivetype = dv[a]["ElectivePap"].ToString();
                                                                            if (hatdegreesubtype.Contains(dv[a]["degree_code"].ToString()))
                                                                            {
                                                                                string gettype = hatdegreesubtype[dv[a]["degree_code"].ToString()].ToString();
                                                                                if (gettype.Trim().ToLower() != ds1.Tables[0].Rows[u]["subject_type"].ToString().Trim().ToLower())
                                                                                {
                                                                                    samesubtype = true;
                                                                                }
                                                                            }
                                                                            if (samesubtype == false)
                                                                            {
                                                                                if (eleetivetype.Trim().ToLower() != "true" && eleetivetype.Trim() != "1")
                                                                                {
                                                                                    string attempts = dv[a]["attempts"].ToString();
                                                                                    if (attempts != "0")
                                                                                    {
                                                                                        if (hatelesubtype.Contains(dv[a]["subject_code"].ToString().Trim().ToLower()))
                                                                                        {
                                                                                            alreadyset = true;
                                                                                        }
                                                                                    }
                                                                                    if (alreadyset == false)
                                                                                    {
                                                                                        if (hatcheckdegree.Contains(chechdegreedetails))
                                                                                        {
                                                                                            alreadyset = true;
                                                                                        }
                                                                                    }
                                                                                }

                                                                                if (alreadyset == false)//Current Paper Set
                                                                                {
                                                                                    Boolean paperflag = false;
                                                                                    int finorder = 1;
                                                                                    if (ds1.Tables[0].Rows[u]["subject_type"].ToString().Trim().ToLower() == "major course")
                                                                                    {
                                                                                        if (hatdegreesuborder.Contains(dv[a]["degree_code"].ToString() + '-' + dv[a]["current_semester"].ToString() + '-' + ds1.Tables[0].Rows[u]["subject_type"].ToString()))
                                                                                        {
                                                                                            string getsuor = hatdegreesuborder[dv[a]["degree_code"].ToString() + '-' + dv[a]["current_semester"].ToString() + '-' + ds1.Tables[0].Rows[u]["subject_type"].ToString()].ToString();
                                                                                            finorder = Convert.ToInt32(getsuor);
                                                                                            finorder++;
                                                                                            hatdegreesuborder[dv[a]["degree_code"].ToString() + '-' + dv[a]["current_semester"].ToString() + '-' + ds1.Tables[0].Rows[u]["subject_type"].ToString()] = finorder.ToString();
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            hatdegreesuborder.Add(dv[a]["degree_code"].ToString() + '-' + dv[a]["current_semester"].ToString() + '-' + ds1.Tables[0].Rows[u]["subject_type"].ToString(), "1");
                                                                                        }

                                                                                        //string strva = dv[a]["subject_code"].ToString();
                                                                                        //string exampapreno = strva[strva.Length - 1].ToString();
                                                                                        //if (exampapreno != finorder.ToString())
                                                                                        //{
                                                                                        //    paperflag = true;
                                                                                        //}
                                                                                    }
                                                                                    if (paperflag == false)
                                                                                    {
                                                                                        ha = dv[a]["subject_no"].ToString() + "-" + dv[a]["degree_code"].ToString();
                                                                                        if (!ht.ContainsKey(ha))
                                                                                        {
                                                                                            string exm = "if not exists(select * from exmtt where degree_code='" + dv[a]["degree_code"].ToString() + "' and batchFrom='" + dv[a]["batch_year"].ToString() + "' and Semester='" + dv[a]["current_semester"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ') insert into exmtt(degree_code,batchFrom,batchTo,Exam_month,Exam_year,exam_type,Semester) values('" + dv[a]["degree_code"].ToString() + "','" + dv[a]["batch_year"].ToString() + "','" + dv[a]["batch_year"].ToString() + "','" + ddlMonth.SelectedValue + "','" + ddlYear.SelectedItem.Text + "','univ','" + dv[a]["current_semester"].ToString() + "') select * from exmtt where degree_code='" + dv[a]["degree_code"].ToString() + "' and batchFrom='" + dv[a]["batch_year"].ToString() + "' and Semester='" + dv[a]["current_semester"].ToString() + "'  and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ'";
                                                                                            int s = dt.update_method_wo_parameter(exm, "text");
                                                                                            DataSet ds2 = new DataSet();
                                                                                            ds2 = dt.select_method_wo_parameter(exm, "text");
                                                                                            if (ds2.Tables[0].Rows.Count > 0)
                                                                                            {
                                                                                                flagg = 1;
                                                                                                string save = "if exists(select * from exmtt_det where subject_no='" + dv[a]["subject_no"].ToString() + "' and exam_date='" + dtt + "'and exam_session='" + session + "' and exam_code='" + ds2.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + Session["collegecode"] + "' and exam_type='Univ')update exmtt_det set subject_no='" + dv[a]["subject_no"].ToString() + "' , start_time='" + startdate + "' , end_time='" + enddate + "' , exam_date='" + dtt + "', exam_session='" + session + "' , exam_code='" + ds2.Tables[0].Rows[0]["exam_code"].ToString() + "' , coll_code='" + Session["collegecode"] + "' , exam_type='Univ' where subject_no='" + dv[a]["subject_no"].ToString() + "' and exam_date='" + dtt + "'and exam_session='" + session + "' and exam_code='" + ds2.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + Session["collegecode"] + "' and exam_type='Univ' else insert into exmtt_det(subject_no, start_time, end_time, exam_date, exam_session, exam_code, coll_code, exam_type) values('" + dv[a]["subject_no"].ToString() + "' ,'" + startdate + "' ,'" + enddate + "' ,'" + dtt + "','" + session + "' ,'" + ds2.Tables[0].Rows[0]["exam_code"].ToString() + "' ,'" + Session["collegecode"] + "' ,'Univ')";
                                                                                                int v = dt.update_method_wo_parameter(save, "text");
                                                                                            }
                                                                                            ht.Add(ha, dv[a]["degree_code"].ToString());
                                                                                            setflag = true;

                                                                                            //Moving Current Paper Matching Arrear Papares
                                                                                            dsallarrear.Tables[0].DefaultView.RowFilter = " subject_code='" + dv[a]["subject_code"].ToString() + "'";
                                                                                            DataView dvallareear = dsallarrear.Tables[0].DefaultView;
                                                                                            for (int allar = 0; allar < dvallareear.Count; allar++)
                                                                                            {
                                                                                                ha = dvallareear[allar]["subject_no"].ToString() + "-" + dvallareear[allar]["degree_code"].ToString();
                                                                                                if (!ht.ContainsKey(ha))
                                                                                                {
                                                                                                    exm = "if not exists(select * from exmtt where degree_code='" + dvallareear[allar]["degree_code"].ToString() + "' and batchFrom='" + dvallareear[allar]["batch_year"].ToString() + "' and Semester='" + dvallareear[allar]["current_semester"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ') insert into exmtt(degree_code,batchFrom,batchTo,Exam_month,Exam_year,exam_type,Semester) values('" + dvallareear[allar]["degree_code"].ToString() + "','" + dvallareear[allar]["batch_year"].ToString() + "','" + dvallareear[allar]["batch_year"].ToString() + "','" + ddlMonth.SelectedValue + "','" + ddlYear.SelectedItem.Text + "','univ','" + dvallareear[allar]["current_semester"].ToString() + "') select * from exmtt where degree_code='" + dvallareear[allar]["degree_code"].ToString() + "' and batchFrom='" + dvallareear[allar]["batch_year"].ToString() + "' and Semester='" + dvallareear[allar]["current_semester"].ToString() + "'  and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ'";
                                                                                                    s = dt.update_method_wo_parameter(exm, "text");
                                                                                                    DataSet ds2allarr = new DataSet();
                                                                                                    ds2allarr = dt.select_method_wo_parameter(exm, "text");
                                                                                                    if (ds2allarr.Tables[0].Rows.Count > 0)
                                                                                                    {
                                                                                                        string examcodealllarear = ds2allarr.Tables[0].Rows[0]["exam_code"].ToString();
                                                                                                        flagg = 1;
                                                                                                        string save = "if exists(select * from exmtt_det where subject_no='" + dvallareear[allar]["subject_no"].ToString() + "' and exam_date='" + dtt + "'and exam_session='" + session + "' and exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + Session["collegecode"] + "' and exam_type='Univ')update exmtt_det set subject_no='" + dvallareear[allar]["subject_no"].ToString() + "' , start_time='" + startdate + "' , end_time='" + enddate + "' , exam_date='" + dtt + "', exam_session='" + session + "' , exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' , coll_code='" + Session["collegecode"] + "' , exam_type='Univ' where subject_no='" + dvallareear[allar]["subject_no"].ToString() + "' and exam_date='" + dtt + "'and exam_session='" + session + "' and exam_code='" + ds2.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + Session["collegecode"] + "' and exam_type='Univ' else insert into exmtt_det(subject_no, start_time, end_time, exam_date, exam_session, exam_code, coll_code, exam_type) values('" + dvallareear[allar]["subject_no"].ToString() + "' ,'" + startdate + "' ,'" + enddate + "' ,'" + dtt + "','" + session + "' ,'" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' ,'" + Session["collegecode"] + "' ,'Univ')";
                                                                                                        int v = dt.update_method_wo_parameter(save, "text");
                                                                                                    }
                                                                                                    ht.Add(ha, dvallareear[allar]["degree_code"].ToString());
                                                                                                }
                                                                                            }


                                                                                            string attempts = dv[a]["attempts"].ToString();
                                                                                            if (attempts == "0")
                                                                                            {
                                                                                                if (!hatdegreesubtype.Contains(dv[a]["degree_code"].ToString()))
                                                                                                {
                                                                                                    hatdegreesubtype.Add(dv[a]["degree_code"].ToString(), ds1.Tables[0].Rows[u]["subject_type"].ToString());
                                                                                                }
                                                                                            }

                                                                                            if (eleetivetype.Trim().ToLower() != "true" && eleetivetype.Trim() != "1")//Non Elective Paper Move
                                                                                            {
                                                                                                if (!hatcheckdegree.Contains(chechdegreedetails))
                                                                                                {
                                                                                                    hatcheckdegree.Add(chechdegreedetails, chechdegreedetails);
                                                                                                }

                                                                                                string getequalsubval = "select Equal_Subject_Code as subjectcode from tbl_equal_paper_Matching where Com_Subject_Code in(select Com_Subject_Code from tbl_equal_paper_Matching where Equal_Subject_Code='" + dv[a]["subject_code"].ToString() + "')";
                                                                                                DataSet dssubval = dt.select_method_wo_parameter(getequalsubval, "text");
                                                                                                for (int eq = 0; eq < dssubval.Tables[0].Rows.Count; eq++)
                                                                                                {
                                                                                                    string streqsubcode = dssubval.Tables[0].Rows[eq]["subjectcode"].ToString();

                                                                                                    //Moving Current Matching Arrear Papares
                                                                                                    dsallarrear.Tables[0].DefaultView.RowFilter = " subject_code='" + streqsubcode.ToString() + "'";
                                                                                                    DataView dvallareearequal = dsallarrear.Tables[0].DefaultView;
                                                                                                    for (int allar = 0; allar < dvallareearequal.Count; allar++)
                                                                                                    {
                                                                                                        string chechdegreedetailseq = dvallareear[allar]["batch_year"].ToString() + '-' + dvallareear[allar]["degree_code"].ToString() + '-' + dvallareear[allar]["current_semester"].ToString();
                                                                                                        if (!hatcheckdegree.Contains(chechdegreedetailseq))
                                                                                                        {
                                                                                                            ha = dvallareear[allar]["subject_no"].ToString() + "-" + dvallareear[allar]["degree_code"].ToString();
                                                                                                            if (!ht.ContainsKey(ha))
                                                                                                            {
                                                                                                                exm = "if not exists(select * from exmtt where degree_code='" + dvallareear[allar]["degree_code"].ToString() + "' and batchFrom='" + dvallareear[allar]["batch_year"].ToString() + "' and Semester='" + dvallareear[allar]["current_semester"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ') insert into exmtt(degree_code,batchFrom,batchTo,Exam_month,Exam_year,exam_type,Semester) values('" + dvallareear[allar]["degree_code"].ToString() + "','" + dvallareear[allar]["batch_year"].ToString() + "','" + dvallareear[allar]["batch_year"].ToString() + "','" + ddlMonth.SelectedValue + "','" + ddlYear.SelectedItem.Text + "','univ','" + dvallareear[allar]["current_semester"].ToString() + "') select * from exmtt where degree_code='" + dvallareear[allar]["degree_code"].ToString() + "' and batchFrom='" + dvallareear[allar]["batch_year"].ToString() + "' and Semester='" + dvallareear[allar]["current_semester"].ToString() + "'  and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ'";
                                                                                                                s = dt.update_method_wo_parameter(exm, "text");
                                                                                                                DataSet ds2allarr = new DataSet();
                                                                                                                ds2allarr = dt.select_method_wo_parameter(exm, "text");
                                                                                                                if (ds2allarr.Tables[0].Rows.Count > 0)
                                                                                                                {
                                                                                                                    flagg = 1;
                                                                                                                    string save = "if exists(select * from exmtt_det where subject_no='" + dvallareear[allar]["subject_no"].ToString() + "' and exam_date='" + dtt + "'and exam_session='" + session + "' and exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + Session["collegecode"] + "' and exam_type='Univ')update exmtt_det set subject_no='" + dvallareear[allar]["subject_no"].ToString() + "' , start_time='" + startdate + "' , end_time='" + enddate + "' , exam_date='" + dtt + "', exam_session='" + session + "' , exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' , coll_code='" + Session["collegecode"] + "' , exam_type='Univ' where subject_no='" + dvallareear[allar]["subject_no"].ToString() + "' and exam_date='" + dtt + "'and exam_session='" + session + "' and exam_code='" + ds2.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + Session["collegecode"] + "' and exam_type='Univ' else insert into exmtt_det(subject_no, start_time, end_time, exam_date, exam_session, exam_code, coll_code, exam_type) values('" + dvallareear[allar]["subject_no"].ToString() + "' ,'" + startdate + "' ,'" + enddate + "' ,'" + dtt + "','" + session + "' ,'" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' ,'" + Session["collegecode"] + "' ,'Univ')";
                                                                                                                    int v = dt.update_method_wo_parameter(save, "text");
                                                                                                                }
                                                                                                                ht.Add(ha, dvallareear[allar]["degree_code"].ToString());
                                                                                                                hatcheckdegree.Add(chechdegreedetailseq, chechdegreedetailseq);
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }

                                                                                                //Non Elective subject type but Subject Wies Elective Move
                                                                                                if (hatmaele.Contains(dv[a]["subject_code"].ToString().Trim().ToLower()))
                                                                                                {
                                                                                                    string strgetsubval = "select distinct s.subject_no,ed.batch_year,ed.degree_code,ed.current_semester,s.subject_name,s.subType_no,ss.subject_type,s.subject_code,ss.ElectivePap from Exam_Details ed,exam_application ea,exam_appl_details ead,course c,Degree d,sub_sem ss,subject s,syllabus_master sy where  ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and s.subType_no=ss.subType_no and s.subject_no=ead.subject_no and ss.promote_count=1 and isnull(s.sub_lab,'0')=0 and sy.syll_code=ss.syll_code and c.Edu_Level='" + sub[1].ToString() + "' and c.type='" + strtype + "' and ed.Exam_Month='" + ddlMonth.SelectedValue + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text + "' and ed.degree_code='" + dv[a]["degree_code"].ToString() + "' " + arrearyear + " and Elective=1 and ss.subject_type='" + ds1.Tables[0].Rows[u]["subject_type"].ToString() + "' order by ed.degree_code,ed.batch_year desc,ed.current_semester";
                                                                                                    DataSet dsmaljeel = dt.select_method_wo_parameter(strgetsubval, "text");
                                                                                                    for (int mael = 0; mael < dsmaljeel.Tables[0].Rows.Count; mael++)
                                                                                                    {
                                                                                                        ha = dsmaljeel.Tables[0].Rows[mael]["subject_no"].ToString() + "-" + dsmaljeel.Tables[0].Rows[mael]["degree_code"].ToString();
                                                                                                        if (!ht.ContainsKey(ha))
                                                                                                        {
                                                                                                            exm = "if not exists(select * from exmtt where degree_code='" + dsmaljeel.Tables[0].Rows[mael]["degree_code"].ToString() + "' and batchFrom='" + dsmaljeel.Tables[0].Rows[mael]["batch_year"].ToString() + "' and Semester='" + dsmaljeel.Tables[0].Rows[mael]["current_semester"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ') insert into exmtt(degree_code,batchFrom,batchTo,Exam_month,Exam_year,exam_type,Semester) values('" + dsmaljeel.Tables[0].Rows[mael]["degree_code"].ToString() + "','" + dsmaljeel.Tables[0].Rows[mael]["batch_year"].ToString() + "','" + dsmaljeel.Tables[0].Rows[mael]["batch_year"].ToString() + "','" + ddlMonth.SelectedValue + "','" + ddlYear.SelectedItem.Text + "','univ','" + dsmaljeel.Tables[0].Rows[mael]["current_semester"].ToString() + "') select * from exmtt where degree_code='" + dsmaljeel.Tables[0].Rows[mael]["degree_code"].ToString() + "' and batchFrom='" + dsmaljeel.Tables[0].Rows[mael]["batch_year"].ToString() + "' and Semester='" + dsmaljeel.Tables[0].Rows[mael]["current_semester"].ToString() + "'  and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ'";
                                                                                                            s = dt.update_method_wo_parameter(exm, "text");
                                                                                                            ds2.Reset();
                                                                                                            ds.Dispose();
                                                                                                            ds2 = dt.select_method_wo_parameter(exm, "text");
                                                                                                            if (ds2.Tables[0].Rows.Count > 0)
                                                                                                            {
                                                                                                                flagg = 1;
                                                                                                                string save = "if exists(select * from exmtt_det where subject_no='" + dsmaljeel.Tables[0].Rows[mael]["subject_no"].ToString() + "' and exam_date='" + dtt + "'and exam_session='" + session + "' and exam_code='" + ds2.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + Session["collegecode"] + "' and exam_type='Univ')update exmtt_det set subject_no='" + dsmaljeel.Tables[0].Rows[mael]["subject_no"].ToString() + "' , start_time='" + startdate + "' , end_time='" + enddate + "' , exam_date='" + dtt + "', exam_session='" + session + "' , exam_code='" + ds2.Tables[0].Rows[0]["exam_code"].ToString() + "' , coll_code='" + Session["collegecode"] + "' , exam_type='Univ' where subject_no='" + dsmaljeel.Tables[0].Rows[mael]["subject_no"].ToString() + "' and exam_date='" + dtt + "'and exam_session='" + session + "' and exam_code='" + ds2.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + Session["collegecode"] + "' and exam_type='Univ' else insert into exmtt_det(subject_no, start_time, end_time, exam_date, exam_session, exam_code, coll_code, exam_type) values('" + dsmaljeel.Tables[0].Rows[mael]["subject_no"].ToString() + "' ,'" + startdate + "' ,'" + enddate + "' ,'" + dtt + "','" + session + "' ,'" + ds2.Tables[0].Rows[0]["exam_code"].ToString() + "' ,'" + Session["collegecode"] + "' ,'Univ')";
                                                                                                                int v = dt.update_method_wo_parameter(save, "text");
                                                                                                            }
                                                                                                            ht.Add(ha, dsmaljeel.Tables[0].Rows[mael]["degree_code"].ToString());
                                                                                                        }

                                                                                                        //Moving Current Paper Matching Arrear Papares
                                                                                                        dsallarrear.Tables[0].DefaultView.RowFilter = " subject_code='" + dv[a]["subject_code"].ToString() + "'";
                                                                                                        DataView dvallareear1 = dsallarrear.Tables[0].DefaultView;
                                                                                                        for (int allar = 0; allar < dvallareear1.Count; allar++)
                                                                                                        {
                                                                                                            ha = dvallareear1[allar]["subject_no"].ToString() + "-" + dvallareear1[allar]["degree_code"].ToString();
                                                                                                            if (!ht.ContainsKey(ha))
                                                                                                            {
                                                                                                                exm = "if not exists(select * from exmtt where degree_code='" + dvallareear1[allar]["degree_code"].ToString() + "' and batchFrom='" + dvallareear1[allar]["batch_year"].ToString() + "' and Semester='" + dvallareear1[allar]["current_semester"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ') insert into exmtt(degree_code,batchFrom,batchTo,Exam_month,Exam_year,exam_type,Semester) values('" + dvallareear1[allar]["degree_code"].ToString() + "','" + dvallareear1[allar]["batch_year"].ToString() + "','" + dvallareear1[allar]["batch_year"].ToString() + "','" + ddlMonth.SelectedValue + "','" + ddlYear.SelectedItem.Text + "','univ','" + dvallareear1[allar]["current_semester"].ToString() + "') select * from exmtt where degree_code='" + dvallareear1[allar]["degree_code"].ToString() + "' and batchFrom='" + dvallareear1[allar]["batch_year"].ToString() + "' and Semester='" + dvallareear1[allar]["current_semester"].ToString() + "'  and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ'";
                                                                                                                s = dt.update_method_wo_parameter(exm, "text");
                                                                                                                DataSet ds2allarr = new DataSet();
                                                                                                                ds2allarr = dt.select_method_wo_parameter(exm, "text");
                                                                                                                if (ds2allarr.Tables[0].Rows.Count > 0)
                                                                                                                {
                                                                                                                    string examcodealllarear = ds2allarr.Tables[0].Rows[0]["exam_code"].ToString();
                                                                                                                    flagg = 1;
                                                                                                                    string save = "if exists(select * from exmtt_det where subject_no='" + dvallareear1[allar]["subject_no"].ToString() + "' and exam_date='" + dtt + "'and exam_session='" + session + "' and exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + Session["collegecode"] + "' and exam_type='Univ')update exmtt_det set subject_no='" + dvallareear1[allar]["subject_no"].ToString() + "' , start_time='" + startdate + "' , end_time='" + enddate + "' , exam_date='" + dtt + "', exam_session='" + session + "' , exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' , coll_code='" + Session["collegecode"] + "' , exam_type='Univ' where subject_no='" + dvallareear1[allar]["subject_no"].ToString() + "' and exam_date='" + dtt + "'and exam_session='" + session + "' and exam_code='" + ds2.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + Session["collegecode"] + "' and exam_type='Univ' else insert into exmtt_det(subject_no, start_time, end_time, exam_date, exam_session, exam_code, coll_code, exam_type) values('" + dvallareear1[allar]["subject_no"].ToString() + "' ,'" + startdate + "' ,'" + enddate + "' ,'" + dtt + "','" + session + "' ,'" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' ,'" + Session["collegecode"] + "' ,'Univ')";
                                                                                                                    int v = dt.update_method_wo_parameter(save, "text");
                                                                                                                }
                                                                                                                ht.Add(ha, dvallareear1[allar]["degree_code"].ToString());
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
                                                                    }
                                                                    if (setflag == true)
                                                                    {
                                                                        u = ds1.Tables[0].Rows.Count;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            hattypeyear.Add(strtype.Trim().ToLower() + '-' + sub[0].ToString().ToLower().Trim() + '-' + sub[0].ToString().ToLower().Trim(), strtype.Trim().ToLower() + '-' + sub[0].ToString().ToLower().Trim() + '-' + sub[0].ToString().ToLower().Trim());
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
                }
            }
            //======================Missing Subject Move==========================
            int allowedtsrgeh = 2300;
            noofallow = txtnoofstudent.Text.ToString();
            if (noofallow.Trim() != "")
            {
                allowedtsrgeh = Convert.ToInt32(noofallow);
            }

            Hashtable hatstrfiled = new Hashtable();
            Hashtable hasharrsub = new Hashtable();
            Hashtable hsaarrsub = new Hashtable();
            string STRMISSUSBCODE = "select distinct s.subject_code,s.subject_name from Exam_Details ed,exam_application ea,exam_appl_details ead,sub_sem ss,subject s,tbl_exam_time_table_batch tb,degree d,course c";
            STRMISSUSBCODE = STRMISSUSBCODE + " where  ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=d.degree_code and d.course_id=c.course_id and c.edu_level=tb.edu_level and ed.batch_year=tb.batch_year and s.subType_no=ss.subType_no and s.subject_no=ead.subject_no and ss.promote_count=1";
            STRMISSUSBCODE = STRMISSUSBCODE + " and ss.lab=0 and isnull(s.sub_lab,'0')=0 and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and s.subject_no not in (select et.subject_no from exmtt e,exmtt_det et where e.exam_code=et.exam_code  and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' ";
            STRMISSUSBCODE = STRMISSUSBCODE + " and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' ) order by s.subject_code desc";
            DataSet dismissingsubcode = dt.select_method_wo_parameter(STRMISSUSBCODE, "Text");

            string strmissingsubjcerget = "select distinct ed.batch_year,ed.degree_code,ed.current_semester,s.subject_no,s.subject_code,s.subject_name from Exam_Details ed,exam_application ea,exam_appl_details ead,sub_sem ss,subject s,tbl_exam_time_table_batch tb,degree d,course c";
            strmissingsubjcerget = strmissingsubjcerget + " where  ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=d.degree_code and d.course_id=c.course_id and c.edu_level=tb.edu_level and ed.batch_year=tb.batch_year and s.subType_no=ss.subType_no and s.subject_no=ead.subject_no and ss.promote_count=1";
            strmissingsubjcerget = strmissingsubjcerget + " and ss.lab=0 and isnull(s.sub_lab,'0')=0 and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and s.subject_no not in (select et.subject_no from exmtt e,exmtt_det et where e.exam_code=et.exam_code  and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' ";
            strmissingsubjcerget = strmissingsubjcerget + " and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' ) order by s.subject_code";
            DataSet dsmissingsubjectmove = dt.select_method_wo_parameter(strmissingsubjcerget, "Text");


            string subtypecouquery = "select distinct c.type,c.Edu_Level,s.subject_code,s.subject_name,sy.semester from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,syllabus_master sy,Degree d,Course c,tbl_exam_time_table_batch tb ";
            subtypecouquery = subtypecouquery + " where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and s.syll_code=sy.syll_code and ed.degree_code=d.Degree_Code and c.edu_level=tb.edu_level and ed.batch_year=tb.batch_year and d.Course_Id=c.Course_Id and isnull(s.sub_lab,'0')=0 and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "'";
            subtypecouquery = subtypecouquery + " order by  c.type,c.Edu_Level,sy.semester,s.subject_code,s.subject_name";
            DataSet dssubtypcou = dt.select_method_wo_parameter(subtypecouquery, "Text");

            string getnoallroll = "select distinct ead.subject_no,ea.roll_no,s.subject_code  from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,tbl_exam_time_table_batch tb,degree d,course c where  ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and isnull(s.sub_lab,'0')=0 and ed.degree_code=d.degree_code and d.course_id=c.course_id and c.edu_level=tb.edu_level and ed.batch_year=tb.batch_year and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "'  ";
            //   getnoallroll = getnoallroll + " and ead.subject_no not in (select et.subject_no from exmtt e,exmtt_det et where e.exam_code=et.exam_code  and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' )  order by ead.subject_no";
            DataSet dsnotroll = dt.select_method_wo_parameter(getnoallroll, "Text");

            for (int msalte = 0; msalte < dismissingsubcode.Tables[0].Rows.Count; msalte++)
            {
                //===================Check And Move existing date
                Boolean flagsetfal = false;
                string missubcode = dismissingsubcode.Tables[0].Rows[msalte]["subject_code"].ToString();

                if (!hsaarrsub.Contains(missubcode.Trim().ToLower()))
                {
                    string getequlacode = "select Equal_Subject_Code as subjectcode from tbl_equal_paper_Matching where Com_Subject_Code in(select Com_Subject_Code from tbl_equal_paper_Matching  where (Equal_Subject_Code='" + missubcode + "' or Com_Subject_Code='" + missubcode + "') )";
                    DataSet dsequlcheck = dt.select_method_wo_parameter(getequlacode, "Text");
                    string esubjectget = "";
                    for (int esc = 0; esc < dsequlcheck.Tables[0].Rows.Count; esc++)
                    {
                        if (esubjectget == "")
                        {
                            esubjectget = "'" + dsequlcheck.Tables[0].Rows[esc]["subjectcode"].ToString() + "'";
                        }
                        else
                        {
                            esubjectget = esubjectget + ",'" + dsequlcheck.Tables[0].Rows[esc]["subjectcode"].ToString() + "'";
                        }

                    }
                    if (esubjectget.Trim() == "")
                    {
                        esubjectget = "'" + missubcode + "'";
                    }
                    string misequsla = "subject_code in(" + esubjectget + ")";
                    esubjectget = " and s.subject_code in(" + esubjectget + ")";

                    string getexamdatequery = "select distinct et.exam_date,et.exam_session from exmtt e,exmtt_det et,subject s where e.exam_code=et.exam_code and et.subject_no=s.subject_no and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' " + esubjectget + "";
                    DataSet dsgetexmdetaisl = dt.select_method_wo_parameter(getexamdatequery, "Text");
                    if (dsgetexmdetaisl.Tables[0].Rows.Count > 0)
                    {

                        DateTime dtsetexdate = Convert.ToDateTime(dsgetexmdetaisl.Tables[0].Rows[0]["exam_date"].ToString());
                        string sessione = dsgetexmdetaisl.Tables[0].Rows[0]["exam_session"].ToString();
                        if (sessione.Trim().ToLower() == "f.n")
                        {
                            session = "F.N";
                            startdate = ddltheoryStartTimeamHrs.SelectedItem.ToString() + ":" + ddltheoryStartTimeamMin.SelectedItem.ToString() + "AM";
                            enddate = ddltheoryendtimeamHrs.SelectedItem.ToString() + ":" + ddltheoryendtimeamMin.SelectedItem.ToString() + "" + ddltheoryendtimeampm.SelectedItem.ToString();

                        }
                        else
                        {
                            session = "A.N";
                            startdate = ddlTheoryStartTimeHrsPm.SelectedItem.ToString() + ":" + ddlTheoryStartTimeMinPm.SelectedItem.ToString() + "PM";
                            enddate = ddlTheoryEndTimeHrsPm.SelectedItem.ToString() + ":" + ddlTheoryEndTimeMinPm.SelectedItem.ToString() + "PM";

                        }

                        dsmissingsubjectmove.Tables[0].DefaultView.RowFilter = misequsla;
                        DataView dvmissub = dsmissingsubjectmove.Tables[0].DefaultView;
                        for (int msq = 0; msq < dvmissub.Count; msq++)
                        {

                            string missbatch = dvmissub[msq]["batch_year"].ToString();
                            string missdegree = dvmissub[msq]["degree_code"].ToString();
                            string misssem = dvmissub[msq]["current_semester"].ToString();
                            string msubjectcode = dvmissub[msq]["subject_code"].ToString();
                            string missssubno = dvmissub[msq]["subject_no"].ToString();
                            string ha = missssubno + "-" + missdegree;
                            if (!hasharrsub.ContainsKey(ha))
                            {
                                string exm = "if not exists(select * from exmtt where degree_code='" + missdegree.ToString() + "' and batchFrom='" + missbatch.ToString() + "' and Semester='" + misssem.ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ') insert into exmtt(degree_code,batchFrom,batchTo,Exam_month,Exam_year,exam_type,Semester) values('" + missdegree.ToString() + "','" + missbatch.ToString() + "','" + missbatch.ToString() + "','" + ddlMonth.SelectedValue + "','" + ddlYear.SelectedItem.Text + "','univ','" + misssem.ToString() + "') select * from exmtt where degree_code='" + missdegree.ToString() + "' and batchFrom='" + missbatch.ToString() + "' and Semester='" + misssem.ToString() + "'  and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ'";
                                int s = dt.update_method_wo_parameter(exm, "text");
                                DataSet ds2allarr = new DataSet();
                                ds2allarr = dt.select_method_wo_parameter(exm, "text");
                                if (ds2allarr.Tables[0].Rows.Count > 0)
                                {
                                    flagsetfal = true;
                                    string examcodealllarear = ds2allarr.Tables[0].Rows[0]["exam_code"].ToString();
                                    flagg = 1;
                                    string save = "if exists(select * from exmtt_det where subject_no='" + missssubno.ToString() + "' and exam_date='" + dtsetexdate + "'and exam_session='" + session + "' and exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + Session["collegecode"] + "' and exam_type='Univ')update exmtt_det set subject_no='" + missssubno.ToString() + "' , start_time='" + startdate + "' , end_time='" + enddate + "' , exam_date='" + dtsetexdate + "', exam_session='" + session + "' , exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' , coll_code='" + Session["collegecode"] + "' , exam_type='Univ' where subject_no='" + missssubno.ToString() + "' and exam_date='" + dtsetexdate + "'and exam_session='" + session + "' and exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + Session["collegecode"] + "' and exam_type='Univ' else insert into exmtt_det(subject_no, start_time, end_time, exam_date, exam_session, exam_code, coll_code, exam_type) values('" + missssubno.ToString() + "' ,'" + startdate + "' ,'" + enddate + "' ,'" + dtsetexdate + "','" + session + "' ,'" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' ,'" + Session["collegecode"] + "' ,'Univ')";
                                    int v = dt.update_method_wo_parameter(save, "text");
                                }
                                hasharrsub.Add(ha, ha);
                                if (!hsaarrsub.Contains(msubjectcode.Trim().ToLower()))
                                {
                                    hsaarrsub.Add(msubjectcode.Trim().ToLower(), msubjectcode.Trim().ToLower());
                                }
                            }
                        }
                    }


                    //Date Loop
                    p = 0;
                    if (flagsetfal == false)
                    {
                        string binroll = "";
                        dsnotroll.Tables[0].DefaultView.RowFilter = misequsla;
                        DataView dvcheckrol = dsnotroll.Tables[0].DefaultView;
                        for (int subr = 0; subr < dvcheckrol.Count; subr++)
                        {
                            if (binroll == "")
                            {
                                binroll = "'" + dvcheckrol[subr]["roll_no"].ToString() + "'";
                            }
                            else
                            {
                                binroll = binroll + ",'" + dvcheckrol[subr]["roll_no"].ToString() + "'";
                            }
                        }

                        dssubtypcou.Tables[0].DefaultView.RowFilter = "subject_code='" + missubcode + "'";
                        DataView dvsubtpeco = dssubtypcou.Tables[0].DefaultView;
                        if (dvsubtpeco.Count > 0)
                        {
                            string arrsubtype = dvsubtpeco[0]["type"].ToString().Trim().ToLower();
                            string arrcourse = dvsubtpeco[0]["Edu_Level"].ToString().Trim().ToLower();
                            string arrsem = dvsubtpeco[0]["semester"].ToString().Trim().ToLower();

                            if (arrsem == "1" || arrsem == "2")
                            {
                                arrsem = "1";
                            }
                            else if (arrsem == "3" || arrsem == "4")
                            {
                                arrsem = "2";
                            }
                            else if (arrsem == "5" || arrsem == "6")
                            {
                                arrsem = "3";
                            }
                            else if (arrsem == "7" || arrsem == "7")
                            {
                                arrsem = "4";
                            }
                            else if (arrsem == "9" || arrsem == "10")
                            {
                                arrsem = "5";
                            }

                            DateTime dttfa = dtfrom;
                            for (DateTime dtt = dttfa; dtt <= dtto; dtt = dtt.AddDays(1))
                            {
                                if (cblHolidays.Items[p].Selected == false)//Holidates Check
                                {
                                    for (int se = 0; se < 2; se++)
                                    {
                                        string sesval = "F.N";
                                        session = "F.N";
                                        if (se > 0)
                                        {
                                            session = "A.N";
                                            sesval = "A.N";
                                        }

                                        if (!hatstrfiled.Contains(dtt.ToString() + '-' + sesval))
                                        {

                                            Boolean allowarrear = false;
                                            string datesee = dtt.ToString("MM/dd/yyyy") + "-" + session;
                                            if (hatdateyer.Contains(datesee))
                                            {
                                                string getyear = hatdateyer[datesee].ToString();
                                                if (getyear.Trim().ToString() != "")
                                                {
                                                    string[] spget = getyear.Split('&');
                                                    for (int tsu = 0; tsu <= spget.GetUpperBound(0); tsu++)
                                                    {
                                                        string[] spty = spget[tsu].Split('-');
                                                        if (spty.GetUpperBound(0) == 2)
                                                        {
                                                            if (spty[0].Trim().ToLower() == arrsubtype && spty[1].Trim().ToLower() == arrcourse && spty[2].Trim().ToLower().Contains(arrsem))
                                                            {
                                                                allowarrear = true;
                                                                tsu = spget.GetUpperBound(0) + 1;
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    allowarrear = true;
                                                }
                                            }
                                            else
                                            {
                                                allowarrear = true;
                                            }
                                            if (allowarrear == true)
                                            {
                                                string strgetdetails = dt.GetFunction("select isnull(count(ead.subject_no),'0') as stucount from exmtt e,exmtt_det et,subject s,exam_appl_details ead where e.exam_code=et.exam_code and et.subject_no=s.subject_no and isnull(s.sub_lab,'0')=0 and ead.subject_no=et.subject_no and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "'  and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and et.exam_date='" + dtt.ToString() + "' and et.exam_session='" + sesval + "'  and ead.appl_no in(select ea.appl_no from Exam_Details ed,exam_application ea where ed.exam_code=ea.exam_code and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year=" + ddlYear.SelectedValue.ToString() + ") group by et.exam_date,et.exam_session");
                                                if (strgetdetails.Trim() == "" || strgetdetails.Trim() == null)
                                                {
                                                    strgetdetails = "0";
                                                }
                                                int countval = Convert.ToInt32(strgetdetails);
                                                if (countval < allowedtsrgeh)
                                                {
                                                    string checkroll = "select isnull(count(ea.roll_no),'0') from Exam_Details ed,exam_application ea,exam_appl_details ead,exmtt e,exmtt_det et where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and e.exam_code=et.exam_code and ead.subject_no=et.subject_no and ed.Exam_Month=e.Exam_month and ed.Exam_year=e.Exam_year and ed.degree_code=e.degree_code and ed.batch_year=e.batchFrom and ed.current_semester=e.Semester  and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and et.exam_date='" + dtt.ToString("MM/dd/yyyy") + "' and et.exam_session='" + session + "' and ea.roll_no in ( " + binroll + ")";
                                                    int getsubjectstucount = Convert.ToInt32(dt.GetFunction(checkroll));
                                                    if (getsubjectstucount == 0)
                                                    {
                                                        DateTime dtsetexdate = dtt;
                                                        string sessione = sesval;
                                                        if (sessione.Trim().ToLower() == "f.n")
                                                        {
                                                            session = "F.N";
                                                            startdate = ddltheoryStartTimeamHrs.SelectedItem.ToString() + ":" + ddltheoryStartTimeamMin.SelectedItem.ToString() + "AM";
                                                            enddate = ddltheoryendtimeamHrs.SelectedItem.ToString() + ":" + ddltheoryendtimeamMin.SelectedItem.ToString() + "" + ddltheoryendtimeampm.SelectedItem.ToString();
                                                        }
                                                        else
                                                        {
                                                            session = "A.N";
                                                            startdate = ddlTheoryStartTimeHrsPm.SelectedItem.ToString() + ":" + ddlTheoryStartTimeMinPm.SelectedItem.ToString() + "PM";
                                                            enddate = ddlTheoryEndTimeHrsPm.SelectedItem.ToString() + ":" + ddlTheoryEndTimeMinPm.SelectedItem.ToString() + "PM";
                                                        }

                                                        dsmissingsubjectmove.Tables[0].DefaultView.RowFilter = misequsla;
                                                        DataView dvmissub = dsmissingsubjectmove.Tables[0].DefaultView;
                                                        for (int msq = 0; msq < dvmissub.Count; msq++)
                                                        {
                                                            string missbatch = dvmissub[msq]["batch_year"].ToString();
                                                            string missdegree = dvmissub[msq]["degree_code"].ToString();
                                                            string misssem = dvmissub[msq]["current_semester"].ToString();
                                                            string msubjectcode = dvmissub[msq]["subject_code"].ToString();
                                                            string missssubno = dvmissub[msq]["subject_no"].ToString();
                                                            string ha = missssubno + "-" + missdegree;
                                                            if (!hasharrsub.ContainsKey(ha))
                                                            {
                                                                string exm = "if not exists(select * from exmtt where degree_code='" + missdegree.ToString() + "' and batchFrom='" + missbatch.ToString() + "' and Semester='" + misssem.ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ') insert into exmtt(degree_code,batchFrom,batchTo,Exam_month,Exam_year,exam_type,Semester) values('" + missdegree.ToString() + "','" + missbatch.ToString() + "','" + missbatch.ToString() + "','" + ddlMonth.SelectedValue + "','" + ddlYear.SelectedItem.Text + "','univ','" + misssem.ToString() + "') select * from exmtt where degree_code='" + missdegree.ToString() + "' and batchFrom='" + missbatch.ToString() + "' and Semester='" + misssem.ToString() + "'  and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ'";
                                                                int s = dt.update_method_wo_parameter(exm, "text");
                                                                DataSet ds2allarr = new DataSet();
                                                                ds2allarr = dt.select_method_wo_parameter(exm, "text");
                                                                if (ds2allarr.Tables[0].Rows.Count > 0)
                                                                {
                                                                    string examcodealllarear = ds2allarr.Tables[0].Rows[0]["exam_code"].ToString();
                                                                    flagg = 1;
                                                                    string save = "if not exists(select * from exmtt_det where subject_no='" + missssubno.ToString() + "' and exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + Session["collegecode"] + "' and exam_type='Univ') insert into exmtt_det(subject_no, start_time, end_time, exam_date, exam_session, exam_code, coll_code, exam_type) values('" + missssubno.ToString() + "' ,'" + startdate + "' ,'" + enddate + "' ,'" + dtsetexdate + "','" + session + "' ,'" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' ,'" + Session["collegecode"] + "' ,'Univ')";
                                                                    int v = dt.update_method_wo_parameter(save, "text");
                                                                }
                                                                hasharrsub.Add(ha, ha);
                                                                if (!hsaarrsub.Contains(msubjectcode.Trim().ToLower()))
                                                                {
                                                                    hsaarrsub.Add(msubjectcode.Trim().ToLower(), msubjectcode.Trim().ToLower());
                                                                }
                                                                dtt = dtto.AddDays(30);
                                                                se = 5;
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    hatstrfiled.Add(dtt.ToString() + '-' + sesval, countval);
                                                }
                                            }
                                        }
                                    }
                                }
                                p++;
                            }
                        }
                    }
                }
            }
            //======================Missing Subject Move End==========================
            if (flagg == 1)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
            }
            else if (flagg == 0)
            {
                if (subflag == true)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Allot The Subject And Then Proceed ')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Set Proper Date and Settings ')", true);
                }
            }

            dsequalsubject.Clear();
            dsequalsubject = null;
            ds.Clear();
            ds = null;
            ds4.Clear();
            ds4 = null;
            //dsarresub.Clear();
            //dsarresub = null;
            //dscurrsub.Clear();
            //dscurrsub = null;
            //dselesubtype.Clear();
            //dselesubtype = null;
            dsequalgetsubject.Clear();
            dsequalgetsubject = null;

            hatelesubtype.Clear();
            hatelesubtype = null;
            ht.Clear();
            ht = null;
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void btnsettingclick(object sender, EventArgs e)
    {
        try
        {
            vl = "";
            type();
            edu(ddltp.SelectedItem.ToString());
            year(ddltp.SelectedItem.ToString());
            loadsettings();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    public void year(string type)
    {
        try
        {
            ddlyears.Items.Clear();
            if (ddleduc.Text.ToString() == "UG")
            {
                ds = dt.select_method_wo_parameter("select distinct TextCode, TextVal from textvaltable where TextCriteria='Feeca' and TextVal like '%Year%' and college_code = '" + Session["collegecode"] + "' order by TextVal asc", "Text");
            }
            else if (ddleduc.Text == "PG" && type != "MCA")
            {
                ds = dt.select_method_wo_parameter("select top(2) TextCode, TextVal from textvaltable where TextCriteria='Feeca' and TextVal like '%Year%' and college_code = '" + Session["collegecode"] + "' order by TextVal asc", "Text");
            }
            else
            {
                ds = dt.select_method_wo_parameter("select distinct TextCode, TextVal from textvaltable where TextCriteria='Feeca' and TextVal like '%Year%' and college_code = '" + Session["collegecode"] + "' order by TextVal asc", "Text");
            }
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlyears.DataSource = ds;
                ddlyears.DataTextField = "TextVal";
                ddlyears.DataValueField = "TextCode";
                ddlyears.DataBind();

                dddyr.DataSource = ds;
                dddyr.DataTextField = "TextVal";
                dddyr.DataValueField = "TextCode";
                dddyr.DataBind();
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();

        }
    }
    public void edu(string type)
    {
        try
        {
            ddledu.Items.Clear();
            ddleduc.Items.Clear();
            ds = dt.select_method_wo_parameter("select distinct Edu_Level from Course where college_code='" + Session["collegecode"] + "' and type='" + type.ToString() + "'", "Text");

            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddledu.DataSource = ds;
                ddledu.DataTextField = "Edu_Level";
                ddledu.DataValueField = "Edu_Level";
                ddledu.DataBind();

                ddleduc.DataSource = ds;
                ddleduc.DataTextField = "Edu_Level";
                ddleduc.DataValueField = "Edu_Level";
                ddleduc.DataBind();

                ddlbatchedu.DataSource = ds;
                ddlbatchedu.DataTextField = "Edu_Level";
                ddlbatchedu.DataValueField = "Edu_Level";
                ddlbatchedu.DataBind();

            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();

        }
    }
    public void type()
    {
        try
        {
            ddltp.Items.Clear();
            ds = dt.select_method_wo_parameter("select distinct type from Course where college_code='" + Session["collegecode"] + "'", "Text");

            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddltp.DataSource = ds;
                ddltp.DataTextField = "type";
                ddltp.DataValueField = "type";
                ddltp.DataBind();

                ddlmd.DataSource = ds;
                ddlmd.DataTextField = "type";
                ddlmd.DataValueField = "type";
                ddlmd.DataBind();

            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }
    protected void ddleduselectvl(object sender, EventArgs e)
    {

        try
        {
            if (ddleduc.SelectedItem.Text == "PG" && ddlmd.SelectedItem.ToString() != "MCA")
            {
                dddyr.Items.Clear();

                ds = dt.select_method_wo_parameter("select top(2) TextCode, TextVal from textvaltable where TextCriteria='Feeca' and TextVal like '%Year%' and college_code = '" + Session["collegecode"] + "' order by TextVal asc", "Text");

                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    dddyr.DataSource = ds;
                    dddyr.DataTextField = "TextVal";
                    dddyr.DataValueField = "TextCode";
                    dddyr.DataBind();
                }
            }
            else
            {
                dddyr.Items.Clear();

                ds = dt.select_method_wo_parameter("select distinct TextCode, TextVal from textvaltable where TextCriteria='Feeca' and TextVal like '%Year%' and college_code = '" + Session["collegecode"] + "' order by TextVal asc", "Text");

                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    dddyr.DataSource = ds;
                    dddyr.DataTextField = "TextVal";
                    dddyr.DataValueField = "TextCode";
                    dddyr.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();

        }

    }
    protected void ddledusel(object sender, EventArgs e)
    {

        try
        {
            if (ddledu.SelectedItem.Text == "PG")
            {
                ddlyears.Items.Clear();

                ds = dt.select_method_wo_parameter("select top(2) TextCode, TextVal from textvaltable where TextCriteria='Feeca' and TextVal like '%Year%' and college_code = '" + Session["collegecode"] + "' order by TextVal asc", "Text");

                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    ddlyears.DataSource = ds;
                    ddlyears.DataTextField = "TextVal";
                    ddlyears.DataValueField = "TextCode";
                    ddlyears.DataBind();
                }
            }
            else
            {
                ddlyears.Items.Clear();

                ds = dt.select_method_wo_parameter("select distinct TextCode, TextVal from textvaltable where TextCriteria='Feeca' and TextVal like '%Year%' and college_code = '" + Session["collegecode"] + "' order by TextVal asc", "Text");

                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    ddlyears.DataSource = ds;
                    ddlyears.DataTextField = "TextVal";
                    ddlyears.DataValueField = "TextCode";
                    ddlyears.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }

    }
    protected void btn_click(object sender, EventArgs e)
    {
        panel6.Visible = false;
        panel3.Visible = false;
        txtvl.Text = "";
    }
    protected void btnsubjectclick(object sender, EventArgs e)
    {
        try
        {
            type();
            edu(ddlmd.SelectedItem.ToString());
            year(ddlmd.SelectedItem.ToString());
            panel4.Visible = true;
            FpSpread2.Visible = false;
            Button3.Visible = false;
            Button4.Visible = false;
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();

        }
    }
    protected void btnreset(object sender, EventArgs e)
    {
        try
        {
            val = 0;
            string del = "delete from exampriority where college_code='" + Session["collegecode"] + "' and education='" + ddleduc.SelectedItem.Text + "' and year='" + dddyr.SelectedItem.Text + "' and mode='" + ddlmd.SelectedItem.Text + "'";
            int g = dt.update_method_wo_parameter(del, "text");
            bindprior();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void btnset(object sender, EventArgs e)
    {
        try
        {
            int fllg = 0;
            FpSpread2.SaveChanges();
            string del = "delete from exampriority where college_code='" + Session["collegecode"] + "' and education='" + ddleduc.SelectedItem.Text + "' and year='" + dddyr.SelectedItem.Text + "' and mode='" + ddlmd.SelectedItem.Text + "'";
            int g = dt.update_method_wo_parameter(del, "text");
            for (int h = 0; h < FpSpread2.Sheets[0].RowCount; h++)
            {
                if (FpSpread2.Sheets[0].Cells[h, 2].Text != "")
                {
                    fllg = 1;
                    string ins = "insert into exampriority(mode,year,education,subject_type,priority,college_code)values('" + ddlmd.SelectedItem.Text + "','" + dddyr.SelectedItem.Text + "','" + ddleduc.SelectedItem.Text + "','" + FpSpread2.Sheets[0].Cells[h, 1].Text + "','" + FpSpread2.Sheets[0].Cells[h, 2].Text + "','" + Session["collegecode"] + "')";
                    int i = dt.update_method_wo_parameter(ins, "text");
                }
            }
            if (fllg == 1)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Set Priority Successfully')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Set Priority')", true);
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();

        }
    }
    protected void FpSpread2_command(object sender, EventArgs e)
    {
        try
        {
            int isval1 = 0;
            string activerow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
            isval1 = Convert.ToInt32(FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Value);
            if (isval1 == 1)
            {
                val = val + 1;
                FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text = val.ToString();
                FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Font.Name = "Book Antiqua";
                FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Locked = true;

            }
            else if (isval1 == 0)
            {
                val = val - 1;
                FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text = "";
                FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Font.Name = "Book Antiqua";
                FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Locked = false;

            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    public void bindprior()
    {
        try
        {
            lblmsg.Visible = false;
            FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
            spred();
            //string sql = "select distinct subject_type from sub_sem ss,syllabus_master sy,Degree d,course c,Department de,Registration r where d.Degree_Code=sy.degree_code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and sy.syll_code=ss.syll_code and r.degree_code=d.Degree_Code and ss.promote_count=1 and sy.degree_code=r.degree_code and sy.Batch_Year=r.Batch_Year and r.DelFlag=0 and r.Exam_Flag<>'debar' and c.type='" + ddlmd.SelectedItem.Text + "' and c.Edu_Level='" + ddleduc.SelectedItem.Text + "'";
            string sem = "";
            if (dddyr.SelectedItem.Text == "1 Year")
            {
                sem = " and sy.semester in(1,2)";
            }
            else if (dddyr.SelectedItem.Text == "2 Year")
            {
                sem = "  and sy.semester in(3,4)";
            }
            else if (dddyr.SelectedItem.Text == "3 Year")
            {
                sem = " and sy.semester in(5,6)";
            }
            else if (dddyr.SelectedItem.Text == "4 Year")
            {
                sem = " and sy.semester in(7,8)";
            }
            else if (dddyr.SelectedItem.Text == "5 Year")
            {
                sem = " and sy.semester in(10,11)";
            }
            string strmod = "";
            if (ddlmd.Enabled == true)
            {
                if (ddlmd.SelectedItem.Text != "")
                {
                    strmod = " And c.TYPE = '" + ddlmd.SelectedItem.Text + "'";
                }
            }
            //string sql = "select distinct subject_type from sub_sem ss,syllabus_master sy where ss.promote_count=1 and sy.syll_code=ss.syll_code and exists (select sy.syll_code from Degree d,course c,Registration r where sy.syll_code=ss.syll_code and  d.Degree_Code=sy.degree_code and sy.degree_code=r.degree_code and sy.Batch_Year=r.Batch_Year and sy.semester=r.Current_Semester and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id " + sem + "  and r.DelFlag=0 and r.Exam_Flag<>'debar' and c.type='" + ddlmd.SelectedItem.Text + "' and c.Edu_Level='" + ddleduc.SelectedItem.Text + "')";
            //string sql = "select distinct ss.subject_type,ed.current_semester from sub_sem ss,subject s,Exam_Details ed,exam_application ea,exam_appl_details ead,Degree D,Course C,syllabus_master sy where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.attempts=0 and ead.subject_no=s.subject_no and s.subType_no=ss.subType_no   AND ED.degree_code = D.Degree_Code AND D.Course_Id = C.Course_Id and sy.syll_code=ss.syll_code and s.syll_code=sy.syll_code and sy.Batch_Year=ed.batch_year and sy.degree_code=ed.degree_code and sy.degree_code=D.Degree_Code and sy.semester=ed.current_semester and ss.promote_count=1 AND Exam_Month = '" + ddlMonth.SelectedValue.ToString() + "' AND Exam_year = '" + ddlYear.SelectedValue.ToString() + "' AND c.TYPE = '" + ddlmd.SelectedItem.Text + "' and c.Edu_Level='" + ddleduc.SelectedItem.Text + "' " + sem + "";


            string sql = "select distinct ss.subject_type from sub_sem ss,subject s,Degree D,Course C,syllabus_master sy where s.subType_no=ss.subType_no   AND D.Course_Id = C.Course_Id and sy.syll_code=ss.syll_code and s.syll_code=sy.syll_code and  sy.degree_code=D.Degree_Code and ss.promote_count=1 " + strmod + " and c.Edu_Level='" + ddleduc.SelectedItem.Text + "' " + sem + "";
            ds = dt.select_method_wo_parameter(sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Button4.Enabled = true;
                FpSpread2.Visible = true;
                Button3.Visible = true;
                Button4.Visible = true;
                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    FpSpread2.Sheets[0].RowCount++;
                    FpSpread2.Sheets[0].Cells[k, 0].Text = Convert.ToString(FpSpread2.Sheets[0].RowCount);
                    FpSpread2.Sheets[0].Cells[k, 0].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].Cells[k, 0].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Cells[k, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Cells[k, 1].Text = ds.Tables[0].Rows[k]["subject_type"].ToString();
                    FpSpread2.Sheets[0].Cells[k, 1].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].Cells[k, 1].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Cells[k, 1].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread2.Sheets[0].Cells[k, 3].CellType = chkcell1;
                    FpSpread2.Sheets[0].Cells[k, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                    chkcell1.AutoPostBack = true;
                }
                FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
            }
            else
            {
                lblmsg.Visible = true;
                lblmsg.Text = "No Records Found";
                FpSpread2.Visible = false;
                Button3.Visible = false;
                Button4.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }
    public void spred()
    {
        try
        {
            Button3.Visible = true;
            Button4.Visible = true;
            FpSpread2.Visible = true;
            FpSpread2.Sheets[0].AutoPostBack = false;
            FpSpread2.Sheets[0].RowHeader.Visible = false;
            FpSpread2.CommandBar.Visible = false;
            FpSpread2.Sheets[0].RowCount = 0;
            FpSpread2.Sheets[0].ColumnCount = 4;
            FpSpread2.Width = 560;
            FpSpread2.Height = 350;

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Subject";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Priority";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Select";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Columns[0].Locked = true;
            FpSpread2.Sheets[0].Columns[1].Locked = true;
            FpSpread2.Sheets[0].Columns[2].Locked = true;
            FpSpread2.Sheets[0].Columns[1].Width = 292;
            FpSpread2.Sheets[0].Columns[2].Width = 80;
            FpSpread2.Sheets[0].Columns[3].Width = 80;

            //if (dddyr.SelectedItem.Text == "1 Year")
            //{
            //    sem = Convert.ToString("1") + "," + Convert.ToString("2");
            //}
            //else if (dddyr.SelectedItem.Text == "2 Year")
            //{
            //    sem = Convert.ToString("3") + "," + Convert.ToString("4");
            //}
            //else if (dddyr.SelectedItem.Text == "3 Year")
            //{
            //    sem = Convert.ToString("5") + "," + Convert.ToString("6");
            //}
            //else if (dddyr.SelectedItem.Text == "4 Year")
            //{
            //    sem = Convert.ToString("7") + "," + Convert.ToString("8");
            //}
            //else if (dddyr.SelectedItem.Text == "5 Year")
            //{
            //    sem = Convert.ToString("9") + "," + Convert.ToString("10");
            //}
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    public void bindsub()
    {
        try
        {

            FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
            spred();
            string strmod = "";
            if (ddlmd.Enabled == true)
            {
                if (ddlmd.SelectedItem.Text != "")
                {
                    strmod = " and c.type='" + ddlmd.SelectedItem.Text + "'";
                }
            }
            string sql1 = "select distinct ss.subject_type,ep.priority from sub_sem ss,syllabus_master sy,Degree d,course c,Department de,exampriority ep where d.Degree_Code=sy.degree_code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and sy.syll_code=ss.syll_code and ss.promote_count=1 and ep.mode=c.type and ep.subject_type=ss.subject_type and ep.education=c.Edu_Level and ep.year in('" + dddyr.SelectedItem.Text + "') " + strmod + "  and c.Edu_Level='" + ddleduc.SelectedItem.Text + "' order by ep.priority";
            ds = dt.select_method_wo_parameter(sql1, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Button4.Enabled = false;
                FpSpread2.Visible = true;
                Button3.Visible = true;
                Button4.Visible = true;
                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    FpSpread2.Sheets[0].RowCount++;
                    FpSpread2.Sheets[0].Cells[k, 0].Text = Convert.ToString(FpSpread2.Sheets[0].RowCount);
                    FpSpread2.Sheets[0].Cells[k, 0].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].Cells[k, 0].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Cells[k, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Cells[k, 1].Text = ds.Tables[0].Rows[k]["subject_type"].ToString();
                    FpSpread2.Sheets[0].Cells[k, 1].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].Cells[k, 1].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Cells[k, 1].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread2.Sheets[0].Cells[k, 2].Text = ds.Tables[0].Rows[k]["priority"].ToString();
                    FpSpread2.Sheets[0].Cells[k, 2].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].Cells[k, 2].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Cells[k, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Cells[k, 3].CellType = chkcell1;
                    FpSpread2.Sheets[0].Cells[k, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                    chkcell1.AutoPostBack = true;
                    FpSpread2.Sheets[0].Cells[k, 3].Locked = true;
                }
                FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
            }
            else
            {
                bindprior();

            }

        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void btnokclick(object sender, EventArgs e)
    {
        try
        {
            val = 0;
            bindsub();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void FpSpread1_CellClick(object sender, EventArgs e)
    {
        Cellclick = true;
    }
    protected void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cellclick == true)
        {
            panel3.Visible = true;
        }
    }
    protected void btnCopyclick(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            int activecol = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveColumn.ToString());
            for (int s = 0; s < FpSpread1.Sheets[0].RowCount; s++)
            {
                if (ddlcpfrom.SelectedItem.Text == FpSpread1.Sheets[0].Cells[s, 0].Text)
                {
                    for (int h = 0; h < FpSpread1.Sheets[0].RowCount; h++)
                    {
                        for (int f = 0; f < cheklist_copyto.Items.Count; f++)
                        {
                            if (cheklist_copyto.Items[f].Selected == true)
                            {
                                if (FpSpread1.Sheets[0].Cells[h, 0].Text == cheklist_copyto.Items[f].Text)
                                {
                                    //if (ddlsess.SelectedValue == "1")
                                    //{
                                    FpSpread1.Sheets[0].Cells[h, activecol].Text = FpSpread1.Sheets[0].Cells[s, Convert.ToInt32(ddlsess.SelectedValue.ToString())].Text;
                                    FpSpread1.Sheets[0].Cells[h, activecol].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[h, activecol].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[h, activecol].HorizontalAlign = HorizontalAlign.Center;
                                    //}
                                    //else if (ddlsess.SelectedValue == "2")
                                    //{
                                    //    FpSpread1.Sheets[0].Cells[h, 2].Text = FpSpread1.Sheets[0].Cells[s, activecol].Text;
                                    //    FpSpread1.Sheets[0].Cells[h, 2].Font.Name = "Book Antiqua";
                                    //    FpSpread1.Sheets[0].Cells[h, 2].Font.Size = FontUnit.Medium;
                                    //    FpSpread1.Sheets[0].Cells[h, 2].HorizontalAlign = HorizontalAlign.Center;
                                    //}
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void checkcopyto_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (checkcopyto.Checked == true)
            {
                for (int i = 0; i < cheklist_copyto.Items.Count; i++)
                {

                    cheklist_copyto.Items[i].Selected = true;
                    txt_copy.Text = "DAY(" + (cheklist_copyto.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < cheklist_copyto.Items.Count; i++)
                {
                    cheklist_copyto.Items[i].Selected = false;
                    txt_copy.Text = "--Select--";
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void cheklist_copyto_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            checkcopyto.Checked = false;
            for (int i = 0; i < cheklist_copyto.Items.Count; i++)
            {
                if (cheklist_copyto.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }

            }
            if (seatcount == cheklist_copyto.Items.Count)
            {
                txt_copy.Text = "DAY(" + seatcount.ToString() + ")";
                checkcopyto.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_copy.Text = "--Select--";
            }
            else
            {
                txt_copy.Text = "DAY(" + seatcount.ToString() + ")";
            }

        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void btnsaveclick(object sender, EventArgs e)
    {
        try
        {
            int fl = 0;
            FpSpread1.SaveChanges();
            //delete for MCC
            string dl = "delete from examtimetablesetting where collegecode='" + Session["collegecode"] + "' and type='" + ddltp.SelectedItem.Text + "' and mode='" + Convert.ToString(ddltype.SelectedItem.Text).Trim() + "'";
            int a = dt.update_method_wo_parameter(dl, "text");
            //
            for (int j = 0; j < FpSpread1.Sheets[0].RowCount; j++)
            {
                if (FpSpread1.Sheets[0].Cells[j, 0].Text != "" && FpSpread1.Sheets[0].Cells[j, 1].Text != "" || FpSpread1.Sheets[0].Cells[j, 2].Text != "")
                {
                    fl = 1;
                    string sv = "insert into examtimetablesetting(mode,day,FN,AN,collegecode,type) values('" + ddltype.SelectedItem.Text + "','" + FpSpread1.Sheets[0].Cells[j, 0].Text + "','" + FpSpread1.Sheets[0].Cells[j, 1].Text + "','" + FpSpread1.Sheets[0].Cells[j, 2].Text + "','" + Session["collegecode"] + "','" + ddltp.SelectedItem.Text + "')";
                    int k = dt.update_method_wo_parameter(sv, "text");
                }
            }
            if (fl == 1)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Set Year & Education')", true);
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void btnremove(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            int activerow = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveRow.ToString());
            int activecol = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveColumn.ToString());
            if (activerow != -1 && activecol != -1)
            {
                FpSpread1.Sheets[0].Cells[activerow, activecol].Text = "";
                txtvl.Text = "";
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select Remove Field')", true);
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void btnkclick(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();

            int activerow = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveRow.ToString());
            int activecol = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveColumn.ToString());
            if (activerow == arow || activerow == 0)
            {

                if (activecol == acol || activecol == 0)
                {
                    vl = txtvl.Text.ToString();
                    if (arriercheck.Checked == false)
                    {
                        if (vl == "")
                        {
                            vl = ddlyears.SelectedItem.Text + "/" + ddledu.SelectedItem.Text;
                        }
                        else
                        {
                            vl = vl + "," + ddlyears.SelectedItem.Text + "/" + ddledu.SelectedItem.Text;
                        }
                    }
                    else if (arriercheck.Checked == true)
                    {
                        if (vl == "")
                        {
                            vl = ddlyears.SelectedItem.Text + "/" + ddledu.SelectedItem.Text + "/Arrears";
                        }
                        else
                        {
                            vl = vl + "," + ddlyears.SelectedItem.Text + "/" + ddledu.SelectedItem.Text + "/Arrears";
                        }
                    }
                }
                else
                {

                    if (arriercheck.Checked == false)
                    {
                        if (vl == "")
                        {
                            vl = ddlyears.SelectedItem.Text + "/" + ddledu.SelectedItem.Text;
                        }
                        else
                        {
                            vl = vl + "," + ddlyears.SelectedItem.Text + "/" + ddledu.SelectedItem.Text;
                        }
                    }
                    else if (arriercheck.Checked == true)
                    {
                        if (vl == "")
                        {
                            vl = ddlyears.SelectedItem.Text + "/" + ddledu.SelectedItem.Text + "/Arrears";
                        }
                        else
                        {
                            vl = vl + "," + ddlyears.SelectedItem.Text + "/" + ddledu.SelectedItem.Text + "/Arrears";
                        }
                    }

                }
            }
            else
            {
                txtvl.Text = "";
                vl = txtvl.Text.ToString();
                if (arriercheck.Checked == false)
                {
                    if (vl == "")
                    {
                        vl = ddlyears.SelectedItem.Text + "/" + ddledu.SelectedItem.Text;
                    }
                    else
                    {
                        vl = vl + "/" + ddlyears.SelectedItem.Text + "/" + ddledu.SelectedItem.Text;
                    }
                }
                else if (arriercheck.Checked == true)
                {
                    if (vl == "")
                    {
                        vl = ddlyears.SelectedItem.Text + " " + ddledu.SelectedItem.Text + "/Arrears";
                    }
                    else
                    {
                        vl = vl + "," + ddlyears.SelectedItem.Text + " " + ddledu.SelectedItem.Text + "/Arrears";
                    }
                }

            }
            txtvl.Text = vl;
            arow = activerow;
            acol = activecol;
            FpSpread1.Sheets[0].Cells[activerow, activecol].Text = txtvl.Text;
            FpSpread1.Sheets[0].Cells[activerow, activecol].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Cells[activerow, activecol].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Cells[activerow, activecol].HorizontalAlign = HorizontalAlign.Center;
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void txtExamFinishDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            pnlHolidays.Visible = true;
            if (Convert.ToString(txtExamFinishDate.Text.ToString()) != " " && Convert.ToString(txtExamFinishDate.Text.ToString()) != null)
            {
                string date1 = "";
                string date2 = "";
                string datefrom = "";
                string dateto = "";
                date1 = txtexamstartdate.Text.ToString();
                string[] splitd = date1.Split(new Char[] { '-' });
                datefrom = splitd[1].ToString() + "-" + splitd[0].ToString() + "-" + splitd[2].ToString();
                date2 = txtExamFinishDate.Text.ToString();
                string[] splitd1 = date2.Split(new Char[] { '-' });
                dateto = splitd1[1].ToString() + "-" + splitd1[0].ToString() + "-" + splitd1[2].ToString();
                DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
                DateTime dt2 = Convert.ToDateTime(dateto.ToString());
                TimeSpan ts = dt2.Subtract(dt1);
                int days;
                days = ts.Days;
                if (days < 0)
                {

                    lblerror.Text = "Start date must be less than Finish Date";
                    lblerror.Visible = true;
                    //btnGenerate.Enabled = false;
                    cblHolidays.Items.Clear();
                }
                else
                {
                    lblerror.Text = "";
                    // btnGenerate.Enabled = true;
                    lblerror.Visible = false;
                    cblHolidays.Items.Clear();
                    for (int i = 0; i <= days; i++)
                    {
                        txtHolidays.Text = "";
                        string holidays;
                        holidays = dt1.AddDays(i).ToString();
                        string[] splitholiday = holidays.Split(new Char[] { '/', ' ' });
                        string finalholiday;
                        finalholiday = splitholiday[1].ToString() + "-" + splitholiday[0].ToString() + "-" + splitholiday[2].ToString();
                        cblHolidays.Items.Add(finalholiday.ToString());

                    }
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void ddltheoryendtimeampm_SelectedIndexChanged(object sender, EventArgs e)
    {
        TheoryAmDuration();
    }
    protected void ddlTheoryEndTimeMinPm_SelectedIndexChanged(object sender, EventArgs e)
    {
        TheoryPmDuration();
    }
    public void TheoryAmDuration()
    {

        string TheoryStarttimeAm;
        string TheoryendttimeAm;

        DateTime theorystarttime;
        DateTime Theoryendtime;
        if (Convert.ToInt16(ddltheoryStartTimeamHrs.SelectedValue) != 0 && Convert.ToInt16(ddltheoryStartTimeamMin.SelectedValue) != 0 && Convert.ToInt16(ddltheoryendtimeamHrs.SelectedValue) != 0 && Convert.ToInt16(ddltheoryendtimeamMin.SelectedValue) != 0)
        {
            TheoryStarttimeAm = ddltheoryStartTimeamHrs.SelectedItem.Text.ToString() + ":" + ddltheoryStartTimeamMin.SelectedItem.Text.ToString() + "  " + "AM";
            TheoryendttimeAm = ddltheoryendtimeamHrs.SelectedItem.Text.ToString() + ":" + ddltheoryendtimeamMin.SelectedItem.Text.ToString() + "  " + ddltheoryendtimeampm.SelectedItem.Text.ToString();
            theorystarttime = Convert.ToDateTime(TheoryStarttimeAm.ToString());
            Theoryendtime = Convert.ToDateTime(TheoryendttimeAm.ToString());
            TimeSpan ts = Theoryendtime.Subtract(theorystarttime);
            int hrs;
            hrs = ts.Hours;
            int min;
            min = ts.Minutes;

            if (hrs <= 0 && min <= 0)
            {
                lblerror.Visible = true;
                lblerror.Text = " Theory Am Start Time Must be Less than End Time";
                txtTheoryDurationam.Text = "";
                //  btnGenerate.Enabled = false;
            }
            else
            {
                lblerror.Visible = false;
                txtTheoryDurationam.Text = hrs.ToString() + ":" + min.ToString();
                // btnGenerate.Enabled = true;
            }
        }
        else
        {
            lblerror.Visible = true;
            lblerror.Text = " Select The Theory [AM] Time";
            txtTheoryDurationam.Text = "";
            // btnGenerate.Enabled = false;
        }

    }
    public void TheoryPmDuration()
    {

        string TheoryStarttimePm;
        string TheoryendttimePm;

        DateTime theorystarttime;
        DateTime Theoryendtime;

        if (Convert.ToInt16(ddlTheoryStartTimeHrsPm.SelectedValue) != 0 && Convert.ToInt16(ddlTheoryStartTimeMinPm.SelectedValue) != 0 && Convert.ToInt16(ddlTheoryEndTimeHrsPm.SelectedValue) != 0 && Convert.ToInt16(ddlTheoryEndTimeMinPm.SelectedValue) != 0)
        {
            TheoryStarttimePm = ddlTheoryStartTimeHrsPm.SelectedItem.Text.ToString() + ":" + ddlTheoryStartTimeMinPm.SelectedItem.Text.ToString() + "  " + "PM";
            TheoryendttimePm = ddlTheoryEndTimeHrsPm.SelectedItem.Text.ToString() + ":" + ddlTheoryEndTimeMinPm.SelectedItem.Text.ToString() + "  " + ddlTheoryendtimePmam.SelectedItem.Text.ToString();
            theorystarttime = Convert.ToDateTime(TheoryStarttimePm.ToString());
            Theoryendtime = Convert.ToDateTime(TheoryendttimePm.ToString());
            TimeSpan ts = Theoryendtime.Subtract(theorystarttime);
            int hrs;
            hrs = ts.Hours;
            int min;
            min = ts.Minutes;

            if (hrs <= 0 && min <= 0)
            {
                lblerror.Visible = true;
                lblerror.Text = " Theory Pm Start Time Must be Less than End Time";
                txtTheoryDurationpm.Text = "";
                //btnGenerate.Enabled = false;
            }
            else
            {
                lblerror.Visible = false;
                txtTheoryDurationpm.Text = hrs.ToString() + ":" + min.ToString();
                //btnGenerate.Enabled = true;
            }
        }
        else
        {
            lblerror.Visible = true;
            lblerror.Text = " Select The Theory [PM] Time";
            txtTheoryDurationpm.Text = "";
            //btnGenerate.Enabled = false;
        }


    }
    public void PracticalAmDuration()
    {

        string PracStarttimeAm;
        string pracendttimeAm;

        DateTime pracstarttime;
        DateTime pracendtime;

        if (Convert.ToInt16(ddlpracstarttimeamHrs.SelectedValue) != 0 && Convert.ToInt16(ddlpracstarttimeamMin.SelectedValue) != 0 && Convert.ToInt16(ddlpracendtimeamHrs.SelectedValue) != 0 && Convert.ToInt16(ddlpracendtimeamMin.SelectedValue) != 0)
        {
            PracStarttimeAm = ddlpracstarttimeamHrs.SelectedItem.Text.ToString() + ":" + ddlpracstarttimeamMin.SelectedItem.Text.ToString() + "  " + "AM";
            pracendttimeAm = ddlpracendtimeamHrs.SelectedItem.Text.ToString() + ":" + ddlpracendtimeamMin.SelectedItem.Text.ToString() + "  " + ddlpracendtimeAmPm.SelectedItem.Text.ToString();
            pracstarttime = Convert.ToDateTime(PracStarttimeAm.ToString());
            pracendtime = Convert.ToDateTime(pracendttimeAm.ToString());
            TimeSpan ts = pracendtime.Subtract(pracstarttime);
            int hrs;
            hrs = ts.Hours;
            int min;
            min = ts.Minutes;

            if (hrs <= 0 && min <= 0)
            {
                lblerror.Visible = true;
                lblerror.Text = " Practical Am Start Time Must be Less than End Time";
                txtpracdurationam.Text = "";
                //btnGenerate.Enabled = false;
            }
            else
            {
                lblerror.Visible = false;
                txtpracdurationam.Text = hrs.ToString() + ":" + min.ToString();
                //btnGenerate.Enabled = true;
            }
        }
        else
        {
            lblerror.Visible = true;
            lblerror.Text = " Select The Practical [AM] Time";
            txtpracdurationam.Text = "";
            //btnGenerate.Enabled = false;
        }

    }
    public void PracticalPmDuration()
    {

        string PracStarttimePm;
        string pracendttimePm;

        DateTime pracstarttime;
        DateTime pracendtime;
        if (Convert.ToInt16(ddlpracstarttimePmHrs.SelectedValue) != 0 && Convert.ToInt16(ddlpracstarttimePmMin.SelectedValue) != 0 && Convert.ToInt16(ddlpracendtimePmHrs.SelectedValue) != 0 && Convert.ToInt16(ddlpracendtimePmMin.SelectedValue) != 0)
        {
            PracStarttimePm = ddlpracstarttimePmHrs.SelectedItem.Text.ToString() + ":" + ddlpracstarttimePmMin.SelectedItem.Text.ToString() + "  " + "PM";
            pracendttimePm = ddlpracendtimePmHrs.SelectedItem.Text.ToString() + ":" + ddlpracendtimePmMin.SelectedItem.Text.ToString() + "  " + ddlpracendtimePmAm.SelectedItem.Text.ToString();
            pracstarttime = Convert.ToDateTime(PracStarttimePm.ToString());
            pracendtime = Convert.ToDateTime(pracendttimePm.ToString());
            TimeSpan ts = pracendtime.Subtract(pracstarttime);
            int hrs;
            hrs = ts.Hours;
            int min;
            min = ts.Minutes;

            if (hrs <= 0 && min <= 0)
            {
                lblerror.Visible = true;
                lblerror.Text = " Theory Pm Start Time Must be Less than End Time";
                txtpracdurationpm.Text = "";
                //btnGenerate.Enabled = false;
            }
            else
            {

                lblerror.Visible = false;
                txtpracdurationpm.Text = hrs.ToString() + ":" + min.ToString();
                //btnGenerate.Enabled = true;
            }
        }
        else
        {
            lblerror.Visible = true;
            lblerror.Text = " Select The Practical [PM] Time";
            txtpracdurationpm.Text = "";
            //btnGenerate.Enabled = false;
        }


    }
    protected void ddlpracendtimeamMin_SelectedIndexChanged(object sender, EventArgs e)
    {
        PracticalAmDuration();
    }
    protected void ddlpracendtimeAmPm_SelectedIndexChanged(object sender, EventArgs e)
    {
        PracticalAmDuration();
    }
    protected void ddlpracendtimePmMin_SelectedIndexChanged(object sender, EventArgs e)
    {
        PracticalPmDuration();
    }
    protected void ddlpracendtimePmHrs_SelectedIndexChanged(object sender, EventArgs e)
    {
        PracticalPmDuration();
    }
    protected void ddlTheoryEndTimeHrsPm_SelectedIndexChanged(object sender, EventArgs e)
    {
        TheoryPmDuration();
    }
    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt16(ddlMonth.SelectedValue) != 0)
        {
            lblerror.Visible = false;

        }
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt16(ddlYear.SelectedValue) != 0)
            {
                lblerror.Visible = false;
                string hol = "select * from examholiday where exammonth='" + ddlMonth.SelectedValue + "' and examyear='" + ddlYear.SelectedItem.Text + "'";
                ds = dt.select_method_wo_parameter(hol, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string[] stdat = ds.Tables[0].Rows[0]["startdate"].ToString().Split('-');
                    string startdate = stdat[1].ToString() + "-" + stdat[0].ToString() + "-" + stdat[2].ToString();
                    string[] endat = ds.Tables[0].Rows[0]["enddate"].ToString().Split('-');
                    string enddate = endat[1].ToString() + "-" + endat[0].ToString() + "-" + endat[2].ToString();
                    txtexamstartdate.Text = startdate;
                    txtExamFinishDate.Text = enddate;
                    //string enddate = ds.Tables[0].Rows[0]["enddate"].ToString();
                    //string startdate = ds.Tables[0].Rows[0]["startdate"].ToString();
                    //txtexamstartdate.Text = startdate;
                    //txtexamstartdate.Text = enddate;
                    pnlHolidays.Visible = true;
                    if (Convert.ToString(txtExamFinishDate.Text.ToString()) != " " && Convert.ToString(txtExamFinishDate.Text.ToString()) != null)
                    {
                        string date1 = "";
                        string date2 = "";
                        string datefrom = "";
                        string dateto = "";
                        date1 = txtexamstartdate.Text.ToString();
                        string[] splitd = date1.Split(new Char[] { '-' });
                        datefrom = splitd[1].ToString() + "-" + splitd[0].ToString() + "-" + splitd[2].ToString();
                        date2 = txtExamFinishDate.Text.ToString();
                        string[] splitd1 = date2.Split(new Char[] { '-' });
                        dateto = splitd1[1].ToString() + "-" + splitd1[0].ToString() + "-" + splitd1[2].ToString();
                        DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
                        DateTime dt2 = Convert.ToDateTime(dateto.ToString());
                        TimeSpan ts = dt2.Subtract(dt1);
                        int days;
                        days = ts.Days;
                        if (days < 0)
                        {

                            lblerror.Text = "Start date must be less than Finish Date";
                            lblerror.Visible = true;
                            cblHolidays.Items.Clear();
                        }
                        else
                        {
                            lblerror.Text = "";
                            lblerror.Visible = false;
                            cblHolidays.Items.Clear();
                            int n = 0;
                            for (int i = 0; i <= days; i++)
                            {
                                txtHolidays.Text = "";
                                string holidays;
                                holidays = dt1.AddDays(i).ToString();
                                string[] splitholiday = holidays.Split(new Char[] { '/', ' ' });
                                string finalholiday;
                                finalholiday = splitholiday[1].ToString() + "-" + splitholiday[0].ToString() + "-" + splitholiday[2].ToString();
                                cblHolidays.Items.Add(finalholiday.ToString());
                                for (int h = 0; h < ds.Tables[0].Rows.Count; h++)
                                {
                                    string[] hool = ds.Tables[0].Rows[h]["holiday_date"].ToString().Split('-');
                                    string holll = hool[1].ToString() + "-" + hool[0].ToString() + "-" + hool[2].ToString();
                                    if (finalholiday == holll)
                                    {
                                        cblHolidays.Items[i].Selected = true;
                                        n = n + 1;
                                    }

                                    txtHolidays.Text = "(" + n + ")  Selected";
                                }
                            }
                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void cblHolidays_SelectedIndexChanged(object sender, EventArgs e)
    {
        int n = 0;
        for (int j = 0; j < cblHolidays.Items.Count; j++)
        {

            if (cblHolidays.Items[j].Selected == true)
            {

                n = n + 1;

            }

            txtHolidays.Text = "(" + n + ")  Selected";

        }
    }

    public void LoadSettings()
    {
        string startdate;
        string endDate;
        string currentMonth;
        string[] sdate;
        string[] edate;

        txtTheoryDurationam.Enabled = false;
        txtTheoryDurationpm.Enabled = false;
        txtpracdurationam.Enabled = false;
        txtpracdurationpm.Enabled = false;

        DateTime now = DateTime.Now;

        currentMonth = now.ToString("MMMM") + "  /  " + DateTime.Today.Year.ToString();

        startdate = DateTime.Today.Date.ToShortDateString();
        sdate = startdate.Split(new char[] { '/' });
        endDate = DateTime.Today.Date.ToShortDateString();
        edate = endDate.Split(new char[] { '/' });
        txtexamstartdate.Text = sdate[1].ToString() + "-" + sdate[0].ToString() + "-" + sdate[2].ToString();
        txtExamFinishDate.Text = edate[1].ToString() + "-" + edate[0].ToString() + "-" + edate[2].ToString();

        lblerror.Visible = false;

        ddltheoryStartTimeamHrs.Items.Insert(0, new ListItem(" ", "0"));
        ddltheoryStartTimeamHrs.Items.Insert(1, new ListItem("01", "1"));
        ddltheoryStartTimeamHrs.Items.Insert(2, new ListItem("02", "2"));
        ddltheoryStartTimeamHrs.Items.Insert(3, new ListItem("03", "3"));
        ddltheoryStartTimeamHrs.Items.Insert(4, new ListItem("04", "4"));
        ddltheoryStartTimeamHrs.Items.Insert(5, new ListItem("05", "5"));
        ddltheoryStartTimeamHrs.Items.Insert(6, new ListItem("06", "6"));
        ddltheoryStartTimeamHrs.Items.Insert(7, new ListItem("07", "7"));
        ddltheoryStartTimeamHrs.Items.Insert(8, new ListItem("08", "8"));
        ddltheoryStartTimeamHrs.Items.Insert(9, new ListItem("09", "9"));
        ddltheoryStartTimeamHrs.Items.Insert(10, new ListItem("10", "10"));
        ddltheoryStartTimeamHrs.Items.Insert(11, new ListItem("11", "11"));
        ddltheoryStartTimeamHrs.Items.Insert(12, new ListItem("12", "12"));

        ddlTheoryStartTimeHrsPm.Items.Insert(0, new ListItem(" ", "0"));
        ddlTheoryStartTimeHrsPm.Items.Insert(1, new ListItem("01", "1"));
        ddlTheoryStartTimeHrsPm.Items.Insert(2, new ListItem("02", "2"));
        ddlTheoryStartTimeHrsPm.Items.Insert(3, new ListItem("03", "3"));
        ddlTheoryStartTimeHrsPm.Items.Insert(4, new ListItem("04", "4"));
        ddlTheoryStartTimeHrsPm.Items.Insert(5, new ListItem("05", "5"));
        ddlTheoryStartTimeHrsPm.Items.Insert(6, new ListItem("06", "6"));
        ddlTheoryStartTimeHrsPm.Items.Insert(7, new ListItem("07", "7"));
        ddlTheoryStartTimeHrsPm.Items.Insert(8, new ListItem("08", "8"));
        ddlTheoryStartTimeHrsPm.Items.Insert(9, new ListItem("09", "9"));
        ddlTheoryStartTimeHrsPm.Items.Insert(10, new ListItem("10", "10"));
        ddlTheoryStartTimeHrsPm.Items.Insert(11, new ListItem("11", "11"));
        ddlTheoryStartTimeHrsPm.Items.Insert(12, new ListItem("12", "12"));

        ddlpracstarttimeamHrs.Items.Insert(0, new ListItem(" ", "0"));
        ddlpracstarttimeamHrs.Items.Insert(1, new ListItem("01", "1"));
        ddlpracstarttimeamHrs.Items.Insert(2, new ListItem("02", "2"));
        ddlpracstarttimeamHrs.Items.Insert(3, new ListItem("03", "3"));
        ddlpracstarttimeamHrs.Items.Insert(4, new ListItem("04", "4"));
        ddlpracstarttimeamHrs.Items.Insert(5, new ListItem("05", "5"));
        ddlpracstarttimeamHrs.Items.Insert(6, new ListItem("06", "6"));
        ddlpracstarttimeamHrs.Items.Insert(7, new ListItem("07", "7"));
        ddlpracstarttimeamHrs.Items.Insert(8, new ListItem("08", "8"));
        ddlpracstarttimeamHrs.Items.Insert(9, new ListItem("09", "9"));
        ddlpracstarttimeamHrs.Items.Insert(10, new ListItem("10", "10"));
        ddlpracstarttimeamHrs.Items.Insert(11, new ListItem("11", "11"));
        ddlpracstarttimeamHrs.Items.Insert(12, new ListItem("12", "12"));


        ddlpracstarttimePmHrs.Items.Insert(0, new ListItem(" ", "0"));
        ddlpracstarttimePmHrs.Items.Insert(1, new ListItem("01", "1"));
        ddlpracstarttimePmHrs.Items.Insert(2, new ListItem("02", "2"));
        ddlpracstarttimePmHrs.Items.Insert(3, new ListItem("03", "3"));
        ddlpracstarttimePmHrs.Items.Insert(4, new ListItem("04", "4"));
        ddlpracstarttimePmHrs.Items.Insert(5, new ListItem("05", "5"));
        ddlpracstarttimePmHrs.Items.Insert(6, new ListItem("06", "6"));
        ddlpracstarttimePmHrs.Items.Insert(7, new ListItem("07", "7"));
        ddlpracstarttimePmHrs.Items.Insert(8, new ListItem("08", "8"));
        ddlpracstarttimePmHrs.Items.Insert(9, new ListItem("09", "9"));
        ddlpracstarttimePmHrs.Items.Insert(10, new ListItem("10", "10"));
        ddlpracstarttimePmHrs.Items.Insert(11, new ListItem("11", "11"));
        ddlpracstarttimePmHrs.Items.Insert(12, new ListItem("12", "12"));


        ddltheoryendtimeamHrs.Items.Insert(0, new ListItem(" ", "0"));
        ddltheoryendtimeamHrs.Items.Insert(1, new ListItem("01", "1"));
        ddltheoryendtimeamHrs.Items.Insert(2, new ListItem("02", "2"));
        ddltheoryendtimeamHrs.Items.Insert(3, new ListItem("03", "3"));
        ddltheoryendtimeamHrs.Items.Insert(4, new ListItem("04", "4"));
        ddltheoryendtimeamHrs.Items.Insert(5, new ListItem("05", "5"));
        ddltheoryendtimeamHrs.Items.Insert(6, new ListItem("06", "6"));
        ddltheoryendtimeamHrs.Items.Insert(7, new ListItem("07", "7"));
        ddltheoryendtimeamHrs.Items.Insert(8, new ListItem("08", "8"));
        ddltheoryendtimeamHrs.Items.Insert(9, new ListItem("09", "9"));
        ddltheoryendtimeamHrs.Items.Insert(10, new ListItem("10", "10"));
        ddltheoryendtimeamHrs.Items.Insert(11, new ListItem("11", "11"));
        ddltheoryendtimeamHrs.Items.Insert(12, new ListItem("12", "12"));

        ddlTheoryEndTimeHrsPm.Items.Insert(0, new ListItem(" ", "0"));
        ddlTheoryEndTimeHrsPm.Items.Insert(1, new ListItem("01", "1"));
        ddlTheoryEndTimeHrsPm.Items.Insert(2, new ListItem("02", "2"));
        ddlTheoryEndTimeHrsPm.Items.Insert(3, new ListItem("03", "3"));
        ddlTheoryEndTimeHrsPm.Items.Insert(4, new ListItem("04", "4"));
        ddlTheoryEndTimeHrsPm.Items.Insert(5, new ListItem("05", "5"));
        ddlTheoryEndTimeHrsPm.Items.Insert(6, new ListItem("06", "6"));
        ddlTheoryEndTimeHrsPm.Items.Insert(7, new ListItem("07", "7"));
        ddlTheoryEndTimeHrsPm.Items.Insert(8, new ListItem("08", "8"));
        ddlTheoryEndTimeHrsPm.Items.Insert(9, new ListItem("09", "9"));
        ddlTheoryEndTimeHrsPm.Items.Insert(10, new ListItem("10", "10"));
        ddlTheoryEndTimeHrsPm.Items.Insert(11, new ListItem("11", "11"));
        ddlTheoryEndTimeHrsPm.Items.Insert(12, new ListItem("12", "12"));

        ddlpracendtimeamHrs.Items.Insert(0, new ListItem(" ", "0"));
        ddlpracendtimeamHrs.Items.Insert(1, new ListItem("01", "1"));
        ddlpracendtimeamHrs.Items.Insert(2, new ListItem("02", "2"));
        ddlpracendtimeamHrs.Items.Insert(3, new ListItem("03", "3"));
        ddlpracendtimeamHrs.Items.Insert(4, new ListItem("04", "4"));
        ddlpracendtimeamHrs.Items.Insert(5, new ListItem("05", "5"));
        ddlpracendtimeamHrs.Items.Insert(6, new ListItem("06", "6"));
        ddlpracendtimeamHrs.Items.Insert(7, new ListItem("07", "7"));
        ddlpracendtimeamHrs.Items.Insert(8, new ListItem("08", "8"));
        ddlpracendtimeamHrs.Items.Insert(9, new ListItem("09", "9"));
        ddlpracendtimeamHrs.Items.Insert(10, new ListItem("10", "10"));
        ddlpracendtimeamHrs.Items.Insert(11, new ListItem("11", "11"));
        ddlpracendtimeamHrs.Items.Insert(12, new ListItem("12", "12"));

        ddlpracendtimePmHrs.Items.Insert(0, new ListItem(" ", "0"));
        ddlpracendtimePmHrs.Items.Insert(1, new ListItem("01", "1"));
        ddlpracendtimePmHrs.Items.Insert(2, new ListItem("02", "2"));
        ddlpracendtimePmHrs.Items.Insert(3, new ListItem("03", "3"));
        ddlpracendtimePmHrs.Items.Insert(4, new ListItem("04", "4"));
        ddlpracendtimePmHrs.Items.Insert(5, new ListItem("05", "5"));
        ddlpracendtimePmHrs.Items.Insert(6, new ListItem("06", "6"));
        ddlpracendtimePmHrs.Items.Insert(7, new ListItem("07", "7"));
        ddlpracendtimePmHrs.Items.Insert(8, new ListItem("08", "8"));
        ddlpracendtimePmHrs.Items.Insert(9, new ListItem("09", "9"));
        ddlpracendtimePmHrs.Items.Insert(10, new ListItem("10", "10"));
        ddlpracendtimePmHrs.Items.Insert(11, new ListItem("11", "11"));
        ddlpracendtimePmHrs.Items.Insert(12, new ListItem("12", "12"));



        ddltheoryendtimeampm.Items.Insert(0, new ListItem("AM", "0"));
        ddltheoryendtimeampm.Items.Insert(1, new ListItem("PM", "1"));

        ddlTheoryendtimePmam.Items.Insert(0, new ListItem("PM", "0"));
        ddlTheoryendtimePmam.Enabled = false;

        ddlpracendtimeAmPm.Items.Insert(0, new ListItem("AM", "0"));
        ddlpracendtimeAmPm.Items.Insert(1, new ListItem("PM", "1"));

        ddlpracendtimePmAm.Items.Insert(0, new ListItem("PM", "0"));
        ddlpracendtimePmAm.Enabled = false;


        ddltheoryStartTimeamMin.Items.Insert(0, new ListItem(" ", "0"));
        ddltheoryStartTimeamMin.Items.Insert(1, new ListItem("00", "1"));
        ddltheoryStartTimeamMin.Items.Insert(2, new ListItem("05", "2"));

        ddlTheoryStartTimeMinPm.Items.Insert(0, new ListItem(" ", "0"));
        ddlTheoryStartTimeMinPm.Items.Insert(1, new ListItem("00", "1"));
        ddlTheoryStartTimeMinPm.Items.Insert(2, new ListItem("05", "2"));

        ddlpracstarttimeamMin.Items.Insert(0, new ListItem(" ", "0"));
        ddlpracstarttimeamMin.Items.Insert(1, new ListItem("00", "1"));
        ddlpracstarttimeamMin.Items.Insert(2, new ListItem("05", "2"));

        ddlpracstarttimePmMin.Items.Insert(0, new ListItem(" ", "0"));
        ddlpracstarttimePmMin.Items.Insert(1, new ListItem("00", "1"));
        ddlpracstarttimePmMin.Items.Insert(2, new ListItem("05", "2"));

        ddltheoryendtimeamMin.Items.Insert(0, new ListItem(" ", "0"));
        ddltheoryendtimeamMin.Items.Insert(1, new ListItem("00", "1"));
        ddltheoryendtimeamMin.Items.Insert(2, new ListItem("05", "2"));

        ddlTheoryEndTimeMinPm.Items.Insert(0, new ListItem(" ", "0"));
        ddlTheoryEndTimeMinPm.Items.Insert(1, new ListItem("00", "1"));
        ddlTheoryEndTimeMinPm.Items.Insert(2, new ListItem("05", "2"));


        ddlpracendtimeamMin.Items.Insert(0, new ListItem(" ", "0"));
        ddlpracendtimeamMin.Items.Insert(1, new ListItem("00", "1"));
        ddlpracendtimeamMin.Items.Insert(2, new ListItem("05", "2"));

        ddlpracendtimePmMin.Items.Insert(0, new ListItem(" ", "0"));
        ddlpracendtimePmMin.Items.Insert(1, new ListItem("00", "1"));
        ddlpracendtimePmMin.Items.Insert(2, new ListItem("05", "2"));


        for (int i = 10; i < 60; i += 5)
        {
            ddltheoryStartTimeamMin.Items.Add(Convert.ToString(i));
            ddltheoryendtimeamMin.Items.Add(Convert.ToString(i));
            ddlTheoryStartTimeMinPm.Items.Add(Convert.ToString(i));
            ddlTheoryEndTimeMinPm.Items.Add(Convert.ToString(i));
            ddlpracstarttimeamMin.Items.Add(Convert.ToString(i));
            ddlpracendtimeamMin.Items.Add(Convert.ToString(i));
            ddlpracstarttimePmMin.Items.Add(Convert.ToString(i));
            ddlpracendtimePmMin.Items.Add(Convert.ToString(i));
        }


        ddlMonth.Items.Insert(0, new ListItem("  ", "0"));
        ddlMonth.Items.Insert(1, new ListItem("Jan", "1"));
        ddlMonth.Items.Insert(2, new ListItem("Feb", "2"));
        ddlMonth.Items.Insert(3, new ListItem("Mar", "3"));
        ddlMonth.Items.Insert(4, new ListItem("Apr", "4"));
        ddlMonth.Items.Insert(5, new ListItem("May", "5"));
        ddlMonth.Items.Insert(6, new ListItem("Jun", "6"));
        ddlMonth.Items.Insert(7, new ListItem("Jul", "7"));
        ddlMonth.Items.Insert(8, new ListItem("Aug", "8"));
        ddlMonth.Items.Insert(9, new ListItem("Sep", "9"));
        ddlMonth.Items.Insert(10, new ListItem("Oct", "10"));
        ddlMonth.Items.Insert(11, new ListItem("Nov", "11"));
        ddlMonth.Items.Insert(12, new ListItem("Dec", "12"));

        ddlYear.Items.Clear();
        DataSet dsss = dt.select_method_wo_parameter(" select distinct Exam_year from exam_details order by Exam_year desc", "Text");
        if (dsss.Tables[0].Rows.Count > 0)
        {
            ddlYear.DataSource = dsss;
            ddlYear.DataTextField = "Exam_year";
            ddlYear.DataBind();
        }
        ddlYear.Items.Insert(0, new ListItem("  ", "0"));

    }
    protected void ddlExamstartswith_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void btnbatchset_Click(object sender, EventArgs e)
    {
        BatchSetPanel.Visible = true;

        ddlbatchedu.Items.Clear();
        ds = dt.select_method_wo_parameter("select distinct Edu_Level from Course where college_code='" + Session["collegecode"] + "'", "Text");
        int count = ds.Tables[0].Rows.Count;
        if (count > 0)
        {
            ddlbatchedu.DataSource = ds;
            ddlbatchedu.DataTextField = "Edu_Level";
            ddlbatchedu.DataValueField = "Edu_Level";
            ddlbatchedu.DataBind();
        }
        LOADBATCHSET();
    }
    protected void btnbatchsetclose_click(object sender, EventArgs e)
    {
        BatchSetPanel.Visible = false;
    }
    protected void ddlbatchedu_SelectedIndexChanged(object sender, EventArgs e)
    {
        LOADBATCHSET();
    }
    protected void btnbatchsetsave_click(object sender, EventArgs e)
    {
        try
        {
            FpBatchSetting.SaveChanges();
            for (int d = 0; d < FpBatchSetting.Sheets[0].RowCount; d++)
            {
                int isval = Convert.ToInt32(FpBatchSetting.Sheets[0].Cells[d, 2].Value);
                string insquery = "delete from tbl_exam_time_table_batch where Edu_Level='" + ddlbatchedu.SelectedItem.ToString() + "' and batch_year='" + FpBatchSetting.Sheets[0].Cells[d, 1].Text.ToString() + "'";
                int val = dt.update_method_wo_parameter(insquery, "text");
                if (isval == 1)
                {
                    insquery = "insert into tbl_exam_time_table_batch(Edu_Level,batch_year) values('" + ddlbatchedu.SelectedItem.ToString() + "','" + FpBatchSetting.Sheets[0].Cells[d, 1].Text.ToString() + "')";
                    val = dt.update_method_wo_parameter(insquery, "text");
                }
            }
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
        }
        catch (Exception ex)
        {
            lblbatcherror.Visible = true;
            lblbatcherror.Text = ex.ToString();
        }
    }
    public void LOADBATCHSET()
    {
        try
        {
            string strquery = "select distinct batch_year from Exam_Details ed,Degree d,Course c where ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and c.Edu_Level='" + ddlbatchedu.SelectedItem.ToString() + "' order by batch_year desc";
            strquery = strquery + " select * from tbl_exam_time_table_batch where Edu_Level='" + ddlbatchedu.SelectedItem.ToString() + "'";
            DataSet dseduval = dt.select_method_wo_parameter(strquery, "Text");
            if (dseduval.Tables[0].Rows.Count > 0)
            {
                FpBatchSetting.Visible = true;
                FpBatchSetting.CommandBar.Visible = false;
                FpBatchSetting.Sheets[0].SheetCorner.ColumnCount = 0;

                FpBatchSetting.Sheets[0].ColumnCount = 3;
                FpBatchSetting.Sheets[0].RowCount = 0;


                FpBatchSetting.Sheets[0].Columns[0].Font.Bold = true;
                FpBatchSetting.Sheets[0].Columns[1].Font.Bold = true;
                FpBatchSetting.Sheets[0].Columns[2].Font.Bold = true;
                FpBatchSetting.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpBatchSetting.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                FpBatchSetting.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;

                FpBatchSetting.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpBatchSetting.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpBatchSetting.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch Year";
                FpBatchSetting.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpBatchSetting.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Select";
                FpBatchSetting.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;

                FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();

                for (int b = 0; b < dseduval.Tables[0].Rows.Count; b++)
                {
                    FpBatchSetting.Sheets[0].RowCount++;
                    FpBatchSetting.Sheets[0].Cells[FpBatchSetting.Sheets[0].RowCount - 1, 0].Text = FpBatchSetting.Sheets[0].RowCount.ToString();
                    FpBatchSetting.Sheets[0].Cells[FpBatchSetting.Sheets[0].RowCount - 1, 1].Text = dseduval.Tables[0].Rows[b]["Batch_year"].ToString();
                    FpBatchSetting.Sheets[0].Cells[FpBatchSetting.Sheets[0].RowCount - 1, 1].Locked = true;
                    FpBatchSetting.Sheets[0].Cells[FpBatchSetting.Sheets[0].RowCount - 1, 2].CellType = chk;

                    dseduval.Tables[1].DefaultView.RowFilter = "Batch_Year='" + dseduval.Tables[0].Rows[b]["Batch_year"].ToString() + "'";
                    DataView dvbatch = dseduval.Tables[1].DefaultView;
                    if (dvbatch.Count > 0)
                    {
                        FpBatchSetting.Sheets[0].Cells[FpBatchSetting.Sheets[0].RowCount - 1, 2].Value = 1;
                    }
                    else
                    {
                        FpBatchSetting.Sheets[0].Cells[FpBatchSetting.Sheets[0].RowCount - 1, 2].Value = 0;
                    }
                }
            }
            else
            {
                lblbatcherror.Visible = true;
                lblbatcherror.Text = "No Batch Year Found Please Check The Exam Application";
                FpBatchSetting.Visible = false;
            }
            int hegiht = FpBatchSetting.Sheets[0].RowCount;
            FpBatchSetting.Height = hegiht * 25 + 25;
            FpBatchSetting.Width = 300;
            FpBatchSetting.Sheets[0].PageSize = FpBatchSetting.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            lblbatcherror.Visible = true;
            lblbatcherror.Text = ex.ToString();
        }
    }
    protected void FpBatchSetting_command(object sender, EventArgs e)
    {
        try
        {
            int isval1 = 0;
            string activerow = FpBatchSetting.ActiveSheetView.ActiveRow.ToString();
            isval1 = Convert.ToInt32(FpBatchSetting.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Value);
            if (isval1 == 1)
            {
                val = val + 1;
                FpBatchSetting.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text = val.ToString();
                FpBatchSetting.Sheets[0].Cells[Convert.ToInt32(activerow), 2].HorizontalAlign = HorizontalAlign.Center;
                FpBatchSetting.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Font.Name = "Book Antiqua";
                FpBatchSetting.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Locked = true;

            }
            else if (isval1 == 0)
            {
                val = val - 1;
                FpBatchSetting.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text = "";
                FpBatchSetting.Sheets[0].Cells[Convert.ToInt32(activerow), 2].HorizontalAlign = HorizontalAlign.Center;
                FpBatchSetting.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Font.Name = "Book Antiqua";
                FpBatchSetting.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Locked = false;

            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }


    protected void ddltheoryendtimeamMin_SelectedIndexChanged(object sender, EventArgs e)
    {
        TheoryAmDuration();
    }

    protected void ddltheoryendtimeamHrs_SelectedIndexChanged(object sender, EventArgs e)
    {
        TheoryAmDuration();
    }
    protected void ddltheoryStartTimeamMin_SelectedIndexChanged(object sender, EventArgs e)
    {
        TheoryAmDuration();
    }
    protected void ddltheoryStartTimeamHrs_SelectedIndexChanged(object sender, EventArgs e)
    {
        TheoryAmDuration();
    }
    protected void ddlTheoryStartTimeMinPm_SelectedIndexChanged(object sender, EventArgs e)
    {
        TheoryPmDuration();
    }
    protected void ddlTheoryStartTimeHrsPm_SelectedIndexChanged(object sender, EventArgs e)
    {
        TheoryPmDuration();
    }
    protected void ddlpracendtimeamHrs_SelectedIndexChanged(object sender, EventArgs e)
    {
        PracticalAmDuration();
    }
    protected void ddlpracstarttimeamMin_SelectedIndexChanged(object sender, EventArgs e)
    {
        PracticalAmDuration();
    }
    protected void ddlpracstarttimeamHrs_SelectedIndexChanged(object sender, EventArgs e)
    {
        PracticalAmDuration();
    }
    protected void ddlpracstarttimePmHrs_SelectedIndexChanged(object sender, EventArgs e)
    {
        PracticalPmDuration();
    }
    protected void ddlpracstarttimePmMin_SelectedIndexChanged(object sender, EventArgs e)
    {
        PracticalPmDuration();
    }




    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        edu(ddltp.SelectedItem.ToString());
        year(ddltp.SelectedItem.ToString());
        loadsettings();
    }
    public void loadsettings()
    {
        try
        {
            int datecount = 0;

            FpSpread1.Visible = true;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 3;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Day";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "FN";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "AN";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;


            string date1 = "";
            string date2 = "";
            string datefrom = "";
            string dateto = "";
            date1 = txtexamstartdate.Text.ToString();
            string[] splitd = date1.Split(new Char[] { '-' });
            datefrom = splitd[1].ToString() + "-" + splitd[0].ToString() + "-" + splitd[2].ToString();
            date2 = txtExamFinishDate.Text.ToString();
            string[] splitd1 = date2.Split(new Char[] { '-' });
            dateto = splitd1[1].ToString() + "-" + splitd1[0].ToString() + "-" + splitd1[2].ToString();
            DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
            DateTime dt2 = Convert.ToDateTime(dateto.ToString());
            TimeSpan ts = dt2.Subtract(dt1);
            int date = ts.Days;
            if (txtHolidays.Text != "")
            {
                string[] arr = txtHolidays.Text.Split(')');
                string[] arrr = arr[0].Split('(');
                datecount = (date + 1) - Convert.ToInt32(arrr[1].ToString());
            }
            else
            {
                datecount = (date + 1);
            }
            string name = "";
            if (datecount > 0)
            {
                string strquery = " select * from examtimetablesetting where mode='" + ddltype.SelectedItem.ToString() + "' and TYPE='" + ddltp.SelectedItem.ToString() + "' and collegecode='" + Session["collegecode"] + "'";
                DataSet dsexam = dt.select_method_wo_parameter(strquery, "text");
                panel6.Visible = true;
                ddlcpfrom.Items.Clear();
                cheklist_copyto.Items.Clear();
                for (int i = 0; i < datecount; i++)
                {
                    FpSpread1.Sheets[0].RowCount++;
                    name = "DAY" + " " + FpSpread1.Sheets[0].RowCount;
                    FpSpread1.Sheets[0].Cells[i, 0].Text = name;
                    FpSpread1.Sheets[0].Cells[i, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[i, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[i, 0].Font.Bold = true;
                    FpSpread1.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;

                    dsexam.Tables[0].DefaultView.RowFilter = " day='" + name + "'";
                    DataView dvexam = dsexam.Tables[0].DefaultView;
                    if (dvexam.Count > 0)
                    {
                        string fnvalue = dvexam[0]["FN"].ToString();
                        string anvalue = dvexam[0]["AN"].ToString();
                        FpSpread1.Sheets[0].Cells[i, 1].Text = fnvalue;
                        FpSpread1.Sheets[0].Cells[i, 2].Text = anvalue;

                        FpSpread1.Sheets[0].Cells[i, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[i, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[i, 1].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].Cells[i, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[i, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[i, 2].HorizontalAlign = HorizontalAlign.Center;

                    }
                    ddlcpfrom.Items.Insert(i, name);
                    cheklist_copyto.Items.Insert(i, name);

                }
            }
            else
            {
                panel6.Visible = false;
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Start Date Should Be Less Than End Date')", true);
            }
            txt_copy.Text = "--Select--";
            FpSpread1.Sheets[0].Columns[1].Width = 300;
            FpSpread1.Sheets[0].Columns[2].Width = 300;
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void ddlmd_SelectedIndexChanged(object sender, EventArgs e)
    {
        edu(ddlmd.SelectedItem.ToString());
        year(ddlmd.SelectedItem.ToString());
    }



    #region "UNWANTED CODE"
    //protected void btngenerateclicknewformat(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string arrearyear = "";
    //        int smarrva = 0;
    //        Hashtable ht = new Hashtable();
    //        Boolean subflag = false;
    //        int flagg = 0;
    //        int count = 0;
    //        int p = 0;
    //        string sm = "";
    //        string session = "";
    //        string[] fnan = new string[n];


    //        if (txtTheoryDurationam.Text.ToString().Trim() == "")
    //        {
    //            errmsg.Visible = true;
    //            errmsg.Text = "PLEASE SET THE FORE NOON TIME IN THEORY";
    //            return;
    //        }
    //        if (txtTheoryDurationpm.Text.ToString().Trim() == "")
    //        {
    //            errmsg.Visible = true;
    //            errmsg.Text = "PLEASE SET THE AFTER NOON TIME IN THEORY";
    //            return;
    //        }
    //        string startdate = ddltheoryStartTimeamHrs.SelectedItem.ToString() + ":" + ddltheoryStartTimeamMin.SelectedItem.ToString() + "AM";
    //        string enddate = ddltheoryendtimeamHrs.SelectedItem.ToString() + ":" + ddltheoryendtimeamMin.SelectedItem.ToString() + "" + ddltheoryendtimeampm.SelectedItem.ToString();

    //        string[] getfdate = txtexamstartdate.Text.ToString().Split('-');
    //        DateTime dtfrom = Convert.ToDateTime(getfdate[1] + '/' + getfdate[0] + '/' + getfdate[2]);
    //        string[] gettdate = txtExamFinishDate.Text.ToString().Split('-');
    //        DateTime dtto = Convert.ToDateTime(gettdate[1] + '/' + gettdate[0] + '/' + gettdate[2]);


    //        Hashtable hatdegreesuborder = new Hashtable();

    //        string deletexam = "delete from exmtt_det where exam_code in(select exam_code from exmtt where Exam_month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "')";
    //        int delexam = dt.update_method_wo_parameter(deletexam, "text");

    //        string deletholiday = "delete from examholiday where Exammonth='" + ddlMonth.SelectedValue.ToString() + "' and Examyear='" + ddlYear.SelectedValue.ToString() + "'";
    //        delexam = dt.update_method_wo_parameter(deletholiday, "text");

    //        string strexamtimesetting = "select * from examtimetablesetting where mode='" + ddlexammode.SelectedItem.ToString() + "' order by day";
    //        ds.Dispose();
    //        ds.Reset();
    //        ds = dt.select_method_wo_parameter(strexamtimesetting, "text");

    //        string strexmpriority = "select * from exampriority order by mode,education desc,year,priority";
    //        DataSet dspriority = dt.select_method_wo_parameter(strexmpriority, "Text");

    //        string strcoursesubjectdetails = "select distinct c.type,c.Edu_Level,c.Course_Name,ss.subject_type,ss.ElectivePap,sy.semester,ep.priority from Exam_Details ed,exam_application ea,exam_appl_details ead,course c,Degree d,sub_sem ss,subject s,exampriority ep,syllabus_master sy ";
    //        strcoursesubjectdetails = strcoursesubjectdetails + " where  ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and s.subType_no=ss.subType_no and s.subject_no=ead.subject_no and ss.promote_count=1 and sy.syll_code=ss.syll_code and ss.subject_type=ep.subject_type and ep.education=c.Edu_Level and ep.mode = c.type";
    //        strcoursesubjectdetails = strcoursesubjectdetails + " and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "'";
    //        DataSet dscoursesub = dt.select_method_wo_parameter(strcoursesubjectdetails, "text");

    //        string strallsubdetails = "select distinct c.type,c.Edu_Level,c.Course_Name,ed.batch_year,ed.degree_code,ed.current_semester,sy.semester,ss.subject_type,s.subject_code,s.subject_name,s.subject_no from Exam_Details ed,exam_application ea,exam_appl_details ead,course c,Degree d,sub_sem ss,subject s,exampriority ep,syllabus_master sy ";
    //        strallsubdetails = strallsubdetails + " where  ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and s.subType_no=ss.subType_no and s.subject_no=ead.subject_no and ss.promote_count=1 and sy.syll_code=ss.syll_code and ss.subject_type=ep.subject_type and ep.education=c.Edu_Level";
    //        strallsubdetails = strallsubdetails + " and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' order by c.type,c.Edu_Level desc,c.Course_Name,sy.semester,ss.subject_type,s.subject_code,ed.batch_year desc,ed.degree_code";
    //        DataSet dsallsubd = dt.select_method_wo_parameter(strallsubdetails, "text");

    //        string electivesubject = "select Equal_Subject_Code ,Com_Subject_Code from tbl_equal_paper_Matching where Com_Subject_Code in(select Com_Subject_Code from tbl_equal_paper_Matching  where Equal_Subject_Code in(select subject_code from subject))";
    //        DataSet dselective = dt.select_method_wo_parameter(electivesubject, "Text");

    //        string strgetcheckroll = "select s.subject_code,ea.roll_no from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s where  ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' ";
    //        DataSet dscheckrpll = dt.select_method_wo_parameter(strgetcheckroll, "text");

    //        //string strgetbatchdegree = "select distinct ed.batch_year,ed.degree_code,ed.current_semester,s.subject_code,s.subject_name,s.subject_no from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s where  ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' ";

    //        string strgetbatchdegree = "select distinct c.type,c.Edu_Level,c.Course_Name,ed.batch_year,ed.degree_code,ed.current_semester,sy.semester,ss.subject_type,s.subject_code,s.subject_name,s.subject_no from Exam_Details ed,exam_application ea,exam_appl_details ead,course c,Degree d,sub_sem ss,subject s,exampriority ep,syllabus_master sy ";
    //        strgetbatchdegree = strgetbatchdegree + " where  ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and s.subType_no=ss.subType_no and s.subject_no=ead.subject_no and ss.promote_count=1 and sy.syll_code=ss.syll_code and ss.subject_type=ep.subject_type and ep.education=c.Edu_Level";
    //        strgetbatchdegree = strgetbatchdegree + " and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' order by c.type,c.Edu_Level desc,c.Course_Name,sy.semester,ss.subject_type,ed.batch_year desc,ed.degree_code";
    //        DataSet dsbatchdegree = dt.select_method_wo_parameter(strgetbatchdegree, "text");


    //        Hashtable hatdateyer = new Hashtable();
    //        //===========================Date Loop========================================
    //        for (DateTime dtt = dtfrom; dtt <= dtto; dtt = dtt.AddDays(1))
    //        {
    //            int flag = 0;
    //            if (cblHolidays.Items[p].Selected == true)//Holidates Check
    //            {
    //                string holy = cblHolidays.Items[p].Text;
    //                string[] holl = holy.Split('-');
    //                string holid = holl[1].ToString() + "-" + holl[0].ToString() + "-" + holl[2].ToString();
    //                string holi = "if exists(select * from examholiday where exammonth='" + ddlMonth.SelectedValue + "' and examyear='" + ddlYear.SelectedItem.Text + "' and holiday_date='" + holid + "' and startdate='" + dtfrom.ToString("MM-dd-yyyy") + "' and enddate='" + dtto.ToString("MM-dd-yyyy") + "' and college_code='" + Session["collegecode"] + "')delete from examholiday where exammonth='" + ddlMonth.SelectedValue + "' and examyear='" + ddlYear.SelectedItem.Text + "' and holiday_date='" + holid + "' and startdate='" + dtfrom.ToString("MM-dd-yyyy") + "' and enddate='" + dtto.ToString("MM-dd-yyyy") + "' and college_code='" + Session["collegecode"] + "' insert into examholiday(exammonth,examyear,startdate,enddate,holiday_date,college_code) values('" + ddlMonth.SelectedValue + "','" + ddlYear.SelectedItem.Text + "','" + dtfrom.ToString("MM-dd-yyyy") + "','" + dtto.ToString("MM-dd-yyyy") + "','" + holid + "','" + Session["collegecode"] + "')";
    //                int h = dt.update_method_wo_parameter(holi, "text");
    //                p++;
    //            }
    //            else
    //            {
    //                p++;
    //                count++;
    //                string day = "DAY" + " " + count;
    //                DataView dv1 = new DataView();
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    ds.Tables[0].DefaultView.RowFilter = "day='" + day + "'";//Exam day Filter
    //                    dv1 = ds.Tables[0].DefaultView;
    //                    if (dv1.Count > 0)
    //                    {


    //                        for (int l = 0; l < 2; l++)
    //                        {
    //                            Hashtable hatcoursepriority = new Hashtable();
    //                            for (int cty = 0; cty < dv1.Count; cty++)//Type Spilt Loop
    //                            {
    //                                string strtype = dv1[cty]["type"].ToString();
    //                                string strmodoeva = "";
    //                                if (strtype != "" && strtype != null)
    //                                {
    //                                    strmodoeva = "and mode='" + strtype + "' ";
    //                                }

    //                                Hashtable hatallarrear = new Hashtable();

    //                                if (l == 0)
    //                                {
    //                                    flag = 1;
    //                                    session = "F.N";
    //                                    fnan = dv1[cty]["FN"].ToString().Split(',');
    //                                    startdate = ddltheoryStartTimeamHrs.SelectedItem.ToString() + ":" + ddltheoryStartTimeamMin.SelectedItem.ToString() + "AM";
    //                                    enddate = ddltheoryendtimeamHrs.SelectedItem.ToString() + ":" + ddltheoryendtimeamMin.SelectedItem.ToString() + "" + ddltheoryendtimeampm.SelectedItem.ToString();

    //                                }
    //                                else if (l == 1)
    //                                {
    //                                    flag = 1;
    //                                    session = "A.N";
    //                                    fnan = dv1[cty]["AN"].ToString().Split(',');

    //                                    startdate = ddlTheoryStartTimeHrsPm.SelectedItem.ToString() + ":" + ddlTheoryStartTimeMinPm.SelectedItem.ToString() + "PM";
    //                                    enddate = ddlTheoryEndTimeHrsPm.SelectedItem.ToString() + ":" + ddlTheoryEndTimeMinPm.SelectedItem.ToString() + "PM";
    //                                }

    //                                for (int c = 0; c <= fnan.GetUpperBound(0); c++)
    //                                {
    //                                    string stredu = fnan[c].ToString();
    //                                    if (stredu.Trim() != "")
    //                                    {
    //                                        string[] spyesub = stredu.Split('/');
    //                                        if (spyesub.GetUpperBound(0) > 0)
    //                                        {
    //                                            string year = spyesub[0].ToString();
    //                                            string course = spyesub[1].ToString();
    //                                            int sem = 0;
    //                                            if (year == "1 Year")
    //                                            {
    //                                                sem = 1;
    //                                            }
    //                                            else if (year == "2 Year")
    //                                            {
    //                                                sem = 3;
    //                                            }
    //                                            else if (year == "3 Year")
    //                                            {
    //                                                sem = 5;
    //                                            }
    //                                            else if (year == "4 Year")
    //                                            {
    //                                                sem = 7;
    //                                            }
    //                                            else if (year == "5 Year")
    //                                            {
    //                                                sem = 9;
    //                                            }
    //                                            if (ddlexammode.SelectedItem.ToString() == "EVEN")
    //                                            {
    //                                                sem++;
    //                                            }

    //                                            //==============================Getting subjecttype for Course and Type=====================
    //                                            dspriority.Tables[0].DefaultView.RowFilter = "year='" + year + "' and Mode='" + strtype + "' and education='" + course + "'";
    //                                            DataView dvpriority = dspriority.Tables[0].DefaultView;
    //                                            for (int pri = 0; pri < dvpriority.Count; pri++)
    //                                            {
    //                                                string subjectype = dvpriority[pri]["subject_type"].ToString();
    //                                                string coursename = "";
    //                                                string elective = "";

    //                                                dscoursesub.Tables[0].DefaultView.RowFilter = "type='" + strtype + "' and Edu_Level='" + course + "' and subject_type='" + subjectype + "' and semester='" + sem + "'";
    //                                                DataView dvcoursesub = dscoursesub.Tables[0].DefaultView;
    //                                                if (dvcoursesub.Count > 0)
    //                                                {
    //                                                    for (int cs = 0; cs < dvcoursesub.Count; cs++)
    //                                                    {
    //                                                        coursename = dvcoursesub[cs]["Course_Name"].ToString();
    //                                                        elective = dvcoursesub[cs]["ElectivePap"].ToString();

    //                                                        Boolean allsubflag = false;
    //                                                        if (elective.Trim().ToLower() == "true" || elective.Trim() == "1")
    //                                                        {
    //                                                            allsubflag = true;
    //                                                        }

    //                                                        if (!hatcoursepriority.Contains(strtype + '-' + course + '-' + year + '-' + coursename))
    //                                                        {
    //                                                            Hashtable hatbatchdegree = new Hashtable();
    //                                                            dsallsubd.Tables[0].DefaultView.RowFilter = "type='" + strtype + "' and Edu_Level='" + course + "' and subject_type='" + subjectype + "' and Course_Name='" + coursename + "'  and semester='" + sem + "'";
    //                                                            DataView dvallsubd = dsallsubd.Tables[0].DefaultView;
    //                                                            if (dvallsubd.Count > 0)
    //                                                            {
    //                                                                for (int sp = 0; sp < dvallsubd.Count; sp++)
    //                                                                {
    //                                                                    string missbatch = dvallsubd[sp]["Batch_year"].ToString();
    //                                                                    string missdegree = dvallsubd[sp]["degree_code"].ToString();
    //                                                                    string misssem = dvallsubd[sp]["current_semester"].ToString();
    //                                                                    string missssubno = dvallsubd[sp]["subject_no"].ToString();
    //                                                                    string missubcode = dvallsubd[sp]["subject_code"].ToString();

    //                                                                    string ha = missssubno + "-" + missdegree;
    //                                                                    //Non Elective PaperMove

    //                                                                    if (allsubflag == false)
    //                                                                    {
    //                                                                        if (!hatbatchdegree.Contains(missbatch + '-' + missdegree ))
    //                                                                        {
    //                                                                            string esubjectget = "";
    //                                                                            string commonsubcode = "";
    //                                                                            dselective.Tables[0].DefaultView.RowFilter= " Equal_Subject_Code='" + missubcode + "'";
    //                                                                            DataView dvclecode = dselective.Tables[0].DefaultView;
    //                                                                            if (dvclecode.Count > 0)
    //                                                                            {
    //                                                                                commonsubcode = dvclecode[0]["Com_Subject_Code"].ToString();
    //                                                                            }
    //                                                                            if (commonsubcode.Trim() != "")
    //                                                                            {
    //                                                                                dselective.Tables[0].DefaultView.RowFilter = " Com_Subject_Code='" + commonsubcode + "'";
    //                                                                                DataView dvelesub = dselective.Tables[0].DefaultView;
    //                                                                                for (int esc = 0; esc < dvelesub.Count; esc++)
    //                                                                                {
    //                                                                                    if (esubjectget == "")
    //                                                                                    {
    //                                                                                        esubjectget = "'" + dvelesub[esc]["Equal_Subject_Code"].ToString() + "'";
    //                                                                                    }
    //                                                                                    else
    //                                                                                    {
    //                                                                                        esubjectget = esubjectget + ",'" + dvelesub[esc]["Equal_Subject_Code"].ToString() + "'";
    //                                                                                    }

    //                                                                                }
    //                                                                            }
    //                                                                            if (esubjectget.Trim() == "")
    //                                                                            {
    //                                                                                esubjectget = "'" + missubcode + "'";
    //                                                                            }
    //                                                                            esubjectget = " subject_code in(" + esubjectget + ")";

    //                                                                            dscheckrpll.Tables[0].DefaultView.RowFilter = esubjectget;
    //                                                                            DataView dvcheckrol = dscheckrpll.Tables[0].DefaultView;
    //                                                                            string binroll = "";
    //                                                                            for (int subr = 0; subr < dvcheckrol.Count; subr++)
    //                                                                            {
    //                                                                                if (binroll == "")
    //                                                                                {
    //                                                                                    binroll = "'" + dvcheckrol[subr]["roll_no"].ToString() + "'";
    //                                                                                }
    //                                                                                else
    //                                                                                {
    //                                                                                    binroll = binroll + ",'" + dvcheckrol[subr]["roll_no"].ToString() + "'";
    //                                                                                }
    //                                                                            }

    //                                                                            string checkroll = "select isnull(count(ea.roll_no),'0') from Exam_Details ed,exam_application ea,exam_appl_details ead,exmtt e,exmtt_det et where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and e.exam_code=et.exam_code and ead.subject_no=et.subject_no and ed.Exam_Month=e.Exam_month and ed.Exam_year=e.Exam_year and ed.degree_code=e.degree_code and ed.batch_year=e.batchFrom and ed.current_semester=e.Semester and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and et.exam_date='" + dtt.ToString("MM/dd/yyyy") + "' and et.exam_session='" + session + "' and ea.roll_no in ( " + binroll + ")";
    //                                                                            int getsubjectstucount = Convert.ToInt32(dt.GetFunction(checkroll));
    //                                                                            if (getsubjectstucount == 0)
    //                                                                            {
    //                                                                                dsbatchdegree.Tables[0].DefaultView.RowFilter = esubjectget;
    //                                                                                DataView dvbatchdegreesubject = dsbatchdegree.Tables[0].DefaultView;
    //                                                                                for (int ev = 0; ev < dvbatchdegreesubject.Count; ev++)
    //                                                                                {
    //                                                                                    string evbatch = dvbatchdegreesubject[ev]["batch_year"].ToString();
    //                                                                                    string evdegree = dvbatchdegreesubject[ev]["degree_code"].ToString();
    //                                                                                    string evsem = dvbatchdegreesubject[ev]["current_semester"].ToString();
    //                                                                                    string evsubno = dvbatchdegreesubject[ev]["subject_no"].ToString();
    //                                                                                    ha = evsubno + "-" + evdegree;
    //                                                                                    if (!ht.ContainsKey(ha))
    //                                                                                    {
    //                                                                                        string exm = "if not exists(select * from exmtt where degree_code='" + evdegree.ToString() + "' and batchFrom='" + evbatch.ToString() + "' and Semester='" + evsem.ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ') insert into exmtt(degree_code,batchFrom,batchTo,Exam_month,Exam_year,exam_type,Semester) values('" + evdegree.ToString() + "','" + evbatch.ToString() + "','" + evbatch.ToString() + "','" + ddlMonth.SelectedValue + "','" + ddlYear.SelectedItem.Text + "','univ','" + evsem.ToString() + "') select * from exmtt where degree_code='" + evdegree.ToString() + "' and batchFrom='" + evbatch.ToString() + "' and Semester='" + evsem.ToString() + "'  and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ'";
    //                                                                                        int s = dt.update_method_wo_parameter(exm, "text");
    //                                                                                        DataSet ds2allarr = new DataSet();
    //                                                                                        ds2allarr = dt.select_method_wo_parameter(exm, "text");
    //                                                                                        if (ds2allarr.Tables[0].Rows.Count > 0)
    //                                                                                        {
    //                                                                                            string examcodealllarear = ds2allarr.Tables[0].Rows[0]["exam_code"].ToString();
    //                                                                                            flagg = 1;
    //                                                                                            string save = "if exists(select * from exmtt_det where subject_no='" + evsubno.ToString() + "' and exam_date='" + dtt + "'and exam_session='" + session + "' and exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + Session["collegecode"] + "' and exam_type='Univ')update exmtt_det set subject_no='" + evsubno.ToString() + "' , start_time='" + startdate + "' , end_time='" + enddate + "' , exam_date='" + dtt + "', exam_session='" + session + "' , exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' , coll_code='" + Session["collegecode"] + "' , exam_type='Univ' where subject_no='" + evsubno.ToString() + "' and exam_date='" + dtt + "'and exam_session='" + session + "' and exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + Session["collegecode"] + "' and exam_type='Univ' else insert into exmtt_det(subject_no, start_time, end_time, exam_date, exam_session, exam_code, coll_code, exam_type) values('" + evsubno.ToString() + "' ,'" + startdate + "' ,'" + enddate + "' ,'" + dtt + "','" + session + "' ,'" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' ,'" + Session["collegecode"] + "' ,'Univ')";
    //                                                                                            int v = dt.update_method_wo_parameter(save, "text");
    //                                                                                            if (!hatcoursepriority.Contains(strtype + '-' + course + '-' + year + '-' + coursename))
    //                                                                                            {
    //                                                                                                hatcoursepriority.Add(strtype + '-' + course + '-' + year + '-' + coursename, 1);
    //                                                                                                pri = dvpriority.Count + 2;
    //                                                                                            }
    //                                                                                            if (!hatbatchdegree.Contains(evbatch + '-' + evdegree))
    //                                                                                            {
    //                                                                                                hatbatchdegree.Add(evbatch + '-' + evdegree, evbatch + '-' + evdegree);
    //                                                                                            }
    //                                                                                        }
    //                                                                                        ht.Add(ha, ha);
    //                                                                                    }
    //                                                                                }
    //                                                                            }
    //                                                                        }
    //                                                                    }
    //                                                                    else
    //                                                                    {
    //                                                                        if (!ht.ContainsKey(ha))
    //                                                                        {
    //                                                                            string exm = "if not exists(select * from exmtt where degree_code='" + missdegree.ToString() + "' and batchFrom='" + missbatch.ToString() + "' and Semester='" + misssem.ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ') insert into exmtt(degree_code,batchFrom,batchTo,Exam_month,Exam_year,exam_type,Semester) values('" + missdegree.ToString() + "','" + missbatch.ToString() + "','" + missbatch.ToString() + "','" + ddlMonth.SelectedValue + "','" + ddlYear.SelectedItem.Text + "','univ','" + misssem.ToString() + "') select * from exmtt where degree_code='" + missdegree.ToString() + "' and batchFrom='" + missbatch.ToString() + "' and Semester='" + misssem.ToString() + "'  and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ'";
    //                                                                            int s = dt.update_method_wo_parameter(exm, "text");
    //                                                                            DataSet ds2allarr = new DataSet();
    //                                                                            ds2allarr = dt.select_method_wo_parameter(exm, "text");
    //                                                                            if (ds2allarr.Tables[0].Rows.Count > 0)
    //                                                                            {
    //                                                                                string examcodealllarear = ds2allarr.Tables[0].Rows[0]["exam_code"].ToString();
    //                                                                                flagg = 1;
    //                                                                                string save = "if exists(select * from exmtt_det where subject_no='" + missssubno.ToString() + "' and exam_date='" + dtt + "'and exam_session='" + session + "' and exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + Session["collegecode"] + "' and exam_type='Univ')update exmtt_det set subject_no='" + missssubno.ToString() + "' , start_time='" + startdate + "' , end_time='" + enddate + "' , exam_date='" + dtt + "', exam_session='" + session + "' , exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' , coll_code='" + Session["collegecode"] + "' , exam_type='Univ' where subject_no='" + missssubno.ToString() + "' and exam_date='" + dtt + "'and exam_session='" + session + "' and exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + Session["collegecode"] + "' and exam_type='Univ' else insert into exmtt_det(subject_no, start_time, end_time, exam_date, exam_session, exam_code, coll_code, exam_type) values('" + missssubno.ToString() + "' ,'" + startdate + "' ,'" + enddate + "' ,'" + dtt + "','" + session + "' ,'" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' ,'" + Session["collegecode"] + "' ,'Univ')";
    //                                                                                int v = dt.update_method_wo_parameter(save, "text");
    //                                                                            }
    //                                                                            ht.Add(ha, ha);
    //                                                                            if (!hatcoursepriority.Contains(strtype + '-' + course + '-' + year + '-' + coursename))
    //                                                                            {
    //                                                                                hatcoursepriority.Add(strtype + '-' + course + '-' + year + '-' + coursename, 1);
    //                                                                                hatbatchdegree.Add(missbatch + '-' + missdegree + '-' + misssem, missbatch + '-' + missdegree + '-' + misssem);
    //                                                                                pri = dvpriority.Count + 2;
    //                                                                            }
    //                                                                        }
    //                                                                    }
    //                                                                }
    //                                                            }
    //                                                        }
    //                                                    }
    //                                                }
    //                                            }
    //                                        }
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        //======================Missing Subject Move==========================

    //        //int allowedtsrgeh=1200;
    //        //string noofallow = txtnoofstudent.Text.ToString();
    //        //if (noofallow.Trim() != "")
    //        //{
    //        //    allowedtsrgeh = Convert.ToInt32(noofallow);
    //        //}

    //        //Hashtable hatstrfiled = new Hashtable();

    //        //string strmissingsubjcerget = "select distinct s.subject_no,ed.batch_year,ed.degree_code,ed.current_semester,sy.semester,s.subject_name,s.subType_no,ss.subject_type,s.subject_code,ep.priority,ss.ElectivePap,ead.attempts,c.Edu_Level,c.Course_Name,de.Dept_Name ";
    //        //strmissingsubjcerget = strmissingsubjcerget + " from Exam_Details ed,exam_application ea,exam_appl_details ead,course c,Degree d,sub_sem ss,subject s,exampriority ep,syllabus_master sy,Department de where  ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and de.Dept_Code=d.Dept_Code and s.subType_no=ss.subType_no and s.subject_no=ead.subject_no and ss.promote_count=1 ";
    //        //strmissingsubjcerget = strmissingsubjcerget + " and sy.syll_code=ss.syll_code and ss.subject_type=ep.subject_type and ep.education=c.Edu_Level and ep.mode = c.type and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "'  and s.subject_no not in (select et.subject_no from exmtt e,exmtt_det et where e.exam_code=et.exam_code  and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' )";
    //        //strmissingsubjcerget = strmissingsubjcerget + " order by s.subject_code";
    //        //DataSet dsmissingsubjectmove = dt.select_method_wo_parameter(strmissingsubjcerget, "Text");

    //        //string getnoallroll = "select distinct ead.subject_no,ea.roll_no  from Exam_Details ed,exam_application ea,exam_appl_details ead where  ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "'  ";
    //        //getnoallroll = getnoallroll + " and ead.subject_no not in (select et.subject_no from exmtt e,exmtt_det et where e.exam_code=et.exam_code  and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' )  order by ead.subject_no";
    //        //DataSet dsnotroll = dt.select_method_wo_parameter(getnoallroll, "Text");

    //        //for (int msalte = 0; msalte < dsmissingsubjectmove.Tables[0].Rows.Count; msalte++)
    //        //{
    //        //    //===================Check And Move existing date
    //        //    Boolean flagsetfal = false;
    //        //    string missbatch = dsmissingsubjectmove.Tables[0].Rows[msalte]["batch_year"].ToString();
    //        //    string missdegree = dsmissingsubjectmove.Tables[0].Rows[msalte]["degree_code"].ToString();
    //        //    string misssem = dsmissingsubjectmove.Tables[0].Rows[msalte]["current_semester"].ToString();
    //        //    string msubjectcode = dsmissingsubjectmove.Tables[0].Rows[msalte]["subject_code"].ToString();
    //        //    string missssubno = dsmissingsubjectmove.Tables[0].Rows[msalte]["subject_no"].ToString();

    //        //    string getequlacode = "select Equal_Subject_Code as subjectcode from tbl_equal_paper_Matching where Com_Subject_Code in(select Com_Subject_Code from tbl_equal_paper_Matching  where Equal_Subject_Code='" + msubjectcode + "')";
    //        //    DataSet dsequlcheck = dt.select_method_wo_parameter(getequlacode, "Text");
    //        //    string esubjectget = "";
    //        //    for (int esc = 0; esc < dsequlcheck.Tables[0].Rows.Count; esc++)
    //        //    {
    //        //        if (esubjectget == "")
    //        //        {
    //        //            esubjectget = "'" + dsequlcheck.Tables[0].Rows[esc]["subjectcode"].ToString() + "'";
    //        //        }
    //        //        else
    //        //        {
    //        //            esubjectget = esubjectget + ",'" + dsequlcheck.Tables[0].Rows[esc]["subjectcode"].ToString() + "'";
    //        //        }

    //        //    }
    //        //    if (esubjectget.Trim() == "")
    //        //    {
    //        //        esubjectget = "'" + msubjectcode + "'";
    //        //    }
    //        //    esubjectget = " and s.subject_code in(" + esubjectget + ")";
    //        //    string getexamdatequery = "select distinct et.exam_date,et.exam_session from exmtt e,exmtt_det et,subject s where e.exam_code=et.exam_code and et.subject_no=s.subject_no and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' " + esubjectget + "";
    //        //    DataSet dsgetexmdetaisl = dt.select_method_wo_parameter(getexamdatequery, "Text");
    //        //    if (dsgetexmdetaisl.Tables[0].Rows.Count > 0)
    //        //    {
    //        //        DateTime dtsetexdate = Convert.ToDateTime(dsgetexmdetaisl.Tables[0].Rows[0]["exam_date"].ToString());
    //        //        string sessione = dsgetexmdetaisl.Tables[0].Rows[0]["exam_session"].ToString();
    //        //        if (sessione.Trim().ToLower() == "f.n")
    //        //        {
    //        //            session = "F.N";
    //        //            startdate = ddltheoryStartTimeamHrs.SelectedItem.ToString() + ":" + ddltheoryStartTimeamMin.SelectedItem.ToString() + "AM";
    //        //            enddate = ddltheoryendtimeamHrs.SelectedItem.ToString() + ":" + ddltheoryendtimeamMin.SelectedItem.ToString() + "" + ddltheoryendtimeampm.SelectedItem.ToString();

    //        //        }
    //        //        else
    //        //        {
    //        //            session = "A.N";
    //        //            startdate = ddlTheoryStartTimeHrsPm.SelectedItem.ToString() + ":" + ddlTheoryStartTimeMinPm.SelectedItem.ToString() + "PM";
    //        //            enddate = ddlTheoryEndTimeHrsPm.SelectedItem.ToString() + ":" + ddlTheoryEndTimeMinPm.SelectedItem.ToString() + "PM";

    //        //        }
    //        //        string exm = "if not exists(select * from exmtt where degree_code='" + missdegree.ToString() + "' and batchFrom='" + missbatch.ToString() + "' and Semester='" + misssem.ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ') insert into exmtt(degree_code,batchFrom,batchTo,Exam_month,Exam_year,exam_type,Semester) values('" + missdegree.ToString() + "','" + missbatch.ToString() + "','" + missbatch.ToString() + "','" + ddlMonth.SelectedValue + "','" + ddlYear.SelectedItem.Text + "','univ','" + misssem.ToString() + "') select * from exmtt where degree_code='" + missdegree.ToString() + "' and batchFrom='" + missbatch.ToString() + "' and Semester='" + misssem.ToString() + "'  and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ'";
    //        //        int s = dt.update_method_wo_parameter(exm, "text");
    //        //        DataSet ds2allarr = new DataSet();
    //        //        ds2allarr = dt.select_method_wo_parameter(exm, "text");
    //        //        if (ds2allarr.Tables[0].Rows.Count > 0)
    //        //        {
    //        //            flagsetfal = true;
    //        //            string examcodealllarear = ds2allarr.Tables[0].Rows[0]["exam_code"].ToString();
    //        //            flagg = 1;
    //        //            string save = "if exists(select * from exmtt_det where subject_no='" + missssubno.ToString() + "' and exam_date='" + dtsetexdate + "'and exam_session='" + session + "' and exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + Session["collegecode"] + "' and exam_type='Univ')update exmtt_det set subject_no='" + missssubno.ToString() + "' , start_time='" + startdate + "' , end_time='" + enddate + "' , exam_date='" + dtsetexdate + "', exam_session='" + session + "' , exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' , coll_code='" + Session["collegecode"] + "' , exam_type='Univ' where subject_no='" + missssubno.ToString() + "' and exam_date='" + dtsetexdate + "'and exam_session='" + session + "' and exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + Session["collegecode"] + "' and exam_type='Univ' else insert into exmtt_det(subject_no, start_time, end_time, exam_date, exam_session, exam_code, coll_code, exam_type) values('" + missssubno.ToString() + "' ,'" + startdate + "' ,'" + enddate + "' ,'" + dtsetexdate + "','" + session + "' ,'" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' ,'" + Session["collegecode"] + "' ,'Univ')";
    //        //            int v = dt.update_method_wo_parameter(save, "text");
    //        //        }
    //        //    }


    //        //    //Date Loop
    //        //    p = 0;
    //        //    if (flagsetfal == false)
    //        //    {
    //        //        DateTime dttfa = dtfrom.AddDays(2);
    //        //        for (DateTime dtt = dttfa; dtt <= dtto; dtt = dtt.AddDays(1))
    //        //        {
    //        //            if (cblHolidays.Items[p].Selected == false)//Holidates Check
    //        //            {
    //        //                for (int se = 0; se < 2; se++)
    //        //                {
    //        //                    string sesval = "F.N";
    //        //                    if (se > 0)
    //        //                    {
    //        //                        sesval = "A.N";
    //        //                    }
    //        //                    if (!hatstrfiled.Contains(dtt.ToString() + '-' + sesval))
    //        //                    {
    //        //                        string strgetdetails = "select et.exam_date,et.exam_session,isnull(count(ead.subject_no),'0') as stucount from exmtt e,exmtt_det et,subject s,exam_appl_details ead where e.exam_code=et.exam_code and et.subject_no=s.subject_no and ead.subject_no=et.subject_no and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "'  and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and et.exam_date='" + dtt.ToString() + "' and et.exam_session='" + sesval + "'  and ead.appl_no in(select ea.appl_no from Exam_Details ed,exam_application ea where ed.exam_code=ea.exam_code and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year=" + ddlYear.SelectedValue.ToString() + ") group by et.exam_date,et.exam_session";
    //        //                        DataSet dsstucoun = dt.select_method_wo_parameter(strgetdetails, "Text");
    //        //                        if (dsstucoun.Tables[0].Rows.Count > 0)
    //        //                        {
    //        //                            int countval = Convert.ToInt32(dsstucoun.Tables[0].Rows[0]["stucount"].ToString());
    //        //                            if (countval < allowedtsrgeh)
    //        //                            {
    //        //                                string binroll = "";
    //        //                                dsnotroll.Tables[0].DefaultView.RowFilter = "Subject_no='" + missssubno + "'";
    //        //                                DataView dvcheckrol = dsnotroll.Tables[0].DefaultView;
    //        //                                for (int subr = 0; subr < dvcheckrol.Count; subr++)
    //        //                                {
    //        //                                    if (binroll == "")
    //        //                                    {
    //        //                                        binroll = "'" + dvcheckrol[subr]["roll_no"].ToString() + "'";
    //        //                                    }
    //        //                                    else
    //        //                                    {
    //        //                                        binroll = binroll + ",'" + dvcheckrol[subr]["roll_no"].ToString() + "'";
    //        //                                    }
    //        //                                }

    //        //                                string checkroll = "select isnull(count(ea.roll_no),'0') from Exam_Details ed,exam_application ea,exam_appl_details ead,exmtt e,exmtt_det et where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and e.exam_code=et.exam_code and ead.subject_no=et.subject_no and ed.Exam_Month=e.Exam_month and ed.Exam_year=e.Exam_year and ed.degree_code=e.degree_code and ed.batch_year=e.batchFrom and ed.current_semester=e.Semester and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and et.exam_date='" + dtt.ToString("MM/dd/yyyy") + "' and et.exam_session='" + session + "' and ea.roll_no in ( " + binroll + ")";
    //        //                                int getsubjectstucount = Convert.ToInt32(dt.GetFunction(checkroll));
    //        //                                if (getsubjectstucount == 0)
    //        //                                {
    //        //                                    DateTime dtsetexdate = dtt;
    //        //                                    string sessione = dsstucoun.Tables[0].Rows[0]["exam_session"].ToString();
    //        //                                    if (sessione.Trim().ToLower() == "f.n")
    //        //                                    {
    //        //                                        session = "F.N";
    //        //                                        startdate = ddltheoryStartTimeamHrs.SelectedItem.ToString() + ":" + ddltheoryStartTimeamMin.SelectedItem.ToString() + "AM";
    //        //                                        enddate = ddltheoryendtimeamHrs.SelectedItem.ToString() + ":" + ddltheoryendtimeamMin.SelectedItem.ToString() + "" + ddltheoryendtimeampm.SelectedItem.ToString();

    //        //                                    }
    //        //                                    else
    //        //                                    {
    //        //                                        session = "A.N";
    //        //                                        startdate = ddlTheoryStartTimeHrsPm.SelectedItem.ToString() + ":" + ddlTheoryStartTimeMinPm.SelectedItem.ToString() + "PM";
    //        //                                        enddate = ddlTheoryEndTimeHrsPm.SelectedItem.ToString() + ":" + ddlTheoryEndTimeMinPm.SelectedItem.ToString() + "PM";

    //        //                                    }
    //        //                                    string exm = "if not exists(select * from exmtt where degree_code='" + missdegree.ToString() + "' and batchFrom='" + missbatch.ToString() + "' and Semester='" + misssem.ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ') insert into exmtt(degree_code,batchFrom,batchTo,Exam_month,Exam_year,exam_type,Semester) values('" + missdegree.ToString() + "','" + missbatch.ToString() + "','" + missbatch.ToString() + "','" + ddlMonth.SelectedValue + "','" + ddlYear.SelectedItem.Text + "','univ','" + misssem.ToString() + "') select * from exmtt where degree_code='" + missdegree.ToString() + "' and batchFrom='" + missbatch.ToString() + "' and Semester='" + misssem.ToString() + "'  and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and exam_type='univ'";
    //        //                                    int s = dt.update_method_wo_parameter(exm, "text");
    //        //                                    DataSet ds2allarr = new DataSet();
    //        //                                    ds2allarr = dt.select_method_wo_parameter(exm, "text");
    //        //                                    if (ds2allarr.Tables[0].Rows.Count > 0)
    //        //                                    {
    //        //                                        string examcodealllarear = ds2allarr.Tables[0].Rows[0]["exam_code"].ToString();
    //        //                                        flagg = 1;
    //        //                                        string save = "if exists(select * from exmtt_det where subject_no='" + missssubno.ToString() + "' and exam_date='" + dtsetexdate + "'and exam_session='" + session + "' and exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + Session["collegecode"] + "' and exam_type='Univ')update exmtt_det set subject_no='" + missssubno.ToString() + "' , start_time='" + startdate + "' , end_time='" + enddate + "' , exam_date='" + dtsetexdate + "', exam_session='" + session + "' , exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' , coll_code='" + Session["collegecode"] + "' , exam_type='Univ' where subject_no='" + missssubno.ToString() + "' and exam_date='" + dtsetexdate + "'and exam_session='" + session + "' and exam_code='" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' and coll_code='" + Session["collegecode"] + "' and exam_type='Univ' else insert into exmtt_det(subject_no, start_time, end_time, exam_date, exam_session, exam_code, coll_code, exam_type) values('" + missssubno.ToString() + "' ,'" + startdate + "' ,'" + enddate + "' ,'" + dtsetexdate + "','" + session + "' ,'" + ds2allarr.Tables[0].Rows[0]["exam_code"].ToString() + "' ,'" + Session["collegecode"] + "' ,'Univ')";
    //        //                                        int v = dt.update_method_wo_parameter(save, "text");
    //        //                                    }
    //        //                                    se = 5;
    //        //                                    dtt = dtto.AddDays(5);
    //        //                                }
    //        //                            }
    //        //                            else
    //        //                            {
    //        //                                hatstrfiled.Add(dtt.ToString() + '-' + sesval, countval);
    //        //                            }
    //        //                        }
    //        //                    }
    //        //                }
    //        //            }
    //        //        }
    //        //    }
    //        //}
    //        //======================Missing Subject Move End==========================
    //        if (flagg == 1)
    //        {
    //            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
    //        }
    //        else if (flagg == 0)
    //        {
    //            if (subflag == true)
    //            {
    //                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Allot The Subject And Then Proceed ')", true);
    //            }
    //            else
    //            {
    //                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Set Proper Date and Settings ')", true);
    //            }
    //        }

    //        //dsequalsubject.Clear();
    //        //dsequalsubject = null;
    //        //ds.Clear();
    //        //ds = null;
    //        //ds4.Clear();
    //        //ds4 = null;
    //        ////dsarresub.Clear();
    //        ////dsarresub = null;
    //        ////dscurrsub.Clear();
    //        ////dscurrsub = null;
    //        ////dselesubtype.Clear();
    //        ////dselesubtype = null;
    //        //dsequalgetsubject.Clear();
    //        //dsequalgetsubject = null;

    //        //hatelesubtype.Clear();
    //        //hatelesubtype = null;
    //        ht.Clear();
    //        ht = null;
    //    }
    //    catch (Exception ex)
    //    {
    //        errmsg.Visible = true;
    //        errmsg.Text = ex.ToString();
    //    }
    //}

    //protected void btnView_Click(object sender, EventArgs e)
    //{
    //    //    Connection();
    //    pnlvisible.Visible = true;
    //    //    btnGenerate.Visible = true;
    //    //    pnlSession.Visible = true;
    //    //    pnlHolidays.Visible = true;
    //    //    Fpstudents.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
    //    //    Fpstudents.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
    //    //    Fpstudents.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
    //    //    Fpstudents.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
    //    //    Fpstudents.Sheets[0].DefaultStyle.Font.Bold = false;
    //    //    Fpstudents.CommandBar.Visible = false;
    //    //    Fpstudents.RowHeader.Visible = false;
    //    //    FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();


    //    //    Fpstudents.Sheets[0].FrozenRowCount = 1;
    //    //    Fpstudents.Sheets[0].ColumnHeader.RowCount = 2;
    //    //    Fpstudents.Sheets[0].ColumnCount = 10;
    //    //    Fpstudents.Width = 920;
    //    //    Fpstudents.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
    //    //    Fpstudents.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
    //    //    Fpstudents.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
    //    //    Fpstudents.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
    //    //    Fpstudents.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
    //    //    Fpstudents.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
    //    //    Fpstudents.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);
    //    //    Fpstudents.Sheets[0].Columns[0].Width = 50;
    //    //    Fpstudents.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
    //    //    Fpstudents.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
    //    //    Fpstudents.Sheets[0].Columns[1].Width = 70;
    //    //    Fpstudents.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Year";
    //    //    Fpstudents.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Degree";
    //    //    Fpstudents.Sheets[0].Columns[3].Width = 70;
    //    //    Fpstudents.Sheets[0].Columns[2].Width = 60;
    //    //    Fpstudents.Sheets[0].Columns[4].Width = 70;
    //    //    Fpstudents.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
    //    //    Fpstudents.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Course";
    //    //    Fpstudents.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Semester";
    //    //    Fpstudents.Sheets[0].Columns[5].Width = 320;
    //    //    Fpstudents.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Subject";
    //    //    Fpstudents.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 1, 3);
    //    //    Fpstudents.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Students";
    //    //    Fpstudents.Sheets[0].Columns[7].Width = 70;
    //    //    Fpstudents.Sheets[0].ColumnHeader.Cells[1, 6].Text = "Regular";
    //    //    Fpstudents.Sheets[0].ColumnHeader.Cells[1, 7].Text = "Arear";
    //    //    Fpstudents.Sheets[0].Columns[8].Width = 70;
    //    //    Fpstudents.Sheets[0].ColumnHeader.Cells[1, 8].Text = "Total";
    //    //    Fpstudents.Sheets[0].Columns[9].Width = 50;
    //    //    Fpstudents.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Select";
    //    //    Fpstudents.Sheets[0].Columns[9].CellType = chk;
    //    //    Fpstudents.Sheets[0].Cells[0, 9].CellType = chk1;
    //    //    chk.AutoPostBack = false;
    //    //    Fpstudents.Sheets[0].DefaultRowHeight = 24;
    //    //    Fpstudents.Sheets[0].AddSpanCell(0, 0, 1, 9);

    //    //    Fpstudents.Sheets[0].Columns[0].Locked = true;
    //    //    Fpstudents.Sheets[0].Columns[1].Locked = true;
    //    //    Fpstudents.Sheets[0].Columns[2].Locked = true;
    //    //    Fpstudents.Sheets[0].Columns[3].Locked = true;
    //    //    Fpstudents.Sheets[0].Columns[4].Locked = true;
    //    //    Fpstudents.Sheets[0].Columns[5].Locked = true;
    //    //    Fpstudents.Sheets[0].Columns[6].Locked = true;
    //    //    Fpstudents.Sheets[0].Columns[7].Locked = true;
    //    //    Fpstudents.Sheets[0].Columns[8].Locked = true;


    //    //    Fpstudents.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
    //    //    Fpstudents.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
    //    //    Fpstudents.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
    //    //    Fpstudents.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
    //    //    Fpstudents.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
    //    //    Fpstudents.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
    //    //    Fpstudents.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);


    //    //   studentspread();
    //    //    SqlCommand cmd = new SqlCommand("ProcExamTimeTableGeneratedSubjects", con);
    //    //    cmd.CommandType = CommandType.StoredProcedure;
    //    //    SqlDataAdapter da = new SqlDataAdapter(cmd);
    //    //    DataSet ds = new DataSet();
    //    //    da.Fill(ds);
    //    //    if (ds.Tables[0].Rows.Count > 0)
    //    //    {
    //    //        for (int j = 1; j < Fpstudents.Sheets[0].RowCount; j++)
    //    //        {
    //    //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //    //              {
    //    //                  if (Fpstudents.Sheets[0].Cells[j, 5].Note.ToString() == ds.Tables[0].Rows[i]["SubjectNo"].ToString())
    //    //                  {
    //    //                      Fpstudents.Sheets[0].Rows[j].ForeColor = Color.YellowGreen;

    //    //                  }
    //    //              }


    //    //        }

    //    //    }

    //    //    ddltheoryendtimeamHrs.Items[0].Selected = true;
    //    //    ddltheoryendtimeamMin.Items[0].Selected = true;
    //    //    ddlTheoryEndTimeHrsPm.Items[0].Selected = true;
    //    //    ddlTheoryEndTimeMinPm.Items[0].Selected = true;
    //    //    ddltheoryStartTimeamHrs.Items[0].Selected = true;
    //    //    ddlTheoryStartTimeHrsPm.Items[0].Selected = true;
    //    //    ddlTheoryStartTimeMinPm.Items[0].Selected = true;
    //    //    ddltheoryStartTimeamMin.Items[0].Selected = true;
    //    //    ddlpracendtimeamHrs.Items[0].Selected = true;
    //    //    ddlpracendtimeamMin.Items[0].Selected = true;
    //    //    ddlpracendtimePmHrs.Items[0].Selected = true;
    //    //    ddlpracendtimePmMin.Items[0].Selected = true;
    //    //    ddlpracstarttimeamHrs.Items[0].Selected = true;
    //    //    ddlpracstarttimeamMin.Items[0].Selected = true;
    //    //    ddlpracstarttimePmHrs.Items[0].Selected = true;
    //    //    ddlpracstarttimePmMin.Items[0].Selected = true;

    //    //    txtSession.Text = "";
    //    //    txtTheoryDurationam.Text = "";
    //    //    txtTheoryDurationpm.Text = "";
    //    //    txtpracdurationam.Text = "";
    //    //    txtpracdurationpm.Text = "";
    //    //    Fpstudents.Visible = true;

    //    //    if (Convert.ToInt16(ddlExmType.SelectedValue) == 0)
    //    //    {
    //    //        ddltheoryendtimeamHrs.Enabled = true;
    //    //        ddltheoryendtimeamMin.Enabled = true;
    //    //        ddlTheoryEndTimeHrsPm.Enabled = true;
    //    //        ddlTheoryEndTimeMinPm.Enabled = true;
    //    //        ddltheoryStartTimeamHrs.Enabled = true;
    //    //        ddlTheoryStartTimeHrsPm.Enabled = true;
    //    //        ddlTheoryStartTimeMinPm.Enabled = true;
    //    //        ddltheoryStartTimeamMin.Enabled = true;
    //    //        ddlpracendtimeamHrs.Enabled = true;
    //    //        ddlpracendtimeamMin.Enabled = true;
    //    //        ddlpracendtimePmHrs.Enabled = true;
    //    //        ddlpracendtimePmMin.Enabled = true;
    //    //        ddlpracstarttimeamHrs.Enabled = true;
    //    //        ddlpracstarttimeamMin.Enabled = true;
    //    //        ddlpracstarttimePmHrs.Enabled = true;
    //    //        ddlpracstarttimePmMin.Enabled = true;
    //    //    }
    //    //    else if (Convert.ToInt16(ddlExmType.SelectedValue) == 1)
    //    //    {
    //    //        ddltheoryendtimeamHrs.Enabled = true;
    //    //        ddltheoryendtimeamMin.Enabled = true;
    //    //        ddlTheoryEndTimeHrsPm.Enabled = true;
    //    //        ddlTheoryEndTimeMinPm.Enabled = true;
    //    //        ddltheoryStartTimeamHrs.Enabled = true;
    //    //        ddlTheoryStartTimeHrsPm.Enabled = true;
    //    //        ddlTheoryStartTimeMinPm.Enabled = true;
    //    //        ddltheoryStartTimeamMin.Enabled = true;
    //    //        ddltheoryendtimeampm.Enabled = true;
    //    //        ddlpracendtimeAmPm.Enabled = false;
    //    //        ddlpracendtimeamHrs.Enabled = false;
    //    //        ddlpracendtimeamMin.Enabled = false;
    //    //        ddlpracendtimePmHrs.Enabled = false;
    //    //        ddlpracendtimePmMin.Enabled = false;
    //    //        ddlpracstarttimeamHrs.Enabled = false;
    //    //        ddlpracstarttimeamMin.Enabled = false;
    //    //        ddlpracstarttimePmHrs.Enabled = false;
    //    //        ddlpracstarttimePmMin.Enabled = false;

    //    //    }
    //    //    else if (Convert.ToInt16(ddlExmType.SelectedValue) == 2)
    //    //    {
    //    //        ddltheoryendtimeamHrs.Enabled = false;
    //    //        ddltheoryendtimeamMin.Enabled = false;
    //    //        ddlTheoryEndTimeHrsPm.Enabled = false;
    //    //        ddlTheoryEndTimeMinPm.Enabled = false;
    //    //        ddltheoryStartTimeamHrs.Enabled = false;
    //    //        ddlTheoryStartTimeHrsPm.Enabled = false;
    //    //        ddlTheoryStartTimeMinPm.Enabled = false;
    //    //        ddltheoryStartTimeamMin.Enabled = false;
    //    //        ddltheoryendtimeampm.Enabled = false;
    //    //        ddlpracendtimeAmPm.Enabled = true;
    //    //        ddlpracendtimeamHrs.Enabled = true;
    //    //        ddlpracendtimeamMin.Enabled = true;
    //    //        ddlpracendtimePmHrs.Enabled = true;
    //    //        ddlpracendtimePmMin.Enabled = true;
    //    //        ddlpracstarttimeamHrs.Enabled = true;
    //    //        ddlpracstarttimeamMin.Enabled = true;
    //    //        ddlpracstarttimePmHrs.Enabled = true;
    //    //        ddlpracstarttimePmMin.Enabled = true;

    //    //    }

    //}

    //protected void clSession_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    int count = 0;
    //    string sess = "";
    //    string[] s;
    //    txtSession.Text = "";
    //    for (int k = 0; k < clSession.Items.Count; k++)
    //    {
    //        if (clSession.Items[k].Selected == true)
    //        {
    //            count = count + 1;
    //            if (sess == "")
    //            {
    //                sess = clSession.Items[k].Text.ToString();
    //            }
    //            else
    //            {

    //                sess = sess + "=" + clSession.Items[k].Text.ToString();

    //            }
    //        }

    //    }
    //    txtSession.Text = "(" + Convert.ToString(count) + ")   Selected";
    //    s = sess.Split(new char[] { '=' });
    //    int t = 0;
    //    if (count > 1)
    //    {

    //        if (s[t].ToString() == "AM-Regular" && s[t + 1].ToString() == "PM-Regular")
    //        {

    //            lblerror.Text = "Select AM-Regular [OR] PM-Regular";
    //            lblerror.Visible = true;

    //        }
    //        else if (s[t + 1].ToString() == "AM-Regular" && s[t].ToString() == "PM-Regular")
    //        {

    //            lblerror.Text = "Select AM-Regular [OR] PM-Regular";
    //            lblerror.Visible = true;
    //        }
    //        else if (s[t].ToString() == "AM-Arear" && s[t + 1].ToString() == "PM-Arear")
    //        {

    //            lblerror.Text = "Select AM-Arear [OR] PM-Arear";
    //            lblerror.Visible = true;

    //        }
    //        else if (s[t + 1].ToString() == "AM-Arear" && s[t].ToString() == "PM-Arear")
    //        {

    //            lblerror.Text = "Select AM-Arear [OR] PM-Arear";
    //            lblerror.Visible = true;
    //        }
    //        else

    //            if (count == 1 && clSession.Items[2].Selected == false && ddlExamType.SelectedItem.Text.ToString() == "All" && ddlExmType.SelectedItem.Text.ToString() == "All")
    //            {
    //                lblerror.Text = "Select The Session Correctly";
    //                lblerror.Visible = true;
    //                txtSession.Focus();
    //            }
    //            else if (count == 2 && clSession.Items[2].Selected == true && ddlExamType.SelectedItem.Text.ToString() == "All" && ddlExmType.SelectedItem.Text.ToString() == "All")
    //            {
    //                lblerror.Text = "Select The Session Correctly";
    //                lblerror.Visible = true;
    //                txtSession.Focus();
    //            }
    //            else if (count > 2 && ddlExamType.SelectedItem.Text.ToString() == "All" && ddlExmType.SelectedItem.Text.ToString() == "All")
    //            {
    //                lblerror.Text = "Select The Session Correctly";
    //                lblerror.Visible = true;
    //                txtSession.Focus();
    //            }


    //    }
    //    else
    //    {

    //        lblerror.Visible = false;
    //    }
    //    if (count > 0)
    //    {

    //        if (ddlExamType.SelectedItem.Text.ToString() != "All" && ddlExmType.SelectedItem.Text.ToString() != "All")
    //        {

    //            if (ddlExamStartType.SelectedItem.Text.ToString() == "Theory" && clSession.SelectedItem.Text.ToString() == "AM-Regular")
    //            {
    //                ddltheoryendtimeamHrs.Enabled = true;
    //                ddltheoryStartTimeamHrs.Enabled = true;
    //                ddltheoryendtimeamMin.Enabled = true;
    //                ddltheoryStartTimeamMin.Enabled = true;
    //                ddltheoryendtimeampm.Enabled = true;
    //                ddlTheoryEndTimeHrsPm.Enabled = false;
    //                ddlTheoryEndTimeMinPm.Enabled = false;
    //                ddlTheoryStartTimeHrsPm.Enabled = false;
    //                ddlTheoryStartTimeMinPm.Enabled = false;
    //            }
    //            else if (ddlExamStartType.SelectedItem.Text.ToString() == "Theory" && clSession.SelectedItem.Text.ToString() == "AM-Arear")
    //            {
    //                ddltheoryendtimeamHrs.Enabled = true;
    //                ddltheoryStartTimeamHrs.Enabled = true;
    //                ddltheoryendtimeamMin.Enabled = true;
    //                ddltheoryStartTimeamMin.Enabled = true;
    //                ddltheoryendtimeampm.Enabled = true;
    //                ddlTheoryEndTimeHrsPm.Enabled = false;
    //                ddlTheoryEndTimeMinPm.Enabled = false;
    //                ddlTheoryStartTimeHrsPm.Enabled = false;
    //                ddlTheoryStartTimeMinPm.Enabled = false;
    //            }
    //            else if (ddlExamStartType.SelectedItem.Text.ToString() == "Theory" && clSession.SelectedItem.Text.ToString() == "PM-Regular")
    //            {
    //                ddltheoryendtimeamHrs.Enabled = false;
    //                ddltheoryStartTimeamHrs.Enabled = false;
    //                ddltheoryendtimeamMin.Enabled = false;
    //                ddltheoryStartTimeamMin.Enabled = false;
    //                ddltheoryendtimeampm.Enabled = false;

    //                ddlTheoryEndTimeHrsPm.Enabled = true;
    //                ddlTheoryEndTimeMinPm.Enabled = true;
    //                ddlTheoryStartTimeHrsPm.Enabled = true;
    //                ddlTheoryStartTimeMinPm.Enabled = true;
    //            }
    //            else if (ddlExamStartType.SelectedItem.Text.ToString() == "Theory" && clSession.SelectedItem.Text.ToString() == "PM-Arear")
    //            {
    //                ddltheoryendtimeamHrs.Enabled = false;
    //                ddltheoryStartTimeamHrs.Enabled = false;
    //                ddltheoryendtimeamMin.Enabled = false;
    //                ddltheoryStartTimeamMin.Enabled = false;
    //                ddltheoryendtimeampm.Enabled = false;

    //                ddlTheoryEndTimeHrsPm.Enabled = true;
    //                ddlTheoryEndTimeMinPm.Enabled = true;
    //                ddlTheoryStartTimeHrsPm.Enabled = true;
    //                ddlTheoryStartTimeMinPm.Enabled = true;
    //            }
    //            else if (ddlExamStartType.SelectedItem.Text.ToString() == "Practical" && clSession.SelectedItem.Text.ToString() == "AM-Regular")
    //            {
    //                ddlpracendtimeAmPm.Enabled = true;
    //                ddlpracendtimeamHrs.Enabled = true;
    //                ddlpracendtimeamMin.Enabled = true;
    //                ddlpracstarttimeamHrs.Enabled = true;
    //                ddlpracstarttimeamMin.Enabled = true;
    //                ddlpracstarttimePmHrs.Enabled = false;
    //                ddlpracstarttimePmMin.Enabled = false;
    //                ddlpracendtimePmHrs.Enabled = false;
    //                ddlpracendtimePmMin.Enabled = false;
    //            }
    //            else if (ddlExamStartType.SelectedItem.Text.ToString() == "Practical" && clSession.SelectedItem.Text.ToString() == "AM-Arear")
    //            {
    //                ddlpracendtimeAmPm.Enabled = true;
    //                ddlpracendtimeamHrs.Enabled = true;
    //                ddlpracendtimeamMin.Enabled = true;
    //                ddlpracstarttimeamHrs.Enabled = true;
    //                ddlpracstarttimeamMin.Enabled = true;
    //                ddlpracstarttimePmHrs.Enabled = false;
    //                ddlpracstarttimePmMin.Enabled = false;
    //                ddlpracendtimePmHrs.Enabled = false;
    //                ddlpracendtimePmMin.Enabled = false;
    //            }
    //            else if (ddlExamStartType.SelectedItem.Text.ToString() == "Practical" && clSession.SelectedItem.Text.ToString() == "PM-Regular")
    //            {
    //                ddlpracendtimeAmPm.Enabled = false;
    //                ddlpracendtimeamHrs.Enabled = false;
    //                ddlpracendtimeamMin.Enabled = false;
    //                ddlpracstarttimeamHrs.Enabled = false;
    //                ddlpracstarttimeamMin.Enabled = false;
    //                ddlpracstarttimePmHrs.Enabled = true;
    //                ddlpracstarttimePmMin.Enabled = true;
    //                ddlpracendtimePmHrs.Enabled = true;
    //                ddlpracendtimePmMin.Enabled = true;
    //            }
    //            else if (ddlExamStartType.SelectedItem.Text.ToString() == "Practical" && clSession.SelectedItem.Text.ToString() == "PM-Arear")
    //            {
    //                ddlpracendtimeAmPm.Enabled = false;
    //                ddlpracendtimeamHrs.Enabled = false;
    //                ddlpracendtimeamMin.Enabled = false;
    //                ddlpracstarttimeamHrs.Enabled = false;
    //                ddlpracstarttimeamMin.Enabled = false;
    //                ddlpracstarttimePmHrs.Enabled = true;
    //                ddlpracstarttimePmMin.Enabled = true;
    //                ddlpracendtimePmHrs.Enabled = true;
    //                ddlpracendtimePmMin.Enabled = true;
    //            }
    //        }
    //        else if (ddlExamType.SelectedItem.Text.ToString() == "All" && ddlExmType.SelectedItem.Text.ToString() != "All")
    //        {
    //            if (ddlExamStartType.SelectedItem.Text.ToString() == "Theory" && clSession.SelectedItem.Text.ToString() == "AM-Regular")
    //            {
    //                ddltheoryendtimeamHrs.Enabled = true;
    //                ddltheoryStartTimeamHrs.Enabled = true;
    //                ddltheoryendtimeamMin.Enabled = true;
    //                ddltheoryStartTimeamMin.Enabled = true;
    //                ddltheoryendtimeampm.Enabled = true;
    //                ddlTheoryEndTimeHrsPm.Enabled = false;
    //                ddlTheoryEndTimeMinPm.Enabled = false;
    //                ddlTheoryStartTimeHrsPm.Enabled = false;
    //                ddlTheoryStartTimeMinPm.Enabled = false;
    //            }
    //            else if (ddlExamStartType.SelectedItem.Text.ToString() == "Theory" && clSession.SelectedItem.Text.ToString() == "AM-Arear")
    //            {
    //                ddltheoryendtimeamHrs.Enabled = true;
    //                ddltheoryStartTimeamHrs.Enabled = true;
    //                ddltheoryendtimeamMin.Enabled = true;
    //                ddltheoryStartTimeamMin.Enabled = true;
    //                ddltheoryendtimeampm.Enabled = true;
    //                ddlTheoryEndTimeHrsPm.Enabled = false;
    //                ddlTheoryEndTimeMinPm.Enabled = false;
    //                ddlTheoryStartTimeHrsPm.Enabled = false;
    //                ddlTheoryStartTimeMinPm.Enabled = false;
    //            }
    //            else if (ddlExamStartType.SelectedItem.Text.ToString() == "Theory" && clSession.SelectedItem.Text.ToString() == "PM-Regular")
    //            {
    //                ddltheoryendtimeamHrs.Enabled = false;
    //                ddltheoryStartTimeamHrs.Enabled = false;
    //                ddltheoryendtimeamMin.Enabled = false;
    //                ddltheoryStartTimeamMin.Enabled = false;
    //                ddltheoryendtimeampm.Enabled = false;

    //                ddlTheoryEndTimeHrsPm.Enabled = true;
    //                ddlTheoryEndTimeMinPm.Enabled = true;
    //                ddlTheoryStartTimeHrsPm.Enabled = true;
    //                ddlTheoryStartTimeMinPm.Enabled = true;
    //            }
    //            else if (ddlExamStartType.SelectedItem.Text.ToString() == "Theory" && clSession.SelectedItem.Text.ToString() == "PM-Arear")
    //            {
    //                ddltheoryendtimeamHrs.Enabled = false;
    //                ddltheoryStartTimeamHrs.Enabled = false;
    //                ddltheoryendtimeamMin.Enabled = false;
    //                ddltheoryStartTimeamMin.Enabled = false;
    //                ddltheoryendtimeampm.Enabled = false;

    //                ddlTheoryEndTimeHrsPm.Enabled = true;
    //                ddlTheoryEndTimeMinPm.Enabled = true;
    //                ddlTheoryStartTimeHrsPm.Enabled = true;
    //                ddlTheoryStartTimeMinPm.Enabled = true;
    //            }
    //            else if (ddlExamStartType.SelectedItem.Text.ToString() == "Practical" && clSession.SelectedItem.Text.ToString() == "AM-Regular")
    //            {
    //                ddlpracendtimeAmPm.Enabled = true;
    //                ddlpracendtimeamHrs.Enabled = true;
    //                ddlpracendtimeamMin.Enabled = true;
    //                ddlpracstarttimeamHrs.Enabled = true;
    //                ddlpracstarttimeamMin.Enabled = true;
    //                ddlpracstarttimePmHrs.Enabled = false;
    //                ddlpracstarttimePmMin.Enabled = false;
    //                ddlpracendtimePmHrs.Enabled = false;
    //                ddlpracendtimePmMin.Enabled = false;
    //            }
    //            else if (ddlExamStartType.SelectedItem.Text.ToString() == "Practical" && clSession.SelectedItem.Text.ToString() == "AM-Arear")
    //            {
    //                ddlpracendtimeAmPm.Enabled = true;
    //                ddlpracendtimeamHrs.Enabled = true;
    //                ddlpracendtimeamMin.Enabled = true;
    //                ddlpracstarttimeamHrs.Enabled = true;
    //                ddlpracstarttimeamMin.Enabled = true;
    //                ddlpracstarttimePmHrs.Enabled = false;
    //                ddlpracstarttimePmMin.Enabled = false;
    //                ddlpracendtimePmHrs.Enabled = false;
    //                ddlpracendtimePmMin.Enabled = false;
    //            }
    //            else if (ddlExamStartType.SelectedItem.Text.ToString() == "Practical" && clSession.SelectedItem.Text.ToString() == "PM-Regular")
    //            {
    //                ddlpracendtimeAmPm.Enabled = false;
    //                ddlpracendtimeamHrs.Enabled = false;
    //                ddlpracendtimeamMin.Enabled = false;
    //                ddlpracstarttimeamHrs.Enabled = false;
    //                ddlpracstarttimeamMin.Enabled = false;
    //                ddlpracstarttimePmHrs.Enabled = true;
    //                ddlpracstarttimePmMin.Enabled = true;
    //                ddlpracendtimePmHrs.Enabled = true;
    //                ddlpracendtimePmMin.Enabled = true;
    //            }
    //            else if (ddlExamStartType.SelectedItem.Text.ToString() == "Practical" && clSession.SelectedItem.Text.ToString() == "PM-Arear")
    //            {
    //                ddlpracendtimeAmPm.Enabled = false;
    //                ddlpracendtimeamHrs.Enabled = false;
    //                ddlpracendtimeamMin.Enabled = false;
    //                ddlpracstarttimeamHrs.Enabled = false;
    //                ddlpracstarttimeamMin.Enabled = false;
    //                ddlpracstarttimePmHrs.Enabled = true;
    //                ddlpracstarttimePmMin.Enabled = true;
    //                ddlpracendtimePmHrs.Enabled = true;
    //                ddlpracendtimePmMin.Enabled = true;
    //            }
    //            if (count > 1)
    //            {
    //                if (s[t].ToString() == "AM-Regular" && s[t + 1].ToString() == "AM-Arear" && ddlExmType.SelectedItem.Text.ToString() == "Theory")
    //                {
    //                    ddltheoryendtimeamHrs.Enabled = true;
    //                    ddltheoryStartTimeamHrs.Enabled = true;
    //                    ddltheoryendtimeamMin.Enabled = true;
    //                    ddltheoryStartTimeamMin.Enabled = true;
    //                    ddltheoryendtimeampm.Enabled = true;
    //                    ddlTheoryEndTimeHrsPm.Enabled = false;
    //                    ddlTheoryEndTimeMinPm.Enabled = false;
    //                    ddlTheoryStartTimeHrsPm.Enabled = false;
    //                    ddlTheoryStartTimeMinPm.Enabled = false;

    //                }
    //                else if (s[t].ToString() == "PM-Regular" && s[t + 1].ToString() == "PM-Arear" && ddlExmType.SelectedItem.Text.ToString() == "Theory")
    //                {
    //                    ddltheoryendtimeamHrs.Enabled = false;
    //                    ddltheoryStartTimeamHrs.Enabled = false;
    //                    ddltheoryendtimeamMin.Enabled = false;
    //                    ddltheoryStartTimeamMin.Enabled = false;
    //                    ddltheoryendtimeampm.Enabled = false;
    //                    ddlTheoryEndTimeHrsPm.Enabled = true;
    //                    ddlTheoryEndTimeMinPm.Enabled = true;
    //                    ddlTheoryStartTimeHrsPm.Enabled = true;
    //                    ddlTheoryStartTimeMinPm.Enabled = true;

    //                }
    //                else if (s[t].ToString() == "AM-Regular" && s[t + 1].ToString() == "AM-Arear" && ddlExmType.SelectedItem.Text.ToString() == "Practical")
    //                {

    //                    ddlpracendtimeAmPm.Enabled = true;
    //                    ddlpracendtimeamHrs.Enabled = true;
    //                    ddlpracendtimeamMin.Enabled = true;
    //                    ddlpracstarttimeamHrs.Enabled = true;
    //                    ddlpracstarttimeamMin.Enabled = true;
    //                    ddlpracstarttimePmHrs.Enabled = false;
    //                    ddlpracstarttimePmMin.Enabled = false;
    //                    ddlpracendtimePmHrs.Enabled = false;
    //                    ddlpracendtimePmMin.Enabled = false;

    //                }
    //                else if (s[t].ToString() == "PM-Regular" && s[t + 1].ToString() == "PM-Arear" && ddlExmType.SelectedItem.Text.ToString() == "Practical")
    //                {
    //                    ddlpracendtimeAmPm.Enabled = false;
    //                    ddlpracendtimeamHrs.Enabled = false;
    //                    ddlpracendtimeamMin.Enabled = false;
    //                    ddlpracstarttimeamHrs.Enabled = false;
    //                    ddlpracstarttimeamMin.Enabled = false;
    //                    ddlpracstarttimePmHrs.Enabled = true;
    //                    ddlpracstarttimePmMin.Enabled = true;
    //                    ddlpracendtimePmHrs.Enabled = true;
    //                    ddlpracendtimePmMin.Enabled = true;

    //                }
    //                if (s[t].ToString() == "AM-Regular" && s[t + 1].ToString() == "PM-Arear" && ddlExmType.SelectedItem.Text.ToString() == "Theory")
    //                {
    //                    ddltheoryendtimeamHrs.Enabled = true;
    //                    ddltheoryStartTimeamHrs.Enabled = true;
    //                    ddltheoryendtimeamMin.Enabled = true;
    //                    ddltheoryStartTimeamMin.Enabled = true;
    //                    ddltheoryendtimeampm.Enabled = true;
    //                    ddlTheoryEndTimeHrsPm.Enabled = true;
    //                    ddlTheoryEndTimeMinPm.Enabled = true;
    //                    ddlTheoryStartTimeHrsPm.Enabled = true;
    //                    ddlTheoryStartTimeMinPm.Enabled = true;

    //                }
    //                else if (s[t].ToString() == "PM-Regular" && s[t + 1].ToString() == "AM-Arear" && ddlExmType.SelectedItem.Text.ToString() == "Theory")
    //                {
    //                    ddltheoryendtimeamHrs.Enabled = true;
    //                    ddltheoryStartTimeamHrs.Enabled = true;
    //                    ddltheoryendtimeamMin.Enabled = true;
    //                    ddltheoryStartTimeamMin.Enabled = true;
    //                    ddltheoryendtimeampm.Enabled = true;
    //                    ddlTheoryEndTimeHrsPm.Enabled = true;
    //                    ddlTheoryEndTimeMinPm.Enabled = true;
    //                    ddlTheoryStartTimeHrsPm.Enabled = true;
    //                    ddlTheoryStartTimeMinPm.Enabled = true;

    //                }
    //                else if (s[t].ToString() == "AM-Regular" && s[t + 1].ToString() == "PM-Arear" && ddlExmType.SelectedItem.Text.ToString() == "Practical")
    //                {

    //                    ddlpracendtimeAmPm.Enabled = true;
    //                    ddlpracendtimeamHrs.Enabled = true;
    //                    ddlpracendtimeamMin.Enabled = true;
    //                    ddlpracstarttimeamHrs.Enabled = true;
    //                    ddlpracstarttimeamMin.Enabled = true;
    //                    ddlpracstarttimePmHrs.Enabled = true;
    //                    ddlpracstarttimePmMin.Enabled = true;
    //                    ddlpracendtimePmHrs.Enabled = true;
    //                    ddlpracendtimePmMin.Enabled = true;

    //                }
    //                else if (s[t].ToString() == "PM-Regular" && s[t + 1].ToString() == "AM-Arear" && ddlExmType.SelectedItem.Text.ToString() == "Practical")
    //                {
    //                    ddlpracendtimeAmPm.Enabled = true;
    //                    ddlpracendtimeamHrs.Enabled = true;
    //                    ddlpracendtimeamMin.Enabled = true;
    //                    ddlpracstarttimeamHrs.Enabled = true;
    //                    ddlpracstarttimeamMin.Enabled = true;
    //                    ddlpracstarttimePmHrs.Enabled = true;
    //                    ddlpracstarttimePmMin.Enabled = true;
    //                    ddlpracendtimePmHrs.Enabled = true;
    //                    ddlpracendtimePmMin.Enabled = true;

    //                }

    //            }

    //        }

    //        else if (ddlExamType.SelectedItem.Text.ToString() == "All" && ddlExmType.SelectedItem.Text.ToString() == "All")
    //        {

    //            ddlpracendtimeAmPm.Enabled = true;
    //            ddlpracendtimeamHrs.Enabled = true;
    //            ddlpracendtimeamMin.Enabled = true;
    //            ddlpracstarttimeamHrs.Enabled = true;
    //            ddlpracstarttimeamMin.Enabled = true;
    //            ddlpracstarttimePmHrs.Enabled = true;
    //            ddlpracstarttimePmMin.Enabled = true;
    //            ddlpracendtimePmHrs.Enabled = true;
    //            ddlpracendtimePmMin.Enabled = true;

    //            ddltheoryendtimeamHrs.Enabled = true;
    //            ddltheoryStartTimeamHrs.Enabled = true;
    //            ddltheoryendtimeamMin.Enabled = true;
    //            ddltheoryStartTimeamMin.Enabled = true;
    //            ddltheoryendtimeampm.Enabled = true;
    //            ddlTheoryEndTimeHrsPm.Enabled = true;
    //            ddlTheoryEndTimeMinPm.Enabled = true;
    //            ddlTheoryStartTimeHrsPm.Enabled = true;
    //            ddlTheoryStartTimeMinPm.Enabled = true;
    //        }
    //    }





    //    if (ddlExamType.SelectedItem.Text.ToString() == "Regular" && ddlExmType.SelectedItem.Text.ToString() == "All")
    //    {
    //        if (count > 0)
    //        {
    //            if (clSession.SelectedItem.Text.ToString() == "AM-Regular")
    //            {
    //                ddltheoryendtimeamHrs.Enabled = true;
    //                ddltheoryStartTimeamHrs.Enabled = true;
    //                ddltheoryendtimeamMin.Enabled = true;
    //                ddltheoryStartTimeamMin.Enabled = true;
    //                ddltheoryendtimeampm.Enabled = true;
    //                ddlTheoryEndTimeHrsPm.Enabled = false;
    //                ddlTheoryEndTimeMinPm.Enabled = false;
    //                ddlTheoryStartTimeHrsPm.Enabled = false;
    //                ddlTheoryStartTimeMinPm.Enabled = false;
    //                ddlpracendtimeAmPm.Enabled = true;
    //                ddlpracendtimeamHrs.Enabled = true;
    //                ddlpracendtimeamMin.Enabled = true;
    //                ddlpracstarttimeamHrs.Enabled = true;
    //                ddlpracstarttimeamMin.Enabled = true;
    //                ddlpracstarttimePmHrs.Enabled = false;
    //                ddlpracstarttimePmMin.Enabled = false;
    //                ddlpracendtimePmHrs.Enabled = false;
    //                ddlpracendtimePmMin.Enabled = false;

    //            }
    //            else if (clSession.SelectedItem.Text.ToString() == "PM-Regular")
    //            {


    //                ddltheoryendtimeamHrs.Enabled = false;
    //                ddltheoryStartTimeamHrs.Enabled = false;
    //                ddltheoryendtimeamMin.Enabled = false;
    //                ddltheoryStartTimeamMin.Enabled = false;
    //                ddltheoryendtimeampm.Enabled = false;
    //                ddlTheoryEndTimeHrsPm.Enabled = true;
    //                ddlTheoryEndTimeMinPm.Enabled = true;
    //                ddlTheoryStartTimeHrsPm.Enabled = true;
    //                ddlTheoryStartTimeMinPm.Enabled = true;
    //                ddlpracendtimeAmPm.Enabled = false;
    //                ddlpracendtimeamHrs.Enabled = false;
    //                ddlpracendtimeamMin.Enabled = false;
    //                ddlpracstarttimeamHrs.Enabled = false;
    //                ddlpracstarttimeamMin.Enabled = false;
    //                ddlpracstarttimePmHrs.Enabled = true;
    //                ddlpracstarttimePmMin.Enabled = true;
    //                ddlpracendtimePmHrs.Enabled = true;
    //                ddlpracendtimePmMin.Enabled = true;

    //            }
    //        }
    //        else if (count == 0)
    //        {

    //            ddltheoryendtimeamHrs.Enabled = true;
    //            ddltheoryStartTimeamHrs.Enabled = true;
    //            ddltheoryendtimeamMin.Enabled = true;
    //            ddltheoryStartTimeamMin.Enabled = true;
    //            ddltheoryendtimeampm.Enabled = true;
    //            ddlTheoryEndTimeHrsPm.Enabled = true;
    //            ddlTheoryEndTimeMinPm.Enabled = true;
    //            ddlTheoryStartTimeHrsPm.Enabled = true;
    //            ddlTheoryStartTimeMinPm.Enabled = true;
    //            ddlpracendtimeAmPm.Enabled = true;
    //            ddlpracendtimeamHrs.Enabled = true;
    //            ddlpracendtimeamMin.Enabled = true;
    //            ddlpracstarttimeamHrs.Enabled = true;
    //            ddlpracstarttimeamMin.Enabled = true;
    //            ddlpracstarttimePmHrs.Enabled = true;
    //            ddlpracstarttimePmMin.Enabled = true;
    //            ddlpracendtimePmHrs.Enabled = true;
    //            ddlpracendtimePmMin.Enabled = true;

    //            ddltheoryendtimeamHrs.Items[0].Selected = true;
    //            ddltheoryendtimeamMin.Items[0].Selected = true;
    //            ddlTheoryEndTimeHrsPm.Items[0].Selected = true;
    //            ddlTheoryEndTimeMinPm.Items[0].Selected = true;
    //            ddltheoryStartTimeamHrs.Items[0].Selected = true;
    //            ddlTheoryStartTimeHrsPm.Items[0].Selected = true;
    //            ddlTheoryStartTimeMinPm.Items[0].Selected = true;
    //            ddltheoryStartTimeamMin.Items[0].Selected = true;
    //            ddlpracendtimeamHrs.Items[0].Selected = true;
    //            ddlpracendtimeamMin.Items[0].Selected = true;
    //            ddlpracendtimePmHrs.Items[0].Selected = true;
    //            ddlpracendtimePmMin.Items[0].Selected = true;
    //            ddlpracstarttimeamHrs.Items[0].Selected = true;
    //            ddlpracstarttimeamMin.Items[0].Selected = true;
    //            ddlpracstarttimePmHrs.Items[0].Selected = true;
    //            ddlpracstarttimePmMin.Items[0].Selected = true;


    //        }
    //    }
    //    else if (ddlExamType.SelectedItem.Text.ToString() == "Arear" && ddlExmType.SelectedItem.Text.ToString() == "All")
    //    {
    //        if (count > 0)
    //        {
    //            if (clSession.SelectedItem.Text.ToString() == "AM-Arear")
    //            {
    //                ddltheoryendtimeamHrs.Enabled = true;
    //                ddltheoryStartTimeamHrs.Enabled = true;
    //                ddltheoryendtimeamMin.Enabled = true;
    //                ddltheoryStartTimeamMin.Enabled = true;
    //                ddltheoryendtimeampm.Enabled = true;
    //                ddlTheoryEndTimeHrsPm.Enabled = false;
    //                ddlTheoryEndTimeMinPm.Enabled = false;
    //                ddlTheoryStartTimeHrsPm.Enabled = false;
    //                ddlTheoryStartTimeMinPm.Enabled = false;
    //                ddlpracendtimeAmPm.Enabled = true;
    //                ddlpracendtimeamHrs.Enabled = true;
    //                ddlpracendtimeamMin.Enabled = true;
    //                ddlpracstarttimeamHrs.Enabled = true;
    //                ddlpracstarttimeamMin.Enabled = true;
    //                ddlpracstarttimePmHrs.Enabled = false;
    //                ddlpracstarttimePmMin.Enabled = false;
    //                ddlpracendtimePmHrs.Enabled = false;
    //                ddlpracendtimePmMin.Enabled = false;

    //            }
    //            else if (clSession.SelectedItem.Text.ToString() == "PM-Arear")
    //            {


    //                ddltheoryendtimeamHrs.Enabled = false;
    //                ddltheoryStartTimeamHrs.Enabled = false;
    //                ddltheoryendtimeamMin.Enabled = false;
    //                ddltheoryStartTimeamMin.Enabled = false;
    //                ddltheoryendtimeampm.Enabled = false;
    //                ddlTheoryEndTimeHrsPm.Enabled = true;
    //                ddlTheoryEndTimeMinPm.Enabled = true;
    //                ddlTheoryStartTimeHrsPm.Enabled = true;
    //                ddlTheoryStartTimeMinPm.Enabled = true;
    //                ddlpracendtimeAmPm.Enabled = false;
    //                ddlpracendtimeamHrs.Enabled = false;
    //                ddlpracendtimeamMin.Enabled = false;
    //                ddlpracstarttimeamHrs.Enabled = false;
    //                ddlpracstarttimeamMin.Enabled = false;
    //                ddlpracstarttimePmHrs.Enabled = true;
    //                ddlpracstarttimePmMin.Enabled = true;
    //                ddlpracendtimePmHrs.Enabled = true;
    //                ddlpracendtimePmMin.Enabled = true;

    //            }
    //        }

    //        else if (Convert.ToInt16(count) == 0)
    //        {
    //            ddltheoryendtimeamHrs.Enabled = true;
    //            ddltheoryStartTimeamHrs.Enabled = true;
    //            ddltheoryendtimeamMin.Enabled = true;
    //            ddltheoryStartTimeamMin.Enabled = true;
    //            ddltheoryendtimeampm.Enabled = true;
    //            ddlTheoryEndTimeHrsPm.Enabled = true;
    //            ddlTheoryEndTimeMinPm.Enabled = true;
    //            ddlTheoryStartTimeHrsPm.Enabled = true;
    //            ddlTheoryStartTimeMinPm.Enabled = true;
    //            ddlpracendtimeAmPm.Enabled = true;
    //            ddlpracendtimeamHrs.Enabled = true;
    //            ddlpracendtimeamMin.Enabled = true;
    //            ddlpracstarttimeamHrs.Enabled = true;
    //            ddlpracstarttimeamMin.Enabled = true;
    //            ddlpracstarttimePmHrs.Enabled = true;
    //            ddlpracstarttimePmMin.Enabled = true;
    //            ddlpracendtimePmHrs.Enabled = true;
    //            ddlpracendtimePmMin.Enabled = true;

    //            ddltheoryendtimeamHrs.Items[0].Selected = true;
    //            ddltheoryendtimeamMin.Items[0].Selected = true;
    //            ddlTheoryEndTimeHrsPm.Items[0].Selected = true;
    //            ddlTheoryEndTimeMinPm.Items[0].Selected = true;
    //            ddltheoryStartTimeamHrs.Items[0].Selected = true;
    //            ddlTheoryStartTimeHrsPm.Items[0].Selected = true;
    //            ddlTheoryStartTimeMinPm.Items[0].Selected = true;
    //            ddltheoryStartTimeamMin.Items[0].Selected = true;
    //            ddlpracendtimeamHrs.Items[0].Selected = true;
    //            ddlpracendtimeamMin.Items[0].Selected = true;
    //            ddlpracendtimePmHrs.Items[0].Selected = true;
    //            ddlpracendtimePmMin.Items[0].Selected = true;
    //            ddlpracstarttimeamHrs.Items[0].Selected = true;
    //            ddlpracstarttimeamMin.Items[0].Selected = true;
    //            ddlpracstarttimePmHrs.Items[0].Selected = true;
    //            ddlpracstarttimePmMin.Items[0].Selected = true;

    //        }

    //    }
    //    else if (ddlExamStartType.SelectedItem.Text.ToString() == "Practical" && Convert.ToInt16(count) == 0)
    //    {
    //        ddlpracendtimeAmPm.Enabled = true;
    //        ddlpracendtimeamHrs.Enabled = true;
    //        ddlpracendtimeamMin.Enabled = true;
    //        ddlpracstarttimeamHrs.Enabled = true;
    //        ddlpracstarttimeamMin.Enabled = true;
    //        ddlpracstarttimePmHrs.Enabled = true;
    //        ddlpracstarttimePmMin.Enabled = true;
    //        ddlpracendtimePmHrs.Enabled = true;
    //        ddlpracendtimePmMin.Enabled = true;
    //        ddltheoryendtimeamHrs.Items[0].Selected = true;
    //        ddltheoryendtimeamMin.Items[0].Selected = true;
    //        ddlTheoryEndTimeHrsPm.Items[0].Selected = true;
    //        ddlTheoryEndTimeMinPm.Items[0].Selected = true;
    //        ddltheoryStartTimeamHrs.Items[0].Selected = true;
    //        ddlTheoryStartTimeHrsPm.Items[0].Selected = true;
    //        ddlTheoryStartTimeMinPm.Items[0].Selected = true;
    //        ddltheoryStartTimeamMin.Items[0].Selected = true;
    //        ddlpracendtimeamHrs.Items[0].Selected = true;
    //        ddlpracendtimeamMin.Items[0].Selected = true;
    //        ddlpracendtimePmHrs.Items[0].Selected = true;
    //        ddlpracendtimePmMin.Items[0].Selected = true;
    //        ddlpracstarttimeamHrs.Items[0].Selected = true;
    //        ddlpracstarttimeamMin.Items[0].Selected = true;
    //        ddlpracstarttimePmHrs.Items[0].Selected = true;
    //        ddlpracstarttimePmMin.Items[0].Selected = true;

    //    }
    //    else if (ddlExamStartType.SelectedItem.Text.ToString() == "Theory" && Convert.ToInt16(count) == 0)
    //    {
    //        ddltheoryendtimeamHrs.Enabled = true;
    //        ddltheoryStartTimeamHrs.Enabled = true;
    //        ddltheoryendtimeamMin.Enabled = true;
    //        ddltheoryStartTimeamMin.Enabled = true;
    //        ddltheoryendtimeampm.Enabled = true;
    //        ddlTheoryEndTimeHrsPm.Enabled = true;
    //        ddlTheoryEndTimeMinPm.Enabled = true;
    //        ddlTheoryStartTimeHrsPm.Enabled = true;
    //        ddlTheoryStartTimeMinPm.Enabled = true;

    //        ddltheoryendtimeamHrs.Items[0].Selected = true;
    //        ddltheoryendtimeamMin.Items[0].Selected = true;
    //        ddlTheoryEndTimeHrsPm.Items[0].Selected = true;
    //        ddlTheoryEndTimeMinPm.Items[0].Selected = true;
    //        ddltheoryStartTimeamHrs.Items[0].Selected = true;
    //        ddlTheoryStartTimeHrsPm.Items[0].Selected = true;
    //        ddlTheoryStartTimeMinPm.Items[0].Selected = true;
    //        ddltheoryStartTimeamMin.Items[0].Selected = true;
    //        ddlpracendtimeamHrs.Items[0].Selected = true;
    //        ddlpracendtimeamMin.Items[0].Selected = true;
    //        ddlpracendtimePmHrs.Items[0].Selected = true;
    //        ddlpracendtimePmMin.Items[0].Selected = true;
    //        ddlpracstarttimeamHrs.Items[0].Selected = true;
    //        ddlpracstarttimeamMin.Items[0].Selected = true;
    //        ddlpracstarttimePmHrs.Items[0].Selected = true;
    //        ddlpracstarttimePmMin.Items[0].Selected = true;
    //    }
    //}

    //protected void Fpstudents_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    //{

    //    int count = 0;
    //    string actrow1;
    //    actrow1 = e.SheetView.ActiveRow.ToString();
    //    if (flag_true == false && actrow1 == "0")
    //    {
    //        for (int j = 1; j < Convert.ToInt16(Fpstudents.Sheets[0].RowCount); j++)
    //        {
    //            string actcol1 = e.SheetView.ActiveColumn.ToString();
    //            string seltext = e.EditValues[Convert.ToInt16(actcol1)].ToString();
    //            if (seltext == "True")
    //            {
    //                count = count + 1;
    //                if (seltext != "System.Object")
    //                    Fpstudents.Sheets[0].Cells[j, Convert.ToInt16(actcol1)].Text = seltext.ToString();

    //            }
    //            else
    //            {
    //                Fpstudents.Sheets[0].Cells[j, Convert.ToInt16(actcol1)].Text = seltext.ToString();


    //            }
    //        }
    //        flag_true = true;
    //    }
    //}

    //private void studentspread()
    // {
    //     int examType;
    //     int exmType;
    //     examType = Convert.ToInt16(ddlExamType.SelectedValue);
    //     exmType = Convert.ToInt16(ddlExmType.SelectedValue);
    //     CollegeCode = Session["CollegeCode"].ToString();
    //     if (Convert.ToInt16(ddlExamType.SelectedValue) == 0 && Convert.ToInt16(ddlExmType.SelectedValue) == 0)
    //     {

    //         ddlExamstartswith.Items.Clear();
    //         ddlExamstartswith.Items.Insert(0, new ListItem("Regular", "0"));
    //         ddlExamstartswith.Items.Insert(1, new ListItem("Arear", "1"));
    //         ddlExamstartswith.Items.Insert(2, new ListItem("Mixed", "2"));

    //         ddlExamStartType.Items.Clear();
    //         ddlExamStartType.Items.Insert(0, new ListItem("Theory", "0"));
    //         ddlExamStartType.Items.Insert(1, new ListItem("Practical", "1"));

    //         clSession.Items.Clear();
    //         clSession.Items.Insert(0, new ListItem("AM-Regular", "0"));
    //         clSession.Items.Insert(1, new ListItem("PM-Regular", "1"));
    //         clSession.Items.Insert(2, new ListItem("Mixed", "2"));
    //         clSession.Items.Insert(3, new ListItem("AM-Arear", "3"));
    //         clSession.Items.Insert(4, new ListItem("PM-Arear", "4"));

    //         SqlCommand cmd = new SqlCommand("ProcExamTimeTableSelectSubjectData", con);
    //         cmd.CommandType = CommandType.StoredProcedure;
    //         cmd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //         SqlDataAdapter da = new SqlDataAdapter(cmd);
    //         DataSet examds = new DataSet();
    //         da.Fill(examds);
    //         Fpstudents.Sheets[0].RowCount = 2;
    //         int count;
    //         int countp;
    //         int countA;
    //         int countAP;
    //         int Sno = 0;
    //         string Temp = "";
    //         string Arear = "0";
    //         string Regular = "0";
    //         if (examds.Tables[0].Rows.Count > 0)
    //         {
    //             for (int i = 0; i < examds.Tables[0].Rows.Count; i++)
    //             {

    //                 SqlCommand cd = new SqlCommand("ProcExamTimeTableSubjectCount", con);
    //                 cd.CommandType = CommandType.StoredProcedure;
    //                 cd.Parameters.AddWithValue("@DegreeCode", examds.Tables[0].Rows[i]["DegreeCode"].ToString());
    //                 cd.Parameters.AddWithValue("@BatchYear", examds.Tables[0].Rows[i]["BatchYear"].ToString());
    //                 cd.Parameters.AddWithValue("@Semester", examds.Tables[0].Rows[i]["Semester"].ToString());
    //                 cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                 cd.Parameters.AddWithValue("@ExamType", examType.ToString());
    //                 cd.Parameters.AddWithValue("@ExmType", exmType.ToString());
    //                 SqlDataAdapter da1 = new SqlDataAdapter(cd);
    //                 DataSet ds1 = new DataSet();
    //                 da1.Fill(ds1);
    //                 if (ds1.Tables[0].Rows.Count > 0)
    //                 {
    //                     for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
    //                     {
    //                         if (Temp != examds.Tables[0].Rows[i]["Department"].ToString())
    //                         {
    //                             Sno = Sno + 1;
    //                         }
    //                         count = Fpstudents.Sheets[0].RowCount - 1;
    //                         Fpstudents.Sheets[0].Cells[count, 0].Text = Sno.ToString();
    //                         Fpstudents.Sheets[0].Cells[count, 1].Text = examds.Tables[0].Rows[i]["Year"].ToString();
    //                         Fpstudents.Sheets[0].Cells[count, 1].Note = examds.Tables[0].Rows[i]["BatchYear"].ToString();
    //                         Fpstudents.Sheets[0].Cells[count, 2].Text = examds.Tables[0].Rows[i]["Course"].ToString();
    //                         Fpstudents.Sheets[0].Cells[count, 3].Text = examds.Tables[0].Rows[i]["Department"].ToString();
    //                         Fpstudents.Sheets[0].Cells[count, 3].Note = examds.Tables[0].Rows[i]["DegreeCode"].ToString();
    //                         Fpstudents.Sheets[0].Cells[count, 4].Text = examds.Tables[0].Rows[i]["Semester"].ToString();
    //                         Fpstudents.Sheets[0].Cells[count, 5].Text = ds1.Tables[0].Rows[j]["SubjectName"].ToString();
    //                         Fpstudents.Sheets[0].Cells[count, 5].Note = ds1.Tables[0].Rows[j]["SubjectNo"].ToString();
    //                         Fpstudents.Sheets[0].Cells[count, 5].Tag = "RT";
    //                         Fpstudents.Sheets[0].Rows[count].ForeColor = Color.Blue;
    //                         Fpstudents.Sheets[0].Cells[count, 6].Text = ds1.Tables[0].Rows[j]["RegularTheoryCount"].ToString();
    //                         Fpstudents.Sheets[0].Cells[count, 7].Text = Arear.ToString();
    //                         Fpstudents.Sheets[0].Cells[count, 8].Text = Convert.ToString(Convert.ToInt16(ds1.Tables[0].Rows[j]["RegularTheoryCount"].ToString()) + Convert.ToInt16(Arear.ToString()));
    //                         Fpstudents.Sheets[0].RowCount++;

    //                         Temp = examds.Tables[0].Rows[i]["Department"].ToString();
    //                     }
    //                 }

    //                 if (ds1.Tables[1].Rows.Count > 0)
    //                 {
    //                     for (int jp = 0; jp < ds1.Tables[1].Rows.Count; jp++)
    //                     {
    //                         if (Temp != examds.Tables[0].Rows[i]["Department"].ToString())
    //                         {
    //                             Sno = Sno + 1;
    //                         }
    //                         countp = Fpstudents.Sheets[0].RowCount - 1;
    //                         Fpstudents.Sheets[0].Cells[countp, 0].Text = Sno.ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 1].Text = examds.Tables[0].Rows[i]["Year"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 1].Note = examds.Tables[0].Rows[i]["BatchYear"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 2].Text = examds.Tables[0].Rows[i]["Course"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 3].Text = examds.Tables[0].Rows[i]["Department"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 3].Note = examds.Tables[0].Rows[i]["DegreeCode"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 4].Text = examds.Tables[0].Rows[i]["Semester"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 5].Text = ds1.Tables[1].Rows[jp]["SubjectName"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 5].Note = ds1.Tables[1].Rows[jp]["SubjectNo"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 5].Tag = "RP";
    //                         Fpstudents.Sheets[0].Rows[countp].ForeColor = Color.BlueViolet;
    //                         Fpstudents.Sheets[0].Cells[countp, 6].Text = ds1.Tables[1].Rows[jp]["RegularPracticalCount"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 7].Text = Arear.ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 8].Text = Convert.ToString(Convert.ToInt16(ds1.Tables[1].Rows[jp]["RegularPracticalCount"].ToString()) + Convert.ToInt16(Arear.ToString()));
    //                         Fpstudents.Sheets[0].RowCount++;

    //                         Temp = examds.Tables[0].Rows[i]["Department"].ToString();
    //                     }
    //                 }


    //                 if (ds1.Tables[2].Rows.Count > 0)
    //                 {
    //                     for (int k = 0; k < ds1.Tables[2].Rows.Count; k++)
    //                     {
    //                         if (Temp != examds.Tables[0].Rows[i]["Department"].ToString())
    //                         {
    //                             Sno = Sno + 1;
    //                         }
    //                         countA = Fpstudents.Sheets[0].RowCount - 1;
    //                         Fpstudents.Sheets[0].Cells[countA, 0].Text = Sno.ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 1].Text = examds.Tables[0].Rows[i]["Year"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 1].Note = examds.Tables[0].Rows[i]["BatchYear"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 2].Text = examds.Tables[0].Rows[i]["Course"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 3].Text = examds.Tables[0].Rows[i]["Department"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 3].Note = examds.Tables[0].Rows[i]["DegreeCode"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 4].Text = ds1.Tables[2].Rows[k]["Semester"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 5].Text = ds1.Tables[2].Rows[k]["SubjectName"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 5].Note = ds1.Tables[2].Rows[k]["SubjectNo"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 5].Tag = "AT";
    //                         Fpstudents.Sheets[0].Rows[countA].ForeColor = Color.Orange;
    //                         Fpstudents.Sheets[0].Cells[countA, 6].Text = Regular.ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 7].Text = ds1.Tables[2].Rows[k]["ArearTheoryCount"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 8].Text = Convert.ToString(Convert.ToInt16(ds1.Tables[2].Rows[k]["ArearTheoryCount"].ToString()) + Convert.ToInt16(Regular.ToString()));
    //                         Fpstudents.Sheets[0].RowCount++;

    //                         Temp = examds.Tables[0].Rows[i]["Department"].ToString();


    //                     }
    //                 }
    //                 if (ds1.Tables[3].Rows.Count > 0)
    //                 {

    //                     for (int kp = 0; kp < ds1.Tables[3].Rows.Count; kp++)
    //                     {
    //                         if (Temp != examds.Tables[0].Rows[i]["Department"].ToString())
    //                         {
    //                             Sno = Sno + 1;
    //                         }
    //                         countAP = Fpstudents.Sheets[0].RowCount - 1;
    //                         Fpstudents.Sheets[0].Cells[countAP, 0].Text = Sno.ToString();
    //                         Fpstudents.Sheets[0].Cells[countAP, 1].Text = examds.Tables[0].Rows[i]["Year"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countAP, 1].Note = examds.Tables[0].Rows[i]["BatchYear"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countAP, 2].Text = examds.Tables[0].Rows[i]["Course"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countAP, 3].Text = examds.Tables[0].Rows[i]["Department"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countAP, 3].Note = examds.Tables[0].Rows[i]["DegreeCode"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countAP, 4].Text = ds1.Tables[3].Rows[kp]["Semester"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countAP, 5].Text = ds1.Tables[3].Rows[kp]["SubjectName"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countAP, 5].Note = ds1.Tables[3].Rows[kp]["SubjectNo"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countAP, 5].Tag = "AP";
    //                         Fpstudents.Sheets[0].Rows[countAP].ForeColor = Color.OrangeRed;
    //                         Fpstudents.Sheets[0].Cells[countAP, 6].Text = Regular.ToString();
    //                         Fpstudents.Sheets[0].Cells[countAP, 7].Text = ds1.Tables[3].Rows[kp]["ArearPracticalCount"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countAP, 8].Text = Convert.ToString(Convert.ToInt16(ds1.Tables[3].Rows[kp]["ArearPracticalCount"].ToString()) + Convert.ToInt16(Regular.ToString()));
    //                         Fpstudents.Sheets[0].RowCount++;

    //                         Temp = examds.Tables[0].Rows[i]["Department"].ToString();


    //                     }


    //                 }

    //             }
    //             Fpstudents.Sheets[0].RowCount--;
    //             Fpstudents.Sheets[0].PageSize = Fpstudents.Sheets[0].RowCount * 24+80;
    //             Fpstudents.Height = Fpstudents.Sheets[0].RowCount * 24+80;
    //         }
    //     }
    //     else if ((Convert.ToInt16(ddlExamType.SelectedValue) == 1 && Convert.ToInt16(ddlExmType.SelectedValue) == 1))
    //     {
    //         ddlExamstartswith.Items.Clear();
    //         ddlExamstartswith.Items.Insert(0, new ListItem("Regular", "0"));

    //         ddlExamStartType.Items.Clear();
    //         ddlExamStartType.Items.Insert(0, new ListItem("Theory", "0"));

    //         clSession.Items.Clear();
    //         clSession.Items.Insert(0, new ListItem("AM-Regular", "0"));
    //         clSession.Items.Insert(1, new ListItem("PM-Regular", "1"));


    //         SqlCommand cmd = new SqlCommand("ProcExamTimeTableSelectSubjectData", con);
    //         cmd.CommandType = CommandType.StoredProcedure;
    //         cmd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //         SqlDataAdapter da = new SqlDataAdapter(cmd);
    //         DataSet examds = new DataSet();
    //         da.Fill(examds);
    //         Fpstudents.Sheets[0].RowCount = 2;
    //         int count;

    //         int Sno = 0;
    //         string Temp = "";
    //         string Arear = "0";

    //         if (examds.Tables[0].Rows.Count > 0)
    //         {
    //             for (int i = 0; i < examds.Tables[0].Rows.Count; i++)
    //             {
    //                 SqlCommand cd = new SqlCommand("ProcExamTimeTableSubjectCount", con);
    //                 cd.CommandType = CommandType.StoredProcedure;
    //                 cd.Parameters.AddWithValue("@DegreeCode", examds.Tables[0].Rows[i]["DegreeCode"].ToString());
    //                 cd.Parameters.AddWithValue("@BatchYear", examds.Tables[0].Rows[i]["BatchYear"].ToString());
    //                 cd.Parameters.AddWithValue("@Semester", examds.Tables[0].Rows[i]["Semester"].ToString());
    //                 cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                 cd.Parameters.AddWithValue("@ExamType", examType.ToString());
    //                 cd.Parameters.AddWithValue("@ExmType", exmType.ToString());
    //                 SqlDataAdapter da1 = new SqlDataAdapter(cd);
    //                 DataSet ds1 = new DataSet();
    //                 da1.Fill(ds1);
    //                 if (ds1.Tables[0].Rows.Count > 0)
    //                 {
    //                     for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
    //                     {
    //                         if (Temp != examds.Tables[0].Rows[i]["Department"].ToString())
    //                         {
    //                             Sno = Sno + 1;
    //                         }
    //                         count = Fpstudents.Sheets[0].RowCount - 1;
    //                         Fpstudents.Sheets[0].Cells[count, 0].Text = Sno.ToString();
    //                         Fpstudents.Sheets[0].Cells[count, 1].Text = examds.Tables[0].Rows[i]["Year"].ToString();
    //                         Fpstudents.Sheets[0].Cells[count, 1].Note = examds.Tables[0].Rows[i]["BatchYear"].ToString();
    //                         Fpstudents.Sheets[0].Cells[count, 2].Text = examds.Tables[0].Rows[i]["Course"].ToString();
    //                         Fpstudents.Sheets[0].Cells[count, 3].Text = examds.Tables[0].Rows[i]["Department"].ToString();
    //                         Fpstudents.Sheets[0].Cells[count, 3].Note = examds.Tables[0].Rows[i]["DegreeCode"].ToString();
    //                         Fpstudents.Sheets[0].Cells[count, 4].Text = examds.Tables[0].Rows[i]["Semester"].ToString();
    //                         Fpstudents.Sheets[0].Cells[count, 5].Text = ds1.Tables[0].Rows[j]["SubjectName"].ToString();
    //                         Fpstudents.Sheets[0].Cells[count, 5].Note = ds1.Tables[0].Rows[j]["SubjectNo"].ToString();
    //                         Fpstudents.Sheets[0].Cells[count, 5].Tag = "RT";
    //                         Fpstudents.Sheets[0].Rows[count].ForeColor = Color.Blue;
    //                         Fpstudents.Sheets[0].Cells[count, 6].Text = ds1.Tables[0].Rows[j]["RegularTheoryCount"].ToString();
    //                         Fpstudents.Sheets[0].Cells[count, 7].Text = Arear.ToString();
    //                         Fpstudents.Sheets[0].Cells[count, 8].Text = Convert.ToString(Convert.ToInt16(ds1.Tables[0].Rows[j]["RegularTheoryCount"].ToString()) + Convert.ToInt16(Arear.ToString()));
    //                         Fpstudents.Sheets[0].RowCount++;

    //                         Temp = examds.Tables[0].Rows[i]["Department"].ToString();
    //                     }
    //                 }

    //             }
    //             Fpstudents.Sheets[0].RowCount--;
    //             Fpstudents.Sheets[0].PageSize = Fpstudents.Sheets[0].RowCount * 24+80;
    //             Fpstudents.Height = Fpstudents.Sheets[0].RowCount * 24+80;
    //         }
    //     }
    //     else if ((Convert.ToInt16(ddlExamType.SelectedValue) == 1 && Convert.ToInt16(ddlExmType.SelectedValue) == 2))
    //     {
    //         ddlExamstartswith.Items.Clear();
    //         ddlExamstartswith.Items.Insert(0, new ListItem("Regular", "0"));

    //         ddlExamStartType.Items.Clear();
    //         ddlExamStartType.Items.Insert(0, new ListItem("Practical", "0"));


    //         clSession.Items.Clear();
    //         clSession.Items.Insert(0, new ListItem("AM-Regular", "0"));
    //         clSession.Items.Insert(1, new ListItem("PM-Regular", "1"));


    //         SqlCommand cmd = new SqlCommand("ProcExamTimeTableSelectSubjectData", con);
    //         cmd.CommandType = CommandType.StoredProcedure;
    //         cmd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //         SqlDataAdapter da = new SqlDataAdapter(cmd);
    //         DataSet examds = new DataSet();
    //         da.Fill(examds);
    //         Fpstudents.Sheets[0].RowCount = 2;

    //         int countp;

    //         int Sno = 0;
    //         string Temp = "";
    //         string Arear = "0";
    //         if (examds.Tables[0].Rows.Count > 0)
    //         {
    //             for (int i = 0; i < examds.Tables[0].Rows.Count; i++)
    //             {
    //                 SqlCommand cd = new SqlCommand("ProcExamTimeTableSubjectCount", con);
    //                 cd.CommandType = CommandType.StoredProcedure;
    //                 cd.Parameters.AddWithValue("@DegreeCode", examds.Tables[0].Rows[i]["DegreeCode"].ToString());
    //                 cd.Parameters.AddWithValue("@BatchYear", examds.Tables[0].Rows[i]["BatchYear"].ToString());
    //                 cd.Parameters.AddWithValue("@Semester", examds.Tables[0].Rows[i]["Semester"].ToString());
    //                 cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                 cd.Parameters.AddWithValue("@ExamType", examType.ToString());
    //                 cd.Parameters.AddWithValue("@ExmType", exmType.ToString());
    //                 SqlDataAdapter da1 = new SqlDataAdapter(cd);
    //                 DataSet ds1 = new DataSet();
    //                 da1.Fill(ds1);
    //                 if (ds1.Tables[0].Rows.Count > 0)
    //                 {
    //                     for (int jp = 0; jp < ds1.Tables[0].Rows.Count; jp++)
    //                     {
    //                         if (Temp != examds.Tables[0].Rows[i]["Department"].ToString())
    //                         {
    //                             Sno = Sno + 1;
    //                         }
    //                         countp = Fpstudents.Sheets[0].RowCount - 1;
    //                         Fpstudents.Sheets[0].Cells[countp, 0].Text = Sno.ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 1].Text = examds.Tables[0].Rows[i]["Year"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 1].Note = examds.Tables[0].Rows[i]["BatchYear"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 2].Text = examds.Tables[0].Rows[i]["Course"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 3].Text = examds.Tables[0].Rows[i]["Department"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 3].Note = examds.Tables[0].Rows[i]["DegreeCode"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 4].Text = examds.Tables[0].Rows[i]["Semester"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 5].Text = ds1.Tables[0].Rows[jp]["SubjectName"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 5].Note = ds1.Tables[0].Rows[jp]["SubjectNo"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 5].Tag = "RP";
    //                         Fpstudents.Sheets[0].Rows[countp].ForeColor = Color.BlueViolet;
    //                         Fpstudents.Sheets[0].Cells[countp, 6].Text = ds1.Tables[0].Rows[jp]["RegularPracticalCount"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 7].Text = Arear.ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 8].Text = Convert.ToString(Convert.ToInt16(ds1.Tables[0].Rows[jp]["RegularPracticalCount"].ToString()) + Convert.ToInt16(Arear.ToString()));
    //                         Fpstudents.Sheets[0].RowCount++;

    //                         Temp = examds.Tables[0].Rows[i]["Department"].ToString();
    //                     }
    //                 }
    //             }
    //             Fpstudents.Sheets[0].RowCount--;
    //             Fpstudents.Sheets[0].PageSize = Fpstudents.Sheets[0].RowCount *24+80;
    //             Fpstudents.Height = Fpstudents.Sheets[0].RowCount * 24+80;
    //         }

    //     }

    //     else if ((Convert.ToInt16(ddlExamType.SelectedValue) == 2 && Convert.ToInt16(ddlExmType.SelectedValue) == 1))
    //     {
    //         ddlExamstartswith.Items.Clear();

    //         ddlExamstartswith.Items.Insert(0, new ListItem("Arear", "0"));

    //         ddlExamStartType.Items.Clear();
    //         ddlExamStartType.Items.Insert(0, new ListItem("Theory", "0"));

    //         clSession.Items.Clear();

    //         clSession.Items.Insert(0, new ListItem("AM-Arear", "0"));
    //         clSession.Items.Insert(1, new ListItem("PM-Arear", "1"));


    //         SqlCommand cmd = new SqlCommand("ProcExamTimeTableSelectSubjectData", con);
    //         cmd.CommandType = CommandType.StoredProcedure;
    //         cmd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //         SqlDataAdapter da = new SqlDataAdapter(cmd);
    //         DataSet examds = new DataSet();
    //         da.Fill(examds);
    //         Fpstudents.Sheets[0].RowCount = 2;

    //         int countA;

    //         int Sno = 0;
    //         string Temp = "";

    //         string Regular = "0";
    //         if (examds.Tables[0].Rows.Count > 0)
    //         {
    //             for (int i = 0; i < examds.Tables[0].Rows.Count; i++)
    //             {
    //                 SqlCommand cd = new SqlCommand("ProcExamTimeTableSubjectCount", con);
    //                 cd.CommandType = CommandType.StoredProcedure;
    //                 cd.Parameters.AddWithValue("@DegreeCode", examds.Tables[0].Rows[i]["DegreeCode"].ToString());
    //                 cd.Parameters.AddWithValue("@BatchYear", examds.Tables[0].Rows[i]["BatchYear"].ToString());
    //                 cd.Parameters.AddWithValue("@Semester", examds.Tables[0].Rows[i]["Semester"].ToString());
    //                 cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                 cd.Parameters.AddWithValue("@ExamType", examType.ToString());
    //                 cd.Parameters.AddWithValue("@ExmType", exmType.ToString());
    //                 SqlDataAdapter da1 = new SqlDataAdapter(cd);
    //                 DataSet ds1 = new DataSet();
    //                 da1.Fill(ds1);
    //                 if (ds1.Tables[0].Rows.Count > 0)
    //                 {
    //                     for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
    //                     {
    //                         if (Temp != examds.Tables[0].Rows[i]["Department"].ToString())
    //                         {
    //                             Sno = Sno + 1;
    //                         }
    //                         countA = Fpstudents.Sheets[0].RowCount - 1;
    //                         Fpstudents.Sheets[0].Cells[countA, 0].Text = Sno.ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 1].Text = examds.Tables[0].Rows[i]["Year"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 1].Note = examds.Tables[0].Rows[i]["BatchYear"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 2].Text = examds.Tables[0].Rows[i]["Course"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 3].Text = examds.Tables[0].Rows[i]["Department"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 3].Note = examds.Tables[0].Rows[i]["DegreeCode"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 4].Text = ds1.Tables[0].Rows[k]["Semester"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 5].Text = ds1.Tables[0].Rows[k]["SubjectName"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 5].Note = ds1.Tables[0].Rows[k]["SubjectNo"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 5].Tag = "AT";
    //                         Fpstudents.Sheets[0].Rows[countA].ForeColor = Color.Orange;
    //                         Fpstudents.Sheets[0].Cells[countA, 6].Text = Regular.ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 7].Text = ds1.Tables[0].Rows[k]["ArearTheoryCount"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 8].Text = Convert.ToString(Convert.ToInt16(ds1.Tables[0].Rows[k]["ArearTheoryCount"].ToString()) + Convert.ToInt16(Regular.ToString()));
    //                         Fpstudents.Sheets[0].RowCount++;

    //                         Temp = examds.Tables[0].Rows[i]["Department"].ToString();


    //                     }
    //                 }

    //             }
    //             Fpstudents.Sheets[0].RowCount--;
    //             Fpstudents.Sheets[0].PageSize = Fpstudents.Sheets[0].RowCount * 24+80;
    //             Fpstudents.Height = Fpstudents.Sheets[0].RowCount * 24+80;
    //         }

    //     }
    //     if (Convert.ToInt16(ddlExamType.SelectedValue) == 2 && Convert.ToInt16(ddlExmType.SelectedValue) == 2)
    //     {
    //         ddlExamstartswith.Items.Clear();

    //         ddlExamstartswith.Items.Insert(0, new ListItem("Arear", "0"));

    //         ddlExamStartType.Items.Clear();
    //         ddlExamStartType.Items.Insert(0, new ListItem("Practical", "0"));


    //         clSession.Items.Clear();
    //         clSession.Items.Insert(0, new ListItem("AM-Arear", "0"));
    //         clSession.Items.Insert(1, new ListItem("PM-Arear", "1"));

    //         SqlCommand cmd = new SqlCommand("ProcExamTimeTableSelectSubjectData", con);
    //         cmd.CommandType = CommandType.StoredProcedure;
    //         cmd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //         SqlDataAdapter da = new SqlDataAdapter(cmd);
    //         DataSet examds = new DataSet();
    //         da.Fill(examds);
    //         Fpstudents.Sheets[0].RowCount = 2;

    //         int countAP;
    //         int Sno = 0;
    //         string Temp = "";

    //         string Regular = "0";
    //         if (examds.Tables[0].Rows.Count > 0)
    //         {
    //             for (int i = 0; i < examds.Tables[0].Rows.Count; i++)
    //             {
    //                 SqlCommand cd = new SqlCommand("ProcExamTimeTableSubjectCount", con);
    //                 cd.CommandType = CommandType.StoredProcedure;
    //                 cd.Parameters.AddWithValue("@DegreeCode", examds.Tables[0].Rows[i]["DegreeCode"].ToString());
    //                 cd.Parameters.AddWithValue("@BatchYear", examds.Tables[0].Rows[i]["BatchYear"].ToString());
    //                 cd.Parameters.AddWithValue("@Semester", examds.Tables[0].Rows[i]["Semester"].ToString());
    //                 cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                 cd.Parameters.AddWithValue("@ExamType", examType.ToString());
    //                 cd.Parameters.AddWithValue("@ExmType", exmType.ToString());
    //                 SqlDataAdapter da1 = new SqlDataAdapter(cd);
    //                 DataSet ds1 = new DataSet();
    //                 da1.Fill(ds1);
    //                 if (ds1.Tables[0].Rows.Count > 0)
    //                 {

    //                     for (int kp = 0; kp < ds1.Tables[0].Rows.Count; kp++)
    //                     {
    //                         if (Temp != examds.Tables[0].Rows[i]["Department"].ToString())
    //                         {
    //                             Sno = Sno + 1;
    //                         }
    //                         countAP = Fpstudents.Sheets[0].RowCount - 1;
    //                         Fpstudents.Sheets[0].Cells[countAP, 0].Text = Sno.ToString();
    //                         Fpstudents.Sheets[0].Cells[countAP, 1].Text = examds.Tables[0].Rows[i]["Year"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countAP, 1].Note = examds.Tables[0].Rows[i]["BatchYear"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countAP, 2].Text = examds.Tables[0].Rows[i]["Course"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countAP, 3].Text = examds.Tables[0].Rows[i]["Department"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countAP, 3].Note = examds.Tables[0].Rows[i]["DegreeCode"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countAP, 4].Text = ds1.Tables[0].Rows[kp]["Semester"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countAP, 5].Text = ds1.Tables[0].Rows[kp]["SubjectName"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countAP, 5].Note = ds1.Tables[0].Rows[kp]["SubjectNo"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countAP, 5].Tag = "AP";
    //                         Fpstudents.Sheets[0].Rows[countAP].ForeColor = Color.OrangeRed;
    //                         Fpstudents.Sheets[0].Cells[countAP, 6].Text = Regular.ToString();
    //                         Fpstudents.Sheets[0].Cells[countAP, 7].Text = ds1.Tables[0].Rows[kp]["ArearPracticalCount"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countAP, 8].Text = Convert.ToString(Convert.ToInt16(ds1.Tables[0].Rows[kp]["ArearPracticalCount"].ToString()) + Convert.ToInt16(Regular.ToString()));
    //                         Fpstudents.Sheets[0].RowCount++;

    //                         Temp = examds.Tables[0].Rows[i]["Department"].ToString();


    //                     }
    //                 }

    //             }
    //             Fpstudents.Sheets[0].RowCount--;
    //             Fpstudents.Sheets[0].PageSize = Fpstudents.Sheets[0].RowCount * 24+80;
    //             Fpstudents.Height = Fpstudents.Sheets[0].RowCount * 24+80;
    //         }
    //     }
    //     else if (Convert.ToInt16(ddlExamType.SelectedValue) == 0 && Convert.ToInt16(ddlExmType.SelectedValue) == 1)
    //     {
    //         ddlExamstartswith.Items.Clear();
    //         ddlExamstartswith.Items.Insert(0, new ListItem("Regular", "0"));
    //         ddlExamstartswith.Items.Insert(1, new ListItem("Arear", "1"));

    //         ddlExamStartType.Items.Clear();
    //         ddlExamStartType.Items.Insert(0, new ListItem("Theory", "0"));

    //         clSession.Items.Clear();
    //         clSession.Items.Insert(0, new ListItem("AM-Regular", "0"));
    //         clSession.Items.Insert(1, new ListItem("PM-Regular", "1"));
    //         clSession.Items.Insert(2, new ListItem("Mixed", "2"));
    //         clSession.Items.Insert(3, new ListItem("AM-Arear", "3"));
    //         clSession.Items.Insert(4, new ListItem("PM-Arear", "4"));

    //         SqlCommand cmd = new SqlCommand("ProcExamTimeTableSelectSubjectData", con);
    //         cmd.CommandType = CommandType.StoredProcedure;
    //         cmd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //         SqlDataAdapter da = new SqlDataAdapter(cmd);
    //         DataSet examds = new DataSet();
    //         da.Fill(examds);
    //         Fpstudents.Sheets[0].RowCount = 2;

    //         int countA;
    //         int Sno = 0;
    //         string Arear = "0";
    //         string Temp = "";
    //         int countp;
    //         string Regular = "0";
    //         if (examds.Tables[0].Rows.Count > 0)
    //         {
    //             for (int i = 0; i < examds.Tables[0].Rows.Count; i++)
    //             {
    //                 SqlCommand cd = new SqlCommand("ProcExamTimeTableSubjectCount", con);
    //                 cd.CommandType = CommandType.StoredProcedure;
    //                 cd.Parameters.AddWithValue("@DegreeCode", examds.Tables[0].Rows[i]["DegreeCode"].ToString());
    //                 cd.Parameters.AddWithValue("@BatchYear", examds.Tables[0].Rows[i]["BatchYear"].ToString());
    //                 cd.Parameters.AddWithValue("@Semester", examds.Tables[0].Rows[i]["Semester"].ToString());
    //                 cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                 cd.Parameters.AddWithValue("@ExamType", examType.ToString());
    //                 cd.Parameters.AddWithValue("@ExmType", exmType.ToString());
    //                 SqlDataAdapter da1 = new SqlDataAdapter(cd);
    //                 DataSet ds1 = new DataSet();
    //                 da1.Fill(ds1);
    //                 if (ds1.Tables[0].Rows.Count > 0)
    //                 {
    //                     for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
    //                     {
    //                         if (Temp != examds.Tables[0].Rows[i]["Department"].ToString())
    //                         {
    //                             Sno = Sno + 1;
    //                         }
    //                         countA = Fpstudents.Sheets[0].RowCount - 1;
    //                         Fpstudents.Sheets[0].Cells[countA, 0].Text = Sno.ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 1].Text = examds.Tables[0].Rows[i]["Year"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 1].Note = examds.Tables[0].Rows[i]["BatchYear"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 2].Text = examds.Tables[0].Rows[i]["Course"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 3].Text = examds.Tables[0].Rows[i]["Department"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 3].Note = examds.Tables[0].Rows[i]["DegreeCode"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 4].Text = examds.Tables[0].Rows[i]["Semester"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 5].Text = ds1.Tables[0].Rows[k]["SubjectName"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 5].Note = ds1.Tables[0].Rows[k]["SubjectNo"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 5].Tag = "RT";
    //                         Fpstudents.Sheets[0].Rows[countA].ForeColor = Color.Blue;
    //                         Fpstudents.Sheets[0].Cells[countA, 7].Text = Arear.ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 6].Text = ds1.Tables[0].Rows[k]["RegularTheoryCount"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 8].Text = Convert.ToString(Convert.ToInt16(ds1.Tables[0].Rows[k]["RegularTheoryCount"].ToString()) + Convert.ToInt16(Regular.ToString()));
    //                         Fpstudents.Sheets[0].RowCount++;

    //                         Temp = examds.Tables[0].Rows[i]["Department"].ToString();


    //                     }
    //                 }


    //                 if (ds1.Tables[1].Rows.Count > 0)
    //                 {
    //                     for (int jp = 0; jp < ds1.Tables[1].Rows.Count; jp++)
    //                     {
    //                         if (Temp != examds.Tables[0].Rows[i]["Department"].ToString())
    //                         {
    //                             Sno = Sno + 1;
    //                         }
    //                         countp = Fpstudents.Sheets[0].RowCount - 1;
    //                         Fpstudents.Sheets[0].Cells[countp, 0].Text = Sno.ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 1].Text = examds.Tables[0].Rows[i]["Year"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 1].Note = examds.Tables[0].Rows[i]["BatchYear"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 2].Text = examds.Tables[0].Rows[i]["Course"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 3].Text = examds.Tables[0].Rows[i]["Department"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 3].Note = examds.Tables[0].Rows[i]["DegreeCode"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 4].Text = ds1.Tables[1].Rows[jp]["Semester"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 5].Text = ds1.Tables[1].Rows[jp]["SubjectName"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 5].Note = ds1.Tables[1].Rows[jp]["SubjectNo"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 5].Tag = "AT";
    //                         Fpstudents.Sheets[0].Rows[countp].ForeColor = Color.Orange;
    //                         Fpstudents.Sheets[0].Cells[countp, 7].Text = ds1.Tables[1].Rows[jp]["ArearTheoryCount"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 6].Text = Regular.ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 8].Text = Convert.ToString(Convert.ToInt16(ds1.Tables[1].Rows[jp]["ArearTheoryCount"].ToString()) + Convert.ToInt16(Arear.ToString()));
    //                         Fpstudents.Sheets[0].RowCount++;

    //                         Temp = examds.Tables[0].Rows[i]["Department"].ToString();
    //                     }
    //                 }

    //             }

    //             Fpstudents.Sheets[0].RowCount--;
    //             Fpstudents.Sheets[0].PageSize = Fpstudents.Sheets[0].RowCount * 24+80;
    //             Fpstudents.Height = Fpstudents.Sheets[0].RowCount * 24+80;
    //         }

    //     }
    //     else if (Convert.ToInt16(ddlExamType.SelectedValue) == 0 && Convert.ToInt16(ddlExmType.SelectedValue) == 2)
    //     {
    //         ddlExamstartswith.Items.Clear();
    //         ddlExamstartswith.Items.Insert(0, new ListItem("Regular", "0"));
    //         ddlExamstartswith.Items.Insert(1, new ListItem("Arear", "1"));

    //         ddlExamStartType.Items.Clear();
    //         ddlExamStartType.Items.Insert(0, new ListItem("Practical", "0"));

    //         clSession.Items.Clear();
    //         clSession.Items.Insert(0, new ListItem("AM-Regular", "0"));
    //         clSession.Items.Insert(1, new ListItem("PM-Regular", "1"));
    //         clSession.Items.Insert(2, new ListItem("Mixed", "2"));
    //         clSession.Items.Insert(3, new ListItem("AM-Arear", "3"));
    //         clSession.Items.Insert(4, new ListItem("PM-Arear", "4"));

    //         SqlCommand cmd = new SqlCommand("ProcExamTimeTableSelectSubjectData", con);
    //         cmd.CommandType = CommandType.StoredProcedure;
    //         cmd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //         SqlDataAdapter da = new SqlDataAdapter(cmd);
    //         DataSet examds = new DataSet();
    //         da.Fill(examds);
    //         Fpstudents.Sheets[0].RowCount = 2;
    //         string Arear = "0";
    //         int countA;
    //         int Sno = 0;
    //         string Temp = "";
    //         int countp;
    //         string Regular = "0";
    //         if (examds.Tables[0].Rows.Count > 0)
    //         {
    //             for (int i = 0; i < examds.Tables[0].Rows.Count; i++)
    //             {
    //                 SqlCommand cd = new SqlCommand("ProcExamTimeTableSubjectCount", con);
    //                 cd.CommandType = CommandType.StoredProcedure;
    //                 cd.Parameters.AddWithValue("@DegreeCode", examds.Tables[0].Rows[i]["DegreeCode"].ToString());
    //                 cd.Parameters.AddWithValue("@BatchYear", examds.Tables[0].Rows[i]["BatchYear"].ToString());
    //                 cd.Parameters.AddWithValue("@Semester", examds.Tables[0].Rows[i]["Semester"].ToString());
    //                 cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                 cd.Parameters.AddWithValue("@ExamType", examType.ToString());
    //                 cd.Parameters.AddWithValue("@ExmType", exmType.ToString());
    //                 SqlDataAdapter da1 = new SqlDataAdapter(cd);
    //                 DataSet ds1 = new DataSet();
    //                 da1.Fill(ds1);
    //                 if (ds1.Tables[0].Rows.Count > 0)
    //                 {
    //                     for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
    //                     {
    //                         if (Temp != examds.Tables[0].Rows[i]["Department"].ToString())
    //                         {
    //                             Sno = Sno + 1;
    //                         }
    //                         countA = Fpstudents.Sheets[0].RowCount - 1;
    //                         Fpstudents.Sheets[0].Cells[countA, 0].Text = Sno.ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 1].Text = examds.Tables[0].Rows[i]["Year"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 1].Note = examds.Tables[0].Rows[i]["BatchYear"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 2].Text = examds.Tables[0].Rows[i]["Course"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 3].Text = examds.Tables[0].Rows[i]["Department"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 3].Note = examds.Tables[0].Rows[i]["DegreeCode"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 4].Text = examds.Tables[0].Rows[i]["Semester"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 5].Text = ds1.Tables[0].Rows[k]["SubjectName"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 5].Note = ds1.Tables[0].Rows[k]["SubjectNo"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 5].Tag = "RP";
    //                         Fpstudents.Sheets[0].Rows[countA].ForeColor = Color.BlueViolet;
    //                         Fpstudents.Sheets[0].Cells[countA, 7].Text = Arear.ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 6].Text = ds1.Tables[0].Rows[k]["RegularPracticalCount"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 8].Text = Convert.ToString(Convert.ToInt16(ds1.Tables[0].Rows[k]["RegularPracticalCount"].ToString()) + Convert.ToInt16(Arear.ToString()));
    //                         Fpstudents.Sheets[0].RowCount++;

    //                         Temp = examds.Tables[0].Rows[i]["Department"].ToString();


    //                     }
    //                 }


    //                 if (ds1.Tables[1].Rows.Count > 0)
    //                 {
    //                     for (int jp = 0; jp < ds1.Tables[1].Rows.Count; jp++)
    //                     {
    //                         if (Temp != examds.Tables[0].Rows[i]["Department"].ToString())
    //                         {
    //                             Sno = Sno + 1;
    //                         }
    //                         countp = Fpstudents.Sheets[0].RowCount - 1;
    //                         Fpstudents.Sheets[0].Cells[countp, 0].Text = Sno.ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 1].Text = examds.Tables[0].Rows[i]["Year"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 1].Note = examds.Tables[0].Rows[i]["BatchYear"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 2].Text = examds.Tables[0].Rows[i]["Course"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 3].Text = examds.Tables[0].Rows[i]["Department"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 3].Note = examds.Tables[0].Rows[i]["DegreeCode"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 4].Text = ds1.Tables[1].Rows[jp]["Semester"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 5].Text = ds1.Tables[1].Rows[jp]["SubjectName"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 5].Note = ds1.Tables[1].Rows[jp]["SubjectNo"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 5].Tag = "AP";
    //                         Fpstudents.Sheets[0].Rows[countp].ForeColor = Color.OrangeRed;
    //                         Fpstudents.Sheets[0].Cells[countp, 7].Text = ds1.Tables[1].Rows[jp]["ArearPracticalCount"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 6].Text = Regular.ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 8].Text = Convert.ToString(Convert.ToInt16(ds1.Tables[1].Rows[jp]["ArearPracticalCount"].ToString()) + Convert.ToInt16(Regular.ToString()));
    //                         Fpstudents.Sheets[0].RowCount++;

    //                         Temp = examds.Tables[0].Rows[i]["Department"].ToString();
    //                     }
    //                 }

    //             }

    //             Fpstudents.Sheets[0].RowCount--;
    //             Fpstudents.Sheets[0].PageSize = Fpstudents.Sheets[0].RowCount * 24+80;
    //             Fpstudents.Height = Fpstudents.Sheets[0].RowCount * 24+80;
    //         }
    //     }
    //     else if (Convert.ToInt16(ddlExamType.SelectedValue) == 1 && Convert.ToInt16(ddlExmType.SelectedValue) == 0)
    //     {
    //         ddlExamstartswith.Items.Clear();
    //         ddlExamstartswith.Items.Insert(0, new ListItem("Regular", "0"));


    //         ddlExamStartType.Items.Clear();
    //         ddlExamStartType.Items.Insert(0, new ListItem("Theory", "0"));
    //         ddlExamStartType.Items.Insert(1, new ListItem("Practical", "1"));

    //         clSession.Items.Clear();
    //         clSession.Items.Insert(0, new ListItem("AM-Regular", "0"));
    //         clSession.Items.Insert(1, new ListItem("PM-Regular", "1"));

    //         SqlCommand cmd = new SqlCommand("ProcExamTimeTableSelectSubjectData", con);
    //         cmd.CommandType = CommandType.StoredProcedure;
    //         cmd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //         SqlDataAdapter da = new SqlDataAdapter(cmd);
    //         DataSet examds = new DataSet();
    //         da.Fill(examds);
    //         Fpstudents.Sheets[0].RowCount = 2;
    //         string Arear = "0";
    //         int countA;
    //         int Sno = 0;
    //         string Temp = "";
    //         int countp;

    //         if (examds.Tables[0].Rows.Count > 0)
    //         {
    //             for (int i = 0; i < examds.Tables[0].Rows.Count; i++)
    //             {
    //                 SqlCommand cd = new SqlCommand("ProcExamTimeTableSubjectCount", con);
    //                 cd.CommandType = CommandType.StoredProcedure;
    //                 cd.Parameters.AddWithValue("@DegreeCode", examds.Tables[0].Rows[i]["DegreeCode"].ToString());
    //                 cd.Parameters.AddWithValue("@BatchYear", examds.Tables[0].Rows[i]["BatchYear"].ToString());
    //                 cd.Parameters.AddWithValue("@Semester", examds.Tables[0].Rows[i]["Semester"].ToString());
    //                 cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                 cd.Parameters.AddWithValue("@ExamType", examType.ToString());
    //                 cd.Parameters.AddWithValue("@ExmType", exmType.ToString());
    //                 SqlDataAdapter da1 = new SqlDataAdapter(cd);
    //                 DataSet ds1 = new DataSet();
    //                 da1.Fill(ds1);
    //                 if (ds1.Tables[0].Rows.Count > 0)
    //                 {
    //                     for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
    //                     {
    //                         if (Temp != examds.Tables[0].Rows[i]["Department"].ToString())
    //                         {
    //                             Sno = Sno + 1;
    //                         }
    //                         countA = Fpstudents.Sheets[0].RowCount - 1;
    //                         Fpstudents.Sheets[0].Cells[countA, 0].Text = Sno.ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 1].Text = examds.Tables[0].Rows[i]["Year"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 1].Note = examds.Tables[0].Rows[i]["BatchYear"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 2].Text = examds.Tables[0].Rows[i]["Course"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 3].Text = examds.Tables[0].Rows[i]["Department"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 3].Note = examds.Tables[0].Rows[i]["DegreeCode"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 4].Text = examds.Tables[0].Rows[i]["Semester"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 5].Text = ds1.Tables[0].Rows[k]["SubjectName"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 5].Note = ds1.Tables[0].Rows[k]["SubjectNo"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 5].Tag = "RT";
    //                         Fpstudents.Sheets[0].Rows[countA].ForeColor = Color.Blue;
    //                         Fpstudents.Sheets[0].Cells[countA, 7].Text = Arear.ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 6].Text = ds1.Tables[0].Rows[k]["RegularTheoryCount"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 8].Text = Convert.ToString(Convert.ToInt16(ds1.Tables[0].Rows[k]["RegularTheoryCount"].ToString()) + Convert.ToInt16(Arear.ToString()));
    //                         Fpstudents.Sheets[0].RowCount++;

    //                         Temp = examds.Tables[0].Rows[i]["Department"].ToString();


    //                     }
    //                 }


    //                 if (ds1.Tables[1].Rows.Count > 0)
    //                 {
    //                     for (int jp = 0; jp < ds1.Tables[1].Rows.Count; jp++)
    //                     {
    //                         if (Temp != examds.Tables[0].Rows[i]["Department"].ToString())
    //                         {
    //                             Sno = Sno + 1;
    //                         }
    //                         countp = Fpstudents.Sheets[0].RowCount - 1;
    //                         Fpstudents.Sheets[0].Cells[countp, 0].Text = Sno.ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 1].Text = examds.Tables[0].Rows[i]["Year"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 1].Note = examds.Tables[0].Rows[i]["BatchYear"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 2].Text = examds.Tables[0].Rows[i]["Course"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 3].Text = examds.Tables[0].Rows[i]["Department"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 3].Note = examds.Tables[0].Rows[i]["DegreeCode"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 4].Text = examds.Tables[0].Rows[i]["Semester"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 5].Text = ds1.Tables[1].Rows[jp]["SubjectName"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 5].Note = ds1.Tables[1].Rows[jp]["SubjectNo"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 5].Tag = "RP";
    //                         Fpstudents.Sheets[0].Rows[countp].ForeColor = Color.BlueViolet;
    //                         Fpstudents.Sheets[0].Cells[countp, 6].Text = ds1.Tables[1].Rows[jp]["RegularPracticalCount"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 7].Text = Arear.ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 8].Text = Convert.ToString(Convert.ToInt16(ds1.Tables[1].Rows[jp]["RegularPracticalCount"].ToString()) + Convert.ToInt16(Arear.ToString()));
    //                         Fpstudents.Sheets[0].RowCount++;

    //                         Temp = examds.Tables[0].Rows[i]["Department"].ToString();
    //                     }
    //                 }

    //             }

    //             Fpstudents.Sheets[0].RowCount--;
    //             Fpstudents.Sheets[0].PageSize = Fpstudents.Sheets[0].RowCount * 24+80;
    //             Fpstudents.Height = Fpstudents.Sheets[0].RowCount * 24+80;
    //         }
    //     }
    //     else if (Convert.ToInt16(ddlExamType.SelectedValue) == 2 && Convert.ToInt16(ddlExmType.SelectedValue) == 0)
    //     {
    //         ddlExamstartswith.Items.Clear();
    //         ddlExamstartswith.Items.Insert(0, new ListItem("Arear", "0"));

    //         ddlExamStartType.Items.Clear();
    //         ddlExamStartType.Items.Insert(0, new ListItem("Theory", "0"));
    //         ddlExamStartType.Items.Insert(1, new ListItem("Practical", "1"));

    //         clSession.Items.Clear();
    //         clSession.Items.Insert(0, new ListItem("AM-Arear", "0"));
    //         clSession.Items.Insert(1, new ListItem("PM-Arear", "1"));

    //         SqlCommand cmd = new SqlCommand("ProcExamTimeTableSelectSubjectData", con);
    //         cmd.CommandType = CommandType.StoredProcedure;
    //         cmd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //         SqlDataAdapter da = new SqlDataAdapter(cmd);
    //         DataSet examds = new DataSet();
    //         da.Fill(examds);
    //         Fpstudents.Sheets[0].RowCount = 2;
    //         string Regular = "0";
    //         int countA;
    //         int Sno = 0;
    //         string Temp = "";
    //         int countp;

    //         if (examds.Tables[0].Rows.Count > 0)
    //         {
    //             for (int i = 0; i < examds.Tables[0].Rows.Count; i++)
    //             {
    //                 SqlCommand cd = new SqlCommand("ProcExamTimeTableSubjectCount", con);
    //                 cd.CommandType = CommandType.StoredProcedure;
    //                 cd.Parameters.AddWithValue("@DegreeCode", examds.Tables[0].Rows[i]["DegreeCode"].ToString());
    //                 cd.Parameters.AddWithValue("@BatchYear", examds.Tables[0].Rows[i]["BatchYear"].ToString());
    //                 cd.Parameters.AddWithValue("@Semester", examds.Tables[0].Rows[i]["Semester"].ToString());
    //                 cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                 cd.Parameters.AddWithValue("@ExamType", examType.ToString());
    //                 cd.Parameters.AddWithValue("@ExmType", exmType.ToString());
    //                 SqlDataAdapter da1 = new SqlDataAdapter(cd);
    //                 DataSet ds1 = new DataSet();
    //                 da1.Fill(ds1);
    //                 if (ds1.Tables[0].Rows.Count > 0)
    //                 {
    //                     for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
    //                     {
    //                         if (Temp != examds.Tables[0].Rows[i]["Department"].ToString())
    //                         {
    //                             Sno = Sno + 1;
    //                         }
    //                         countA = Fpstudents.Sheets[0].RowCount - 1;
    //                         Fpstudents.Sheets[0].Cells[countA, 0].Text = Sno.ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 1].Text = examds.Tables[0].Rows[i]["Year"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 1].Note = examds.Tables[0].Rows[i]["BatchYear"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 2].Text = examds.Tables[0].Rows[i]["Course"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 3].Text = examds.Tables[0].Rows[i]["Department"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 3].Note = examds.Tables[0].Rows[i]["DegreeCode"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 4].Text = ds1.Tables[0].Rows[k]["Semester"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 5].Text = ds1.Tables[0].Rows[k]["SubjectName"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 5].Note = ds1.Tables[0].Rows[k]["SubjectNo"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 5].Tag = "AT";
    //                         Fpstudents.Sheets[0].Rows[countA].ForeColor = Color.Orange;
    //                         Fpstudents.Sheets[0].Cells[countA, 6].Text = Regular.ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 7].Text = ds1.Tables[0].Rows[k]["ArearTheoryCount"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countA, 8].Text = Convert.ToString(Convert.ToInt16(ds1.Tables[0].Rows[k]["ArearTheoryCount"].ToString()) + Convert.ToInt16(Regular.ToString()));
    //                         Fpstudents.Sheets[0].RowCount++;

    //                         Temp = examds.Tables[0].Rows[i]["Department"].ToString();


    //                     }
    //                 }


    //                 if (ds1.Tables[1].Rows.Count > 0)
    //                 {
    //                     for (int jp = 0; jp < ds1.Tables[1].Rows.Count; jp++)
    //                     {
    //                         if (Temp != examds.Tables[0].Rows[i]["Department"].ToString())
    //                         {
    //                             Sno = Sno + 1;
    //                         }
    //                         countp = Fpstudents.Sheets[0].RowCount - 1;
    //                         Fpstudents.Sheets[0].Cells[countp, 0].Text = Sno.ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 1].Text = examds.Tables[0].Rows[i]["Year"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 1].Note = examds.Tables[0].Rows[i]["BatchYear"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 1].Note = examds.Tables[0].Rows[i]["BatchYear"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 2].Text = examds.Tables[0].Rows[i]["Course"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 3].Text = examds.Tables[0].Rows[i]["Department"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 3].Note = examds.Tables[0].Rows[i]["DegreeCode"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 4].Text = ds1.Tables[0].Rows[jp]["Semester"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 5].Text = ds1.Tables[1].Rows[jp]["SubjectName"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 5].Note = ds1.Tables[1].Rows[jp]["SubjectNo"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 5].Tag = "AP";
    //                         Fpstudents.Sheets[0].Rows[countp].ForeColor = Color.OrangeRed;
    //                         Fpstudents.Sheets[0].Cells[countp, 7].Text = ds1.Tables[1].Rows[jp]["ArearPracticalCount"].ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 6].Text = Regular.ToString();
    //                         Fpstudents.Sheets[0].Cells[countp, 8].Text = Convert.ToString(Convert.ToInt16(ds1.Tables[1].Rows[jp]["ArearPracticalCount"].ToString()) + Convert.ToInt16(Regular.ToString()));
    //                         Fpstudents.Sheets[0].RowCount++;

    //                         Temp = examds.Tables[0].Rows[i]["Department"].ToString();
    //                     }
    //                 }

    //             }

    //             Fpstudents.Sheets[0].RowCount--;
    //             Fpstudents.Sheets[0].PageSize = Fpstudents.Sheets[0].RowCount * 24+80;
    //             Fpstudents.Height = Fpstudents.Sheets[0].RowCount * 24+80;
    //         }
    //     }

    // }

    //protected void btnGenerate_Click(object sender, EventArgs e)
    //{
    //    int count = 0;
    //    for (int k = 0; k < clSession.Items.Count; k++)
    //    {
    //        if (clSession.Items[k].Selected == true)
    //        {
    //            count = count + 1;

    //        }
    //    }

    //    if (count < 1)
    //    {

    //        lblerror.Text = "Select the Session";
    //        lblerror.Visible = true;
    //        txtSession.Focus();

    //    }
    //    else if (Convert.ToInt16(ddlMonth.SelectedValue) == 0)
    //    {
    //        lblerror.Text = "Select The Month";
    //        lblerror.Visible = true;
    //        ddlMonth.Focus();
    //    }
    //    else if (Convert.ToInt16(ddlYear.SelectedValue) == 0)
    //    {
    //        lblerror.Text = "Select The Year";
    //        lblerror.Visible = true;
    //        ddlYear.Focus();
    //    }
    //    else if (ddlExamType.SelectedItem.Text.ToString() == "All" && ddlExmType.SelectedItem.Text.ToString() == "Theory" && txtTheoryDurationam.Text.ToString() == "")
    //    {

    //        lblerror.Text = "Select The Theory Time [AM]";
    //        lblerror.Visible = true;
    //    }
    //    else if (ddlExamType.SelectedItem.Text.ToString() == "All" && ddlExmType.SelectedItem.Text.ToString() == "Theory" && txtTheoryDurationpm.Text.ToString() == "")
    //    {

    //        lblerror.Text = "Select The Theory Time [PM]";
    //        lblerror.Visible = true;
    //    }

    //    else if (ddlExamType.SelectedItem.Text.ToString() == "All" && ddlExmType.SelectedItem.Text.ToString() == "Practical" && txtpracdurationam.Text.ToString() == "")
    //    {

    //        lblerror.Text = "Select The Practical Time [AM]";
    //        lblerror.Visible = true;
    //    }
    //    else if (ddlExamType.SelectedItem.Text.ToString() == "All" && ddlExmType.SelectedItem.Text.ToString() == "Practical" && txtpracdurationpm.Text.ToString() == "")
    //    {

    //        lblerror.Text = "Select The Practical Time [PM]";
    //        lblerror.Visible = true;
    //    }


    //    else if (txtTheoryDurationam.Text.ToString() == "" && ddlExamType.SelectedItem.Text.ToString() == "All" && ddlExmType.SelectedItem.Text.ToString() == "All")
    //    {
    //        lblerror.Text = "Select The Theory Time [AM]";
    //        lblerror.Visible = true;
    //    }
    //    else if (txtTheoryDurationpm.Text.ToString() == "" && ddlExamType.SelectedItem.Text.ToString() == "All" && ddlExmType.SelectedItem.Text.ToString() == "All")
    //    {
    //        lblerror.Text = "Select The Theory Time [PM]";
    //        lblerror.Visible = true;
    //    }
    //    else if (txtpracdurationam.Text.ToString() == "" && ddlExamType.SelectedItem.Text.ToString() == "All" && ddlExmType.SelectedItem.Text.ToString() == "All")
    //    {
    //        lblerror.Text = "Select The Practical Time [AM]";
    //        lblerror.Visible = true;
    //    }
    //    else if (txtpracdurationpm.Text.ToString() == "" && ddlExamType.SelectedItem.Text.ToString() == "All" && ddlExmType.SelectedItem.Text.ToString() == "All")
    //    {
    //        lblerror.Text = "Select The Practical Time [PM]";
    //        lblerror.Visible = true;
    //    }
    //    else if (ddlExamType.SelectedItem.Text.ToString() == "Regular" && ddlExmType.SelectedItem.Text.ToString() == "All" && ddlExmType.SelectedItem.Text.ToString() == "Theory" && clSession.SelectedItem.Text.ToString() == "AM-Regular" && txtTheoryDurationam.Text.ToString() == "")
    //    {
    //        lblerror.Text = "Select The Theory Time [AM]";
    //        lblerror.Visible = true;
    //    }
    //    else if (ddlExamType.SelectedItem.Text.ToString() == "Regular" && ddlExmType.SelectedItem.Text.ToString() == "All" && ddlExmType.SelectedItem.Text.ToString() == "Theory" && clSession.SelectedItem.Text.ToString() == "AM-Arear" && txtTheoryDurationam.Text.ToString() == "")
    //    {
    //        lblerror.Text = "Select The Theory Time [AM]";
    //        lblerror.Visible = true;
    //    }
    //    else if (ddlExamType.SelectedItem.Text.ToString() == "Regular" && ddlExmType.SelectedItem.Text.ToString() == "All" && ddlExmType.SelectedItem.Text.ToString() == "Practical" && clSession.SelectedItem.Text.ToString() == "AM-Regular" && txtpracdurationam.Text.ToString() == "")
    //    {
    //        lblerror.Text = "Select The Practical Time [AM]";
    //        lblerror.Visible = true;
    //    }
    //    else if (ddlExamType.SelectedItem.Text.ToString() == "Regular" && ddlExmType.SelectedItem.Text.ToString() == "All" && ddlExmType.SelectedItem.Text.ToString() == "Practical" && clSession.SelectedItem.Text.ToString() == "PM-Regular" && txtpracdurationpm.Text.ToString() == "")
    //    {
    //        lblerror.Text = "Select The Practical Time [PM]";
    //        lblerror.Visible = true;
    //    }
    //    else if (ddlExamType.SelectedItem.Text.ToString() == "Regular" && ddlExmType.SelectedItem.Text.ToString() == "All" && ddlExmType.SelectedItem.Text.ToString() == "Theory" && clSession.SelectedItem.Text.ToString() == "AM-Regular" && txtTheoryDurationam.Text.ToString() == "")
    //    {
    //        lblerror.Text = "Select The Theory Time [AM]";
    //        lblerror.Visible = true;
    //    }
    //    else if (ddlExamType.SelectedItem.Text.ToString() == "Regular" && ddlExmType.SelectedItem.Text.ToString() == "All" && ddlExmType.SelectedItem.Text.ToString() == "Theory" && clSession.SelectedItem.Text.ToString() == "PM-Regular" && txtTheoryDurationpm.Text.ToString() == "")
    //    {
    //        lblerror.Text = "Select The Theory Time [PM]";
    //        lblerror.Visible = true;
    //    }
    //    else if (ddlExamType.SelectedItem.Text.ToString() == "Arear" && ddlExmType.SelectedItem.Text.ToString() == "All" && ddlExmType.SelectedItem.Text.ToString() == "Theory" && clSession.SelectedItem.Text.ToString() == "AM-Arear" && txtTheoryDurationam.Text.ToString() == "")
    //    {
    //        lblerror.Text = "Select The Theory Time [AM]";
    //        lblerror.Visible = true;
    //    }

    //    else if (ddlExamType.SelectedItem.Text.ToString() == "Arear" && ddlExmType.SelectedItem.Text.ToString() == "All" && ddlExmType.SelectedItem.Text.ToString() == "Theory" && clSession.SelectedItem.Text.ToString() == "PM-Arear" && txtTheoryDurationpm.Text.ToString() == "")
    //    {
    //        lblerror.Text = "Select The Theory Time [PM]";
    //        lblerror.Visible = true;
    //    }
    //    else if (ddlExamType.SelectedItem.Text.ToString() == "Arear" && ddlExmType.SelectedItem.Text.ToString() == "All" && ddlExmType.SelectedItem.Text.ToString() == "Practical" && clSession.SelectedItem.Text.ToString() == "AM-Arear" && txtpracdurationam.Text.ToString() == "")
    //    {
    //        lblerror.Text = "Select The Practical Time [AM]";
    //        lblerror.Visible = true;
    //    }

    //    else if (ddlExamType.SelectedItem.Text.ToString() == "Arear" && ddlExmType.SelectedItem.Text.ToString() == "All" && ddlExmType.SelectedItem.Text.ToString() == "Practical" && clSession.SelectedItem.Text.ToString() == "PM-Arear" && txtpracdurationpm.Text.ToString() == "")
    //    {
    //        lblerror.Text = "Select The Practical Time [AM]";
    //        lblerror.Visible = true;
    //    }
    //    else if (ddlExamType.SelectedItem.Text.ToString() == "Regular" && ddlExmType.SelectedItem.Text.ToString() == "Theory" && clSession.SelectedItem.Text.ToString() == "AM-Regular" && txtTheoryDurationam.Text.ToString() == "")
    //    {
    //        lblerror.Text = "Select The Theory Time [AM]";
    //        lblerror.Visible = true;
    //    }
    //    else if (ddlExamType.SelectedItem.Text.ToString() == "Regular" && ddlExmType.SelectedItem.Text.ToString() == "Theory" && clSession.SelectedItem.Text.ToString() == "PM-Regular" && txtTheoryDurationpm.Text.ToString() == "")
    //    {
    //        lblerror.Text = "Select The Theory Time [PM]";
    //        lblerror.Visible = true;
    //    }
    //    else if (ddlExamType.SelectedItem.Text.ToString() == "Regular" && ddlExmType.SelectedItem.Text.ToString() == "Practical" && clSession.SelectedItem.Text.ToString() == "AM-Regular" && txtpracdurationam.Text.ToString() == "")
    //    {
    //        lblerror.Text = "Select The Practical Time [AM]";
    //        lblerror.Visible = true;
    //    }
    //    else if (ddlExamType.SelectedItem.Text.ToString() == "Regular" && ddlExmType.SelectedItem.Text.ToString() == "Practical" && clSession.SelectedItem.Text.ToString() == "PM-Regular" && txtpracdurationpm.Text.ToString() == "")
    //    {
    //        lblerror.Text = "Select The Practical Time [PM]";
    //        lblerror.Visible = true;
    //    }
    //    else if (ddlExamType.SelectedItem.Text.ToString() == "Arear" && ddlExmType.SelectedItem.Text.ToString() == "Theory" && clSession.SelectedItem.Text.ToString() == "AM-Arear" && txtTheoryDurationam.Text.ToString() == "")
    //    {
    //        lblerror.Text = "Select The Theory Time [AM]";
    //        lblerror.Visible = true;
    //    }
    //    else if (ddlExamType.SelectedItem.Text.ToString() == "Arear" && ddlExmType.SelectedItem.Text.ToString() == "Theory" && clSession.SelectedItem.Text.ToString() == "PM-Arear" && txtTheoryDurationpm.Text.ToString() == "")
    //    {
    //        lblerror.Text = "Select The Theory Time [PM]";
    //        lblerror.Visible = true;
    //    }
    //    else if (ddlExamType.SelectedItem.Text.ToString() == "Arear" && ddlExmType.SelectedItem.Text.ToString() == "Practical" && clSession.SelectedItem.Text.ToString() == "AM-Arear" && txtpracdurationam.Text.ToString() == "")
    //    {
    //        lblerror.Text = "Select The Practical Time [AM]";
    //        lblerror.Visible = true;
    //    }
    //    else if (ddlExamType.SelectedItem.Text.ToString() == "Arear" && ddlExmType.SelectedItem.Text.ToString() == "Practical" && clSession.SelectedItem.Text.ToString() == "PM-Arear" && txtpracdurationpm.Text.ToString() == "")
    //    {
    //        lblerror.Text = "Select The Practical Time [PM]";
    //        lblerror.Visible = true;
    //    }
    //    else
    //    {
    //        lblerror.Visible = false;
    //        Generate();
    //        if (lblerror.Visible == false)
    //        {
    //            //        string Msg2;
    //            //        Msg2 = "alert('Time Table Generated successfully')";
    //            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), Msg2, true);
    //            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Time Table Generated successfully')", true);
    //        }


    //    }



    //}
    //public void Generate()
    //{
    //    Connection();
    //    SqlCommand cmd = new SqlCommand("ProcExamTimeTableCreateDummyTable", con);
    //    cmd.CommandType = CommandType.StoredProcedure;
    //    cmd.ExecuteNonQuery();

    //    //Fpstudents.SaveChanges();
    //    int count = 0;
    //    string DegreeCode;
    //    string BatchTo;
    //    string SubjectNo;
    //    string SubjectName;
    //    string ExamType = "Univ";
    //    string Semester;
    //    string temp = "";
    //    string degree;
    //    string WorkingDays = "";
    //    string[] workdays;
    //    int workcount = 0;
    //    string Type = "";
    //    string Proc = "";


    //    CollegeCode = Session["CollegeCode"].ToString();

    //    string TheoryStarttimeAm;
    //    string TheoryendttimeAm;
    //    string TheoryStarttimePm;
    //    string TheoryendttimePm;
    //    string PracStarttimeAm;
    //    string pracendttimeAm;
    //    string PracStarttimePm;
    //    string pracendttimePm;

    //    PracStarttimeAm = ddlpracstarttimeamHrs.SelectedItem.Text.ToString() + ":" + ddlpracstarttimeamMin.SelectedItem.Text.ToString() + "  " + "AM";
    //    pracendttimeAm = ddlpracendtimeamHrs.SelectedItem.Text.ToString() + ":" + ddlpracendtimeamMin.SelectedItem.Text.ToString() + "  " + ddlpracendtimeAmPm.SelectedItem.Text.ToString();
    //    PracStarttimePm = ddlpracstarttimePmHrs.SelectedItem.Text.ToString() + ":" + ddlpracstarttimePmMin.SelectedItem.Text.ToString() + " " + "PM";
    //    pracendttimePm = ddlpracendtimePmHrs.SelectedItem.Text.ToString() + ":" + ddlpracendtimePmMin.SelectedItem.Text.ToString() + " " + "PM";

    //    TheoryStarttimeAm = ddltheoryStartTimeamHrs.SelectedItem.Text.ToString() + ":" + ddltheoryStartTimeamMin.SelectedItem.Text.ToString() + "  " + "AM";
    //    TheoryendttimeAm = ddltheoryendtimeamHrs.SelectedItem.Text.ToString() + ":" + ddltheoryendtimeamMin.SelectedItem.Text.ToString() + "  " + ddltheoryendtimeampm.SelectedItem.Text.ToString();
    //    TheoryStarttimePm = ddlTheoryStartTimeHrsPm.SelectedItem.Text.ToString() + ":" + ddlTheoryStartTimeMinPm.SelectedItem.Text.ToString() + "  " + "PM";
    //    TheoryendttimePm = ddlTheoryEndTimeHrsPm.SelectedItem.Text.ToString() + ":" + ddlTheoryEndTimeMinPm.SelectedItem.Text.ToString() + "  " + ddlTheoryendtimePmam.SelectedItem.Text.ToString();


    //    //for (int i = 1; i < Convert.ToInt16(Fpstudents.Sheets[0].RowCount); i++)
    //    //{

    //    //    if (Convert.ToInt16(Fpstudents.Sheets[0].Cells[i, 9].Value) == 1)
    //    //    {
    //    //        count = count + 1;
    //    //        BatchTo = Fpstudents.Sheets[0].Cells[i, 1].Note.ToString();
    //    //        DegreeCode = Fpstudents.Sheets[0].Cells[i, 3].Note.ToString();
    //    //        Semester = Fpstudents.Sheets[0].Cells[i, 4].Text.ToString();
    //    //       SubjectName = Fpstudents.Sheets[0].Cells[i, 5].Text.ToString();
    //    //        SubjectNo = Fpstudents.Sheets[0].Cells[i, 5].Note.ToString();
    //    //        Type = Fpstudents.Sheets[0].Cells[i, 5].Tag.ToString();

    //    //        if (Type == "AT")
    //    //        {

    //    //            Proc = "ProcExamTimeTableDummyArearTheorySave";
    //    //        }
    //    //        else if (Type == "AP")
    //    //        {

    //    //            Proc = "ProcExamTimeTableDummyArearPracticalSave";
    //    //        }
    //    //        else if (Type == "RT")
    //    //        {

    //    //            Proc = "ProcExamTimeTableDummyRegularTheorySave";
    //    //        }
    //    //        else if (Type == "RP")
    //    //        {
    //    //            Proc = "ProcExamTimeTableDummyRegularPracticalSave";
    //    //        }

    //    //        if (Type != "" && Proc!="")
    //    //        {
    //    //            SqlCommand cd = new SqlCommand(Proc,con);
    //    //            cd.CommandType = CommandType.StoredProcedure;

    //    //            cd.Parameters.AddWithValue("@Batch", BatchTo);
    //    //           cd.Parameters.AddWithValue("@Degree", DegreeCode);
    //    //           cd.Parameters.AddWithValue("@Semester", Semester);
    //    //           cd.Parameters.AddWithValue("@SubjectName", SubjectName);
    //    //          cd.Parameters.AddWithValue("@SubjectNo", SubjectNo);
    //    //            cd.ExecuteNonQuery();
    //    //        }


    //    //           SqlCommand cdex = new SqlCommand("ProcExamTimeTableExamCode", con);
    //    //            cdex.CommandType = CommandType.StoredProcedure;
    //    //            cdex.Parameters.AddWithValue("@degree_code", DegreeCode);
    //    //            cdex.Parameters.AddWithValue("@batchto", BatchTo);
    //    //            cdex.Parameters.AddWithValue("@Exam_Month", ddlMonth.SelectedIndex.ToString());
    //    //            cdex.Parameters.AddWithValue("@Exam_Year", ddlYear.SelectedItem.Text.ToString());
    //    //            cdex.Parameters.AddWithValue("@ExamType", ExamType);
    //    //            cdex.Parameters.AddWithValue("@Semester", Semester);
    //    //            cdex.ExecuteNonQuery();



    //    //    }

    //    //}

    //    if (count < 1)
    //    {
    //        lblerror.Visible = true;
    //        lblerror.Text = "Select The Subject To Genarate Time Table";
    //    }
    //    else
    //    {
    //        lblerror.Visible = false;
    //    }

    //    //Working Days

    //    for (int j = 0; j < cblHolidays.Items.Count; j++)
    //    {
    //        if (cblHolidays.Items[j].Selected == false)
    //        {
    //            string wday;
    //            wday = cblHolidays.Items[j].Text.ToString();
    //            string[] work;
    //            work = wday.Split(new char[] { '-' });
    //            string work1;
    //            work1 = work[1].ToString() + "-" + work[0].ToString() + "-" + work[2].ToString();
    //            if (WorkingDays == "")
    //            {
    //                WorkingDays = work1;
    //            }
    //            else
    //            {
    //                WorkingDays = WorkingDays + "," + work1;

    //            }


    //        }


    //    }
    //    workdays = WorkingDays.Split(new char[] { ',' });
    //    workcount = workdays.Length;
    //    string examdate = "";


    //    if (ddlExamType.SelectedItem.Text.ToString() == "Regular" && ddlExmType.SelectedItem.Text.ToString() == "Theory")
    //    {
    //        int n = 0;
    //        int cnt = 0;

    //        string StartTime = "";
    //        string EndTime = "";
    //        string Duration = "";
    //        string session = "";

    //        if (clSession.SelectedItem.Text.ToString() == "AM-Regular")
    //        {

    //            StartTime = TheoryStarttimeAm;
    //            EndTime = TheoryendttimeAm;
    //            Duration = txtTheoryDurationam.Text.ToString();
    //            session = "F.N";
    //        }
    //        else if (clSession.SelectedItem.Text.ToString() == "PM-Regular")
    //        {
    //            StartTime = TheoryStarttimePm;
    //            EndTime = TheoryendttimePm;
    //            Duration = txtTheoryDurationpm.Text.ToString();
    //            session = "A.N";
    //        }

    //        degree = "select distinct degree,Batch,Semester  from  ExamTimeTableDummyTable order by Semester";
    //        SqlDataAdapter da = new SqlDataAdapter(degree, con);
    //        DataSet ds = new DataSet();
    //        da.Fill(ds);
    //    dd: if (ds.Tables[0].Rows.Count > cnt)
    //        {
    //            string s;
    //            int it = 0;
    //            int dcount = 0;
    //        l: int i = 0;
    //            int emptysubject = 0;
    //            while (i < ds.Tables[0].Rows.Count)
    //            {

    //                string ssr;
    //                ssr = "select exam_code,exam_type from exmtt where Degree_Code='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and batchTo='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "'and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                SqlDataAdapter ssd = new SqlDataAdapter(ssr, con);
    //                DataSet sdss = new DataSet();
    //                ssd.Fill(sdss);
    //                if (sdss.Tables[0].Rows.Count > 0)
    //                {
    //                    s = "select * from ExamTimeTableDummyTable  where  Degree='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and Semester='" + ds.Tables[0].Rows[i]["Semester"].ToString() + "' and Batch='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                    SqlDataAdapter sda = new SqlDataAdapter(s, con);
    //                    DataSet sds = new DataSet();
    //                    sda.Fill(sds);
    //                    if (sds.Tables[0].Rows.Count > 0)
    //                    {
    //                        if (it < sds.Tables[0].Rows.Count)
    //                        {
    //                            SqlCommand cdex = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                            cdex.CommandType = CommandType.StoredProcedure;
    //                            cdex.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                            cdex.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                            SqlDataAdapter daex = new SqlDataAdapter(cdex);
    //                            DataSet dsex = new DataSet();
    //                            daex.Fill(dsex);
    //                            if (dsex.Tables[1].Rows.Count == 0)
    //                            {
    //                                existsubject = 0;
    //                                if (dsex.Tables[0].Rows.Count > dcount)
    //                                {
    //                                    if (dsex.Tables[0].Rows.Count > 1)
    //                                    {
    //                                        for (int dd = 0; dd < dsex.Tables[0].Rows.Count; dd++)
    //                                        {
    //                                            if (workcount <= days)
    //                                            {

    //                                                lblerror.Visible = true;
    //                                                lblerror.Text = "Extend the Working Days";
    //                                                cnt = ds.Tables[0].Rows.Count;
    //                                                goto Err;

    //                                            }
    //                                            examdate = dsex.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                            DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                            DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                            TimeSpan ts1 = exammdate.Subtract(workday);
    //                                            if (ts1.Days == 0)
    //                                            {
    //                                                days = days + 1;

    //                                            }

    //                                        }
    //                                    }
    //                                    else
    //                                    {
    //                                        examdate = dsex.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                        DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                        DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                        TimeSpan ts1 = exammdate.Subtract(workday);
    //                                        if (ts1.Days == 0)
    //                                        {
    //                                            days = days + 1;

    //                                        }
    //                                    }

    //                                    n = 0;
    //                                }

    //                                if (workcount <= days)
    //                                {

    //                                    lblerror.Visible = true;
    //                                    lblerror.Text = "Extend the Working Days";
    //                                    cnt = ds.Tables[0].Rows.Count;
    //                                    goto Err;

    //                                }
    //                                SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                cd.CommandType = CommandType.StoredProcedure;
    //                                cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                cd.Parameters.AddWithValue("@Duration", Duration);
    //                                cd.Parameters.AddWithValue("@StartTime", StartTime);
    //                                cd.Parameters.AddWithValue("@EndTime", EndTime);
    //                                cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                cd.Parameters.AddWithValue("@Session", session);
    //                                cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                cd.ExecuteNonQuery();
    //                                i = i + 1;
    //                                n = n + 1;
    //                            }
    //                            else
    //                            {
    //                                existsubject = 1;
    //                                if (workcount <= days)
    //                                {

    //                                    lblerror.Visible = true;
    //                                    lblerror.Text = "Extend the Working Days";
    //                                    cnt = ds.Tables[0].Rows.Count;
    //                                    goto Err;

    //                                }
    //                                SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                cd.CommandType = CommandType.StoredProcedure;
    //                                cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                cd.Parameters.AddWithValue("@Duration", Duration);
    //                                cd.Parameters.AddWithValue("@StartTime", StartTime);
    //                                cd.Parameters.AddWithValue("@EndTime", EndTime);
    //                                cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                cd.Parameters.AddWithValue("@Session", session);
    //                                cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                cd.ExecuteNonQuery();
    //                                i = i + 1;
    //                            }
    //                        }
    //                        else
    //                        {

    //                            i = i + 1;

    //                            emptysubject = emptysubject + 1;
    //                            if (emptysubject == ds.Tables[0].Rows.Count)
    //                            {
    //                                cnt = ds.Tables[0].Rows.Count;
    //                                goto Err;

    //                            }
    //                        }

    //                    }

    //                    //if (n == 4)
    //                    //{

    //                    //    days = days + 1;
    //                    //    n = 0;

    //                    //}
    //                }
    //            }


    //            it = it + 1;
    //            goto l;



    //        }
    //    }
    //    else if (ddlExamType.SelectedItem.Text.ToString() == "Regular" && ddlExmType.SelectedItem.Text.ToString() == "Practical")
    //    {
    //        int n = 0;
    //        int cnt = 0;

    //        string StartTime = "";
    //        string EndTime = "";
    //        string Duration = "";
    //        string session = "";

    //        if (clSession.SelectedItem.Text.ToString() == "AM-Regular")
    //        {

    //            StartTime = PracStarttimeAm;
    //            EndTime = pracendttimeAm;
    //            Duration = txtpracdurationam.Text.ToString();
    //            session = "F.N";
    //        }
    //        else if (clSession.SelectedItem.Text.ToString() == "PM-Regular")
    //        {
    //            StartTime = PracStarttimePm;
    //            EndTime = pracendttimePm;
    //            Duration = txtpracdurationpm.Text.ToString();
    //            session = "A.N";
    //        }

    //        degree = "select distinct degree,Batch,Semester  from  ExamTimeTableDummyPracTable order by Semester,degree";
    //        SqlDataAdapter da = new SqlDataAdapter(degree, con);
    //        DataSet ds = new DataSet();
    //        da.Fill(ds);
    //    dd: if (ds.Tables[0].Rows.Count > cnt)
    //        {
    //            string s;
    //            int it = 0;

    //        l: int i = 0;
    //            int emptysubject = 0;
    //            while (i < ds.Tables[0].Rows.Count)
    //            {

    //                string ssr;
    //                ssr = "select exam_code,exam_type from exmtt where Degree_Code='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and batchTo='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                SqlDataAdapter ssd = new SqlDataAdapter(ssr, con);
    //                DataSet sdss = new DataSet();
    //                ssd.Fill(sdss);
    //                if (sdss.Tables[0].Rows.Count > 0)
    //                {
    //                    s = "select * from ExamTimeTableDummyPracTable  where  Degree='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and Semester='" + ds.Tables[0].Rows[i]["Semester"].ToString() + "' and Batch='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                    SqlDataAdapter sda = new SqlDataAdapter(s, con);
    //                    DataSet sds = new DataSet();
    //                    sda.Fill(sds);
    //                    if (sds.Tables[0].Rows.Count > 0)
    //                    {
    //                        if (it < sds.Tables[0].Rows.Count)
    //                        {
    //                            SqlCommand cdex = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                            cdex.CommandType = CommandType.StoredProcedure;
    //                            cdex.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                            cdex.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                            SqlDataAdapter daex = new SqlDataAdapter(cdex);
    //                            DataSet dsex = new DataSet();
    //                            daex.Fill(dsex);
    //                            if (dsex.Tables[1].Rows.Count == 0)
    //                            {
    //                                existsubject = 0;
    //                                if (dsex.Tables[0].Rows.Count > 0)
    //                                {
    //                                    if (dsex.Tables[0].Rows.Count > 1)
    //                                    {
    //                                        for (int dd = 0; dd < dsex.Tables[0].Rows.Count; dd++)
    //                                        {

    //                                            if (workcount <= days)
    //                                            {

    //                                                lblerror.Visible = true;
    //                                                lblerror.Text = "Extend the Working Days";
    //                                                cnt = ds.Tables[0].Rows.Count;
    //                                                goto Err;

    //                                            }
    //                                            examdate = dsex.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                            DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                            DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                            TimeSpan ts1 = exammdate.Subtract(workday);
    //                                            if (ts1.Days == 0)
    //                                            {
    //                                                days = days + 1;

    //                                            }

    //                                        }
    //                                    }
    //                                    else
    //                                    {
    //                                        examdate = dsex.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                        DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                        DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                        TimeSpan ts1 = exammdate.Subtract(workday);
    //                                        if (ts1.Days == 0)
    //                                        {
    //                                            days = days + 1;

    //                                        }
    //                                    }

    //                                    n = 0;
    //                                }

    //                                if (workcount <= days)
    //                                {

    //                                    lblerror.Visible = true;
    //                                    lblerror.Text = "Extend the Working Days";
    //                                    cnt = ds.Tables[0].Rows.Count;
    //                                    goto Err;

    //                                }
    //                                SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                cd.CommandType = CommandType.StoredProcedure;
    //                                cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                cd.Parameters.AddWithValue("@Duration", Duration);
    //                                cd.Parameters.AddWithValue("@StartTime", StartTime);
    //                                cd.Parameters.AddWithValue("@EndTime", EndTime);
    //                                cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                cd.Parameters.AddWithValue("@Session", session);
    //                                cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                cd.ExecuteNonQuery();
    //                                i = i + 1;
    //                                n = n + 1;
    //                            }
    //                            else
    //                            {
    //                                existsubject = 1;
    //                                if (workcount <= days)
    //                                {

    //                                    lblerror.Visible = true;
    //                                    lblerror.Text = "Extend the Working Days";
    //                                    cnt = ds.Tables[0].Rows.Count;
    //                                    goto Err;

    //                                }
    //                                SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                cd.CommandType = CommandType.StoredProcedure;
    //                                cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                cd.Parameters.AddWithValue("@Duration", Duration);
    //                                cd.Parameters.AddWithValue("@StartTime", StartTime);
    //                                cd.Parameters.AddWithValue("@EndTime", EndTime);
    //                                cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                cd.Parameters.AddWithValue("@Session", session);
    //                                cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                cd.ExecuteNonQuery();
    //                                i = i + 1;
    //                            }
    //                        }
    //                        else
    //                        {

    //                            i = i + 1;

    //                            emptysubject = emptysubject + 1;
    //                            if (emptysubject == ds.Tables[0].Rows.Count)
    //                            {
    //                                cnt = ds.Tables[0].Rows.Count;
    //                                goto dd;

    //                            }
    //                        }
    //                    }

    //                    //if (n == 4)
    //                    //{

    //                    //    days = days + 1;
    //                    //    n = 0;

    //                    //}
    //                }
    //            }


    //            it = it + 1;
    //            goto l;



    //        }
    //    }

    //    else if (ddlExamType.SelectedItem.Text.ToString() == "Arear" && ddlExmType.SelectedItem.Text.ToString() == "Theory")
    //    {
    //        int n = 0;
    //        int cnt = 0;

    //        string StartTime = "";
    //        string EndTime = "";
    //        string Duration = "";
    //        string session = "";

    //        if (clSession.SelectedItem.Text.ToString() == "AM-Arear")
    //        {

    //            StartTime = TheoryStarttimeAm;
    //            EndTime = TheoryendttimeAm;
    //            Duration = txtTheoryDurationam.Text.ToString();
    //            session = "F.N";
    //        }
    //        else if (clSession.SelectedItem.Text.ToString() == "PM-Arear")
    //        {
    //            StartTime = TheoryStarttimePm;
    //            EndTime = TheoryendttimePm;
    //            Duration = txtTheoryDurationpm.Text.ToString();
    //            session = "A.N";
    //        }

    //        degree = "select  degree,Batch,Semester  from  ExamTimeTableDummyArearTable order by Degree";
    //        SqlDataAdapter da = new SqlDataAdapter(degree, con);
    //        DataSet ds = new DataSet();
    //        da.Fill(ds);
    //    dd: if (ds.Tables[0].Rows.Count > cnt)
    //        {
    //            string s;
    //            int it = 0;

    //        l: int i = 0;
    //            int emptysubject = 0;
    //            while (i < ds.Tables[0].Rows.Count)
    //            {

    //                string ssr;
    //                ssr = "select exam_code,exam_type from exmtt where Degree_Code='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and batchTo='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                SqlDataAdapter ssd = new SqlDataAdapter(ssr, con);
    //                DataSet sdss = new DataSet();
    //                ssd.Fill(sdss);
    //                if (sdss.Tables[0].Rows.Count > 0)
    //                {
    //                    s = "select * from ExamTimeTableDummyArearTable  where  Degree='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and Batch='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                    SqlDataAdapter sda = new SqlDataAdapter(s, con);
    //                    DataSet sds = new DataSet();
    //                    sda.Fill(sds);
    //                    if (sds.Tables[0].Rows.Count > 0)
    //                    {
    //                        if (it < sds.Tables[0].Rows.Count)
    //                        {
    //                            SqlCommand cdex = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                            cdex.CommandType = CommandType.StoredProcedure;
    //                            cdex.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                            cdex.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                            SqlDataAdapter daex = new SqlDataAdapter(cdex);
    //                            DataSet dsex = new DataSet();
    //                            daex.Fill(dsex);
    //                            if (dsex.Tables[1].Rows.Count == 0)
    //                            {
    //                                existsubject = 0;
    //                                if (dsex.Tables[0].Rows.Count > 0)
    //                                {
    //                                    if (dsex.Tables[0].Rows.Count > 1)
    //                                    {
    //                                        for (int dd = 0; dd < dsex.Tables[0].Rows.Count; dd++)
    //                                        {

    //                                            if (workcount <= days)
    //                                            {

    //                                                lblerror.Visible = true;
    //                                                lblerror.Text = "Extend the Working Days";
    //                                                cnt = ds.Tables[0].Rows.Count;
    //                                                goto Err;

    //                                            }
    //                                            examdate = dsex.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                            DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                            DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                            TimeSpan ts1 = exammdate.Subtract(workday);
    //                                            if (ts1.Days == 0)
    //                                            {
    //                                                days = days + 1;

    //                                            }

    //                                        }
    //                                    }
    //                                    else
    //                                    {
    //                                        examdate = dsex.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                        DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                        DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                        TimeSpan ts1 = exammdate.Subtract(workday);
    //                                        if (ts1.Days == 0)
    //                                        {
    //                                            days = days + 1;

    //                                        }
    //                                    }

    //                                    n = 0;
    //                                }


    //                                if (workcount <= days)
    //                                {

    //                                    lblerror.Visible = true;
    //                                    lblerror.Text = "Extend the Working Days";
    //                                    cnt = ds.Tables[0].Rows.Count;
    //                                    goto Err;

    //                                }
    //                                SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                cd.CommandType = CommandType.StoredProcedure;
    //                                cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                cd.Parameters.AddWithValue("@Duration", Duration);
    //                                cd.Parameters.AddWithValue("@StartTime", StartTime);
    //                                cd.Parameters.AddWithValue("@EndTime", EndTime);
    //                                cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                cd.Parameters.AddWithValue("@Session", session);
    //                                cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                cd.ExecuteNonQuery();
    //                                i = i + 1;
    //                                n = n + 1;
    //                            }

    //                            else
    //                            {
    //                                existsubject = 1;
    //                                if (workcount <= days)
    //                                {

    //                                    lblerror.Visible = true;
    //                                    lblerror.Text = "Extend the Working Days";
    //                                    cnt = ds.Tables[0].Rows.Count;
    //                                    goto Err;

    //                                }
    //                                SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                cd.CommandType = CommandType.StoredProcedure;
    //                                cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                cd.Parameters.AddWithValue("@Duration", Duration);
    //                                cd.Parameters.AddWithValue("@StartTime", StartTime);
    //                                cd.Parameters.AddWithValue("@EndTime", EndTime);
    //                                cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                cd.Parameters.AddWithValue("@Session", session);
    //                                cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                cd.ExecuteNonQuery();
    //                                i = i + 1;
    //                            }
    //                        }
    //                        else
    //                        {

    //                            i = i + 1;

    //                            emptysubject = emptysubject + 1;
    //                            if (emptysubject == ds.Tables[0].Rows.Count)
    //                            {
    //                                cnt = ds.Tables[0].Rows.Count;
    //                                goto Err;

    //                            }
    //                        }
    //                    }

    //                    //if (n == 4)
    //                    //{

    //                    //    days = days + 1;
    //                    //    n = 0;

    //                    //}
    //                }
    //            }


    //            it = it + 1;
    //            goto l;



    //        }
    //    }
    //    else if (ddlExamType.SelectedItem.Text.ToString() == "Arear" && ddlExmType.SelectedItem.Text.ToString() == "Practical")
    //    {
    //        int n = 0;
    //        int cnt = 0;

    //        string StartTime = "";
    //        string EndTime = "";
    //        string Duration = "";
    //        string session = "";

    //        if (clSession.SelectedItem.Text.ToString() == "AM-Arear")
    //        {

    //            StartTime = PracStarttimeAm;
    //            EndTime = pracendttimeAm;
    //            Duration = txtpracdurationam.Text.ToString();
    //            session = "F.N";
    //        }
    //        else if (clSession.SelectedItem.Text.ToString() == "PM-Arear")
    //        {
    //            StartTime = PracStarttimePm;
    //            EndTime = pracendttimePm;
    //            Duration = txtpracdurationpm.Text.ToString();
    //            session = "A.N";
    //        }

    //        degree = "select  degree,Batch,Semester  from  ExamTimeTableDummyArearPracTable order by degree";
    //        SqlDataAdapter da = new SqlDataAdapter(degree, con);
    //        DataSet ds = new DataSet();
    //        da.Fill(ds);
    //    dd: if (ds.Tables[0].Rows.Count > cnt)
    //        {
    //            string s;
    //            int it = 0;

    //        l: int i = 0;
    //            int emptysubject = 0;
    //            while (i < ds.Tables[0].Rows.Count)
    //            {

    //                string ssr;
    //                ssr = "select exam_code,exam_type from exmtt where Degree_Code='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and batchTo='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                SqlDataAdapter ssd = new SqlDataAdapter(ssr, con);
    //                DataSet sdss = new DataSet();
    //                ssd.Fill(sdss);
    //                if (sdss.Tables[0].Rows.Count > 0)
    //                {
    //                    s = "select * from ExamTimeTableDummyArearPracTable  where  Degree='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and Batch='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                    SqlDataAdapter sda = new SqlDataAdapter(s, con);
    //                    DataSet sds = new DataSet();
    //                    sda.Fill(sds);
    //                    if (sds.Tables[0].Rows.Count > 0)
    //                    {
    //                        if (it < sds.Tables[0].Rows.Count)
    //                        {
    //                            SqlCommand cdex = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                            cdex.CommandType = CommandType.StoredProcedure;
    //                            cdex.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                            cdex.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                            SqlDataAdapter daex = new SqlDataAdapter(cdex);
    //                            DataSet dsex = new DataSet();
    //                            daex.Fill(dsex);
    //                            if (dsex.Tables[1].Rows.Count == 0)
    //                            {
    //                                existsubject = 0;
    //                                if (dsex.Tables[0].Rows.Count > 0)
    //                                {
    //                                    if (dsex.Tables[0].Rows.Count > 1)
    //                                    {
    //                                        for (int dd = 0; dd < dsex.Tables[0].Rows.Count; dd++)
    //                                        {
    //                                            if (workcount <= days)
    //                                            {

    //                                                lblerror.Visible = true;
    //                                                lblerror.Text = "Extend the Working Days";
    //                                                cnt = ds.Tables[0].Rows.Count;
    //                                                goto Err;

    //                                            }
    //                                            examdate = dsex.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                            DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                            DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                            TimeSpan ts1 = exammdate.Subtract(workday);
    //                                            if (ts1.Days == 0)
    //                                            {
    //                                                days = days + 1;

    //                                            }

    //                                        }
    //                                    }
    //                                    else
    //                                    {

    //                                        examdate = dsex.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                        DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                        DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                        TimeSpan ts1 = exammdate.Subtract(workday);
    //                                        if (ts1.Days == 0)
    //                                        {
    //                                            days = days + 1;

    //                                        }
    //                                    }

    //                                    n = 0;
    //                                }

    //                                if (workcount <= days)
    //                                {

    //                                    lblerror.Visible = true;
    //                                    lblerror.Text = "Extend the Working Days";
    //                                    cnt = ds.Tables[0].Rows.Count;
    //                                    goto Err;

    //                                }
    //                                SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                cd.CommandType = CommandType.StoredProcedure;
    //                                cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                cd.Parameters.AddWithValue("@Duration", Duration);
    //                                cd.Parameters.AddWithValue("@StartTime", StartTime);
    //                                cd.Parameters.AddWithValue("@EndTime", EndTime);
    //                                cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                cd.Parameters.AddWithValue("@Session", session);
    //                                cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                cd.ExecuteNonQuery();
    //                                i = i + 1;
    //                                n = n + 1;
    //                            }
    //                            else
    //                            {
    //                                existsubject = 1;


    //                                if (workcount <= days)
    //                                {

    //                                    lblerror.Visible = true;
    //                                    lblerror.Text = "Extend the Working Days";
    //                                    cnt = ds.Tables[0].Rows.Count;
    //                                    goto Err;

    //                                }
    //                                SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                cd.CommandType = CommandType.StoredProcedure;
    //                                cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                cd.Parameters.AddWithValue("@Duration", Duration);
    //                                cd.Parameters.AddWithValue("@StartTime", StartTime);
    //                                cd.Parameters.AddWithValue("@EndTime", EndTime);
    //                                cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                cd.Parameters.AddWithValue("@Session", session);
    //                                cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                cd.ExecuteNonQuery();
    //                                i = i + 1;

    //                            }
    //                        }

    //                        else
    //                        {

    //                            i = i + 1;

    //                            emptysubject = emptysubject + 1;
    //                            if (emptysubject == ds.Tables[0].Rows.Count)
    //                            {
    //                                cnt = ds.Tables[0].Rows.Count;
    //                                goto dd;

    //                            }
    //                        }
    //                    }

    //                    //if (n == 4)
    //                    //{

    //                    //    days = days + 1;
    //                    //    n = 0;

    //                    //}
    //                }
    //            }



    //            it = it + 1;
    //            goto l;



    //        }
    //    }

    //    else if (ddlExamType.SelectedItem.Text.ToString() == "All" && ddlExmType.SelectedItem.Text.ToString() == "Theory")
    //    {
    //        string RTheoryStartTime = "0:00";
    //        string RTheoryEndTime = "0:00";
    //        string RTheoryDuration = "0:00";
    //        string RExamSession = "";
    //        string AExamSession = "";
    //        string ATheoryStartTime = "0:00";
    //        string ATheoryEndTime = "0:00";
    //        string ATheoryDuration = "0:00";


    //        string session = "";
    //        string[] sessen;
    //        for (int k = 0; k < clSession.Items.Count; k++)
    //        {
    //            if (clSession.Items[k].Selected == true)
    //            {
    //                if (session == "")
    //                {
    //                    session = clSession.Items[k].Text.ToString();
    //                }
    //                else
    //                {
    //                    session = session + "," + clSession.Items[k].Text.ToString();
    //                }
    //            }

    //        }

    //        sessen = session.Split(new char[] { ',' });


    //        if (sessen.Length > 0)
    //        {
    //            int s1 = 0;

    //            if (sessen.Length == 2)
    //            {

    //                if ((sessen[s1].ToString() == "AM-Regular" && sessen[s1 + 1].ToString() == "PM-Arear"))
    //                {

    //                    RTheoryStartTime = TheoryStarttimeAm;
    //                    RTheoryEndTime = TheoryendttimeAm;
    //                    RTheoryDuration = txtTheoryDurationam.Text.ToString();
    //                    RExamSession = "F.N";

    //                    ATheoryStartTime = TheoryStarttimePm;
    //                    ATheoryEndTime = TheoryendttimePm;
    //                    ATheoryDuration = txtTheoryDurationpm.Text.ToString();

    //                    AExamSession = "A.N";

    //                }
    //                else if (sessen[s1].ToString() == "PM-Regular" && sessen[s1 + 1].ToString() == "AM-Arear")
    //                {

    //                    RTheoryStartTime = TheoryStarttimePm;
    //                    RTheoryEndTime = TheoryendttimePm;
    //                    RTheoryDuration = txtTheoryDurationpm.Text.ToString();
    //                    RExamSession = "A.N";

    //                    ATheoryStartTime = TheoryStarttimeAm;
    //                    ATheoryEndTime = TheoryendttimeAm;
    //                    ATheoryDuration = txtTheoryDurationam.Text.ToString();
    //                    AExamSession = "F.N";

    //                }
    //                else if ((sessen[s1].ToString() == "PM-Regular" && sessen[s1 + 1].ToString() == "PM-Arear"))
    //                {

    //                    RTheoryStartTime = TheoryStarttimePm;
    //                    RTheoryEndTime = TheoryendttimePm;
    //                    RTheoryDuration = txtTheoryDurationpm.Text.ToString();
    //                    RExamSession = "A.N";

    //                    ATheoryStartTime = TheoryStarttimePm;
    //                    ATheoryEndTime = TheoryendttimePm;
    //                    ATheoryDuration = txtTheoryDurationpm.Text.ToString();
    //                    AExamSession = "A.N";

    //                }
    //                else if ((sessen[s1].ToString() == "AM-Regular" && sessen[s1 + 1].ToString() == "AM-Arear"))
    //                {

    //                    RTheoryStartTime = TheoryStarttimeAm;
    //                    RTheoryEndTime = TheoryendttimeAm;
    //                    RTheoryDuration = txtTheoryDurationam.Text.ToString();
    //                    RExamSession = "F.N";

    //                    ATheoryStartTime = TheoryStarttimeAm;
    //                    ATheoryEndTime = TheoryendttimeAm;
    //                    ATheoryDuration = txtTheoryDurationam.Text.ToString();
    //                    AExamSession = "F.N";

    //                }
    //            }
    //            else if (clSession.SelectedItem.Text.ToString() == "Mixed")
    //            {
    //                string t1 = "";
    //                string t2 = "";

    //                int day;
    //                day = 0;
    //                string ASession = "A.N";
    //                string FSession = "F.N";
    //                if (ddlExamstartswith.SelectedItem.Text.ToString() == "Regular")
    //                {
    //                    t1 = "ExamTimeTableDummyTable";
    //                    t2 = "ExamTimeTableDummyArearTable";
    //                }
    //                else if (ddlExamstartswith.SelectedItem.Text.ToString() == "Arear")
    //                {
    //                    t1 = "ExamTimeTableDummyArearTable";
    //                    t2 = "ExamTimeTableDummyTable";
    //                }

    //                int n = 0;
    //                int cnt = 0;
    //                degree = "";
    //                if (t1 == "ExamTimeTableDummyArearTable")
    //                {
    //                    degree = "select  degree,Batch,Semester  from  " + t1 + " ";
    //                }
    //                else if (t1 == "ExamTimeTableDummyArearPracTable")
    //                {

    //                    degree = "select  degree,Batch,Semester  from  " + t1 + " ";
    //                }
    //                else
    //                {
    //                    degree = "select distinct degree,Batch,Semester  from  " + t1 + " ";

    //                }

    //                SqlDataAdapter da = new SqlDataAdapter(degree, con);
    //                DataSet ds = new DataSet();
    //                da.Fill(ds);
    //            dd: if (ds.Tables[0].Rows.Count > cnt)
    //                {


    //                    string s = "";
    //                    int it = 0;

    //                l: int i = 0;
    //                    int emptysubject = 0;
    //                    while (i < ds.Tables[0].Rows.Count)
    //                    {

    //                        string ssr;
    //                        ssr = "select exam_code,exam_type from exmtt where Degree_Code='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and batchTo='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd = new SqlDataAdapter(ssr, con);
    //                        DataSet sdss = new DataSet();
    //                        ssd.Fill(sdss);
    //                        if (sdss.Tables[0].Rows.Count > 0)
    //                        {
    //                            if (t1 == "ExamTimeTableDummyArearTable")
    //                            {
    //                                s = "select * from " + t1 + "  where Degree='" + ds.Tables[0].Rows[i]["Degree"].ToString() + " ' and Batch='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                            }
    //                            else if (t1 == "ExamTimeTableDummyArearPracTable")
    //                            {

    //                                s = "select * from " + t1 + "  where Degree='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and Batch='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                            }
    //                            else
    //                            {
    //                                s = "select * from " + t1 + "  where Degree='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "'  and  Semester='" + ds.Tables[0].Rows[i]["Semester"].ToString() + " ' and Batch='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "'";

    //                            }

    //                            SqlDataAdapter sda = new SqlDataAdapter(s, con);
    //                            DataSet sds = new DataSet();
    //                            sda.Fill(sds);
    //                            if (sds.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it < sds.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex.CommandType = CommandType.StoredProcedure;
    //                                    cdex.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex = new SqlDataAdapter(cdex);
    //                                    DataSet dsex = new DataSet();
    //                                    daex.Fill(dsex);
    //                                    if (dsex.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt = ds.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0 && FSession == dsex.Tables[0].Rows[dd]["ExamSession"].ToString())
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0 && FSession == dsex.Tables[0].Rows[0]["ExamSession"].ToString())
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }

    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt = ds.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd.CommandType = CommandType.StoredProcedure;
    //                                        cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                        cd.Parameters.AddWithValue("@Duration", txtTheoryDurationam.Text.ToString());
    //                                        cd.Parameters.AddWithValue("@StartTime", TheoryStarttimeAm);
    //                                        cd.Parameters.AddWithValue("@EndTime", TheoryendttimeAm);
    //                                        cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd.Parameters.AddWithValue("@Session", FSession);
    //                                        cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd.ExecuteNonQuery();
    //                                        i = i + 1;
    //                                        n = n + 1;
    //                                    }

    //                                    else
    //                                    {
    //                                        existsubject = 1;
    //                                        SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd.CommandType = CommandType.StoredProcedure;
    //                                        cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                        cd.Parameters.AddWithValue("@Duration", txtTheoryDurationam.Text.ToString());
    //                                        cd.Parameters.AddWithValue("@StartTime", TheoryStarttimeAm);
    //                                        cd.Parameters.AddWithValue("@EndTime", TheoryendttimeAm);
    //                                        cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd.Parameters.AddWithValue("@Session", FSession);
    //                                        cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd.ExecuteNonQuery();
    //                                        i = i + 1;
    //                                    }
    //                                }
    //                                else
    //                                {

    //                                    i = i + 1;

    //                                    emptysubject = emptysubject + 1;
    //                                    if (emptysubject == ds.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt = ds.Tables[0].Rows.Count;
    //                                        goto dd;

    //                                    }
    //                                }
    //                            }

    //                            //if (n == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n = 0;

    //                            //}
    //                        }
    //                    }

    //                    it = it + 1;
    //                    goto l;

    //                }


    //                string degree3 = "";
    //                int n3 = 0;
    //                int cnt3 = 0;
    //                if (t2 == "ExamTimeTableDummyArearTable")
    //                {
    //                    degree3 = "select  degree,Batch,Semester  from  " + t2 + "";
    //                }
    //                else if (t2 == "ExamTimeTableDummyArearPracTable")
    //                {

    //                    degree3 = "select  degree,Batch,Semester  from  " + t2 + "";
    //                }
    //                else
    //                {
    //                    degree3 = "select distinct degree,Batch,Semester  from  " + t2 + "";

    //                }

    //                SqlDataAdapter da3 = new SqlDataAdapter(degree3, con);
    //                DataSet ds3 = new DataSet();
    //                da3.Fill(ds3);
    //            dd3: if (ds3.Tables[0].Rows.Count > cnt3)
    //                {


    //                    string s3 = "";
    //                    int it3 = 0;

    //                l3: int i3 = 0;
    //                    int emptysubject3 = 0;
    //                    while (i3 < ds3.Tables[0].Rows.Count)
    //                    {

    //                        string ssr3;
    //                        ssr3 = "select exam_code,exam_type from exmtt where Degree_Code='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and batchTo='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd3 = new SqlDataAdapter(ssr3, con);
    //                        DataSet sdss3 = new DataSet();
    //                        ssd3.Fill(sdss3);
    //                        if (sdss3.Tables[0].Rows.Count > 0)
    //                        {
    //                            if (t2 == "ExamTimeTableDummyArearTable")
    //                            {
    //                                s3 = "select * from " + t2 + "  where Degree='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and Batch='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "'";
    //                            }
    //                            else if (t2 == "ExamTimeTableDummyArearPracTable")
    //                            {

    //                                s3 = "select * from " + t2 + "  where Degree='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and Batch='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "'";
    //                            }
    //                            else
    //                            {
    //                                s3 = "select * from " + t2 + "  where Degree='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and  Semester='" + ds3.Tables[0].Rows[i3]["Semester"].ToString() + " ' and Batch='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "'";

    //                            }

    //                            SqlDataAdapter sda3 = new SqlDataAdapter(s3, con);
    //                            DataSet sds3 = new DataSet();
    //                            sda3.Fill(sds3);
    //                            if (sds3.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it3 < sds3.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex3 = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex3.CommandType = CommandType.StoredProcedure;
    //                                    cdex3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex3 = new SqlDataAdapter(cdex3);
    //                                    DataSet dsex3 = new DataSet();
    //                                    daex3.Fill(dsex3);
    //                                    if (dsex3.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex3.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex3.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex3.Tables[0].Rows.Count; dd++)
    //                                                {

    //                                                    if (workcount <= day)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt3 = ds3.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex3.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[day].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0 && ASession == dsex3.Tables[0].Rows[dd]["ExamSession"].ToString())
    //                                                    {
    //                                                        day = day + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex3.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[day].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0 && ASession == dsex3.Tables[0].Rows[0]["ExamSession"].ToString())
    //                                                {
    //                                                    day = day + 1;

    //                                                }
    //                                            }


    //                                        }

    //                                        if (workcount <= day)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt3 = ds3.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd3.CommandType = CommandType.StoredProcedure;
    //                                        cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                        cd3.Parameters.AddWithValue("@Duration", txtTheoryDurationpm.Text.ToString());
    //                                        cd3.Parameters.AddWithValue("@StartTime", TheoryStarttimePm);
    //                                        cd3.Parameters.AddWithValue("@EndTime", TheoryendttimePm);
    //                                        cd3.Parameters.AddWithValue("@ExamDate", workdays[day].ToString());
    //                                        cd3.Parameters.AddWithValue("@Session", ASession);
    //                                        cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd3.ExecuteNonQuery();
    //                                        i3 = i3 + 1;
    //                                        n3 = n3 + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        existsubject = 1;
    //                                        SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd3.CommandType = CommandType.StoredProcedure;
    //                                        cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                        cd3.Parameters.AddWithValue("@Duration", txtTheoryDurationpm.Text.ToString());
    //                                        cd3.Parameters.AddWithValue("@StartTime", TheoryStarttimePm);
    //                                        cd3.Parameters.AddWithValue("@EndTime", TheoryendttimePm);
    //                                        cd3.Parameters.AddWithValue("@ExamDate", workdays[day].ToString());
    //                                        cd3.Parameters.AddWithValue("@Session", ASession);
    //                                        cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd3.ExecuteNonQuery();
    //                                        i3 = i3 + 1;
    //                                    }
    //                                }
    //                                else
    //                                {

    //                                    i3 = i3 + 1;

    //                                    emptysubject3 = emptysubject3 + 1;
    //                                    if (emptysubject3 == ds3.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt3 = ds3.Tables[0].Rows.Count;
    //                                        goto dd3;

    //                                    }
    //                                }
    //                            }

    //                            //if (n3 == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n3 = 0;

    //                            //}
    //                        }
    //                    }


    //                    it3 = it3 + 1;
    //                    goto l3;



    //                }
    //            }
    //        }

    //        if (clSession.SelectedItem.Text.ToString() != "Mixed")
    //        {
    //            if (ddlExamstartswith.SelectedItem.Text.ToString() == "Regular")
    //            {

    //                int n = 0;
    //                int cnt = 0;

    //                degree = "select distinct degree,Batch,Semester  from  ExamTimeTableDummyTable ";
    //                SqlDataAdapter da = new SqlDataAdapter(degree, con);
    //                DataSet ds = new DataSet();
    //                da.Fill(ds);
    //            dd: if (ds.Tables[0].Rows.Count > cnt)
    //                {


    //                    string s;
    //                    int it = 0;

    //                l: int i = 0;
    //                    int emptysubject = 0;
    //                    while (i < ds.Tables[0].Rows.Count)
    //                    {

    //                        string ssr;
    //                        ssr = "select exam_code,exam_type from exmtt where Degree_Code='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and batchTo='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd = new SqlDataAdapter(ssr, con);
    //                        DataSet sdss = new DataSet();
    //                        ssd.Fill(sdss);
    //                        if (sdss.Tables[0].Rows.Count > 0)
    //                        {
    //                            s = "select * from ExamTimeTableDummyTable  where Degree='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "'  and  Semester='" + ds.Tables[0].Rows[i]["Semester"].ToString() + " ' and Batch='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                            SqlDataAdapter sda = new SqlDataAdapter(s, con);
    //                            DataSet sds = new DataSet();
    //                            sda.Fill(sds);
    //                            if (sds.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it < sds.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex.CommandType = CommandType.StoredProcedure;
    //                                    cdex.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex = new SqlDataAdapter(cdex);
    //                                    DataSet dsex = new DataSet();
    //                                    daex.Fill(dsex);
    //                                    if (dsex.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt = ds.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt = ds.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd.CommandType = CommandType.StoredProcedure;
    //                                        cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                        cd.Parameters.AddWithValue("@Duration", RTheoryDuration);
    //                                        cd.Parameters.AddWithValue("@StartTime", RTheoryStartTime);
    //                                        cd.Parameters.AddWithValue("@EndTime", RTheoryEndTime);
    //                                        cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd.ExecuteNonQuery();
    //                                        i = i + 1;
    //                                        n = n + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt = ds.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        existsubject = 1;
    //                                        SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd.CommandType = CommandType.StoredProcedure;
    //                                        cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                        cd.Parameters.AddWithValue("@Duration", RTheoryDuration);
    //                                        cd.Parameters.AddWithValue("@StartTime", RTheoryStartTime);
    //                                        cd.Parameters.AddWithValue("@EndTime", RTheoryEndTime);
    //                                        cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd.ExecuteNonQuery();
    //                                        i = i + 1;
    //                                        n = n + 1;
    //                                    }

    //                                }
    //                                else
    //                                {

    //                                    i = i + 1;

    //                                    emptysubject = emptysubject + 1;
    //                                    if (emptysubject == ds.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt = ds.Tables[0].Rows.Count;
    //                                        goto dd;

    //                                    }
    //                                }
    //                            }

    //                            //if (n == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n = 0;

    //                            //}
    //                        }
    //                    }

    //                    it = it + 1;
    //                    goto l;

    //                }

    //                days = days + 1;
    //                string degree3;
    //                int n3 = 0;
    //                int cnt3 = 0;
    //                degree3 = "select  degree,Batch,Semester  from  ExamTimeTableDummyArearTable ";
    //                SqlDataAdapter da3 = new SqlDataAdapter(degree3, con);
    //                DataSet ds3 = new DataSet();
    //                da3.Fill(ds3);
    //            dd3: if (ds3.Tables[0].Rows.Count > cnt3)
    //                {


    //                    string s3;
    //                    int it3 = 0;

    //                l3: int i3 = 0;
    //                    int emptysubject3 = 0;
    //                    while (i3 < ds3.Tables[0].Rows.Count)
    //                    {

    //                        string ssr3;
    //                        ssr3 = "select exam_code,exam_type from exmtt where Degree_Code='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and batchTo='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd3 = new SqlDataAdapter(ssr3, con);
    //                        DataSet sdss3 = new DataSet();
    //                        ssd3.Fill(sdss3);
    //                        if (sdss3.Tables[0].Rows.Count > 0)
    //                        {
    //                            s3 = "select * from ExamTimeTableDummyArearTable  where Degree='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and Batch='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "'";
    //                            SqlDataAdapter sda3 = new SqlDataAdapter(s3, con);
    //                            DataSet sds3 = new DataSet();
    //                            sda3.Fill(sds3);
    //                            if (sds3.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it3 < sds3.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex3 = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex3.CommandType = CommandType.StoredProcedure;
    //                                    cdex3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex3 = new SqlDataAdapter(cdex3);
    //                                    DataSet dsex3 = new DataSet();
    //                                    daex3.Fill(dsex3);
    //                                    if (dsex3.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex3.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex3.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex3.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount == days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt3 = ds3.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex3.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex3.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }

    //                                        if (workcount == days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt3 = ds3.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd3.CommandType = CommandType.StoredProcedure;
    //                                        cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                        cd3.Parameters.AddWithValue("@Duration", ATheoryDuration);
    //                                        cd3.Parameters.AddWithValue("@StartTime", ATheoryStartTime);
    //                                        cd3.Parameters.AddWithValue("@EndTime", ATheoryEndTime);
    //                                        cd3.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd3.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd3.ExecuteNonQuery();
    //                                        i3 = i3 + 1;
    //                                        n3 = n3 + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        if (workcount == days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt = ds.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        existsubject = 1;
    //                                        SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd3.CommandType = CommandType.StoredProcedure;
    //                                        cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                        cd3.Parameters.AddWithValue("@Duration", ATheoryDuration);
    //                                        cd3.Parameters.AddWithValue("@StartTime", ATheoryStartTime);
    //                                        cd3.Parameters.AddWithValue("@EndTime", ATheoryEndTime);
    //                                        cd3.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd3.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd3.ExecuteNonQuery();
    //                                        i3 = i3 + 1;
    //                                    }
    //                                }
    //                                else
    //                                {

    //                                    i3 = i3 + 1;

    //                                    emptysubject3 = emptysubject3 + 1;
    //                                    if (emptysubject3 == ds3.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt3 = ds3.Tables[0].Rows.Count;
    //                                        goto dd3;

    //                                    }
    //                                }
    //                            }

    //                            //if (n3 == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n3 = 0;

    //                            //}
    //                        }
    //                    }


    //                    it3 = it3 + 1;
    //                    goto l3;



    //                }
    //            }

    //            else if (ddlExamstartswith.SelectedItem.Text.ToString() == "Arear")
    //            {

    //                int n = 0;
    //                int cnt = 0;

    //                degree = "select  degree,Batch,Semester  from  ExamTimeTableDummyArearTable ";
    //                SqlDataAdapter da = new SqlDataAdapter(degree, con);
    //                DataSet ds = new DataSet();
    //                da.Fill(ds);
    //            dd: if (ds.Tables[0].Rows.Count > cnt)
    //                {


    //                    string s;
    //                    int it = 0;

    //                l: int i = 0;
    //                    int emptysubject = 0;
    //                    while (i < ds.Tables[0].Rows.Count)
    //                    {

    //                        string ssr;
    //                        ssr = "select exam_code,exam_type from exmtt where Degree_Code='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and batchTo='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd = new SqlDataAdapter(ssr, con);
    //                        DataSet sdss = new DataSet();
    //                        ssd.Fill(sdss);
    //                        if (sdss.Tables[0].Rows.Count > 0)
    //                        {
    //                            s = "select * from ExamTimeTableDummyArearTable  where Degree='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and Batch='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                            SqlDataAdapter sda = new SqlDataAdapter(s, con);
    //                            DataSet sds = new DataSet();
    //                            sda.Fill(sds);
    //                            if (sds.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it < sds.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex.CommandType = CommandType.StoredProcedure;
    //                                    cdex.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex = new SqlDataAdapter(cdex);
    //                                    DataSet dsex = new DataSet();
    //                                    daex.Fill(dsex);
    //                                    if (dsex.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt = ds.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt = ds.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd.CommandType = CommandType.StoredProcedure;
    //                                        cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                        cd.Parameters.AddWithValue("@Duration", ATheoryDuration);
    //                                        cd.Parameters.AddWithValue("@StartTime", ATheoryStartTime);
    //                                        cd.Parameters.AddWithValue("@EndTime", ATheoryEndTime);
    //                                        cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd.ExecuteNonQuery();
    //                                        i = i + 1;
    //                                        n = n + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt = ds.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        existsubject = 1;
    //                                        SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd.CommandType = CommandType.StoredProcedure;
    //                                        cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                        cd.Parameters.AddWithValue("@Duration", ATheoryDuration);
    //                                        cd.Parameters.AddWithValue("@StartTime", ATheoryStartTime);
    //                                        cd.Parameters.AddWithValue("@EndTime", ATheoryEndTime);
    //                                        cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd.ExecuteNonQuery();
    //                                        i = i + 1;

    //                                    }
    //                                }
    //                                else
    //                                {

    //                                    i = i + 1;

    //                                    emptysubject = emptysubject + 1;
    //                                    if (emptysubject == ds.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt = ds.Tables[0].Rows.Count;
    //                                        goto dd;

    //                                    }
    //                                }
    //                            }

    //                            //if (n == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n = 0;

    //                            //}
    //                        }
    //                    }

    //                    it = it + 1;
    //                    goto l;

    //                }

    //                days = days + 1;
    //                string degree3;
    //                int n3 = 0;
    //                int cnt3 = 0;
    //                degree3 = "select distinct degree,Batch,Semester  from  ExamTimeTableDummyTable";
    //                SqlDataAdapter da3 = new SqlDataAdapter(degree3, con);
    //                DataSet ds3 = new DataSet();
    //                da3.Fill(ds3);
    //            dd3: if (ds3.Tables[0].Rows.Count > cnt3)
    //                {


    //                    string s3;
    //                    int it3 = 0;

    //                l3: int i3 = 0;
    //                    int emptysubject3 = 0;
    //                    while (i3 < ds3.Tables[0].Rows.Count)
    //                    {

    //                        string ssr3;
    //                        ssr3 = "select exam_code,exam_type from exmtt where Degree_Code='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and batchTo='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd3 = new SqlDataAdapter(ssr3, con);
    //                        DataSet sdss3 = new DataSet();
    //                        ssd3.Fill(sdss3);
    //                        if (sdss3.Tables[0].Rows.Count > 0)
    //                        {
    //                            s3 = "select * from ExamTimeTableDummyTable  where Degree='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and  Semester='" + ds3.Tables[0].Rows[i3]["Semester"].ToString() + " ' and Batch='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "'";
    //                            SqlDataAdapter sda3 = new SqlDataAdapter(s3, con);
    //                            DataSet sds3 = new DataSet();
    //                            sda3.Fill(sds3);
    //                            if (sds3.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it3 < sds3.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex3 = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex3.CommandType = CommandType.StoredProcedure;
    //                                    cdex3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex3 = new SqlDataAdapter(cdex3);
    //                                    DataSet dsex3 = new DataSet();
    //                                    daex3.Fill(dsex3);
    //                                    if (dsex3.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex3.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex3.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex3.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt = ds.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex3.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex3.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt3 = ds3.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd3.CommandType = CommandType.StoredProcedure;
    //                                        cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                        cd3.Parameters.AddWithValue("@Duration", RTheoryDuration);
    //                                        cd3.Parameters.AddWithValue("@StartTime", RTheoryStartTime);
    //                                        cd3.Parameters.AddWithValue("@EndTime", RTheoryEndTime);
    //                                        cd3.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd3.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd3.ExecuteNonQuery();
    //                                        i3 = i3 + 1;
    //                                        n3 = n3 + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt3 = ds3.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        existsubject = 1;
    //                                        SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd3.CommandType = CommandType.StoredProcedure;
    //                                        cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                        cd3.Parameters.AddWithValue("@Duration", RTheoryDuration);
    //                                        cd3.Parameters.AddWithValue("@StartTime", RTheoryStartTime);
    //                                        cd3.Parameters.AddWithValue("@EndTime", RTheoryEndTime);
    //                                        cd3.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd3.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd3.ExecuteNonQuery();
    //                                        i3 = i3 + 1;
    //                                    }
    //                                }

    //                                else
    //                                {

    //                                    i3 = i3 + 1;

    //                                    emptysubject3 = emptysubject3 + 1;
    //                                    if (emptysubject3 == ds3.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt3 = ds3.Tables[0].Rows.Count;
    //                                        goto Err;

    //                                    }
    //                                }
    //                            }

    //                            //if (n3 == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n3 = 0;

    //                            //}
    //                        }
    //                    }


    //                    it3 = it3 + 1;
    //                    goto l3;



    //                }
    //            }
    //        }
    //    }
    //    else if (ddlExamType.SelectedItem.Text.ToString() == "All" && ddlExmType.SelectedItem.Text.ToString() == "Practical")
    //    {

    //        string RPracStartTime = "0:00";
    //        string RPracEndTime = "0:00";
    //        string RPracDuration = "0:00";
    //        string RExamSession = "";
    //        string AExamSession = "";
    //        string APracStartTime = "0:00";
    //        string APracEndTime = "0:00";
    //        string APracDuration = "0:00";


    //        string session = "";
    //        string[] sessen;
    //        for (int k = 0; k < clSession.Items.Count; k++)
    //        {
    //            if (clSession.Items[k].Selected == true)
    //            {
    //                if (session == "")
    //                {
    //                    session = clSession.Items[k].Text.ToString();
    //                }
    //                else
    //                {
    //                    session = session + "," + clSession.Items[k].Text.ToString();
    //                }
    //            }

    //        }

    //        sessen = session.Split(new char[] { ',' });


    //        if (sessen.Length > 0)
    //        {
    //            int s1 = 0;

    //            if (sessen.Length == 2)
    //            {

    //                if ((sessen[s1].ToString() == "AM-Regular" && sessen[s1 + 1].ToString() == "PM-Arear"))
    //                {

    //                    RPracStartTime = PracStarttimeAm;
    //                    RPracEndTime = pracendttimeAm;
    //                    RPracDuration = txtpracdurationam.Text.ToString();
    //                    RExamSession = "F.N";

    //                    APracStartTime = PracStarttimePm;
    //                    APracEndTime = pracendttimePm;
    //                    APracDuration = txtpracdurationpm.Text.ToString();

    //                    AExamSession = "A.N";

    //                }
    //                else if (sessen[s1].ToString() == "PM-Regular" && sessen[s1 + 1].ToString() == "AM-Arear")
    //                {

    //                    RPracStartTime = PracStarttimePm;
    //                    RPracEndTime = pracendttimePm;
    //                    RPracDuration = txtpracdurationpm.Text.ToString();
    //                    RExamSession = "A.N";

    //                    APracStartTime = PracStarttimeAm;
    //                    APracEndTime = pracendttimeAm;
    //                    APracDuration = txtpracdurationam.Text.ToString();
    //                    AExamSession = "F.N";

    //                }
    //                else if ((sessen[s1].ToString() == "PM-Regular" && sessen[s1 + 1].ToString() == "PM-Arear"))
    //                {

    //                    RPracStartTime = PracStarttimePm;
    //                    RPracEndTime = pracendttimePm;
    //                    RPracDuration = txtpracdurationpm.Text.ToString();
    //                    RExamSession = "A.N";

    //                    APracStartTime = PracStarttimePm;
    //                    APracEndTime = pracendttimePm;
    //                    APracDuration = txtpracdurationpm.Text.ToString();
    //                    AExamSession = "A.N";

    //                }
    //                else if ((sessen[s1].ToString() == "AM-Regular" && sessen[s1 + 1].ToString() == "AM-Arear"))
    //                {

    //                    RPracStartTime = PracStarttimeAm;
    //                    RPracEndTime = pracendttimeAm;
    //                    RPracDuration = txtpracdurationam.Text.ToString();
    //                    RExamSession = "F.N";

    //                    APracStartTime = PracStarttimeAm;
    //                    APracEndTime = pracendttimeAm;
    //                    APracDuration = txtpracdurationam.Text.ToString();
    //                    AExamSession = "F.N";

    //                }
    //            }
    //            else if (clSession.SelectedItem.Text.ToString() == "Mixed")
    //            {
    //                string t1 = "";
    //                string t2 = "";

    //                int day;
    //                day = 1;
    //                string ASession = "A.N";
    //                string FSession = "F.N";
    //                if (ddlExamstartswith.SelectedItem.Text.ToString() == "Regular")
    //                {
    //                    t1 = "ExamTimeTableDummyPracTable";
    //                    t2 = "ExamTimeTableDummyArearPracTable";
    //                }
    //                else if (ddlExamstartswith.SelectedItem.Text.ToString() == "Arear")
    //                {
    //                    t1 = "ExamTimeTableDummyArearPracTable";
    //                    t2 = "ExamTimeTableDummyPracTable";
    //                }

    //                int n = 0;
    //                int cnt = 0;
    //                degree = "";
    //                if (t1 == "ExamTimeTableDummyArearPracTable")
    //                {

    //                    degree = "select degree,Batch,Semester  from  " + t1 + " ";
    //                }
    //                else
    //                {
    //                    degree = "select distinct degree,Batch,Semester  from  " + t1 + " ";
    //                }
    //                SqlDataAdapter da = new SqlDataAdapter(degree, con);
    //                DataSet ds = new DataSet();
    //                da.Fill(ds);
    //            dd: if (ds.Tables[0].Rows.Count > cnt)
    //                {


    //                    string s = "";
    //                    int it = 0;

    //                l: int i = 0;
    //                    int emptysubject = 0;
    //                    while (i < ds.Tables[0].Rows.Count)
    //                    {

    //                        string ssr;
    //                        ssr = "select exam_code,exam_type from exmtt where Degree_Code='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and batchTo='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd = new SqlDataAdapter(ssr, con);
    //                        DataSet sdss = new DataSet();
    //                        ssd.Fill(sdss);
    //                        if (sdss.Tables[0].Rows.Count > 0)
    //                        {
    //                            if (t1 == "ExamTimeTableDummyArearPracTable")
    //                            {

    //                                s = "select * from " + t1 + "  where Degree='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and Batch='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                            }
    //                            else
    //                            {
    //                                s = "select * from " + t1 + "  where Degree='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "'  and  Semester='" + ds.Tables[0].Rows[i]["Semester"].ToString() + " ' and Batch='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                            }

    //                            SqlDataAdapter sda = new SqlDataAdapter(s, con);
    //                            DataSet sds = new DataSet();
    //                            sda.Fill(sds);
    //                            if (sds.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it < sds.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex.CommandType = CommandType.StoredProcedure;
    //                                    cdex.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex = new SqlDataAdapter(cdex);
    //                                    DataSet dsex = new DataSet();
    //                                    daex.Fill(dsex);
    //                                    if (dsex.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt = ds.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0 && FSession == dsex.Tables[0].Rows[dd]["ExamSession"].ToString())
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0 && FSession == dsex.Tables[0].Rows[0]["ExamSession"].ToString())
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt = ds.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd.CommandType = CommandType.StoredProcedure;
    //                                        cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                        cd.Parameters.AddWithValue("@Duration", txtpracdurationam.Text.ToString());
    //                                        cd.Parameters.AddWithValue("@StartTime", PracStarttimeAm);
    //                                        cd.Parameters.AddWithValue("@EndTime", pracendttimeAm);
    //                                        cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd.Parameters.AddWithValue("@Session", FSession);
    //                                        cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd.ExecuteNonQuery();
    //                                        i = i + 1;
    //                                        n = n + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt = ds.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        existsubject = 1;
    //                                        SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd.CommandType = CommandType.StoredProcedure;
    //                                        cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                        cd.Parameters.AddWithValue("@Duration", txtpracdurationam.Text.ToString());
    //                                        cd.Parameters.AddWithValue("@StartTime", PracStarttimeAm);
    //                                        cd.Parameters.AddWithValue("@EndTime", pracendttimeAm);
    //                                        cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd.Parameters.AddWithValue("@Session", FSession);
    //                                        cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd.ExecuteNonQuery();
    //                                        i = i + 1;
    //                                    }

    //                                }
    //                                else
    //                                {

    //                                    i = i + 1;

    //                                    emptysubject = emptysubject + 1;
    //                                    if (emptysubject == ds.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt = ds.Tables[0].Rows.Count;
    //                                        goto dd;

    //                                    }
    //                                }
    //                            }

    //                            //if (n == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n = 0;

    //                            //}
    //                        }
    //                    }

    //                    it = it + 1;
    //                    goto l;

    //                }

    //                day = 0;
    //                string degree3 = "";
    //                int n3 = 0;
    //                int cnt3 = 0;
    //                if (t2 == "ExamTimeTableDummyArearPracTable")
    //                {
    //                    degree3 = "select degree,Batch,Semester  from  " + t2 + "";
    //                }
    //                else
    //                {
    //                    degree3 = "select distinct degree,Batch,Semester  from  " + t2 + "";
    //                }
    //                SqlDataAdapter da3 = new SqlDataAdapter(degree3, con);
    //                DataSet ds3 = new DataSet();
    //                da3.Fill(ds3);
    //            dd3: if (ds3.Tables[0].Rows.Count > cnt3)
    //                {


    //                    string s3;
    //                    int it3 = 0;

    //                l3: int i3 = 0;
    //                    int emptysubject3 = 0;
    //                    while (i3 < ds3.Tables[0].Rows.Count)
    //                    {

    //                        string ssr3;
    //                        ssr3 = "select exam_code,exam_type from exmtt where Degree_Code='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and batchTo='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd3 = new SqlDataAdapter(ssr3, con);
    //                        DataSet sdss3 = new DataSet();
    //                        ssd3.Fill(sdss3);
    //                        if (sdss3.Tables[0].Rows.Count > 0)
    //                        {
    //                            if (t2 == "ExamTimeTableDummyArearPracTable")
    //                            {
    //                                s3 = "select * from " + t2 + "  where Degree='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and Batch='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "'";
    //                            }
    //                            else
    //                            {
    //                                s3 = "select * from " + t2 + "  where Degree='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "'  and  Semester='" + ds3.Tables[0].Rows[i3]["Semester"].ToString() + " ' and Batch='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "'";
    //                            }
    //                            SqlDataAdapter sda3 = new SqlDataAdapter(s3, con);
    //                            DataSet sds3 = new DataSet();
    //                            sda3.Fill(sds3);
    //                            if (sds3.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it3 < sds3.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex3 = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex3.CommandType = CommandType.StoredProcedure;
    //                                    cdex3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex3 = new SqlDataAdapter(cdex3);
    //                                    DataSet dsex3 = new DataSet();
    //                                    daex3.Fill(dsex3);
    //                                    if (dsex3.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex3.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex3.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex3.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= day)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt3 = ds3.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex3.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[day].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0 && ASession == dsex3.Tables[0].Rows[dd]["ExamSession"].ToString())
    //                                                    {
    //                                                        day = day + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex3.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[day].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0 && ASession == dsex3.Tables[0].Rows[0]["ExamSession"].ToString())
    //                                                {
    //                                                    day = day + 1;

    //                                                }
    //                                            }


    //                                        }

    //                                        if (workcount <= day)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt3 = ds3.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd3.CommandType = CommandType.StoredProcedure;
    //                                        cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                        cd3.Parameters.AddWithValue("@Duration", txtpracdurationpm.Text.ToString());
    //                                        cd3.Parameters.AddWithValue("@StartTime", PracStarttimePm);
    //                                        cd3.Parameters.AddWithValue("@EndTime", pracendttimePm);
    //                                        cd3.Parameters.AddWithValue("@ExamDate", workdays[day].ToString());
    //                                        cd3.Parameters.AddWithValue("@Session", ASession);
    //                                        cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd3.ExecuteNonQuery();
    //                                        i3 = i3 + 1;
    //                                        n3 = n3 + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        if (workcount <= day)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt3 = ds3.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        existsubject = 1;
    //                                        SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd3.CommandType = CommandType.StoredProcedure;
    //                                        cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                        cd3.Parameters.AddWithValue("@Duration", txtpracdurationpm.Text.ToString());
    //                                        cd3.Parameters.AddWithValue("@StartTime", PracStarttimePm);
    //                                        cd3.Parameters.AddWithValue("@EndTime", pracendttimePm);
    //                                        cd3.Parameters.AddWithValue("@ExamDate", workdays[day].ToString());
    //                                        cd3.Parameters.AddWithValue("@Session", ASession);
    //                                        cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd3.ExecuteNonQuery();
    //                                        i3 = i3 + 1;
    //                                    }
    //                                }
    //                                else
    //                                {

    //                                    i3 = i3 + 1;

    //                                    emptysubject3 = emptysubject3 + 1;
    //                                    if (emptysubject3 == ds3.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt3 = ds3.Tables[0].Rows.Count;
    //                                        goto dd3;

    //                                    }
    //                                }
    //                            }

    //                            //if (n3 == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n3 = 0;

    //                            //}
    //                        }
    //                    }


    //                    it3 = it3 + 1;
    //                    goto l3;



    //                }
    //            }
    //        }
    //        if (clSession.SelectedItem.Text.ToString() != "Mixed")
    //        {

    //            if (ddlExamstartswith.SelectedItem.Text.ToString() == "Regular")
    //            {

    //                int n = 0;
    //                int cnt = 0;

    //                degree = "select distinct degree,Batch ,Semester from  ExamTimeTableDummyPracTable ";
    //                SqlDataAdapter da = new SqlDataAdapter(degree, con);
    //                DataSet ds = new DataSet();
    //                da.Fill(ds);
    //            dd: if (ds.Tables[0].Rows.Count > cnt)
    //                {


    //                    string s;
    //                    int it = 0;

    //                l: int i = 0;
    //                    int emptysubject = 0;
    //                    while (i < ds.Tables[0].Rows.Count)
    //                    {

    //                        string ssr;
    //                        ssr = "select exam_code,exam_type from exmtt where Degree_Code='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and batchTo='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd = new SqlDataAdapter(ssr, con);
    //                        DataSet sdss = new DataSet();
    //                        ssd.Fill(sdss);
    //                        if (sdss.Tables[0].Rows.Count > 0)
    //                        {
    //                            s = "select * from ExamTimeTableDummyPracTable  where Degree='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "'  and  Semester='" + ds.Tables[0].Rows[i]["Semester"].ToString() + " ' and Batch='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                            SqlDataAdapter sda = new SqlDataAdapter(s, con);
    //                            DataSet sds = new DataSet();
    //                            sda.Fill(sds);
    //                            if (sds.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it < sds.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex.CommandType = CommandType.StoredProcedure;
    //                                    cdex.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex = new SqlDataAdapter(cdex);
    //                                    DataSet dsex = new DataSet();
    //                                    daex.Fill(dsex);
    //                                    if (dsex.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt = ds.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt = ds.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd.CommandType = CommandType.StoredProcedure;
    //                                        cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                        cd.Parameters.AddWithValue("@Duration", RPracDuration);
    //                                        cd.Parameters.AddWithValue("@StartTime", RPracStartTime);
    //                                        cd.Parameters.AddWithValue("@EndTime", RPracEndTime);
    //                                        cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd.ExecuteNonQuery();
    //                                        i = i + 1;
    //                                        n = n + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt = ds.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        existsubject = 1;
    //                                        SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd.CommandType = CommandType.StoredProcedure;
    //                                        cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                        cd.Parameters.AddWithValue("@Duration", RPracDuration);
    //                                        cd.Parameters.AddWithValue("@StartTime", RPracStartTime);
    //                                        cd.Parameters.AddWithValue("@EndTime", RPracEndTime);
    //                                        cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd.ExecuteNonQuery();
    //                                        i = i + 1;
    //                                    }
    //                                }
    //                                else
    //                                {

    //                                    i = i + 1;

    //                                    emptysubject = emptysubject + 1;
    //                                    if (emptysubject == ds.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt = ds.Tables[0].Rows.Count;
    //                                        goto dd;

    //                                    }
    //                                }
    //                            }

    //                            //if (n == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n = 0;

    //                            //}
    //                        }
    //                    }

    //                    it = it + 1;
    //                    goto l;

    //                }

    //                days = days + 1;
    //                string degree3;
    //                int n3 = 0;
    //                int cnt3 = 0;
    //                degree3 = "select  degree,Batch,Semester from  ExamTimeTableDummyArearPracTable ";
    //                SqlDataAdapter da3 = new SqlDataAdapter(degree3, con);
    //                DataSet ds3 = new DataSet();
    //                da3.Fill(ds3);
    //            dd3: if (ds3.Tables[0].Rows.Count > cnt3)
    //                {


    //                    string s3;
    //                    int it3 = 0;

    //                l3: int i3 = 0;
    //                    int emptysubject3 = 0;
    //                    while (i3 < ds3.Tables[0].Rows.Count)
    //                    {

    //                        string ssr3;
    //                        ssr3 = "select exam_code,exam_type from exmtt where Degree_Code='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and batchTo='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd3 = new SqlDataAdapter(ssr3, con);
    //                        DataSet sdss3 = new DataSet();
    //                        ssd3.Fill(sdss3);
    //                        if (sdss3.Tables[0].Rows.Count > 0)
    //                        {
    //                            s3 = "select * from ExamTimeTableDummyArearPracTable  where Degree='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "'";
    //                            SqlDataAdapter sda3 = new SqlDataAdapter(s3, con);
    //                            DataSet sds3 = new DataSet();
    //                            sda3.Fill(sds3);
    //                            if (sds3.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it3 < sds3.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex3 = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex3.CommandType = CommandType.StoredProcedure;
    //                                    cdex3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex3 = new SqlDataAdapter(cdex3);
    //                                    DataSet dsex3 = new DataSet();
    //                                    daex3.Fill(dsex3);
    //                                    if (dsex3.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex3.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex3.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex3.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt3 = ds3.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex3.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex3.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }

    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt3 = ds3.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd3.CommandType = CommandType.StoredProcedure;
    //                                        cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                        cd3.Parameters.AddWithValue("@Duration", APracDuration);
    //                                        cd3.Parameters.AddWithValue("@StartTime", APracStartTime);
    //                                        cd3.Parameters.AddWithValue("@EndTime", APracEndTime);
    //                                        cd3.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd3.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd3.ExecuteNonQuery();
    //                                        i3 = i3 + 1;
    //                                        n3 = n3 + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt3 = ds3.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        existsubject = 1;
    //                                        SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd3.CommandType = CommandType.StoredProcedure;
    //                                        cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                        cd3.Parameters.AddWithValue("@Duration", APracDuration);
    //                                        cd3.Parameters.AddWithValue("@StartTime", APracStartTime);
    //                                        cd3.Parameters.AddWithValue("@EndTime", APracEndTime);
    //                                        cd3.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd3.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd3.ExecuteNonQuery();
    //                                        i3 = i3 + 1;

    //                                    }
    //                                }
    //                                else
    //                                {

    //                                    i3 = i3 + 1;

    //                                    emptysubject3 = emptysubject3 + 1;
    //                                    if (emptysubject3 == ds3.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt3 = ds3.Tables[0].Rows.Count;
    //                                        goto dd3;

    //                                    }
    //                                }
    //                            }

    //                            //if (n3 == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n3 = 0;

    //                            //}
    //                        }
    //                    }


    //                    it3 = it3 + 1;
    //                    goto l3;



    //                }
    //            }

    //            else if (ddlExamstartswith.SelectedItem.Text.ToString() == "Arear")
    //            {

    //                int n = 0;
    //                int cnt = 0;

    //                degree = "select  degree,Batch ,Semester from  ExamTimeTableDummyArearPracTable ";
    //                SqlDataAdapter da = new SqlDataAdapter(degree, con);
    //                DataSet ds = new DataSet();
    //                da.Fill(ds);
    //            dd: if (ds.Tables[0].Rows.Count > cnt)
    //                {


    //                    string s;
    //                    int it = 0;

    //                l: int i = 0;
    //                    int emptysubject = 0;
    //                    while (i < ds.Tables[0].Rows.Count)
    //                    {

    //                        string ssr;
    //                        ssr = "select exam_code,exam_type from exmtt where Degree_Code='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and batchTo='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd = new SqlDataAdapter(ssr, con);
    //                        DataSet sdss = new DataSet();
    //                        ssd.Fill(sdss);
    //                        if (sdss.Tables[0].Rows.Count > 0)
    //                        {
    //                            s = "select * from ExamTimeTableDummyArearPracTable  where Degree='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and Batch='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                            SqlDataAdapter sda = new SqlDataAdapter(s, con);
    //                            DataSet sds = new DataSet();
    //                            sda.Fill(sds);
    //                            if (sds.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it < sds.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex.CommandType = CommandType.StoredProcedure;
    //                                    cdex.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex = new SqlDataAdapter(cdex);
    //                                    DataSet dsex = new DataSet();
    //                                    daex.Fill(dsex);
    //                                    if (dsex.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt = ds.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }

    //                                                    examdate = dsex.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt = ds.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd.CommandType = CommandType.StoredProcedure;
    //                                        cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                        cd.Parameters.AddWithValue("@Duration", APracDuration);
    //                                        cd.Parameters.AddWithValue("@StartTime", APracStartTime);
    //                                        cd.Parameters.AddWithValue("@EndTime", APracEndTime);
    //                                        cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd.ExecuteNonQuery();
    //                                        i = i + 1;
    //                                        n = n + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        existsubject = 1;
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt = ds.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd.CommandType = CommandType.StoredProcedure;
    //                                        cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                        cd.Parameters.AddWithValue("@Duration", APracDuration);
    //                                        cd.Parameters.AddWithValue("@StartTime", APracStartTime);
    //                                        cd.Parameters.AddWithValue("@EndTime", APracEndTime);
    //                                        cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd.ExecuteNonQuery();
    //                                        i = i + 1;


    //                                    }
    //                                }
    //                                else
    //                                {

    //                                    i = i + 1;

    //                                    emptysubject = emptysubject + 1;
    //                                    if (emptysubject == ds.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt = ds.Tables[0].Rows.Count;
    //                                        goto dd;

    //                                    }
    //                                }
    //                            }

    //                            //if (n == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n = 0;

    //                            //}
    //                        }
    //                    }

    //                    it = it + 1;
    //                    goto l;

    //                }

    //                days = days + 1;
    //                string degree3;
    //                int n3 = 0;
    //                int cnt3 = 0;
    //                degree3 = "select distinct degree,Batch,Semester  from  ExamTimeTableDummyPracTable";
    //                SqlDataAdapter da3 = new SqlDataAdapter(degree3, con);
    //                DataSet ds3 = new DataSet();
    //                da3.Fill(ds3);
    //            dd3: if (ds3.Tables[0].Rows.Count > cnt3)
    //                {


    //                    string s3;
    //                    int it3 = 0;

    //                l3: int i3 = 0;
    //                    int emptysubject3 = 0;
    //                    while (i3 < ds3.Tables[0].Rows.Count)
    //                    {

    //                        string ssr3;
    //                        ssr3 = "select exam_code,exam_type from exmtt where Degree_Code='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and batchTo='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd3 = new SqlDataAdapter(ssr3, con);
    //                        DataSet sdss3 = new DataSet();
    //                        ssd3.Fill(sdss3);
    //                        if (sdss3.Tables[0].Rows.Count > 0)
    //                        {
    //                            s3 = "select * from ExamTimeTableDummyPracTable  where Degree='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "'  and  Semester='" + ds3.Tables[0].Rows[i3]["Semester"].ToString() + " ' and Batch='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "'";
    //                            SqlDataAdapter sda3 = new SqlDataAdapter(s3, con);
    //                            DataSet sds3 = new DataSet();
    //                            sda3.Fill(sds3);
    //                            if (sds3.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it3 < sds3.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex3 = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex3.CommandType = CommandType.StoredProcedure;
    //                                    cdex3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex3 = new SqlDataAdapter(cdex3);
    //                                    DataSet dsex3 = new DataSet();
    //                                    daex3.Fill(dsex3);
    //                                    if (dsex3.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex3.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex3.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex3.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt3 = ds3.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex3.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex3.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt3 = ds3.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd3.CommandType = CommandType.StoredProcedure;
    //                                        cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                        cd3.Parameters.AddWithValue("@Duration", RPracDuration);
    //                                        cd3.Parameters.AddWithValue("@StartTime", RPracStartTime);
    //                                        cd3.Parameters.AddWithValue("@EndTime", RPracEndTime);
    //                                        cd3.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd3.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd3.ExecuteNonQuery();
    //                                        i3 = i3 + 1;
    //                                        n3 = n3 + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        existsubject = 1;
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt3 = ds3.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd3.CommandType = CommandType.StoredProcedure;
    //                                        cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                        cd3.Parameters.AddWithValue("@Duration", RPracDuration);
    //                                        cd3.Parameters.AddWithValue("@StartTime", RPracStartTime);
    //                                        cd3.Parameters.AddWithValue("@EndTime", RPracEndTime);
    //                                        cd3.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd3.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd3.ExecuteNonQuery();
    //                                        i3 = i3 + 1;


    //                                    }
    //                                }
    //                                else
    //                                {

    //                                    i3 = i3 + 1;

    //                                    emptysubject3 = emptysubject3 + 1;
    //                                    if (emptysubject3 == ds3.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt3 = ds3.Tables[0].Rows.Count;
    //                                        goto dd3;

    //                                    }
    //                                }
    //                            }

    //                            //if (n3 == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n3 = 0;

    //                            //}
    //                        }
    //                    }


    //                    it3 = it3 + 1;
    //                    goto l3;



    //                }
    //            }
    //        }



    //    }
    //    else if (ddlExamType.SelectedItem.Text.ToString() == "Arear" && ddlExmType.SelectedItem.Text.ToString() == "All")
    //    {
    //        string ATheoryStartTime = "0:00";
    //        string ATheoryEndTime = "0:00";
    //        string ATheoryDuration = "0:00";
    //        string AExamSession = "";
    //        string APracStartTime = "0:00";
    //        string APracEndTime = "0:00";
    //        string APracDuration = "0:00";

    //        if (clSession.SelectedItem.Text.ToString() == "AM-Arear")
    //        {
    //            ATheoryStartTime = TheoryStarttimeAm;
    //            ATheoryEndTime = TheoryendttimeAm;
    //            ATheoryDuration = txtTheoryDurationam.Text.ToString();
    //            AExamSession = "F.N";

    //            APracStartTime = PracStarttimeAm;
    //            APracEndTime = pracendttimeAm;
    //            APracDuration = txtpracdurationam.Text.ToString();
    //        }
    //        if (clSession.SelectedItem.Text.ToString() == "PM-Arear")
    //        {
    //            ATheoryStartTime = TheoryStarttimePm;
    //            ATheoryEndTime = TheoryendttimePm;
    //            ATheoryDuration = txtTheoryDurationpm.Text.ToString();
    //            AExamSession = "A.N";
    //            APracStartTime = PracStarttimePm;
    //            APracEndTime = pracendttimePm;
    //            APracDuration = txtpracdurationpm.Text.ToString();
    //        }

    //        if (ddlExamStartType.SelectedItem.Text.ToString() == "Theory")
    //        {
    //            int n = 0;
    //            int cnt = 0;

    //            degree = "select  degree,Batch ,Semester from  ExamTimeTableDummyArearTable ";
    //            SqlDataAdapter da = new SqlDataAdapter(degree, con);
    //            DataSet ds = new DataSet();
    //            da.Fill(ds);
    //        dd: if (ds.Tables[0].Rows.Count > cnt)
    //            {
    //                string s;
    //                int it = 0;

    //            l: int i = 0;
    //                int emptysubject = 0;
    //                while (i < ds.Tables[0].Rows.Count)
    //                {

    //                    string ssr;
    //                    ssr = "select exam_code,exam_type from exmtt where Degree_Code='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and batchTo='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                    SqlDataAdapter ssd = new SqlDataAdapter(ssr, con);
    //                    DataSet sdss = new DataSet();
    //                    ssd.Fill(sdss);
    //                    if (sdss.Tables[0].Rows.Count > 0)
    //                    {
    //                        s = "select * from ExamTimeTableDummyArearTable  where Degree='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "'and Batch='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                        SqlDataAdapter sda = new SqlDataAdapter(s, con);
    //                        DataSet sds = new DataSet();
    //                        sda.Fill(sds);
    //                        if (sds.Tables[0].Rows.Count > 0)
    //                        {
    //                            if (it < sds.Tables[0].Rows.Count)
    //                            {
    //                                SqlCommand cdex = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                cdex.CommandType = CommandType.StoredProcedure;
    //                                cdex.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                cdex.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                SqlDataAdapter daex = new SqlDataAdapter(cdex);
    //                                DataSet dsex = new DataSet();
    //                                daex.Fill(dsex);
    //                                if (dsex.Tables[1].Rows.Count == 0)
    //                                {
    //                                    existsubject = 0;
    //                                    if (dsex.Tables[0].Rows.Count > 0)
    //                                    {
    //                                        if (dsex.Tables[0].Rows.Count > 1)
    //                                        {
    //                                            for (int dd = 0; dd < dsex.Tables[0].Rows.Count; dd++)
    //                                            {
    //                                                if (workcount <= days)
    //                                                {

    //                                                    lblerror.Visible = true;
    //                                                    lblerror.Text = "Extend the Working Days";
    //                                                    cnt = ds.Tables[0].Rows.Count;
    //                                                    goto Err;

    //                                                }
    //                                                examdate = dsex.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }

    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            examdate = dsex.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                            DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                            DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                            TimeSpan ts1 = exammdate.Subtract(workday);
    //                                            if (ts1.Days == 0)
    //                                            {
    //                                                days = days + 1;

    //                                            }
    //                                        }


    //                                    }
    //                                    if (workcount <= days)
    //                                    {

    //                                        lblerror.Visible = true;
    //                                        lblerror.Text = "Extend the Working Days";
    //                                        cnt = ds.Tables[0].Rows.Count;
    //                                        goto Err;

    //                                    }

    //                                    SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                    cd.CommandType = CommandType.StoredProcedure;
    //                                    cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                    cd.Parameters.AddWithValue("@Duration", ATheoryDuration);
    //                                    cd.Parameters.AddWithValue("@StartTime", ATheoryStartTime);
    //                                    cd.Parameters.AddWithValue("@EndTime", ATheoryEndTime);
    //                                    cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                    cd.Parameters.AddWithValue("@Session", AExamSession);
    //                                    cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                    cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                    cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                    cd.ExecuteNonQuery();
    //                                    i = i + 1;
    //                                    n = n + 1;
    //                                }
    //                                else
    //                                {
    //                                    existsubject = 1;
    //                                    if (workcount <= days)
    //                                    {

    //                                        lblerror.Visible = true;
    //                                        lblerror.Text = "Extend the Working Days";
    //                                        cnt = ds.Tables[0].Rows.Count;
    //                                        goto Err;

    //                                    }
    //                                    SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                    cd.CommandType = CommandType.StoredProcedure;
    //                                    cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                    cd.Parameters.AddWithValue("@Duration", ATheoryDuration);
    //                                    cd.Parameters.AddWithValue("@StartTime", ATheoryStartTime);
    //                                    cd.Parameters.AddWithValue("@EndTime", ATheoryEndTime);
    //                                    cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                    cd.Parameters.AddWithValue("@Session", AExamSession);
    //                                    cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                    cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                    cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                    cd.ExecuteNonQuery();
    //                                    i = i + 1;

    //                                }
    //                            }
    //                            else
    //                            {

    //                                i = i + 1;

    //                                emptysubject = emptysubject + 1;
    //                                if (emptysubject == ds.Tables[0].Rows.Count)
    //                                {
    //                                    cnt = ds.Tables[0].Rows.Count;
    //                                    goto dd;

    //                                }
    //                            }
    //                        }

    //                        //if (n == Batches)
    //                        //{

    //                        //    days = days + 1;
    //                        //    n = 0;

    //                        //}
    //                    }
    //                }

    //                it = it + 1;
    //                goto l;

    //            }

    //            days = days + 1;
    //            string degree3;
    //            int n3 = 0;
    //            int cnt3 = 0;
    //            degree3 = "select  degree,Batch,Semester  from  ExamTimeTableDummyArearPracTable";
    //            SqlDataAdapter da3 = new SqlDataAdapter(degree3, con);
    //            DataSet ds3 = new DataSet();
    //            da3.Fill(ds3);
    //        dd3: if (ds3.Tables[0].Rows.Count > cnt3)
    //            {


    //                string s3;
    //                int it3 = 0;

    //            l3: int i3 = 0;
    //                int emptysubject3 = 0;
    //                while (i3 < ds3.Tables[0].Rows.Count)
    //                {

    //                    string ssr3;
    //                    ssr3 = "select exam_code,exam_type from exmtt where Degree_Code='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and batchTo='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                    SqlDataAdapter ssd3 = new SqlDataAdapter(ssr3, con);
    //                    DataSet sdss3 = new DataSet();
    //                    ssd3.Fill(sdss3);
    //                    if (sdss3.Tables[0].Rows.Count > 0)
    //                    {
    //                        s3 = "select * from ExamTimeTableDummyArearPracTable  where Degree='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "'   and Batch='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "'";
    //                        SqlDataAdapter sda3 = new SqlDataAdapter(s3, con);
    //                        DataSet sds3 = new DataSet();
    //                        sda3.Fill(sds3);
    //                        if (sds3.Tables[0].Rows.Count > 0)
    //                        {
    //                            if (it3 < sds3.Tables[0].Rows.Count)
    //                            {
    //                                SqlCommand cdex3 = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                cdex3.CommandType = CommandType.StoredProcedure;
    //                                cdex3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                cdex3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                SqlDataAdapter daex3 = new SqlDataAdapter(cdex3);
    //                                DataSet dsex3 = new DataSet();
    //                                daex3.Fill(dsex3);
    //                                if (dsex3.Tables[1].Rows.Count == 0)
    //                                {
    //                                    existsubject = 0;
    //                                    if (dsex3.Tables[0].Rows.Count > 0)
    //                                    {
    //                                        if (dsex3.Tables[0].Rows.Count > 1)
    //                                        {
    //                                            for (int dd = 0; dd < dsex3.Tables[0].Rows.Count; dd++)
    //                                            {
    //                                                if (workcount <= days)
    //                                                {

    //                                                    lblerror.Visible = true;
    //                                                    lblerror.Text = "Extend the Working Days";
    //                                                    cnt = ds.Tables[0].Rows.Count;
    //                                                    goto Err;

    //                                                }
    //                                                examdate = dsex3.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }

    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            examdate = dsex3.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                            DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                            DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                            TimeSpan ts1 = exammdate.Subtract(workday);
    //                                            if (ts1.Days == 0)
    //                                            {
    //                                                days = days + 1;

    //                                            }
    //                                        }


    //                                    }
    //                                    if (workcount <= days)
    //                                    {

    //                                        lblerror.Visible = true;
    //                                        lblerror.Text = "Extend the Working Days";
    //                                        cnt3 = ds3.Tables[0].Rows.Count;
    //                                        goto Err;

    //                                    }

    //                                    SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                    cd3.CommandType = CommandType.StoredProcedure;
    //                                    cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                    cd3.Parameters.AddWithValue("@Duration", APracDuration);
    //                                    cd3.Parameters.AddWithValue("@StartTime", APracStartTime);
    //                                    cd3.Parameters.AddWithValue("@EndTime", APracEndTime);
    //                                    cd3.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                    cd3.Parameters.AddWithValue("@Session", AExamSession);
    //                                    cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                    cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                    cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                    cd3.ExecuteNonQuery();
    //                                    i3 = i3 + 1;
    //                                    n3 = n3 + 1;
    //                                }
    //                                else
    //                                {
    //                                    if (workcount <= days)
    //                                    {

    //                                        lblerror.Visible = true;
    //                                        lblerror.Text = "Extend the Working Days";
    //                                        cnt = ds.Tables[0].Rows.Count;
    //                                        goto Err;

    //                                    }
    //                                    existsubject = 1;
    //                                    SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                    cd3.CommandType = CommandType.StoredProcedure;
    //                                    cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                    cd3.Parameters.AddWithValue("@Duration", APracDuration);
    //                                    cd3.Parameters.AddWithValue("@StartTime", APracStartTime);
    //                                    cd3.Parameters.AddWithValue("@EndTime", APracEndTime);
    //                                    cd3.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                    cd3.Parameters.AddWithValue("@Session", AExamSession);
    //                                    cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                    cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                    cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                    cd3.ExecuteNonQuery();
    //                                    i3 = i3 + 1;
    //                                }

    //                            }
    //                            else
    //                            {

    //                                i3 = i3 + 1;

    //                                emptysubject3 = emptysubject3 + 1;
    //                                if (emptysubject3 == ds3.Tables[0].Rows.Count)
    //                                {
    //                                    cnt3 = ds3.Tables[0].Rows.Count;
    //                                    goto Err;

    //                                }
    //                            }
    //                        }

    //                        //if (n3 == Batches)
    //                        //{

    //                        //    days = days + 1;
    //                        //    n3 = 0;

    //                        //}
    //                    }
    //                }


    //                it3 = it3 + 1;
    //                goto l3;



    //            }

    //        }
    //        else if (ddlExamStartType.SelectedItem.Text.ToString() == "Practical")
    //        {

    //            int n = 0;
    //            int cnt = 0;

    //            degree = "select  degree,Batch,Semester  from  ExamTimeTableDummyArearPracTable ";
    //            SqlDataAdapter da = new SqlDataAdapter(degree, con);
    //            DataSet ds = new DataSet();
    //            da.Fill(ds);
    //        dd: if (ds.Tables[0].Rows.Count > cnt)
    //            {
    //                string s;
    //                int it = 0;

    //            l: int i = 0;
    //                int emptysubject = 0;
    //                while (i < ds.Tables[0].Rows.Count)
    //                {

    //                    string ssr;
    //                    ssr = "select exam_code,exam_type from exmtt where Degree_Code='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and batchTo='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                    SqlDataAdapter ssd = new SqlDataAdapter(ssr, con);
    //                    DataSet sdss = new DataSet();
    //                    ssd.Fill(sdss);
    //                    if (sdss.Tables[0].Rows.Count > 0)
    //                    {
    //                        s = "select * from ExamTimeTableDummyArearPracTable  where Degree='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "  ' and Batch='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                        SqlDataAdapter sda = new SqlDataAdapter(s, con);
    //                        DataSet sds = new DataSet();
    //                        sda.Fill(sds);
    //                        if (sds.Tables[0].Rows.Count > 0)
    //                        {
    //                            if (it < sds.Tables[0].Rows.Count)
    //                            {
    //                                SqlCommand cdex = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                cdex.CommandType = CommandType.StoredProcedure;
    //                                cdex.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                cdex.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                SqlDataAdapter daex = new SqlDataAdapter(cdex);
    //                                DataSet dsex = new DataSet();
    //                                daex.Fill(dsex);
    //                                if (dsex.Tables[1].Rows.Count == 0)
    //                                {
    //                                    existsubject = 0;
    //                                    if (dsex.Tables[0].Rows.Count > 0)
    //                                    {
    //                                        if (dsex.Tables[0].Rows.Count > 1)
    //                                        {
    //                                            for (int dd = 0; dd < dsex.Tables[0].Rows.Count; dd++)
    //                                            {
    //                                                if (workcount <= days)
    //                                                {

    //                                                    lblerror.Visible = true;
    //                                                    lblerror.Text = "Extend the Working Days";
    //                                                    cnt = ds.Tables[0].Rows.Count;
    //                                                    goto Err;

    //                                                }
    //                                                examdate = dsex.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }

    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            examdate = dsex.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                            DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                            DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                            TimeSpan ts1 = exammdate.Subtract(workday);
    //                                            if (ts1.Days == 0)
    //                                            {
    //                                                days = days + 1;

    //                                            }
    //                                        }


    //                                    }
    //                                    if (workcount <= days)
    //                                    {

    //                                        lblerror.Visible = true;
    //                                        lblerror.Text = "Extend the Working Days";
    //                                        cnt = ds.Tables[0].Rows.Count;
    //                                        goto Err;

    //                                    }

    //                                    SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                    cd.CommandType = CommandType.StoredProcedure;
    //                                    cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                    cd.Parameters.AddWithValue("@Duration", APracDuration);
    //                                    cd.Parameters.AddWithValue("@StartTime", APracStartTime);
    //                                    cd.Parameters.AddWithValue("@EndTime", APracEndTime);
    //                                    cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                    cd.Parameters.AddWithValue("@Session", AExamSession);
    //                                    cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                    cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                    cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                    cd.ExecuteNonQuery();
    //                                    i = i + 1;
    //                                    n = n + 1;
    //                                }
    //                                else
    //                                {

    //                                    existsubject = 1;
    //                                    if (workcount <= days)
    //                                    {

    //                                        lblerror.Visible = true;
    //                                        lblerror.Text = "Extend the Working Days";
    //                                        cnt = ds.Tables[0].Rows.Count;
    //                                        goto Err;

    //                                    }
    //                                    SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                    cd.CommandType = CommandType.StoredProcedure;
    //                                    cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                    cd.Parameters.AddWithValue("@Duration", APracDuration);
    //                                    cd.Parameters.AddWithValue("@StartTime", APracStartTime);
    //                                    cd.Parameters.AddWithValue("@EndTime", APracEndTime);
    //                                    cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                    cd.Parameters.AddWithValue("@Session", AExamSession);
    //                                    cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                    cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                    cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                    cd.ExecuteNonQuery();
    //                                    i = i + 1;
    //                                }
    //                            }
    //                            else
    //                            {

    //                                i = i + 1;

    //                                emptysubject = emptysubject + 1;
    //                                if (emptysubject == ds.Tables[0].Rows.Count)
    //                                {
    //                                    cnt = ds.Tables[0].Rows.Count;
    //                                    goto dd;

    //                                }
    //                            }
    //                        }

    //                        //if (n == Batches)
    //                        //{

    //                        //    days = days + 1;
    //                        //    n = 0;

    //                        //}
    //                    }
    //                }

    //                it = it + 1;
    //                goto l;

    //            }

    //            days = days + 1;
    //            string degree3;
    //            int n3 = 0;
    //            int cnt3 = 0;
    //            degree3 = "select  degree,Batch,Semester  from  ExamTimeTableDummyArearTable";
    //            SqlDataAdapter da3 = new SqlDataAdapter(degree3, con);
    //            DataSet ds3 = new DataSet();
    //            da3.Fill(ds3);
    //        dd3: if (ds3.Tables[0].Rows.Count > cnt3)
    //            {


    //                string s3;
    //                int it3 = 0;

    //            l3: int i3 = 0;
    //                int emptysubject3 = 0;
    //                while (i3 < ds3.Tables[0].Rows.Count)
    //                {

    //                    string ssr3;
    //                    ssr3 = "select exam_code,exam_type from exmtt where Degree_Code='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and batchTo='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                    SqlDataAdapter ssd3 = new SqlDataAdapter(ssr3, con);
    //                    DataSet sdss3 = new DataSet();
    //                    ssd3.Fill(sdss3);
    //                    if (sdss3.Tables[0].Rows.Count > 0)
    //                    {
    //                        s3 = "select * from ExamTimeTableDummyArearTable  where Degree='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and Batch='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "'";
    //                        SqlDataAdapter sda3 = new SqlDataAdapter(s3, con);
    //                        DataSet sds3 = new DataSet();
    //                        sda3.Fill(sds3);
    //                        if (sds3.Tables[0].Rows.Count > 0)
    //                        {
    //                            if (it3 < sds3.Tables[0].Rows.Count)
    //                            {
    //                                SqlCommand cdex3 = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                cdex3.CommandType = CommandType.StoredProcedure;
    //                                cdex3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                cdex3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                SqlDataAdapter daex3 = new SqlDataAdapter(cdex3);
    //                                DataSet dsex3 = new DataSet();
    //                                daex3.Fill(dsex3);
    //                                if (dsex3.Tables[1].Rows.Count == 0)
    //                                {
    //                                    existsubject = 0;
    //                                    if (dsex3.Tables[0].Rows.Count > 0)
    //                                    {
    //                                        if (dsex3.Tables[0].Rows.Count > 1)
    //                                        {
    //                                            for (int dd = 0; dd < dsex3.Tables[0].Rows.Count; dd++)
    //                                            {
    //                                                if (workcount <= days)
    //                                                {

    //                                                    lblerror.Visible = true;
    //                                                    lblerror.Text = "Extend the Working Days";
    //                                                    cnt3 = ds3.Tables[0].Rows.Count;
    //                                                    goto Err;

    //                                                }
    //                                                examdate = dsex3.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }

    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            examdate = dsex3.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                            DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                            DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                            TimeSpan ts1 = exammdate.Subtract(workday);
    //                                            if (ts1.Days == 0)
    //                                            {
    //                                                days = days + 1;

    //                                            }
    //                                        }


    //                                    }
    //                                    if (workcount <= days)
    //                                    {

    //                                        lblerror.Visible = true;
    //                                        lblerror.Text = "Extend the Working Days";
    //                                        cnt3 = ds3.Tables[0].Rows.Count;
    //                                        goto Err;

    //                                    }

    //                                    SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                    cd3.CommandType = CommandType.StoredProcedure;
    //                                    cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                    cd3.Parameters.AddWithValue("@Duration", ATheoryDuration);
    //                                    cd3.Parameters.AddWithValue("@StartTime", ATheoryStartTime);
    //                                    cd3.Parameters.AddWithValue("@EndTime", ATheoryEndTime);
    //                                    cd3.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                    cd3.Parameters.AddWithValue("@Session", AExamSession);
    //                                    cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                    cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                    cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                    cd3.ExecuteNonQuery();
    //                                    i3 = i3 + 1;
    //                                    n3 = n3 + 1;
    //                                }
    //                                else
    //                                {
    //                                    existsubject = 1;
    //                                    if (workcount <= days)
    //                                    {

    //                                        lblerror.Visible = true;
    //                                        lblerror.Text = "Extend the Working Days";
    //                                        cnt3 = ds3.Tables[0].Rows.Count;
    //                                        goto Err;

    //                                    }
    //                                    SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                    cd3.CommandType = CommandType.StoredProcedure;
    //                                    cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                    cd3.Parameters.AddWithValue("@Duration", ATheoryDuration);
    //                                    cd3.Parameters.AddWithValue("@StartTime", ATheoryStartTime);
    //                                    cd3.Parameters.AddWithValue("@EndTime", ATheoryEndTime);
    //                                    cd3.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                    cd3.Parameters.AddWithValue("@Session", AExamSession);
    //                                    cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                    cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                    cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                    cd3.ExecuteNonQuery();
    //                                    i3 = i3 + 1;


    //                                }
    //                            }
    //                            else
    //                            {

    //                                i3 = i3 + 1;

    //                                emptysubject3 = emptysubject3 + 1;
    //                                if (emptysubject3 == ds3.Tables[0].Rows.Count)
    //                                {
    //                                    cnt3 = ds3.Tables[0].Rows.Count;
    //                                    goto Err;

    //                                }
    //                            }
    //                        }

    //                        //if (n3 == Batches)
    //                        //{

    //                        //    days = days + 1;
    //                        //    n3 = 0;

    //                        //}
    //                    }
    //                }


    //                it3 = it3 + 1;
    //                goto l3;



    //            }

    //        }


    //    }
    //    else if (ddlExamType.SelectedItem.Text.ToString() == "Regular" && ddlExmType.SelectedItem.Text.ToString() == "All")
    //    {
    //        string RTheoryStartTime = "0:00";
    //        string RTheoryEndTime = "0:00";
    //        string RTheoryDuration = "0:00";
    //        string RExamSession = "";
    //        string RPracStartTime = "0:00";
    //        string RPracEndTime = "0:00";
    //        string RPracDuration = "0:00";

    //        if (clSession.SelectedItem.Text.ToString() == "AM-Regular")
    //        {
    //            RTheoryStartTime = TheoryStarttimeAm;
    //            RTheoryEndTime = TheoryendttimeAm;
    //            RTheoryDuration = txtTheoryDurationam.Text.ToString();
    //            RExamSession = "F.N";

    //            RPracStartTime = PracStarttimeAm;
    //            RPracEndTime = pracendttimeAm;
    //            RPracDuration = txtpracdurationam.Text.ToString();
    //        }
    //        if (clSession.SelectedItem.Text.ToString() == "PM-Regular")
    //        {
    //            RTheoryStartTime = TheoryStarttimePm;
    //            RTheoryEndTime = TheoryendttimePm;
    //            RTheoryDuration = txtTheoryDurationpm.Text.ToString();
    //            RExamSession = "A.N";

    //            RPracStartTime = PracStarttimePm;
    //            RPracEndTime = pracendttimePm;
    //            RPracDuration = txtpracdurationpm.Text.ToString();
    //        }

    //        if (ddlExamStartType.SelectedItem.Text.ToString() == "Theory")
    //        {
    //            int n = 0;
    //            int cnt = 0;

    //            degree = "select distinct degree,Batch,Semester  from  ExamTimeTableDummyTable ";
    //            SqlDataAdapter da = new SqlDataAdapter(degree, con);
    //            DataSet ds = new DataSet();
    //            da.Fill(ds);
    //        dd: if (ds.Tables[0].Rows.Count > cnt)
    //            {
    //                string s;
    //                int it = 0;

    //            l: int i = 0;
    //                int emptysubject = 0;
    //                while (i < ds.Tables[0].Rows.Count)
    //                {

    //                    string ssr;
    //                    ssr = "select exam_code,exam_type from exmtt where Degree_Code='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and batchTo='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                    SqlDataAdapter ssd = new SqlDataAdapter(ssr, con);
    //                    DataSet sdss = new DataSet();
    //                    ssd.Fill(sdss);
    //                    if (sdss.Tables[0].Rows.Count > 0)
    //                    {
    //                        s = "select * from ExamTimeTableDummyTable  where Degree='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "'  and  Semester='" + ds.Tables[0].Rows[i]["Semester"].ToString() + " ' and Batch='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                        SqlDataAdapter sda = new SqlDataAdapter(s, con);
    //                        DataSet sds = new DataSet();
    //                        sda.Fill(sds);
    //                        if (sds.Tables[0].Rows.Count > 0)
    //                        {
    //                            if (it < sds.Tables[0].Rows.Count)
    //                            {
    //                                SqlCommand cdex = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                cdex.CommandType = CommandType.StoredProcedure;
    //                                cdex.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                cdex.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                SqlDataAdapter daex = new SqlDataAdapter(cdex);
    //                                DataSet dsex = new DataSet();
    //                                daex.Fill(dsex);
    //                                if (dsex.Tables[1].Rows.Count == 0)
    //                                {
    //                                    existsubject = 0;
    //                                    if (dsex.Tables[0].Rows.Count > 0)
    //                                    {
    //                                        if (dsex.Tables[0].Rows.Count > 1)
    //                                        {
    //                                            for (int dd = 0; dd < dsex.Tables[0].Rows.Count; dd++)
    //                                            {
    //                                                if (workcount <= days)
    //                                                {

    //                                                    lblerror.Visible = true;
    //                                                    lblerror.Text = "Extend the Working Days";
    //                                                    cnt = ds.Tables[0].Rows.Count;
    //                                                    goto Err;

    //                                                }
    //                                                examdate = dsex.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }

    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            examdate = dsex.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                            DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                            DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                            TimeSpan ts1 = exammdate.Subtract(workday);
    //                                            if (ts1.Days == 0)
    //                                            {
    //                                                days = days + 1;

    //                                            }
    //                                        }


    //                                    }
    //                                    if (workcount <= days)
    //                                    {

    //                                        lblerror.Visible = true;
    //                                        lblerror.Text = "Extend the Working Days";
    //                                        cnt = ds.Tables[0].Rows.Count;
    //                                        goto Err;

    //                                    }

    //                                    SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                    cd.CommandType = CommandType.StoredProcedure;
    //                                    cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                    cd.Parameters.AddWithValue("@Duration", RTheoryDuration);
    //                                    cd.Parameters.AddWithValue("@StartTime", RTheoryStartTime);
    //                                    cd.Parameters.AddWithValue("@EndTime", RTheoryEndTime);
    //                                    cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                    cd.Parameters.AddWithValue("@Session", RExamSession);
    //                                    cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                    cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                    cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                    cd.ExecuteNonQuery();
    //                                    i = i + 1;
    //                                    n = n + 1;
    //                                }
    //                                else
    //                                {
    //                                    existsubject = 1;
    //                                    if (workcount <= days)
    //                                    {

    //                                        lblerror.Visible = true;
    //                                        lblerror.Text = "Extend the Working Days";
    //                                        cnt = ds.Tables[0].Rows.Count;
    //                                        goto Err;

    //                                    }
    //                                    SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                    cd.CommandType = CommandType.StoredProcedure;
    //                                    cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                    cd.Parameters.AddWithValue("@Duration", RTheoryDuration);
    //                                    cd.Parameters.AddWithValue("@StartTime", RTheoryStartTime);
    //                                    cd.Parameters.AddWithValue("@EndTime", RTheoryEndTime);
    //                                    cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                    cd.Parameters.AddWithValue("@Session", RExamSession);
    //                                    cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                    cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                    cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                    cd.ExecuteNonQuery();
    //                                    i = i + 1;

    //                                }
    //                            }
    //                            else
    //                            {

    //                                i = i + 1;

    //                                emptysubject = emptysubject + 1;
    //                                if (emptysubject == ds.Tables[0].Rows.Count)
    //                                {
    //                                    cnt = ds.Tables[0].Rows.Count;
    //                                    goto dd;

    //                                }
    //                            }
    //                        }

    //                        //if (n == Batches)
    //                        //{

    //                        //    days = days + 1;
    //                        //    n = 0;

    //                        //}
    //                    }
    //                }

    //                it = it + 1;
    //                goto l;

    //            }

    //            days = days + 1;
    //            string degree3;
    //            int n3 = 0;
    //            int cnt3 = 0;
    //            degree3 = "select distinct degree,Batch,Semester  from  ExamTimeTableDummyPracTable";
    //            SqlDataAdapter da3 = new SqlDataAdapter(degree3, con);
    //            DataSet ds3 = new DataSet();
    //            da3.Fill(ds3);
    //        dd3: if (ds3.Tables[0].Rows.Count > cnt3)
    //            {


    //                string s3;
    //                int it3 = 0;

    //            l3: int i3 = 0;
    //                int emptysubject3 = 0;
    //                while (i3 < ds3.Tables[0].Rows.Count)
    //                {

    //                    string ssr3;
    //                    ssr3 = "select exam_code,exam_type from exmtt where Degree_Code='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and batchTo='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                    SqlDataAdapter ssd3 = new SqlDataAdapter(ssr3, con);
    //                    DataSet sdss3 = new DataSet();
    //                    ssd3.Fill(sdss3);
    //                    if (sdss3.Tables[0].Rows.Count > 0)
    //                    {
    //                        s3 = "select * from ExamTimeTableDummyPracTable  where Degree='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "'  and  Semester='" + ds3.Tables[0].Rows[i3]["Semester"].ToString() + " ' and Batch='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "'";
    //                        SqlDataAdapter sda3 = new SqlDataAdapter(s3, con);
    //                        DataSet sds3 = new DataSet();
    //                        sda3.Fill(sds3);
    //                        if (sds3.Tables[0].Rows.Count > 0)
    //                        {
    //                            if (it3 < sds3.Tables[0].Rows.Count)
    //                            {
    //                                SqlCommand cdex3 = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                cdex3.CommandType = CommandType.StoredProcedure;
    //                                cdex3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                cdex3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                SqlDataAdapter daex3 = new SqlDataAdapter(cdex3);
    //                                DataSet dsex3 = new DataSet();
    //                                daex3.Fill(dsex3);
    //                                if (dsex3.Tables[1].Rows.Count == 0)
    //                                {
    //                                    existsubject = 0;
    //                                    if (dsex3.Tables[0].Rows.Count > 0)
    //                                    {
    //                                        if (dsex3.Tables[0].Rows.Count > 1)
    //                                        {
    //                                            for (int dd = 0; dd < dsex3.Tables[0].Rows.Count; dd++)
    //                                            {
    //                                                if (workcount <= days)
    //                                                {

    //                                                    lblerror.Visible = true;
    //                                                    lblerror.Text = "Extend the Working Days";
    //                                                    cnt3 = ds3.Tables[0].Rows.Count;
    //                                                    goto Err;

    //                                                }
    //                                                examdate = dsex3.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }

    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            examdate = dsex3.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                            DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                            DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                            TimeSpan ts1 = exammdate.Subtract(workday);
    //                                            if (ts1.Days == 0)
    //                                            {
    //                                                days = days + 1;

    //                                            }
    //                                        }


    //                                    }
    //                                    if (workcount <= days)
    //                                    {

    //                                        lblerror.Visible = true;
    //                                        lblerror.Text = "Extend the Working Days";
    //                                        cnt3 = ds3.Tables[0].Rows.Count;
    //                                        goto Err;

    //                                    }

    //                                    SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                    cd3.CommandType = CommandType.StoredProcedure;
    //                                    cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                    cd3.Parameters.AddWithValue("@Duration", RPracDuration);
    //                                    cd3.Parameters.AddWithValue("@StartTime", RPracStartTime);
    //                                    cd3.Parameters.AddWithValue("@EndTime", RPracEndTime);
    //                                    cd3.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                    cd3.Parameters.AddWithValue("@Session", RExamSession);
    //                                    cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                    cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                    cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                    cd3.ExecuteNonQuery();
    //                                    i3 = i3 + 1;
    //                                    n3 = n3 + 1;
    //                                }
    //                                else
    //                                {
    //                                    existsubject = 1;
    //                                    if (workcount <= days)
    //                                    {

    //                                        lblerror.Visible = true;
    //                                        lblerror.Text = "Extend the Working Days";
    //                                        cnt3 = ds3.Tables[0].Rows.Count;
    //                                        goto Err;

    //                                    }
    //                                    SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                    cd3.CommandType = CommandType.StoredProcedure;
    //                                    cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                    cd3.Parameters.AddWithValue("@Duration", RPracDuration);
    //                                    cd3.Parameters.AddWithValue("@StartTime", RPracStartTime);
    //                                    cd3.Parameters.AddWithValue("@EndTime", RPracEndTime);
    //                                    cd3.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                    cd3.Parameters.AddWithValue("@Session", RExamSession);
    //                                    cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                    cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                    cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                    cd3.ExecuteNonQuery();
    //                                    i3 = i3 + 1;
    //                                }
    //                            }
    //                            else
    //                            {

    //                                i3 = i3 + 1;

    //                                emptysubject3 = emptysubject3 + 1;
    //                                if (emptysubject3 == ds3.Tables[0].Rows.Count)
    //                                {
    //                                    cnt3 = ds3.Tables[0].Rows.Count;
    //                                    goto Err;

    //                                }
    //                            }
    //                        }

    //                        //if (n3 == Batches)
    //                        //{

    //                        //    days = days + 1;
    //                        //    n3 = 0;

    //                        //}
    //                    }
    //                }


    //                it3 = it3 + 1;
    //                goto l3;



    //            }

    //        }
    //        else if (ddlExamStartType.SelectedItem.Text.ToString() == "Practical")
    //        {

    //            int n = 0;
    //            int cnt = 0;

    //            degree = "select distinct degree,Batch,Semester  from  ExamTimeTableDummyPracTable ";
    //            SqlDataAdapter da = new SqlDataAdapter(degree, con);
    //            DataSet ds = new DataSet();
    //            da.Fill(ds);
    //        dd: if (ds.Tables[0].Rows.Count > cnt)
    //            {
    //                string s;
    //                int it = 0;

    //            l: int i = 0;
    //                int emptysubject = 0;
    //                while (i < ds.Tables[0].Rows.Count)
    //                {

    //                    string ssr;
    //                    ssr = "select exam_code,exam_type from exmtt where Degree_Code='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and batchTo='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                    SqlDataAdapter ssd = new SqlDataAdapter(ssr, con);
    //                    DataSet sdss = new DataSet();
    //                    ssd.Fill(sdss);
    //                    if (sdss.Tables[0].Rows.Count > 0)
    //                    {
    //                        s = "select * from ExamTimeTableDummyPracTable  where Degree='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "'  and  Semester='" + ds.Tables[0].Rows[i]["Semester"].ToString() + " ' and Batch='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                        SqlDataAdapter sda = new SqlDataAdapter(s, con);
    //                        DataSet sds = new DataSet();
    //                        sda.Fill(sds);
    //                        if (sds.Tables[0].Rows.Count > 0)
    //                        {
    //                            if (it < sds.Tables[0].Rows.Count)
    //                            {
    //                                SqlCommand cdex = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                cdex.CommandType = CommandType.StoredProcedure;
    //                                cdex.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                cdex.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                SqlDataAdapter daex = new SqlDataAdapter(cdex);
    //                                DataSet dsex = new DataSet();
    //                                daex.Fill(dsex);
    //                                if (dsex.Tables[1].Rows.Count == 0)
    //                                {
    //                                    existsubject = 0;
    //                                    if (dsex.Tables[0].Rows.Count > 0)
    //                                    {
    //                                        if (dsex.Tables[0].Rows.Count > 1)
    //                                        {
    //                                            for (int dd = 0; dd < dsex.Tables[0].Rows.Count; dd++)
    //                                            {
    //                                                if (workcount <= days)
    //                                                {

    //                                                    lblerror.Visible = true;
    //                                                    lblerror.Text = "Extend the Working Days";
    //                                                    cnt = ds.Tables[0].Rows.Count;
    //                                                    goto Err;

    //                                                }
    //                                                examdate = dsex.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }

    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            examdate = dsex.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                            DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                            DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                            TimeSpan ts1 = exammdate.Subtract(workday);
    //                                            if (ts1.Days == 0)
    //                                            {
    //                                                days = days + 1;

    //                                            }
    //                                        }


    //                                    }
    //                                    if (workcount <= days)
    //                                    {

    //                                        lblerror.Visible = true;
    //                                        lblerror.Text = "Extend the Working Days";
    //                                        cnt = ds.Tables[0].Rows.Count;
    //                                        goto Err;

    //                                    }

    //                                    SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                    cd.CommandType = CommandType.StoredProcedure;
    //                                    cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                    cd.Parameters.AddWithValue("@Duration", RPracDuration);
    //                                    cd.Parameters.AddWithValue("@StartTime", RPracStartTime);
    //                                    cd.Parameters.AddWithValue("@EndTime", RPracEndTime);
    //                                    cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                    cd.Parameters.AddWithValue("@Session", RExamSession);
    //                                    cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                    cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                    cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                    cd.ExecuteNonQuery();
    //                                    i = i + 1;
    //                                    n = n + 1;
    //                                }
    //                                else
    //                                {
    //                                    existsubject = 1;
    //                                    if (workcount <= days)
    //                                    {

    //                                        lblerror.Visible = true;
    //                                        lblerror.Text = "Extend the Working Days";
    //                                        cnt = ds.Tables[0].Rows.Count;
    //                                        goto Err;

    //                                    }
    //                                    SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                    cd.CommandType = CommandType.StoredProcedure;
    //                                    cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                    cd.Parameters.AddWithValue("@Duration", RPracDuration);
    //                                    cd.Parameters.AddWithValue("@StartTime", RPracStartTime);
    //                                    cd.Parameters.AddWithValue("@EndTime", RPracEndTime);
    //                                    cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                    cd.Parameters.AddWithValue("@Session", RExamSession);
    //                                    cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                    cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                    cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                    cd.ExecuteNonQuery();
    //                                    i = i + 1;
    //                                }

    //                            }
    //                            else
    //                            {

    //                                i = i + 1;

    //                                emptysubject = emptysubject + 1;
    //                                if (emptysubject == ds.Tables[0].Rows.Count)
    //                                {
    //                                    cnt = ds.Tables[0].Rows.Count;
    //                                    goto dd;

    //                                }
    //                            }
    //                        }

    //                        //if (n == Batches)
    //                        //{

    //                        //    days = days + 1;
    //                        //    n = 0;

    //                        //}
    //                    }
    //                }

    //                it = it + 1;
    //                goto l;

    //            }

    //            days = days + 1;
    //            string degree3;
    //            int n3 = 0;
    //            int cnt3 = 0;
    //            degree3 = "select distinct degree,Batch,Semester  from  ExamTimeTableDummyTable";
    //            SqlDataAdapter da3 = new SqlDataAdapter(degree3, con);
    //            DataSet ds3 = new DataSet();
    //            da3.Fill(ds3);
    //        dd3: if (ds3.Tables[0].Rows.Count > cnt3)
    //            {


    //                string s3;
    //                int it3 = 0;

    //            l3: int i3 = 0;
    //                int emptysubject3 = 0;
    //                while (i3 < ds3.Tables[0].Rows.Count)
    //                {

    //                    string ssr3;
    //                    ssr3 = "select exam_code,exam_type from exmtt where Degree_Code='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and batchTo='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                    SqlDataAdapter ssd3 = new SqlDataAdapter(ssr3, con);
    //                    DataSet sdss3 = new DataSet();
    //                    ssd3.Fill(sdss3);
    //                    if (sdss3.Tables[0].Rows.Count > 0)
    //                    {
    //                        s3 = "select * from ExamTimeTableDummyTable  where Degree='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "'  and  Semester='" + ds3.Tables[0].Rows[i3]["Semester"].ToString() + " ' and Batch='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "'";
    //                        SqlDataAdapter sda3 = new SqlDataAdapter(s3, con);
    //                        DataSet sds3 = new DataSet();
    //                        sda3.Fill(sds3);
    //                        if (sds3.Tables[0].Rows.Count > 0)
    //                        {
    //                            if (it3 < sds3.Tables[0].Rows.Count)
    //                            {
    //                                SqlCommand cdex3 = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                cdex3.CommandType = CommandType.StoredProcedure;
    //                                cdex3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                cdex3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                SqlDataAdapter daex3 = new SqlDataAdapter(cdex3);
    //                                DataSet dsex3 = new DataSet();
    //                                daex3.Fill(dsex3);
    //                                if (dsex3.Tables[1].Rows.Count == 0)
    //                                {
    //                                    existsubject = 0;
    //                                    if (dsex3.Tables[0].Rows.Count > 0)
    //                                    {
    //                                        if (dsex3.Tables[0].Rows.Count > 1)
    //                                        {
    //                                            for (int dd = 0; dd < dsex3.Tables[0].Rows.Count; dd++)
    //                                            {
    //                                                if (workcount <= days)
    //                                                {

    //                                                    lblerror.Visible = true;
    //                                                    lblerror.Text = "Extend the Working Days";
    //                                                    cnt = ds.Tables[0].Rows.Count;
    //                                                    goto Err;

    //                                                }
    //                                                examdate = dsex3.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }

    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            examdate = dsex3.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                            DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                            DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                            TimeSpan ts1 = exammdate.Subtract(workday);
    //                                            if (ts1.Days == 0)
    //                                            {
    //                                                days = days + 1;

    //                                            }
    //                                        }


    //                                    }
    //                                    if (workcount <= days)
    //                                    {

    //                                        lblerror.Visible = true;
    //                                        lblerror.Text = "Extend the Working Days";
    //                                        cnt = ds.Tables[0].Rows.Count;
    //                                        goto Err;

    //                                    }

    //                                    SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                    cd3.CommandType = CommandType.StoredProcedure;
    //                                    cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                    cd3.Parameters.AddWithValue("@Duration", RTheoryDuration);
    //                                    cd3.Parameters.AddWithValue("@StartTime", RTheoryStartTime);
    //                                    cd3.Parameters.AddWithValue("@EndTime", RTheoryEndTime);
    //                                    cd3.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                    cd3.Parameters.AddWithValue("@Session", RExamSession);
    //                                    cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                    cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                    cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                    cd3.ExecuteNonQuery();
    //                                    i3 = i3 + 1;
    //                                    n3 = n3 + 1;
    //                                }
    //                                else
    //                                {
    //                                    existsubject = 1;
    //                                    if (workcount <= days)
    //                                    {

    //                                        lblerror.Visible = true;
    //                                        lblerror.Text = "Extend the Working Days";
    //                                        cnt = ds.Tables[0].Rows.Count;
    //                                        goto Err;

    //                                    }
    //                                    SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                    cd3.CommandType = CommandType.StoredProcedure;
    //                                    cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                    cd3.Parameters.AddWithValue("@Duration", RTheoryDuration);
    //                                    cd3.Parameters.AddWithValue("@StartTime", RTheoryStartTime);
    //                                    cd3.Parameters.AddWithValue("@EndTime", RTheoryEndTime);
    //                                    cd3.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                    cd3.Parameters.AddWithValue("@Session", RExamSession);
    //                                    cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                    cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                    cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                    cd3.ExecuteNonQuery();
    //                                    i3 = i3 + 1;

    //                                }
    //                            }
    //                            else
    //                            {

    //                                i3 = i3 + 1;

    //                                emptysubject3 = emptysubject3 + 1;
    //                                if (emptysubject3 == ds3.Tables[0].Rows.Count)
    //                                {
    //                                    cnt3 = ds3.Tables[0].Rows.Count;
    //                                    goto Err;

    //                                }
    //                            }
    //                        }

    //                        //if (n3 == Batches)
    //                        //{

    //                        //    days = days + 1;
    //                        //    n3 = 0;

    //                        //}
    //                    }
    //                }


    //                it3 = it3 + 1;
    //                goto l3;



    //            }

    //        }


    //    }
    //    else if (ddlExamType.SelectedItem.Text.ToString() == "All" && ddlExmType.SelectedItem.Text.ToString() == "All")
    //    {

    //        string RTheoryStartTime = "0:00";
    //        string RTheoryEndTime = "0:00";
    //        string RTheoryDuration = "0:00";

    //        string RPracticalStartTime = "0:00";
    //        string RPracticalEndTime = "0:00";
    //        string RPracticalDuration = "0:00";
    //        string RExamSession = "";

    //        string ATheoryStartTime = "0:00";
    //        string ATheoryEndTime = "0:00";
    //        string ATheoryDuration = "0:00";

    //        string APracticalStartTime = "0:00";
    //        string APracticalEndTime = "0:00";
    //        string APracticalDuration = "0:00";
    //        string AExamSession = "";


    //        string session = "";
    //        string[] sessen;



    //        for (int k = 0; k < clSession.Items.Count; k++)
    //        {
    //            if (clSession.Items[k].Selected == true)
    //            {
    //                count = count + 1;
    //                if (session == "")
    //                {
    //                    session = clSession.Items[k].Text.ToString();
    //                }
    //                else
    //                {

    //                    session = session + "=" + clSession.Items[k].Text.ToString();

    //                }
    //            }

    //        }

    //        sessen = session.Split(new char[] { '=' });
    //        if (sessen.Length > 0)
    //        {
    //            int s1 = 0;

    //            if (sessen.Length == 2)
    //            {

    //                if ((sessen[s1].ToString() == "AM-Regular" && sessen[s1 + 1].ToString() == "PM-Arear"))
    //                {

    //                    RTheoryStartTime = TheoryStarttimeAm;
    //                    RTheoryEndTime = TheoryendttimeAm;
    //                    RTheoryDuration = txtTheoryDurationam.Text.ToString();

    //                    RPracticalDuration = txtpracdurationam.Text.ToString();
    //                    RPracticalEndTime = pracendttimeAm;
    //                    RPracticalStartTime = PracStarttimeAm;
    //                    RExamSession = "F.N";

    //                    ATheoryStartTime = TheoryStarttimePm;
    //                    ATheoryEndTime = TheoryendttimePm;
    //                    ATheoryDuration = txtTheoryDurationpm.Text.ToString();

    //                    APracticalDuration = txtpracdurationpm.Text.ToString();
    //                    APracticalEndTime = pracendttimePm;
    //                    APracticalStartTime = PracStarttimePm;
    //                    AExamSession = "A.N";

    //                }
    //                else if (sessen[s1].ToString() == "PM-Regular" && sessen[s1 + 1].ToString() == "AM-Arear")
    //                {

    //                    RTheoryStartTime = TheoryStarttimePm;
    //                    RTheoryEndTime = TheoryendttimePm;
    //                    RTheoryDuration = txtTheoryDurationpm.Text.ToString();

    //                    RPracticalDuration = txtpracdurationpm.Text.ToString();
    //                    RPracticalEndTime = pracendttimePm;
    //                    RPracticalStartTime = PracStarttimePm;
    //                    RExamSession = "A.N";

    //                    ATheoryStartTime = TheoryStarttimeAm;
    //                    ATheoryEndTime = TheoryendttimeAm;
    //                    ATheoryDuration = txtTheoryDurationam.Text.ToString();

    //                    APracticalDuration = txtpracdurationam.Text.ToString();
    //                    APracticalEndTime = pracendttimeAm;
    //                    APracticalStartTime = PracStarttimeAm;
    //                    AExamSession = "F.N";

    //                }
    //                else if ((sessen[s1].ToString() == "PM-Regular" && sessen[s1 + 1].ToString() == "PM-Arear"))
    //                {

    //                    RTheoryStartTime = TheoryStarttimePm;
    //                    RTheoryEndTime = TheoryendttimePm;
    //                    RTheoryDuration = txtTheoryDurationpm.Text.ToString();

    //                    RPracticalDuration = txtpracdurationpm.Text.ToString();
    //                    RPracticalEndTime = pracendttimePm;
    //                    RPracticalStartTime = PracStarttimePm;
    //                    RExamSession = "A.N";

    //                    ATheoryStartTime = TheoryStarttimePm;
    //                    ATheoryEndTime = TheoryendttimePm;
    //                    ATheoryDuration = txtTheoryDurationpm.Text.ToString();

    //                    APracticalDuration = txtpracdurationpm.Text.ToString();
    //                    APracticalEndTime = pracendttimePm;
    //                    APracticalStartTime = PracStarttimePm;
    //                    AExamSession = "A.N";

    //                }
    //                else if ((sessen[s1].ToString() == "AM-Regular" && sessen[s1 + 1].ToString() == "AM-Arear"))
    //                {

    //                    RTheoryStartTime = TheoryStarttimeAm;
    //                    RTheoryEndTime = TheoryendttimeAm;
    //                    RTheoryDuration = txtTheoryDurationam.Text.ToString();

    //                    RPracticalDuration = txtpracdurationam.Text.ToString();
    //                    RPracticalEndTime = pracendttimeAm;
    //                    RPracticalStartTime = PracStarttimeAm;
    //                    RExamSession = "F.N";

    //                    ATheoryStartTime = TheoryStarttimeAm;
    //                    ATheoryEndTime = TheoryendttimeAm;
    //                    ATheoryDuration = txtTheoryDurationam.Text.ToString();

    //                    APracticalDuration = txtpracdurationam.Text.ToString();
    //                    APracticalEndTime = pracendttimeAm;
    //                    APracticalStartTime = PracStarttimeAm;
    //                    AExamSession = "F.N";

    //                }
    //            }
    //        }
    //        if (clSession.SelectedItem.Text.ToString() != "Mixed")
    //        {

    //            if (ddlExamstartswith.SelectedItem.Text.ToString() == "Regular" && ddlExamStartType.SelectedItem.Text.ToString() == "Theory")
    //            {


    //                int n = 0;
    //                int cnt = 0;
    //                degree = "select distinct degree,Batch,Semester  from  ExamTimeTableDummyTable ";
    //                SqlDataAdapter da = new SqlDataAdapter(degree, con);
    //                DataSet ds = new DataSet();
    //                da.Fill(ds);
    //            dd: if (ds.Tables[0].Rows.Count > cnt)
    //                {
    //                    string ss;
    //                    int it = 0;

    //                l: int i = 0;
    //                    int emptysubject = 0;
    //                    while (i < ds.Tables[0].Rows.Count)
    //                    {

    //                        string ssr;
    //                        ssr = "select exam_code,exam_type from exmtt where Degree_Code='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and batchTo='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd = new SqlDataAdapter(ssr, con);
    //                        DataSet sdss = new DataSet();
    //                        ssd.Fill(sdss);
    //                        if (sdss.Tables[0].Rows.Count > 0)
    //                        {
    //                            ss = "select * from ExamTimeTableDummyTable  where Degree='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "'   and  Semester='" + ds.Tables[0].Rows[i]["Semester"].ToString() + " ' and Batch='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                            SqlDataAdapter sda = new SqlDataAdapter(ss, con);
    //                            DataSet sds = new DataSet();
    //                            sda.Fill(sds);
    //                            if (sds.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it < sds.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex.CommandType = CommandType.StoredProcedure;
    //                                    cdex.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex = new SqlDataAdapter(cdex);
    //                                    DataSet dsex = new DataSet();
    //                                    daex.Fill(dsex);
    //                                    if (dsex.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt = ds.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt = ds.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd.CommandType = CommandType.StoredProcedure;
    //                                        cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                        cd.Parameters.AddWithValue("@Duration", RTheoryDuration);
    //                                        cd.Parameters.AddWithValue("@StartTime", RTheoryStartTime);
    //                                        cd.Parameters.AddWithValue("@EndTime", RTheoryEndTime);
    //                                        cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd.ExecuteNonQuery();
    //                                        i = i + 1;
    //                                        n = n + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        existsubject = 1;
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt = ds.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd.CommandType = CommandType.StoredProcedure;
    //                                        cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                        cd.Parameters.AddWithValue("@Duration", RTheoryDuration);
    //                                        cd.Parameters.AddWithValue("@StartTime", RTheoryStartTime);
    //                                        cd.Parameters.AddWithValue("@EndTime", RTheoryEndTime);
    //                                        cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd.ExecuteNonQuery();
    //                                        i = i + 1;


    //                                    }
    //                                }
    //                                else
    //                                {

    //                                    i = i + 1;

    //                                    emptysubject = emptysubject + 1;
    //                                    if (emptysubject == ds.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt = ds.Tables[0].Rows.Count;
    //                                        goto dd;

    //                                    }
    //                                }
    //                            }

    //                            //if (n == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n = 0;

    //                            //}
    //                        }
    //                    }

    //                    it = it + 1;
    //                    goto l;

    //                }
    //                days = days + 1;
    //                string degree1;
    //                int n1 = 0;
    //                int cnt1 = 0;
    //                degree1 = "select distinct degree,Batch,Semester  from  ExamTimeTableDummyPracTable ";
    //                SqlDataAdapter da1 = new SqlDataAdapter(degree1, con);
    //                DataSet ds1 = new DataSet();
    //                da1.Fill(ds1);
    //            dd1: if (ds1.Tables[0].Rows.Count > cnt1)
    //                {
    //                    string ss1;
    //                    int it1 = 0;

    //                l1: int i1 = 0;
    //                    int emptysubject1 = 0;
    //                    while (i1 < ds1.Tables[0].Rows.Count)
    //                    {

    //                        string ssr1;
    //                        ssr1 = "select exam_code,exam_type from exmtt where Degree_Code='" + ds1.Tables[0].Rows[i1]["Degree"].ToString() + "' and batchTo='" + ds1.Tables[0].Rows[i1]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd1 = new SqlDataAdapter(ssr1, con);
    //                        DataSet sdss1 = new DataSet();
    //                        ssd1.Fill(sdss1);
    //                        if (sdss1.Tables[0].Rows.Count > 0)
    //                        {
    //                            ss1 = "select * from ExamTimeTableDummyPracTable  where Degree='" + ds1.Tables[0].Rows[i1]["Degree"].ToString() + "'  and  Semester='" + ds1.Tables[0].Rows[i1]["Semester"].ToString() + " '   and Batch='" + ds1.Tables[0].Rows[i1]["Batch"].ToString() + "'";
    //                            SqlDataAdapter sda1 = new SqlDataAdapter(ss1, con);
    //                            DataSet sds1 = new DataSet();
    //                            sda1.Fill(sds1);
    //                            if (sds1.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it1 < sds1.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex1 = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex1.CommandType = CommandType.StoredProcedure;
    //                                    cdex1.Parameters.AddWithValue("@ExamCode", sdss1.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex1.Parameters.AddWithValue("@SubjectNo", sds1.Tables[0].Rows[it1]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex1 = new SqlDataAdapter(cdex1);
    //                                    DataSet dsex1 = new DataSet();
    //                                    daex1.Fill(dsex1);
    //                                    if (dsex1.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex1.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex1.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex1.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt = ds.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex1.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex1.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt1 = ds1.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd1 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd1.CommandType = CommandType.StoredProcedure;
    //                                        cd1.Parameters.AddWithValue("@SubjectNo", sds1.Tables[0].Rows[it1]["SubjectNo"].ToString());
    //                                        cd1.Parameters.AddWithValue("@Duration", RPracticalDuration);
    //                                        cd1.Parameters.AddWithValue("@StartTime", RPracticalStartTime);
    //                                        cd1.Parameters.AddWithValue("@EndTime", RPracticalEndTime);
    //                                        cd1.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd1.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd1.Parameters.AddWithValue("@ExamCode", sdss1.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd1.Parameters.AddWithValue("@ExamType", sdss1.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd1.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd1.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd1.ExecuteNonQuery();
    //                                        i1 = i1 + 1;
    //                                        n1 = n1 + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        existsubject = 1;
    //                                        if (workcount == days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt = ds.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        SqlCommand cd1 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd1.CommandType = CommandType.StoredProcedure;
    //                                        cd1.Parameters.AddWithValue("@SubjectNo", sds1.Tables[0].Rows[it1]["SubjectNo"].ToString());
    //                                        cd1.Parameters.AddWithValue("@Duration", RPracticalDuration);
    //                                        cd1.Parameters.AddWithValue("@StartTime", RPracticalStartTime);
    //                                        cd1.Parameters.AddWithValue("@EndTime", RPracticalEndTime);
    //                                        cd1.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd1.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd1.Parameters.AddWithValue("@ExamCode", sdss1.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd1.Parameters.AddWithValue("@ExamType", sdss1.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd1.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd1.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd1.ExecuteNonQuery();
    //                                        i1 = i1 + 1;
    //                                    }
    //                                }
    //                                else
    //                                {

    //                                    i1 = i1 + 1;

    //                                    emptysubject1 = emptysubject1 + 1;
    //                                    if (emptysubject1 == ds1.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt1 = ds1.Tables[0].Rows.Count;
    //                                        goto dd1;

    //                                    }
    //                                }
    //                            }

    //                            //if (n == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n = 0;

    //                            //}
    //                        }
    //                    }

    //                    it1 = it1 + 1;
    //                    goto l1;

    //                }


    //                days = days + 1;
    //                string degree2;
    //                int n2 = 0;
    //                int cnt2 = 0;
    //                degree2 = "select  degree,Batch,Semester  from  ExamTimeTableDummyArearTable ";
    //                SqlDataAdapter da2 = new SqlDataAdapter(degree2, con);
    //                DataSet ds2 = new DataSet();
    //                da2.Fill(ds2);
    //            dd2: if (ds2.Tables[0].Rows.Count > cnt2)
    //                {
    //                    string ss2;
    //                    int it2 = 0;

    //                l2: int i2 = 0;
    //                    int emptysubject2 = 0;
    //                    while (i2 < ds2.Tables[0].Rows.Count)
    //                    {

    //                        string ssr2;
    //                        ssr2 = "select exam_code,exam_type from exmtt where Degree_Code='" + ds2.Tables[0].Rows[i2]["Degree"].ToString() + "' and batchTo='" + ds2.Tables[0].Rows[i2]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd2 = new SqlDataAdapter(ssr2, con);
    //                        DataSet sdss2 = new DataSet();
    //                        ssd2.Fill(sdss2);
    //                        if (sdss2.Tables[0].Rows.Count > 0)
    //                        {
    //                            ss2 = "select * from ExamTimeTableDummyArearTable  where Degree='" + ds2.Tables[0].Rows[i2]["Degree"].ToString() + "' and Batch='" + ds2.Tables[0].Rows[i2]["Batch"].ToString() + "'";
    //                            SqlDataAdapter sda2 = new SqlDataAdapter(ss2, con);
    //                            DataSet sds2 = new DataSet();
    //                            sda2.Fill(sds2);
    //                            if (sds2.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it2 < sds2.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex2 = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex2.CommandType = CommandType.StoredProcedure;
    //                                    cdex2.Parameters.AddWithValue("@ExamCode", sdss2.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex2.Parameters.AddWithValue("@SubjectNo", sds2.Tables[0].Rows[it2]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex2 = new SqlDataAdapter(cdex2);
    //                                    DataSet dsex2 = new DataSet();
    //                                    daex2.Fill(dsex2);
    //                                    if (dsex2.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex2.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex2.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex2.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt = ds.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex2.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex2.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt2 = ds2.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd2 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd2.CommandType = CommandType.StoredProcedure;
    //                                        cd2.Parameters.AddWithValue("@SubjectNo", sds2.Tables[0].Rows[it2]["SubjectNo"].ToString());
    //                                        cd2.Parameters.AddWithValue("@Duration", ATheoryDuration);
    //                                        cd2.Parameters.AddWithValue("@StartTime", ATheoryStartTime);
    //                                        cd2.Parameters.AddWithValue("@EndTime", ATheoryEndTime);
    //                                        cd2.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd2.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd2.Parameters.AddWithValue("@ExamCode", sdss2.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd2.Parameters.AddWithValue("@ExamType", sdss2.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd2.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd2.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd2.ExecuteNonQuery();
    //                                        i2 = i2 + 1;
    //                                        n2 = n2 + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        existsubject = 1;
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt = ds.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        SqlCommand cd2 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd2.CommandType = CommandType.StoredProcedure;
    //                                        cd2.Parameters.AddWithValue("@SubjectNo", sds2.Tables[0].Rows[it2]["SubjectNo"].ToString());
    //                                        cd2.Parameters.AddWithValue("@Duration", ATheoryDuration);
    //                                        cd2.Parameters.AddWithValue("@StartTime", ATheoryStartTime);
    //                                        cd2.Parameters.AddWithValue("@EndTime", ATheoryEndTime);
    //                                        cd2.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd2.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd2.Parameters.AddWithValue("@ExamCode", sdss2.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd2.Parameters.AddWithValue("@ExamType", sdss2.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd2.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd2.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd2.ExecuteNonQuery();
    //                                        i2 = i2 + 1;
    //                                    }
    //                                }
    //                                else
    //                                {

    //                                    i2 = i2 + 1;

    //                                    emptysubject2 = emptysubject2 + 1;
    //                                    if (emptysubject2 == ds2.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt2 = ds2.Tables[0].Rows.Count;
    //                                        goto dd2;

    //                                    }
    //                                }
    //                            }

    //                            //if (n == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n = 0;

    //                            //}
    //                        }
    //                    }

    //                    it2 = it2 + 1;
    //                    goto l2;

    //                }

    //                days = days + 1;
    //                string degree3;
    //                int n3 = 0;
    //                int cnt3 = 0;
    //                degree3 = "select degree,Batch,Semester  from  ExamTimeTableDummyArearPracTable ";
    //                SqlDataAdapter da3 = new SqlDataAdapter(degree3, con);
    //                DataSet ds3 = new DataSet();
    //                da3.Fill(ds3);
    //            dd3: if (ds3.Tables[0].Rows.Count > cnt3)
    //                {
    //                    string ss3;
    //                    int it3 = 0;

    //                l3: int i3 = 0;
    //                    int emptysubject3 = 0;
    //                    while (i3 < ds3.Tables[0].Rows.Count)
    //                    {

    //                        string ssr3;
    //                        ssr3 = "select exam_code,exam_type from exmtt where Degree_Code='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and batchTo='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "' ";
    //                        SqlDataAdapter ssd3 = new SqlDataAdapter(ssr3, con);
    //                        DataSet sdss3 = new DataSet();
    //                        ssd3.Fill(sdss3);
    //                        if (sdss3.Tables[0].Rows.Count > 0)
    //                        {
    //                            ss3 = "select * from ExamTimeTableDummyArearPracTable  where Degree='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and Batch='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "'";
    //                            SqlDataAdapter sda3 = new SqlDataAdapter(ss3, con);
    //                            DataSet sds3 = new DataSet();
    //                            sda3.Fill(sds3);
    //                            if (sds3.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it3 < sds3.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex3 = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex3.CommandType = CommandType.StoredProcedure;
    //                                    cdex3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex3 = new SqlDataAdapter(cdex3);
    //                                    DataSet dsex3 = new DataSet();
    //                                    daex3.Fill(dsex3);
    //                                    if (dsex3.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex3.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex3.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex3.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt3 = ds3.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex3.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex3.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt3 = ds3.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd3.CommandType = CommandType.StoredProcedure;
    //                                        cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                        cd3.Parameters.AddWithValue("@Duration", APracticalDuration);
    //                                        cd3.Parameters.AddWithValue("@StartTime", APracticalStartTime);
    //                                        cd3.Parameters.AddWithValue("@EndTime", APracticalEndTime);
    //                                        cd3.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd3.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd3.ExecuteNonQuery();
    //                                        i3 = i3 + 1;
    //                                        n3 = n3 + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt3 = ds3.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        existsubject = 1;
    //                                        SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd3.CommandType = CommandType.StoredProcedure;
    //                                        cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                        cd3.Parameters.AddWithValue("@Duration", APracticalDuration);
    //                                        cd3.Parameters.AddWithValue("@StartTime", APracticalStartTime);
    //                                        cd3.Parameters.AddWithValue("@EndTime", APracticalEndTime);
    //                                        cd3.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd3.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd3.ExecuteNonQuery();
    //                                        i3 = i3 + 1;

    //                                    }
    //                                }
    //                                else
    //                                {

    //                                    i3 = i3 + 1;

    //                                    emptysubject3 = emptysubject3 + 1;
    //                                    if (emptysubject3 == ds3.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt3 = ds3.Tables[0].Rows.Count;
    //                                        goto Err;

    //                                    }
    //                                }
    //                            }

    //                            //if (n == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n = 0;

    //                            //}
    //                        }
    //                    }

    //                    it3 = it3 + 1;
    //                    goto l3;

    //                }

    //            }
    //            else if (ddlExamstartswith.SelectedItem.Text.ToString() == "Regular" && ddlExamStartType.SelectedItem.Text.ToString() == "Practical")
    //            {
    //                string degree1;
    //                int n1 = 0;
    //                int cnt1 = 0;
    //                degree1 = "select distinct degree,Batch,Semester  from  ExamTimeTableDummyPracTable ";
    //                SqlDataAdapter da1 = new SqlDataAdapter(degree1, con);
    //                DataSet ds1 = new DataSet();
    //                da1.Fill(ds1);
    //            dd1: if (ds1.Tables[0].Rows.Count > cnt1)
    //                {
    //                    string ss1;
    //                    int it1 = 0;

    //                l1: int i1 = 0;
    //                    int emptysubject1 = 0;
    //                    while (i1 < ds1.Tables[0].Rows.Count)
    //                    {

    //                        string ssr1;
    //                        ssr1 = "select exam_code,exam_type from exmtt where Degree_Code='" + ds1.Tables[0].Rows[i1]["Degree"].ToString() + "' and batchTo='" + ds1.Tables[0].Rows[i1]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd1 = new SqlDataAdapter(ssr1, con);
    //                        DataSet sdss1 = new DataSet();
    //                        ssd1.Fill(sdss1);
    //                        if (sdss1.Tables[0].Rows.Count > 0)
    //                        {
    //                            ss1 = "select * from ExamTimeTableDummyPracTable  where Degree='" + ds1.Tables[0].Rows[i1]["Degree"].ToString() + "'  and  Semester='" + ds1.Tables[0].Rows[i1]["Semester"].ToString() + " ' and Batch='" + ds1.Tables[0].Rows[i1]["Batch"].ToString() + "'";
    //                            SqlDataAdapter sda1 = new SqlDataAdapter(ss1, con);
    //                            DataSet sds1 = new DataSet();
    //                            sda1.Fill(sds1);
    //                            if (sds1.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it1 < sds1.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex1 = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex1.CommandType = CommandType.StoredProcedure;
    //                                    cdex1.Parameters.AddWithValue("@ExamCode", sdss1.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex1.Parameters.AddWithValue("@SubjectNo", sds1.Tables[0].Rows[it1]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex1 = new SqlDataAdapter(cdex1);
    //                                    DataSet dsex1 = new DataSet();
    //                                    daex1.Fill(dsex1);
    //                                    if (dsex1.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex1.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex1.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex1.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt1 = ds1.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }

    //                                                    examdate = dsex1.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex1.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt1 = ds1.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd1 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd1.CommandType = CommandType.StoredProcedure;
    //                                        cd1.Parameters.AddWithValue("@SubjectNo", sds1.Tables[0].Rows[it1]["SubjectNo"].ToString());
    //                                        cd1.Parameters.AddWithValue("@Duration", RPracticalDuration);
    //                                        cd1.Parameters.AddWithValue("@StartTime", RPracticalStartTime);
    //                                        cd1.Parameters.AddWithValue("@EndTime", RPracticalEndTime);
    //                                        cd1.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd1.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd1.Parameters.AddWithValue("@ExamCode", sdss1.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd1.Parameters.AddWithValue("@ExamType", sdss1.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd1.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd1.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd1.ExecuteNonQuery();
    //                                        i1 = i1 + 1;
    //                                        n1 = n1 + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        existsubject = 1;
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt1 = ds1.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd1 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd1.CommandType = CommandType.StoredProcedure;
    //                                        cd1.Parameters.AddWithValue("@SubjectNo", sds1.Tables[0].Rows[it1]["SubjectNo"].ToString());
    //                                        cd1.Parameters.AddWithValue("@Duration", RPracticalDuration);
    //                                        cd1.Parameters.AddWithValue("@StartTime", RPracticalStartTime);
    //                                        cd1.Parameters.AddWithValue("@EndTime", RPracticalEndTime);
    //                                        cd1.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd1.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd1.Parameters.AddWithValue("@ExamCode", sdss1.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd1.Parameters.AddWithValue("@ExamType", sdss1.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd1.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd1.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd1.ExecuteNonQuery();
    //                                        i1 = i1 + 1;

    //                                    }
    //                                }
    //                                else
    //                                {

    //                                    i1 = i1 + 1;

    //                                    emptysubject1 = emptysubject1 + 1;
    //                                    if (emptysubject1 == ds1.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt1 = ds1.Tables[0].Rows.Count;
    //                                        goto dd1;

    //                                    }
    //                                }
    //                            }

    //                            //if (n == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n = 0;

    //                            //}
    //                        }
    //                    }

    //                    it1 = it1 + 1;
    //                    goto l1;

    //                }

    //                days = days + 1;

    //                int n = 0;
    //                int cnt = 0;
    //                degree = "select distinct degree,Batch,Semester  from  ExamTimeTableDummyTable ";
    //                SqlDataAdapter da = new SqlDataAdapter(degree, con);
    //                DataSet ds = new DataSet();
    //                da.Fill(ds);
    //            dd: if (ds.Tables[0].Rows.Count > cnt)
    //                {
    //                    string ss;
    //                    int it = 0;

    //                l: int i = 0;
    //                    int emptysubject = 0;
    //                    while (i < ds.Tables[0].Rows.Count)
    //                    {

    //                        string ssr;
    //                        ssr = "select exam_code,exam_type from exmtt where Degree_Code='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and batchTo='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd = new SqlDataAdapter(ssr, con);
    //                        DataSet sdss = new DataSet();
    //                        ssd.Fill(sdss);
    //                        if (sdss.Tables[0].Rows.Count > 0)
    //                        {
    //                            ss = "select * from ExamTimeTableDummyTable  where Degree='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "'  and  Semester='" + ds.Tables[0].Rows[i]["Semester"].ToString() + " ' and Batch='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                            SqlDataAdapter sda = new SqlDataAdapter(ss, con);
    //                            DataSet sds = new DataSet();
    //                            sda.Fill(sds);
    //                            if (sds.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it < sds.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex.CommandType = CommandType.StoredProcedure;
    //                                    cdex.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex = new SqlDataAdapter(cdex);
    //                                    DataSet dsex = new DataSet();
    //                                    daex.Fill(dsex);
    //                                    if (dsex.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;

    //                                        if (dsex.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt = ds.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt = ds.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd.CommandType = CommandType.StoredProcedure;
    //                                        cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                        cd.Parameters.AddWithValue("@Duration", RTheoryDuration);
    //                                        cd.Parameters.AddWithValue("@StartTime", RTheoryStartTime);
    //                                        cd.Parameters.AddWithValue("@EndTime", RTheoryEndTime);
    //                                        cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd.ExecuteNonQuery();
    //                                        i = i + 1;
    //                                        n = n + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt = ds.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        existsubject = 1;
    //                                        SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd.CommandType = CommandType.StoredProcedure;
    //                                        cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                        cd.Parameters.AddWithValue("@Duration", RTheoryDuration);
    //                                        cd.Parameters.AddWithValue("@StartTime", RTheoryStartTime);
    //                                        cd.Parameters.AddWithValue("@EndTime", RTheoryEndTime);
    //                                        cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd.ExecuteNonQuery();
    //                                        i = i + 1;

    //                                    }
    //                                }
    //                                else
    //                                {

    //                                    i = i + 1;

    //                                    emptysubject = emptysubject + 1;
    //                                    if (emptysubject == ds.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt = ds.Tables[0].Rows.Count;
    //                                        goto dd;

    //                                    }
    //                                }
    //                            }

    //                            //if (n == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n = 0;

    //                            //}
    //                        }
    //                    }

    //                    it = it + 1;
    //                    goto l;

    //                }

    //                days = days + 1;
    //                string degree3;
    //                int n3 = 0;
    //                int cnt3 = 0;
    //                degree3 = "select degree,Batch,Semester  from  ExamTimeTableDummyArearPracTable ";
    //                SqlDataAdapter da3 = new SqlDataAdapter(degree3, con);
    //                DataSet ds3 = new DataSet();
    //                da3.Fill(ds3);
    //            dd3: if (ds3.Tables[0].Rows.Count > cnt3)
    //                {
    //                    string ss3;
    //                    int it3 = 0;

    //                l3: int i3 = 0;
    //                    int emptysubject3 = 0;
    //                    while (i3 < ds3.Tables[0].Rows.Count)
    //                    {

    //                        string ssr3;
    //                        ssr3 = "select exam_code,exam_type from exmtt where Degree_Code='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and batchTo='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd3 = new SqlDataAdapter(ssr3, con);
    //                        DataSet sdss3 = new DataSet();
    //                        ssd3.Fill(sdss3);
    //                        if (sdss3.Tables[0].Rows.Count > 0)
    //                        {
    //                            ss3 = "select * from ExamTimeTableDummyArearPracTable  where Degree='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and Batch='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "'";
    //                            SqlDataAdapter sda3 = new SqlDataAdapter(ss3, con);
    //                            DataSet sds3 = new DataSet();
    //                            sda3.Fill(sds3);
    //                            if (sds3.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it3 < sds3.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex3 = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex3.CommandType = CommandType.StoredProcedure;
    //                                    cdex3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex3 = new SqlDataAdapter(cdex3);
    //                                    DataSet dsex3 = new DataSet();
    //                                    daex3.Fill(dsex3);
    //                                    if (dsex3.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex3.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex3.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex3.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt3 = ds3.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex3.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex3.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt3 = ds3.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd3.CommandType = CommandType.StoredProcedure;
    //                                        cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                        cd3.Parameters.AddWithValue("@Duration", APracticalDuration);
    //                                        cd3.Parameters.AddWithValue("@StartTime", APracticalStartTime);
    //                                        cd3.Parameters.AddWithValue("@EndTime", APracticalEndTime);
    //                                        cd3.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd3.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd3.ExecuteNonQuery();
    //                                        i3 = i3 + 1;
    //                                        n3 = n3 + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt3 = ds3.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        existsubject = 1;
    //                                        SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd3.CommandType = CommandType.StoredProcedure;
    //                                        cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                        cd3.Parameters.AddWithValue("@Duration", APracticalDuration);
    //                                        cd3.Parameters.AddWithValue("@StartTime", APracticalStartTime);
    //                                        cd3.Parameters.AddWithValue("@EndTime", APracticalEndTime);
    //                                        cd3.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd3.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd3.ExecuteNonQuery();
    //                                        i3 = i3 + 1;

    //                                    }
    //                                }
    //                                else
    //                                {

    //                                    i3 = i3 + 1;

    //                                    emptysubject3 = emptysubject3 + 1;
    //                                    if (emptysubject3 == ds3.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt3 = ds3.Tables[0].Rows.Count;
    //                                        goto dd3;

    //                                    }
    //                                }
    //                            }

    //                            //if (n == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n = 0;

    //                            //}
    //                        }
    //                    }

    //                    it3 = it3 + 1;
    //                    goto l3;

    //                }




    //                days = days + 1;
    //                string degree2;
    //                int n2 = 0;
    //                int cnt2 = 0;
    //                degree2 = "select  degree,Batch,Semester  from  ExamTimeTableDummyArearTable ";
    //                SqlDataAdapter da2 = new SqlDataAdapter(degree2, con);
    //                DataSet ds2 = new DataSet();
    //                da2.Fill(ds2);
    //            dd2: if (ds2.Tables[0].Rows.Count > cnt2)
    //                {
    //                    string ss2;
    //                    int it2 = 0;

    //                l2: int i2 = 0;
    //                    int emptysubject2 = 0;
    //                    while (i2 < ds2.Tables[0].Rows.Count)
    //                    {

    //                        string ssr2;
    //                        ssr2 = "select exam_code,exam_type from exmtt where Degree_Code='" + ds2.Tables[0].Rows[i2]["Degree"].ToString() + "' and batchTo='" + ds2.Tables[0].Rows[i2]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd2 = new SqlDataAdapter(ssr2, con);
    //                        DataSet sdss2 = new DataSet();
    //                        ssd2.Fill(sdss2);
    //                        if (sdss2.Tables[0].Rows.Count > 0)
    //                        {
    //                            ss2 = "select * from ExamTimeTableDummyArearTable  where Degree='" + ds2.Tables[0].Rows[i2]["Degree"].ToString() + "' and Batch='" + ds2.Tables[0].Rows[i2]["Batch"].ToString() + "'";
    //                            SqlDataAdapter sda2 = new SqlDataAdapter(ss2, con);
    //                            DataSet sds2 = new DataSet();
    //                            sda2.Fill(sds2);
    //                            if (sds2.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it2 < sds2.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex2 = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex2.CommandType = CommandType.StoredProcedure;
    //                                    cdex2.Parameters.AddWithValue("@ExamCode", sdss2.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex2.Parameters.AddWithValue("@SubjectNo", sds2.Tables[0].Rows[it2]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex2 = new SqlDataAdapter(cdex2);
    //                                    DataSet dsex2 = new DataSet();
    //                                    daex2.Fill(dsex2);
    //                                    if (dsex2.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex2.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex2.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex2.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt2 = ds2.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex2.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex2.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt2 = ds2.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd2 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd2.CommandType = CommandType.StoredProcedure;
    //                                        cd2.Parameters.AddWithValue("@SubjectNo", sds2.Tables[0].Rows[it2]["SubjectNo"].ToString());
    //                                        cd2.Parameters.AddWithValue("@Duration", ATheoryDuration);
    //                                        cd2.Parameters.AddWithValue("@StartTime", ATheoryStartTime);
    //                                        cd2.Parameters.AddWithValue("@EndTime", ATheoryEndTime);
    //                                        cd2.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd2.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd2.Parameters.AddWithValue("@ExamCode", sdss2.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd2.Parameters.AddWithValue("@ExamType", sdss2.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd2.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd2.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd2.ExecuteNonQuery();
    //                                        i2 = i2 + 1;
    //                                        n2 = n2 + 1;
    //                                    }
    //                                    else
    //                                    {

    //                                        existsubject = 1;
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt2 = ds2.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        SqlCommand cd2 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd2.CommandType = CommandType.StoredProcedure;
    //                                        cd2.Parameters.AddWithValue("@SubjectNo", sds2.Tables[0].Rows[it2]["SubjectNo"].ToString());
    //                                        cd2.Parameters.AddWithValue("@Duration", ATheoryDuration);
    //                                        cd2.Parameters.AddWithValue("@StartTime", ATheoryStartTime);
    //                                        cd2.Parameters.AddWithValue("@EndTime", ATheoryEndTime);
    //                                        cd2.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd2.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd2.Parameters.AddWithValue("@ExamCode", sdss2.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd2.Parameters.AddWithValue("@ExamType", sdss2.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd2.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd2.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd2.ExecuteNonQuery();
    //                                        i2 = i2 + 1;
    //                                    }

    //                                }
    //                                else
    //                                {

    //                                    i2 = i2 + 1;

    //                                    emptysubject2 = emptysubject2 + 1;
    //                                    if (emptysubject2 == ds2.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt2 = ds2.Tables[0].Rows.Count;
    //                                        goto Err;

    //                                    }
    //                                }
    //                            }

    //                            //if (n == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n = 0;

    //                            //}
    //                        }
    //                    }

    //                    it2 = it2 + 1;
    //                    goto l2;

    //                }


    //            }
    //            else if (ddlExamstartswith.SelectedItem.Text.ToString() == "Arear" && ddlExamStartType.SelectedItem.Text.ToString() == "Theory")
    //            {

    //                string degree2;
    //                int n2 = 0;
    //                int cnt2 = 0;
    //                degree2 = "select  degree,Batch,Semester  from  ExamTimeTableDummyArearTable ";
    //                SqlDataAdapter da2 = new SqlDataAdapter(degree2, con);
    //                DataSet ds2 = new DataSet();
    //                da2.Fill(ds2);
    //            dd2: if (ds2.Tables[0].Rows.Count > cnt2)
    //                {
    //                    string ss2;
    //                    int it2 = 0;

    //                l2: int i2 = 0;
    //                    int emptysubject2 = 0;
    //                    while (i2 < ds2.Tables[0].Rows.Count)
    //                    {

    //                        string ssr2;
    //                        ssr2 = "select exam_code,exam_type from exmtt where Degree_Code='" + ds2.Tables[0].Rows[i2]["Degree"].ToString() + "' and batchTo='" + ds2.Tables[0].Rows[i2]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd2 = new SqlDataAdapter(ssr2, con);
    //                        DataSet sdss2 = new DataSet();
    //                        ssd2.Fill(sdss2);
    //                        if (sdss2.Tables[0].Rows.Count > 0)
    //                        {
    //                            ss2 = "select * from ExamTimeTableDummyArearTable  where Degree='" + ds2.Tables[0].Rows[i2]["Degree"].ToString() + "' and Batch='" + ds2.Tables[0].Rows[i2]["Batch"].ToString() + "'";
    //                            SqlDataAdapter sda2 = new SqlDataAdapter(ss2, con);
    //                            DataSet sds2 = new DataSet();
    //                            sda2.Fill(sds2);
    //                            if (sds2.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it2 < sds2.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex2 = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex2.CommandType = CommandType.StoredProcedure;
    //                                    cdex2.Parameters.AddWithValue("@ExamCode", sdss2.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex2.Parameters.AddWithValue("@SubjectNo", sds2.Tables[0].Rows[it2]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex2 = new SqlDataAdapter(cdex2);
    //                                    DataSet dsex2 = new DataSet();
    //                                    daex2.Fill(dsex2);
    //                                    if (dsex2.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex2.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex2.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex2.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt2 = ds2.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex2.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex2.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt2 = ds2.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd2 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd2.CommandType = CommandType.StoredProcedure;
    //                                        cd2.Parameters.AddWithValue("@SubjectNo", sds2.Tables[0].Rows[it2]["SubjectNo"].ToString());
    //                                        cd2.Parameters.AddWithValue("@Duration", ATheoryDuration);
    //                                        cd2.Parameters.AddWithValue("@StartTime", ATheoryStartTime);
    //                                        cd2.Parameters.AddWithValue("@EndTime", ATheoryEndTime);
    //                                        cd2.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd2.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd2.Parameters.AddWithValue("@ExamCode", sdss2.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd2.Parameters.AddWithValue("@ExamType", sdss2.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd2.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd2.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd2.ExecuteNonQuery();
    //                                        i2 = i2 + 1;
    //                                        n2 = n2 + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        existsubject = 1;
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt2 = ds2.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        SqlCommand cd2 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd2.CommandType = CommandType.StoredProcedure;
    //                                        cd2.Parameters.AddWithValue("@SubjectNo", sds2.Tables[0].Rows[it2]["SubjectNo"].ToString());
    //                                        cd2.Parameters.AddWithValue("@Duration", ATheoryDuration);
    //                                        cd2.Parameters.AddWithValue("@StartTime", ATheoryStartTime);
    //                                        cd2.Parameters.AddWithValue("@EndTime", ATheoryEndTime);
    //                                        cd2.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd2.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd2.Parameters.AddWithValue("@ExamCode", sdss2.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd2.Parameters.AddWithValue("@ExamType", sdss2.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd2.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd2.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd2.ExecuteNonQuery();
    //                                        i2 = i2 + 1;

    //                                    }
    //                                }
    //                                else
    //                                {

    //                                    i2 = i2 + 1;

    //                                    emptysubject2 = emptysubject2 + 1;
    //                                    if (emptysubject2 == ds2.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt2 = ds2.Tables[0].Rows.Count;
    //                                        goto dd2;

    //                                    }
    //                                }
    //                            }

    //                            //if (n == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n = 0;

    //                            //}
    //                        }
    //                    }

    //                    it2 = it2 + 1;
    //                    goto l2;

    //                }

    //                days = days + 1;
    //                int n = 0;
    //                int cnt = 0;
    //                degree = "select distinct degree,Batch,Semester  from  ExamTimeTableDummyTable ";
    //                SqlDataAdapter da = new SqlDataAdapter(degree, con);
    //                DataSet ds = new DataSet();
    //                da.Fill(ds);
    //            dd: if (ds.Tables[0].Rows.Count > cnt)
    //                {
    //                    string ss;
    //                    int it = 0;

    //                l: int i = 0;
    //                    int emptysubject = 0;
    //                    while (i < ds.Tables[0].Rows.Count)
    //                    {

    //                        string ssr;
    //                        ssr = "select exam_code,exam_type from exmtt where Degree_Code='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and batchTo='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd = new SqlDataAdapter(ssr, con);
    //                        DataSet sdss = new DataSet();
    //                        ssd.Fill(sdss);
    //                        if (sdss.Tables[0].Rows.Count > 0)
    //                        {
    //                            ss = "select * from ExamTimeTableDummyTable  where Degree='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "'  and  Semester='" + ds.Tables[0].Rows[i]["Semester"].ToString() + " ' and Batch='" + ds.Tables[0].Rows[i]["Semester"].ToString() + "'";
    //                            SqlDataAdapter sda = new SqlDataAdapter(ss, con);
    //                            DataSet sds = new DataSet();
    //                            sda.Fill(sds);
    //                            if (sds.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it < sds.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex.CommandType = CommandType.StoredProcedure;
    //                                    cdex.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex = new SqlDataAdapter(cdex);
    //                                    DataSet dsex = new DataSet();
    //                                    daex.Fill(dsex);
    //                                    if (dsex.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt = ds.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt = ds.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd.CommandType = CommandType.StoredProcedure;
    //                                        cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                        cd.Parameters.AddWithValue("@Duration", RTheoryDuration);
    //                                        cd.Parameters.AddWithValue("@StartTime", RTheoryStartTime);
    //                                        cd.Parameters.AddWithValue("@EndTime", RTheoryEndTime);
    //                                        cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd.ExecuteNonQuery();
    //                                        i = i + 1;
    //                                        n = n + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        existsubject = 1;
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt = ds.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd.CommandType = CommandType.StoredProcedure;
    //                                        cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                        cd.Parameters.AddWithValue("@Duration", RTheoryDuration);
    //                                        cd.Parameters.AddWithValue("@StartTime", RTheoryStartTime);
    //                                        cd.Parameters.AddWithValue("@EndTime", RTheoryEndTime);
    //                                        cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd.ExecuteNonQuery();
    //                                        i = i + 1;

    //                                    }
    //                                }
    //                                else
    //                                {

    //                                    i = i + 1;

    //                                    emptysubject = emptysubject + 1;
    //                                    if (emptysubject == ds.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt = ds.Tables[0].Rows.Count;
    //                                        goto dd;

    //                                    }
    //                                }
    //                            }

    //                            //if (n == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n = 0;

    //                            //}
    //                        }
    //                    }

    //                    it = it + 1;
    //                    goto l;

    //                }

    //                days = days + 1;


    //                string degree3;
    //                int n3 = 0;
    //                int cnt3 = 0;
    //                degree3 = "select  degree,Batch,Semester  from  ExamTimeTableDummyArearPracTable ";
    //                SqlDataAdapter da3 = new SqlDataAdapter(degree3, con);
    //                DataSet ds3 = new DataSet();
    //                da3.Fill(ds3);
    //            dd3: if (ds3.Tables[0].Rows.Count > cnt3)
    //                {
    //                    string ss3;
    //                    int it3 = 0;

    //                l3: int i3 = 0;
    //                    int emptysubject3 = 0;
    //                    while (i3 < ds3.Tables[0].Rows.Count)
    //                    {

    //                        string ssr3;
    //                        ssr3 = "select exam_code,exam_type from exmtt where Degree_Code='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and batchTo='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd3 = new SqlDataAdapter(ssr3, con);
    //                        DataSet sdss3 = new DataSet();
    //                        ssd3.Fill(sdss3);
    //                        if (sdss3.Tables[0].Rows.Count > 0)
    //                        {
    //                            ss3 = "select * from ExamTimeTableDummyArearPracTable  where Degree='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and Batch='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "'";
    //                            SqlDataAdapter sda3 = new SqlDataAdapter(ss3, con);
    //                            DataSet sds3 = new DataSet();
    //                            sda3.Fill(sds3);
    //                            if (sds3.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it3 < sds3.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex3 = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex3.CommandType = CommandType.StoredProcedure;
    //                                    cdex3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex3 = new SqlDataAdapter(cdex3);
    //                                    DataSet dsex3 = new DataSet();
    //                                    daex3.Fill(dsex3);
    //                                    if (dsex3.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex3.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex3.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex3.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt3 = ds3.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex3.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex3.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt3 = ds3.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd3.CommandType = CommandType.StoredProcedure;
    //                                        cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                        cd3.Parameters.AddWithValue("@Duration", APracticalDuration);
    //                                        cd3.Parameters.AddWithValue("@StartTime", APracticalStartTime);
    //                                        cd3.Parameters.AddWithValue("@EndTime", APracticalEndTime);
    //                                        cd3.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd3.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd3.ExecuteNonQuery();
    //                                        i3 = i3 + 1;
    //                                        n3 = n3 + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        existsubject = 1;
    //                                        if (workcount == days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt3 = ds3.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd3.CommandType = CommandType.StoredProcedure;
    //                                        cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                        cd3.Parameters.AddWithValue("@Duration", APracticalDuration);
    //                                        cd3.Parameters.AddWithValue("@StartTime", APracticalStartTime);
    //                                        cd3.Parameters.AddWithValue("@EndTime", APracticalEndTime);
    //                                        cd3.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd3.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd3.ExecuteNonQuery();
    //                                        i3 = i3 + 1;

    //                                    }
    //                                }
    //                                else
    //                                {

    //                                    i3 = i3 + 1;

    //                                    emptysubject3 = emptysubject3 + 1;
    //                                    if (emptysubject3 == ds3.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt3 = ds3.Tables[0].Rows.Count;
    //                                        goto dd3;

    //                                    }
    //                                }
    //                            }

    //                            //if (n == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n = 0;

    //                            //}
    //                        }
    //                    }

    //                    it3 = it3 + 1;
    //                    goto l3;

    //                }





    //                days = days + 1;
    //                string degree1;
    //                int n1 = 0;
    //                int cnt1 = 0;
    //                degree1 = "select distinct degree,Batch,Semester  from  ExamTimeTableDummyPracTable ";
    //                SqlDataAdapter da1 = new SqlDataAdapter(degree1, con);
    //                DataSet ds1 = new DataSet();
    //                da1.Fill(ds1);
    //            dd1: if (ds1.Tables[0].Rows.Count > cnt1)
    //                {
    //                    string ss1;
    //                    int it1 = 0;

    //                l1: int i1 = 0;
    //                    int emptysubject1 = 0;
    //                    while (i1 < ds1.Tables[0].Rows.Count)
    //                    {

    //                        string ssr1;
    //                        ssr1 = "select exam_code,exam_type from exmtt where Degree_Code='" + ds1.Tables[0].Rows[i1]["Degree"].ToString() + "' and batchTo='" + ds1.Tables[0].Rows[i1]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd1 = new SqlDataAdapter(ssr1, con);
    //                        DataSet sdss1 = new DataSet();
    //                        ssd1.Fill(sdss1);
    //                        if (sdss1.Tables[0].Rows.Count > 0)
    //                        {
    //                            ss1 = "select * from ExamTimeTableDummyPracTable  where Degree='" + ds1.Tables[0].Rows[i1]["Degree"].ToString() + "'  and  Semester='" + ds1.Tables[0].Rows[i1]["Semester"].ToString() + " ' and Batch='" + ds1.Tables[0].Rows[i1]["Batch"].ToString() + "'";
    //                            SqlDataAdapter sda1 = new SqlDataAdapter(ss1, con);
    //                            DataSet sds1 = new DataSet();
    //                            sda1.Fill(sds1);
    //                            if (sds1.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it1 < sds1.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex1 = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex1.CommandType = CommandType.StoredProcedure;
    //                                    cdex1.Parameters.AddWithValue("@ExamCode", sdss1.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex1.Parameters.AddWithValue("@SubjectNo", sds1.Tables[0].Rows[it1]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex1 = new SqlDataAdapter(cdex1);
    //                                    DataSet dsex1 = new DataSet();
    //                                    daex1.Fill(dsex1);
    //                                    if (dsex1.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex1.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex1.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex1.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt1 = ds1.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex1.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex1.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt1 = ds1.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd1 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd1.CommandType = CommandType.StoredProcedure;
    //                                        cd1.Parameters.AddWithValue("@SubjectNo", sds1.Tables[0].Rows[it1]["SubjectNo"].ToString());
    //                                        cd1.Parameters.AddWithValue("@Duration", RPracticalDuration);
    //                                        cd1.Parameters.AddWithValue("@StartTime", RPracticalStartTime);
    //                                        cd1.Parameters.AddWithValue("@EndTime", RPracticalEndTime);
    //                                        cd1.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd1.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd1.Parameters.AddWithValue("@ExamCode", sdss1.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd1.Parameters.AddWithValue("@ExamType", sdss1.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd1.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd1.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd1.ExecuteNonQuery();
    //                                        i1 = i1 + 1;
    //                                        n1 = n1 + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt1 = ds1.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        existsubject = 1;
    //                                        SqlCommand cd1 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd1.CommandType = CommandType.StoredProcedure;
    //                                        cd1.Parameters.AddWithValue("@SubjectNo", sds1.Tables[0].Rows[it1]["SubjectNo"].ToString());
    //                                        cd1.Parameters.AddWithValue("@Duration", RPracticalDuration);
    //                                        cd1.Parameters.AddWithValue("@StartTime", RPracticalStartTime);
    //                                        cd1.Parameters.AddWithValue("@EndTime", RPracticalEndTime);
    //                                        cd1.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd1.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd1.Parameters.AddWithValue("@ExamCode", sdss1.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd1.Parameters.AddWithValue("@ExamType", sdss1.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd1.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd1.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd1.ExecuteNonQuery();
    //                                        i1 = i1 + 1;

    //                                    }
    //                                }
    //                                else
    //                                {

    //                                    i1 = i1 + 1;

    //                                    emptysubject1 = emptysubject1 + 1;
    //                                    if (emptysubject1 == ds1.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt1 = ds1.Tables[0].Rows.Count;
    //                                        goto Err;

    //                                    }
    //                                }
    //                            }

    //                            //if (n == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n = 0;

    //                            //}
    //                        }
    //                    }

    //                    it1 = it1 + 1;
    //                    goto l1;

    //                }



    //            }
    //            else if (ddlExamstartswith.SelectedItem.Text.ToString() == "Arear" && ddlExamStartType.SelectedItem.Text.ToString() == "Practical")
    //            {
    //                string degree3;
    //                int n3 = 0;
    //                int cnt3 = 0;
    //                degree3 = "select degree,Batch,Semester  from  ExamTimeTableDummyArearPracTable ";
    //                SqlDataAdapter da3 = new SqlDataAdapter(degree3, con);
    //                DataSet ds3 = new DataSet();
    //                da3.Fill(ds3);
    //            dd3: if (ds3.Tables[0].Rows.Count > cnt3)
    //                {
    //                    string ss3;
    //                    int it3 = 0;

    //                l3: int i3 = 0;
    //                    int emptysubject3 = 0;
    //                    while (i3 < ds3.Tables[0].Rows.Count)
    //                    {

    //                        string ssr3;
    //                        ssr3 = "select exam_code,exam_type from exmtt where Degree_Code='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and batchTo='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd3 = new SqlDataAdapter(ssr3, con);
    //                        DataSet sdss3 = new DataSet();
    //                        ssd3.Fill(sdss3);
    //                        if (sdss3.Tables[0].Rows.Count > 0)
    //                        {
    //                            ss3 = "select * from ExamTimeTableDummyArearPracTable  where Degree='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and Batch='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "'";
    //                            SqlDataAdapter sda3 = new SqlDataAdapter(ss3, con);
    //                            DataSet sds3 = new DataSet();
    //                            sda3.Fill(sds3);
    //                            if (sds3.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it3 < sds3.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex3 = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex3.CommandType = CommandType.StoredProcedure;
    //                                    cdex3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex3 = new SqlDataAdapter(cdex3);
    //                                    DataSet dsex3 = new DataSet();
    //                                    daex3.Fill(dsex3);
    //                                    if (dsex3.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex3.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex3.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex3.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt3 = ds3.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex3.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex3.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt3 = ds3.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd3.CommandType = CommandType.StoredProcedure;
    //                                        cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                        cd3.Parameters.AddWithValue("@Duration", APracticalDuration);
    //                                        cd3.Parameters.AddWithValue("@StartTime", APracticalStartTime);
    //                                        cd3.Parameters.AddWithValue("@EndTime", APracticalEndTime);
    //                                        cd3.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd3.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd3.ExecuteNonQuery();
    //                                        i3 = i3 + 1;
    //                                        n3 = n3 + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        existsubject = 1;
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt3 = ds3.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd3.CommandType = CommandType.StoredProcedure;
    //                                        cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                        cd3.Parameters.AddWithValue("@Duration", APracticalDuration);
    //                                        cd3.Parameters.AddWithValue("@StartTime", APracticalStartTime);
    //                                        cd3.Parameters.AddWithValue("@EndTime", APracticalEndTime);
    //                                        cd3.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd3.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd3.ExecuteNonQuery();
    //                                        i3 = i3 + 1;

    //                                    }
    //                                }
    //                                else
    //                                {

    //                                    i3 = i3 + 1;

    //                                    emptysubject3 = emptysubject3 + 1;
    //                                    if (emptysubject3 == ds3.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt3 = ds3.Tables[0].Rows.Count;
    //                                        goto dd3;

    //                                    }
    //                                }
    //                            }

    //                            //if (n == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n = 0;

    //                            //}
    //                        }
    //                    }

    //                    it3 = it3 + 1;
    //                    goto l3;

    //                }





    //                days = days + 1;
    //                string degree1;
    //                int n1 = 0;
    //                int cnt1 = 0;
    //                degree1 = "select distinct degree,Batch,Semester  from  ExamTimeTableDummyPracTable ";
    //                SqlDataAdapter da1 = new SqlDataAdapter(degree1, con);
    //                DataSet ds1 = new DataSet();
    //                da1.Fill(ds1);
    //            dd1: if (ds1.Tables[0].Rows.Count > cnt1)
    //                {
    //                    string ss1;
    //                    int it1 = 0;

    //                l1: int i1 = 0;
    //                    int emptysubject1 = 0;
    //                    while (i1 < ds1.Tables[0].Rows.Count)
    //                    {

    //                        string ssr1;
    //                        ssr1 = "select exam_code,exam_type from exmtt where Degree_Code='" + ds1.Tables[0].Rows[i1]["Degree"].ToString() + "' and batchTo='" + ds1.Tables[0].Rows[i1]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd1 = new SqlDataAdapter(ssr1, con);
    //                        DataSet sdss1 = new DataSet();
    //                        ssd1.Fill(sdss1);
    //                        if (sdss1.Tables[0].Rows.Count > 0)
    //                        {
    //                            ss1 = "select * from ExamTimeTableDummyPracTable  where Degree='" + ds1.Tables[0].Rows[i1]["Degree"].ToString() + "' and  Semester='" + ds1.Tables[0].Rows[i1]["Semester"].ToString() + " ' and Batch='" + ds1.Tables[0].Rows[i1]["Batch"].ToString() + "'";
    //                            SqlDataAdapter sda1 = new SqlDataAdapter(ss1, con);
    //                            DataSet sds1 = new DataSet();
    //                            sda1.Fill(sds1);
    //                            if (sds1.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it1 < sds1.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex1 = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex1.CommandType = CommandType.StoredProcedure;
    //                                    cdex1.Parameters.AddWithValue("@ExamCode", sdss1.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex1.Parameters.AddWithValue("@SubjectNo", sds1.Tables[0].Rows[it1]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex1 = new SqlDataAdapter(cdex1);
    //                                    DataSet dsex1 = new DataSet();
    //                                    daex1.Fill(dsex1);
    //                                    if (dsex1.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex1.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex1.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex1.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt1 = ds1.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex1.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex1.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt1 = ds1.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd1 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd1.CommandType = CommandType.StoredProcedure;
    //                                        cd1.Parameters.AddWithValue("@SubjectNo", sds1.Tables[0].Rows[it1]["SubjectNo"].ToString());
    //                                        cd1.Parameters.AddWithValue("@Duration", RPracticalDuration);
    //                                        cd1.Parameters.AddWithValue("@StartTime", RPracticalStartTime);
    //                                        cd1.Parameters.AddWithValue("@EndTime", RPracticalEndTime);
    //                                        cd1.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd1.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd1.Parameters.AddWithValue("@ExamCode", sdss1.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd1.Parameters.AddWithValue("@ExamType", sdss1.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd1.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd1.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd1.ExecuteNonQuery();
    //                                        i1 = i1 + 1;
    //                                        n1 = n1 + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        existsubject = 1;
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt1 = ds1.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        SqlCommand cd1 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd1.CommandType = CommandType.StoredProcedure;
    //                                        cd1.Parameters.AddWithValue("@SubjectNo", sds1.Tables[0].Rows[it1]["SubjectNo"].ToString());
    //                                        cd1.Parameters.AddWithValue("@Duration", RPracticalDuration);
    //                                        cd1.Parameters.AddWithValue("@StartTime", RPracticalStartTime);
    //                                        cd1.Parameters.AddWithValue("@EndTime", RPracticalEndTime);
    //                                        cd1.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd1.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd1.Parameters.AddWithValue("@ExamCode", sdss1.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd1.Parameters.AddWithValue("@ExamType", sdss1.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd1.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd1.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd1.ExecuteNonQuery();
    //                                        i1 = i1 + 1;
    //                                    }
    //                                }
    //                                else
    //                                {

    //                                    i1 = i1 + 1;

    //                                    emptysubject1 = emptysubject1 + 1;
    //                                    if (emptysubject1 == ds1.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt1 = ds1.Tables[0].Rows.Count;
    //                                        goto dd1;

    //                                    }
    //                                }
    //                            }

    //                            //if (n == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n = 0;

    //                            //}
    //                        }
    //                    }

    //                    it1 = it1 + 1;
    //                    goto l1;

    //                }


    //                days = days + 1;
    //                string degree2;
    //                int n2 = 0;
    //                int cnt2 = 0;
    //                degree2 = "select  degree,Batch,Semester  from  ExamTimeTableDummyArearTable ";
    //                SqlDataAdapter da2 = new SqlDataAdapter(degree2, con);
    //                DataSet ds2 = new DataSet();
    //                da2.Fill(ds2);
    //            dd2: if (ds2.Tables[0].Rows.Count > cnt2)
    //                {
    //                    string ss2;
    //                    int it2 = 0;

    //                l2: int i2 = 0;
    //                    int emptysubject2 = 0;
    //                    while (i2 < ds2.Tables[0].Rows.Count)
    //                    {

    //                        string ssr2;
    //                        ssr2 = "select exam_code,exam_type from exmtt where Degree_Code='" + ds2.Tables[0].Rows[i2]["Degree"].ToString() + "' and batchTo='" + ds2.Tables[0].Rows[i2]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd2 = new SqlDataAdapter(ssr2, con);
    //                        DataSet sdss2 = new DataSet();
    //                        ssd2.Fill(sdss2);
    //                        if (sdss2.Tables[0].Rows.Count > 0)
    //                        {
    //                            ss2 = "select * from ExamTimeTableDummyArearTable  where Degree='" + ds2.Tables[0].Rows[i2]["Degree"].ToString() + "' and Batch='" + ds2.Tables[0].Rows[i2]["Batch"].ToString() + "'";
    //                            SqlDataAdapter sda2 = new SqlDataAdapter(ss2, con);
    //                            DataSet sds2 = new DataSet();
    //                            sda2.Fill(sds2);
    //                            if (sds2.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it2 < sds2.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex2 = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex2.CommandType = CommandType.StoredProcedure;
    //                                    cdex2.Parameters.AddWithValue("@ExamCode", sdss2.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex2.Parameters.AddWithValue("@SubjectNo", sds2.Tables[0].Rows[it2]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex2 = new SqlDataAdapter(cdex2);
    //                                    DataSet dsex2 = new DataSet();
    //                                    daex2.Fill(dsex2);
    //                                    if (dsex2.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex2.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex2.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex2.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt2 = ds2.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex2.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex2.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt2 = ds2.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd2 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd2.CommandType = CommandType.StoredProcedure;
    //                                        cd2.Parameters.AddWithValue("@SubjectNo", sds2.Tables[0].Rows[it2]["SubjectNo"].ToString());
    //                                        cd2.Parameters.AddWithValue("@Duration", ATheoryDuration);
    //                                        cd2.Parameters.AddWithValue("@StartTime", ATheoryStartTime);
    //                                        cd2.Parameters.AddWithValue("@EndTime", ATheoryEndTime);
    //                                        cd2.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd2.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd2.Parameters.AddWithValue("@ExamCode", sdss2.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd2.Parameters.AddWithValue("@ExamType", sdss2.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd2.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd2.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd2.ExecuteNonQuery();
    //                                        i2 = i2 + 1;
    //                                        n2 = n2 + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt2 = ds2.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        existsubject = 1;
    //                                        SqlCommand cd2 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd2.CommandType = CommandType.StoredProcedure;
    //                                        cd2.Parameters.AddWithValue("@SubjectNo", sds2.Tables[0].Rows[it2]["SubjectNo"].ToString());
    //                                        cd2.Parameters.AddWithValue("@Duration", ATheoryDuration);
    //                                        cd2.Parameters.AddWithValue("@StartTime", ATheoryStartTime);
    //                                        cd2.Parameters.AddWithValue("@EndTime", ATheoryEndTime);
    //                                        cd2.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd2.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd2.Parameters.AddWithValue("@ExamCode", sdss2.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd2.Parameters.AddWithValue("@ExamType", sdss2.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd2.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd2.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd2.ExecuteNonQuery();
    //                                        i2 = i2 + 1;

    //                                    }
    //                                }
    //                                else
    //                                {

    //                                    i2 = i2 + 1;

    //                                    emptysubject2 = emptysubject2 + 1;
    //                                    if (emptysubject2 == ds2.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt2 = ds2.Tables[0].Rows.Count;
    //                                        goto dd2;

    //                                    }
    //                                }
    //                            }

    //                            //if (n == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n = 0;

    //                            //}
    //                        }
    //                    }

    //                    it2 = it2 + 1;
    //                    goto l2;

    //                }

    //                days = days + 1;
    //                int n = 0;
    //                int cnt = 0;
    //                degree = "select distinct degree,Batch,Semester  from  ExamTimeTableDummyTable ";
    //                SqlDataAdapter da = new SqlDataAdapter(degree, con);
    //                DataSet ds = new DataSet();
    //                da.Fill(ds);
    //            dd: if (ds.Tables[0].Rows.Count > cnt)
    //                {
    //                    string ss;
    //                    int it = 0;

    //                l: int i = 0;
    //                    int emptysubject = 0;
    //                    while (i < ds.Tables[0].Rows.Count)
    //                    {

    //                        string ssr;
    //                        ssr = "select exam_code,exam_type from exmtt where Degree_Code='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and batchTo='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd = new SqlDataAdapter(ssr, con);
    //                        DataSet sdss = new DataSet();
    //                        ssd.Fill(sdss);
    //                        if (sdss.Tables[0].Rows.Count > 0)
    //                        {
    //                            ss = "select * from ExamTimeTableDummyTable  where Degree='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and  Semester='" + ds.Tables[0].Rows[i]["Semester"].ToString() + " ' and Batch='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                            SqlDataAdapter sda = new SqlDataAdapter(ss, con);
    //                            DataSet sds = new DataSet();
    //                            sda.Fill(sds);
    //                            if (sds.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it < sds.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex.CommandType = CommandType.StoredProcedure;
    //                                    cdex.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex = new SqlDataAdapter(cdex);
    //                                    DataSet dsex = new DataSet();
    //                                    daex.Fill(dsex);
    //                                    if (dsex.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt = ds.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt = ds.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd.CommandType = CommandType.StoredProcedure;
    //                                        cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                        cd.Parameters.AddWithValue("@Duration", RTheoryDuration);
    //                                        cd.Parameters.AddWithValue("@StartTime", RTheoryStartTime);
    //                                        cd.Parameters.AddWithValue("@EndTime", RTheoryEndTime);
    //                                        cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd.ExecuteNonQuery();
    //                                        i = i + 1;
    //                                        n = n + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        existsubject = 1;
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt = ds.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd.CommandType = CommandType.StoredProcedure;
    //                                        cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                        cd.Parameters.AddWithValue("@Duration", RTheoryDuration);
    //                                        cd.Parameters.AddWithValue("@StartTime", RTheoryStartTime);
    //                                        cd.Parameters.AddWithValue("@EndTime", RTheoryEndTime);
    //                                        cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd.ExecuteNonQuery();
    //                                        i = i + 1;
    //                                    }
    //                                }
    //                                else
    //                                {

    //                                    i = i + 1;

    //                                    emptysubject = emptysubject + 1;
    //                                    if (emptysubject == ds.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt = ds.Tables[0].Rows.Count;
    //                                        goto Err;

    //                                    }
    //                                }
    //                            }

    //                            //if (n == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n = 0;

    //                            //}
    //                        }
    //                    }

    //                    it = it + 1;
    //                    goto l;

    //                }

    //            }
    //            else if (ddlExamstartswith.SelectedItem.Text.ToString() == "Mixed")
    //            {
    //                int n = 0;
    //                int cnt = 0;
    //                degree = "select distinct degree,Batch,Semester  from  ExamTimeTableDummyTable ";
    //                SqlDataAdapter da = new SqlDataAdapter(degree, con);
    //                DataSet ds = new DataSet();
    //                da.Fill(ds);
    //            dd: if (ds.Tables[0].Rows.Count > cnt)
    //                {
    //                    string ss;
    //                    int it = 0;

    //                l: int i = 0;
    //                    int emptysubject = 0;
    //                    while (i < ds.Tables[0].Rows.Count)
    //                    {

    //                        string ssr;
    //                        ssr = "select exam_code,exam_type from exmtt where Degree_Code='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and batchTo='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd = new SqlDataAdapter(ssr, con);
    //                        DataSet sdss = new DataSet();
    //                        ssd.Fill(sdss);
    //                        if (sdss.Tables[0].Rows.Count > 0)
    //                        {
    //                            ss = "select * from ExamTimeTableDummyTable  where Degree='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and  Semester='" + ds.Tables[0].Rows[i]["Semester"].ToString() + " ' and Batch='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                            SqlDataAdapter sda = new SqlDataAdapter(ss, con);
    //                            DataSet sds = new DataSet();
    //                            sda.Fill(sds);
    //                            if (sds.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it < sds.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex.CommandType = CommandType.StoredProcedure;
    //                                    cdex.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex = new SqlDataAdapter(cdex);
    //                                    DataSet dsex = new DataSet();
    //                                    daex.Fill(dsex);
    //                                    if (dsex.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt = ds.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }

    //                                                    examdate = dsex.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt = ds.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd.CommandType = CommandType.StoredProcedure;
    //                                        cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                        cd.Parameters.AddWithValue("@Duration", RTheoryDuration);
    //                                        cd.Parameters.AddWithValue("@StartTime", RTheoryStartTime);
    //                                        cd.Parameters.AddWithValue("@EndTime", RTheoryEndTime);
    //                                        cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd.ExecuteNonQuery();
    //                                        i = i + 1;
    //                                        n = n + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        existsubject = 1;
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt = ds.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd.CommandType = CommandType.StoredProcedure;
    //                                        cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                        cd.Parameters.AddWithValue("@Duration", RTheoryDuration);
    //                                        cd.Parameters.AddWithValue("@StartTime", RTheoryStartTime);
    //                                        cd.Parameters.AddWithValue("@EndTime", RTheoryEndTime);
    //                                        cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd.ExecuteNonQuery();
    //                                        i = i + 1;

    //                                    }
    //                                }
    //                                else
    //                                {

    //                                    i = i + 1;

    //                                    emptysubject = emptysubject + 1;
    //                                    if (emptysubject == ds.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt = ds.Tables[0].Rows.Count;
    //                                        goto dd;

    //                                    }
    //                                }
    //                            }

    //                            //if (n == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n = 0;

    //                            //}
    //                        }
    //                    }

    //                    it = it + 1;
    //                    goto l;

    //                }

    //                days = days + 1;
    //                string degree3;
    //                int n3 = 0;
    //                int cnt3 = 0;
    //                degree3 = "select  degree,Batch,Semester  from  ExamTimeTableDummyArearPracTable ";
    //                SqlDataAdapter da3 = new SqlDataAdapter(degree3, con);
    //                DataSet ds3 = new DataSet();
    //                da3.Fill(ds3);
    //            dd3: if (ds3.Tables[0].Rows.Count > cnt3)
    //                {
    //                    string ss3;
    //                    int it3 = 0;

    //                l3: int i3 = 0;
    //                    int emptysubject3 = 0;
    //                    while (i3 < ds3.Tables[0].Rows.Count)
    //                    {

    //                        string ssr3;
    //                        ssr3 = "select exam_code,exam_type from exmtt where Degree_Code='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and batchTo='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd3 = new SqlDataAdapter(ssr3, con);
    //                        DataSet sdss3 = new DataSet();
    //                        ssd3.Fill(sdss3);
    //                        if (sdss3.Tables[0].Rows.Count > 0)
    //                        {
    //                            ss3 = "select * from ExamTimeTableDummyArearPracTable  where Degree='" + ds3.Tables[0].Rows[i3]["Degree"].ToString() + "' and Batch='" + ds3.Tables[0].Rows[i3]["Batch"].ToString() + "'";
    //                            SqlDataAdapter sda3 = new SqlDataAdapter(ss3, con);
    //                            DataSet sds3 = new DataSet();
    //                            sda3.Fill(sds3);
    //                            if (sds3.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it3 < sds3.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex3 = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex3.CommandType = CommandType.StoredProcedure;
    //                                    cdex3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex3 = new SqlDataAdapter(cdex3);
    //                                    DataSet dsex3 = new DataSet();
    //                                    daex3.Fill(dsex3);
    //                                    if (dsex3.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex3.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex3.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex3.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt3 = ds3.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex3.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex3.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt3 = ds3.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd3.CommandType = CommandType.StoredProcedure;
    //                                        cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                        cd3.Parameters.AddWithValue("@Duration", APracticalDuration);
    //                                        cd3.Parameters.AddWithValue("@StartTime", APracticalStartTime);
    //                                        cd3.Parameters.AddWithValue("@EndTime", APracticalEndTime);
    //                                        cd3.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd3.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd3.ExecuteNonQuery();
    //                                        i3 = i3 + 1;
    //                                        n3 = n3 + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        existsubject = 1;
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt3 = ds3.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        SqlCommand cd3 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd3.CommandType = CommandType.StoredProcedure;
    //                                        cd3.Parameters.AddWithValue("@SubjectNo", sds3.Tables[0].Rows[it3]["SubjectNo"].ToString());
    //                                        cd3.Parameters.AddWithValue("@Duration", APracticalDuration);
    //                                        cd3.Parameters.AddWithValue("@StartTime", APracticalStartTime);
    //                                        cd3.Parameters.AddWithValue("@EndTime", APracticalEndTime);
    //                                        cd3.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd3.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd3.Parameters.AddWithValue("@ExamCode", sdss3.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExamType", sdss3.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd3.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd3.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd3.ExecuteNonQuery();
    //                                        i3 = i3 + 1;

    //                                    }
    //                                }
    //                                else
    //                                {

    //                                    i3 = i3 + 1;

    //                                    emptysubject3 = emptysubject3 + 1;
    //                                    if (emptysubject3 == ds3.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt3 = ds3.Tables[0].Rows.Count;
    //                                        goto dd3;

    //                                    }
    //                                }
    //                            }

    //                            //if (n == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n = 0;

    //                            //}
    //                        }
    //                    }

    //                    it3 = it3 + 1;
    //                    goto l3;

    //                }

    //                days = days + 1;
    //                string degree2;
    //                int n2 = 0;
    //                int cnt2 = 0;
    //                degree2 = "select degree,Batch,Semester  from  ExamTimeTableDummyArearTable ";
    //                SqlDataAdapter da2 = new SqlDataAdapter(degree2, con);
    //                DataSet ds2 = new DataSet();
    //                da2.Fill(ds2);
    //            dd2: if (ds2.Tables[0].Rows.Count > cnt2)
    //                {
    //                    string ss2;
    //                    int it2 = 0;

    //                l2: int i2 = 0;
    //                    int emptysubject2 = 0;
    //                    while (i2 < ds2.Tables[0].Rows.Count)
    //                    {

    //                        string ssr2;
    //                        ssr2 = "select exam_code,exam_type from exmtt where Degree_Code='" + ds2.Tables[0].Rows[i2]["Degree"].ToString() + "' and batchTo='" + ds2.Tables[0].Rows[i2]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd2 = new SqlDataAdapter(ssr2, con);
    //                        DataSet sdss2 = new DataSet();
    //                        ssd2.Fill(sdss2);
    //                        if (sdss2.Tables[0].Rows.Count > 0)
    //                        {
    //                            ss2 = "select * from ExamTimeTableDummyArearTable  where Degree='" + ds2.Tables[0].Rows[i2]["Degree"].ToString() + "' Batch='" + ds2.Tables[0].Rows[i2]["Batch"].ToString() + "'";
    //                            SqlDataAdapter sda2 = new SqlDataAdapter(ss2, con);
    //                            DataSet sds2 = new DataSet();
    //                            sda2.Fill(sds2);
    //                            if (sds2.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it2 < sds2.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex2 = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex2.CommandType = CommandType.StoredProcedure;
    //                                    cdex2.Parameters.AddWithValue("@ExamCode", sdss2.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex2.Parameters.AddWithValue("@SubjectNo", sds2.Tables[0].Rows[it2]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex2 = new SqlDataAdapter(cdex2);
    //                                    DataSet dsex2 = new DataSet();
    //                                    daex2.Fill(dsex2);
    //                                    if (dsex2.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 0;
    //                                        if (dsex2.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex2.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex2.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt2 = ds2.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }
    //                                                    examdate = dsex2.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex2.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt2 = ds2.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd2 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd2.CommandType = CommandType.StoredProcedure;
    //                                        cd2.Parameters.AddWithValue("@SubjectNo", sds2.Tables[0].Rows[it2]["SubjectNo"].ToString());
    //                                        cd2.Parameters.AddWithValue("@Duration", ATheoryDuration);
    //                                        cd2.Parameters.AddWithValue("@StartTime", ATheoryStartTime);
    //                                        cd2.Parameters.AddWithValue("@EndTime", ATheoryEndTime);
    //                                        cd2.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd2.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd2.Parameters.AddWithValue("@ExamCode", sdss2.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd2.Parameters.AddWithValue("@ExamType", sdss2.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd2.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd2.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd2.ExecuteNonQuery();
    //                                        i2 = i2 + 1;
    //                                        n2 = n2 + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        existsubject = 1;
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt2 = ds2.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }
    //                                        SqlCommand cd2 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd2.CommandType = CommandType.StoredProcedure;
    //                                        cd2.Parameters.AddWithValue("@SubjectNo", sds2.Tables[0].Rows[it2]["SubjectNo"].ToString());
    //                                        cd2.Parameters.AddWithValue("@Duration", ATheoryDuration);
    //                                        cd2.Parameters.AddWithValue("@StartTime", ATheoryStartTime);
    //                                        cd2.Parameters.AddWithValue("@EndTime", ATheoryEndTime);
    //                                        cd2.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd2.Parameters.AddWithValue("@Session", AExamSession);
    //                                        cd2.Parameters.AddWithValue("@ExamCode", sdss2.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd2.Parameters.AddWithValue("@ExamType", sdss2.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd2.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd2.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd2.ExecuteNonQuery();
    //                                        i2 = i2 + 1;
    //                                    }
    //                                }

    //                                else
    //                                {

    //                                    i2 = i2 + 1;

    //                                    emptysubject2 = emptysubject2 + 1;
    //                                    if (emptysubject2 == ds2.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt2 = ds2.Tables[0].Rows.Count;
    //                                        goto dd2;

    //                                    }
    //                                }
    //                            }

    //                            //if (n == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n = 0;

    //                            //}
    //                        }
    //                    }

    //                    it2 = it2 + 1;
    //                    goto l2;

    //                }


    //                days = days + 1;
    //                string degree1;
    //                int n1 = 0;
    //                int cnt1 = 0;
    //                degree1 = "select distinct degree,Batch ,Semester from  ExamTimeTableDummyPracTable ";
    //                SqlDataAdapter da1 = new SqlDataAdapter(degree1, con);
    //                DataSet ds1 = new DataSet();
    //                da1.Fill(ds1);
    //            dd1: if (ds1.Tables[0].Rows.Count > cnt1)
    //                {
    //                    string ss1;
    //                    int it1 = 0;

    //                l1: int i1 = 0;
    //                    int emptysubject1 = 0;
    //                    while (i1 < ds1.Tables[0].Rows.Count)
    //                    {

    //                        string ssr1;
    //                        ssr1 = "select exam_code,exam_type from exmtt where Degree_Code='" + ds1.Tables[0].Rows[i1]["Degree"].ToString() + "' and batchTo='" + ds1.Tables[0].Rows[i1]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                        SqlDataAdapter ssd1 = new SqlDataAdapter(ssr1, con);
    //                        DataSet sdss1 = new DataSet();
    //                        ssd1.Fill(sdss1);
    //                        if (sdss1.Tables[0].Rows.Count > 0)
    //                        {
    //                            ss1 = "select * from ExamTimeTableDummyPracTable  where Degree='" + ds1.Tables[0].Rows[i1]["Degree"].ToString() + "' and  Semester='" + ds1.Tables[0].Rows[i1]["Semester"].ToString() + " ' and Batch='" + ds1.Tables[0].Rows[i1]["Batch"].ToString() + "'";
    //                            SqlDataAdapter sda1 = new SqlDataAdapter(ss1, con);
    //                            DataSet sds1 = new DataSet();
    //                            sda1.Fill(sds1);
    //                            if (sds1.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (it1 < sds1.Tables[0].Rows.Count)
    //                                {
    //                                    SqlCommand cdex1 = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                    cdex1.CommandType = CommandType.StoredProcedure;
    //                                    cdex1.Parameters.AddWithValue("@ExamCode", sdss1.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cdex1.Parameters.AddWithValue("@SubjectNo", sds1.Tables[0].Rows[it1]["SubjectNo"].ToString());
    //                                    SqlDataAdapter daex1 = new SqlDataAdapter(cdex1);
    //                                    DataSet dsex1 = new DataSet();
    //                                    daex1.Fill(dsex1);
    //                                    if (dsex1.Tables[1].Rows.Count == 0)
    //                                    {
    //                                        existsubject = 1;
    //                                        if (dsex1.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if (dsex1.Tables[0].Rows.Count > 1)
    //                                            {
    //                                                for (int dd = 0; dd < dsex1.Tables[0].Rows.Count; dd++)
    //                                                {
    //                                                    if (workcount <= days)
    //                                                    {

    //                                                        lblerror.Visible = true;
    //                                                        lblerror.Text = "Extend the Working Days";
    //                                                        cnt1 = ds1.Tables[0].Rows.Count;
    //                                                        goto Err;

    //                                                    }

    //                                                    examdate = dsex1.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                    DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                    DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                    TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                    if (ts1.Days == 0)
    //                                                    {
    //                                                        days = days + 1;

    //                                                    }

    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                examdate = dsex1.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0)
    //                                                {
    //                                                    days = days + 1;

    //                                                }
    //                                            }


    //                                        }
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt1 = ds1.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd1 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd1.CommandType = CommandType.StoredProcedure;
    //                                        cd1.Parameters.AddWithValue("@SubjectNo", sds1.Tables[0].Rows[it1]["SubjectNo"].ToString());
    //                                        cd1.Parameters.AddWithValue("@Duration", RPracticalDuration);
    //                                        cd1.Parameters.AddWithValue("@StartTime", RPracticalStartTime);
    //                                        cd1.Parameters.AddWithValue("@EndTime", RPracticalEndTime);
    //                                        cd1.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd1.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd1.Parameters.AddWithValue("@ExamCode", sdss1.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd1.Parameters.AddWithValue("@ExamType", sdss1.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd1.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd1.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd1.ExecuteNonQuery();
    //                                        i1 = i1 + 1;
    //                                        n1 = n1 + 1;
    //                                    }
    //                                    else
    //                                    {
    //                                        existsubject = 1;
    //                                        if (workcount <= days)
    //                                        {

    //                                            lblerror.Visible = true;
    //                                            lblerror.Text = "Extend the Working Days";
    //                                            cnt1 = ds1.Tables[0].Rows.Count;
    //                                            goto Err;

    //                                        }

    //                                        SqlCommand cd1 = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                        cd1.CommandType = CommandType.StoredProcedure;
    //                                        cd1.Parameters.AddWithValue("@SubjectNo", sds1.Tables[0].Rows[it1]["SubjectNo"].ToString());
    //                                        cd1.Parameters.AddWithValue("@Duration", RPracticalDuration);
    //                                        cd1.Parameters.AddWithValue("@StartTime", RPracticalStartTime);
    //                                        cd1.Parameters.AddWithValue("@EndTime", RPracticalEndTime);
    //                                        cd1.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                        cd1.Parameters.AddWithValue("@Session", RExamSession);
    //                                        cd1.Parameters.AddWithValue("@ExamCode", sdss1.Tables[0].Rows[0]["exam_code"].ToString());
    //                                        cd1.Parameters.AddWithValue("@ExamType", sdss1.Tables[0].Rows[0]["exam_type"].ToString());
    //                                        cd1.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                        cd1.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                        cd1.ExecuteNonQuery();
    //                                        i1 = i1 + 1;
    //                                    }
    //                                }
    //                                else
    //                                {

    //                                    i1 = i1 + 1;

    //                                    emptysubject1 = emptysubject1 + 1;
    //                                    if (emptysubject1 == ds1.Tables[0].Rows.Count)
    //                                    {
    //                                        cnt1 = ds1.Tables[0].Rows.Count;
    //                                        goto Err;

    //                                    }
    //                                }
    //                            }

    //                            //if (n == Batches)
    //                            //{

    //                            //    days = days + 1;
    //                            //    n = 0;

    //                            //}
    //                        }
    //                    }

    //                    it1 = it1 + 1;
    //                    goto l1;

    //                }



    //            }

    //        }
    //        else if (clSession.SelectedItem.Text.ToString() == "Mixed")
    //        {

    //            string msession1 = "F.N";
    //            string msession2 = "F.N";
    //            string msession3 = "A.N";
    //            string msession4 = "A.N";

    //            string t1 = "";
    //            string t2 = "";
    //            string t3 = "";
    //            string t4 = "";
    //            if (ddlExamstartswith.SelectedItem.Text.ToString() == "Regular")
    //            {
    //                t1 = "ExamTimeTableDummyTable";
    //                t2 = "ExamTimeTableDummyPracTable";
    //                t3 = "ExamTimeTableDummyArearTable";
    //                t4 = "ExamTimeTableDummyArearPracTable";
    //            }
    //            else if (ddlExamstartswith.SelectedItem.Text.ToString() == "Arear")
    //            {

    //                t1 = "ExamTimeTableDummyArearTable";
    //                t2 = "ExamTimeTableDummyArearPracTable";
    //                t3 = "ExamTimeTableDummyTable";
    //                t4 = "ExamTimeTableDummyPracTable";
    //            }
    //            else if (ddlExamstartswith.SelectedItem.Text.ToString() == "Mixed")
    //            {
    //                t1 = "ExamTimeTableDummyTable";
    //                t3 = "ExamTimeTableDummyArearTable";
    //                t2 = "ExamTimeTableDummyPracTable";
    //                t4 = "ExamTimeTableDummyArearPracTable";
    //            }

    //            int day = 0;
    //            int n = 0;
    //            int cnt = 0;
    //            days = 0;
    //            degree = "";
    //            if (t1 == "ExamTimeTableDummyArearTable")
    //            {

    //                degree = "select  degree,Batch,Semester  from   " + t1 + "";
    //            }
    //            else if (t1 == "ExamTimeTableDummyArearPracTable")
    //            {
    //                degree = "select  degree,Batch,Semester  from   " + t1 + "";

    //            }
    //            else
    //            {
    //                degree = "select distinct degree,Batch,Semester  from   " + t1 + "";
    //            }
    //            SqlDataAdapter da = new SqlDataAdapter(degree, con);
    //            DataSet ds = new DataSet();
    //            da.Fill(ds);
    //        dd: if (ds.Tables[0].Rows.Count > cnt)
    //            {
    //                string s = "";
    //                int it = 0;

    //            l: int i = 0;
    //                int emptysubject = 0;
    //                while (i < ds.Tables[0].Rows.Count)
    //                {

    //                    string ssr;
    //                    ssr = "select exam_code,exam_type from exmtt where Degree_Code='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and batchTo='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                    SqlDataAdapter ssd = new SqlDataAdapter(ssr, con);
    //                    DataSet sdss = new DataSet();
    //                    ssd.Fill(sdss);
    //                    if (sdss.Tables[0].Rows.Count > 0)
    //                    {
    //                        if (t1 == "ExamTimeTableDummyArearTable")
    //                        {

    //                            s = "select * from  " + t1 + " where  Degree='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and Batch='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                        }
    //                        else if (t1 == "ExamTimeTableDummyArearPracTable")
    //                        {
    //                            s = "select * from  " + t1 + " where  Degree='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "' and Batch='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "'";

    //                        }
    //                        else
    //                        {
    //                            s = "select * from  " + t1 + " where  Degree='" + ds.Tables[0].Rows[i]["Degree"].ToString() + "'  and  Semester='" + ds.Tables[0].Rows[i]["Semester"].ToString() + " '  and Batch='" + ds.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                        }

    //                        SqlDataAdapter sda = new SqlDataAdapter(s, con);
    //                        DataSet sds = new DataSet();
    //                        sda.Fill(sds);
    //                        if (sds.Tables[0].Rows.Count > 0)
    //                        {
    //                            if (it < sds.Tables[0].Rows.Count)
    //                            {

    //                                SqlCommand cdex = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                cdex.CommandType = CommandType.StoredProcedure;
    //                                cdex.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                cdex.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                SqlDataAdapter daex = new SqlDataAdapter(cdex);
    //                                DataSet dsex = new DataSet();
    //                                daex.Fill(dsex);
    //                                if (dsex.Tables[1].Rows.Count == 0)
    //                                {
    //                                    existsubject = 0;
    //                                    if (dsex.Tables[0].Rows.Count > 0)
    //                                    {
    //                                        if (dsex.Tables[0].Rows.Count > 1)
    //                                        {
    //                                            for (int dd = 0; dd < dsex.Tables[0].Rows.Count; dd++)
    //                                            {
    //                                                if (workcount <= days)
    //                                                {

    //                                                    lblerror.Visible = true;
    //                                                    lblerror.Text = "Extend the Working Days";
    //                                                    cnt = ds.Tables[0].Rows.Count;
    //                                                    goto dd;

    //                                                }
    //                                                examdate = dsex.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0 && msession1 == dsex.Tables[0].Rows[dd]["ExamSession"].ToString())
    //                                                {
    //                                                    days = days + 1;

    //                                                }

    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            examdate = dsex.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                            DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                            DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                            TimeSpan ts1 = exammdate.Subtract(workday);
    //                                            if (ts1.Days == 0 && msession1 == dsex.Tables[0].Rows[0]["ExamSession"].ToString())
    //                                            {
    //                                                days = days + 1;

    //                                            }
    //                                        }


    //                                    }
    //                                    if (workcount <= days)
    //                                    {

    //                                        lblerror.Visible = true;
    //                                        lblerror.Text = "Extend the Working Days";
    //                                        cnt = ds.Tables[0].Rows.Count;
    //                                        goto dd;

    //                                    }
    //                                    SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                    cd.CommandType = CommandType.StoredProcedure;
    //                                    cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                    cd.Parameters.AddWithValue("@Duration", txtTheoryDurationam.Text.ToString());
    //                                    cd.Parameters.AddWithValue("@StartTime", TheoryStarttimeAm);
    //                                    cd.Parameters.AddWithValue("@EndTime", TheoryendttimeAm);
    //                                    cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                    cd.Parameters.AddWithValue("@Session", msession1);
    //                                    cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                    cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                    cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                    cd.ExecuteNonQuery();
    //                                    n = n + 1;
    //                                    i = i + 1;
    //                                }
    //                                else
    //                                {
    //                                    existsubject = 1;
    //                                    if (workcount <= days)
    //                                    {

    //                                        lblerror.Visible = true;
    //                                        lblerror.Text = "Extend the Working Days";
    //                                        cnt = ds.Tables[0].Rows.Count;
    //                                        goto dd;

    //                                    }
    //                                    SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                    cd.CommandType = CommandType.StoredProcedure;
    //                                    cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                    cd.Parameters.AddWithValue("@Duration", txtTheoryDurationam.Text.ToString());
    //                                    cd.Parameters.AddWithValue("@StartTime", TheoryStarttimeAm);
    //                                    cd.Parameters.AddWithValue("@EndTime", TheoryendttimeAm);
    //                                    cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                    cd.Parameters.AddWithValue("@Session", msession1);
    //                                    cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                    cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                    cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                    cd.ExecuteNonQuery();
    //                                    i = i + 1;
    //                                    n = n + 1;
    //                                }
    //                            }
    //                            else
    //                            {

    //                                i = i + 1;

    //                                emptysubject = emptysubject + 1;
    //                                if (emptysubject == ds.Tables[0].Rows.Count)
    //                                {
    //                                    cnt = ds.Tables[0].Rows.Count;
    //                                    goto dd;

    //                                }
    //                            }
    //                        }
    //                        //if (n == Batches)
    //                        //{

    //                        //    day = day + 1;
    //                        //    n = 0;

    //                        //}


    //                    }
    //                }
    //                it = it + 1;
    //                goto l;

    //            }



    //            days = days + 1;
    //            int cnt2 = 0;
    //            int n2 = 0;
    //            string sdegree = "";
    //            if (t2 == "ExamTimeTableDummyArearTable")
    //            {

    //                sdegree = "select  degree,Batch,Semester  from   " + t2 + "";
    //            }
    //            else if (t2 == "ExamTimeTableDummyArearPracTable")
    //            {
    //                sdegree = "select  degree,Batch,Semester  from   " + t2 + "";

    //            }
    //            else
    //            {
    //                sdegree = "select distinct degree,Batch,Semester  from   " + t2 + "";
    //            }

    //            SqlDataAdapter das = new SqlDataAdapter(sdegree, con);
    //            DataSet dss = new DataSet();
    //            das.Fill(dss);
    //        dd2: if (dss.Tables[0].Rows.Count > cnt2)
    //            {

    //                string s;
    //                int it = 0;

    //            l: int i = 0;
    //                int emptysubject = 0;
    //                while (i < dss.Tables[0].Rows.Count)
    //                {


    //                    string ssr;
    //                    ssr = "select exam_code,exam_type from exmtt where Degree_Code='" + dss.Tables[0].Rows[i]["Degree"].ToString() + "' and batchTo='" + dss.Tables[0].Rows[i]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                    SqlDataAdapter ssd = new SqlDataAdapter(ssr, con);
    //                    DataSet sdss = new DataSet();
    //                    ssd.Fill(sdss);
    //                    if (sdss.Tables[0].Rows.Count > 0)
    //                    {
    //                        if (t2 == "ExamTimeTableDummyArearTable")
    //                        {

    //                            s = "select * from  " + t2 + " where  Degree='" + dss.Tables[0].Rows[i]["Degree"].ToString() + "' and Batch='" + dss.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                        }
    //                        else if (t2 == "ExamTimeTableDummyArearPracTable")
    //                        {
    //                            s = "select * from  " + t2 + " where  Degree='" + dss.Tables[0].Rows[i]["Degree"].ToString() + "' and Batch='" + dss.Tables[0].Rows[i]["Batch"].ToString() + "'";

    //                        }
    //                        else
    //                        {
    //                            s = "select * from  " + t2 + " where  Degree='" + dss.Tables[0].Rows[i]["Degree"].ToString() + "'  and  Semester='" + dss.Tables[0].Rows[i]["Semester"].ToString() + " '  and Batch='" + dss.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                        }

    //                        SqlDataAdapter sda = new SqlDataAdapter(s, con);
    //                        DataSet sds = new DataSet();
    //                        sda.Fill(sds);
    //                        if (sds.Tables[0].Rows.Count > 0)
    //                        {
    //                            if (it < sds.Tables[0].Rows.Count)
    //                            {
    //                                SqlCommand cdex = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                cdex.CommandType = CommandType.StoredProcedure;
    //                                cdex.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                cdex.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                SqlDataAdapter daex = new SqlDataAdapter(cdex);
    //                                DataSet dsex = new DataSet();
    //                                daex.Fill(dsex);
    //                                if (dsex.Tables[1].Rows.Count == 0)
    //                                {
    //                                    existsubject = 0;
    //                                    if (dsex.Tables[0].Rows.Count > 0)
    //                                    {
    //                                        if (dsex.Tables[0].Rows.Count > 1)
    //                                        {
    //                                            for (int dd = 0; dd < dsex.Tables[0].Rows.Count; dd++)
    //                                            {
    //                                                if (workcount <= days)
    //                                                {

    //                                                    lblerror.Visible = true;
    //                                                    lblerror.Text = "Extend the Working Days";
    //                                                    cnt2 = dss.Tables[0].Rows.Count;
    //                                                    goto dd2;

    //                                                }
    //                                                examdate = dsex.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0 && msession2 == dsex.Tables[0].Rows[dd]["ExamSession"].ToString())
    //                                                {
    //                                                    days = days + 1;

    //                                                }

    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            examdate = dsex.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                            DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                            DateTime workday = Convert.ToDateTime(workdays[days].ToString());
    //                                            TimeSpan ts1 = exammdate.Subtract(workday);
    //                                            if (ts1.Days == 0 && msession2 == dsex.Tables[0].Rows[0]["ExamSession"].ToString())
    //                                            {
    //                                                days = days + 1;

    //                                            }
    //                                        }


    //                                    }
    //                                    if (workcount <= days)
    //                                    {

    //                                        lblerror.Visible = true;
    //                                        lblerror.Text = "Extend the Working Days";
    //                                        cnt2 = dss.Tables[0].Rows.Count;
    //                                        goto dd2;

    //                                    }
    //                                    SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                    cd.CommandType = CommandType.StoredProcedure;
    //                                    cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                    cd.Parameters.AddWithValue("@Duration", txtpracdurationam.Text.ToString());
    //                                    cd.Parameters.AddWithValue("@StartTime", PracStarttimeAm);
    //                                    cd.Parameters.AddWithValue("@EndTime", pracendttimeAm);
    //                                    cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                    cd.Parameters.AddWithValue("@Session", msession2);
    //                                    cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                    cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                    cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                    cd.ExecuteNonQuery();
    //                                    i = i + 1;
    //                                    n2 = n2 + 1;

    //                                }
    //                                else
    //                                {
    //                                    existsubject = 1;
    //                                    if (workcount <= days)
    //                                    {

    //                                        lblerror.Visible = true;
    //                                        lblerror.Text = "Extend the Working Days";
    //                                        cnt2 = dss.Tables[0].Rows.Count;
    //                                        goto dd2;

    //                                    }
    //                                    SqlCommand cd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                    cd.CommandType = CommandType.StoredProcedure;
    //                                    cd.Parameters.AddWithValue("@SubjectNo", sds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                    cd.Parameters.AddWithValue("@Duration", txtpracdurationam.Text.ToString());
    //                                    cd.Parameters.AddWithValue("@StartTime", PracStarttimeAm);
    //                                    cd.Parameters.AddWithValue("@EndTime", pracendttimeAm);
    //                                    cd.Parameters.AddWithValue("@ExamDate", workdays[days].ToString());
    //                                    cd.Parameters.AddWithValue("@Session", msession2);
    //                                    cd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    cd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                    cd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                    cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                    cd.ExecuteNonQuery();
    //                                    i = i + 1;
    //                                }
    //                            }
    //                            else
    //                            {
    //                                i = i + 1;

    //                                emptysubject = emptysubject + 1;
    //                                if (emptysubject == dss.Tables[0].Rows.Count)
    //                                {
    //                                    cnt2 = dss.Tables[0].Rows.Count;
    //                                    goto dd2;

    //                                }
    //                            }

    //                        }
    //                        //if (n2 == Batches)
    //                        //{

    //                        //    day = day + 1;
    //                        //    n2 = 0;

    //                        //}

    //                    }

    //                }
    //                it = it + 1;
    //                goto l;



    //            }

    //            days1 = days1 + 1;
    //            string Adegree = "";
    //            int n1 = 0;
    //            int cnt3 = 0;
    //            if (t3 == "ExamTimeTableDummyArearTable")
    //            {
    //                Adegree = "select degree,Batch,Semester  from " + t3 + " ";
    //            }
    //            else if (t3 == "ExamTimeTableDummyArearPracTable")
    //            {

    //                Adegree = "select  degree,Batch,Semester  from " + t3 + " ";

    //            }
    //            else
    //            {
    //                Adegree = "select distinct degree,Batch,Semester  from " + t3 + " ";
    //            }

    //            SqlDataAdapter Ada = new SqlDataAdapter(Adegree, con);
    //            DataSet Ads = new DataSet();
    //            Ada.Fill(Ads);
    //        dd3: if (Ads.Tables[0].Rows.Count > cnt3)
    //            {

    //                int it = 0;

    //            Al: int i = 0;
    //                int emptysubject = 0;
    //                while (i < Ads.Tables[0].Rows.Count)
    //                {
    //                    string ssr;
    //                    ssr = "select exam_code,exam_type from exmtt where Degree_Code='" + Ads.Tables[0].Rows[i]["Degree"].ToString() + "' and batchTo='" + Ads.Tables[0].Rows[i]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                    SqlDataAdapter ssd = new SqlDataAdapter(ssr, con);
    //                    DataSet sdss = new DataSet();
    //                    ssd.Fill(sdss);
    //                    if (sdss.Tables[0].Rows.Count > 0)
    //                    {
    //                        string As = "";
    //                        if (t3 == "ExamTimeTableDummyArearTable")
    //                        {
    //                            As = "select * from " + t3 + " where  Degree='" + Ads.Tables[0].Rows[i]["Degree"].ToString() + "' and Batch='" + Ads.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                        }
    //                        else if (t3 == "ExamTimeTableDummyArearPracTable")
    //                        {

    //                            As = "select * from " + t3 + " where  Degree='" + Ads.Tables[0].Rows[i]["Degree"].ToString() + "' and Batch='" + Ads.Tables[0].Rows[i]["Batch"].ToString() + "'";

    //                        }
    //                        else
    //                        {
    //                            As = "select * from " + t3 + " where  Degree='" + Ads.Tables[0].Rows[i]["Degree"].ToString() + "'  and  Semester='" + Ads.Tables[0].Rows[i]["Semester"].ToString() + " ' and Batch='" + Ads.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                        }


    //                        SqlDataAdapter Asda = new SqlDataAdapter(As, con);
    //                        DataSet Asds = new DataSet();
    //                        Asda.Fill(Asds);
    //                        if (Asds.Tables[0].Rows.Count > 0)
    //                        {
    //                            if (it < Asds.Tables[0].Rows.Count)
    //                            {
    //                                SqlCommand cdex = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                cdex.CommandType = CommandType.StoredProcedure;
    //                                cdex.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                cdex.Parameters.AddWithValue("@SubjectNo", Asds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                SqlDataAdapter daex = new SqlDataAdapter(cdex);
    //                                DataSet dsex = new DataSet();
    //                                daex.Fill(dsex);
    //                                if (dsex.Tables[1].Rows.Count == 0)
    //                                {
    //                                    existsubject = 0;
    //                                    if (dsex.Tables[0].Rows.Count > 0)
    //                                    {
    //                                        if (dsex.Tables[0].Rows.Count > 1)
    //                                        {
    //                                            for (int dd = 0; dd < dsex.Tables[0].Rows.Count; dd++)
    //                                            {
    //                                                if (workcount <= days1)
    //                                                {

    //                                                    lblerror.Visible = true;
    //                                                    lblerror.Text = "Extend the Working Days";
    //                                                    cnt3 = Ads.Tables[0].Rows.Count;
    //                                                    goto dd3;

    //                                                }
    //                                                examdate = dsex.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days1].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0 && msession3 == dsex.Tables[0].Rows[dd]["ExamSession"].ToString())
    //                                                {
    //                                                    days1 = days1 + 1;

    //                                                }

    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            examdate = dsex.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                            DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                            DateTime workday = Convert.ToDateTime(workdays[days1].ToString());
    //                                            TimeSpan ts1 = exammdate.Subtract(workday);
    //                                            if (ts1.Days == 0 && msession3 == dsex.Tables[0].Rows[0]["ExamSession"].ToString())
    //                                            {
    //                                                days1 = days1 + 1;

    //                                            }
    //                                        }


    //                                    }
    //                                    if (workcount <= days1)
    //                                    {

    //                                        lblerror.Visible = true;
    //                                        lblerror.Text = "Extend the Working Days";
    //                                        cnt3 = Ads.Tables[0].Rows.Count;
    //                                        goto dd3;

    //                                    }
    //                                    SqlCommand Acd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                    Acd.CommandType = CommandType.StoredProcedure;
    //                                    Acd.Parameters.AddWithValue("@SubjectNo", Asds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                    Acd.Parameters.AddWithValue("@Duration", txtTheoryDurationpm.Text.ToString());
    //                                    Acd.Parameters.AddWithValue("@StartTime", TheoryStarttimePm);
    //                                    Acd.Parameters.AddWithValue("@EndTime", TheoryendttimePm);
    //                                    Acd.Parameters.AddWithValue("@ExamDate", workdays[days1].ToString());
    //                                    Acd.Parameters.AddWithValue("@Session", msession3);
    //                                    Acd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    Acd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                    Acd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                    Acd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                    Acd.ExecuteNonQuery();
    //                                    i = i + 1;
    //                                    n1 = n1 + 1;
    //                                }
    //                                else
    //                                {
    //                                    existsubject = 1;
    //                                    if (workcount <= days1)
    //                                    {

    //                                        lblerror.Visible = true;
    //                                        lblerror.Text = "Extend the Working Days";
    //                                        cnt3 = Ads.Tables[0].Rows.Count;
    //                                        goto dd3;

    //                                    }
    //                                    SqlCommand Acd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                    Acd.CommandType = CommandType.StoredProcedure;
    //                                    Acd.Parameters.AddWithValue("@SubjectNo", Asds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                    Acd.Parameters.AddWithValue("@Duration", txtTheoryDurationpm.Text.ToString());
    //                                    Acd.Parameters.AddWithValue("@StartTime", TheoryStarttimePm);
    //                                    Acd.Parameters.AddWithValue("@EndTime", TheoryendttimePm);
    //                                    Acd.Parameters.AddWithValue("@ExamDate", workdays[days1].ToString());
    //                                    Acd.Parameters.AddWithValue("@Session", msession3);
    //                                    Acd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    Acd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                    Acd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                    Acd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                    Acd.ExecuteNonQuery();
    //                                    i = i + 1;

    //                                }
    //                            }
    //                            else
    //                            {
    //                                i = i + 1;


    //                                emptysubject = emptysubject + 1;
    //                                if (emptysubject == Ads.Tables[0].Rows.Count)
    //                                {
    //                                    cnt3 = Ads.Tables[0].Rows.Count;
    //                                    goto dd3;

    //                                }
    //                            }
    //                        }
    //                        //if (n1 == Batches)
    //                        //{

    //                        //    days1 = days1 + 1;
    //                        //    n1 = 0;

    //                        //}

    //                    }
    //                }

    //                it = it + 1;
    //                goto Al;

    //            }


    //            days1 = days1 + 1;
    //            int cnt4 = 0;
    //            n1 = 0;
    //            string TAdegree = "";
    //            if (t4 == "ExamTimeTableDummyArearTable")
    //            {
    //                TAdegree = "select  degree,Batch,Semester from " + t4 + " ";
    //            }
    //            else if (t4 == "ExamTimeTableDummyArearpracTable")
    //            {
    //                TAdegree = "select  degree,Batch,Semester from " + t4 + " ";
    //            }
    //            else
    //            {

    //                TAdegree = "select distinct degree,Batch,Semester from " + t4 + " ";
    //            }

    //            SqlDataAdapter TAda = new SqlDataAdapter(TAdegree, con);
    //            DataSet TAds = new DataSet();
    //            TAda.Fill(TAds);
    //        dd4: if (TAds.Tables[0].Rows.Count > cnt4)
    //            {

    //                int it = 0;

    //            TAl: int i = 0;
    //                int emptysubject = 0;
    //                while (i < TAds.Tables[0].Rows.Count)
    //                {
    //                    string ssr;
    //                    ssr = "select exam_code,exam_type from exmtt where Degree_Code='" + TAds.Tables[0].Rows[i]["Degree"].ToString() + "' and batchTo='" + TAds.Tables[0].Rows[i]["Batch"].ToString() + "' and Exam_Month='" + ddlMonth.SelectedIndex.ToString() + "' and Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
    //                    SqlDataAdapter ssd = new SqlDataAdapter(ssr, con);
    //                    DataSet sdss = new DataSet();
    //                    ssd.Fill(sdss);
    //                    if (sdss.Tables[0].Rows.Count > 0)
    //                    {
    //                        string TAs = "";
    //                        if (t4 == "ExamTimeTableDummyArearTable")
    //                        {
    //                            TAs = "select * from " + t4 + " where  Degree='" + TAds.Tables[0].Rows[i]["Degree"].ToString() + "' and Batch='" + TAds.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                        }
    //                        else if (t4 == "ExamTimeTableDummyArearPracTable")
    //                        {
    //                            TAs = "select * from " + t4 + " where  Degree='" + TAds.Tables[0].Rows[i]["Degree"].ToString() + "' and Batch='" + TAds.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                        }
    //                        else
    //                        {

    //                            TAs = "select * from " + t4 + " where  Degree='" + TAds.Tables[0].Rows[i]["Degree"].ToString() + "'  and  Semester='" + TAds.Tables[0].Rows[i]["Semester"].ToString() + " ' and Batch='" + TAds.Tables[0].Rows[i]["Batch"].ToString() + "'";
    //                        }

    //                        SqlDataAdapter TAsda = new SqlDataAdapter(TAs, con);
    //                        DataSet TAsds = new DataSet();
    //                        TAsda.Fill(TAsds);
    //                        if (TAsds.Tables[0].Rows.Count > 0)
    //                        {
    //                            if (it < TAsds.Tables[0].Rows.Count)
    //                            {
    //                                SqlCommand cdex = new SqlCommand("ProcExamTimeTableDayCheck", con);
    //                                cdex.CommandType = CommandType.StoredProcedure;
    //                                cdex.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                cdex.Parameters.AddWithValue("@SubjectNo", TAsds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                SqlDataAdapter daex = new SqlDataAdapter(cdex);
    //                                DataSet dsex = new DataSet();
    //                                daex.Fill(dsex);
    //                                if (dsex.Tables[1].Rows.Count == 0)
    //                                {
    //                                    existsubject = 0;
    //                                    if (dsex.Tables[0].Rows.Count > 0)
    //                                    {
    //                                        if (dsex.Tables[0].Rows.Count > 1)
    //                                        {
    //                                            for (int dd = 0; dd < dsex.Tables[0].Rows.Count; dd++)
    //                                            {
    //                                                if (workcount <= days1)
    //                                                {

    //                                                    lblerror.Visible = true;
    //                                                    lblerror.Text = "Extend the Working Days";
    //                                                    cnt4 = TAds.Tables[0].Rows.Count;
    //                                                    goto Err;

    //                                                }
    //                                                examdate = dsex.Tables[0].Rows[dd]["ExamDate"].ToString();
    //                                                DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                                DateTime workday = Convert.ToDateTime(workdays[days1].ToString());
    //                                                TimeSpan ts1 = exammdate.Subtract(workday);
    //                                                if (ts1.Days == 0 && msession4 == dsex.Tables[0].Rows[dd]["ExamSession"].ToString())
    //                                                {
    //                                                    days1 = days1 + 1;

    //                                                }

    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            examdate = dsex.Tables[0].Rows[0]["ExamDate"].ToString();
    //                                            DateTime exammdate = Convert.ToDateTime(examdate.ToString());
    //                                            DateTime workday = Convert.ToDateTime(workdays[days1].ToString());
    //                                            TimeSpan ts1 = exammdate.Subtract(workday);
    //                                            if (ts1.Days == 0 && msession4 == dsex.Tables[0].Rows[0]["ExamSession"].ToString())
    //                                            {
    //                                                days1 = days1 + 1;

    //                                            }
    //                                        }


    //                                    }
    //                                    if (workcount <= days1)
    //                                    {

    //                                        lblerror.Visible = true;
    //                                        lblerror.Text = "Extend the Working Days";
    //                                        cnt4 = TAds.Tables[0].Rows.Count;
    //                                        goto Err;

    //                                    }

    //                                    SqlCommand TAcd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                    TAcd.CommandType = CommandType.StoredProcedure;
    //                                    TAcd.Parameters.AddWithValue("@SubjectNo", TAsds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                    TAcd.Parameters.AddWithValue("@Duration", txtpracdurationpm.Text.ToString());
    //                                    TAcd.Parameters.AddWithValue("@StartTime", PracStarttimePm);
    //                                    TAcd.Parameters.AddWithValue("@EndTime", pracendttimePm);
    //                                    TAcd.Parameters.AddWithValue("@ExamDate", workdays[days1].ToString());
    //                                    TAcd.Parameters.AddWithValue("@Session", msession4);
    //                                    TAcd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    TAcd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                    TAcd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                    TAcd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                    TAcd.ExecuteNonQuery();
    //                                    i = i + 1;
    //                                    n1 = n1 + 1;
    //                                }
    //                                else
    //                                {
    //                                    existsubject = 1;
    //                                    if (workcount <= days1)
    //                                    {

    //                                        lblerror.Visible = true;
    //                                        lblerror.Text = "Extend the Working Days";
    //                                        cnt4 = TAds.Tables[0].Rows.Count;
    //                                        goto Err;

    //                                    }
    //                                    SqlCommand TAcd = new SqlCommand("ProcExamTimeTableDetailsSave", con);
    //                                    TAcd.CommandType = CommandType.StoredProcedure;
    //                                    TAcd.Parameters.AddWithValue("@SubjectNo", TAsds.Tables[0].Rows[it]["SubjectNo"].ToString());
    //                                    TAcd.Parameters.AddWithValue("@Duration", txtpracdurationpm.Text.ToString());
    //                                    TAcd.Parameters.AddWithValue("@StartTime", PracStarttimePm);
    //                                    TAcd.Parameters.AddWithValue("@EndTime", pracendttimePm);
    //                                    TAcd.Parameters.AddWithValue("@ExamDate", workdays[days1].ToString());
    //                                    TAcd.Parameters.AddWithValue("@Session", msession4);
    //                                    TAcd.Parameters.AddWithValue("@ExamCode", sdss.Tables[0].Rows[0]["exam_code"].ToString());
    //                                    TAcd.Parameters.AddWithValue("@ExamType", sdss.Tables[0].Rows[0]["exam_type"].ToString());
    //                                    TAcd.Parameters.AddWithValue("@ExistSubject", existsubject);
    //                                    TAcd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
    //                                    TAcd.ExecuteNonQuery();
    //                                    i = i + 1;

    //                                }
    //                            }
    //                            else
    //                            {

    //                                i = i + 1;

    //                                emptysubject = emptysubject + 1;
    //                                if (emptysubject == TAds.Tables[0].Rows.Count)
    //                                {
    //                                    cnt4 = TAds.Tables[0].Rows.Count;
    //                                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Time Table Generated successfully')", true); return;
    //                                    goto Err;

    //                                }
    //                            }

    //                        }
    //                        //if (n1 == Batches)
    //                        //{

    //                        //    days1 = days1 + 1;
    //                        //    n1 = 0;

    //                        //}


    //                    }

    //                }
    //                it = it + 1;
    //                goto TAl;
    //            }




    //        }


    //    }
    //Err:
    //    int ex = 0;

    //}
    # endregion
}