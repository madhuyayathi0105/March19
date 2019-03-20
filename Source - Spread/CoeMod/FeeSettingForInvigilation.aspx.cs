using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using InsproDataAccess;
using System.Configuration;

public partial class CoeMod_FeeSettingForInvigilation : System.Web.UI.Page
{
    static string examMonth = string.Empty;
    static string examyear = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string grouporusercode = string.Empty;
    string CollegeCode = string.Empty;
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataSet dsload = new DataSet();
    string selectQuery = string.Empty;
    Hashtable hat = new Hashtable();
    InsproDirectAccess dirAccess = new InsproDirectAccess();
    DataTable dtFees = new DataTable();
    DataRow drCurrentRow;

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
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            CollegeCode = Session["collegecode"].ToString();
            if (!IsPostBack)
            {
                Bindcollege();
                year1();
                month1();
                degree();
                bindbranch1();
                BindBatchYear();
                subjecttypebind();
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
            string strUser = da.getUserCode(Convert.ToString(Session["group_code"]), Convert.ToString(Session["usercode"]), 1);
            ds.Clear();
            ddlCollege.Items.Clear();
            selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where " + strUser + " and cp.college_code=cf.college_code";
            ds = da.select_method_wo_parameter(selectQuery, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlCollege.DataSource = ds;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
            }
        }
        catch (Exception ex) { }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {

        }
    }

    public void year1()
    {
        CollegeCode = Convert.ToString(ddlCollege.SelectedValue);
        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {

            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            grouporusercode = " and group_code='" + group_user + "'";
        }
        else
        {
            grouporusercode = " and usercode='" + Session["usercode"].ToString().Trim() + "'";
        }
        Boolean setflag = false;
        ddlYear1.Items.Clear();
        string settings = "Exam year and month for Mark" + CollegeCode;
        string getexamvalue = da.GetFunction("select value from master_settings where settings='" + settings.Trim() + "' " + grouporusercode + "");
        if (getexamvalue.Trim() != null && getexamvalue.Trim() != "" && getexamvalue.Trim() != "0")
        {
            string[] spe = getexamvalue.Split(',');
            if (spe.GetUpperBound(0) == 1)
            {
                if (spe[0].Trim() != "0")
                {
                    ddlYear1.Items.Add(new ListItem(Convert.ToString(spe[0]), Convert.ToString(spe[0])));
                    setflag = true;
                }
            }
        }
        if (setflag == false)
        {
            ds = da.select_method_wo_parameter(" select distinct Exam_year from exam_details order by Exam_year desc", "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlYear1.DataSource = ds;
                ddlYear1.DataTextField = "Exam_year";
                ddlYear1.DataValueField = "Exam_year";
                ddlYear1.DataBind();

                //if (cbl_ExamYear.Items.Count > 0)
                //{
                //    for (int i = 0; i < cbl_ExamYear.Items.Count; i++)
                //    {
                //        cbl_ExamYear.Items[i].Selected = true;
                //    }
                //    txt_ExamYear.Text = "Exam Year(" + cbl_ExamYear.Items.Count + ")";
                //    cb_ExamYear.Checked = true;
                //}
            }
        }
        //cbl_ExamYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
    }

    protected void ddlYear1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            month1();
            examyear = Convert.ToString(ddlYear1.SelectedItem);
        }
        catch (Exception ex)
        {
        }
    }

    protected void month1()
    {
        try
        {
            CollegeCode = Convert.ToString(ddlCollege.SelectedValue);
            ddlMonth1.Items.Clear();
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {

                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                grouporusercode = " and group_code='" + group_user + "'";
            }
            else
            {
                grouporusercode = " and usercode='" + Session["usercode"].ToString().Trim() + "'";
            }
            Boolean setflag = false;
            string monthval = string.Empty;
            string settings = "Exam year and month for Mark" + CollegeCode;
            string getexamvalue = da.GetFunction("select value from master_settings where settings='" + settings.Trim() + "' " + grouporusercode + "");
            if (getexamvalue.Trim() != null && getexamvalue.Trim() != "" && getexamvalue.Trim() != "0")
            {
                string[] spe = getexamvalue.Split(',');
                if (spe.GetUpperBound(0) == 1)
                {
                    if (spe[1].Trim() != "0")
                    {
                        string val = spe[1].ToString();
                        monthval = " and Exam_month='" + val + "'";
                    }
                }
            }
            ds.Clear();
            string year1 = Convert.ToString(ddlYear1.SelectedValue);
            string strsql = "select distinct Exam_month,upper(convert(varchar(3),DateAdd(month,Exam_month,-1))) as monthName from exam_details where Exam_year in('" + year1 + "') " + monthval + " order by Exam_month desc";
            ds = da.select_method_wo_parameter(strsql, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlMonth1.DataSource = ds;
                ddlMonth1.DataTextField = "monthName";
                ddlMonth1.DataValueField = "Exam_month";
                ddlMonth1.DataBind();
            }
            subjecttypebind();
            degree();
            bindbranch1();
            BindBatchYear();
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlMonth1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            examMonth = Convert.ToString(ddlMonth1.SelectedItem);
            subjecttypebind();
        }
        catch (Exception ex)
        {
        }
    }

    public void degree()
    {
        try
        {
            cbl_Degree.Items.Clear();
            string usercode = Session["usercode"].ToString();
            string collegecode = Convert.ToString(ddlCollege.SelectedValue);
            string singleuser = Session["single_user"].ToString();
            string group_user = Session["group_code"].ToString();

            string codevalues = string.Empty;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                codevalues = "and group_code='" + group_user + "'";
            }
            else
            {
                codevalues = "and user_code='" + usercode + "'";
            }
            string strquery = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where  course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code='" + collegecode + "' and deptprivilages.Degree_code=degree.Degree_code " + codevalues + " ";
            ds = da.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cbl_Degree.DataSource = ds;
                cbl_Degree.DataTextField = "course_name";
                cbl_Degree.DataValueField = "course_id";
                cbl_Degree.DataBind();
                //    if (cbl_Degree.Items.Count > 0)
                //    {
                //        for (int i = 0; i < cbl_Degree.Items.Count; i++)
                //        {
                //            cbl_Degree.Items[i].Selected = true;
                //        }
                //        txt_Degree.Text = "Degree(" + cbl_Degree.Items.Count + ")";
                //        cb_Degree.Checked = true;
                //    }
            }
            bindbranch1();
            //cbl_Degree.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
        }
        catch (Exception ex)
        {
        }
    }

    protected void cb_Degree_checkedchange(object sender, EventArgs e)
    {
        bindbranch1();
        CallCheckboxChange(cb_Degree, cbl_Degree, txt_Degree, LblDegree.Text, "--Select--");
    }

    protected void cbl_Degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch1();
        CallCheckboxListChange(cb_Degree, cbl_Degree, txt_Degree, LblDegree.Text, "--Select--");
    }

    public void bindbranch1()
    {
        try
        {
            cbl_Dept.Items.Clear();
            hat.Clear();
            string usercode = Session["usercode"].ToString();
            string collegecode = Convert.ToString(ddlCollege.SelectedValue);
            string singleuser = Session["single_user"].ToString();
            string group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }

            string degree = "";
            for (int i = 0; i < cbl_Degree.Items.Count; i++)
            {
                if (cbl_Degree.Items[i].Selected == true)
                {
                    if (degree == "")
                    {
                        degree = Convert.ToString(cbl_Degree.Items[i].Value);
                    }
                    else
                    {
                        degree += "," + Convert.ToString(cbl_Degree.Items[i].Value);
                    }
                }

            }
            ds = da.BindBranchMultiple(singleuser, group_user, degree, collegecode, usercode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count2 = ds.Tables[0].Rows.Count;
                if (count2 > 0)
                {
                    cbl_Dept.DataSource = ds;
                    cbl_Dept.DataTextField = "dept_name";
                    cbl_Dept.DataValueField = "degree_code";
                    cbl_Dept.DataBind();
                    if (cbl_Dept.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_Dept.Items.Count; i++)
                        {
                            cbl_Dept.Items[i].Selected = true;
                        }
                        txt_Dept.Text = "Department(" + cbl_Dept.Items.Count + ")";
                        cb_Dept.Checked = true;
                    }
                }
            }
            // cbl_Dept.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
        }
        catch (Exception ex)
        {
        }
    }

    protected void cb_Dept_checkedchange(object sender, EventArgs e)
    {
        subjecttypebind();
        CallCheckboxChange(cb_Dept, cbl_Dept, txt_Dept, LblDept.Text, "--Select--");
    }

    protected void cbl_Dept_SelectedIndexChanged(object sender, EventArgs e)
    {
        subjecttypebind();
        CallCheckboxListChange(cb_Dept, cbl_Dept, txt_Dept, LblDept.Text, "--Select--");
    }

    protected void BindBatchYear()
    {
        string qry = " select distinct Batch_Year from Registration order by batch_year desc";
        DataTable dtbatchyr = dirAccess.selectDataTable(qry);
        cbl_BatchYear.Items.Clear();
        if (dtbatchyr.Rows.Count > 0)
        {
            cbl_BatchYear.DataSource = dtbatchyr;
            cbl_BatchYear.DataTextField = "Batch_Year";
            cbl_BatchYear.DataValueField = "Batch_Year";
            cbl_BatchYear.DataBind();

            //if (cbl_BatchYear.Items.Count > 0)
            //{
            //    for (int i = 0; i < cbl_BatchYear.Items.Count; i++)
            //    {
            //        cbl_BatchYear.Items[i].Selected = true;
            //    }
            //    txt_BatchYear.Text = "Batch(" + cbl_BatchYear.Items.Count + ")";
            //    cb_BatchYear.Checked = true;
            //}
        }
    }

    protected void cb_BatchYear_checkedchange(object sender, EventArgs e)
    {
        subjecttypebind();
        CallCheckboxChange(cb_BatchYear, cbl_BatchYear, txt_BatchYear, lblBatchYr.Text, "--Select--");
    }

    protected void cbl_BatchYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        subjecttypebind();
        CallCheckboxListChange(cb_BatchYear, cbl_BatchYear, txt_BatchYear, lblBatchYr.Text, "--Select--");
    }

    protected void subjecttypebind()
    {
        try
        {
            cbl_SubType.Items.Clear();
            ds.Clear();
            string month = Convert.ToString(ddlMonth1.SelectedValue);
            string year = Convert.ToString(ddlYear1.SelectedValue);
            string batch = getCblSelectedValue(cb_BatchYear, cbl_BatchYear);
            string degCode = getCblSelectedValue(cb_Dept, cbl_Dept);
            string selBranch = "select distinct ss.subject_type from syllabus_master sy,sub_sem ss,subject s where sy.syll_code=sy.syll_code and sy.syll_code=s.syll_code and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and ss.promote_count=1 and  sy.Batch_Year='" + batch + "' and sy.degree_code in('" + degCode + "')  order by ss.subject_type";//sub_sem.syll_Code = subject.syll_code and
            ds = da.select_method_wo_parameter(selBranch, "Text");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cbl_SubType.DataSource = ds;
                cbl_SubType.DataTextField = "subject_type";
                cbl_SubType.DataBind();

                if (cbl_SubType.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_SubType.Items.Count; i++)
                    {
                        cbl_SubType.Items[i].Selected = true;
                    }
                    txt_Subtype.Text = "Subject Type(" + cbl_SubType.Items.Count + ")";
                    cb_SubType.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void cb_SubType_checkedchange(object sender, EventArgs e)
    {
        subjectbind();
        CallCheckboxChange(cb_SubType, cbl_SubType, txt_Subtype, lblsubtype.Text, "--Select--");
    }

    protected void cbl_SubType_SelectedIndexChanged(object sender, EventArgs e)
    {
        subjectbind();
        CallCheckboxListChange(cb_SubType, cbl_SubType, txt_Subtype, lblsubtype.Text, "--Select--");
    }

    protected void subjectbind()
    {
        try
        {
            cbl_Subject.Items.Clear();
            ds.Clear();
            string branc = getCblSelectedValue(cb_Dept, cbl_Dept);
            //string semmv = Convert.ToString(ddlsem1.SelectedValue).Trim();
            string month = Convert.ToString(ddlMonth1.SelectedValue);
            string year = Convert.ToString(ddlYear1.SelectedValue);
            string subtype = getCblSelectedValue(cb_SubType, cbl_SubType);
            string typeval = string.Empty;
            string batch = getCblSelectedValue(cb_BatchYear, cbl_BatchYear);
            string degCode = getCblSelectedValue(cb_Dept, cbl_Dept);

            string qeryss = "SELECT distinct ss.subject_type,s.subject_name,s.subject_code,s.subject_name+' - '+s.subject_code as subnamecode,s.subject_no FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,syllabus_master sy,sub_sem ss,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and  ed.Exam_Month in('" + month + "') and ed.Exam_year in('" + year + "') and ss.subject_type in('" + subtype + "')";
            qeryss = qeryss + " union SELECT distinct ss.subject_type,s.subject_name,s.subject_code,s.subject_name+' - '+s.subject_code as subnamecode,s.subject_no FROM subjectChooser sc,subject s,Registration r,Exam_Details ed,sub_sem ss,Degree d,Course c where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and sc.semester=ed.Current_Semester and s.subType_no=ss.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id  and ed.Exam_Month in('" + month + "') and ed.Exam_year in('" + year + "') and ss.subject_type in('" + subtype + "') and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar'  order by s.subject_name,s.subject_code desc";//and r.degree_code in('" + degCode + "') and r.batch_year in('" + batch + "')

            ds = da.select_method(qeryss, hat, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cbl_Subject.DataSource = ds;
                cbl_Subject.DataTextField = "subnamecode";
                cbl_Subject.DataValueField = "subject_code";
                cbl_Subject.DataBind();

                if (cbl_Subject.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_Subject.Items.Count; i++)
                    {
                        cbl_Subject.Items[i].Selected = true;
                    }
                    txt_Subject.Text = "Subject Type(" + cbl_Subject.Items.Count + ")";
                    cb_Subject.Checked = true;
                }
            }
            SubSubject();
        }
        catch (Exception ex)
        {

        }
    }

    protected void cb_Subject_checkedchange(object sender, EventArgs e)
    {
        SubSubject();
        CallCheckboxChange(cb_Subject, cbl_Subject, txt_Subject, lblsubject.Text, "--Select--");
    }

    protected void cbl_Subject_SelectedIndexChanged(object sender, EventArgs e)
    {
        SubSubject();
        CallCheckboxListChange(cb_Subject, cbl_Subject, txt_Subject, lblsubject.Text, "--Select--");
    }

    protected void SubSubject()
    {
        try
        {
            cbl_SubSubject.Items.Clear();
            string month = Convert.ToString(ddlMonth1.SelectedValue);
            string year = Convert.ToString(ddlYear1.SelectedValue);
            string valDegree = getCblSelectedValue(cb_Dept, cbl_Dept);
            // string sem = Convert.ToString(ddlsem1.SelectedValue).Trim();
            string subject = getCblSelectedValue(cb_Subject, cbl_Subject);
            string SubSubjectQ = " select ss.SubPart,ss.SubSubjectID from COESubSubjectPartSettings ss,COESubSubjectPartMater sm where ss.id=sm.id and sm.DegreeCode in('" + valDegree + "') and sm.ExamMonth in('" + month + "') and sm.ExamYear in('" + year + "') and ss.SubCode in('" + subject + "')";
            ds = da.select_method_wo_parameter(SubSubjectQ, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cbl_SubSubject.DataSource = ds;
                cbl_SubSubject.DataTextField = "SubPart";
                cbl_SubSubject.DataValueField = "SubSubjectID";
                cbl_SubSubject.DataBind();

                if (cbl_SubSubject.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_SubSubject.Items.Count; i++)
                    {
                        cbl_SubSubject.Items[i].Selected = true;
                    }
                    txt_SubSubject.Text = "Sub-Subject(" + cbl_SubSubject.Items.Count + ")";
                    cb_SubSubject.Checked = true;
                }
            }
        }
        catch (Exception ex) { }
    }

    protected void cb_SubSubject_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_SubSubject, cbl_SubSubject, txt_SubSubject, LblSubSubject.Text, "--Select--");
    }

    protected void cbl_SubSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_SubSubject, cbl_SubSubject, txt_SubSubject, LblSubSubject.Text, "--Select--");
    }

   
    protected void cb_Category_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_Category, cbl_Category, txt_Category, LblCat.Text, "--Select--");
    }

    protected void cbl_Category_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_Category, cbl_Category, txt_Category, LblCat.Text, "--Select--");
    }

    #region Common Checkbox and Checkboxlist Event

    private string getCblSelectedValue(CheckBox cb, CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedvalue = new System.Text.StringBuilder();
        try
        {
            if (cb.Checked)
            {
                for (int sel = 0; sel < cblSelected.Items.Count; sel++)
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
            else
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

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            string College = Convert.ToString(ddlCollege.SelectedValue);
            string ExamMonth = Convert.ToString(ddlMonth1.SelectedValue);
            string ExamYear = Convert.ToString(ddlYear1.SelectedValue);
            string batch = getCblSelectedValue(cb_BatchYear, cbl_BatchYear);
            string sem = getCblSelectedValue(cb_Subject, cbl_Subject); // Convert.ToString(ddlsem1.SelectedValue);
            string subject = getCblSelectedValue(cb_Subject, cbl_Subject);
            string SubSubject = getCblSelectedValue(cb_SubSubject, cbl_SubSubject);
            string Category = getCblSelectedValue(cb_Category, cbl_Category);
            string degree = getCblSelectedValue(cb_Dept, cbl_Dept);
            string examcode = "";
            string SubjectNo = "";
            string selQry = "select * from Exam_Details where batch_year in('" + batch + "') and degree_code in('" + degree + "') and Exam_Month='" + ExamMonth + "' and Exam_year='" + ExamYear + "' ";
            dsload = da.select_method_wo_parameter(selQry, "text");
            if (dsload.Tables[0].Rows.Count > 0)
            {
                examcode = Convert.ToString(dsload.Tables[0].Rows[0]["exam_code"]);
            }
            string sql = "select subject_no from subject s,syllabus_master ss where subject_code='" + subject + "'  and ss.syll_code=s.syll_code and ss.Batch_Year='" + batch + "' and ss.degree_code='" + degree + "'";

            ds = da.select_method_wo_parameter(sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                SubjectNo = Convert.ToString(ds.Tables[0].Rows[0]["subject_no"]);
            }
            sql = "select Header,Ledger,amount from InvigilationFeesSetting where ExamCode in('" + examcode + "')  and Collegecode='" + College + "'  and SubjectNo in('" + SubjectNo + "') and SubSubjectCode in('" + SubSubject + "') and Category in('" + Category + "')";
            ds = da.select_method_wo_parameter(sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    dtFees.Columns.Add("Header");
                    dtFees.Columns.Add("Amount");
                    DataRow drow = dtFees.NewRow();
                    drow["Header"] = Convert.ToString(ds.Tables[0].Rows[i]["Header"]);
                    drow["Amount"] = Convert.ToString(ds.Tables[0].Rows[i]["amount"]);
                    dtFees.Rows.Add(drow);
                }
                ViewState["CurrentTable"] = dtFees;
                grdFeesSetting.DataSource = dtFees;
                grdFeesSetting.DataBind();
                grdFeesSetting.Visible = true;
                DivFees.Visible = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnAddRow_OnClick(object sender, EventArgs e)
    {
        try
        {
            int row_cnt = grdFeesSetting.Rows.Count;
            if (row_cnt == 0)
            {
                dtFees.Columns.Add("Header");
                dtFees.Columns.Add("Amount");
                DataRow drow = dtFees.NewRow();
                drow["Header"] = "";
                drow["Amount"] = "";
                dtFees.Rows.Add(drow);
                ViewState["CurrentTable"] = dtFees;
                grdFeesSetting.DataSource = dtFees;
                grdFeesSetting.DataBind();
                grdFeesSetting.Visible = true;
                DivFees.Visible = true;
            }
            else
            {
                if (ViewState["CurrentTable"] != null)
                {
                    dtFees = (DataTable)ViewState["CurrentTable"];
                    drCurrentRow = null;
                    int TotRowCnt = Convert.ToInt32(dtFees.Rows.Count);
                    if (dtFees.Rows.Count > 0)
                    {
                        drCurrentRow = dtFees.NewRow();
                        drCurrentRow["Header"] = "";
                        drCurrentRow["Amount"] = "";
                        dtFees.Rows.Add(drCurrentRow);
                        ViewState["CurrentTable"] = dtFees;
                        grdFeesSetting.DataSource = dtFees;
                        grdFeesSetting.DataBind();
                        grdFeesSetting.Visible = true;
                        DivFees.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void grdFeesSetting_DataBound(object sender, EventArgs e)
    {
        try
        {
            string collegecode = Convert.ToString(ddlCollege.SelectedValue);
            usercode = Session["usercode"].ToString();
            (grdFeesSetting.Rows[0].FindControl("ddlHeader") as DropDownList).Items.Clear();

            if (grdFeesSetting.Rows.Count > 0)
            {
                string strHeader = string.Empty;
                string strLedger = string.Empty;
                for (int a = 0; a < grdFeesSetting.Rows.Count; a++)
                {
                    string query = "SELECT (CONVERT(nvarchar, HeaderPK)+' - '+CONVERT(nvarchar, LedgerPK)) as header ,HeaderName+' - '+LedgerName as headername,hd_priority FROM FM_HeaderMaster H,FS_HeaderPrivilage P,FM_LedgerMaster L WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = '" + usercode + "' AND H.CollegeCode = '" + collegecode + "' and h.HeaderPK=l.HeaderFK  order by len(isnull(hd_priority,10000)),hd_priority asc";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(query, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        (grdFeesSetting.Rows[a].FindControl("ddlHeader") as DropDownList).DataSource = ds;
                        (grdFeesSetting.Rows[a].FindControl("ddlHeader") as DropDownList).DataTextField = "headername";
                        (grdFeesSetting.Rows[a].FindControl("ddlHeader") as DropDownList).DataValueField = "header";
                        (grdFeesSetting.Rows[a].FindControl("ddlHeader") as DropDownList).DataBind();

                        string strcode = Convert.ToString((grdFeesSetting.Rows[a].FindControl("lbl_Header") as Label).Text);
                        if (!string.IsNullOrEmpty(strcode))
                            strHeader = strcode;
                    }
                }
            }
        }
        catch
        {
        }
    }

    protected void grdFeesSetting_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
        if (e.Row.RowIndex > 0)
        {
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
        }
    }

    protected void BtnSave_OnClick(object sender, EventArgs e)
    {
        string College = Convert.ToString(ddlCollege.SelectedValue);
        string ExamMonth = Convert.ToString(ddlMonth1.SelectedValue);
        string ExamYear = Convert.ToString(ddlYear1.SelectedValue);
        string degree = "";
        string batch = "";
        string SubSubject = "";
        string Category = "";
        int insert = 0;
        string subject = "";
        string examcode = "";
        string SubjectNo = "";


        for (int deg = 0; deg < cbl_Dept.Items.Count; deg++)
        {
            if (!cbl_Dept.Items[deg].Selected)
                continue;
            degree = Convert.ToString(cbl_Dept.Items[deg].Value);

            for (int bat = 0; bat < cbl_BatchYear.Items.Count; bat++)
            {
                if (!cbl_BatchYear.Items[bat].Selected)
                    continue;
                batch = Convert.ToString(cbl_BatchYear.Items[bat].Value);

                string selQry = "select * from Exam_Details where batch_year='" + batch + "' and degree_code='" + degree + "' and Exam_Month='" + ExamMonth + "' and Exam_year='" + ExamYear + "' ";
                dsload = da.select_method_wo_parameter(selQry, "text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    examcode = Convert.ToString(dsload.Tables[0].Rows[0]["exam_code"]);

                    for (int subno = 0; subno < cbl_Subject.Items.Count; subno++)
                    {
                        if (!cbl_Subject.Items[subno].Selected)
                            continue;
                        subject = Convert.ToString(cbl_Subject.Items[subno].Value);

                        for (int subSubj = 0; subSubj < cbl_SubSubject.Items.Count; subSubj++)
                        {
                            if (!cbl_SubSubject.Items[subSubj].Selected)
                                continue;
                            SubSubject = Convert.ToString(cbl_SubSubject.Items[subSubj].Value);

                            for (int cate = 0; cate < cbl_Category.Items.Count; cate++)
                            {
                                if (!cbl_Category.Items[cate].Selected)
                                    continue;
                                Category = Convert.ToString(cbl_Category.Items[cate].Value);

                                string sql = "select subject_no from subject s,syllabus_master ss where subject_code='" + subject + "'  and ss.syll_code=s.syll_code and ss.Batch_Year='" + batch + "' and ss.degree_code='" + degree + "'";

                                ds = da.select_method_wo_parameter(sql, "text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    SubjectNo = Convert.ToString(ds.Tables[0].Rows[0]["subject_no"]);
                                }
                                foreach (GridViewRow row in grdFeesSetting.Rows)
                                {
                                    DropDownList lblHeader = (DropDownList)row.FindControl("ddlHeader");
                                    TextBox txtAmount = (TextBox)row.FindControl("txt_Amount");
                                    string amount = Convert.ToString(txtAmount.Text.Trim());
                                    string header = lblHeader.SelectedValue;
                                    string[] legder = header.Split('-');
                                    if (!string.IsNullOrEmpty(amount))
                                    {
                                        string Sql = "if exists(select * from InvigilationFeesSetting where ExamCode='" + examcode + "' and SubjectNo='" + SubjectNo + "' and Collegecode='" + College + "' and SubSubjectCode='" + SubSubject + "' and Category='" + Category + "' and header='" + legder[0] + "' and Ledger='" + legder[1] + "') update InvigilationFeesSetting set amount='" + amount + "',Header='" + legder[0] + "',Ledger='" + legder[1] + "',category='" + Category + "' where ExamCode='" + examcode + "' and SubjectNo='" + SubjectNo + "' and Collegecode='" + College + "' and SubSubjectCode='" + SubSubject + "' and Category='" + Category + "' and header='" + legder[0] + "' and Ledger='" + legder[1] + "' else insert into InvigilationFeesSetting (ExamCode,Collegecode,SubjectNo,SubSubjectCode,Category,Header,Ledger,amount) values('" + examcode + "','" + College + "','" + SubjectNo + "','" + SubSubject + "','" + Category + "','" + legder[0] + "','" + legder[1] + "','" + amount + "')";
                                        insert = da.update_method_wo_parameter(Sql, "Text");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        if (insert > 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
        }
    }


}