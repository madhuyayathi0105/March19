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
using System.Drawing;
public partial class Inv_Dayscholar_stud_staff : System.Web.UI.Page
{
    string user_code;
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    Boolean Cellclick = false;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string rollflag1 = string.Empty;
    string regflag1 = string.Empty;
    string stuflag1 = string.Empty;
    string date = DateTime.Now.ToString("dd/MM/yyyy");
    string college_code = "";
    string college = "";
    DataSet ds2 = new DataSet();
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    bool check = false;
    bool flag = false;
    private object sender;
    private EventArgs e;
    string sql = "";
    int rowcount;
    static string code = "";
    static string deptcod = "";
    string grouporusercode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        //****************************************************//
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("hostelindex"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/HostelMod/hostelindex.aspx");
                return;
            }
        }
        //****************************************************//
        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        hat.Add("single_user", singleuser);
        hat.Add("group_code", group_user);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
        }
        else
        {
            grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
        }
        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        string Master = "select * from Master_Settings where " + grouporusercode + "";
        DataSet ds = d2.select_method(Master, hat, "Text");
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            if (ds.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
            {
                Session["Rollflag"] = "1";
                rollflag1 = Session["Rollflag"].ToString();
            }
            if (ds.Tables[0].Rows[i]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
            {
                Session["Regflag"] = "1";
                regflag1 = Session["Regflag"].ToString();
            }
            if (ds.Tables[0].Rows[i]["settings"].ToString() == "Student_Type" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
            {
                Session["Studflag"] = "1";
                stuflag1 = Session["Studflag"].ToString();
            }
        }
        if (!IsPostBack)
        {
            //stafftrue();//new28/08/15            
            rdb_staff.Checked = false;
            rdb_student.Checked = true;
            bindcollege();
            loadhostel();
            loadsession();
            bindbatch();
            // degree();
            bindbranch(college);
            bindcbldept();
            binddesig();
            bindstafftype();
            bindsection();
            loadhostelpopup();
            // bindsex();
            studentrue();
            bindstaffsession();
            binddepartment();
            loadsessionnew();
            loadhour();
            loadsecond();
            loadminits();
            timevalue();
            bindhostelname1();
            bindhostelnamestaff();
            bindbatch1();
            binddegree2();
            bindbranch1(college);
            lbl_error1.Visible = false;
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 0;
            Fpspread1.Visible = false;
            //   div1.Visible = false;
            btn_ok.Visible = false;
            btn_exit2.Visible = false;
            Fpspread2.Sheets[0].RowCount = 0;
            Fpspread2.Sheets[0].ColumnCount = 0;
            txt_Search.Visible = true;
            Session["staffc"] = null;
            txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_date.Attributes.Add("readonly", "readonly");
            txt_stf_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_stf_date.Attributes.Add("readonly", "readonly");
            //magesh 12.3.18
            BindStudentType();
        }
        lbl_error1.Visible = false;
        lbl_validation1.Visible = false;
    }
    public void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }
    //home page
    protected void bindcollege()
    {
        try
        {
            ds.Clear();
            ddl_college.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college.DataSource = ds;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();
                ddl_collegename1.DataSource = ds;
                ddl_collegename1.DataTextField = "collname";
                ddl_collegename1.DataValueField = "college_code";
                ddl_collegename1.DataBind();
            }
            degree();
        }
        catch
        {
        }
    }
    public void loadhostel()
    {
        try
        {
            //ds.Clear();
            //cbl_hostelname.Items.Clear();
            //string selecthostel = "select Hostel_code,Hostel_Name  from Hostel_Details order by Hostel_code";
            //ds = d2.select_method_wo_parameter(selecthostel, "Text");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    cbl_hostelname.DataSource = ds;
            //    cbl_hostelname.DataTextField = "Hostel_Name";
            //    cbl_hostelname.DataValueField = "Hostel_code";
            //    cbl_hostelname.DataBind();
            //    if (cbl_hostelname.Items.Count > 0)
            //    {
            //        for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            //        {
            //            cbl_hostelname.Items[i].Selected = true;
            //        }
            //        txt_hostelname.Text = "Hostel Name(" + cbl_hostelname.Items.Count + ")";
            //    }
            //}
            //else
            //{
            //    txt_hostelname.Text = "--Select--";
            //}
            // 15.10.15 theivamani
            ds.Clear();
            cbl_hostelname.Items.Clear();
            //string selecthostel = "select Hostel_code,Hostel_Name  from Hostel_Details order by Hostel_code";
            //ds = d2.select_method_wo_parameter(selecthostel, "Text");
            //ds = d2.Bindmess_inv(collegecode1);
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname.DataSource = ds;
                cbl_hostelname.DataTextField = "MessName";
                cbl_hostelname.DataValueField = "MessMasterPK";
                cbl_hostelname.DataBind();
                if (cbl_hostelname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                    {
                        cbl_hostelname.Items[i].Selected = true;
                    }
                    txt_hostelname.Text = "Mess Name(" + cbl_hostelname.Items.Count + ")";
                }
            }
            else
            {
                txt_hostelname.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void cb_hostelname_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_hostelname.Checked == true)
        {
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                cbl_hostelname.Items[i].Selected = true;
            }
            txt_hostelname.Text = "Mess Name(" + (cbl_hostelname.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                cbl_hostelname.Items[i].Selected = false;
            }
            txt_hostelname.Text = "--Select--";
        }
        loadsession();
    }
    protected void cbl_hostelname_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_hostelname.Text = "--Select--";
        cb_hostelname.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cbl_hostelname.Items.Count; i++)
        {
            if (cbl_hostelname.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txt_hostelname.Text = "Mess Name(" + commcount.ToString() + ")";
            if (commcount == cbl_hostelname.Items.Count)
            {
                cb_hostelname.Checked = true;
            }
        }
        loadsession();
    }
    public void loadsession()
    {
        try
        {
            ds.Clear();
            cbl_sessionname.Items.Clear();
            string itemheader = "";
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_hostelname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_hostelname.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (itemheader.Trim() != "")
            {
                //string selecthostel = "select distinct Session_Code,Session_Name  from Session_Master where Hostel_Code in ('" + itemheader + "')";
                //ds = d2.select_method_wo_parameter(selecthostel, "Text");
                ds = d2.BindSession_inv(itemheader);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_sessionname.DataSource = ds;
                    cbl_sessionname.DataTextField = "SessionName";
                    cbl_sessionname.DataValueField = "SessionMasterPK";
                    cbl_sessionname.DataBind();
                    if (cbl_sessionname.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_sessionname.Items.Count; i++)
                        {
                            cbl_sessionname.Items[i].Selected = true;
                        }
                        txt_sessionname.Text = "Session Name(" + cbl_sessionname.Items.Count + ")";
                    }
                }
                else
                {
                    txt_sessionname.Text = "--Select--";
                }
            }
            else
            {
                txt_sessionname.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void cb_sessionname_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_sessionname.Checked == true)
        {
            for (int i = 0; i < cbl_sessionname.Items.Count; i++)
            {
                cbl_sessionname.Items[i].Selected = true;
            }
            txt_sessionname.Text = "Session Name(" + (cbl_sessionname.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_sessionname.Items.Count; i++)
            {
                cbl_sessionname.Items[i].Selected = false;
            }
            txt_sessionname.Text = "--Select--";
        }
    }
    protected void cbl_sessionname_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_sessionname.Text = "--Select--";
        cb_sessionname.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cbl_sessionname.Items.Count; i++)
        {
            if (cbl_sessionname.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txt_sessionname.Text = "Session Name(" + commcount.ToString() + ")";
            if (commcount == cbl_sessionname.Items.Count)
            {
                cb_sessionname.Checked = true;
            }
        }
    }
    public void cb_both_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_both.Checked == true)
        {
            txt_batch.Enabled = true;
            txt_branch.Enabled = true;
            txt_degree.Enabled = true;
        }
        else if (cb_both.Checked == false)
        {
            txt_batch.Enabled = false;
            txt_branch.Enabled = false;
            txt_degree.Enabled = false;
        }
    }
    public void bindbatch()
    {
        try
        {
            cbl_batch.Items.Clear();
            hat.Clear();
            //string sqlyear = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year desc";
            string Master1 = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                string group = Session["group_code"].ToString();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = group_semi[0].ToString();
                }
            }
            else
            {
                Master1 = Session["usercode"].ToString();
            }
            string sqlyear = "";
            sqlyear = "select distinct batch_year from tbl_attendance_rights where user_id='" + Master1 + "' and college_code='" + collegecode1 + "'  order by batch_year desc ";
            ds = d2.select_method(sqlyear, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_batch.DataSource = ds;
                cbl_batch.DataTextField = "batch_year";
                cbl_batch.DataValueField = "batch_year";
                cbl_batch.DataBind();
                if (cbl_batch.Items.Count > 0)
                {
                    for (int ro = 0; ro < cbl_batch.Items.Count; ro++)
                    {
                        cbl_batch.Items[ro].Selected = true;
                    }
                    txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
                }
            }
        }
        catch
        {
        }
    }
    public void cb_batch_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_batch.Checked == true)
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = true;
                }
                txt_batch.Text = "Batch(" + (cbl_batch.Items.Count) + ")";
                degree();
                bindbranch(college);
                bindsection();
            }
            else
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = false;
                }
                txt_batch.Text = "--Select--";
                txt_degree.Text = "--Select--";
                txt_branch.Text = "--Select--";
                txt_section.Text = "--Select--";
                cbl_degree.Items.Clear();
                cbl_branch.Items.Clear();
                cbl_section.Items.Clear();
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_batch.Text = "--Select--";
            cb_batch.Checked = false;
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
                degree();
                bindbranch(college);
                bindsection();
            }
            if (commcount > 0)
            {
                txt_batch.Text = "Batch(" + commcount.ToString() + ")";
                if (commcount == cbl_batch.Items.Count)
                {
                    cb_batch.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void degree()
    {
        try
        {
            cbl_degree.Items.Clear();
            user_code = Session["usercode"].ToString();
            // college_code = Session["collegecode"].ToString();
            // theivamani 30.10.15
            college_code = ddl_college.SelectedItem.Value.ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Clear();
            hat.Add("single_user", singleuser.ToString());
            hat.Add("group_code", group_user);
            hat.Add("college_code", college_code);
            hat.Add("user_code", user_code);
            ds = d2.select_method("bind_degree", hat, "sp");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
                if (cbl_degree.Items.Count > 0)
                {
                    for (int ro = 0; ro < cbl_degree.Items.Count; ro++)
                    {
                        cbl_degree.Items[ro].Selected = true;
                    }
                    txt_degree.Text = "Degree(" + cbl_degree.Items.Count + ")";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_degree_ChekedChange(object sender, EventArgs e)
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
                        txt_degree.Text = "Degree(" + (cbl_degree.Items.Count) + ")";
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
                    bindbranch(buildvalue1);
                    bindsection();
                }
            }
            //  bindbranch(buildvalue1);
            else
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = false;
                    txt_degree.Text = "--Select--";
                    txt_branch.Text = "--Select--";
                    txt_section.Text = "--Select--";
                    cbl_branch.Items.Clear();
                    cb_branch.Checked = false;
                    cbl_section.Items.Clear();
                    cb_section.Checked = false;
                }
            }
            // bindbranch(college);
            // Button2.Focus();
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
            bindsection();
            if (seatcount == cbl_degree.Items.Count)
            {
                txt_degree.Text = "Degree(" + seatcount.ToString() + ")";
                cb_degree.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_degree.Text = "--Select--";
                txt_degree.Text = "--Select--";
            }
            else
            {
                txt_degree.Text = "Degree(" + seatcount.ToString() + ")";
            }
            bindbranch(college);
        }
        catch (Exception ex)
        {
        }
    }
    public void bindbranch(string branch)
    {
        try
        {
            txt_branch.Text = "--Select--";
            cbl_branch.Items.Clear();
            string itemheader = "";
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_degree.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_degree.Items[i].Value.ToString() + "";
                    }
                }
            }
            string commname = "";
            if (itemheader != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + itemheader + "') and deptprivilages.Degree_code=degree.Degree_code ";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code";
            }
            ds = d2.select_method(commname, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_branch.DataSource = ds;
                cbl_branch.DataTextField = "dept_name";
                cbl_branch.DataValueField = "degree_code";
                cbl_branch.DataBind();
                if (cbl_branch.Items.Count > 0)
                {
                    for (int ro = 0; ro < cbl_branch.Items.Count; ro++)
                    {
                        cbl_branch.Items[ro].Selected = true;
                    }
                    txt_branch.Text = "Branch(" + cbl_branch.Items.Count + ")";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_branch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_branch.Checked == true)
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = true;
                }
                txt_branch.Text = "Branch(" + (cbl_branch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = false;
                }
                txt_branch.Text = "--Select--";
                txt_section.Text = "--Select--";
            }
            bindsection();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_branch.Text = "--Select--";
            cb_branch.Checked = false;
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_branch.Items.Count)
            {
                txt_branch.Text = "Branch(" + commcount.ToString() + ")";
                cb_branch.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_degree.Text = "--Select--";
            }
            else
            {
                txt_branch.Text = "Branch(" + commcount.ToString() + ")";
            }
            bindsection();
        }
        catch (Exception ex)
        {
        }
    }
    protected void bindcbldept()
    {
        try
        {
            ds.Clear();
            string query = "";
            query = "select distinct dept_name,dept_code from hrdept_master where college_code='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_department.DataSource = ds;
                cbl_department.DataTextField = "dept_name";
                cbl_department.DataValueField = "dept_code";
                cbl_department.DataBind();
                if (cbl_department.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_department.Items.Count; i++)
                    {
                        cbl_department.Items[i].Selected = true;
                    }
                    txt_department.Text = "Department(" + cbl_department.Items.Count + ")";
                }
            }
            else
            {
                txt_department.Text = "--Select--";
            }
        }
        catch { }
    }
    public void cb_department_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_department.Checked == true)
            {
                for (int i = 0; i < cbl_department.Items.Count; i++)
                {
                    cbl_department.Items[i].Selected = true;
                }
                txt_department.Text = "Department(" + (cbl_department.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_department.Items.Count; i++)
                {
                    cbl_department.Items[i].Selected = false;
                }
                txt_department.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_department_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_department.Text = "--Select--";
            cb_department.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_department.Items.Count; i++)
            {
                if (cbl_department.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_department.Text = "Department(" + commcount.ToString() + ")";
                if (commcount == cbl_department.Items.Count)
                {
                    cb_department.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void binddesig()
    {
        try
        {
            ds.Clear();
            ds = d2.binddesi(collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_design.DataSource = ds;
                cbl_design.DataTextField = "desig_name";
                cbl_design.DataValueField = "desig_code";
                cbl_design.DataBind();
                if (cbl_design.Items.Count > 0)
                {
                    for (int ro = 0; ro < cbl_design.Items.Count; ro++)
                    {
                        cbl_design.Items[ro].Selected = true;
                    }
                    txt_design.Text = "Designation(" + cbl_design.Items.Count + ")";
                }
            }
        }
        catch
        {
        }
    }
    public void cb_desig_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_design.Checked == true)
            {
                for (int i = 0; i < cbl_design.Items.Count; i++)
                {
                    cbl_design.Items[i].Selected = true;
                }
                txt_design.Text = "Designation(" + (cbl_design.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_design.Items.Count; i++)
                {
                    cbl_design.Items[i].Selected = false;
                }
                txt_design.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_desig_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_design.Text = "--Select--";
            cb_design.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_design.Items.Count; i++)
            {
                if (cbl_design.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_design.Text = "Designation(" + commcount.ToString() + ")";
                if (commcount == cbl_design.Items.Count)
                {
                    cb_design.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindstafftype()
    {
        try
        {
            ds.Clear();
            ds = d2.loadstafftype(collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_stafftype.DataSource = ds;
                cbl_stafftype.DataTextField = "StfType";
                cbl_stafftype.DataValueField = "StfType";
                cbl_stafftype.DataBind();
                if (cbl_stafftype.Items.Count > 0)
                {
                    for (int ro = 0; ro < cbl_stafftype.Items.Count; ro++)
                    {
                        cbl_stafftype.Items[ro].Selected = true;
                    }
                    txt_stafftype.Text = "Staff Type(" + cbl_stafftype.Items.Count + ")";
                }
            }
        }
        catch
        {
        }
    }
    public void cb_stafftype_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_stafftype.Checked == true)
            {
                for (int i = 0; i < cbl_stafftype.Items.Count; i++)
                {
                    cbl_stafftype.Items[i].Selected = true;
                }
                txt_stafftype.Text = "Staff Type(" + (cbl_stafftype.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_stafftype.Items.Count; i++)
                {
                    cbl_stafftype.Items[i].Selected = false;
                }
                txt_stafftype.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_stafftype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_stafftype.Text = "--Select--";
            cb_stafftype.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_stafftype.Items.Count; i++)
            {
                if (cbl_stafftype.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_stafftype.Text = "Staff Type(" + commcount.ToString() + ")";
                if (commcount == cbl_stafftype.Items.Count)
                {
                    cb_stafftype.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindsection()
    {
        try
        {
            string itemheader = "";
            cbl_section.Items.Clear();
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "," + "" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                }
            }
            string itemheader1 = "";
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (itemheader1 == "")
                    {
                        itemheader1 = "" + cbl_batch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader1 = itemheader1 + "," + "" + cbl_batch.Items[i].Value.ToString() + "";
                    }
                }
            }
            ds.Clear();
            if (itemheader.Trim() != "" && itemheader1.Trim() != "")
            {
                ds = d2.BindSectionDetail(itemheader1, itemheader);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_section.DataSource = ds;
                    cbl_section.DataTextField = "sections";
                    cbl_section.DataValueField = "sections";
                    cbl_section.DataBind();
                    if (cbl_section.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_section.Items.Count; row++)
                        {
                            cbl_section.Items[row].Selected = true;
                        }
                        txt_section.Text = "Section (" + cbl_section.Items.Count + ")";
                    }
                }
            }
        }
        catch
        {
        }
    }
    public void cb_section_checkedchange(object sender, EventArgs e)
    {
        if (cb_section.Checked == true)
        {
            for (int i = 0; i < cbl_section.Items.Count; i++)
            {
                cbl_section.Items[i].Selected = true;
            }
            txt_section.Text = "Section(" + cbl_section.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < cbl_section.Items.Count; i++)
            {
                cbl_section.Items[i].Selected = false;
            }
            txt_section.Text = "--Select--";
        }
    }
    public void cbl_section_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_section.Text = "--Select--";
        cb_section.Checked = false;
        int ccount = 0;
        for (int i = 0; i < cbl_section.Items.Count; i++)
        {
            if (cbl_section.Items[i].Selected == true)
            {
                ccount = ccount + 1;
                cb_section.Checked = false;
            }
        }
        if (ccount > 0)
        {
            txt_section.Text = "Section(" + ccount.ToString() + ")";
            if (ccount == cbl_section.Items.Count)
            {
                cb_section.Checked = true;
            }
            //txtpop3session.Text = "Session Name(" + ccount.ToString() + ")";
        }
    }
    //public void bindsex()
    //{
    //    cbl_sex.Items.Clear();
    //    txt_sex.Text = "---Select---";
    //    cb_sex.Checked = false;
    //    cbl_sex.Items.Insert(0, "Male");
    //    cbl_sex.Items.Insert(1, "Female");
    //    cbl_sex.Items.Insert(2, "Transgender");
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        if (cbl_sex.Items.Count > 0)
    //        {
    //            for (int i = 0; i < cbl_sex.Items.Count; i++)
    //            {
    //                cbl_sex.Items[i].Selected = true;
    //            }
    //            txt_sex.Text = "Gender(" + cbl_sex.Items.Count + ")";
    //            //  cb_criteria.Checked = true;
    //        }
    //    }
    //    else
    //    {
    //        txt_sex.Text = "--Select--";
    //    }
    //}
    public void bindsex()
    {
        try
        {
            ds.Clear();
            cbl_sex.Items.Clear();
            //string selecthostel = "select Hostel_code,Hostel_Name from Hostel_Details order by Hostel_code";
            //ds = d2.select_method_wo_parameter(selecthostel, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                //cbl_sex.DataSource = ds;
                //cbl_sex.DataTextField = "Hostel_Name";
                //cbl_sex.DataValueField = "Hostel_code";
                //cbl_sex.DataBind();
                if (cbl_sex.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_sex.Items.Count; i++)
                    {
                        cbl_sex.Items[i].Selected = true;
                    }
                    txt_sex.Text = "Gender(" + cbl_sex.Items.Count + ")";
                }
            }
            else
            {
                txt_sex.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void cb_sex_checkedchange(object sender, EventArgs e)
    {
        if (cb_sex.Checked == true)
        {
            for (int i = 0; i < cbl_sex.Items.Count; i++)
            {
                cbl_sex.Items[i].Selected = true;
            }
            txt_sex.Text = "Gender(" + cbl_sex.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < cbl_sex.Items.Count; i++)
            {
                cbl_sex.Items[i].Selected = false;
            }
            txt_sex.Text = "--Select--";
        }
    }
    public void cbl_sex_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_sex.Text = "--Select--";
        cb_sex.Checked = false;
        int ccount = 0;
        for (int i = 0; i < cbl_sex.Items.Count; i++)
        {
            if (cbl_sex.Items[i].Selected == true)
            {
                ccount = ccount + 1;
                cb_sex.Checked = false;
            }
        }
        if (ccount > 0)
        {
            txt_sex.Text = "Gender(" + ccount.ToString() + ")";
            if (ccount == cbl_sex.Items.Count)
            {
                cb_sex.Checked = true;
            }
            //txtpop3session.Text = "Session Name(" + ccount.ToString() + ")";
        }
    }
    protected void rdb_staff_Select(object sender, EventArgs e)
    {
        Fpspread1.Visible = false;
        rptprint.Visible = false;
        lbl_stucnt.Visible = false;
        // div1.Visible = false;
        stafftrue();
        studentfalse();
    }
    protected void rdb_student_select(object sender, EventArgs e)
    {
        Fpspread1.Visible = false;
        rptprint.Visible = false;
        lbl_staffcnt.Visible = false;
        studentrue();
        stafffalse();
    }
    protected void stafffalse()
    {
        lbl_department.Visible = false;
        txt_department.Visible = false;
        pp0.Visible = false;
        lbl_design.Visible = false;
        txt_design.Visible = false;
        pp2.Visible = false;
        lblstaff.Visible = false;
        txt_stafftype.Visible = false;
        pp3.Visible = false;
    }
    protected void stafftrue()
    {
        loadhostel();
        loadsession();
        lbl_department.Visible = true;
        txt_department.Visible = true;
        bindcbldept();
        pp0.Visible = true;
        lbl_design.Visible = true;
        txt_design.Visible = true;
        binddesig();
        pp2.Visible = true;
        lblstaff.Visible = true;
        txt_stafftype.Visible = true;
        bindstafftype();
        pp3.Visible = true;
    }
    protected void studentfalse()
    {
        lbl_batch.Visible = false;
        txt_batch.Visible = false;
        p2.Visible = false;
        lbl_degree.Visible = false;
        txt_degree.Visible = false;
        p3.Visible = false;
        lbl_branch.Visible = false;
        txt_branch.Visible = false;
        p6.Visible = false;
        lbl_section.Visible = false;
        txt_section.Visible = false;
        p4.Visible = false;
        lbl_sex.Visible = false;
        txt_sex.Visible = false;
        p11.Visible = false;
    }
    protected void studentrue()
    {
        loadhostel();
        loadsession();
        lbl_batch.Visible = true;
        txt_batch.Visible = true;
        p2.Visible = true;
        bindbatch();
        lbl_degree.Visible = true;
        txt_degree.Visible = true;
        p3.Visible = true;
        degree();
        lbl_branch.Visible = true;
        txt_branch.Visible = true;
        p6.Visible = true;
        bindbranch(college);
        lbl_section.Visible = true;
        txt_section.Visible = true;
        p4.Visible = true;
        bindsection();
        lbl_sex.Visible = true;
        txt_sex.Visible = true;
        p11.Visible = true;
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            string hostelcode = "";
            int sno = 0;
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    if (hostelcode == "")
                    {
                        hostelcode = "" + cbl_hostelname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        hostelcode = hostelcode + "'" + "," + "" + "'" + cbl_hostelname.Items[i].Value.ToString() + "";
                    }
                }
            }
            string sessioncode = "";
            for (int i = 0; i < cbl_sessionname.Items.Count; i++)
            {
                if (cbl_sessionname.Items[i].Selected == true)
                {
                    if (sessioncode == "")
                    {
                        sessioncode = "" + cbl_sessionname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        sessioncode = sessioncode + "'" + "," + "" + "'" + cbl_sessionname.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (rdb_student.Checked == true)
            {
                string batchyear = "";
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    if (cbl_batch.Items[i].Selected == true)
                    {
                        if (batchyear == "")
                        {
                            batchyear = "" + cbl_batch.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            batchyear = batchyear + "'" + "," + "" + "'" + cbl_batch.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string degreecode = "";
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    if (cbl_degree.Items[i].Selected == true)
                    {
                        if (degreecode == "")
                        {
                            degreecode = "" + cbl_degree.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            degreecode = degreecode + "'" + "," + "" + "'" + cbl_degree.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string section = "";
                if (cbl_section.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_section.Items.Count; i++)
                    {
                        if (cbl_section.Items[i].Selected == true)
                        {
                            if (section == "")
                            {
                                section = "" + cbl_section.Items[i].Value.ToString() + "";
                            }
                            else
                            {
                                section = section + "'" + "," + "" + "'" + cbl_section.Items[i].Value.ToString() + "";
                            }
                        }
                    }
                }
                string sex = "";
                for (int i = 0; i < cbl_sex.Items.Count; i++)
                {
                    if (cbl_sex.Items[i].Selected == true)
                    {
                        if (sex == "")
                        {
                            sex = "" + cbl_sex.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            sex = sex + "'" + "," + "" + "'" + cbl_sex.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string branch = "";
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    if (cbl_branch.Items[i].Selected == true)
                    {
                        if (branch == "")
                        {
                            branch = "" + cbl_branch.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            branch = branch + "'" + "," + "" + "'" + cbl_branch.Items[i].Value.ToString() + "";
                        }
                    }
                }
                if (hostelcode != "" && sessioncode != "" && batchyear != "" && degreecode != "" && section != "" && branch != "")
                {
                    //string q = "select  distinct r.Roll_Admit,r.Roll_No,r.Stud_Name,h.MessName,Sm.Session_Name, sm.Session_Code,CONVERT(varchar(10), da.Date,103) as Date,da.Time   from applyn a, Registration r,Degree d,Department dt,Course c ,DayScholourStaffAdd da,MessMaster h ,Session_Master sm where r.Roll_Admit =da.Roll_Admit and r.degree_code =d.Degree_Code  and da.Degree_code =r.degree_code and da.Degree_code =d.Degree_Code and d.Dept_Code  =dt.Dept_Code and d.Course_Id =c.Course_Id and d.college_code =da.College_code   and h.MessID  =da.Hostel_Code and sm.Session_Code =da.Session_code and sm.Hostel_Code =h.MessID  and da.Hostel_Code =sm.Hostel_Code and a.app_no =r.App_No    and a.degree_code =r.degree_code and a.degree_code =d.Degree_Code and a.degree_code =da.Degree_code and r.degree_code in ('" + branch + "')    and r.college_code ='" + collegecode1 + "' and h.MessID in ('" + hostelcode + "')    and sm.Session_Code in('" + sessioncode + "') and r.Batch_Year in ('" + batchyear + "')";
                    string q = "select  distinct r.Roll_Admit,r.Roll_No,r.Stud_Name,h.MessName,Sm.SessionName, sm.SessionMasterPK,CONVERT(varchar(10), da.Date,103) as Date,da.Time   from applyn a, Registration r,Degree d,Department dt,Course c ,DayScholourStaffAdd da,HM_MessMaster h ,HM_SessionMaster sm where r.Roll_No  =da.Roll_No  and r.degree_code =d.Degree_Code  and da.Degree_code =r.degree_code and da.Degree_code =d.Degree_Code and d.Dept_Code  =dt.Dept_Code and d.Course_Id =c.Course_Id and d.college_code =da.College_code   and h.MessMasterPK   =da.Hostel_Code and sm.SessionMasterPK  =da.Session_code and sm.MessMasterFK =h.MessMasterPK   and da.Hostel_Code =sm.MessMasterFK  and a.app_no =r.App_No    and a.degree_code =r.degree_code and a.degree_code =d.Degree_Code and a.degree_code =da.Degree_code and r.degree_code in ('" + branch + "')    and r.college_code ='" + collegecode1 + "' and h.MessMasterPK  in ('" + hostelcode + "')    and sm.SessionMasterPK in('" + sessioncode + "') and r.Batch_Year in ('" + batchyear + "')";
                    if (sex != "")
                    {
                        q = q + " and sex in ('" + sex + "' )";
                    }
                    if (section.Trim() != "")
                    {
                        q = q + "and r.Sections in ('" + section + "','')";
                    }
                    string strorderby = d2.GetFunction("select value from Master_Settings where settings='order_by'");
                    if (strorderby == "")
                    {
                        strorderby = "";
                    }
                    else
                    {
                        if (strorderby == "0")
                        {
                            strorderby = "ORDER BY r.Roll_No";
                        }
                        //else if (strorderby == "1")
                        //{
                        //    strorderby = "ORDER BY r.Reg_No";
                        //}
                        else if (strorderby == "2")
                        {
                            strorderby = "ORDER BY r.Stud_Name";
                        }
                        else if (strorderby == "0,2")
                        {
                            strorderby = "ORDER BY r.Roll_No,r.Stud_Name";
                        }
                        else
                        {
                            strorderby = "";
                        }
                        //else if (strorderby == "0,1")
                        //{
                        //    strorderby = "ORDER BY r.Roll_No,r.Reg_No";
                        //}
                        //else if (strorderby == "1,2")
                        //{
                        //    strorderby = "ORDER BY r.Reg_No,r.Stud_Name";
                        //}
                        //else if (strorderby == "0,2")
                        //{
                        //    strorderby = "ORDER BY r.Roll_No,r.Stud_Name";
                        //}
                    }
                    ArrayList addroll = new ArrayList();
                    string query = q + strorderby;
                    ds = d2.select_method_wo_parameter(query, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Fpspread1.Sheets[0].RowCount = 0;
                        Fpspread1.Sheets[0].ColumnCount = 0;
                        Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                        Fpspread1.CommandBar.Visible = false;
                        Fpspread1.Sheets[0].ColumnCount = 8;
                        Fpspread1.Sheets[0].AutoPostBack = false;
                        Fpspread1.Sheets[0].RowHeader.Visible = false;
                        Fpspread1.Width = 926;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Columns[0].Width = 50;
                        Fpspread1.Columns[0].Locked = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Columns[1].Width = 150;
                        Fpspread1.Columns[1].Locked = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Name";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        Fpspread1.Columns[2].Width = 200;
                        Fpspread1.Columns[2].Locked = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Mess Name";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        Fpspread1.Columns[3].Width = 150;
                        Fpspread1.Columns[3].Locked = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Session Name";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        Fpspread1.Columns[4].Width = 100;
                        Fpspread1.Columns[4].Locked = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Date";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        Fpspread1.Columns[5].Locked = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Time";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        Fpspread1.Columns[6].Locked = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Select";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                        Fpspread1.Columns[7].Width = 80;
                        Fpspread1.Columns[7].Locked = false;
                        FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                        chkall.AutoPostBack = true;
                        //Fpspread1.Columns[5].CellType = chkall;
                        FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
                        chk.AutoPostBack = false;
                        Fpspread1.Rows.Count = 1;
                        Fpspread1.Sheets[0].Cells[0, 7].CellType = chkall;
                        Fpspread1.Sheets[0].Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            sno++;
                            if (!addroll.Contains(Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"])))
                            {
                                addroll.Add(Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]));
                            }
                            //Fpspread1.Sheets[0].RowCount++;
                            //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                            Fpspread1.Sheets[0].RowCount = Fpspread1.Sheets[0].RowCount + 1;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_Admit"]);
                            //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["MessName"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["SessionName"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["Date"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["Time"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].CellType = chk;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        }
                        // div1.Visible = true;
                        Fpspread1.Visible = true;
                        rptprint.Visible = true;
                        lbl_error1.Visible = false;
                        lbl_stucnt.Visible = true;
                        lbl_stucnt.Text = "No of Student :" + addroll.Count.ToString();
                        Fpspread1.SaveChanges();
                        Fpspread1.Sheets[0].FrozenRowCount = 1;
                        // theivamani 29.10.15
                        Fpspread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        Fpspread1.Columns[2].VerticalAlign = VerticalAlign.Middle;
                        //theivamani 31.10.15
                        if (rollflag1 == "1")
                        {
                            Fpspread1.Columns[1].Visible = true;
                        }
                        else
                        {
                            Fpspread1.Columns[1].Visible = false;
                        }
                        Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    }
                    else
                    {
                        Fpspread1.Visible = false;
                        // div1.Visible = false;
                        lbl_error1.Visible = true;
                        lbl_stucnt.Visible = false;
                        lbl_error1.Text = "No Records Found";
                        rptprint.Visible = false;
                    }
                }
                //theivamani 30.10.15
                else
                {
                    Fpspread1.Visible = false;
                    rptprint.Visible = false;
                    lbl_error1.Visible = true;
                    lbl_stucnt.Visible = false;
                    lbl_error1.Text = "Please Select Any one Record";
                    rptprint.Visible = false;
                    //imgdiv2.Visible = true;
                    //lbl_alert.Visible = true;
                    //lbl_alert.Text = "Please Select Any one Item Name";
                }
            }
            if (rdb_staff.Checked == true)
            {
                string deptcode = "";
                for (int i = 0; i < cbl_department.Items.Count; i++)
                {
                    if (cbl_department.Items[i].Selected == true)
                    {
                        if (deptcode == "")
                        {
                            deptcode = "" + cbl_department.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            deptcode = deptcode + "'" + "," + "" + "'" + cbl_department.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string desigcode = "";
                for (int i = 0; i < cbl_design.Items.Count; i++)
                {
                    if (cbl_design.Items[i].Selected == true)
                    {
                        if (desigcode == "")
                        {
                            desigcode = "" + cbl_design.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            desigcode = desigcode + "'" + "," + "" + "'" + cbl_design.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string stafftype = "";
                if (cbl_section.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_stafftype.Items.Count; i++)
                    {
                        if (cbl_stafftype.Items[i].Selected == true)
                        {
                            if (stafftype == "")
                            {
                                stafftype = "" + cbl_stafftype.Items[i].Value.ToString() + "";
                            }
                            else
                            {
                                stafftype = stafftype + "'" + "," + "" + "'" + cbl_stafftype.Items[i].Value.ToString() + "";
                            }
                        }
                    }
                }
                if (hostelcode != "" && sessioncode != "" && deptcode.Trim() != "" && desigcode.Trim() != "" && stafftype.Trim() != "")
                {
                    //string selectquery = "select hd.MessID ,hd.MessName ,sm.Session_Code,sm.Session_Name,sm.Session_Code,ds.Staff_code ,ds.Staff_name,h.dept_name,h.dept_code,CONVERT(varchar(10), ds.Date,103) as Date,ds.Time   from DayScholourStaffAdd ds,staffmaster s,stafftrans st,hrdept_master h,desig_master d,MessMaster hd,Session_Master sm where ds.Staff_code =s.staff_code and s.staff_code =st.staff_code and ds.Staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and ds.Hostel_Code =hd.MessID  and sm.Hostel_Code =hd.MessID  and ds.Hostel_Code =sm.Hostel_Code and sm.Session_Code =ds.Session_code and hd.MessID  in('" + hostelcode + "') and sm.Session_Code in ('" + sessioncode + "') and d.desig_code in ('" + desigcode + "') and st.StfType in('" + stafftype + "') and s.resign =0 and s.settled =0 order by ds.Staff_code";
                    string selectquery = " select hd.MessMasterPK ,hd.MessName ,sm.SessionName,sm.SessionMasterPK, ds.Staff_code ,ds.Staff_name,h.dept_name,h.dept_code,CONVERT(varchar(10), ds.Date,103) as Date,ds.Time   from DayScholourStaffAdd ds,staffmaster s,stafftrans st,hrdept_master h,desig_master d,HM_MessMaster hd,HM_SessionMaster sm where ds.Staff_code =s.staff_code and s.staff_code =st.staff_code and ds.Staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and ds.Hostel_Code =hd.MessMasterPK  and sm.MessMasterFK =hd.MessMasterPK  and ds.Hostel_Code =sm.MessMasterFK  and sm.SessionMasterPK  =ds.Session_code and hd.MessMasterPK  in('" + hostelcode + "') and sm.SessionMasterPK  in ('" + sessioncode + "') and d.desig_code in ('" + desigcode + "') and st.StfType in('" + stafftype + "') and h.dept_code in('" + deptcode + "') and s.resign =0 and s.settled =0 order by ds.Staff_code";
                    ArrayList addstaff = new ArrayList();
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Fpspread1.Sheets[0].ColumnCount = 0;
                        Fpspread1.Sheets[0].RowCount = 0;
                        Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                        Fpspread1.CommandBar.Visible = false;
                        Fpspread1.Sheets[0].ColumnCount = 9;
                        Fpspread1.Sheets[0].AutoPostBack = false;
                        Fpspread1.Sheets[0].RowHeader.Visible = false;
                        Fpspread1.Width = 940;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Columns[0].Width = 50;
                        Fpspread1.Columns[0].Locked = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Columns[1].Width = 150;
                        Fpspread1.Columns[1].Locked = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        Fpspread1.Columns[2].Width = 150;
                        Fpspread1.Columns[2].Locked = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department Name";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        Fpspread1.Columns[3].Width = 150;
                        Fpspread1.Columns[3].Locked = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Mess Name";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        Fpspread1.Columns[4].Width = 150;
                        Fpspread1.Columns[4].Locked = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Session Name";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        Fpspread1.Columns[5].Width = 100;
                        Fpspread1.Columns[5].Locked = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Date";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        Fpspread1.Columns[6].Width = 100;
                        Fpspread1.Columns[6].Locked = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Time";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                        Fpspread1.Columns[7].Width = 100;
                        Fpspread1.Columns[7].Locked = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Select";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                        Fpspread1.Columns[8].Width = 80;
                        Fpspread1.Columns[8].Locked = false;
                        FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                        chkall.AutoPostBack = true;
                        //Fpspread1.Columns[5].CellType = chkall;
                        FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
                        chk.AutoPostBack = false;
                        Fpspread1.Rows.Count = 1;
                        Fpspread1.Sheets[0].Cells[0, 8].CellType = chkall;
                        Fpspread1.Sheets[0].Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            //Fpspread1.Sheets[0].RowCount++;
                            // Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                            sno++;
                            if (!addstaff.Contains(Convert.ToString(ds.Tables[0].Rows[row]["Staff_code"])))
                            {
                                addstaff.Add(Convert.ToString(ds.Tables[0].Rows[row]["Staff_code"]));
                            }
                            Fpspread1.Sheets[0].RowCount = Fpspread1.Sheets[0].RowCount + 1;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["Staff_code"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["Staff_name"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["dept_name"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["MessName"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["SessionName"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[row]["SessionMasterPK"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["Date"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[row]["Time"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].CellType = chk;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        }
                        Fpspread1.Visible = true;
                        rptprint.Visible = true;
                        lbl_error1.Visible = false;
                        lbl_staffcnt.Visible = true;
                        lbl_staffcnt.Text = "No of Staff :" + addstaff.Count.ToString();
                        Fpspread1.SaveChanges();
                        Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                        Fpspread1.Sheets[0].FrozenRowCount = 1;
                        Fpspread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        Fpspread1.Columns[2].VerticalAlign = VerticalAlign.Middle;
                        //Fpspread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        //Fpspread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        //Fpspread1.Sheets[0].
                    }
                    else
                    {
                        Fpspread1.Visible = false;
                        lbl_error1.Visible = true;
                        lbl_staffcnt.Visible = false;
                        lbl_error1.Text = "No Records Found";
                        rptprint.Visible = false;
                    }
                }
                else
                {
                    Fpspread1.Visible = false;
                    rptprint.Visible = false;
                    lbl_error1.Visible = true;
                    lbl_staffcnt.Visible = false;
                    lbl_error1.Text = "Please Select Any one Record";
                    rptprint.Visible = false;
                }
            }
        }
        catch
        {
        }
    }
    //09.10.15
    protected void Fpspread_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = Fpspread1.Sheets[0].ActiveRow.ToString();
            string actcol = Fpspread1.Sheets[0].ActiveColumn.ToString();
            if (rdb_staff.Checked == true)
            {
                if (actrow.Trim() == "0" && actcol.Trim() == "8")
                {
                    if (Fpspread1.Sheets[0].RowCount > 0)
                    {
                        int checkval = Convert.ToInt32(Fpspread1.Sheets[0].Cells[0, 8].Value);
                        if (checkval == 0)
                        {
                            for (int i = 1; i < Fpspread1.Sheets[0].RowCount; i++)
                            {
                                Fpspread1.Sheets[0].Cells[i, 8].Value = 1;
                            }
                        }
                        if (checkval == 1)
                        {
                            for (int i = 1; i < Fpspread1.Sheets[0].RowCount; i++)
                            {
                                Fpspread1.Sheets[0].Cells[i, 8].Value = 0;
                            }
                        }
                    }
                }
            }
            else
            {
                if (actrow.Trim() == "0" && actcol.Trim() == "7")
                {
                    if (Fpspread1.Sheets[0].RowCount > 0)
                    {
                        int checkval = Convert.ToInt32(Fpspread1.Sheets[0].Cells[0, 7].Value);
                        if (checkval == 0)
                        {
                            for (int i = 1; i < Fpspread1.Sheets[0].RowCount; i++)
                            {
                                Fpspread1.Sheets[0].Cells[i, 7].Value = 1;
                            }
                        }
                        if (checkval == 1)
                        {
                            for (int i = 1; i < Fpspread1.Sheets[0].RowCount; i++)
                            {
                                Fpspread1.Sheets[0].Cells[i, 7].Value = 0;
                            }
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    //protected void btn_delete_click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        bool delet = false;
    //        int del = 0;
    //        Fpspread1.SaveChanges();
    //        if (rdb_staff.Checked == true)
    //        {
    //            for (int i = 0; i < Fpspread1.Sheets[0].Rows.Count; i++)
    //            {
    //                int checkval = Convert.ToInt32(Fpspread1.Sheets[0].Cells[i, 6].Value);
    //                if (checkval != 0)
    //                {
    //                    string staffcode = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 1].Value);
    //                    string q1 = "delete DayScholourStaffAdd where Staff_code='" + staffcode + "'";
    //                    del = d2.update_method_wo_parameter(q1, "Text");
    //                    if (del != 0)
    //                    {
    //                        delet = true;
    //                    }
    //                }
    //            }
    //            if (delet == true)
    //            {
    //                alertpopwindow.Visible = true;
    //                lblalerterr.Text = "Deleted Successfully";
    //                lblalerterr.Visible = true;
    //            }
    //        }
    //        if (rdb_student.Checked == true)
    //        {
    //            for (int i = 0; i < Fpspread1.Sheets[0].Rows.Count; i++)
    //            {
    //                int checkval = Convert.ToInt32(Fpspread1.Sheets[0].Cells[i, 5].Value);
    //                if (checkval != 0)
    //                {
    //                    string rollno = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 1].Value);
    //                    string q1 = "delete DayScholourStaffAdd where Roll_No='" + rollno + "'";
    //                    del = d2.update_method_wo_parameter(q1, "Text");
    //                    if (del != 0)
    //                    {
    //                        delet = true;
    //                    }
    //                }
    //            }
    //            if (delet == true)
    //            {
    //                alertpopwindow.Visible = true;
    //                lblalerterr.Text = "Deleted Successfully";
    //                lblalerterr.Visible = true;
    //            }
    //        }
    //        btn_go_Click(sender, e);
    //    }
    //    catch
    //    { }
    //}
    //protected void btn_addnew_Click(object sender, EventArgs e)
    //{
    //    loadsessionnew();
    //    bindhostelname1();
    //    bindstaffsession();
    //    cb_session2.Checked = true;
    //    //txt_rolladmit.Text = "";
    //    txt_rollno.Text = "";
    //    txt_name.Text = "";
    //    txt_degree1.Text = "";
    //    txt_staffname.Text = "";
    //    txt_department1.Text = "";
    //    if (rdb_staff.Checked == true)
    //    {
    //        popstaff.Visible = true;
    //    }
    //    else
    //    {
    //        popupstudaddinl.Visible = true;
    //    }
    //}
    protected void btn_Excel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txt_excelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, reportname);
                lbl_validation1.Visible = false;
            }
            else
            {
                lbl_validation1.Text = "Please Enter Your Report Name";
                lbl_validation1.Visible = true;
                txt_excelname.Focus();
            }
        }
        catch
        {
        }
    }
    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "DayScholar Student / Staff Registration Report";
            string pagename = "days_scholour.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }
    }
    //staff save = popstaff
    protected void bindstaffsession()
    {
        try
        {
            ds.Clear();
            cbl_session2.Items.Clear();
            string itemheader = Convert.ToString(ddl_hostelname2.SelectedItem.Value);
            if (itemheader.Trim() != "")
            {
                //string selecthostel = "select distinct Session_Code,Session_Name  from Session_Master where Hostel_Code in ('" + itemheader + "')";
                //ds = d2.select_method_wo_parameter(selecthostel, "Text");
                ds = d2.BindSession_inv(itemheader);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_session2.DataSource = ds;
                    cbl_session2.DataTextField = "SessionName";
                    cbl_session2.DataValueField = "SessionMasterPK";
                    cbl_session2.DataBind();
                    if (cbl_session2.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_session2.Items.Count; row++)
                        {
                            cbl_session2.Items[row].Selected = true;
                        }
                        txt_session2.Text = "Session Name(" + cbl_session2.Items.Count + ")";
                    }
                }
                else { txt_session2.Text = "--Select--"; }
            }
        }
        catch
        {
        }
    }
    protected void cb_session2_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_session2.Checked == true)
            {
                for (int i = 0; i < cbl_session2.Items.Count; i++)
                {
                    cbl_session2.Items[i].Selected = true;
                }
                txt_session2.Text = "Sesssion Name(" + (cbl_session2.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_session2.Items.Count; i++)
                {
                    cbl_session2.Items[i].Selected = false;
                }
                txt_session2.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_session2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_session2.Text = "--Select--";
            cb_session2.Checked = false;
            int ccount = 0;
            for (int i = 0; i < cbl_session2.Items.Count; i++)
            {
                if (cbl_session2.Items[i].Selected == true)
                {
                    ccount = ccount + 1;
                    cb_session2.Checked = false;
                }
            }
            if (ccount > 0)
            {
                txt_session2.Text = "Session Name(" + ccount.ToString() + ")";
                if (ccount == cbl_session2.Items.Count)
                {
                    cb_session2.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_save2staff_Click(object sender, EventArgs e)
    {
        try
        {
            bool nicecheck = false;
            string hostelcode = Convert.ToString(ddl_hostelname2.SelectedItem.Value);
            string Staffname = Convert.ToString(txt_staffname.Text);
            string Deptname = Convert.ToString(txt_department1.Text);
            //string Staffcode = Convert.ToString(Session["Staff_codeNew"]);
            date = Convert.ToString(txt_stf_date.Text);
            string[] splitdate = date.Split('-');
            splitdate = splitdate[0].Split('/');
            DateTime dt = new DateTime();
            //magesh 12.3.18
            string studmesstype = string.Empty;
            int messtype = 0;
            int.TryParse(Convert.ToString(ddlStudType.SelectedValue), out messtype);
            studmesstype = Convert.ToString(messtype - 1);//magesh 12.3.18
            if (splitdate.Length > 0)
            {
                dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            }
            string getday = dt.ToString("MM/dd/yyyy");
            string hr = Convert.ToString(ddl_stfhr.SelectedItem.Text);
            string min = Convert.ToString(ddl_stfm.SelectedItem.Text);
            string day = Convert.ToString(ddl_stfam.SelectedItem.Text);
            string time = hr + ":" + min + ":" + day;
            string staffc1 = Convert.ToString(Session["staffc"]);
            if (staffc1.Trim() == "")
            {
                if (txt_staffname.Text.Trim() != "")
                {
                    staffc1 = d2.GetFunction("select staff_code from staffmaster where staff_name ='" + txt_staffname.Text.Trim() + "' ");
                }
            }
            if (staffc1.Trim() != "")
            {
                if (cbl_session2.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_session2.Items.Count; row++)
                    {
                        if (cbl_session2.Items[row].Selected == true)
                        {
                            char[] delimiterChars = { ',' };
                            string[] hostel1 = hostelcode.Split(delimiterChars);
                            string[] sname = Staffname.Split(delimiterChars);
                            string[] dname = Deptname.Split(delimiterChars);
                            string[] scode = staffc1.Split(delimiterChars);
                            string sname1 = "";
                            string scode1 = "";
                            foreach (string s in scode)
                            {
                                scode1 = (s);
                                string q1 = "select s.staff_code,s.staff_name from staffmaster s,stafftrans st,hrdept_master h ,desig_master d where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and s.staff_code='" + scode1 + "'";
                                ds = d2.select_method_wo_parameter(q1, "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    sname1 = Convert.ToString(ds.Tables[0].Rows[0][1].ToString());
                                    scode1 = Convert.ToString(ds.Tables[0].Rows[0][0].ToString());
                                    //dcode1 = Convert.ToString(ds.Tables[0].Rows[0][2].ToString());
                                    string insertquery = " if exists ( select * from DayScholourStaffAdd where Staff_code ='" + scode1 + "' and hostel_code='" + hostelcode + "' and Session_code='" + cbl_session2.Items[row].Value + "') update DayScholourStaffAdd set Date ='" + dt.ToString("MM/dd/yyyy") + "' ,Time ='" + time + "' , Staff_name ='" + sname1 + "' ,College_code ='" + ddl_college.SelectedItem.Value + "',StudMessType='" + studmesstype + "',Hostel_id='" + txtid1.Text + "' where Staff_code ='" + scode1 + "' and hostel_code='" + hostelcode + "' and Session_code='" + cbl_session2.Items[row].Value + "' else insert into DayScholourStaffAdd(Date,Time,Staff_name,Typ,Hostel_Code,Session_code,College_code,Staff_code,StudMessType,Hostel_id) values ('" + dt.ToString("MM/dd/yyyy") + "','" + time + "','" + sname1 + "','" + 2 + "','" + hostelcode + "','" + cbl_session2.Items[row].Value + "','" + ddl_college.SelectedItem.Value + "','" + scode1 + "','" + studmesstype + "','" + txtid1.Text + "')";
                                    int inst = d2.update_method_wo_parameter(insertquery, "Text");
                                    nicecheck = true;
                                }
                            }
                        }
                    }
                    if (nicecheck == true)
                    {
                        //popstaff.Visible = false;
                        alertpopwindow.Visible = true;
                        Session["staffc"] = null;
                        lblalerterr.Text = "Saved Successfully";
                        lblalerterr.Visible = true;
                        txt_staffname.Text = "";
                        txt_department1.Text = "";
                        bindhostelnamestaff();
                        bindstaffsession();
                        txt_stf_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        timevalue();
                    }
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Create Session Name";
                    lblalerterr.Visible = true;
                }
            }
        }
        catch
        {
        }
    }
    //protected void btn_exit3staff_Click(object sender, EventArgs e)
    //{
    //    popstaff.Visible = false;
    //}
    //protected void imagebtnpopclose3_Click(object sender, EventArgs e)
    //{
    //    popstaff.Visible = false;
    //}
    //popup staff selection spread = popupsscode1
    protected void ddl_search_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_Search.SelectedValue == "0")
        {
            txt_Search.Visible = true;
            txt_wardencode.Visible = false;
            txt_wardencode.Text = "";
        }
        else if (ddl_Search.SelectedValue == "1")
        {
            txt_Search.Visible = false;
            txt_Search.Text = "";
            txt_wardencode.Visible = true;
        }
        deptcod = Convert.ToString(ddl_department2.SelectedItem.Value);
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select s.staff_name from staffmaster s, stafftrans st,hrdept_master h ,desig_master d where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code  and h.dept_code in ('" + deptcod + "') and s.staff_code not in (select staff_code  from DayScholourStaffAdd where ISNULL(staff_name,'') <>'') and s.staff_name like '" + prefixText + "%'";
        // string query = "select staff_name  from staffmaster,hrdept_master h where resign =0 and settled =0 and staff_name like  '" + prefixText + "%' and h.dept_code='"+code+"'";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getstaffcode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_code,staff_name from staffmaster where resign =0 and settled =0 and staff_code not in (select staff_code  from DayScholourStaffAdd where ISNULL(staff_code,'') <>'' ) and staff_code like  '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }
    //theivamani 29.10.15
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffNameadd(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select s.staff_name from staffmaster s, stafftrans st,hrdept_master h ,desig_master d where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and s.staff_code not in (select staff_code  from DayScholourStaffAdd where ISNULL(staff_name,'') <>'')   and s.staff_name like '" + prefixText + "%'";
        // string query = "select staff_name  from staffmaster,hrdept_master h where resign =0 and settled =0 and staff_name like  '" + prefixText + "%' and h.dept_code='"+code+"'";
        name = ws.Getname(query);
        return name;
    }
    protected void btn_staff_Click(object sender, EventArgs e)
    {
        popupsscode1.Visible = true;
        binddepartment();
        btn_searchbygo_Click(sender, e);
        bindcollege();
    }
    protected void binddepartment()
    {
        ds.Clear();
        string query = "";
        //  query = "select distinct dept_name,dept_code from hrdept_master where college_code='" + ddl_collegename1.SelectedValue.ToString() + "'";
        ds = d2.loaddepartment(ddl_college.SelectedItem.Value);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_department2.DataSource = ds;
            ddl_department2.DataTextField = "Dept_Name";
            ddl_department2.DataValueField = "Dept_Code";
            ddl_department2.DataBind();
            //ddl_department2.Items.Insert(0, "All");
        }
    }
    public void ddl_department2_SelectedIndexChanged(object sender, EventArgs e)
    {
        staff_name();
    }
    public void staff_name()
    {
        code = d2.GetFunction("select dept_code  from hrdept_master where dept_code ='" + ddl_department2.SelectedItem.Value + "'");
    }
    protected void btn_searchbygo_Click(object sender, EventArgs e)
    {
        try
        {
            int rolcount = 0;
            int sno = 0;
            deptcod = Convert.ToString(ddl_department2.SelectedItem.Value);
            if (txt_Search.Text != "")
            {
                if (ddl_Search.SelectedIndex == 0)
                {
                    sql = "select s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and s.Staff_name ='" + Convert.ToString(txt_Search.Text) + "' and s.staff_code not in (select staff_code  from DayScholourStaffAdd where ISNULL(staff_name,'') <>'')  and s.college_code='" + collegecode1 + "' order by s.staff_code";
                }
            }
            else if (txt_wardencode.Text.Trim() != "")
            {
                if (ddl_Search.SelectedIndex == 1)
                {
                    sql = "select s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and s.staff_code ='" + Convert.ToString(txt_wardencode.Text) + "' and staff_code not in (select staff_code  from DayScholourStaffAdd where ISNULL(staff_code,'') <>'' ) and s.college_code='" + collegecode1 + "' order by s.staff_code";
                }
            }
            else
            {
                sql = "select s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and h.dept_code in ('" + ddl_department2.SelectedItem.Value + "')and s.staff_code not in (select staff_code  from DayScholourStaffAdd where ISNULL(staff_name,'') <>'') and s.college_code='" + collegecode1 + "' order by s.staff_code";
            }
            Fpstaff.Sheets[0].RowCount = 0;
            Fpstaff.SaveChanges();
            Fpstaff.SheetCorner.ColumnCount = 0;
            Fpstaff.CommandBar.Visible = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            Fpstaff.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
            Fpstaff.Sheets[0].RowCount = Fpstaff.Sheets[0].RowCount + 1;
            Fpstaff.Sheets[0].SpanModel.Add(Fpstaff.Sheets[0].RowCount - 1, 0, 1, 3);
            Fpstaff.Sheets[0].AutoPostBack = false;
            ds = d2.select_method_wo_parameter(sql, "Text");
            Fpstaff.Sheets[0].RowCount = 0;
            Fpstaff.Sheets[0].ColumnCount = 6;
            if (ds.Tables[0].Rows.Count > 0)
            {
                Fpstaff.Visible = true;
                btn_save4.Visible = true;
                btn_exit4.Visible = true;
                Fpstaff.Sheets[0].RowCount = 1;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[0].Locked = true;
                Fpstaff.Columns[0].Width = 80;
                FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                chkall.AutoPostBack = true;
                Fpstaff.Sheets[0].Cells[0, 1].CellType = chkall;
                Fpstaff.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                //Fpstaff.Sheets[0].Columns[1].CellType = cb;
                Fpstaff.Columns[1].Width = 80;
                Fpstaff.Sheets[0].Columns[2].Locked = false;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Code";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[2].Locked = true;
                Fpstaff.Columns[2].Width = 100;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff Name";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[3].Locked = true;
                Fpstaff.Columns[3].Width = 200;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Department";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[4].Locked = true;
                Fpstaff.Columns[4].Width = 250;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Designation";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Columns[5].Width = 200;
                Fpstaff.Sheets[0].Columns[5].Locked = true;
                Fpstaff.Width = 700;
                FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                for (rolcount = 0; rolcount < ds.Tables[0].Rows.Count; rolcount++)
                {
                    sno++;
                    //Fpstaff.Sheets[0].RowCount++;
                    //name = ds.Tables[0].Rows[rolcount]["staff_name"].ToString();
                    //code = ds.Tables[0].Rows[rolcount]["staff_code"].ToString();
                    Fpstaff.Sheets[0].RowCount = Fpstaff.Sheets[0].RowCount + 1;
                    //Fpstaff.Sheets[0].Rows[Fpstaff.Sheets[0].RowCount - 1].Font.Bold = false;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    //Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["select"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].CellType = cb1;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpstaff.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                    cb.AutoPostBack = false;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["staff_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["staff_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["dept_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["dept_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["desig_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["desig_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                }
                error.Visible = false;
                lbl_errorsearch.Visible = true;
                lbl_errorsearch.Text = "No Records Found";
                lbl_errorsearch.Text = "No of Staff :" + sno.ToString();
                rowcount = Fpstaff.Sheets[0].RowCount;
                Fpstaff.Height = 370;
                Fpstaff.Sheets[0].PageSize = 25 + (rowcount * 20);
                Fpstaff.SaveChanges();
            }
            else
            {
                error.Visible = true;
                error.Text = "No Records Found";
                lbl_errorsearch.Visible = false;
                Fpstaff.Visible = false;
                btn_save4.Visible = false;
                btn_exit4.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void Fpspread1_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = Fpstaff.Sheets[0].ActiveRow.ToString();
            string actcol = Fpstaff.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (Fpstaff.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(Fpstaff.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < Fpstaff.Sheets[0].RowCount; i++)
                        {
                            Fpstaff.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < Fpstaff.Sheets[0].RowCount; i++)
                        {
                            Fpstaff.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void Fpspread2_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = Fpspread2.Sheets[0].ActiveRow.ToString();
            string actcol = Fpspread2.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (Fpspread2.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(Fpspread2.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < Fpspread2.Sheets[0].RowCount; i++)
                        {
                            Fpspread2.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < Fpspread2.Sheets[0].RowCount; i++)
                        {
                            Fpspread2.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void btn_save4_Click(object sender, EventArgs e)
    {
        try
        {
            if (Fpstaff.Sheets[0].RowCount > 0)
            {
                Fpstaff.SaveChanges();
                string name1 = "";
                string degreecode1 = "";
                string staffc = "";
                for (int i = 1; i < Fpstaff.Sheets[0].RowCount; i++)
                {
                    int checkval = Convert.ToInt32(Fpstaff.Sheets[0].Cells[i, 1].Value);
                    if (checkval == 1)
                    {
                        string name = Fpstaff.Sheets[0].Cells[Convert.ToInt32(i), 3].Text;
                        string degreecode = Fpstaff.Sheets[0].Cells[Convert.ToInt32(i), 4].Text;
                        // string deptcode = Convert.ToString(Fpstaff.Sheets[0].Cells[Convert.ToInt32(i), 4].Tag);
                        string staffcode = Fpstaff.Sheets[0].Cells[Convert.ToInt32(i), 2].Text;
                        if (name1 == "")
                        {
                            name1 = Convert.ToString(name);
                        }
                        else
                        {
                            name1 = name1 + "," + Convert.ToString(name) + "";
                        }
                        if (degreecode1 == "")
                        {
                            degreecode1 = Convert.ToString(degreecode);
                        }
                        else
                        {
                            degreecode1 = degreecode1 + "," + Convert.ToString(degreecode) + "";
                        }
                        if (staffc == "")
                        {
                            staffc = Convert.ToString(staffcode);
                        }
                        else
                        {
                            staffc = staffc + "," + Convert.ToString(staffcode) + "";
                        }
                        //Session["Staff_codeNew"] = Convert.ToString(Fpstaff.Sheets[0].Cells[Convert.ToInt32(i), 2].Text);
                    }
                    else
                    {
                        lbl_errorsearch.Visible = true;
                        lbl_errorsearch.Text = "Please Select Any One Staff";
                    }
                }
                txt_staffname.Text = name1;
                txt_department1.Text = degreecode1;
                Session["staffc"] = Convert.ToString(staffc);
                popupsscode1.Visible = false;
            }
            else
            {
                lbl_errorstaff.Visible = true;
                lbl_errorstaff.Text = "No Records Found";
                Fpstaff.Visible = false;
            }
            //    string activerow = "";
            //    string activecol = "";
            //    if (Fpstaff.Sheets[0].RowCount != 0)
            //    {
            //        activerow = Fpstaff.ActiveSheetView.ActiveRow.ToString();
            //        activecol = Fpstaff.ActiveSheetView.ActiveColumn.ToString();
            //        if (activerow != Convert.ToString(-1))
            //        {
            //            if (txt_Search.Text == "" || txt_Search.Text != "")
            //            {
            //                string name = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
            //                txt_staffname.Text = name;
            //                //string dept = Convert.ToString(Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
            //                string dept = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
            //                txt_department1.Text = dept;
            //                Session["Staff_codeNew"] = Convert.ToString(Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
            //            }
            //            popupsscode1.Visible = false;
            //        }
            //        else
            //        {
            //            lbl_errorsearch.Visible = true;
            //            lbl_errorsearch.Text = "Please Select Any One Staff";
            //        }
            //    }
            //    else
            //    {
            //        lbl_errorstaff.Visible = true;
            //        lbl_errorstaff.Text = "No Records Found";
            //        Fpstaff.Visible = false;
            //    }
            //}
            //catch (Exception ex)
            //{
            //}
        }
        catch
        {
        }
    }
    protected void btn_exit4_Click(object sender, EventArgs e)
    {
        try
        {
            popupsscode1.Visible = false;
        }
        catch
        {
        }
    }
    protected void imagebtnpopclose4_Click(object sender, EventArgs e)
    {
        popupsscode1.Visible = false;
    }
    //student save = popupstudaddinl
    public void loadsessionnew()
    {
        try
        {
            ds.Clear();
            cbl_sessionname1.Items.Clear();
            string itemheader = Convert.ToString(ddl_hostelname1.SelectedItem.Value);
            if (itemheader.Trim() != "")
            {
                //string selecthostel = "select distinct Session_Code,Session_Name  from Session_Master where Hostel_Code in ('" + itemheader + "')";
                //ds = d2.select_method_wo_parameter(selecthostel, "Text");
                ds = d2.BindSession_inv(itemheader);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_sessionname1.DataSource = ds;
                    cbl_sessionname1.DataTextField = "SessionName";
                    cbl_sessionname1.DataValueField = "SessionMasterPK";
                    cbl_sessionname1.DataBind();
                    if (cbl_sessionname1.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_sessionname1.Items.Count; row++)
                        {
                            cbl_sessionname1.Items[row].Selected = true;
                        }
                        txt_sessionname1.Text = "Session Name(" + cbl_sessionname1.Items.Count + ")";
                    }
                }
                else
                {
                    txt_sessionname1.Text = "--Select--";
                }
            }
        }
        catch
        {
        }
    }
    protected void cb_sessionname1_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_sessionname1.Checked == true)
        {
            for (int i = 0; i < cbl_sessionname1.Items.Count; i++)
            {
                cbl_sessionname1.Items[i].Selected = true;
            }
            txt_sessionname1.Text = "Session Name(" + (cbl_sessionname1.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_sessionname1.Items.Count; i++)
            {
                cbl_sessionname1.Items[i].Selected = false;
            }
            txt_sessionname1.Text = "--Select--";
        }
    }
    protected void cbl_sessionname1_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_sessionname1.Text = "--Select--";
        cb_sessionname1.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cbl_sessionname1.Items.Count; i++)
        {
            if (cbl_sessionname1.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txt_sessionname1.Text = "Session Name(" + commcount.ToString() + ")";
            if (commcount == cbl_sessionname1.Items.Count)
            {
                cb_sessionname1.Checked = true;
            }
        }
    }
    protected void btnroladmit_click(object sender, EventArgs e)
    {
        try
        {
            popupselectstd.Visible = true;
            bindbatch1();
            binddegree2();
            bindbranch1(college);
            txt_rollno1.Text = "";
            //Fpspread2.Visible = false;
            //btn_ok.Visible = false;
            //btn_exit2.Visible = false;
            //lbl_errormsg.Visible = false;
            btn_go1_Click(sender, e);
        }
        catch
        {
        }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            bool nicecheck1 = false;
            string hostelcode = Convert.ToString(ddl_hostelname1.SelectedItem.Value);
            // string rolladmit = Convert.ToString(txt_rolladmit.Text);
            string Rollno = Convert.ToString(txt_rollno.Text);
            string Name = Convert.ToString(txt_name.Text);
            string degreecode = Convert.ToString(Session["degreecodenew"]);
            date = Convert.ToString(txt_date.Text);
            string[] splitdate = date.Split('-');
            splitdate = splitdate[0].Split('/');
            DateTime dt = new DateTime();
            //magesh 12.3.18
            string studmesstype = string.Empty;
            int messtype = 0;
            int.TryParse(Convert.ToString(ddlStudType.SelectedValue), out messtype);
            studmesstype = Convert.ToString(messtype - 1);//magesh 12.3.18
            if (splitdate.Length > 0)
            {
                dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            }
            string getday = dt.ToString("MM/dd/yyyy");
            string hr = Convert.ToString(ddl_hour.SelectedItem.Text);
            string min = Convert.ToString(ddl_minits.SelectedItem.Text);
            string day = Convert.ToString(ddl_timeformate.SelectedItem.Text);
            string time = hr + ":" + min + ":" + day;
            if (cbl_sessionname1.Items.Count > 0)
            {
                for (int row = 0; row < cbl_sessionname1.Items.Count; row++)
                {
                    if (cbl_sessionname1.Items[row].Selected == true)
                    {
                        string rollad = "";
                        string rolln = "";
                        string nam = "";
                        string degree = "";
                        char[] delimiterChars = { ',' };
                        string[] rollno1 = Rollno.Split(delimiterChars);
                        //foreach (string rno in rollno1)
                        //{
                        foreach (string r in rollno1)
                        {
                            rollad = (r);
                            rolln = (r);
                            string q1 = "select Roll_Admit,Stud_Name,degree_code,college_code from Registration where Roll_No='" + rolln + "'";
                            ds = d2.select_method_wo_parameter(q1, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                degree = Convert.ToString(ds.Tables[0].Rows[0][2].ToString());
                                nam = Convert.ToString(ds.Tables[0].Rows[0][1].ToString());
                                string radmit = Convert.ToString(ds.Tables[0].Rows[0][0].ToString());
                                string insertquery = "if exists (select * from DayScholourStaffAdd where Roll_Admit ='" + radmit + "' and hostel_code='" + hostelcode + "' and Session_code='" + cbl_sessionname1.Items[row].Value + "' ) update DayScholourStaffAdd set Date ='" + dt.ToString("MM/dd/yyyy") + "' ,Time ='" + time + "' ,Roll_No ='" + rolln + "' ,Stud_Name ='" + nam + "' ,College_code ='" + ddl_college.SelectedItem.Value + "',Degree_code ='" + degree + "',StudMessType='" + studmesstype + "',Hostel_id='" + txtid.Text + "' where Roll_Admit ='" + radmit + "' and hostel_code='" + hostelcode + "' and Session_code='" + cbl_sessionname1.Items[row].Value + "' else insert into DayScholourStaffAdd(Date,Time,Roll_Admit,Roll_No,Stud_Name,Typ,Hostel_Code,Session_code,College_code,Degree_code,StudMessType,Hostel_id) values ('" + dt.ToString("MM/dd/yyyy") + "','" + time + "','" + radmit + "','" + rolln + "','" + nam + "','1','" + hostelcode + "','" + cbl_sessionname1.Items[row].Value + "','" + ddl_college.SelectedItem.Value + "','" + degree + "','" + studmesstype + "','" + txtid.Text + "')";
                                int inst = d2.update_method_wo_parameter(insertquery, "Text");
                                nicecheck1 = true;
                            }
                        }
                        //}
                        // string insertquery = "  if exists (select * from DayScholourStaffAdd where Roll_Admit ='" + rollad + "' and hostel_code='" + hostel + "' and Session_code='" + cbl_sessionname1.Items[row].Value + "' ) update DayScholourStaffAdd set Roll_No ='" + rolln + "' ,Stud_Name ='" + nam + "' ,College_code ='" + ddl_college.SelectedItem.Value + "',Degree_code ='" + degree + "' where Roll_Admit ='" + rollad + "' and hostel_code='" + hostel + "' and Session_code='" + cbl_sessionname1.Items[row].Value + "' else insert into DayScholourStaffAdd(Roll_Admit,Roll_No,Stud_Name,Typ,Hostel_Code,Session_code,College_code,Degree_code) values ('" + rollad + "','" + rolln + "','" + nam + "','1','" + hostel + "','" + cbl_sessionname1.Items[row].Value + "'," + ddl_college.SelectedItem.Value + "','" + degree + "')";
                        // string insertquery = "  if exists (select * from DayScholourStaffAdd where Roll_Admit ='" + rolladmit + "' and hostel_code='" + hostelcode + "' and Session_code='" + cbl_sessionname1.Items[row].Value + "' ) update DayScholourStaffAdd set Roll_No ='" + Rollno + "' ,Stud_Name ='" + Name + "' ,College_code ='" + ddl_college.SelectedItem.Value + "',Degree_code ='" + degreecode + "' where Roll_Admit ='" + rolladmit + "' and hostel_code='" + hostelcode + "' and Session_code='" + cbl_sessionname1.Items[row].Value + "' else insert into DayScholourStaffAdd(Roll_Admit,Roll_No,Stud_Name,Typ,Hostel_Code,Session_code,College_code,Degree_code) values ('" + rolladmit + "','" + Rollno + "','" + Name + "','1','" + hostelcode + "','" + cbl_sessionname1.Items[row].Value + "','" + ddl_college.SelectedItem.Value + "','" + degreecode + "')";
                        //int inst = d2.update_method_wo_parameter(insertquery, "Text");
                        //nicecheck = true;
                    }
                }
                if (nicecheck1 == true)
                {
                    //popupstudaddinl.Visible = false;
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Saved Successfully";
                    lblalerterr.Visible = true;
                    txt_rollno.Text = "";
                    txt_name.Text = "";
                    txt_degree1.Text = "";
                    bindhostelname1();
                    loadsessionnew();
                    txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    timevalue();
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Create Session Name";
                lblalerterr.Visible = true;
            }
        }
        catch
        {
        }
    }
    //protected void btn_exit1_Click(object sender, EventArgs e)
    //{
    //    popupstudaddinl.Visible = false;
    //}
    //protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    //{
    //    popupstudaddinl.Visible = false;
    //}
    //popup student selection spread = popupselectstd
    public void bindbatch1()
    {
        try
        {
            ddl_batch1.Items.Clear();
            hat.Clear();
            //string sqlyear = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year desc";
            string Master1 = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                string group = Session["group_code"].ToString();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = group_semi[0].ToString();
                }
            }
            else
            {
                Master1 = Session["usercode"].ToString();
            }
            string sqlyear = "";
            sqlyear = "select distinct batch_year from tbl_attendance_rights where user_id='" + Master1 + "' and college_code='" + collegecode1 + "'";
            ds = d2.select_method(sqlyear, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch1.DataSource = ds;
                ddl_batch1.DataTextField = "batch_year";
                ddl_batch1.DataValueField = "batch_year";
                ddl_batch1.DataBind();
                ddl_batch1.SelectedIndex = 3;
            }
        }
        catch
        {
        }
    }
    public void binddegree2()
    {
        try
        {
            ds.Clear();
            cbl_degree2.Items.Clear();
            string query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code='" + collegecode1 + "' and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + usercode + "";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree2.DataSource = ds;
                cbl_degree2.DataTextField = "course_name";
                cbl_degree2.DataValueField = "course_id";
                cbl_degree2.DataBind();
                if (cbl_degree2.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree2.Items.Count; i++)
                    {
                        cbl_degree2.Items[i].Selected = true;
                    }
                    txt_degree2.Text = "Degree(" + cbl_degree2.Items.Count + ")";
                }
                else
                {
                    txt_degree2.Text = "--Select--";
                }
            }
            else
            {
                txt_degree2.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void cb_degree2_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            string buildvalue1 = "";
            string build1 = "";
            if (cb_degree2.Checked == true)
            {
                for (int i = 0; i < cbl_degree2.Items.Count; i++)
                {
                    if (cb_degree2.Checked == true)
                    {
                        cbl_degree2.Items[i].Selected = true;
                        txt_degree2.Text = "Degree(" + (cbl_degree2.Items.Count) + ")";
                        build1 = cbl_degree2.Items[i].Value.ToString();
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
                bindbranch1(buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_degree2.Items.Count; i++)
                {
                    cbl_degree2.Items[i].Selected = false;
                    txt_degree2.Text = "--Select--";
                    txt_branch2.Text = "--Select--";
                    cbl_branch1.ClearSelection();
                    cb_branch1.Checked = false;
                }
            }
            bindbranch1(college);
            // Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_degree2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_degree2.Checked = false;
            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_degree2.Items.Count; i++)
            {
                if (cbl_degree2.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    //  txt_branch.Text = "--Select--";
                    build = cbl_degree2.Items[i].Value.ToString();
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
            bindbranch1(buildvalue);
            if (seatcount == cbl_degree.Items.Count)
            {
                txt_degree2.Text = "Degree(" + seatcount.ToString() + ")";
                cb_degree2.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_degree2.Text = "--Select--";
                txt_degree2.Text = "--Select--";
            }
            else
            {
                txt_degree2.Text = "Degree(" + seatcount.ToString() + ")";
            }
            // bindbranch(college);
        }
        catch (Exception ex)
        {
        }
    }
    public void bindbranch1(string branch)
    {
        try
        {
            cbl_branch1.Items.Clear();
            string itemheader = "";
            for (int i = 0; i < cbl_degree2.Items.Count; i++)
            {
                if (cbl_degree2.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_degree2.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_degree2.Items[i].Value.ToString() + "";
                    }
                }
            }
            string commname = "";
            if (branch != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code ";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code";
            }
            if (itemheader.Trim() != "")
            {
                ds = d2.select_method(commname, hat, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch1.DataSource = ds;
                    cbl_branch1.DataTextField = "dept_name";
                    cbl_branch1.DataValueField = "degree_code";
                    cbl_branch1.DataBind();
                    if (cbl_branch1.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_branch1.Items.Count; i++)
                        {
                            cbl_branch1.Items[i].Selected = true;
                        }
                        txt_branch2.Text = "Branch(" + cbl_branch1.Items.Count + ")";
                    }
                }
                else
                {
                    txt_branch2.Text = "--Select--";
                }
            }
            else
            {
                txt_branch2.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_branch1_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_branch1.Checked == true)
            {
                for (int i = 0; i < cbl_branch1.Items.Count; i++)
                {
                    cbl_branch1.Items[i].Selected = true;
                }
                txt_branch2.Text = "Branch(" + (cbl_branch1.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_branch1.Items.Count; i++)
                {
                    cbl_branch1.Items[i].Selected = false;
                }
                txt_branch2.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_branch1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_branch2.Text = "--Select--";
            cb_branch1.Checked = false;
            for (int i = 0; i < cbl_branch1.Items.Count; i++)
            {
                if (cbl_branch1.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_branch2.Text = "Branch(" + commcount.ToString() + ")";
                if (commcount == cbl_branch.Items.Count)
                {
                    cb_branch1.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getroll(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct R.Roll_No from Registration r where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and r.roll_no not in (select Roll_No  from DayScholourStaffAdd where ISNULL(Roll_No,'') <>'' )  and R.roll_no like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    // theivamani 29.10.15
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getroll1(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct R.Roll_No from Registration r where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and r.roll_no not in (select Roll_No  from DayScholourStaffAdd where ISNULL(Roll_No,'') <>'' )   and R.roll_no like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    //protected void btn_go1_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string selectquery = "";
    //        int sno = 0;
    //        Fpspread2.SaveChanges();
    //        string itemheader = "";
    //        for (int i = 0; i < cbl_branch1.Items.Count; i++)
    //        {
    //            if (cbl_branch1.Items[i].Selected == true)
    //            {
    //                if (itemheader == "")
    //                {
    //                    itemheader = "" + cbl_branch1.Items[i].Value.ToString() + "";
    //                }
    //                else
    //                {
    //                    itemheader = itemheader + "'" + "," + "" + "'" + cbl_branch1.Items[i].Value.ToString() + "";
    //                }
    //            }
    //        }
    //        string batch_year = Convert.ToString(ddl_batch1.SelectedItem.Text);
    //        //theivamani 30.10.15
    //        if (itemheader.Trim() != "" && batch_year.Trim() != "")
    //        {
    //            if (txt_rollno1.Text == "")
    //            {
    //                //  selectquery = "select Roll_No,Roll_Admit,Stud_Name,d.Degree_Code ,(C.Course_Name +' - '+ dt.dept_acronym) as Department  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Batch_Year =" + batch_year + " and r.degree_code in ('" + itemheader + "') and r.Stud_Type <>'Hostler' and r.roll_no not in (select Roll_No  from DayScholourStaffAdd where ISNULL(Roll_No,'') <>'' )  order by Roll_No,d.Degree_Code ";
    //                selectquery = "select Roll_No,Roll_Admit,Stud_Name,d.Degree_Code ,(C.Course_Name +' - '+ dt.dept_acronym) as Department  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Batch_Year =" + batch_year + " and r.degree_code in ('" + itemheader + "') and r.Stud_Type <>'Hostler' and r.roll_no not in (select Roll_No  from DayScholourStaffAdd where ISNULL(Roll_No,'') <>'' )   ";
    //            }
    //            else
    //            {
    //                //selectquery = "select Roll_No,Roll_Admit,Stud_Name,d.Degree_Code ,(C.Course_Name +' - '+ dt.Dept_Name) as Department  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Batch_Year =" + batch_year + " and r.degree_code in ('" + itemheader + "')  order by Roll_No,d.Degree_Code ";
    //                selectquery = "select Roll_No,Roll_Admit,Stud_Name,d.Degree_Code ,(C.Course_Name +' - '+ dt.dept_acronym) as Department  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and r.Stud_Type <>'Hostler' and r.roll_no not in (select Roll_No  from DayScholourStaffAdd where ISNULL(Roll_No,'') <>'' ) and r.Roll_No ='" + txt_rollno1.Text + "'";
    //            }
    //            //theivamani 31.10.15
    //            string strorderby = d2.GetFunction("select value from Master_Settings where settings='order_by'");
    //            if (strorderby == "")
    //            {
    //                strorderby = "";
    //            }
    //            else
    //            {
    //                if (strorderby == "0")
    //                {
    //                    strorderby = "ORDER BY r.Roll_No";
    //                }
    //                //else if (strorderby == "1")
    //                //{
    //                //    strorderby = "ORDER BY r.Reg_No";
    //                //}
    //                else if (strorderby == "2")
    //                {
    //                    strorderby = "ORDER BY r.Stud_Name";
    //                }
    //                else if (strorderby == "0,2")
    //                {
    //                    strorderby = "ORDER BY r.Roll_No,r.Stud_Name";
    //                }
    //                else
    //                {
    //                    strorderby = "";
    //                }
    //            }
    //            string query = selectquery + strorderby;
    //            ds.Clear();
    //            ds = d2.select_method_wo_parameter(query, "Text");
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                Fpspread2.Sheets[0].RowCount = 1;
    //                Fpspread2.Sheets[0].ColumnCount = 0;
    //                Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
    //                Fpspread2.CommandBar.Visible = false;
    //                Fpspread2.Sheets[0].ColumnCount = 6;
    //                Fpspread2.Sheets[0].RowHeader.Visible = false;
    //                Fpspread2.Sheets[0].AutoPostBack = false;
    //                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
    //                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
    //                darkstyle.ForeColor = Color.White;
    //                Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
    //                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
    //                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
    //                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
    //                Fpspread2.Sheets[0].Columns[0].Locked = true;
    //                Fpspread2.Columns[0].Width = 50;
    //                FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
    //                chkall.AutoPostBack = true;
    //                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
    //                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
    //                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
    //                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
    //                Fpspread2.Columns[1].Width = 80;
    //                Fpspread2.Sheets[0].Columns[1].Locked = false;
    //                Fpspread2.Sheets[0].Cells[0, 1].CellType = chkall;
    //                Fpspread2.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
    //                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll Admit";
    //                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
    //                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
    //                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
    //                //theivamani 29.10.15
    //                Fpspread2.Sheets[0].Columns[2].Visible = false;
    //                Fpspread2.Sheets[0].Columns[2].Locked = true;
    //                Fpspread2.Columns[2].Width = 100;
    //                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Roll No";
    //                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
    //                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
    //                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
    //                Fpspread2.Sheets[0].Columns[3].Locked = true;
    //                Fpspread2.Columns[3].Width = 100;
    //                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Name";
    //                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
    //                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
    //                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
    //                Fpspread2.Sheets[0].Columns[4].Locked = true;
    //                Fpspread2.Columns[4].Width = 250;
    //                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Degree";
    //                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
    //                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
    //                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
    //                Fpspread2.Sheets[0].Columns[5].Locked = true;
    //                //  Fpspread2.Columns[5].Width = 250;
    //                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
    //                {
    //                    sno++;
    //                    Fpspread2.Sheets[0].RowCount++;
    //                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
    //                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
    //                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
    //                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
    //                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
    //                    //
    //                    FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
    //                    check.AutoPostBack = false;
    //                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = check;
    //                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
    //                    //
    //                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_Admit"]);
    //                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
    //                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
    //                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
    //                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]);
    //                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
    //                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
    //                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
    //                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]);
    //                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
    //                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
    //                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
    //                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Degree_Code"]);
    //                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["Department"]);
    //                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
    //                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
    //                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
    //                }
    //                Fpspread2.SaveChanges();
    //                Fpspread2.Visible = true;
    //                //theivamani 29.10.15
    //                lbl_cnt.Visible = true;
    //                lbl_cnt.Text = "No of Student :" + sno.ToString();
    //                //Fpspread2.Visible = true;
    //                btn_ok.Visible = true;
    //                btn_exit1.Visible = true;
    //                btn_exit2.Visible = true;
    //                lbl_errormsg.Visible = false;
    //                // Fpspread2.DataBind();
    //                //theivamani 31.10.15
    //                if (rollflag1 == "1")
    //                {
    //                    Fpspread2.Columns[3].Visible = true;
    //                }
    //                else
    //                {
    //                    Fpspread2.Columns[3].Visible = false;
    //                }
    //                Fpspread2.SaveChanges();
    //                Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
    //                Fpspread2.Sheets[0].SpanModel.Add(0, 2, 1, 4);
    //                Fpspread2.Sheets[0].FrozenRowCount = 1;
    //            }
    //            else
    //            {
    //                Fpspread2.Visible = false;
    //                lbl_cnt.Visible = false;
    //                lbl_errormsg.Visible = true;
    //                lbl_errormsg.Text = "No Records Found";
    //                btn_ok.Visible = false;
    //                btn_exit2.Visible = false;
    //            }
    //        }
    //        //theivamani 30.10.15
    //        else
    //        {
    //            Fpspread2.Visible = false;
    //            lbl_errormsg.Visible = true;
    //            lbl_cnt.Visible = false;
    //            lbl_errormsg.Text = "Please Select Any One Record";
    //            btn_ok.Visible = false;
    //            btn_exit2.Visible = false;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    protected void buttonok_Click(object sender, EventArgs e)
    {
        try
        {
            if (Fpspread2.Sheets[0].RowCount > 0)
            {
                Fpspread2.SaveChanges();
                string rollno = "";
                string rolladmit = "";
                string degreename1 = "";
                string name1 = "";
                string degreecode1 = "";
                for (int i = 1; i < Fpspread2.Sheets[0].RowCount; i++)
                {
                    int checkval = Convert.ToInt32(Fpspread2.Sheets[0].Cells[i, 1].Value);
                    if (checkval == 1)
                    {
                        string roll_no = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(i), 3].Text);
                        string roll_admit = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(i), 2].Text);
                        //string degreename = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(i), 5].Text);
                        //string degreecode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(i), 5].Tag);
                        //theivamani 12.12.15
                        string degreename = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(i), 3].Tag);
                        string degreecode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(i), 3].Tag);
                        string name = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(i), 4].Text);
                        if (rollno == "")
                        {
                            rollno = Convert.ToString(roll_no);
                        }
                        else
                        {
                            rollno = rollno + "," + Convert.ToString(roll_no) + "";
                        }
                        if (rolladmit == "")
                        {
                            rolladmit = Convert.ToString(roll_admit);
                        }
                        else
                        {
                            rolladmit = rolladmit + "," + Convert.ToString(roll_admit) + "";
                        }
                        if (degreename1 == "")
                        {
                            degreename1 = Convert.ToString(degreename);
                        }
                        else
                        {
                            degreename1 = degreename1 + "," + Convert.ToString(degreename) + "";
                        }
                        if (name1 == "")
                        {
                            name1 = Convert.ToString(name);
                        }
                        else
                        {
                            name1 = name1 + "," + Convert.ToString(name) + "";
                        }
                        if (degreecode1 == "")
                        {
                            degreecode1 = Convert.ToString(degreecode);
                        }
                        else
                        {
                            degreecode1 = degreecode1 + "," + Convert.ToString(degreecode) + "";
                        }
                    }
                }
                txt_rollno.Text = Convert.ToString(rollno);
                // txt_rolladmit.Text = Convert.ToString(rolladmit);
                txt_degree1.Text = Convert.ToString(degreename1);
                txt_name.Text = Convert.ToString(name1);
                Session["degreecodenew"] = Convert.ToString(degreecode1);
                popupselectstd.Visible = false;
            }
        }
        catch
        {
        }
    }
    protected void btn_exit2_Click(object sender, EventArgs e)
    {
        popupselectstd.Visible = false;
    }
    protected void imagebtnpopclose2_Click(object sender, EventArgs e)
    {
        popupselectstd.Visible = false;
    }
    //both dropdown binding hostel
    public void bindhostelname1()
    {
        try
        {
            ds.Clear();
            //string itemname = "select Hostel_code,Hostel_Name  from Hostel_Details order by Hostel_code";
            //ds = d2.select_method_wo_parameter(itemname, "Text");
            // ds = d2.Bindmess_inv(collegecode1);
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_hostelname1.DataSource = ds;
                ddl_hostelname1.DataTextField = "MessName";
                ddl_hostelname1.DataValueField = "MessMasterPK";
                ddl_hostelname1.DataBind();
                //ddl_hostelname2.DataSource = ds;
                //ddl_hostelname2.DataTextField = "MessName";
                //ddl_hostelname2.DataValueField = "MessID";
                //ddl_hostelname2.DataBind();
            }
        }
        catch
        {
        }
    }
    public void bindhostelnamestaff()
    {
        try
        {
            //theivamani 15.10.15
            ds.Clear();
            //string itemname = "select Hostel_code,Hostel_Name  from Hostel_Details order by Hostel_code";
            //ds = d2.select_method_wo_parameter(itemname, "Text");
            //ds = d2.Bindmess_inv(collegecode1);
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                //ddl_hostelname1.DataSource = ds;
                //ddl_hostelname1.DataTextField = "MessName";
                //ddl_hostelname1.DataValueField = "MessID";
                //ddl_hostelname1.DataBind();
                ddl_hostelname2.DataSource = ds;
                ddl_hostelname2.DataTextField = "MessName";
                ddl_hostelname2.DataValueField = "MessMasterPK";
                ddl_hostelname2.DataBind();
            }
        }
        catch
        {
        }
    }
    //button alert mini popup
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }
    //protected void chkhostlnm_ChekedChange(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (chkhostlnm.Checked == true)
    //        {
    //            for (int i = 0; i < chklsthostlnm.Items.Count; i++)
    //            {
    //                chklsthostlnm.Items[i].Selected = true;
    //            }
    //            txthostlnm.Text = "Hostel(" + (chklsthostlnm.Items.Count) + ")";
    //        }
    //        else
    //        {
    //            for (int i = 0; i < chklsthostlnm.Items.Count; i++)
    //            {
    //                chklsthostlnm.Items[i].Selected = false;
    //            }
    //            txthostlnm.Text = "--Select--";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //protected void chklsthostlnm_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        txthostlnm.Text = "--Select--";
    //        chkhostlnm.Checked = false;
    //        int commcount = 0;
    //        for (int i = 0; i < chklsthostlnm.Items.Count; i++)
    //        {
    //            if (chklsthostlnm.Items[i].Selected == true)
    //            {
    //                commcount = commcount + 1;
    //            }
    //        }
    //        if (commcount > 0)
    //        {
    //            txthostlnm.Text = "Hostel(" + commcount.ToString() + ")";
    //            if (commcount == chklsthostlnm.Items.Count)
    //            {
    //                chkhostlnm.Checked = true;
    //            }
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}   
    protected void ddl_hostelname1_Change(object sender, EventArgs e)
    {
        try
        {
            loadsessionnew();
        }
        catch
        {
        }
    }
    protected void ddl_hostelname2_Change(object sender, EventArgs e)
    {
        try
        {
            bindstaffsession();
        }
        catch
        {
        }
    }
    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        delete();
        //surediv.Visible = false;
    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        alertpopwindow.Visible = false;
    }
    protected void delete()
    {
        try
        {
            flag = false;
            surediv.Visible = false;
            bool delet = false;
            int del = 0;
            Fpspread1.SaveChanges();
            if (rdb_staff.Checked == true)
            {
                if (Fpspread1.Rows.Count > 0)
                {
                    for (int i = 0; i < Fpspread1.Sheets[0].Rows.Count; i++)
                    {
                        string staffcode = "";
                        string sessioncode = "";
                        int checkval = Convert.ToInt32(Fpspread1.Sheets[0].Cells[i, 8].Value);
                        if (checkval != 0)
                        {
                            flag = true;
                            if (staffcode == "")
                            {
                                staffcode = "" + Fpspread1.Sheets[0].Cells[i, 1].Value + "";
                            }
                            else
                            {
                                staffcode = staffcode + "'" + "," + "'" + Fpspread1.Sheets[0].Cells[i, 1].Value + "";
                            }
                            if (sessioncode == "")
                            {
                                sessioncode = "" + Fpspread1.Sheets[0].Cells[i, 5].Tag + "";
                            }
                            else
                            {
                                sessioncode = sessioncode + "'" + "," + "'" + Fpspread1.Sheets[0].Cells[i, 5].Tag + "";
                            }
                            string[] separators = { ",", "'" };
                            string[] staffcoe = staffcode.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                            string[] separatorshoscode = { ",", "'" };
                            string[] sessioncoe = sessioncode.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                            for (int ij = 0; ij < staffcoe.Length && ij < sessioncoe.Length; ij++)
                            {
                                //string staffcode = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 1].Value);
                                //  string sessioncode = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 5].Tag);
                                string q1 = "delete DayScholourStaffAdd where Staff_code='" + staffcoe[ij] + "' and Session_code ='" + sessioncoe[ij] + "'";
                                del = d2.update_method_wo_parameter(q1, "Text");
                            }
                            //if (del != 0)
                            //{
                            //    delet = true;
                            //}
                            // }
                        }
                    }
                    if (del != 0)
                    {
                        surediv.Visible = false;
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Deleted Successfully";
                        lblalerterr.Visible = true;
                        btn_go_Click(sender, e);
                    }
                    else
                    {
                        surediv.Visible = false;
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Please Select Any Record";
                        lblalerterr.Visible = true;
                        btn_go_Click(sender, e);
                    }
                    //if (delet == true)
                    //{
                    //    surediv.Visible = false;
                    //    alertpopwindow.Visible = true;
                    //    lblalerterr.Text = "Deleted Successfully";
                    //    lblalerterr.Visible = true;
                    //}
                }
                else
                {
                    surediv.Visible = false;
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Records Found";
                    lblalerterr.Visible = true;
                    btn_go_Click(sender, e);
                }
            }
            if (rdb_student.Checked == true)
            {
                if (Fpspread1.Rows.Count > 0)
                {
                    string rollno = "";
                    string sessioncode = "";
                    for (int i = 0; i < Fpspread1.Sheets[0].Rows.Count; i++)
                    {
                        int checkval = Convert.ToInt32(Fpspread1.Sheets[0].Cells[i, 7].Value);
                        if (checkval != 0)
                        {
                            flag = true;
                            if (rollno == "")
                            {
                                rollno = "" + Fpspread1.Sheets[0].Cells[i, 1].Value + "";
                            }
                            else
                            {
                                rollno = rollno + "'" + "," + "'" + Fpspread1.Sheets[0].Cells[i, 1].Value + "";
                            }
                            if (sessioncode == "")
                            {
                                sessioncode = "" + Fpspread1.Sheets[0].Cells[i, 4].Tag + "";
                            }
                            else
                            {
                                sessioncode = sessioncode + "'" + "," + "'" + Fpspread1.Sheets[0].Cells[i, 4].Tag + "";
                            }
                            string[] separators = { ",", "'" };
                            string[] rno = rollno.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                            string[] separatorshoscode = { ",", "'" };
                            string[] sessioncoe = sessioncode.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                            for (int ij = 0; ij < rno.Length && ij < sessioncoe.Length; ij++)
                            {
                                //string rollno = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 1].Value);
                                //  string sessioncode = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 4].Tag);
                                string q1 = "delete DayScholourStaffAdd where Roll_No='" + rno[ij] + "'  and Session_code ='" + sessioncoe[ij] + "'";
                                del = d2.update_method_wo_parameter(q1, "Text");
                            }
                            //if (del != 0)
                            //{
                            //    delet = true;
                            //}
                            // }
                        }
                    }
                    if (del != 0)
                    {
                        surediv.Visible = false;
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Deleted Successfully";
                        lblalerterr.Visible = true;
                        btn_go_Click(sender, e);
                    }
                    else
                    {
                        surediv.Visible = false;
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Please Select Any Record";
                        lblalerterr.Visible = true;
                        btn_go_Click(sender, e);
                    }
                }
                else
                {
                    surediv.Visible = false;
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Records Found";
                    lblalerterr.Visible = true;
                    btn_go_Click(sender, e);
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_delete_click(object sender, EventArgs e)
    {
        //try
        //{
        //    flag = false;
        //    surediv.Visible = false;
        //    bool delet = false;
        //    int del = 0;
        //    Fpspread1.SaveChanges();
        //    if (rdb_staff.Checked == true)
        //    {
        //        string staffcode = "";
        //        string sessioncode = "";
        //        for (int i = 0; i < Fpspread1.Sheets[0].Rows.Count; i++)
        //        {
        //            int checkval = Convert.ToInt32(Fpspread1.Sheets[0].Cells[i, 8].Value);
        //            if (checkval != 0)
        //            {
        //                 flag = true;
        //                if (staffcode == "")
        //                {
        //                    staffcode = "" + Fpspread1.Sheets[0].Cells[i, 1].Value + "";
        //                }
        //                else
        //                {
        //                    staffcode = staffcode + "'" + "," + "'" + Fpspread1.Sheets[0].Cells[i, 1].Value + "";
        //                }
        //                if (sessioncode == "")
        //                {
        //                    sessioncode = "" + Fpspread1.Sheets[0].Cells[i, 5].Tag + "";
        //                }
        //                else
        //                {
        //                    sessioncode = sessioncode + "'" + "," + "'" + Fpspread1.Sheets[0].Cells[i, 5].Tag + "";
        //                }
        //                string[] separators = { ",", "'" };
        //                string[] staffcoe = staffcode.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        //                string[] separatorshoscode = { ",", "'" };
        //                string[] sessioncoe = sessioncode.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        //                for (int ij = 0; ij < staffcoe.Length && ij < sessioncoe.Length; ij++)
        //                {
        //                    //  string staffcode = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 1].Value);
        //                    // string sessioncode = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 5].Tag);
        //                    string q1 = "delete DayScholourStaffAdd where Staff_code='" + staffcoe[ij] + "' and Session_code ='" + sessioncoe[ij] + "'";
        //                    del = d2.update_method_wo_parameter(q1, "Text");
        //                }
        //                    if (del != 0)
        //                    {
        //                        delet = true;
        //                    }
        //               // }
        //            }
        //        }
        //        if (delet == true)
        //        {
        //            surediv.Visible = false;
        //            alertpopwindow.Visible = true;
        //            lblalerterr.Text = "Deleted Successfully";
        //            lblalerterr.Visible = true;
        //        }
        //    }
        //    if (rdb_student.Checked == true)
        //    {
        //        string rollno = "";
        //        string sessioncode = "";
        //        for (int i = 0; i < Fpspread1.Sheets[0].Rows.Count; i++)
        //        {
        //            int checkval = Convert.ToInt32(Fpspread1.Sheets[0].Cells[i, 7].Value);
        //            if (checkval != 0)
        //            {
        //                flag = true;
        //                if (rollno == "")
        //                {
        //                    rollno = "" + Fpspread1.Sheets[0].Cells[i, 1].Value + "";
        //                }
        //                else
        //                {
        //                    rollno = rollno + "'" + "," + "'" + Fpspread1.Sheets[0].Cells[i, 1].Value + "";
        //                }
        //                if (sessioncode == "")
        //                {
        //                    sessioncode = "" + Fpspread1.Sheets[0].Cells[i, 4].Tag + "";
        //                }
        //                else
        //                {
        //                    sessioncode = sessioncode + "'" + "," + "'" + Fpspread1.Sheets[0].Cells[i, 4].Tag + "";
        //                }
        //                string[] separators = { ",", "'" };
        //                string[] rno = rollno.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        //                string[] separatorshoscode = { ",", "'" };
        //                string[] sessioncoe = sessioncode.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        //                for (int ij = 0; ij < rno.Length && ij < sessioncoe.Length; ij++)
        //                {
        //                    // string rollno = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 1].Value);
        //                    //  string sessioncode = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 4].Tag);
        //                    string q1 = "delete DayScholourStaffAdd where Roll_No='" + rno[ij] + "'  and Session_code ='" + sessioncoe[ij] + "'";
        //                    del = d2.update_method_wo_parameter(q1, "Text");
        //                }
        //                    if (del != 0)
        //                    {
        //                        delet = true;
        //                    }
        //                //}
        //            }
        //        }
        //        if (delet == true)
        //        {
        //            surediv.Visible = false;
        //            alertpopwindow.Visible = true;
        //            lblalerterr.Text = "Deleted Successfully";
        //            lblalerterr.Visible = true;
        //        }
        //    }
        //      btn_go_Click(sender, e);
        //    if (flag == true)
        //    {
        //        alertpopwindow.Visible = false;
        //        surediv.Visible = true;
        //        lbl_sure.Text = "Do you want to Delete this Record?";
        //    }
        //    else
        //    {
        //        surediv.Visible = false;
        //        alertpopwindow.Visible = true;
        //        lblalerterr.Text = "Please select any record";
        //        lblalerterr.Visible = true;
        //        btn_go_Click(sender, e);
        //    }
        //}
        //catch
        //{
        //}
        try
        {
            if (btn_delete.Text == "Delete")
            {
                surediv.Visible = true;
                lbl_sure.Text = "Do you want to Delete this Record?";
            }
        }
        catch
        {
        }
    }
    protected void txt_staffname_Text_Changed(object sender, EventArgs e)
    {
        string staffname = Convert.ToString(txt_staffname.Text);
        string q1 = "select s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,desig_name from staffmaster s,stafftrans st,hrdept_master h ,desig_master d where staff_name='" + staffname + "' and s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code";
        ds.Clear();
        ds = d2.select_method_wo_parameter(q1, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            string StaffName = Convert.ToString(txt_staffname.Text);
            string StaffDepartment = Convert.ToString(ds.Tables[0].Rows[0][3]);
            txt_staffname.Text = StaffName;
            txt_department1.Text = StaffDepartment;
        }
        else
        {
            alertpopwindow.Visible = true;
            lblalerterr.Text = "This Staff Name Already Added";
            lblalerterr.Visible = true;
            txt_staffname.Text = "";
            txt_department1.Text = "";
        }
    }
    protected void ddl_college_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadhostel();
        loadsession();
        bindbatch();
        degree();
        bindbranch(college);
        bindcbldept();
        binddesig();
        bindstafftype();
        bindsection();
        Fpspread1.Visible = false;
        rptprint.Visible = false;
    }
    protected void txt_rollno_txtchange(object sender, EventArgs e)
    {
        try
        {
            string rollno = Convert.ToString(txt_rollno.Text);
            string selectquery = "select Roll_No,Roll_Admit,Stud_Name,d.Degree_Code ,(C.Course_Name +' - '+ dt.dept_acronym) as Department  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR'  and r.Roll_No ='" + txt_rollno.Text + "' and r.roll_no not in (select Roll_No  from DayScholourStaffAdd where ISNULL(Roll_No,'') <>'' ) ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                rollno = Convert.ToString(txt_rollno.Text);
                string stuname = Convert.ToString(ds.Tables[0].Rows[0][2]);
                string degree = Convert.ToString(ds.Tables[0].Rows[0][4]);
                txt_name.Text = stuname;
                txt_degree1.Text = degree;
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "This Student Name Already Added";
                lblalerterr.Visible = true;
                txt_rollno.Text = "";
                txt_name.Text = "";
                txt_degree1.Text = "";
            }
        }
        catch
        {
        }
    }
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }
    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = true;
        loadsessionnew();
       
        loadhour();
        loadsecond();
        loadminits();
        timevalue();
        bindhostelname1();
        bindhostelnamestaff();
        bindstaffsession();
        cb_session2.Checked = true;
        //lbl_date.Visible = false;
        //txt_date.Visible = false;
        //lbl_time.Visible = false;
        //ddl_hour.Visible = false;
        //ddl_minits.Visible = false;
        //// ddl_seconds.Visible = false;
        //ddl_timeformate.Visible = false;
        //lbl_hostelname1.Visible = false;
        //ddl_hostelname1.Visible = false;
        //lbl_session1.Visible = false;
        //UpdatePanel4.Visible = false;
        //lbl_rollno.Visible = false;
        //txt_rollno.Visible = false;
        //btn_rolladmit.Visible = false;
        //stu.Visible = false;
        //lbl_name.Visible = false;
        //txt_name.Visible = false;
        //lbl_degree1.Visible = false;
        //txt_degree1.Visible = false;
        //btn_save.Visible = false;
        //btn_exit1.Visible = false;
        lbl_stf_date.Visible = false;
        txt_stf_date.Visible = false;
        lbl_stf_time.Visible = false;
        ddl_stfhr.Visible = false;
        ddl_stfm.Visible = false;
        ddl_stfam.Visible = false;
        lbl_hostelname2.Visible = false;
        ddl_hostelname2.Visible = false;
        lbl_session2.Visible = false;
        uup6.Visible = false;
        lbl_Staffname.Visible = false;
        txt_staffname.Visible = false;
        btn_staff.Visible = false;
        staff.Visible = false;
        lbl_department1.Visible = false;
        txt_department1.Visible = false;
        lbl_stafftype1.Visible = false;
        txt_stafftype1.Visible = false;
        txt_department1.Visible = false;
        lbl_stafftype1.Visible = false;
        btn_save2staff.Visible = false;
        btn_exit3staff.Visible = false;
        rdb_stu.Checked = true;
        rdb_sta.Checked = false;
        //txt_rollno.Text = "";
        //txt_name.Text = "";
        //txt_degree1.Text = "";
        //txt_staffname.Text = "";
        //txt_department1.Text = "";
        lbl_date.Visible = true;
        txt_date.Visible = true;
        lbl_time.Visible = true;
        ddl_hour.Visible = true;
        ddl_minits.Visible = true;
        //ddl_seconds.Visible = true;
        ddl_timeformate.Visible = true;
        lbl_hostelname1.Visible = true;
        ddl_hostelname1.Visible = true;
        lbl_session1.Visible = true;
        UpdatePanel4.Visible = true;
        lbl_rollno.Visible = true;
        txt_rollno.Visible = true;
        lblid.Visible = true;
        txtid.Visible = true;
        txtid1.Visible = false;
        Llid.Visible = false;
        btn_rolladmit.Visible = true;
        stu.Visible = true;
        lbl_name.Visible = true;
        txt_name.Visible = true;
        lbl_degree1.Visible = true;
        txt_degree1.Visible = true;
        btn_save.Visible = true;
        btn_exit1.Visible = true;
        txt_rollno.Text = "";
        txt_name.Text = "";
        txt_degree1.Text = "";
        txt_staffname.Text = "";
        txt_department1.Text = "";
        idgeneration();
    }
    protected void rdb_sta_CheckedChanged(object sender, EventArgs e)
    {
        if (rdb_sta.Checked == true)
        {
            lbl_stf_date.Visible = true;
            txt_stf_date.Visible = true;
            lbl_stf_time.Visible = true;
            ddl_stfhr.Visible = true;
            ddl_stfm.Visible = true;
            ddl_stfam.Visible = true;
            lbl_hostelname2.Visible = true;
            ddl_hostelname2.Visible = true;
            lbl_session2.Visible = true;
            uup6.Visible = true;
            lbl_Staffname.Visible = true;
            txt_staffname.Visible = true;
            btn_staff.Visible = true;
            staff.Visible = true;
            lbl_department1.Visible = true;
            txt_department1.Visible = true;
            lbl_stafftype1.Visible = true;
            txt_stafftype1.Visible = true;
            txt_department1.Visible = true;
            lbl_stafftype1.Visible = true;
            btn_save2staff.Visible = true;
            btn_exit3staff.Visible = true;
            lbl_date.Visible = false;
            txt_date.Visible = false;
            lbl_time.Visible = false;
            ddl_hour.Visible = false;
            ddl_minits.Visible = false;
            // ddl_seconds.Visible = false;
            ddl_timeformate.Visible = false;
            lbl_hostelname1.Visible = false;
            ddl_hostelname1.Visible = false;
            lbl_session1.Visible = false;
            UpdatePanel4.Visible = false;
            lbl_rollno.Visible = false;
            txt_rollno.Visible = false;
            lblid.Visible = false;
            txtid.Visible = false;
            txtid1.Visible = true;
            Llid.Visible = true;
            btn_rolladmit.Visible = false;
            stu.Visible = false;
            lbl_name.Visible = false;
            txt_name.Visible = false;
            lbl_degree1.Visible = false;
            txt_degree1.Visible = false;
            btn_save.Visible = false;
            btn_exit1.Visible = false;
            //  rdb_stu.Enabled = false;
            //  btn_staff_Click(sender, e);
            txt_staffname.Text = "";
            txt_department1.Text = "";
            idgeneration();
        }
        else
        {
            lbl_stf_date.Visible = false;
            txt_stf_date.Visible = false;
            lbl_stf_time.Visible = false;
            ddl_stfhr.Visible = false;
            ddl_stfm.Visible = false;
            ddl_stfam.Visible = false;
            lbl_hostelname2.Visible = false;
            ddl_hostelname2.Visible = false;
            lbl_session2.Visible = false;
            uup6.Visible = false;
            lbl_Staffname.Visible = false;
            txt_staffname.Visible = false;
            btn_staff.Visible = false;
            staff.Visible = false;
            lbl_department1.Visible = false;
            txt_department1.Visible = false;
            lbl_stafftype1.Visible = false;
            txt_stafftype1.Visible = false;
            txt_department1.Visible = false;
            lbl_stafftype1.Visible = false;
            btn_save2staff.Visible = false;
            btn_exit3staff.Visible = false;
            //  rdb_stu.Enabled = true;
        }
    }
    protected void rdb_stu_CheckedChanged(object sender, EventArgs e)
    {
        if (rdb_stu.Checked == true)
        {
            timevalue();
            lbl_date.Visible = true;
            txt_date.Visible = true;
            lbl_time.Visible = true;
            ddl_hour.Visible = true;
            ddl_minits.Visible = true;
            //ddl_seconds.Visible = true;
            ddl_timeformate.Visible = true;
            lbl_hostelname1.Visible = true;
            ddl_hostelname1.Visible = true;
            lbl_session1.Visible = true;
            UpdatePanel4.Visible = true;
            lbl_rollno.Visible = true;
            txt_rollno.Visible = true;
            lblid.Visible = true;
            txtid.Visible = true;
            txtid1.Visible = false;
            Llid.Visible = false;
            btn_rolladmit.Visible = true;
            stu.Visible = true;
            lbl_name.Visible = true;
            txt_name.Visible = true;
            lbl_degree1.Visible = true;
            txt_degree1.Visible = true;
            btn_save.Visible = true;
            btn_exit1.Visible = true;
            lbl_stf_date.Visible = false;
            txt_stf_date.Visible = false;
            lbl_stf_time.Visible = false;
            ddl_stfhr.Visible = false;
            ddl_stfm.Visible = false;
            ddl_stfam.Visible = false;
            lbl_hostelname2.Visible = false;
            ddl_hostelname2.Visible = false;
            lbl_session2.Visible = false;
            uup6.Visible = false;
            lbl_Staffname.Visible = false;
            txt_staffname.Visible = false;
            btn_staff.Visible = false;
            staff.Visible = false;
            lbl_department1.Visible = false;
            txt_department1.Visible = false;
            lbl_stafftype1.Visible = false;
            txt_stafftype1.Visible = false;
            txt_department1.Visible = false;
            lbl_stafftype1.Visible = false;
            btn_save2staff.Visible = false;
            btn_exit3staff.Visible = false;
            //btn_save2staff.Visible = true;
            //btn_exit3staff.Visible = true;
            //  rdb_stu.Enabled = false;
            //  btn_staff_Click(sender, e);
            txt_rollno.Text = "";
            txt_name.Text = "";
            txt_degree1.Text = "";
            idgeneration();
        }
        else
        {
            lbl_date.Visible = false;
            txt_date.Visible = false;
            lbl_time.Visible = false;
            ddl_hour.Visible = false;
            ddl_minits.Visible = false;
            // ddl_seconds.Visible = false;
            ddl_timeformate.Visible = false;
            lbl_hostelname1.Visible = false;
            ddl_hostelname1.Visible = false;
            lbl_session1.Visible = false;
            UpdatePanel4.Visible = false;
            lbl_rollno.Visible = false;
            txt_rollno.Visible = false;
            lblid.Visible = false;
            txtid.Visible = false;
            txtid1.Visible = true;
            Llid.Visible = true;
            btn_rolladmit.Visible = false;
            stu.Visible = false;
            lbl_name.Visible = false;
            txt_name.Visible = false;
            lbl_degree1.Visible = false;
            txt_degree1.Visible = false;
            btn_save.Visible = false;
            btn_exit1.Visible = false;
            //  rdb_stu.Enabled = true;
        }
    }
    protected void btn_exit3staff_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }
    protected void btn_exit1_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }
    public void txt_datestud_changed(object sender, EventArgs e)
    {
        try
        {
            // div_stud.Attributes.Add("style", "display:block");
            if (txt_date.Text != "")
            {
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_date.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = date.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                if (dt1 < dt)
                {
                    txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
            }
        }
        catch
        {
        }
    }
    public void loadhour()
    {
        try
        {
            ddl_hour.Items.Clear();
            ddl_stfhr.Items.Clear();
            for (int i = 1; i <= 24; i++)
            {
                ddl_hour.Items.Add(Convert.ToString(i));
                ddl_stfhr.Items.Add(Convert.ToString(i));
                ddl_hour.SelectedIndex = ddl_hour.Items.Count - 1;
                ddl_stfhr.SelectedIndex = ddl_hour.Items.Count - 1;
            }
        }
        catch
        {
        }
    }
    public void loadsecond()
    {
        ddl_seconds.Items.Clear();
        for (int i = 0; i <= 60; i++)
        {
            string value = Convert.ToString(i);
            if (value.Length == 1)
            {
                value = "0" + "" + value;
            }
            ddl_seconds.Items.Add(Convert.ToString(value));
        }
    }
    public void loadminits()
    {
        ddl_minits.Items.Clear();
        ddl_stfm.Items.Clear();
        for (int i = 0; i <= 59; i++)
        {
            string value = Convert.ToString(i);
            if (value.Length == 1)
            {
                value = "0" + "" + value;
            }
            ddl_minits.Items.Add(Convert.ToString(value));
            ddl_stfm.Items.Add(Convert.ToString(value));
        }
    }
    public void timevalue()
    {
        try
        {
            string time = DateTime.Now.ToString("HH:mm:ss");
            //string time =Convert.ToString(txt_viewtime.Text);
            string[] ay = time.Split(':');
            string val_hr = ay[0].ToString();
            int hr = Convert.ToInt16(val_hr);
            ddl_hour.Text = ay[0].ToString();
            ddl_minits.Text = ay[1].ToString();
            ddl_stfhr.Text = ay[0].ToString();
            ddl_stfm.Text = ay[1].ToString();
            if (hr >= 12)
            {
                ddl_timeformate.Text = "PM";
                ddl_stfam.Text = "PM";
            }
            else
            {
                ddl_timeformate.Text = "AM";
                ddl_stfam.Text = "AM";
            }
        }
        catch
        {
        }
    }
    public void txt_stf_date_changed(object sender, EventArgs e)
    {
        try
        {
            //div_staff.Attributes.Add("style", "display:block");
            if (txt_stf_date.Text != "")
            {
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_stf_date.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = date.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                if (dt1 < dt)
                {
                    txt_stf_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
            }
        }
        catch
        {
        }
    }
    protected void btn_go1_Click(object sender, EventArgs e)
    {
        try
        {
            string selectquery = "";
            int sno = 0;
            Fpspread2.SaveChanges();
            string itemheader = "";
            for (int i = 0; i < cbl_branch1.Items.Count; i++)
            {
                if (cbl_branch1.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_branch1.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_branch1.Items[i].Value.ToString() + "";
                    }
                }
            }
            string batch_year = Convert.ToString(ddl_batch1.SelectedItem.Text);
            //theivamani 30.10.15
            if (itemheader.Trim() != "" && batch_year.Trim() != "")
            {
                if (txt_rollno1.Text == "")
                {
                    //  selectquery = "select Roll_No,Roll_Admit,Stud_Name,d.Degree_Code ,(C.Course_Name +' - '+ dt.dept_acronym) as Department  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Batch_Year =" + batch_year + " and r.degree_code in ('" + itemheader + "') and r.Stud_Type <>'Hostler' and r.roll_no not in (select Roll_No  from DayScholourStaffAdd where ISNULL(Roll_No,'') <>'' )  order by Roll_No,d.Degree_Code ";
                    selectquery = "select Roll_No,Roll_Admit,Stud_Name,d.Degree_Code ,(C.Course_Name +' - '+ dt.dept_acronym) as Department  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Batch_Year =" + batch_year + " and r.degree_code in ('" + itemheader + "')  and r.roll_no not in (select Roll_No  from DayScholourStaffAdd where ISNULL(Roll_No,'') <>'' )   ";
                }
                else
                {
                    //selectquery = "select Roll_No,Roll_Admit,Stud_Name,d.Degree_Code ,(C.Course_Name +' - '+ dt.Dept_Name) as Department  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Batch_Year =" + batch_year + " and r.degree_code in ('" + itemheader + "')  order by Roll_No,d.Degree_Code ";
                    selectquery = "select Roll_No,Roll_Admit,Stud_Name,d.Degree_Code ,(C.Course_Name +' - '+ dt.dept_acronym) as Department  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and  r.roll_no not in (select Roll_No  from DayScholourStaffAdd where ISNULL(Roll_No,'') <>'' ) and r.Roll_No ='" + txt_rollno1.Text + "'";
                }
                //theivamani 31.10.15
                string strorderby = d2.GetFunction("select value from Master_Settings where settings='order_by'");
                if (strorderby == "")
                {
                    strorderby = "";
                }
                else
                {
                    if (strorderby == "0")
                    {
                        strorderby = "ORDER BY r.Roll_No";
                    }
                    //else if (strorderby == "1")
                    //{
                    //    strorderby = "ORDER BY r.Reg_No";
                    //}
                    else if (strorderby == "2")
                    {
                        strorderby = "ORDER BY r.Stud_Name";
                    }
                    else if (strorderby == "0,2")
                    {
                        strorderby = "ORDER BY r.Roll_No,r.Stud_Name";
                    }
                    else
                    {
                        strorderby = "";
                    }
                }
                string query = selectquery + strorderby;
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Fpspread2.Sheets[0].RowCount = 1;
                    Fpspread2.Sheets[0].ColumnCount = 0;
                    Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread2.CommandBar.Visible = false;
                    Fpspread2.Sheets[0].ColumnCount = 5;
                    Fpspread2.Sheets[0].RowHeader.Visible = false;
                    Fpspread2.Sheets[0].AutoPostBack = false;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].Columns[0].Locked = true;
                    Fpspread2.Columns[0].Width = 50;
                    FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                    chkall.AutoPostBack = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Columns[1].Width = 80;
                    Fpspread2.Sheets[0].Columns[1].Locked = false;
                    Fpspread2.Sheets[0].Cells[0, 1].CellType = chkall;
                    Fpspread2.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll Admit";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    //theivamani 29.10.15
                    Fpspread2.Sheets[0].Columns[2].Visible = false;
                    Fpspread2.Sheets[0].Columns[2].Locked = true;
                    Fpspread2.Columns[2].Width = 100;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Roll No";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].Columns[3].Locked = true;
                    Fpspread2.Columns[3].Width = 100;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Name";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].Columns[4].Locked = true;
                    Fpspread2.Columns[4].Width = 250;
                    //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Degree";
                    //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    //Fpspread2.Sheets[0].Columns[5].Locked = true;
                    //  Fpspread2.Columns[5].Width = 250;
                    //for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    //{
                    sno = 0;
                    int studcount = 0;
                    for (int row1 = 0; row1 < cbl_branch1.Items.Count; row1++)
                    {
                        if (cbl_branch1.Items[row1].Selected)
                        {
                            ds.Tables[0].DefaultView.RowFilter = "Degree_Code='" + Convert.ToSingle(cbl_branch1.Items[row1].Value) + "'";
                            DataView dv = ds.Tables[0].DefaultView;
                            if (dv.Count > 0)
                            {
                                Fpspread2.Sheets[0].RowCount = Fpspread2.Sheets[0].RowCount + 1;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dv[0]["Degree_Code"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(dv[0]["Department"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].AddSpanCell(Fpspread2.Sheets[0].RowCount - 1, 0, 1, 5);
                                sno++;
                                for (int row = 0; row < dv.Count; row++)
                                {
                                    studcount++;
                                    // sno++;
                                    Fpspread2.Sheets[0].RowCount++;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                    //   Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    //
                                    FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                                    check.AutoPostBack = false;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = check;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                    //
                                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_Admit"]);
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv[row]["Roll_Admit"]);
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]);
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[row]["Roll_No"]);
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(dv[0]["Department"]);
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]);
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dv[row]["Stud_Name"]);
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Degree_Code"]);
                                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["Department"]);
                                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                    //}
                                }
                            }
                        }
                    }
                    Fpspread2.SaveChanges();
                    Fpspread2.Visible = true;
                    //theivamani 29.10.15
                    lbl_cnt.Visible = true;
                    lbl_cnt.Text = "No of Students :" + studcount.ToString();
                    //Fpspread2.Visible = true;
                    btn_ok.Visible = true;
                    btn_exit1.Visible = true;
                    btn_exit2.Visible = true;
                    lbl_errormsg.Visible = false;
                    // Fpspread2.DataBind();
                    //theivamani 31.10.15
                    if (rollflag1 == "1")
                    {
                        Fpspread2.Columns[3].Visible = true;
                    }
                    else
                    {
                        Fpspread2.Columns[3].Visible = false;
                    }
                    Fpspread2.SaveChanges();
                    Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                    Fpspread2.Sheets[0].SpanModel.Add(0, 2, 1, 4);
                    Fpspread2.Sheets[0].FrozenRowCount = 1;
                }
                else
                {
                    Fpspread2.Visible = false;
                    lbl_cnt.Visible = false;
                    lbl_errormsg.Visible = true;
                    lbl_errormsg.Text = "No Records Found";
                    btn_ok.Visible = false;
                    btn_exit2.Visible = false;
                }
            }
            //theivamani 30.10.15
            else
            {
                Fpspread2.Visible = false;
                lbl_errormsg.Visible = true;
                lbl_cnt.Visible = false;
                lbl_errormsg.Text = "Please Select Any One Record";
                btn_ok.Visible = false;
                btn_exit2.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    //magesh 12.3.18
    protected void BindStudentType()
    {
        try
        {
            ddlStudType.Items.Clear();
            ds.Clear();
            string sql = "select StudentType,StudentTypeName from HostelStudentType where CollegeCode='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlStudType.DataSource = ds;
                ddlStudType.DataTextField = "StudentTypeName";
                ddlStudType.DataValueField = "StudentType";
                ddlStudType.DataBind();
            }
        }
        catch
        {
        }
    }


    protected void ddl_pop1hostelname_SelectedIndexChanged(object sender, EventArgs e)
    {
        idgeneration();
    }

    protected void idgeneration()
    {
        try
        {
            string newitemcode = "";

            string ishostel = string.Empty;
            string memtype = string.Empty;
            string newins = string.Empty;
            string hos_code = string.Empty;
            string hos_code1 = string.Empty;
            string colcode = ddl_college.SelectedValue;
            if (usercode != "")
            {
                newins = "select * from New_InsSettings where LinkName='hostelid generation' and user_code ='" + usercode + "' and college_code ='" + ddl_college.SelectedValue + "'";
            }
            else
            {
                newins = "select * from New_InsSettings where LinkName='hostelid generation' and user_code ='" + group_user + "' and college_code ='" + ddl_college.SelectedValue + "'";
            }
            ds = d2.select_method_wo_parameter(newins, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ishostel = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
            }
            if (ishostel == "1")
            {
                hos_code = Convert.ToString(ddl_pop1hostelname.SelectedValue);
                hos_code1 = Convert.ToString(ddl_pop1hostelname.SelectedValue);

                memtype = "3";
            }
            if (ishostel == "0")
            {
                hos_code = Convert.ToString(ddl_pop1hostelname.SelectedValue);
                hos_code1 = "0";
                ishostel = "0";
                if (rdb_stu.Checked == true)
                    memtype = "0";
                if (rdb_sta.Checked == true)
                    memtype = "1";

            }
            string selectquery = "select idAcr,idStNo,idSize from Hostelidgeneration where college_code='" + colcode + "' and hostelcode='" + hos_code1 + "' and ishostel='" + ishostel + "' and memtype='" + memtype + "' order by FromDate desc";//where Latestrec =1"
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string itemacronym = Convert.ToString(ds.Tables[0].Rows[0]["idAcr"]);
                string itemstarno = Convert.ToString(ds.Tables[0].Rows[0]["idStNo"]);
                string itemsize = Convert.ToString(ds.Tables[0].Rows[0]["idSize"]);
                if (itemacronym.Trim() != "" && itemstarno.Trim() != "")
                {
                    selectquery = " select distinct top (1) id  from HT_HostelRegistration where id like '" + Convert.ToString(itemacronym) + "[0-9]%'  and HostelMasterFK='" + hos_code + "' order by id desc";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string itemcode = Convert.ToString(ds.Tables[0].Rows[0]["id"]);
                        string itemacr = Convert.ToString(itemacronym);
                        int len = itemacr.Length;
                        itemcode = itemcode.Remove(0, len);
                        int len1 = Convert.ToString(itemcode).Length;
                        string newnumber = Convert.ToString((Convert.ToInt32(itemcode) + 1));
                        len = Convert.ToString(newnumber).Length;
                        len1 = len1 - len;
                        if (len1 == 2)
                        {
                            newitemcode = "00" + newnumber;
                        }
                        else if (len1 == 1)
                        {
                            newitemcode = "0" + newnumber;
                        }
                        else if (len1 == 3)
                        {
                            newitemcode = "000" + newnumber;
                        }
                        else if (len1 == 4)
                        {
                            newitemcode = "0000" + newnumber;
                        }
                        else if (len1 == 5)
                        {
                            newitemcode = "00000" + newnumber;
                        }
                        else if (len1 == 6)
                        {
                            newitemcode = "000000" + newnumber;
                        }
                        else
                        {
                            newitemcode = Convert.ToString(newnumber);
                        }
                        if (newitemcode.Trim() != "")
                        {
                            newitemcode = itemacr + "" + newitemcode;
                        }
                    }
                    else
                    {
                        string itemacr = Convert.ToString(itemstarno);
                        int len = itemacr.Length;
                        string items = Convert.ToString(itemsize);
                        int len1 = Convert.ToInt32(items);
                        int size = len1 - len;
                        if (size == 2)
                        {
                            newitemcode = "00" + itemstarno;
                        }
                        else if (size == 1)
                        {
                            newitemcode = "0" + itemstarno;
                        }
                        else if (size == 3)
                        {
                            newitemcode = "000" + itemstarno;
                        }
                        else if (size == 4)
                        {
                            newitemcode = "0000" + itemstarno;
                        }
                        else if (size == 5)
                        {
                            newitemcode = "00000" + itemstarno;
                        }
                        else if (size == 6)
                        {
                            newitemcode = "000000" + itemstarno;
                        }
                        else
                        {
                            newitemcode = Convert.ToString(itemstarno);
                        }
                        newitemcode = Convert.ToString(itemacronym) + "" + Convert.ToString(newitemcode);
                    }
                    if (rdb_sta.Checked == true)
                        txtid1.Text = Convert.ToString(newitemcode);
                    if (rdb_stu.Checked == true)
                        txtid.Text = Convert.ToString(newitemcode);
                    //poperrjs.Visible = true;
                    //btnsave.Visible = true;
                    //SelectdptGrid.Visible = false;
                    //btnupdate.Visible = false;
                    // btndelete.Visible = false;
                    // bindstore();
                    // bindunitddl();
                    // loadheadername();
                    //loadsubheadername();
                    // loaditem();
                    // bind_subheader();
                }
                else
                {
                   
                    //lbl_alert.Text = "Please Update Code Master";
                }
            }
        }
        catch
        {
        }
    }

  public void loadhostelpopup()
    {
        try
        {
            ds.Clear();
            //string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            //ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            string itemname = "select HostelMasterPK ,HostelName  from HM_HostelMaster  order by HostelMasterPK ";
            ds = d2.select_method_wo_parameter(itemname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {

                ddl_pop1hostelname.DataSource = ds;
                ddl_pop1hostelname.DataTextField = "HostelName";
                ddl_pop1hostelname.DataValueField = "HostelMasterPK";
                ddl_pop1hostelname.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }
}