using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using InsproDataAccess;
using System.Configuration;

public partial class Revaluation_MarkEntry : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet dv = new DataSet();
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess strAcc = new InsproStoreAccess();
    Hashtable hat = new Hashtable();
    bool dateflag = false;
    bool gradeflag1 = false;

    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Page.MaintainScrollPositionOnPostBack = true;
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
            usercode = Convert.ToString(Session["usercode"]).Trim();
            collegecode1 = Convert.ToString(Session["collegecode"]).Trim();
            singleuser = Convert.ToString(Session["single_user"]).Trim();
            group_user = Convert.ToString(Session["group_code"]).Trim();

            if (!IsPostBack)
            {
                ddl_mm.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
                ddl_mm.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Jan", "1"));
                ddl_mm.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Feb", "2"));
                ddl_mm.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Mar", "3"));
                ddl_mm.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Apr", "4"));
                ddl_mm.Items.Insert(5, new System.Web.UI.WebControls.ListItem("May", "5"));
                ddl_mm.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jun", "6"));
                ddl_mm.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Jul", "7"));
                ddl_mm.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Aug", "8"));
                ddl_mm.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Sep", "9"));
                ddl_mm.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Oct", "10"));
                ddl_mm.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Nov", "11"));
                ddl_mm.Items.Insert(12, new System.Web.UI.WebControls.ListItem("Dec", "12"));

                // RadioButton1.Checked = true;
                int year1 = Convert.ToInt16(DateTime.Now.ToString("yyyy"));
                ddl_yy.Items.Clear();
                for (int l = 0; l <= 10; l++)
                {
                    ddl_yy.Items.Add(Convert.ToString(year1 - l));
                }
                ddl_yy.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
                bool showDummyNumber = ShowDummyNumber();
                if (showDummyNumber)
                    lbl_regno.Text = "Dummy No";
            }
        }
        catch (Exception ex)
        {

        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetRollNo(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select Roll_No from Registration where DelFlag=0 and Exam_Flag <>'Debar' and Roll_No Like '" + prefixText + "%' order by Roll_No";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetRegNo(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select Reg_No  from Registration where Reg_No like '" + prefixText + "%' and DelFlag=0 and Exam_Flag <>'Debar' order by Reg_No  ";
        name = ws.Getname(query);
        return name;
    }

    protected void txt_searchbyreg_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            imgdiv2.Visible = false;
            FpBefore.Visible = false;
            btnSave.Visible = false;
            string regno = txt_searchbyreg.Text;

            #region Dummy Number Display by rajkumar for sns
            byte dummyNumberMode = getDummyNumberMode();//0-serial , 1-random
            string dummyNumberType = string.Empty;
            string selDummyQ = string.Empty;
            selDummyQ = "select dummy_no,regno,roll_no from dummynumber where exam_month='" + Convert.ToString(ddl_mm.SelectedValue) + "' and exam_year='" + Convert.ToString(ddl_yy.SelectedValue) + "' and dummy_no='" + regno + "'";
            DataTable dtMappedNumbers = dirAcc.selectDataTable(selDummyQ);
            bool showDummyNumber = ShowDummyNumber();
            bool isval = false;
            if (showDummyNumber)
            {
                if (dtMappedNumbers.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Dummy Numbers Generated')", true);
                    //return;
                }
                else
                {
                    regno = Convert.ToString(dtMappedNumbers.Rows[0]["regno"]);
                    isval = true;
                }
            }
            #endregion
            if (isval || !showDummyNumber)
            {
                if (regno != "" && regno != null)
                {
                    string query = "select  r.stud_name, r.Roll_no,dt.Dept_Name,r.Current_Semester ,r.Sections ,r.Batch_Year,C.Course_Name,r.degree_code,r.Reg_No,r.college_code from Registration r ,Degree d,course c,Department dt where  r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and Reg_No ='" + regno + "' ";
                    ds = d2.select_method_wo_parameter(query, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        ControlVisibility(true, btn_go, Name, lbl_name, Rno, lbl_rno, Batch, lbl_batch, lbl_dept, degree, lbl_degree, Dept, lblDegCode, lbl_sec, Section, result, lbl_reg);

                        result.Visible = true;
                        string batch = Convert.ToString(ds.Tables[0].Rows[0]["Batch_Year"]).Trim();
                        Batch.Visible = true;
                        lbl_batch.Text = batch;
                        string rno = Convert.ToString(ds.Tables[0].Rows[0]["Reg_No"]).Trim();
                        Rno.Visible = true;
                        lbl_rno.Text = Convert.ToString(ds.Tables[0].Rows[0]["Roll_no"]).Trim();
                        lbl_rno.Visible = false;
                        lbl_reg.Text = rno;
                        string sname = Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]).Trim();
                        Name.Visible = true;
                        lbl_name.Text = sname;
                        string sec = Convert.ToString(ds.Tables[0].Rows[0]["Sections"]).Trim();
                        if (sec != "")
                        {
                            Section.Visible = true;
                            lbl_sec.Text = sec;
                        }
                        else
                        {
                            lbl_sec.Visible = false;
                            Section.Visible = false;
                        }
                        string deg = Convert.ToString(ds.Tables[0].Rows[0]["Course_Name"]).Trim();
                        degree.Visible = true;
                        lbl_degree.Text = deg;
                        string sem = Convert.ToString(ds.Tables[0].Rows[0]["Current_Semester"]).Trim();
                        //Semester.Visible = true;
                        lbl_sem.Text = sem;
                        lblCollegeC.Text = Convert.ToString(ds.Tables[0].Rows[0]["college_code"]).Trim();
                        //lbl_sem.Visible = true;
                        string deptstu = Convert.ToString(ds.Tables[0].Rows[0]["Dept_Name"]).Trim();
                        Dept.Visible = true;
                        lbl_dept.Text = deptstu;
                        lbl_dept.Visible = true;
                        lblDegCode.Text = Convert.ToString(ds.Tables[0].Rows[0]["degree_code"]).Trim();
                        lblDegCode.Visible = false;
                        btn_go.Visible = true;
                    }
                    else
                    {
                        //btn_go.Visible = false;
                        //result.Visible = false;
                        //ControlVisibility(false, btn_go, result, lbl_dept, lbl_sem, Semester, Batch);
                        lbl_alert1.Text = "No Records Found";
                        imgdiv2.Visible = true;
                    }
                }
                else
                {
                    ControlVisibility(false, btn_go, Name, lbl_name, Rno, lbl_rno, Batch, lbl_batch, lbl_dept, degree, lbl_degree, Dept, lblDegCode, lbl_sec, Section, result, lbl_reg);
                }
            }
            else
            {
                lbl_alert1.Text = "Invalid Dummy No";
                imgdiv2.Visible = true;
                return;
            }
            if (showDummyNumber)//Rajkumar 30/4/2018
            {
                result.Visible = false;
                btn_go_OnClick(sender, e);
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        lbl_alert1.Text = string.Empty;
        imgdiv2.Visible = false;
    }

    protected void btn_save_OnClick(object sender, EventArgs e)
    {
        try
        {
            string roll_no = Convert.ToString(lbl_rno.Text).Trim();
            string reg_no = Convert.ToString(txt_searchbyreg.Text).Trim();
            string batch_year = Convert.ToString(lbl_batch.Text).Trim();
            string degree_code = Convert.ToString(lblDegCode.Text).Trim();
            string currentSem = Convert.ToString(lbl_sem.Text).Trim();
            string collegeCode = Convert.ToString(lblCollegeC.Text).Trim();
            string query = string.Empty;
            int res = 0;
            string exam_code = string.Empty;
            string month = Convert.ToString(ddl_mm.SelectedItem.Value).Trim();
            string year = Convert.ToString(ddl_yy.SelectedItem.Text).Trim();
            string yr_val = Convert.ToString(ddl_yy.SelectedItem.Value).Trim();
            FpBefore.SaveChanges();

            if (month == "0" || yr_val == "0")
            {
                lbl_alert1.Text = "Please Select Month and Year";
                imgdiv2.Visible = true;
                return;
            }

            if (txt_searchbyreg.Text == null || Convert.ToString(txt_searchbyreg.Text).Trim() == "")
            {
                lbl_alert1.Text = "Please Enter Register No";
                imgdiv2.Visible = true;
                return;
            }

            FpBefore.SaveChanges();
            if (rdb_take.Checked)
            {
                bool checkflage = false;
                exam_code = d2.GetFunctionv("select exam_code from Exam_Details where batch_year='" + batch_year + "' and degree_code='" + degree_code + "' and Exam_year='" + year + "' and Exam_Month='" + month + "'");
                if (string.IsNullOrEmpty(exam_code) || exam_code == "0")
                {
                    string qry = "if not exists(select * from exam_details where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and exam_month='" + month + "' and exam_year='" + year + "') insert into exam_details (degree_code,exam_month,exam_year,batch_year,current_semester,coll_code,isSupplementaryExam) values ('" + degree_code + "','" + month + "','" + year + "','" + batch_year + "','" + currentSem + "','" + collegeCode + "','0')--else update exam_details set isSupplementaryExam='0'  where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and exam_month='" + month + "' and exam_year='" + year + "'";
                    int ins = dirAcc.updateData(qry);
                    exam_code = dirAcc.selectScalarString("select exam_code from exam_details where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and exam_month='" + month + "' and exam_year='" + year + "'");
                }
                if (exam_code.Trim() != "" && roll_no.Trim() != "")
                {
                    if (FpBefore.Sheets[0].RowCount > 0)
                    {
                        for (int r = 0; r < FpBefore.Sheets[0].RowCount; r++)
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(FpBefore.Sheets[0].Cells[r, 6].Text).Trim()))
                            {
                                double revaluation_mark = 0;
                                double Reval_Tot = 0;
                                bool grade3flag = false;

                                string result = string.Empty;
                                string passorfail = string.Empty;
                                double minpass = 0;

                                double minTotal = 0;
                                double maxTotal = 0;

                                double minExternal = 0;
                                double maxExternal = 0;

                                double minInternal = 0;
                                double maxInternal = 0;
                                double revaluationMarkNew = 0;

                                double.TryParse(Convert.ToString(FpBefore.Sheets[0].Cells[r, 6].Tag).Trim(), out minpass);
                                double.TryParse(Convert.ToString(FpBefore.Sheets[0].Cells[r, 6].Text).Trim(), out revaluation_mark);
                                string subno = Convert.ToString(FpBefore.Sheets[0].Cells[r, 1].Tag).Trim();
                                string subcode = Convert.ToString(FpBefore.Sheets[0].Cells[r, 1].Text).Trim();
                                string actual_internal = Convert.ToString(FpBefore.Sheets[0].Cells[r, 3].Text).Trim();
                                string actual_external = Convert.ToString(FpBefore.Sheets[0].Cells[r, 4].Text).Trim();
                                string actual_total = Convert.ToString(FpBefore.Sheets[0].Cells[r, 5].Text).Trim();
                                double revalexternal = 0;
                                double.TryParse(actual_external.Trim(), out revalexternal);

                                double.TryParse(Convert.ToString(FpBefore.Sheets[0].Cells[r, 6].Text).Trim(), out revaluation_mark);
                                revaluationMarkNew = revaluation_mark;


                                double.TryParse(Convert.ToString(FpBefore.Sheets[0].Cells[r, 6].Tag).Trim(), out minpass);
                                double.TryParse(Convert.ToString(FpBefore.Sheets[0].Cells[r, 6].Note).Trim(), out maxTotal);

                                double.TryParse(Convert.ToString(FpBefore.Sheets[0].Cells[r, 3].Tag).Trim(), out minInternal);
                                double.TryParse(Convert.ToString(FpBefore.Sheets[0].Cells[r, 3].Note).Trim(), out maxInternal);

                                double.TryParse(Convert.ToString(FpBefore.Sheets[0].Cells[r, 4].Tag).Trim(), out minExternal);
                                double.TryParse(Convert.ToString(FpBefore.Sheets[0].Cells[r, 4].Note).Trim(), out maxExternal);

                                if (revalexternal < 0)
                                {
                                    string qry = d2.GetFunctionv("select MAX(external_mark) from mark_entry where roll_no='" + roll_no + "' and subject_no='" + subno + "' and exam_code in (select exam_code from Exam_Details where  batch_year='" + batch_year + "' and degree_code='" + degree_code + "')");
                                    actual_external = qry;
                                }
                                double.TryParse(actual_external, out revalexternal);
                                double actual_ext = 0, actual_int = 0, actual_tot = 0;

                                double.TryParse(actual_external, out actual_ext);
                                double.TryParse(actual_internal, out actual_int);
                                if (actual_ext > 0 && actual_int > 0)
                                {
                                    actual_tot = actual_int + actual_ext;
                                }
                                else if (actual_int < 0)
                                {
                                    actual_tot = actual_ext;
                                }
                                else if (actual_ext < 0)
                                {
                                    actual_tot = actual_int;
                                }
                                actual_total = Convert.ToString(actual_tot);
                                if (revaluation_mark < actual_int)
                                {
                                    revaluation_mark = actual_int;
                                    //actualExternalMark = revaluationMarkNew;
                                    //actual_external = revaluationMarkNew.ToString();
                                }
                                if (revalexternal > 0 && revaluation_mark > 0)
                                {
                                    Reval_Tot = revalexternal + revaluation_mark;
                                }
                                else if (revalexternal < 0)
                                {
                                    Reval_Tot = revaluation_mark;
                                }
                                else if (revaluation_mark < 0)
                                {
                                    Reval_Tot = revalexternal;
                                }


                                //if (Reval_Tot < minpass)
                                //{
                                //    result = "Fail";
                                //    passorfail = "0";
                                //}
                                //else
                                //{
                                //    result = "Pass";
                                //    passorfail = "1";
                                //}
                                if (Reval_Tot < minpass || actual_ext < minExternal || revaluation_mark < minInternal)
                                {
                                    result = "Fail";
                                    passorfail = "0";
                                }
                                else
                                {
                                    result = "Pass";
                                    passorfail = "1";
                                }
                                if (revaluation_mark == -1 || actual_ext == -1)
                                {
                                    result = "AAA";
                                    passorfail = "0";
                                }
                                string grade = string.Empty;
                                double dummytot = 0;
                                bool hasGrade = false;
                                result = "Fail";
                                passorfail = "0";
                                string grad_flag = d2.GetFunctionv("select grade_flag from grademaster where batch_year='" + batch_year + "' and degree_code='" + degree_code + "' and exam_month='" + month + "' and exam_year='" + year + "'").Trim();
                                // 1 - mark and grade ; 2 - grade only ; 3 - mark Only
                                if (grad_flag.Trim() == "" || grad_flag.Trim() == "0")
                                {
                                    grad_flag = "3";
                                }
                                if (grad_flag.Trim() == "2" || grad_flag.Trim() == "1")
                                {
                                    dummytot = Math.Round(Reval_Tot, 0, MidpointRounding.AwayFromZero);
                                    grade3flag = true;
                                    grade = d2.GetFunctionv("select Mark_Grade from Grade_Master where batch_year='" + batch_year + "' and Degree_Code='" + degree_code + "'  and '" + dummytot + "'>=Frange and '" + dummytot + "'<=Trange");
                                    if (!string.IsNullOrEmpty(grade.Trim()) && grade.Trim() != "0")
                                    {
                                        hasGrade = true;
                                    }
                                }
                                if (grad_flag.Trim() == "1" || grad_flag.Trim() == "3")
                                {
                                    grade3flag = false;
                                }
                                else
                                {
                                    grade3flag = true;
                                }
                                if (!grade3flag)
                                {
                                    if (Reval_Tot < minpass || actual_ext < minExternal || revaluation_mark < minInternal)
                                    {
                                        result = "Fail";
                                        passorfail = "0";
                                    }
                                    else
                                    {
                                        result = "Pass";
                                        passorfail = "1";
                                    }
                                    if (revaluation_mark == -1 || actual_ext == -1)
                                    {
                                        result = "AAA";
                                        passorfail = "0";
                                    }
                                }
                                else
                                {
                                    if (grade.Trim() != "" && hasGrade && grade3flag)
                                    {
                                        if (grade.ToUpper() == "RA")
                                        {
                                            result = "Fail";
                                            passorfail = "0";
                                        }
                                        else
                                        {
                                            result = "Pass";
                                            passorfail = "1";
                                        }
                                    }
                                }

                                //query = "update mark_entry set actual_internal_mark='" + actual_internal + "',actual_external_mark='" + actual_external + "',actual_total='" + actual_total + "',internal_mark='" + revaluation_mark + "',external_mark='" + actual_external + "',total='" + Reval_Tot + "',result='" + result + "' where roll_no='" + roll_no + "' and subject_no='" + subno + "' and exam_code='" + exam_code + "'";

                                query = " if exists (select * from mark_entry where  roll_no='" + roll_no + "' and subject_no='" + subno + "' and exam_code='" + exam_code + "')  update mark_entry set actual_internal_mark='" + actual_internal + "',actual_external_mark='" + actual_external + "',actual_total='" + actual_total + "',internal_mark='" + revaluation_mark + "',external_mark='" + actual_external + "',total='" + Reval_Tot + "',result='" + result + "',passorfail='" + passorfail + "',Act_ReTotalMark='" + revaluationMarkNew + "' where roll_no='" + roll_no + "' and subject_no='" + subno + "' and exam_code='" + exam_code + "' else  insert into mark_entry (roll_no,subject_no,internal_mark,external_mark,total,result,passorfail,exam_code,attempts,actual_internal_mark,actual_external_mark,actual_total,Act_ReTotalMark) values ('" + roll_no + "','" + subno + "','" + revaluation_mark + "','" + actual_external + "','" + Reval_Tot + "','" + result + "','" + passorfail + "','" + exam_code + "','0','" + actual_internal + "','" + actual_external + "','" + actual_total + "','" + revaluationMarkNew + "')";
                                res = d2.update_method_wo_parameter(query, "Text");
                                if (res == 1)
                                {
                                    checkflage = true;
                                }
                            }
                        }
                        if (checkflage == true)
                        {
                            btn_go_OnClick(sender, e);
                            lbl_alert1.Text = "Saved Successfully";
                            imgdiv2.Visible = true;
                            return;
                        }
                    }
                    else
                    {
                        btn_go_OnClick(sender, e);
                        lbl_alert1.Text = "No Re-Take Request are Found";
                        imgdiv2.Visible = true;
                        return;
                    }
                }
                else
                {
                    btn_go_OnClick(sender, e);
                    lbl_alert1.Text = "No Found Any Exam";
                    imgdiv2.Visible = true;
                    return;
                }
            }
            else if (rdb_tot.Checked)
            {
                bool checkflage = false;
                exam_code = d2.GetFunctionv("select exam_code from Exam_Details where batch_year='" + batch_year + "' and degree_code='" + degree_code + "' and Exam_year='" + year + "' and Exam_Month='" + month + "'");
                if (exam_code.Trim() != "" && roll_no.Trim() != "")
                {
                    if (FpBefore.Sheets[0].RowCount > 0)
                    {
                        for (int r = 0; r < FpBefore.Sheets[0].RowCount; r++)
                        {
                            FpBefore.SaveChanges();
                            if (FpBefore.Sheets[0].ColumnHeader.Cells[0, 3].Text == "Grade")
                            {
                                if (Convert.ToString(FpBefore.Sheets[0].Cells[r, 4].Text).Trim() != "")
                                {
                                    string result = string.Empty;
                                    string passorfail = string.Empty;
                                    bool grade3flag = false;
                                    string revaluationgrade = string.Empty;
                                    string newrevaluationgrade = string.Empty;

                                    double minpass = 0;


                                    string subno = Convert.ToString(FpBefore.Sheets[0].Cells[r, 1].Tag).Trim();
                                    string subname = Convert.ToString(FpBefore.Sheets[0].Cells[r, 2].Text).Trim();
                                    string subcode = Convert.ToString(FpBefore.Sheets[0].Cells[r, 1].Text).Trim();

                                    string actual_grade = Convert.ToString(FpBefore.Sheets[0].Cells[r, 3].Text).Trim();
                                    revaluationgrade = Convert.ToString(FpBefore.Sheets[0].Cells[r, 4].Text).Trim();
                                    newrevaluationgrade = revaluationgrade;
                                    string minfrange = string.Empty;
                                    string mintorange = string.Empty;
                                    string maxfrange = string.Empty;
                                    string maxtorange = string.Empty;
                                    string minrevfrange = string.Empty;
                                    string minrevtorange = string.Empty;
                                    string maxrevfrange = string.Empty;
                                    string maxrevtorange = string.Empty;
                                    string subjectAttempt = d2.GetFunction(" select COUNT(distinct ed.exam_code) as attempts from mark_entry m,Exam_Details ed,subject s where ed.exam_code=m.exam_code and s.subject_no=m.subject_no and s.subject_code in('" + subcode + "') and m.roll_no='" + roll_no + "'");
                                    int subAttempt = 1;
                                    int.TryParse(subjectAttempt, out subAttempt);
                                    //string grade = string.Empty;
                                    string grade = "select distinct  frange,trange from Grade_Master where mark_grade='" + actual_grade + "' and degree_code='" + degree_code + "' and college_code='" + collegeCode + "'";
                                    DataSet dsgrade = d2.select_method_wo_parameter(grade, "text");
                                    if (dsgrade.Tables.Count > 0)
                                    {
                                        minfrange = dsgrade.Tables[0].Rows[0]["frange"].ToString();
                                        mintorange = dsgrade.Tables[0].Rows[0]["trange"].ToString();
                                        maxfrange = string.Empty;
                                        maxtorange = string.Empty;
                                        for (int i1 = 0; i1 < dsgrade.Tables[0].Rows.Count; i1++)
                                        {
                                            maxfrange = dsgrade.Tables[0].Rows[i1]["frange"].ToString();
                                            maxtorange = dsgrade.Tables[0].Rows[i1]["trange"].ToString();
                                        }
                                    }
                                    string revgrade = "select distinct  frange,trange from Grade_Master where mark_grade='" + revaluationgrade + "' and degree_code='" + degree_code + "' and college_code='" + collegeCode + "'";
                                    DataSet dsrevgrade = d2.select_method_wo_parameter(revgrade, "text");
                                    if (dsrevgrade.Tables.Count > 0)
                                    {
                                        minrevfrange = dsrevgrade.Tables[0].Rows[0]["frange"].ToString();
                                        minrevtorange = dsrevgrade.Tables[0].Rows[0]["trange"].ToString();
                                        maxrevfrange = string.Empty;
                                        maxrevtorange = string.Empty;
                                        for (int i1 = 0; i1 < dsrevgrade.Tables[0].Rows.Count; i1++)
                                        {
                                            maxrevfrange = dsrevgrade.Tables[0].Rows[i1]["frange"].ToString();
                                            maxrevtorange = dsrevgrade.Tables[0].Rows[i1]["trange"].ToString();
                                        }
                                    }
                                    string minfrange1 = string.Empty;
                                    if (Convert.ToInt32(minfrange) > Convert.ToInt32(minrevfrange))
                                    {
                                        minfrange1 = minfrange;
                                    }
                                    else
                                    {
                                        minfrange1 = minrevfrange;
                                    }
                                    string maxfrange1 = string.Empty;
                                    if (Convert.ToInt32(maxfrange) > Convert.ToInt32(maxrevfrange))
                                    {
                                        maxfrange1 = maxfrange;
                                    }
                                    else
                                    {
                                        maxfrange1 = maxrevfrange;
                                    }
                                    string mintrange1 = string.Empty;
                                    if (Convert.ToInt32(mintorange) > Convert.ToInt32(minrevtorange))
                                    {
                                        mintrange1 = mintorange;
                                    }
                                    else
                                    {
                                        mintrange1 = minrevtorange;
                                    }
                                    string maxtrange1 = string.Empty;
                                    if (Convert.ToInt32(maxtorange) > Convert.ToInt32(maxrevtorange))
                                    {
                                        maxtrange1 = maxtorange;
                                    }
                                    else
                                    {
                                        maxtrange1 = maxrevtorange;
                                    }
                                    string markfrange = string.Empty;
                                    //string newrevgrade = "select distinct mark_grade from Grade_Master where frange between '" + minfrange1 + "' and '" + maxfrange1 + "' and degree_code='" + degree_code + "' and college_code='" + collegeCode + "'";
                                    //DataSet dsfinalgrade = d2.select_method_wo_parameter(newrevgrade, "text");
                                    //if (dsfinalgrade.Tables[0].Rows.Count > 0)
                                    //{
                                    //    newrevaluationgrade = Convert.ToString(dsfinalgrade.Tables[0].Rows[0]["mark_grade"]).Trim();
                                    //}
                                    string grademark = "select distinct max(frange)as frange from Grade_Master where mark_grade='" + actual_grade + "' and degree_code='" + degree_code + "' and college_code='" + collegeCode + "'";
                                    DataSet dsgrademark = d2.select_method_wo_parameter(grademark, "text");
                                    if (dsgrademark.Tables[0].Rows.Count > 0)
                                    {
                                        markfrange = dsgrademark.Tables[0].Rows[0]["frange"].ToString();

                                    }
                                    if (Convert.ToInt32(minfrange1) > Convert.ToInt32(markfrange))
                                    {

                                        actual_grade = newrevaluationgrade;
                                    }
                                   

                                    if (cbRegulation.Checked == true)
                                    {


                                        string strgetmaek = "select m.result,m.attempts,s.max_ext_marks,ss.lab from mark_entry m,sub_sem ss,subject s where m.subject_no=s.subject_no and s.subtype_no=ss.subtype_no and m.roll_no='" + roll_no + "' and exam_code='" + exam_code + "' and s.subject_no='" + subno + "'";
                                        DataSet dsexmark = d2.select_method_wo_parameter(strgetmaek, "Text");
                                        if (dsexmark.Tables.Count > 0 && dsexmark.Tables[0].Rows.Count > 0)
                                        {
                                            string presult = Convert.ToString(dsexmark.Tables[0].Rows[0]["result"]).Trim();
                                            int attenmpts = 0;
                                            int.TryParse(Convert.ToString(dsexmark.Tables[0].Rows[0]["attempts"]).Trim(), out attenmpts);
                                            subcode = Convert.ToString(FpBefore.Sheets[0].Cells[r, 2].Text).Trim();
                                            grade = "RA";
                                            result = "Fail";
                                            passorfail = "0";
                                            string lab = Convert.ToString(dsexmark.Tables[0].Rows[0]["lab"]).Trim();

                                            string equalsub = string.Empty;
                                            string strsuboquery = "select subject_no from tbl_equal_subject_Grade_System where Common_Subject_no in(select Common_Subject_no from tbl_equal_subject_Grade_System where Subject_no='" + subno + "')";
                                            DataSet dsequlsub = d2.select_method_wo_parameter(strsuboquery, "text");
                                            if (dsequlsub.Tables.Count > 0 && dsequlsub.Tables[0].Rows.Count > 0)
                                            {
                                                for (int es = 0; es < dsequlsub.Tables[0].Rows.Count; es++)
                                                {
                                                    string getsubno = Convert.ToString(dsequlsub.Tables[0].Rows[es]["subject_no"]).Trim();
                                                    if (equalsub.Trim() != "")
                                                    {
                                                        equalsub = equalsub + ",'" + getsubno + "'";
                                                    }
                                                    else
                                                    {
                                                        equalsub = "'" + getsubno + "'";
                                                    }
                                                }
                                            }
                                            if (equalsub.Trim() == "")
                                            {
                                                equalsub = "'" + subno + "'";
                                            }

                                            string strquery = " select * from SubWiseGrdeMaster where Exam_Year='" + year + "' and Exam_Month='" + month + "' and SubjectCode in(" + equalsub + ") order by Frange desc";
                                            ds.Dispose();
                                            ds = d2.select_method_wo_parameter(strquery, "Text");
                                            if (ds.Tables.Count > 0)
                                            {
                                                if (subAttempt > 1 && ds.Tables[0].Rows.Count == 0)
                                                {
                                                    strquery = " select *,(Exam_Year*12+Exam_Month) exmonval from SubWiseGrdeMaster where SubjectName='" + subname + "' and (Exam_Year*12+Exam_Month)<('" + year + "'*12+'" + month + "') order by exmonval desc,Frange desc";
                                                }
                                            }
                                            ds.Dispose();
                                            ds = d2.select_method_wo_parameter(strquery, "Text");
                                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                            {
                                                ds.Tables[0].DefaultView.RowFilter = "grade='B'";
                                                DataView dvgrade = ds.Tables[0].DefaultView;


                                                bool failgrade = false;

                                                string reese = Convert.ToString(FpBefore.Sheets[0].Cells[r, 4].Text).Trim();

                                                string stumark = minfrange1;

                                                if (lab.Trim() == "1" || lab.Trim().ToLower() == "true")
                                                {
                                                    if (Convert.ToInt32(stumark) < 50)
                                                    {
                                                        failgrade = true;
                                                    }
                                                }
                                                else
                                                {
                                                    if (Convert.ToInt32(stumark) < 50)
                                                    {
                                                        failgrade = true;
                                                    }
                                                }
                                                if (failgrade == false)
                                                {
                                                    ds.Tables[0].DefaultView.RowFilter = "Frange<'" + minfrange1 + "' and Trange >='" + minfrange1 + "'";
                                                    if (subAttempt > 1)
                                                    {
                                                        ds.Tables[0].DefaultView.RowFilter = "Frange<'" + minfrange1 + "' and Trange >='" + minfrange1 + "'";
                                                    }
                                                    dvgrade = ds.Tables[0].DefaultView;
                                                    dvgrade.Sort = "Frange asc";
                                                    if (dvgrade.Count > 0)
                                                    {
                                                        grade = Convert.ToString(dvgrade[0]["Grade"]).Trim();
                                                        result = "Pass";
                                                        passorfail = "1";
                                                    }
                                                    else
                                                    {
                                                        grade = "B";
                                                        result = "Pass";
                                                        passorfail = "1";
                                                    }
                                                }
                                                if (Convert.ToInt32(minfrange1) == -1)
                                                {
                                                    result = "AAA";
                                                    passorfail = "0";
                                                    grade = "Ra";
                                                }
                                                else if (Convert.ToInt32(minfrange1) == -4)
                                                {
                                                    result = "WHD";
                                                    passorfail = "0";
                                                    grade = "Ra";
                                                }
                                                if (presult.Trim().ToLower().Contains("aa"))
                                                {
                                                    result = "AAA";
                                                    passorfail = "0";
                                                    grade = "Ra";
                                                }
                                                else if (presult.Trim().ToLower().Contains("w"))
                                                {
                                                    result = "WHD";
                                                    passorfail = "0";
                                                    grade = "Ra";
                                                }

                                            }
                                        }
                                    }
                                    else
                                    {
                                        bool hasGrade = false;
                                        result = "Fail";
                                        passorfail = "0";
                                        string grad_flag = d2.GetFunctionv("select grade_flag from grademaster where batch_year='" + batch_year + "' and degree_code='" + degree_code + "' and exam_month='" + month + "' and exam_year='" + year + "'").Trim();
                                        if (grad_flag.Trim() == "" || grad_flag.Trim() == "0")
                                        {
                                            grad_flag = "3";
                                        }
                                        if (grad_flag.Trim() == "2" || grad_flag.Trim() == "1")
                                        {

                                            grade3flag = true;
                                            grade = newrevaluationgrade;
                                            if (!string.IsNullOrEmpty(grade.Trim()) && grade.Trim() != "0")
                                            {
                                                hasGrade = true;
                                            }
                                        }
                                        if (grad_flag.Trim() == "1" || grad_flag.Trim() == "3")
                                        {
                                            grade3flag = false;
                                        }
                                        else
                                        {
                                            grade3flag = true;
                                        }
                                        if (!grade3flag)
                                        {
                                            if (Convert.ToInt32(minfrange1) >= 50)
                                            {
                                                result = "Pass";
                                                passorfail = "1";
                                            }
                                            else if ((Convert.ToInt32(minfrange1)) < 50)
                                            {
                                                result = "Fail";
                                                passorfail = "0";
                                            }

                                        }
                                        else
                                        {
                                            if (grade.Trim() != "" && hasGrade && grade3flag)
                                            {
                                                if (grade.ToUpper() == "RA")
                                                {
                                                    result = "Fail";
                                                    passorfail = "0";
                                                }
                                                else
                                                {
                                                    result = "Pass";
                                                    passorfail = "1";
                                                }
                                            }
                                        }
                                    }


                                    string updategradeqry = "update mark_entry set grade='" + actual_grade + "',Actual_Grade='" + newrevaluationgrade + "',passorfail='" + subAttempt + "',result='" + result + "',actual_internal_mark='',actual_external_mark='',actual_total='',Act_Reval_Mark='',internal_mark='',external_mark='',total='' where roll_no='" + roll_no + "'and exam_code='" + exam_code + "' and subject_no='" + subno + "'";
                                    res = d2.update_method_wo_parameter(updategradeqry, "text");




                                }
                            }

                            else
                            {
                                if (Convert.ToString(FpBefore.Sheets[0].Cells[r, 6].Text).Trim() != "")
                                {
                                    string result = string.Empty;
                                    string passorfail = string.Empty;
                                    bool grade3flag = false;

                                    double minpass = 0;

                                    double minTotal = 0;
                                    double maxTotal = 0;

                                    double minExternal = 0;
                                    double maxExternal = 0;

                                    double minInternal = 0;
                                    double maxInternal = 0;

                                    double dummytot = 0;

                                    double revalinternal = 0;
                                    double revaluation_mark = 0;
                                    double Reval_Tot = 0;
                                    double revaluationMarkNew = 0;

                                    double actualExternalMark = 0;

                                    string subno = Convert.ToString(FpBefore.Sheets[0].Cells[r, 1].Tag).Trim();
                                    string subname = Convert.ToString(FpBefore.Sheets[0].Cells[r, 2].Text).Trim();
                                    string subcode = Convert.ToString(FpBefore.Sheets[0].Cells[r, 1].Text).Trim();

                                    string actual_internal = Convert.ToString(FpBefore.Sheets[0].Cells[r, 3].Text).Trim();
                                   // string actual_external = Convert.ToString(FpBefore.Sheets[0].Cells[r, 4].Text).Trim();
                                    string actual_total = Convert.ToString(FpBefore.Sheets[0].Cells[r, 5].Text).Trim();
                                    string actual_grade = Convert.ToString(FpBefore.Sheets[0].Cells[r, 5].Tag).Trim();
                                    string rev_grade = Convert.ToString(FpBefore.Sheets[0].Cells[r, 6].Tag).Trim();

                                   // double.TryParse(Convert.ToString(actual_external).Trim(), out actualExternalMark);
                                    double.TryParse(Convert.ToString(FpBefore.Sheets[0].Cells[r, 6].Text).Trim(), out revaluation_mark);
                                    revaluationMarkNew = revaluation_mark;
                                    double.TryParse(actual_internal, out revalinternal);
                                    double.TryParse(Convert.ToString(FpBefore.Sheets[0].Cells[r, 6].Tag).Trim(), out minpass);
                                    double.TryParse(Convert.ToString(FpBefore.Sheets[0].Cells[r, 6].Note).Trim(), out maxTotal);

                                    double.TryParse(Convert.ToString(FpBefore.Sheets[0].Cells[r, 3].Tag).Trim(), out minInternal);
                                    double.TryParse(Convert.ToString(FpBefore.Sheets[0].Cells[r, 3].Note).Trim(), out maxInternal);

                                    double.TryParse(Convert.ToString(FpBefore.Sheets[0].Cells[r, 4].Tag).Trim(), out minExternal);
                                    double.TryParse(Convert.ToString(FpBefore.Sheets[0].Cells[r, 4].Note).Trim(), out maxExternal);

                                    string actual_external = dirAcc.selectScalarString("select ISNULL(actual_external_mark,'0') as actual_external_mark from mark_entry where roll_no='" + roll_no + "' and subject_no='" + subno + "' and exam_code='" + exam_code + "'"); //modified by Mullai
                                    actualExternalMark = Convert.ToDouble(actual_external);
                                    if (actual_external == "0")
                                    {

                                        actual_external = dirAcc.selectScalarString("select ISNULL(external_mark,'0') from mark_entry where roll_no='" + roll_no + "' and subject_no='" + subno + "' and exam_code='" + exam_code + "'");
                                        actualExternalMark = Convert.ToDouble(actual_external);
                                    }

                                    string subjectAttempt = d2.GetFunction(" select COUNT(distinct ed.exam_code) as attempts from mark_entry m,Exam_Details ed,subject s where ed.exam_code=m.exam_code and s.subject_no=m.subject_no and s.subject_code in('" + subcode + "') and m.roll_no='" + roll_no + "'");
                                    int subAttempt = 1;
                                    int.TryParse(subjectAttempt, out subAttempt);
                                    string grade = string.Empty;
                                    string actmark = "select * from mark_entry where roll_no='" + roll_no + "' and exam_code='" + exam_code + "' and subject_no='" + subno + "'";
                                    DataSet actexmr = new DataSet();
                                    actexmr = da.select_method_wo_parameter(actmark, "text");
                                    string actextmrk = Convert.ToString(actexmr.Tables[0].Rows[0]["actual_external_mark"]).Trim();
                                    string revalmrk1 = Convert.ToString(actexmr.Tables[0].Rows[0]["revaluation_1"]).Trim();
                                    string revalmrk2 = Convert.ToString(actexmr.Tables[0].Rows[0]["revaluation_2"]).Trim();
                                    string revalmrk3 = Convert.ToString(actexmr.Tables[0].Rows[0]["revaluation_3"]).Trim();
                                    string actrev = Convert.ToString(actexmr.Tables[0].Rows[0]["external_mark"]).Trim();
                                    double revaldiff2 = 0;
                                    double revaldiff1 = 0;
                                    double revaldiff3 = 0;
                                    double revaldiff4 = 0;
                                    string exmtype = "2";
                                    string appl_no = dirAcc.selectScalarString("select appl_no from exam_application where roll_no ='" + roll_no + "' and Exam_type ='" + exmtype + "' and exam_code ='" + exam_code + "'");
                                    string revalcount = dirAcc.selectScalarString("select revaluation_count from exam_appl_details where subject_no ='" + subno + "' and appl_no ='" + appl_no + "'");
                                    string revaluationct = string.Empty;
                                     if(revalcount=="1")
                                     {
                                          revaluationct = " , revaluation_1='" + revaluationMarkNew + "'";
                                     }
                                     else if (revalcount == "2")
                                     {
                                         revaluationct = " , revaluation_2='" + revaluationMarkNew + "'";
                                     }
                                     else if (revalcount == "3")
                                     {
                                         revaluationct = " , revaluation_3='" + revaluationMarkNew + "'";
                                     }
                                   
                                  
                                    if (chknearestval.Checked == true)
                                    {
                                        if (revalcount == "")
                                        {
                                            revalcount = "0";
                                        }
                                        if (Convert.ToInt32(revalcount) == 2)
                                        {
                                            if (revaluationMarkNew > Convert.ToDouble(actextmrk))
                                            {
                                                revaldiff2 = revaluationMarkNew - Convert.ToDouble(actextmrk);
                                            }
                                            if (revaldiff2 != 0)
                                            {
                                                if (Convert.ToDouble(revalmrk1) > Convert.ToDouble(actextmrk))
                                                {
                                                    revaldiff1 = Convert.ToDouble(revalmrk1) - Convert.ToDouble(actextmrk);
                                                }
                                                
                                                if (revaldiff2 > revaldiff1)
                                                {
                                                    if (revaldiff1 != 0)
                                                    {
                                                        revaluation_mark = Convert.ToDouble(revalmrk1);
                                                    }
                                                    else
                                                    {
                                                        revaluation_mark = revaluationMarkNew;
                                                    }
                                                }
                                                else
                                                {
                                                    revaluation_mark = revaluationMarkNew;
                                                }
                                            }
                                            else
                                            {
                                                revaluation_mark = Convert.ToDouble(actrev);
                                            }
                                        }
                                        else if (Convert.ToInt32(revalcount) == 3)
                                        {
                                            if (revaluationMarkNew > Convert.ToDouble(actextmrk))
                                            {
                                                revaldiff3 = revaluationMarkNew - Convert.ToDouble(actextmrk);
                                            }
                                            if (revaldiff3 != 0)
                                            {
                                                if (Convert.ToDouble(revalmrk2) > Convert.ToDouble(actextmrk))
                                                {
                                                    revaldiff2 = Convert.ToDouble(revalmrk2) - Convert.ToDouble(actextmrk);
                                                }
                                                if (Convert.ToDouble(revalmrk1) > Convert.ToDouble(actextmrk))
                                                {
                                                    revaldiff1 = Convert.ToDouble(revalmrk1) - Convert.ToDouble(actextmrk);
                                                }
                                                if (revaldiff2 > revaldiff1)
                                                {
                                                    revaldiff4 = revaldiff1;
                                                }
                                                else
                                                {
                                                    revaldiff4 = revaldiff2;
                                                }
                                                if (revaldiff4 > revaldiff3)
                                                {
                                                    revaluation_mark = Convert.ToDouble(actrev);
                                                }
                                                else
                                                {
                                                    revaluation_mark = revaluationMarkNew;
                                                }
                                               
                                            }
                                            else
                                            {
                                                revaluation_mark = Convert.ToDouble(actrev);
                                            }

                                        }
                                        if (revalinternal > 0 && revaluation_mark > 0)
                                        {
                                            Reval_Tot = revalinternal + revaluation_mark;
                                        }
                                        else if (revalinternal < 0)
                                        {
                                            Reval_Tot = revaluation_mark;
                                        }
                                        else if (revaluation_mark < 0)
                                        {
                                            Reval_Tot = revalinternal;
                                        }
                                        
                                    }
                                    else
                                    {
                                        if (revaluation_mark < actualExternalMark)
                                        {
                                            revaluation_mark = actualExternalMark;
                                            //actualExternalMark = revaluationMarkNew;
                                            //actual_external = revaluationMarkNew.ToString();
                                        }

                                        if (revalinternal > 0 && revaluation_mark > 0)
                                        {
                                            Reval_Tot = revalinternal + revaluation_mark;
                                        }
                                        else if (revalinternal < 0)
                                        {
                                            Reval_Tot = revaluation_mark;
                                        }
                                        else if (revaluation_mark < 0)
                                        {
                                            Reval_Tot = revalinternal;
                                        }
                                    }
                                    string markfrange = string.Empty;
                                    

                                    string newrevgrade = "select distinct mark_grade from Grade_Master where Frange <='" + Reval_Tot + "' and Trange >='" + Reval_Tot + "' and degree_code='" + degree_code + "' and college_code='" + collegeCode + "' and batch_year='" + batch_year + "'";
                                    DataSet dsfinalgrade = d2.select_method_wo_parameter(newrevgrade, "text");
                                    if (dsfinalgrade.Tables[0].Rows.Count > 0)
                                    {
                                        rev_grade = dsfinalgrade.Tables[0].Rows[0]["mark_grade"].ToString();
                                    }
                                    




                                    //string grademark = "select distinct max(frange)as frange from Grade_Master where mark_grade='" + actual_total + "' and degree_code='" + degree_code + "' and college_code='" + collegeCode + "'";
                                    //DataSet dsgrademark = d2.select_method_wo_parameter(grademark, "text");
                                    //if (dsgrademark.Tables[0].Rows.Count > 0)
                                    //{
                                    //    markfrange = dsgrademark.Tables[0].Rows[0]["frange"].ToString();

                                    //}
                                    //if (!string.IsNullOrEmpty(markfrange))
                                    //{
                                    //    if (Convert.ToInt32(Reval_Tot) > Convert.ToInt32(markfrange))
                                    //    {

                                    //        actual_grade = rev_grade;
                                    //    }
                                    //    else
                                    //    {
                                    //        rev_grade = actual_grade;
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    rev_grade = actual_grade;
                                    //}

                                    DataSet dsgrade = new DataSet();
                                    if (cbRegulation.Checked == true)
                                    {
                                        //grade = d2.GetFunctionv("SELECT Grade from SubWiseGrdeMaster where College_Code='" + Convert.ToString(Session["collegecode"]) + "' and Exam_Month='" + month + "' and Exam_Year='" + year + "' and SubjectCode='" + subcode + "' and '" + Reval_Tot + "' >= Trange and '" + Reval_Tot + "'<=FRange");

                                        string strgetmaek = "select m.result,m.attempts,s.max_ext_marks,ss.lab from mark_entry m,sub_sem ss,subject s where m.subject_no=s.subject_no and s.subtype_no=ss.subtype_no and m.roll_no='" + roll_no + "' and exam_code='" + exam_code + "' and s.subject_no='" + subno + "'";
                                        DataSet dsexmark = d2.select_method_wo_parameter(strgetmaek, "Text");
                                        if (dsexmark.Tables.Count > 0 && dsexmark.Tables[0].Rows.Count > 0)
                                        {
                                            string presult = Convert.ToString(dsexmark.Tables[0].Rows[0]["result"]).Trim();
                                            int attenmpts = 0;// Convert.ToInt32(Convert.ToString(dsexmark.Tables[0].Rows[0]["attempts"]));
                                            int.TryParse(Convert.ToString(dsexmark.Tables[0].Rows[0]["attempts"]).Trim(), out attenmpts);
                                            subcode = Convert.ToString(FpBefore.Sheets[0].Cells[r, 2].Text).Trim();
                                            grade = "RA";
                                            result = "Fail";
                                            passorfail = "0";
                                            double minextmark = 0;
                                            double mintotal = 0;
                                            string lab = Convert.ToString(dsexmark.Tables[0].Rows[0]["lab"]).Trim();
                                            double maxexter = 0;// Convert.ToDouble(Convert.ToString(dsexmark.Tables[0].Rows[0]["max_ext_marks"]).Trim());
                                            double.TryParse(Convert.ToString(dsexmark.Tables[0].Rows[0]["max_ext_marks"]).Trim(), out maxexter);

                                            string equalsub = string.Empty;
                                            string strsuboquery = "select subject_no from tbl_equal_subject_Grade_System where Common_Subject_no in(select Common_Subject_no from tbl_equal_subject_Grade_System where Subject_no='" + subno + "')";
                                            DataSet dsequlsub = d2.select_method_wo_parameter(strsuboquery, "text");
                                            if (dsequlsub.Tables.Count > 0 && dsequlsub.Tables[0].Rows.Count > 0)
                                            {
                                                for (int es = 0; es < dsequlsub.Tables[0].Rows.Count; es++)
                                                {
                                                    string getsubno = Convert.ToString(dsequlsub.Tables[0].Rows[es]["subject_no"]).Trim();
                                                    if (equalsub.Trim() != "")
                                                    {
                                                        equalsub = equalsub + ",'" + getsubno + "'";
                                                    }
                                                    else
                                                    {
                                                        equalsub = "'" + getsubno + "'";
                                                    }
                                                }
                                            }
                                            if (equalsub.Trim() == "")
                                            {
                                                equalsub = "'" + subno + "'";
                                            }

                                            string strquery = " select * from SubWiseGrdeMaster where Exam_Year='" + year + "' and Exam_Month='" + month + "' and SubjectCode in(" + equalsub + ") order by Frange desc";
                                            ds.Dispose();
                                            ds = d2.select_method_wo_parameter(strquery, "Text");
                                            if (ds.Tables.Count > 0)
                                            {
                                                if (subAttempt > 1 && ds.Tables[0].Rows.Count == 0)
                                                {
                                                    strquery = " select *,(Exam_Year*12+Exam_Month) exmonval from SubWiseGrdeMaster where SubjectName='" + subname + "' and (Exam_Year*12+Exam_Month)<('" + year + "'*12+'" + month + "') order by exmonval desc,Frange desc";
                                                }
                                            }
                                            ds.Dispose();
                                            ds = d2.select_method_wo_parameter(strquery, "Text");
                                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                            {
                                                ds.Tables[0].DefaultView.RowFilter = "grade='B'";
                                                DataView dvgrade = ds.Tables[0].DefaultView;
                                                if (dvgrade.Count > 0)
                                                {
                                                    if (subAttempt > 1)
                                                    {
                                                        minextmark = 0;// Convert.ToDouble(dvgrade[0]["trange"].ToString());
                                                        double.TryParse(Convert.ToString(dvgrade[0]["Frange"]).Trim(), out minextmark);
                                                        minextmark = (minextmark * maxExternal) / 100;
                                                        mintotal = 0;// Convert.ToDouble(dvgrade[0]["trange"].ToString());
                                                        double.TryParse(Convert.ToString(dvgrade[0]["Frange"]).Trim(), out mintotal);
                                                    }
                                                    else
                                                    {
                                                        minextmark = 0;//Convert.ToDouble(dvgrade[0]["frange"].ToString());
                                                        double.TryParse(Convert.ToString(dvgrade[0]["Frange"]).Trim(), out minextmark);
                                                        minextmark = (minextmark * maxExternal) / 100;
                                                        mintotal = 0;//Convert.ToDouble(dvgrade[0]["frange"].ToString());
                                                        double.TryParse(Convert.ToString(dvgrade[0]["Frange"]).Trim(), out mintotal);
                                                    }
                                                }
                                                if (mintotal > 50)
                                                {
                                                    mintotal = 50;
                                                }
                                                bool failgrade = false;
                                                double stumarkval = Reval_Tot;

                                                string reese = Convert.ToString(FpBefore.Sheets[0].Cells[r, 6].Text).Trim();
                                                double esemark = 0;
                                                if (reese != "")
                                                {
                                                    esemark = 0;//Convert.ToDouble(reese);
                                                    double.TryParse(Convert.ToString(reese).Trim(), out esemark);
                                                }
                                                double eseofmark = Convert.ToDouble(esemark) / Convert.ToDouble(maxexter) * 100;
                                                eseofmark = Math.Round(eseofmark, 2, MidpointRounding.AwayFromZero);

                                                if (lab.Trim() == "1" || lab.Trim().ToLower() == "true")
                                                {
                                                    if (stumarkval < 50 || eseofmark < 50)
                                                    {
                                                        failgrade = true;
                                                    }
                                                }
                                                else
                                                {
                                                    if (stumarkval < mintotal || Convert.ToDouble(eseofmark) < minextmark)
                                                    {
                                                        failgrade = true;
                                                    }
                                                }
                                                if (failgrade == false)
                                                {
                                                    ds.Tables[0].DefaultView.RowFilter = "Frange<'" + Reval_Tot + "' and Trange >='" + Reval_Tot + "'";// "Frange<='" + Reval_Tot + "'"; 
                                                    if (subAttempt > 1)
                                                    {
                                                        ds.Tables[0].DefaultView.RowFilter = "Frange<'" + Reval_Tot + "' and Trange >='" + Reval_Tot + "'";// "Frange<='" + Reval_Tot + "'";
                                                    }
                                                    dvgrade = ds.Tables[0].DefaultView;
                                                    dvgrade.Sort = "Frange asc";
                                                    if (dvgrade.Count > 0)
                                                    {
                                                        grade = Convert.ToString(dvgrade[0]["Grade"]).Trim();
                                                        result = "Pass";
                                                        passorfail = "1";
                                                    }
                                                    else
                                                    {
                                                        grade = "B";
                                                        result = "Pass";
                                                        passorfail = "1";
                                                    }
                                                }
                                                if (esemark == -1)
                                                {
                                                    result = "AAA";
                                                    passorfail = "0";
                                                    grade = "Ra";
                                                }
                                                else if (esemark == -4)
                                                {
                                                    result = "WHD";
                                                    passorfail = "0";
                                                    grade = "Ra";
                                                }
                                                if (presult.Trim().ToLower().Contains("aa"))
                                                {
                                                    result = "AAA";
                                                    passorfail = "0";
                                                    grade = "Ra";
                                                }
                                                else if (presult.Trim().ToLower().Contains("w"))
                                                {
                                                    result = "WHD";
                                                    passorfail = "0";
                                                    grade = "Ra";
                                                }

                                                double actInt = 0;
                                                int revMArk = 0;
                                                int.TryParse(Convert.ToString(revaluation_mark), out revMArk);
                                                double.TryParse(actual_internal, out actInt);
                                                // double.TryParse(revaluation_mark, out revMArk);
                                                double total = actInt + revaluation_mark;

                                                //Cmd by Saranyadevi 10.8.2018
                                                //string insupdatequery = "update mark_entry set grade='" + grade + "',Actual_Grade='" + actual_grade + "',Actual_Grade='" + rev_grade + "',passorfail='" + passorfail + "',result='" + result + "',actual_internal_mark='" + actual_internal + "',actual_external_mark='" + actual_external + "',actual_total='" + actual_total + "',Act_Reval_Mark='" + revaluationMarkNew + "',internal_mark='" + actual_internal + "',external_mark='" + revaluation_mark + "',total='" + Reval_Tot + "' where roll_no='" + roll_no + "' and exam_code='" + exam_code + "' and subject_no='" + subno + "'";
                                                //Added by Saranyadevi 10.8.2018
                                                string insupdatequery = "update mark_entry set grade='" + grade + "',Actual_Grade='" + actual_grade + "',passorfail='" + passorfail + "',result='" + result + "',actual_internal_mark='" + actual_internal + "',actual_external_mark='" + actual_external + "',actual_total='" + actual_total + "',Act_Reval_Mark='" + revaluationMarkNew + "',internal_mark='" + actual_internal + "',external_mark='" + revaluation_mark + "',total='" + total + "' where roll_no='" + roll_no + "' and exam_code='" + exam_code + "' and subject_no='" + subno + "'";
                                                res = d2.update_method_wo_parameter(insupdatequery, "text");
                                            }
                                            else
                                            {
                                                lbl_alert1.Text = "Please Calculate the Choice Based System for the Subject : " + subcode;
                                                imgdiv2.Visible = true;
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            lbl_alert1.Text = "Please Check Previous Mark Entry for the Subject : " + subcode;
                                            imgdiv2.Visible = true;
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        bool hasGrade = false;
                                        result = "Fail";
                                        passorfail = "0";
                                        string grad_flag = d2.GetFunctionv("select grade_flag from grademaster where batch_year='" + batch_year + "' and degree_code='" + degree_code + "' and exam_month='" + month + "' and exam_year='" + year + "'").Trim();
                                        // 1 - mark and grade ; 2 - grade only ; 3 - mark Only
                                        if (grad_flag.Trim() == "" || grad_flag.Trim() == "0")
                                        {
                                            grad_flag = "3";
                                        }
                                        if (grad_flag.Trim() == "2" || grad_flag.Trim() == "1")
                                        {
                                            dummytot = Math.Round(Reval_Tot, 0, MidpointRounding.AwayFromZero);
                                            grade3flag = true;
                                            grade = d2.GetFunctionv("select Mark_Grade from Grade_Master where batch_year='" + batch_year + "' and Degree_Code='" + degree_code + "'  and '" + dummytot + "'>=Frange and '" + dummytot + "'<=Trange");
                                            if (!string.IsNullOrEmpty(grade.Trim()) && grade.Trim() != "0")
                                            {
                                                hasGrade = true;
                                            }
                                        }
                                        if (grad_flag.Trim() == "1" || grad_flag.Trim() == "3")
                                        {
                                            grade3flag = false;
                                        }
                                        else
                                        {
                                            grade3flag = true;
                                        }
                                        if (!grade3flag)
                                        {
                                            if (Reval_Tot >= minpass && revalinternal >= minInternal && revaluation_mark >= minExternal)
                                            {
                                                result = "Pass";
                                                passorfail = "1";
                                            }
                                            else if (revalinternal < minInternal)
                                            {
                                                result = "Fail";
                                                passorfail = "0";
                                            }
                                            else if (revaluation_mark < minExternal)
                                            {
                                                result = "Fail";
                                                passorfail = "0";
                                            }
                                            else if (Reval_Tot < minpass)
                                            {
                                                result = "Fail";
                                                passorfail = "0";
                                            }
                                        }
                                        else
                                        {
                                            if (grade.Trim() != "" && hasGrade && grade3flag)
                                            {
                                                if (grade.ToUpper() == "RA")
                                                {
                                                    result = "Fail";
                                                    passorfail = "0";
                                                }
                                                else
                                                {
                                                    result = "Pass";
                                                    passorfail = "1";
                                                }
                                            }
                                        }
                                        double actInt = 0;
                                        int revMArk = 0;
                                        int.TryParse(Convert.ToString(revaluation_mark), out revMArk);
                                        double.TryParse(actual_internal, out actInt);
                                        // double.TryParse(revaluation_mark, out revMArk);
                                        double total = actInt + revaluation_mark;

                                        //Cmd by Saranyadevi 10.8.2018
                                        //query = "update mark_entry set actual_internal_mark='" + actual_internal + "',actual_external_mark='" + actual_external + "',actual_total='" + actual_total + "',internal_mark='" + actual_internal + "',Act_Reval_Mark='" + revaluationMarkNew + "',external_mark='" + revaluation_mark + "',total='" + Reval_Tot + "',result='" + result + "' , Grade='" + rev_grade + "' ,Actual_Grade='" + actual_grade + "',passorfail='" + passorfail + "' where roll_no='" + roll_no + "' and subject_no='" + subno + "' and exam_code='" + exam_code + "'";
                                        //Added by Saranyadevi 10.8.2018
                                        query = "update mark_entry set actual_internal_mark='" + actual_internal + "',actual_external_mark='" + actual_external + "',actual_total='" + actual_total + "',internal_mark='" + actual_internal + "',Act_Reval_Mark='" + revaluationMarkNew + "',external_mark='" + revaluation_mark + "',total='" + total + "',result='" + result + "' , Grade='" + rev_grade + "' ,Actual_Grade='" + actual_grade + "',passorfail='" + passorfail + "' "+revaluationct+" where roll_no='" + roll_no + "' and subject_no='" + subno + "' and exam_code='" + exam_code + "'";
                                        res = d2.update_method_wo_parameter(query, "Text");//rev_grade
                                    }
                                }
                            }
                                    if (res == 1)
                                    {
                                        checkflage = true;
                                    }
                                
                            
                        }
                        if (checkflage == true)
                        {
                            btn_go_OnClick(sender, e);
                            lbl_alert1.Text = "Saved Successfully";
                            imgdiv2.Visible = true;
                            return;
                        }
                    }
                    else
                    {
                        btn_go_OnClick(sender, e);
                        lbl_alert1.Text = "No Re-Val Request are Found";
                        imgdiv2.Visible = true;
                        return;
                    }
                }
                else
                {
                    btn_go_OnClick(sender, e);
                    lbl_alert1.Text = "No Found Any Exam";
                    imgdiv2.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            lbl_alert1.Text = ex.ToString();
            imgdiv2.Visible = true;
        }
    }

    protected void btn_go_OnClick(object sender, EventArgs e)
    {
        try
        {
            string roll_no = Convert.ToString(lbl_rno.Text).Trim();
            string reg_no = Convert.ToString(txt_searchbyreg.Text).Trim();
            string batch_year = Convert.ToString(lbl_batch.Text).Trim();
            string degree_code = Convert.ToString(lblDegCode.Text).Trim();
            string query = string.Empty;
            string exam_code = string.Empty;
            string month = Convert.ToString(ddl_mm.SelectedItem.Value).Trim();
            string year = Convert.ToString(ddl_yy.SelectedItem.Text).Trim();
            string yr_val = Convert.ToString(ddl_yy.SelectedItem.Value).Trim();
            FpBefore.Visible = false;
            btnSave.Visible = false;
            DataSet dsMark = new DataSet();
            if (month.Trim() == "0" || yr_val.Trim() == "0")
            {
                lbl_alert1.Text = "Please Select Month and Year";
                imgdiv2.Visible = true;
                return;
            }
            if (txt_searchbyreg.Text == null || Convert.ToString(txt_searchbyreg.Text).Trim() == "")
            {
                lbl_alert1.Text = "Please Enter Register No";
                imgdiv2.Visible = true;
                return;
            }
            if (rdb_take.Checked)
            {
                //exam_code = d2.GetFunctionv("select exam_code from Exam_Details where batch_year='" + batch_year + "' and degree_code='" + degree_code + "' and Exam_year='" + year + "' and Exam_Month='" + month + "'");
                if (roll_no.Trim() != "")
                {
                    query = " select internal_mark,external_mark,total,s.subject_name,s.subject_code,S.subject_no,s.min_int_marks,s.max_int_marks,s.min_ext_marks,s.max_ext_marks,s.mintotal,s.maxtotal,m.grade,M.Actual_Grade from exam_application e,exam_appl_details ea,mark_entry M,subject s where e.appl_no =ea.appl_no and m.subject_no =ea.subject_no and e.roll_no =m.roll_no and s.subject_no =ea.subject_no and s.subject_no = m.subject_no and e.roll_no ='" + roll_no + "' and Exam_type='6' order by S.subject_no,m.total desc; ";
                    //query = "select max(internal_mark) as internal_mark,external_mark,s.subject_name,s.subject_code,S.subject_no,s.max_ext_marks,s.max_int_marks,s.mintotal from exam_application e,exam_appl_details ea,mark_entry M,subject s where e.appl_no =ea.appl_no and m.subject_no =ea.subject_no and e.roll_no =m.roll_no and s.subject_no =ea.subject_no and s.subject_no = m.subject_no and e.roll_no ='" + roll_no + "' and Exam_type =6 group by external_mark,s.subject_name,s.subject_code,S.subject_no,s.max_ext_marks,s.max_int_marks,s.mintotal order by external_mark desc";
                    dsMark = d2.select_method_wo_parameter(query, "Text");
                    if (dsMark.Tables.Count > 0 && dsMark.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtSubjects = new DataTable();
                        dtSubjects = dsMark.Tables[0].DefaultView.ToTable(true, "subject_no");
                        FpBefore.Visible = true;
                        FpBefore.Sheets[0].AutoPostBack = false;
                        FpBefore.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                        FpBefore.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                        FpBefore.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                        FpBefore.ActiveSheetView.RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
                        FpBefore.ActiveSheetView.RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                        FpBefore.ActiveSheetView.RowHeader.DefaultStyle.Font.Bold = true;
                        FpBefore.ActiveSheetView.DefaultStyle.Font.Name = "Book Antiqua";
                        FpBefore.ActiveSheetView.DefaultStyle.Font.Size = FontUnit.Medium;
                        FpBefore.Sheets[0].RowHeader.Visible = false;
                        FpBefore.Sheets[0].ColumnHeader.RowCount = 0;
                        FpBefore.Sheets[0].ColumnCount = 0;
                        FpBefore.Sheets[0].RowCount = 0;
                        FpBefore.Width = 800;

                        FpBefore.Sheets[0].RowHeader.Visible = false;
                        //Fpspread1.Sheets[0].AutoPostBack = true;
                        FpBefore.CommandBar.Visible = false;

                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.Black;
                        darkstyle.Font.Name = "Book Antiqua";
                        darkstyle.Font.Size = FontUnit.Medium;
                        darkstyle.Font.Bold = true;
                        darkstyle.Border.BorderSize = 0;
                        darkstyle.HorizontalAlign = HorizontalAlign.Center;
                        darkstyle.VerticalAlign = VerticalAlign.Middle;
                        darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;
                        FpBefore.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        //doubletype.MaximumValue = 100;
                        //chktypeall.AutoPostBack = true;
                        //FarPoint.Web.Spread.CheckBoxCellType chktype = new FarPoint.Web.Spread.CheckBoxCellType();
                        FarPoint.Web.Spread.StyleInfo darkstyle1 = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle1.Font.Name = "Book Antiqua";
                        darkstyle1.Font.Size = FontUnit.Medium;

                        FpBefore.Sheets[0].DefaultStyle = darkstyle1;
                        FpBefore.Sheets[0].RowCount = 0;
                        FpBefore.Sheets[0].ColumnCount = 7;
                        FpBefore.Sheets[0].ColumnHeader.RowCount = 1;
                        FpBefore.Sheets[0].ColumnHeader.Columns[0].Width = 50;
                        FpBefore.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpBefore.Columns[0].Locked = true;
                        FpBefore.Columns[0].Width = 75;

                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Subject Code";
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpBefore.Columns[1].Width = 150;
                        FpBefore.Columns[1].Locked = true;

                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Name";
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpBefore.Columns[2].Locked = true;
                        FpBefore.Columns[2].Width = 300;

                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Internal Mark";
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpBefore.Columns[3].Locked = true;
                        FpBefore.Columns[3].Width = 80;

                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 4].Text = "External Mark";
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpBefore.Columns[4].Locked = true;
                        FpBefore.Columns[4].Width = 80;

                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Total Mark";
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpBefore.Columns[5].Locked = true;
                        FpBefore.Columns[5].Width = 80;

                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Re-Take Internal Marks ";
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;

                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        //FpBefore.Columns[6].Locked = true;
                        FpBefore.Columns[6].Width = 100;

                        btnSave.Visible = true;
                        // FpBefore.Sheets[0].RowCount = dsMark.Tables[0].Rows.Count + 1;
                        //FpBefore.Sheets[0].Cells[0, 3].CellType = chktypeall;
                        //FpBefore.Sheets[0].Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        //FpBefore.Sheets[0].Cells[0, 3].VerticalAlign = VerticalAlign.Middle;
                        FpBefore.SaveChanges();
                        if (dtSubjects.Rows.Count > 0)
                        {
                            int r = 0;
                            foreach (DataRow drSubjects in dtSubjects.Rows)
                            {
                                string subjectNo = Convert.ToString(drSubjects["subject_no"]).Trim();
                                dsMark.Tables[0].DefaultView.RowFilter = "subject_no='" + subjectNo + "'";
                                DataView dvMarksList = dsMark.Tables[0].DefaultView;
                                dvMarksList.Sort = "internal_mark desc";
                                if (dvMarksList.Count > 0)
                                {
                                    double internalMarks = 0;
                                    double externalMarks = 0;
                                    double totalMarks = 0;
                                    double reValuationInternalMarks = 0;

                                    double intern = 0;
                                    ////List<double> lstInternal = (from x in dvMarksList.ToTable().AsEnumerable().Select(x => x.Field<double>("internal_mark"))).ToList();

                                    ////List<double> lstInternal = (from DataRow row in dvMarksList.ToTable().AsEnumerable().Select(row => row.Field<double>("internal_mark"))).ToList(); 
                                    //List<double> lstInternal = (from row in dvMarksList.ToTable().AsEnumerable() select row.Field<double>("internal_mark")).ToList();
                                    //List<double> lstExternal = (from row in dvMarksList.ToTable().AsEnumerable() select row.Field<double>("external_mark")).ToList();
                                    //List<double> lstTotal = (from row in dvMarksList.ToTable().AsEnumerable() select row.Field<double>("total")).ToList();
                                    //double tempInt = lstInternal.ToArray().Max();
                                    //double tempExt = lstExternal.ToArray().Max();
                                    //double tempTotal = lstTotal.ToArray().Max();
                                    ////List<double> lstInternalq = (from row in dvMarksList.ToTable().AsEnumerable() where Convert.ToString(row.Field<double>("internal_mark"))!="" select row.Field<double>("internal_mark")).ToList();tempInt
                                    ////List<System.Decimal> lstExternal = dvMarksList.ToTable().AsEnumerable().Select(ro => ro.Field<System.Decimal>("external_mark")).ToList();where r.Field<int>("ID") == 0

                                    double.TryParse(Convert.ToString(dvMarksList[0]["internal_mark"]).Trim(), out internalMarks);
                                    double.TryParse(Convert.ToString(dvMarksList[0]["external_mark"]).Trim(), out externalMarks);
                                    double.TryParse(Convert.ToString(dvMarksList[0]["total"]).Trim(), out totalMarks);

                                    //double.TryParse(Convert.ToString(dvMarksList[0]["internal_mark"]).Trim(), out internalMarks);
                                    FpBefore.Sheets[0].RowCount++;
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(r + 1);
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    //FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dvMarksList[0]["subject_code"]).Trim();
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dvMarksList[0]["subject_no"]).Trim();
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    //FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dvMarksList[0]["subject_name"]).Trim();
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                    //FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dvMarksList[0]["internal_mark"]).Trim();
                                    //FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(dvMarksList[0]["internal_mark"]).Trim();
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(dvMarksList[0]["min_int_marks"]).Trim();
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].Note = Convert.ToString(dvMarksList[0]["max_int_marks"]).Trim();
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                    //FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dvMarksList[0]["external_mark"]).Trim();
                                    //FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(dvMarksList[0]["external_mark"]).Trim();
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(dvMarksList[0]["min_ext_marks"]).Trim();
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].Note = Convert.ToString(dvMarksList[0]["max_ext_marks"]).Trim();
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                    //FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dvMarksList[0]["total"]).Trim();
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                    //FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                                    FpBefore.Visible = true;

                                    //doubletype
                                    FarPoint.Web.Spread.DoubleCellType doubletype = new FarPoint.Web.Spread.DoubleCellType();
                                    double max_int = 0;
                                    double.TryParse(Convert.ToString(dvMarksList[0]["max_int_marks"]).Trim(), out max_int);
                                    //if (max_int == 0)
                                    //{
                                    //    max_int = 100;
                                    //}
                                    //FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(dvMarksList[0]["mintotal"]).Trim();
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(dvMarksList[0]["mintotal"]).Trim();
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 6].Note = Convert.ToString(dvMarksList[0]["maxtotal"]).Trim();
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                                    //doubletype.MaximumValue = max_int;
                                    //doubletype.ErrorMessage = "Enter Valid Mark";
                                    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 6].CellType = doubletype;
                                    r++;
                                }
                            }
                        }

                        //for (int r = 0; r < dsMark.Tables[0].Rows.Count; r++)
                        //{
                        //    //subject_code,subject_name,s.subject_no
                        //    FpBefore.Sheets[0].RowCount++;
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(r + 1);
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dsMark.Tables[0].Rows[r]["subject_code"]);
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dsMark.Tables[0].Rows[r]["subject_no"]);
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dsMark.Tables[0].Rows[r]["subject_name"]);
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dsMark.Tables[0].Rows[r]["internal_mark"]);
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dsMark.Tables[0].Rows[r]["external_mark"]);
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dsMark.Tables[0].Rows[r]["total"]);
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                        //    FpBefore.Visible = true;

                        //    //doubletype
                        //    double max_int = 0;
                        //    double.TryParse(Convert.ToString(dsMark.Tables[0].Rows[r]["max_int_marks"]), out max_int);
                        //    if (max_int == 0)
                        //    {
                        //        max_int = 100;
                        //    }
                        //    //mintotal
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(dsMark.Tables[0].Rows[r]["mintotal"]);
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                        //    FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 6].CellType = doubletype;
                        //    doubletype.MaximumValue = max_int;
                        //    doubletype.ErrorMessage = "Enter Valid Mark";
                        //}
                        FpBefore.Sheets[0].PageSize = FpBefore.Sheets[0].RowCount;
                        FpBefore.Height = (FpBefore.Sheets[0].RowCount * 60) + 65;
                        FpBefore.Width = 900;
                        if ((FpBefore.Sheets[0].RowCount * 60) + 65 <= 350)
                        {
                            FpBefore.Height = 400;
                        }
                        FpBefore.SaveChanges();
                    }
                    else
                    {
                        lbl_alert1.Text = "No Re_Take Request are Found";
                        imgdiv2.Visible = true;
                        return;
                    }
                }
                else
                {
                    lbl_alert1.Text = "No Exam are Found";
                    imgdiv2.Visible = true;
                    return;
                }
            }
            else if (rdb_tot.Checked)
            {
                exam_code = d2.GetFunctionv("select exam_code from Exam_Details where batch_year='" + batch_year + "' and degree_code='" + degree_code + "' and Exam_year='" + year + "' and Exam_Month='" + month + "'");
                if (exam_code.Trim() != "" && roll_no.Trim() != "")
                {
                    query = "select m.internal_mark,m.external_mark,m.total,s.subject_name,s.subject_code,S.subject_no,s.min_int_marks,s.max_int_marks,s.min_ext_marks,s.max_ext_marks,s.mintotal,s.maxtotal,m.grade,m.Act_Reval_Mark,m.Actual_Grade from mark_entry M,Exam_Details E,subject S,exam_application Ea,exam_appl_details Ed where  M.exam_code =E.exam_code and E.exam_code =ea.exam_code and ea.appl_no =ed.appl_no and ea.roll_no =m.roll_no  and s.subject_no =M.subject_no and s.subject_no =ed.subject_no and ed.subject_no =M.subject_no and Exam_type ='2' and M.exam_code ='" + exam_code + "' and M.roll_no ='" + roll_no + "' order by S.subject_no";  //modified by mullai
                    dsMark = d2.select_method_wo_parameter(query, "Text");                 
                    if (dsMark.Tables.Count > 0 && dsMark.Tables[0].Rows.Count > 0)
                    {
                        FpBefore.Visible = true;
                        FpBefore.Sheets[0].AutoPostBack = false;
                        FpBefore.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                        FpBefore.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                        FpBefore.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                        FpBefore.ActiveSheetView.RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
                        FpBefore.ActiveSheetView.RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                        FpBefore.ActiveSheetView.RowHeader.DefaultStyle.Font.Bold = true;
                        FpBefore.ActiveSheetView.DefaultStyle.Font.Name = "Book Antiqua";
                        FpBefore.ActiveSheetView.DefaultStyle.Font.Size = FontUnit.Medium;
                        FpBefore.Sheets[0].RowHeader.Visible = false;
                        FpBefore.Sheets[0].ColumnHeader.RowCount = 0;
                        FpBefore.Sheets[0].ColumnCount = 0;
                        FpBefore.Sheets[0].RowCount = 0;
                        FpBefore.Width = 800;

                        FpBefore.Sheets[0].RowHeader.Visible = false;
                        //Fpspread1.Sheets[0].AutoPostBack = true;
                        FpBefore.CommandBar.Visible = false;

                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.Black;
                        darkstyle.Font.Name = "Book Antiqua";
                        darkstyle.Font.Size = FontUnit.Medium;
                        darkstyle.Font.Bold = true;
                        darkstyle.Border.BorderSize = 0;
                        darkstyle.HorizontalAlign = HorizontalAlign.Center;
                        darkstyle.VerticalAlign = VerticalAlign.Middle;
                        darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;
                        FpBefore.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        //doubletype.MaximumValue = 100;
                        //chktypeall.AutoPostBack = true;
                        //FarPoint.Web.Spread.CheckBoxCellType chktype = new FarPoint.Web.Spread.CheckBoxCellType();
                        FarPoint.Web.Spread.StyleInfo darkstyle1 = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle1.Font.Name = "Book Antiqua";
                        darkstyle1.Font.Size = FontUnit.Medium;

                        FpBefore.Sheets[0].DefaultStyle = darkstyle1;
                        FpBefore.Sheets[0].RowCount = 0;
                       
                        FpBefore.Sheets[0].ColumnHeader.RowCount = 1;
                      

                       

                        if ((dsMark.Tables[0].Rows[0]["internal_mark"].ToString().Trim() == "" || dsMark.Tables[0].Rows[0]["internal_mark"].ToString().Trim() == "0") && (Convert.ToString(dsMark.Tables[0].Rows[0]["external_mark"]).Trim() == "" || Convert.ToString(dsMark.Tables[0].Rows[0]["external_mark"]).Trim() == "0") && (Convert.ToString(dsMark.Tables[0].Rows[0]["total"]).Trim() == "" || Convert.ToString(dsMark.Tables[0].Rows[0]["total"]).Trim() == "0"))
                        {
                            FpBefore.Sheets[0].ColumnCount = 5;
                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Grade";
                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                            FpBefore.Columns[3].Locked = true;
                            FpBefore.Columns[3].Width = 80;

                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Revaluation Grade ";
                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                            FpBefore.Columns[4].Width = 90;
                            gradeflag1 = true;
                        }
                        else
                        {
                            FpBefore.Sheets[0].ColumnCount = 7;
                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Internal Mark";
                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                            FpBefore.Columns[3].Locked = true;
                            FpBefore.Columns[3].Width = 80;

                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 4].Text = "External Mark";
                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                            FpBefore.Columns[4].Locked = true;
                            FpBefore.Columns[4].Width = 80;

                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Total Mark";
                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                            FpBefore.Columns[5].Locked = true;
                            FpBefore.Columns[5].Width = 80;

                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Revaluation Marks ";
                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;

                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                            FpBefore.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                            FpBefore.Columns[6].Width = 90;
                        }
                        //FpBefore.Columns[6].Locked = true;
                      

                        FpBefore.Sheets[0].ColumnHeader.Columns[0].Width = 50;
                        FpBefore.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpBefore.Columns[0].Locked = true;
                        FpBefore.Columns[0].Width = 75;

                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Subject Code";
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpBefore.Columns[1].Width = 150;
                        FpBefore.Columns[1].Locked = true;

                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Name";
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpBefore.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpBefore.Columns[2].Locked = true;
                        FpBefore.Columns[2].Width = 300;

                        btnSave.Visible = true;
                        // FpBefore.Sheets[0].RowCount = dsMark.Tables[0].Rows.Count + 1;
                        //FpBefore.Sheets[0].Cells[0, 3].CellType = chktypeall;
                        //FpBefore.Sheets[0].Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        //FpBefore.Sheets[0].Cells[0, 3].VerticalAlign = VerticalAlign.Middle;
                        FpBefore.SaveChanges();
                        for (int r = 0; r < dsMark.Tables[0].Rows.Count; r++)
                        {
                            //subject_code,subject_name,s.subject_no
                            FpBefore.Sheets[0].RowCount++;
                            FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(r + 1);
                            FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                            FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dsMark.Tables[0].Rows[r]["subject_code"]).Trim();
                            FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dsMark.Tables[0].Rows[r]["subject_no"]).Trim();
                            FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                            FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                            FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dsMark.Tables[0].Rows[r]["subject_name"]).Trim();
                            FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                            FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                            FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dsMark.Tables[0].Rows[r]["internal_mark"]).Trim();
                            if ((dsMark.Tables[0].Rows[r]["internal_mark"].ToString().Trim() == "" || dsMark.Tables[0].Rows[r]["internal_mark"].ToString().Trim() == "0") && (Convert.ToString(dsMark.Tables[0].Rows[r]["external_mark"]).Trim() == "" || Convert.ToString(dsMark.Tables[0].Rows[r]["external_mark"]).Trim() == "0") && (Convert.ToString(dsMark.Tables[0].Rows[r]["total"]).Trim() == "" || Convert.ToString(dsMark.Tables[0].Rows[r]["total"]).Trim() == "0"))
                            {
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dsMark.Tables[0].Rows[r]["grade"]).Trim();
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;


                                //FarPoint.Web.Spread.DoubleCellType doubletype = new FarPoint.Web.Spread.DoubleCellType();
                               


                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dsMark.Tables[0].Rows[r]["Actual_Grade"]).Trim();  
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                               // FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].CellType = doubletype;
                               
                            }
                            else
                            {
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(dsMark.Tables[0].Rows[r]["min_int_marks"]).Trim();
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].Note = Convert.ToString(dsMark.Tables[0].Rows[r]["max_int_marks"]).Trim();
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;


                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dsMark.Tables[0].Rows[r]["external_mark"]).Trim();
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(dsMark.Tables[0].Rows[r]["min_ext_marks"]).Trim();
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].Note = Convert.ToString(dsMark.Tables[0].Rows[r]["max_ext_marks"]).Trim();
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dsMark.Tables[0].Rows[r]["total"]).Trim();
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(dsMark.Tables[0].Rows[r]["grade"]).Trim();
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;


                                FarPoint.Web.Spread.DoubleCellType doubletype = new FarPoint.Web.Spread.DoubleCellType();
                                double max_ext = 0;
                                double.TryParse(Convert.ToString(dsMark.Tables[0].Rows[r]["max_ext_marks"]).Trim(), out max_ext);
                                if (max_ext == 0)
                                {
                                    max_ext = 100;
                                }

                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(dsMark.Tables[0].Rows[r]["mintotal"]).Trim();
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 6].Note = Convert.ToString(dsMark.Tables[0].Rows[r]["maxtotal"]).Trim();
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dsMark.Tables[0].Rows[r]["Act_Reval_Mark"]).Trim();  // added by mullai
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(dsMark.Tables[0].Rows[r]["Actual_Grade"]).Trim();
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                                FpBefore.Sheets[0].Cells[FpBefore.Sheets[0].RowCount - 1, 6].CellType = doubletype;

                            }
                            FpBefore.Visible = true;

                           
                            //doubletype.MaximumValue = max_ext;
                            //doubletype.ErrorMessage = "Enter Valid Mark";

                            //string regularNewRaja = @"^(AB)?$|^(Ab)?$|^(aB)?$|^(ab)?$|^(a)?$|^(A)?$|^(M)?$|^(m)?$|^(lt)?$|^(LT)?$|^(Lt)?$|^(lT)?$|^(nr)?$|^(Nr)?$|^(nR)?$|^(NR)?$|^(NE)?$|^(nE)?$|^(Ne)?$|^(ne)?$|^(RA)?$|^(rA)?$|^(Ra)?$|^(ra)?$";
                            //string regexpree = "AB|ab||NR|nr|NE|ne|ra||RA|a|A|M|m|LT|lt|00|01|02|03|04|05|06|07|08|09|";
                            //string newExapressionRaja = string.Empty;
                            //string roundValuesRaja = "1,2";
                            //for (int i = 0; i <= Convert.ToInt32(max_ext); i++)
                            //{
                            //    regexpree = regexpree + "|" + "" + i + "";
                            //    if (i != Convert.ToInt32(max_ext))
                            //    {
                            //        newExapressionRaja += @"|" + "^(" + i + ")(\\.[0-9]{" + roundValuesRaja + "})?$";
                            //        for (int d = 0; d < 100; d++)
                            //        {
                            //            regexpree = regexpree + "|" + "" + i + "." + d;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        newExapressionRaja += @"|" + "^(" + i + ")(\\.[0]{" + roundValuesRaja + "})?$";
                            //    }
                            //}
                            //rgex.ValidationExpression = regularNewRaja + newExapressionRaja;
                            //rgex.ErrorMessage = "Please Enter the Mark Less Than or Equal to (" + max_ext + ")";

                            

                        }
                        FpBefore.Sheets[0].PageSize = FpBefore.Sheets[0].RowCount;
                        FpBefore.Height = (dsMark.Tables[0].Rows.Count * 60) + 60;
                        FpBefore.Width = 900;
                        FpBefore.SaveChanges();
                    }
                    else
                    {
                        lbl_alert1.Text = "No Revaluation Request are Found";
                        imgdiv2.Visible = true;
                        return;
                    }
                }
                else
                {
                    lbl_alert1.Text = "No Exam are Found";
                    imgdiv2.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    /// <summary>
    /// Developed By Malang Raja T
    /// </summary>
    /// <param name="visible">bool True or False</param>
    /// <param name="c">Set of Controls passed as Params to set visibility </param>
    public void ControlVisibility(bool visible, params Control[] c)
    {
        int len = c.Length;
        for (int i = 0; i < len; i++)
        {
            c[i].Visible = visible;
        }
    }

    protected void rdb_tot_CheckedChanged(object sender, EventArgs e)
    {
        btnSave.Visible = false;
        FpBefore.Visible = false;
    }

    protected void rdb_take_CheckedChanged(object sender, EventArgs e)
    {
        btnSave.Visible = false;
        FpBefore.Visible = false;
    }

    private bool ShowDummyNumber()
    {
        bool retval = false;
        string saveDummy = da.GetFunction("select LinkValue from New_InsSettings where LinkName='ShowDummyNumberOnMarkEntryCOE'  and user_code ='" + usercode + "'  ").Trim();
        if (saveDummy == "1")
        {
            retval = true;
        }
        return retval;
    }

    private byte DummyNumberType()
    {
        byte retval = 0;//0-common , 1- subjectwise
        string typeDummy = da.GetFunction("select LinkValue from New_InsSettings where LinkName='DummyNumberTypeOnMarkEntryCOE'  and user_code ='" + usercode + "'  ").Trim();
        if (typeDummy == "1")
        {
            retval = 1;
        }
        return retval;
    }

    private byte getDummyNumberMode()
    {
        byte retval = 0;//0-Serial , 1- Random
        string modeDummy = da.GetFunction("select LinkValue from New_InsSettings where LinkName='DummyNumberModeOnMarkEntryCOE'  and user_code ='" + usercode + "'  ").Trim();
        if (modeDummy == "1")
        {
            retval = 1;
        }
        return retval;
    }

}