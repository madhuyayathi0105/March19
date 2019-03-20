using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Web.Services;
using System.Drawing;
using System.Globalization;
public partial class StudentMod_Student_applied_admited_details_report : System.Web.UI.Page
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
                bindreligion();
                bindcommunity();
                bindstatus();
                bindphysicalchallaged();
                bindBoardUniv();
                bindcaste();
                CalendarExtender10.EndDate = DateTime.Now;
                CalendarExtender1.EndDate = DateTime.Now;
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Attributes.Add("readonly", "readonly");
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
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddlcollege.SelectedItem.Value + "' " + rights + "";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddlcollege.SelectedItem.Value + "' " + rights + "";
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
                    //    for (int i = 0; i < cbl_branch.Items.Count; i++)
                    //    {
                    cbl_branch.Items[0].Selected = true;
                }
                txt_branch.Text = lbl_branch.Text + "(" + 1 + ")";
                //}
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
        Boolean first_year;
        first_year = false;
        int duration = 0;
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
                    build = cbl_branch.Items[i].Value.ToString();
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
            string strsql1 = "select distinct duration,first_year_nonsemester  from degree where degree_code in (" + branch + ") and college_code=" + ddlcollege.SelectedItem.Value + "";
            ds = d2.select_method_wo_parameter(strsql1, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string dur = Convert.ToString(ds.Tables[0].Rows[i][0]);
                    if (dur.Trim() != "")
                    {
                        if (duration < Convert.ToInt32(dur))
                        {
                            duration = Convert.ToInt32(dur);
                        }
                    }
                }
            }
            if (duration != 0)
            {
                for (i = 1; i <= duration; i++)
                {
                    cbl_sem.Items.Add(Convert.ToString(i));
                }
                if (cbl_sem.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_sem.Items.Count; row++)
                    {
                        cbl_sem.Items[row].Selected = true;
                        cb_sem.Checked = true;
                    }
                    txt_sem.Text = lbl_org_sem.Text + "(" + cbl_sem.Items.Count + ")";
                }
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
                        string build = cbl_branch.Items[i].Value.ToString();
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
            if (cbl_batch.Items.Count > 0)
            {
                for (i = 0; i < cbl_batch.Items.Count; i++)
                {
                    if (cbl_batch.Items[i].Selected == true)
                    {
                        string build = cbl_batch.Items[i].Value.ToString();
                        if (batch == "")
                        {
                            batch = build;
                        }
                        else
                        {
                            batch = batch + "','" + build;
                        }
                    }
                }
            }
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
                //    for (int i = 0; i < cbl_graduation.Items.Count; i++)
                //    {
                cbl_graduation.Items[0].Selected = true;
            }
            txt_graduation.Text = "Graduation(" + 1 + ")";
        }
    }
    public void bindreligion()
    {
        try
        {
            string religion = "";
            cbl_religion.Items.Clear();
            string reliquery = "SELECT Distinct religion,T.TextVal  FROM applyn A,Registration R,TextValTable T WHERE A.app_no=R.App_No AND T.TextCode =A.religion AND R.college_code ='" + ddlcollege.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(reliquery, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_religion.DataSource = ds;
                    cbl_religion.DataTextField = "TextVal";
                    cbl_religion.DataValueField = "religion";
                    cbl_religion.DataBind();
                    if (cbl_religion.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_religion.Items.Count; i++)
                        {
                            cbl_religion.Items[i].Selected = true;
                            religion = Convert.ToString(cbl_religion.Items[i].Text);
                        }
                        if (cbl_religion.Items.Count == 1)
                        {
                            txt_religion.Text = "" + religion + "";
                        }
                        else
                        {
                            txt_religion.Text = "Religion(" + cbl_religion.Items.Count + ")";
                        }
                        cb_religion.Checked = true;
                    }
                }
            }
            else
            {
                txt_religion.Text = "--Select--";
                cb_religion.Checked = false;
            }
        }
        catch
        {
        }
    }
    public void bindcaste()
    {
        try
        {
            string st = "";
            st = "select distinct Textval,TextCode  from TextValtable t,applyn a,Registration r where a.app_no=r.App_No and TextCriteria='Caste' and textval<>'' and a.caste=t.TextCode  order by textval";
            ds.Clear();
            cbl_caste.Items.Clear();
            ds = d2.select_method_wo_parameter(st, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_caste.DataSource = ds;
                cbl_caste.DataTextField = "Textval";
                cbl_caste.DataValueField = "TextCode";
                cbl_caste.DataBind();
                if (cbl_caste.Items.Count > 0)
                {
                    cbl_caste.Items[0].Selected = true;
                }
                txt_Caste.Text = "Caste(" + 1 + ")";
            }
        }
        catch { }
    }
    public void bindcommunity()
    {
        try
        {
            string comm = "";
            string selq = "SELECT Distinct community,T.TextVal  FROM applyn A,Registration R,TextValTable T WHERE A.app_no=R.App_No AND T.TextCode =A.community  AND TextVal<>''AND R.college_code ='" + ddlcollege.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_comm.DataSource = ds;
                    cbl_comm.DataTextField = "TextVal";
                    cbl_comm.DataValueField = "community";
                    cbl_comm.DataBind();
                    if (cbl_comm.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_comm.Items.Count; i++)
                        {
                            cbl_comm.Items[i].Selected = true;
                            comm = Convert.ToString(cbl_comm.Items[i].Text);
                        }
                        if (cbl_comm.Items.Count == 1)
                        {
                            txt_comm.Text = "" + comm + "";
                        }
                        else
                        {
                            txt_comm.Text = "Community(" + cbl_comm.Items.Count + ")";
                        }
                        cb_comm.Checked = true;
                    }
                }
            }
            else
            {
                txt_comm.Text = "--Select--";
                cb_comm.Checked = false;
            }
        }
        catch
        {
        }
    }
    public void bindstatus()
    {
        string type = "";
        string[] statusname = { "Applied", "Admitted" };
        for (int i = 0; i < 2; i++)
        {
            cbl_status.Items.Add(new System.Web.UI.WebControls.ListItem(statusname[i], Convert.ToString(i + 1)));
        }
        if (cbl_status.Items.Count > 0)
        {
            for (int i = 0; i < cbl_status.Items.Count; i++)
            {
                cbl_status.Items[i].Selected = true;
                type = Convert.ToString(cbl_status.Items[i].Text);
            }
            if (cbl_status.Items.Count == 1)
            {
                txt_status.Text = "Status(" + type + ")";
            }
            else
            {
                txt_status.Text = "Status(" + cbl_status.Items.Count + ")";
            }
            cb_statusdetail.Checked = true;
        }
    }
    public void bindphysicalchallaged()
    {
        string type = "";
        string[] physical = { "Visually Challanged", "Physically Challanged", "Learning Disability", "Others" };//"IsDisable", 
        string[] physical1 = { "visualhandy ", "handy", "islearningdis", "Others" };//"isdisable", "isdisabledisc"
        for (int i = 0; i < 4; i++)
        {
            cbl_phychlg.Items.Add(new System.Web.UI.WebControls.ListItem(physical[i], Convert.ToString(physical1[i])));
        }
        if (cbl_phychlg.Items.Count > 0)
        {
            for (int i = 0; i < cbl_phychlg.Items.Count; i++)
            {
                cbl_phychlg.Items[i].Selected = true;
                type = Convert.ToString(cbl_phychlg.Items[i].Text);
            }
            if (cbl_phychlg.Items.Count == 1)
            {
                txt_phychallage.Text = "Physical Challanged(" + type + ")";
            }
            else
            {
                txt_phychallage.Text = "Physical Challanged(" + cbl_phychlg.Items.Count + ")";
            }
            cb_phychlg.Checked = true;
        }
    }
    private void bindBoardUniv()
    {
        cbl_BoardUniv.Items.Clear();
        try
        {
            if (ddlcollege.Items.Count > 0 && cbl_batch.Items.Count > 0 && cbl_branch.Items.Count > 0)
            {
                string degreecode = rs.GetSelectedItemsValueAsString(cbl_branch);
                string batch = rs.GetSelectedItemsValueAsString(cbl_batch);
                if (degreecode.Trim() != "" && batch.Trim() != "")
                {
                    string query = "   select distinct TextVal,TextCode from textvaltable t,Stud_prev_details s,applyn a where T.TextCode=S.Course_code and a.app_no=s.app_no and a.batch_year in('" + batch + "') and a.degree_code in('" + degreecode + "')  and t.college_code=" + ddlcollege.SelectedItem.Value + " and Textval is not null and Textval<>'' order by Textval asc";
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
                        txtBoardUniv.Text = "Board(" + cbl_BoardUniv.Items.Count + ")";
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
    public void cb_statusdetail_checkedchange(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_statusdetail, cbl_status, txt_status, "Status", "--Select--");
        }
        catch
        {
        }
    }
    public void cbl_status_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_statusdetail, cbl_status, txt_status, "Status");
        }
        catch
        {
        }
    }
    public void cb_religion_checkedchange(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_religion, cbl_religion, txt_religion, "Religion", "--Select--");
        }
        catch
        {
        }
    }
    public void cbl_religion_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_religion, cbl_religion, txt_religion, "Religion");
        }
        catch
        {
        }
    }
    public void cb_comm_checkedchange(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_comm, cbl_comm, txt_comm, "Community", "--Select--");
        }
        catch
        {
        }
    }
    public void cbl_comm_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_comm, cbl_comm, txt_comm, "Community");
        }
        catch
        {
        }
    }
    public void cb_caste_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_caste, cbl_caste, txt_Caste, "Caste", "--Select--");
    }
    public void cbl_caste_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_caste, cbl_caste, txt_Caste, "Caste");
    }
    protected void cb_gender_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_gender, cbl_gender, txt_gender, "Gender", "--Select--");
    }
    protected void cbl_gender_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_gender, cbl_gender, txt_gender, "Gender");
    }
    public void cb_phychlg_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_phychlg, cbl_phychlg, txt_phychallage, "Physical Challaged", "--Select--");
    }
    public void cbl_phychlg_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_phychlg, cbl_phychlg, txt_phychallage, "Physical Challaged");
    }
    protected void cbl_maxmin_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_maxmin, cbl_maxmin, txt_maxmin, "Min Max");
    }
    protected void cb_maxmin_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_maxmin, cbl_maxmin, txt_maxmin, "Min Max", "--Select--");
    }
    protected void cb_BoardUniv_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_BoardUniv, cbl_BoardUniv, txtBoardUniv, "Board", "--Select--");
    }
    protected void cbl_BoardUniv_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_BoardUniv, cbl_BoardUniv, txtBoardUniv, "Board");
    }
    public void cb_from_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_from.Checked == true)
        {
            txt_fromdate.Enabled = true;
            txt_todate.Enabled = true;
        }
        else
        {
            txt_fromdate.Enabled = false;
            txt_todate.Enabled = false;
        }
    }
    public void cb_religion_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_religion1.Checked == true)
        {
            tdrelichk1.Visible = true;
        }
        else
        {
            tdrelichk1.Visible = false;
        }
    }
    public void cb_com_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_com.Checked == true)
        {
            tdcommchk1.Visible = true;
        }
        else
        {
            tdcommchk1.Visible = false;
        }
    }
    protected void cb_caste_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_caste1.Checked == true)
        {
            tdcaste.Visible = true;
        }
        else
        {
            tdcaste.Visible = false;
        }
    }
    protected void cb_genderT_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_genderT.Checked == true)
        {
            tdgender.Visible = true;
            cb_gender.Checked = true;
            cb_gender_checkedchange(sender, e);
        }
        else
        {
            tdgender.Visible = false;
        }
    }
    protected void cb_phychallange_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_phychallange.Checked == true)
        {
            tdphychallange.Visible = true;
        }
        else
        {
            tdphychallange.Visible = false;
        }
    }
    protected void cb_maxminum_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_maxminum.Checked == true)
        {
            td_maxminum.Visible = true;
        }
        else
        {
            td_maxminum.Visible = false;
        }
    }
    protected void cb_boardT_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_boardT.Checked == true)
        {
            td_board.Visible = true;
        }
        else
        {
            td_board.Visible = false;
        }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
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
            int religionselectcount = 0;
            int comselectcount = 0;
            int casteselectcount = 0;
            int physicalselectcount = 0;
            int boardselectcount = 0;
            int.TryParse(cblselecteditemcount(cbl_status), out statusselectcount);
            int columncount = 0;
            int rowheader = 1;
            columncount += statusselectcount;

            #region Rowheader and filter
            if (ddl_reporttype.SelectedIndex == 0)
            {
                if (cb_genderT.Checked == true)
                {
                    int.TryParse(cblselecteditemcount(cbl_gender), out genderselectcount);
                    columncount += genderselectcount;
                    rowheader++;
                }
                if (cb_religion1.Checked == true)
                {
                    int.TryParse(cblselecteditemcount(cbl_religion), out religionselectcount);
                    columncount += religionselectcount; rowheader++;
                }
                if (cb_com.Checked == true)
                {
                    int.TryParse(cblselecteditemcount(cbl_comm), out comselectcount);
                    columncount += comselectcount; rowheader++;
                }
                if (cb_caste1.Checked == true)
                {
                    int.TryParse(cblselecteditemcount(cbl_caste), out casteselectcount);
                    columncount += casteselectcount; rowheader++;
                }
            }
            if (ddl_reporttype.SelectedIndex == 1)
            {
                if (cb_boardT.Checked == true)
                {
                    int.TryParse(cblselecteditemcount(cbl_BoardUniv), out boardselectcount);
                    columncount += boardselectcount; rowheader++;
                }
            }
            if (ddl_reporttype.SelectedIndex == 2)
            {
                if (cb_phychallange.Checked == true)
                {
                    int.TryParse(cblselecteditemcount(cbl_phychlg), out physicalselectcount);
                    columncount += physicalselectcount; rowheader++;
                }
            }
            if (ddl_reporttype.SelectedIndex == 3)
            {
                if (cb_com.Checked == true)
                {
                    int.TryParse(cblselecteditemcount(cbl_comm), out comselectcount);
                    columncount += comselectcount; rowheader++; rowheader++;
                }
            }
            #endregion


            int col = 0;
            Fpspread1.Sheets[0].ColumnHeader.RowCount = rowheader;
            bool genchk = false;
            Hashtable totalvalue_dic = new Hashtable(); double total = 0; double val = 0; double totalvalue = 0; string value1 = "";
            if (cbl_status.Items.Count > 0)
            {
                string batchyear = rs.GetSelectedItemsValueAsString(cbl_batch);
                string degreecode = rs.GetSelectedItemsValueAsString(cbl_branch);
                string sem = rs.GetSelectedItemsValueAsString(cbl_sem);
                string sec = rs.GetSelectedItemsValueAsString(cbl_sec);
                string religion = rs.GetSelectedItemsValueAsString(cbl_religion);
                string comm = rs.GetSelectedItemsValueAsString(cbl_comm);
                string cast = rs.GetSelectedItemsValueAsString(cbl_caste);
                DateTime from = new DateTime();
                DateTime to = new DateTime();
                string[] ay = txt_fromdate.Text.Split('/');
                string[] ay1 = txt_todate.Text.Split('/');
                from = Convert.ToDateTime(ay[1] + "/" + ay[0] + "/" + ay[2]);
                to = Convert.ToDateTime(ay1[1] + "/" + ay1[0] + "/" + ay1[2]);
                string datebetween = "";
                string datebetween1 = "";
                if (cb_from.Checked == true)
                {
                    datebetween = "  and r.Adm_Date between  '" + from.ToString("MM/dd/yyyy") + "' and '" + to.ToString("MM/dd/yyyy") + "' ";
                    datebetween1 = "  and a.date_applied between  '" + from.ToString("MM/dd/yyyy") + "' and '" + to.ToString("MM/dd/yyyy") + "' ";
                }
                if (ddl_reporttype.SelectedIndex == 2)
                {
                    if (rdbDetail.Checked == true)
                    {
                        if (cb_phychallange.Checked == false)
                        {
                            Fpspread1.Visible = false;
                            rptprint.Visible = false;
                            lbl_error.Visible = true;
                            lbl_error.Text = "Please Select PhysicalChallange Details";
                            return;
                        }
                        disabilitydetailswise(batchyear, degreecode, sem, datebetween);
                        Fpspread1.SaveChanges();
                        return;
                    }
                }

                if (ddl_reporttype.SelectedItem.Value == "0")
                {
                    if (cb_genderT.Checked == true && cb_religion1.Checked == true && cb_com.Checked == true && cb_caste1.Checked == true)
                    {
                        #region religion communitity caste gender wise
                        Fpspread1.Sheets[0].Columns.Count++;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "S.NO";
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
                        for (i = 0; i < cbl_status.Items.Count; i++)
                        {
                            if (cbl_status.Items[i].Selected == true)
                            {
                                int religiCnt = 0; int comcnt = 0; int castecnt = 0; int religcnt = 0; int commcnt = 0;
                                if (genchk)
                                {
                                    Fpspread1.Sheets[0].ColumnCount++;
                                }
                                col = Fpspread1.Sheets[0].Columns.Count - 1;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_status.Items[i].Text;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                if (cb_genderT.Checked == true)
                                {
                                    if (cbl_gender.Items.Count > 0)
                                    {
                                        bool booGeder = false;
                                        int gendColCnt = 0;
                                        for (int j = 0; j < cbl_gender.Items.Count; j++)
                                        {
                                            if (cbl_gender.Items[j].Selected == true)
                                            {
                                                int genspan = 0;
                                                if (booGeder)
                                                {
                                                    Fpspread1.Sheets[0].ColumnCount++;
                                                }
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_gender.Items[j].Text;
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_gender.Items[j].Value;
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                                //religion
                                                if (cb_religion1.Checked == true)
                                                {
                                                    if (cbl_religion.Items.Count > 0)
                                                    {
                                                        bool regibool = false; int gencnt = 0;
                                                        gendColCnt = Fpspread1.Sheets[0].ColumnCount - 1;
                                                        for (int k = 0; k < cbl_religion.Items.Count; k++)
                                                        {
                                                            if (cbl_religion.Items[k].Selected == true)
                                                            {
                                                                gencnt = 0;
                                                                if (regibool)
                                                                {
                                                                    Fpspread1.Sheets[0].ColumnCount++;
                                                                }
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_religion.Items[k].Text;
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_religion.Items[k].Value;
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_gender.Items[j].Value;
                                                                regibool = true;
                                                                booGeder = true;
                                                                genchk = true;
                                                                religiCnt++;
                                                                religcnt = Fpspread1.Sheets[0].ColumnCount - 1;
                                                                //Community
                                                                bool comchk = false;
                                                                if (cb_com.Checked == true)
                                                                {
                                                                    if (cbl_comm.Items.Count > 0)
                                                                    {
                                                                        for (int l = 0; l < cbl_comm.Items.Count; l++)
                                                                        {
                                                                            if (cbl_comm.Items[l].Selected == true)
                                                                            {
                                                                                if (comchk)
                                                                                    Fpspread1.Sheets[0].ColumnCount++;
                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[3, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_comm.Items[l].Text;
                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[3, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_comm.Items[l].Value;
                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_gender.Items[j].Value;
                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_religion.Items[k].Value;
                                                                                comcnt = Fpspread1.Sheets[0].ColumnCount - 1;
                                                                                commcnt++;
                                                                                #region Caste
                                                                                bool castechk = false;
                                                                                if (cb_caste1.Checked == true)
                                                                                {
                                                                                    if (cbl_caste.Items.Count > 0)
                                                                                    {
                                                                                        for (int m = 0; m < cbl_caste.Items.Count; m++)
                                                                                        {
                                                                                            if (cbl_caste.Items[m].Selected == true)
                                                                                            {
                                                                                                if (castechk)
                                                                                                    Fpspread1.Sheets[0].ColumnCount++;
                                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[4, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_caste.Items[m].Text;
                                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[4, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_caste.Items[m].Value;
                                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_gender.Items[j].Value;
                                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[3, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_comm.Items[l].Value;
                                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_religion.Items[k].Value;
                                                                                                castechk = true;
                                                                                                castecnt++;
                                                                                                gencnt++;
                                                                                                genspan++;
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                                comchk = true;
                                                                                #endregion
                                                                            }
                                                                            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(3, comcnt, 1, casteselectcount);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(2, religcnt, 1, gencnt);
                                                        }
                                                    }
                                                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, gendColCnt, 1, genspan);
                                                }
                                            }
                                        }
                                        Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 1, castecnt);
                                    }
                                }
                            }
                        }
                        #endregion
                        showrecord = true;
                    }
                    else if (genderselectcount > 0 && comselectcount > 0 && religionselectcount == 0 && (casteselectcount == 0 || casteselectcount > 0))
                    {
                        #region Gender with community Caste
                        Fpspread1.Sheets[0].Columns.Count++;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "S.NO";
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
                        for (i = 0; i < cbl_status.Items.Count; i++)
                        {
                            if (cbl_status.Items[i].Selected == true)
                            {
                                int religiCnt = 0; int comcnt = 0; int castecnt = 0; int religcnt = 0; int commcnt = 0;
                                if (genchk)
                                {
                                    Fpspread1.Sheets[0].ColumnCount++;
                                }
                                col = Fpspread1.Sheets[0].Columns.Count - 1;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_status.Items[i].Text;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                if (cb_genderT.Checked == true)
                                {
                                    if (cbl_gender.Items.Count > 0)
                                    {
                                        bool booGeder = false;
                                        int gendColCnt = 0;
                                        for (int j = 0; j < cbl_gender.Items.Count; j++)
                                        {
                                            if (cbl_gender.Items[j].Selected == true)
                                            {
                                                int genspan = 0;
                                                if (booGeder)
                                                {
                                                    Fpspread1.Sheets[0].ColumnCount++;
                                                }
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_gender.Items[j].Text;
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_gender.Items[j].Value;
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                                if (cb_com.Checked == true)
                                                {
                                                    if (cbl_comm.Items.Count > 0)
                                                    {
                                                        bool regibool = false; int gencnt = 0;
                                                        gendColCnt = Fpspread1.Sheets[0].ColumnCount - 1;
                                                        for (int k = 0; k < cbl_comm.Items.Count; k++)
                                                        {
                                                            if (cbl_comm.Items[k].Selected == true)
                                                            {
                                                                gencnt = 0;
                                                                if (regibool)
                                                                {
                                                                    Fpspread1.Sheets[0].ColumnCount++;
                                                                }
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_comm.Items[k].Text;
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_comm.Items[k].Value;
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_gender.Items[j].Value;
                                                                comcnt = Fpspread1.Sheets[0].ColumnCount - 1;
                                                                commcnt++;
                                                                regibool = true;
                                                                booGeder = true;
                                                                genchk = true;
                                                                religiCnt++;
                                                                religcnt = Fpspread1.Sheets[0].ColumnCount - 1;
                                                                #region Caste
                                                                bool castechk = false;
                                                                if (cb_caste1.Checked == true)
                                                                {
                                                                    if (cbl_caste.Items.Count > 0)
                                                                    {
                                                                        for (int m = 0; m < cbl_caste.Items.Count; m++)
                                                                        {
                                                                            if (cbl_caste.Items[m].Selected == true)
                                                                            {
                                                                                if (castechk)
                                                                                    Fpspread1.Sheets[0].ColumnCount++;
                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[3, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_caste.Items[m].Text;
                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[3, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_caste.Items[m].Value;
                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_gender.Items[j].Value;
                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_comm.Items[k].Value;
                                                                                castechk = true;
                                                                                castecnt++;
                                                                                gencnt++;
                                                                                genspan++;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                #endregion
                                                                if (casteselectcount > 0)
                                                                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(2, comcnt, 1, casteselectcount);
                                                            }
                                                        }
                                                    }
                                                    if (casteselectcount > 0)
                                                    { Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, gendColCnt, 1, genspan); }
                                                    else { Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, gendColCnt, 1, comselectcount); }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (casteselectcount > 0)
                                { Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 1, castecnt); }
                                else
                                { Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 1, religiCnt); }
                            }
                        }
                        #endregion
                        showrecord = true;
                    }
                    else if (genderselectcount > 0 && comselectcount == 0 && religionselectcount == 0 && casteselectcount > 0)
                    {
                        #region Gender with Caste
                        Fpspread1.Sheets[0].Columns.Count++;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "S.NO";
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
                        for (i = 0; i < cbl_status.Items.Count; i++)
                        {
                            if (cbl_status.Items[i].Selected == true)
                            {
                                int religiCnt = 0; int religcnt = 0;
                                if (genchk)
                                {
                                    Fpspread1.Sheets[0].ColumnCount++;
                                }
                                col = Fpspread1.Sheets[0].Columns.Count - 1;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_status.Items[i].Text;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                if (cb_genderT.Checked == true)
                                {
                                    if (cbl_gender.Items.Count > 0)
                                    {
                                        bool booGeder = false;
                                        int gendColCnt = 0;
                                        for (int j = 0; j < cbl_gender.Items.Count; j++)
                                        {
                                            if (cbl_gender.Items[j].Selected == true)
                                            {
                                                if (booGeder)
                                                {
                                                    Fpspread1.Sheets[0].ColumnCount++;
                                                }
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_gender.Items[j].Text;
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_gender.Items[j].Value;
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                                if (cb_caste1.Checked == true)
                                                {
                                                    if (cbl_caste.Items.Count > 0)
                                                    {
                                                        bool regibool = false;
                                                        gendColCnt = Fpspread1.Sheets[0].ColumnCount - 1;
                                                        for (int k = 0; k < cbl_caste.Items.Count; k++)
                                                        {
                                                            if (cbl_caste.Items[k].Selected == true)
                                                            {
                                                                if (regibool)
                                                                {
                                                                    Fpspread1.Sheets[0].ColumnCount++;
                                                                }
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_caste.Items[k].Text;
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_caste.Items[k].Value;
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_gender.Items[j].Value;
                                                                regibool = true;
                                                                booGeder = true;
                                                                genchk = true;
                                                                religiCnt++;
                                                                religcnt = Fpspread1.Sheets[0].ColumnCount - 1;
                                                            }
                                                        }
                                                    }
                                                }
                                                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, gendColCnt, 1, casteselectcount);
                                            }
                                        }
                                    }
                                }
                                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 1, religiCnt);
                            }
                        }
                        #endregion
                        showrecord = true;
                    }
                    else if (genderselectcount > 0 && religionselectcount > 0 && casteselectcount > 0 && comselectcount == 0)
                    {
                        #region Gender religion & caste
                        Fpspread1.Sheets[0].Columns.Count++;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "S.NO";
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
                        for (i = 0; i < cbl_status.Items.Count; i++)
                        {
                            if (cbl_status.Items[i].Selected == true)
                            {
                                int religiCnt = 0; int comcnt = 0; int castecnt = 0; int religcnt = 0; int commcnt = 0;
                                if (genchk)
                                {
                                    Fpspread1.Sheets[0].ColumnCount++;
                                }
                                col = Fpspread1.Sheets[0].Columns.Count - 1;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_status.Items[i].Text;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                if (cb_genderT.Checked == true)
                                {
                                    if (cbl_gender.Items.Count > 0)
                                    {
                                        bool booGeder = false;
                                        int gendColCnt = 0;
                                        for (int j = 0; j < cbl_gender.Items.Count; j++)
                                        {
                                            if (cbl_gender.Items[j].Selected == true)
                                            {
                                                int genspan = 0;
                                                if (booGeder)
                                                {
                                                    Fpspread1.Sheets[0].ColumnCount++;
                                                }
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_gender.Items[j].Text;
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_gender.Items[j].Value;
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                                if (cb_religion1.Checked == true)
                                                {
                                                    if (cbl_religion.Items.Count > 0)
                                                    {
                                                        bool regibool = false; int gencnt = 0;
                                                        gendColCnt = Fpspread1.Sheets[0].ColumnCount - 1;
                                                        for (int k = 0; k < cbl_religion.Items.Count; k++)
                                                        {
                                                            if (cbl_religion.Items[k].Selected == true)
                                                            {
                                                                gencnt = 0;
                                                                if (regibool)
                                                                {
                                                                    Fpspread1.Sheets[0].ColumnCount++;
                                                                }
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_religion.Items[k].Text;
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_religion.Items[k].Value;
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_gender.Items[j].Value;
                                                                comcnt = Fpspread1.Sheets[0].ColumnCount - 1;
                                                                commcnt++;
                                                                regibool = true;
                                                                booGeder = true;
                                                                genchk = true;
                                                                religiCnt++;
                                                                religcnt = Fpspread1.Sheets[0].ColumnCount - 1;
                                                                #region Caste
                                                                bool castechk = false;
                                                                if (cb_caste1.Checked == true)
                                                                {
                                                                    if (cbl_caste.Items.Count > 0)
                                                                    {
                                                                        for (int m = 0; m < cbl_caste.Items.Count; m++)
                                                                        {
                                                                            if (cbl_caste.Items[m].Selected == true)
                                                                            {
                                                                                if (castechk)
                                                                                    Fpspread1.Sheets[0].ColumnCount++;
                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[3, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_caste.Items[m].Text;
                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[3, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_caste.Items[m].Value;
                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_gender.Items[j].Value;
                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_religion.Items[k].Value;
                                                                                castechk = true;
                                                                                castecnt++;
                                                                                gencnt++;
                                                                                genspan++;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                #endregion
                                                                if (casteselectcount > 0)
                                                                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(2, comcnt, 1, casteselectcount);
                                                            }
                                                        }
                                                    }
                                                    if (casteselectcount > 0)
                                                    { Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, gendColCnt, 1, genspan); }
                                                    else { Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, gendColCnt, 1, religionselectcount); }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (casteselectcount > 0)
                                { Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 1, castecnt); }
                                else
                                { Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 1, religiCnt); }
                            }
                        }
                        #endregion
                        showrecord = true;
                    }
                    else if (genderselectcount == 0 && religionselectcount > 0 && casteselectcount > 0 && comselectcount == 0)
                    {
                        #region religion & caste
                        Fpspread1.Sheets[0].Columns.Count++;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "S.NO";
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
                        for (i = 0; i < cbl_status.Items.Count; i++)
                        {
                            if (cbl_status.Items[i].Selected == true)
                            {
                                int comcnt = 0; int castecnt = 0;
                                if (genchk)
                                {
                                    Fpspread1.Sheets[0].ColumnCount++;
                                }
                                col = Fpspread1.Sheets[0].Columns.Count - 1;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_status.Items[i].Text;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                int gendColCnt = 0;
                                if (cb_religion1.Checked == true)
                                {
                                    if (cbl_religion.Items.Count > 0)
                                    {
                                        bool regibool = false;
                                        gendColCnt = Fpspread1.Sheets[0].ColumnCount - 1;
                                        for (int k = 0; k < cbl_religion.Items.Count; k++)
                                        {
                                            if (cbl_religion.Items[k].Selected == true)
                                            {
                                                if (regibool)
                                                {
                                                    Fpspread1.Sheets[0].ColumnCount++;
                                                }
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_religion.Items[k].Text;
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_religion.Items[k].Value;
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                                comcnt = Fpspread1.Sheets[0].ColumnCount - 1;
                                                regibool = true;
                                                genchk = true;
                                                #region Caste
                                                bool castechk = false;
                                                if (cb_caste1.Checked == true)
                                                {
                                                    if (cbl_caste.Items.Count > 0)
                                                    {
                                                        for (int m = 0; m < cbl_caste.Items.Count; m++)
                                                        {
                                                            if (cbl_caste.Items[m].Selected == true)
                                                            {
                                                                if (castechk)
                                                                    Fpspread1.Sheets[0].ColumnCount++;
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_caste.Items[m].Text;
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_caste.Items[m].Value;
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_religion.Items[k].Value;
                                                                castechk = true;
                                                                castecnt++;
                                                            }
                                                        }
                                                    }
                                                }
                                                #endregion
                                                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, comcnt, 1, casteselectcount);
                                            }
                                        }
                                    }
                                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 1, castecnt);
                                }
                            }
                        }
                        #endregion
                        showrecord = true;
                    }
                    else if (genderselectcount == 0 && comselectcount > 0 && religionselectcount == 0 && casteselectcount == 0)
                    {
                        fpreadSinglefilterselected(cbl_comm, cb_com, rowheader);
                        showrecord = true;
                    }
                    else if (genderselectcount == 0 && comselectcount == 0 && religionselectcount > 0 && casteselectcount == 0)
                    {
                        fpreadSinglefilterselected(cbl_religion, cb_religion1, rowheader);
                        showrecord = true;
                    }
                    else if (genderselectcount == 0 && comselectcount == 0 && religionselectcount == 0 && casteselectcount > 0)
                    {
                        fpreadSinglefilterselected(cbl_caste, cb_caste1, rowheader);
                        showrecord = true;
                    }
                    else if (cb_genderT.Checked == true)//&& cb_religion1.Checked == true && cb_com.Checked == true
                    {
                        #region religion community gender
                        Fpspread1.Sheets[0].Columns.Count++;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "S.NO";
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
                        for (i = 0; i < cbl_status.Items.Count; i++)
                        {
                            if (cbl_status.Items[i].Selected == true)
                            {
                                int religiCnt = 0; int comcnt = 0; int castecnt = 0; int religcnt = 0; int commcnt = 0;
                                if (genchk)
                                {
                                    Fpspread1.Sheets[0].ColumnCount++;
                                }
                                col = Fpspread1.Sheets[0].Columns.Count - 1;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_status.Items[i].Text;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                if (cb_genderT.Checked == true)
                                {
                                    if (cbl_gender.Items.Count > 0)
                                    {
                                        bool booGeder = false;
                                        int gendColCnt = 0;
                                        for (int j = 0; j < cbl_gender.Items.Count; j++)
                                        {
                                            if (cbl_gender.Items[j].Selected == true)
                                            {
                                                int gencnt = 0;
                                                if (booGeder)
                                                {
                                                    Fpspread1.Sheets[0].ColumnCount++;
                                                }
                                                gendColCnt = Fpspread1.Sheets[0].ColumnCount - 1;
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_gender.Items[j].Text;
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_gender.Items[j].Value;
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                                booGeder = true;
                                                genchk = true;
                                                //religion
                                                if (cb_religion1.Checked == true)
                                                {
                                                    if (cbl_religion.Items.Count > 0)
                                                    {
                                                        bool regibool = false;
                                                        for (int k = 0; k < cbl_religion.Items.Count; k++)
                                                        {
                                                            if (cbl_religion.Items[k].Selected == true)
                                                            {
                                                                if (regibool)
                                                                {
                                                                    Fpspread1.Sheets[0].ColumnCount++;
                                                                }
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_religion.Items[k].Text;
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_religion.Items[k].Value;
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_gender.Items[j].Value;
                                                                regibool = true;
                                                                //booGeder = true;
                                                                //genchk = true;
                                                                religiCnt++;
                                                                religcnt = Fpspread1.Sheets[0].ColumnCount - 1;
                                                                //Community
                                                                bool comchk = false;
                                                                if (cb_com.Checked == true)
                                                                {
                                                                    if (cbl_comm.Items.Count > 0)
                                                                    {
                                                                        for (int l = 0; l < cbl_comm.Items.Count; l++)
                                                                        {
                                                                            if (cbl_comm.Items[l].Selected == true)
                                                                            {
                                                                                if (comchk)
                                                                                    Fpspread1.Sheets[0].ColumnCount++;
                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[3, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_comm.Items[l].Text;
                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[3, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_comm.Items[l].Value;
                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_gender.Items[j].Value;
                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_religion.Items[k].Value;
                                                                                comcnt = Fpspread1.Sheets[0].ColumnCount - 1;
                                                                                commcnt++;
                                                                                comchk = true;
                                                                                gencnt++;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (cb_com.Checked == true)
                                                                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(2, religcnt, 1, comselectcount);
                                                                else
                                                                { }
                                                            }
                                                        }
                                                        if (cb_com.Checked == true)
                                                            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, gendColCnt, 1, gencnt);
                                                        else
                                                            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, gendColCnt, 1, religionselectcount);
                                                    }
                                                }
                                            }
                                        }
                                        if (cb_genderT.Checked == true && cb_religion1.Checked == false && cb_com.Checked == false)
                                        {
                                            if (genderselectcount > 0)
                                                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 1, genderselectcount);
                                        }
                                        else
                                        {
                                            if (cb_com.Checked == true)
                                                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 1, commcnt);
                                            else
                                                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 1, religiCnt);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        showrecord = true;
                    }
                    else if (cb_genderT.Checked == false)
                    {
                        #region religion community caste
                        Fpspread1.Sheets[0].Columns.Count++;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "S.NO";
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
                        for (i = 0; i < cbl_status.Items.Count; i++)
                        {
                            if (cbl_status.Items[i].Selected == true)
                            {
                                int religiCnt = 0; int comcnt = 0; int castecnt = 0; int religcnt = 0; int commcnt = 0;
                                if (genchk)
                                {
                                    Fpspread1.Sheets[0].ColumnCount++;
                                }
                                col = Fpspread1.Sheets[0].Columns.Count - 1;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_status.Items[i].Text;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                int gencnt = 0;
                                genchk = true;
                                if (cb_religion1.Checked == true)
                                {
                                    if (cbl_religion.Items.Count > 0)
                                    {
                                        bool regibool = false;
                                        for (int k = 0; k < cbl_religion.Items.Count; k++)
                                        {
                                            if (cbl_religion.Items[k].Selected == true)
                                            {
                                                if (regibool)
                                                {
                                                    Fpspread1.Sheets[0].ColumnCount++;
                                                }
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_religion.Items[k].Text;
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_religion.Items[k].Value;
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                                regibool = true;
                                                gencnt = 0;
                                                religcnt = Fpspread1.Sheets[0].ColumnCount - 1;
                                                bool comchk = false;
                                                if (cb_com.Checked == true)
                                                {
                                                    if (cbl_comm.Items.Count > 0)
                                                    {
                                                        for (int l = 0; l < cbl_comm.Items.Count; l++)
                                                        {
                                                            if (cbl_comm.Items[l].Selected == true)
                                                            {
                                                                if (comchk)
                                                                    Fpspread1.Sheets[0].ColumnCount++;
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_comm.Items[l].Text;
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_comm.Items[l].Value;
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_religion.Items[k].Value;
                                                                comcnt = Fpspread1.Sheets[0].ColumnCount - 1;
                                                                commcnt++;
                                                                comchk = true;
                                                                #region Caste
                                                                bool castechk = false;
                                                                if (cb_caste1.Checked == true)
                                                                {
                                                                    if (cbl_caste.Items.Count > 0)
                                                                    {
                                                                        for (int m = 0; m < cbl_caste.Items.Count; m++)
                                                                        {
                                                                            if (cbl_caste.Items[m].Selected == true)
                                                                            {
                                                                                if (castechk)
                                                                                    Fpspread1.Sheets[0].ColumnCount++;
                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[3, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_caste.Items[m].Text;
                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[3, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_caste.Items[m].Value;
                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_religion.Items[k].Value;
                                                                                Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_comm.Items[l].Value;
                                                                                castechk = true;
                                                                                castecnt++;
                                                                                religiCnt++;
                                                                                gencnt++;
                                                                            }
                                                                        }
                                                                    }
                                                                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(2, comcnt, 1, casteselectcount);
                                                                }
                                                                #endregion
                                                            }
                                                        }
                                                    }
                                                }
                                                if (cb_caste1.Checked == true)
                                                {
                                                    if (gencnt != 0)
                                                        Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, religcnt, 1, gencnt);
                                                }
                                                else
                                                    if (comselectcount != 0)
                                                        Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, religcnt, 1, comselectcount);
                                            }
                                        }
                                    }
                                }
                                if (cb_caste1.Checked == true)
                                {
                                    if (castecnt != 0)
                                        Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 1, castecnt);
                                }
                                else
                                {
                                    if (commcnt != 0)
                                        Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 1, commcnt);
                                }
                            }
                        }
                        #endregion
                        showrecord = true;
                    }
                }
                else if (ddl_reporttype.SelectedItem.Value == "1")
                {
                    #region Boardwise
                    if (cb_boardT.Checked == false)
                    {
                        Fpspread1.Visible = false;
                        rptprint.Visible = false;
                        lbl_error.Visible = true;
                        lbl_error.Text = "Please Select Board Details";
                        return;
                    }
                    Fpspread1.Sheets[0].Columns.Count++;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "S.NO";
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
                    for (i = 0; i < cbl_status.Items.Count; i++)
                    {
                        if (cbl_status.Items[i].Selected == true)
                        {
                            if (genchk)
                            {
                                Fpspread1.Sheets[0].ColumnCount++;
                            }
                            col = Fpspread1.Sheets[0].Columns.Count - 1;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_status.Items[i].Text;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                            if (cb_boardT.Checked == true)
                            {
                                if (cbl_BoardUniv.Items.Count > 0)
                                {
                                    bool booGeder = false;
                                    int gendColCnt = 0;
                                    gendColCnt = Fpspread1.Sheets[0].ColumnCount - 1;
                                    for (int j = 0; j < cbl_BoardUniv.Items.Count; j++)
                                    {
                                        if (cbl_BoardUniv.Items[j].Selected == true)
                                        {
                                            if (booGeder)
                                            {
                                                Fpspread1.Sheets[0].ColumnCount++;
                                            }
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_BoardUniv.Items[j].Text;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_BoardUniv.Items[j].Value;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                            booGeder = true;
                                            genchk = true;
                                        }
                                    }
                                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, gendColCnt, 1, boardselectcount);
                                }
                            }
                        }
                    }
                    #endregion
                    showrecord = true;
                }
                else if (ddl_reporttype.SelectedItem.Value == "2")
                {
                    #region disability
                    if (cb_phychallange.Checked == false)
                    {
                        Fpspread1.Visible = false;
                        rptprint.Visible = false;
                        lbl_error.Visible = true;
                        lbl_error.Text = "Please Select PhysicalChallange Details";
                        return;
                    }
                    Fpspread1.Sheets[0].Columns.Count++;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "S.NO";
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
                    for (i = 0; i < cbl_status.Items.Count; i++)
                    {
                        if (cbl_status.Items[i].Selected == true)
                        {
                            if (genchk)
                            {
                                Fpspread1.Sheets[0].ColumnCount++;
                            }
                            col = Fpspread1.Sheets[0].Columns.Count - 1;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_status.Items[i].Text;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                            if (cb_phychallange.Checked == true)
                            {
                                if (cbl_phychlg.Items.Count > 0)
                                {
                                    bool booGeder = false;
                                    int gendColCnt = 0;
                                    gendColCnt = Fpspread1.Sheets[0].ColumnCount - 1;
                                    for (int j = 0; j < cbl_phychlg.Items.Count; j++)
                                    {
                                        if (cbl_phychlg.Items[j].Selected == true)
                                        {
                                            if (booGeder)
                                            {
                                                Fpspread1.Sheets[0].ColumnCount++;
                                            }
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_phychlg.Items[j].Text;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_phychlg.Items[j].Value;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                            booGeder = true;
                                            genchk = true;
                                        }
                                    }
                                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, gendColCnt, 1, physicalselectcount);
                                }
                            }
                        }
                    }
                    #endregion
                    showrecord = true;
                }
                else if (ddl_reporttype.SelectedItem.Value == "3")
                {
                    #region minmax mark
                    Fpspread1.Sheets[0].Columns.Count++;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "S.NO";
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
                    for (i = 0; i < cbl_status.Items.Count; i++)
                    {
                        if (cbl_status.Items[i].Selected == true)
                        {
                            if (genchk)
                            {
                                Fpspread1.Sheets[0].ColumnCount++;
                            }
                            col = Fpspread1.Sheets[0].Columns.Count - 1;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_status.Items[i].Text;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                            if (cb_com.Checked == true)
                            {
                                if (cbl_comm.Items.Count > 0)
                                {
                                    bool booGeder = false;
                                    int gendColCnt = 0;
                                    gendColCnt = Fpspread1.Sheets[0].ColumnCount - 1;
                                    for (int j = 0; j < cbl_comm.Items.Count; j++)
                                    {
                                        bool markchk = false;
                                        if (cbl_comm.Items[j].Selected == true)
                                        {
                                            if (booGeder)
                                            {
                                                Fpspread1.Sheets[0].ColumnCount++;
                                            }
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_comm.Items[j].Text;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_comm.Items[j].Value;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                            if (markchk)
                                            {
                                                Fpspread1.Sheets[0].ColumnCount++;
                                            }
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Max-Min";
                                            markchk = true;
                                            booGeder = true;
                                            genchk = true;
                                        }
                                    }
                                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, gendColCnt, 1, comselectcount);
                                }
                            }
                        }
                    }
                    #endregion
                    showrecord = true;
                }
                if (rowheader > 0 && showrecord == true)
                {
                    #region spread value bind
                    Fpspread1.SaveChanges();
                    string section = "";
                    if (sec.Trim() != "")
                    {
                        section = " and isnull(r.Sections,'') in ('" + sec + "','')";
                    }
                    string q1 = "  select COUNT(r.app_no)as TotalStrength,isnull( r.Sections,'') as Sections,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name,a.sex,a.religion,a.community,a.caste  from Registration r,applyn a, degree d,Department dt,Course C where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No and isconfirm ='1' and admission_status ='1' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code in('" + degreecode + "')and r.Batch_Year in('" + batchyear + "') and  r.Current_Semester in('" + sem + "')   and a.college_code='" + ddlcollege.SelectedItem.Value + "' " + section + " " + datebetween + " group by No_Of_seats,r.degree_code ,r.Batch_Year,r.Current_Semester, C.Course_Name,c.Course_Id ,Dt.Dept_Name ,isnull( r.Sections,''),a.sex,a.religion,a.community,a.caste ";
                    q1 += " select COUNT(a.app_no)as TotalStrength,a.degree_code,a.Batch_Year,a.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name,a.sex,a.religion,a.community,a.caste  from applyn a, degree d,Department dt,Course C where d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and a.degree_code in('" + degreecode + "')and a.Batch_Year in('" + batchyear + "')  and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween1 + " group by No_Of_seats,a.degree_code ,a.Batch_Year,a.Current_Semester, C.Course_Name,c.Course_Id ,Dt.Dept_Name ,a.sex,a.religion,a.community,a.caste ";
                    //and  a.Current_Semester in('" + sem + "')
                    if (ddl_reporttype.SelectedIndex == 1)
                    {
                        q1 = " select COUNT(r.app_no)as TotalStrength,r.degree_code,s.course_code from Registration r,applyn a,Stud_prev_details s where a.app_no =r.App_No and isconfirm ='1' and r.App_No=s.app_no and admission_status ='1' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code in('" + degreecode + "')and r.Batch_Year in('" + batchyear + "') and  r.Current_Semester in('" + sem + "')  " + datebetween + " and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + section + "  group by r.degree_code,s.course_code   ";
                        q1 += "  select COUNT(a.app_no)as TotalStrength,a.degree_code,s.course_code   from applyn a,Stud_prev_details s where isconfirm ='1' and admission_status ='1' and a.degree_code in('" + degreecode + "')and a.Batch_Year in('" + batchyear + "')  " + datebetween1 + " and a.college_code='" + ddlcollege.SelectedItem.Value + "' and a.app_no=s.app_no   group by a.degree_code ,s.course_code  ";//and  a.Current_Semester in('" + sem + "')
                    }
                    if (ddl_reporttype.SelectedIndex == 2)
                    {
                        q1 = "  select COUNT(r.app_no)as TotalStrength,r.degree_code,visualhandy ,CONVERT(varchar(20), isdisabledisc)isdisabledisc,CONVERT(smallint, islearningdis)islearningdis,handy,case when isnull(visualhandy,0)=0 and isnull(islearningdis,0)=0 and isnull(handy,0)=0 then '1' end as others from Registration r,applyn a  where  a.app_no =r.App_No and isconfirm ='1' and admission_status ='1' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code in('" + degreecode + "')and r.Batch_Year in('" + batchyear + "') and  r.Current_Semester in('" + sem + "')   and a.college_code='" + ddlcollege.SelectedItem.Value + "' " + datebetween + " and isdisable='1' group by r.degree_code ,r.Batch_Year,r.Current_Semester, isnull( r.Sections,''),visualhandy, isdisabledisc, islearningdis, handy";//isdisable
                        q1 += "  select COUNT(app_no)as TotalStrength,degree_code,visualhandy,CONVERT(varchar(20), isdisabledisc)isdisabledisc,CONVERT(smallint, islearningdis)islearningdis,handy,case when isnull(visualhandy,0)=0 and isnull(islearningdis,0)=0 and isnull(handy,0)=0 then '1' end as others from applyn where  isconfirm ='1' and admission_status ='1' and degree_code in('" + degreecode + "')and Batch_Year in('" + batchyear + "') and college_code='" + ddlcollege.SelectedItem.Value + "' " + datebetween1 + " and isdisable='1' group by degree_code ,Batch_Year,Current_Semester,visualhandy, isdisabledisc, islearningdis, handy";// and Current_Semester in('" + sem + "')
                    }
                    if (ddl_reporttype.SelectedIndex == 3)
                    {
                        q1 = " select r.degree_code,a.community,CONVERT(varchar(10), max(s.percentage))+'-'+CONVERT(varchar(10),MIN(s.percentage))TotalStrength from Stud_prev_details s,Registration r,applyn a where a.app_no=s.app_no and a.app_no=r.App_No and  s.app_no=r.App_No and percentage is not null  and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code in('" + degreecode + "') and r.Batch_Year in('" + batchyear + "') and  r.Current_Semester in('" + sem + "')   and a.college_code='" + ddlcollege.SelectedItem.Value + "' " + datebetween + " group by a.community,r.degree_code order by degree_code ";
                        q1 += "   select a.degree_code,a.community,CONVERT(varchar(10), max(s.percentage))+'-'+CONVERT(varchar(10),MIN(s.percentage))TotalStrength from Stud_prev_details s,applyn a where a.app_no=s.app_no and percentage is not null  and a.degree_code in('" + degreecode + "') and a.Batch_Year in('" + batchyear + "')  " + datebetween1 + "  and a.college_code='" + ddlcollege.SelectedItem.Value + "' group by a.community,a.degree_code order by degree_code ";//and  a.Current_Semester in('" + sem + "')
                    }
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
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(cbl_branch.Items[i].Text);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(cbl_branch.Items[i].Value);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            for (int k = 2; k < Fpspread1.Sheets[0].ColumnCount; k++)
                            {
                                string status = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[0, k].Tag);
                                string gender = "";
                                string reglion = ""; string commm = ""; string caste = "";
                                string regF = "";
                                string comF = "";
                                string casteF = "";
                                string genderF = "";
                                string boardF = "";

                                #region Filter Condition
                                if (ddl_reporttype.SelectedIndex == 0)
                                {
                                    if (genderselectcount > 0 && comselectcount > 0 && religionselectcount == 0 && casteselectcount == 0)
                                        commm = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[2, k].Tag);
                                    if (genderselectcount > 0 && comselectcount == 0 && religionselectcount == 0 && casteselectcount > 0)
                                        caste = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[2, k].Tag);
                                    if (genderselectcount > 0 && comselectcount > 0 && casteselectcount > 0 && religionselectcount == 0)
                                    {
                                        caste = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[3, k].Tag);
                                        commm = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[2, k].Tag);
                                    }
                                    if (genderselectcount > 0 && religionselectcount > 0 && casteselectcount > 0 && comselectcount == 0)
                                    {
                                        reglion = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[2, k].Tag);
                                        caste = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[3, k].Tag);
                                    }
                                    //without gender
                                    if (genderselectcount == 0 && comselectcount > 0 && casteselectcount == 0 && religionselectcount == 0)
                                    {
                                        commm = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, k].Tag);
                                    }
                                    if (genderselectcount == 0 && religionselectcount > 0 && comselectcount == 0 && casteselectcount == 0)
                                    {
                                        reglion = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, k].Tag);
                                    }
                                    if (genderselectcount == 0 && casteselectcount > 0 && comselectcount == 0 && religionselectcount == 0)
                                    {
                                        caste = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, k].Tag);
                                    }
                                    if (genderselectcount == 0 && casteselectcount > 0 && religionselectcount > 0 && comselectcount == 0)
                                    {
                                        reglion = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, k].Tag);
                                        caste = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[2, k].Tag);
                                    }
                                    if (genderselectcount == 0 && (religionselectcount > 0 && comselectcount > 0) || (religionselectcount > 0 && casteselectcount > 0 && comselectcount > 0))
                                    {
                                        if (religionselectcount > 0 && comselectcount > 0 && casteselectcount > 0)
                                        {
                                            reglion = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, k].Tag);
                                            commm = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[2, k].Tag);
                                            caste = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[3, k].Tag);
                                        }
                                        if (religionselectcount > 0 && comselectcount > 0)
                                        {
                                            reglion = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, k].Tag);
                                            commm = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[2, k].Tag);
                                        }
                                    }
                                    if (cb_genderT.Checked == true)
                                        gender = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, k].Tag);
                                    if (reglion.Trim() == "")
                                        if (cb_religion1.Checked == true)
                                            reglion = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[2, k].Tag);
                                    if (commm.Trim() == "")
                                        if (cb_com.Checked == true)
                                            commm = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[3, k].Tag);
                                    if (caste.Trim() == "")
                                        if (cb_caste1.Checked == true)
                                            caste = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[4, k].Tag);
                                    if (cb_genderT.Checked == true)
                                        if (gender.Trim() != "")
                                            genderF = " and sex='" + gender + "'";
                                    if (cb_religion1.Checked == true)
                                        if (reglion.Trim() != "")
                                            regF = " and  religion='" + reglion + "'";
                                    if (cb_com.Checked == true)
                                        if (commm.Trim() != "")
                                            comF = " and community='" + commm + "' ";
                                    if (cb_caste1.Checked == true)
                                        if (caste.Trim() != "")
                                            casteF = " and caste='" + caste + "' ";

                                }
                                string board = "";
                                if (ddl_reporttype.SelectedIndex == 1)
                                {
                                    if (cb_boardT.Checked == true)
                                        board = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, k].Tag);
                                    if (cb_boardT.Checked == true)
                                        boardF = " and course_code='" + board + "'";
                                }
                                if (ddl_reporttype.SelectedIndex == 3)
                                {
                                    if (cb_com.Checked == true)
                                        commm = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, k].Tag);
                                    if (cb_com.Checked == true)
                                        comF = " and community='" + commm + "' ";
                                }
                                string disable = "";
                                if (ddl_reporttype.SelectedIndex == 2)
                                {
                                    disable = " and " + Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, k].Tag) + " ='1'";
                                }
                                #endregion

                                double countval = 0;
                                string markper = "";
                                if (ddl_reporttype.SelectedIndex == 3)
                                {
                                    DataView dv = new DataView();
                                    if (status.Trim() == "1")
                                    {
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            ds.Tables[0].DefaultView.RowFilter = " degree_code='" + Convert.ToString(cbl_branch.Items[i].Value) + "' " + comF + " ";
                                            dv = ds.Tables[0].DefaultView;
                                            if (dv.Count > 0)
                                            {
                                                markper = Convert.ToString(dv[0]["TotalStrength"]);
                                            }
                                        }
                                    }
                                    if (status.Trim() == "2")
                                    {
                                        if (ds.Tables[1].Rows.Count > 0)
                                        {
                                            ds.Tables[1].DefaultView.RowFilter = " degree_code='" + Convert.ToString(cbl_branch.Items[i].Value) + "' " + comF + " ";
                                            dv = ds.Tables[0].DefaultView;
                                            if (dv.Count > 0)
                                            {
                                                markper = Convert.ToString(dv[0]["TotalStrength"]);
                                            }
                                        }
                                    }
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Text = markper;
                                }
                                else
                                {
                                    if (status.Trim() == "1")
                                    {
                                        if (ds.Tables[1].Rows.Count > 0)
                                        {
                                            double.TryParse(Convert.ToString(ds.Tables[1].Compute("Sum(TotalStrength)", " degree_code='" + Convert.ToString(cbl_branch.Items[i].Value) + "' " + genderF + " " + regF + " " + casteF + " " + comF + " " + boardF + " " + disable + "")), out countval);
                                        }
                                    }
                                    if (status.Trim() == "2")
                                    {
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum(TotalStrength)", "  degree_code='" + Convert.ToString(cbl_branch.Items[i].Value) + "' " + genderF + " " + regF + " " + casteF + " " + comF + " " + boardF + " " + disable + "")), out countval);
                                        }
                                    }
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Text = Convert.ToString(countval);
                                }
                                if (ddl_reporttype.SelectedIndex == 2)
                                {
                                    string disability = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, k].Tag);
                                    totalvalue += countval;
                                    total += countval;
                                    if (totalvalue_dic.Contains(status + "-" + disability))
                                    {
                                        value1 = "";
                                        value1 = totalvalue_dic[status + "-" + disability].ToString();
                                        totalvalue_dic.Remove(status + "-" + disability);
                                        total = 0;
                                        total = Convert.ToInt32(value1) + Convert.ToInt32(countval);
                                        totalvalue_dic.Add(status + "-" + disability, total);
                                    }
                                    else
                                    {
                                        totalvalue_dic.Add(status + "-" + disability, Convert.ToInt32(countval));
                                    }
                                }
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
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Total";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, Fpspread1.Sheets[0].ColumnCount - 1, rowheader, 1);
                for (int r = 0; r < Fpspread1.Sheets[0].RowCount; r++)
                {
                    rowtotal = 0;
                    for (int c = 2; c < Fpspread1.Sheets[0].ColumnCount - 1; c++)
                    {
                        Fpspread1.Sheets[0].Cells[r, c].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[r, c].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[r, c].Font.Size = FontUnit.Medium;
                        double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[r, c].Text), out value);
                        totalval += value;
                        rowtotal += value;
                    }
                    Fpspread1.Sheets[0].Cells[r, Fpspread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(rowtotal);
                    Fpspread1.Sheets[0].Cells[r, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[r, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                }
                if (ddl_reporttype.SelectedIndex == 2)
                {
                    Fpspread1.Sheets[0].RowCount++;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = "Grand Total";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                    for (int k = 2; k < Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1; k++)
                    {
                        string status = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[0, k].Tag);
                        string disability1 = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, k].Tag);
                        if (totalvalue_dic.Count > 0)
                        {
                            value1 = "";
                            if (totalvalue_dic.Contains(status + "-" + disability1))
                            {
                                value1 = Convert.ToString(totalvalue_dic[status + "-" + disability1]);
                            }
                            else
                            {
                                value1 = "0";
                            }
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Text = value1;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Font.Name = "Book Antiqua";
                        }
                    }
                    Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].BackColor = Color.Bisque;
                    Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].ForeColor = Color.IndianRed;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = Convert.ToString(totalvalue);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Name = "Book Antiqua";
                }
                if (ddl_reporttype.SelectedIndex == 3)
                    Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].Visible = false;
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
        }
        catch (Exception Ex)
        {
            lbl_error.Visible = true;
            lbl_error.Text = Ex.ToString();
            Fpspread1.Visible = false;
            rptprint.Visible = false;
        }
    }
    //Disability details wise
    protected void disabilitydetailswise(string batchyear, string degreecode, string sem, string datebetween)
    {
        Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
        Fpspread1.Sheets[0].Columns.Count++;
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "S.No";
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
        Fpspread1.Sheets[0].Columns.Count++;
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Roll No";
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
        Fpspread1.Sheets[0].Columns.Count++;
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Reg No";
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
        Fpspread1.Sheets[0].Columns.Count++;
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Admission No";
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
        Fpspread1.Sheets[0].Columns.Count++;
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Student Name";
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
        Fpspread1.Sheets[0].Columns.Count++;
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Batch Year";
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
        Fpspread1.Sheets[0].Columns.Count++;
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Department Name";
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
        Fpspread1.Sheets[0].Columns.Count++;
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Address";
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
        Fpspread1.Sheets[0].Columns.Count++;
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Mobile No";
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
        Fpspread1.Sheets[0].Columns.Count++;
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Details";
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
        Fpspread1.Sheets[0].Columns.Count++;
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Description";
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;

        string q1 = " select r.app_no,r.Roll_No,r.Reg_No,r.Roll_Admit,r.Stud_Name,r.Batch_Year, r.degree_code,(Course_Name+' - '+Dept_Name) as Dept,case when visualhandy='1' then 'Visually Challanged' when islearningdis='1' then 'Learning Disability' when handy='1' then 'Handy' else 'others' end disability ,CONVERT(varchar(20), isdisabledisc)isdisabledisc,(a.parent_addressC+','+a.parent_pincodec+','+(select textval from textvaltable where TextCode=a.parent_statec)) as Address,a.Student_Mobile from Registration r,applyn a,Degree d,course c,Department dt where d.Degree_Code=r.degree_code and c.Course_Id=d.Course_Id and dt.Dept_Code=d.Dept_Code  and  a.app_no =r.App_No and isconfirm ='1' and admission_status ='1' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code in('" + degreecode + "')and r.Batch_Year in('" + batchyear + "') and  r.Current_Semester in('" + sem + "')   and a.college_code='" + ddlcollege.SelectedItem.Value + "' " + datebetween + "   and isdisable='1'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(q1, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                i++;
                Fpspread1.Sheets[0].RowCount++;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = i.ToString();
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dr["roll_no"]);
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dr["Reg_No"]);
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dr["Roll_Admit"]);
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dr["Stud_Name"]);
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dr["Batch_Year"]);
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dr["Dept"]);
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dr["Address"]);
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                //Student_Mobile

                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].CellType = txt;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(dr["Student_Mobile"]);
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(dr["disability"]); // Convert.ToString(dr["isdisabledisc"]);
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(dr["isdisabledisc"]);
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Left;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";

                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(dr["roll_no"]);
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 11].Font.Size = FontUnit.Medium;
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.Visible = true;
                rptprint.Visible = true;
            }
        }
    }



    protected void fpreadSinglefilterselected(CheckBoxList Cbl, CheckBox cb, int rowheadercout)
    {
        bool genchk = false; int col = 0; int spancolumn = 0;
        Fpspread1.Sheets[0].Columns.Count++;
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "S.NO";
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
        Fpspread1.Sheets[0].Columns.Count++;
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Department";
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
        Fpspread1.Sheets[0].Columns.Count++;
        Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, rowheadercout, 1);
        Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, rowheadercout, 1);
        for (i = 0; i < cbl_status.Items.Count; i++)
        {
            if (cbl_status.Items[i].Selected == true)
            {
                if (genchk)
                {
                    Fpspread1.Sheets[0].ColumnCount++;
                }
                int.TryParse(cblselecteditemcount(Cbl), out spancolumn);
                col = Fpspread1.Sheets[0].Columns.Count - 1;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = cbl_status.Items[i].Text;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                if (cb.Checked == true)
                {
                    if (Cbl.Items.Count > 0)
                    {
                        bool booGeder = false;
                        for (int j = 0; j < Cbl.Items.Count; j++)
                        {
                            if (Cbl.Items[j].Selected == true)
                            {
                                if (booGeder)
                                    Fpspread1.Sheets[0].ColumnCount++;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Text = Cbl.Items[j].Text;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = Cbl.Items[j].Value;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_status.Items[i].Value;
                                booGeder = true;
                                genchk = true;
                            }
                        }
                        Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 1, spancolumn);
                    }
                }
            }
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
            string pagename = "StudentMod_Student_applied_admited_details_report.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        { }
    }
    protected void ddl_reporttype_selectedindexchange(object sender, EventArgs e)
    {
        rptprint.Visible = false;
        if (ddl_reporttype.SelectedIndex == 0)
        {
            physicalchange_td.Visible = false;
            cb_genderT.Visible = true;
            cb_religion1.Visible = true;
            cb_com.Visible = true;
            cb_caste1.Visible = true;
            cb_phychallange.Visible = false;
            cb_boardT.Visible = false;
            td_board.Visible = false;
            Fpspread1.Visible = false;
            tdphychallange.Visible = false;
        }
        else if (ddl_reporttype.SelectedIndex == 1)
        {
            physicalchange_td.Visible = false;
            cb_genderT.Visible = false;
            cb_religion1.Visible = false;
            cb_com.Visible = false;
            cb_caste1.Visible = false;
            cb_phychallange.Visible = false;
            cb_boardT.Visible = true;
            tdcaste.Visible = false;
            tdcommchk1.Visible = false;
            tdgender.Visible = false;
            tdphychallange.Visible = false;
            cb_religion1.Visible = false;
            tdrelichk1.Visible = false;
            Fpspread1.Visible = false;
        }
        else if (ddl_reporttype.SelectedIndex == 2)
        {
            physicalchange_td.Visible = true;
            cb_genderT.Visible = false;
            cb_religion1.Visible = false;
            cb_com.Visible = false;
            cb_caste1.Visible = false;
            cb_phychallange.Visible = true;
            cb_boardT.Visible = false;
            td_board.Visible = false;
            tdcaste.Visible = false;
            tdcommchk1.Visible = false;
            tdgender.Visible = false;
            tdrelichk1.Visible = false;
            cb_religion1.Visible = false;
            Fpspread1.Visible = false;
        }
        else if (ddl_reporttype.SelectedIndex == 3)
        {
            physicalchange_td.Visible = false;
            cb_genderT.Visible = false;
            cb_religion1.Visible = false;
            cb_com.Visible = true;
            cb_caste1.Visible = false;
            cb_phychallange.Visible = false;
            cb_boardT.Visible = false;
            tdcaste.Visible = false;
            tdcommchk1.Visible = true;
            tdgender.Visible = false;
            tdphychallange.Visible = false;
            cb_religion1.Visible = false;
            tdrelichk1.Visible = false;
            Fpspread1.Visible = false;
            td_board.Visible = false;
        }
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
}