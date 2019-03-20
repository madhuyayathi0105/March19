using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using System.IO;
using System.Collections;
using System.Configuration;


public partial class generaldescriptivefeedbackreport : System.Web.UI.Page
{
    bool cellclk = false;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string query = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    ReuasableMethods rs = new ReuasableMethods();
    DataTable data = new DataTable();
    DataRow drow;
    Hashtable hat = new Hashtable();


    static string grouporusercode = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["collegecode"] == null)
        //{
        //    Response.Redirect("~/Default.aspx");
        //}

        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("Feedbackhome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/FeedBackMOD/Feedbackhome.aspx");
                    return;
                }
            }

        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        collegecode1 = Session["collegecode"].ToString();
       
  
        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
        }
        else
        {
            grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
        }
        if (!IsPostBack)
        {
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Admitflag"] = "0";
            bindcollege();
            BindBatch();
            BindDegree();
            bindbranch();
            bindsem();
            bindsec();
            bindfeedback();
            string Master = "select * from Master_Settings where " + grouporusercode + "";
            DataSet ds = d2.select_method(Master, hat, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Admission No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Admitflag"] = "1";
                    }
                }
            }
        }
        }
        catch (Exception ex)
        {
            // d2.sendErrorMail(ex, collegecode1, "Feedback_anonymousisgender");
        }
    }
    public void bindcollege()
    {
        try
        {
            ds.Clear();
            Cbl_college.Items.Clear();
            string clgname = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(clgname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Cbl_college.DataSource = ds;
                Cbl_college.DataTextField = "collname";
                Cbl_college.DataValueField = "college_code";
                Cbl_college.DataBind();
            }
            if (Cbl_college.Items.Count > 0)
            {

                Cbl_college.Items[0].Selected = true;
                Cb_college.Checked = false;

                Txt_college.Text = "College(1)";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void BindBatch()
    {
        try
        {
            cbl_batch.Items.Clear();
            cb_batch.Checked = false;
            txt_batch.Text = "--Select--";
            string college_cd = rs.GetSelectedItemsValueAsString(Cbl_college);
            if (college_cd != "")
            {
                ds = d2.BindBatch();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_batch.DataSource = ds;
                    cbl_batch.DataTextField = "batch_year";
                    cbl_batch.DataValueField = "batch_year";
                    cbl_batch.DataBind();
                }
                if (cbl_batch.Items.Count > 0)
                {
                    cbl_batch.Items[0].Selected = true;

                    txt_batch.Text = "Batch(1)";
                }
                else
                {
                    txt_batch.Text = "--Select--";
                }
            }
            BindDegree();
        }
        catch (Exception ex)
        {

        }
    }

    public void BindDegree()
    {
        try
        {
            cbl_degree.Items.Clear();
            string college_cd = rs.GetSelectedItemsValueAsString(Cbl_college);
            if (college_cd.Trim() != "")
            {
                ds.Clear();
                query = "select distinct degree.course_id,course.course_name from degree,course where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code in ('" + college_cd + "')";
                ds = d2.select_method_wo_parameter(query, "Text");

                int count1 = ds.Tables[0].Rows.Count;
                if (count1 > 0)
                {
                    cbl_degree.DataSource = ds;
                    cbl_degree.DataTextField = "course_name";
                    cbl_degree.DataValueField = "course_id";
                    cbl_degree.DataBind();
                    if (cbl_degree.Items.Count > 0)
                    {

                        cbl_degree.Items[0].Selected = true;

                        txt_degree.Text = "Degree(1)";
                    }
                }
            }
            else
            {
                cb_degree.Checked = false;
                txt_degree.Text = "--Select--";
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
            cbl_branch.Items.Clear();
            string college_cd = rs.GetSelectedItemsValueAsString(Cbl_college);
            string course_id = rs.GetSelectedItemsValueAsString(cbl_degree);
            string query = "";
            if (course_id != "" && college_cd != "")
            {
                ds.Clear();
                query = " select distinct degree.degree_code,department.dept_name,department.dept_code from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + course_id + "') and degree.college_code in ('" + college_cd + "')";
                ds = d2.select_method_wo_parameter(query, "Text");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch.DataSource = ds;
                    cbl_branch.DataTextField = "dept_name";
                    cbl_branch.DataValueField = "degree_code";
                    cbl_branch.DataBind();
                    if (cbl_branch.Items.Count > 0)
                    {

                        cbl_branch.Items[0].Selected = true;

                        txt_branch.Text = "Department(1)";
                    }
                }
            }
            else
            {
                cb_branch.Checked = false;
                txt_branch.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void bindsem()
    {
        cbl_sem.Items.Clear();
        txt_sem.Text = "--Select--";
        ds.Clear();
        string branch = rs.GetSelectedItemsValueAsString(cbl_branch);
        string batch = rs.GetSelectedItemsValueAsString(cbl_batch);
        string college_cd = rs.GetSelectedItemsValueAsString(Cbl_college);
        if (branch.Trim() != "" && batch.Trim() != "")
        {
            string query = " select distinct  MAX( ndurations)as ndurations from ndegree where Degree_code in('" + branch + "') and  college_code in('" + college_cd + "') union select distinct  MAX(duration) as ndurations  from degree where Degree_Code in('" + branch + "') and college_code in('" + college_cd + "') ";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_sem.Items.Clear();
                string sem = Convert.ToString(ds.Tables[0].Rows[0]["ndurations"]);
                for (int j = 1; j <= Convert.ToInt32(sem); j++)
                {
                    cbl_sem.Items.Add(new System.Web.UI.WebControls.ListItem(j.ToString(), j.ToString()));
                    cbl_sem.Items[j - 1].Selected = true;
                    cb_sem.Checked = true;
                }
                txt_sem.Text = "Semester(" + sem + ")";
            }
        }
    }

    public void bindsec()
    {
        try
        {
            cbl_sec.Items.Clear();
            txt_sec.Text = "---Select---";
            cb_sec.Checked = false;
            string batch = rs.GetSelectedItemsValueAsString(cbl_batch);
            string branchcode1 = rs.GetSelectedItemsValueAsString(cbl_branch);
            if (batch != "" && branchcode1 != "")
            {
                ds = d2.BindSectionDetail("'" + batch + "'", "'" + branchcode1 + "'");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_sec.DataSource = ds;
                    cbl_sec.DataTextField = "sections";
                    cbl_sec.DataValueField = "sections";
                    cbl_sec.DataBind();
                    if (cbl_sec.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_sec.Items.Count; row++)
                        {
                            cbl_sec.Items[row].Selected = true;
                            cb_sec.Checked = true;
                        }
                        txt_sec.Text = "Section(" + cbl_sec.Items.Count + ")";
                    }
                }
                else
                {
                    cbl_sec.Items.Add("Empty");
                    for (int row = 0; row < cbl_sec.Items.Count; row++)
                    {
                        cbl_sec.Items[row].Selected = true;
                        cb_sec.Checked = true;
                    }
                    txt_sec.Text = "Section(" + cbl_sec.Items.Count + ")";
                }
            }
            else
            {
                cbl_sec.Items.Add("Empty");
                for (int row = 0; row < cbl_sec.Items.Count; row++)
                {
                    cbl_sec.Items[row].Selected = true;
                    cb_sec.Checked = true;
                }
                txt_sec.Text = "Section(" + cbl_sec.Items.Count + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindfeedback()
    {
        try
        {
            ds.Clear();
            string college_cd = rs.GetSelectedItemsValueAsString(Cbl_college);
            ddl_Feedbackname.Items.Clear();
            query = "select  distinct  FeedBackName from CO_FeedBackMaster where CollegeCode in ('" + college_cd + "') ";

            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            ddl_Feedbackname.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_Feedbackname.DataSource = ds;
                ddl_Feedbackname.DataTextField = "FeedBackName";
                ddl_Feedbackname.DataValueField = "FeedBackName";
                ddl_Feedbackname.DataBind();
                ddl_Feedbackname.Items.Insert(0, "Select");
            }
            else
            {
                ddl_Feedbackname.Items.Clear();
                ddl_Feedbackname.Items.Insert(0, "Select");
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
    }
    public void Cb_college_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(Cbl_college, Cb_college, Txt_college, "College");
    }
    public void Cbl_college_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(Cbl_college, Cb_college, Txt_college, "College");
    }
    public void cb_batch_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_batch, cb_batch, txt_batch, "Batch");
        BindDegree();
        bindbranch();
        bindsem();
        bindsec(); bindfeedback();
    }
    public void cbl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_batch, cb_batch, txt_batch, "Batch");
        BindDegree();
        bindbranch();
        bindsem();
        bindsec(); bindfeedback();
    }
    public void cb_degree_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_degree, cb_degree, txt_degree, "Degree");
        bindbranch();
        bindsem();
        bindsec(); bindfeedback();
    }
    public void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_degree, cb_degree, txt_degree, "Degree");
        bindbranch();
        bindsem();
        bindsec(); bindfeedback();
    }
    public void cb_branch_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_branch, cb_branch, txt_branch, "Department");
        bindsem();
        bindsec(); bindfeedback();
    }
    public void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_branch, cb_branch, txt_branch, "Department");
        bindsem();
        bindsec(); bindfeedback();
    }
    public void cb_sem_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_sem, cb_sem, txt_sem, "Semester");
    }
    public void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_sem, cb_sem, txt_sem, "Semester");
    }
    public void cb_sec_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_sec, cb_sec, txt_sec, "Section");
    }
    public void cbl_sec_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_sec, cb_sec, txt_sec, "Section");
    }
    protected void ddl_Feedbackname_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            string college_cd = string.Empty;
            string Batch_Year = string.Empty;
            string degree_code = string.Empty;
            string semester = string.Empty;
            
            DataSet newds = new DataSet();
            divMainContents.Visible = false;

            lbl_reportname.Visible = false;
            txt_excelname.Visible = false;
            btn_Excel.Visible = false;
            btn_printmaster.Visible = false;
            btnPrint.Visible = false;
            div_report.Visible = false;
            newds.Clear();
            if (ddl_Feedbackname.SelectedItem.Text.Trim() != "Select" && ddl_Feedbackname.SelectedItem.Text != "")
            {
                if (rdbgeneral.Checked == true)
                {
                    string section = string.Empty;
                    college_cd = rs.GetSelectedItemsValueAsString(Cbl_college);
                    Batch_Year = rs.GetSelectedItemsValueAsString(cbl_batch);
                    degree_code = rs.GetSelectedItemsValueAsString(cbl_branch);
                    semester = rs.GetSelectedItemsValueAsString(cbl_sem);
                    section = rs.GetSelectedItemsValueAsString(cbl_sec);

                    string fbpk = " select FeedBackMasterPK,ISNULL(issubjecttype,0)issubjecttype from CO_FeedBackMaster where FeedBackName ='" + ddl_Feedbackname.SelectedItem.Value + "'";// and DegreeCode in ('" + degreecode + "') and semester in ('" + sem + "') and Batch_Year in('" + batchyear + "') and section in ('" + sec + "')";
                    DataSet dsfb = d2.select_method_wo_parameter(fbpk, "Text");
                    string feedbakpk = string.Empty;
                    string issubjecttype = string.Empty;
                    if (dsfb.Tables.Count > 0)
                    {
                        if (dsfb.Tables[0].Rows.Count > 0)
                        {
                            issubjecttype = Convert.ToString(dsfb.Tables[0].Rows[0]["issubjecttype"]);
                            for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                            {
                                if (string.IsNullOrEmpty(feedbakpk))
                                    feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                                else
                                    feedbakpk = feedbakpk + "," + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                            }
                        }
                    }

                    ArrayList arrColHdrNames1 = new ArrayList();
                    arrColHdrNames1.Add("S.No");
                    data.Columns.Add("col0");
                    arrColHdrNames1.Add("Branch Name");

                    data.Columns.Add("col1");
                    arrColHdrNames1.Add("Register No");

                    data.Columns.Add("col2");
                    arrColHdrNames1.Add("Roll No");

                    data.Columns.Add("col3");
                    arrColHdrNames1.Add("Admission No");
                    data.Columns.Add("col4");

                    int col = 4;
                    string query = string.Empty;

                    query = "select  qm.HeaderCode, (select TextVal from TextValTable where TextCode= HeaderCode) as HeaderName,qm.Question,qm.QuestionMasterPK,FeedBackMasterPK from CO_FeedBackMaster m,CO_FeedBackQuestions q ,CO_QuestionMaster qm where  m.FeedBackMasterPK =q.FeedBackMasterFK and qm.QuestionMasterPK=q.QuestionMasterFK  and FeedBackType ='2' and q.objdes='2' and FeedBackMasterPK in (select top 1 FeedBackMasterPK  from CO_FeedBackMaster where FeedBackType ='2'and FeedBackMasterPK in(" + feedbakpk + ") order by StartDate desc) ";

                    ds = d2.select_method_wo_parameter(query, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            col++;
                            string quest = Convert.ToString(ds.Tables[0].Rows[i]["Question"]);
                            string questionpk = Convert.ToString(ds.Tables[0].Rows[i]["QuestionMasterPK"]);
                            arrColHdrNames1.Add(quest);
                            data.Columns.Add("col" + col);
                            col++;
                            arrColHdrNames1.Add(questionpk);
                            data.Columns.Add("col" + col);


                        }
                        DataRow drHdr1 = data.NewRow();
                        for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                            drHdr1["col" + grCol] = arrColHdrNames1[grCol];
                        data.Rows.Add(drHdr1);

                        string selqry = "";
                        selqry = " SELECT Course_Name+'-'+Dept_Name Degree, f.app_no,Roll_No,Roll_Admit,Reg_No FROM CO_StudFeedBack F,CO_FeedBackMaster M,Registration R,Degree G,Course C,Department D WHERE F.App_No = R.App_No AND R.degree_code = G.Degree_Code AND G.Course_Id = C.Course_Id  AND G.college_code = C.college_code AND g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and f.FeedBackMasterFK = m.FeedBackMasterPK and m.FeedBackName='" + ddl_Feedbackname.SelectedItem.Text + "' and m.FeedBackType = '2' AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK' and R.degree_code in ('" + degree_code + "') and R.college_code in ('" + college_cd + "') and R.Batch_Year in ('" + Batch_Year + "') and R.Current_Semester in ('" + semester + "')";
                        //   selqry = selqry + " SELECT Course_Name+'-'+Dept_Name Degree,f.app_no,Roll_No,Roll_Admit,Reg_No FROM Registration R,Degree G,Course C,Department D WHERE R.degree_code = G.Degree_Code AND G.Course_Id = C.Course_Id AND G.college_code = C.college_code AND g.Dept_Code = d.Dept_Code and g.college_code = d.college_code  AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK' and R.college_code in ('" + college_cd + "') and R.degree_code in ('" + degree_code + "') and R.Batch_Year in ('" + Batch_Year + "') and R.Current_Semester in ('" + semester + "')";
                        if (section != "")
                        {
                            selqry = selqry + " and R.Sections in ('" + section + "') GROUP BY Course_Name,Dept_Name,Current_semester,Sections,R.Batch_Year,R.degree_code, R.college_code, f.app_no,Roll_No,Roll_Admit,Reg_No  ORDER BY  R.degree_code ";
                            //Batch_Year,Course_Name,Dept_Name,Current_Semester,Sections";
                        }
                        else
                        {
                            selqry = selqry + " GROUP BY Course_Name,Dept_Name,Current_semester,Sections,R.Batch_Year,R.degree_code,R.college_code, f.app_no,Roll_No,Roll_Admit,Reg_No ORDER BY Batch_Year,Course_Name,Dept_Name,Current_Semester,Sections";
                        }

                        newds = d2.select_method_wo_parameter(selqry, "Text");
                        if (newds.Tables[0].Rows.Count > 0)
                        {
                            int sno = 0;
                            for (int val = 0; val < newds.Tables[0].Rows.Count; val++)
                            {

                                drow = data.NewRow();
                                data.Rows.Add(drow);
                                sno++;
                                data.Rows[data.Rows.Count - 1][0] = Convert.ToString(sno);
                                data.Rows[data.Rows.Count - 1][1] = newds.Tables[0].Rows[val]["Degree"].ToString();
                                data.Rows[data.Rows.Count - 1][2] = newds.Tables[0].Rows[val]["Reg_No"].ToString();
                                data.Rows[data.Rows.Count - 1][3] = newds.Tables[0].Rows[val]["Roll_No"].ToString();
                                data.Rows[data.Rows.Count - 1][4] = newds.Tables[0].Rows[val]["Roll_Admit"].ToString();
                                string appno = Convert.ToString(newds.Tables[0].Rows[val]["app_no"]);
                                for (int j = 5; j < data.Columns.Count; j++)
                                {
                                    string question = Convert.ToString(data.Rows[0][j]);

                                    string questionpk = Convert.ToString(data.Rows[0][j + 1]);
                                    string answer = d2.GetFunction("select answerdesc from CO_StudFeedBack  where app_no='" + appno + "' and QuestionMasterFK='" + questionpk + "' and FeedBackMasterFk='" + feedbakpk + "'");
                                    data.Rows[data.Rows.Count - 1][j] = Convert.ToString(answer);
                                    j++;

                                }




                            }
                            if (data.Columns.Count > 0 && data.Rows.Count > 0)
                            {

                                Showgrid.DataSource = data;
                                Showgrid.DataBind();
                                Showgrid.Visible = true;
                                Showgrid.Width = 500;
                                if (Showgrid.Rows.Count > 0)
                                {
                                    Showgrid.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                    Showgrid.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                                    Showgrid.Rows[0].Font.Bold = true;



                                }

                            }
                            divMainContents.Visible = true;

                            lbl_reportname.Visible = true;
                            txt_excelname.Visible = true;
                            btn_Excel.Visible = true;
                            btn_printmaster.Visible = true;
                            btnPrint.Visible = true;
                            div_report.Visible = true;

                        }
                        else
                        {
                            divMainContents.Visible = false;

                            lbl_reportname.Visible = false;
                            txt_excelname.Visible = false;
                            btn_Excel.Visible = false;
                            btn_printmaster.Visible = false;
                            btnPrint.Visible = false;
                            div_report.Visible = false;
                            ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"No Records Found\");", true);
                            Showgrid.Visible = false;
                        }



                    }
                    else
                    {
                        divMainContents.Visible = false;

                        lbl_reportname.Visible = false;
                        txt_excelname.Visible = false;
                        btn_Excel.Visible = false;
                        btn_printmaster.Visible = false;
                        btnPrint.Visible = false;
                        div_report.Visible = false;
                        ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"No Records Found\");", true);
                        Showgrid.Visible = false;
                    }

                }
                else if (rdbstaffwise.Checked == true)
                {
                    lbl_error.Visible = false;
                   
                    


                    string degreecode = rs.GetSelectedItemsValueAsString(cbl_branch);
                    string sem = rs.GetSelectedItemsValueAsString(cbl_sem);
                    string batchyear = rs.GetSelectedItemsValueAsString(cbl_batch);
                    string clgcode = rs.GetSelectedItemsValueAsString(Cbl_college);
                    string sec = rs.GetSelectedItemsValueAsString(cbl_sec);
                    
                    if (!string.IsNullOrEmpty(degreecode) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(batchyear))
                    {
                       

                        if (ddl_Feedbackname.SelectedItem.Text.Trim() != "--Select--")
                        {
                            string type = "1";
                            string fbpk = " select FeedBackMasterPK,ISNULL(issubjecttype,0)issubjecttype from CO_FeedBackMaster where FeedBackName ='" + ddl_Feedbackname.SelectedItem.Value + "'";
                            DataSet dsfb = d2.select_method_wo_parameter(fbpk, "Text");
                            string feedbakpk = string.Empty;
                            string issubjecttype = string.Empty;
                            if (dsfb.Tables.Count > 0)
                            {
                                if (dsfb.Tables[0].Rows.Count > 0)
                                {
                                    issubjecttype = Convert.ToString(dsfb.Tables[0].Rows[0]["issubjecttype"]);
                                    for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                                    {
                                        if (string.IsNullOrEmpty(feedbakpk))
                                            feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                                        else
                                            feedbakpk = feedbakpk + "," + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                                    }
                                }
                            }
                            //rs.Fpreadheaderbindmethod("S No-50/Department-200/StaffCode & StaffName-250/Subject Code-150/Subject Name-250/SubjectType-100", FpSpread1, "false");

                            //Modified by saranya on 20/08/2018
                            rs.Fpreadheaderbindmethod("S No-50/StaffName-200/Department-200/Subject Code-150/Subject Name-250/Roll_NO-100/Student Name-150", FpSpread1, "false");
                            //Hashtable hat = new Hashtable();
                            //hat.Add("@CollegeCode", clgcode);
                            //hat.Add("@batchyear", batchyear);
                            //hat.Add("@Degreecode", degreecode);
                            //hat.Add("@semester", sem);
                            //hat.Add("@section", sec);
                            //hat.Add("@FeedbackName", Convert.ToString(ddl_feedback.SelectedItem.Text));
                            //hat.Add("@FeedbackMasterFK", feedbakpk);
                            ////   hat.Add("@StaffAppNo", StaffAppID);
                            //hat.Add("@FeedbackType", type);
                            //hat.Add("@subjectno", subjectcode);
    
                            string query = " select  S.App_No,r.Roll_No,r.Stud_Name,S.answerdesc,(convert(varchar(10), f.Batch_Year)+'-'+co.Course_Name+'-'+ dt.dept_acronym+'-'+convert(varchar(10), f.Semester)+'-'+f.Section ) as department,c.subject_code, s.StaffApplNo,sm.staff_code +' - '+staff_name as staff ,f.FeedBackMasterPK,s.QuestionMasterFK,f.Batch_Year ,f.semester,f.DegreeCode,f.Section,SubjectNo,c.subject_name,sm.staff_code,staff_name,c.acronym from CO_FeedBackMaster F,CO_StudFeedBack S,staff_appl_master sa,staffmaster sm ,subject c,Department dt,course co,Degree d,Registration r,CO_QuestionMaster Q where d.Degree_Code =f.degreecode and dt.Dept_Code =d.Dept_Code and co.Course_Id =d.Course_Id and c.subject_no=s.SubjectNo and sa.appl_no=sm.appl_no  and S.App_No=r.App_No and  sa.appl_id=s.StaffApplNo and s.FeedBackMasterFK =f.FeedBackMasterPK and q.QuestionMasterPK =s.QuestionMasterFK and Q.QuestType='1' and Q.objdes='2' and f.Batch_Year in('" + batchyear + "') and f.semester in ('" + sem + "')  and isnull(f.Section,'') in ('" + sec + "') and f.FeedBackMasterPK in ('" + feedbakpk + "') and s.App_No is not null and s.answerdesc is not null ";

                            query += " SELECT distinct Question,questionacr,QuestionMasterPK,HeaderCode FROM CO_FeedBackMaster B,CO_QuestionMaster Q ,CO_FeedBackQuestions FB WHERE  b.FeedBackMasterPK =fb.FeedBackMasterFK and q.QuestionMasterPK =fb.QuestionMasterFK  and B.FeedBackName='" + Convert.ToString(ddl_Feedbackname.SelectedItem.Text) + "' and Q.QuestType='1' and Q.objdes='2'  and  B.CollegeCode in ('" + clgcode + "') order by HeaderCode";
                            query += " SELECT StaffApplNo,QuestionMasterfK,SubjectNo,isnull(b.Section,'')Section FROM CO_StudFeedBack F,CO_FeedBackMaster B,CO_QuestionMaster Q where F.FeedBackMasterFK = B.FeedBackMasterPK and q.QuestionMasterPK =f.QuestionMasterFK and StaffApplNo is not null and B.FeedBackMasterpK in ('" + feedbakpk + "') and B.CollegeCode in ('" + clgcode + "') and b.Batch_Year in('" + batchyear + "') and b.semester in ('" + sem + "')  and isnull(b.Section,'') in ('" + sec + "') and Q.QuestType='1' and Q.objdes='2' group by StaffApplNo,QuestionMasterfK,SubjectNo,isnull(b.Section,'') order by  StaffApplNo,SubjectNo";
                            query += " select count(App_No)studentcount,degree_code,sections,college_code from Registration where  college_code in('" + clgcode + "') and isnull(Sections,'') in('" + sec + "') and cc=0 and delflag=0 and exam_flag<>'Debar' group by degree_code,college_code,sections";
                            query += " select COUNT( distinct fq.QuestionMasterFK)question_count,isnull(fq.SubjectType,'')SubjectType from CO_FeedBackQuestions fq,CO_QuestionMaster qm where fq.FeedBackMasterFK in ('" + feedbakpk + "') and qm.QuestionMasterPK=fq.QuestionMasterFK and qm.QuestType='1' and qm.objdes='2' group by isnull(SubjectType,'')";

                            ds = d2.select_method_wo_parameter(query, "Text");
                           
                            double question_count = 0;
                            if (ds.Tables.Count > 0)
                            {
                                
                               
                                if (ds.Tables[1].Rows.Count > 0)
                                {
                                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                                    {
                                        FpSpread1.Sheets[0].ColumnCount++;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[1].Rows[i]["Question"]);
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[1].Rows[i]["QuestionMasterPK"]);
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                        FpSpread1.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 300;
                                    }
                                    
                                }
                                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[2].Rows.Count > 0)
                                {
                                    int k = 0; string staffname = ""; int s = 1;
                                    FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                                    cb.AutoPostBack = true;
                                    FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                                    cb1.AutoPostBack = false;
                                    //FpSpread1.Sheets[0].RowCount++;
                                    double staffavg = 0; bool staffinvdiavg = false; double sumavgpoint = 0; int staffrowcount = 0;

                                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                                    {

                                        ds.Tables[0].DefaultView.RowFilter = " QuestionMasterfK ='" + ds.Tables[2].Rows[i]["QuestionMasterfK"] + "' and StaffApplNo='" + ds.Tables[2].Rows[i]["StaffApplNo"] + "'  and SubjectNo='" + Convert.ToString(ds.Tables[2].Rows[i]["SubjectNo"]) + "' and Section='" + Convert.ToString(ds.Tables[2].Rows[i]["Section"]) +"'";

                                        DataView dv = ds.Tables[0].DefaultView;

                                        staffinvdiavg = false;
                                        FpSpread1.Sheets[0].RowCount++;
                                        
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(s);
                                        string staff = Convert.ToString(dv.Table.Rows[i]["staff"]);
                                        string[] staffSplit = staff.Split('-');

                                        
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = staffSplit[1];//k.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["department"].ToString();
                                        staffname = ds.Tables[0].Rows[i]["staff_name"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                                        //Changed by saranya on 20/8/2018
                                       
                                        ////////////////////////////////////

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["subject_code"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["Subject_Name"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                        

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ds.Tables[0].Rows[i]["Roll_No"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = ds.Tables[0].Rows[i]["Stud_Name"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = ds.Tables[0].Rows[i]["section"].ToString();
                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Bold = true;


                                        

                                        double gtotal = 0; double mtotal = 0; double avgper = 0;
                                        string filterquery = string.Empty;
                                        string section = Convert.ToString(ds.Tables[0].Rows[i]["Section"]);
                                        filterquery = "degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["DegreeCode"]) + "' ";
                                        if (section.Trim() != "")
                                        {
                                            filterquery = filterquery + " and Sections='" + section + "'";
                                        }
                                        ds.Tables[3].DefaultView.RowFilter = "" + filterquery + "";
                                        DataView dvnew = ds.Tables[3].DefaultView;
                                        string totalstudnent = "";
                                        if (dvnew.Count > 0)
                                        {
                                            totalstudnent = Convert.ToString(dvnew[0]["studentcount"]);
                                        }
                                        if (totalstudnent.Trim() == "")
                                            totalstudnent = "0";
                                        //double maximun = Convert.ToDouble(question_count) * Convert.ToDouble(sum_total) * Convert.ToDouble(totalstudnent);
                                        //Double attendstrength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                                        //double maximun = Convert.ToDouble(sum_total) * Convert.ToDouble(attendstrength);
                                        double QuestionAttendcount = 0;
                                        for (int j = 7; j < FpSpread1.Columns.Count; j++)//modified by saranya on 27Aug2018 FpSpread1.Columns.Count changed to FpSpread1.Columns.Count - 2
                                        {
                                            string questionmasterPK = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, j].Tag);
                                            
                                           
                                            if (dv.Count > 0)
                                            {
                                                QuestionAttendcount++;
                                               
                                                
                                                double questavgpoint = 0;// 100;
                                                questavgpoint = Math.Round(questavgpoint, 0, MidpointRounding.AwayFromZero);
                                                //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = Convert.ToString(Math.Round(questavgpoint, 2)); //Convert.ToString(dv[0]["points"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = Convert.ToString(questavgpoint);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                gtotal += Convert.ToDouble((Math.Round(questavgpoint, 2)));
                                            }
                                            else
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = "-";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                            }
                                            FpSpread1.Columns[j - 1].Locked = true;
                                            FpSpread1.Columns[4].Locked = true;
                                        }
                                        
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].CellType = new FarPoint.Web.Spread.TextCellType();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Text = Convert.ToString("");
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";

                                       
                                       
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Text = Convert.ToString("");
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";

                                    }

                                    FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                                    // FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
                                    //FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
                                    //FpSpread1.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                    FpSpread1.Height = 500;
                                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                                    SpreadDiv.Visible = true;
                                }
                                else
                                {
                                    SpreadDiv.Visible = false;
                                    divMainContents.Visible = false;

                                    lbl_reportname.Visible = false;
                                    txt_excelname.Visible = false;
                                    btn_Excel.Visible = false;
                                    btn_printmaster.Visible = false;
                                    btnPrint.Visible = false;
                                    div_report.Visible = false;
                                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"No Records Found\");", true);
                                    Showgrid.Visible = false;
                                }
                            }
                            else
                            {
                                SpreadDiv.Visible = false;
                                divMainContents.Visible = false;

                                lbl_reportname.Visible = false;
                                txt_excelname.Visible = false;
                                btn_Excel.Visible = false;
                                btn_printmaster.Visible = false;
                                btnPrint.Visible = false;
                                div_report.Visible = false;
                                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"No Records Found\");", true);
                                Showgrid.Visible = false;
                                
                                
                            }
                        }
                        else
                        {
                            SpreadDiv.Visible = false;
                            divMainContents.Visible = false;

                            lbl_reportname.Visible = false;
                            txt_excelname.Visible = false;
                            btn_Excel.Visible = false;
                            btn_printmaster.Visible = false;
                            btnPrint.Visible = false;
                            div_report.Visible = false;
                            ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Feedback Name\");", true);
                            Showgrid.Visible = false;
                            
                        }
                    }
                    else
                    {
                        SpreadDiv.Visible = false;
                        divMainContents.Visible = false;

                        lbl_reportname.Visible = false;
                        txt_excelname.Visible = false;
                        btn_Excel.Visible = false;
                        btn_printmaster.Visible = false;
                        btnPrint.Visible = false;
                        div_report.Visible = false;
                        ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please select all fields\");", true);
                        Showgrid.Visible = false;
                       
                    }
                }


            }
            else
            {
                divMainContents.Visible = false;

                lbl_reportname.Visible = false;
                txt_excelname.Visible = false;
                btn_Excel.Visible = false;
                btn_printmaster.Visible = false;
                btnPrint.Visible = false;
                div_report.Visible = false;
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Feedback\");", true);
                Showgrid.Visible = false;

            }

        }
        catch (Exception ex)
        {

        }

    }

    public void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string attendance = "General FeedBack Report";
            string pagename = "generaldescriptivefeedbackreport.aspx";
            Printcontrol.loadspreaddetails(Showgrid, pagename, attendance);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
        }
    }
    protected void Showgrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                    e.Row.Cells[grCol].Visible = false;
                for (int val = 5; val < data.Columns.Count; val++)
                {
                    e.Row.Cells[val].Width = 200;
                    e.Row.Cells[val].HorizontalAlign = HorizontalAlign.Center;
                    val++;
                    e.Row.Cells[val].Visible = false;

                }
                e.Row.Cells[0].Width = 50;
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[1].Width = 200;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[2].Width = 150;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[3].Width = 150;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].Width = 150;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                for (int j = 0; j < data.Columns.Count; j++)
                    e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
                for (int val = 5; val < data.Columns.Count; val++)
                {
                    e.Row.Cells[val].Width = 200;
                    e.Row.Cells[val].HorizontalAlign = HorizontalAlign.Left;
                    val++;
                    e.Row.Cells[val].Visible = false;

                }
                e.Row.Cells[0].Width = 50;
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[1].Width = 200;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[2].Width = 150;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[3].Width = 150;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[4].Width = 150;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Left;
                if (Session["Rollflag"].ToString() == "1")
                {
                    e.Row.Cells[3].Visible = true;
                }
                else
                {
                    e.Row.Cells[3].Visible = false;
                }
                if (Session["Regflag"].ToString() == "1")
                {
                    e.Row.Cells[2].Visible = true;
                }
                else
                {
                    e.Row.Cells[2].Visible = false;
                }
                if (Session["Admitflag"].ToString() == "1")
                {
                    e.Row.Cells[4].Visible = true;
                }
                else
                {
                    e.Row.Cells[4].Visible = false;
                }
            }



        }
        catch
        {
        }
    }
    public void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {

            string report = txt_excelname.Text;
            if (report.ToString().Trim() != "")
            {
                lbl_norec.Visible = false;
                lbl_norec.Text = "";
                d2.printexcelreportgrid(Showgrid, report);

            }
            else
            {
                lbl_norec.Text = "Please Enter Your Report Name";
                lbl_norec.Visible = true;
            }
            btn_Excel.Focus();
        }

        catch (Exception ex)
        {
            lbl_norec.Text = ex.ToString();
        }

    }
    protected void txtexcelname_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txt_excelname.Visible = true;
            btn_Excel.Visible = true;
            btn_printmaster.Visible = true;
            lbl_reportname.Visible = true;
            btn_Excel.Focus();
            btnPrint.Visible = true;
        }
        catch (Exception ex)
        {

        }
    }

    protected void rdbstaffwise_Click(object sender, EventArgs e)
    {
        if (rdbstaffwise.Checked == true)
        {
            rdbgeneral.Checked = false;
            
        }

    }
    protected void rdbgeneral_Click(object sender, EventArgs e)
    {
        if (rdbgeneral.Checked == true)
        {
            rdbstaffwise.Checked = false;
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    { }


}