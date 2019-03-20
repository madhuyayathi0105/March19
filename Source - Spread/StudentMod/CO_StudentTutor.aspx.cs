using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.IO;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Configuration;
public partial class CO_StudentTutor : System.Web.UI.Page
{
    string user_code;
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string college_code = "";
    string college = "";
    bool check = false;
    bool flag = false;
    static int btnflag;
    string rollflag1 = string.Empty;
    string regflag1 = string.Empty;
    string stuflag1 = string.Empty;
    DataSet ds2 = new DataSet();
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    ReuasableMethods rs = new ReuasableMethods();
    string grouporusercode = "";
    string build = "";
    private object sender;
    private EventArgs e;

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
            collegecode1 = Session["collegecode"].ToString();
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
                TRhostler.Attributes.Add("Style", "display:none;");
                BindCollegeinfo();
                bindhostel();
                bindcollege();
                bindbatch();
                degree();
                bindbranch();
                bindsem();
                BindSectionDetail();
                bindbatch1();
                degree1();
                bindbranch1();

                bindsem1();
                BindSectionDetail1();
                btnflag = 0;
                divdcorder.Visible = false;
                if (ddl_search2.SelectedItem.Text == "Staff Name")
                {
                    txt_searchbyname.Visible = true;
                    txt_searchbycode.Visible = false;
                }
                else if (ddl_search2.SelectedItem.Text == "Staff Code")
                {
                    txt_searchbycode.Visible = true;
                    txt_searchbyname.Visible = false;
                }
                Fpspread2.Sheets[0].ColumnCount = 0;
                Fpspread2.Sheets[0].RowCount = 0;
                Fpspread2.Visible = false;
                txt_studname.Visible = true;
                BindColumnOrder();
            }
        }
        catch (Exception ex) { }
    }

    #region Bind Method

    public void bindhostel()
    {
        try
        {
            string college = Convert.ToString(ddl_collegename.SelectedValue);
            string itemname = "select HostelMasterPK ,HostelName  from HM_HostelMaster  order by HostelMasterPK ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "Text");
            //  ds = d2.BindHostel(collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname.DataSource = ds;
                cbl_hostelname.DataTextField = "HostelName";
                cbl_hostelname.DataValueField = "HostelMasterPK";
                cbl_hostelname.DataBind();
                //Hostelcode = cbl_hostelname.SelectedValue;
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    cbl_hostelname.Items[i].Selected = true;
                    txt_hostelname.Text = "Hostel Name(" + (cbl_hostelname.Items.Count) + ")";
                    cb_hostelname.Checked = true;
                }
                string lochosname = "";
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    if (cbl_hostelname.Items[i].Selected == true)
                    {
                        string hosname = cbl_hostelname.Items[i].Value.ToString();
                        if (lochosname == "")
                        {
                            lochosname = hosname;
                        }
                        else
                        {
                            lochosname = lochosname + "'" + "," + "'" + hosname;
                        }
                    }
                }
                clgbuild(lochosname);
            }
            else
            {
                cbl_hostelname.Items.Insert(0, "--Select--");
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void binddept(string college)
    {
        try
        {
            ddl_deptname2.Items.Clear();
            ds = d2.loaddepartment(college);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_deptname2.DataSource = ds;
                ddl_deptname2.DataTextField = "dept_name";
                ddl_deptname2.DataValueField = "dept_code";
                ddl_deptname2.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void bindcollege()
    {
        try
        {
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegename2.DataSource = ds;
                ddl_collegename2.DataTextField = "collname";
                ddl_collegename2.DataValueField = "college_code";
                ddl_collegename2.DataBind();
            }
            binddept(ddl_collegename2.SelectedItem.Value.ToString());
        }
        catch
        {
        }
    }

    protected void bindhosteladd()
    {
        try
        {
            string itemname = "select HostelMasterPK ,HostelName  from HM_HostelMaster  order by HostelMasterPK ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "Text");
            // ds = d2.BindHostel(collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelnameadd.DataSource = ds;
                cbl_hostelnameadd.DataTextField = "HostelName";
                cbl_hostelnameadd.DataValueField = "HostelMasterPK";
                cbl_hostelnameadd.DataBind();
                //Hostelcode = cbl_hostelname.SelectedValue;
                for (int i = 0; i < cbl_hostelnameadd.Items.Count; i++)
                {
                    cbl_hostelnameadd.Items[i].Selected = true;
                    txt_hostelnameadd.Text = "Hostel Name(" + (cbl_hostelnameadd.Items.Count) + ")";
                    cb_hostelnameadd.Checked = true;
                }
                string lochosname = "";
                for (int i = 0; i < cbl_hostelnameadd.Items.Count; i++)
                {
                    if (cbl_hostelnameadd.Items[i].Selected == true)
                    {
                        string hosname = cbl_hostelnameadd.Items[i].Value.ToString();
                        if (lochosname == "")
                        {
                            lochosname = hosname;
                        }
                        else
                        {
                            lochosname = lochosname + "'" + "," + "'" + hosname;
                        }
                    }
                }
                clgbuildpop(lochosname);
            }
            else
            {
                cbl_hostelnameadd.Items.Insert(0, "--Select--");
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindbatch()
    {
        try
        {
            hat.Clear();
            cbl_batch.Items.Clear();
            ds.Clear();
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
            string query = "";
            string rights = "";
            string collegeCode = ddl_collegename.SelectedValue;
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if ((group_user.ToString().Trim() != "") && (group_user.Trim() != "0") && (group_user.ToString().Trim() != "-1"))
                rights = "and group_code='" + group_user + "'";
            else
                rights = " and user_code='" + usercode + "'";

            query = "select distinct d.Course_Id,c.Course_Name from Degree d,course c ,DeptPrivilages p where p.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.college_code=c.college_code   " + rights + " and d.college_code='" + collegeCode + "'";

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
                    cbl_degree.Items[0].Selected = true;
                }
                txt_degree.Text = lbl_degree.Text + "(" + 1 + ")";
                bindbranch();
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

    public void bindbranch()
    {
        try
        {
            string branch = rs.GetSelectedItemsValueAsString(cbl_degree);
            string rights = string.Empty;
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if ((group_user.ToString().Trim() != "") && (group_user.Trim() != "0") && (group_user.ToString().Trim() != "-1"))
                rights = "and group_code='" + group_user + "'";
            else
                rights = " and user_code='" + usercode + "'";
            cb_branch.Checked = false;
            string commname = "";
            cbl_branch.Items.Clear();
            txt_branch.Text = "--Select--";
            string collegeCode = ddl_collegename.SelectedValue;
            if (branch != "")
            {
                commname = "select distinct convert(varchar(20), degree.degree_code)+'$'+convert(varchar(20), degree.No_Of_seats)as degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code " + rights + " and degree.college_code='" + collegeCode + "'";
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
                        cbl_branch.Items[0].Selected = true;
                    txt_branch.Text = lbl_branch.Text + "(" + 1 + ")";
                }
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
        int i = 0;
        ds.Clear();
        string branch = "";
        string build = "";
        string build1 = "";
        string batch = "";
        string collegeCode = ddl_collegename.SelectedValue;
        if (cbl_branch.Items.Count > 0)
        {
            for (i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    build = cbl_branch.Items[i].Value.ToString().Split('$')[0];
                    if (branch == "")
                        branch = build;
                    else
                        branch = branch + "," + build;
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
                        batch = build1;
                    else
                        batch = batch + "," + build1;
                }
            }
        }
        if (branch.Trim() != "" && batch.Trim() != "")
        {
            string strsql1 = "select distinct duration,first_year_nonsemester  from degree where degree_code in (" + branch + ")  and college_code=" + collegeCode + " order by Duration desc";//
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
            int i = 0;
            string branch = string.Empty;
            string batch = rs.GetSelectedItemsValueAsString(cbl_batch);
            if (cbl_branch.Items.Count > 0)
            {
                for (i = 0; i < cbl_branch.Items.Count; i++)
                {
                    if (cbl_branch.Items[i].Selected == true)
                    {
                        string build = cbl_branch.Items[i].Value.ToString().Split('$')[0];
                        if (branch == "")
                            branch = build;
                        else
                            branch = branch + "','" + build;
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

    public void bindbatch1()
    {
        try
        {
            hat.Clear();
            cbl_batch1.Items.Clear();
            ds.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_batch1.DataSource = ds;
                cbl_batch1.DataTextField = "batch_year";
                cbl_batch1.DataValueField = "batch_year";
                cbl_batch1.DataBind();
                if (cbl_batch1.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_batch1.Items.Count; i++)
                    {
                        cbl_batch1.Items[0].Selected = true;
                    }
                    txt_batch1.Text = "Batch(" + 1 + ")";
                }
                else
                {
                    txt_batch1.Text = "--Select--";
                    cb_batch1.Checked = false;
                }
            }
        }
        catch
        {
        }
    }

    public void degree1()
    {
        try
        {
            string query = "";
            string rights = "";
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if ((group_user.ToString().Trim() != "") && (group_user.Trim() != "0") && (group_user.ToString().Trim() != "-1"))
                rights = "and group_code='" + group_user + "'";
            else
                rights = " and user_code='" + usercode + "'";
            string collegeCode = ddl_collegename.SelectedValue;
            query = "select distinct d.Course_Id,c.Course_Name from Degree d,course c ,DeptPrivilages p where p.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.college_code=c.college_code   " + rights + " and d.college_code='" + collegeCode + "'";//

            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                cbl_degree1.DataSource = ds;
                cbl_degree1.DataTextField = "course_name";
                cbl_degree1.DataValueField = "course_id";
                cbl_degree1.DataBind();
                if (cbl_degree1.Items.Count > 0)
                {
                    cbl_degree1.Items[0].Selected = true;
                }
                txt_degree1.Text = lbl_degree.Text + "(" + 1 + ")";
                bindbranch1();
            }
            else
            {
                txt_degree1.Text = "--Select--";
                cb_degree1.Checked = false;
                cbl_degree1.Items.Clear();
                txt_branch1.Text = "--Select--";
                cb_branch1.Checked = false;
                cbl_branch1.Items.Clear();
                cb_sem1.Checked = false;
                txt_sem1.Text = "--Select--";
                cbl_sem1.Items.Clear();
                cb_sec1.Checked = false;
                txt_sec1.Text = "--Select--";
                cbl_sec1.Items.Clear();
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindbranch1()
    {
        try
        {
            string branch = rs.GetSelectedItemsValueAsString(cbl_degree1);
            string rights = string.Empty;
            string collegeCode = ddl_collegename.SelectedValue;
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if ((group_user.ToString().Trim() != "") && (group_user.Trim() != "0") && (group_user.ToString().Trim() != "-1"))
                rights = "and group_code='" + group_user + "'";
            else
                rights = " and user_code='" + usercode + "'";
            cb_branch1.Checked = false;
            string commname = "";
            cbl_branch1.Items.Clear();
            txt_branch1.Text = "--Select--";
            if (branch != "")
            {
                commname = "select distinct convert(varchar(20), degree.degree_code)+'$'+convert(varchar(20), degree.No_Of_seats)as degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code " + rights + " and  degree.college_code='" + collegeCode + "' ";//
                ds.Clear();
                ds = d2.select_method(commname, hat, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch1.DataSource = ds;
                    cbl_branch1.DataTextField = "dept_name";
                    cbl_branch1.DataValueField = "degree_code";
                    cbl_branch1.DataBind();
                    if (cbl_branch1.Items.Count > 0)
                        cbl_branch1.Items[0].Selected = true;
                    txt_branch1.Text = lbl_branch.Text + "(" + 1 + ")";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindsem1()
    {
        cbl_sem1.Items.Clear();
        txt_sem1.Text = "--Select--";
        int i = 0;
        ds.Clear();
        string branch = "";
        string build = "";
        string build1 = "";
        string batch = "";
        string collegeCode = ddl_collegename.SelectedValue;
        if (cbl_branch1.Items.Count > 0)
        {
            for (i = 0; i < cbl_branch1.Items.Count; i++)
            {
                if (cbl_branch1.Items[i].Selected == true)
                {
                    build = cbl_branch1.Items[i].Value.ToString().Split('$')[0];
                    if (branch == "")
                        branch = build;
                    else
                        branch = branch + "," + build;
                }
            }
        }
        if (cbl_batch1.Items.Count > 0)
        {
            for (i = 0; i < cbl_batch1.Items.Count; i++)
            {
                if (cbl_batch1.Items[i].Selected == true)
                {
                    build1 = cbl_batch1.Items[i].Value.ToString();
                    if (batch == "")
                        batch = build1;
                    else
                        batch = batch + "," + build1;
                }
            }
        }
        if (branch.Trim() != "" && batch.Trim() != "")
        {
            string strsql1 = "select distinct duration,first_year_nonsemester  from degree where degree_code in (" + branch + ") and college_code=" + collegeCode + " order by Duration desc";//
            ds = d2.select_method_wo_parameter(strsql1, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                int dur = 0;
                int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]), out dur);
                for (i = 1; i <= dur; i++)
                {
                    cbl_sem1.Items.Add(Convert.ToString(i));
                    cbl_sem1.Items[i - 1].Selected = true;
                    cb_sem1.Checked = true;
                }
                txt_sem1.Text = lbl_org_sem.Text + "(" + cbl_sem1.Items.Count + ")";
            }
        }
    }

    public void BindSectionDetail1()
    {
        try
        {
            cbl_sec1.Items.Clear();
            int i = 0;
            string branch = string.Empty;
            string batch = rs.GetSelectedItemsValueAsString(cbl_batch1);
            if (cbl_branch1.Items.Count > 0)
            {
                for (i = 0; i < cbl_branch1.Items.Count; i++)
                {
                    if (cbl_branch1.Items[i].Selected == true)
                    {
                        string build = cbl_branch1.Items[i].Value.ToString().Split('$')[0];
                        if (branch == "")
                            branch = build;
                        else
                            branch = branch + "','" + build;
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
                    cbl_sec1.DataSource = ds;
                    cbl_sec1.DataTextField = "sections";
                    cbl_sec1.DataValueField = "sections";
                    cbl_sec1.DataBind();
                    if (cbl_sem1.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_sec1.Items.Count; row++)
                        {
                            cbl_sec1.Items[row].Selected = true;
                            cb_sec1.Checked = true;
                        }
                        txt_sec1.Text = "Section(" + cbl_sec1.Items.Count + ")";
                    }
                    else
                    {
                        txt_sec1.Text = "--Select--";
                    }
                }
                else
                {
                    txt_sec1.Text = "--Select--";
                }
            }
        }
        catch
        {
        }
    }

    #endregion

    protected void lb2_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }
        catch (Exception ex)
        {
        }
    }

    protected void cbl_hostelname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_hostelname.Checked = false;
            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    // txt_hostelname.Text = "--Select--";
                    //  cb_hostelname.Checked = false;
                    cb_buildingname.Checked = true;
                    build = cbl_hostelname.Items[i].Text.ToString();
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
            clgbuild(buildvalue);
            //clgfloorpop(buildvalue);
            if (seatcount == cbl_hostelname.Items.Count)
            {
                txt_hostelname.Text = "Hostel Name(" + seatcount + ")";
                cb_hostelname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_hostelname.Text = "--Select--";
                cbl_buildingname.Items.Clear();
                cb_buildingname.Checked = false;
                txt_buildingname.Text = "--Select--";
                cbl_floorname.Items.Clear();
                cb_floorname.Checked = false;
                txt_floorname.Text = "--Select--";
                txt_roomname.Text = "--Select--";
                cb_roomname.Checked = false;
                cbl_roomname.Items.Clear();
            }
            else
            {
                txt_hostelname.Text = "Hostel Name(" + seatcount + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void cb_hostelname_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_hostelname.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    if (cb_hostelname.Checked == true)
                    {
                        cbl_hostelname.Items[i].Selected = true;
                        txt_hostelname.Text = "Hostel Name(" + (cbl_hostelname.Items.Count) + ")";
                        //txt_floorname.Text = "--Select--";
                        build1 = cbl_hostelname.Items[i].Text.ToString();
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
                clgbuild(buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    cbl_hostelname.Items[i].Selected = false;
                    txt_hostelname.Text = "--Select--";
                    cbl_buildingname.Items.Clear();
                    cb_buildingname.Checked = false;
                    txt_buildingname.Text = "--Select--";
                    cbl_floorname.Items.Clear();
                    cb_floorname.Checked = false;
                    txt_floorname.Text = "--Select--";
                    txt_roomname.Text = "--Select--";
                    cb_roomname.Checked = false;
                    cbl_roomname.Items.Clear();
                }
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    protected void chkhstlname_checkedchange(object sender, EventArgs e)
    {
    }

    protected void chklsthstlname_Change(object sender, EventArgs e)
    {
    }

    public void clgbuild(string build)
    {
        try
        {
            cbl_buildingname.Items.Clear();
            string bul = "";
            if (cbl_hostelname.Items.Count > 0)
            {
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    if (cbl_hostelname.Items[i].Selected == true)
                    {
                        if (bul == "")
                        {
                            bul = Convert.ToString(cbl_hostelname.Items[i].Value);
                        }
                        else
                        {
                            bul = bul + "'" + "," + "'" + Convert.ToString(cbl_hostelname.Items[i].Value);
                        }
                    }
                }
            }
            build = d2.GetBuildingCode_inv(bul);
            ds = d2.BindBuilding(build);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_buildingname.DataSource = ds;
                cbl_buildingname.DataTextField = "Building_Name";
                cbl_buildingname.DataValueField = "code";
                cbl_buildingname.DataBind();
            }
            for (int i = 0; i < cbl_buildingname.Items.Count; i++)
            {
                cbl_buildingname.Items[i].Selected = true;
                txt_buildingname.Text = "Building(" + (cbl_buildingname.Items.Count) + ")";
                cb_buildingname.Checked = true;
            }
            string locbuild = "";
            for (int i = 0; i < cbl_buildingname.Items.Count; i++)
            {
                if (cbl_buildingname.Items[i].Selected == true)
                {
                    string builname = cbl_buildingname.Items[i].Text;
                    if (locbuild == "")
                    {
                        locbuild = builname;
                    }
                    else
                    {
                        locbuild = locbuild + "'" + "," + "'" + builname;
                    }
                }
            }
            clgfloor(locbuild);
        }
        catch (Exception ex)
        {
        }
    }

    protected void chkbuildname_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_buildingname.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                for (int i = 0; i < cbl_buildingname.Items.Count; i++)
                {
                    if (cb_buildingname.Checked == true)
                    {
                        cbl_buildingname.Items[i].Selected = true;
                        txt_buildingname.Text = "Building(" + (cbl_buildingname.Items.Count) + ")";
                        //txt_floorname.Text = "--Select--";
                        build1 = cbl_buildingname.Items[i].Text.ToString();
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
                clgfloor(buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_buildingname.Items.Count; i++)
                {
                    cbl_buildingname.Items[i].Selected = false;
                    txt_buildingname.Text = "--Select--";
                    cbl_floorname.Items.Clear();
                    cb_floorname.Checked = false;
                    txt_floorname.Text = "--Select--";
                    txt_roomname.Text = "--Select--";
                    cb_roomname.Checked = false;
                    cbl_roomname.Items.Clear();
                }
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    protected void chklstbuildname_Change(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_buildingname.Checked = false;
            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_buildingname.Items.Count; i++)
            {
                if (cbl_buildingname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    // txt_floorname.Text = "--Select--";
                    cb_floorname.Checked = true;
                    build = cbl_buildingname.Items[i].Text.ToString();
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
            clgfloor(buildvalue);
            if (seatcount == cbl_buildingname.Items.Count)
            {
                txt_buildingname.Text = "Building(" + seatcount + ")";
                cb_buildingname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_buildingname.Text = "--Select--";
            }
            else
            {
                txt_buildingname.Text = "Building(" + seatcount + ")";
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    public void clgfloor(string buildname)
    {
        try
        {
            //chklstfloorpo3.Items.Clear();
            cbl_floorname.Items.Clear();
            ds = d2.BindFloor(buildname);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_floorname.DataSource = ds;
                cbl_floorname.DataTextField = "Floor_Name";
                cbl_floorname.DataValueField = "Floorpk";
                cbl_floorname.DataBind();
                cbl_floorname1.DataSource = ds;
                cbl_floorname1.DataTextField = "Floor_Name";
                cbl_floorname1.DataValueField = "Floorpk";
                cbl_floorname1.DataBind();
            }
            else
            {
                txt_floorname.Text = "--Select--";
            }
            //for selected floor
            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                cbl_floorname.Items[i].Selected = true;
                cb_floorname.Checked = true;
            }
            string locfloor = "";
            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                if (cbl_floorname.Items[i].Selected == true)
                {
                    txt_floorname.Text = "Floor(" + (cbl_floorname.Items.Count) + ")";
                    string flrname = cbl_floorname.Items[i].Text; //cbl_floorname.SelectedItem.Text; 
                    if (locfloor == "")
                    {
                        locfloor = flrname;
                    }
                    else
                    {
                        locfloor = locfloor + "'" + "," + "'" + flrname;
                    }
                }
            }
            clgroom(locfloor, buildname);
        }
        catch (Exception ex)
        {
        }
    }

    protected void chkflrname_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_floorname.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                string build2 = "";
                string buildvalue2 = "";
                if (cb_buildingname.Checked == true)
                {
                    for (int i = 0; i < cbl_buildingname.Items.Count; i++)
                    {
                        build1 = cbl_buildingname.Items[i].Text.ToString();
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
                if (cb_floorname.Checked == true)
                {
                    for (int j = 0; j < cbl_floorname.Items.Count; j++)
                    {
                        cbl_floorname.Items[j].Selected = true;
                        txt_floorname.Text = "Floor(" + (cbl_floorname.Items.Count) + ")";
                        build2 = cbl_floorname.Items[j].Text.ToString();
                        if (buildvalue2 == "")
                        {
                            buildvalue2 = build2;
                        }
                        else
                        {
                            buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
                        }
                    }
                }
                clgroom(buildvalue2, buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_floorname.Items.Count; i++)
                {
                    cbl_floorname.Items[i].Selected = false;
                    txt_floorname.Text = "--Select--";
                }
                cb_roomname.Checked = false;
                cbl_roomname.Items.Clear();
                txt_roomname.Text = "--Select--";
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    protected void chklstflrname_Change(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_floorname.Checked = false;
            string buildvalue1 = "";
            string build1 = "";
            string build2 = "";
            string buildvalue2 = "";
            for (int i = 0; i < cbl_buildingname.Items.Count; i++)
            {
                if (cbl_buildingname.Items[i].Selected == true)
                {
                    build1 = cbl_buildingname.Items[i].Text.ToString();
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
            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                if (cbl_floorname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    build2 = cbl_floorname.Items[i].Text.ToString();
                    if (buildvalue2 == "")
                    {
                        buildvalue2 = build2;
                    }
                    else
                    {
                        buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
                    }
                }
            }
            clgroom(buildvalue2, buildvalue1);
            if (seatcount == cbl_floorname.Items.Count)
            {
                txt_floorname.Text = "Floor(" + seatcount.ToString() + ")";
                cb_floorname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_floorname.Text = "--Select--";
            }
            else
            {
                txt_floorname.Text = "Floor(" + seatcount.ToString() + ")";
            }
            //   Button2.Focus();
            //  clgroom(buildvalue1, buildvalue2);
        }
        catch (Exception ex)
        {
        }
    }

    public void clgroom(string floorname, string buildname)
    {
        try
        {
            cbl_roomname.Items.Clear();
            cbl_roomname1.Items.Clear();
            ds = d2.BindRoom(floorname, buildname);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_roomname.DataSource = ds;
                cbl_roomname.DataTextField = "Room_Name";
                cbl_roomname.DataValueField = "Roompk";
                cbl_roomname.DataBind();
                cbl_roomname1.DataSource = ds;
                cbl_roomname1.DataTextField = "Room_Name";
                cbl_roomname1.DataValueField = "Roompk";
                cbl_roomname1.DataBind();
            }
            else
            {
                txt_roomname.Text = "--Select--";
            }
            for (int i = 0; i < cbl_roomname.Items.Count; i++)
            {
                cbl_roomname.Items[i].Selected = true;
                txt_roomname.Text = "Room(" + (cbl_roomname.Items.Count) + ")";
                cb_roomname.Checked = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void chkroomname_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_roomname.Checked == true)
            {
                for (int i = 0; i < cbl_roomname.Items.Count; i++)
                {
                    cbl_roomname.Items[i].Selected = true;
                }
                txt_roomname.Text = "Room(" + (cbl_roomname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_roomname.Items.Count; i++)
                {
                    cbl_roomname.Items[i].Selected = false;
                }
                txt_roomname.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void chklstroomname_Change(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_roomname.Checked = false;
            for (int i = 0; i < cbl_roomname.Items.Count; i++)
            {
                if (cbl_roomname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }
            }
            if (seatcount == cbl_roomname.Items.Count)
            {
                txt_roomname.Text = "Room(" + seatcount.ToString() + ")";
                cb_roomname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_roomname.Text = "--Select--";
            }
            else
            {
                txt_roomname.Text = "Room(" + seatcount.ToString() + ")";
            }
            //   Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnQ_Click(object sender, EventArgs e)
    {
        popAddStaff.Visible = true;
        btnflag = 1;
        Fpstaff.Visible = false;
        btn_save2.Visible = false;
        btn_exit2.Visible = false;
        lbl_search3.Visible = false;
        txt_staffname.Text = "";
        rptprint.Visible = false;
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            maindiv.Visible = false;
            btn_delete.Visible = false;
            string floorname = string.Empty;
            DataView dv1 = new DataView();
            string hostelcode = rs.GetSelectedItemsValueAsString(cbl_hostelname);
            string buildname = rs.GetSelectedItemsValueAsString(cbl_buildingname);
            string locfloorname = rs.GetSelectedItemsValueAsString(cbl_floorname);
            string locroomtype = rs.GetSelectedItemsValueAsString(cbl_roomname);
            string batchYear = rs.GetSelectedItemsValueAsString(cbl_batch);
            string DegreeCode = string.Empty;// rs.GetSelectedItemsValueAsString(cbl_branch);
            string CurrentSem = rs.GetSelectedItemsValueAsString(cbl_sem);
            string Section = rs.GetSelectedItemsValueAsString(cbl_sec);

            if (cbl_branch.Items.Count > 0)
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    if (cbl_branch.Items[i].Selected == true)
                    {
                        build = cbl_branch.Items[i].Value.ToString().Split('$')[0];
                        if (DegreeCode == "")
                            DegreeCode = build;
                        else
                            DegreeCode = DegreeCode + "','" + build;
                    }
                }
            }
            if (!string.IsNullOrEmpty(Section))
            {
                Section += "','";
            }
            if (ItemList.Count == 0)
            {
                ItemList.Add("Roll_No");
                ItemList.Add("Reg_No");
                ItemList.Add("Stud_Name");
                ItemList.Add("Staff_Name");
            }
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            Hashtable columnhash = new Hashtable();
            columnhash.Add("Roll_No", "Roll No");
            columnhash.Add("Reg_No", "Reg No");
            columnhash.Add("Stud_Name", "Student Name");
            columnhash.Add("Staff_Name", "Staff Name");
            columnhash.Add("Degree", "Degree");
            if (rdb_hostel.Checked)
            {
                columnhash.Add("HostelName", "Hostel Name");
                columnhash.Add("BuildingFK", "Building Name");
                columnhash.Add("FloorFK", "Floor Name");
                columnhash.Add("RoomFK", "Room Name");
            }
            else
            {
                ItemList.Remove("HostelName");
                ItemList.Remove("BuildingFK");
                ItemList.Remove("FloorFK");
                ItemList.Remove("RoomFK");
            }
            //columnhash.Add("Room_Type", "Room Type");
            string sql = string.Empty;
            string applid = string.Empty;

            string tutorType = string.Empty;
            if (rdb_hostel.Checked)
                tutorType = "1";
            else if (rdb_allstudent.Checked)
                tutorType = "2";
            #region hosteler
            if (rdb_hostel.Checked)
            {
                #region hostler
                if (hostelcode.Trim() != "" && buildname.Trim() != "" && locfloorname.Trim() != "" && locroomtype.Trim() != "")
                {
                    if (txt_staffname.Text != "")
                    {
                        string staff_code = d2.GetFunction("select Staff_Code from staffmaster where Staff_Name='" + txt_staffname.Text + "'");
                        applid = d2.GetFunction("select appl_id  from staff_appl_master sam,staffmaster sm where  sm.staff_code='" + staff_code + "' and sam.appl_no = sm.appl_no");
                        sql = "select st.APP_No,r.Roll_No,r.Reg_No,r.Stud_Name,sm.Staff_Name,h.HostelName,hs.BuildingFK,hs.FloorFK,hs.RoomFK,c.Course_Name+'-'+dt.Dept_Name as Degree from HM_HostelMaster h,HT_HostelRegistration hs,Registration r,staff_appl_master a,CO_StudentTutor as st,staffmaster as sm  ,Degree d,Department dt,Course c where dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and d.Degree_Code=r.degree_code and h.HostelMasterPK =hs.HostelMasterFK and a.appl_no=sm.appl_no and a.appl_id=st.StaffMasterFK   and r.App_No =st.App_No and hs.APP_No =r.App_No  and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and h.HostelMasterPK in('" + hostelcode + "') and hs.BuildingFK in ('" + buildname + "') and hs.FloorFK in ('" + locfloorname + "') and hs.RoomFK in ('" + locroomtype + "') and st.StaffMasterFK ='" + applid + "' and st.Tutorfor='" + tutorType + "'";
                    }
                    else if (txt_studname.Text.Trim() != "")
                    {
                        sql = "select st.APP_No,r.Roll_No,r.Reg_No,r.Stud_Name,sm.Staff_Name,h.HostelName,hs.BuildingFK,hs.FloorFK,hs.RoomFK,c.Course_Name+'-'+dt.Dept_Name as Degree from HM_HostelMaster h,HT_HostelRegistration hs,Registration r,staff_appl_master a,CO_StudentTutor as st,staffmaster as sm  ,Degree d,Department dt,Course c where dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and d.Degree_Code=r.degree_code and h.HostelMasterPK =hs.HostelMasterFK and a.appl_no=sm.appl_no and a.appl_id=st.StaffMasterFK   and r.App_No =st.App_No and hs.APP_No =r.App_No  and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and h.HostelMasterPK in('" + hostelcode + "') and hs.BuildingFK in ('" + buildname + "') and hs.FloorFK in ('" + locfloorname + "') and hs.RoomFK in ('" + locroomtype + "') and r.Stud_Name='" + Convert.ToString(txt_studname.Text) + "'  and st.Tutorfor='" + tutorType + "'";
                    }
                    else
                    {
                        sql = "select st.APP_No,r.Roll_No,r.Reg_No,r.Stud_Name,sm.Staff_Name,h.HostelName,hs.BuildingFK,hs.FloorFK,hs.RoomFK,c.Course_Name+'-'+dt.Dept_Name as Degree from HM_HostelMaster h,HT_HostelRegistration hs,Registration r,staff_appl_master a,CO_StudentTutor as st,staffmaster as sm  ,Degree d,Department dt,Course c where dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and d.Degree_Code=r.degree_code and h.HostelMasterPK =hs.HostelMasterFK and a.appl_no=sm.appl_no and a.appl_id=st.StaffMasterFK   and r.App_No =st.App_No and hs.APP_No =r.App_No  and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and h.HostelMasterPK in('" + hostelcode + "') and hs.BuildingFK in ('" + buildname + "') and hs.FloorFK in ('" + locfloorname + "') and hs.RoomFK in ('" + locroomtype + "')  and st.Tutorfor='" + tutorType + "'";
                    }
                    sql = sql + " select Building_Name,Code  from Building_Master";
                    sql = sql + " select Floor_Name,Floorpk  from Floor_Master";
                    sql = sql + " select Room_Name,Roompk from Room_Detail";
                }
                else
                {
                    FpSpread1.Sheets[0].RowCount = 0;
                    FpSpread1.Sheets[0].ColumnCount = 0;
                    FpSpread1.Visible = false;
                    maindiv.Visible = false;
                    btn_delete.Visible = false;
                    rptprint.Visible = false;
                    lbl_error.Text = "Please Select Any One Record";
                    lbl_error.Visible = true;
                    btn_delete.Visible = false;
                    divdcorder.Visible = false;
                }
                #endregion
            }
            if (rdb_allstudent.Checked)
            {
                #region All Student
                //if (hostelcode.Trim() != "" && buildname.Trim() != "" && locfloorname.Trim() != "" && locroomtype.Trim() != "")
                //{
                    if (txt_staffname.Text != "")
                    {
                        string staff_code = d2.GetFunction("select Staff_Code from staffmaster where Staff_Name='" + txt_staffname.Text + "'");
                        applid = d2.GetFunction("select appl_id  from staff_appl_master sam,staffmaster sm where  sm.staff_code='" + staff_code + "' and sam.appl_no = sm.appl_no");
                        sql = " select st.APP_No,r.Roll_No,r.Reg_No,r.Stud_Name,sm.Staff_Name,c.Course_Name+'-'+dt.Dept_Name as Degree from Registration r,staff_appl_master a,CO_StudentTutor as st,staffmaster as sm ,Degree d,Department dt,Course c where dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and d.Degree_Code=r.degree_code and a.appl_no=sm.appl_no and a.appl_id=st.StaffMasterFK   and r.App_No =st.App_No and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and st.StaffMasterFK ='" + applid + "' and st.Tutorfor='" + tutorType + "' and r.Batch_Year in('" + batchYear + "') and r.degree_code in('" + DegreeCode + "') and r.Current_Semester in('" + CurrentSem + "') ";
                    }
                    else if (txt_studname.Text.Trim() != "")
                    {
                        sql = "  select st.APP_No,r.Roll_No,r.Reg_No,r.Stud_Name,sm.Staff_Name,c.Course_Name+'-'+dt.Dept_Name as Degree from Registration r,staff_appl_master a,CO_StudentTutor as st,staffmaster as sm ,Degree d,Department dt,Course c where dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and d.Degree_Code=r.degree_code and a.appl_no=sm.appl_no and a.appl_id=st.StaffMasterFK   and r.App_No =st.App_No and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and st.Tutorfor='" + tutorType + "' and r.Batch_Year in('" + batchYear + "') and r.degree_code in('" + DegreeCode + "') and r.Current_Semester in('" + CurrentSem + "') and r.Stud_Name='" + Convert.ToString(txt_studname.Text) + "'";
                    }
                    else
                    {
                        sql = "  select st.APP_No,r.Roll_No,r.Reg_No,r.Stud_Name,sm.Staff_Name,c.Course_Name+'-'+dt.Dept_Name as Degree from Registration r,staff_appl_master a,CO_StudentTutor as st,staffmaster as sm ,Degree d,Department dt,Course c where dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and d.Degree_Code=r.degree_code and a.appl_no=sm.appl_no and a.appl_id=st.StaffMasterFK   and r.App_No =st.App_No and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and st.Tutorfor='" + tutorType + "' and r.Batch_Year in('" + batchYear + "') and r.degree_code in('" + DegreeCode + "') and r.Current_Semester in('" + CurrentSem + "')";
                    }
                    if (!string.IsNullOrEmpty(Section))
                        sql += " and ISNULL(r.Sections,'') in ('" + Section + "') ";
                //}
                //else
                //{
                //    FpSpread1.Sheets[0].RowCount = 0;
                //    FpSpread1.Sheets[0].ColumnCount = 0;
                //    FpSpread1.Visible = false;
                //    maindiv.Visible = false;
                //    btn_delete.Visible = false;
                //    rptprint.Visible = false;
                //    lbl_error.Text = "Please Select Any One Record";
                //    lbl_error.Visible = true;
                //    btn_delete.Visible = false;
                //    divdcorder.Visible = false;
                //}
                #endregion
            }
            if (!string.IsNullOrEmpty(sql))
            {
                ds.Clear();
                ds = d2.select_method_wo_parameter(sql, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    btn_delete.Visible = true;
                    divdcorder.Visible = true;
                    FpSpread1.Sheets[0].RowCount = 0;
                    FpSpread1.Sheets[0].ColumnCount = 0;
                    FpSpread1.CommandBar.Visible = false;
                    FpSpread1.Sheets[0].AutoPostBack = false;
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread1.Sheets[0].RowHeader.Visible = false;
                    FpSpread1.Sheets[0].ColumnCount = ItemList.Count + 2;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[0].Width = 50;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                    check.AutoPostBack = true;
                    FarPoint.Web.Spread.CheckBoxCellType check1 = new FarPoint.Web.Spread.CheckBoxCellType();
                    check1.AutoPostBack = false;
                    FarPoint.Web.Spread.TextCellType txtcelltype = new FarPoint.Web.Spread.TextCellType();
                    FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    string colno = "";
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        colno = Convert.ToString(ds.Tables[0].Columns[j]);
                        if (ItemList.Contains(Convert.ToString(colno)))
                        {
                            int insdex = ItemList.IndexOf(Convert.ToString(colno));
                            FpSpread1.Columns[insdex + 2].Width = 100;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].Text = Convert.ToString(columnhash[colno]);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].HorizontalAlign = HorizontalAlign.Center;
                            if (colno == "Stud_Name")
                                FpSpread1.Columns[insdex + 2].Width = 200;
                            if (colno == "Staff_Name")
                                FpSpread1.Columns[insdex + 2].Width = 200;
                            if (colno == "Degree")
                                FpSpread1.Columns[insdex + 2].Width = 250;
                        }
                    }
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = check;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = ds.Tables[0].Rows[i]["APP_No"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = check1;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (ItemList.Contains(Convert.ToString(ds.Tables[0].Columns[j].ToString())))
                            {
                                int insdex = ItemList.IndexOf(Convert.ToString(ds.Tables[0].Columns[j].ToString()));
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, insdex + 2].CellType = txtcelltype;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, insdex + 2].Text = ds.Tables[0].Rows[i][j].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, insdex + 2].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, insdex + 2].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, insdex + 2].Locked = true;
                                colno = Convert.ToString(ds.Tables[0].Columns[j]);
                                if (colno.Trim() != "BuildingFK")
                                {
                                }
                                else
                                {
                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        if (Convert.ToString(ds.Tables[0].Rows[i][j]).Trim() != "")
                                        {
                                            ds.Tables[1].DefaultView.RowFilter = "code in (" + Convert.ToString(ds.Tables[0].Rows[i][j]) + ")";
                                            dv1 = ds.Tables[1].DefaultView;
                                            if (dv1.Count > 0)
                                            {
                                                string buildvalue = "";
                                                for (int r = 0; r < dv1.Count; r++)
                                                {
                                                    if (buildvalue == "")
                                                    {
                                                        buildvalue = Convert.ToString(dv1[r]["Building_Name"]);
                                                    }
                                                    else
                                                    {
                                                        buildvalue = buildvalue + "," + Convert.ToString(dv1[r]["Building_Name"]);
                                                    }
                                                }
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, insdex + 2].Text = Convert.ToString(buildvalue);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, insdex + 2].HorizontalAlign = HorizontalAlign.Left;
                                            }
                                        }
                                    }
                                }
                                if (colno.Trim() != "FloorFK")
                                {
                                }
                                else
                                {
                                    if (ds.Tables[2].Rows.Count > 0)
                                    {
                                        if (Convert.ToString(ds.Tables[0].Rows[i][j]).Trim() != "")
                                        {
                                            ds.Tables[2].DefaultView.RowFilter = "Floorpk in (" + Convert.ToString(ds.Tables[0].Rows[i][j]) + ")";
                                            dv1 = ds.Tables[2].DefaultView;
                                            if (dv1.Count > 0)
                                            {
                                                string buildvalue = "";
                                                for (int r = 0; r < dv1.Count; r++)
                                                {
                                                    if (buildvalue == "")
                                                    {
                                                        buildvalue = Convert.ToString(dv1[r]["Floor_Name"]);
                                                    }
                                                    else
                                                    {
                                                        buildvalue = buildvalue + "," + Convert.ToString(dv1[r]["Floor_Name"]);
                                                    }
                                                }
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, insdex + 2].Text = Convert.ToString(buildvalue);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, insdex + 2].HorizontalAlign = HorizontalAlign.Left;
                                            }
                                        }
                                    }
                                }
                                if (colno.Trim() != "RoomFK")
                                {
                                }
                                else
                                {
                                    if (ds.Tables[3].Rows.Count > 0)
                                    {
                                        if (Convert.ToString(ds.Tables[0].Rows[i][j]).Trim() != "")
                                        {
                                            ds.Tables[3].DefaultView.RowFilter = "Roompk in (" + Convert.ToString(ds.Tables[0].Rows[i][j]) + ")";
                                            dv1 = ds.Tables[3].DefaultView;
                                            if (dv1.Count > 0)
                                            {
                                                string buildvalue = "";
                                                for (int r = 0; r < dv1.Count; r++)
                                                {
                                                    if (buildvalue == "")
                                                    {
                                                        buildvalue = Convert.ToString(dv1[r]["Room_Name"]);
                                                    }
                                                    else
                                                    {
                                                        buildvalue = buildvalue + "," + Convert.ToString(dv1[r]["Room_Name"]);
                                                    }
                                                }
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, insdex + 2].Text = Convert.ToString(buildvalue);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, insdex + 2].HorizontalAlign = HorizontalAlign.Left;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    FpSpread1.Visible = true;
                    maindiv.Visible = true;
                    btn_delete.Visible = true;
                    rptprint.Visible = true;
                    lbl_error.Visible = false;
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    txt_studname.Text = "";
                    txt_staffname.Text = "";
                }
                else
                {
                    FpSpread1.Sheets[0].RowCount = 0;
                    FpSpread1.Sheets[0].ColumnCount = 0;
                    FpSpread1.Visible = false;
                    maindiv.Visible = false;
                    btn_delete.Visible = false;
                    rptprint.Visible = false;
                    lbl_error.Text = "No Records Found";
                    lbl_error.Visible = true;
                    btn_delete.Visible = false;
                    divdcorder.Visible = false;
                }
            }
            #endregion
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnaddnew_Click(object sender, EventArgs e)
    {
        TRAllhostelerPop.Visible = false;
        TRAllStudPop.Visible = false;
        if (rdb_hostel.Checked)
            TRAllhostelerPop.Visible = true;
        else if (rdb_allstudent.Checked)
            TRAllStudPop.Visible = true;

        Printcontrol.Visible = false;
        popAddNew.Visible = true;
        btn_save.Visible = false;
        btn_exit.Visible = false;
        txt_staffname1.Text = "";
        Fpspread2.Visible = false;
        rptprint.Visible = false;
        bindhosteladd();
        //  btn_go_Click(sender, e);
        // clgbuildpop(ddl_hostelname1.SelectedItem.Value.ToString());
    }

    protected void delete()
    {
        try
        {
            flag = false;
            surediv.Visible = false;
            string roll_no = string.Empty;
            lbl_erroralert.Text = "";
            string TutorType = string.Empty;
            if (rdb_allstudent.Checked)
                TutorType = "2";
            else
                TutorType = "1";
            for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                FpSpread1.SaveChanges();
                int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 1].Value);
                if (checkval == 1)
                {
                    if (roll_no == "")
                        roll_no = "" + FpSpread1.Sheets[0].Cells[i, 0].Tag + "";
                    else
                        roll_no = roll_no + "'" + "," + "'" + FpSpread1.Sheets[0].Cells[i, 0].Tag + "";
                }
            }
            if (!string.IsNullOrEmpty(roll_no))
            {
                string[] separators = { ",", "'" };
                string[] rno = roll_no.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                string sql = string.Empty;
                string applid = string.Empty;
                int insert = 0;
                for (int j = 0; j < rno.Length; j++)
                {
                    if (txt_staffname.Text != "")
                    {
                        string staff_code = d2.GetFunction("select Staff_Code from staffmaster where Staff_Name='" + txt_staffname.Text + "'");
                        applid = d2.GetFunction("select appl_id  from staff_appl_master sam,staffmaster sm where  sm.staff_code='" + staff_code + "' and sam.appl_no = sm.appl_no");
                        sql = "DELETE FROM CO_StudentTutor WHERE App_No='" + rno[j] + "'  AND StaffMasterFK ='" + applid + "' and TutorFor='" + TutorType + "'";
                    }
                    else if (txt_staffname.Text == "")
                        sql = "DELETE FROM CO_StudentTutor WHERE App_No='" + rno[j] + "' and TutorFor='" + TutorType + "'";
                    if (!string.IsNullOrEmpty(sql))
                        insert = d2.update_method_wo_parameter(sql, "TEXT");
                    if (insert != 0)
                        flag = true;
                }
            }
            if (flag == true)
            {
                surediv.Visible = false;
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "Deleted Successfully";
                btn_go_Click(sender, e);
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "Please Select Any Record";
                btn_go_Click(sender, e);
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btn_delete_Click(object sender, EventArgs e)
    {
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

    protected void ddl_stud_staff_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_stud_staff.SelectedValue == "0")
        {
            txt_studname.Visible = true;
            txt_staffname.Visible = false;
            txt_studname.Text = "";
            txt_staffname.Text = "";
        }
        else if (ddl_stud_staff.SelectedValue == "1")
        {
            txt_staffname.Visible = true;
            txt_studname.Visible = false;
            txt_studname.Text = "";
            txt_staffname.Text = "";
        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getstud_Name(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select Stud_Name from CO_StudentTutor co,Registration r where co.App_No=r.App_No and r.Stud_Name like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            if (btn_save.Text == "Save")
            {
                int checkvalall = Convert.ToInt32(Fpspread2.Sheets[0].Cells[0, 1].Value);
                if (checkvalall == 1)
                {
                    savediv.Visible = true;
                    lbl_saveconfirm.Text = "Mentor is alloted for all student";
                }
                else
                {
                    savedall();
                }
            }
            else
            {
            }
        }
        catch
        {
        }
    }

    protected void btn_saveconfirm_Click(object sender, EventArgs e)
    {
        savedall();
    }

    protected void btn_savenotconfirm_Click(object sender, EventArgs e)
    {
        savediv.Visible = false;
    }

    protected void btn_excel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txt_excelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
                lbl_validation.Visible = false;
            }
            else
            {
                lbl_validation.Text = "Please Enter Your Report Name";
                lbl_validation.Visible = true;
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
            string degreedetails = "Student Mentor Report";
            string pagename = "CO_StudentTutor.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }
    }

    //column order
    protected void cb_columnorder_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_columnorder.Checked == true)
            {
                txt_columnorder.Text = "";
                ItemList.Clear();
                for (int i = 0; i < cbl_columnorder.Items.Count; i++)
                {
                    string si = Convert.ToString(i);
                    cbl_columnorder.Items[i].Selected = true;
                    lnk_columnorder.Visible = true;
                    ItemList.Add(cbl_columnorder.Items[i].Value.ToString());
                    Itemindex.Add(si);
                }
                lnk_columnorder.Visible = true;
                txt_columnorder.Visible = true;
                txt_columnorder.Text = "";
                int j = 0;
                string colname12 = "";
                for (int i = 0; i < ItemList.Count; i++)
                {
                    j = j + 1;
                    if (colname12 == "")
                    {
                        colname12 = ItemList[i].ToString() + "(" + (j).ToString() + ")";
                    }
                    else
                    {
                        colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (j).ToString() + ")";
                    }
                }
                txt_columnorder.Text = colname12;
            }
            else
            {
                for (int i = 0; i < cbl_columnorder.Items.Count; i++)
                {
                    cbl_columnorder.Items[i].Selected = false;
                    lnk_columnorder.Visible = false;
                    ItemList.Clear();
                    Itemindex.Clear();
                }
                txt_columnorder.Text = "";
                txt_columnorder.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void lnk_columnorder_Click(object sender, EventArgs e)
    {
        try
        {
            cbl_columnorder.ClearSelection();
            cb_columnorder.Checked = false;
            lnk_columnorder.Visible = false;
            ItemList.Clear();
            Itemindex.Clear();
            // txt_order.Text = "";
            // txt_order.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void cbl_columnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cb_columnorder.Checked = false;
            string value = "";
            int index;
            cbl_columnorder.Items[0].Selected = true;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cbl_columnorder.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    ItemList.Add(cbl_columnorder.Items[index].Value.ToString());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(cbl_columnorder.Items[index].Value.ToString());
                Itemindex.Remove(sindex);
            }
            for (int i = 0; i < cbl_columnorder.Items.Count; i++)
            {
                if (cbl_columnorder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(cbl_columnorder.Items[i].Value.ToString());
                    Itemindex.Remove(sindex);
                }
            }
            lnk_columnorder.Visible = true;
            txt_columnorder.Visible = true;
            txt_columnorder.Text = "";
            string colname12 = "";
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (colname12 == "")
                {
                    colname12 = ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";
                }
                else
                {
                    colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";
                }
            }
            txt_columnorder.Text = colname12;
            if (ItemList.Count == 14)
            {
                cb_columnorder.Checked = true;
            }
            if (ItemList.Count == 0)
            {
                txt_columnorder.Visible = false;
                lnk_columnorder.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }

    // popAddNew


    //protected void ddl2HostelName_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        cbl_building1.Items.Clear();
    //        for (int i = 0; i < cbl_building1.Items.Count; i++)
    //        {
    //            cbl_building1.Items[i].Selected = true;
    //        }
    //        clgbuildpop(ddl_hostelname1.SelectedItem.Value.ToString());
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //public void degree()
    //{
    //    try
    //    {
    //        user_code = Session["usercode"].ToString();
    //        college_code = Session["collegecode"].ToString();
    //        singleuser = Session["single_user"].ToString();
    //        group_user = Session["group_code"].ToString();
    //        if (group_user.Contains(';'))
    //        {
    //            string[] group_semi = group_user.Split(';');
    //            group_user = group_semi[0].ToString();
    //        }
    //        hat.Clear();
    //        hat.Add("single_user", singleuser.ToString());
    //        hat.Add("group_code", group_user);
    //        hat.Add("college_code", college_code);
    //        hat.Add("user_code", user_code);
    //        ds = d2.select_method("bind_degree", hat, "sp");
    //        int count1 = ds.Tables[0].Rows.Count;
    //        if (count1 > 0)
    //        {
    //            //ddl3Dept.DataSource = ds;
    //            //ddl3Dept.DataTextField = "course_name";
    //            //ddl3Dept.DataValueField = "course_id";
    //            //ddl3Dept.DataBind();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //} 

    protected void ddl_deptname2_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_searchbyname.Text = "";
        txt_searchbycode.Text = "";
        FpSpread1.Visible = false;
        maindiv.Visible = false;
        btn_delete.Visible = false;
        Fpspread2.Visible = false;
        Fpstaff.Visible = false;
        lbl_search3.Visible = false;
        btn_save2.Visible = false;
        btn_exit2.Visible = false;
    }

    public void cb_hostelnameadd_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_hostelnameadd.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                for (int i = 0; i < cbl_hostelnameadd.Items.Count; i++)
                {
                    if (cb_hostelnameadd.Checked == true)
                    {
                        cbl_hostelnameadd.Items[i].Selected = true;
                        txt_hostelnameadd.Text = "Hostel Name(" + (cbl_hostelnameadd.Items.Count) + ")";
                        //txt_floorname.Text = "--Select--";
                        build1 = cbl_hostelnameadd.Items[i].Text.ToString();
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
                clgbuildpop(buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_hostelnameadd.Items.Count; i++)
                {
                    cbl_hostelnameadd.Items[i].Selected = false;
                    txt_hostelnameadd.Text = "--Select--";
                    cbl_building1.Items.Clear();
                    cb_building1.Checked = false;
                    txtbuildingpop1.Text = "--Select--";
                    cbl_floorname1.Items.Clear();
                    cb_floorname1.Checked = false;
                    txt_floorname1.Text = "--Select--";
                    txt_roomname1.Text = "--Select--";
                    cb_roomname1.Checked = false;
                    cbl_roomname1.Items.Clear();
                }
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    public void cbl_hostelnameadd_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_hostelnameadd.Checked = false;
            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_hostelnameadd.Items.Count; i++)
            {
                if (cbl_hostelnameadd.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    // txt_hostelname.Text = "--Select--";
                    //  cb_hostelname.Checked = false;
                    cb_building1.Checked = true;
                    build = cbl_hostelnameadd.Items[i].Text.ToString();
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
            clgbuildpop(buildvalue);
            //clgfloorpop(buildvalue);
            if (seatcount == cbl_hostelnameadd.Items.Count)
            {
                txt_hostelnameadd.Text = "Hostel Name(" + seatcount + ")";
                cb_hostelnameadd.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_hostelnameadd.Text = "--Select--";
                cbl_building1.Items.Clear();
                cb_building1.Checked = false;
                txtbuildingpop1.Text = "--Select--";
                cbl_floorname1.Items.Clear();
                cb_floorname1.Checked = false;
                txt_floorname1.Text = "--Select--";
                txt_roomname1.Text = "--Select--";
                cb_roomname1.Checked = false;
                cbl_roomname1.Items.Clear();
            }
            else
            {
                txt_hostelnameadd.Text = "Hostel Name(" + seatcount + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void clgbuildpop(string build)
    {
        try
        {
            cbl_building1.Items.Clear();
            string bul = "";
            if (cbl_hostelnameadd.Items.Count > 0)
            {
                for (int i = 0; i < cbl_hostelnameadd.Items.Count; i++)
                {
                    if (cbl_hostelnameadd.Items[i].Selected == true)
                    {
                        if (bul == "")
                        {
                            bul = Convert.ToString(cbl_hostelnameadd.Items[i].Value);
                        }
                        else
                        {
                            bul = bul + "'" + "," + "'" + Convert.ToString(cbl_hostelnameadd.Items[i].Value);
                        }
                    }
                }
            }
            build = d2.GetBuildingCode_inv(bul);
            ds = d2.BindBuilding(build);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_building1.DataSource = ds;
                cbl_building1.DataTextField = "Building_Name";
                cbl_building1.DataValueField = "code";
                cbl_building1.DataBind();
            }
            for (int i = 0; i < cbl_building1.Items.Count; i++)
            {
                cbl_building1.Items[i].Selected = true;
                txtbuildingpop1.Text = "Building(" + (cbl_building1.Items.Count) + ")";
                cb_building1.Checked = true;
            }
            //string locbuild = "";
            //for (int i = 0; i < cbl_building1.Items.Count; i++)
            //{
            //    if (cbl_building1.Items[i].Selected == true)
            //    {
            //        string builname = cbl_building1.SelectedItem.Text;
            //        if (locbuild == "")
            //        {
            //            locbuild = builname;
            //        }
            //        else
            //        {
            //            locbuild = locbuild + "," + builname;
            //        }
            //    }
            //}
            //clgfloorpop(locbuild);
            string locbuild = "";
            for (int i = 0; i < cbl_building1.Items.Count; i++)
            {
                if (cbl_building1.Items[i].Selected == true)
                {
                    string builname = cbl_building1.Items[i].Text;
                    if (locbuild == "")
                    {
                        locbuild = builname;
                    }
                    else
                    {
                        locbuild = locbuild + "'" + "," + "'" + builname;
                    }
                }
            }
            clgfloorpop(locbuild);
        }
        catch (Exception ex)
        {
        }
    }

    protected void chkbuildpop1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_building1.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                for (int i = 0; i < cbl_building1.Items.Count; i++)
                {
                    if (cb_building1.Checked == true)
                    {
                        cbl_building1.Items[i].Selected = true;
                        txtbuildingpop1.Text = "Building(" + (cbl_building1.Items.Count) + ")";
                        //txt_floorname1.Text = "--Select--";
                        build1 = cbl_building1.Items[i].Text.ToString();
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
                clgfloorpop(buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_building1.Items.Count; i++)
                {
                    cbl_building1.Items[i].Selected = false;
                    txtbuildingpop1.Text = "--Select--";
                    cbl_floorname1.Items.Clear();
                    cb_floorname1.Checked = false;
                    txt_floorname1.Text = "--Select--";
                    cb_roomname1.Checked = false;
                    cbl_roomname1.Items.Clear();
                    txt_roomname1.Text = "--Select--";
                }
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    protected void chklbuildpop1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_building1.Checked = false;
            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_building1.Items.Count; i++)
            {
                if (cbl_building1.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_floorname1.Text = "--Select--";
                    cb_floorname1.Checked = false;
                    build = cbl_building1.Items[i].Text.ToString();
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
            clgfloorpop(buildvalue);
            if (seatcount == cbl_building1.Items.Count)
            {
                txtbuildingpop1.Text = "Building(" + seatcount + ")";
                cb_building1.Checked = true;
            }
            else if (seatcount == 0)
            {
                txtbuildingpop1.Text = "--Select--";
            }
            else
            {
                txtbuildingpop1.Text = "Building(" + seatcount + ")";
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    public void clgfloorpop(string buildname)
    {
        try
        {
            cbl_floorname1.Items.Clear();
            ds = d2.BindFloor(buildname);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_floorname1.DataSource = ds;
                cbl_floorname1.DataTextField = "Floor_Name";
                cbl_floorname1.DataValueField = "Floorpk";
                cbl_floorname1.DataBind();
            }
            else
            {
                txt_floorname1.Text = "--Select--";
            }
            //for selected floor
            for (int i = 0; i < cbl_floorname1.Items.Count; i++)
            {
                cbl_floorname1.Items[i].Selected = true;
                cb_floorname1.Checked = true;
            }
            string locfloor = "";
            for (int i = 0; i < cbl_floorname1.Items.Count; i++)
            {
                if (cbl_floorname1.Items[i].Selected == true)
                {
                    txt_floorname1.Text = "Floor(" + (cbl_floorname1.Items.Count) + ")";
                    string flrname = cbl_floorname1.Items[i].Text; //cbl_floorname.SelectedItem.Text; 
                    if (locfloor == "")
                    {
                        locfloor = flrname;
                    }
                    else
                    {
                        locfloor = locfloor + "'" + "," + "'" + flrname;
                    }
                }
            }
            clgroompop(locfloor, buildname);
        }
        catch (Exception ex)
        {
        }
    }

    protected void chklfloorpop1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_floorname1.Checked = false;
            string buildvalue1 = "";
            string build1 = "";
            string build2 = "";
            string buildvalue2 = "";
            for (int i = 0; i < cbl_building1.Items.Count; i++)
            {
                if (cbl_building1.Items[i].Selected == true)
                {
                    build1 = cbl_building1.Items[i].Text.ToString();
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
            for (int i = 0; i < cbl_floorname1.Items.Count; i++)
            {
                if (cbl_floorname1.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    build2 = cbl_floorname1.Items[i].Text.ToString();
                    if (buildvalue2 == "")
                    {
                        buildvalue2 = build2;
                    }
                    else
                    {
                        buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
                    }
                }
            }
            clgroompop(buildvalue2, buildvalue1);
            if (seatcount == cbl_floorname1.Items.Count)
            {
                txt_floorname1.Text = "Floor(" + seatcount.ToString() + ")";
                cb_floorname1.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_floorname1.Text = "--Select--";
            }
            else
            {
                txt_floorname1.Text = "Floor(" + seatcount.ToString() + ")";
            }
            //   Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    protected void chkfloorpop1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_floorname1.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                string build2 = "";
                string buildvalue2 = "";
                if (cb_building1.Checked == true)
                {
                    for (int i = 0; i < cbl_building1.Items.Count; i++)
                    {
                        build1 = cbl_building1.Items[i].Text.ToString();
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
                if (cb_floorname1.Checked == true)
                {
                    for (int j = 0; j < cbl_floorname1.Items.Count; j++)
                    {
                        cbl_floorname1.Items[j].Selected = true;
                        txt_floorname1.Text = "Floor(" + (cbl_floorname1.Items.Count) + ")";
                        build2 = cbl_floorname1.Items[j].Text.ToString();
                        if (buildvalue2 == "")
                        {
                            buildvalue2 = build2;
                        }
                        else
                        {
                            buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
                        }
                    }
                }
                clgroompop(buildvalue2, buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_floorname1.Items.Count; i++)
                {
                    cbl_floorname1.Items[i].Selected = false;
                    cbl_floorname1.ClearSelection();
                    txt_floorname1.Text = "--Select--";
                    cbl_roomname1.Items.Clear();
                    txt_roomname1.Text = "--Select--";
                }
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    public void clgroompop(string flooor, string buildname)
    {
        try
        {
            cbl_roomname1.Items.Clear();
            string roomname = "";
            ds = d2.BindRoom(flooor, buildname);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_roomname1.DataSource = ds;
                cbl_roomname1.DataTextField = "Room_Name";
                cbl_roomname1.DataValueField = "Roompk";
                cbl_roomname1.DataBind();
            }
            else
            {
                txt_roomname1.Text = "--Select--";
                cbl_roomname1.Items.Clear();
            }
            for (int i = 0; i < cbl_roomname1.Items.Count; i++)
            {
                cbl_roomname1.Items[i].Selected = true;
                txt_roomname1.Text = "Room(" + (cbl_roomname1.Items.Count) + ")";
                cb_roomname1.Checked = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void chklroompop1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_roomname1.Checked = false;
            for (int i = 0; i < cbl_roomname1.Items.Count; i++)
            {
                if (cbl_roomname1.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }
            }
            if (seatcount == cbl_roomname1.Items.Count)
            {
                txt_roomname1.Text = "Room(" + seatcount.ToString() + ")";
                cb_roomname1.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_roomname1.Text = "--Select--";
            }
            else
            {
                txt_roomname1.Text = "Room(" + seatcount.ToString() + ")";
            }
            //   Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    protected void chkroompop1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_roomname1.Checked == true)
            {
                for (int i = 0; i < cbl_roomname1.Items.Count; i++)
                {
                    cbl_roomname1.Items[i].Selected = true;
                }
                txt_roomname1.Text = "Room(" + (cbl_roomname1.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_roomname1.Items.Count; i++)
                {
                    cbl_roomname1.Items[i].Selected = false;
                }
                txt_roomname1.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnAddStaff_Click(object sender, EventArgs e)
    {
        btnflag = 2;
        popAddStaff.Visible = true;
        btn_save2.Visible = false;
        btn_exit2.Visible = false;
        Fpstaff.Visible = false;
        lbl_search3.Visible = false;
    }

    protected void btn_addstaffgo_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_staffname1.Text.Trim() != "")
            {
                string hostelcode = rs.GetSelectedItemsValueAsString(cbl_hostelnameadd);
                string buildname = rs.GetSelectedItemsValueAsString(cbl_building1);
                string locfloorname = rs.GetSelectedItemsValueAsString(cbl_floorname1);
                string locroomtype = rs.GetSelectedItemsValueAsString(cbl_roomname1);

                string batchyear = rs.GetSelectedItemsValueAsString(cbl_batch1);
                string semester = rs.GetSelectedItemsValueAsString(cbl_sem1);
                string section = rs.GetSelectedItemsValueAsString(cbl_sec1);
                string degreecode = string.Empty;
                if (cbl_branch1.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_branch1.Items.Count; i++)
                    {
                        if (cbl_branch1.Items[i].Selected == true)
                        {
                            build = cbl_branch1.Items[i].Value.ToString().Split('$')[0];
                            if (degreecode == "")
                                degreecode = build;
                            else
                                degreecode = degreecode + "','" + build;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(section))
                    section += "','";
                string q = string.Empty;
                if (rdb_hostel.Checked)
                {
                    #region HostelStudent

                    if (txt_hostelnameadd.Text.Trim() != "--Select--" && txtbuildingpop1.Text.Trim() != "--Select--" && txt_floorname1.Text.Trim() != "--Select--" && txt_roomname1.Text.Trim() != "--Select--")
                    {
                        q = "select r.App_No,r.Roll_No,r.Reg_No,r.Stud_Name,h.HostelName,hs.BuildingFK,hs.FloorFK,hs.RoomFK,h.HostelMasterPK  from HM_HostelMaster h,HT_HostelRegistration hs,Registration r where h.HostelMasterPK =hs.HostelMasterFK and hs.APP_No =r.App_No and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and isnull(IsVacated,'0')<>1 and isnull(IsSuspend,'0')<>1 and isnull(IsDiscontinued,'0')<>1  and h.HostelMasterPK in('" + hostelcode + "') and hs.BuildingFK in ('" + buildname + "') and hs.FloorFK in ('" + locfloorname + "') and hs.RoomFK in ('" + locroomtype + "') and r.App_No not in( select App_No  from CO_StudentTutor where HostelMasterPK in('" + hostelcode + "'))";
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
                            else if (strorderby == "1")
                            {
                                strorderby = "ORDER BY r.Reg_No";
                            }
                            else if (strorderby == "2")
                            {
                                strorderby = "ORDER BY r.Stud_Name";
                            }
                            else if (strorderby == "0,1,2")
                            {
                                strorderby = "ORDER BY r.Roll_No,r.Reg_No,r.Stud_Name";
                            }
                            else if (strorderby == "0,1")
                            {
                                strorderby = "ORDER BY r.Roll_No,r.Reg_No";
                            }
                            else if (strorderby == "1,2")
                            {
                                strorderby = "ORDER BY r.Reg_No,r.Stud_Name";
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
                        string query = q + strorderby;
                        query = query + " select Building_Name,Code  from Building_Master";
                        query = query + " select Floor_Name,Floorpk  from Floor_Master";
                        query = query + " select Room_Name,Roompk from Room_Detail";
                        ds = d2.select_method_wo_parameter(query, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Fpspread2.Sheets[0].ColumnCount = 0;
                            Fpspread2.Sheets[0].RowCount = 0;
                            Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
                            Fpspread2.CommandBar.Visible = false;
                            Fpspread2.Sheets[0].ColumnCount = 10;
                            Fpspread2.Sheets[0].AutoPostBack = false;
                            Fpspread2.Sheets[0].RowHeader.Visible = false;
                            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            darkstyle.ForeColor = Color.White;
                            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            Fpspread2.Columns[0].Width = 50;
                            Fpspread2.Columns[0].Locked = true;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                            Fpspread2.Columns[2].Width = 150;
                            Fpspread2.Columns[2].Locked = true;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                            Fpspread2.Columns[3].Width = 150;
                            Fpspread2.Columns[3].Locked = true;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                            Fpspread2.Columns[4].Width = 200;
                            Fpspread2.Columns[4].Locked = true;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Hostel Name";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                            Fpspread2.Columns[5].Width = 100;
                            Fpspread2.Columns[5].Locked = true;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Building Name";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                            Fpspread2.Columns[6].Width = 100;
                            Fpspread2.Columns[6].Locked = true;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Floor Name";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                            Fpspread2.Columns[7].Width = 100;
                            Fpspread2.Columns[7].Locked = true;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Room Name";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                            Fpspread2.Columns[8].Width = 100;
                            Fpspread2.Columns[8].Locked = true;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Room Type";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                            Fpspread2.Columns[9].Width = 100;
                            Fpspread2.Columns[9].Locked = true;
                            Fpspread2.Columns[9].Visible = false;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                            FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                            check.AutoPostBack = true;
                            FarPoint.Web.Spread.CheckBoxCellType check1 = new FarPoint.Web.Spread.CheckBoxCellType();
                            check1.AutoPostBack = false;
                            Fpspread2.Sheets[0].RowCount++;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = check;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            if (buildname != "" && locfloorname != "" && locroomtype != "")
                            {
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    Fpspread2.Sheets[0].RowCount++;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[i]["App_No"]);
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = check1;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Roll_no"]);
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                    FarPoint.Web.Spread.TextCellType txtreg = new FarPoint.Web.Spread.TextCellType();
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].CellType = txtreg;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["HostelName"]);
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[i]["HostelMasterPK"]);
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                    string colno = "";
                                    DataView dv1 = new DataView();
                                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                                    {
                                        colno = Convert.ToString(ds.Tables[0].Columns[j]);
                                        if (colno.Trim() != "BuildingFK")
                                        {
                                        }
                                        else
                                        {
                                            if (ds.Tables[1].Rows.Count > 0)
                                            {
                                                if (Convert.ToString(ds.Tables[0].Rows[i][j]).Trim() != "")
                                                {
                                                    ds.Tables[1].DefaultView.RowFilter = "code in (" + Convert.ToString(ds.Tables[0].Rows[i][j]) + ")";
                                                    dv1 = ds.Tables[1].DefaultView;
                                                    if (dv1.Count > 0)
                                                    {
                                                        string buildvalue = "";
                                                        for (int r = 0; r < dv1.Count; r++)
                                                        {
                                                            if (buildvalue == "")
                                                            {
                                                                buildvalue = Convert.ToString(dv1[r]["Building_Name"]);
                                                            }
                                                            else
                                                            {
                                                                buildvalue = buildvalue + "," + Convert.ToString(dv1[r]["Building_Name"]);
                                                            }
                                                        }
                                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(buildvalue);
                                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                }
                                            }
                                        }
                                        if (colno.Trim() != "FloorFK")
                                        {
                                        }
                                        else
                                        {
                                            if (ds.Tables[2].Rows.Count > 0)
                                            {
                                                if (Convert.ToString(ds.Tables[0].Rows[i][j]).Trim() != "")
                                                {
                                                    ds.Tables[2].DefaultView.RowFilter = "Floorpk in (" + Convert.ToString(ds.Tables[0].Rows[i][j]) + ")";
                                                    dv1 = ds.Tables[2].DefaultView;
                                                    if (dv1.Count > 0)
                                                    {
                                                        string buildvalue = "";
                                                        for (int r = 0; r < dv1.Count; r++)
                                                        {
                                                            if (buildvalue == "")
                                                            {
                                                                buildvalue = Convert.ToString(dv1[r]["Floor_Name"]);
                                                            }
                                                            else
                                                            {
                                                                buildvalue = buildvalue + "," + Convert.ToString(dv1[r]["Floor_Name"]);
                                                            }
                                                        }
                                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(buildvalue);
                                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                }
                                            }
                                        }
                                        if (colno.Trim() != "RoomFK")
                                        {
                                        }
                                        else
                                        {
                                            if (ds.Tables[3].Rows.Count > 0)
                                            {
                                                if (Convert.ToString(ds.Tables[0].Rows[i][j]).Trim() != "")
                                                {
                                                    ds.Tables[3].DefaultView.RowFilter = "Roompk in (" + Convert.ToString(ds.Tables[0].Rows[i][j]) + ")";
                                                    dv1 = ds.Tables[3].DefaultView;
                                                    if (dv1.Count > 0)
                                                    {
                                                        string buildvalue = "";
                                                        for (int r = 0; r < dv1.Count; r++)
                                                        {
                                                            if (buildvalue == "")
                                                            {
                                                                buildvalue = Convert.ToString(dv1[r]["Room_Name"]);
                                                            }
                                                            else
                                                            {
                                                                buildvalue = buildvalue + "," + Convert.ToString(dv1[r]["Room_Name"]);
                                                            }
                                                        }
                                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(buildvalue);
                                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                                if (rollflag1 == "1")
                                {
                                    Fpspread2.Columns[2].Visible = true;
                                }
                                else
                                {
                                    Fpspread2.Columns[2].Visible = false;
                                }
                                if (stuflag1 == "1")
                                {
                                    Fpspread2.Columns[3].Visible = true;
                                }
                                else
                                {
                                    Fpspread2.Columns[3].Visible = false;
                                }
                                Fpspread2.SaveChanges();
                                btn_save.Visible = true;
                                btn_exit.Visible = true;
                                Fpspread2.Visible = true;
                                lbl_error1.Visible = false;
                            }
                        }
                        else
                        {
                            Fpspread2.Visible = false;
                            btn_save.Visible = false;
                            btn_exit.Visible = false;
                            lbl_error1.Visible = true;
                            lbl_error1.Text = "No Records Found";
                        }
                    }
                    else
                    {
                        Fpspread2.Visible = false;
                        btn_save.Visible = false;
                        btn_exit.Visible = false;
                        lbl_error1.Visible = true;
                        lbl_error1.Text = "Please Select All Fields";
                    }
                    #endregion
                }
                if (rdb_allstudent.Checked)
                {
                    #region Allstudent
                    if (txt_batch1.Text.Trim() != "--Select--" && txt_degree1.Text.Trim() != "--Select--" && txt_branch1.Text.Trim() != "--Select--" && txt_sem1.Text.Trim() != "--Select--")
                    {
                        q = " select r.App_No,r.Roll_No,r.Reg_No,r.Stud_Name,r.Batch_Year,c.Course_Name+'-'+dt.Dept_Name as Degree,r.Stud_Type,d.Degree_Code  from Registration r,Degree d,Department dt,Course c where r.degree_code=d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and r.App_No not in( select App_No  from CO_StudentTutor)";
                        q += " and r.degree_code in('" + degreecode + "') and r.Batch_Year in('" + batchyear + "')";
                        if (!string.IsNullOrEmpty(section))
                            q += " and isnull(r.Sections,'') in('" + section + "')";
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
                            else if (strorderby == "1")
                            {
                                strorderby = "ORDER BY r.Reg_No";
                            }
                            else if (strorderby == "2")
                            {
                                strorderby = "ORDER BY r.Stud_Name";
                            }
                            else if (strorderby == "0,1,2")
                            {
                                strorderby = "ORDER BY r.Roll_No,r.Reg_No,r.Stud_Name";
                            }
                            else if (strorderby == "0,1")
                            {
                                strorderby = "ORDER BY r.Roll_No,r.Reg_No";
                            }
                            else if (strorderby == "1,2")
                            {
                                strorderby = "ORDER BY r.Reg_No,r.Stud_Name";
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
                        string query = q + strorderby;
                        ds = d2.select_method_wo_parameter(query, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Fpspread2.Sheets[0].ColumnCount = 0;
                            Fpspread2.Sheets[0].RowCount = 0;
                            Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
                            Fpspread2.CommandBar.Visible = false;
                            Fpspread2.Sheets[0].ColumnCount = 8;
                            Fpspread2.Sheets[0].AutoPostBack = false;
                            Fpspread2.Sheets[0].RowHeader.Visible = false;
                            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            darkstyle.ForeColor = Color.White;
                            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            Fpspread2.Columns[0].Width = 50;
                            Fpspread2.Columns[0].Locked = true;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                            Fpspread2.Columns[2].Width = 150;
                            Fpspread2.Columns[2].Locked = true;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                            Fpspread2.Columns[3].Width = 150;
                            Fpspread2.Columns[3].Locked = true;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                            Fpspread2.Columns[4].Width = 200;
                            Fpspread2.Columns[4].Locked = true;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Batch Year";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                            Fpspread2.Columns[5].Width = 100;
                            Fpspread2.Columns[5].Locked = true;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Degree";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                            Fpspread2.Columns[6].Width = 250;
                            Fpspread2.Columns[6].Locked = true;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Student Type";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                            Fpspread2.Columns[7].Width = 100;
                            Fpspread2.Columns[7].Locked = true;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                            FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                            check.AutoPostBack = true;
                            FarPoint.Web.Spread.CheckBoxCellType check1 = new FarPoint.Web.Spread.CheckBoxCellType();
                            check1.AutoPostBack = false;
                            Fpspread2.Sheets[0].RowCount++;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = check;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            //if (buildname != "" && locfloorname != "" && locroomtype != "")
                            //{
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                Fpspread2.Sheets[0].RowCount++;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[i]["App_No"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = check1;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Roll_no"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                FarPoint.Web.Spread.TextCellType txtreg = new FarPoint.Web.Spread.TextCellType();
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].CellType = txtreg;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["Degree"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Type"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                            }
                            Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                            if (rollflag1 == "1")
                            {
                                Fpspread2.Columns[2].Visible = true;
                            }
                            else
                            {
                                Fpspread2.Columns[2].Visible = false;
                            }
                            if (stuflag1 == "1")
                            {
                                Fpspread2.Columns[3].Visible = true;
                            }
                            else
                            {
                                Fpspread2.Columns[3].Visible = false;
                            }
                            if (Convert.ToString(Session["Studflag"]) == "1")
                                Fpspread2.Columns[7].Visible = true;
                            else
                                Fpspread2.Columns[7].Visible = false;
                            Fpspread2.SaveChanges();
                            btn_save.Visible = true;
                            btn_exit.Visible = true;
                            Fpspread2.Visible = true;
                            lbl_error1.Visible = false;
                            //}
                        }
                        else
                        {
                            Fpspread2.Visible = false;
                            btn_save.Visible = false;
                            btn_exit.Visible = false;
                            lbl_error1.Visible = true;
                            lbl_error1.Text = "No Records Found";
                        }
                    }
                    else
                    {
                        Fpspread2.Visible = false;
                        btn_save.Visible = false;
                        btn_exit.Visible = false;
                        lbl_error1.Visible = true;
                        lbl_error1.Text = "Please Select All Fields";
                    }

                    #endregion
                }



            }
            else
            {
                imgdiv2.Visible = true;
                lbl_error1.Visible = true;
                lbl_erroralert.Text = "Select the Staff Name ?";
                Fpspread2.Visible = false;
                btn_save.Visible = false;
                btn_exit.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void savedall()
    {
        try
        {
            string dtaccessdate = DateTime.Now.ToString("MM/dd/yyyy");
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            string roll_no = string.Empty;
            string applid = string.Empty;
            string tutorType = "1";
            if (rdb_allstudent.Checked)
                tutorType = "2";

            for (int i = 1; i < Fpspread2.Sheets[0].RowCount; i++)
            {
                Fpspread2.SaveChanges();
                int checkval = Convert.ToInt32(Fpspread2.Sheets[0].Cells[i, 1].Value);
                if (checkval == 1)
                {
                    if (roll_no == "")
                    {
                        roll_no = "" + Fpspread2.Sheets[0].Cells[i, 0].Tag + "";
                    }
                    else
                    {
                        roll_no = roll_no + "'" + "," + "'" + Fpspread2.Sheets[0].Cells[i, 0].Tag + "";
                    }
                    string[] separators = { ",", "'" };
                    string[] rno = roll_no.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    for (int ij = 0; ij < rno.Length; ij++)
                    {
                        string staff_code = d2.GetFunction("select Staff_Code from staffmaster where Staff_Name='" + txt_staffname1.Text + "'");
                        applid = d2.GetFunction("select appl_id  from staff_appl_master sam,staffmaster sm where  sm.staff_code='" + staff_code + "' and sam.appl_no = sm.appl_no");
                        //added By Mullai
                        if (applid == "0")
                        {
                            Fpspread2.Visible = false;
                            btn_save.Visible = false;
                            btn_exit.Visible = false;
                            savediv.Visible = false;
                            imgdiv2.Visible = true;
                            lbl_erroralert.Text = "Invalid Staff Name";
                            return;
                        }
                        //**
                        string sql = "";
                        sql = "if exists (select * from CO_StudentTutor where App_No ='" + rno[ij] + "') update CO_StudentTutor set StaffMasterFK ='" + applid + "', TutorFor='" + tutorType + "' where  App_No ='" + rno[ij] + "' else INSERT INTO CO_StudentTutor(App_No,StaffMasterFK,TutorFor) values ('" + rno[ij] + "','" + applid + "','" + tutorType + "')";
                        int insert = d2.update_method_wo_parameter(sql, "TEXT");
                        if (insert != 0)
                        {
                            flag = true;
                        }
                    }
                }
            }
            if (flag == true)
            {
                savediv.Visible = false;
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "Saved Successfully";
                btnaddnew_Click(sender, e);
            }
            else
            {
                savediv.Visible = false;
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "Please select any record";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnexit_Click(object sender, EventArgs e)
    {
        popAddNew.Visible = false;
        txt_staffname.Text = "";
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

    protected void Fpspread1_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = FpSpread1.Sheets[0].ActiveRow.ToString();
            string actcol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (FpSpread1.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            FpSpread1.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            FpSpread1.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    // popAddStaff         
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_name  from staffmaster where resign =0 and settled =0 and staff_name like  '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffCode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_code from staffmaster where resign =0 and settled =0 and staff_code like  '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }

    protected void ddl_search2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_search2.SelectedItem.Text == "Staff Name")
        {
            txt_searchbyname.Visible = true;
            txt_searchbycode.Visible = false;
        }
        else if (ddl_search2.SelectedItem.Text == "Staff Code")
        {
            txt_searchbycode.Visible = true;
            txt_searchbyname.Visible = false;
        }
    }

    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        popAddStaff.Visible = false;
        FpSpread1.Visible = false;
        maindiv.Visible = false;
        Fpspread2.Visible = false;
        txt_staffname1.Text = "";
        txt_staffname.Text = "";
        divdcorder.Visible = false;
        btn_delete.Visible = false;
    }

    protected void Cell_Click(object sender, EventArgs e)
    {
        try
        {
            check = true;
        }
        catch
        {
        }
    }

    protected void butnsearchbygo_Click(object sender, EventArgs e)
    {
        try
        {
            int rolcount = 0;
            int sno = 0;
            string sql = "";
            int rowcount;
            Fpstaff.Visible = true;
            if (txt_searchbyname.Text != "")
            {
                if (ddl_search2.SelectedIndex == 0)
                {
                    sql = "select a.appl_id,s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,d.desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d,staff_appl_master a where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and s.appl_no = a.appl_no and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and d.collegeCode =s.college_code and s.Staff_name ='" + Convert.ToString(txt_searchbyname.Text) + "'";//added
                }
                else if(ddl_search2.SelectedIndex == 1)
                {


                    sql = "select a.appl_id,s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,d.desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d,staff_appl_master a where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and s.appl_no = a.appl_no and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and d.collegeCode =s.college_code and s.staff_code ='" + Convert.ToString(txt_searchbycode.Text) + "'";
                }
              
            }
            else if (txt_searchbycode.Text.Trim() != "")
            {
                if (ddl_search2.SelectedIndex == 1)
                {
                    sql = "select a.appl_id,s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,d.desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d,staff_appl_master a where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and s.appl_no = a.appl_no and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and s.staff_code ='" + Convert.ToString(txt_searchbycode.Text) + "'";
                }
            }
            else
            {
                sql = "select a.appl_id,s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,d.desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d,staff_appl_master a where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and s.appl_no = a.appl_no and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and h.dept_code in ('" + ddl_deptname2.SelectedItem.Value + "')";
            }
            Fpstaff.Sheets[0].RowCount = 0;
            Fpstaff.SaveChanges();
            Fpstaff.SheetCorner.ColumnCount = 0;
            Fpstaff.CommandBar.Visible = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            Fpstaff.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
            Fpstaff.Sheets[0].RowCount = Fpstaff.Sheets[0].RowCount + 1;
            Fpstaff.Sheets[0].SpanModel.Add(Fpstaff.Sheets[0].RowCount - 1, 0, 1, 3);
            Fpstaff.Sheets[0].AutoPostBack = false;
            ds = d2.select_method_wo_parameter(sql, "Text");
            Fpstaff.Sheets[0].RowCount = 0;
            Fpstaff.Sheets[0].ColumnCount = 5;
            if (ds.Tables[0].Rows.Count > 0)
            {
                Fpstaff.Visible = true;
                btn_save2.Visible = true;
                btn_exit2.Visible = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[0].Locked = true;
                Fpstaff.Columns[0].Width = 80;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[1].Locked = true;
                Fpstaff.Columns[1].Width = 100;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[2].Locked = true;
                Fpstaff.Columns[2].Width = 200;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[3].Locked = true;
                Fpstaff.Columns[3].Width = 250;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Designation";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                Fpstaff.Columns[4].Width = 200;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[4].Locked = true;
                Fpstaff.Width = 700;
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
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["appl_id"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["staff_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["staff_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["dept_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["dept_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["desig_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["desig_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                }
                lbl_search3.Visible = true;
                lbl_error3.Visible = false;
                // lbl_search3.Text = "No Records Found";
                lbl_search3.Text = "No of Staff :" + sno.ToString();
                rowcount = Fpstaff.Sheets[0].RowCount;
                Fpstaff.Height = 370;
                Fpstaff.Sheets[0].PageSize = 25 + (rowcount * 20);
                Fpstaff.SaveChanges();
            }
            else
            {
                lbl_search3.Visible = false;
                Fpstaff.Visible = false;
                lbl_error3.Visible = true;
                lbl_error3.Text = "No Records Found";
                btn_save2.Visible = false;
                btn_exit2.Visible = false;
                //err.Visible = true;
                //err.Text = "No Records Found";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnsav_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_searchbycode.Text != "" || txt_searchbyname.Text != "" || ddl_deptname2.SelectedIndex != -1)
            {
                if (Fpstaff.Visible == true)
                {
                    if (btnflag == 1)
                    {
                        popAddNew.Visible = false;
                        popAddStaff.Visible = false;
                        string activerow = "";
                        string activecol = "";
                        activerow = Fpstaff.ActiveSheetView.ActiveRow.ToString();
                        activecol = Fpstaff.ActiveSheetView.ActiveColumn.ToString();
                        if (activerow.Trim() != "")
                        {
                            string StaffCode = Convert.ToString(Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                            string applid = "";
                            applid = d2.GetFunction("select appl_id  from staff_appl_master sam,staffmaster sm where  sm.staff_code='" + StaffCode + "' and sam.appl_no = sm.appl_no");
                            ViewState["appl_id"] = Convert.ToString(applid);
                            string query = "select staff_name  from staffmaster where resign =0 and settled =0 and staff_code like  '" + StaffCode + "%' ";
                            txt_staffname.Text = d2.GetFunction(query);
                        }
                        txt_searchbycode.Text = "";
                        txt_searchbyname.Text = "";
                    }
                    if (btnflag == 2)
                    {
                        popAddNew.Visible = true;
                        popAddStaff.Visible = false;
                        string activerow = "";
                        string activecol = "";
                        activerow = Fpstaff.ActiveSheetView.ActiveRow.ToString();
                        activecol = Fpstaff.ActiveSheetView.ActiveColumn.ToString();
                        if (activerow.Trim() != "")
                        {
                            string StaffCode = Convert.ToString(Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                            string applid = "";
                            applid = d2.GetFunction("select appl_id  from staff_appl_master sam,staffmaster sm where  sm.staff_code='" + StaffCode + "' and sam.appl_no = sm.appl_no");
                            ViewState["appl_id"] = Convert.ToString(applid);
                            string query = "select staff_name  from staffmaster where resign =0 and settled =0 and staff_code = '" + StaffCode + "'";
                            txt_staffname1.Text = d2.GetFunction(query);
                        }
                        txt_searchbycode.Text = "";
                        txt_searchbyname.Text = "";
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "No records found";
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "Please Select Any Staff code or name";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void imagebtnpopclose_Click(object sender, ImageClickEventArgs e)
    {
        popAddNew.Visible = false;
        FpSpread1.Visible = false;
        maindiv.Visible = false;
        Fpspread2.Visible = false;
        txt_staffname1.Text = "";
        txt_staffname.Text = "";
        divdcorder.Visible = false;
        btn_delete.Visible = false;
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        delete();
    }

    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        imgdiv2.Visible = false;
    }

    protected void btnex_Click(object sender, EventArgs e)
    {
        popAddStaff.Visible = false;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffNameadd(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_name  from staffmaster where resign =0 and settled =0 and staff_name like  '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }

    public void cb_batch_checkedchange(object sender, EventArgs e)
    {
        try
        {
            rs.CallCheckBoxChangedEvent(cbl_batch, cb_batch, txt_batch, "Batch");
            bindsem();
            BindSectionDetail();
        }
        catch (Exception ex)
        {
        }
    }

    public void cbl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            rs.CallCheckBoxListChangedEvent(cbl_batch, cb_batch, txt_batch, "Batch");
            bindsem();
            BindSectionDetail();
        }
        catch (Exception ex)
        {
        }
    }

    public void cb_degree_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_degree.Checked == true)
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    if (cb_degree.Checked == true)
                    {
                        cbl_degree.Items[i].Selected = true;
                        txt_degree.Text = lbl_degree.Text + "(" + (cbl_degree.Items.Count) + ")";
                    }
                }
                bindbranch();
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
                }
            }

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
            bindbranch();
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
            rs.CallCheckBoxChangedEvent(cbl_branch, cb_branch, txt_branch, "Branch");
            BindSectionDetail();
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
            rs.CallCheckBoxListChangedEvent(cbl_branch, cb_branch, txt_branch, "Branch");
            BindSectionDetail();
            bindsem();
        }
        catch
        {
        }
    }

    public void cb_sem_checkedchange(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_sem, cb_sem, txt_sem, lbl_org_sem.Text);
    }

    public void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_sem, cb_sem, txt_sem, lbl_org_sem.Text);
    }

    public void cb_sec_checkedchange(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_sec, cb_sec, txt_sec, "Section");
    }

    public void cbl_sec_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_sec, cb_sec, txt_sec, "Section");
    }

    public void cb_batch1_checkedchange(object sender, EventArgs e)
    {
        try
        {
            rs.CallCheckBoxChangedEvent(cbl_batch1, cb_batch1, txt_batch1, "Batch");
            bindsem1();
            BindSectionDetail1();
        }
        catch (Exception ex)
        {
        }
    }

    public void cbl_batch1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            rs.CallCheckBoxListChangedEvent(cbl_batch1, cb_batch1, txt_batch1, "Batch");
            bindsem1();
            BindSectionDetail1();
        }
        catch (Exception ex)
        {
        }
    }

    public void cb_degree1_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_degree1.Checked == true)
            {
                for (int i = 0; i < cbl_degree1.Items.Count; i++)
                {
                    if (cb_degree1.Checked == true)
                    {
                        cbl_degree1.Items[i].Selected = true;
                        txt_degree1.Text = Label2.Text + "(" + (cbl_degree1.Items.Count) + ")";
                    }
                }
            }
            else
            {
                for (int i = 0; i < cbl_degree1.Items.Count; i++)
                {
                    cbl_degree1.Items[i].Selected = false;
                    txt_degree1.Text = "--Select--";
                    txt_branch1.Text = "--Select--";
                    cbl_branch1.ClearSelection();
                    cb_branch1.Checked = false;
                }
            }
            bindbranch1();
            bindsem1();
            BindSectionDetail1();
        }
        catch (Exception ex)
        {
        }
    }

    public void cbl_degree1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_degree1.Checked = false;
            for (int i = 0; i < cbl_degree1.Items.Count; i++)
            {
                if (cbl_degree1.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_branch1.Text = "--Select--";
                }
            }
            if (seatcount == cbl_degree1.Items.Count)
            {
                txt_degree1.Text = Label2.Text + "(" + seatcount.ToString() + ")";
                cb_degree1.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_degree1.Text = "--Select--";
            }
            else
            {
                txt_degree1.Text = Label2.Text + "(" + seatcount.ToString() + ")";
            }
            bindbranch1();
            bindsem1();
            BindSectionDetail1();
        }
        catch (Exception ex)
        {
        }
    }

    public void cb_branch1_checkedchange(object sender, EventArgs e)
    {
        try
        {
            rs.CallCheckBoxChangedEvent(cbl_branch1, cb_branch1, txt_branch1, "Branch");
            BindSectionDetail1();
            bindsem1();
        }
        catch
        {
        }
    }

    public void cbl_branch1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            rs.CallCheckBoxListChangedEvent(cbl_branch1, cb_branch1, txt_branch1, "Branch");
            BindSectionDetail1();
            bindsem1();
        }
        catch
        {
        }
    }

    public void cb_sem1_checkedchange(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_sem1, cb_sem1, txt_sem1, Label4.Text);
    }

    public void cbl_sem1_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_sem1, cb_sem1, txt_sem1, Label4.Text);
    }

    public void cb_sec1_checkedchange(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_sec1, cb_sec1, txt_sec1, "Section");
    }

    public void cbl_sec1_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_sec1, cb_sec1, txt_sec1, "Section");
    }

    void BindColumnOrder()
    {
        cbl_columnorder.Items.Clear();
        cbl_columnorder.Items.Add(new ListItem("Roll No", "Roll_No"));
        cbl_columnorder.Items.Add(new ListItem("Reg No", "Reg_No"));
        cbl_columnorder.Items.Add(new ListItem("Student Name", "Stud_Name"));
        cbl_columnorder.Items.Add(new ListItem("Staff Name", "Staff_Name"));
        cbl_columnorder.Items.Add(new ListItem("Degree", "Degree"));
        cbl_columnorder.Items.Add(new ListItem("Hostel Name", "HostelName"));
        cbl_columnorder.Items.Add(new ListItem("Building Name", "BuildingFK"));
        cbl_columnorder.Items.Add(new ListItem("Floor Name", "FloorFK"));
        cbl_columnorder.Items.Add(new ListItem("Room Name", "RoomFK"));
        cbl_columnorder.Items[0].Selected = true;
        cbl_columnorder.Items[2].Selected = true;
        cbl_columnorder.Items[3].Selected = true;
    }

    protected void mentorTypeOnclick(object sender, EventArgs e)
    {
        if (rdb_allstudent.Checked == true)
        {
            TRhostler.Attributes.Add("Style", "display:none;");
            TRallstud.Attributes.Add("Style", "display:block;");
        }
        if (rdb_hostel.Checked == true)
        {
            TRhostler.Attributes.Add("Style", "display:block;");
            TRallstud.Attributes.Add("Style", "display:none;");
        }
    }

    //Added by Saranya on 6.12/2018

    protected void ddl_collegename_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindbatch();
        degree();
        bindbranch();
        bindsem();
        BindSectionDetail();
        bindbatch1();
        degree1();
        bindbranch1();
        bindsem1();
        BindSectionDetail1();
    }

    protected void BindCollegeinfo()
    {
        try
        {
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegename.DataSource = ds;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
            }
            binddept(ddl_collegename.SelectedItem.Value.ToString());
        }
        catch
        {
        }
    }

}