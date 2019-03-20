using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Collections.Generic;
using System.Configuration;
public partial class StatewiseStrengthReport : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    Hashtable hat = new Hashtable();
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    ReuasableMethods rs = new ReuasableMethods();
    int i = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("StudentHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/StudentMod/StudentHome.aspx");
                    return;
                }
            }
            //****************************************************//
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                setLabelText();
                BindCollege();
                bindbatch();
                edu_level();
                degree();
                bindsem();
                BindSectionDetail();
                loadstream();
                bindstate();
                cbl_gender.Items.Clear();
                cbl_gender.Items.Add(new ListItem("Male", "0"));
                cbl_gender.Items.Add(new ListItem("Female", "1"));
                cb_gender.Checked = true;
                cb_gender_checkedchange(sender, e);
                lbl_Filtertype.Text = ddl_reporttype.SelectedItem.Text;
                lbl_state.Visible = true;
                state.Visible = true;
            }
        }
        catch (Exception ex) { }
    }
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

        lbl.Add(lbl_clgname);
        lbl.Add(lbl_Stream);
        lbl.Add(lbl_degree);

        lbl.Add(lbl_branch);
        lbl.Add(lbl_org_sem);
        fields.Add(0);
        fields.Add(1);
        fields.Add(2);

        fields.Add(3);
        fields.Add(4);

        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }
    void BindCollege()
    {
        try
        {
            //string srisql = "select collname,college_code from collinfo";
            ds.Clear();
            //ds = d2.select_method_wo_parameter(srisql, "Text");
            byte userType = 0;
            string userOrGroupCode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                userOrGroupCode = Convert.ToString(Session["group_code"]).Trim();
                userType = 0;
            }
            else if (Session["usercode"] != null)
            {
                userOrGroupCode = Convert.ToString(Session["usercode"]).Trim();
                userType = 1;
            }
            ds.Clear();
            ds = d2.BindCollegebaseonrights(userOrGroupCode, userType);
            ddlcollege.DataSource = ds;
            ddlcollege.DataTextField = "collname";
            ddlcollege.DataValueField = "college_code";
            ddlcollege.DataBind();
            bindBoardUniv();
        }
        catch
        {
        }
    }
    public void bindbatch()
    {
        try
        {
            hat.Clear();
            cbl_batch.Items.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_batch.DataSource = ds;
                cbl_batch.DataTextField = "batch_year";
                cbl_batch.DataValueField = "batch_year";
                cbl_batch.DataBind();
                if (cbl_batch.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_batch.Items.Count; i++)
                    {
                        cbl_batch.Items[0].Selected = true;
                    }
                    txt_batch.Text = "Batch(" + 1 + ")";
                }
                else
                {
                    txt_batch.Text = "--Select--";
                    cb_batch.Checked = false;
                }
            }
        }
        catch
        {
        }
        bindBoardUniv();
    }
    public void degree()
    {
        try
        {
            string edulvl = "";
            for (int i = 0; i < cbl_graduation.Items.Count; i++)
            {
                if (cbl_graduation.Items[i].Selected == true)
                {
                    string build = cbl_graduation.Items[i].Value.ToString();
                    if (edulvl == "")
                    {
                        edulvl = build;
                    }
                    else
                    {
                        edulvl = edulvl + "','" + build;
                    }
                }
            }
            string query = "";
            string type = "";
            if (txt_stream.Enabled == true)
            {
                type = rs.GetSelectedItemsValueAsString(cbl_stream);
            }
            string rights = "";
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if ((group_user.ToString().Trim() != "") && (group_user.Trim() != "0") && (group_user.ToString().Trim() != "-1"))
            {
                rights = "and group_code='" + group_user + "'";
            }
            else
            {
                rights = " and user_code='" + usercode + "'";
            }
            if (type != "")
            {
                query = "select distinct d.Course_Id,c.Course_Name from Degree d,course c ,DeptPrivilages p where p.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + ddlcollege.SelectedItem.Value + "' and Edu_Level in('" + edulvl + "') " + rights + " and type in('" + type + "')";
            }
            else
            {
                query = "select distinct d.Course_Id,c.Course_Name from Degree d,course c ,DeptPrivilages p where p.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + ddlcollege.SelectedItem.Value + "' and Edu_Level in('" + edulvl + "') " + rights + "";
            }
            ds.Clear();
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
                    //    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    //    {
                    cbl_degree.Items[0].Selected = true;
                }
                txt_degree.Text = lbl_degree.Text + "(" + 1 + ")";
                // cb_degree.Checked = true;
                //}
                //else
                //{
                //    txt_degree.Text = "--Select--";
                //    cb_degree.Checked = false;
                //}
                string deg = "";
                if (cbl_degree.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        if (cbl_degree.Items[i].Selected == true)
                        {
                            string build = cbl_degree.Items[i].Value.ToString();
                            if (deg == "")
                            {
                                deg = build;
                            }
                            else
                            {
                                deg = deg + "','" + build;
                            }
                        }
                    }
                }
                bindbranch(deg);
            }
            else
            {
                txt_degree.Text = "--Select--";
                cb_degree.Checked = false;
                cbl_degree.Items.Clear();
                txt_branch.Text = "--Select--";
                cb_branch.Checked = false;
                cbl_branch.Items.Clear();
                cb_sem.Checked = false;
                txt_sem.Text = "--Select--";
                cbl_sem.Items.Clear();
                cb_sec.Checked = false;
                txt_sec.Text = "--Select--";
                cbl_sec.Items.Clear();
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindbranch(string branch)
    {
        try
        {
            branch = "";
            if (cbl_degree.Items.Count > 0)
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    if (cbl_degree.Items[i].Selected == true)
                    {
                        string build = cbl_degree.Items[i].Value.ToString();
                        if (branch == "")
                        {
                            branch = build;
                        }
                        else
                        {
                            branch = branch + "','" + build;
                        }
                    }
                }
            }
            string rights = "";
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if ((group_user.ToString().Trim() != "") && (group_user.Trim() != "0") && (group_user.ToString().Trim() != "-1"))
            {
                rights = "and group_code='" + group_user + "'";
            }
            else
            {
                rights = " and user_code='" + usercode + "'";
            }
            cb_branch.Checked = false;
            string commname = "";
            if (branch != "")
            {
                commname = "select distinct convert(varchar(20), degree.degree_code)+'$'+convert(varchar(20), degree.No_Of_seats)+'$'+convert(varchar(20), course.Course_Name)as degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddlcollege.SelectedItem.Value + "' " + rights + "";
            }
            else
            {
                commname = " select distinct convert(varchar(20), degree.degree_code)+'$'+convert(varchar(20), degree.No_Of_seats)+'$'+convert(varchar(20), course.Course_Name)as degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddlcollege.SelectedItem.Value + "' " + rights + "";
            }
            ds.Clear();
            cbl_branch.Items.Clear();
            ds = d2.select_method(commname, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_branch.DataSource = ds;
                cbl_branch.DataTextField = "dept_name";
                cbl_branch.DataValueField = "degree_code";
                cbl_branch.DataBind();
                if (cbl_branch.Items.Count > 0)
                {
                    cbl_branch.Items[0].Selected = true;
                }
                txt_branch.Text = lbl_branch.Text + "(" + 1 + ")";
            }
            bindBoardUniv();
        }
        catch (Exception ex)
        {
        }
    }
    public void bindsem()
    {
        cbl_sem.Items.Clear();
        txt_sem.Text = "--Select--";
        int i = 0;
        ds.Clear();
        string branch = "";
        string build = "";
        string build1 = "";
        string batch = "";
        if (cbl_branch.Items.Count > 0)
        {
            for (i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    build = cbl_branch.Items[i].Value.ToString().Split('$')[0];
                    if (branch == "")
                    {
                        branch = build;
                    }
                    else
                    {
                        branch = branch + "," + build;
                    }
                }
            }
        }
        if (cbl_batch.Items.Count > 0)
        {
            for (i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    build1 = cbl_batch.Items[i].Value.ToString();
                    if (batch == "")
                    {
                        batch = build1;
                    }
                    else
                    {
                        batch = batch + "," + build1;
                    }
                }
            }
        }
        //batch = build;
        if (branch.Trim() != "" && batch.Trim() != "")
        {
            // ds = d2.BindSem(branch, batch, ddlcollege.SelectedItem.Value);
            string strsql1 = "select distinct duration,first_year_nonsemester  from degree where degree_code in (" + branch + ") and college_code=" + ddlcollege.SelectedItem.Value + " order by Duration desc";
            ds = d2.select_method_wo_parameter(strsql1, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                int dur = 0;
                int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]), out dur);
                for (i = 1; i <= dur; i++)
                {
                    cbl_sem.Items.Add(Convert.ToString(i));
                    cbl_sem.Items[i - 1].Selected = true;
                    cb_sem.Checked = true;
                }
                txt_sem.Text = lbl_org_sem.Text + "(" + cbl_sem.Items.Count + ")";
            }
        }
    }
    public void BindSectionDetail()
    {
        try
        {
            cbl_sec.Items.Clear();
            string batch = "";
            string branch = "";
            int i = 0;
            if (cbl_branch.Items.Count > 0)
            {
                for (i = 0; i < cbl_branch.Items.Count; i++)
                {
                    if (cbl_branch.Items[i].Selected == true)
                    {
                        string build = cbl_branch.Items[i].Value.ToString().Split('$')[0];
                        if (branch == "")
                        {
                            branch = build;
                        }
                        else
                        {
                            branch = branch + "','" + build;
                        }
                    }
                }
            }
            batch = rs.GetSelectedItemsValueAsString(cbl_batch);
            if (batch.Trim() != "" && branch.Trim() != "")
            {
                string sqlquery = "select distinct sections from registration where batch_year in('" + batch + "') and degree_code in('" + branch + "') and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar' order by sections";
                DataSet ds = new DataSet();
                ds = d2.select_method_wo_parameter(sqlquery, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_sec.DataSource = ds;
                    cbl_sec.DataTextField = "sections";
                    cbl_sec.DataValueField = "sections";
                    cbl_sec.DataBind();
                    if (cbl_sem.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_sec.Items.Count; row++)
                        {
                            cbl_sec.Items[row].Selected = true;
                            cb_sec.Checked = true;
                        }
                        txt_sec.Text = "Section(" + cbl_sec.Items.Count + ")";
                    }
                    else
                    {
                        txt_sec.Text = "--Select--";
                    }
                }
                else
                {
                    txt_sec.Text = "--Select--";
                }
            }
        }
        catch
        {
        }
    }
    public void loadstream()
    {
        try
        {
            string stream = "";
            cbl_stream.Items.Clear();
            string deptquery = "select distinct type from Course where type is not null and type<>'' and college_code  in ('" + ddlcollege.SelectedItem.Value + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_stream.DataSource = ds;
                cbl_stream.DataTextField = "type";
                cbl_stream.DataBind();
                if (cbl_stream.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_stream.Items.Count; i++)
                    {
                        cbl_stream.Items[i].Selected = true;
                    }
                    txt_stream.Text = lbl_Stream.Text + "(" + cbl_stream.Items.Count + ")";
                    cb_stream.Checked = true;
                    txt_stream.Enabled = true;
                }
                else
                {
                    txt_stream.Text = "--Select--";
                    cb_stream.Checked = false;
                    txt_stream.Enabled = false;
                }
            }
            else
            {
                txt_stream.Enabled = false;
            }
        }
        catch
        {
        }
    }
    public void edu_level()
    {
        string st = "";
        string type = rs.GetSelectedItemsValueAsString(cbl_stream);
        if (type != "")
        {
            st = "select distinct edu_level,priority from course where college_code='" + ddlcollege.SelectedItem.Value + "' and type in('" + type + "') order by priority";
        }
        else
        {
            st = "select distinct edu_level,priority from course where college_code='" + ddlcollege.SelectedItem.Value + "' order by priority";
        }
        ds = d2.select_method_wo_parameter(st, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_graduation.DataSource = ds;
            cbl_graduation.DataTextField = "edu_level";
            cbl_graduation.DataValueField = "edu_level";
            cbl_graduation.DataBind();
            if (cbl_graduation.Items.Count > 0)
            {
                cbl_graduation.Items[0].Selected = true;
            }
            txt_graduation.Text = "Graduation(" + 1 + ")";
        }
    }
    public void bindstate()
    {
        try
        {
            string comm = "";
            string selq = " select distinct parent_statep,t.textval from Registration r, applyn a,textvaltable t where r.App_No=a.app_no and a.parent_statep=t.TextCode and isnull(parent_statep,-1)<>'-1' and t.textval<>'' and R.college_code ='" + ddlcollege.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_state.DataSource = ds;
                    cbl_state.DataTextField = "TextVal";
                    cbl_state.DataValueField = "parent_statep";
                    cbl_state.DataBind();
                    if (cbl_state.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_state.Items.Count; i++)
                        {
                            cbl_state.Items[i].Selected = true;
                            comm = Convert.ToString(cbl_state.Items[i].Text);
                        }
                        if (cbl_state.Items.Count == 1)
                        {
                            txt_comm.Text = "" + comm + "";
                        }
                        else
                        {
                            txt_comm.Text = "State(" + cbl_state.Items.Count + ")";
                        }
                        cb_state.Checked = true;
                    }
                }
            }
            else
            {
                txt_comm.Text = "--Select--";
                cb_state.Checked = false;
            }
        }
        catch
        {
        }
    }
    private void bindBoardUniv()
    {
        cbl_BoardUniv.Items.Clear();
        try
        {
            if (ddlcollege.Items.Count > 0 && cbl_batch.Items.Count > 0 && cbl_branch.Items.Count > 0)
            {
                string degreecode = GetSelectedItemsValueAsString(cbl_branch).Split('$')[0];
                string batch = rs.GetSelectedItemsValueAsString(cbl_batch);
                if (degreecode.Trim() != "" && batch.Trim() != "")
                {
                    string QryType = "S.Course_code";
                    if (ddl_reporttype.SelectedItem.Value == "2")
                        QryType = " S.university_code";
                    string query = "   select distinct TextVal,TextCode from textvaltable t,Stud_prev_details s,applyn a where T.TextCode=" + QryType + " and a.app_no=s.app_no and a.batch_year in('" + batch + "') and a.degree_code in('" + degreecode + "')  and t.college_code=" + ddlcollege.SelectedItem.Value + " and Textval is not null and Textval<>'' order by Textval asc";
                    DataSet dsBoardUniv = d2.select_method_wo_parameter(query, "Text");
                    if (dsBoardUniv.Tables.Count > 0 && dsBoardUniv.Tables[0].Rows.Count > 0)
                    {
                        cbl_BoardUniv.DataSource = dsBoardUniv;
                        cbl_BoardUniv.DataTextField = "textval";
                        cbl_BoardUniv.DataValueField = "TextCode";
                        cbl_BoardUniv.DataBind();
                        for (int i = 0; i < cbl_BoardUniv.Items.Count; i++)
                        {
                            cbl_BoardUniv.Items[i].Selected = true;
                        }
                        txtBoardUniv.Text = lbl_Filtertype.Text + "(" + cbl_BoardUniv.Items.Count + ")";
                        cb_BoardUniv.Checked = true;
                    }
                }
            }
        }
        catch { }
    }
    public void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadstream();
        edu_level();
        degree();
        bindbatch();
        bindsem();
        BindSectionDetail();
        bindBoardUniv();
    }
    public void cb_stream_checkedchange(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_stream, cbl_stream, txt_stream, "Stream", "--Select--");
            edu_level();
            degree();
        }
        catch { }
    }
    public void cbl_stream_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_stream, cbl_stream, txt_stream, "Stream");
            edu_level();
            degree();
        }
        catch { }
    }
    public void cb_graduation_checkedchange(object sender, EventArgs e)
    {
        try
        {
            string deg = "";
            if (cb_graduation.Checked == true)
            {
                for (int i = 0; i < cbl_graduation.Items.Count; i++)
                {
                    cbl_graduation.Items[i].Selected = true;
                }
                txt_graduation.Text = "Edu Level(" + (cbl_graduation.Items.Count) + ")";
                if (cbl_degree.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        if (cbl_degree.Items[i].Selected == true)
                        {
                            string build = cbl_degree.Items[i].Value.ToString();
                            if (deg == "")
                            {
                                deg = build;
                            }
                            else
                            {
                                deg = deg + "','" + build;
                            }
                        }
                    }
                }
                degree();
                bindbranch(deg);
                bindsem();
                BindSectionDetail();
                bindBoardUniv();
            }
            else
            {
                for (int i = 0; i < cbl_graduation.Items.Count; i++)
                {
                    cbl_graduation.Items[i].Selected = false;
                }
                txt_graduation.Text = "--Select--";
                txt_degree.Text = "--Select--";
                cb_degree.Checked = false;
                cbl_degree.Items.Clear();
                txt_branch.Text = "--Select--";
                cb_branch.Checked = false;
                cbl_branch.Items.Clear();
                cb_sem.Checked = false;
                txt_sem.Text = "--Select--";
                cbl_sem.Items.Clear();
                cb_sec.Checked = false;
                txt_sec.Text = "--Select--";
                cbl_sec.Items.Clear();
            }
        }
        catch
        {
        }
    }
    public void cbl_graduation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            string deg = "";
            txt_graduation.Text = "--Select--";
            cb_graduation.Checked = false;
            for (int i = 0; i < cbl_graduation.Items.Count; i++)
            {
                if (cbl_graduation.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_graduation.Items.Count)
            {
                txt_graduation.Text = "Edu Level(" + commcount.ToString() + ")";
                cb_graduation.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_graduation.Text = "--Select--";
                txt_graduation.Text = "--Select--";
                txt_degree.Text = "--Select--";
                cb_degree.Checked = false;
                cbl_degree.Items.Clear();
                txt_branch.Text = "--Select--";
                cb_branch.Checked = false;
                cbl_branch.Items.Clear();
                cb_sem.Checked = false;
                txt_sem.Text = "--Select--";
                cbl_sem.Items.Clear();
                cb_sec.Checked = false;
                txt_sec.Text = "--Select--";
                cbl_sec.Items.Clear();
            }
            else
            {
                txt_graduation.Text = "Edu Level(" + commcount.ToString() + ")";
                if (cbl_degree.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        if (cbl_degree.Items[i].Selected == true)
                        {
                            string build = cbl_degree.Items[i].Value.ToString();
                            if (deg == "")
                            {
                                deg = build;
                            }
                            else
                            {
                                deg = deg + "','" + build;
                            }
                        }
                    }
                }
            }
            degree();
            bindbranch(deg);
            bindsem();
            BindSectionDetail();
            bindBoardUniv();
        }
        catch
        {
        }
    }
    public void cb_batch_checkedchange(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_batch, cbl_batch, txt_batch, "Batch", "--Select--");
            bindsem();
            BindSectionDetail();
            bindBoardUniv();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_batch, cbl_batch, txt_batch, "Batch");
            bindsem();
            BindSectionDetail();
            bindBoardUniv();
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_degree_checkedchange(object sender, EventArgs e)
    {
        try
        {
            string buildvalue1 = "";
            string build1 = "";
            if (cb_degree.Checked == true)
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    if (cb_degree.Checked == true)
                    {
                        cbl_degree.Items[i].Selected = true;
                        txt_degree.Text = lbl_degree.Text + "(" + (cbl_degree.Items.Count) + ")";
                        build1 = cbl_degree.Items[i].Value.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                        }
                    }
                }
                bindbranch(buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = false;
                    txt_degree.Text = "--Select--";
                    txt_branch.Text = "--Select--";
                    cbl_branch.ClearSelection();
                    cb_branch.Checked = false;
                }
            }
            bindsem();
            BindSectionDetail();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_degree.Checked = false;
            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_branch.Text = "--Select--";
                    build = cbl_degree.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "'" + "," + "'" + build;
                    }
                }
            }
            bindbranch(buildvalue);
            if (seatcount == cbl_degree.Items.Count)
            {
                txt_degree.Text = lbl_degree.Text + "(" + seatcount.ToString() + ")";
                cb_degree.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_degree.Text = "--Select--";
            }
            else
            {
                txt_degree.Text = lbl_degree.Text + "(" + seatcount.ToString() + ")";
            }
            bindsem();
            BindSectionDetail();
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_branch_checkedchange(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_branch, cbl_branch, txt_branch, "Branch", "--Select--");
            BindSectionDetail();
            bindBoardUniv();
            bindsem();
        }
        catch
        {
        }
    }
    public void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_branch, cbl_branch, txt_branch, "Branch");
            BindSectionDetail();
            bindBoardUniv();
            bindsem();
        }
        catch
        {
        }
    }
    public void cb_sem_checkedchange(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_sem, cbl_sem, txt_sem, lbl_org_sem.Text, "--Select--");
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_sem, cbl_sem, txt_sem, lbl_org_sem.Text);
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_sec_checkedchange(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_sec, cbl_sec, txt_sec, "Section", "--Select--");
        }
        catch
        {
        }
    }
    public void cbl_sec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_sec, cbl_sec, txt_sec, "Section");
        }
        catch
        {
        }
    }
    public void cb_state_checkedchange(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_state, cbl_state, txt_comm, "State", "--Select--");
        }
        catch
        {
        }
    }
    public void cbl_state_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_state, cbl_state, txt_comm, "State");
        }
        catch
        {
        }
    }
    protected void cb_gender_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_gender, cbl_gender, txt_gender, "Gender", "--Select--");
    }
    protected void cbl_gender_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_gender, cbl_gender, txt_gender, "Gender");
    }
    protected void cb_BoardUniv_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_BoardUniv, cbl_BoardUniv, txtBoardUniv, lbl_Filtertype.Text, "--Select--");
    }
    protected void cbl_BoardUniv_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_BoardUniv, cbl_BoardUniv, txtBoardUniv, lbl_Filtertype.Text);
    }
    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst)
    {
        try
        {
            int sel = 0;
            int count = 0;
            cb.Checked = false;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = dipst + "(" + count + ")";
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
            else
            {
                txt.Text = "--Select--";
            }
        }
        catch { }
    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
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
    protected void ddl_reporttype_selectedchange(object sender, EventArgs e)
    {
        lbl_Filtertype.Text = ddl_reporttype.SelectedItem.Text;
        lbl_state.Visible = true;
        if (ddl_reporttype.SelectedItem.Value == "0")
        {
            state.Visible = true;
            td_board.Visible = false;
        }
        else if (ddl_reporttype.SelectedItem.Value == "1" || ddl_reporttype.SelectedItem.Value == "2")
        {
            state.Visible = false;
            td_board.Visible = true;
            bindBoardUniv();
        }
        if (ddl_reporttype.SelectedItem.Value == "3")
        {
            lbl_state.Visible = false;
            lbl_Filtertype.Text = "";
            state.Visible = false;
            td_board.Visible = false;
        }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        bool showrecord = false;
        Printcontrol.Visible = false; lbl_error.Visible = false;
        Fpspread1.Sheets[0].RowCount = 0;
        Fpspread1.Sheets[0].ColumnCount = 0;
        Fpspread1.CommandBar.Visible = false;
        Fpspread1.Sheets[0].AutoPostBack = true;
        Fpspread1.Sheets[0].RowHeader.Visible = false;
        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        darkstyle.ForeColor = Color.Black;
        Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
        int statusselectcount = 0;
        int genderselectcount = 0;
        int comselectcount = 0;
        int columncount = 0;
        int boardcount = 0;
        int rowheader = 0;
        columncount += statusselectcount;
        int.TryParse(cblselecteditemcount(cbl_gender), out genderselectcount);
        columncount += genderselectcount;
        rowheader++;
        int.TryParse(cblselecteditemcount(cbl_state), out comselectcount);
        columncount += comselectcount; rowheader++;
        int col = 0;
        Fpspread1.Sheets[0].ColumnHeader.RowCount = rowheader;
        Hashtable totalvalue_dic = new Hashtable();
        string batchyear = rs.GetSelectedItemsValueAsString(cbl_batch);
        string degreecode = GetSelectedItemsValueAsString(cbl_branch);
        string sem = rs.GetSelectedItemsValueAsString(cbl_sem);
        string sec = rs.GetSelectedItemsValueAsString(cbl_sec);
        string comm = rs.GetSelectedItemsValueAsString(cbl_state);
        if (!string.IsNullOrEmpty(batchyear) && !string.IsNullOrEmpty(degreecode) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(degreecode))
        {
            #region State
            Fpspread1.Sheets[0].Columns.Count++;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "S.No";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            Fpspread1.Sheets[0].Columns.Count++;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Department";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            Fpspread1.Sheets[0].Columns.Count++;
            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, rowheader, 1);
            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, rowheader, 1);
            col = Fpspread1.Sheets[0].Columns.Count - 1;
            if (cbl_state.Items.Count > 0)
            {
                bool booGeder = false;
                int gendColCnt = 0;
                if (ddl_reporttype.SelectedItem.Value == "0")
                {
                    #region State
                    for (int j = 0; j < cbl_state.Items.Count; j++)
                    {
                        if (cbl_state.Items[j].Selected == true)
                        {
                            if (booGeder)
                            {
                                Fpspread1.Sheets[0].ColumnCount++;
                            }
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_state.Items[j].Text;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_state.Items[j].Value;
                            if (cbl_gender.Items.Count > 0)
                            {
                                bool regibool = false;
                                gendColCnt = Fpspread1.Sheets[0].ColumnCount - 1;
                                for (int k = 0; k < cbl_gender.Items.Count; k++)
                                {
                                    if (cbl_gender.Items[k].Selected == true)
                                    {
                                        if (regibool)
                                        {
                                            Fpspread1.Sheets[0].ColumnCount++;
                                        }
                                        Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_gender.Items[k].Text;
                                        Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_gender.Items[k].Value;
                                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_state.Items[j].Value;
                                        showrecord = true;
                                        regibool = true;
                                        booGeder = true;
                                    }
                                }
                                if (showrecord)
                                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, gendColCnt, 1, genderselectcount);
                            }
                        }
                    }
                    #endregion
                }
                if (ddl_reporttype.SelectedItem.Value == "1" || ddl_reporttype.SelectedItem.Value == "2" || ddl_reporttype.SelectedItem.Value == "3")
                {
                    #region board or University
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ddl_reporttype.SelectedItem.Text);
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ddl_reporttype.SelectedItem.Value);
                    CheckBoxList cblList = new CheckBoxList();
                    if (ddl_reporttype.SelectedItem.Value == "3")
                        cblList = cbl_gender;
                    else if (ddl_reporttype.SelectedItem.Value == "1" || ddl_reporttype.SelectedItem.Value == "2")
                        cblList = cbl_BoardUniv;
                    int.TryParse(cblselecteditemcount(cblList), out boardcount);
                    if (cblList.Items.Count > 0)
                    {
                        bool regibool = false;
                        gendColCnt = Fpspread1.Sheets[0].ColumnCount - 1;
                        for (int k = 0; k < cblList.Items.Count; k++)
                        {
                            if (cblList.Items[k].Selected == true)
                            {
                                if (regibool)
                                {
                                    Fpspread1.Sheets[0].ColumnCount++;
                                }
                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Text = cblList.Items[k].Text;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cblList.Items[k].Value;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ddl_reporttype.SelectedItem.Value);
                                showrecord = true;
                                regibool = true;
                            }
                        }
                        if (showrecord)
                            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, gendColCnt, 1, boardcount);
                    }
                    #endregion
                }
                if (rowheader > 0 && showrecord == true)
                {
                    #region spread value bind
                    Fpspread1.SaveChanges();
                    string section = "";
                    if (sec.Trim() != "")
                        section = " and isnull(r.Sections,'') in ('" + sec + "','')";
                    string boardoruniversity = ddl_reporttype.SelectedItem.Value == "1" ? "s.course_code " : " s.university_code";
                    string q1 = "  select COUNT(r.app_no)as TotalStrength,isnull( r.Sections,'') as Sections,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name,a.sex,a.parent_statep,ISNULL(IsExService,0)IsExService from Registration r,applyn a, degree d,Department dt,Course C where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No and isconfirm ='1' and admission_status ='1' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code in('" + degreecode + "')and r.Batch_Year in('" + batchyear + "') and  r.Current_Semester in('" + sem + "')   and a.college_code='" + ddlcollege.SelectedItem.Value + "' " + section + "  group by No_Of_seats,r.degree_code ,r.Batch_Year,r.Current_Semester, C.Course_Name,c.Course_Id ,Dt.Dept_Name ,isnull( r.Sections,''),a.sex,a.parent_statep ,a.IsExService";
                    q1 += " select COUNT(r.app_no)as TotalStrength,isnull( r.Sections,'') as Sections,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name,a.sex," + boardoruniversity + " from Registration r,applyn a, degree d,Department dt,Course C,Stud_prev_details s where r.App_No=s.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No and isconfirm ='1' and admission_status ='1' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code in('" + degreecode + "')and r.Batch_Year in('" + batchyear + "') and  r.Current_Semester in('" + sem + "')   and a.college_code='" + ddlcollege.SelectedItem.Value + "' and isnull(" + boardoruniversity + ",-1)<>'-1'  " + section + " group by No_Of_seats,r.degree_code ,r.Batch_Year,r.Current_Semester, C.Course_Name,c.Course_Id ,Dt.Dept_Name ,isnull( r.Sections,''),a.sex," + boardoruniversity + " ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q1, "text");
                    int r = 1;
                    for (i = 0; i < cbl_branch.Items.Count; i++)
                    {
                        if (cbl_branch.Items[i].Selected == true)
                        {
                            Fpspread1.Sheets[0].RowCount++;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(r++);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(cbl_branch.Items[i].Value.Split('$')[2] + "-" + cbl_branch.Items[i].Text);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(cbl_branch.Items[i].Value.Split('$')[0]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                            for (int k = 2; k < Fpspread1.Sheets[0].ColumnCount; k++)
                            {
                                string gender = string.Empty;
                                string commm = string.Empty;
                                string comF = string.Empty;
                                string genderF = string.Empty;
                                string Board = string.Empty;
                                string BoardF = string.Empty;
                                int tableId = 0;
                                string IsExServiceF = string.Empty;
                                #region Filter Condition
                                if (ddl_reporttype.SelectedItem.Value == "0")
                                {
                                    if (comselectcount > 0)
                                    {
                                        commm = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[0, k].Tag);
                                        if (!string.IsNullOrEmpty(commm.Trim()))
                                            comF = " and parent_statep='" + commm + "' ";
                                    }
                                    if (genderselectcount > 0)
                                    {
                                        gender = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, k].Tag);
                                        if (!string.IsNullOrEmpty(gender.Trim()))
                                            genderF = " and sex='" + gender + "'";
                                    }
                                }
                                else if (ddl_reporttype.SelectedItem.Value == "1" || ddl_reporttype.SelectedItem.Value == "2")
                                {
                                    if (boardcount > 0)
                                    {
                                        Board = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, k].Tag);
                                        if (!string.IsNullOrEmpty(Board.Trim()))
                                        {
                                            BoardF = (ddl_reporttype.SelectedItem.Value == "1" ? "and course_code= " : " and university_code=") + "'" + Board + "'";
                                            tableId = 1;
                                        }
                                    }
                                }
                                else if (ddl_reporttype.SelectedItem.Value == "3")
                                {
                                    if (genderselectcount > 0)
                                    {
                                        gender = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, k].Tag);
                                        if (!string.IsNullOrEmpty(gender.Trim()))
                                            genderF = " and sex='" + gender + "'";
                                    }
                                    IsExServiceF = " and IsExService='1'";
                                }
                                #endregion
                                if (!string.IsNullOrEmpty(BoardF) || (commm.Trim() != "T" && (!string.IsNullOrEmpty(comF.Trim()) || !string.IsNullOrEmpty(genderF.Trim()))))
                                {
                                    double countval = 0;
                                    if (ds.Tables[tableId].Rows.Count > 0)
                                    {
                                        double.TryParse(Convert.ToString(ds.Tables[tableId].Compute("Sum(TotalStrength)", "  degree_code='" + Convert.ToString(cbl_branch.Items[i].Value.Split('$')[0]) + "' " + genderF + " " + comF + " " + BoardF + " " + IsExServiceF + "")), out countval);
                                    }
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Text = Convert.ToString(Convert.ToString(countval) == "0" ? " - " : Convert.ToString(countval));
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Font.Name = "Book Antiqua";
                                }
                                else
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Tag = "Total";
                            }
                        }
                    }
                    #endregion
                }
            }
            if (showrecord == true)
            {
                #region Total bind
                for (int r = 0; r < Fpspread1.Sheets[0].ColumnHeader.RowCount; r++)
                {
                    for (int c = 2; c < Fpspread1.Sheets[0].ColumnCount; c++)
                    {
                        Fpspread1.Sheets[0].ColumnHeader.Cells[r, c].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[r, c].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[r, c].Font.Size = FontUnit.Medium;
                    }
                }
                double value = 0; double totalval = 0; double rowtotal = 0;

                Fpspread1.Sheets[0].ColumnCount++;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = "G";
                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = "G";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Grand Total";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, Fpspread1.Sheets[0].ColumnCount - 1, rowheader, 1);
                for (int r = 0; r < Fpspread1.Sheets[0].RowCount; r++)
                {
                    rowtotal = 0; totalval = 0;
                    for (int c = 2; c < Fpspread1.Sheets[0].ColumnCount - 1; c++)
                    {
                        Fpspread1.Sheets[0].Cells[r, c].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[r, c].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[r, c].Font.Size = FontUnit.Medium;
                        double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[r, c].Text), out value);
                        totalval += value;
                        rowtotal += value;
                        if (Convert.ToString(Fpspread1.Sheets[0].Cells[r, c].Tag).ToUpper() == "TOTAL")
                        {
                            Fpspread1.Sheets[0].Cells[r, c].Text = Convert.ToString(totalval);
                            Fpspread1.Sheets[0].Cells[r, c].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[r, c].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[r, c].ForeColor = Color.Brown;
                            totalval = 0;
                        }
                        double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[r, c].Text), out value);
                        string gender = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[0, c].Tag);
                        string community = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, c].Tag);
                        double value1 = 0;
                        double total = 0;
                        if (totalvalue_dic.Contains(gender + "-" + community))
                        {
                            double.TryParse(Convert.ToString(totalvalue_dic[gender + "-" + community]), out value1);
                            totalvalue_dic.Remove(gender + "-" + community);
                            total = 0;
                            total = Convert.ToInt32(value1) + Convert.ToInt32(value);
                            totalvalue_dic.Add(gender + "-" + community, total);
                        }
                        else
                        {
                            totalvalue_dic.Add(gender + "-" + community, Convert.ToInt32(value));
                        }
                    }
                    Fpspread1.Sheets[0].Cells[r, Fpspread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(rowtotal);
                    Fpspread1.Sheets[0].Cells[r, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[r, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[r, Fpspread1.Sheets[0].ColumnCount - 1].ForeColor = Color.Brown;
                    string gender1 = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag);
                    string community1 = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag);
                    if (totalvalue_dic.Contains(gender1 + "-" + community1))
                    {
                        double value1 = 0;
                        double.TryParse(Convert.ToString(totalvalue_dic[gender1 + "-" + community1]), out value1);
                        totalvalue_dic.Remove(gender1 + "-" + community1);
                        double total = 0;
                        total = Convert.ToInt32(value1) + Convert.ToInt32(rowtotal);
                        totalvalue_dic.Add(gender1 + "-" + community1, total);
                    }
                    else
                    {
                        totalvalue_dic.Add(gender1 + "-" + community1, Convert.ToInt32(rowtotal));
                    }
                }
                Fpspread1.Sheets[0].RowCount++;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].ForeColor = Color.Brown;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, Fpspread1.Sheets[0].RowCount - 1, 2);
                for (int k = 2; k < Fpspread1.Sheets[0].ColumnHeader.Columns.Count; k++)
                {
                    string status = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[0, k].Tag);
                    string disability1 = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, k].Tag);
                    if (totalvalue_dic.Count > 0)
                    {
                        double value1 = 0;
                        if (totalvalue_dic.Contains(status + "-" + disability1))
                        {
                            double.TryParse(Convert.ToString(totalvalue_dic[status + "-" + disability1]), out value1);
                        }
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Text = Convert.ToString(value1);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].ForeColor = Color.Brown;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Font.Bold = true;
                    }
                }
                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.Visible = true;
                rptprint.Visible = true;
                #endregion
            }
            else
            {
                Fpspread1.Visible = false;
                rptprint.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "No Records Founds";
            }
            #endregion
        }
        else
        {
            Fpspread1.Visible = false;
            rptprint.Visible = false;
            lbl_error.Visible = true;
            lbl_error.Text = "Please Select All Fields";
        }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        {
        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = txtexcelname.Text;
            string pagename = "StudentAdmittedCommunitywiseReport.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        { }
    }
    public string cblselecteditemcount(CheckBoxList cblSelected)
    {
        int count = 0;
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    count++;
                }
            }
        }
        catch { count = 0; }
        return count.ToString();
    }
    public string GetSelectedItemsValueAsString(CheckBoxList cblSelected, int position = 0)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Value).Split('$')[0]);
                    }
                    else
                    {
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[sel].Value).Split('$')[0]);
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
}