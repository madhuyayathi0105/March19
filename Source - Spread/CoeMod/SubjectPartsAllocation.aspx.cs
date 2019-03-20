using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Farpoint = FarPoint.Web.Spread;
using System.Configuration;

public partial class SubjectPartsAllocation : System.Web.UI.Page
{
    Hashtable hat = new Hashtable();
    Hashtable has = new Hashtable();

    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string edu_level = string.Empty;
    string qry = string.Empty;

    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();

    DataSet ds = new DataSet();

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
            if (!IsPostBack)
            {
                divSubjectDetails.Visible = false;
                errmsg.Text = string.Empty;
                errmsg.Visible = false;
                lblPopAlert.Text = string.Empty;
                divPopUpAlert.Visible = false;
                chkEnableSubjectType.Checked = true;
                chkEnableSubject.Checked = true;
                bindEduLevel();
                bindbatch();
                binddegree();
                bindbranch();
                bindsem();
                bindsubjtype();
                Subjects();
            }
        }
        catch
        {
        }
    }

    public void bindbatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            ds.Clear();
            ds = da.BindBatch();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    ddlbatch.DataSource = ds;
                    ddlbatch.DataTextField = "batch_year";
                    ddlbatch.DataValueField = "batch_year";
                    ddlbatch.DataBind();
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
            ddldegree.Items.Clear();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            edu_level = Convert.ToString(rblCourse.SelectedItem);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            has.Clear();
            has.Add("single_user", singleuser);
            has.Add("group_code", group_user);
            has.Add("college_code", Convert.ToString(Session["collegecode"]));
            has.Add("user_code", usercode);
            ds.Clear();
            ds = da.select_method_wo_parameter("select distinct dg.course_id,c.course_name from Degree dg,Course c,Department dt where dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code and c.Edu_Level='" + edu_level + "'  and c.college_code='" + Convert.ToString(Session["collegecode"]) + "'", "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count1 = ds.Tables[0].Rows.Count;
                if (count1 > 0)
                {
                    ddldegree.DataSource = ds;
                    ddldegree.DataTextField = "course_name";
                    ddldegree.DataValueField = "course_id";
                    ddldegree.DataBind();
                }
                ddldegree.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindsem()
    {
        try
        {
            ddlsem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            string sqlnew = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " and college_code=" + Session["collegecode"].ToString() + "";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = d2.select_method_wo_parameter(sqlnew, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsem.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i == 2)
                    {
                        ddlsem.Items.Add(i.ToString());
                    }
                }
            }
            else
            {
                sqlnew = "select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and college_code=" + Session["collegecode"].ToString() + "";
                ds.Clear();
                ds = d2.select_method_wo_parameter(sqlnew, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                    duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlsem.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlsem.Items.Add(i.ToString());
                        }
                    }
                }
            }
            bindsubjtype();
        }
        catch (Exception ex)
        {
        }
    }

    public void bindbranch()
    {
        try
        {
            has.Clear();
            ddlbranch.Items.Clear();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            collegecode = Convert.ToString(Session["collegecode"]);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            has.Add("single_user", singleuser);
            has.Add("group_code", group_user);
            has.Add("course_id", ddldegree.SelectedValue);
            has.Add("college_code", collegecode);
            has.Add("user_code", usercode);
            ds.Clear();
            ds = da.select_method("bind_branch", has, "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count2 = ds.Tables[0].Rows.Count;
                if (count2 > 0)
                {
                    ddlbranch.DataSource = ds;
                    ddlbranch.DataTextField = "dept_name";
                    ddlbranch.DataValueField = "degree_code";
                    ddlbranch.DataBind();
                }
            }
            bindsem();
        }
        catch (Exception ex)
        {
        }
    }

    public void bindsubjtype()
    {
        try
        {
            ddl_SubType.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            string sqlnew = "select subject_type,subtype_no from syllabus_master sm,sub_sem s where sm.syll_code=s.syll_code  and batch_year='" + ddlbatch.Text.ToString() + "' and semester='" + ddlsem.Text.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "' ";//order by case when priority is not null and priority<>0 then priority else cast(subType_no as bigint) end
            DataSet ds = new DataSet();
            ds.Clear();
            ds = d2.select_method_wo_parameter(sqlnew, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddl_SubType.DataSource = ds;
                ddl_SubType.DataTextField = "subject_type";
                ddl_SubType.DataValueField = "subtype_no";
                ddl_SubType.DataBind();
            }
            if (chkEnableSubjectType.Checked)
            {
                ddl_SubType.Enabled = true;
            }
            else
            {
                ddl_SubType.Enabled = false;
            }
            Subjects();
        }
        catch (Exception ex)
        {
        }
    }

    public void Subjects()
    {
        try
        {
            string subjecttype = string.Empty;
            string qrySubjectType = string.Empty;
            cbl_Subjects.Items.Clear();
            ds.Clear();
            if (chkEnableSubjectType.Checked)
            {
                if (ddl_SubType.Items.Count > 0)
                {
                    subjecttype = Convert.ToString(ddl_SubType.SelectedItem);
                    qrySubjectType = " and ss.subtype_no='" + Convert.ToString(ddl_SubType.SelectedItem.Value).Trim() + "'";
                }
            }
            else
            {
                qrySubjectType = string.Empty;
            }
            if (rblOptions.SelectedValue == "0")
            {
                string srtre = "select Convert(Varchar(Max),(rtrim(Ltrim(subject_code))+' - ' +rtrim(Ltrim(subject_name))))  As Subject_Name,subject_no,s.subjectpriority from subject s,syllabus_master sm ,sub_sem ss where ss.syll_code=sm.syll_code and sm.syll_code=s.syll_code and ss.syll_code=s.syll_code and ss.subtype_no=s.subtype_no and batch_year='" + ddlbatch.SelectedItem.Text.ToString() + "' and semester='" + ddlsem.SelectedItem.Text.ToString() + "' and degree_code='" + Convert.ToString(ddlbranch.SelectedValue) + "' " + qrySubjectType + " order by s.subjectpriority";
                ds = d2.select_method_wo_parameter(srtre, "Text");
                //"select subject_name,subject_no from subject s,sub_sem sm where s.subType_no =sm.subType_no and sm.subject_type ='" + subjecttype + "' order by subject_name,subject_no"  distinct s.subject_no,subject_code,subject_name,subject_type,ss.subtype_no,subjectpriority 
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cbl_Subjects.DataSource = ds;
                    cbl_Subjects.DataTextField = "Subject_Name";
                    cbl_Subjects.DataValueField = "subject_no";
                    cbl_Subjects.DataBind();
                    for (int h = 0; h < cbl_Subjects.Items.Count; h++)
                    {
                        cbl_Subjects.Items[h].Selected = true;
                    }
                    txt_Subject.Text = "Subject(" + (cbl_Subjects.Items.Count) + ")";
                    cb_Subjects.Checked = true;
                }
                else
                {
                    txt_Subject.Text = "-- Select --";
                    cb_Subjects.Checked = false;
                }
            }
            else if (rblOptions.SelectedValue == "1")
            {
                string srtre = "select subject_code,Convert(Varchar(Max),(rtrim(Ltrim(subject_code))+' - ' +rtrim(Ltrim(subject_name))))  As Subject_Name,s.subjectpriority from subject s,syllabus_master sm ,sub_sem ss where ss.syll_code=sm.syll_code and sm.syll_code=s.syll_code and ss.syll_code=s.syll_code and ss.subtype_no=s.subtype_no and batch_year='" + ddlbatch.SelectedItem.Text.ToString() + "' and semester='" + ddlsem.SelectedItem.Text.ToString() + "' and degree_code='" + Convert.ToString(ddlbranch.SelectedValue) + "' " + qrySubjectType + " order by subject_code";
                ds = d2.select_method_wo_parameter(srtre, "Text");
                //select subject_code,Convert(Varchar(Max),(rtrim(Ltrim(subject_code))+' - ' +rtrim(Ltrim(subject_name))))  As Subject_Name from subject s,sub_sem sm where s.subType_no =sm.subType_no and sm.subject_type ='" + subjecttype + "' order by subject_code,Subject_Name
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cbl_Subjects.DataSource = ds;
                    cbl_Subjects.DataTextField = "Subject_Name";
                    cbl_Subjects.DataValueField = "subject_code";
                    cbl_Subjects.DataBind();
                    for (int h = 0; h < cbl_Subjects.Items.Count; h++)
                    {
                        cbl_Subjects.Items[h].Selected = true;
                    }
                    txt_Subject.Text = "Subject(" + (cbl_Subjects.Items.Count) + ")";
                    cb_Subjects.Checked = true;
                }
                else
                {
                    txt_Subject.Text = "-- Select --";
                    cb_Subjects.Checked = false;
                }
            }
            if (chkEnableSubject.Checked)
            {
                txt_Subject.Enabled = true;
            }
            else
            {
                txt_Subject.Enabled = false;
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindEduLevel()
    {
        try
        {
            ds.Clear();
            rblCourse.Items.Clear();
            string qry = "select distinct Edu_Level from course where college_code='" + Convert.ToString(Session["collegecode"]) + "' order by Edu_Level desc";
            ds = d2.select_method_wo_parameter(qry, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                rblCourse.DataSource = ds;
                rblCourse.DataTextField = "Edu_Level";
                rblCourse.DataValueField = "Edu_Level";
                rblCourse.DataBind();
                rblCourse.SelectedIndex = 0;
            }
            else
            {
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void rbCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        divSubjectDetails.Visible = false;
        errmsg.Text = string.Empty;
        errmsg.Visible = false;
        lblPopAlert.Text = string.Empty;
        divPopUpAlert.Visible = false;
        binddegree();
        bindbranch();
        bindsem();
        bindsubjtype();
        Subjects();
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            divSubjectDetails.Visible = false;
            errmsg.Text = string.Empty;
            errmsg.Visible = false;
            lblPopAlert.Text = string.Empty;
            divPopUpAlert.Visible = false;
            binddegree();
            bindbranch();
            bindsem();
            bindsubjtype();
            Subjects();
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            divSubjectDetails.Visible = false;
            errmsg.Text = string.Empty;
            errmsg.Visible = false;
            lblPopAlert.Text = string.Empty;
            divPopUpAlert.Visible = false;
            bindbranch();
            bindsem();
            bindsubjtype();
            Subjects();
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            divSubjectDetails.Visible = false;
            errmsg.Text = string.Empty;
            errmsg.Visible = false;
            lblPopAlert.Text = string.Empty;
            divPopUpAlert.Visible = false;
            bindsem();
            bindsubjtype();
            Subjects();
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            divSubjectDetails.Visible = false;
            errmsg.Text = string.Empty;
            errmsg.Visible = false;
            lblPopAlert.Text = string.Empty;
            divPopUpAlert.Visible = false;
            bindsubjtype();
            Subjects();
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddl_SubType_SelectedIndexChanged(object sender, EventArgs e)
    {
        divSubjectDetails.Visible = false;
        errmsg.Text = string.Empty;
        errmsg.Visible = false;
        lblPopAlert.Text = string.Empty;
        divPopUpAlert.Visible = false;
        Subjects();
    }

    protected void ddl_Parts_SelectedIndexChanged(object sender, EventArgs e)
    {
        divSubjectDetails.Visible = false;
        errmsg.Text = string.Empty;
        errmsg.Visible = false;
        lblPopAlert.Text = string.Empty;
        divPopUpAlert.Visible = false;
    }

    protected void cb_Subjects_CheckedChanged(object sender, EventArgs e)
    {
        divSubjectDetails.Visible = false;
        errmsg.Text = string.Empty;
        errmsg.Visible = false;
        lblPopAlert.Text = string.Empty;
        divPopUpAlert.Visible = false;
        int cout = 0;
        errmsg.Text = string.Empty;
        errmsg.Visible = false;
        lblPopAlert.Text = string.Empty;
        divPopUpAlert.Visible = false;
        txt_Subject.Text = "--Select--";
        if (cb_Subjects.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_Subjects.Items.Count; i++)
            {
                cbl_Subjects.Items[i].Selected = true;
            }
            txt_Subject.Text = "Subject(" + (cbl_Subjects.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_Subjects.Items.Count; i++)
            {
                cbl_Subjects.Items[i].Selected = false;
            }
            txt_Subject.Text = "--Select--";
        }
    }

    protected void cbl_Subjects_SelectedIndexChanged(object sender, EventArgs e)
    {
        divSubjectDetails.Visible = false;
        errmsg.Text = string.Empty;
        errmsg.Visible = false;
        lblPopAlert.Text = string.Empty;
        divPopUpAlert.Visible = false;
        int commcount = 0;
        txt_Subject.Text = "--Select--";
        cb_Subjects.Checked = false;
        for (int i = 0; i < cbl_Subjects.Items.Count; i++)
        {
            if (cbl_Subjects.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_Subjects.Items.Count)
            {
                cb_Subjects.Checked = true;
            }
            txt_Subject.Text = "Subject(" + commcount.ToString() + ")";
        }
    }

    protected void rblOptional_SelectedIndexChanged(object sender, EventArgs e)
    {
        divSubjectDetails.Visible = false;
        errmsg.Text = string.Empty;
        errmsg.Visible = false;
        lblPopAlert.Text = string.Empty;
        divPopUpAlert.Visible = false;
        if (rblOptions.SelectedValue == "0")
        {
            Subjects();
        }
        else if (rblOptions.SelectedValue == "1")
        {
            Subjects();
        }
    }

    protected void txt_noofpart_TextChanged(object sender, EventArgs e)
    {
        divSubjectDetails.Visible = false;
        errmsg.Text = string.Empty;
        errmsg.Visible = false;
        lblPopAlert.Text = string.Empty;
        divPopUpAlert.Visible = false;
        ddl_Parts.Items.Clear();
        string num = txt_noofpart.Text.ToString();
        int no = 0;
        int.TryParse(num, out no);
        for (int i = 0; i < no; i++)
        {
            ddl_Parts.Items.Insert(i, new ListItem(Convert.ToString(("Part " + (i + 1))), Convert.ToString((i + 1))));
        }
    }

    protected void txtSelectPart_TextChanged(object sender, EventArgs e)
    {

    }

    protected void btnAllocate_Click(object sender, EventArgs e)
    {
        try
        {
            errmsg.Text = string.Empty;
            errmsg.Visible = false;
            lblPopAlert.Text = string.Empty;
            divPopUpAlert.Visible = false;
            string totpart = Convert.ToString(txt_noofpart.Text.Trim());
            int ttpart = 0;
            int res = 0;
            int.TryParse(totpart, out ttpart);
            string part = string.Empty;
            string partno = string.Empty;
            string subtype = string.Empty;
            string sub_no = string.Empty;
            string batch = string.Empty;
            string degree_code = string.Empty;
            string sem = string.Empty;
            bool result = false;
            int selectedCount = 0;
            StringBuilder spsubname = new StringBuilder();
            if (ddlbatch.Items.Count == 0)
            {
                lblPopAlert.Text = "Batch Year is Not Found";
                divPopUpAlert.Visible = true;
                return;
            }
            if (ddldegree.Items.Count == 0)
            {
                lblPopAlert.Text = "Degree is Not Found";
                divPopUpAlert.Visible = true;
                return;
            }
            if (ddlbranch.Items.Count == 0)
            {
                lblPopAlert.Text = "Branch is Not Found";
                divPopUpAlert.Visible = true;
                return;
            }
            if (ddlsem.Items.Count == 0)
            {
                lblPopAlert.Text = "Semester is Not Found";
                divPopUpAlert.Visible = true;
                return;
            }
            if (ddl_SubType.Items.Count > 0)
            {
                subtype = ddl_SubType.SelectedItem.ToString();
            }
            else
            {
                lblPopAlert.Text = "Subject Type is Not Found";
                divPopUpAlert.Visible = true;
                return;
            }
            if (cbl_Subjects.Items.Count == 0)
            {
                lblPopAlert.Text = "Subject is Not Found";
                divPopUpAlert.Visible = true;
                return;
            }
            else
            {
                selectedCount = 0;
                foreach (ListItem liSubject in cbl_Subjects.Items)
                {
                    if (liSubject.Selected)
                    {
                        selectedCount++;
                    }
                }
                if (selectedCount == 0)
                {
                    lblPopAlert.Text = "Subject is Not Found";
                    divPopUpAlert.Visible = true;
                    return;
                }
            }
            if (ddl_Parts.Items.Count == 0)
            {
                lblPopAlert.Text = "Parts are Not Found";
                divPopUpAlert.Visible = true;
                return;
            }
            if (totpart.Trim() != "" && totpart.Trim() != null)
            {
                if (ddl_Parts.Items.Count > 0)
                {
                    part = ddl_Parts.SelectedItem.ToString();
                    partno = ddl_Parts.SelectedValue.ToString();
                    int cnt = 0;
                    if (rblOptions.SelectedValue == "0")
                    {
                        for (int i = 0; i < cbl_Subjects.Items.Count; i++)
                        {
                            if (cbl_Subjects.Items[i].Selected == true)
                            {
                                cnt++;
                                sub_no = cbl_Subjects.Items[i].Value.ToString();
                                string part_type = d2.GetFunction("select Part_Type from subject where subject_no='" + sub_no + "'");
                                res = d2.update_method_wo_parameter("update subject set Part_Type='" + partno + "' where subject_no='" + sub_no + "'", "Text");
                                if (res > 0)
                                {
                                    result = true;
                                }
                            }
                        }
                    }
                    else if (rblOptions.SelectedValue == "1")
                    {
                        for (int i = 0; i < cbl_Subjects.Items.Count; i++)
                        {
                            if (cbl_Subjects.Items[i].Selected == true)
                            {
                                cnt++;
                                sub_no = cbl_Subjects.Items[i].Value.ToString();
                                string part_type = d2.GetFunction("select Part_Type from subject where subject_code='" + sub_no + "'");
                                res = d2.update_method_wo_parameter("update subject set Part_Type='" + partno + "' where subject_code='" + sub_no + "'", "Text");
                                if (res > 0)
                                {
                                    result = true;
                                }
                            }
                        }
                    }
                }
            }
            if (result == false)
            {
                lblPopAlert.Text = "Parts are not allocated!!!";
                divPopUpAlert.Visible = true;
                return;
            }
            else if (result == true)
            {
                lblPopAlert.Text = "Parts are allocated Successfully";
                divPopUpAlert.Visible = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void chkEnableSubjectType_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkEnableSubjectType.Checked)
            {
                ddl_SubType.Enabled = true;
            }
            else
            {
                ddl_SubType.Enabled = false;
            }
            Subjects();
        }
        catch
        {
        }
    }

    protected void chkEnableSubject_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkEnableSubject.Checked)
            {
                txt_Subject.Enabled = true;
            }
            else
            {
                txt_Subject.Enabled = false;
            }
        }
        catch
        {

        }
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        try
        {
            txtSelectPart.Text = string.Empty;
            divSubjectDetails.Visible = false;
            errmsg.Text = string.Empty;
            errmsg.Visible = false;
            lblPopAlert.Text = string.Empty;
            divPopUpAlert.Visible = false;
            int selectedCount = 0;
            int totalPartCount = 0;
            StringBuilder spSubjectName = new StringBuilder();
            string partName = string.Empty;
            string partNo = string.Empty;
            string subjectType = string.Empty;
            string subjectTypeNo = string.Empty;
            string subjectNo = string.Empty;
            string batchYear = string.Empty;
            string degreeCode = string.Empty;
            string courseId = string.Empty;
            string semester = string.Empty;
            string qryBatchYear = string.Empty;
            string qryCourseId = string.Empty;
            string qryDegreeCode = string.Empty;
            string qrySemester = string.Empty;
            string qrySubjectType = string.Empty;
            string qrySubjectNo = string.Empty;
            bool result = false;
            string totpart = Convert.ToString(txt_noofpart.Text.Trim());
            int.TryParse(totpart, out totalPartCount);
            if (ddlbatch.Items.Count == 0)
            {
                lblPopAlert.Text = "Batch Year is Not Found";
                divPopUpAlert.Visible = true;
                return;
            }
            else
            {
                batchYear = Convert.ToString(ddlbatch.SelectedItem.Text).Trim();
                selectedCount = 0;
                qryBatchYear = string.Empty;
                foreach (ListItem liBatch in ddlbatch.Items)
                {
                    if (liBatch.Selected)
                    {
                        selectedCount++;
                        if (string.IsNullOrEmpty(qryBatchYear.Trim()))
                        {
                            qryBatchYear = "'" + liBatch.Value + "'";
                        }
                        else
                        {
                            qryBatchYear += ",'" + liBatch.Value + "'";
                        }
                    }
                }
                //if (!string.IsNullOrEmpty(qryBatchYear.Trim()))
                //{
                //}
            }
            if (ddldegree.Items.Count == 0)
            {
                lblPopAlert.Text = "Degree is Not Found";
                divPopUpAlert.Visible = true;
                return;
            }
            else
            {
                courseId = Convert.ToString(ddldegree.SelectedValue).Trim();
                selectedCount = 0;
                qryCourseId = string.Empty;
                foreach (ListItem liDegree in ddldegree.Items)
                {
                    if (liDegree.Selected)
                    {
                        selectedCount++;
                        if (string.IsNullOrEmpty(qryCourseId.Trim()))
                        {
                            qryCourseId = "'" + liDegree.Value + "'";
                        }
                        else
                        {
                            qryCourseId += ",'" + liDegree.Value + "'";
                        }
                    }
                }
                //if (!string.IsNullOrEmpty(qryCourseId.Trim()))
                //{
                //}
            }
            if (ddlbranch.Items.Count == 0)
            {
                lblPopAlert.Text = "Branch is Not Found";
                divPopUpAlert.Visible = true;
                return;
            }
            else
            {
                degreeCode = Convert.ToString(ddlbranch.SelectedValue).Trim();
                selectedCount = 0;
                qryDegreeCode = string.Empty;
                foreach (ListItem liBranch in ddlbranch.Items)
                {
                    if (liBranch.Selected)
                    {
                        selectedCount++;
                        if (string.IsNullOrEmpty(qryDegreeCode.Trim()))
                        {
                            qryDegreeCode = "'" + liBranch.Value + "'";
                        }
                        else
                        {
                            qryDegreeCode += ",'" + liBranch.Value + "'";
                        }
                    }
                }
                //if (!string.IsNullOrEmpty(qryDegreeCode.Trim()))
                //{
                //}
            }
            if (ddlsem.Items.Count == 0)
            {
                lblPopAlert.Text = "Semester is Not Found";
                divPopUpAlert.Visible = true;
                return;
            }
            else
            {
                semester = Convert.ToString(ddlsem.SelectedValue).Trim();
                qrySemester = string.Empty;
                foreach (ListItem liSem in ddlsem.Items)
                {
                    if (liSem.Selected)
                    {
                        selectedCount++;
                        if (string.IsNullOrEmpty(qrySemester.Trim()))
                        {
                            qrySemester = "'" + liSem.Value + "'";
                        }
                        else
                        {
                            qrySemester += ",'" + liSem.Value + "'";
                        }
                    }
                }
                //if (!string.IsNullOrEmpty(qrySemester.Trim()))
                //{
                //}
            }
            if (chkEnableSubjectType.Checked)
            {
                qrySubjectType = string.Empty;
                if (ddl_SubType.Items.Count > 0)
                {
                    subjectType = Convert.ToString(ddl_SubType.SelectedItem.Text).Trim();
                    subjectTypeNo = Convert.ToString(ddl_SubType.SelectedItem.Value).Trim();
                    qrySubjectType = string.Empty;
                    foreach (ListItem liSubType in ddl_SubType.Items)
                    {
                        if (liSubType.Selected)
                        {
                            selectedCount++;
                            if (string.IsNullOrEmpty(qrySubjectType.Trim()))
                            {
                                qrySubjectType = "'" + liSubType.Value + "'";
                            }
                            else
                            {
                                qrySubjectType += ",'" + liSubType.Value + "'";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(qrySubjectType.Trim()))
                    {
                        qrySubjectType = " and ss.subType_no in(" + qrySubjectType + ")";
                    }
                }
                else
                {
                    lblPopAlert.Text = "Subject Type is Not Found";
                    divPopUpAlert.Visible = true;
                    return;
                }
            }
            if (chkEnableSubject.Checked)
            {
                qrySubjectType = string.Empty;
                if (cbl_Subjects.Items.Count == 0)
                {
                    lblPopAlert.Text = "Subject is Not Found";
                    divPopUpAlert.Visible = true;
                    return;
                }
                else
                {
                    selectedCount = 0;
                    qrySubjectNo = string.Empty;
                    foreach (ListItem liSubject in cbl_Subjects.Items)
                    {
                        if (liSubject.Selected)
                        {
                            selectedCount++;
                            if (string.IsNullOrEmpty(qrySubjectNo.Trim()))
                            {
                                qrySubjectNo = "'" + liSubject.Value + "'";
                            }
                            else
                            {
                                qrySubjectNo += ",'" + liSubject.Value + "'";
                            }
                        }
                    }
                    if (selectedCount == 0)
                    {
                        lblPopAlert.Text = "Subject is Not Found";
                        divPopUpAlert.Visible = true;
                        return;
                    }
                    if (!string.IsNullOrEmpty(qrySubjectNo.Trim()))
                    {
                        if (rblOptions.SelectedValue == "0")
                        {
                            qrySubjectNo = " and s.subject_no in(" + qrySubjectNo + ")";
                        }
                        else if (rblOptions.SelectedValue == "1")
                        {
                            qrySubjectNo = " and s.subject_code in(" + qrySubjectNo + ")";
                        }
                    }
                }
            }
            qry = "select ltrim(rtrim(isnull(c.type,''))) as Type,c.Edu_Level,c.Course_Name,dt.Dept_Name,dt.dept_acronym,case when(ltrim(rtrim(isnull(c.type,'')))<>'') then ltrim(rtrim(isnull(c.type,'')))+case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then ' '+c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end  else case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end end as DegreeDetails,sm.Batch_Year,sm.degree_code,sm.semester,sm.syll_code,ss.subType_no,ss.subject_type,ss.priority,s.subject_code,s.subject_no,s.subject_name,s.subjectpriority,s.Part_Type from subject s, sub_sem ss,syllabus_master sm,Course c,Department dt ,Degree dg where dg.Degree_Code=sm.degree_code and dt.Dept_Code=dg.Dept_Code and c.Course_Id=dg.Course_Id and s.subType_no=ss.subType_no and ss.syll_code=s.syll_code and ss.syll_code=sm.syll_code and s.syll_code=sm.syll_code and sm.Batch_Year in(" + qryBatchYear + ") and sm.degree_code in(" + qryDegreeCode + ") and sm.semester in(" + qrySemester + ") " + qrySubjectType + qrySubjectNo + " order by sm.Batch_Year desc,sm.degree_code,sm.semester asc,case when (s.Part_Type is not null and s.Part_Type<>0) then s.Part_Type else case when (ss.priority is not null and ss.priority<>0) then ss.priority else  ss.subType_no end end,case when (ss.priority is not null and ss.priority<>0) then ss.priority else ss.subType_no end,case when (s.subjectpriority is not null and s.subjectpriority<>0) then s.subjectpriority else s.subject_no end";
            DataSet dsSubjectList = new DataSet();// order by sm.Batch_Year desc,sm.degree_code,sm.semester asc,case when (s.Part_Type is not null and s.Part_Type<>0) then s.Part_Type else case when (ss.priority is not null and ss.priority<>0) then ss.priority else case when (s.subjectpriority is not null and s.subjectpriority<>0) then s.subjectpriority else ss.subType_no end end end,case when (ss.priority is not null and ss.priority<>0) then ss.priority else ss.subType_no end,case when (s.subjectpriority is not null and s.subjectpriority<>0) then s.subjectpriority else s.subject_no end
            dsSubjectList = d2.select_method_wo_parameter(qry, "Text");
            if (dsSubjectList.Tables.Count > 0 && dsSubjectList.Tables[0].Rows.Count > 0)
            {
                Init_Spread(FpSubjectsList);
                FpSubjectsList.Sheets[0].RowCount = 0;
                Farpoint.CheckBoxCellType chkCellAll = new Farpoint.CheckBoxCellType();
                chkCellAll.AutoPostBack = true;
                Farpoint.TextCellType txtCell = new Farpoint.TextCellType();
                Farpoint.CheckBoxCellType chkCell = new Farpoint.CheckBoxCellType();
                chkCell.AutoPostBack = false;
                //FpSubjectsList.Sheets[0].RowCount++;
                //FpSubjectsList.Sheets[0].Columns[0].CellType = chkCell;
                //FpSubjectsList.Sheets[0].Cells[0, 0].CellType = chkCellAll;
                //FpSubjectsList.Sheets[0].Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                //FpSubjectsList.Sheets[0].Cells[0, 0].VerticalAlign = VerticalAlign.Middle;
                //FpSubjectsList.Sheets[0].AddSpanCell(0, 1, 1, FpSubjectsList.Sheets[0].ColumnCount - 1);
                //FpSubjectsList.Sheets[0].FrozenRowCount = 1;
                string type = string.Empty;
                string eduLevel = string.Empty;
                string courseName = string.Empty;
                string departmentName = string.Empty;
                string departmentAcrronymn = string.Empty;
                string degreeDetails = string.Empty;
                string batchYearNew = string.Empty;
                string degreeCodeNew = string.Empty;
                string semesterNew = string.Empty;
                string syllCode = string.Empty;
                string subjectTypeNoNew = string.Empty;
                string subjectTypeNew = string.Empty;
                string subjectName = string.Empty;
                string subjectCode = string.Empty;
                string subjectNoNew = string.Empty;
                string partType = string.Empty;
                int serialNo = 0;
                foreach (DataRow drSubjectList in dsSubjectList.Tables[0].Rows)
                {
                    serialNo++;
                    type = string.Empty;
                    eduLevel = string.Empty;
                    courseName = string.Empty;
                    departmentName = string.Empty;
                    departmentAcrronymn = string.Empty;
                    degreeDetails = string.Empty;
                    batchYearNew = string.Empty;
                    degreeCodeNew = string.Empty;
                    semesterNew = string.Empty;
                    syllCode = string.Empty;
                    subjectTypeNoNew = string.Empty;
                    subjectTypeNew = string.Empty;
                    subjectName = string.Empty;
                    subjectCode = string.Empty;
                    subjectNoNew = string.Empty;
                    partType = string.Empty;
                    type = Convert.ToString(drSubjectList["Type"]).Trim();
                    eduLevel = Convert.ToString(drSubjectList["Edu_Level"]).Trim();
                    courseName = Convert.ToString(drSubjectList["Course_Name"]).Trim();
                    departmentName = Convert.ToString(drSubjectList["Dept_Name"]).Trim();
                    departmentAcrronymn = Convert.ToString(drSubjectList["dept_acronym"]).Trim();
                    degreeDetails = Convert.ToString(drSubjectList["DegreeDetails"]).Trim();
                    batchYearNew = Convert.ToString(drSubjectList["Batch_Year"]).Trim();
                    degreeCodeNew = Convert.ToString(drSubjectList["degree_code"]).Trim();
                    semesterNew = Convert.ToString(drSubjectList["semester"]).Trim();
                    syllCode = Convert.ToString(drSubjectList["syll_code"]).Trim();
                    subjectTypeNoNew = Convert.ToString(drSubjectList["subType_no"]).Trim();
                    subjectTypeNew = Convert.ToString(drSubjectList["subject_type"]).Trim();
                    subjectName = Convert.ToString(drSubjectList["subject_name"]).Trim();
                    subjectCode = Convert.ToString(drSubjectList["subject_code"]).Trim();
                    subjectNoNew = Convert.ToString(drSubjectList["subject_no"]).Trim();
                    partType = Convert.ToString(drSubjectList["Part_Type"]).Trim();
                    FpSubjectsList.Sheets[0].RowCount++;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 0].CellType = chkCell;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 0].Locked = false;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(serialNo).Trim();
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 1].CellType = txtCell;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 1].Locked = true;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(batchYearNew).Trim();
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 2].Locked = true;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(degreeDetails).Trim();
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 3].CellType = txtCell;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(degreeCodeNew).Trim();
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 3].Locked = true;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(semesterNew).Trim();
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 4].CellType = txtCell;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 4].Locked = true;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(subjectTypeNew).Trim();
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(subjectTypeNoNew).Trim();
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 5].CellType = txtCell;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 5].Locked = true;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;

                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(subjectCode).Trim();
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(subjectNoNew).Trim();
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 6].CellType = txtCell;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 6].Locked = true;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;

                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(subjectName).Trim();
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 7].CellType = txtCell;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 7].Locked = true;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;

                    string partNames = string.Empty;
                    if (string.IsNullOrEmpty(partType.Trim()) || partType.Trim() == "0")
                    {
                        partNames = string.Empty;
                    }
                    else
                    {
                        partNames = partType.Trim();
                    }
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(partNames).Trim();//"Part " +
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 8].Tag = Convert.ToString(partType).Trim();
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 8].CellType = txtCell;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 8].Locked = false;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                    FpSubjectsList.Sheets[0].Cells[FpSubjectsList.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                }
                FpSubjectsList.Sheets[0].PageSize = FpSubjectsList.Sheets[0].RowCount;
                FpSubjectsList.Width = 980;
                FpSubjectsList.Height = 500;
                FpSubjectsList.SaveChanges();
                FpSubjectsList.Visible = true;
                divSubjectDetails.Visible = true;
                divPrint1.Visible = true;
            }
            else
            {
                lblPopAlert.Text = "No Record(s) Were Found";
                divPopUpAlert.Visible = true;
                return;
            }
        }
        catch
        {
        }
    }

    public void Init_Spread(Farpoint.FpSpread FpSpread1, int type = 0)
    {
        try
        {
            #region FpSpread Style

            FpSpread1.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;

            #endregion FpSpread Style

            #region SpreadStyles

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.Font.Bold = true;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.VerticalAlign = VerticalAlign.Middle;
            darkstyle.ForeColor = System.Drawing.Color.Black;
            darkstyle.Border.BorderSize = 1;
            darkstyle.Border.BorderColor = System.Drawing.Color.Black;

            FarPoint.Web.Spread.StyleInfo sheetstyle = new FarPoint.Web.Spread.StyleInfo();
            sheetstyle.Font.Name = "Book Antiqua";
            sheetstyle.Font.Size = FontUnit.Medium;
            sheetstyle.Font.Bold = true;
            sheetstyle.HorizontalAlign = HorizontalAlign.Center;
            sheetstyle.VerticalAlign = VerticalAlign.Middle;
            sheetstyle.ForeColor = System.Drawing.Color.Black;
            sheetstyle.Border.BorderSize = 1;
            sheetstyle.Border.BorderColor = System.Drawing.Color.Black;

            #endregion SpreadStyles

            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.Sheets[0].DefaultStyle = sheetstyle;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;

            FpSpread1.HorizontalScrollBarPolicy = Farpoint.ScrollBarPolicy.AsNeeded;
            FpSpread1.VerticalScrollBarPolicy = Farpoint.ScrollBarPolicy.AsNeeded;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.RowHeader.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = false;
            FpSpread1.Sheets[0].RowCount = 0;
            if (type == 0)
            {
                FpSpread1.Sheets[0].ColumnCount = 9;

                FpSpread1.Sheets[0].Columns[0].Width = 45;
                FpSpread1.Sheets[0].Columns[1].Width = 38;
                FpSpread1.Sheets[0].Columns[2].Width = 60;
                FpSpread1.Sheets[0].Columns[3].Width = 165;
                FpSpread1.Sheets[0].Columns[4].Width = 75;
                FpSpread1.Sheets[0].Columns[5].Width = 165;
                FpSpread1.Sheets[0].Columns[6].Width = 100;
                FpSpread1.Sheets[0].Columns[7].Width = 260;
                FpSpread1.Sheets[0].Columns[8].Width = 45;

                FpSpread1.Sheets[0].Columns[0].Locked = false;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[5].Locked = true;
                FpSpread1.Sheets[0].Columns[6].Locked = true;
                FpSpread1.Sheets[0].Columns[7].Locked = true;
                FpSpread1.Sheets[0].Columns[8].Locked = false;

                FpSpread1.Sheets[0].Columns[0].Resizable = false;
                FpSpread1.Sheets[0].Columns[1].Resizable = false;
                FpSpread1.Sheets[0].Columns[2].Resizable = false;
                FpSpread1.Sheets[0].Columns[3].Resizable = false;
                FpSpread1.Sheets[0].Columns[4].Resizable = false;
                FpSpread1.Sheets[0].Columns[5].Resizable = false;
                FpSpread1.Sheets[0].Columns[6].Resizable = false;
                FpSpread1.Sheets[0].Columns[7].Resizable = false;
                FpSpread1.Sheets[0].Columns[8].Resizable = false;

                FpSpread1.Sheets[0].Columns[0].Visible = true;
                FpSpread1.Sheets[0].Columns[1].Visible = true;
                FpSpread1.Sheets[0].Columns[2].Visible = true;
                FpSpread1.Sheets[0].Columns[3].Visible = true;
                FpSpread1.Sheets[0].Columns[4].Visible = true;
                FpSpread1.Sheets[0].Columns[5].Visible = true;
                FpSpread1.Sheets[0].Columns[6].Visible = true;
                FpSpread1.Sheets[0].Columns[7].Visible = true;
                FpSpread1.Sheets[0].Columns[8].Visible = true;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Select";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Batch";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Degree";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Semester";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Subject Type";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Subject_Code";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Subject Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Part";

                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);

                FpSpread1.Sheets[0].SetColumnMerge(2, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(3, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(4, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(5, Farpoint.Model.MergePolicy.Always);

            }
            else
            {
                FpSpread1.Sheets[0].ColumnCount = 9;

                FpSpread1.Sheets[0].Columns[0].Width = 45;
                FpSpread1.Sheets[0].Columns[1].Width = 30;
                FpSpread1.Sheets[0].Columns[2].Width = 80;
                FpSpread1.Sheets[0].Columns[3].Width = 150;
                FpSpread1.Sheets[0].Columns[4].Width = 50;
                FpSpread1.Sheets[0].Columns[5].Width = 150;
                FpSpread1.Sheets[0].Columns[6].Width = 100;
                FpSpread1.Sheets[0].Columns[7].Width = 240;
                FpSpread1.Sheets[0].Columns[8].Width = 80;

                FpSpread1.Sheets[0].Columns[0].Locked = false;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[5].Locked = true;
                FpSpread1.Sheets[0].Columns[6].Locked = true;
                FpSpread1.Sheets[0].Columns[7].Locked = true;
                FpSpread1.Sheets[0].Columns[8].Locked = false;

                FpSpread1.Sheets[0].Columns[0].Resizable = false;
                FpSpread1.Sheets[0].Columns[1].Resizable = false;
                FpSpread1.Sheets[0].Columns[2].Resizable = false;
                FpSpread1.Sheets[0].Columns[3].Resizable = false;
                FpSpread1.Sheets[0].Columns[4].Resizable = false;
                FpSpread1.Sheets[0].Columns[5].Resizable = false;
                FpSpread1.Sheets[0].Columns[6].Resizable = false;
                FpSpread1.Sheets[0].Columns[7].Resizable = false;
                FpSpread1.Sheets[0].Columns[8].Resizable = false;

                FpSpread1.Sheets[0].Columns[0].Visible = true;
                FpSpread1.Sheets[0].Columns[1].Visible = true;
                FpSpread1.Sheets[0].Columns[2].Visible = true;
                FpSpread1.Sheets[0].Columns[3].Visible = true;
                FpSpread1.Sheets[0].Columns[4].Visible = true;
                FpSpread1.Sheets[0].Columns[5].Visible = true;
                FpSpread1.Sheets[0].Columns[6].Visible = true;
                FpSpread1.Sheets[0].Columns[7].Visible = true;
                FpSpread1.Sheets[0].Columns[8].Visible = true;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Select";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Batch Year";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Degree";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Semester";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Subject Type";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Subject_Code";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Subject Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Part Type";

                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnSetPart_Click(object sender, EventArgs e)
    {
        try
        {
            FpSubjectsList.SaveChanges();
            string partNo = txtSelectPart.Text.Trim();
            bool isSet = false;
            txtSelectPart.Text = string.Empty;
            if (string.IsNullOrEmpty(partNo.Trim()))
            {
                lblPopAlert.Text = "Please Enter Part Number";
                divPopUpAlert.Visible = true;
                return;
            }
            else
            {
                if (partNo.Trim() == "0")
                {
                    lblPopAlert.Text = "Please Enter Valid Part Number Other Than 0";
                    divPopUpAlert.Visible = true;
                    return;
                }
                if (FpSubjectsList.Sheets[0].RowCount == 0)
                {
                    lblPopAlert.Text = "No Record(s) Were Found";
                    divPopUpAlert.Visible = true;
                    return;
                }
                else
                {
                    for (int row = 0; row < FpSubjectsList.Sheets[0].RowCount; row++)
                    {
                        int val = 0;
                        //val = FpSubjectsList.Sheets[0].Cells[row, 0].Value;
                        int.TryParse(Convert.ToString(FpSubjectsList.Sheets[0].Cells[row, 0].Value).Trim(), out val);
                        if (val == 1)
                        {
                            isSet = true;
                            FpSubjectsList.Sheets[0].Cells[row, FpSubjectsList.Sheets[0].ColumnCount - 1].Text = partNo;
                            FpSubjectsList.Sheets[0].Cells[row, FpSubjectsList.Sheets[0].ColumnCount - 1].Value = partNo;
                        }
                        FpSubjectsList.Sheets[0].Cells[row, 0].Value = 0;
                    }
                    FpSubjectsList.SaveChanges();
                }
            }
            if (!isSet)
            {
                lblPopAlert.Text = "Please Select Any Subject And Then Proceed";
                divPopUpAlert.Visible = true;
                return;
            }
        }
        catch
        {
        }
    }

    protected void btnSavePart_Click(object sender, EventArgs e)
    {
        try
        {
            FpSubjectsList.SaveChanges();
            txtSelectPart.Text = string.Empty;
            bool isSet = false;
            if (FpSubjectsList.Sheets[0].RowCount == 0)
            {
                lblPopAlert.Text = "No Record(s) Were Found";
                divPopUpAlert.Visible = true;
                return;
            }
            else
            {
                for (int row = 0; row < FpSubjectsList.Sheets[0].RowCount; row++)
                {
                    string subjectNo = Convert.ToString(FpSubjectsList.Sheets[0].Cells[row, 6].Tag).Trim();
                    string partNo = Convert.ToString(FpSubjectsList.Sheets[0].Cells[row, 8].Value).Trim();
                    string updateQ = string.Empty;
                    if (!string.IsNullOrEmpty(partNo) && !string.IsNullOrEmpty(subjectNo) && partNo != "0")
                    {
                        int res = d2.update_method_wo_parameter("update subject set Part_Type='" + partNo + "' where subject_no='" + subjectNo + "'", "Text");
                        if (res > 0)
                        {
                            isSet = true;
                        }
                    }
                }
            }
            if (isSet)
            {
                lblPopAlert.Text = "Saved Successfully";
                divPopUpAlert.Visible = true;
                return;
            }
            else
            {
                lblPopAlert.Text = "Not Saved";
                divPopUpAlert.Visible = true;
                return;
            }
            btnView_Click(sender, e);
        }
        catch
        {
        }
    }

    #region Generate Excel

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            lblPopAlert.Text = string.Empty;
            divPopUpAlert.Visible = false;
            errmsg.Text = string.Empty;
            errmsg.Visible = false;
            Printcontrol1.Visible = false;
            string reportname = txtexcelname1.Text.Trim().Replace(" ", "_").Trim();
            if (Convert.ToString(reportname).Trim() != "")
            {
                if (FpSubjectsList.Visible == true)
                {
                    d2.printexcelreport(FpSubjectsList, reportname);
                }
                lbl_norec1.Visible = false;
            }
            else
            {
                lbl_norec1.Text = "Please Enter Your Report Name";
                lbl_norec1.Visible = true;
                txtexcelname1.Focus();
            }
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
        }
    }

    #endregion Generate Excel

    #region Print PDF

    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            lblPopAlert.Text = string.Empty;
            divPopUpAlert.Visible = false;
            errmsg.Text = string.Empty;
            errmsg.Visible = false;
            string rptheadname = string.Empty;
            rptheadname = "Subject Parts Allocation Report";
            string pagename = "SubjectPartsAllocation.aspx";
            string Course_Name = Convert.ToString(ddldegree.SelectedItem).Trim();
            rptheadname += "@ " + Course_Name + " - " + Convert.ToString(ddlbranch.SelectedItem).Trim() + "@ " + " Year of Admission : " + Convert.ToString(ddlbatch.SelectedItem).Trim() + "@ " + " Semester : " + Convert.ToString(ddlsem.SelectedItem).Trim();
            if (FpSubjectsList.Visible == true)
            {
                Printcontrol1.loadspreaddetails(FpSubjectsList, pagename, rptheadname);
            }
            Printcontrol1.Visible = true;
            lbl_norec1.Visible = false;
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
        }
    }

    #endregion Print PDF

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        lblPopAlert.Text = string.Empty;
        divPopUpAlert.Visible = false;
    }

}