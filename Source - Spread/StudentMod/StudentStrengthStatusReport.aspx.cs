using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Gios.Pdf;
using System.Configuration;
public partial class StudentMod_StudentStrengthStatusReport : System.Web.UI.Page
{
    ArrayList addcertificate = new ArrayList();
    static ArrayList ItemList_stud = new ArrayList();
    static ArrayList Itemindex_stud = new ArrayList();
    bool appliedbool = false;
    bool Cellclick = false;
    bool admitedbool = false;
    bool delbool = false;
    bool debarbool = false;
    bool cm_coursebool = false;
    bool ccccc = false;
    DataSet ds = new DataSet();
    DataSet nofar = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    Hashtable addtotalhash = new Hashtable();
    Hashtable hat1 = new Hashtable();
    Hashtable totalmode = new Hashtable();
    Hashtable newhash = new Hashtable();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static string loadval = string.Empty;
    static string colval = string.Empty;
    static string printval = string.Empty;
    static string loadval1 = string.Empty;
    static string colval1 = string.Empty;
    static string savecolumnoder = string.Empty;
    static string columnname = string.Empty;
    static string columnname1 = string.Empty;
    int n_arrear;
    ReuasableMethods rs = new ReuasableMethods();
    static byte roll = 0;

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
                btn_viewsprd2.Visible = false;
                lnk_admisstionformbtn.Visible = false;
                BindCollege();
                bindbatch();
                edu_level();
                degree();
                bindsem();
                BindSectionDetail();
                loadstutype();
                loadstream();
                loadseat();
                loadtype();

                // loadState();
                bindstate();
                loadreligion();
                loadcommunity();
                loadallotedcommunity();
                loadTypeName();
                loadTypeSize();
                loadUserName();//abarna
                loadReferBy();//abarna
                bindstatus();
                columnordertype();
                ItemList_stud.Clear();
                loadquota();

                //CalendarExtender10.EndDate = DateTime.Now;
                //CalendarExtender1.EndDate = DateTime.Now;
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Attributes.Add("readonly", "readonly");
                detailcolumn();
                bindresidency();
                bindsports();
                bindlanguage();
                bindmothertongue();
                bindphysicalchallaged();
                bindtransport();
                ddl_status.Visible = false;
                //Added By Saranyadevi24.2.2018
                LoadDisContinueReason();
                loadBoardUniv();
            }
            if (Request.Params["lst_setting1"] != null && (string)Request.Params["lst_setting1"] == "doubleclicked")
            {
            }
        }
        catch (Exception ex) { }
    }

    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);
    }

    public DateTime TextToDate(TextBox txt)
    {
        DateTime dt = new DateTime();
        string firstdate = Convert.ToString(txt.Text);
        string[] split = firstdate.Split('/');
        dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
        return dt;
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

    public void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadstream();
        edu_level();
        degree();
        bindbatch();
        bindsem();
        BindSectionDetail();
        loadstutype();
        //loadstream();
        loadseat();
        loadtype();
        loadreligion();
        loadcommunity();
        loadallotedcommunity();
        loadTypeName();
        loadTypeSize();
        loadUserName();//abarna
        loadReferBy();//abarna
        columnordertype();
        bindsports();
        bindresidency();
        bindlanguage();
        bindmothertongue();
        // bindstatus();
        loadquota();
        loadBoardUniv();
    }

    public void bindbatch()
    {
        try
        {
            cbl_batch.Items.Clear();
            hat.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_batch.DataSource = ds;
                cbl_batch.DataTextField = "batch_year";
                cbl_batch.DataValueField = "batch_year";
                cbl_batch.DataBind();
                //ddl_batch1.SelectedIndex = 3;
                if (cbl_batch.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_batch.Items.Count; i++)
                    {
                        cbl_batch.Items[0].Selected = true;
                    }
                    // txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
                    txt_batch.Text = "Batch(" + 1 + ")";
                    //cb_batch.Checked = true;
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
            string edulvl = string.Empty;
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
            string query = string.Empty;
            string type = string.Empty;
            if (txt_stream.Enabled == true)
            {
                type = rs.GetSelectedItemsValueAsString(cbl_stream);
            }
            string rights = string.Empty;
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
                    // cbl_degree.Items[0].Selected = true;
                    //cbl_sem.Items[i].Selected = true;
                    //studtype = Convert.ToString(cbl_sem.Items[i].Text);
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        cbl_sem.Items[i].Selected = true;
                        txt_degree.Text = Convert.ToString(cbl_degree.Items[i].Text);
                    }
                    if (cbl_sem.Items.Count == 1)
                    {
                        txt_sem.Text = "Degree(" + txt_degree.Text + ")";
                    }
                    else
                    {
                        txt_sem.Text = "Degree(" + txt_degree.Text + ")";
                    }
                }
                txt_degree.Text = lbl_degree.Text + "(" + 1 + ")";
                // cb_degree.Checked = true;
                //}
                //else
                //{
                //    txt_degree.Text = "--Select--";
                //    cb_degree.Checked = false;
                //}
                string deg = string.Empty;
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
            branch = string.Empty;
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
            string rights = string.Empty;
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
            string commname = string.Empty;
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
        try
        {
            string branch = string.Empty;
            string build = string.Empty;
            string build1 = string.Empty;
            string batch = string.Empty;
            int j = 0;
            cbl_sem.Items.Clear();
            string studtype = string.Empty;
            if (cbl_branch.Items.Count > 0)
            {
                for (j = 0; j < cbl_branch.Items.Count; j++)
                {
                    if (cbl_branch.Items[j].Selected == true)
                    {
                        build = cbl_branch.Items[j].Value.ToString();
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
            if (branch.Trim() != "")
            {
                string deptquery = "select distinct Current_Semester from Registration where degree_code in (" + branch + ")  order by Current_Semester";
                ds.Clear();
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_sem.DataSource = ds;
                    cbl_sem.DataTextField = "Current_Semester";
                    cbl_sem.DataBind();
                    if (cbl_sem.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_sem.Items.Count; i++)
                        {
                            cbl_sem.Items[i].Selected = true;
                            studtype = Convert.ToString(cbl_sem.Items[i].Text);
                        }
                        if (cbl_sem.Items.Count == 1)
                        {
                            txt_sem.Text = "Semester(" + studtype + ")";
                        }
                        else
                        {
                            txt_sem.Text = "Semester(" + cbl_studtype.Items.Count + ")";
                        }
                        cb_sem.Checked = true;
                    }
                }
                else
                {
                    txt_sem.Text = "--Select--";
                }
            }
        }
        catch
        {
        }
    }

    //public void bindsem()
    //{
    //    cbl_sem.Items.Clear();
    //    txt_sem.Text = "--Select--";
    //    Boolean first_year;
    //    first_year = false;
    //    int duration = 0;
    //    int i = 0;
    //    ds.Clear();
    //    string branch = string.Empty;
    //    string build = string.Empty;
    //    string build1 = string.Empty;
    //    string batch = string.Empty;
    //    if (cbl_branch.Items.Count > 0)
    //    {
    //        for (i = 0; i < cbl_branch.Items.Count; i++)
    //        {
    //            if (cbl_branch.Items[i].Selected == true)
    //            {
    //                build = cbl_branch.Items[i].Value.ToString();
    //                if (branch == "")
    //                {
    //                    branch = build;
    //                }
    //                else
    //                {
    //                    branch = branch + "," + build;
    //                }
    //            }
    //        }
    //    }
    //    if (cbl_batch.Items.Count > 0)
    //    {
    //        for (i = 0; i < cbl_batch.Items.Count; i++)
    //        {
    //            if (cbl_batch.Items[i].Selected == true)
    //            {
    //                build1 = cbl_batch.Items[i].Value.ToString();
    //                if (batch == "")
    //                {
    //                    batch = build1;
    //                }
    //                else
    //                {
    //                    batch = batch + "," + build1;
    //                }
    //            }
    //        }
    //    }
    //    //batch = build;
    //    if (branch.Trim() != "" && batch.Trim() != "")
    //    {
    //        // ds = d2.BindSem(branch, batch, ddlcollege.SelectedItem.Value);
    //        string strsql1 = "select distinct duration,first_year_nonsemester  from degree where degree_code in (" + branch + ") and college_code=" + ddlcollege.SelectedItem.Value + "";
    //        ds = d2.select_method_wo_parameter(strsql1, "text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
    //            {
    //                string dur = Convert.ToString(ds.Tables[0].Rows[i][0]);
    //                if (dur.Trim() != "")
    //                {
    //                    if (duration < Convert.ToInt32(dur)+1)
    //                    {
    //                        duration = Convert.ToInt32(dur);
    //                    }
    //                }
    //            }
    //        }
    //        if (duration != 0)
    //        {
    //            for (i = 1; i <= duration; i++)
    //            {
    //                cbl_sem.Items.Add(Convert.ToString(i));
    //            }
    //            if (cbl_sem.Items.Count > 0)
    //            {
    //                for (int row = 0; row < cbl_sem.Items.Count; row++)
    //                {
    //                    cbl_sem.Items[row].Selected = true;
    //                    cb_sem.Checked = true;
    //                }
    //                txt_sem.Text = lbl_org_sem.Text + "(" + cbl_sem.Items.Count + ")";
    //            }
    //        }
    //    }
    //}

    public void BindSectionDetail()
    {
        try
        {
            cbl_sec.Items.Clear();
            string batch = string.Empty;
            string branch = string.Empty;
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
                    if (cbl_sec.Items.Count > 0)
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

    public void loadstutype()
    {
        try
        {
            cbl_studtype.Items.Clear();
            string studtype = string.Empty;
            string deptquery = "select distinct Stud_Type from Registration where college_code in('" + ddlcollege.SelectedItem.Value + "') and Stud_Type is not null and Stud_Type<>''";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_studtype.DataSource = ds;
                cbl_studtype.DataTextField = "Stud_Type";
                cbl_studtype.DataBind();
                if (cbl_studtype.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_studtype.Items.Count; i++)
                    {
                        cbl_studtype.Items[i].Selected = true;
                        studtype = Convert.ToString(cbl_studtype.Items[i].Text);
                    }
                    if (cbl_studtype.Items.Count == 1)
                    {
                        txt_studtype.Text = "Student Type(" + studtype + ")";
                    }
                    else
                    {
                        txt_studtype.Text = "Student Type(" + cbl_studtype.Items.Count + ")";
                    }
                    cb_studtype.Checked = true;
                }
            }
            else
            {
                txt_studtype.Text = "--Select--";
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
            string stream = string.Empty;
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
        string st = string.Empty;
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
            //    //cb_graduation.Checked = true;
            //}
            //else
            //{
            //    txt_graduation.Text = "--Select--";
            //    cb_graduation.Checked = false;
            //    txt_degree.Text = "--Select--";
            //    cb_degree.Checked = false;
            //    cbl_degree.Items.Clear();
            //    txt_branch.Text = "--Select--";
            //    cb_branch.Checked = false;
            //    cbl_branch.Items.Clear();
            //    cb_sem.Checked = false;
            //    txt_sem.Text = "--Select--";
            //    cbl_sem.Items.Clear();
            //    cb_sec.Checked = false;
            //    txt_sec.Text = "--Select--";
            //    cbl_sec.Items.Clear();
            //}
        }
    }

    public void loadseat()
    {
        try
        {
            cbl_seat.Items.Clear();
            string seat = string.Empty;
            string deptquery = "select Distinct TextCode,TextVal  from TextValTable where TextCriteria ='Seat' and college_code ='" + ddlcollege.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_seat.DataSource = ds;
                cbl_seat.DataTextField = "TextVal";
                cbl_seat.DataValueField = "TextCode";
                cbl_seat.DataBind();
                if (cbl_seat.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_seat.Items.Count; i++)
                    {
                        cbl_seat.Items[i].Selected = true;
                        seat = Convert.ToString(cbl_seat.Items[i].Text);
                    }
                    if (cbl_seat.Items.Count == 1)
                    {
                        txt_seat.Text = "Seat(" + seat + ")";
                    }
                    else
                    {
                        txt_seat.Text = "Seat(" + cbl_seat.Items.Count + ")";
                    }
                    cb_seat.Checked = true;
                }
            }
            else
            {
                txt_seat.Text = "--Select--";
            }
        }
        catch
        {
        }
    }

    public void loadtype()
    {
        try
        {
            string type = string.Empty;
            cbl_type.Items.Clear();
            string[] typemode = { "Regular", "Transfer", "Lateral", "IrRegular" };
            for (int i = 0; i < 4; i++) //Transfer
            {
                cbl_type.Items.Add(new System.Web.UI.WebControls.ListItem(typemode[i], Convert.ToString(i + 1)));
            }
            if (cbl_type.Items.Count > 0)
            {
                for (int i = 0; i < cbl_type.Items.Count; i++)
                {
                    cbl_type.Items[i].Selected = true;
                    type = Convert.ToString(cbl_type.Items[i].Text);
                }
                if (cbl_type.Items.Count == 1)
                {
                    txt_type.Text = "Type(" + type + ")";
                }
                else
                {
                    txt_type.Text = "Type(" + cbl_type.Items.Count + ")";
                }
                cb_type.Checked = true;
            }
        }
        catch
        {
        }
    }

    public void loadreligion()
    {
        try
        {
            string religion = string.Empty;
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

    public void loadcommunity()
    {
        try
        {
            string comm = string.Empty;
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

    #region loadallotedCommunity added by abarna
    public void loadallotedcommunity()
    {
        try
        {
            string comm = string.Empty;
            string selq = "SELECT Distinct allotcomm,T.TextVal  FROM applyn A,Registration R,TextValTable T WHERE A.app_no=R.App_No AND T.TextCode =A.allotcomm  AND TextVal<>''AND R.college_code ='" + ddlcollege.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_allotcomm.DataSource = ds;
                    cbl_allotcomm.DataTextField = "TextVal";
                    cbl_allotcomm.DataValueField = "allotcomm";
                    cbl_allotcomm.DataBind();
                    if (cbl_allotcomm.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_allotcomm.Items.Count; i++)
                        {
                            cbl_allotcomm.Items[i].Selected = true;
                            comm = Convert.ToString(cbl_allotcomm.Items[i].Text);
                        }
                        if (cbl_allotcomm.Items.Count == 1)
                        {
                            txt_Allotcomm.Text = "" + comm + "";
                        }
                        else
                        {
                            txt_Allotcomm.Text = "Community(" + cbl_allotcomm.Items.Count + ")";
                        }
                        cb_allotcomm.Checked = true;
                    }
                }
            }
            else
            {
                txt_comm.Text = "--Select--";
                cb_allotcomm.Checked = false;
            }
        }
        catch
        {
        }
    }
    #endregion

    #region Typename added by abarna
    public void loadTypeName()
    {
        try
        {
            string comm = string.Empty;
            string selq = "SELECT Distinct mastervalue,mastercode  FROM St_personalInfod s,CO_MasterValues c WHERE c.mastercode=s.typenamevalue";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_typename.DataSource = ds;
                    cbl_typename.DataTextField = "mastervalue";
                    cbl_typename.DataValueField = "mastercode";
                    cbl_typename.DataBind();
                    if (cbl_typename.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_typename.Items.Count; i++)
                        {
                            cbl_typename.Items[i].Selected = true;
                            comm = Convert.ToString(cbl_typename.Items[i].Text);
                        }
                        if (cbl_typename.Items.Count == 1)
                        {
                            txt_Typename.Text = "" + comm + "";
                        }
                        else
                        {
                            txt_Typename.Text = "TypeName(" + cbl_typename.Items.Count + ")";
                        }
                        cb_typename.Checked = true;
                    }
                }
            }
            else
            {
                txt_Typename.Text = "--Select--";
                cb_typename.Checked = false;
            }
        }
        catch
        {
        }
    }
    public void cb_typename_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_typename.Checked == true)
            {
                for (int i = 0; i < cbl_typename.Items.Count; i++)
                {
                    cbl_typename.Items[i].Selected = true;
                }
                txt_Typename.Text = "Typename(" + (cbl_typename.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_typename.Items.Count; i++)
                {
                    cbl_typename.Items[i].Selected = false;
                }
                txt_Typename.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void cbl_typename_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_Typename.Text = "--Select--";
            cb_typename.Checked = false;
            for (int i = 0; i < cbl_typename.Items.Count; i++)
            {
                if (cbl_typename.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_typename.Items.Count)
            {
                txt_Typename.Text = "Typename(" + commcount.ToString() + ")";
                cb_typename.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_Typename.Text = "--Select--";
            }
            else
            {
                txt_Typename.Text = "Typename(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }
    #endregion

    #region Typesize added by abarna
    public void loadTypeSize()
    {
        try
        {
            string comm = string.Empty;
            string selq = "SELECT Distinct mastervalue,mastercode  FROM St_personalInfod s,CO_MasterValues c WHERE c.mastercode=s.typesizevalue";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_typesize.DataSource = ds;
                    cbl_typesize.DataTextField = "mastervalue";
                    cbl_typesize.DataValueField = "mastercode";
                    cbl_typesize.DataBind();
                    if (cbl_typesize.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_typesize.Items.Count; i++)
                        {
                            cbl_typesize.Items[i].Selected = true;
                            comm = Convert.ToString(cbl_typesize.Items[i].Text);
                        }
                        if (cbl_typesize.Items.Count == 1)
                        {
                            txt_typesize.Text = "" + comm + "";
                        }
                        else
                        {
                            txt_typesize.Text = "TypeSize(" + cbl_typesize.Items.Count + ")";
                        }
                        cb_typesize.Checked = true;
                    }
                }
            }
            else
            {
                txt_typesize.Text = "--Select--";
                cb_typesize.Checked = false;
            }
        }
        catch
        {
        }
    }

    public void cb_typesize_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_typesize.Checked == true)
            {
                for (int i = 0; i < cbl_typesize.Items.Count; i++)
                {
                    cbl_typesize.Items[i].Selected = true;
                }
                txt_typesize.Text = "Typesize(" + (cbl_typesize.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_typesize.Items.Count; i++)
                {
                    cbl_typesize.Items[i].Selected = false;
                }
                txt_typesize.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void cbl_typesize_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            cbl_typesize.Text = "--Select--";
            cb_typesize.Checked = false;
            for (int i = 0; i < cbl_typesize.Items.Count; i++)
            {
                if (cbl_typesize.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_typesize.Items.Count)
            {
                txt_typesize.Text = "Typesize(" + commcount.ToString() + ")";
                cb_typesize.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_typesize.Text = "--Select--";
            }
            else
            {
                txt_typesize.Text = "Typesize(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }
    #endregion

    #region Username added by abarna
    public void loadUserName()
    {
        try
        {
            string comm = string.Empty;
            string selq = "SELECT Distinct user_code,user_id from usermaster where college_code='" + ddlcollege.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_username.DataSource = ds;
                    cbl_username.DataTextField = "user_id";
                    cbl_username.DataValueField = "user_code";
                    cbl_username.DataBind();
                    if (cbl_username.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_username.Items.Count; i++)
                        {
                            cbl_username.Items[i].Selected = true;
                            comm = Convert.ToString(cbl_username.Items[i].Text);
                        }
                        if (cbl_username.Items.Count == 1)
                        {
                            Usernametxt.Text = "" + comm + "";
                        }
                        else
                        {
                            Usernametxt.Text = "UserName(" + cbl_username.Items.Count + ")";
                        }
                        cb_username.Checked = true;
                    }
                }
            }
            else
            {
                Usernametxt.Text = "--Select--";
                cb_username.Checked = false;
            }
        }
        catch
        {
        }
    }

    public void cb_username_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_username.Checked == true)
            {
                for (int i = 0; i < cbl_username.Items.Count; i++)
                {
                    cbl_username.Items[i].Selected = true;
                }
                Usernametxt.Text = "UserName(" + (cbl_username.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_username.Items.Count; i++)
                {
                    cbl_username.Items[i].Selected = false;
                }
                Usernametxt.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void cbl_username_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            // cbl_username.Text = "--Select--";
            cb_username.Checked = false;
            for (int i = 0; i < cbl_username.Items.Count; i++)
            {
                if (cbl_username.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_username.Items.Count)
            {
                Usernametxt.Text = "UserName(" + commcount.ToString() + ")";
                cb_username.Checked = true;
            }
            else if (commcount == 0)
            {
                Usernametxt.Text = "--Select--";
            }
            else
            {
                Usernametxt.Text = "UserName(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }
    #endregion

    #region Referby added by abarna

    public void loadReferBy()
    {
        try
        {
            string comm = string.Empty;
            string selq = "select distinct case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,direct_refer from applyn where college_code='" + ddlcollege.SelectedItem.Value + "' and direct_refer  is not null";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_refername.DataSource = ds;
                    cbl_refername.DataTextField = "referby";
                    cbl_refername.DataValueField = "direct_refer";
                    cbl_refername.DataBind();
                    if (cbl_refername.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_refername.Items.Count; i++)
                        {
                            cbl_refername.Items[i].Selected = true;
                            comm = Convert.ToString(cbl_refername.Items[i].Text);
                        }
                        if (cbl_refername.Items.Count == 1)
                        {
                            txtReferid.Text = "" + comm + "";
                        }
                        else
                        {
                            txtReferid.Text = "Refer By(" + cbl_refername.Items.Count + ")";
                        }
                        cb_Refername.Checked = true;
                    }
                }
            }
            else
            {
                txtReferid.Text = "--Select--";
                cb_Refername.Checked = false;
            }
        }
        catch
        {
        }
    }
    public void cb_Refername_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_Refername.Checked == true)
            {
                for (int i = 0; i < cbl_refername.Items.Count; i++)
                {
                    cbl_refername.Items[i].Selected = true;
                }
                txtReferid.Text = "Refer By(" + (cbl_refername.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_refername.Items.Count; i++)
                {
                    cbl_refername.Items[i].Selected = false;
                }
                txtReferid.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void cbl_refername_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            // cbl_username.Text = "--Select--";
            cb_Refername.Checked = false;
            for (int i = 0; i < cbl_refername.Items.Count; i++)
            {
                if (cbl_refername.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_refername.Items.Count)
            {
                txtReferid.Text = "Refer By(" + commcount.ToString() + ")";
                cb_Refername.Checked = true;
            }
            else if (commcount == 0)
            {
                txtReferid.Text = "--Select--";
            }
            else
            {
                txtReferid.Text = "Refer By(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }
    #endregion

    public void bindstatus()
    {
        string type = string.Empty;
        string[] statusname = { "Applied", "Waiting For Admitted", "Left", "Admitted", "Discontinue", "De-Bar", "Course Completed", "Prolong Absent", "Transport Canceled Student", "Hostel Canceled Student", "Enquiry", "Applied All" };//modified
        for (int i = 0; i < 12; i++)
        {
            cbl_status.Items.Add(new System.Web.UI.WebControls.ListItem(statusname[i], Convert.ToString(i + 1)));
        }
        for (int i = 0; i < 12; i++)
        {
            ddl_status.Items.Add(new System.Web.UI.WebControls.ListItem(statusname[i], Convert.ToString(i + 1)));
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

    public void bindtransport()
    {
        string type = string.Empty;
        string[] bindtrans = { "Own Transport", "Institution Transport" };
        for (int i = 0; i < 2; i++)
        {
            cbl_transport.Items.Add(new System.Web.UI.WebControls.ListItem(bindtrans[i], Convert.ToString(i + 1)));
        }
        if (cbl_transport.Items.Count > 0)
        {
            for (int i = 0; i < cbl_transport.Items.Count; i++)
            {
                cbl_transport.Items[i].Selected = true;
                type = Convert.ToString(cbl_transport.Items[i].Text);
            }
            if (cbl_transport.Items.Count == 1)
            {
                txt_transport.Text = "Transport(" + type + ")";
            }
            else
            {
                txt_transport.Text = "Transport(" + cbl_transport.Items.Count + ")";
            }
            cb_statusdetail.Checked = true;
        }
    }

    //Added By Saranyadevi 24.2.2018

    #region DisContinueReason

    public void LoadDisContinueReason()
    {
        try
        {

            cbl_reason.Items.Clear();
            cb_reason.Checked = false;
            txt_reason.Text = "---Select---";
            ds.Clear();

            string Query = "select distinct Reason from Discontinue where isnull(reason,'')<>'' group by Reason ";
            //string Query = " select distinct case when isnull(Reason,'')='' then 'Empty' else Reason end  as Reason from Discontinue";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_reason.DataSource = ds;
                cbl_reason.DataTextField = "Reason";
                //cbl_reason.DataValueField = "Criteria_no";
                cbl_reason.DataBind();
                cbl_reason.Items.Insert(0, "Empty");
                if (cbl_reason.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_reason.Items.Count; i++)
                    {
                        cbl_reason.Items[i].Selected = true;
                    }
                    txt_reason.Text = "Reason(" + cbl_reason.Items.Count + ")";
                    cb_reason.Checked = true;
                }
            }

        }
        //catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationSelection"); }
        catch
        {
        }


    }


    protected void cb_reason_checkedchange(object sender, EventArgs e)
    {
        rs.CallCheckboxChange(cb_reason, cbl_reason, txt_reason, "Reason", "--Select--");


    }



    protected void cbl_reason_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckboxListChange(cb_reason, cbl_reason, txt_reason, "Reason", "--Select--");
    }


    protected void cb_Disreaason_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_Disreaason.Checked == true)
        {
            txt_reason.Enabled = true;
        }
        else
        {
            txt_reason.Enabled = false;
        }
    }
    #endregion

    //End By Saranyadevi 24.2.2018

    public void cb_stream_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_stream.Checked == true)
            {
                for (int i = 0; i < cbl_stream.Items.Count; i++)
                {
                    cbl_stream.Items[i].Selected = true;
                }
                txt_stream.Text = lbl_Stream.Text + "(" + (cbl_stream.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_stream.Items.Count; i++)
                {
                    cbl_stream.Items[i].Selected = false;
                }
                txt_stream.Text = "--Select--";
            }
            edu_level();
            degree();
        }
        catch
        {
        }
    }

    public void cbl_stream_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_stream.Text = "--Select--";
            cb_stream.Checked = false;
            for (int i = 0; i < cbl_stream.Items.Count; i++)
            {
                if (cbl_stream.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            edu_level();
            degree();
            if (commcount == cbl_stream.Items.Count)
            {
                txt_stream.Text = lbl_Stream.Text + "(" + commcount.ToString() + ")";
                cb_stream.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_stream.Text = "--Select--";
            }
            else
            {
                txt_stream.Text = lbl_Stream.Text + "(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }

    public void cb_graduation_checkedchange(object sender, EventArgs e)
    {
        try
        {
            string deg = string.Empty;
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
            string deg = string.Empty;
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
                //degree();
                //bindbranch(deg);
                //bindsem();
                //BindSectionDetail();
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
            string buildvalue1 = string.Empty;
            string build1 = string.Empty;
            if (cb_batch.Checked == true)
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    if (cb_batch.Checked == true)
                    {
                        cbl_batch.Items[i].Selected = true;
                        txt_batch.Text = "Batch(" + (cbl_batch.Items.Count) + ")";
                        build1 = cbl_batch.Items[i].Value.ToString();
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
            }
            else
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = false;
                    txt_batch.Text = "--Select--";
                }
            }
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
            int seatcount = 0;
            cb_batch.Checked = false;
            string buildvalue = string.Empty;
            string build = string.Empty;
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_batch.Text = "--Select--";
                    build = cbl_batch.Items[i].Value.ToString();
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
            if (seatcount == cbl_batch.Items.Count)
            {
                txt_batch.Text = "Batch(" + seatcount.ToString() + ")";
                cb_batch.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_batch.Text = "--Select--";
                cb_batch.Text = "--Select--";
            }
            else
            {
                txt_batch.Text = "Batch(" + seatcount.ToString() + ")";
            }
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
            string buildvalue1 = string.Empty;
            string build1 = string.Empty;
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
            string buildvalue = string.Empty;
            string build = string.Empty;
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
            if (cb_branch.Checked == true)
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = true;
                }
                txt_branch.Text = lbl_branch.Text + "(" + (cbl_branch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = false;
                }
                txt_branch.Text = "--Select--";
            }
            bindsem();
            BindSectionDetail();
        }
        catch
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
                txt_branch.Text = lbl_branch.Text + "(" + commcount.ToString() + ")";
                cb_branch.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_degree.Text = "--Select--";
            }
            else
            {
                txt_branch.Text = lbl_branch.Text + "(" + commcount.ToString() + ")";
            }
            bindsem();
            BindSectionDetail();
        }
        catch
        {
        }
    }

    public void cb_sem_checkedchange(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_sem.Text = "--Select--";
            if (cb_sem.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = true;
                }
                txt_sem.Text = lbl_org_sem.Text + "(" + (cbl_sem.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cb_sem.Checked = false;
            int commcount = 0;
            txt_sem.Text = "--Select--";
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_sem.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_sem.Items.Count)
                {
                    cb_sem.Checked = true;
                }
                txt_sem.Text = lbl_org_sem.Text + "(" + commcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void cb_sec_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_sec.Checked == true)
            {
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    cbl_sec.Items[i].Selected = true;
                }
                txt_sec.Text = "Section(" + (cbl_sec.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    cbl_sec.Items[i].Selected = false;
                }
                txt_sec.Text = "--Select--";
            }
        }
        catch
        {
        }
    }

    public void cbl_sec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_sec.Text = "--Select--";
            cb_sec.Checked = false;
            for (int i = 0; i < cbl_sec.Items.Count; i++)
            {
                if (cbl_sec.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_sec.Items.Count)
            {
                txt_sec.Text = "Section(" + commcount.ToString() + ")";
                cb_sec.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_sec.Text = "--Select--";
            }
            else
            {
                txt_sec.Text = "Section(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }

    public void cb_studtype_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_studtype.Checked == true)
            {
                for (int i = 0; i < cbl_studtype.Items.Count; i++)
                {
                    cbl_studtype.Items[i].Selected = true;
                }
                txt_studtype.Text = "Student Type(" + (cbl_studtype.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_studtype.Items.Count; i++)
                {
                    cbl_studtype.Items[i].Selected = false;
                }
                txt_studtype.Text = "--Select--";
            }
        }
        catch
        {
        }
    }

    public void cbl_studtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_studtype.Text = "--Select--";
            cb_studtype.Checked = false;
            for (int i = 0; i < cbl_studtype.Items.Count; i++)
            {
                if (cbl_studtype.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_studtype.Items.Count)
            {
                txt_studtype.Text = "Student Type(" + commcount.ToString() + ")";
                cb_studtype.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_studtype.Text = "--Select--";
            }
            else
            {
                txt_studtype.Text = "Student Type(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }

    public void cb_seat_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_seat.Checked == true)
            {
                for (int i = 0; i < cbl_seat.Items.Count; i++)
                {
                    cbl_seat.Items[i].Selected = true;
                }
                txt_seat.Text = "Seat(" + (cbl_seat.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_seat.Items.Count; i++)
                {
                    cbl_seat.Items[i].Selected = false;
                }
                txt_seat.Text = "--Select--";
            }
        }
        catch
        {
        }
    }

    public void cbl_seat_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_seat.Text = "--Select--";
            cb_seat.Checked = false;
            for (int i = 0; i < cbl_seat.Items.Count; i++)
            {
                if (cbl_seat.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_seat.Items.Count)
            {
                txt_seat.Text = "Seat(" + commcount.ToString() + ")";
                cb_seat.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_seat.Text = "--Select--";
            }
            else
            {
                txt_seat.Text = "Seat(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }

    public void cb_type_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_type.Checked == true)
            {
                for (int i = 0; i < cbl_type.Items.Count; i++)
                {
                    cbl_type.Items[i].Selected = true;
                }
                txt_type.Text = "Type(" + (cbl_type.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_type.Items.Count; i++)
                {
                    cbl_type.Items[i].Selected = false;
                }
                txt_type.Text = "--Select--";
            }
        }
        catch
        {
        }
    }

    public void cbl_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_type.Text = "--Select--";
            cb_type.Checked = false;
            for (int i = 0; i < cbl_type.Items.Count; i++)
            {
                if (cbl_type.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_type.Items.Count)
            {
                txt_type.Text = "Type(" + commcount.ToString() + ")";
                cb_type.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_type.Text = "--Select--";
            }
            else
            {
                txt_type.Text = "Type(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }

    public void cb_religion_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_religion.Checked == true)
            {
                for (int i = 0; i < cbl_religion.Items.Count; i++)
                {
                    cbl_religion.Items[i].Selected = true;
                }
                txt_religion.Text = "Religion(" + (cbl_religion.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_religion.Items.Count; i++)
                {
                    cbl_religion.Items[i].Selected = false;
                }
                txt_religion.Text = "--Select--";
            }
        }
        catch
        {
        }
    }

    public void cbl_religion_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_religion.Text = "--Select--";
            cb_religion.Checked = false;
            for (int i = 0; i < cbl_religion.Items.Count; i++)
            {
                if (cbl_religion.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_religion.Items.Count)
            {
                txt_religion.Text = "Religion(" + commcount.ToString() + ")";
                cb_religion.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_religion.Text = "--Select--";
            }
            else
            {
                txt_religion.Text = "Religion(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }

    public void cb_comm_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_comm.Checked == true)
            {
                for (int i = 0; i < cbl_comm.Items.Count; i++)
                {
                    cbl_comm.Items[i].Selected = true;
                }
                txt_comm.Text = "Community(" + (cbl_comm.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_comm.Items.Count; i++)
                {
                    cbl_comm.Items[i].Selected = false;
                }
                txt_comm.Text = "--Select--";
            }
        }
        catch
        {
        }
    }

    public void cbl_comm_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_comm.Text = "--Select--";
            cb_comm.Checked = false;
            for (int i = 0; i < cbl_comm.Items.Count; i++)
            {
                if (cbl_comm.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_comm.Items.Count)
            {
                txt_comm.Text = "Community(" + commcount.ToString() + ")";
                cb_comm.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_comm.Text = "--Select--";
            }
            else
            {
                txt_comm.Text = "Community(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }

    public void cb_allotcomm_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_allotcomm.Checked == true)
            {
                for (int i = 0; i < cbl_allotcomm.Items.Count; i++)
                {
                    cbl_allotcomm.Items[i].Selected = true;
                }
                txt_Allotcomm.Text = "Community(" + (cbl_allotcomm.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_allotcomm.Items.Count; i++)
                {
                    cbl_allotcomm.Items[i].Selected = false;
                }
                txt_Allotcomm.Text = "--Select--";
            }
        }
        catch
        {
        }
    }

    public void cbl_allotcomm_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_Allotcomm.Text = "--Select--";
            cb_allotcomm.Checked = false;
            for (int i = 0; i < cbl_allotcomm.Items.Count; i++)
            {
                if (cbl_allotcomm.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_allotcomm.Items.Count)
            {
                txt_Allotcomm.Text = "Community(" + commcount.ToString() + ")";
                cb_allotcomm.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_Allotcomm.Text = "--Select--";
            }
            else
            {
                txt_Allotcomm.Text = "Community(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }

    public void cb_statusdetail_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_statusdetail.Checked == true)
            {
                for (int i = 0; i < cbl_status.Items.Count; i++)
                {
                    cbl_status.Items[i].Selected = true;
                }
                txt_status.Text = "Status(" + (cbl_status.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_status.Items.Count; i++)
                {
                    cbl_status.Items[i].Selected = false;
                }
                txt_status.Text = "--Select--";
            }
        }
        catch
        {
        }
    }

    public void cbl_status_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_status.Text = "--Select--";
            cb_statusdetail.Checked = false;
            for (int i = 0; i < cbl_status.Items.Count; i++)
            {
                if (cbl_status.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_status.Items.Count)
            {
                txt_status.Text = "Status(" + commcount.ToString() + ")";
                cb_statusdetail.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_status.Text = "--Select--";
            }
            else
            {
                txt_status.Text = "Status(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }

    public void btn_go_Click(object sender, EventArgs e)
    {
        savecolumnoder = "1";
        if (rdb_cumm.Checked == true)
        {
            go();
        }
    }

    public void btndetailgo_Click(object sender, EventArgs e)
    {
        savecolumnoder = "1";
        Fpspread2.Visible = false;
        lbl_headernamespd2.Visible = false;
        imgbtn_columsetting.Visible = true;
        btn_viewsprd2.Visible = false;
        lnk_admisstionformbtn.Visible = false;
        img_settingpdf.Visible = false;
        div_report.Visible = false;
        if (rdb_cumm.Checked == true)
        {
            go();
        }
        else
        {
            go1();
        }
    }

    public void btn_confcolm_Click(object sender, EventArgs e)
    {
        imgbtn_all_Click(sender, e);
        div_confmcolm.Visible = false;
    }

    public void btn_ntconfcolm_Click(object sender, EventArgs e)
    {
        string query = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='StudentStatusReport' and CollegeCode='" + ddlcollege.SelectedItem.Value + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ddl_colord.SelectedItem.Text != "Select")
            {
                div_confmcolm.Visible = false;
                Fpspread1.Visible = false;
                if (rdb_cumm.Checked == true)
                {
                    go();
                }
                else
                {
                    godetail();
                }
            }
            else
            {
                div_confmcolm.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Text = "Kindly Select Report Type";
            }
        }
        else
        {
            div_confmcolm.Visible = false;
            imgbtn_all_Click(sender, e);
            imgdiv2.Visible = true;
            lbl_alert.Text = "Kindly Set Report Type";
        }
    }

    public void cb_studtypechk_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_studtypechk.Checked == true)
        {
            txt_studtype.Enabled = true;
            div_report.Visible = false;
            rptprint.Visible = false;
            lblvalidation1.Visible = false;
            lbl_norec.Visible = false;
        }
        else
        {
            txt_studtype.Enabled = false;
        }
    }

    public void cb_seatchk_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_seatchk.Checked == true)
        {
            txt_seat.Enabled = true;
        }
        else
        {
            txt_seat.Enabled = false;
        }
    }

    public void cb_typechk_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_typechk.Checked == true)
        {
            txt_type.Enabled = true;
        }
        else
        {
            txt_type.Enabled = false;
        }
    }

    public void cb_relichk_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_relichk.Checked == true)
        {
            txt_religion.Enabled = true;
        }
        else
        {
            txt_religion.Enabled = false;
        }
    }

    public void cb_commchk_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_commchk.Checked == true)
        {
            txt_comm.Enabled = true;
        }
        else
        {
            txt_comm.Enabled = false;
        }
    }

    #region alloted community added by abarna
    public void allotcommchk_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (allotcommchk.Checked == true)
        {
            txt_Allotcomm.Enabled = true;
        }
        else
        {
            txt_Allotcomm.Enabled = false;
        }
    }
    public void chk_typesizename_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (chk_typesizename.Checked == true)
        {
            txt_typesize.Enabled = true;
        }
        else
        {
            txt_typesize.Enabled = false;
        }
    }
    #endregion

    #region typename added by abarna
    public void chk_typename_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (chk_typename.Checked == true)
        {
            txt_Typename.Enabled = true;
        }
        else
        {
            txt_Typename.Enabled = false;
        }
    }
    #endregion

    #region username added by abarna
    public void chk_user_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (chk_user.Checked == true)
        {
            Usernametxt.Enabled = true;
        }
        else
        {
            Usernametxt.Enabled = false;
        }
    }
    #endregion

    #region ReferedBy added By Abarna
    public void chkrefer_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (chkrefer.Checked == true)
        {
            txtReferid.Enabled = true;
        }
        else
        {
            txtReferid.Enabled = false;
        }
    }
    #endregion

    public void cb_status_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_status.Checked == true)
        {
            ddl_status.Enabled = true;
            txt_status.Enabled = true;
        }
        else
        {
            txt_status.Enabled = false;
            ddl_status.Enabled = false;
        }
    }

    public void rdb_cumm_CheckedChanged(object sender, EventArgs e)
    {
        Fpspread1.Visible = false;
        Fpspread2.Visible = false;
        imgbtn_columsetting.Visible = false;
        lbl_headernamespd2.Visible = false;
        btn_viewsprd2.Visible = false;
        lnk_admisstionformbtn.Visible = false;
        img_settingpdf.Visible = false;
        div_report.Visible = false;
        rptprint.Visible = false;
        divcolor.Visible = false;
        //2.6.2016
        UpdatePanel10.Visible = true;
        detailcolumn();
        // cb_status.Checked = false;
        ddl_status.Visible = false;
        cb_Gender.Visible = false;
        txt_gen.Visible = false;
        lbl_gen.Visible = false;
        Panel17.Visible = false;
        tdcbstate.Visible = false;
        tdcbboard.Visible = false;
        tdboard.Visible = false;
        tdstate.Visible = false;
        //Added By Saranyadevi 24.2.2018
        tdcbdisreason.Visible = false;
        tddisreason.Visible = false;
        tdquota.Visible = false;
        tdquotapanel.Visible = false;
        //abarna
        tdallotcommunity.Visible = false;
        tdallotcommunity1.Visible = false;
        typenametd.Visible = false;
        typenametd2.Visible = false;//abarna
        typesizetd1.Visible = false;
        typesizetd2.Visible = false;
        Usernametd.Visible = false;
        UserNameTd2.Visible = false;//abarna
        refertd.Visible = false;
        refertd2.Visible = false;//abarna
    }

    public void rdb_detail_CheckedChanged(object sender, EventArgs e)
    {
        tdquotapanel.Visible = true;
        tdquota.Visible = true;
        tdcbstate.Visible = true;
        tdcbboard.Visible = true;
        tdboard.Visible = true;
        tdstate.Visible = true;
        cb_status.Checked = true;
        UpdatePanel10.Visible = false;
        ddl_status.Enabled = true;
        ddl_status.Visible = true;
        Fpspread1.Visible = false;
        Fpspread2.Visible = false;
        imgbtn_columsetting.Visible = false;
        lbl_headernamespd2.Visible = false;
        btn_viewsprd2.Visible = false;
        lnk_admisstionformbtn.Visible = false;
        img_settingpdf.Visible = false;
        div_report.Visible = false;
        rptprint.Visible = false;
        divcolor.Visible = false;
        tdlblstudtype.Visible = true;
        tdstudetype.Visible = true;
        tdseattype.Visible = true;
        tdseattype1.Visible = true;
        tdtype.Visible = true;
        tdtype1.Visible = true;
        tdrelichk.Visible = true;
        tdrelichk1.Visible = true;
        tdcommchk.Visible = true;
        tdcommchk1.Visible = true;
        tdresident.Visible = true;
        tdresident1.Visible = true;
        tdsports.Visible = true;
        tdsports1.Visible = true;
        tdlang.Visible = true;
        tdlang1.Visible = true;
        tdmothertng.Visible = true;
        tdphychallange.Visible = true;
        tdtransport.Visible = true;
        cb_Gender.Visible = true;
        txt_gen.Visible = true;
        lbl_gen.Visible = true;
        Panel17.Visible = true;
        //abarna
        //---------------------------
        tdallotcommunity.Visible = true;
        tdallotcommunity1.Visible = true;
        typenametd.Visible = true;
        typenametd2.Visible = true;
        typesizetd1.Visible = true;
        typesizetd2.Visible = true;
        Usernametd.Visible = true;
        UserNameTd2.Visible = true;
        refertd.Visible = true;
        refertd2.Visible = true;

        //------------------------end
        //Added By Saranyadevi 24.2.2018
        tdcbdisreason.Visible = false;
        tddisreason.Visible = false;

        loadBoardUniv();
    }

    public void go()
    {
        try
        {
            lblerror.Visible = false;
            string printtot = string.Empty;
            string sectionvalue = string.Empty;
            string sectionvalue2 = string.Empty;
            int tot_allot = 0;
            int tot_strg = 0;
            int ccstud = 0;
            int ccseat = 0;
            int ccmode = 0;
            int ccreli = 0;
            int cccomm = 0;
            int i = 0;
            string addstream = string.Empty;
            string addgraud = string.Empty;
            string addbatch = string.Empty;
            string adddeg = string.Empty;
            string addsem = string.Empty;
            string addstudtype = string.Empty;
            string addstudtypeval = string.Empty;
            string addseat = string.Empty;
            string addseatval = string.Empty;
            string addtypeval = string.Empty;
            string addcommval = string.Empty;
            string addrelival = string.Empty;
            string addseatttt = string.Empty;
            string addrelllli = string.Empty;
            string addcommmm = string.Empty;
            string adddsec = string.Empty;
            string statusname = string.Empty;
            string statusnme = string.Empty;
            Fpspread1.Visible = false;
            addstream = rs.GetSelectedItemsValueAsString(cbl_stream);
            addgraud = rs.GetSelectedItemsValueAsString(cbl_graduation);
            addbatch = rs.GetSelectedItemsValueAsString(cbl_batch);
            adddeg = rs.GetSelectedItemsValueAsString(cbl_branch);
            addsem = rs.GetSelectedItemsValueAsString(cbl_sem);
            adddsec = rs.GetSelectedItemsValueAsString(cbl_sec);
            addstudtypeval = rs.GetSelectedItemsValueAsString(cbl_studtype);
            addseatttt = rs.GetSelectedItemsValueAsString(cbl_seat);
            addtypeval = rs.GetSelectedItemsValueAsString(cbl_type);
            addrelllli = rs.GetSelectedItemsValueAsString(cbl_religion);
            addcommmm = rs.GetSelectedItemsValueAsString(cbl_comm);
            string columnvalue = string.Empty;
            DateTime from = new DateTime();
            DateTime to = new DateTime();
            string[] ay = txt_fromdate.Text.Split('/');
            string[] ay1 = txt_todate.Text.Split('/');
            //magesh 8.3.18 mcc
            //from = Convert.ToDateTime(ay[1] + "/" + ay[0] + "/" + ay[2]);
            //to = Convert.ToDateTime(ay1[1] + "/" + ay1[0] + "/" + ay1[2]);
            from = Convert.ToDateTime(ay[2] + "-" + ay[1] + "-" + ay[0]);
            to = Convert.ToDateTime(ay1[2] + "-" + ay1[1] + "-" + ay1[0]);
            string datebetween = string.Empty;
            string datebetween1 = string.Empty;
            string datebetweenCanceled = string.Empty;
            if (cb_from.Checked == true)
            {
                //magesh 8.3.18
                //datebetween = "  and r.Adm_Date between  '" + from.ToString("MM/dd/yyyy") + "' and '" + to.ToString("MM/dd/yyyy") + "' ";
                //datebetween1 = "  and a.date_applied between  '" + from.ToString("MM/dd/yyyy") + "' and '" + to.ToString("MM/dd/yyyy") + "' ";
                //datebetweenCanceled = " and d.Discontinue_Date between  '" + from.ToString("dd/MM/yyyy") + "' and '" + to.ToString("dd/MM/yyyy") + "' ";
                datebetween = "  and r.Adm_Date between  '" + from.ToString("yyyy/MM/dd") + "' and '" + to.ToString("yyyy/MM/dd") + "' ";
                datebetween1 = "  and a.date_applied between  '" + from.ToString("yyyy/MM/dd") + "' and '" + to.ToString("yyyy/MM/dd") + "' ";
                datebetweenCanceled = " and d.Discontinue_Date between  '" + from.ToString("yyyy/MM/dd") + "' and '" + to.ToString("yyyy/MM/dd") + "' ";
            }
            if (cb_studtypechk.Checked == true)
            {
                columnvalue = " and r.Stud_Type in('" + addstudtypeval + "') ";
            }
            if (cb_seatchk.Checked == true)
            {
                columnvalue = columnvalue + " and a.seattype in('" + addseatttt + "') ";
            }
            if (cb_typechk.Checked == true)
            {
                columnvalue = columnvalue + " and a.mode in('" + addtypeval + "') ";
            }
            if (cb_relichk.Checked == true)
            {
                columnvalue = columnvalue + " and a.religion in('" + addrelllli + "') ";
            }
            if (cb_commchk.Checked == true)
            {
                columnvalue = columnvalue + " and a.community in('" + addcommmm + "') ";
            }
            if (addgraud == "")
            {
                lbl_err_stud.Visible = true;
                lbl_err_stud.Text = "Kindly Select The Graduation";
                return;
            }
            if (addbatch == "")
            {
                lbl_err_stud.Visible = true;
                lbl_err_stud.Text = "Kindly Select The Batch Year ";
                return;
            }
            if (adddeg == "")
            {
                lbl_err_stud.Visible = true;
                lbl_err_stud.Text = "Kindly Select The Degree ";
                return;
            }
            if (adddeg == "")
            {
                lbl_err_stud.Visible = true;
                lbl_err_stud.Text = "Kindly Select The Branch ";
                return;
            }
            if (addsem == "")
            {
                lbl_err_stud.Visible = true;
                lbl_err_stud.Text = "Kindly Select The Semester ";
                return;
            }
            if (cb_status.Checked == true)
            {
                for (i = 0; i < cbl_status.Items.Count; i++)
                {
                    if (cbl_status.Items[i].Selected == true)
                    {
                        string addstatus = cbl_status.Items[i].Text.ToString();
                        string addstatus1 = cbl_status.Items[i].Value.ToString();
                        if (statusname == "")
                        {
                            statusname = addstatus;
                        }
                        else
                        {
                            statusname = statusname + "," + addstatus;
                        }
                        if (statusnme == "")
                        {
                            statusnme = addstatus1;
                        }
                        else
                        {
                            statusnme = addcommmm + "," + addstatus1;
                        }
                    }
                }
            }
            else
            {
                appliedbool = false;
                admitedbool = false;
                delbool = false;
                debarbool = false;
                cm_coursebool = false;
            }
            int count = 0;
            int count1 = 0;
            if (cb_studtypechk.Checked == true)
            {
                count1++;
                count1 = count1++;
            }
            if (cb_seatchk.Checked == true)
            {
                count1++;
                count1 = count1++;
            }
            if (cb_typechk.Checked == true)
            {
                count1++;
                count1 = count1++;
            }
            if (cb_relichk.Checked == true)
            {
                count1++;
                count1 = count1++;
            }
            if (cb_commchk.Checked == true)
            {
                count1++;
                count1 = count1++;
            }
            if (cb_status.Checked == true)
            {
                count1++;
                count1 = count1++;
            }
            if (addstudtypeval == "")
            {
                addstudtypeval = "0";
            }
            if (addseatval == "")
            {
                addseatval = "0";
            }
            if (addtypeval == "")
            {
                addtypeval = "0";
            }
            if (addrelival == "")
            {
                addrelival = "0";
            }
            if (addcommval == "")
            {
                addcommval = "0";
            }
            Fpspread1.Sheets[0].Visible = true;
            Fpspread1.Sheets[0].RowHeader.Visible = false;
            Fpspread1.CommandBar.Visible = false;
            Fpspread1.Sheets[0].AutoPostBack = true;
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread1.Sheets[0].ColumnCount = 8;
            FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
            darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle2.ForeColor = Color.Black;
            darkstyle2.HorizontalAlign = HorizontalAlign.Center;
            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
            string query = string.Empty;
            int r = 0;
            string admittedSectioncol = "";
            double admittedSectiontot = 0;
            if (cb_status.Checked == true)
            {
                for (i = 0; i < cbl_status.Items.Count; i++)
                {
                    if (cbl_status.Items[i].Selected == true)
                    {
                        string addstatus = cbl_status.Items[i].Text.ToString();
                        string addstatus1 = cbl_status.Items[i].Value.ToString();//delsi1402
                        if (addstatus.ToLower() == "applied all")
                        {
                            query = " select COUNT(a.app_no)as TotalStrength,No_Of_seats,''Sections, A.degree_code,A.Batch_Year,a.Current_Semester,C.Course_Name ,C.Course_Id,Dt.Dept_Name from applyn a, degree d,Department dt,Course C where d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and isconfirm ='1'   and a.degree_code in('" + adddeg + "') and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'   group by A.degree_code ,a.Batch_Year,a.Current_Semester,C.Course_Name ,C.Course_Id,Dt.Dept_Name,No_Of_seats ";
                        }
                        else
                        {
                            if (addstatus.ToLower() == "applied")
                            {
                                //query = " select COUNT(a.app_no)as TotalStrength,No_Of_seats,''Sections, A.degree_code,A.Batch_Year,a.Current_Semester,C.Course_Name ,C.Course_Id,Dt.Dept_Name from applyn a, degree d,Department dt,Course C where d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and isconfirm ='1'   and a.degree_code in('" + adddeg + "') and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'   and a.app_no not in(select r.app_no from registration r) and a.seattype<>'0'   group by A.degree_code ,a.Batch_Year,a.Current_Semester,C.Course_Name ,C.Course_Id,Dt.Dept_Name,No_Of_seats ";
                                query = " select COUNT(a.app_no)as TotalStrength,No_Of_seats,''Sections, A.degree_code,A.Batch_Year,a.Current_Semester,C.Course_Name ,C.Course_Id,Dt.Dept_Name from applyn a, degree d,Department dt,Course C where d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and isconfirm ='1'   and a.degree_code in('" + adddeg + "') and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'   and a.app_no not in(select r.app_no from registration r)   group by A.degree_code ,a.Batch_Year,a.Current_Semester,C.Course_Name ,C.Course_Id,Dt.Dept_Name,No_Of_seats ";



                            }
                            else if (addstatus.ToLower() == "left")//added by 25.01.2019
                            {
                                query = "  select COUNT(r.app_no)as TotalStrength,No_Of_seats,isnull( r.Sections,'') as Sections,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name  from Registration r,applyn a, degree d,Department dt,Course C where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No and isconfirm ='1' and admission_status ='2' and r.delflag='2' and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "')   and a.college_code='" + ddlcollege.SelectedItem.Value + "'  group by No_Of_seats,r.degree_code ,r.Batch_Year,r.Current_Semester, C.Course_Name,c.Course_Id ,Dt.Dept_Name ,isnull( r.Sections,'')";//" + datebetween + " and CC=0 and DelFlag =0 and Exam_Flag <>'Debar'
                            }
                            else
                            {
                                query = "  select COUNT(r.app_no)as TotalStrength,No_Of_seats,isnull( r.Sections,'') as Sections,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name  from Registration r,applyn a, degree d,Department dt,Course C where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No and isconfirm ='1' and admission_status ='1' and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "')   and a.college_code='" + ddlcollege.SelectedItem.Value + "'  group by No_Of_seats,r.degree_code ,r.Batch_Year,r.Current_Semester, C.Course_Name,c.Course_Id ,Dt.Dept_Name ,isnull( r.Sections,'')";//" + datebetween + " and CC=0 and DelFlag =0 and Exam_Flag <>'Debar'

                            }
                        }
                    }
                }
            }


            //query = " select COUNT(a.app_no)as TotalStrength,No_Of_seats,'' Sections,a.degree_code,a.Batch_Year,a.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name from applyn a, degree d,Department dt,Course C where d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'  and a.degree_code in('" + adddeg + "')and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "') " + datebetween1 + " group by No_Of_seats,a.degree_code ,a.Batch_Year,a.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name  union all ";

            //query = query + " select COUNT(r.app_no)as TotalStrength,r.Stud_Type,a.seattype,a.religion,a.community,r.mode,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name   from Registration r,applyn a, degree d,Department dt,Course C where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No and isconfirm ='1' and admission_status ='1' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar'and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "')  " + columnvalue + "   and a.college_code='" + ddlcollege.SelectedItem.Value + "' " + datebetween + "  group by r.Stud_Type, r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,a.seattype,r.mode,a.religion,a.community order by r.degree_code ,r.Current_Semester ";
            // query = query + " select Is_Enroll,COUNT(a.app_no)as TotalStrength, A.degree_code,'' Exam_Flag,0 DelFlag,''cc,A.Batch_Year,a.Current_Semester,C.Course_Name ,Dt.Dept_Name,admission_status ,'' sections,'' Stud_Type,a.seattype,a.religion,a.community,'0' mode,selection_status from applyn a, degree d,Department dt,Course C where d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and isconfirm ='1'   and a.degree_code in('" + adddeg + "') and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween1 + " and a.app_no not in(select r.app_no from registration r) and a.seattype<>'0'   group by a.degree_code ,a.Batch_Year,a.Current_Semester,C.Course_Name ,Dt.Dept_Name ,admission_status,Stud_Type,a.seattype,a.religion,a.community,mode,selection_status,Is_Enroll";

            query = query + " select Is_Enroll,COUNT(a.app_no)as TotalStrength, A.degree_code,'' Exam_Flag,0 DelFlag,''cc,A.Batch_Year,a.Current_Semester,C.Course_Name ,Dt.Dept_Name,admission_status ,'' sections,'' Stud_Type,a.seattype,a.religion,a.community,'0' mode,selection_status from applyn a, degree d,Department dt,Course C where d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and isconfirm ='1'   and a.degree_code in('" + adddeg + "') and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween1 + " and a.app_no not in(select r.app_no from registration r)  group by a.degree_code ,a.Batch_Year,a.Current_Semester,C.Course_Name ,Dt.Dept_Name ,admission_status,Stud_Type,a.seattype,a.religion,a.community,mode,selection_status,Is_Enroll";



            //
            //union all select COUNT(r.app_no)as TotalStrength,r.degree_code,r.Exam_Flag,r.DelFlag,r.cc,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,admission_status,Sections,r.Stud_Type,a.seattype,a.religion,a.community,r.mode   from Registration r,applyn a, degree d,Department dt,Course C where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No and isconfirm ='1' and admission_status ='1' and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "')  and r.App_No=a.app_no   " + datebetween + "   and a.college_code='" + ddlcollege.SelectedItem.Value + "' group by  r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name ,r.Exam_Flag,r.DelFlag,r.CC,admission_status ,Sections ,r.Stud_Type,a.seattype,a.religion,a.community,r.mode
            query = query + "   select  COUNT(distinct a.app_no)as TotalStrength, isdisable,isdisabledisc,islearningdis,handy,s.Part1Language,visualhandy,  mother_tongue, DistinctSport, CampusReq,A.degree_code,'' Exam_Flag,0 DelFlag,''cc,A.Batch_Year,a.Current_Semester,ISNULL(admission_status,'0') as admission_status ,'' sections,'' Stud_Type,a.seattype,a.religion,a.community,a. mode from Stud_prev_details s, applyn a left join Registration r on a.app_no=r.App_No  where r.App_No is null  and isconfirm ='1'   and a.degree_code in('" + adddeg + "') and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "')   and a.college_code='" + ddlcollege.SelectedItem.Value + "' and a.app_no=s.app_no " + datebetween1 + " group by isdisable,isdisabledisc,islearningdis,handy,s.Part1Language,visualhandy,mother_tongue,DistinctSport,CampusReq,a.degree_code ,a.Batch_Year,a.Current_Semester,ISNULL(admission_status,'0'),a.Stud_Type,a.seattype,a.religion,a.community,a.mode  ";
            //query = query + " select COUNT(a.app_no)as TotalStrength, A.degree_code,'' Exam_Flag,0 DelFlag,''cc,A.Batch_Year,a.Current_Semester,C.Course_Name ,Dt.Dept_Name,ISNULL(admission_status,'0') as admission_status ,'' sections from degree d,Department dt,Course C ,applyn a where d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and isconfirm ='1'  and a.degree_code in('" + adddeg + "')and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "') " + datebetween1 + "  group by a.degree_code ,a.Batch_Year,a.Current_Semester,C.Course_Name ,Dt.Dept_Name ,ISNULL(admission_status,'0')";
            query = query + " select COUNT(r.app_no)as TotalStrength,r.degree_code,r.Exam_Flag,r.DelFlag,r.cc,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,admission_status,Sections from Registration r,applyn a, degree d,Department dt,Course C where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No  and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "')  and r.App_No=a.app_no      and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween + "  group by  r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name ,r.Exam_Flag,r.DelFlag,r.CC,admission_status ,Sections ";
            //query = query + " select COUNT(a.app_no)as TotalStrength, A.degree_code,'' Exam_Flag,0 DelFlag,''cc,A.Batch_Year,a.Current_Semester,C.Course_Name ,Dt.Dept_Name,ISNULL(admission_status,'0') as admission_status ,'' sections from degree d,Department dt,Course C ,applyn a where d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and isconfirm ='1'  and a.degree_code in('" + adddeg + "')and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "') " + datebetween1 + "  group by a.degree_code ,a.Batch_Year,a.Current_Semester,C.Course_Name ,Dt.Dept_Name ,ISNULL(admission_status,'0')";
            query = query + "  select COUNT(a.app_no)as TotalStrength, r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,ISNULL(admission_status,'0') as admission_status ,sections from degree d,Department dt,Course C ,applyn a,Registration r where r.App_No =a.app_no and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and isconfirm ='1'  and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "')   group by r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name ,ISNULL(admission_status,'0'),Sections ";

            query = query + "select COUNT(r.app_no)as TotalStrength,r.degree_code,r.Exam_Flag,r.DelFlag,r.cc,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,admission_status,Sections,r.ProlongAbsent from Registration r,applyn a, degree d,Department dt,Course C where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No  and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "')  and r.App_No=a.app_no and r.ProlongAbsent=0 and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween + "  group by  r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name ,r.Exam_Flag,r.DelFlag,r.CC,admission_status ,Sections,r.ProlongAbsent ";//prolongabsent query

            query = query + "select COUNT(r.app_no)as TotalStrength,r.degree_code,r.Exam_Flag,r.DelFlag,r.cc,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,admission_status,Sections,r.ProlongAbsent from Registration r,applyn a, degree d,Department dt,Course C where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No  and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "')  and r.App_No=a.app_no and  r.DelFlag<>0 and r.ProlongAbsent<>0 and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween + "  group by  r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name ,r.Exam_Flag,r.DelFlag,r.CC,admission_status,r.ProlongAbsent,Sections ";

            //............... sty type,mode etc...//
            // query = query + " select COUNT(r.app_no)as TotalStrength,r.degree_code,r.Exam_Flag,r.DelFlag,r.cc,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,admission_status,Sections,r.Stud_Type,a.seattype,a.religion,a.community,r.mode   from Registration r,applyn a, degree d,Department dt,Course C where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No  and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "')  and r.App_No=a.app_no   " + datebetween + "   and a.college_code='" + ddlcollege.SelectedItem.Value + "' group by  r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name ,r.Exam_Flag,r.DelFlag,r.CC,admission_status ,Sections ,r.Stud_Type,a.seattype,a.religion,a.community,r.mode";
            //union all select COUNT(r.app_no)as TotalStrength,r.degree_code,r.Exam_Flag,r.DelFlag,r.cc,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,admission_status,Sections   from Registration r,applyn a, degree d,Department dt,Course C where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No and isconfirm ='1' and admission_status ='1' and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "')  and r.App_No=a.app_no " + datebetween + "   and a.college_code='" + ddlcollege.SelectedItem.Value + "' group by  r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name ,r.Exam_Flag,r.DelFlag,r.CC,admission_status ,Sections

            //==================Added by Saranya on 05/01/2018 for Transport and Hostel Canceled Student================//
            //===========Transport============//
            query = query + " select COUNT(distinct d.app_no)as TotalStrength,isnull( r.Sections,'') as Sections,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,d.boarding,d.bus_routeid,d.vehid from Registration r,Discontinue d,applyn a, degree de,Department dt,Course C where de.Degree_Code =r.degree_code and de.Dept_Code =dt.Dept_Code and c.Course_Id =de.Course_Id and r.App_No=d.app_no and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "' and d.Catogery=4 and a.app_no =r.App_No " + datebetweenCanceled + " group by isnull( r.Sections,''),r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,d.boarding,d.bus_routeid,d.vehid ";
            //===========Hostel============//
            query = query + "  select COUNT(distinct d.app_no)as TotalStrength,isnull( r.Sections,'') as Sections,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,d.buildingname,d.hostelname,d.roomname from Registration r,Discontinue d,applyn a, degree de,Department dt,Course C where de.Degree_Code =r.degree_code and de.Dept_Code =dt.Dept_Code and c.Course_Id =de.Course_Id and r.App_No=d.app_no and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "' and d.Catogery=3 and a.app_no =r.App_No " + datebetweenCanceled + "   group by isnull( r.Sections,''),r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,d.buildingname,d.hostelname,d.roomname";
            //==========================================================================================================//
            //enquiry 24.01.18 barath
            query = query + " select Is_Enroll,COUNT(a.app_no)as TotalStrength, A.degree_code,'' Exam_Flag,0 DelFlag,''cc,A.Batch_Year,a.Current_Semester,C.Course_Name ,Dt.Dept_Name,admission_status ,'' sections,'' Stud_Type,a.seattype,a.religion,a.community,'0' mode,selection_status from applyn a, degree d,Department dt,Course C where d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and isnull(IsEnquiry,0)='1'  and a.degree_code in('" + adddeg + "') and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween1 + "  group by a.degree_code ,a.Batch_Year,a.Current_Semester,C.Course_Name ,Dt.Dept_Name ,admission_status,Stud_Type,a.seattype,a.religion,a.community,mode,selection_status,Is_Enroll";
            //and a.app_no not in(select r.app_no from registration r) 
            //discontinue 28/3/2018 delsi
            query = query + " select COUNT(r.app_no)as TotalStrength,r.degree_code,r.Exam_Flag,r.DelFlag,r.cc,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,admission_status,Sections from Registration r,applyn a, degree d,Department dt,Course C where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No  and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "')  and r.App_No=a.app_no and a.college_code='" + ddlcollege.SelectedItem.Value + "' and isnull(DelFlag,0) =1  group by  r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name ,r.Exam_Flag,r.DelFlag,r.CC,admission_status ,Sections";


            ds = d2.select_method_wo_parameter(query, "Text");
            if (query == "")
            {
                Fpspread1.Sheets[0].Visible = false;
                lbl_err_stud.Visible = true;
                lbl_err_stud.Text = "Kindly Select All List ";
                return;
            }
            else
            {
                if (query != "")
                {
                    //ds = d2.select_method(query, hat, "Text");
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        Fpspread1.Sheets[0].Visible = false;
                        Fpspread1.Visible = false;
                        lbl_err_stud.Visible = true;
                        lbl_err_stud.Text = "No Records Found";
                        lbl_headernamespd2.Visible = false;
                        lnk_admisstionformbtn.Visible = false;
                        btn_viewsprd2.Visible = false;
                        img_settingpdf.Visible = false;
                        Fpspread2.Visible = false;
                        div_report.Visible = false;
                        rptprint.Visible = false;
                        return;
                    }
                    else
                    {
                        lbl_err_stud.Visible = false;
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                            //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = lbl_degree.Text;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = lbl_branch.Text;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = lbl_org_sem.Text;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Section";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Total Seat Allotment";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Total Strength";
                            Fpspread1.Sheets[0].Columns[7].Visible = false;
                            #region
                            if (count1 != 0)
                            {
                                int cc = 7;
                                int j = 0;
                                if (cb_status.Checked == true)
                                {
                                    string[] sp = statusname.Split(',');
                                    for (j = 0; j < sp.Length; j++)
                                    {
                                        cc++;
                                        Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = sp[j];
                                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "Status";
                                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                        cccomm = cc;
                                        //Added by saranya
                                        string statusVal = sp[j];
                                        if (statusVal.ToLower() == "admitted")
                                        {
                                            cc++;
                                            Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = "Section Total";
                                            //Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "Status";
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                            admittedSectioncol = Convert.ToString(cc);
                                        }

                                    }
                                }
                                //if (cb_studtypechk.Checked == true)
                                //{
                                //    for (j = 0; j < cbl_studtype.Items.Count; j++)
                                //    {
                                //        if (cbl_studtype.Items[j].Selected == true)
                                //        {
                                //            cc++;
                                //            Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                //            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = cbl_studtype.Items[j].Text;
                                //            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "Stud_Type";
                                //            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Note = "Stud_Type" + "-" + cbl_studtype.Items[j].Value;
                                //            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                //            ccstud = cc;
                                //        }
                                //    }
                                //}
                                //if (cb_seatchk.Checked == true)
                                //{
                                //    for (j = 0; j < cbl_seat.Items.Count; j++)
                                //    {
                                //        if (cbl_seat.Items[j].Selected == true)
                                //        {
                                //            cc++;
                                //            Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                //            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = cbl_seat.Items[j].Text;
                                //            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "seattype";
                                //            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Note = "seattype" + "-" + cbl_seat.Items[j].Value;
                                //            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                //            ccseat = cc;
                                //        }
                                //    }
                                //}
                                //if (cb_typechk.Checked == true)
                                //{
                                //    for (j = 0; j < cbl_type.Items.Count; j++)
                                //    {
                                //        if (cbl_type.Items[j].Selected == true)
                                //        {
                                //            cc++;
                                //            Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                //            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = cbl_type.Items[j].Text;
                                //            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "mode";
                                //            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Note = "mode" + "-" + cbl_type.Items[j].Value;
                                //            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                //            ccmode = cc;
                                //        }
                                //    }
                                //}
                                //if (cb_relichk.Checked == true)
                                //{
                                //    for (j = 0; j < cbl_religion.Items.Count; j++)
                                //    {
                                //        if (cbl_religion.Items[j].Selected == true)
                                //        {
                                //            cc++;
                                //            Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                //            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = cbl_religion.Items[j].Text;
                                //            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "religion";
                                //            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Note = "religion" + "-" + cbl_religion.Items[j].Value;
                                //            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                //            ccreli = cc;
                                //        }
                                //    }
                                //}
                                //if (cb_commchk.Checked == true)
                                //{
                                //    for (j = 0; j < cbl_comm.Items.Count; j++)
                                //    {
                                //        if (cbl_comm.Items[j].Selected == true)
                                //        {
                                //            cc++;
                                //            Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                //            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = cbl_comm.Items[j].Text;
                                //            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "community";
                                //            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Note = "community" + "-" + cbl_comm.Items[j].Value;
                                //            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                //            cccomm = cc;
                                //        }
                                //    }
                                //}
                            }
                            #endregion
                            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                            style2.Font.Size = 14;
                            style2.Font.Name = "Book Antiqua";
                            style2.Font.Bold = true;
                            style2.HorizontalAlign = HorizontalAlign.Center;
                            style2.ForeColor = Color.Black;
                            style2.BackColor = Color.AliceBlue;
                            Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                            Fpspread1.Sheets[0].RowCount = 0;
                            int getcc = 0;
                            //DataTable dtNew = ds.Tables[1].DefaultView.ToTable(true, "TotalStrength");
                            string DegreeCodeReplication = string.Empty;
                            int colspan = 0; bool colspanBool = false;
                            string SecTotDegreeCodeReplication = string.Empty;
                            int SecTotColSpan = 0;
                            int totSectionCount = 0;
                            double sectionwisetot = 0;
                            DataView colSpanDV = new DataView();
                            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                Fpspread1.Sheets[0].RowCount++;
                                count++;
                                int cc = 7;
                                int D = 0;
                                int j = 0;
                                DataView dv = new DataView();
                                if (cb_studtypechk.Checked == true)
                                {
                                    // cc++;
                                }
                                if (adddsec != "")
                                {
                                    sectionvalue2 = " and ISNULL( r.Sections,'') in('','" + Convert.ToString(ds.Tables[0].Rows[i]["Sections"]) + "')";
                                }
                                else
                                {
                                    sectionvalue2 = string.Empty;
                                }
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Locked = true;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Locked = true;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Course_Name"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Course_Id"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Locked = true;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Column.Width = 300;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Locked = true;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Locked = true;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                if (adddsec != "")
                                {
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["Sections"]);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Sections"]);
                                }
                                else
                                {
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = "-";
                                }
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Locked = true;
                                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["No_Of_seats"]);//barath 26.12.17
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Locked = true;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["TotalStrength"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Locked = true;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;

                                ds.Tables[0].DefaultView.RowFilter = " degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' ";
                                colSpanDV = ds.Tables[0].DefaultView;
                                if (DegreeCodeReplication != Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]))//barath 26.12.17
                                {
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["No_Of_seats"]);
                                    int totalseatcount = Convert.ToInt32(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text);
                                    if (tot_allot == 0)
                                        tot_allot = totalseatcount;
                                    else
                                        tot_allot = tot_allot + totalseatcount;
                                    if (DegreeCodeReplication != "")
                                        if (colSpanDV.Count > 0)
                                            Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - colspan - 2, 6, colspan + 1, 1);
                                    colspan = 0;
                                    colspanBool = true;
                                    totSectionCount = 0;
                                }
                                else if (DegreeCodeReplication != "")//barath 26.12.17
                                {
                                    colspan++; colspanBool = false;
                                    totSectionCount++;
                                }
                                DegreeCodeReplication = Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]);//

                                //int totalseatcount = Convert.ToInt32(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text);
                                //if (tot_allot == 0)
                                //{
                                //    tot_allot = totalseatcount;
                                //}
                                //else
                                //{
                                //    tot_allot = tot_allot + totalseatcount;
                                //}
                                int totstrenth = Convert.ToInt32(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text);
                                if (tot_strg == 0)
                                {
                                    tot_strg = totstrenth;
                                }
                                else
                                {
                                    tot_strg = tot_strg + totstrenth;
                                }
                                string tagval = string.Empty;
                                cc = 7;
                                if (count1 != 0)
                                {
                                    if (cb_status.Checked == true)
                                    {
                                        for (int k = 0; k < cbl_status.Items.Count; k++)
                                        {
                                            if (cbl_status.Items[k].Selected == true)
                                            {
                                                string val = cbl_status.Items[k].Value.ToString();
                                                string values = newfunction(val);
                                                cc++;
                                                if (val == "2")
                                                {
                                                    if (ds.Tables[1].Rows.Count > 0)
                                                    {
                                                        ds.Tables[1].DefaultView.RowFilter = " Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' " + values + "";
                                                        dv = ds.Tables[1].DefaultView;
                                                    }
                                                }
                                                else if (val == "3")
                                                {
                                                    ds.Tables[4].DefaultView.RowFilter = " Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' and isnull (Sections,'')='" + Convert.ToString(ds.Tables[0].Rows[i]["Sections"]) + "' " + values + "";
                                                    dv = ds.Tables[4].DefaultView;
                                                }
                                                else if (val == "1")
                                                {
                                                    ds.Tables[1].DefaultView.RowFilter = " Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' and isnull (Sections,'')='" + Convert.ToString(ds.Tables[0].Rows[i]["Sections"]) + "' " + values + "";
                                                    dv = ds.Tables[1].DefaultView;
                                                }
                                                else if (val == "5")
                                                {
                                                    //ds.Tables[5].DefaultView.RowFilter = " Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' and isnull (Sections,'')='" + Convert.ToString(ds.Tables[0].Rows[i]["Sections"]) + "' " + values + "";
                                                    //dv = ds.Tables[5].DefaultView; //delsi2803
                                                    ds.Tables[10].DefaultView.RowFilter = " Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' and isnull (Sections,'')='" + Convert.ToString(ds.Tables[0].Rows[i]["Sections"]) + "' " + values + "";
                                                    dv = ds.Tables[10].DefaultView;


                                                }
                                                else if (val == "7")
                                                {
                                                    ds.Tables[5].DefaultView.RowFilter = " Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' and Sections='" + Convert.ToString(ds.Tables[0].Rows[i]["Sections"]) + "' " + values + "";
                                                    dv = ds.Tables[5].DefaultView;
                                                }
                                                else if (val == "8")
                                                {
                                                    ds.Tables[6].DefaultView.RowFilter = " Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' and Sections='" + Convert.ToString(ds.Tables[0].Rows[i]["Sections"]) + "' " + values + "";
                                                    dv = ds.Tables[6].DefaultView;
                                                }
                                                //Added By Saranya on 05/01/2018//
                                                else if (val == "9")
                                                {
                                                    ds.Tables[7].DefaultView.RowFilter = " Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' and Sections='" + Convert.ToString(ds.Tables[0].Rows[i]["Sections"]) + "' " + values + "";
                                                    dv = ds.Tables[7].DefaultView;
                                                }
                                                else if (val == "10")
                                                {
                                                    ds.Tables[8].DefaultView.RowFilter = " Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' and Sections='" + Convert.ToString(ds.Tables[0].Rows[i]["Sections"]) + "' " + values + "";
                                                    dv = ds.Tables[8].DefaultView;
                                                }
                                                //==============================//enquiry 
                                                else if (val == "11")
                                                {
                                                    ds.Tables[9].DefaultView.RowFilter = " Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "'  " + values + "";
                                                    dv = ds.Tables[9].DefaultView;
                                                }
                                                else if (val == "12")
                                                {
                                                    ds.Tables[0].DefaultView.RowFilter = " Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' and isnull (Sections,'')='" + Convert.ToString(ds.Tables[0].Rows[i]["Sections"]) + "'";
                                                    dv = ds.Tables[0].DefaultView;
                                                }
                                                else
                                                {
                                                    ds.Tables[3].DefaultView.RowFilter = " Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' and isnull (Sections,'')='" + Convert.ToString(ds.Tables[0].Rows[i]["Sections"]) + "' " + values + "";
                                                    dv = ds.Tables[3].DefaultView;
                                                }
                                                if (dv.Count > 0)
                                                {
                                                    DataTable dt = new DataTable();
                                                    dt = dv.ToTable();
                                                    int total = Convert.ToInt32(dt.Compute("Sum(TotalStrength)", ""));
                                                    string tot = Convert.ToString(total);
                                                    //if (val == "2")23.02.17 barath
                                                    //{
                                                    //    if (ds.Tables[2].Rows.Count == 0)
                                                    //    {
                                                    //        tot = string.Empty;
                                                    //    }
                                                    //}
                                                    if (tot == "")
                                                    {
                                                        printtot = "-";
                                                    }
                                                    else
                                                    {
                                                        printtot = tot;
                                                    }
                                                    string sectionVal = Convert.ToString(cbl_status.Items[k].Text);
                                                    if (sectionVal.ToLower() == "admitted")//Added by saranya
                                                    {
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Text = printtot;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Tag = tot;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].HorizontalAlign = HorizontalAlign.Center;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Font.Name = "Book Antiqua";
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Font.Size = FontUnit.Medium;

                                                        int admittedCol = Convert.ToInt32(admittedSectioncol);
                                                        if (SecTotDegreeCodeReplication != Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]))
                                                        {
                                                            sectionwisetot = Convert.ToDouble(printtot);
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, admittedCol].Text = printtot.ToString();
                                                            SecTotColSpan = 0;
                                                            admittedSectiontot = 0;
                                                        }
                                                        else if (SecTotDegreeCodeReplication != "")
                                                        {
                                                            admittedSectiontot = admittedSectiontot + sectionwisetot + Convert.ToDouble(printtot);
                                                            if (SecTotDegreeCodeReplication != "")
                                                                if (colSpanDV.Count == totSectionCount + 1)
                                                                {
                                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - (totSectionCount + 1), admittedCol].Text = admittedSectiontot.ToString();
                                                                    Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - SecTotColSpan - 2, admittedCol, SecTotColSpan + 2, 1);
                                                                }

                                                            SecTotColSpan++;
                                                            sectionwisetot = 0;
                                                        }
                                                        SecTotDegreeCodeReplication = Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]);

                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, admittedCol].HorizontalAlign = HorizontalAlign.Center;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, admittedCol].Font.Name = "Book Antiqua";
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, admittedCol].Font.Size = FontUnit.Medium;
                                                    }
                                                    else
                                                    {
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Text = printtot;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Tag = tot;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].HorizontalAlign = HorizontalAlign.Center;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Font.Name = "Book Antiqua";
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Font.Size = FontUnit.Medium;
                                                    }
                                                    if (!totalmode.Contains(Convert.ToString(cc)))
                                                    {
                                                        totalmode.Add(Convert.ToString(cc), Convert.ToString(tot));
                                                    }
                                                    else
                                                    {
                                                        string getvalue = Convert.ToString(totalmode[Convert.ToString(cc)]);
                                                        if (getvalue.Trim() != "")
                                                        {
                                                            getvalue = getvalue + "," + tot;
                                                            totalmode.Remove(Convert.ToString(cc));
                                                            if (getvalue.Trim() != "")
                                                            {
                                                                totalmode.Add(Convert.ToString(cc), Convert.ToString(getvalue));
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    string v = string.Empty;
                                    string v1 = string.Empty;
                                    string getvalues = string.Empty;
                                    int dd = cc + 1;
                                    //if (cc < Fpspread1.Sheets[0].ColumnCount)
                                    //{
                                    //    if (ds.Tables[1].Rows.Count > 0)
                                    //    {
                                    //        for (int jk = dd; jk < Fpspread1.Sheets[0].ColumnCount; jk++)
                                    //        {
                                    //            cc++;
                                    //            getvalues = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Note);
                                    //            string[] spp = getvalues.Split('-');
                                    //            v = spp[0];
                                    //            v1 = spp[1];
                                    //            ds.Tables[1].DefaultView.RowFilter = " Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and admission_status='1' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' and " + v + "='" + v1 + "' ";
                                    //            dv = ds.Tables[1].DefaultView;
                                    //            if (dv.Count > 0)
                                    //            {
                                    //                DataTable dt = new DataTable();
                                    //                dt = dv.ToTable();
                                    //                int tot = 0;
                                    //                int total = Convert.ToInt32(dt.Compute("Sum(TotalStrength)", ""));
                                    //                tot = tot + total;
                                    //                if (tot == 0)
                                    //                {
                                    //                    printtot = "-";
                                    //                }
                                    //                else
                                    //                {
                                    //                    printtot = Convert.ToString(tot);
                                    //                }
                                    //                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Text = printtot;
                                    //                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Tag = tot;
                                    //                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].HorizontalAlign = HorizontalAlign.Center;
                                    //                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Font.Name = "Book Antiqua";
                                    //                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Font.Size = FontUnit.Medium;
                                    //                if (!totalmode.Contains(Convert.ToString(cc)))
                                    //                {
                                    //                    totalmode.Add(Convert.ToString(cc), Convert.ToString(tot));
                                    //                }
                                    //                else
                                    //                {
                                    //                    string getvalue = Convert.ToString(totalmode[Convert.ToString(cc)]);
                                    //                    if (getvalue.Trim() != "")
                                    //                    {
                                    //                        getvalue = getvalue + "," + tot;
                                    //                        totalmode.Remove(Convert.ToString(cc));
                                    //                        if (getvalue.Trim() != "")
                                    //                        {
                                    //                            totalmode.Add(Convert.ToString(cc), Convert.ToString(getvalue));
                                    //                        }
                                    //                    }
                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                    ///// seat type
                                }
                                getcc = cc;
                            }
                            Fpspread1.Sheets[0].RowCount++;
                            if (!colspanBool)
                                if (DegreeCodeReplication != "")
                                    if (colSpanDV.Count > 0)
                                        Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - colspan - 2, 6, colspan + 1, 1);
                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 5].Text = "Total";
                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 5].Tag = "Total";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 5].BackColor = ColorTranslator.FromHtml("#80EDED");
                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 5].Font.Bold = true;
                            Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, 1, 5);
                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 5].ForeColor = Color.Maroon;
                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 5].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 6].Text = Convert.ToString(tot_allot);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            // Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 7].Text = Convert.ToString(tot_strg);
                            // Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 6].ForeColor = ColorTranslator.FromHtml("#107532");
                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 7].ForeColor = ColorTranslator.FromHtml("#107532");
                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 6].Font.Bold = true;
                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 7].Font.Bold = true;
                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 6].BackColor = ColorTranslator.FromHtml("#80EDED");
                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 7].BackColor = ColorTranslator.FromHtml("#80EDED");
                            // Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 8].BackColor = ColorTranslator.FromHtml("#80EDED");
                            // Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), Fpspread1.Sheets[0].ColumnCount - 1].BackColor = ColorTranslator.FromHtml("#80EDED");
                            if (totalmode.Count > 0)
                            {
                                for (int r1 = 8; r1 <= getcc; r1++)
                                {
                                    string totalvalue = Convert.ToString(totalmode[Convert.ToString(r1)]);
                                    if (totalvalue != "")
                                    {
                                        int gettotalvalue = 0;
                                        string[] spl = totalvalue.Split(',');
                                        for (int l = 0; l < spl.Length; l++)
                                        {
                                            int get_tot = Convert.ToInt32(spl[l]);
                                            if (gettotalvalue == 0)
                                            {
                                                gettotalvalue = get_tot;
                                            }
                                            else
                                            {
                                                gettotalvalue = gettotalvalue + get_tot;
                                            }
                                        }
                                        Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), r1].Text = Convert.ToString(gettotalvalue);
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, r1].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), r1].ForeColor = ColorTranslator.FromHtml("#107532");
                                        Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), r1].Font.Bold = true;
                                        Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), r1].BackColor = ColorTranslator.FromHtml("#80EDED");
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, r1].Font.Name = "Book Antiqua";
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, r1].Font.Size = FontUnit.Medium;
                                        // Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), Fpspread1.Sheets[0].ColumnCount - 1].BackColor = ColorTranslator.FromHtml("#80EDED");
                                    }
                                }
                            }
                            Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                            Fpspread1.Width = 900;
                            Fpspread1.Height = 420;
                            Fpspread1.Visible = true;
                            rptprint.Visible = true;
                            Fpspread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            Fpspread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            Fpspread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            Fpspread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx");
        }
    }

    public string newfunction(string val)
    {
        string text_val = string.Empty;
        if (val == "1")
        {
            text_val = " and ISNULL(admission_status,'0')=0 and ISNULL(selection_status,'0')=0 ";
        }
        if (val == "2")
        {
            text_val = " and ISNULL(admission_status,'0') ='1' and (selection_status='True' or ISNULL(selection_status,'0')=1)";//and admission_status='1' and selection_status='1'";barath 23.02.17 and isnull(Is_Enroll,'0')<>'2'
        }
        if (val == "3")
        {
            text_val = " and admission_status='2' ";
        }
        if (val == "4")
        {
            text_val = "  and admission_status='1' and DelFlag='0' and Exam_Flag='OK' and (cc='False' or cc=0)";//cc=0 (cc='False' or cc='0') 
        }
        if (val == "5")
        {
            text_val = " and DelFlag<>'0'";// and ProlongAbsent=0
        }
        if (val == "6")
        {
            text_val = " and Exam_Flag='DEBAR' ";
        }
        if (val == "7")
        {
            text_val = " and CC='True'";
        }
        if (val == "8")
        {
            text_val = " and  DelFlag<>0 and ProlongAbsent<>0";
        }

        return text_val;
    }

    public void go1()
    {
        try
        {
            lblerror.Visible = false;
            string printtot = string.Empty;
            string sectionvalue = string.Empty;
            string sectionvalue2 = string.Empty;
            int tot_allot = 0;
            int tot_strg = 0;
            int ccstud = 0;
            int ccseat = 0;
            int ccmode = 0;
            int ccreli = 0;
            int cccomm = 0;
            int ccallotcomm = 0;
            int ccquo = 0;
            int ccstate = 0;
            int ccboard = 0;
            int cctype = 0;
            int ccsize = 0;
            int ccuser = 0;
            int ccRefer = 0;
            //abarna
            int i = 0;
            string addstream = string.Empty;
            string addgraud = string.Empty;
            string addbatch = string.Empty;
            string adddeg = string.Empty;
            string addsem = string.Empty;
            string addstudtype = string.Empty;
            string addstudtypeval = string.Empty;
            string addseat = string.Empty;
            string addseatval = string.Empty;
            string addtypeval = string.Empty;
            string addcommval = string.Empty;
            string addrelival = string.Empty;
            string addseatttt = string.Empty;
            string addrelllli = string.Empty;
            string addcommmm = string.Empty;
            string adddsec = string.Empty;
            string statusname = string.Empty;
            string statusnme = string.Empty;
            string addboard = string.Empty;
            string DisContinuereason = string.Empty;//Added bu Saranyadevi 24.2.2018
            string disreason = string.Empty;
            string addquota = string.Empty;
            string addallotcomm = string.Empty;
            string addtypename = string.Empty;
            string addsize = string.Empty;
            string adduser = string.Empty;
            string addstate = string.Empty;
            string addrefer = string.Empty;
            Fpspread1.Visible = false;
            addstream = rs.GetSelectedItemsValueAsString(cbl_stream);
            addgraud = rs.GetSelectedItemsValueAsString(cbl_graduation);
            addbatch = rs.GetSelectedItemsValueAsString(cbl_batch);
            adddeg = rs.GetSelectedItemsValueAsString(cbl_branch);
            addsem = rs.GetSelectedItemsValueAsString(cbl_sem);
            adddsec = rs.GetSelectedItemsValueAsString(cbl_sec);
            addstudtypeval = rs.GetSelectedItemsValueAsString(cbl_studtype);
            addseatttt = rs.GetSelectedItemsValueAsString(cbl_seat);
            addtypeval = rs.GetSelectedItemsValueAsString(cbl_type);
            addrelllli = rs.GetSelectedItemsValueAsString(cbl_religion);
            addcommmm = rs.GetSelectedItemsValueAsString(cbl_comm);
            addquota = rs.GetSelectedItemsValueAsString(cblQuota);
            addallotcomm = rs.GetSelectedItemsValueAsString(cbl_allotcomm);//abarna
            addtypename = rs.GetSelectedItemsValueAsString(cbl_typename);
            addsize = rs.GetSelectedItemsValueAsString(cbl_typesize);
            adduser = rs.GetSelectedItemsValueAsString(cbl_username);
            addstate = rs.GetSelectedItemsValueAsString(cbl_state);
            addrefer = rs.GetSelectedItemsValueAsString(cbl_refername);
            string columnvalue = string.Empty;
            DateTime from = new DateTime();
            DateTime to = new DateTime();
            string[] ay = txt_fromdate.Text.Split('/');
            string[] ay1 = txt_todate.Text.Split('/');
            from = Convert.ToDateTime(ay[1] + "/" + ay[0] + "/" + ay[2]);
            to = Convert.ToDateTime(ay1[1] + "/" + ay1[0] + "/" + ay1[2]);
            string datebetween = string.Empty;
            string datebetween1 = string.Empty;
            string datebetweenCanceled = string.Empty;
            if (cb_from.Checked == true)
            {
                datebetween = "  and r.Adm_Date between  '" + from.ToString("MM/dd/yyyy") + "' and '" + to.ToString("MM/dd/yyyy") + "' ";
                datebetween1 = "  and a.date_applied between  '" + from.ToString("MM/dd/yyyy") + "' and '" + to.ToString("MM/dd/yyyy") + "' ";
                datebetweenCanceled = " and d.Discontinue_Date between  '" + from.ToString("dd/MM/yyyy") + "' and '" + to.ToString("dd/MM/yyyy") + "' ";
            }
            if (cb_studtypechk.Checked == true)
            {
                columnvalue = " and r.Stud_Type in('" + addstudtypeval + "') ";
            }
            if (cb_seatchk.Checked == true)
            {
                columnvalue = columnvalue + " and a.seattype in('" + addseatttt + "') ";
            }
            if (cb_state.Checked == true)
            {
                columnvalue = columnvalue + " and a.parent_statep in('" + addstate + "') ";
            }
            if (cb_typechk.Checked == true)
            {
                columnvalue = columnvalue + " and a.mode in('" + addtypeval + "') ";
            }
            if (cb_relichk.Checked == true)
            {
                columnvalue = columnvalue + " and a.religion in('" + addrelllli + "') ";
            }
            if (cb_commchk.Checked == true)
            {
                columnvalue = columnvalue + " and a.community in('" + addcommmm + "') ";
            }
            if (cbquotacheck.Checked == true)
            {
                columnvalue = columnvalue + " and a.quota in('" + addquota + "') ";//abarna
            }
            if (allotcommchk.Checked == true)//abarna
            {
                columnvalue = columnvalue + " and a.allotcomm in('" + addallotcomm + "') ";
            }
            if (chk_typename.Checked == true)//abarna
            {
                //columnvalue = columnvalue + " and a.allotcomm in('" + addallotcomm + "') ";
            }
            if (chk_user.Checked == true)
            {
                columnvalue = columnvalue + " and a.user_code in('" + adduser + "') ";//abarna
            }
            if (chkrefer.Checked == true)
            {
                columnname = columnname + " and a.direct_refer in('" + addrefer + "')";//abarna
            }
            if (addgraud == "")
            {
                lbl_err_stud.Visible = true;
                lbl_err_stud.Text = "Kindly Select The Graduation";
                return;
            }
            if (addbatch == "")
            {
                lbl_err_stud.Visible = true;
                lbl_err_stud.Text = "Kindly Select The Batch Year ";
                return;
            }
            if (adddeg == "")
            {
                lbl_err_stud.Visible = true;
                lbl_err_stud.Text = "Kindly Select The Degree ";
                return;
            }
            if (adddeg == "")
            {
                lbl_err_stud.Visible = true;
                lbl_err_stud.Text = "Kindly Select The Branch ";
                return;
            }
            if (addsem == "")
            {
                lbl_err_stud.Visible = true;
                lbl_err_stud.Text = "Kindly Select The Semester ";
                return;
            }

            if (cb_status.Checked == true)
            {
                for (i = 0; i < cbl_status.Items.Count; i++)
                {
                    if (cbl_status.Items[i].Selected == true)
                    {
                        string addstatus = cbl_status.Items[i].Text.ToString();
                        string addstatus1 = cbl_status.Items[i].Value.ToString();
                        if (statusname == "")
                        {
                            statusname = addstatus;
                        }
                        else
                        {
                            statusname = statusname + "," + addstatus;
                        }
                        if (statusnme == "")
                        {
                            statusnme = addstatus1;
                        }
                        else
                        {
                            statusnme = addcommmm + "," + addstatus1;
                        }
                    }
                }
            }
            else
            {
                appliedbool = false;
                admitedbool = false;
                delbool = false;
                debarbool = false;
                cm_coursebool = false;
            }
            int count = 0;
            int count1 = 0;
            if (cb_studtypechk.Checked == true)
            {
                count1++;
                count1 = count1++;
            }
            if (cb_seatchk.Checked == true)
            {
                count1++;
                count1 = count1++;
            }
            if (cbquotacheck.Checked == true)//abarna
            {
                count1++;
                count1 = count1++;
            }
            if (cb_state.Checked == true)//abarna
            {
                count1++;
                count1 = count1++;
            }
            if (allotcommchk.Checked == true)//abarna
            {
                count1++;
                count1 = count1++;
            }
            if (chk_user.Checked == true)//abarna
            {
                count1++;
                count1 = count1++;
            }
            if (cb_typechk.Checked == true)
            {
                count1++;
                count1 = count1++;
            }
            if (cb_relichk.Checked == true)
            {
                count1++;
                count1 = count1++;
            }
            if (cb_commchk.Checked == true)
            {
                count1++;
                count1 = count1++;
            }
            if (cb_status.Checked == true)
            {
                count1++;
                count1 = count1++;
            }
            if (cb_residency.Checked == true)
            {
                count1++;
                count1 = count1++;
            }
            if (cb_sports.Checked == true)
            {
                count1++;
                count1 = count1++;
            }
            if (cb_lang.Checked == true)
            {
                count1++;
                count1 = count1++;
            }
            if (cb_mothertng.Checked == true)
            {
                count1++;
                count1 = count1++;
            }
            if (cb_phychallange.Checked == true)
            {
                count1++;
                count1 = count1++;
            }
            if (cb_trans.Checked == true)
            {
                count1++;
                count1 = count1++;
            }
            if (cb_Gender.Checked == true)
            {
                count1++;
                count1 = count1++;
            }
            //if (cb_board.Checked == true)
            //{
            //    count1++;
            //    count1 = count1++;
            //}
            //if (cb_state.Checked == true)
            //{
            //    count1++;
            //    count1 = count1++;
            //}

            if (addstudtypeval == "")
            {
                addstudtypeval = "0";
            }
            if (addseatval == "")
            {
                addseatval = "0";
            }
            if (addtypeval == "")
            {
                addtypeval = "0";
            }
            if (addrelival == "")
            {
                addrelival = "0";
            }
            if (addcommval == "")
            {
                addcommval = "0";
            }
            if (addallotcomm == "")
            {
                addallotcomm = "0";
            }
            if (adduser == "")
            {
                adduser = "0";
            }
            if (addrefer == "")
            {
                addrefer = "0";
            }
            Fpspread1.Sheets[0].Visible = true;
            Fpspread1.Sheets[0].RowHeader.Visible = false;
            Fpspread1.CommandBar.Visible = false;
            Fpspread1.Sheets[0].AutoPostBack = true;
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread1.Sheets[0].ColumnCount = 8;
            FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
            darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle2.ForeColor = Color.Black;
            darkstyle2.HorizontalAlign = HorizontalAlign.Center;
            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
            string query = string.Empty;
            int r = 0;
            #region old query
            //query = " select COUNT(a.app_no)as TotalStrength,No_Of_seats,'' Sections,a.degree_code,a.Batch_Year,a.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name from applyn a, degree d,Department dt,Course C where d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'  and a.degree_code in('" + adddeg + "')and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "') " + datebetween1 + " group by No_Of_seats,a.degree_code ,a.Batch_Year,a.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name";
            //if (ddl_status.SelectedIndex < 2)
            //{
            //    query = query + " select COUNT(distinct a.app_no)as TotalStrength, isdisable,isdisabledisc,islearningdis,handy,s.Part1Language,visualhandy,  mother_tongue, DistinctSport, CampusReq,A.degree_code,'' Exam_Flag,0 DelFlag,''cc,A.Batch_Year,a.Current_Semester,C.Course_Name ,Dt.Dept_Name,ISNULL(admission_status,'0') as admission_status ,'' sections,'' Stud_Type,a.seattype,a.religion,a.community,a.mode from applyn a,Stud_prev_details s, degree d,Department dt,Course C where a.app_no =s.app_no  and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and isconfirm ='1'  and a.degree_code in('" + adddeg + "') and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween1 + "  group by isdisable,isdisabledisc,islearningdis,handy,s.Part1Language,visualhandy,mother_tongue,DistinctSport,CampusReq,a.degree_code ,a.Batch_Year,a.Current_Semester,C.Course_Name ,Dt.Dept_Name ,ISNULL(admission_status,'0'),Stud_Type,a.seattype,a.religion,a.community,mode";
            //    //case when CampusReq='0' then 'No' else 'Yes'  end as
            //    //query = query + " select COUNT(a.app_no)as TotalStrength, A.degree_code,'' Exam_Flag,0 DelFlag,''cc,A.Batch_Year,a.Current_Semester,ISNULL(admission_status,'0') as admission_status ,'' sections from applyn a where isconfirm ='1'  and a.degree_code in('" + adddeg + "')and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "')   and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween1 + "   group by a.degree_code ,a.Batch_Year,a.Current_Semester,ISNULL(admission_status,'0')";
            //    query = query + "   select  COUNT(distinct a.app_no)as TotalStrength, isdisable,isdisabledisc,islearningdis,handy,s.Part1Language,visualhandy,  mother_tongue, DistinctSport, CampusReq,A.degree_code,'' Exam_Flag,0 DelFlag,''cc,A.Batch_Year,a.Current_Semester,ISNULL(admission_status,'0') as admission_status ,'' sections,'' Stud_Type,a.seattype,a.religion,a.community,a. mode from Stud_prev_details s, applyn a left join Registration r on a.app_no=r.App_No  where r.App_No is null  and isconfirm ='1'   and a.degree_code in('" + adddeg + "') and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "')  and a.college_code='13' and a.app_no=s.app_no    group by isdisable,isdisabledisc,islearningdis,handy,s.Part1Language,visualhandy,mother_tongue,DistinctSport,CampusReq,a.degree_code ,a.Batch_Year,a.Current_Semester,ISNULL(admission_status,'0'),a.Stud_Type,a.seattype,a.religion,a.community,a.mode  ";
            //}
            //else
            //{
            //    query = query + "  select COUNT(a.app_no)as TotalStrength,isdisable,isdisabledisc,islearningdis,handy,s.Part1Language,visualhandy, (Select textval FROM textvaltable T WHERE mother_tongue = TextCode) mother_tongue,(Select textval FROM textvaltable T WHERE DistinctSport = TextCode) DistinctSport,case when CampusReq='0' then 'No' else 'Yes'  end as CampusReq, A.degree_code, Exam_Flag,DelFlag,cc,A.Batch_Year,a.Current_Semester,ISNULL(admission_status,'0') as admission_status ,sections,r.Stud_Type,a.seattype,a.religion,a.community,'0' mode from Registration r,Stud_prev_details s, applyn a where a.app_no =s.app_no and r.App_No =a.app_no and isconfirm ='1'  and r.degree_code in('" + adddeg + "') and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween1 + " group by isdisable,isdisabledisc,islearningdis,handy,visualhandy,mother_tongue,DistinctSport,CampusReq,a.degree_code ,a.Batch_Year,a.Current_Semester ,ISNULL(admission_status,'0'),r.Stud_Type,a.seattype,a.religion,a.community,r.mode,Exam_Flag ,DelFlag,cc,sections,r.Stud_Type,Part1Language";
            //    query = query + "  select COUNT(a.app_no)as TotalStrength,a.mode, A.degree_code,Exam_Flag,DelFlag,cc,A.Batch_Year,a.Current_Semester,ISNULL(admission_status,'0') as admission_status ,sections from applyn a,Registration r where a.app_no =r.App_No and isconfirm ='1'  and r.degree_code in('" + adddeg + "') and r.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "')  and  r.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween1 + "   group by a.degree_code ,a.Batch_Year,a.Current_Semester ,ISNULL(admission_status,'0'),Exam_Flag,DelFlag,cc,Sections,a.mode  ";
            //}
            //query = query + " select COUNT(r.app_no)as TotalStrength,r.degree_code,r.Exam_Flag,r.DelFlag,r.cc,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,admission_status,Sections from Registration r,applyn a, degree d,Department dt,Course C where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No  and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "')  and r.App_No=a.app_no      and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween + "  group by  r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name ,r.Exam_Flag,r.DelFlag,r.CC,admission_status ,Sections ";
            //query = query + " select COUNT(a.app_no)as TotalStrength, A.degree_code,'' Exam_Flag,0 DelFlag,''cc,A.Batch_Year,a.Current_Semester,C.Course_Name ,Dt.Dept_Name,ISNULL(admission_status,'0') as admission_status ,'' sections from degree d,Department dt,Course C ,applyn a where d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and isconfirm ='1'  and a.degree_code in('" + adddeg + "')and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "') " + datebetween1 + "  group by a.degree_code ,a.mode,a.Batch_Year,a.Current_Semester,C.Course_Name ,Dt.Dept_Name ,ISNULL(admission_status,'0')";
            #endregion
            string Partlanguage = string.Empty;
            if (cb_lang.Checked == true)
            {
                Partlanguage = ",Part1Language";
            }
            #region newquery
            string gender = string.Empty; string groupgender = string.Empty;
            if (cb_gen.Checked == true)
            {
                gender = " count(sex)gender,sex, ";
                groupgender = " ,sex ";
            }
            DataSet gender_ds = new DataSet();
            string q1 = string.Empty;
            string Isconfirm = " and isconfirm ='1' ";//barath 24.01.18
            if (ddl_status.SelectedItem.Value == "11")
                Isconfirm = " and isnull(IsEnquiry,0)='1' ";
            //Added By Saranyadevi 24.2.2018
            string DisConreason = "";
            if (cb_Disreaason.Checked == true)
            {
                if (cbl_reason.Items.Count > 0)
                {

                    for (int j = 0; j < cbl_reason.Items.Count; j++)
                    {
                        if (cbl_reason.Items[j].Selected == true)
                        {
                            DisConreason = cbl_reason.Items[j].Text;
                            if (DisConreason.ToLower() == "empty")
                            {
                                DisConreason = "";
                            }
                            DisContinuereason = DisContinuereason + "','" + DisConreason;

                        }
                    }


                }
            }
            if (ddl_status.SelectedItem.Value == "12")
            {
                q1 = " select count(sex)gendercount,sex,a.degree_code,a.Batch_Year,a.Current_Semester,c.Course_Id,admission_status  from applyn a, degree d,Department dt,Course C where  d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id " + Isconfirm + "  and a.degree_code in('" + adddeg + "')and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "')  group by No_Of_seats,a.degree_code ,a.Batch_Year,a.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name,sex,admission_status ";
            }
            else
            {
                if (ddl_status.SelectedIndex >= 3 && ddl_status.SelectedItem.Value != "11")// ISNULL(r.sections,'') delsi2701
                {
                    q1 = "  select count(sex)gendercount,sex,r.degree_code,r.Batch_Year,r.Current_Semester,c.Course_Id,ISNULL(r.sections,'')as sections,admission_status,CC,DelFlag, Exam_Flag,ProlongAbsent from applyn a,Registration r, degree d,Department dt,Course C where a.app_no =r.App_No and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'  and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "')   group by No_Of_seats,r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name,sex,ISNULL(r.sections,''),CC,DelFlag,ProlongAbsent,Exam_Flag,admission_status ";
                }
                else
                {
                    q1 = " select count(sex)gendercount,sex,a.degree_code,a.Batch_Year,a.Current_Semester,c.Course_Id,admission_status  from applyn a, degree d,Department dt,Course C where  d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id " + Isconfirm + "  and a.degree_code in('" + adddeg + "')and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "')  group by No_Of_seats,a.degree_code ,a.Batch_Year,a.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name,sex,admission_status ";
                }
            }
            if (q1.Trim() != "")
            {
                gender_ds = d2.select_method_wo_parameter(q1, "text");
            }

            if (ddl_status.SelectedIndex < 2 || ddl_status.SelectedItem.Value == "11")
            {
                if (ddl_status.SelectedItem.Value == "12")
                {
                    //query = " select COUNT(distinct a.app_no)as TotalStrength,No_Of_seats,'' Sections,a.degree_code,a.Batch_Year,a.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name from applyn a, degree d,Department dt,Course C  where  d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id " + Isconfirm + "  and a.degree_code in('" + adddeg + "')and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "') " + datebetween1 + " group by No_Of_seats,a.degree_code ,a.Batch_Year,a.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name";
                }
                else
                {
                    query = " select COUNT(distinct a.app_no)as TotalStrength,No_Of_seats,'' Sections,a.degree_code,a.Batch_Year,a.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name from applyn a, degree d,Department dt,Course C  where  d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id " + Isconfirm + "  and a.degree_code in('" + adddeg + "')and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "') " + datebetween1 + " group by No_Of_seats,a.degree_code ,a.Batch_Year,a.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name";//s.app_no=a.app_no and ,Stud_prev_details s
                }
                if (chk_typename.Checked == true || chk_typesizename.Checked == true)
                {
                    query = query + " select COUNT(distinct a.app_no)as TotalStrength, isdisable,isdisabledisc,islearningdis,handy " + Partlanguage + ",visualhandy,  mother_tongue, DistinctSport, CampusReq,A.degree_code,'' Exam_Flag,0 DelFlag,''cc,A.Batch_Year,a.Current_Semester,C.Course_Name ,Dt.Dept_Name,ISNULL(admission_status,'0') as admission_status ,'' sections,'' Stud_Type,a.seattype,a.quota,a.religion,a.community,a.mode,a.allotcomm,s.typenamevalue,s.typesizevalue,a.parent_statep from applyn a, degree d,Department dt,Course C,St_personalInfod s where s.appno=a.app_no and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id   " + Isconfirm + "   and a.degree_code in('" + adddeg + "') and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween1 + "  group by isdisable,isdisabledisc,islearningdis,handy " + Partlanguage + ",visualhandy,mother_tongue,DistinctSport,CampusReq,a.degree_code ,a.Batch_Year,a.Current_Semester,C.Course_Name ,Dt.Dept_Name ,ISNULL(admission_status,'0'),Stud_Type,a.seattype,a.quota,a.religion,a.community,mode,a.allotcomm,typenamevalue,typesizevalue,a.parent_statep";//,Stud_prev_details s a.app_no =s.app_no  and abar
                    //case when CampusReq='0' then 'No' else 'Yes'  end as
                    //query = query + " select COUNT(a.app_no)as TotalStrength, A.degree_code,'' Exam_Flag,0 DelFlag,''cc,A.Batch_Year,a.Current_Semester,ISNULL(admission_status,'0') as admission_status ,'' sections from applyn a where isconfirm ='1'  and a.degree_code in('" + adddeg + "')and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "')   and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween1 + "   group by a.degree_code ,a.Batch_Year,a.Current_Semester,ISNULL(admission_status,'0')";
                    query = query + "   select selection_status,Is_enroll, COUNT(distinct a.app_no)as TotalStrength, isdisable,isdisabledisc,islearningdis,handy " + Partlanguage + ",visualhandy,  mother_tongue, DistinctSport, CampusReq,A.degree_code,'' Exam_Flag,0 DelFlag,''cc,A.Batch_Year,a.Current_Semester,ISNULL(admission_status,'0') as admission_status ,'' sections,'' Stud_Type,a.seattype,a.quota,a.religion,a.community,a. mode,a.allotcomm,typenamevalue,s.typesizevalue,a.parent_statep from applyn a,St_personalInfod s where  s.appno=a.app_no and a.degree_code in('" + adddeg + "')  " + Isconfirm + "  and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "')  and a.college_code='" + ddlcollege.SelectedItem.Value + "'     group by isdisable,isdisabledisc,islearningdis,handy " + Partlanguage + ",visualhandy,mother_tongue,DistinctSport,CampusReq,a.degree_code ,a.Batch_Year,a.Current_Semester,ISNULL(admission_status,'0'),a.Stud_Type,a.seattype,a.quota,a.religion,a.community,a.mode,Is_enroll,selection_status,a.allotcomm,typenamevalue,typesizevalue,a.parent_statep ";
                }
                else
                {
                    query = query + " select COUNT(distinct a.app_no)as TotalStrength, isdisable,isdisabledisc,islearningdis,handy " + Partlanguage + ",visualhandy,  mother_tongue, DistinctSport, CampusReq,A.degree_code,'' Exam_Flag,0 DelFlag,''cc,A.Batch_Year,a.Current_Semester,C.Course_Name ,Dt.Dept_Name,ISNULL(admission_status,'0') as admission_status ,'' sections,'' Stud_Type,a.seattype,a.quota,a.religion,a.community,a.mode,a.allotcomm,a.user_code,a.parent_statep,a.direct_refer from applyn a, degree d,Department dt,Course C where  d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id   " + Isconfirm + "   and a.degree_code in('" + adddeg + "') and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween1 + "  group by isdisable,isdisabledisc,islearningdis,handy " + Partlanguage + ",visualhandy,mother_tongue,DistinctSport,CampusReq,a.degree_code ,a.Batch_Year,a.Current_Semester,C.Course_Name ,Dt.Dept_Name ,ISNULL(admission_status,'0'),Stud_Type,a.seattype,a.quota,a.religion,a.community,mode,a.allotcomm,a.user_code,a.parent_statep,a.direct_refer";//,Stud_prev_details s a.app_no =s.app_no  and abar
                    //case when CampusReq='0' then 'No' else 'Yes'  end as
                    //query = query + " select COUNT(a.app_no)as TotalStrength, A.degree_code,'' Exam_Flag,0 DelFlag,''cc,A.Batch_Year,a.Current_Semester,ISNULL(admission_status,'0') as admission_status ,'' sections from applyn a where isconfirm ='1'  and a.degree_code in('" + adddeg + "')and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "')   and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween1 + "   group by a.degree_code ,a.Batch_Year,a.Current_Semester,ISNULL(admission_status,'0')";
                    query = query + "   select selection_status,Is_enroll, COUNT(distinct a.app_no)as TotalStrength, isdisable,isdisabledisc,islearningdis,handy " + Partlanguage + ",visualhandy,  mother_tongue, DistinctSport, CampusReq,A.degree_code,'' Exam_Flag,0 DelFlag,''cc,A.Batch_Year,a.Current_Semester,ISNULL(admission_status,'0') as admission_status ,'' sections,'' Stud_Type,a.seattype,a.quota,a.religion,a.community,a. mode,a.allotcomm,a.parent_statep from applyn a where   a.degree_code in('" + adddeg + "')  " + Isconfirm + "  and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "')  and a.college_code='" + ddlcollege.SelectedItem.Value + "'     group by isdisable,isdisabledisc,islearningdis,handy " + Partlanguage + ",visualhandy,mother_tongue,DistinctSport,CampusReq,a.degree_code ,a.Batch_Year,a.Current_Semester,ISNULL(admission_status,'0'),a.Stud_Type,a.seattype,a.quota,a.religion,a.community,a.mode,Is_enroll,selection_status,a.allotcomm,a.parent_statep  ";//
                }//Stud_prev_details s, and a.app_no=s.app_no
                //abar
                if (ddl_status.SelectedItem.Value == "9" || ddl_status.SelectedItem.Value == "10")//18.01.18 barath
                {
                    //=======Added By Saranya 06/01/2018 For Transport and Hostel Canceled Student=========//
                    query = query + " select COUNT(distinct d.app_no)as TotalStrength,isnull( r.Sections,'') as Sections,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,isdisable,isdisabledisc,islearningdis,handy " + Partlanguage + ",visualhandy,  mother_tongue, DistinctSport,'' Stud_Type,a.seattype,a.quota,a.religion,a.community,a. mode,d.boarding,d.bus_routeid,d.vehid,a.allotcomm,a.parent_statep from Registration r,Discontinue d,applyn a, degree de,Department dt,Course C where de.Degree_Code =r.degree_code and de.Dept_Code =dt.Dept_Code and c.Course_Id =de.Course_Id and r.App_No=d.app_no and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "' and d.Catogery=4 and a.app_no =r.App_No " + datebetweenCanceled + " group by isdisable,isdisabledisc,islearningdis,handy " + Partlanguage + ",visualhandy,mother_tongue,DistinctSport,isnull( r.Sections,''),r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,d.boarding,d.bus_routeid,d.vehid,a.Stud_Type,a.seattype,a.quota,a.religion,a.community,a.mode,a.allotcomm,a.parent_statep ";//abar
                    query = query + " select COUNT(distinct d.app_no)as TotalStrength,isnull( r.Sections,'') as Sections,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,isdisable,isdisabledisc,islearningdis,handy " + Partlanguage + ",visualhandy,  mother_tongue, DistinctSport,'' Stud_Type,a.seattype,a.quota,a.religion,a.community,a. mode,d.buildingname,d.hostelname,d.roomname,a.allotcomm,a.parent_statep from Registration r,Discontinue d,applyn a, degree de,Department dt,Course C where de.Degree_Code =r.degree_code and de.Dept_Code =dt.Dept_Code and c.Course_Id =de.Course_Id and r.App_No=d.app_no and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "' and d.Catogery=3 and a.app_no =r.App_No " + datebetweenCanceled + " group by isdisable,isdisabledisc,islearningdis,handy " + Partlanguage + ",visualhandy,mother_tongue,DistinctSport,isnull( r.Sections,''),r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,d.buildingname,d.hostelname,d.roomname,a.Stud_Type,a.seattype,a.quota,a.religion,a.community,a.mode,a.allotcomm,a.parent_statep ";
                }
                //========================================================================//
            }
            else
            {
                string values = newfunction(Convert.ToString(ddl_status.SelectedItem.Value));

                //============cmd saranyadevi24.2.2018===================
                //query = "  select COUNT(a.app_no)as TotalStrength,No_Of_seats, Sections,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name from applyn a,Registration r, degree d,Department dt,Course C where a.app_no =r.App_No and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'  and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "')  " + values + " group by No_Of_seats,r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name,Sections";//and CC=0 and DelFlag =0 and Exam_Flag <>'Debar'

                //query = query + "  select COUNT(distinct a.app_no)as TotalStrength,r.Boarding,isdisable,isdisabledisc,islearningdis,handy " + Partlanguage + ",visualhandy, (Select textval FROM textvaltable T WHERE TextCode =ISNULL(mother_tongue,0))as mother_tongue,(Select textval FROM textvaltable T WHERE convert(varchar, TextCode)=isnull(DistinctSport,0)) DistinctSport,case when CampusReq='0' then 'No' else 'Yes'  end as CampusReq, r.degree_code, Exam_Flag,DelFlag,cc,ProlongAbsent,r.Batch_Year,r.Current_Semester,ISNULL(admission_status,'0') as admission_status ,sections,r.Stud_Type,a.seattype,a.religion,a.community,r. mode from Registration r, applyn a where r.App_No =a.app_no and isconfirm ='1'  and r.degree_code in('" + adddeg + "') and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween1 + "   group by isdisable,isdisabledisc,islearningdis,handy,visualhandy,mother_tongue,DistinctSport,CampusReq,r.degree_code ,r.Batch_Year,r.Current_Semester ,ISNULL(admission_status,'0'),r.Stud_Type,a.seattype,a.religion,a.community,r.mode,Exam_Flag ,DelFlag,cc,ProlongAbsent,sections,r.Stud_Type" + Partlanguage + ",r.Boarding";//,Stud_prev_details s a.app_no =s.app_no and 
                //query = query + "  select COUNT(a.app_no)as TotalStrength,a.mode, A.degree_code,Exam_Flag,DelFlag,cc,ProlongAbsent,A.Batch_Year,a.Current_Semester,ISNULL(admission_status,'0') as admission_status ,sections from applyn a,Registration r where a.app_no =r.App_No and isconfirm ='1'  and r.degree_code in('" + adddeg + "') and r.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "')  and  r.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween1 + "   group by a.degree_code ,a.Batch_Year,a.Current_Semester ,ISNULL(admission_status,'0'),Exam_Flag,DelFlag,cc,ProlongAbsent,Sections,a.mode  ";

                //============Modified saranyadevi24.2.2018===================
                if (ddl_status.SelectedItem.Value == "12")
                {
                    query = " select COUNT(distinct a.app_no)as TotalStrength,No_Of_seats,'' Sections,a.degree_code,a.Batch_Year,a.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name from applyn a, degree d,Department dt,Course C  where  d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id " + Isconfirm + "  and a.degree_code in('" + adddeg + "')and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "') " + datebetween1 + " group by No_Of_seats,a.degree_code ,a.Batch_Year,a.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name";
                    //query = " select count(sex)gendercount,sex,a.degree_code,a.Batch_Year,a.Current_Semester,c.Course_Id,admission_status  from applyn a, degree d,Department dt,Course C where  d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id " + Isconfirm + "  and a.degree_code in('" + adddeg + "')and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "')  group by No_Of_seats,a.degree_code ,a.Batch_Year,a.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name,sex,admission_status ";
                }
                else
                {
                    if (ddl_status.SelectedItem.Value == "5")//saranyadevi Modification
                    {
                        query = "  select COUNT(a.app_no)as TotalStrength,No_Of_seats, Sections,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name from applyn a,Registration r, degree d,Department dt,Course C,Discontinue dis  where a.app_no =r.App_No and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and dis.app_no=r.App_No and a.app_no=dis.app_no and isconfirm ='1'  and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "')  and dis.Reason in('" + DisContinuereason + "')   " + values + " group by No_Of_seats,r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name,Sections";
                        query = query + "  select COUNT(distinct a.app_no)as TotalStrength,r.Boarding,isdisable,isdisabledisc,islearningdis,handy " + Partlanguage + ",visualhandy, (Select textval FROM textvaltable T WHERE TextCode =ISNULL(mother_tongue,0))as mother_tongue,(Select textval FROM textvaltable T WHERE convert(varchar, TextCode)=isnull(DistinctSport,0)) DistinctSport,case when CampusReq='0' then 'No' else 'Yes'  end as CampusReq, r.degree_code, Exam_Flag,DelFlag,cc,ProlongAbsent,r.Batch_Year,r.Current_Semester,ISNULL(admission_status,'0') as admission_status ,sections,r.Stud_Type,a.seattype,a.quota,a.religion,a.community,r. mode,a.allotcomm,a.parent_statep from Registration r, applyn a,Discontinue d where r.App_No =a.app_no and isconfirm ='1'  and r.degree_code in('" + adddeg + "') and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween1 + " and d.app_no=a.app_no and d.app_no=r.App_No  and d.Reason in('" + DisContinuereason + "') group by isdisable,isdisabledisc,islearningdis,handy,visualhandy,mother_tongue,DistinctSport,CampusReq,r.degree_code ,r.Batch_Year,r.Current_Semester ,ISNULL(admission_status,'0'),r.Stud_Type,a.seattype,a.quota,a.religion,a.community,a.allotcomm,r.mode,Exam_Flag ,DelFlag,cc,ProlongAbsent,sections,r.Stud_Type" + Partlanguage + ",r.Boarding,a.parent_statep";//,Stud_prev_details s a.app_no =s.app_no and 
                        query = query + "  select COUNT(a.app_no)as TotalStrength,a.mode, A.degree_code,Exam_Flag,DelFlag,cc,ProlongAbsent,A.Batch_Year,a.Current_Semester,ISNULL(admission_status,'0') as admission_status ,sections from applyn a,Registration r,Discontinue dis where a.app_no =r.App_No and dis.app_no=r.App_No and a.app_no=dis.app_no  and isconfirm ='1'  and r.degree_code in('" + adddeg + "') and r.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "')  and  dis.Reason in('" + DisContinuereason + "') and r.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween1 + "   group by a.degree_code ,a.Batch_Year,a.Current_Semester ,ISNULL(admission_status,'0'),Exam_Flag,DelFlag,cc,ProlongAbsent,Sections,a.mode  ";
                    }

                    else//old query Delsi
                    {
                        if (chk_typename.Checked == true || chk_typesizename.Checked == true)
                        {
                            query = "  select COUNT(a.app_no)as TotalStrength,No_Of_seats, Sections,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name,s.typenamevalue,s.typesizevalue from applyn a,Registration r, degree d,Department dt,Course C,St_personalInfod s where a.app_no =r.App_No and s.appno =a.app_no   and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'  and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "')  " + values + " group by No_Of_seats,r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name,Sections,typenamevalue,typesizevalue";//and CC=0 and DelFlag =0 and Exam_Flag <>'Debar'modified 11

                            query = query + "  select COUNT(distinct a.app_no)as TotalStrength,r.Boarding,isdisable,isdisabledisc,islearningdis,handy " + Partlanguage + ",visualhandy, (Select textval FROM textvaltable T WHERE TextCode =ISNULL(mother_tongue,0))as mother_tongue,(Select textval FROM textvaltable T WHERE convert(varchar, TextCode)=isnull(DistinctSport,0)) DistinctSport,case when CampusReq='0' then 'No' else 'Yes'  end as CampusReq, r.degree_code, Exam_Flag,DelFlag,cc,ProlongAbsent,r.Batch_Year,r.Current_Semester,ISNULL(admission_status,'0') as admission_status ,sections,r.Stud_Type,a.seattype,a.quota,a.religion,a.community,r. mode,a.allotcomm,s.typenamevalue,s.typesizevalue,a.parent_statep from Registration r, applyn a,St_personalInfod s where s.appno =a.app_no and r.App_No =a.app_no  and isconfirm ='1'  and r.degree_code in('" + adddeg + "') and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween1 + "   group by isdisable,isdisabledisc,islearningdis,handy,visualhandy,mother_tongue,DistinctSport,CampusReq,r.degree_code ,r.Batch_Year,r.Current_Semester ,ISNULL(admission_status,'0'),r.Stud_Type,a.seattype,a.quota,a.religion,a.community,r.mode,Exam_Flag ,DelFlag,cc,ProlongAbsent,sections,r.Stud_Type" + Partlanguage + ",r.Boarding,a.allotcomm,typenamevalue,typesizevalue,a.parent_statep";//,Stud_prev_details s a.app_no =s.app_no and aba
                            query = query + "  select COUNT(a.app_no)as TotalStrength,a.mode, A.degree_code,Exam_Flag,DelFlag,cc,ProlongAbsent,A.Batch_Year,a.Current_Semester,ISNULL(admission_status,'0') as admission_status ,sections,typenamevalue,s.typesizevalue from applyn a,Registration r,St_personalInfod s where s.appno =a.app_no and a.app_no =r.App_No and isconfirm ='1'  and r.degree_code in('" + adddeg + "') and r.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "')  and  r.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween1 + "   group by a.degree_code ,a.Batch_Year,a.Current_Semester ,ISNULL(admission_status,'0'),Exam_Flag,DelFlag,cc,ProlongAbsent,Sections,a.mode,typenamevalue,typesizevalue ";
                        }
                        else
                        {
                            query = "  select COUNT(a.app_no)as TotalStrength,No_Of_seats, Sections,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name from applyn a,Registration r, degree d,Department dt,Course C where a.app_no =r.App_No  and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'  and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "')  " + values + " group by No_Of_seats,r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name,Sections";//and CC=0 and DelFlag =0 and Exam_Flag <>'Debar'modified 11
                            if (cb_board.Checked == true)
                            {
                                query = query + "  select COUNT(distinct a.app_no)as TotalStrength,r.Boarding,isdisable,isdisabledisc,islearningdis,handy " + Partlanguage + ",visualhandy, (Select textval FROM textvaltable T WHERE TextCode =ISNULL(mother_tongue,0))as mother_tongue,(Select textval FROM textvaltable T WHERE convert(varchar, TextCode)=isnull(DistinctSport,0)) DistinctSport,case when CampusReq='0' then 'No' else 'Yes'  end as CampusReq, r.degree_code, Exam_Flag,DelFlag,cc,ProlongAbsent,r.Batch_Year,r.Current_Semester,ISNULL(admission_status,'0') as admission_status ,sections,r.Stud_Type,a.seattype,a.quota,a.religion,a.community,r. mode,a.allotcomm,s.course_code,a.parent_statep from Registration r, applyn a,Stud_prev_details s where  a.app_no =s.app_no and r.App_No =a.app_no  and isconfirm ='1'  and r.degree_code in('" + adddeg + "') and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween1 + "   group by isdisable,isdisabledisc,islearningdis,handy,visualhandy,mother_tongue,DistinctSport,CampusReq,r.degree_code ,r.Batch_Year,r.Current_Semester ,ISNULL(admission_status,'0'),r.Stud_Type,a.seattype,a.quota,a.religion,a.community,r.mode,Exam_Flag ,DelFlag,cc,ProlongAbsent,sections,r.Stud_Type" + Partlanguage + ",r.Boarding,a.allotcomm,s.course_code,a.parent_statep";
                            }
                            else
                            {
                                query = query + "  select COUNT(distinct a.app_no)as TotalStrength,r.Boarding,isdisable,isdisabledisc,islearningdis,handy " + Partlanguage + ",visualhandy, (Select textval FROM textvaltable T WHERE TextCode =ISNULL(mother_tongue,0))as mother_tongue,(Select textval FROM textvaltable T WHERE convert(varchar, TextCode)=isnull(DistinctSport,0)) DistinctSport,case when CampusReq='0' then 'No' else 'Yes'  end as CampusReq, r.degree_code, Exam_Flag,DelFlag,cc,ProlongAbsent,r.Batch_Year,r.Current_Semester,ISNULL(admission_status,'0') as admission_status ,sections,r.Stud_Type,a.seattype,a.quota,a.religion,a.community,r. mode,a.allotcomm,r.entryusercode,a.parent_statep,a.direct_refer from Registration r, applyn a,Stud_prev_details s   where  r.App_No =a.app_no and a.app_no =s.app_no  and isconfirm ='1'  and r.degree_code in('" + adddeg + "') and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween1 + "   group by isdisable,isdisabledisc,islearningdis,handy,visualhandy,mother_tongue,DistinctSport,CampusReq,r.degree_code ,r.Batch_Year,r.Current_Semester ,ISNULL(admission_status,'0'),r.Stud_Type,a.seattype,a.quota,a.religion,a.community,r.mode,Exam_Flag ,DelFlag,cc,ProlongAbsent,sections,r.Stud_Type" + Partlanguage + ",r.Boarding,a.allotcomm,r.entryusercode,a.parent_statep,a.direct_refer";//,Stud_prev_details s a.app_no =s.app_no and aba
                            }
                            query = query + "  select COUNT(a.app_no)as TotalStrength,a.mode, A.degree_code,Exam_Flag,DelFlag,cc,ProlongAbsent,A.Batch_Year,a.Current_Semester,ISNULL(admission_status,'0') as admission_status ,sections from applyn a,Registration r where a.app_no =r.App_No and isconfirm ='1'  and r.degree_code in('" + adddeg + "') and r.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "')  and  r.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween1 + "   group by a.degree_code ,a.Batch_Year,a.Current_Semester ,ISNULL(admission_status,'0'),Exam_Flag,DelFlag,cc,ProlongAbsent,Sections,a.mode  ";
                        }

                    }

                }
                if (ddl_status.SelectedItem.Value == "9" || ddl_status.SelectedItem.Value == "10")//18.01.18 barath
                {
                    //=======Added By Saranya 06/01/2018 For transport Canceled Student=========//
                    query = query + " select COUNT(distinct d.app_no)as TotalStrength,isnull( r.Sections,'') as Sections,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,isdisable,isdisabledisc,islearningdis,handy " + Partlanguage + ",visualhandy,  mother_tongue, DistinctSport,'' Stud_Type,a.seattype,a.quota,a.religion,a.community,a. mode,d.boarding,d.bus_routeid,d.vehid,a.allotcomm,a.parent_statep from Registration r,Discontinue d,applyn a, degree de,Department dt,Course C where de.Degree_Code =r.degree_code and de.Dept_Code =dt.Dept_Code and c.Course_Id =de.Course_Id and r.App_No=d.app_no and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "' and d.Catogery=4 and a.app_no =r.App_No " + datebetweenCanceled + " group by isdisable,isdisabledisc,islearningdis,handy " + Partlanguage + ",visualhandy,mother_tongue,DistinctSport,isnull( r.Sections,''),r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,d.boarding,d.bus_routeid,d.vehid,a.Stud_Type,a.seattype,a.quota,a.religion,a.community,a.mode,a.allotcomm,a.parent_statep ";
                    query = query + " select COUNT(distinct d.app_no)as TotalStrength,isnull( r.Sections,'') as Sections,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,isdisable,isdisabledisc,islearningdis,handy " + Partlanguage + ",visualhandy,  mother_tongue, DistinctSport,'' Stud_Type,a.seattype,a.quota,a.religion,a.community,a. mode,d.buildingname,d.hostelname,d.roomname,a.allotcomm,a.parent_statep from Registration r,Discontinue d,applyn a, degree de,Department dt,Course C where de.Degree_Code =r.degree_code and de.Dept_Code =dt.Dept_Code and c.Course_Id =de.Course_Id and r.App_No=d.app_no and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "' and d.Catogery=3 and a.app_no =r.App_No " + datebetweenCanceled + " group by isdisable,isdisabledisc,islearningdis,handy " + Partlanguage + ",visualhandy,mother_tongue,DistinctSport,isnull( r.Sections,''),r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,d.buildingname,d.hostelname,d.roomname,a.Stud_Type,a.seattype,a.quota,a.religion,a.community,a.mode,a.allotcomm,a.parent_statep ";
                    //========================================================================//
                }
            }
            //=========================cmd by saranyadevi 24.2.2018===================
            //query = query + " select COUNT(distinct a.app_no)as TotalStrength,r.Boarding,r.degree_code,r.Exam_Flag,r.DelFlag,r.cc,r.ProlongAbsent,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,admission_status,Sections from Registration r,applyn a, degree d,Department dt,Course C where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No  and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "')  and r.App_No=a.app_no  and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween + "  group by  r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name ,r.Exam_Flag,r.DelFlag,r.CC,r.ProlongAbsent,admission_status ,Sections,r.Boarding ";//, Stud_prev_details s s.app_no=a.app_no and 
            //query = query + " select COUNT(distinct a.app_no)as TotalStrength, A.degree_code,'' Exam_Flag,0 ,'' ProlongAbsent,0 DelFlag,''cc,A.Batch_Year,a.Current_Semester,C.Course_Name ,Dt.Dept_Name,ISNULL(admission_status,'0') as admission_status ,'' sections,'0'selection_status from degree d,Department dt,Course C ,applyn a  where  d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and isconfirm ='1'  and a.degree_code in('" + adddeg + "')and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "') " + datebetween1 + "  group by a.degree_code ,a.mode,a.Batch_Year,a.Current_Semester,C.Course_Name ,Dt.Dept_Name ,ISNULL(admission_status,'0')";//,Stud_prev_details s s.app_no=a.app_no and

            //=========================Modifie by saranyadevi 24.2.2018===================

            if (ddl_status.SelectedItem.Value == "5")//saranyadevi
            {
                query = query + " select COUNT(distinct a.app_no)as TotalStrength,r.Boarding,r.degree_code,r.Exam_Flag,r.DelFlag,r.cc,r.ProlongAbsent,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,admission_status,Sections from Registration r,applyn a, degree d,Department dt,Course C,Discontinue dis  where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No and dis.app_no=r.App_No and a.app_no=dis.app_no  and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "')  and dis.Reason in('" + DisContinuereason + "')   and r.App_No=a.app_no  and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween + "  group by  r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name ,r.Exam_Flag,r.DelFlag,r.CC,r.ProlongAbsent,admission_status ,Sections,r.Boarding ";//, Stud_prev_details s s.app_no=a.app_no and 


                query = query + " select COUNT(distinct a.app_no)as TotalStrength, A.degree_code,'' Exam_Flag,0 ,'' ProlongAbsent,0 DelFlag,''cc,A.Batch_Year,a.Current_Semester,C.Course_Name ,Dt.Dept_Name,ISNULL(admission_status,'0') as admission_status ,'' sections,'0'selection_status from degree d,Department dt,Course C ,applyn a,Discontinue dis  where  d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and a.app_no=dis.app_no and isconfirm ='1'  and a.degree_code in('" + adddeg + "')and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "') " + datebetween1 + " and dis.Reason in('" + DisContinuereason + "')  group by a.degree_code ,a.mode,a.Batch_Year,a.Current_Semester,C.Course_Name ,Dt.Dept_Name ,ISNULL(admission_status,'0')";//,Stud_prev_details s s.app_no=a.app_no and
            }
            else//delsi old query
            {

                query = query + " select COUNT(distinct a.app_no)as TotalStrength,r.Boarding,r.degree_code,r.Exam_Flag,r.DelFlag,r.cc,r.ProlongAbsent,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,admission_status,Sections from Registration r,applyn a, degree d,Department dt,Course C  where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No and  r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "')  and r.App_No=a.app_no  and r.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween + "  group by  r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name ,r.Exam_Flag,r.DelFlag,r.CC,r.ProlongAbsent,admission_status ,Sections,r.Boarding ";//,  Stud_prev_details s s.app_no=a.app_no   and
                query = query + " select COUNT(distinct a.app_no)as TotalStrength, A.degree_code,'' Exam_Flag,0 ,'' ProlongAbsent,0 DelFlag,''cc,A.Batch_Year,a.Current_Semester,C.Course_Name ,Dt.Dept_Name,ISNULL(admission_status,'0') as admission_status ,'' sections,'0'selection_status from degree d,Department dt,Course C ,applyn a  where  d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and isconfirm ='1'  and a.degree_code in('" + adddeg + "')and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "') " + datebetween1 + "  group by a.degree_code ,a.mode,a.Batch_Year,a.Current_Semester,C.Course_Name ,Dt.Dept_Name ,ISNULL(admission_status,'0')";//,Stud_prev_details s s.app_no=a.app_no and
            }
            //if (chk_typename.Checked == true)
            //{
            //    query = string.Empty;
            //    query ="select 
            //}
            #endregion
            ds = d2.select_method_wo_parameter(query, "Text");
            if (query == "")
            {
                Fpspread1.Sheets[0].Visible = false;
                lbl_err_stud.Visible = true;
                lbl_err_stud.Text = "Kindly Select All List ";
                return;
            }
            else
            {
                if (query != "")
                {
                    //ds = d2.select_method(query, hat, "Text");
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        Fpspread1.Sheets[0].Visible = false;
                        Fpspread1.Visible = false;
                        lbl_err_stud.Visible = true;
                        lbl_err_stud.Text = "No Records Found";
                        lbl_headernamespd2.Visible = false;
                        btn_viewsprd2.Visible = false;
                        lnk_admisstionformbtn.Visible = false;
                        img_settingpdf.Visible = false;
                        Fpspread2.Visible = false;
                        div_report.Visible = false;
                        rptprint.Visible = false;
                        return;
                    }
                    else
                    {
                        lbl_err_stud.Visible = false;
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                            //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = lbl_degree.Text;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = lbl_branch.Text;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = lbl_org_sem.Text;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Section";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Total Seat Allotment";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Total Strength";
                            Fpspread1.Sheets[0].Columns[7].Visible = false;
                            #region
                            if (count1 != 0)
                            {
                                int cc = 7;
                                int j = 0;
                                if (cb_status.Checked == true)
                                {
                                    cc++;
                                    Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = Convert.ToString(ddl_status.SelectedItem.Text);
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "Status";
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                    cccomm = cc;
                                }
                                if (cb_Gender.Checked == true)
                                {
                                    cc++;
                                    Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = "Male";
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "Status";
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                    cc++;
                                    Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = "Female";
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "Status";
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                    cccomm = cc;
                                }
                                if (cb_studtypechk.Checked == true)
                                {
                                    for (j = 0; j < cbl_studtype.Items.Count; j++)
                                    {
                                        if (cbl_studtype.Items[j].Selected == true)
                                        {
                                            cc++;
                                            Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = cbl_studtype.Items[j].Text;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "Stud_Type";
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Note = "Stud_Type" + "-" + cbl_studtype.Items[j].Value;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                            ccstud = cc;
                                        }
                                    }
                                }
                                if (cb_seatchk.Checked == true)
                                {
                                    for (j = 0; j < cbl_seat.Items.Count; j++)
                                    {
                                        if (cbl_seat.Items[j].Selected == true)
                                        {
                                            cc++;
                                            Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = cbl_seat.Items[j].Text;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "seattype";
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Note = "seattype" + "-" + cbl_seat.Items[j].Value;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                            ccseat = cc;
                                        }
                                    }
                                }
                                if (cbquotacheck.Checked == true)//abarna
                                {
                                    for (j = 0; j < cblQuota.Items.Count; j++)
                                    {
                                        if (cblQuota.Items[j].Selected == true)
                                        {
                                            cc++;
                                            Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = cblQuota.Items[j].Text;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "quota";
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Note = "quota" + "-" + cblQuota.Items[j].Value;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                            ccquo = cc;
                                        }
                                    }
                                }
                                if (cb_state.Checked == true)//abarna
                                {
                                    for (j = 0; j < cbl_state.Items.Count; j++)
                                    {
                                        if (cbl_state.Items[j].Selected == true)
                                        {
                                            cc++;
                                            Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = cbl_state.Items[j].Text;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "parent_statep";
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Note = "parent_statep" + "-" + cbl_state.Items[j].Value;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                            ccstate = cc;
                                        }
                                    }
                                }
                                if (cb_board.Checked == true)
                                {
                                    for (j = 0; j < cbl_BoardUniv.Items.Count; j++)
                                    {
                                        if (cbl_BoardUniv.Items[j].Selected == true)
                                        {
                                            cc++;
                                            Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = cbl_BoardUniv.Items[j].Text;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "course_code";
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Note = "course_code" + "-" + cbl_BoardUniv.Items[j].Value;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                            ccboard = cc;
                                        }
                                    }
                                }
                                if (cb_typechk.Checked == true)
                                {
                                    for (j = 0; j < cbl_type.Items.Count; j++)
                                    {
                                        if (cbl_type.Items[j].Selected == true)
                                        {
                                            cc++;
                                            Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = cbl_type.Items[j].Text;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "mode";
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Note = "mode" + "-" + cbl_type.Items[j].Value;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                            ccmode = cc;
                                        }
                                    }
                                }
                                if (cb_relichk.Checked == true)
                                {
                                    for (j = 0; j < cbl_religion.Items.Count; j++)
                                    {
                                        if (cbl_religion.Items[j].Selected == true)
                                        {
                                            cc++;
                                            Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = cbl_religion.Items[j].Text;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "religion";
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Note = "religion" + "-" + cbl_religion.Items[j].Value;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                            ccreli = cc;
                                        }
                                    }
                                }
                                if (cb_commchk.Checked == true)
                                {
                                    for (j = 0; j < cbl_comm.Items.Count; j++)
                                    {
                                        if (cbl_comm.Items[j].Selected == true)
                                        {
                                            cc++;
                                            Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = cbl_comm.Items[j].Text;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "community";
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Note = "community" + "-" + cbl_comm.Items[j].Value;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                            cccomm = cc;
                                        }
                                    }
                                }
                                if (allotcommchk.Checked == true)//abarna
                                {
                                    for (j = 0; j < cbl_allotcomm.Items.Count; j++)
                                    {
                                        if (cbl_allotcomm.Items[j].Selected == true)
                                        {
                                            cc++;
                                            Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = cbl_allotcomm.Items[j].Text;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "allotcomm";
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Note = "allotcomm" + "-" + cbl_allotcomm.Items[j].Value;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                            ccallotcomm = cc;
                                        }
                                    }
                                }
                                if (chk_typename.Checked == true)//abarna
                                {
                                    for (j = 0; j < cbl_typename.Items.Count; j++)
                                    {
                                        if (cbl_typename.Items[j].Selected == true)
                                        {
                                            cc++;
                                            Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = cbl_typename.Items[j].Text;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "typenamevalue";
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Note = "typenamevalue" + "-" + cbl_typename.Items[j].Value;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                            cctype = cc;
                                        }
                                    }
                                }
                                if (chk_typesizename.Checked == true)//abarna
                                {
                                    for (j = 0; j < cbl_typesize.Items.Count; j++)
                                    {
                                        if (cbl_typesize.Items[j].Selected == true)
                                        {
                                            cc++;
                                            Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = cbl_typesize.Items[j].Text;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "typesizevalue";
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Note = "typesizevalue" + "-" + cbl_typesize.Items[j].Value;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                            ccsize = cc;
                                        }
                                    }
                                }
                                if (chkrefer.Checked == true)//abarna
                                {
                                    for (j = 0; j < cbl_refername.Items.Count; j++)
                                    {
                                        if (cbl_refername.Items[j].Selected == true)
                                        {
                                            cc++;
                                            Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = cbl_refername.Items[j].Text;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "Direct_Refer";
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Note = "Direct_Refer" + "-" + cbl_refername.Items[j].Value;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                            ccRefer = cc;
                                        }
                                    }
                                }
                                if (ddl_status.SelectedIndex == 0)
                                {
                                    if (chk_user.Checked == true)//abarna added 
                                    {
                                        for (j = 0; j < cbl_username.Items.Count; j++)
                                        {
                                            if (cbl_username.Items[j].Selected == true)
                                            {
                                                cc++;
                                                Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = cbl_username.Items[j].Text;
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "user_code";
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Note = "user_code" + "-" + cbl_username.Items[j].Value;
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                                ccuser = cc;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (chk_user.Checked == true)//abarna added 
                                    {
                                        for (j = 0; j < cbl_username.Items.Count; j++)
                                        {
                                            if (cbl_username.Items[j].Selected == true)
                                            {
                                                cc++;
                                                Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = cbl_username.Items[j].Text;
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "entryusercode";
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Note = "entryusercode" + "-" + cbl_username.Items[j].Value;
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                                ccuser = cc;
                                            }
                                        }
                                    }
                                }
                                if (cb_resident.Checked == true)
                                {
                                    string residencyvalue = string.Empty;
                                    for (j = 0; j < cbl_residency.Items.Count; j++)
                                    {
                                        if (cbl_residency.Items[j].Selected == true)
                                        {
                                            cc++;
                                            Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = cbl_residency.Items[j].Text;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "residency";
                                            if (cbl_residency.Items[j].Value == "1")
                                            {
                                                residencyvalue = "False";
                                            }
                                            else
                                            {
                                                residencyvalue = "True";
                                            }
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Note = "CampusReq" + "-" + residencyvalue;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                            cccomm = cc;
                                        }
                                    }
                                }
                                if (cb_sports.Checked == true)
                                {
                                    for (j = 0; j < cbl_sport.Items.Count; j++)
                                    {
                                        if (cbl_sport.Items[j].Selected == true)
                                        {
                                            cc++;
                                            Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = cbl_sport.Items[j].Text;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "DistinctSport";
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Note = "DistinctSport" + "-" + cbl_sport.Items[j].Value;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                            cccomm = cc;
                                        }
                                    }
                                }
                                if (cb_lang.Checked == true)
                                {
                                    for (j = 0; j < cbl_language.Items.Count; j++)
                                    {
                                        if (cbl_language.Items[j].Selected == true)
                                        {
                                            cc++;
                                            Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = cbl_language.Items[j].Text;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "Part1Language";
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Note = "Part1Language" + "-" + cbl_language.Items[j].Value;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                            cccomm = cc;
                                        }
                                    }
                                }
                                if (cb_mothertng.Checked == true)
                                {
                                    for (j = 0; j < cbl_mothertongue.Items.Count; j++)
                                    {
                                        if (cbl_mothertongue.Items[j].Selected == true)
                                        {
                                            cc++;
                                            Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = cbl_mothertongue.Items[j].Text;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "mother_tongue";
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Note = "mother_tongue" + "-" + cbl_mothertongue.Items[j].Value;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                            cccomm = cc;
                                        }
                                    }
                                }
                                if (cb_phychallange.Checked == true)
                                {
                                    for (j = 0; j < cbl_phychlg.Items.Count; j++)
                                    {
                                        if (cbl_phychlg.Items[j].Selected == true)
                                        {
                                            cc++;
                                            Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = cbl_phychlg.Items[j].Text;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "PhysicalChallanged";
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Note = "visualhandy" + "-" + cbl_phychlg.Items[j].Value;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                            cccomm = cc;
                                        }
                                    }
                                }
                                if (cb_trans.Checked == true)
                                {
                                    for (j = 0; j < cbl_transport.Items.Count; j++)
                                    {
                                        if (cbl_transport.Items[j].Selected == true)
                                        {
                                            cc++;
                                            Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Text = cbl_transport.Items[j].Text;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = "Transport";
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Note = "Boarding" + "-" + cbl_transport.Items[j].Value;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                            cccomm = cc;
                                        }
                                    }
                                }
                            }
                            #endregion
                            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                            style2.Font.Size = 14;
                            style2.Font.Name = "Book Antiqua";
                            style2.Font.Bold = true;
                            style2.HorizontalAlign = HorizontalAlign.Center;
                            style2.ForeColor = Color.Black;
                            style2.BackColor = Color.AliceBlue;
                            Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                            Fpspread1.Sheets[0].RowCount = 0;
                            int getcc = 0;

                            string DegreeCodeReplication = string.Empty;
                            int colspan = 0; bool colspanBool = false;
                            DataView colSpanDV = new DataView();
                            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                Fpspread1.Sheets[0].RowCount++;
                                count++;
                                int cc = 7;
                                int D = 0;
                                int j = 0;
                                DataView dv = new DataView();
                                if (cb_studtypechk.Checked == true)
                                {
                                    // cc++;
                                }
                                if (adddsec != "")
                                {
                                    sectionvalue2 = " ISNULL( r.Sections,'') in('','" + Convert.ToString(ds.Tables[0].Rows[i]["Sections"]) + "')";
                                }
                                else
                                {
                                    sectionvalue2 = string.Empty;
                                }
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Locked = true;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Locked = true;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Course_Name"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Course_Id"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Locked = true;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Column.Width = 300;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Locked = true;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Locked = true;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                if (adddsec != "")
                                {
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["Sections"]);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Sections"]);
                                }
                                else
                                {
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = "-";
                                }
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Locked = true;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["No_Of_seats"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Locked = true;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["TotalStrength"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Locked = true;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;

                                ds.Tables[0].DefaultView.RowFilter = " degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' ";
                                colSpanDV = ds.Tables[0].DefaultView;
                                //colspan = 0;
                                if (DegreeCodeReplication != Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]))//barath 26.12.17
                                {
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["No_Of_seats"]);
                                    int totalseatcount = Convert.ToInt32(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text);
                                    if (tot_allot == 0)
                                        tot_allot = totalseatcount;
                                    else
                                        tot_allot = tot_allot + totalseatcount;
                                    if (DegreeCodeReplication != "")
                                        if (colSpanDV.Count > 0)
                                            Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - colspan - 2, 6, colspan + 1, 1);
                                    colspan = 0;
                                    colspanBool = true;
                                }
                                else if (DegreeCodeReplication != "")//barath 26.12.17
                                {
                                    colspan++; colspanBool = false;
                                }
                                DegreeCodeReplication = Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]); //

                                //int totalseatcount = Convert.ToInt32(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text);
                                //if (tot_allot == 0)
                                //{
                                //    tot_allot = totalseatcount;
                                //}
                                //else
                                //{
                                //    tot_allot = tot_allot + totalseatcount;
                                //}
                                int totstrenth = Convert.ToInt32(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text);
                                if (tot_strg == 0)
                                {
                                    tot_strg = totstrenth;
                                }
                                else
                                {
                                    tot_strg = tot_strg + totstrenth;
                                }
                                string tagval = string.Empty;
                                cc = 7;
                                if (count1 != 0)
                                {
                                    if (cb_status.Checked == true)
                                    {
                                        string val = Convert.ToString(ddl_status.SelectedItem.Value);
                                        string values = newfunction(val);
                                        cc++; string sec = string.Empty;
                                        if (val == "2")
                                        {
                                            if (ds.Tables[2].Rows.Count > 0)
                                            {
                                                ds.Tables[2].DefaultView.RowFilter = " Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' " + values + "";
                                                dv = ds.Tables[2].DefaultView;
                                            }
                                        }
                                        else if (val == "1")//val == "3" ||
                                        {
                                            //if (val == "3")
                                            //    sec = " sections ='" + Convert.ToString(ds.Tables[0].Rows[i]["Sections"]) + "'";
                                            ds.Tables[4].DefaultView.RowFilter = " Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' " + values + " ";
                                            dv = ds.Tables[4].DefaultView;
                                        }
                                        else if (val == "12")//abarna
                                        {
                                            ds.Tables[0].DefaultView.RowFilter = " Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "'";
                                            dv = ds.Tables[0].DefaultView;
                                        }
                                        else if (val == "9")
                                        {
                                            ds.Tables[3].DefaultView.RowFilter = " Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' " + values + " ";
                                            dv = ds.Tables[3].DefaultView;
                                        }
                                        else if (val == "11")//barath 24.01.18
                                        {
                                            ds.Tables[2].DefaultView.RowFilter = " Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' ";
                                            dv = ds.Tables[2].DefaultView;
                                        }
                                        else
                                        {
                                            string sectionfilter = "";
                                            if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[i]["Sections"])))
                                            {
                                                sectionfilter = " and sections ='" + Convert.ToString(ds.Tables[0].Rows[i]["Sections"]) + "'";
                                            }
                                            else if (string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[i]["Sections"])))//abarna
                                            {
                                                sectionfilter = " and isnull(sections,'') =''";
                                            }


                                            string asdf = "Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' " + values + " " + sectionfilter + "";
                                            ds.Tables[3].DefaultView.RowFilter = " Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' " + values + " " + sectionfilter + "";
                                            dv = ds.Tables[3].DefaultView;
                                        }
                                        if (dv.Count > 0)
                                        {
                                            DataTable dt = new DataTable();
                                            dt = dv.ToTable();
                                            int total = Convert.ToInt32(dt.Compute("Sum(TotalStrength)", ""));
                                            string tot = Convert.ToString(total);
                                            //if (val == "2")23.02.17 barath
                                            //{
                                            //    if (ds.Tables[2].Rows.Count > 0)
                                            //    {
                                            //        tot = string.Empty;
                                            //    }
                                            //}
                                            if (tot == "")
                                            {
                                                printtot = "-";
                                            }
                                            else
                                            {
                                                printtot = tot;
                                            }
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Text = printtot;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Tag = tot;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].HorizontalAlign = HorizontalAlign.Center;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Font.Name = "Book Antiqua";
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Font.Size = FontUnit.Medium;
                                            if (!totalmode.Contains(Convert.ToString(cc)))
                                            {
                                                totalmode.Add(Convert.ToString(cc), Convert.ToString(tot));
                                            }
                                            else
                                            {
                                                string getvalue = Convert.ToString(totalmode[Convert.ToString(cc)]);
                                                if (getvalue.Trim() != "")
                                                {
                                                    getvalue = getvalue + "," + tot;
                                                    totalmode.Remove(Convert.ToString(cc));
                                                    if (getvalue.Trim() != "")
                                                    {
                                                        totalmode.Add(Convert.ToString(cc), Convert.ToString(getvalue));
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Text = "-";
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                    }
                                    if (cb_Gender.Checked == true)
                                    {
                                        cc++;
                                        DataView gen_dv = new DataView();
                                        string val = Convert.ToString(ddl_status.SelectedItem.Value);
                                        string values = newfunction(val);
                                        string condition = values.Replace("CC='False'", "cc=0");
                                        string regcondition = condition.Replace("CC='True'", "cc=1");
                                        if (ddl_status.SelectedIndex >= 3)
                                        {
                                            gender_ds.Tables[0].DefaultView.RowFilter = "  degree_code ='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' and Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and Course_Id='" + Convert.ToString(ds.Tables[0].Rows[i]["Course_Id"]) + "' and sections ='" + Convert.ToString(ds.Tables[0].Rows[i]["Sections"]) + "' " + regcondition + "";//
                                            gen_dv = gender_ds.Tables[0].DefaultView;
                                        }
                                        else
                                        {
                                            gender_ds.Tables[0].DefaultView.RowFilter = "  degree_code ='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' and Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and Course_Id='" + Convert.ToString(ds.Tables[0].Rows[i]["Course_Id"]) + "' " + regcondition + " ";
                                            gen_dv = gender_ds.Tables[0].DefaultView;
                                        }
                                        if (gen_dv.Count > 0)
                                        {
                                            string malecount = Convert.ToString(gen_dv.ToTable().Compute("Sum(gendercount)", "sex=0"));
                                            string femalecount = Convert.ToString(gen_dv.ToTable().Compute("Sum(gendercount)", "sex=1"));
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Text = malecount;
                                            if (malecount.Trim() == "")
                                                malecount = "0";
                                            if (!totalmode.Contains(Convert.ToString(cc)))
                                            {
                                                totalmode.Add(Convert.ToString(cc), Convert.ToString(malecount));
                                            }
                                            else
                                            {
                                                string getvalue = Convert.ToString(totalmode[Convert.ToString(cc)]);
                                                if (getvalue.Trim() != "")
                                                {
                                                    getvalue = getvalue + "," + malecount;
                                                    totalmode.Remove(Convert.ToString(cc));
                                                    if (getvalue.Trim() != "")
                                                    {
                                                        totalmode.Add(Convert.ToString(cc), Convert.ToString(getvalue));
                                                    }
                                                }
                                            }
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].HorizontalAlign = HorizontalAlign.Center;
                                            cc++;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Text = femalecount;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].HorizontalAlign = HorizontalAlign.Center;
                                            if (femalecount.Trim() == "")
                                                femalecount = "0";
                                            if (!totalmode.Contains(Convert.ToString(cc)))
                                            {
                                                totalmode.Add(Convert.ToString(cc), Convert.ToString(femalecount));
                                            }
                                            else
                                            {
                                                string getvalue = Convert.ToString(totalmode[Convert.ToString(cc)]);
                                                if (getvalue.Trim() != "")
                                                {
                                                    getvalue = getvalue + "," + femalecount;
                                                    totalmode.Remove(Convert.ToString(cc));
                                                    if (getvalue.Trim() != "")
                                                    {
                                                        totalmode.Add(Convert.ToString(cc), Convert.ToString(getvalue));
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                        }
                                    }
                                    string addfilter = string.Empty;
                                    string v = string.Empty;
                                    string v1 = string.Empty;
                                    string getvalues = string.Empty;
                                    int dd = cc + 1;
                                    if (cc < Fpspread1.Sheets[0].ColumnCount)
                                    {
                                        if (ds.Tables[1].Rows.Count > 0)
                                        {
                                            for (int jk = dd; jk < Fpspread1.Sheets[0].ColumnCount; jk++)
                                            {
                                                cc++;
                                                getvalues = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Note);
                                                if (getvalues.Trim() != "")
                                                {
                                                    string[] spp = getvalues.Split('-');
                                                    if (spp.Length >= 2)
                                                    {
                                                        v = spp[0];
                                                        v1 = spp[1];
                                                        string v2 = string.Empty;
                                                        if (Convert.ToString(v1).Trim() == "isdisable" || Convert.ToString(v1).Trim() == "handy" || Convert.ToString(v1).Trim() == "visualhandy" || Convert.ToString(v1).Trim() == "islearningdis" || Convert.ToString(v1).Trim() == "isdisabledisc")
                                                        {
                                                            v = v1;
                                                            if (v.Trim() == "islearningdis")
                                                            {
                                                                v = v1;
                                                                v1 = "True";
                                                            }
                                                            else if (v1.Trim() == "isdisable")
                                                            {
                                                                v = v1;
                                                                v1 = "True";
                                                            }
                                                            else if (v1.Trim() == "handy")
                                                            {
                                                                v = v1;
                                                                v1 = "1";
                                                            }
                                                            else if (v1.Trim() == "visualhandy")
                                                            {
                                                                v = v1;
                                                                v1 = "1";
                                                            }
                                                            else
                                                            {
                                                                v2 = " and  isdisabledisc IS NOT NULL and isdisabledisc <>''";
                                                                v = "isdisable";
                                                                v1 = "True";
                                                            }
                                                        }

                                                        if (Convert.ToString(v).Trim() == "Boarding")
                                                        {
                                                            if (Convert.ToString(v1).Trim() == "1")
                                                            {
                                                                addfilter = " and isnull(Boarding,'') ='' and Stud_Type='Day Scholar'";//abarna

                                                            }
                                                            else
                                                            {
                                                                addfilter = " and  isnull(Boarding,'') <>''  and Boarding IS NOT NULL and  Stud_Type='Day Scholar'";
                                                                //and Stud_Type='Day Scholar'";
                                                            }
                                                        }

                                                        else
                                                        {
                                                            if (!string.IsNullOrEmpty(Convert.ToString(v1)))
                                                            {
                                                                addfilter = " and " + v + "='" + v1 + "' " + v2 + "";
                                                            }
                                                            else if (string.IsNullOrEmpty(Convert.ToString(v1)))
                                                            {
                                                                addfilter = " and isnull(" + v + ",'')='" + v1 + "' " + v2 + "";
                                                            }
                                                        }
                                                        if (Convert.ToString(v1).Trim() == "DistinctSport")
                                                        {
                                                            addfilter = " and DistinctSport IS NOT NULL and DistinctSport <>'0'";
                                                        }
                                                    }
                                                    string Section = string.Empty;//deepali 11/08/2017
                                                    if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[i]["Sections"])))
                                                    {
                                                        Section = " and sections ='" + Convert.ToString(ds.Tables[0].Rows[i]["Sections"]) + "'";
                                                    }
                                                    else if (string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[i]["Sections"])))
                                                    {
                                                        Section = " and isnull(sections,'') =''";
                                                    }
                                                    //string q = string.Empty;//deepali 11/08/2017
                                                    //if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[1].Rows[i]["quota"])))
                                                    //{
                                                    //    q = " and quota ='" + Convert.ToString(ds.Tables[1].Rows[i]["quota"]) + "'";
                                                    //}
                                                    //else if (string.IsNullOrEmpty(Convert.ToString(ds.Tables[1].Rows[i]["quota"])))
                                                    //{
                                                    //    q = " and isnull(quota,'') =''";
                                                    //}
                                                    //addfilter += q;
                                                    addfilter += Section;
                                                    if (ddl_status.SelectedIndex == 0)
                                                    {
                                                        ds.Tables[1].DefaultView.RowFilter = " Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "'  and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' " + addfilter + " ";
                                                    }
                                                    else if (ddl_status.SelectedIndex == 1)
                                                    {
                                                        ds.Tables[2].DefaultView.RowFilter = " Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and admission_status='1' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "'  " + addfilter + " ";
                                                    }
                                                    else if (ddl_status.SelectedIndex == 2)
                                                    {
                                                        ds.Tables[1].DefaultView.RowFilter = " Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and admission_status='2' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "'  " + addfilter + " ";
                                                    }
                                                    else if (ddl_status.SelectedIndex == 3)
                                                    {
                                                        ds.Tables[1].DefaultView.RowFilter = " Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and admission_status='1' and DelFlag='0' and cc='False' and Exam_Flag='OK' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' " + addfilter + " ";
                                                    }
                                                    else if (ddl_status.SelectedIndex == 4)
                                                    {
                                                        ds.Tables[1].DefaultView.RowFilter = " Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and  DelFlag<>'0' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' " + addfilter + " ";
                                                    }
                                                    else if (ddl_status.SelectedIndex == 5)
                                                    {
                                                        ds.Tables[1].DefaultView.RowFilter = " Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and  Exam_Flag='DEBAR' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' " + addfilter + " ";
                                                    }
                                                    else if (ddl_status.SelectedIndex == 6)
                                                    {
                                                        ds.Tables[1].DefaultView.RowFilter = " Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and  cc='True' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' " + addfilter + " ";
                                                    }
                                                    if (ddl_status.SelectedIndex == 1)
                                                    {
                                                        dv = ds.Tables[2].DefaultView;
                                                    }
                                                    else
                                                    {
                                                        dv = ds.Tables[1].DefaultView;
                                                    }
                                                    if (dv.Count > 0)
                                                    {
                                                        DataTable dt = new DataTable();
                                                        dt = dv.ToTable();
                                                        int tot = 0;
                                                        int total = Convert.ToInt32(dt.Compute("Sum(TotalStrength)", ""));
                                                        tot = tot + total;
                                                        if (tot == 0)
                                                        {
                                                            printtot = "-";
                                                        }
                                                        else
                                                        {
                                                            printtot = Convert.ToString(tot);
                                                        }
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Text = printtot;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Tag = tot;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].HorizontalAlign = HorizontalAlign.Center;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Font.Name = "Book Antiqua";
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Font.Size = FontUnit.Medium;
                                                        if (!totalmode.Contains(Convert.ToString(cc)))
                                                        {
                                                            totalmode.Add(Convert.ToString(cc), Convert.ToString(tot));
                                                        }
                                                        else
                                                        {
                                                            string getvalue = Convert.ToString(totalmode[Convert.ToString(cc)]);
                                                            if (getvalue.Trim() != "")
                                                            {
                                                                getvalue = getvalue + "," + tot;
                                                                totalmode.Remove(Convert.ToString(cc));
                                                                if (getvalue.Trim() != "")
                                                                {
                                                                    totalmode.Add(Convert.ToString(cc), Convert.ToString(getvalue));
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Text = "-";
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].HorizontalAlign = HorizontalAlign.Center;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                        }
                                    }
                                    ///// seat type
                                }
                                getcc = cc;
                            }
                            Fpspread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;//barath 18.01.17
                            Fpspread1.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
                            Fpspread1.Sheets[0].RowCount++;
                            if (!colspanBool)
                                if (DegreeCodeReplication != "")
                                    if (colSpanDV.Count > 0)
                                        Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - colspan - 2, 6, colspan + 1, 1);

                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 5].Text = "Total";
                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 5].Tag = "Total";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 5].BackColor = ColorTranslator.FromHtml("#80EDED");
                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 5].Font.Bold = true;
                            Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, 1, 5);
                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 5].ForeColor = Color.Maroon;
                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 5].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 6].Text = Convert.ToString(tot_allot);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            // Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 7].Text = Convert.ToString(tot_strg);
                            // Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 6].ForeColor = ColorTranslator.FromHtml("#107532");
                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 7].ForeColor = ColorTranslator.FromHtml("#107532");
                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 6].Font.Bold = true;
                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 7].Font.Bold = true;
                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 6].BackColor = ColorTranslator.FromHtml("#80EDED");
                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 7].BackColor = ColorTranslator.FromHtml("#80EDED");
                            // Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 8].BackColor = ColorTranslator.FromHtml("#80EDED");
                            // Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), Fpspread1.Sheets[0].ColumnCount - 1].BackColor = ColorTranslator.FromHtml("#80EDED");
                            if (totalmode.Count > 0)
                            {
                                for (int r1 = 8; r1 <= getcc; r1++)
                                {
                                    string totalvalue = Convert.ToString(totalmode[Convert.ToString(r1)]);
                                    if (totalvalue != "")
                                    {
                                        int gettotalvalue = 0;
                                        string[] spl = totalvalue.Split(',');
                                        for (int l = 0; l < spl.Length; l++)
                                        {
                                            int get_tot = Convert.ToInt32(spl[l]);
                                            if (gettotalvalue == 0)
                                            {
                                                gettotalvalue = get_tot;
                                            }
                                            else
                                            {
                                                gettotalvalue = gettotalvalue + get_tot;
                                            }
                                        }
                                        Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), r1].Text = Convert.ToString(gettotalvalue);
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, r1].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), r1].ForeColor = ColorTranslator.FromHtml("#107532");
                                        Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), r1].Font.Bold = true;
                                        Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), r1].BackColor = ColorTranslator.FromHtml("#80EDED");
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, r1].Font.Name = "Book Antiqua";
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, r1].Font.Size = FontUnit.Medium;
                                        // Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), Fpspread1.Sheets[0].ColumnCount - 1].BackColor = ColorTranslator.FromHtml("#80EDED");
                                    }
                                }
                            }
                            Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                            Fpspread1.Width = 900;
                            Fpspread1.Height = 420;
                            Fpspread1.Visible = true;
                            rptprint.Visible = true;
                            Fpspread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            Fpspread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            Fpspread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            Fpspread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //  d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx");
        }
    }

    public void FpSpread1_CellClick(object sender, EventArgs e)
    {
        try
        {
            Cellclick = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }

    public void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cellclick == true)
            {
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                string query = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='StudentStatusReport' and CollegeCode='" + ddlcollege.SelectedItem.Value + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ddl_colord.SelectedItem.Text != "Select")
                    {
                        fpspread1go1();
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Kindly Select Report Type";
                    }
                }
                else
                {
                    imgbtn_all_Click(sender, e);
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Kindly Set Report Type";
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }

    public void fpspread1go()
    {
        try
        {
            RollAndRegSettings();
            string orderStr = string.Empty;
            if (roll == 0)
                orderStr = " Order by roll_no,reg_no,roll_admit ";
            else if (roll == 1)
                orderStr = " Order by roll_no,reg_no,roll_admit ";
            else if (roll == 2)
                orderStr = " Order by roll_no ";
            else if (roll == 3)
                orderStr = " Order by reg_no ";
            else if (roll == 4)
                orderStr = " Order by roll_admit ";
            else if (roll == 5)
                orderStr = " Order by roll_no,reg_no ";
            else if (roll == 6)
                orderStr = " Order by reg_no,roll_admit ";
            else if (roll == 7)
                orderStr = " Order by roll_no,roll_admit ";
            lbl_headernamespd2.Visible = true;
            btn_viewsprd2.Visible = true;
            lnk_admisstionformbtn.Visible = true;
            img_settingpdf.Visible = true;
            Fpspread2.Visible = true;
            div_report.Visible = true;
            string activerow = string.Empty;
            string activecol = string.Empty;
            int val = 0;
            int count = 0;
            int count1 = 0;
            int i = 0;
            string header = string.Empty;
            string actval = string.Empty;
            string sectionvalue = string.Empty;
            string headertype1 = string.Empty;
            string headertype = string.Empty;
            activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
            activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
            string sec_textvalue = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text);
            string Batch_tagvalue = string.Empty;
            string dept_tagvalue = string.Empty;
            string course_tagvalue = string.Empty;
            string sem_tagvalue = string.Empty;
            string sec_tagvalue = string.Empty;
            string FromdateApplyn = string.Empty;
            string FromdateReg = string.Empty;

            DateTime from = new DateTime();
            DateTime to = new DateTime();
            if (cb_from.Checked == true)
            {
                string[] ay = txt_fromdate.Text.Split('/');
                string[] ay1 = txt_todate.Text.Split('/');
                from = Convert.ToDateTime(ay[1] + "/" + ay[0] + "/" + ay[2]);
                to = Convert.ToDateTime(ay1[1] + "/" + ay1[0] + "/" + ay1[2]);
                FromdateApplyn = "  and date_applied between '" + from.ToString("MM/dd/yyyy") + "' and '" + to.ToString("MM/dd/yyyy") + "'";
                FromdateReg = "  and Adm_Date between '" + from.ToString("MM/dd/yyyy") + "' and '" + to.ToString("MM/dd/yyyy") + "'";
            }
            if (sec_textvalue != "Total")
            {
                Batch_tagvalue = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                course_tagvalue = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                dept_tagvalue = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag);
                sem_tagvalue = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Tag);
                sec_tagvalue = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Tag);
            }
            else
            {
                for (i = 0; i < cbl_batch.Items.Count; i++)
                {
                    if (cbl_batch.Items[i].Selected == true)
                    {
                        string addbatch1 = cbl_batch.Items[i].Value.ToString();
                        if (Batch_tagvalue == "")
                        {
                            Batch_tagvalue = addbatch1;
                        }
                        else
                        {
                            Batch_tagvalue = Batch_tagvalue + "'" + "," + "'" + addbatch1;
                        }
                    }
                }
                for (i = 0; i < cbl_branch.Items.Count; i++)
                {
                    if (cbl_branch.Items[i].Selected == true)
                    {
                        string adddeg1 = cbl_branch.Items[i].Value.ToString();
                        if (dept_tagvalue == "")
                        {
                            dept_tagvalue = adddeg1;
                        }
                        else
                        {
                            dept_tagvalue = dept_tagvalue + "'" + "," + "'" + adddeg1;
                        }
                    }
                }
                for (i = 0; i < cbl_sem.Items.Count; i++)
                {
                    if (cbl_sem.Items[i].Selected == true)
                    {
                        string addsem1 = cbl_sem.Items[i].Value.ToString();
                        if (sem_tagvalue == "")
                        {
                            sem_tagvalue = addsem1;
                        }
                        else
                        {
                            sem_tagvalue = sem_tagvalue + "'" + "," + "'" + addsem1;
                        }
                    }
                }
                for (i = 0; i < cbl_sec.Items.Count; i++)
                {
                    if (cbl_sec.Items[i].Selected == true)
                    {
                        string addsem1 = cbl_sec.Items[i].Value.ToString();
                        if (sec_tagvalue == "")
                        {
                            sec_tagvalue = addsem1;
                        }
                        else
                        {
                            sec_tagvalue = sec_tagvalue + "'" + "," + "'" + addsem1;
                        }
                    }
                }
            }
            if (sec_tagvalue != "")
            {
                sectionvalue = " and ISNULL( r.Sections,'') in('','" + sec_tagvalue + "')";
            }
            else
                sectionvalue = string.Empty;
            {
            }
            if (Convert.ToInt32(activecol) <= 6)
            {
                header = "All";
                val = 0;
            }
            else
            {
                actval = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Text);
                header = Fpspread1.Sheets[0].ColumnHeader.Cells[Convert.ToInt32(0), Convert.ToInt32(activecol)].Text;
                headertype = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[Convert.ToInt32(0), Convert.ToInt32(activecol)].Tag);
                headertype1 = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[Convert.ToInt32(0), Convert.ToInt32(activecol)].Text);
            }
            string name = header;
            string addqur = string.Empty;
            if (headertype == "Stud_Type" || headertype == "seattype" || headertype == "mode" || headertype == "religion" || headertype == "community")
            {
                val = 1;
                if (headertype == "Stud_Type")
                {
                    addqur = " and r.Stud_Type='" + header + "'";
                }
                if (headertype == "seattype")
                {
                    header = d2.GetFunction("SELECT seattype FROM applyn A,Registration R,TextValTable T WHERE A.app_no=R.App_No AND T.TextCode =A.seattype AND R.college_code ='" + collegecode1 + "' and T.TextVal='" + header + "' ");
                    addqur = " and a.seattype='" + header + "'";
                }
                if (headertype == "mode")
                {
                    headertype = "Type";
                    if (header == "Regular")
                    {
                        header = "1";
                    }
                    else if (header == "Lateral")
                    {
                        header = "3";
                    }
                    else if (header == "Transfer")
                    {
                        header = "2";
                    }
                    addqur = " and r.mode='" + header + "'";
                }
                if (headertype == "religion")
                {
                    header = d2.GetFunction("SELECT religion FROM applyn A,Registration R,TextValTable T WHERE A.app_no=R.App_No AND T.TextCode =A.religion AND R.college_code ='" + collegecode1 + "' and T.TextVal='" + header + "' ");
                    addqur = " and a.religion='" + header + "'";
                }
                if (headertype == "community")
                {
                    header = d2.GetFunction("SELECT community FROM applyn A,Registration R,TextValTable T WHERE A.app_no=R.App_No AND T.TextCode =A.community AND R.college_code ='" + collegecode1 + "' and T.TextVal='" + header + "' ");
                    addqur = " and a.community='" + header + "'";
                }
            }
            if (headertype1 == "Applied")
            {
                header = string.Empty;
                val = 6;
            }
            if (headertype1 == "Waiting For Admitted")
            {
                header = string.Empty;
                val = 7;
            }
            if (headertype1 == "Left")
            {
                header = string.Empty;
                val = 8;
            }
            if (headertype1 == "Admitted")
            {
                header = string.Empty;
                val = 9;
            }
            if (headertype1 == "De-Bar")
            {
                header = string.Empty;
                val = 10;
            }
            if (headertype1 == "Discontinue")
            {
                header = string.Empty;
                val = 11;
            }
            if (headertype1 == "Course Completed")
            {
                header = string.Empty;
                val = 12;
            }
            if (headertype1 == "Prolong Absent")
            {
                header = string.Empty;
                val = 14;
            }
            //=====Added By saranya 0n 05/01/2018 for transport and hostel cancel students=====// 

            if (headertype1 == "Transport Canceled Student")
            {
                header = string.Empty;
                val = 15;
            }
            if (headertype1 == "Hostel Canceled Student")
            {
                header = string.Empty;
                val = 16;
            }

            //=============================================================================//

            string queryadd = string.Empty;
            if (headertype == "residency" || headertype == "DistinctSport" || headertype == "Part1Language" || headertype == "mother_tongue" || headertype == "PhysicalChallanged")
            {
                val = 13;
                if (headertype == "residency")
                {
                    if (header.Trim() == "Campus Required")
                    {
                        header = "1";
                    }
                    else
                    {
                        header = "0";
                    }
                    queryadd = " and CampusReq='" + header + "'";
                }
                if (headertype == "DistinctSport")
                {
                    if (header.Trim() == "IsSports")
                    {
                        queryadd = " and DistinctSport IS NOT NULL and DistinctSport <>'0'";
                    }
                    else
                    {
                        header = d2.GetFunction("SELECT DistinctSport FROM applyn A,TextValTable T WHERE T.TextCode =A.DistinctSport AND a.college_code ='" + ddlcollege.SelectedItem.Value + "' and T.TextVal='" + header + "' ");
                        queryadd = " and DistinctSport='" + header + "'";
                    }
                }
                if (headertype == "Part1Language")
                {
                    header = d2.GetFunction("select  Part1Language  from applyn a,Stud_prev_details s,TextValTable t where a.app_no =s.app_no and s.Part1Language =t.TextCode and a.college_code='" + ddlcollege.SelectedItem.Value + "' and  T.TextVal='" + header + "'  ");
                    queryadd = " and Part1Language='" + header + "'";
                }
                if (headertype == "mother_tongue")
                {
                    header = d2.GetFunction("select  mother_tongue  from applyn a,TextValTable t where  a.mother_tongue =t.TextCode and a.college_code='" + ddlcollege.SelectedItem.Value + "' and  T.TextVal='" + header + "'");
                    queryadd = " and mother_tongue='" + header + "'";
                }
                if (headertype == "PhysicalChallanged")
                {
                    if (header.Trim() == "IsDisable")
                    {
                        queryadd = " and  isdisable ='" + "True" + "'";
                    }
                    else if (header.Trim() == "Visually Challanged")
                    {
                        queryadd = " and  visualhandy ='" + 1 + "'";
                    }
                    else if (header.Trim() == "Physically Challanged")
                    {
                        queryadd = " and  handy ='" + 1 + "'";
                    }
                    else if (header.Trim() == "Learning Disability")
                    {
                        queryadd = " and  islearningdis ='" + "True" + "'";
                    }
                    else
                    {
                        queryadd = " and  isdisabledisc<>''";
                    }
                }
            }
            Fpspread2.Sheets[0].Visible = true;
            Fpspread2.Sheets[0].RowHeader.Visible = false;
            Fpspread2.CommandBar.Visible = false;
            Fpspread2.Sheets[0].AutoPostBack = false;
            Fpspread2.Sheets[0].RowCount = 0;
            Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread2.Sheets[0].ColumnCount = 3;
            FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
            darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle2.ForeColor = Color.Black;
            darkstyle2.HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
            loadlcolumns();
            string query = string.Empty;
            if (val == 0)
            {
                query = "select distinct StuPer_Id,(Select textval FROM textvaltable T WHERE mother_tongue = t.TextCode) mother_tongue,case when  DistinctSport='0' then 'No' else (Select textval FROM textvaltable T WHERE DistinctSport = t.TextCode)   end as DistinctSport,case when CampusReq='0' then 'No' else 'Yes' end as CampusReq,case when handy='0' then 'No' else 'Yes' end as handy,case when islearningdis='0' then 'No' else 'Yes' end as islearningdis,isdisabledisc,case when isdisable='0' then 'No' else 'Yes' end as isdisable ,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no,a.missionarydisc,a.MissionaryChild,a.parent_phnoc,a.parent_pincodec,a.Student_Mobile,(Select textval FROM textvaltable T WHERE Countryc = t.TextCode) Countryc,(Select textval FROM textvaltable T WHERE parent_statec = t.TextCode) parent_statec,a.Cityc,a.Streetc,a.parent_addressC,(Select textval FROM textvaltable T WHERE co_curricular = t.TextCode) co_curricular,(Select textval FROM textvaltable T WHERE citizen = t.TextCode) citizen,a.visualhandy,a.first_graduate,(Select textval FROM textvaltable T WHERE TamilOrginFromAndaman = t.TextCode) TamilOrginFromAndaman ,(Select textval FROM textvaltable T WHERE seattype = t.TextCode) seattype,(Select textval FROM textvaltable T WHERE parent_occu = t.TextCode) parent_occu,(Select textval FROM textvaltable T WHERE caste = t.TextCode) caste,(Select textval FROM textvaltable T WHERE community = t.TextCode) community,(Select textval FROM textvaltable T WHERE religion = t.TextCode) religion,a.parent_name,''Roll_No,a.Stud_Name,a.Batch_Year,a.Current_Semester,c.Course_Name,Dt.Dept_Name,''Sections,CONVERT(VARCHAR(11),dob,103) as dob from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='1' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " " + orderStr + "  ";//order by r.Roll_No,r.Stud_Name,r.Reg_No
            }
            if (val == 1)
            {
                query = "select StuPer_Id,(Select textval FROM textvaltable T WHERE mother_tongue = t.TextCode) mother_tongue,case when  DistinctSport='0' then 'No' else (Select textval FROM textvaltable T WHERE DistinctSport = t.TextCode)   end as DistinctSport,case when CampusReq='0' then 'No' else 'Yes' end as CampusReq,case when handy='0' then 'No' else 'Yes' end as handy,case when islearningdis='0' then 'No' else 'Yes' end as islearningdis, isdisabledisc,case when isdisable='0' then 'No' else 'Yes' end as isdisable ,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no,a.missionarydisc,a.MissionaryChild,a.parent_phnoc,a.parent_pincodec,a.Student_Mobile,a.Countryc,a.parent_statec,a.Cityc,a.Streetc,a.parent_addressC,a.co_curricular,a.citizen,a.visualhandy,a.first_graduate,a.TamilOrginFromAndaman ,a.seattype,a.parent_occu,a.caste,a.community,a.religion,a.parent_name,r.Roll_No,r.Stud_Name,r.Batch_Year,r.Current_Semester,c.Course_Name,Dt.Dept_Name,isnull( r.Sections,'') as Sections,CONVERT(VARCHAR(11),dob,103) as dob from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='1' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " " + addqur + " " + orderStr + "  ";//order by r.Roll_No,r.Stud_Name,r.Reg_No
            }
            else if (val == 6)
            {
                query = "select distinct StuPer_Id,(Select textval FROM textvaltable T WHERE mother_tongue = t.TextCode) mother_tongue,case when  DistinctSport='0' then 'No' else (Select textval FROM textvaltable T WHERE DistinctSport = t.TextCode)   end as DistinctSport,case when CampusReq='0' then 'No' else 'Yes' end as CampusReq,case when handy='0' then 'No' else 'Yes' end as handy,case when islearningdis='0' then 'No' else 'Yes' end as islearningdis, isdisabledisc,case when isdisable='0' then 'No' else 'Yes' end as isdisable ,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no,a.missionarydisc,a.MissionaryChild,a.parent_phnoc,a.parent_pincodec,a.Student_Mobile,(Select textval FROM textvaltable T WHERE Countryc = t.TextCode) Countryc,(Select textval FROM textvaltable T WHERE parent_statec = t.TextCode) parent_statec,a.Cityc,a.Streetc,a.parent_addressC,(Select textval FROM textvaltable T WHERE co_curricular = t.TextCode) co_curricular,(Select textval FROM textvaltable T WHERE citizen = t.TextCode) citizen,a.visualhandy,a.first_graduate,(Select textval FROM textvaltable T WHERE TamilOrginFromAndaman = t.TextCode) TamilOrginFromAndaman ,(Select textval FROM textvaltable T WHERE seattype = t.TextCode) seattype,(Select textval FROM textvaltable T WHERE parent_occu = t.TextCode) parent_occu,(Select textval FROM textvaltable T WHERE caste = t.TextCode) caste,(Select textval FROM textvaltable T WHERE community = t.TextCode) community,(Select textval FROM textvaltable T WHERE religion = t.TextCode) religion,a.parent_name,''Roll_No,a.Stud_Name,a.Batch_Year,a.Current_Semester,c.Course_Name,Dt.Dept_Name,''Sections,CONVERT(VARCHAR(11),dob,103) as dob from applyn a,degree d,Department dt,Course C where  d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "') and IsConfirm='1' and  a.Current_Semester in('" + sem_tagvalue + "')  " + FromdateApplyn + "   order by a.Stud_Name  ";
            }
            else if (val == 7)
            {
                query = "select distinct StuPer_Id,(Select textval FROM textvaltable T WHERE mother_tongue = t.TextCode) mother_tongue,case when  DistinctSport='0' then 'No' else (Select textval FROM textvaltable T WHERE DistinctSport = t.TextCode)   end as DistinctSport,case when CampusReq='0' then 'No' else 'Yes' end as CampusReq,case when handy='0' then 'No' else 'Yes' end as handy,case when islearningdis='0' then 'No' else 'Yes' end as islearningdis,isdisabledisc,case when isdisable='0' then 'No' else 'Yes' end as isdisable ,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no,a.missionarydisc,a.MissionaryChild,a.parent_phnoc,a.parent_pincodec,a.Student_Mobile,(Select textval FROM textvaltable T WHERE Countryc = t.TextCode) Countryc,(Select textval FROM textvaltable T WHERE parent_statec = t.TextCode) parent_statec,a.Cityc,a.Streetc,a.parent_addressC,(Select textval FROM textvaltable T WHERE co_curricular = t.TextCode) co_curricular,(Select textval FROM textvaltable T WHERE citizen = t.TextCode) citizen,a.visualhandy,a.first_graduate,(Select textval FROM textvaltable T WHERE TamilOrginFromAndaman = t.TextCode) TamilOrginFromAndaman ,(Select textval FROM textvaltable T WHERE seattype = t.TextCode) seattype,(Select textval FROM textvaltable T WHERE parent_occu = t.TextCode) parent_occu,(Select textval FROM textvaltable T WHERE caste = t.TextCode) caste,(Select textval FROM textvaltable T WHERE community = t.TextCode) community,(Select textval FROM textvaltable T WHERE religion = t.TextCode) religion,a.parent_name,''Roll_No,a.Stud_Name,a.Batch_Year,a.Current_Semester,c.Course_Name,Dt.Dept_Name,''Sections,CONVERT(VARCHAR(11),dob,103) as dob from degree d,Department dt,Course C ,applyn a left join Registration r on a.app_no=r.App_No  where r.App_No is null  and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='1' and  a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "') and  a.Current_Semester in('" + sem_tagvalue + "')  " + sectionvalue + " order by a.Stud_Name ";
            }
            else if (val == 8)
            {
                query = "select distinct StuPer_Id,(Select textval FROM textvaltable T WHERE mother_tongue = t.TextCode) mother_tongue,case when  DistinctSport='0' then 'No' else (Select textval FROM textvaltable T WHERE DistinctSport = t.TextCode)   end as DistinctSport,case when CampusReq='0' then 'No' else 'Yes' end as CampusReq,case when handy='0' then 'No' else 'Yes' end as handy,case when islearningdis='0' then 'No' else 'Yes' end as islearningdis, isdisabledisc,case when isdisable='0' then 'No' else 'Yes' end as isdisable ,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no,a.missionarydisc,a.MissionaryChild,a.parent_phnoc,a.parent_pincodec,a.Student_Mobile,(Select textval FROM textvaltable T WHERE Countryc = t.TextCode) Countryc,(Select textval FROM textvaltable T WHERE parent_statec = t.TextCode) parent_statec,a.Cityc,a.Streetc,a.parent_addressC,(Select textval FROM textvaltable T WHERE co_curricular = t.TextCode) co_curricular,(Select textval FROM textvaltable T WHERE citizen = t.TextCode) citizen,a.visualhandy,a.first_graduate,(Select textval FROM textvaltable T WHERE TamilOrginFromAndaman = t.TextCode) TamilOrginFromAndaman ,(Select textval FROM textvaltable T WHERE seattype = t.TextCode) seattype,(Select textval FROM textvaltable T WHERE parent_occu = t.TextCode) parent_occu,(Select textval FROM textvaltable T WHERE caste = t.TextCode) caste,(Select textval FROM textvaltable T WHERE community = t.TextCode) community,(Select textval FROM textvaltable T WHERE religion = t.TextCode) religion,a.parent_name,''Roll_No,a.Stud_Name,a.Batch_Year,a.Current_Semester,c.Course_Name,Dt.Dept_Name,''Sections,CONVERT(VARCHAR(11),dob,103) as dob from applyn a,degree d,Department dt,Course C where  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='2' and   a.degree_code in('" + dept_tagvalue + "') and a.degree_code=d.Degree_Code and a.Batch_Year in('" + Batch_tagvalue + "') and  a.Current_Semester in('" + sem_tagvalue + "')  order by a.Stud_Name  ";
            }
            else if (val == 9)
            {
                query = "select  distinct StuPer_Id,(Select textval FROM textvaltable T WHERE mother_tongue = t.TextCode) mother_tongue,case when  DistinctSport='0' then 'No' else (Select textval FROM textvaltable T WHERE DistinctSport = t.TextCode)   end as DistinctSport,case when CampusReq='0' then 'No' else 'Yes' end as CampusReq,case when handy='0' then 'No' else 'Yes' end as handy,case when islearningdis='0' then 'No' else 'Yes' end as islearningdis, isdisabledisc,case when isdisable='0' then 'No' else 'Yes' end as isdisable ,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no,a.missionarydisc,a.MissionaryChild,a.parent_phnoc,a.parent_pincodec,a.Student_Mobile,(Select textval FROM textvaltable T WHERE Countryc = t.TextCode) Countryc,(Select textval FROM textvaltable T WHERE parent_statec = t.TextCode) parent_statec,a.Cityc,a.Streetc,a.parent_addressC,(Select textval FROM textvaltable T WHERE co_curricular = t.TextCode) co_curricular,(Select textval FROM textvaltable T WHERE citizen = t.TextCode) citizen,a.visualhandy,a.first_graduate,(Select textval FROM textvaltable T WHERE TamilOrginFromAndaman = t.TextCode) TamilOrginFromAndaman ,(Select textval FROM textvaltable T WHERE seattype = t.TextCode) seattype,(Select textval FROM textvaltable T WHERE parent_occu = t.TextCode) parent_occu,(Select textval FROM textvaltable T WHERE caste = t.TextCode) caste,(Select textval FROM textvaltable T WHERE community = t.TextCode) community,(Select textval FROM textvaltable T WHERE religion = t.TextCode) religion,a.parent_name,r.Reg_No ,r.Roll_No,r.Stud_Name,a.Batch_Year,a.Current_Semester,c.Course_Name,Dt.Dept_Name,isnull( r.Sections,'') as Sections,CONVERT(VARCHAR(11),dob,103) as dob  from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='1' and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " " + orderStr + " ";//order by r.Roll_No,r.Stud_Name,r.Reg_No 
            }
            else if (val == 10)
            {
                query = "select  distinct StuPer_Id,(Select textval FROM textvaltable T WHERE mother_tongue = t.TextCode) mother_tongue,case when  DistinctSport='0' then 'No' else (Select textval FROM textvaltable T WHERE DistinctSport = t.TextCode)   end as DistinctSport,case when CampusReq='0' then 'No' else 'Yes' end as CampusReq,case when handy='0' then 'No' else 'Yes' end as handy,case when islearningdis='0' then 'No' else 'Yes' end as islearningdis, isdisabledisc,case when isdisable='0' then 'No' else 'Yes' end as isdisable ,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no,a.missionarydisc,a.MissionaryChild,a.parent_phnoc,a.parent_pincodec,a.Student_Mobile,(Select textval FROM textvaltable T WHERE Countryc = t.TextCode) Countryc,(Select textval FROM textvaltable T WHERE parent_statec = t.TextCode) parent_statec,a.Cityc,a.Streetc,a.parent_addressC,(Select textval FROM textvaltable T WHERE co_curricular = t.TextCode) co_curricular,(Select textval FROM textvaltable T WHERE citizen = t.TextCode) citizen,a.visualhandy,a.first_graduate,(Select textval FROM textvaltable T WHERE TamilOrginFromAndaman = t.TextCode) TamilOrginFromAndaman ,(Select textval FROM textvaltable T WHERE seattype = t.TextCode) seattype,(Select textval FROM textvaltable T WHERE parent_occu = t.TextCode) parent_occu,(Select textval FROM textvaltable T WHERE caste = t.TextCode) caste,(Select textval FROM textvaltable T WHERE community = t.TextCode) community,(Select textval FROM textvaltable T WHERE religion = t.TextCode) religion,a.parent_name,r.Reg_No ,r.Roll_No,r.Stud_Name,a.Batch_Year,a.Current_Semester,c.Course_Name,Dt.Dept_Name,isnull( r.Sections,'') as Sections,CONVERT(VARCHAR(11),dob,103) as dob  from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'  and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " and r.Exam_Flag='DEBAR' " + orderStr + "  ";//order by r.Roll_No,r.Stud_Name,r.Reg_No
            }
            else if (val == 11)
            {
                query = "select  distinct StuPer_Id,(Select textval FROM textvaltable T WHERE mother_tongue = t.TextCode) mother_tongue,case when  DistinctSport='0' then 'No' else (Select textval FROM textvaltable T WHERE DistinctSport = t.TextCode)   end as DistinctSport,case when CampusReq='0' then 'No' else 'Yes' end as CampusReq,case when handy='0' then 'No' else 'Yes' end as handy,case when islearningdis='0' then 'No' else 'Yes' end as islearningdis,isdisabledisc,case when isdisable='0' then 'No' else 'Yes' end as isdisable ,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no,a.missionarydisc,a.MissionaryChild,a.parent_phnoc,a.parent_pincodec,a.Student_Mobile,(Select textval FROM textvaltable T WHERE Countryc = t.TextCode) Countryc,(Select textval FROM textvaltable T WHERE parent_statec = t.TextCode) parent_statec,a.Cityc,a.Streetc,a.parent_addressC,(Select textval FROM textvaltable T WHERE co_curricular = t.TextCode) co_curricular,(Select textval FROM textvaltable T WHERE citizen = t.TextCode) citizen,a.visualhandy,a.first_graduate,(Select textval FROM textvaltable T WHERE TamilOrginFromAndaman = t.TextCode) TamilOrginFromAndaman ,(Select textval FROM textvaltable T WHERE seattype = t.TextCode) seattype,(Select textval FROM textvaltable T WHERE parent_occu = t.TextCode) parent_occu,(Select textval FROM textvaltable T WHERE caste = t.TextCode) caste,(Select textval FROM textvaltable T WHERE community = t.TextCode) community,(Select textval FROM textvaltable T WHERE religion = t.TextCode) religion,a.parent_name,r.Reg_No ,r.Roll_No,r.Stud_Name,a.Batch_Year,a.Current_Semester,c.Course_Name,Dt.Dept_Name,isnull( r.Sections,'') as Sections,CONVERT(VARCHAR(11),dob,103) as dob  from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'   and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " and r.DelFlag<>'0' " + orderStr + "  ";//order by r.Roll_No,r.Stud_Name,r.Reg_No 
            }
            else if (val == 12)
            {
                query = "select  distinct StuPer_Id(Select textval FROM textvaltable T WHERE mother_tongue = t.TextCode) mother_tongue,case when  DistinctSport='0' then 'No' else (Select textval FROM textvaltable T WHERE DistinctSport = t.TextCode)   end as DistinctSport,case when CampusReq='0' then 'No' else 'Yes' end as CampusReq,case when handy='0' then 'No' else 'Yes' end as handy,case when islearningdis='0' then 'No' else 'Yes' end as islearningdis, isdisabledisc,case when isdisable='0' then 'No' else 'Yes' end as isdisable ,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no,a.missionarydisc,a.MissionaryChild,a.parent_phnoc,a.parent_pincodec,a.Student_Mobile,(Select textval FROM textvaltable T WHERE Countryc = t.TextCode) Countryc,(Select textval FROM textvaltable T WHERE parent_statec = t.TextCode) parent_statec,a.Cityc,a.Streetc,a.parent_addressC,(Select textval FROM textvaltable T WHERE co_curricular = t.TextCode) co_curricular,(Select textval FROM textvaltable T WHERE citizen = t.TextCode) citizen,a.visualhandy,a.first_graduate,(Select textval FROM textvaltable T WHERE TamilOrginFromAndaman = t.TextCode) TamilOrginFromAndaman ,(Select textval FROM textvaltable T WHERE seattype = t.TextCode) seattype,(Select textval FROM textvaltable T WHERE parent_occu = t.TextCode) parent_occu,(Select textval FROM textvaltable T WHERE caste = t.TextCode) caste,(Select textval FROM textvaltable T WHERE community = t.TextCode) community,(Select textval FROM textvaltable T WHERE religion = t.TextCode) religion,a.parent_name,r.Reg_No ,r.Roll_No,r.Stud_Name,a.Batch_Year,a.Current_Semester,c.Course_Name,Dt.Dept_Name,isnull( r.Sections,'') as Sections,CONVERT(VARCHAR(11),dob,103) as dob  from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='1'  and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " and r.cc=1 " + orderStr + "  ";//order by r.Roll_No,r.Stud_Name,r.Reg_No
            }
            else if (val == 14)
            {
                query = "select  distinct StuPer_Id,(Select textval FROM textvaltable T WHERE mother_tongue = t.TextCode) mother_tongue,case when  DistinctSport='0' then 'No' else (Select textval FROM textvaltable T WHERE DistinctSport = t.TextCode)   end as DistinctSport,case when CampusReq='0' then 'No' else 'Yes' end as CampusReq,case when handy='0' then 'No' else 'Yes' end as handy,case when islearningdis='0' then 'No' else 'Yes' end as islearningdis,isdisabledisc,case when isdisable='0' then 'No' else 'Yes' end as isdisable ,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no,a.missionarydisc,a.MissionaryChild,a.parent_phnoc,a.parent_pincodec,a.Student_Mobile,(Select textval FROM textvaltable T WHERE Countryc = t.TextCode) Countryc,(Select textval FROM textvaltable T WHERE parent_statec = t.TextCode) parent_statec,a.Cityc,a.Streetc,a.parent_addressC,(Select textval FROM textvaltable T WHERE co_curricular = t.TextCode) co_curricular,(Select textval FROM textvaltable T WHERE citizen = t.TextCode) citizen,a.visualhandy,a.first_graduate,(Select textval FROM textvaltable T WHERE TamilOrginFromAndaman = t.TextCode) TamilOrginFromAndaman ,(Select textval FROM textvaltable T WHERE seattype = t.TextCode) seattype,(Select textval FROM textvaltable T WHERE parent_occu = t.TextCode) parent_occu,(Select textval FROM textvaltable T WHERE caste = t.TextCode) caste,(Select textval FROM textvaltable T WHERE community = t.TextCode) community,(Select textval FROM textvaltable T WHERE religion = t.TextCode) religion,a.parent_name,r.Reg_No ,r.Roll_No,r.Stud_Name,a.Batch_Year,a.Current_Semester,c.Course_Name,Dt.Dept_Name,isnull( r.Sections,'') as Sections,CONVERT(VARCHAR(11),dob,103) as dob  from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'   and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " and r.DelFlag<>'0' and ProlongAbsend<>'0' " + orderStr + "  ";
            }
            else if (val == 13)
            {
                string leftwaiting = string.Empty;
                if (ddl_status.SelectedIndex < 2)
                {
                    if (ddl_status.SelectedIndex == 1)
                    {
                        leftwaiting = " and  admission_status='1'";
                        query = "select distinct StuPer_Id,(Select textval FROM textvaltable T WHERE mother_tongue = t.TextCode) mother_tongue,case when  DistinctSport='0' then 'No' else (Select textval FROM textvaltable T WHERE DistinctSport = t.TextCode)   end as DistinctSport,case when CampusReq='0' then 'No' else 'Yes' end as CampusReq,case when handy='0' then 'No' else 'Yes' end as handy,case when islearningdis='0' then 'No' else 'Yes' end as islearningdis, isdisabledisc,case when isdisable='0' then 'No' else 'Yes' end as isdisable ,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no,a.missionarydisc,a.MissionaryChild,a.parent_phnoc,a.parent_pincodec,a.Student_Mobile,(Select textval FROM textvaltable T WHERE Countryc = t.TextCode) Countryc,(Select textval FROM textvaltable T WHERE parent_statec = t.TextCode) parent_statec,a.Cityc,a.Streetc,a.parent_addressC,(Select textval FROM textvaltable T WHERE co_curricular = t.TextCode) co_curricular,(Select textval FROM textvaltable T WHERE citizen = t.TextCode) citizen,a.visualhandy,a.first_graduate,(Select textval FROM textvaltable T WHERE TamilOrginFromAndaman = t.TextCode) TamilOrginFromAndaman ,(Select textval FROM textvaltable T WHERE seattype = t.TextCode) seattype,(Select textval FROM textvaltable T WHERE parent_occu = t.TextCode) parent_occu,(Select textval FROM textvaltable T WHERE caste = t.TextCode) caste,(Select textval FROM textvaltable T WHERE community = t.TextCode) community,(Select textval FROM textvaltable T WHERE religion = t.TextCode) religion,a.parent_name,''Roll_No,a.Stud_Name,a.Batch_Year,a.Current_Semester,c.Course_Name,Dt.Dept_Name,''Sections,CONVERT(VARCHAR(11),dob,103) as dob from Stud_prev_details s, degree d,Department dt,Course C ,applyn a  left join Registration r on a.app_no=r.App_No  where r.App_No is null and a.app_no =s.app_no  and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and isconfirm ='1'  and a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "') and  a.Current_Semester in('" + sem_tagvalue + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + sectionvalue + " " + queryadd + " " + leftwaiting + " and s.Part1Language<>0 ";
                    }
                    else
                    {
                        if (ddl_status.SelectedIndex == 2)
                        {
                            leftwaiting = " and  admission_status='2'";
                        }
                        query = "select distinct StuPer_Id,(Select textval FROM textvaltable T WHERE mother_tongue = t.TextCode) mother_tongue,case when  DistinctSport='0' then 'No' else (Select textval FROM textvaltable T WHERE DistinctSport = t.TextCode)   end as DistinctSport,case when CampusReq='0' then 'No' else 'Yes' end as CampusReq,case when handy='0' then 'No' else 'Yes' end as handy,case when islearningdis='0' then 'No' else 'Yes' end as islearningdis, isdisabledisc,case when isdisable='0' then 'No' else 'Yes' end as isdisable ,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no,a.missionarydisc,a.MissionaryChild,a.parent_phnoc,a.parent_pincodec,a.Student_Mobile,(Select textval FROM textvaltable T WHERE Countryc = t.TextCode) Countryc,(Select textval FROM textvaltable T WHERE parent_statec = t.TextCode) parent_statec,a.Cityc,a.Streetc,a.parent_addressC,(Select textval FROM textvaltable T WHERE co_curricular = t.TextCode) co_curricular,(Select textval FROM textvaltable T WHERE citizen = t.TextCode) citizen,a.visualhandy,a.first_graduate,(Select textval FROM textvaltable T WHERE TamilOrginFromAndaman = t.TextCode) TamilOrginFromAndaman ,(Select textval FROM textvaltable T WHERE seattype = t.TextCode) seattype,(Select textval FROM textvaltable T WHERE parent_occu = t.TextCode) parent_occu,(Select textval FROM textvaltable T WHERE caste = t.TextCode) caste,(Select textval FROM textvaltable T WHERE community = t.TextCode) community,(Select textval FROM textvaltable T WHERE religion = t.TextCode) religion,a.parent_name,''Roll_No,a.Stud_Name,a.Batch_Year,a.Current_Semester,c.Course_Name,Dt.Dept_Name,''Sections,CONVERT(VARCHAR(11),dob,103) as dob from applyn a,Stud_prev_details s, degree d,Department dt,Course C where a.app_no =s.app_no  and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and isconfirm ='1'  and a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "') and  a.Current_Semester in('" + sem_tagvalue + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + sectionvalue + " " + queryadd + " " + leftwaiting + " and s.Part1Language<>0  ";
                    }
                }
                else
                {
                    if (ddl_status.SelectedIndex == 3)
                    {
                        leftwaiting = " and admission_status='1' and DelFlag='0' and Exam_Flag='OK' and CC='False' ";
                    }
                    else if (ddl_status.SelectedIndex == 4)
                    {
                        leftwaiting = " and DelFlag<>'0'";
                    }
                    else if (ddl_status.SelectedIndex == 5)
                    {
                        leftwaiting = " and Exam_Flag='DEBAR' ";
                    }
                    else if (ddl_status.SelectedIndex == 6)
                    {
                        leftwaiting = " and CC='True' ";
                    }
                    query = "  select distinct StuPer_Id,(Select textval FROM textvaltable T WHERE mother_tongue = t.TextCode) mother_tongue,case when  DistinctSport='0' then 'No' else (Select textval FROM textvaltable T WHERE DistinctSport = t.TextCode)   end as DistinctSport,case when CampusReq='0' then 'No' else 'Yes' end as CampusReq,case when handy='0' then 'No' else 'Yes' end as handy,case when islearningdis='0' then 'No' else 'Yes' end as islearningdis, isdisabledisc,case when isdisable='0' then 'No' else 'Yes' end as isdisable ,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no,a.missionarydisc,a.MissionaryChild,a.parent_phnoc,a.parent_pincodec,a.Student_Mobile,(Select textval FROM textvaltable T WHERE Countryc = t.TextCode) Countryc,(Select textval FROM textvaltable T WHERE parent_statec = t.TextCode) parent_statec,a.Cityc,a.Streetc,a.parent_addressC,(Select textval FROM textvaltable T WHERE co_curricular = t.TextCode) co_curricular,(Select textval FROM textvaltable T WHERE citizen = t.TextCode) citizen,a.visualhandy,a.first_graduate,(Select textval FROM textvaltable T WHERE TamilOrginFromAndaman = t.TextCode) TamilOrginFromAndaman ,(Select textval FROM textvaltable T WHERE seattype = t.TextCode) seattype,(Select textval FROM textvaltable T WHERE parent_occu = t.TextCode) parent_occu,(Select textval FROM textvaltable T WHERE caste = t.TextCode) caste,(Select textval FROM textvaltable T WHERE community = t.TextCode) community,(Select textval FROM textvaltable T WHERE religion = t.TextCode) religion,a.parent_name,''Roll_No,a.Stud_Name,a.Batch_Year,a.Current_Semester,c.Course_Name,Dt.Dept_Name,isnull( r.Sections,'') as Sections,CONVERT(VARCHAR(11),dob,103) as dob from Registration r,degree d,Stud_prev_details s, applyn a ,Department dt,Course C where a.app_no =s.app_no and r.App_No =a.app_no and isconfirm ='1'  and r.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "') and  a.Current_Semester in('" + sem_tagvalue + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  and d.Degree_Code =r.degree_code  and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  " + sectionvalue + " " + queryadd + " " + leftwaiting + " and s.Part1Language<>0 ";
                }
            }
            query = query + "sELECT a.App_No,uni_state,type_semester,university_code,ISNULL(pt.TExtVal,'') Part2Language,(Select textval FROM textvaltable T WHERE Part1Language = t.TextCode) Part1Language,ISNULL(C.TExtVal,'') Course,ISNULL(U.TextVal,'') University,ISNULL(Institute_Name,'') Institute_Name,ISNULL(PassMonth,'') PassMonth,ISNULL(PassYear,'') PassYear FROM Stud_prev_details P INNER JOIN applyn A ON P.app_no = A.app_no LEFT JOIN TextValTable C ON C.TextCode = P.course_code  LEFT JOIN TextValTable U ON U.TextCode = P.university_code LEFT JOIN TextValTable pa ON pa.TextCode = P.Part1Language LEFT JOIN TextValTable pt ON pt.TextCode = P.Part2Language Where p.app_no = a.app_no ";
            query = query + "select * from StudCertDetails s,applyn a where a.App_No=s.App_No";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (query == "")
            {
                Fpspread2.Sheets[0].Visible = false;
                lblerror.Visible = true;
                lblerror.Text = "Kindly Select All List ";
                div_report.Visible = false;
                lblvalidation1.Text = string.Empty;
                return;
            }
            else
            {
                if (query != "")
                {
                    ds = d2.select_method(query, hat, "Text");
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        Fpspread2.Sheets[0].Visible = false;
                        Fpspread2.Visible = false;
                        lblerror.Visible = true;
                        lblerror.Text = "No Records Found";
                        div_report.Visible = false;
                        lbl_headernamespd2.Visible = false;
                        lblvalidation1.Text = string.Empty;
                        return;
                    }
                    else
                    {
                        lblerror.Visible = false;
                        lbl_err_stud.Visible = false;
                        btn_viewsprd2.Visible = true;
                        lnk_admisstionformbtn.Visible = true;
                        img_settingpdf.Visible = true;
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            div_report.Visible = true;
                            lbl_headernamespd2.Visible = true;
                            if (name == "All")
                            {
                                lbl_headernamespd2.Text = name;
                            }
                            else
                            {
                                lbl_headernamespd2.Text = headertype + "-" + name;
                            }
                            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "AppNo";
                            // Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                            // string
                            int cc = 2;
                            int j = 0;
                            //loadlcolumns();
                            DataSet dss = new DataSet();
                            string linkname1 = Convert.ToString(ddl_colord.SelectedItem.Text);
                            string selcol1 = "select LinkValue from New_InsSettings where LinkName='" + linkname1 + "' and  user_code='" + usercode + "' and college_code='" + ddlcollege.SelectedItem.Value + "' ";
                            dss.Clear();
                            dss = d2.select_method_wo_parameter(selcol1, "Text");
                            if (dss.Tables.Count > 0)
                            {
                                if (dss.Tables[0].Rows.Count > 0)
                                {
                                    for (int c = 0; c < dss.Tables[0].Rows.Count; c++)
                                    {
                                        string value = Convert.ToString(dss.Tables[0].Rows[c]["LinkValue"]);
                                        if (value != "")
                                        {
                                            string[] valuesplit = value.Split(',');
                                            if (valuesplit.Length > 0)
                                            {
                                                for (int k = 0; k < valuesplit.Length; k++)
                                                {
                                                    cc++;
                                                    colval = Convert.ToString(valuesplit[k]);
                                                    loadtext();
                                                    Fpspread2.Sheets[0].ColumnCount = cc + 1;
                                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Text = loadval;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    imgdiv2.Visible = true;
                                    lbl_alert.Text = "No Records Found";
                                    Fpspread2.Visible = false;
                                    div_report.Visible = false;
                                    img_settingpdf.Visible = false;
                                    btn_viewsprd2.Visible = false;
                                    lnk_admisstionformbtn.Visible = false;
                                    lbl_headernamespd2.Visible = false;
                                    lblvalidation1.Text = string.Empty;
                                    return;
                                }
                            }
                            else
                            {
                                imgdiv2.Visible = true;
                                lbl_alert.Text = "Set Column Order";
                                Fpspread2.Visible = false;
                                div_report.Visible = false;
                                img_settingpdf.Visible = false;
                                lnk_admisstionformbtn.Visible = false;
                                btn_viewsprd2.Visible = false;
                                lbl_headernamespd2.Visible = false;
                                lblvalidation1.Text = string.Empty;
                                return;
                            }
                            string txt1 = string.Empty;
                            string txt2 = string.Empty;
                            string txt3 = string.Empty;
                            string txt4 = string.Empty;
                            string txt5 = string.Empty;
                            FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                            cb.AutoPostBack = true;
                            FarPoint.Web.Spread.CheckBoxCellType cball = new FarPoint.Web.Spread.CheckBoxCellType();
                            cball.AutoPostBack = true;
                            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (i == 0)
                                {
                                    Fpspread2.Sheets[0].RowCount++;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = cball;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                }
                                Fpspread2.Sheets[0].RowCount++;
                                count++;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Locked = true;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = cb;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["app_no"]); ;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["app_no"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Locked = true;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Locked = true;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                string admi_status = Convert.ToString(ds.Tables[0].Rows[i]["Admission_Status"]);
                                string delflag = Convert.ToString(ds.Tables[0].Rows[i]["DelFlag"]);
                                string examflg = Convert.ToString(ds.Tables[0].Rows[i]["Exam_Flag"]);
                                string coursecomp = Convert.ToString(ds.Tables[0].Rows[i]["CC"]);
                                if (admi_status == "False")
                                {
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].BackColor = ColorTranslator.FromHtml("#B287F2");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].BackColor = ColorTranslator.FromHtml("#B287F2");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#B287F2");
                                }
                                if (admi_status == "0")
                                {
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].BackColor = ColorTranslator.FromHtml("#B287F2");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].BackColor = ColorTranslator.FromHtml("#B287F2");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#B287F2");
                                }
                                else if (delflag == "1")
                                {
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].BackColor = ColorTranslator.FromHtml("#F77474");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].BackColor = ColorTranslator.FromHtml("#F77474");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#F77474");
                                }
                                else if (examflg == "DEBAR")
                                {
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].BackColor = ColorTranslator.FromHtml("#A4F9C9");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].BackColor = ColorTranslator.FromHtml("#A4F9C9");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#A4F9C9");
                                }
                                else if (coursecomp == "True")
                                {
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].BackColor = ColorTranslator.FromHtml("#65F7E1");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].BackColor = ColorTranslator.FromHtml("#65F7E1");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#65F7E1");
                                }
                                else
                                {
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].BackColor = ColorTranslator.FromHtml("#F2C77D");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].BackColor = ColorTranslator.FromHtml("#F2C77D");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#F2C77D");
                                }
                                cc = 2;
                                string text = string.Empty;
                                DataView dv = new DataView();
                                DataView dv1 = new DataView();
                                string linkname = Convert.ToString(ddl_colord.SelectedItem.Text);
                                string columnvalue = string.Empty;
                                DataSet dscol = new DataSet();
                                string selcol = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + ddlcollege.SelectedItem.Value + "' ";
                                dscol.Clear();
                                dscol = d2.select_method_wo_parameter(selcol, "Text");
                                if (dscol.Tables.Count > 0)
                                {
                                    if (dscol.Tables[0].Rows.Count > 0)
                                    {
                                        for (int c = 0; c < dscol.Tables[0].Rows.Count; c++)
                                        {
                                            string value = Convert.ToString(dscol.Tables[0].Rows[c]["LinkValue"]);
                                            if (value != "")
                                            {
                                                string[] valuesplit = value.Split(',');
                                                if (valuesplit.Length > 0)
                                                {
                                                    for (int k = 0; k < valuesplit.Length; k++)
                                                    {
                                                        Fpspread2.Sheets[0].ColumnCount = 3 + valuesplit.Length;
                                                        cc++;
                                                        colval = Convert.ToString(valuesplit[k]);
                                                        loadvalue();
                                                        string col = loadval;
                                                        if (col == "type_semester" || col == "Institute_name" || col == "isgrade" || col == "Part1Language" || col == "Part2Language" || col == "university_code")
                                                        {
                                                            if (ds.Tables[1].Rows.Count > 0)
                                                            {
                                                                if (ds.Tables[1].Rows.Count > 0)
                                                                {
                                                                    ds.Tables[1].DefaultView.RowFilter = "app_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_No"]) + "'";
                                                                    dv = ds.Tables[1].DefaultView;
                                                                    if (dv.Count > 0)
                                                                    {
                                                                        text = Convert.ToString(dv[0][col]);
                                                                    }
                                                                    else
                                                                    {
                                                                        text = string.Empty;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                text = string.Empty;
                                                            }
                                                        }
                                                        else if (col == "CommunityNo" || col == "TCNo" || col == "Ten_CertNo" || col == "Twelth_CertNo" || col == "DeplomProv_CertNo" || col == "DeplomConsolidate_CertNo" || col == "DeplomDegree_CertNo" || col == "UGProv_CertNo" || col == "UGConsolidate_CertNo" || col == "UGDegree_CertNo" || col == "PGProv_CertNo" || col == "PGConsolidate_CertNo" || col == "PGDegree_CertNo")
                                                        {
                                                            if (ds.Tables[2].Rows.Count > 0)
                                                            {
                                                                ds.Tables[2].DefaultView.RowFilter = "app_no='" + Convert.ToString(ds.Tables[0].Rows[i]["app_no"]) + "'";
                                                                dv1 = ds.Tables[2].DefaultView;
                                                                if (dv1.Count > 0)
                                                                {
                                                                    text = Convert.ToString(dv1[0][col]);
                                                                }
                                                                else
                                                                {
                                                                    text = string.Empty;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                text = string.Empty;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            text = Convert.ToString(ds.Tables[0].Rows[i][col]);
                                                        }
                                                        if (col == "visualhandy")
                                                        {
                                                            if (text == "0")
                                                            {
                                                                text = "No";
                                                            }
                                                            else if (text == "1")
                                                            {
                                                                text = "Yes";
                                                            }
                                                        }
                                                        if (col == "first_graduate")
                                                        {
                                                            if (text == "0")
                                                            {
                                                                text = "No";
                                                            }
                                                            else if (text == "1")
                                                            {
                                                                text = "Yes";
                                                            }
                                                        }
                                                        if (text == "0")
                                                        {
                                                            text = string.Empty;
                                                        }
                                                        if (text == "")
                                                        {
                                                            text = string.Empty;
                                                        }
                                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Text = text;
                                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Column.Width = 180;
                                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Locked = true;
                                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Font.Name = "Book Antiqua";
                                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Font.Size = FontUnit.Medium;
                                                        if (col == "Current_Semester")
                                                        {
                                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].HorizontalAlign = HorizontalAlign.Center;
                                                        }
                                                        if (admi_status == "False")
                                                        {
                                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].BackColor = ColorTranslator.FromHtml("#B287F2");
                                                        }
                                                        if (admi_status == "0")
                                                        {
                                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].BackColor = ColorTranslator.FromHtml("#B287F2");
                                                        }
                                                        else if (delflag == "1")
                                                        {
                                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].BackColor = ColorTranslator.FromHtml("#F77474");
                                                        }
                                                        else if (examflg == "DEBAR")
                                                        {
                                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].BackColor = ColorTranslator.FromHtml("#A4F9C9");
                                                        }
                                                        else if (coursecomp == "True")
                                                        {
                                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].BackColor = ColorTranslator.FromHtml("#65F7E1");
                                                        }
                                                        else
                                                        {
                                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].BackColor = ColorTranslator.FromHtml("#F2C77D");
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                        Fpspread2.Width = 900;
                        Fpspread2.Height = 420;
                        Fpspread2.Visible = true;
                        imgbtn_columsetting.Visible = true;
                        lblvalidation1.Text = string.Empty;
                        btn_viewsprd2.Visible = true;
                        lnk_admisstionformbtn.Visible = true;
                        if (Fpspread2.Columns.Count > 2)
                        {
                            //Fpspread2.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        }
                    }
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }

    public void selectcolumnload()
    {
        columnname = string.Empty;
        columnname1 = string.Empty;
        string linkname1 = Convert.ToString(ddl_colord.SelectedItem.Text);
        int cc = 0;
        string selcol1 = "select LinkValue from New_InsSettings where LinkName='" + linkname1 + "'  and college_code='" + ddlcollege.SelectedItem.Value + "' and user_code='" + usercode + "' ";
        ds = d2.select_method_wo_parameter(selcol1, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int c = 0; c < ds.Tables[0].Rows.Count; c++)
            {
                string value = Convert.ToString(ds.Tables[0].Rows[c]["LinkValue"]);
                if (value != "")
                {
                    string[] valuesplit = value.Split(',');
                    if (valuesplit.Length > 0)
                    {
                        for (int k = 0; k < valuesplit.Length; k++)
                        {
                            cc++;
                            colval = Convert.ToString(valuesplit[k]);
                            string c_name = columnload(colval);
                            string c_name1 = columnload1(colval);
                            if (c_name != "")
                            {
                                if (columnname == "")
                                {
                                    columnname = c_name;
                                }
                                else
                                {
                                    columnname = columnname + "," + c_name;
                                }
                                if (columnname1 == "")
                                {
                                    columnname1 = c_name1;
                                }
                                else
                                {
                                    columnname1 = columnname1 + "," + c_name1;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public string columnload(string v)//delsi
    {
        string value = string.Empty;
        if (colval == "1")
        {
            value = "c.Course_Name";
        }
        if (colval == "2")
        {
            value = "Dt.Dept_Name";
        }
        if (colval == "3")
        {
            value = "a.Batch_Year";
        }
        if (colval == "4")
        {
            value = "a.Current_Semester";
        }
        if (colval == "5")
        {
            value = "a.parent_name";
        }
        if (colval == "6")
        {
            value = "CONVERT(VARCHAR(11),dob,103) as dob ";
        }
        if (colval == "7")
        {
            value = "(Select textval FROM textvaltable T WHERE parent_occu = t.TextCode) parent_occu";
        }
        if (colval == "8")
        {
            value = "(Select textval FROM textvaltable T WHERE mother_tongue = t.TextCode) mother_tongue";
        }
        if (colval == "9")
        {
            value = "(Select textval FROM textvaltable T WHERE religion = t.TextCode) religion";
        }
        if (colval == "10")
        {
            value = "(Select textval FROM textvaltable T WHERE citizen = t.TextCode) citizen";
        }
        if (colval == "11")
        {
            value = "(Select textval FROM textvaltable T WHERE community = t.TextCode) community";
        }
        if (colval == "12")
        {
            value = "(Select textval FROM textvaltable T WHERE caste = t.TextCode) caste";
        }
        if (colval == "13")
        {
            value = "case when TamilOrginFromAndaman='0' then 'No' else 'Yes' end as  TamilOrginFromAndaman";
        }
        if (colval == "14")
        {
            value = "a.visualhandy";
        }
        if (colval == "15")
        {
            value = "a.first_graduate";
        }
        if (colval == "16")
        {
            value = "(Select textval FROM textvaltable T WHERE seattype = t.TextCode) seattype";
        }
        if (colval == "17")
        {
            value = "(Select textval FROM textvaltable T WHERE co_curricular = t.TextCode) co_curricular";
        }
        if (colval == "18")
        {
            value = "a.parent_addressP";
        }
        if (colval == "19")
        {
            value = "a.Streetp";
        }
        if (colval == "20")
        {
            value = "a.Cityp";
        }
        if (colval == "21")
        {
            value = "(Select textval FROM textvaltable T WHERE parent_statep = t.TextCode)parent_statep";
        }
        if (colval == "22")
        {
            value = "Countryp";
        }
        if (colval == "23")
        {
            value = "a.Student_Mobile";
        }
        if (colval == "24")
        {
            value = "a.parent_pincodep";
        }
        if (colval == "25")
        {
            value = "a.parent_phnop";
        }
        if (colval == "26")
        {
            value = "case when MissionaryChild='0' then 'No' else 'Yes' end as MissionaryChild";
        }
        if (colval == "27")
        {
            value = "a.missionarydisc";
        }
        if (colval == "34")
        {
            value = "''HostelName";
        }
        if (colval == "35")
        {
            value = "ElectionID_No";
        }
        //if (colval == "29")
        //{
        //    value = "Part1Language";
        //}
        //if (colval == "30")
        //{
        //    value = "Part2Language";
        //}
        //if (colval == "31")
        //{
        //    value = "university_code";
        //}
        if (colval == "48")
        {
            value = "case when CampusReq='0' then 'No' else 'Yes' end as CampusReq";
        }
        if (colval == "49")
        {
            value = "case when handy='0' then 'No' else 'Yes' end as handy";
        }
        if (colval == "50")
        {
            value = "case when  DistinctSport='0' then 'No' else (Select textval FROM textvaltable T WHERE DistinctSport = CONVERT(nvarchar(20), t.TextCode))   end as DistinctSport";
        }
        if (colval == "51")
        {
            value = "case when islearningdis='0' then 'No' else 'Yes' end as islearningdis";
        }
        if (colval == "52")
        {
            value = "isdisabledisc";
        }
        if (colval == "53")
        {
            value = "case when isdisable='0' then 'No' else 'Yes' end as isdisable";
        }
        //if (colval == "54")
        //{
        //    value = "r.Stud_Name";
        //}
        //if (colval == "55")
        //{
        //    value = "r.Roll_No";
        //}
        if (colval == "56")
        {
            value = "StuPer_Id";
        }
        //if (colval == "57")
        //{
        //    value = "r.Reg_No";
        //}
        if (colval == "58")
        {
            value = "'' roll_admit";
        }
        if (colval == "59")
        {
            value = "app_formno";
        }
        if (colval == "60")
        {
            value = "isnull( r.Sections,'') as Sections";
        }
        if (colval == "61")
        {
            value = "case when sex='0' then 'Male' else 'Female' end as sex";
        }
        if (colval == "62")
        {
            value = "(Select textval FROM textvaltable T WHERE bldgrp = t.TextCode) bldgrp";
        }
        if (colval == "63")
        {
            value = "r.stud_type";
        }
        if (colval == "64")
        {
            value = "case when IsExService='0' then 'No' else 'Yes' end as IsExService";
        }
        if (colval == "65")
        {
            value = "case when CampusReq='0' then 'No' else 'Yes' end as CampusReq";
        }
        if (colval == "66")
        {
            value = "case when isdonar='0' then 'No' else 'Yes' end as isdonar";
        }
        if (colval == "67")
        {
            value = "case when ReserveCategory='0' then 'No' else 'Yes' end as  ReserveCategory";
        }
        if (colval == "68")
        {
            value = "case when EconBackword='0' then 'No' else 'Yes' end as EconBackword";
        }
        if (colval == "69")
        {
            value = "case when parentoldstud='0' then 'No' else 'Yes' end as parentoldstud";
        }
        if (colval == "70")
        {
            value = "case when IsDrivingLic='0' then 'No' else 'Yes' end as IsDrivingLic";
        }
        if (colval == "71")
        {
            value = "Driving_details";
        }
        if (colval == "72")
        {
            value = "case when tutionfee_waiver='0' then 'No' else 'Yes' end as tutionfee_waiver";
        }
        if (colval == "73")
        {
            value = "case when IsInsurance='0' then 'No' else 'Yes' end as IsInsurance";
        }
        if (colval == "74")
        {
            value = "ExsRank";
        }
        if (colval == "75")
        {
            value = "ExSPlace";
        }
        if (colval == "76")
        {
            value = "ExsNumber";
        }
        if (colval == "77")
        {
            value = "Insurance_Amount";
        }
        if (colval == "78")
        {
            value = "Insurance_InsBy";
        }
        if (colval == "79")
        {
            value = "Insurance_Nominee";
        }
        if (colval == "80")
        {
            value = "Insurance_NominRelation";
        }
        if (colval == "81")
        {
            value = "CONVERT(VARCHAR(11),date_applied,103) as date_applied";
        }
        if (colval == "82")
        {
            value = "alter_mobileno";
        }
        if (colval == "83")
        {
            //magesh 29/1/18
            value = "(Select textval FROM textvaltable T WHERE SubCaste = t.TextCode) SubCaste";
            // value = "SubCaste";
        }
        if (colval == "84")
        {
            value = "(Select textval FROM textvaltable T WHERE parent_income = t.TextCode) parent_income";
        }
        if (colval == "85")
        {
            value = "parentF_Mobile";
        }
        if (colval == "86")
        {
            value = "emailp";
        }
        if (colval == "87")
        {
            value = "mother";
        }
        if (colval == "88")
        {
            value = "(Select textval FROM textvaltable T WHERE mIncome = t.TextCode) mIncome";
        }
        if (colval == "89")
        {
            value = "parentM_Mobile";
        }
        if (colval == "90")
        {
            value = "emailM";
        }
        if (colval == "91")
        {
            value = "guardian_name";
        }
        if (colval == "92")
        {
            value = "guardian_mobile";
        }
        if (colval == "93")
        {
            value = "emailg";
        }
        if (colval == "94")
        {
            value = "place_birth";
        }
        if (colval == "95")
        {
            value = "Aadharcard_no";
        }
        if (colval == "96")
        {
            value = "(Select textval FROM textvaltable T WHERE motherocc = t.TextCode) motherocc";
        }
        if (colval == "108")
        {
            value = "a.parent_addressC";
        }
        if (colval == "109")
        {
            value = "a.Streetc";
        }
        if (colval == "110")
        {
            value = "a.Cityc";
        }
        if (colval == "111")
        {
            value = "(Select textval FROM textvaltable T WHERE parent_statec = t.TextCode) parent_statec";
        }
        if (colval == "112")
        {
            value = "Countryc";
        }
        if (colval == "113")
        {
            value = "a.parent_pincodec";
        }
        if (colval == "122")
        {
            value = "(Select Stage_Name FROM Stage_Master T WHERE Boarding = T.stage_id) Boarding";
        }
        if (colval == "123")
        {
            value = "vehid";
        }
        if (colval == "43")
        {
            value = "case when a.Mode='1' then 'Regular' when a.mode='2' then 'Transfer' when a.mode='3' then 'Lateral' end Mode ";
        }
        if (colval == "36")//delsii
        {
            value = "CONVERT(varchar(10), r.Adm_Date,103)Adm_Date";
        }
        if (colval == "37")
        {
            value = "CONVERT(varchar(10), a.enrollment_confirm_date,103)enrollment_confirm_date";
        }
        if (colval == "128")
        {
            value = "DATEPART (day,dob) 'day'";
        }
        if (colval == "129")
        {
            value = "DATEPART(MONTH, dob) 'Month'";
        }
        if (colval == "130")
        {
            value = "DATEPART(YEAR, dob) 'Year'";
        }
        return value;
    }

    public string columnload1(string v)
    {
        string value = string.Empty;
        if (colval == "1")
        {
            value = "c.Course_Name";
        }
        if (colval == "2")
        {
            value = "Dt.Dept_Name";
        }
        if (colval == "3")
        {
            value = "a.Batch_Year";
        }
        if (colval == "4")
        {
            value = "a.Current_Semester";
        }
        if (colval == "5")
        {
            value = "a.parent_name";
        }
        if (colval == "6")
        {
            value = "CONVERT(VARCHAR(11),dob,103) as dob ";
        }
        if (colval == "7")
        {
            value = "(Select textval FROM textvaltable T WHERE parent_occu = t.TextCode) parent_occu";
        }
        if (colval == "8")
        {
            value = "(Select textval FROM textvaltable T WHERE mother_tongue = t.TextCode) mother_tongue";
        }
        if (colval == "9")
        {
            value = "(Select textval FROM textvaltable T WHERE religion = t.TextCode) religion";
        }
        if (colval == "10")
        {
            value = "(Select textval FROM textvaltable T WHERE citizen = t.TextCode) citizen";
        }
        if (colval == "11")
        {
            value = "(Select textval FROM textvaltable T WHERE community = t.TextCode) community";
        }
        if (colval == "12")
        {
            value = "(Select textval FROM textvaltable T WHERE caste = t.TextCode) caste";
        }
        if (colval == "13")
        {
            value = "case when TamilOrginFromAndaman='0' then 'No' else 'Yes' end as  TamilOrginFromAndaman";
        }
        if (colval == "14")
        {
            value = "a.visualhandy";
        }
        if (colval == "15")
        {
            value = "a.first_graduate";
        }
        if (colval == "16")
        {
            value = "(Select textval FROM textvaltable T WHERE seattype = t.TextCode) seattype";
        }
        if (colval == "17")
        {
            value = "(Select textval FROM textvaltable T WHERE co_curricular = t.TextCode) co_curricular";
        }
        if (colval == "18")
        {
            value = "a.parent_addressP";
        }
        if (colval == "19")
        {
            value = "a.Streetp";
        }
        if (colval == "20")
        {
            value = "a.Cityp";
        }
        if (colval == "21")
        {
            value = "(Select textval FROM textvaltable T WHERE parent_statep = t.TextCode)parent_statep";
        }
        if (colval == "22")
        {
            value = "Countryp";
        }
        if (colval == "23")
        {
            value = "a.Student_Mobile";
        }
        if (colval == "24")
        {
            value = "a.parent_pincodep";
        }
        if (colval == "25")
        {
            value = "a.parent_phnop";
        }
        if (colval == "26")
        {
            value = "case when MissionaryChild='0' then 'No' else 'Yes' end as MissionaryChild";
        }
        if (colval == "27")
        {
            value = "a.missionarydisc";
        }
        //if (colval == "28")
        //{
        //    value = "Institute_name";
        //}
        //if (colval == "29")
        //{
        //    value = "Part1Language";
        //}
        //if (colval == "30")
        //{
        //    value = "Part2Language";
        //}
        //if (colval == "31")
        //{
        //    value = "university_code";
        //}
        if (colval == "34")
        {
            value = "''HostelName";
        }
        if (colval == "35")
        {
            value = "ElectionID_No";
        }
        if (colval == "48")
        {
            value = "case when CampusReq='0' then 'No' else 'Yes' end as CampusReq";
        }
        if (colval == "49")
        {
            value = "case when handy='0' then 'No' else 'Yes' end as handy";
        }
        if (colval == "50")
        {
            value = "case when  DistinctSport='0' then 'No' else (Select textval FROM textvaltable T WHERE DistinctSport = CONVERT(nvarchar(20), t.TextCode))   end as DistinctSport";
        }
        if (colval == "51")
        {
            value = "case when islearningdis='0' then 'No' else 'Yes' end as islearningdis";
        }
        if (colval == "52")
        {
            value = "isdisabledisc";
        }
        if (colval == "53")
        {
            value = "case when isdisable='0' then 'No' else 'Yes' end as isdisable";
        }
        //if (colval == "54")
        //{
        //    value = "r.Stud_Name";
        //}
        //if (colval == "55")
        //{
        //    value = "r.Roll_No";
        //}
        if (colval == "56")
        {
            value = "StuPer_Id";
        }
        //if (colval == "57")
        //{
        //    value = "r.Reg_No";
        //}
        if (colval == "58")
        {
            value = "'' roll_admit";
        }
        if (colval == "59")
        {
            value = "app_formno";
        }
        if (colval == "60")
        {
            value = "''Sections";
        }
        if (colval == "61")
        {
            value = "case when sex='0' then 'Male' else 'Female' end as sex";
        }
        if (colval == "62")
        {
            value = "(Select textval FROM textvaltable T WHERE bldgrp = t.TextCode) bldgrp";
        }
        if (colval == "63")
        {
            value = "a.stud_type";
        }
        if (colval == "64")
        {
            value = "case when IsExService='0' then 'No' else 'Yes' end as IsExService";
        }
        if (colval == "65")
        {
            value = "case when CampusReq='0' then 'No' else 'Yes' end as CampusReq";
        }
        if (colval == "66")
        {
            value = "case when isdonar='0' then 'No' else 'Yes' end as isdonar";
        }
        if (colval == "67")
        {
            value = "case when ReserveCategory='0' then 'No' else 'Yes' end as  ReserveCategory";
        }
        if (colval == "68")
        {
            value = "case when EconBackword='0' then 'No' else 'Yes' end as EconBackword";
        }
        if (colval == "69")
        {
            value = "case when parentoldstud='0' then 'No' else 'Yes' end as parentoldstud";
        }
        if (colval == "70")
        {
            value = "case when IsDrivingLic='0' then 'No' else 'Yes' end as IsDrivingLic";
        }
        if (colval == "71")
        {
            value = "Driving_details";
        }
        if (colval == "72")
        {
            value = "case when tutionfee_waiver='0' then 'No' else 'Yes' end as tutionfee_waiver";
        }
        if (colval == "73")
        {
            value = "case when IsInsurance='0' then 'No' else 'Yes' end as IsInsurance";
        }
        if (colval == "74")
        {
            value = "ExsRank";
        }
        if (colval == "75")
        {
            value = "ExSPlace";
        }
        if (colval == "76")
        {
            value = "ExsNumber";
        }
        if (colval == "77")
        {
            value = "Insurance_Amount";
        }
        if (colval == "78")
        {
            value = "Insurance_InsBy";
        }
        if (colval == "79")
        {
            value = "Insurance_Nominee";
        }
        if (colval == "80")
        {
            value = "Insurance_NominRelation";
        }
        if (colval == "81")
        {
            value = "CONVERT(VARCHAR(11),date_applied,103) as date_applied";
        }
        if (colval == "82")
        {
            value = "alter_mobileno";
        }
        if (colval == "83")
        {
            //magesh 29/1/18
            value = "(Select textval FROM textvaltable T WHERE SubCaste = t.TextCode) SubCaste";
            // value = "SubCaste";
        }
        if (colval == "84")
        {
            value = "(Select textval FROM textvaltable T WHERE parent_income = t.TextCode) parent_income";
        }
        if (colval == "85")
        {
            value = "parentF_Mobile";
        }
        if (colval == "86")
        {
            value = "emailp";
        }
        if (colval == "87")
        {
            value = "mother";
        }
        if (colval == "88")
        {
            value = "(Select textval FROM textvaltable T WHERE mIncome = t.TextCode) mIncome";
        }
        if (colval == "89")
        {
            value = "parentM_Mobile";
        }
        if (colval == "90")
        {
            value = "emailM";
        }
        if (colval == "91")
        {
            value = "guardian_name";
        }
        if (colval == "92")
        {
            value = "guardian_mobile";
        }
        if (colval == "93")
        {
            value = "emailg";
        }
        if (colval == "94")
        {
            value = "place_birth";
        }
        if (colval == "95")
        {
            value = "Aadharcard_no";
        }
        if (colval == "96")
        {
            value = "(Select textval FROM textvaltable T WHERE motherocc = t.TextCode) motherocc";
        }
        if (colval == "108")
        {
            value = "a.parent_addressC";
        }
        if (colval == "109")
        {
            value = "a.Streetc";
        }
        if (colval == "110")
        {
            value = "a.Cityc";
        }
        if (colval == "111")
        {
            value = "(Select textval FROM textvaltable T WHERE parent_statec = t.TextCode) parent_statec";
        }
        if (colval == "112")
        {
            value = "Countryc";
        }
        if (colval == "113")
        {
            value = "a.parent_pincodec";
        }
        if (colval == "122")
        {
            value = "'' Boarding";
        }
        if (colval == "123")
        {
            value = "''vehid";
        }
        if (colval == "43")
        {
            // value = "case when r.Mode='1' then 'Regular' when r.mode='2' then 'Transfer' when r.mode='3' then 'Lateral' end Mode ";
            value = "case when a.Mode='1' then 'Regular' when a.mode='2' then 'Transfer' when a.mode='3' then 'Lateral' end Mode ";
        }
        if (colval == "36")
        {
            value = "CONVERT(varchar(10), r.Adm_Date,103)Adm_Date";
        }
        if (colval == "37")
        {
            value = " CONVERT(varchar(10), a.enrollment_confirm_date,103)enrollment_confirm_date";
        }
        if (colval == "38")
        {
            value = "CONVERT(varchar(10), r.Adm_Date,103)Adm_Date";
        }
        if (colval == "128")
        {
            value = "DATEPART (day,dob) 'day'";
        }
        if (colval == "129")
        {
            value = "DATEPART(MONTH, dob) 'Month'";
        }
        if (colval == "130")
        {
            value = "DATEPART(YEAR, dob) 'Year'";
        }
        return value;//delsii
    }

    public void fpspread1go1()
    {
        try
        {
            RollAndRegSettings();
            string orderStr = d2.GetFunction("select value from Master_Settings where settings='order_by'");
            if (orderStr == "0")
            {
                if (roll == 0)
                    orderStr = " Order by roll_no,reg_no,roll_admit ";
                else if (roll == 1)
                    orderStr = " Order by roll_no,reg_no,roll_admit ";
                else if (roll == 2)
                    orderStr = " Order by roll_no ";
                else if (roll == 3)
                    orderStr = " Order by reg_no ";
                else if (roll == 4)
                    orderStr = " Order by roll_admit ";
                else if (roll == 5)
                    orderStr = " Order by roll_no,reg_no ";
                else if (roll == 6)
                    orderStr = " Order by reg_no,roll_admit ";
                else if (roll == 7)
                    orderStr = " Order by roll_no,roll_admit ";
            }
            else
            {
                if (orderStr == "0")
                    orderStr = "ORDER BY r.Roll_No";
                else if (orderStr == "1")
                    orderStr = "ORDER BY r.Reg_No";
                else if (orderStr == "2")
                    orderStr = "ORDER BY r.Stud_Name";
                else if (orderStr == "0,1,2")
                    orderStr = "ORDER BY r.Roll_No,r.Reg_No,r.Stud_Name";
                else if (orderStr == "0,1")
                    orderStr = "ORDER BY r.Roll_No,r.Reg_No";
                else if (orderStr == "1,2")
                    orderStr = "ORDER BY r.Reg_No,r.Stud_Name";
                else if (orderStr == "0,2")
                    orderStr = "ORDER BY r.Roll_No,r.Stud_Name";
            }

            lbl_headernamespd2.Visible = true;
            btn_viewsprd2.Visible = true;
            lnk_admisstionformbtn.Visible = true;
            img_settingpdf.Visible = true;
            Fpspread2.Visible = true;

            div_report.Visible = true;
            string activerow = string.Empty;
            string activecol = string.Empty;
            string boards = string.Empty;
            string states = string.Empty;
            int val = 0;
            int count = 0;
            int count1 = 0;
            int i = 0;
            string header = string.Empty;
            string actval = string.Empty;
            string sectionvalue = string.Empty;
            string headertype1 = string.Empty;
            string headertype = string.Empty;
            activerow = Convert.ToString(Fpspread1.ActiveSheetView.ActiveRow);
            activecol = Convert.ToString(Fpspread1.ActiveSheetView.ActiveColumn);
            string sec_textvalue = string.Empty;
            if (activerow.Trim() != "-1" && activecol.Trim() != "-1")
            {
                sec_textvalue = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text);
            }
            else
            {
                Fpspread2.Visible = false;
                lblerror.Visible = true;
                lblerror.Text = "No Records Found ";
                div_report.Visible = false;
                lblvalidation1.Text = string.Empty;
                lbl_headernamespd2.Text = string.Empty;
                return;
            }
            string Batch_tagvalue = string.Empty;
            string dept_tagvalue = string.Empty;
            string course_tagvalue = string.Empty;
            string sem_tagvalue = string.Empty;
            string sec_tagvalue = string.Empty;
            string FromdateApplyn = string.Empty;
            string FromdateReg = string.Empty;
            string FromdateTransHostel = string.Empty;
            DateTime from = new DateTime();
            DateTime to = new DateTime();
            if (cb_from.Checked == true)
            {
                string[] ay = txt_fromdate.Text.Split('/');
                string[] ay1 = txt_todate.Text.Split('/');
                //magesh 8.3.18
                //from = Convert.ToDateTime(ay[1] + "/" + ay[0] + "/" + ay[2]);
                //to = Convert.ToDateTime(ay1[1] + "/" + ay1[0] + "/" + ay1[2]);
                //FromdateApplyn = "  and date_applied between '" + from.ToString("MM/dd/yyyy") + "' and '" + to.ToString("MM/dd/yyyy") + "'";
                //FromdateReg = "  and Adm_Date between '" + from.ToString("MM/dd/yyyy") + "' and '" + to.ToString("MM/dd/yyyy") + "'";
                //FromdateTransHostel = "  and d.Discontinue_Date between '" + from.ToString("dd/MM/yyyy") + "' and '" + to.ToString("dd/MM/yyyy") + "'";
                from = Convert.ToDateTime(ay[2] + "-" + ay[1] + "/" + ay[0]);
                to = Convert.ToDateTime(ay1[2] + "/" + ay1[1] + "/" + ay1[0]);
                FromdateApplyn = "  and date_applied between '" + from.ToString("yyyy/MM/dd") + "' and '" + to.ToString("yyyy/MM/dd") + "'";
                FromdateReg = "  and Adm_Date between '" + from.ToString("yyyy/MM/dd") + "' and '" + to.ToString("yyyy/MM/dd") + "'";
                FromdateTransHostel = "  and d.Discontinue_Date between '" + from.ToString("yyyy/MM/dd") + "' and '" + to.ToString("yyyy/MM/dd") + "'";
            }
            if (sec_textvalue != "Total")
            {
                Batch_tagvalue = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                course_tagvalue = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                dept_tagvalue = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag);
                sem_tagvalue = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Tag);
                sec_tagvalue = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Tag);
            }
            else
            {
                for (i = 0; i < cbl_batch.Items.Count; i++)
                {
                    if (cbl_batch.Items[i].Selected == true)
                    {
                        string addbatch1 = cbl_batch.Items[i].Value.ToString();
                        if (Batch_tagvalue == "")
                        {
                            Batch_tagvalue = addbatch1;
                        }
                        else
                        {
                            Batch_tagvalue = Batch_tagvalue + "'" + "," + "'" + addbatch1;
                        }
                    }
                }
                for (i = 0; i < cbl_branch.Items.Count; i++)
                {
                    if (cbl_branch.Items[i].Selected == true)
                    {
                        string adddeg1 = cbl_branch.Items[i].Value.ToString();
                        if (dept_tagvalue == "")
                        {
                            dept_tagvalue = adddeg1;
                        }
                        else
                        {
                            dept_tagvalue = dept_tagvalue + "'" + "," + "'" + adddeg1;
                        }
                    }
                }
                for (i = 0; i < cbl_sem.Items.Count; i++)
                {
                    if (cbl_sem.Items[i].Selected == true)
                    {
                        string addsem1 = cbl_sem.Items[i].Value.ToString();
                        if (sem_tagvalue == "")
                        {
                            sem_tagvalue = addsem1;
                        }
                        else
                        {
                            sem_tagvalue = sem_tagvalue + "'" + "," + "'" + addsem1;
                        }
                    }
                }
                for (i = 0; i < cbl_sec.Items.Count; i++)
                {
                    if (cbl_sec.Items[i].Selected == true)
                    {
                        string addsem1 = cbl_sec.Items[i].Value.ToString();
                        if (sec_tagvalue == "")
                        {
                            sec_tagvalue = addsem1;
                        }
                        else
                        {
                            sec_tagvalue = sec_tagvalue + "'" + "," + "'" + addsem1;
                        }
                    }
                }
            }
            if (Convert.ToInt32(activecol) <= 6)
            {
                header = "All";
                val = 0;
            }
            else
            {
                actval = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Text);
                header = Fpspread1.Sheets[0].ColumnHeader.Cells[Convert.ToInt32(0), Convert.ToInt32(activecol)].Text;
                headertype = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[Convert.ToInt32(0), Convert.ToInt32(activecol)].Tag);
                headertype1 = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[Convert.ToInt32(0), Convert.ToInt32(activecol)].Text);
            }
            string BoardFilter = string.Empty;
            string StateFilter = string.Empty;
            //if (cb_board.Checked == true)//delsij
            //{
            //    string board = rs.GetSelectedItemsValueAsString(cbl_BoardUniv);
            //    //boards = " And p.university_code in('" + board + "')";
            //    //BoardFilter = " And s.university_code in('" + board + "')";
            //   // boards = " And p.course_code in('" + board + "')";
            //    BoardFilter = " And s.course_code in('" + board + "')";
            //    headertype = "Board";
            //}
            //if (cb_state.Checked == true)
            //{
            //    string state = rs.GetSelectedItemsValueAsString(cbl_state);
            //    states = " And p.uni_state in('" + state + "') ";
            //    StateFilter = " And s.uni_state in('" + state + "')";
            //    headertype = "State";
            //}
            if (sec_tagvalue != "")
            {
                sectionvalue = " AND ISNULL( r.Sections,'') in('','" + sec_tagvalue + "')";
                //sectionvalue =string.Empty;
            }
            else
            {
                sectionvalue = string.Empty;
            }
            #region
            if (actval == "" || actval.Trim() == "-")
            {
                Fpspread2.Visible = false;
                lblerror.Visible = true;
                lblerror.Text = "No Records Found ";
                div_report.Visible = false;
                lblvalidation1.Text = string.Empty;
                lbl_headernamespd2.Text = string.Empty;
                return;
            }
            else
            {
                string name = header;
                string addqur = string.Empty;
                if (headertype == "Stud_Type" || headertype == "seattype" || headertype == "mode" || headertype == "religion" || headertype == "community" || headertype == "Transport" || headertype == "course_code" || headertype == "allotcomm" || headertype == "typenamevalue" || headertype == "typesizevalue" || headertype == "entryusercode" || headertype == "parent_statep" || headertype == "Direct_Refer")
                {
                    val = 13;
                    if (headertype == "Stud_Type")
                    {
                        addqur = " and r.Stud_Type='" + header + "'";
                    }
                    if (headertype == "course_code")//abarna
                    {

                        header = d2.GetFunction("SELECT course_code FROM stud_prev_details A,TextValTable T WHERE T.TextCode =A.course_code AND   T.TextVal='" + header + "' and T.college_code ='" + collegecode1 + "' ");
                        addqur = " and s.course_code='" + header + "'";
                    }
                    if (headertype == "seattype")
                    {
                        header = d2.GetFunction("SELECT seattype FROM applyn A,TextValTable T WHERE T.TextCode =A.seattype AND a.college_code ='" + collegecode1 + "' and T.TextVal='" + header + "' ");
                        addqur = " and a.seattype='" + header + "'";
                    }
                    if (headertype == "parent_statep")//abarna
                    {
                        header = d2.GetFunction("SELECT parent_statep FROM applyn A,TextValTable T WHERE T.TextCode =A.parent_statep AND a.college_code ='" + collegecode1 + "' and T.TextVal='" + header + "' ");
                        addqur = " and a.parent_statep='" + header + "'";
                    }
                    if (headertype == "mode")
                    {
                        headertype = "Type";
                        if (header == "Regular")
                        {
                            header = "1";
                        }
                        else if (header == "Lateral")
                        {
                            header = "3";
                        }
                        else if (header == "Transfer")
                        {
                            header = "2";
                        }
                        else if (header == "IrRegular")
                        {
                            header = "4";
                        }
                        if (ddl_status.SelectedIndex < 2)
                        {
                            addqur = " and a.mode='" + header + "'";
                        }
                        else
                        {
                            addqur = " and r.mode='" + header + "'";
                        }
                    }
                    if (headertype == "religion")
                    {
                        header = d2.GetFunction("SELECT religion FROM applyn A,TextValTable T WHERE T.TextCode =A.religion AND a.college_code ='" + collegecode1 + "' and T.TextVal='" + header + "' ");
                        addqur = " and a.religion='" + header + "'";
                    }
                    if (headertype == "community")
                    {
                        header = d2.GetFunction("SELECT community FROM applyn A,TextValTable T WHERE T.TextCode =A.community AND a.college_code ='" + collegecode1 + "' and T.TextVal='" + header + "' ");
                        addqur = " and a.community='" + header + "'";
                    }
                    if (headertype == "allotcomm")
                    {
                        header = d2.GetFunction("SELECT allotcomm FROM applyn A,TextValTable T WHERE T.TextCode =A.allotcomm AND a.college_code ='" + collegecode1 + "' and T.TextVal='" + header + "' ");
                        addqur = " and a.allotcomm='" + header + "'";
                    }
                    if (headertype == "typenamevalue")
                    {
                        header = d2.GetFunction("SELECT typenamevalue FROM St_personalInfod A,co_mastervalues T WHERE T.mastercode =A.typenamevalue AND T.mastervalue='" + name + "' ");
                        addqur = " and st.typenamevalue='" + header + "'";
                    }
                    if (headertype == "typesizevalue")
                    {
                        header = d2.GetFunction("SELECT typesizevalue FROM St_personalInfod A,co_mastervalues T WHERE T.mastercode =A.typesizevalue AND T.mastervalue='" + name + "' ");
                        addqur = " and st.typesizevalue='" + header + "'";
                    }
                    if (headertype == "Direct_Refer")
                    {
                        if (header.Trim() == "Staff")
                        {
                            header = "1";
                        }
                        else if (header.Trim() == "Student")
                        {
                            header = "3";
                        }
                        else if (header.Trim() == "Direct")
                        {
                            header = "0";
                        }
                        else if (header.Trim() == "Others")
                        {
                            header = "2";
                        }

                        addqur = " and a.Direct_Refer='" + header + "'";
                    }
                    if (ddl_status.SelectedIndex == 0)
                    {
                        if (headertype == "entryusercode")
                        {
                            header = d2.GetFunction("SELECT user_code from usermaster where user_id='" + header + "'");
                            addqur = " and r.entryusercode='" + header + "'";
                        }
                    }
                    else
                    {
                        if (headertype == "entryusercode")
                        {
                            header = d2.GetFunction("SELECT user_code from usermaster where user_id='" + header + "'");
                            addqur = " and r.entryusercode='" + header + "'";
                        }
                    }
                    if (headertype == "Transport")
                    {
                        if (header == "Own Transport")
                        {
                            addqur = " and  isnull(Boarding,'') ='' ";
                        }
                        else
                        {
                            addqur = " and  isnull(Boarding,'') <>''  and Boarding is not null";
                        }
                    }
                }
                if (headertype1 == "Applied" || headertype1 == "Applied All")//modified abarna 12.09.2018
                {
                    header = string.Empty;
                    val = 6;
                }
                if (headertype1 == "Waiting For Admitted")
                {
                    header = string.Empty;
                    val = 7;
                }
                if (headertype1 == "Left")
                {
                    header = string.Empty;
                    val = 8;
                }
                if (headertype1 == "Admitted")
                {
                    header = string.Empty;
                    val = 9;
                }
                if (headertype1 == "De-Bar")
                {
                    header = string.Empty;
                    val = 10;
                }
                if (headertype1 == "Discontinue")
                {
                    header = string.Empty;
                    val = 11;
                }
                if (headertype1 == "Course Completed")
                {
                    header = string.Empty;
                    val = 12;
                }

                //Added by saranya on 05/01/2018//
                if (headertype1 == "Transport Canceled Student")
                {
                    header = string.Empty;
                    val = 15;
                }
                if (headertype1 == "Hostel Canceled Student")
                {
                    header = string.Empty;
                    val = 16;
                }
                if (headertype1 == "Enquiry")
                {
                    header = string.Empty;
                    val = 17;
                }
                //if (headertype == "Refered By")
                //{
                //    val = 127;
                //}

                //=================================//
                //if (headertype == "CGPA")
                //{
                //    val = 125;
                //}
                //if (headertype == "No Of Arrear")
                //{
                //    val = 126;
                //}
                if (headertype == "Board" || headertype == "State")
                    val = 13;
                string queryadd = string.Empty;
                if (headertype == "residency" || headertype == "DistinctSport" || headertype == "Part1Language" || headertype == "mother_tongue" || headertype == "PhysicalChallanged")
                {
                    val = 13;
                    if (headertype == "residency")
                    {
                        if (header.Trim() == "Campus Required")
                        {
                            header = "1";
                        }
                        else
                        {
                            header = "0";
                        }
                        queryadd = " and CampusReq='" + header + "'";
                    }
                    if (headertype == "DistinctSport")
                    {
                        if (header.Trim() == "IsSports")
                        {
                            queryadd = " and DistinctSport IS NOT NULL and DistinctSport <>'0'";
                        }
                        else
                        {
                            header = d2.GetFunction("SELECT DistinctSport FROM applyn A,TextValTable T WHERE T.TextCode =A.DistinctSport AND a.college_code ='" + ddlcollege.SelectedItem.Value + "' and T.TextVal='" + header + "' ");
                            queryadd = " and DistinctSport='" + header + "'";
                        }
                    }
                    if (headertype == "Part1Language")
                    {
                        header = d2.GetFunction("select  Part1Language  from applyn a,Stud_prev_details s,TextValTable t where a.app_no =s.app_no and s.Part1Language =t.TextCode and a.college_code='" + ddlcollege.SelectedItem.Value + "' and  T.TextVal='" + header + "'  ");
                        queryadd = " and Part1Language='" + header + "'";
                    }
                    if (headertype == "mother_tongue")
                    {
                        header = d2.GetFunction("select  mother_tongue  from applyn a,TextValTable t where  a.mother_tongue =t.TextCode and a.college_code='" + ddlcollege.SelectedItem.Value + "' and  T.TextVal='" + header + "'");
                        queryadd = " and mother_tongue='" + header + "'";
                    }
                    if (headertype == "PhysicalChallanged")
                    {
                        if (header.Trim() == "IsDisable")
                        {
                            queryadd = " and  isdisable ='" + "True" + "'";
                        }
                        else if (header.Trim() == "Visually Challanged")
                        {
                            queryadd = " and  visualhandy ='" + 1 + "'";
                        }
                        else if (header.Trim() == "Physically Challanged")
                        {
                            queryadd = " and  handy ='" + 1 + "'";
                        }
                        else if (header.Trim() == "Learning Disability")
                        {
                            queryadd = " and  islearningdis ='" + "True" + "'";
                        }
                        else
                        {
                            queryadd = " and  isdisabledisc<>''";
                        }
                    }
                }

            #endregion
                Fpspread2.Sheets[0].Visible = true;
                Fpspread2.Sheets[0].RowHeader.Visible = false;
                Fpspread2.CommandBar.Visible = false;
                Fpspread2.Sheets[0].AutoPostBack = false;
                Fpspread2.Sheets[0].RowCount = 0;
                Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread2.Sheets[0].ColumnCount = 3;
                FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
                darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle2.ForeColor = Color.Black;
                darkstyle2.HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
                loadlcolumns();
                string query = string.Empty;
                columnname = string.Empty;
                columnname1 = string.Empty;
                selectcolumnload();
                if (columnname != "")
                {
                    columnname = "," + columnname;
                }
                if (columnname1 != "")
                {
                    columnname1 = "," + columnname1;
                }
                if (val == 1 || val == 125 || val == 126)
                {
                    //query = "select  distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + " from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='1' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " " + addqur + " " + orderStr + "  ";//order by r.Roll_No,r.Stud_Name,r.Reg_No

                    //Cmd By Saranyadevi29.12.2018
                    //query = "select  distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='1' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " " + addqur + " " + orderStr + "  ";//order by r.Roll_No,r.Stud_Name,r.Reg_No

                    query = "select  distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else ((select refer_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else ((select refer_agent_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end as refer_agent from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='1' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " " + addqur + " " + orderStr + "";

                }

                else if (val == 6 || (ddl_status.SelectedItem.Value == "1" && val == 0))
                {
                    if (rdb_cumm.Checked == true)
                    {
                        selectcolumnload();
                        if (columnname != "")
                        {
                            columnname = "," + columnname;
                        }
                        //query = "select distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + " from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') and ISNULL(admission_status,'0')='0'   and ISNULL(selection_status,'0')='0' " + sectionvalue + " " + FromdateReg + " " + orderStr + "     ";// order by r.Roll_No,r.Stud_Name,r.Reg_No   and a.app_no not in(select r.app_no from registration r)

                        //Cmd By Saranyadevi29.12.2018
                        //query = "select distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') and ISNULL(admission_status,'0')='0'   and ISNULL(selection_status,'0')='0' " + sectionvalue + " " + FromdateReg + " " + orderStr + "     ";

                        query = "select distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else ((select refer_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else ((select refer_agent_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end as refer_agent from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') and ISNULL(admission_status,'0')='0'   and ISNULL(selection_status,'0')='0' " + sectionvalue + " " + FromdateReg + " " + orderStr + " ";
                    }
                    else
                    {
                        //query = "select distinct  ''Roll_No,a.Stud_Name,''Reg_No,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no" + columnname1 + " " + refer + " from applyn a,degree d,Department dt,Course C where  d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "') and IsConfirm='1' and  a.Current_Semester in('" + sem_tagvalue + "')  " + FromdateApplyn + "   order by a.Stud_Name  ";

                        //Cmd By Saranyadevi29.12.2018
                        //query = "select distinct  ''Roll_No,a.Stud_Name,''Reg_No,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no" + columnname1 + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent from applyn a,degree d,Department dt,Course C where  d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "') and IsConfirm='1' and  a.Current_Semester in('" + sem_tagvalue + "')  " + FromdateApplyn + "   order by a.Stud_Name  ";

                        query = "select distinct  ''Roll_No,a.Stud_Name,''Reg_No,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no" + columnname1 + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else ((select refer_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else ((select refer_agent_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end as refer_agent from applyn a,degree d,Department dt,Course C where  d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "') and IsConfirm='1' and  a.Current_Semester in('" + sem_tagvalue + "')  " + FromdateApplyn + "   order by a.Stud_Name  ";
                    }
                }
                else if (val == 7 || (ddl_status.SelectedItem.Value == "2" && val == 0))
                {
                    //query = "select distinct  ''Roll_No,a.Stud_Name,''Reg_No,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no" + columnname1 + " from degree d,Department dt,Course C ,applyn a left join Registration r on a.app_no=r.App_No  where r.App_No is null  and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='1' and  a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "') and  a.Current_Semester in('" + sem_tagvalue + "')  " + sectionvalue + " " + FromdateApplyn + " order by a.Stud_Name ";

                    //Cmd By Saranyadevi19.12.2018
                    //query = "select distinct  ''Roll_No,a.Stud_Name,''Reg_No,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no" + columnname1 + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent from degree d,Department dt,Course C ,applyn a left join Registration r on a.app_no=r.App_No  where r.App_No is null  and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='1' and  a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "') and  a.Current_Semester in('" + sem_tagvalue + "')  " + sectionvalue + " " + FromdateApplyn + " order by a.Stud_Name ";


                    query = "select distinct  ''Roll_No,a.Stud_Name,''Reg_No,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no" + columnname1 + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then  ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else ((select refer_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else ((select refer_agent_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end as refer_agent from degree d,Department dt,Course C ,applyn a left join Registration r on a.app_no=r.App_No  where r.App_No is null  and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='1' and  a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "') and  a.Current_Semester in('" + sem_tagvalue + "')  " + sectionvalue + " " + FromdateApplyn + " order by a.Stud_Name ";
                }
                else if (val == 8 || (ddl_status.SelectedItem.Value == "3" && val == 0))
                {
                    //query = "select distinct   ''Roll_No,a.Stud_Name,''Reg_No,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no" + columnname1 + " from applyn a,degree d,Department dt,Course C where  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='2' and   a.degree_code in('" + dept_tagvalue + "') and a.degree_code=d.Degree_Code and a.Batch_Year in('" + Batch_tagvalue + "') and  a.Current_Semester in('" + sem_tagvalue + "') " + FromdateApplyn + " order by a.Stud_Name  ";

                    //Cmd By Saranyadevi29.12.2018
                    //query = "select distinct   ''Roll_No,a.Stud_Name,''Reg_No,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no" + columnname1 + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent from applyn a,degree d,Department dt,Course C where  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='2' and   a.degree_code in('" + dept_tagvalue + "') and a.degree_code=d.Degree_Code and a.Batch_Year in('" + Batch_tagvalue + "') and  a.Current_Semester in('" + sem_tagvalue + "') " + FromdateApplyn + " order by a.Stud_Name  ";

                    query = "select distinct   ''Roll_No,a.Stud_Name,''Reg_No,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no" + columnname1 + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then  ((select textval from textvaltable where convert(varchar,TextCode)= refer_name))  else ((select refer_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else ((select refer_agent_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end as refer_agent from applyn a,degree d,Department dt,Course C where  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='2' and   a.degree_code in('" + dept_tagvalue + "') and a.degree_code=d.Degree_Code and a.Batch_Year in('" + Batch_tagvalue + "') and  a.Current_Semester in('" + sem_tagvalue + "') " + FromdateApplyn + " order by a.Stud_Name  ";
                }
                else if (val == 9 || (ddl_status.SelectedItem.Value == "4" && val == 0))
                {
                    columnname1 = columnname1.Replace("a.stud_type", "r.stud_type");
                    columnname1 = columnname1.Replace("roll_admit", "roll_admit");
                    columnname1 = columnname1.Replace("a.Current_Semester", "r.Current_Semester");//Rajkumar on 9-6-2018
                    columnname1 = columnname1.Replace("''Sections", "sections");
                    //query = "select  distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname1 + " from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='1' and CC='0' and DelFlag='0' and Exam_Flag<>'DEBAR' and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " " + FromdateReg + " " + orderStr + " ";//columnname 30.07.16
                    // order by r.Roll_No,r.Stud_Name,r.Reg_No

                    //Cmd By Saranyadevi29.12.2018
                    //query = "select  distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname1 + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='1' and CC='0' and DelFlag='0' and Exam_Flag<>'DEBAR' and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " " + FromdateReg + " " + orderStr + " ";


                    query = "select  distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname1 + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name))  else ((select refer_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent))  else ((select refer_agent_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end as refer_agent from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='1' and CC='0' and DelFlag='0' and Exam_Flag<>'DEBAR' and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " " + FromdateReg + " " + orderStr + " ";
                }
                else if (val == 10 || (ddl_status.SelectedItem.Value == "6" && val == 0))
                {
                    //query = "select  distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + "  from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'  and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " and r.Exam_Flag='DEBAR' " + FromdateReg + " " + orderStr + " ";// order by r.Roll_No,r.Stud_Name,r.Reg_No

                    //Cmd By Saranyadevi29.12.2018
                    //query = "select  distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent  from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'  and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " and r.Exam_Flag='DEBAR' " + FromdateReg + " " + orderStr + " ";


                    query = "select  distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else ((select refer_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else ((select refer_agent_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end as refer_agent  from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'  and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " and r.Exam_Flag='DEBAR' " + FromdateReg + " " + orderStr + " ";
                }
                else if (val == 11 || (ddl_status.SelectedItem.Value == "5" && val == 0))
                {
                    //query = "select  distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + " from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'   and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " and r.DelFlag<>'0' " + FromdateReg + " " + orderStr + " ";//  order by r.Roll_No,r.Stud_Name,r.Reg_No
                    //Cmd By Saranyadevi29.12.2018
                    //query = "select  distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent,CONVERT(varchar(10), Discontinue_Date,103)Discontinue_Date ,Reason  from Registration r,applyn a,degree d,Department dt,Course C,discontinue del where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'   and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " and r.DelFlag<>'0' and del.app_no=r.app_no " + FromdateReg + " " + orderStr + " ";

                    query = "select  distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else ((select refer_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else ((select refer_agent_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end as refer_agent,CONVERT(varchar(10), Discontinue_Date,103)Discontinue_Date ,Reason  from Registration r,applyn a,degree d,Department dt,Course C,discontinue del where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'   and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " and r.DelFlag<>'0' and del.app_no=r.app_no " + FromdateReg + " " + orderStr + " ";
                }
                else if (val == 12 || (ddl_status.SelectedItem.Value == "7" && val == 0))
                {
                    //query = "select  distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + " from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='1'  and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " and r.cc=1 " + FromdateReg + " " + orderStr + " ";// order by r.Roll_No,r.Stud_Name,r.Reg_No

                    //Cmd By Saranyadevi
                    //query = "select  distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='1'  and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " and r.cc=1 " + FromdateReg + " " + orderStr + " ";


                    query = "select  distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else ((select refer_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent))  else ((select refer_agent_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end as refer_agent from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='1'  and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " and r.cc=1 " + FromdateReg + " " + orderStr + " ";

                }
                else if (val == 14 || (ddl_status.SelectedItem.Value == "8" && val == 0))
                {
                    //query = "select  distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + " from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'   and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " and r.DelFlag<>'0' and ProlongAbsend<>'0' " + FromdateReg + " " + orderStr + " ";
                    //query = "select  distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'   and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " and r.DelFlag<>'0' and ProlongAbsend<>'0' " + FromdateReg + " " + orderStr + " ";


                    query = "select  distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then  ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else ((select refer_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else ((select refer_agent_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end as refer_agent from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'   and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " and r.DelFlag<>'0' and ProlongAbsend<>'0' " + FromdateReg + " " + orderStr + " ";
                }


                else if (val == 13)
                {
                    string leftwaiting = string.Empty;
                    if (ddl_status.SelectedIndex < 2)
                    {
                        if (ddl_status.SelectedIndex == 1)
                        {
                            leftwaiting = " and  admission_status='1'";
                            //query = "select distinct  ''Roll_No,a.Stud_Name,''Reg_No,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no" + columnname1 + " from Stud_prev_details s, degree d,Department dt,Course C ,applyn a  left join Registration r on a.app_no=r.App_No  where r.App_No is null and a.app_no =s.app_no  and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and isconfirm ='1'  and a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "') and  a.Current_Semester in('" + sem_tagvalue + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + sectionvalue + " " + queryadd + " " + leftwaiting + "  " + addqur + " " + FromdateApplyn + " " + BoardFilter + " " + StateFilter + " ";
                            if (chk_typename.Checked == true || chk_typesizename.Checked == true)
                            {
                                //query = "select distinct  ''Roll_No,a.Stud_Name,''Reg_No,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no" + columnname1 + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent from Stud_prev_details s, degree d,Department dt,Course C ,applyn a,St_personalInfod st left join Registration r on a.app_no=r.App_No  where r.App_No is null and a.app_no =s.app_no and st.appno=r.app_no and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and isconfirm ='1'  and a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "') and  a.Current_Semester in('" + sem_tagvalue + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + sectionvalue + " " + queryadd + " " + leftwaiting + "  " + addqur + " " + FromdateApplyn + " " + BoardFilter + " " + StateFilter + " ";

                                query = "select distinct  ''Roll_No,a.Stud_Name,''Reg_No,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no" + columnname1 + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then  ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else ((select refer_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else ((select refer_agent_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end as refer_agent from Stud_prev_details s, degree d,Department dt,Course C ,applyn a,St_personalInfod st left join Registration r on a.app_no=r.App_No  where r.App_No is null and a.app_no =s.app_no and st.appno=r.app_no and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and isconfirm ='1'  and a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "') and  a.Current_Semester in('" + sem_tagvalue + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + sectionvalue + " " + queryadd + " " + leftwaiting + "  " + addqur + " " + FromdateApplyn + " " + BoardFilter + " " + StateFilter + " ";
                            }
                            else
                            {
                                //query = "select distinct  ''Roll_No,a.Stud_Name,''Reg_No,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no" + columnname1 + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent from Stud_prev_details s, degree d,Department dt,Course C ,applyn a  left join Registration r on a.app_no=r.App_No  where r.App_No is null and a.app_no =s.app_no  and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and isconfirm ='1'  and a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "') and  a.Current_Semester in('" + sem_tagvalue + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + sectionvalue + " " + queryadd + " " + leftwaiting + "  " + addqur + " " + FromdateApplyn + " " + BoardFilter + " " + StateFilter + " ";

                                query = "select distinct  ''Roll_No,a.Stud_Name,''Reg_No,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no" + columnname1 + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else ((select refer_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else ((select refer_agent_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end as refer_agent from Stud_prev_details s, degree d,Department dt,Course C ,applyn a  left join Registration r on a.app_no=r.App_No  where r.App_No is null and a.app_no =s.app_no  and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and isconfirm ='1'  and a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "') and  a.Current_Semester in('" + sem_tagvalue + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + sectionvalue + " " + queryadd + " " + leftwaiting + "  " + addqur + " " + FromdateApplyn + " " + BoardFilter + " " + StateFilter + " ";
                            }
                        }
                        else
                        {
                            if (ddl_status.SelectedIndex == 2)
                            {
                                leftwaiting = " and  admission_status='2'";
                            }
                            //query = "select distinct  ''Roll_No,a.Stud_Name,''Reg_No,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no" + columnname1 + " from applyn a,Stud_prev_details s, degree d,Department dt,Course C where a.app_no =s.app_no  and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and isconfirm ='1'  and a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "') and  a.Current_Semester in('" + sem_tagvalue + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + sectionvalue + " " + queryadd + " " + leftwaiting + " " + addqur + " " + FromdateApplyn + "  " + BoardFilter + " " + StateFilter + "  ";
                            if (chk_typename.Checked == true || chk_typesizename.Checked == true)
                            {
                                //query = "select distinct  ''Roll_No,a.Stud_Name,''Reg_No,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no" + columnname1 + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent from applyn a,Stud_prev_details s, degree d,Department dt,Course C,St_personalInfod st where st.appno=a.app_no and a.app_no =s.app_no  and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and isconfirm ='1'  and a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "') and  a.Current_Semester in('" + sem_tagvalue + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + queryadd + " " + leftwaiting + " " + addqur + " " + FromdateApplyn + "  " + BoardFilter + " " + StateFilter + "  ";// " + sectionvalue + "




                                query = "select distinct  ''Roll_No,a.Stud_Name,''Reg_No,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no" + columnname1 + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then  ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else ((select refer_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else ((select refer_agent_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end as refer_agent from applyn a,Stud_prev_details s, degree d,Department dt,Course C,St_personalInfod st where st.appno=a.app_no and a.app_no =s.app_no  and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and isconfirm ='1'  and a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "') and  a.Current_Semester in('" + sem_tagvalue + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + queryadd + " " + leftwaiting + " " + addqur + " " + FromdateApplyn + "  " + BoardFilter + " " + StateFilter + "  ";// " + sectionvalue + "
                            }
                            else
                            {
                                //query = "select distinct  ''Roll_No,a.Stud_Name,''Reg_No,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no" + columnname1 + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent from applyn a,Stud_prev_details s, degree d,Department dt,Course C where a.app_no =s.app_no  and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and isconfirm ='1'  and a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "') and  a.Current_Semester in('" + sem_tagvalue + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + sectionvalue + " " + queryadd + " " + leftwaiting + " " + addqur + " " + FromdateApplyn + "  " + BoardFilter + " " + StateFilter + "  ";

                                query = "select distinct  ''Roll_No,a.Stud_Name,''Reg_No,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no" + columnname1 + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else ((select refer_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else ((select refer_agent_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end as refer_agent from applyn a,Stud_prev_details s, degree d,Department dt,Course C where a.app_no =s.app_no  and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and isconfirm ='1'  and a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "') and  a.Current_Semester in('" + sem_tagvalue + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + sectionvalue + " " + queryadd + " " + leftwaiting + " " + addqur + " " + FromdateApplyn + "  " + BoardFilter + " " + StateFilter + "  ";
                            }
                        }
                    }
                    else
                    {
                        if (ddl_status.SelectedIndex == 3)
                        {
                            leftwaiting = " and admission_status='1' and DelFlag='0' and Exam_Flag='OK' and CC='False' ";
                        }
                        else if (ddl_status.SelectedIndex == 4)
                        {
                            leftwaiting = " and DelFlag<>'0'";
                        }
                        else if (ddl_status.SelectedIndex == 5)
                        {
                            leftwaiting = " and Exam_Flag='DEBAR' ";
                        }
                        else if (ddl_status.SelectedIndex == 6)
                        {
                            leftwaiting = " and CC='True' ";
                        }
                        //query = "  select   distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + " from Registration r,degree d,Stud_prev_details s, applyn a ,Department dt,Course C where a.app_no =s.app_no and r.App_No =a.app_no and isconfirm ='1'  and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') and r.college_code='" + ddlcollege.SelectedItem.Value + "'  and d.Degree_Code =r.degree_code  and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  " + sectionvalue + " " + queryadd + " " + leftwaiting + "  " + addqur + " " + FromdateReg + "  " + BoardFilter + " " + StateFilter + "   " + orderStr + " ";
                        if (chk_typename.Checked == true || chk_typesizename.Checked == true)
                        {
                            //query = "  select   distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent from Registration r,degree d,Stud_prev_details s, applyn a ,Department dt,Course C,St_personalInfod st where st.appno=r.app_no and a.app_no =s.app_no and r.App_No =a.app_no and isconfirm ='1'  and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') and r.college_code='" + ddlcollege.SelectedItem.Value + "'  and d.Degree_Code =r.degree_code  and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  " + sectionvalue + " " + queryadd + " " + leftwaiting + "  " + addqur + " " + FromdateReg + "  " + BoardFilter + " " + StateFilter + "   " + orderStr + " ";


                            query = "  select   distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else ((select refer_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else ((select refer_agent_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end as refer_agent from Registration r,degree d,Stud_prev_details s, applyn a ,Department dt,Course C,St_personalInfod st where st.appno=r.app_no and a.app_no =s.app_no and r.App_No =a.app_no and isconfirm ='1'  and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') and r.college_code='" + ddlcollege.SelectedItem.Value + "'  and d.Degree_Code =r.degree_code  and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  " + sectionvalue + " " + queryadd + " " + leftwaiting + "  " + addqur + " " + FromdateReg + "  " + BoardFilter + " " + StateFilter + "   " + orderStr + " ";
                        }
                        else
                        {

                            //query = "  select   distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent from Registration r,degree d,Stud_prev_details s, applyn a ,Department dt,Course C where a.app_no =s.app_no and r.App_No =a.app_no and isconfirm ='1'  and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') and r.college_code='" + ddlcollege.SelectedItem.Value + "'  and d.Degree_Code =r.degree_code  and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  " + sectionvalue + " " + queryadd + " " + leftwaiting + "  " + addqur + " " + FromdateReg + "  " + BoardFilter + " " + StateFilter + "   " + orderStr + " ";

                            query = "  select   distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else ((select refer_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else ((select refer_agent_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end as refer_agent,s.course_code,(select textval from textvaltable where convert(varchar,TextCode)= course_code) as board from Registration r,degree d,Stud_prev_details s, applyn a ,Department dt,Course C where a.app_no =s.app_no and r.App_No =a.app_no and isconfirm ='1'  and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') and r.college_code='" + ddlcollege.SelectedItem.Value + "'  and d.Degree_Code =r.degree_code  and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  " + sectionvalue + " " + queryadd + " " + leftwaiting + "  " + addqur + " " + FromdateReg + "  " + BoardFilter + " " + StateFilter + "   " + orderStr + " ";//change
                        }
                    }
                }//delsi

                //====Added by Saranya on 05/01/2018 for Transport and Hostel canceled Student====//
                else if (val == 15 || (ddl_status.SelectedItem.Value == "9" && val == 0))
                {
                    //query = "select d.app_no,r.reg_no,r.Stud_Name,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,d.boarding,d.bus_routeid,d.vehid from Registration r,Discontinue d,applyn a, degree de,Department dt,Course C where de.Degree_Code =r.degree_code and de.Dept_Code =dt.Dept_Code and c.Course_Id =de.Course_Id and r.App_No=d.app_no and r.degree_code in('" + dept_tagvalue + "')and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "' " + FromdateTransHostel + " and d.Catogery=4 and a.app_no =r.App_No group by d.app_no,r.reg_no,r.Stud_Name,r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,d.boarding,d.bus_routeid,d.vehid ";
                    //query = "select d.app_no,r.reg_no,r.Stud_Name,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,d.boarding,d.bus_routeid,d.vehid,case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent from Registration r,Discontinue d,applyn a, degree de,Department dt,Course C where de.Degree_Code =r.degree_code and de.Dept_Code =dt.Dept_Code and c.Course_Id =de.Course_Id and r.App_No=d.app_no and r.degree_code in('" + dept_tagvalue + "')and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "' " + FromdateTransHostel + " and d.Catogery=4 and a.app_no =r.App_No group by d.app_no,r.reg_no,r.Stud_Name,r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,d.boarding,d.bus_routeid,d.vehid ";

                    query = "select d.app_no,r.reg_no,r.roll_no,sex,r.Stud_Name,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,d.boarding,d.bus_routeid,d.vehid,case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else ((select refer_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else ((select refer_agent_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end as refer_agent from Registration r,Discontinue d,applyn a, degree de,Department dt,Course C where de.Degree_Code =r.degree_code and de.Dept_Code =dt.Dept_Code and c.Course_Id =de.Course_Id and r.App_No=d.app_no and r.degree_code in('" + dept_tagvalue + "')and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "' " + FromdateTransHostel + " and d.Catogery=4 and a.app_no =r.App_No group by d.app_no,r.reg_no,r.Stud_Name,r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,d.boarding,d.bus_routeid,d.vehid,direct_refer,refer_stcode,refer_agent,refer_name,r.roll_no,sex ";

                }
                else if (val == 16 || (ddl_status.SelectedItem.Value == "10" && val == 0))
                {
                    //query = "select d.app_no,r.reg_no,r.Stud_Name,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,d.buildingname,d.hostelname,d.roomname from Registration r,Discontinue d,applyn a, degree de,Department dt,Course C where de.Degree_Code =r.degree_code and de.Dept_Code =dt.Dept_Code and c.Course_Id =de.Course_Id and r.App_No=d.app_no and r.degree_code in('" + dept_tagvalue + "')and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "' " + FromdateTransHostel + " and d.Catogery=3 and a.app_no =r.App_No group by d.app_no,r.reg_no,r.Stud_Name,r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,d.buildingname,d.hostelname,d.roomname ";
                    //query = "select d.app_no,r.reg_no,r.Stud_Name,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,d.buildingname,d.hostelname,d.roomname,case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent from Registration r,Discontinue d,applyn a, degree de,Department dt,Course C where de.Degree_Code =r.degree_code and de.Dept_Code =dt.Dept_Code and c.Course_Id =de.Course_Id and r.App_No=d.app_no and r.degree_code in('" + dept_tagvalue + "')and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "' " + FromdateTransHostel + " and d.Catogery=3 and a.app_no =r.App_No group by d.app_no,r.reg_no,r.Stud_Name,r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,d.buildingname,d.hostelname,d.roomname ";


                    query = "select d.app_no,r.reg_no,r.Stud_Name,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,d.buildingname,d.hostelname,d.roomname,case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else ((select refer_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else ((select refer_agent_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end as refer_agent from Registration r,Discontinue d,applyn a, degree de,Department dt,Course C where de.Degree_Code =r.degree_code and de.Dept_Code =dt.Dept_Code and c.Course_Id =de.Course_Id and r.App_No=d.app_no and r.degree_code in('" + dept_tagvalue + "')and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "' " + FromdateTransHostel + " and d.Catogery=3 and a.app_no =r.App_No group by d.app_no,r.reg_no,r.Stud_Name,r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,d.buildingname,d.hostelname,d.roomname ";
                }
                //==============================================================================//
                //barath 24.01.18 Enquiry
                else if (val == 17)
                {
                    columnname1 = columnname1.Replace("CONVERT(varchar(10), r.Adm_Date,103)Adm_Date", "''Adm_Date");
                    //query = "select distinct  ''Roll_No,a.Stud_Name,''Reg_No,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no" + columnname1 + " from applyn a,degree d,Department dt,Course C where  d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "') and isnull(IsEnquiry,0)='1' and  a.Current_Semester in('" + sem_tagvalue + "')  " + FromdateApplyn + "   order by a.Stud_Name  ";
                    //query = "select distinct  ''Roll_No,a.Stud_Name,''Reg_No,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no" + columnname1 + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent from applyn a,degree d,Department dt,Course C where  d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "') and isnull(IsEnquiry,0)='1' and  a.Current_Semester in('" + sem_tagvalue + "')  " + FromdateApplyn + "   order by a.Stud_Name  ";

                    query = "select distinct  ''Roll_No,a.Stud_Name,''Reg_No,a.Admission_Status,''Exam_Flag,''CC,''DelFlag,a.app_no" + columnname1 + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else ((select refer_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else ((select refer_agent_name from Student_Refer_Details where convert(varchar,IdNo)= refer_name)) end as refer_agent from applyn a,degree d,Department dt,Course C where  d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "') and isnull(IsEnquiry,0)='1' and  a.Current_Semester in('" + sem_tagvalue + "')  " + FromdateApplyn + "   order by a.Stud_Name  ";
                }


                query = query + "sELECT LastTCNo,convert(varchar(10),LastTCDate,103)LastTCDate,instaddress,(Select textval FROM textvaltable T WHERE Xmedium = t.TextCode) Xmedium,(Select textval FROM textvaltable T WHERE medium = CONVERT(nvarchar(20),t.TextCode)) medium,percentage,securedmark,totalmark,passyear,passmonth,case when Vocational_stream='0' then 'No' else 'Yes' end as Vocational_stream,markPriority,Cut_Of_Mark ,a.App_No,us.textval as uni_state,type_semester,university_code,ISNULL(pt.TExtVal,'') Part2Language,(Select textval FROM textvaltable T WHERE Part1Language = t.TextCode) Part1Language,ISNULL(C.TExtVal,'') Course,ISNULL(U.TextVal,'') University,ISNULL(Institute_Name,'') Institute_Name,ISNULL(PassMonth,'') PassMonth,ISNULL(PassYear,'') PassYear FROM Stud_prev_details P INNER JOIN applyn A ON P.app_no = A.app_no LEFT JOIN TextValTable C ON C.TextCode = P.course_code  LEFT JOIN TextValTable U ON U.TextCode = P.university_code LEFT JOIN TextValTable pa ON pa.TextCode = P.Part1Language LEFT JOIN TextValTable pt ON pt.TextCode = P.Part2Language LEFT JOIN TextValTable us ON CONVERT(nvarchar(20),us.TextCode) = P.uni_state Where p.app_no = a.app_no  and a.Batch_Year in('" + Batch_tagvalue + "') and IsConfirm='1' and  a.Current_Semester in('" + sem_tagvalue + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'" + boards + "" + states + " and isnull(markPriority,1)=1";//and a.degree_code in('" + dept_tagvalue + "')"; us.TextCode varchar change 11.09.2018
                query = query + "select * from StudCertDetails_New s,applyn a where a.App_No=s.App_No  and a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "') and  a.Current_Semester in('" + sem_tagvalue + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  ";
                query = query + "select * from studbankdet s,applyn a where a.App_No=s.App_No   and a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "')  and  a.Current_Semester in('" + sem_tagvalue + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  ";
                query = query + "select * from stud_relation s,applyn a where a.App_No=s.application_no   and a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "')  and  a.Current_Semester in('" + sem_tagvalue + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  ";
                query = query + " select HostelName,APP_No from HT_HostelRegistration hr,HM_HostelMaster hm where hr.HostelMasterFK=hm.HostelMasterPK ";
                query = query + "  select COUNT (subject_no)as noofarrear,m.roll_no,a.app_no from mark_entry m , applyn a,Registration r  where r.App_No=a.app_no and  r.Roll_No=m.roll_no and subject_no not in(select s.subject_no from subject s,mark_entry m where m.subject_no=s.subject_no and m.result='pass') group by m.roll_no,a.app_no";
                query = query + " select roll_no,acronym,subject_name from subjectchooser c,subject s ,sub_sem u where c.subject_no = s.subject_no and s.subType_no = u.subType_no and subject_type = 'Foundation Course - I' and roll_no in (select roll_no from Registration r where  r.batch_year in( '" + Batch_tagvalue + "') and r.degree_code in ('" + dept_tagvalue + "') and r.Current_Semester in('" + sem_tagvalue + "') and r.college_code='" + ddlcollege.SelectedItem.Value + "' and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' )  and s.subject_name not like 'Tamil%'";//and semester = '" + ddlSemYr.SelectedItem.Text.ToString() + "'
                query = query + " select r.app_no,r.Current_Semester,r.degree_code,r.stud_name,R.Batch_year,course_name+'-'+dept_name degree,isnull(r.Sections,'') as Sections,(select isnull(Building_acronym,'') from HT_HostelRegistration s,Building_Master b where s.BuildingFK = b.Code and s.APP_No = r.App_No and ISNULL(IsVacated,0)=0 and ISNULL(IsDiscontinued,0)=0 and ISNULL(IsSuspend,0)=0) as hall,(select isnull(building_description ,'') from HT_HostelRegistration s,Building_Master b where s.BuildingFK = b.Code and s.APP_No = r.App_No and ISNULL(IsVacated,0)=0 and ISNULL(IsDiscontinued,0)=0 and ISNULL(IsSuspend,0)=0) as hallname,(select textval from textvaltable t where t.TextCode = a.religion) religion,(select textval from textvaltable t where t.TextCode = a.community ) community,(select textval from textvaltable t where t.TextCode = a.caste) caste, r.roll_no,a.app_no,a.religion as religioncode,a.community as communitycode,a.sex,1 TotalStrength from Registration r,applyn a,Degree g,course c,department d where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code  and g.college_code = d.college_code  and r.batch_year in( '" + Batch_tagvalue + "') and r.degree_code in ('" + dept_tagvalue + "') and r.college_code='" + ddlcollege.SelectedItem.Value + "' and r.Current_Semester in('" + sem_tagvalue + "') and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' ";
                query = query + "select r.app_no,case when StudMessType=0 then 'Veg' when StudMessType=1 then 'Non Veg' else '' end StudMessType from HT_HostelRegistration ht,Registration r where  ht.APP_No = r.App_No and r.college_code=ht.CollegeCode and ISNULL(IsVacated,0)=0 and ISNULL(IsDiscontinued,0)=0 and ISNULL(IsSuspend,0)=0  and r.batch_year in( '" + Batch_tagvalue + "') and r.degree_code in ('" + dept_tagvalue + "') and r.college_code='" + ddlcollege.SelectedItem.Value + "' and r.Current_Semester in('" + sem_tagvalue + "') and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar'  ";

                query = query + "select r.app_no,case when StudMessType=0 then 'Veg' when StudMessType=1 then 'Non Veg' else '' end StudMessType from DayScholourStaffAdd ds,Registration r where  ds.roll_no = r.roll_no and r.college_code=ds.College_Code and r.batch_year in( '" + Batch_tagvalue + "') and r.degree_code in ('" + dept_tagvalue + "') and r.college_code='" + ddlcollege.SelectedItem.Value + "' and r.Current_Semester in('" + sem_tagvalue + "') and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar'  ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");


                string latemode = "1";
                string noarrear = string.Empty;

                if (query == "")
                {
                    Fpspread2.Sheets[0].Visible = false;
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Select All List ";
                    div_report.Visible = false;
                    lblvalidation1.Text = string.Empty;
                    return;
                }
                else
                {
                    if (query != "")
                    {
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
                        {
                            Fpspread2.Sheets[0].Visible = false;
                            Fpspread2.Visible = false;
                            lblerror.Visible = true;
                            lblerror.Text = "No Records Found";
                            div_report.Visible = false;
                            lbl_headernamespd2.Visible = false;
                            lblvalidation1.Text = string.Empty;
                            return;
                        }
                        else
                        {
                            lblerror.Visible = false;
                            lbl_err_stud.Visible = false;
                            btn_viewsprd2.Visible = true;
                            img_settingpdf.Visible = true;
                            lnk_admisstionformbtn.Visible = true;
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                div_report.Visible = true;
                                lbl_headernamespd2.Visible = true;
                                if (name == "All")
                                {
                                    lbl_headernamespd2.Text = name;
                                }
                                else
                                {
                                    lbl_headernamespd2.Text = headertype + "-" + name;
                                }
                                Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                                Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                                Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Column.Visible = false;
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "AppNo";
                                // Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                                // string
                                int cc = 2;
                                int j = 0;
                                //loadlcolumns();
                                DataSet dss = new DataSet();
                                string linkname1 = Convert.ToString(ddl_colord.SelectedItem.Text);
                                string selcol1 = "select LinkValue from New_InsSettings where LinkName='" + linkname1 + "'  and college_code='" + ddlcollege.SelectedItem.Value + "' and user_code='" + usercode + "' ";
                                dss.Clear();
                                dss = d2.select_method_wo_parameter(selcol1, "Text");
                                if (dss.Tables.Count > 0)
                                {
                                    if (dss.Tables[0].Rows.Count > 0)
                                    {
                                        for (int c = 0; c < dss.Tables[0].Rows.Count; c++)
                                        {
                                            string value = Convert.ToString(dss.Tables[0].Rows[c]["LinkValue"]);
                                            if (value != "")
                                            {
                                                string[] valuesplit = value.Split(',');
                                                if (valuesplit.Length > 0)
                                                {
                                                    for (int k = 0; k < valuesplit.Length; k++)
                                                    {
                                                        cc++;
                                                        colval = Convert.ToString(valuesplit[k]);
                                                        loadtext();
                                                        Fpspread2.Sheets[0].ColumnCount = cc + 1;
                                                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Text = loadval;
                                                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Tag = printval;
                                                        if (Convert.ToInt32(colval) > 139)
                                                        {
                                                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Note = "Certificate";
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        imgdiv2.Visible = true;
                                        lbl_alert.Text = "No Records Found";
                                        Fpspread2.Visible = false;
                                        div_report.Visible = false;
                                        img_settingpdf.Visible = false;
                                        btn_viewsprd2.Visible = false;
                                        lnk_admisstionformbtn.Visible = false;
                                        lbl_headernamespd2.Visible = false;
                                        lblvalidation1.Text = string.Empty;
                                        return;
                                    }
                                }
                                else
                                {
                                    imgdiv2.Visible = true;
                                    lbl_alert.Text = "Set Column Order";
                                    Fpspread2.Visible = false;
                                    div_report.Visible = false;
                                    img_settingpdf.Visible = false;
                                    btn_viewsprd2.Visible = false;
                                    lnk_admisstionformbtn.Visible = false;
                                    lbl_headernamespd2.Visible = false;
                                    lblvalidation1.Text = string.Empty;
                                    return;
                                }
                                string txt1 = string.Empty;
                                string txt2 = string.Empty;
                                string txt3 = string.Empty;
                                string txt4 = string.Empty;
                                string txt5 = string.Empty;
                                FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                                cb.AutoPostBack = true;
                                FarPoint.Web.Spread.CheckBoxCellType cball = new FarPoint.Web.Spread.CheckBoxCellType();
                                cball.AutoPostBack = true;
                                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    if (i == 0)
                                    {
                                        Fpspread2.Sheets[0].RowCount++;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = cball;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    Fpspread2.Sheets[0].RowCount++;
                                    count++;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Locked = true;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = cb;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["app_no"]); ;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["app_no"]);
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["app_no"]);
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Locked = true;
                                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Locked = true;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                    if (val == 15 || val == 16)
                                    {
                                    }
                                    else
                                    {
                                        string admi_status = Convert.ToString(ds.Tables[0].Rows[i]["Admission_Status"]);
                                        string delflag = Convert.ToString(ds.Tables[0].Rows[i]["DelFlag"]);
                                        string examflg = Convert.ToString(ds.Tables[0].Rows[i]["Exam_Flag"]);
                                        string coursecomp = Convert.ToString(ds.Tables[0].Rows[i]["CC"]);
                                    }
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].BackColor = ColorTranslator.FromHtml("#F2C77D");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].BackColor = ColorTranslator.FromHtml("#F2C77D");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#F2C77D");
                                    cc = 2;
                                    string text = string.Empty;
                                    DataView dv = new DataView();
                                    DataView dv1 = new DataView();
                                    string linkname = Convert.ToString(ddl_colord.SelectedItem.Text);
                                    string columnvalue = string.Empty;
                                    DataSet dscol = new DataSet();


                                    string rollno = ds.Tables[0].Rows[i]["roll_no"].ToString();


                                    for (int k = 3; k < Fpspread2.Sheets[0].ColumnCount; k++)
                                    {
                                        cc++;
                                        string col = Convert.ToString(Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Tag);
                                        string cerificate = "0";
                                        //string cerificate = d2.GetFunction("select distinct MasterValue from CO_MasterValues where MasterCriteria='CertificateName' and MasterCode='" + col + "' ");
                                        if (col == "type_semester" || col == "Institute_name" || col == "isgrade" || col == "Part1Language" || col == "Part2Language" || col == "university_code" || col == "instaddress" || col == "Xmedium" || col == "medium" || col == "percentage" || col == "securedmark" || col == "totalmark" || col == "passyear" || col == "passmonth" || col == "Vocational_stream" || col == "markPriority" || col == "Cut_Of_Mark" || col == "LastTCNo" || col == "LastTCDate" || col == "uni_state" || col == "University")
                                        {
                                            if (ds.Tables[1].Rows.Count > 0)
                                            {
                                                if (ds.Tables[1].Rows.Count > 0)
                                                {
                                                    if (col.ToLower() == "uni_state")
                                                    {
                                                        ds.Tables[1].DefaultView.RowFilter = "app_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_No"]) + "' ";
                                                        dv = ds.Tables[1].DefaultView;
                                                        DataTable temp = new DataTable();
                                                        temp = dv.ToTable();
                                                        if (dv.Count > 0)
                                                        {
                                                            temp.DefaultView.RowFilter = " uni_state is not null ";
                                                            dv1 = temp.DefaultView;
                                                            if (dv1.Count > 0)
                                                                text = Convert.ToString(dv1[0]["uni_state"]);
                                                            else
                                                                text = string.Empty;
                                                        }
                                                        else
                                                            text = string.Empty;
                                                    }

                                                    else if (col == "University")
                                                    {
                                                        ds.Tables[1].DefaultView.RowFilter = "app_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_No"]) + "' ";
                                                        dv = ds.Tables[1].DefaultView;
                                                        DataTable temp = new DataTable();
                                                        temp = dv.ToTable();
                                                        if (dv.Count > 0)
                                                        {
                                                            temp.DefaultView.RowFilter = " university_code is not null ";
                                                            dv1 = temp.DefaultView;
                                                            if (dv1.Count > 0)
                                                                text = Convert.ToString(dv1[0]["University"]);
                                                            else
                                                                text = string.Empty;
                                                        }
                                                        else
                                                            text = string.Empty;
                                                    }

                                                    else
                                                    {
                                                        ds.Tables[1].DefaultView.RowFilter = "app_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_No"]) + "'";
                                                        dv = ds.Tables[1].DefaultView;
                                                        if (dv.Count > 0)
                                                            text = Convert.ToString(dv[0][col]);
                                                        else
                                                            text = string.Empty;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                text = string.Empty;
                                            }
                                        }
                                        else if (col.ToLower() == "subject_name")
                                        {
                                            ds.Tables[7].DefaultView.RowFilter = "roll_no='" + Convert.ToString(ds.Tables[0].Rows[i]["roll_no"]) + "' ";
                                            dv = ds.Tables[7].DefaultView;
                                            DataTable temp = new DataTable();
                                            temp = dv.ToTable();
                                            if (dv.Count > 0)
                                            {
                                                temp.DefaultView.RowFilter = " subject_name is not null ";
                                                dv1 = temp.DefaultView;
                                                if (dv1.Count > 0)
                                                    text = Convert.ToString(dv1[0]["subject_name"]);
                                                else
                                                    text = string.Empty;
                                            }
                                            else
                                                text = string.Empty;
                                        }
                                        else if (col.ToLower() == "acronym")
                                        {
                                            ds.Tables[7].DefaultView.RowFilter = "roll_no='" + Convert.ToString(ds.Tables[0].Rows[i]["roll_no"]) + "' ";
                                            dv = ds.Tables[7].DefaultView;
                                            DataTable temp = new DataTable();
                                            temp = dv.ToTable();
                                            if (dv.Count > 0)
                                            {
                                                temp.DefaultView.RowFilter = " acronym is not null ";
                                                dv1 = temp.DefaultView;
                                                if (dv1.Count > 0)
                                                    text = Convert.ToString(dv1[0]["acronym"]);
                                                else
                                                    text = string.Empty;
                                            }
                                            else
                                                text = string.Empty;
                                        }
                                        else if (col.ToLower() == "building_description")//abarna
                                        {
                                            ds.Tables[8].DefaultView.RowFilter = "app_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_No"]) + "' ";
                                            dv = ds.Tables[8].DefaultView;
                                            DataTable temp = new DataTable();
                                            temp = dv.ToTable();
                                            if (dv.Count > 0)
                                            {
                                                temp.DefaultView.RowFilter = " hallname is not null ";
                                                dv1 = temp.DefaultView;
                                                if (dv1.Count > 0)
                                                    text = Convert.ToString(dv1[0]["hallname"]);
                                                else
                                                    text = string.Empty;
                                            }
                                            else
                                                text = string.Empty;
                                        }
                                        else if (col.ToLower() == "studmesstype")//abarna
                                        {
                                            ds.Tables[9].DefaultView.RowFilter = "app_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_No"]) + "' ";
                                            dv = ds.Tables[9].DefaultView;
                                            DataTable temp = new DataTable();
                                            temp = dv.ToTable();
                                            if (dv.Count > 0)
                                            {
                                                temp.DefaultView.RowFilter = " StudMessType is not null ";
                                                dv1 = temp.DefaultView;
                                                if (dv1.Count > 0)
                                                    text = Convert.ToString(dv1[0]["StudMessType"]);

                                                else
                                                    text = string.Empty;
                                            }
                                            else
                                            {
                                                ds.Tables[10].DefaultView.RowFilter = "app_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_No"]) + "' ";
                                                dv = ds.Tables[10].DefaultView;
                                                temp = new DataTable();
                                                temp = dv.ToTable();
                                                if (dv.Count > 0)
                                                {
                                                    temp.DefaultView.RowFilter = " StudMessType is not null ";
                                                    dv1 = temp.DefaultView;
                                                    if (dv1.Count > 0)
                                                        text = Convert.ToString(dv1[0]["StudMessType"]);

                                                    else
                                                        text = string.Empty;
                                                }
                                                else
                                                    text = string.Empty;
                                            }

                                        }
                                        else if (col.ToLower() == "building_acronym")//abarna
                                        {
                                            ds.Tables[8].DefaultView.RowFilter = "app_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_No"]) + "' ";
                                            dv = ds.Tables[8].DefaultView;
                                            DataTable temp = new DataTable();
                                            temp = dv.ToTable();
                                            if (dv.Count > 0)
                                            {
                                                temp.DefaultView.RowFilter = " hall is not null ";
                                                dv1 = temp.DefaultView;
                                                if (dv1.Count > 0)
                                                    text = Convert.ToString(dv1[0]["hall"]);
                                                else
                                                    text = string.Empty;
                                            }
                                            else
                                                text = string.Empty;
                                        }
                                        else if (col.ToLower() == "discontinue_date")
                                        {
                                            ds.Tables[0].DefaultView.RowFilter = "roll_no='" + Convert.ToString(ds.Tables[0].Rows[i]["roll_no"]) + "' ";
                                            dv = ds.Tables[0].DefaultView;
                                            DataTable temp = new DataTable();
                                            temp = dv.ToTable();
                                            if (dv.Count > 0)
                                            {
                                                temp.DefaultView.RowFilter = " Discontinue_Date is not null ";
                                                dv1 = temp.DefaultView;
                                                if (dv1.Count > 0)
                                                    text = Convert.ToString(dv1[0]["Discontinue_Date"]);
                                                else
                                                    text = string.Empty;
                                            }
                                            else
                                                text = string.Empty;
                                        }
                                        else if (col.ToLower() == "reason")
                                        {
                                            ds.Tables[0].DefaultView.RowFilter = "roll_no='" + Convert.ToString(ds.Tables[0].Rows[i]["roll_no"]) + "' ";
                                            dv = ds.Tables[0].DefaultView;
                                            DataTable temp = new DataTable();
                                            temp = dv.ToTable();
                                            if (dv.Count > 0)
                                            {
                                                temp.DefaultView.RowFilter = " reason is not null ";
                                                dv1 = temp.DefaultView;
                                                if (dv1.Count > 0)
                                                    text = Convert.ToString(dv1[0]["reason"]);
                                                else
                                                    text = string.Empty;
                                            }
                                            else
                                                text = string.Empty;
                                        }
                                        else if (col.ToLower() == "entryusercode")
                                        {
                                            ds.Tables[0].DefaultView.RowFilter = "roll_no='" + Convert.ToString(ds.Tables[0].Rows[i]["roll_no"]) + "' ";
                                            dv = ds.Tables[0].DefaultView;
                                            DataTable temp = new DataTable();
                                            temp = dv.ToTable();
                                            if (ddl_status.SelectedIndex == 0)
                                            {
                                                if (dv.Count > 0)
                                                {
                                                    temp.DefaultView.RowFilter = " user_code is not null ";
                                                    dv1 = temp.DefaultView;
                                                    if (dv1.Count > 0)
                                                    {
                                                        text = Convert.ToString(dv1[0]["user_code"]);
                                                        text = d2.GetFunction("select user_id from usermaster where user_code='" + text + "'");
                                                    }
                                                    else
                                                    {
                                                        text = string.Empty;
                                                    }
                                                }
                                                else
                                                    text = string.Empty;
                                            }
                                            else
                                            {
                                                if (dv.Count > 0)
                                                {
                                                    temp.DefaultView.RowFilter = " entryusercode is not null ";
                                                    dv1 = temp.DefaultView;
                                                    if (dv1.Count > 0)
                                                    {
                                                        text = Convert.ToString(dv1[0]["entryusercode"]);
                                                        text = d2.GetFunction("select user_id from usermaster where user_code='" + text + "'");
                                                    }
                                                    else
                                                    {
                                                        text = string.Empty;
                                                    }
                                                }
                                                else
                                                    text = string.Empty;
                                            }
                                        }
                                        else if (col == "AccNo" || col == "DebitCardNo" || col == "IFSCCode" || col == "BankName" || col == "Branch")
                                        {
                                            if (ds.Tables[3].Rows.Count > 0)
                                            {
                                                ds.Tables[3].DefaultView.RowFilter = "app_no='" + Convert.ToString(ds.Tables[0].Rows[i]["app_no"]) + "'";
                                                dv1 = ds.Tables[3].DefaultView;
                                                if (dv1.Count > 0)
                                                {
                                                    text = Convert.ToString(dv1[0][col]);
                                                }
                                                else
                                                {
                                                    text = string.Empty;
                                                }
                                            }
                                            else
                                            {
                                                text = string.Empty;
                                            }
                                        }
                                        else if (col == "name_roll" || col == "relationship" || col == "isstaff")
                                        {
                                            if (ds.Tables[4].Rows.Count > 0)
                                            {
                                                ds.Tables[4].DefaultView.RowFilter = "application_no='" + Convert.ToString(ds.Tables[0].Rows[i]["app_no"]) + "'";
                                                dv1 = ds.Tables[4].DefaultView;
                                                if (dv1.Count > 0)
                                                {
                                                    if (col == "isstaff")
                                                    {
                                                        text = Convert.ToString(dv1[0][col]);
                                                        if (text == "0")
                                                        {
                                                            text = "Student";
                                                        }
                                                        else
                                                        {
                                                            text = "Staff";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        text = Convert.ToString(dv1[0][col]);
                                                    }
                                                }
                                                else
                                                {
                                                    text = string.Empty;
                                                }
                                            }
                                            else
                                            {
                                                text = string.Empty;
                                            }
                                        }
                                        else if (col == "HostelName")
                                        {
                                            if (ds.Tables[5].Rows.Count > 0)
                                            {
                                                ds.Tables[5].DefaultView.RowFilter = "APP_No='" + Convert.ToString(ds.Tables[0].Rows[i]["app_no"]) + "'";
                                                dv1 = ds.Tables[5].DefaultView;
                                                if (dv1.Count > 0)
                                                {
                                                    text = Convert.ToString(dv1[0]["HostelName"]);
                                                }
                                                else
                                                {
                                                    text = string.Empty;
                                                }
                                            }
                                            else
                                            {
                                                text = string.Empty;
                                            }
                                        }
                                        //**added by Mullai
                                        else if (col == "noofarrear")
                                        {

                                            string arrearcount = " select COUNT (distinct subject_no)as noofarrear from mark_entry where roll_no='" + rollno + "' and subject_no not in(select s.subject_no from subject s,mark_entry m where m.subject_no=s.subject_no and m.result='pass' and roll_no='" + rollno + "')";
                                            nofar.Clear();
                                            nofar = d2.select_method_wo_parameter(arrearcount, "text");

                                            if (nofar.Tables[0].Rows.Count > 0)
                                            {

                                                dv1 = nofar.Tables[0].DefaultView;

                                                if (dv1.Count > 0)
                                                {


                                                    text = Convert.ToString(dv1[0]["noofarrear"]);
                                                    noarrear = Convert.ToString(dv1[0]["noofarrear"]);
                                                }
                                                else
                                                {
                                                    text = string.Empty;
                                                    noarrear = string.Empty;
                                                }
                                                n_arrear = Convert.ToInt32(noarrear);


                                            }
                                        }

                                        else if (col == "CGPA")
                                        {
                                            if (n_arrear == 0)
                                            {
                                                text = d2.Calculete_CGPA(rollno, sem_tagvalue, dept_tagvalue, Batch_tagvalue, latemode, Convert.ToString(ddlcollege.SelectedValue).Trim());
                                            }
                                            else
                                            {
                                                text = "";
                                            }

                                        }
                                        else if (col == "referby")
                                        {
                                            string code = Convert.ToString(ds.Tables[0].Rows[i]["refer_stcode"]);//Added by saranya on 12/7/2018
                                            string refer = Convert.ToString(ds.Tables[0].Rows[i]["referby"]);//abarna
                                            text = refer + "-" + code;

                                        }
                                        //**
                                        else
                                        {
                                            string Note = Convert.ToString(Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Note);
                                            if (Note.Trim() == "")
                                            {
                                                text = Convert.ToString(ds.Tables[0].Rows[i][col]);
                                            }
                                            else
                                            {
                                                if (ds.Tables[2].Rows.Count > 0)
                                                {
                                                    ds.Tables[2].DefaultView.RowFilter = "app_no='" + Convert.ToString(ds.Tables[0].Rows[i]["app_no"]) + "' and CertificateId='" + col + "'";
                                                    dv1 = ds.Tables[2].DefaultView;
                                                    if (dv1.Count > 0)
                                                    {
                                                        text = Convert.ToString(dv1[0]["certificateno"]);
                                                    }
                                                    else
                                                    {
                                                        text = string.Empty;
                                                    }
                                                }
                                                else
                                                {
                                                    text = string.Empty;
                                                }
                                            }
                                        }
                                        if (col == "visualhandy")
                                        {
                                            if (text == "0")
                                            {
                                                text = "No";
                                            }
                                            else if (text == "1")
                                            {
                                                text = "Yes";
                                            }
                                        }
                                        if (col == "first_graduate")
                                        {
                                            if (text == "0")
                                            {
                                                text = "No";
                                            }
                                            else if (text == "1")
                                            {
                                                text = "Yes";
                                            }
                                        }
                                        if (col == "Countryp" || col == "Countryc")
                                        {
                                            text = d2.GetFunction("select textval from textvaltable where TextCode='" + Convert.ToString(ds.Tables[0].Rows[i][col]) + "'");
                                        }

                                        if (col.ToLower() == "cityp" || col.ToLower() == "cityc")
                                        {
                                            if (!Convert.ToString(ds.Tables[0].Rows[i][col]).Any(char.IsLetter))
                                                text = d2.GetFunction("select textval from textvaltable where TextCriteria ='city' and TextCode='" + Convert.ToString(ds.Tables[0].Rows[i][col]) + "'");
                                        }
                                        if (text == "0")
                                        {
                                            text = string.Empty;
                                        }
                                        if (text == "")
                                        {
                                            text = string.Empty;
                                        }
                                        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                                        cb.AutoPostBack = true;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].CellType = txt;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Text = text;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Column.Width = 180;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Locked = true;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Font.Name = "Book Antiqua";
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Font.Size = FontUnit.Medium;
                                        if (col == "Current_Semester")
                                        {
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        //if (admi_status == "False")
                                        //{
                                        //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].BackColor = ColorTranslator.FromHtml("#B287F2");
                                        //}
                                        //if (admi_status == "0")
                                        //{
                                        //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].BackColor = ColorTranslator.FromHtml("#B287F2");
                                        //}
                                        //else if (delflag == "1")
                                        //{
                                        //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].BackColor = ColorTranslator.FromHtml("#F77474");
                                        //}
                                        //else if (examflg == "DEBAR")
                                        //{
                                        //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].BackColor = ColorTranslator.FromHtml("#A4F9C9");
                                        //}
                                        //else if (coursecomp == "True")
                                        //{
                                        //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].BackColor = ColorTranslator.FromHtml("#65F7E1");
                                        //}
                                        //else
                                        //{
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].BackColor = ColorTranslator.FromHtml("#F2C77D");
                                        // }
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].VerticalAlign = VerticalAlign.Middle;
                                    }
                                }

                                // }
                                //}
                            }
                        }

                        for (int u = 3; u < Fpspread2.Sheets[0].Rows.Count; u++)
                        {
                            //Fpspread2.Sheets[0].SetColumnMerge(u, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        }
                        Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                        Fpspread2.Width = 900;
                        Fpspread2.Height = 420;
                        Fpspread2.Visible = true;
                        imgbtn_columsetting.Visible = true;
                        lblvalidation1.Text = string.Empty;
                        btn_viewsprd2.Visible = true;
                        lnk_admisstionformbtn.Visible = true;
                        if (Fpspread2.Columns.Count > 2)
                        {
                            //Fpspread2.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx");
        }
    }

    public void godetail()
    {
        try
        {
            ccccc = false;
            lbl_headernamespd2.Visible = false;
            btn_viewsprd2.Visible = true;
            lnk_admisstionformbtn.Visible = true;
            img_settingpdf.Visible = true;
            Fpspread2.Visible = true;
            div_report.Visible = true;
            string activerow = string.Empty;
            string activecol = string.Empty;
            int val = 0;
            string addadmited = string.Empty;
            int count = 0;
            int count1 = 0;
            string header = string.Empty;
            string actval = string.Empty;
            string headertype = string.Empty;
            string headertype1 = string.Empty;
            string name = header;
            string addstream = string.Empty;
            string addgraud = string.Empty;
            string addbatch = string.Empty;
            string adddegree = string.Empty;
            string adddeg = string.Empty;
            string addsem = string.Empty;
            string adddsec = string.Empty;
            string addstudtypeval = string.Empty;
            string addseatval = string.Empty;
            string addtypeval = string.Empty;
            string addrelival = string.Empty;
            string addcommval = string.Empty;

            Fpspread2.Sheets[0].Visible = true;
            Fpspread2.Sheets[0].RowHeader.Visible = false;
            Fpspread2.CommandBar.Visible = false;
            Fpspread2.Sheets[0].AutoPostBack = false;
            Fpspread2.Sheets[0].RowCount = 0;
            Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread2.Sheets[0].ColumnCount = 4;
            FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
            darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle2.ForeColor = Color.Black;
            darkstyle2.HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
            loadlcolumns();
            int i = 0;
            int chk = 0;
            for (i = 0; i < cbl_stream.Items.Count; i++)
            {
                if (cbl_stream.Items[i].Selected == true)
                {
                    string addstream1 = cbl_stream.Items[i].Value.ToString();
                    if (addstream == "")
                    {
                        addstream = addstream1;
                    }
                    else
                    {
                        addstream = addstream + "'" + "," + "'" + addstream1;
                    }
                }
            }
            for (i = 0; i < cbl_graduation.Items.Count; i++)
            {
                if (cbl_graduation.Items[i].Selected == true)
                {
                    string addgraud1 = cbl_graduation.Items[i].Value.ToString();
                    if (addgraud == "")
                    {
                        addgraud = addgraud1;
                    }
                    else
                    {
                        addgraud = addgraud + "'" + "," + "'" + addgraud1;
                    }
                }
            }
            if (addgraud == "")
            {
                lbl_err_stud.Visible = true;
                lbl_err_stud.Text = "Kindly Select The Graduation";
                return;
            }
            for (i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    string addbatch1 = cbl_batch.Items[i].Value.ToString();
                    if (addbatch == "")
                    {
                        addbatch = addbatch1;
                    }
                    else
                    {
                        addbatch = addbatch + "'" + "," + "'" + addbatch1;
                    }
                }
            }
            if (addbatch == "")
            {
                lbl_err_stud.Visible = true;
                lbl_err_stud.Text = "Kindly Select The Batch Year ";
                return;
            }
            for (i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    string addstream1 = cbl_degree.Items[i].Value.ToString();
                    if (adddegree == "")
                    {
                        adddegree = addstream1;
                    }
                    else
                    {
                        adddegree = adddegree + "'" + "," + "'" + addstream1;
                    }
                }
            }
            if (adddegree == "")
            {
                lbl_err_stud.Visible = true;
                lbl_err_stud.Text = "Kindly Select The Degree ";
                return;
            }
            for (i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    string adddeg1 = cbl_branch.Items[i].Value.ToString();
                    if (adddeg == "")
                    {
                        adddeg = adddeg1;
                    }
                    else
                    {
                        adddeg = adddeg + "'" + "," + "'" + adddeg1;
                    }
                }
            }
            if (adddeg == "")
            {
                lbl_err_stud.Visible = true;
                lbl_err_stud.Text = "Kindly Select The Branch ";
                return;
            }
            for (i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    string addsem1 = cbl_sem.Items[i].Value.ToString();
                    if (addsem == "")
                    {
                        addsem = addsem1;
                    }
                    else
                    {
                        addsem = addsem + "'" + "," + "'" + addsem1;
                    }
                }
            }
            if (addsem == "")
            {
                lbl_err_stud.Visible = true;
                lbl_err_stud.Text = "Kindly Select The Semester ";
                return;
            }
            for (i = 0; i < cbl_sec.Items.Count; i++)
            {
                if (cbl_sec.Items[i].Selected == true)
                {
                    string addsem1 = cbl_sec.Items[i].Value.ToString();
                    if (adddsec == "")
                    {
                        adddsec = addsem1;
                    }
                    else
                    {
                        adddsec = adddsec + "'" + "," + "'" + addsem1;
                    }
                }
            }
            for (i = 0; i < cbl_studtype.Items.Count; i++)
            {
                if (cbl_studtype.Items[i].Selected == true)
                {
                    string addstudtype1 = cbl_studtype.Items[i].Value.ToString();
                    if (addstudtypeval == "")
                    {
                        addstudtypeval = addstudtype1;
                    }
                    else
                    {
                        addstudtypeval = addstudtypeval + "'" + "," + "'" + addstudtype1;
                    }
                }
            }
            for (i = 0; i < cbl_seat.Items.Count; i++)
            {
                if (cbl_seat.Items[i].Selected == true)
                {
                    string addseat1 = cbl_seat.Items[i].Text.ToString();
                    string addseatval1 = cbl_seat.Items[i].Value.ToString();
                    if (addseatval == "")
                    {
                        addseatval = addseatval1;
                    }
                    else
                    {
                        addseatval = addseatval + "','" + addseatval1;
                    }
                }
            }
            //for (i = 0; i < cblQuota.Items.Count; i++)//abarna
            //{
            //    if (cblQuota.Items[i].Selected == true)
            //    {
            //        string addseat1 = cblQuota.Items[i].Text.ToString();
            //        string addseatval1 = cblQuota.Items[i].Value.ToString();
            //        if (addseatval == "")
            //        {
            //            addseatval = addseatval1;
            //        }
            //        else
            //        {
            //            addseatval = addseatval + "','" + addseatval1;
            //        }
            //    }
            //}
            for (i = 0; i < cbl_type.Items.Count; i++)
            {
                if (cbl_type.Items[i].Selected == true)
                {
                    string addtype1 = cbl_type.Items[i].Text.ToString();
                    string addtypeval1 = cbl_type.Items[i].Value.ToString();
                    if (addtypeval == "")
                    {
                        addtypeval = addtypeval1;
                    }
                    else
                    {
                        addtypeval = addtypeval + "','" + addtypeval1;
                    }
                }
            }
            for (i = 0; i < cbl_religion.Items.Count; i++)
            {
                if (cbl_religion.Items[i].Selected == true)
                {
                    string addreli1 = cbl_religion.Items[i].Text.ToString();
                    string addrelival1 = cbl_religion.Items[i].Value.ToString();
                    if (addrelival == "")
                    {
                        addrelival = addrelival1;
                    }
                    else
                    {
                        addrelival = addrelival + "'" + "," + "'" + addrelival1;
                    }
                }
            }
            for (i = 0; i < cbl_comm.Items.Count; i++)
            {
                if (cbl_comm.Items[i].Selected == true)
                {
                    string addcomm1 = cbl_comm.Items[i].Text.ToString();
                    string addcommval1 = cbl_comm.Items[i].Value.ToString();
                    if (addcommval == "")
                    {
                        addcommval = addcommval1;
                    }
                    else
                    {
                        addcommval = addcommval + "'" + "," + "'" + addcommval1;
                    }
                }
            }
            string query = string.Empty;
            string addd = string.Empty;
            string addd1 = string.Empty;
            if (cb_studtypechk.Checked == true)
            {
                addd1 = " and r.Stud_Type in('" + addstudtypeval + "')";
                chk = 1;
            }
            if (cb_seatchk.Checked == true)
            {
                if (addd1 == "")
                {
                    addd1 = " and a.seattype in('" + addseatval + "')";
                }
                else
                {
                    addd1 = addd1 + " and a.seattype in('" + addseatval + "')";
                }
            }
            if (cb_typechk.Checked == true)
            {
                if (addd == "")
                {
                    addd = " and a.mode in('" + addtypeval + "')";
                }
                else
                {
                    addd = addd + " and a.mode in('" + addtypeval + "')";
                }
            }
            if (cb_relichk.Checked == true)
            {
                if (addd == "")
                {
                    addd = " and a.religion in('" + addrelival + "')";
                }
                else
                {
                    addd = addd + " and a.religion in('" + addrelival + "')";
                }
            }
            if (cb_commchk.Checked == true)
            {
                if (addd == "")
                {
                    addd = " and a.community in('" + addcommval + "')";
                }
                else
                {
                    addd = addd + " and a.community in('" + addcommval + "')";
                }
            }
            if (cb_status.Checked == true)
            {
                ccccc = true;
                if (Convert.ToString(ddl_status.SelectedItem.Value) == "6")
                {
                    if (addd == "")
                    {
                        addd = " and r.Exam_Flag=1";
                    }
                    else
                    {
                        addd = addd + " and r.Exam_Flag='DEBAR'";
                    }
                }
                if (Convert.ToString(ddl_status.SelectedItem.Value) == "3")
                {
                    if (addd == "")
                    {
                        addd = " and a.admission_status='2'";
                    }
                    else
                    {
                        addd = addd + " and a.admission_status='2'";
                    }
                }
                if (Convert.ToString(ddl_status.SelectedItem.Value) == "2")
                {
                    if (addd == "")
                    {
                        addd = " and a.admission_status='1'";
                    }
                    else
                    {
                        addd = addd + " and a.admission_status='1'";
                    }
                }
                if (Convert.ToString(ddl_status.SelectedItem.Value) == "5")
                {
                    if (addd == "")
                    {
                        addd = " and r.DelFlag<>'0'";
                    }
                    else
                    {
                        addd = addd + " and r.DelFlag<>'0'";
                    }
                }
                if (Convert.ToString(ddl_status.SelectedItem.Value) == "7")
                {
                    if (addd == "")
                    {
                        addd = " and r.cc=1";
                    }
                    else
                    {
                        addd = addd + " and r.cc=1";
                    }
                }
            }
            DateTime from = new DateTime();
            DateTime to = new DateTime();
            string[] ay = txt_fromdate.Text.Split('/');
            string[] ay1 = txt_todate.Text.Split('/');
            from = Convert.ToDateTime(ay[1] + "/" + ay[0] + "/" + ay[2]);
            to = Convert.ToDateTime(ay1[1] + "/" + ay1[0] + "/" + ay1[2]);
            string datebetween = string.Empty;
            string datebetween1 = string.Empty;
            if (cb_from.Checked == true)
            {
                datebetween = "  and r.Adm_Date between  '" + from.ToString("MM/dd/yyyy") + "' and '" + to.ToString("MM/dd/yyyy") + "' ";
                datebetween1 = "  and a.date_applied between '" + from.ToString("MM/dd/yyyy") + "' and '" + to.ToString("MM/dd/yyyy") + "' ";
            }
            string sectionvalue = string.Empty;
            if (adddsec != "")
            {
                sectionvalue = "and Sections in('" + adddsec + "')";
            }
            else
            {
                sectionvalue = string.Empty;
            }
            if (ccccc == true)
            {
                query = "select '0' CC,'' Exam_Flag,'0' DelFlag,a.Admission_Status,a.missionarydisc,a.MissionaryChild,a.parent_phnoc,a.parent_pincodec,a.Student_Mobile,a.Countryc,a.parent_statec,a.Cityc,a.Streetc,a.parent_addressC,a.co_curricular,a.citizen,a.visualhandy,a.first_graduate,a.TamilOrginFromAndaman,a.seattype,a.mother_tongue,a.parent_occu,a.caste,a.community,a.religion,a.parent_name,'' Roll_No,a.Stud_Name,a.Batch_Year,a.App_no,a.Current_Semester,c.Course_Name,Dt.Dept_Name,'' Sections,CONVERT(VARCHAR(11),dob,103) as dob, CONVERT(varchar(10), a.enrollment_confirm_date,103)enrollment_confirm_date,CONVERT(varchar(10), r.Adm_Date,103)Adm_Date from applyn a, degree d,Department dt,Course C where d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'  and a.degree_code in('" + adddeg + "')and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "' " + datebetween1 + "  ";
                if (ddl_status.SelectedItem.Value == "2")
                {
                    query = "select distinct '0' CC,'' Exam_Flag,'0' DelFlag,a.Admission_Status,a.missionarydisc,a.MissionaryChild,a.parent_phnoc,a.parent_pincodec,a.Student_Mobile,a.Countryc,a.parent_statec,a.Cityc,a.Streetc,a.parent_addressC,a.co_curricular,a.citizen,a.visualhandy,a.first_graduate,a.TamilOrginFromAndaman,a.seattype,a.mother_tongue,a.parent_occu,a.caste,a.community,a.religion,a.parent_name,'' Roll_No,a.Stud_Name,a.Batch_Year,a.App_no,a.Current_Semester,c.Course_Name,Dt.Dept_Name,'' Sections,CONVERT(VARCHAR(11),dob,103) as dob,CONVERT(varchar(10), a.enrollment_confirm_date,103)enrollment_confirm_date,CONVERT(varchar(10), r.Adm_Date,103)Adm_Date from degree d,Department dt,Course C,applyn a LEFT JOIN Registration r ON a.app_no = r.App_No WHERE r.App_No IS NULL and  d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status='1'   and a.degree_code in('" + adddeg + "')and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  " + datebetween1 + " ";
                }
                if (ddl_status.SelectedItem.Value == "4")
                {
                    query = " select r.CC,r.Exam_Flag,r.DelFlag,a.Admission_Status,a.missionarydisc,a.MissionaryChild,a.parent_phnoc,a.parent_pincodec,a.Student_Mobile,a.Countryc,a.parent_statec,a.Cityc,a.Streetc,a.parent_addressC,a.co_curricular,a.citizen,a.visualhandy,a.first_graduate,a.TamilOrginFromAndaman,a.seattype,a.mother_tongue,a.parent_occu,a.caste,a.community,a.religion,a.parent_name,r.Roll_No,r.Stud_Name,r.Batch_Year,a.App_no,r.Current_Semester,c.Course_Name,Dt.Dept_Name,isnull( r.Sections,'') as Sections,CONVERT(VARCHAR(11),dob,103) as dob,CONVERT(varchar(10), a.enrollment_confirm_date,103)enrollment_confirm_date,CONVERT(varchar(10), r.Adm_Date,103)Adm_Date from Registration r,applyn a, degree d,Department dt,Course C where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No and isconfirm ='1' and admission_status ='1' and r.CC='0' and r.Exam_Flag='OK' and r.DelFlag='0'  and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "')   and a.college_code='" + ddlcollege.SelectedItem.Value + "' " + datebetween + "";
                }
                if (ddl_status.SelectedItem.Value == "6")
                {
                    query = " select r.CC,r.Exam_Flag,r.DelFlag,a.Admission_Status,a.missionarydisc,a.MissionaryChild,a.parent_phnoc,a.parent_pincodec,a.Student_Mobile,a.Countryc,a.parent_statec,a.Cityc,a.Streetc,a.parent_addressC,a.co_curricular,a.citizen,a.visualhandy,a.first_graduate,a.TamilOrginFromAndaman,a.seattype,a.mother_tongue,a.parent_occu,a.caste,a.community,a.religion,a.parent_name,r.Roll_No,r.Stud_Name,r.Batch_Year,a.App_no,r.Current_Semester,c.Course_Name,Dt.Dept_Name,isnull( r.Sections,'') as Sections,CONVERT(VARCHAR(11),dob,103) as dob,CONVERT(varchar(10), a.enrollment_confirm_date,103)enrollment_confirm_date,CONVERT(varchar(10), r.Adm_Date,103)Adm_Date from Registration r,applyn a, degree d,Department dt,Course C where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No and isconfirm ='1'   and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "')   and a.college_code='" + ddlcollege.SelectedItem.Value + "' and Exam_Flag='DEBAR' and r.CC='0' and r.DelFlag='0' " + datebetween + " ";
                }
                if (ddl_status.SelectedItem.Value == "5")
                {
                    query = " select r.CC,r.Exam_Flag,r.DelFlag,a.Admission_Status,a.missionarydisc,a.MissionaryChild,a.parent_phnoc,a.parent_pincodec,a.Student_Mobile,a.Countryc,a.parent_statec,a.Cityc,a.Streetc,a.parent_addressC,a.co_curricular,a.citizen,a.visualhandy,a.first_graduate,a.TamilOrginFromAndaman,a.seattype,a.mother_tongue,a.parent_occu,a.caste,a.community,a.religion,a.parent_name,r.Roll_No,r.Stud_Name,r.Batch_Year,a.App_no,r.Current_Semester,c.Course_Name,Dt.Dept_Name,isnull( r.Sections,'') as Sections,CONVERT(VARCHAR(11),dob,103) as dob,CONVERT(varchar(10), a.enrollment_confirm_date,103)enrollment_confirm_date,CONVERT(varchar(10), r.Adm_Date,103)Adm_Date from Registration r,applyn a, degree d,Department dt,Course C where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No and isconfirm ='1'   and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "')   and a.college_code='" + ddlcollege.SelectedItem.Value + "' and DelFlag<>'0' and Exam_Flag='OK' and r.CC='0' " + datebetween + " ";
                }
                if (ddl_status.SelectedItem.Value == "7")
                {
                    query = " select r.CC,r.Exam_Flag,r.DelFlag,a.Admission_Status,a.missionarydisc,a.MissionaryChild,a.parent_phnoc,a.parent_pincodec,a.Student_Mobile,a.Countryc,a.parent_statec,a.Cityc,a.Streetc,a.parent_addressC,a.co_curricular,a.citizen,a.visualhandy,a.first_graduate,a.TamilOrginFromAndaman,a.seattype,a.mother_tongue,a.parent_occu,a.caste,a.community,a.religion,a.parent_name,r.Roll_No,r.Stud_Name,r.Batch_Year,a.App_no,r.Current_Semester,c.Course_Name,Dt.Dept_Name,isnull( r.Sections,'') as Sections,CONVERT(VARCHAR(11),dob,103) as dob,CONVERT(varchar(10), a.enrollment_confirm_date,103)enrollment_confirm_date,CONVERT(varchar(10), r.Adm_Date,103)Adm_Date from Registration r,applyn a, degree d,Department dt,Course C where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No and isconfirm ='1' and admission_status ='1'  and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "')   and a.college_code='" + ddlcollege.SelectedItem.Value + "' and CC='1' and DelFlag='0' and Exam_Flag='OK' " + datebetween + "";
                }
                if (ddl_status.SelectedItem.Value == "3")
                {
                    query = "  select ''CC,''Exam_Flag,''DelFlag,a.Admission_Status,a.missionarydisc,a.MissionaryChild,a.parent_phnoc,a.parent_pincodec,a.Student_Mobile,a.Countryc,a.parent_statec,a.Cityc,a.Streetc,a.parent_addressC,a.co_curricular,a.citizen,a.visualhandy,a.first_graduate,a.TamilOrginFromAndaman,a.seattype,a.mother_tongue,a.parent_occu,a.caste,a.community,a.religion,a.parent_name,'' Roll_No,a.Stud_Name,a.Batch_Year,a.App_no,a.Current_Semester,c.Course_Name,Dt.Dept_Name,''Sections,CONVERT(VARCHAR(11),dob,103) as dob  from applyn a, degree d,Department dt,Course C where d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='2' and  a.degree_code in('" + adddeg + "')and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "')   and a.college_code='" + ddlcollege.SelectedItem.Value + "' ";
                }
            }
            else if (chk == 1)
            {
                query = "select r.CC,r.Exam_Flag,r.DelFlag,a.Admission_Status,a.missionarydisc,a.MissionaryChild,a.parent_phnoc,a.parent_pincodec,a.Student_Mobile,a.Countryc,a.parent_statec,a.Cityc,a.Streetc,a.parent_addressC,a.co_curricular,a.citizen,a.visualhandy,a.first_graduate,a.TamilOrginFromAndaman,a.seattype,a.mother_tongue,a.parent_occu,a.caste,a.community,a.religion,a.parent_name,r.Roll_No,r.Stud_Name,r.Batch_Year,a.App_no,r.Current_Semester,c.Course_Name,Dt.Dept_Name,isnull( r.Sections,'') as Sections,CONVERT(VARCHAR(11),dob,103) as dob,CONVERT(varchar(10), a.enrollment_confirm_date,103)enrollment_confirm_date,CONVERT(varchar(10), r.Adm_Date,103)Adm_Date from Registration r,applyn a, degree d,Department dt,Course C where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No and isconfirm ='1' and admission_status ='1' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "' " + addd1 + "  ";
            }
            else
            {
                query = "select '0' CC,'' Exam_Flag,'0' DelFlag,a.Admission_Status,a.missionarydisc,a.MissionaryChild,a.parent_phnoc,a.parent_pincodec,a.Student_Mobile,a.Countryc,a.parent_statec,a.Cityc,a.Streetc,a.parent_addressC,a.co_curricular,a.citizen,a.visualhandy,a.first_graduate,a.TamilOrginFromAndaman,a.seattype,a.mother_tongue,a.parent_occu,a.caste,a.community,a.religion,a.parent_name,'' Roll_No,a.Stud_Name,a.Batch_Year,a.App_no,a.Current_Semester,c.Course_Name,Dt.Dept_Name,'' Sections,CONVERT(VARCHAR(11),dob,103) as dob from applyn a, degree d,Department dt,Course C where d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='0'   and a.degree_code in('" + adddeg + "')and a.Batch_Year in('" + addbatch + "') and  a.Current_Semester in('" + addsem + "') " + datebetween1 + "   " + addd + "  union all  select r.CC,r.Exam_Flag,r.DelFlag,a.Admission_Status,a.missionarydisc,a.MissionaryChild,a.parent_phnoc,a.parent_pincodec,a.Student_Mobile,a.Countryc,a.parent_statec,a.Cityc,a.Streetc,a.parent_addressC,a.co_curricular,a.citizen,a.visualhandy,a.first_graduate,a.TamilOrginFromAndaman,a.seattype,a.mother_tongue,a.parent_occu,a.caste,a.community,a.religion,a.parent_name,r.Roll_No,r.Stud_Name,r.Batch_Year,a.App_no,r.Current_Semester,c.Course_Name,Dt.Dept_Name,isnull( r.Sections,'') as Sections,CONVERT(VARCHAR(11),dob,103) as dob,CONVERT(varchar(10), a.enrollment_confirm_date,103)enrollment_confirm_date,CONVERT(varchar(10), r.Adm_Date,103)Adm_Date from Registration r,applyn a, degree d,Department dt,Course C where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No and isconfirm ='1' and admission_status ='1' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + addbatch + "') and  r.Current_Semester in('" + addsem + "')  " + sectionvalue + "  " + addd + "  and a.college_code='" + ddlcollege.SelectedItem.Value + "' " + datebetween + " " + addd1 + "  ";
            }
            query = query + "sELECT a.App_No,uni_state,type_semester,university_code,ISNULL(pt.TExtVal,'') Part2Language,ISNULL(C.TExtVal,'') Part1Language,ISNULL(C.TExtVal,'') Course,ISNULL(U.TextVal,'') University,ISNULL(Institute_Name,'') Institute_Name,ISNULL(PassMonth,'') PassMonth,ISNULL(PassYear,'') PassYear FROM Stud_prev_details P INNER JOIN applyn A ON P.app_no = A.app_no LEFT JOIN TextValTable C ON C.TextCode = P.course_code  LEFT JOIN TextValTable U ON U.TextCode = P.university_code LEFT JOIN TextValTable pa ON pa.TextCode = P.Part1Language LEFT JOIN TextValTable pt ON pt.TextCode = P.Part2Language Where p.app_no = a.app_no  ";
            query = query + "select * from StudCertDetails s,applyn a where a.App_No=s.App_No";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");

            if (query == "")
            {
                Fpspread2.Sheets[0].Visible = false;
                lblerror.Visible = true;
                lblerror.Text = "Kindly Select All List ";
                div_report.Visible = false;
                imgbtn_columsetting.Visible = false;
                btn_viewsprd2.Visible = false;
                lnk_admisstionformbtn.Visible = false;
                img_settingpdf.Visible = false;
                return;
            }
            else
            {
                if (query != "")
                {
                    ds = d2.select_method(query, hat, "Text");
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        Fpspread2.Sheets[0].Visible = false;
                        Fpspread2.Visible = false;
                        lblerror.Visible = true;
                        lblerror.Text = "No Records Found";
                        div_report.Visible = false;
                        imgbtn_columsetting.Visible = false;
                        btn_viewsprd2.Visible = false;
                        lnk_admisstionformbtn.Visible = false;
                        img_settingpdf.Visible = false;
                        lbl_headernamespd2.Visible = false;
                        divcolor.Visible = false;
                        return;
                    }
                    else
                    {
                        lblerror.Visible = false;
                        lbl_err_stud.Visible = false;
                        btn_viewsprd2.Visible = true;
                        lnk_admisstionformbtn.Visible = true;
                        img_settingpdf.Visible = true;
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            div_report.Visible = true;
                            if (name == "All")
                            {
                                lbl_headernamespd2.Text = name;
                            }
                            else
                            {
                                lbl_headernamespd2.Text = headertype + "-" + name;
                            }
                            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                            int cc = 3;
                            int j = 0;
                            //loadlcolumns();
                            DataSet dss = new DataSet();
                            string linkname1 = Convert.ToString(ddl_colord.SelectedItem.Text);
                            string selcol1 = "select LinkValue from New_InsSettings where LinkName='" + linkname1 + "' and  user_code='" + usercode + "' and college_code='" + ddlcollege.SelectedItem.Value + "' ";
                            dss.Clear();
                            dss = d2.select_method_wo_parameter(selcol1, "Text");
                            if (dss.Tables.Count > 0)
                            {
                                if (dss.Tables[0].Rows.Count > 0)
                                {
                                    for (int c = 0; c < dss.Tables[0].Rows.Count; c++)
                                    {
                                        string value = Convert.ToString(dss.Tables[0].Rows[c]["LinkValue"]);
                                        if (value != "")
                                        {
                                            string[] valuesplit = value.Split(',');
                                            if (valuesplit.Length > 0)
                                            {
                                                for (int k = 0; k < valuesplit.Length; k++)
                                                {
                                                    cc++;
                                                    colval = Convert.ToString(valuesplit[k]);
                                                    loadtext();
                                                    Fpspread2.Sheets[0].ColumnCount = cc + 1;
                                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Text = loadval;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    imgdiv2.Visible = true;
                                    lbl_alert.Text = "No Records Found";
                                    Fpspread2.Visible = false;
                                    div_report.Visible = false;
                                    img_settingpdf.Visible = false;
                                    btn_viewsprd2.Visible = false;
                                    lnk_admisstionformbtn.Visible = false;
                                    lbl_headernamespd2.Visible = false;
                                    return;
                                }
                            }
                            else
                            {
                                imgdiv2.Visible = true;
                                lbl_alert.Text = "No Records Found";
                                imgdiv2.Visible = true;
                                lbl_alert.Text = "No Records Found";
                                Fpspread2.Visible = false;
                                div_report.Visible = false;
                                img_settingpdf.Visible = false;
                                btn_viewsprd2.Visible = false;
                                lnk_admisstionformbtn.Visible = false;
                                lbl_headernamespd2.Visible = false;
                                divcolor.Visible = false;
                                return;
                            }
                            string txt1 = string.Empty;
                            string txt2 = string.Empty;
                            string txt3 = string.Empty;
                            string txt4 = string.Empty;
                            string txt5 = string.Empty;
                            FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                            cb.AutoPostBack = true;
                            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (i == 0)
                                {
                                    Fpspread2.Sheets[0].RowCount++;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = cb;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                }
                                Fpspread2.Sheets[0].RowCount++;
                                count++;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Locked = true;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = cb;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Roll_no"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["App_no"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["stud_name"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Column.Width = 250;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Locked = true;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Locked = true;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                string admi_status = Convert.ToString(ds.Tables[0].Rows[i]["Admission_Status"]);
                                string delflag = Convert.ToString(ds.Tables[0].Rows[i]["DelFlag"]);
                                string examflg = Convert.ToString(ds.Tables[0].Rows[i]["Exam_Flag"]);
                                string coursecomp = Convert.ToString(ds.Tables[0].Rows[i]["CC"]);
                                if (admi_status == "False")
                                {
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].BackColor = ColorTranslator.FromHtml("#B287F2");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].BackColor = ColorTranslator.FromHtml("#B287F2");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].BackColor = ColorTranslator.FromHtml("#B287F2");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#B287F2");
                                }
                                if (admi_status == "0")
                                {
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].BackColor = ColorTranslator.FromHtml("#B287F2");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].BackColor = ColorTranslator.FromHtml("#B287F2");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].BackColor = ColorTranslator.FromHtml("#B287F2");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#B287F2");
                                }
                                else if (delflag == "1" || admi_status == "2")
                                {
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].BackColor = ColorTranslator.FromHtml("#F77474");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].BackColor = ColorTranslator.FromHtml("#F77474");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].BackColor = ColorTranslator.FromHtml("#F77474");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#F77474");
                                }
                                else if (examflg == "DEBAR")
                                {
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].BackColor = ColorTranslator.FromHtml("#A4F9C9");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].BackColor = ColorTranslator.FromHtml("#A4F9C9");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].BackColor = ColorTranslator.FromHtml("#A4F9C9");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#A4F9C9");
                                }
                                else if (coursecomp == "True")
                                {
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].BackColor = ColorTranslator.FromHtml("#65F7E1");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].BackColor = ColorTranslator.FromHtml("#65F7E1");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].BackColor = ColorTranslator.FromHtml("#65F7E1");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#65F7E1");
                                }
                                else
                                {
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].BackColor = ColorTranslator.FromHtml("#F2C77D");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].BackColor = ColorTranslator.FromHtml("#F2C77D");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].BackColor = ColorTranslator.FromHtml("#F2C77D");
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#F2C77D");
                                }
                                cc = 3;
                                string text = string.Empty;
                                DataView dv = new DataView();
                                DataView dv1 = new DataView();
                                string linkname = Convert.ToString(ddl_colord.SelectedItem.Text);
                                string columnvalue = string.Empty;
                                DataSet dscol = new DataSet();
                                string selcol = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + ddlcollege.SelectedItem.Value + "' ";
                                dscol.Clear();
                                dscol = d2.select_method_wo_parameter(selcol, "Text");
                                if (dscol.Tables.Count > 0)
                                {
                                    if (dscol.Tables[0].Rows.Count > 0)
                                    {
                                        for (int c = 0; c < dscol.Tables[0].Rows.Count; c++)
                                        {
                                            string value = Convert.ToString(dscol.Tables[0].Rows[c]["LinkValue"]);
                                            if (value != "")
                                            {
                                                string[] valuesplit = value.Split(',');
                                                if (valuesplit.Length > 0)
                                                {
                                                    for (int k = 0; k < valuesplit.Length; k++)
                                                    {
                                                        Fpspread2.Sheets[0].ColumnCount = 4 + valuesplit.Length;
                                                        cc++;
                                                        colval = Convert.ToString(valuesplit[k]);
                                                        loadvalue();
                                                        string col = loadval;
                                                        if (col == "type_semester" || col == "Institute_name" || col == "isgrade" || col == "Part1Language" || col == "Part2Language" || col == "university_code")
                                                        {
                                                            if (ds.Tables[1].Rows.Count > 0)
                                                            {
                                                                string appno = d2.GetFunction("select App_No from Registration where Roll_No='" + Convert.ToString(ds.Tables[0].Rows[i]["Roll_no"]) + "'");
                                                                if (ds.Tables[1].Rows.Count > 0)
                                                                {
                                                                    ds.Tables[1].DefaultView.RowFilter = "app_no='" + appno + "'";
                                                                    dv = ds.Tables[1].DefaultView;
                                                                    if (dv.Count > 0)
                                                                    {
                                                                        text = Convert.ToString(dv[0][col]);
                                                                    }
                                                                    else
                                                                    {
                                                                        text = string.Empty;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                text = string.Empty;
                                                            }
                                                        }
                                                        else if (col == "CommunityNo" || col == "TCNo" || col == "Ten_CertNo" || col == "Twelth_CertNo" || col == "DeplomProv_CertNo" || col == "DeplomConsolidate_CertNo" || col == "DeplomDegree_CertNo" || col == "UGProv_CertNo" || col == "UGConsolidate_CertNo" || col == "UGDegree_CertNo" || col == "PGProv_CertNo" || col == "PGConsolidate_CertNo" || col == "PGDegree_CertNo")
                                                        {
                                                            string appno = d2.GetFunction("select App_No from Registration where Roll_No='" + Convert.ToString(ds.Tables[0].Rows[i]["Roll_no"]) + "'");
                                                            if (ds.Tables[2].Rows.Count > 0)
                                                            {
                                                                ds.Tables[2].DefaultView.RowFilter = "app_no='" + appno + "'";
                                                                dv1 = ds.Tables[2].DefaultView;
                                                                if (dv1.Count > 0)
                                                                {
                                                                    text = Convert.ToString(dv1[0][col]);
                                                                }
                                                                else
                                                                {
                                                                    text = string.Empty;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                text = string.Empty;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            text = Convert.ToString(ds.Tables[0].Rows[i][col]);
                                                        }
                                                        if (col == "religion")
                                                        {
                                                            text = d2.GetFunction("SELECT distinct t.TextVal FROM applyn A,Registration R,TextValTable T WHERE A.app_no=R.App_No AND T.TextCode =A.religion AND R.college_code ='" + collegecode1 + "' and religion='" + text + "' ");
                                                        }
                                                        if (col == "community")
                                                        {
                                                            text = d2.GetFunction("SELECT distinct t.TextVal FROM applyn A,Registration R,TextValTable T WHERE A.app_no=R.App_No AND T.TextCode =A.community AND R.college_code ='" + collegecode1 + "' and community='" + text + "' ");
                                                        }
                                                        if (col == "caste")
                                                        {
                                                            text = d2.GetFunction("SELECT distinct t.TextVal FROM applyn A,Registration R,TextValTable T WHERE A.app_no=R.App_No AND T.TextCode =A.caste  AND R.college_code ='" + collegecode1 + "' and caste='" + text + "' ");
                                                        }
                                                        if (col == "mother_tongue")
                                                        {
                                                            text = d2.GetFunction("SELECT distinct t.TextVal FROM applyn A,Registration R,TextValTable T WHERE A.app_no=R.App_No AND T.TextCode =A.mother_tongue  AND R.college_code ='" + collegecode1 + "' and mother_tongue='" + text + "' ");
                                                        }
                                                        if (col == "parent_occu")
                                                        {
                                                            text = d2.GetFunction("SELECT distinct t.TextVal FROM applyn A,Registration R,TextValTable T WHERE A.app_no=R.App_No AND T.TextCode =A.parent_occu  AND R.college_code ='" + collegecode1 + "' and parent_occu='" + text + "' ");
                                                        }
                                                        if (col == "seattype")
                                                        {
                                                            text = d2.GetFunction("SELECT distinct t.TextVal FROM applyn A,Registration R,TextValTable T WHERE A.app_no=R.App_No AND T.TextCode =A.seattype  AND R.college_code ='" + collegecode1 + "' and seattype='" + text + "' ");
                                                        }
                                                        if (col == "citizen")
                                                        {
                                                            text = d2.GetFunction("SELECT distinct t.TextVal FROM applyn A,Registration R,TextValTable T WHERE A.app_no=R.App_No AND T.TextCode =A.citizen  AND R.college_code ='" + collegecode1 + "' and citizen='" + text + "' ");
                                                        }
                                                        if (col == "visualhandy")
                                                        {
                                                            if (text == "0")
                                                            {
                                                                text = "No";
                                                            }
                                                            else if (text == "1")
                                                            {
                                                                text = "Yes";
                                                            }
                                                        }
                                                        if (col == "first_graduate")
                                                        {
                                                            if (text == "0")
                                                            {
                                                                text = "No";
                                                            }
                                                            else if (text == "1")
                                                            {
                                                                text = "Yes";
                                                            }
                                                        }
                                                        if (col == "TamilOrginFromAndaman")
                                                        {
                                                            text = d2.GetFunction("SELECT distinct t.TextVal FROM applyn A,Registration R,TextValTable T WHERE A.app_no=R.App_No AND T.TextCode =A.TamilOrginFromAndaman  AND R.college_code ='" + collegecode1 + "' and TamilOrginFromAndaman='" + text + "' ");
                                                        }
                                                        if (col == "parent_statec")
                                                        {
                                                            text = d2.GetFunction("SELECT distinct t.TextVal FROM applyn A,Registration R,TextValTable T WHERE A.app_no=R.App_No AND T.TextCode =A.parent_statec  AND R.college_code ='" + collegecode1 + "' and parent_statec='" + text + "' ");
                                                        }
                                                        if (col == "co_curricular")
                                                        {
                                                            text = d2.GetFunction("SELECT distinct t.TextVal FROM applyn A,Registration R,TextValTable T WHERE A.app_no=R.App_No AND T.TextCode =A.co_curricular  AND R.college_code ='" + collegecode1 + "' and co_curricular='" + text + "' ");
                                                        }
                                                        if (col == "Countryc")
                                                        {
                                                            text = d2.GetFunction("SELECT distinct t.TextVal FROM applyn A,Registration R,TextValTable T WHERE A.app_no=R.App_No AND T.TextCode =A.Countryc  AND R.college_code ='" + collegecode1 + "' and Countryc='" + text + "' ");
                                                        }
                                                        if (text == "0")
                                                        {
                                                            text = string.Empty;
                                                        }
                                                        if (text == "")
                                                        {
                                                            text = string.Empty;
                                                        }
                                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Text = text;
                                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Column.Width = 180;
                                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Locked = true;
                                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Font.Name = "Book Antiqua";
                                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Font.Size = FontUnit.Medium;
                                                        if (col == "Current_Semester")
                                                        {
                                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].HorizontalAlign = HorizontalAlign.Center;
                                                        }
                                                        if (admi_status == "False")
                                                        {
                                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].BackColor = ColorTranslator.FromHtml("#B287F2");
                                                        }
                                                        if (admi_status == "0")
                                                        {
                                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].BackColor = ColorTranslator.FromHtml("#B287F2");
                                                        }
                                                        else if (delflag == "1" || admi_status == "2")
                                                        {
                                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].BackColor = ColorTranslator.FromHtml("#F77474");
                                                        }
                                                        else if (examflg == "DEBAR")
                                                        {
                                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].BackColor = ColorTranslator.FromHtml("#A4F9C9");
                                                        }
                                                        else if (coursecomp == "True")
                                                        {
                                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].BackColor = ColorTranslator.FromHtml("#65F7E1");
                                                        }
                                                        else
                                                        {
                                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].BackColor = ColorTranslator.FromHtml("#F2C77D");
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                        Fpspread2.Width = 900;
                        Fpspread2.Height = 420;
                        Fpspread2.Visible = true;
                        imgbtn_columsetting.Visible = true;
                        btn_viewsprd2.Visible = true;
                        lnk_admisstionformbtn.Visible = true;
                        divcolor.Visible = true;
                        if (Fpspread2.Columns.Count > 3)
                        {
                            //Fpspread2.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        }
                    }
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }

    protected void btnMvOneRt_Click(object sender, EventArgs e)
    {
        try
        {
            bool ok = true;
            if (lb_selectcolumn.Items.Count > 0 && lb_selectcolumn.SelectedItem.Value != "")
            {
                for (int j = 0; j < lb_column1.Items.Count; j++)
                {
                    if (lb_column1.Items[j].Value == lb_selectcolumn.SelectedItem.Value)
                    {
                        ok = false;
                    }
                }
                if (ok)
                {
                    ListItem lst = new ListItem(lb_selectcolumn.SelectedItem.Text, lb_selectcolumn.SelectedItem.Value);
                    lb_column1.Items.Add(lst);
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }

    protected void btnMvTwoRt_Click(object sender, EventArgs e)
    {
        try
        {
            lb_column1.Items.Clear();
            if (lb_selectcolumn.Items.Count > 0)
            {
                for (int j = 0; j < lb_selectcolumn.Items.Count; j++)
                {
                    lb_column1.Items.Add(new ListItem(lb_selectcolumn.Items[j].Text.ToString(), lb_selectcolumn.Items[j].Value.ToString()));
                }
            }
            lb_selectcolumn.Items.Clear();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }

    protected void btnMvOneLt_Click(object sender, EventArgs e)
    {
        try
        {
            if (lb_column1.Items.Count > 0 && lb_column1.SelectedItem.Value != "")
            {
                lb_column1.Items.RemoveAt(lb_column1.SelectedIndex);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }

    protected void btnMvTwoLt_Click(object sender, EventArgs e)
    {
        try
        {
            lb_column1.Items.Clear();
            load();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }

    protected void btnok_click(object sender, EventArgs e)
    {
        if (ddl_coltypeadd.SelectedItem.Text != "Select")
        {
            if (lb_column1.Items.Count > 0)
            {
                poppernew.Visible = false;
                savecolumnorder();
                if (savecolumnoder == "")
                {
                    fpspread1go1();
                }
                else
                {
                    if (rdb_cumm.Checked == true)
                    {
                        go();
                    }
                    else
                    {
                        fpspread1go1();
                    }
                    savecolumnoder = string.Empty;
                }
                lblalerterr.Visible = false;
            }
            else
            {
                lblalerterr.Visible = true;
                lblalerterr.Text = "Please select atleast one colunm then proceed!";
            }
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Please Select Report Type";
        }
    }

    protected void btnclose_click(object sender, EventArgs e)
    {
        poppernew.Visible = false;
    }

    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        poppernew.Visible = false;
    }

    public void imgbtn_all_Click(object sender, EventArgs e)
    {
        poppernew.Visible = true;
        load();
        lb_column1.Items.Clear();
    }

    public void load()
    {
        lb_selectcolumn.Items.Clear();
        lb_selectcolumn.Items.Add(new ListItem("Student Name", "54"));
        lb_selectcolumn.Items.Add(new ListItem("Roll No", "55"));
        lb_selectcolumn.Items.Add(new ListItem("Reg No", "57"));
        lb_selectcolumn.Items.Add(new ListItem("Admission No", "58"));
        lb_selectcolumn.Items.Add(new ListItem("Application No", "59"));
        lb_selectcolumn.Items.Add(new ListItem("Applied Date", "81"));
        lb_selectcolumn.Items.Add(new ListItem("Batch", "3"));
        lb_selectcolumn.Items.Add(new ListItem(lbl_degree.Text, "1"));
        lb_selectcolumn.Items.Add(new ListItem(lbl_branch.Text, "2"));
        lb_selectcolumn.Items.Add(new ListItem(lbl_org_sem.Text, "4"));
        lb_selectcolumn.Items.Add(new ListItem("Section", "60"));
        lb_selectcolumn.Items.Add(new ListItem("SeatType", "16"));
        lb_selectcolumn.Items.Add(new ListItem("Student Type", "63"));
        lb_selectcolumn.Items.Add(new ListItem("HostelName", "34"));
        //30.07.16
        lb_selectcolumn.Items.Add(new ListItem("Mode", "43"));
        lb_selectcolumn.Items.Add(new ListItem("Boarding", "122"));
        lb_selectcolumn.Items.Add(new ListItem("Vehicle Id", "123"));
        lb_selectcolumn.Items.Add(new ListItem("Gender", "61"));
        lb_selectcolumn.Items.Add(new ListItem("DOB", "6"));
        lb_selectcolumn.Items.Add(new ListItem("Blood Group", "62"));
        lb_selectcolumn.Items.Add(new ListItem("Father Name", "5"));
        lb_selectcolumn.Items.Add(new ListItem("Father Income", "84"));
        lb_selectcolumn.Items.Add(new ListItem("Father Occupation", "7"));
        lb_selectcolumn.Items.Add(new ListItem("Father Mob No", "85"));
        lb_selectcolumn.Items.Add(new ListItem("Father Email Id", "86"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Name", "87"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Income", "88"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Occupation", "96"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Mob No", "89"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Email Id", "90"));
        lb_selectcolumn.Items.Add(new ListItem("Guardian Name", "91"));
        lb_selectcolumn.Items.Add(new ListItem("Guardian Email Id", "92"));
        lb_selectcolumn.Items.Add(new ListItem("Guardian Mob No", "93"));
        lb_selectcolumn.Items.Add(new ListItem("Place Of Birth", "94"));
        lb_selectcolumn.Items.Add(new ListItem("Adhaar Card No", "95"));
        lb_selectcolumn.Items.Add(new ListItem("Voter ID", "35"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Tongue", "8"));
        lb_selectcolumn.Items.Add(new ListItem("Religion", "9"));
        lb_selectcolumn.Items.Add(new ListItem("Community", "11"));
        lb_selectcolumn.Items.Add(new ListItem("Caste", "12"));
        lb_selectcolumn.Items.Add(new ListItem("Sub Caste", "83"));
        lb_selectcolumn.Items.Add(new ListItem("Citizen", "10"));
        lb_selectcolumn.Items.Add(new ListItem("TamilOrginFromAndaman", "13"));
        lb_selectcolumn.Items.Add(new ListItem("Ex-serviceman", "64"));
        lb_selectcolumn.Items.Add(new ListItem("Rank", "74"));
        lb_selectcolumn.Items.Add(new ListItem("Place", "75"));
        lb_selectcolumn.Items.Add(new ListItem("Number", "76"));
        lb_selectcolumn.Items.Add(new ListItem("IsDisable", "53"));
        lb_selectcolumn.Items.Add(new ListItem("VisualHandy", "14"));
        lb_selectcolumn.Items.Add(new ListItem("Residency", "48"));
        lb_selectcolumn.Items.Add(new ListItem("Physically challange", "49"));
        lb_selectcolumn.Items.Add(new ListItem("Learning Disability", "51"));
        lb_selectcolumn.Items.Add(new ListItem("Other Disability", "52"));
        lb_selectcolumn.Items.Add(new ListItem("Sports", "50"));
        lb_selectcolumn.Items.Add(new ListItem("First Graduate", "15"));
        lb_selectcolumn.Items.Add(new ListItem("MissionaryChild", "26"));
        lb_selectcolumn.Items.Add(new ListItem("missionarydisc", "27"));
        lb_selectcolumn.Items.Add(new ListItem("Hostel accommodation", "65"));
        lb_selectcolumn.Items.Add(new ListItem("Blood Donor", "66"));
        lb_selectcolumn.Items.Add(new ListItem("Reserved Caste", "67"));
        lb_selectcolumn.Items.Add(new ListItem("Economic Backward", "68"));
        lb_selectcolumn.Items.Add(new ListItem("Parents Old Student", "69"));
        lb_selectcolumn.Items.Add(new ListItem("Driving License", "70"));
        lb_selectcolumn.Items.Add(new ListItem("License No", "71"));
        lb_selectcolumn.Items.Add(new ListItem("Tuition Fee Waiver", "72"));
        lb_selectcolumn.Items.Add(new ListItem("Insurance", "73"));
        lb_selectcolumn.Items.Add(new ListItem("Insurance Amount", "77"));
        lb_selectcolumn.Items.Add(new ListItem("Insurance InsBy", "78"));
        lb_selectcolumn.Items.Add(new ListItem("Insurance Nominee", "79"));
        lb_selectcolumn.Items.Add(new ListItem("Insurance NominRelation", "80"));
        lb_selectcolumn.Items.Add(new ListItem("Address", "18"));
        lb_selectcolumn.Items.Add(new ListItem("Street", "19"));
        lb_selectcolumn.Items.Add(new ListItem("City", "20"));
        lb_selectcolumn.Items.Add(new ListItem("State", "21"));
        lb_selectcolumn.Items.Add(new ListItem("Country", "22"));
        lb_selectcolumn.Items.Add(new ListItem("PinCode", "24"));
        lb_selectcolumn.Items.Add(new ListItem("Communication Address", "108"));
        lb_selectcolumn.Items.Add(new ListItem("Communication Street", "109"));
        lb_selectcolumn.Items.Add(new ListItem("Communication City", "110"));
        lb_selectcolumn.Items.Add(new ListItem("Communication State", "111"));
        lb_selectcolumn.Items.Add(new ListItem("Communication Country", "112"));
        lb_selectcolumn.Items.Add(new ListItem("Communication PinCode", "113"));
        lb_selectcolumn.Items.Add(new ListItem("Student Mobile", "23"));
        lb_selectcolumn.Items.Add(new ListItem("Alternate Mob No", "82"));
        lb_selectcolumn.Items.Add(new ListItem("Student EmailId", "56"));
        lb_selectcolumn.Items.Add(new ListItem("Parent Phone No", "25"));
        lb_selectcolumn.Items.Add(new ListItem("Curricular", "17"));
        lb_selectcolumn.Items.Add(new ListItem("Institute Name", "28"));
        lb_selectcolumn.Items.Add(new ListItem("Institute Address", "97"));
        lb_selectcolumn.Items.Add(new ListItem("X Medium", "98"));
        lb_selectcolumn.Items.Add(new ListItem("X11 Medium", "99"));
        lb_selectcolumn.Items.Add(new ListItem("Part1 Language", "29"));
        lb_selectcolumn.Items.Add(new ListItem("Part2 Language", "30"));
        lb_selectcolumn.Items.Add(new ListItem("Percentage", "100"));
        lb_selectcolumn.Items.Add(new ListItem("Secured Mark", "101"));
        lb_selectcolumn.Items.Add(new ListItem("Total Mark", "102"));
        lb_selectcolumn.Items.Add(new ListItem("Pass Month", "103"));
        lb_selectcolumn.Items.Add(new ListItem("Pass Year", "104"));
        lb_selectcolumn.Items.Add(new ListItem("Vocational Stream", "105"));
        lb_selectcolumn.Items.Add(new ListItem("Mark Priority", "106"));
        lb_selectcolumn.Items.Add(new ListItem("Cut Of Mark", "107"));
        lb_selectcolumn.Items.Add(new ListItem("University Name", "31"));
        lb_selectcolumn.Items.Add(new ListItem("State", "40"));
        lb_selectcolumn.Items.Add(new ListItem("Last TC No", "32"));
        lb_selectcolumn.Items.Add(new ListItem("Last TC Date", "33"));//delsii
        //lb_selectcolumn.Items.Add(new ListItem("12th MS", "34"));
        //lb_selectcolumn.Items.Add(new ListItem("Community Certificate No", "35"));
        //lb_selectcolumn.Items.Add(new ListItem("Diploma-Provisional No", "36"));
        //lb_selectcolumn.Items.Add(new ListItem("Diploma-Consolidate", "35"));
        //lb_selectcolumn.Items.Add(new ListItem("Diploma-Degree", "38"));
        //lb_selectcolumn.Items.Add(new ListItem("Diploma- No of Semester", "39"));
        //lb_selectcolumn.Items.Add(new ListItem("UG-Provisional No", "40"));
        //lb_selectcolumn.Items.Add(new ListItem("UG-Consolidate", "41"));
        //lb_selectcolumn.Items.Add(new ListItem("UG-Degree", "42"));
        //lb_selectcolumn.Items.Add(new ListItem("UG- No of Semester", "43"));
        //lb_selectcolumn.Items.Add(new ListItem("PG-Provisional No", "44"));
        //lb_selectcolumn.Items.Add(new ListItem("PG-Consolidate", "45"));
        //lb_selectcolumn.Items.Add(new ListItem("PG-Degree", "46"));
        //lb_selectcolumn.Items.Add(new ListItem("PG- No of Semester", "47"));
        lb_selectcolumn.Items.Add(new ListItem("A/C No", "114"));
        lb_selectcolumn.Items.Add(new ListItem("DebitCard No", "115"));
        lb_selectcolumn.Items.Add(new ListItem("IFSCCode", "116"));
        lb_selectcolumn.Items.Add(new ListItem("Bank Name", "117"));
        lb_selectcolumn.Items.Add(new ListItem("Bank Branch", "118"));
        lb_selectcolumn.Items.Add(new ListItem("Relative Name", "119"));
        lb_selectcolumn.Items.Add(new ListItem("RelationShip", "120"));
        lb_selectcolumn.Items.Add(new ListItem("Student/Staff", "121"));
        lb_selectcolumn.Items.Add(new ListItem("Admission Date", "36"));
        lb_selectcolumn.Items.Add(new ListItem("Enrollment Date", "37"));
        lb_selectcolumn.Items.Add(new ListItem("Join Date", "38"));
        lb_selectcolumn.Items.Add(new ListItem("CGPA", "125"));
        lb_selectcolumn.Items.Add(new ListItem("No Of Arrear", "126"));
        lb_selectcolumn.Items.Add(new ListItem("Refered By", "127"));
        lb_selectcolumn.Items.Add(new ListItem("Dob[DD]", "128"));
        lb_selectcolumn.Items.Add(new ListItem("Dob[MM]", "129"));
        lb_selectcolumn.Items.Add(new ListItem("Dob[YYYY]", "130"));
        lb_selectcolumn.Items.Add(new ListItem("Language", "131"));
        lb_selectcolumn.Items.Add(new ListItem("LanguageAcronym", "132"));
        lb_selectcolumn.Items.Add(new ListItem("Hall", "133"));
        lb_selectcolumn.Items.Add(new ListItem("Hall Acronym", "134"));
        lb_selectcolumn.Items.Add(new ListItem("Discontinue Date", "135"));
        lb_selectcolumn.Items.Add(new ListItem("Discontinue Reason", "136"));
        lb_selectcolumn.Items.Add(new ListItem("Mess Type", "137"));
        lb_selectcolumn.Items.Add(new ListItem("User Name", "138"));
        lb_selectcolumn.Items.Add(new ListItem("Board Of Study", "139"));
        string query = "select distinct MasterCode,MasterValue from CO_MasterValues where MasterCriteria='CertificateName' and CollegeCode='" + ddlcollege.SelectedItem.Value + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(query, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int y = 0; y < ds.Tables[0].Rows.Count; y++)
            {
                lb_selectcolumn.Items.Add(new System.Web.UI.WebControls.ListItem(Convert.ToString(ds.Tables[0].Rows[y]["MasterValue"]), Convert.ToString(ds.Tables[0].Rows[y]["MasterCode"])));
            }
        }
    }

    public void loadvalue()
    {
        if (colval == "1")
        {
            loadval = "Course_Name";
        }
        if (colval == "2")
        {
            loadval = "Dept_Name";
        }
        if (colval == "3")
        {
            loadval = "Batch_Year";
        }
        if (colval == "4")
        {
            loadval = "Current_Semester";
        }
        if (colval == "5")
        {
            loadval = "parent_name";
        }
        if (colval == "6")
        {
            loadval = "dob";
        }
        if (colval == "7")
        {
            loadval = "parent_occu";
        }
        if (colval == "8")
        {
            loadval = "mother_tongue";
        }
        if (colval == "9")
        {
            loadval = "religion";
        }
        if (colval == "10")
        {
            loadval = "citizen";
        }
        if (colval == "11")
        {
            loadval = "community";
        }
        if (colval == "12")
        {
            loadval = "caste";
        }
        if (colval == "13")
        {
            loadval = "TamilOrginFromAndaman";
        }
        if (colval == "14")
        {
            loadval = "visualhandy";
        }
        if (colval == "15")
        {
            loadval = "first_graduate";
        }
        if (colval == "16")
        {
            loadval = "seattype";
        }
        if (colval == "17")
        {
            loadval = "co_curricular";
        }
        if (colval == "18")
        {
            loadval = "parent_addressP";
        }
        if (colval == "19")
        {
            loadval = "Streetp";
        }
        if (colval == "20")
        {
            loadval = "cityp";
        }
        if (colval == "21")
        {
            loadval = "parent_statep";
        }
        if (colval == "22")
        {
            loadval = "Countryp";
        }
        if (colval == "23")
        {
            loadval = "Student_Mobile";
        }
        if (colval == "24")
        {
            loadval = "parent_pincodep";
        }
        if (colval == "25")
        {
            loadval = "parent_phnop";
        }
        if (colval == "26")
        {
            loadval = "MissionaryChild";
        }
        if (colval == "27")
        {
            loadval = "missionarydisc";
        }
        if (colval == "28")
        {
            loadval = "Institute_name";
        }
        if (colval == "29")
        {
            loadval = "Part1Language";
        }
        if (colval == "30")
        {
            loadval = "Part2Language";
        }
        if (colval == "31")
        {
            loadval = "University";
        }
        if (colval == "40")
        {
            loadval = "uni_state";

        }

        if (colval == "32")
        {
            loadval = "LastTCNo";
        }
        if (colval == "33")
        {
            loadval = "LastTCDate";
        }
        if (colval == "35")
        {
            loadval = "ElectionID_No";
        }
        if (colval == "34")
        {
            loadval = "Twelth_CertNo";
        }
        if (colval == "35")
        {
            loadval = "CommunityNo";
        }
        if (colval == "36")
        {
            loadval = "DeplomProv_CertNo";
        }
        if (colval == "37")
        {
            loadval = "DeplomConsolidate_CertNo";
        }
        if (colval == "38")
        {
            loadval = "DeplomDegree_CertNo";
        }
        if (colval == "39")
        {
            loadval = "type_semester";
        }
        if (colval == "40")
        {
            loadval = "UGProv_CertNo";
        }
        if (colval == "41")
        {
            loadval = "UGConsolidate_CertNo";
        }
        if (colval == "42")
        {
            loadval = "UGDegree_CertNo";
        }
        if (colval == "43")
        {
            loadval = "type_semester";
        }
        if (colval == "44")
        {
            loadval = "PGProv_CertNo";
        }
        if (colval == "45")
        {
            loadval = "PGConsolidate_CertNo";
        }
        if (colval == "46")
        {
            loadval = "PGDegree_CertNo";
        }
        if (colval == "47")
        {
            loadval = "type_semester";
        }
        if (colval == "48")
        {
            loadval = "CampusReq";
        }
        if (colval == "49")
        {
            loadval = "handy";
        }
        if (colval == "50")
        {
            loadval = "DistinctSport";
        }
        if (colval == "51")
        {
            loadval = "islearningdis";
        }
        if (colval == "52")
        {
            loadval = "isdisabledisc";
        }
        if (colval == "53")
        {
            loadval = "isdisable";
        }
        if (colval == "54")
        {
            loadval = "stud_name";
        }
        if (colval == "55")
        {
            loadval = "Roll_no";
        }
        if (colval == "56")
        {
            loadval = "StuPer_Id";
        }
        if (colval == "57")
        {
            loadval = "reg_no";
        }
        if (colval == "58")
        {
            loadval = "roll_admit";
        }
        if (colval == "59")
        {
            loadval = "app_formno";
        }
        if (colval == "60")
        {
            loadval = "sections";
        }
        if (colval == "61")
        {
            loadval = "sex";
        }
        if (colval == "62")
        {
            loadval = "bldgrp";
        }
        if (colval == "63")
        {
            loadval = "stud_type";
        }
        if (colval == "64")
        {
            loadval = "IsExService";
        } if (colval == "65")
        {
            loadval = "CampusReq";
        }
        if (colval == "66")
        {
            loadval = "isdonar";
        }
        if (colval == "67")
        {
            loadval = "ReserveCategory";
        }
        if (colval == "68")
        {
            loadval = "EconBackword";
        }
        if (colval == "69")
        {
            loadval = "parentoldstud";
        }
        if (colval == "70")
        {
            loadval = "IsDrivingLic";
        }
        if (colval == "71")
        {
            loadval = "Driving_details";
        }
        if (colval == "72")
        {
            loadval = "tutionfee_waiver";
        }
        if (colval == "73")
        {
            loadval = "IsInsurance";
        }
        if (colval == "74")
        {
            loadval = "ExsRank";
        }
        if (colval == "75")
        {
            loadval = "ExSPlace";
        }
        if (colval == "76")
        {
            loadval = "ExsNumber";
        }
        if (colval == "77")
        {
            loadval = "Insurance_Amount";
        }
        if (colval == "78")
        {
            loadval = "Insurance_InsBy";
        }
        if (colval == "79")
        {
            loadval = "Insurance_Nominee";
        }
        if (colval == "80")
        {
            loadval = "Insurance_NominRelation";
        }
        if (colval == "81")
        {
            loadval = "date_applied";
        }
        if (colval == "82")
        {
            loadval = "alter_mobileno";
        }
        if (colval == "83")
        {
            loadval = "SubCaste";
        }
        if (colval == "84")
        {
            loadval = "parent_income";
        }
        if (colval == "85")
        {
            loadval = "parentF_Mobile";
        }
        if (colval == "86")
        {
            loadval = "emailp";
        }
        if (colval == "87")
        {
            loadval = "mother";
        }
        if (colval == "88")
        {
            loadval = "mIncome";
        }
        if (colval == "89")
        {
            loadval = "parentM_Mobile";
        }
        if (colval == "90")
        {
            loadval = "emailM";
        }
        if (colval == "91")
        {
            loadval = "guardian_name";
        }
        if (colval == "92")
        {
            loadval = "guardian_mobile";
        }
        if (colval == "93")
        {
            loadval = "emailg";
        }
        if (colval == "94")
        {
            loadval = "place_birth";
        }
        if (colval == "95")
        {
            loadval = "Aadharcard_no";
        }
        if (colval == "96")
        {
            loadval = "motherocc";
        }
        if (colval == "97")
        {
            loadval = "instaddress";
        }
        if (colval == "98")
        {
            loadval = "Xmedium";
        }
        if (colval == "99")
        {
            loadval = "medium";
        }
        if (colval == "100")
        {
            loadval = "percentage";
        }
        if (colval == "101")
        {
            loadval = "securedmark";
        }
        if (colval == "102")
        {
            loadval = "totalmark";
        }
        if (colval == "103")
        {
            loadval = "passmonth";
        }
        if (colval == "104")
        {
            loadval = "passyear";
        }
        if (colval == "105")
        {
            loadval = "Vocational_stream";
        }
        if (colval == "106")
        {
            loadval = "markPriority";
        }
        if (colval == "107")
        {
            loadval = "Cut_Of_Mark";
        }
        if (colval == "108")
        {
            loadval = "parent_addressc";
        }
        if (colval == "109")
        {
            loadval = "Streetc";
        }
        if (colval == "110")
        {
            loadval = "cityc";
        }
        if (colval == "111")
        {
            loadval = "parent_statec";
        }
        if (colval == "112")
        {
            loadval = "Countryc";
        }
        if (colval == "113")
        {
            loadval = "parent_pincodec";
        }
        if (colval == "114")
        {
            loadval = "AccNo";
        }
        if (colval == "115")
        {
            loadval = "DebitCardNo";
        }
        if (colval == "116")
        {
            loadval = "IFSCCode";
        }
        if (colval == "117")
        {
            loadval = "BankName";
        }
        if (colval == "118")
        {
            loadval = "Branch";
        }
        if (colval == "119")
        {
            loadval = "name_roll";
        }
        if (colval == "120")
        {
            loadval = "relationship";
        }
        if (colval == "121")
        {
            loadval = "isstaff";
        }
        if (colval == "122")
        {
            loadval = "Boarding";
        }
        if (colval == "123")
        {
            loadval = "vehid";
        }
        if (colval == "36")//delsii
        {
            loadval = "CONVERT(varchar(10), r.Adm_Date,103)Adm_Date";
        }
        if (colval == "37")
        {
            loadval = "CONVERT(varchar(10), a.enrollment_confirm_date,103)enrollment_confirm_date";
        }
        if (colval == "38")//delsii
        {
            loadval = "CONVERT(varchar(10), r.Adm_Date,103)Adm_Date";
        }
        if (colval == "125")
        {
            loadval = "CGPA";
        }
        if (colval == "126")
        {
            loadval = "No_of_arrear";
        }
        if (colval == "128")
        {
            loadval = "DATEPART (day,dob) 'day";
        }
        if (colval == "129")
        {
            loadval = "DATEPART(MONTH, dob) 'Month'";
        }
        if (colval == "130")
        {
            loadval = "DATEPART(YEAR, dob) 'Year'";
        }


        if (Convert.ToInt32(colval) > 123)
        {
            loadval = d2.GetFunction("select distinct MasterCode,MasterValue from CO_MasterValues where MasterCriteria='CertificateName' and CollegeCode='" + ddlcollege.SelectedItem.Value + "' and MasterCode='" + colval + "' ");
        }
    }

    public void loadlcolumns()
    {
        try
        {
            string linkname = "StudentStrengthCommon column order settings";
            DataSet dscol = new DataSet();
            string selcol = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + ddlcollege.SelectedItem.Value + "' ";
            dscol.Clear();
            dscol = d2.select_method_wo_parameter(selcol, "Text");
            if (dscol.Tables.Count > 0)
            {
                if (dscol.Tables[0].Rows.Count > 0)
                {
                    for (int col = 0; col < dscol.Tables[0].Rows.Count; col++)
                    {
                        string value = Convert.ToString(dscol.Tables[0].Rows[col]["LinkValue"]);
                        string[] valuesplit = value.Split(',');
                        if (valuesplit.Length > 0)
                        {
                            for (int k = 0; k < valuesplit.Length; k++)
                            {
                                colval = Convert.ToString(valuesplit[k]);
                                loadtext();
                                lb_column1.Items.Add(new ListItem(loadval, colval));
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }

    public void savecolumnorder()
    {
        string columnvalue = string.Empty;
        DataSet dscol = new DataSet();
        string linkname = Convert.ToString(ddl_coltypeadd.SelectedItem.Text);
        string val = string.Empty;
        for (int j = 0; j < lb_column1.Items.Count; j++)
        {
            val = lb_column1.Items[j].Value;
            if (columnvalue == "")
            {
                columnvalue = val;
            }
            else
            {
                columnvalue = columnvalue + ',' + val;
            }
        }
        string clsinsert = " if exists(select * from New_InsSettings where LinkName='" + linkname + "' and college_code='" + ddlcollege.SelectedItem.Value + "'  ) update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "'  and college_code='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkname + "','" + columnvalue + "','" + usercode + "','" + ddlcollege.SelectedItem.Value + "')";
        int clsupdate = d2.update_method_wo_parameter(clsinsert, "Text");
    }

    public void loadtext()//delsii
    {
        if (colval == "1")
        {
            loadval = "Course";
            printval = "Course_Name";
        }
        if (colval == "2")
        {
            loadval = "Branch";
            printval = "Dept_Name";
        }
        if (colval == "3")
        {
            loadval = "Batch";
            printval = "Batch_Year";
        }
        if (colval == "4")
        {
            loadval = "Semester";
            printval = "Current_Semester";
        }
        if (colval == "5")
        {
            loadval = "Father Name";
            printval = "parent_name";
        }
        if (colval == "6")
        {
            loadval = "DOB";
            printval = "dob";
        }
        if (colval == "7")
        {
            loadval = "Parent Occupation";
            printval = "parent_occu";
        }
        if (colval == "8")
        {
            loadval = "Mother Tongue";
            printval = "mother_tongue";
        }
        if (colval == "9")
        {
            loadval = "Religion";
            printval = "religion";
        }
        if (colval == "10")
        {
            loadval = "Citizen";
            printval = "citizen";
        }
        if (colval == "11")
        {
            loadval = "Community";
            printval = "community";
        }
        if (colval == "12")
        {
            loadval = "Caste";
            printval = "caste";
        }
        if (colval == "13")
        {
            loadval = "TamilOrginFromAndaman";
            printval = "TamilOrginFromAndaman";
        }
        if (colval == "14")
        {
            loadval = "VisualHandy";
            printval = "visualhandy";
        }
        if (colval == "15")
        {
            loadval = "First Graduate";
            printval = "first_graduate";
        }
        if (colval == "16")
        {
            loadval = "SeatType";
            printval = "seattype";
        }
        if (colval == "17")
        {
            loadval = "Curricular";
            printval = "co_curricular";
        }
        if (colval == "18")
        {
            loadval = "Address";
            printval = "parent_addressP";
        }
        if (colval == "19")
        {
            loadval = "Street";
            printval = "Streetp";
        }
        if (colval == "20")
        {
            loadval = "City";
            printval = "cityp";
        }
        if (colval == "21")
        {
            loadval = "State";
            printval = "parent_statep";
        }
        if (colval == "22")
        {
            loadval = "Country";
            printval = "Countryp";
        }
        if (colval == "23")
        {
            loadval = "Student Mobile";
            printval = "Student_Mobile";
        }
        if (colval == "24")
        {
            loadval = "PinCode";
            printval = "parent_pincodep";
        }
        if (colval == "25")
        {
            loadval = "Parent Phone No";
            printval = "parent_phnop";
        }
        if (colval == "26")
        {
            loadval = "MissionaryChild";
            printval = "MissionaryChild";
        }
        if (colval == "27")
        {
            loadval = "missionarydisc";
            printval = "missionarydisc";
        }
        if (colval == "28")
        {
            loadval = "Institute Name";
            printval = "Institute_name";
        }
        if (colval == "29")
        {
            loadval = "Part1 Language";
            printval = "Part1Language";
        }
        if (colval == "30")
        {
            loadval = "Part2 Language";
            printval = "Part2Language";
        }
        if (colval == "31")
        {
            loadval = "University Name";
            printval = "University";
        }
        if (colval == "40")
        {
            loadval = "State";
            printval = "uni_state";

        }
        if (colval == "32")
        {
            loadval = "LastTC No";
            printval = "LastTCNo";
        }
        if (colval == "33")
        {
            loadval = "LastTC Date";
            printval = "LastTCDate";
        }
        if (colval == "34")
        {
            loadval = "HostelName";
            printval = "HostelName";
        }
        if (colval == "35")
        {
            loadval = "Voter ID";
            printval = "ElectionID_No";
        }
        if (colval == "36")
        {
            loadval = "Diploma-Provisional No";
        }
        if (colval == "37")
        {
            loadval = "Diploma-Consolidate";
        }
        if (colval == "38")
        {
            loadval = "Diploma-Degree";
        }
        if (colval == "39")
        {
            loadval = "Diploma- No of Semester";
        }
        if (colval == "40")
        {
            loadval = "UG-Provisional No";
        }
        if (colval == "41")
        {
            loadval = "UG-Consolidate";
        }
        if (colval == "42")
        {
            loadval = "UG-Degree";
        }
        if (colval == "43")
        {
            loadval = "UG- No of Semester";
        }
        if (colval == "44")
        {
            loadval = "PG-Provisional No";
        }
        if (colval == "45")
        {
            loadval = "PG-Consolidate";
        }
        if (colval == "46")
        {
            loadval = "PG-Degree";
        }
        if (colval == "47")
        {
            loadval = "PG- No of Semester";
        }
        if (colval == "48")
        {
            loadval = "Residency";
            printval = "CampusReq";
        }
        if (colval == "49")
        {
            loadval = "Physically challange";
            printval = "handy";
        }
        if (colval == "50")
        {
            printval = "DistinctSport";
            loadval = "Sports";
        }
        if (colval == "51")
        {
            printval = "islearningdis";
            loadval = "Learning Disability";
        }
        if (colval == "52")
        {
            printval = "isdisabledisc";
            loadval = "Other Disability";
        }
        if (colval == "53")
        {
            loadval = "IsDisable";
            printval = "isdisable";
        }
        if (colval == "54")
        {
            loadval = "Student Name";
            printval = "stud_name";
        }
        if (colval == "55")
        {
            loadval = "Roll No";
            printval = "Roll_no";
        }
        if (colval == "56")
        {
            loadval = "Student EmailId";
            printval = "StuPer_Id";
        }
        if (colval == "57")
        {
            loadval = "Reg No";
            printval = "reg_no";
        }
        if (colval == "58")
        {
            loadval = "Admission No";
            printval = "roll_admit";
        }
        if (colval == "59")
        {
            loadval = "Application No";
            printval = "app_formno";
        }
        if (colval == "60")
        {
            loadval = "Section";
            printval = "sections";
        }
        if (colval == "61")
        {
            loadval = "Gender";
            printval = "sex";
        }
        if (colval == "62")
        {
            loadval = "Blood Group";
            printval = "bldgrp";
        }
        if (colval == "63")
        {
            loadval = "Student Type";
            printval = "stud_type";
        }
        if (colval == "64")
        {
            loadval = "Ex-serviceman";
            printval = "IsExService";
        }
        if (colval == "65")
        {
            loadval = "Hostel accommodation";
            printval = "CampusReq";
        }
        if (colval == "66")
        {
            loadval = "Blood Donor";
            printval = "isdonar";
        }
        if (colval == "67")
        {
            loadval = "Reserved Caste";
            printval = "ReserveCategory";
        }
        if (colval == "68")
        {
            loadval = "Economic Backward";
            printval = "EconBackword";
        }
        if (colval == "69")
        {
            loadval = "Parents Old Student";
            printval = "parentoldstud";
        }
        if (colval == "70")
        {
            loadval = "Driving License";
            printval = "IsDrivingLic";
        }
        if (colval == "71")
        {
            loadval = "License No";
            printval = "Driving_details";
        }
        if (colval == "72")
        {
            loadval = "Tuition Fee Waiver";
            printval = "tutionfee_waiver";
        }
        if (colval == "73")
        {
            loadval = "Insurance";
            printval = "IsInsurance";
        }
        if (colval == "74")
        {
            loadval = "Rank";
            printval = "ExsRank";
        }
        if (colval == "75")
        {
            loadval = "Place";
            printval = "ExSPlace";
        }
        if (colval == "76")
        {
            loadval = "Number";
            printval = "ExsNumber";
        }
        if (colval == "77")
        {
            loadval = "Insurance Amount";
            printval = "Insurance_Amount";
        }
        if (colval == "78")
        {
            loadval = "Insurance InsBy";
            printval = "Insurance_InsBy";
        }
        if (colval == "79")
        {
            loadval = "Insurance Nominee";
            printval = "Insurance_Nominee";
        }
        if (colval == "80")
        {
            loadval = "Insurance NominRelation";
            printval = "Insurance_NominRelation";
        }
        if (colval == "81")
        {
            loadval = "Applied Date";
            printval = "date_applied";
        }
        if (colval == "82")
        {
            loadval = "Alternate Mob No";
            printval = "alter_mobileno";
        }
        if (colval == "83")
        {
            loadval = "Sub Caste";
            printval = "SubCaste";
        }
        if (colval == "84")
        {
            loadval = "Father Income";
            printval = "parent_income";
        }
        if (colval == "85")
        {
            loadval = "Father Mob No";
            printval = "parentF_Mobile";
        }
        if (colval == "86")
        {
            loadval = "Father EmailId";
            printval = "emailp";
        }
        if (colval == "87")
        {
            loadval = "Mother";
            printval = "mother";
        }
        if (colval == "88")
        {
            loadval = "Mother Income";
            printval = "mIncome";
        }
        if (colval == "89")
        {
            loadval = "Mother Mob No";
            printval = "parentM_Mobile";
        }
        if (colval == "90")
        {
            loadval = "Mother EmailId";
            printval = "emailM";
        }
        if (colval == "91")
        {
            loadval = "Guardian Name";
            printval = "guardian_name";
        }
        if (colval == "92")
        {
            loadval = "Guardian Mob No";
            printval = "guardian_mobile";
        }
        if (colval == "93")
        {
            loadval = "Guardian Email Id";
            printval = "emailg";
        }
        if (colval == "94")
        {
            loadval = "Place Of Birth";
            printval = "place_birth";
        }
        if (colval == "95")
        {
            loadval = "Adhaar Card No";
            printval = "Aadharcard_no";
        }
        if (colval == "96")
        {
            loadval = "Mother Occupation";
            printval = "motherocc";
        }
        if (colval == "97")
        {
            loadval = "Institution Address";
            printval = "instaddress";
        }
        if (colval == "98")
        {
            loadval = "X medium";
            printval = "Xmedium";
        }
        if (colval == "99")
        {
            loadval = "X11 Medium";
            printval = "medium";
        }
        if (colval == "100")
        {
            loadval = "Percentage";
            printval = "percentage";
        }
        if (colval == "101")
        {
            loadval = "Secured Mark";
            printval = "securedmark";
        }
        if (colval == "102")
        {
            printval = "totalmark";
            loadval = "Total Mark";
        }
        if (colval == "103")
        {
            loadval = "Pass Month";
            printval = "passmonth";
        }
        if (colval == "104")
        {
            loadval = "Pass Year";
            printval = "passyear";
        }
        if (colval == "105")
        {
            loadval = "Vocational Stream";
            printval = "Vocational_stream";
        }
        if (colval == "106")
        {
            loadval = "Mark Priority";
            printval = "markPriority";
        }
        if (colval == "107")
        {
            loadval = "Cut Of Mark";
            printval = "Cut_Of_Mark";
        }
        if (colval == "108")
        {
            loadval = "Communication Address";
            printval = "parent_addressc";
        }
        if (colval == "109")
        {
            loadval = "Communication Street";
            printval = "Streetc";
        }
        if (colval == "110")
        {
            loadval = "Communication City";
            printval = "cityc";
        }
        if (colval == "111")
        {
            loadval = "Communication State";
            printval = "parent_statec";
        }
        if (colval == "112")
        {
            loadval = "Communication Country";
            printval = "Countryc";
        }
        if (colval == "113")
        {
            printval = "parent_pincodec";
            loadval = "Communication PinCode";
        }
        if (colval == "114")
        {
            loadval = "A/C No";
            printval = "AccNo";
        }
        if (colval == "115")
        {
            printval = "DebitCardNo";
            loadval = "DebitCard No";
        }
        if (colval == "116")
        {
            loadval = "IFSCCode";
            printval = "IFSCCode";
        }
        if (colval == "117")
        {
            loadval = "Bank Name";
            printval = "BankName";
        }
        if (colval == "118")
        {
            printval = "Branch";
            loadval = "Department";
        }
        if (colval == "119")
        {
            printval = "name_roll";
            loadval = "Relation Name";
        }
        if (colval == "120")
        {
            printval = "relationship";
            loadval = "Relationship";
        }
        if (colval == "121")
        {
            printval = "isstaff";
            loadval = "Staff/Student";
        }
        if (colval == "122")
        {
            printval = "Boarding";
            loadval = "Boarding";
        }
        if (colval == "123")
        {
            printval = "vehid";
            loadval = "Vehicle Id";
        }
        if (colval == "43")
        {
            printval = "Mode";
            loadval = "Mode";
        }
        if (colval == "36")
        {
            printval = "Adm_Date";
            loadval = "Admission Date";
        }
        if (colval == "37")
        {
            printval = "enrollment_confirm_date";
            loadval = "Enrollment Date";
        }
        if (colval == "38")
        {
            printval = "Adm_Date";
            loadval = "Join Date";
        }
        if (colval == "125")
        {
            printval = "CGPA";
            loadval = "CGPA";
        }
        if (colval == "126")
        {
            printval = "noofarrear";
            loadval = "No of arrear";
        }
        if (colval == "127")//added
        {
            printval = "referby";
            loadval = "Refered By";
        }
        if (colval == "128")
        {
            loadval = "Dob[DD]";
            printval = "day";
        }
        if (colval == "129")
        {
            loadval = "Dob[MM]";
            printval = "Month";
        }
        if (colval == "130")
        {
            loadval = "Dob[YYYY]";
            printval = "Year";
        }
        if (colval == "131")
        {
            loadval = "Language";
            printval = "subject_name";
        }
        if (colval == "132")
        {
            loadval = "Language Acronym";
            printval = "acronym";
        }
        if (colval == "133")
        {
            loadval = "Hall";
            printval = "building_description";
        }
        if (colval == "134")
        {
            loadval = "Hall Acronym";
            printval = "Building_Acronym";
        }
        if (colval == "135")
        {
            loadval = "Discontinue Date";
            printval = "Discontinue_Date";
        }
        if (colval == "136")
        {
            loadval = "Discontinue Reason";
            printval = "Reason";
        }
        if (colval == "137")
        {
            loadval = "Mess Type";
            printval = "StudMessType";
        }
        if (colval == "138")
        {
            loadval = "User Name";
            printval = "entryusercode";
        }
        if (colval == "139")
        {
            loadval = "Board Of Study";
            printval = "board";
        }
        if (Convert.ToInt32(colval) > 140)
        {
            loadval = d2.GetFunction("select distinct MasterValue from CO_MasterValues where MasterCriteria='CertificateName' and CollegeCode='" + ddlcollege.SelectedItem.Value + "' and MasterCode='" + colval + "' ");
            printval = d2.GetFunction("select distinct MasterCode,MasterValue from CO_MasterValues where MasterCriteria='CertificateName' and CollegeCode='" + ddlcollege.SelectedItem.Value + "' and MasterCode='" + colval + "' ");
        }
    }

    public void btn_viewsprd2_Click(object sender, EventArgs e)
    {
        string value = d2.GetFunction("select value from Master_Settings where settings ='StudentAdmissionRegister' and usercode ='" + usercode + "'");
        if (value == "1")
        {
            loadlcolumns1();
            string val = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='StudentStrengthPDF column order settings'");
            if (val != "")
            {
                stud_detailsettingbased();
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Set The Column Order";
                return;
            }
        }
        else
        {
            stud_detail();
        }
    }

    public void stud_detail()
    {
        try
        {
            double page2col = 0;
            double binddatatb = 0;
            string activerow = string.Empty;
            string checkvalue = string.Empty;
            DataSet ds1 = new DataSet();
            if (Fpspread2.Visible == true)
            {
                Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
                System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
                System.Drawing.Font Fontbolda = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
                System.Drawing.Font Fontbold16 = new System.Drawing.Font("Book Antiqua", 16, FontStyle.Bold);
                System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Regular);
                System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
                System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
                System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
                System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
                System.Drawing.Font Fontsmall1bold = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
                System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);
                System.Drawing.Font Fontboldu = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Underline);
                Gios.Pdf.PdfPage mypdfpage;
                PdfTextArea collinfo1;
                int d = 0;
                for (int i = 1; i < Fpspread2.Sheets[0].RowCount; i++)
                {
                    checkvalue = Convert.ToString(Fpspread2.Sheets[0].Cells[i, 1].Value);
                    if (checkvalue == "1")
                    {
                        d = 1;
                        //activerow = Fpspread2.ActiveSheetView.ActiveRow.ToString();
                        string appno = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(i), 2].Text);
                        int coltop = 0;
                        string Collvalue = string.Empty;
                        mypdfpage = mydoc.NewPage();
                        #region
                        string strquery = "Select * from Collinfo where college_code=" + Session["collegecode"].ToString() + "";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(strquery, "Text");
                        strquery = "Select * from Collinfo where college_code=" + Session["collegecode"].ToString() + "";
                        ds1.Clear();
                        ds1 = d2.select_method_wo_parameter(strquery, "Text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            string collinfo = Convert.ToString(ds1.Tables[0].Rows[0]["collname"]);
                            string university = Convert.ToString(ds1.Tables[0].Rows[0]["university"]);
                            string affliatedby = Convert.ToString(ds1.Tables[0].Rows[0]["affliatedby"]);
                            string address1 = ds1.Tables[0].Rows[0]["Address1"].ToString();
                            string address2 = ds1.Tables[0].Rows[0]["Address2"].ToString();
                            string address3 = ds1.Tables[0].Rows[0]["Address3"].ToString();
                            string district = ds1.Tables[0].Rows[0]["district"].ToString();
                            string state = ds1.Tables[0].Rows[0]["State"].ToString();
                            string pincode = ds1.Tables[0].Rows[0]["Pincode"].ToString();
                            string phone = ds1.Tables[0].Rows[0]["Phoneno"].ToString();
                            string fax = ds1.Tables[0].Rows[0]["Faxno"].ToString();
                            string email = ds1.Tables[0].Rows[0]["Email"].ToString();
                            string website = ds1.Tables[0].Rows[0]["Website"].ToString();
                            if (collinfo != "")
                            {
                                collinfo1 = new PdfTextArea(Fontbold16, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 25, mydoc.PageWidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds1.Tables[0].Rows[0]["collname"].ToString() + "");
                                mypdfpage.Add(collinfo1);
                            }
                            //if (university != "")
                            //{
                            //    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 30, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds1.Tables[0].Rows[0]["university"].ToString() + "");
                            //    mypdfpage.Add(collinfo1);
                            //}
                            //else if (affliatedby != "")
                            //{
                            //    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 30, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds1.Tables[0].Rows[0]["affliatedby"].ToString() + "");
                            //    mypdfpage.Add(collinfo1);
                            //}
                            if (address1 != "" || address1 != "" || address3 != "")
                            {
                                if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                                {
                                    Collvalue = address1;
                                }
                                if (address2.Trim() != "" && address2 != null && address2.Length > 1)
                                {
                                    if (Collvalue.Trim() != "" && Collvalue != null)
                                    {
                                        Collvalue = Collvalue + ',' + ' ' + address2;
                                    }
                                    else
                                    {
                                        Collvalue = address2;
                                    }
                                }
                                if (address3.Trim() != "" && address3 != null && address3.Length > 1)
                                {
                                    if (Collvalue.Trim() != "" && Collvalue != null)
                                    {
                                        Collvalue = Collvalue + ',' + ' ' + address3;
                                    }
                                    else
                                    {
                                        Collvalue = address3;
                                    }
                                }
                                collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 45, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                mypdfpage.Add(collinfo1);
                            }
                            else if (address3 != "")
                            {
                                string address11 = ds1.Tables[0].Rows[0]["Address3"].ToString();
                                if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                                {
                                    Collvalue = address1;
                                }
                                collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 45, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                mypdfpage.Add(collinfo1);
                            }
                            if (district != "" || pincode != "")
                            {
                                if (district.Trim() != "" && district != null && district.Length > 1)
                                {
                                    Collvalue = district;
                                }
                                if (pincode.Trim() != "" && pincode != null && pincode.Length > 1)
                                {
                                    if (Collvalue.Trim() != "" && Collvalue != null)
                                    {
                                        Collvalue = Collvalue + ' ' + '-' + ' ' + pincode;
                                    }
                                    else
                                    {
                                        Collvalue = pincode;
                                    }
                                }
                                collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 60, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                mypdfpage.Add(collinfo1);
                            }
                            if (phone != "" || fax != "")
                            {
                                if (phone.Trim() != "" && phone != null && phone.Length > 1)
                                {
                                    Collvalue = "Phone :" + phone;
                                }
                                if (fax.Trim() != "" && fax != null && fax.Length > 1)
                                {
                                    if (Collvalue.Trim() != "" && Collvalue != null)
                                    {
                                        Collvalue = Collvalue + " , Fax : " + fax;
                                    }
                                    else
                                    {
                                        Collvalue = "Fax :" + fax;
                                    }
                                }
                                //collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 75, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                //mypdfpage.Add(collinfo1);
                            }
                            if (email != "" || website != "")
                            {
                                if (email.Trim() != "" && email != null && email.Length > 1)
                                {
                                    Collvalue = "Email :" + email;
                                }
                                if (website.Trim() != "" && website != null && website.Length > 1)
                                {
                                    if (Collvalue.Trim() != "" && Collvalue != null)
                                    {
                                        Collvalue = Collvalue + " , Web Site : " + website;
                                    }
                                    else
                                    {
                                        Collvalue = "Web Site :" + website;
                                    }
                                }
                                //collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 90, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                //mypdfpage.Add(collinfo1);
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                            {
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                mypdfpage.Add(LogoImage, 70, 20, 450);
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                            {
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                mypdfpage.Add(LogoImage, 480, 20, 450);
                            }
                        }
                        #endregion
                        DataView dv = new DataView();
                        //string appno = d2.GetFunction("select App_No from Registration where Roll_No='" + rollno + "'");
                        string sql = string.Empty;
                        string course = d2.GetFunction("select distinct s.course_entno  from Stud_prev_details s where  s.app_no = '" + appno + "' order by course_entno desc");
                        // select distinct s.course_entno  from Stud_prev_details s,perv_marks_history h where s.course_entno =h.course_entno and s.app_no = '" + appno + "'");
                        // string sql = "select a.sex,a.mother,a.MissionaryChild,a.parent_phnoc,a.parent_pincodec,a.Student_Mobile,a.Countryc,a.parent_statec,a.Cityc,a.Streetc,a.parent_addressC,a.co_curricular,a.citizen,a.visualhandy,a.first_graduate,a.TamilOrginFromAndaman ,a.seattype,a.mother_tongue,a.parent_occu,a.caste,a.community,a.religion,a.parent_name,r.Roll_No,r.Stud_Name,r.Batch_Year,r.Current_Semester,c.Course_Name,Dt.Dept_Name,r.Sections,CONVERT(VARCHAR(11),dob,103) as dob from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='1' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and  r.Roll_no='" + rollno + "'";
                        if (ddl_status.SelectedItem.Text.ToUpper() == "APPLIED")//delsi0204 changed a.current_semester and a.app_no to r.current_semester andra.app_no
                            sql = "SELECT a.batch_year,a.degree_code, r.Current_Semester ,A.App_No,Roll_No,Roll_Admit, Reg_No,a.Stud_Name,Course_Name+'-'+Dept_Name Course,CONVERT(varchar(10), Adm_Date,103)as Adm_Date, CONVERT(VARCHAR(11),DOB,103) as DOB,CASE WHEN Sex = 0 THEN 'Male' ELSE 'Female' END Sex,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.SeatType = T.TextCode AND L.App_No = A.App_No) Quota,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.BldGrp = T.TextCode AND L.App_No = A.App_No) BloodGroup,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Citizen = T.TextCode AND L.App_No = A.App_No) Citizen,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Religion = T.TextCode AND L.App_No = A.App_No) Religion,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Community = T.TextCode AND L.App_No = A.App_No) Community,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Caste = T.TextCode AND L.App_No = A.App_No) Caste,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Mother_Tongue = T.TextCode AND L.App_No = A.App_No) MotherTongue,ISNULL(IDMark,'') IDMark,ISNULL(Parent_Name,'') Parent_Name,ISNULL(Mother,'') Mother, Parent_AddressC,StreetC,CityC,Parent_AddressP,StreetP,CityP,CASE WHEN ISNUMERIC(DistrictP) = 1 THEN (SELECT ISNULL(TextVal,'') FROM Textvaltable WHERE Textcode = A.DistrictP) ELSE ISNULL(DistrictP,'') END DistrictP,CASE WHEN ISNUMERIC(DistrictC) = 1 THEN (SELECT ISNULL(TextVal,'') FROM Textvaltable WHERE Textcode = A.DistrictC) ELSE ISNULL(DistrictC,'') END DistrictC,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Parent_StateC = T.TextCode AND L.App_No = A.App_No) StateC,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Parent_StateP = T.TextCode AND L.App_No = A.App_No) StateP,CASE WHEN ISNUMERIC(CountryP) = 1 THEN (SELECT ISNULL(TextVal,'') FROM Textvaltable WHERE Textcode = A.CountryP) ELSE ISNULL(CountryP,'') END CountryP,CASE WHEN ISNUMERIC(CountryC) = 1 THEN (SELECT ISNULL(TextVal,'') FROM Textvaltable WHERE Textcode = A.CountryC) ELSE ISNULL(CountryC,'') END CountryC,ISNULL(Parent_PinCodeC,'') PinC,ISNULL(Parent_PinCodeP,'') PinP,ISNULL(Parent_PhNoC,'') PhNoC,ISNULL(Parent_PhNoP,'') PhNoP, ISNULL(Student_Mobile,'') Student_Mobile,ISNULL(StuPer_ID,'') StuPer_ID FROM  applyn a left join Registration r on a.app_no = r.app_no INNER JOIN Degree G ON G.Degree_Code = r.Degree_Code AND G.College_Code = a.College_Code INNER JOIN Course C ON C.Course_ID = G.Course_ID AND C.College_Code = G.College_Code INNER JOIN Department D ON D.Dept_Code = G.Dept_Code AND D.College_Code = G.College_Code where a.app_no='" + appno + "'";
                        else
                            sql = "SELECT r.batch_year,r.degree_code, r.Current_Semester ,A.App_No,Roll_No,Roll_Admit, Reg_No,R.Stud_Name,Course_Name+'-'+Dept_Name Course,CONVERT(varchar(10), Adm_Date,103)as Adm_Date, CONVERT(VARCHAR(11),DOB,103) as DOB,CASE WHEN Sex = 0 THEN 'Male' ELSE 'Female' END Sex,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.SeatType = T.TextCode AND L.App_No = A.App_No) Quota,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.BldGrp = T.TextCode AND L.App_No = A.App_No) BloodGroup,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Citizen = T.TextCode AND L.App_No = A.App_No) Citizen,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Religion = T.TextCode AND L.App_No = A.App_No) Religion,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Community = T.TextCode AND L.App_No = A.App_No) Community,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Caste = T.TextCode AND L.App_No = A.App_No) Caste,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Mother_Tongue = T.TextCode AND L.App_No = A.App_No) MotherTongue,ISNULL(IDMark,'') IDMark,ISNULL(Parent_Name,'') Parent_Name,ISNULL(Mother,'') Mother, Parent_AddressC,StreetC,CityC,Parent_AddressP,StreetP,CityP,CASE WHEN ISNUMERIC(DistrictP) = 1 THEN (SELECT ISNULL(TextVal,'') FROM Textvaltable WHERE Textcode = A.DistrictP) ELSE ISNULL(DistrictP,'') END DistrictP,CASE WHEN ISNUMERIC(DistrictC) = 1 THEN (SELECT ISNULL(TextVal,'') FROM Textvaltable WHERE Textcode = A.DistrictC) ELSE ISNULL(DistrictC,'') END DistrictC,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Parent_StateC = T.TextCode AND L.App_No = A.App_No) StateC,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Parent_StateP = T.TextCode AND L.App_No = A.App_No) StateP,CASE WHEN ISNUMERIC(CountryP) = 1 THEN (SELECT ISNULL(TextVal,'') FROM Textvaltable WHERE Textcode = A.CountryP) ELSE ISNULL(CountryP,'') END CountryP,CASE WHEN ISNUMERIC(CountryC) = 1 THEN (SELECT ISNULL(TextVal,'') FROM Textvaltable WHERE Textcode = A.CountryC) ELSE ISNULL(CountryC,'') END CountryC,ISNULL(Parent_PinCodeC,'') PinC,ISNULL(Parent_PinCodeP,'') PinP,ISNULL(Parent_PhNoC,'') PhNoC,ISNULL(Parent_PhNoP,'') PhNoP, ISNULL(Student_Mobile,'') Student_Mobile,ISNULL(StuPer_ID,'') StuPer_ID FROM Registration R INNER JOIN Applyn A ON A.App_No = R.App_No INNER JOIN Degree G ON G.Degree_Code = R.Degree_Code AND G.College_Code = R.College_Code INNER JOIN Course C ON C.Course_ID = G.Course_ID AND C.College_Code = G.College_Code INNER JOIN Department D ON D.Dept_Code = G.Dept_Code AND D.College_Code = G.College_Code WHERE R.App_No='" + appno + "'";
                        sql = sql + " sELECT a.app_no,ISNULL(C.TExtVal,'') Course,ISNULL(U.TextVal,'') University,ISNULL(Institute_Name,'') Institute_Name,ISNULL(PassMonth,'') PassMonth,ISNULL(PassYear,'') PassYear,ISNULL(NoOfAttempts ,0) NoOfAttempts,ISNULL(p.Certificate_No,'') Certificate_No,CONVERT(VARCHAR(11),p.Certificate_Date,103) as Certificate_Date,(select textval from textvaltable where TextCode=isnull(course_code,'0'))course_code,LastTCNo+' - '+convert(varchar(10),LastTCDate,103)LastTC  FROM Stud_prev_details P INNER JOIN applyn A ON P.app_no = A.app_no LEFT JOIN TextValTable C ON C.TextCode = P.course_code LEFT JOIN TextValTable U ON U.TextCode = P.university_code Where p.app_no = a.app_no And a.app_no = '" + appno + "' and p.course_entno='" + course + "'";
                        // sql = sql + "select * from StudCertDetails where App_no='" + appno + "'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(sql, "Text");
                        Gios.Pdf.PdfTable table1forpage1;
                        Gios.Pdf.PdfTable table1forpage1datas;
                        Boolean pdfstart = true;
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                            {
                                table1forpage1datas = mydoc.NewTable(Fontsmall1, ds.Tables[0].Rows.Count + ds.Tables[1].Rows.Count, 35, 5);
                                table1forpage1datas.VisibleHeaders = false;
                                table1forpage1datas.SetBorders(Color.Black, 1, BorderType.None);
                                table1forpage1datas.Columns[0].SetWidth(100);
                                table1forpage1datas.Columns[1].SetWidth(190);
                                table1forpage1datas.Columns[2].SetWidth(160);
                                table1forpage1datas.Columns[3].SetWidth(60);
                                #region
                                //mypdfpage.SaveToDocument();
                                coltop = 0;
                                pdfstart = false;
                                collinfo1 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 100, 595, 50), System.Drawing.ContentAlignment.TopCenter, "ADMISSION REGISTER");
                                mypdfpage.Add(collinfo1);
                                collinfo1 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 102, 595, 50), System.Drawing.ContentAlignment.TopCenter, "____________________");
                                mypdfpage.Add(collinfo1);
                                table1forpage1 = mydoc.NewTable(Fontsmall1, 30, 100, 5);
                                table1forpage1.VisibleHeaders = false;
                                table1forpage1.SetBorders(Color.Black, 1, BorderType.None);
                                table1forpage1.Columns[0].SetWidth(380);
                                table1forpage1.Columns[1].SetWidth(30);
                                table1forpage1.Columns[2].SetWidth(530);
                                table1forpage1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(0, 0).SetContent("Name Of The Candidate");
                                table1forpage1.Cell(0, 1).SetContent(":");
                                table1forpage1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(0, 0).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(0, 2).SetContent(ds.Tables[0].Rows[ii]["Stud_name"].ToString());
                                table1forpage1.Cell(1, 0).SetContent("Course Admitted With Date");
                                table1forpage1.Cell(1, 0).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(1, 1).SetContent(":");
                                table1forpage1.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(1, 2).SetContent(ds.Tables[0].Rows[ii]["Course"].ToString() + " - " + ds.Tables[0].Rows[ii]["Adm_Date"].ToString());
                                table1forpage1.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(2, 0).SetContent("Roll No / Admission No / Quota");
                                table1forpage1.Cell(2, 0).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(2, 1).SetContent(":");
                                table1forpage1.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(2, 2).SetContent(ds.Tables[0].Rows[ii]["Roll_no"].ToString() + " / " + ds.Tables[0].Rows[ii]["Roll_Admit"].ToString() + " / " + ds.Tables[0].Rows[ii]["Quota"].ToString());
                                //table1forpage1.Cell(3, 0).SetContent("Quota");
                                //table1forpage1.Cell(3, 0).SetFont(Fontsmall1bold);
                                //table1forpage1.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                //table1forpage1.Cell(3, 1).SetContent(":");
                                //table1forpage1.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                //table1forpage1.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                //table1forpage1.Cell(3, 2).SetContent(ds.Tables[0].Rows[ii]["Quota"].ToString());
                                table1forpage1.Cell(3, 0).SetContent("Father's / Guardian's Name");
                                table1forpage1.Cell(3, 0).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(3, 1).SetContent(":");
                                table1forpage1.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(3, 2).SetContent(ds.Tables[0].Rows[ii]["parent_name"].ToString());
                                table1forpage1.Cell(4, 0).SetContent("Mother's Name");
                                table1forpage1.Cell(4, 0).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(4, 1).SetContent(":");
                                table1forpage1.Cell(4, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(4, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(4, 2).SetContent(ds.Tables[0].Rows[ii]["mother"].ToString());
                                table1forpage1.Cell(5, 0).SetContent("Sex");
                                table1forpage1.Cell(5, 0).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(5, 1).SetContent(":");
                                table1forpage1.Cell(5, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(5, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(5, 2).SetContent(ds.Tables[0].Rows[ii]["sex"].ToString());
                                table1forpage1.Cell(6, 0).SetContent("Date Of Birth");
                                table1forpage1.Cell(6, 0).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(6, 1).SetContent(":");
                                table1forpage1.Cell(6, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(6, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(6, 2).SetContent(ds.Tables[0].Rows[ii]["dob"].ToString());
                                table1forpage1.Cell(7, 0).SetContent("Nationality / Religion");
                                table1forpage1.Cell(7, 0).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(7, 1).SetContent(":");
                                table1forpage1.Cell(7, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(7, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(7, 2).SetContent(ds.Tables[0].Rows[ii]["citizen"].ToString() + " / " + ds.Tables[0].Rows[ii]["religion"].ToString());
                                string Community = ds.Tables[0].Rows[ii]["community"].ToString();
                                string Caste = ds.Tables[0].Rows[ii]["caste"].ToString();
                                if (Community != "" && Caste != "")
                                {
                                    Caste = " / " + Caste;
                                }
                                table1forpage1.Cell(8, 0).SetContent("Community / Caste");
                                table1forpage1.Cell(8, 0).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(8, 1).SetContent(":");
                                table1forpage1.Cell(8, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(8, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(8, 2).SetContent(Community + "" + Caste);
                                table1forpage1.Cell(9, 0).SetContent("Mother Tongue");
                                table1forpage1.Cell(9, 0).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(9, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(9, 1).SetContent(":");
                                table1forpage1.Cell(9, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(9, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(9, 2).SetContent(ds.Tables[0].Rows[ii]["MotherTongue"].ToString());
                                string addressline1 = Convert.ToString(ds.Tables[0].Rows[0]["parent_addressC"]).Replace("\r\n", " "); //.Replace("\r", " ").Replace("\n", " ")
                                string addressline2 = Convert.ToString(ds.Tables[0].Rows[0]["Streetc"]);
                                string addressline3 = string.Empty;
                                if (addressline2.Contains('/') == true)
                                {
                                    string[] splitaddress = addressline2.Split('/');
                                    if (splitaddress.Length > 1)
                                    {
                                        if (splitaddress[0] != "")
                                        {
                                            addressline2 = "," + Convert.ToString(splitaddress[0]);
                                        }
                                        else
                                        {
                                            addressline2 = string.Empty;
                                        }
                                        if (splitaddress[1] != "")
                                        {
                                            addressline3 = "," + Convert.ToString(splitaddress[1]);
                                        }
                                        else
                                        {
                                            addressline3 = string.Empty;
                                        }
                                    }
                                    else
                                    {
                                        addressline2 = Convert.ToString(splitaddress[0]);
                                    }
                                }
                                string CityC = Convert.ToString(ds.Tables[0].Rows[0]["CityC"]);
                                if (!CityC.Any(char.IsLetter))
                                    CityC = d2.GetFunction("select textval from textvaltable where TextCriteria ='city' and TextCode='" + CityC + "'");
                                CityC = (CityC == "0") ? "" : CityC;
                                string pinC = Convert.ToString(ds.Tables[0].Rows[0]["PinC"]);
                                if (CityC != "" && pinC != "")
                                {
                                    pinC = "-" + pinC;
                                }
                                table1forpage1.Cell(10, 0).SetContent("Address For Communication");
                                table1forpage1.Cell(10, 0).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(10, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(10, 1).SetContent(":");
                                table1forpage1.Cell(10, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(10, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(10, 2).SetContent(addressline1.Trim() + "" + addressline2 + "" + addressline3); table1forpage1.Cell(10, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(10, 2).SetFont(Fontsmall1bold);//delsi0204
                                table1forpage1.Cell(11, 2).SetContent(CityC.ToString().Trim() + "" + pinC);
                                table1forpage1.Cell(11, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(12, 2).SetContent(ds.Tables[0].Rows[ii]["DistrictC"].ToString());
                                table1forpage1.Cell(12, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(13, 0).SetContent("Email ID");
                                table1forpage1.Cell(13, 0).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(13, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(13, 1).SetContent(":");
                                table1forpage1.Cell(13, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(13, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(13, 2).SetContent(Convert.ToString(ds.Tables[0].Rows[ii]["StuPer_Id"]).Trim());
                                table1forpage1.Cell(13, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                addressline1 = Convert.ToString(ds.Tables[0].Rows[0]["Parent_AddressP"]).Replace("\r\n", " ");
                                //.Replace("\r", " ").Replace("\n", " ");
                                addressline2 = Convert.ToString(ds.Tables[0].Rows[0]["StreetP"]);
                                addressline3 = string.Empty;
                                if (addressline2.Contains('/') == true)
                                {
                                    string[] splitaddress = addressline2.Split('/');
                                    if (splitaddress.Length > 1)
                                    {
                                        if (splitaddress[0] != "")
                                        {
                                            addressline2 = "," + Convert.ToString(splitaddress[0]);
                                        }
                                        else
                                        {
                                            addressline2 = string.Empty;
                                        }
                                        if (splitaddress[1] != "")
                                        {
                                            addressline3 = "," + Convert.ToString(splitaddress[1]);
                                        }
                                        else
                                        {
                                            addressline3 = string.Empty;
                                        }
                                    }
                                    else
                                    {
                                        addressline2 = Convert.ToString(splitaddress[0]);
                                    }
                                }
                                CityC = Convert.ToString(ds.Tables[0].Rows[0]["CityP"]);
                                if (!CityC.Any(char.IsLetter))
                                    CityC = d2.GetFunction("select textval from textvaltable where TextCriteria ='city' and TextCode='" + CityC + "'");
                                CityC = (CityC == "0") ? "" : CityC;
                                pinC = Convert.ToString(ds.Tables[0].Rows[0]["PinP"]);
                                if (CityC != "" && pinC != "")
                                {
                                    pinC = "-" + pinC;
                                }
                                //23.12.16 
                                table1forpage1.Cell(14, 0).SetContent("Mobile No");
                                table1forpage1.Cell(14, 0).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(14, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(14, 1).SetContent(":");
                                table1forpage1.Cell(14, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(14, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(14, 2).SetContent(Convert.ToString(ds.Tables[0].Rows[ii]["Student_Mobile"]).Trim());
                                table1forpage1.Cell(15, 0).SetContent("Permenant Address");
                                table1forpage1.Cell(15, 0).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(15, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(15, 1).SetContent(":");
                                table1forpage1.Cell(15, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(15, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(15, 2).SetContent(addressline1.Trim() + "" + addressline2 + "" + addressline3);
                                table1forpage1.Cell(15, 2).SetFont(Fontsmall1bold);//delsi0204
                                table1forpage1.Cell(16, 2).SetContent(CityC.ToString().Trim() + "" + pinC);
                                table1forpage1.Cell(16, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(17, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(17, 2).SetContent(ds.Tables[0].Rows[ii]["DistrictP"].ToString());
                                table1forpage1.Cell(18, 0).SetContent("Entry Level Qualification");
                                table1forpage1.Cell(18, 0).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(18, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(18, 1).SetContent(" ");
                                table1forpage1.Cell(18, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(18, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(19, 0).SetContent("(a) HSC / Diploma / Degree");
                                table1forpage1.Cell(19, 0).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(19, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(19, 1).SetContent(":");
                                table1forpage1.Cell(19, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(19, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(20, 0).SetContent("(b) Board Of University");
                                table1forpage1.Cell(20, 0).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(20, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(20, 1).SetContent(":");
                                table1forpage1.Cell(20, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(20, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(21, 0).SetContent("(c) Institution Name");
                                table1forpage1.Cell(21, 0).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(21, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(21, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(21, 1).SetContent(":");
                                table1forpage1.Cell(21, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(22, 0).SetContent("(d) Month And Year Of Passing Number Of        Attempt");
                                table1forpage1.Cell(22, 0).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(22, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(22, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(22, 1).SetContent(":");
                                table1forpage1.Cell(22, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(23, 0).SetContent("Previous TC No With Date");
                                table1forpage1.Cell(23, 0).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(23, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(23, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(23, 1).SetContent(":");
                                table1forpage1.Cell(23, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(24, 0).SetContent("Date Of Last Attendance In Our College");
                                table1forpage1.Cell(24, 0).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(24, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(24, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(24, 1).SetContent(":");
                                table1forpage1.Cell(24, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                string clglastdate = string.Empty;
                                if (Convert.ToString(ds.Tables[0].Rows[ii]["batch_year"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[ii]["degree_code"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[ii]["Current_Semester"]).Trim() != "")
                                {
                                    clglastdate = d2.GetFunction(" select CONVERT(varchar(10), end_date ,103)as end_date from seminfo where batch_year in('" + Convert.ToString(ds.Tables[0].Rows[ii]["batch_year"]) + "') and degree_code in('" + Convert.ToString(ds.Tables[0].Rows[ii]["degree_code"]) + "') and semester in('" + Convert.ToString(ds.Tables[0].Rows[ii]["Current_Semester"]) + "')");
                                }
                                if (clglastdate.Trim() == "0")
                                    clglastdate = string.Empty;
                                //table1forpage1.Cell(24, 2).SetContent(clglastdate);
                                table1forpage1.Cell(24 + 1, 0).SetContent("Date Of TC Issued In Our College");
                                table1forpage1.Cell(24 + 1, 0).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(24 + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(24 + 1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(24 + 1, 1).SetContent(":");
                                table1forpage1.Cell(24 + 1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(25 + 1, 0).SetContent("Our College TC Number");
                                table1forpage1.Cell(25 + 1, 0).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(25 + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(25 + 1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(25 + 1, 1).SetContent(":");
                                table1forpage1.Cell(25 + 1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(26 + 1, 0).SetContent("Reason For Leaving");
                                table1forpage1.Cell(26 + 1, 0).SetFont(Fontsmall1bold);
                                table1forpage1.Cell(26 + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(26 + 1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage1.Cell(26 + 1, 1).SetContent(":");
                                table1forpage1.Cell(26 + 1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                if (ds.Tables[1].Rows.Count > 0)
                                {
                                    ds.Tables[1].DefaultView.RowFilter = "app_no='" + appno + "'";
                                    dv = ds.Tables[1].DefaultView;
                                    if (dv.Count > 0)
                                    {
                                        table1forpage1.Cell(19, 2).SetContent(dv[0]["course_code"].ToString());
                                        table1forpage1.Cell(20, 2).SetContent(dv[0]["University"].ToString());

                                        table1forpage1.Cell(21, 2).SetContent(dv[0]["Institute_Name"].ToString());
                                        table1forpage1.Cell(21, 2).SetContentAlignment(ContentAlignment.MiddleLeft);//delsi2603
                                        table1forpage1.Cell(21, 2).SetFont(Fontsmall1bold);
                                        string mth = dv[0]["PassMonth"].ToString();
                                        string yr = dv[0]["PassYear"].ToString();
                                        string atn = dv[0]["NoOfAttempts"].ToString();
                                        if (mth.Trim() == "" && yr.Trim() == "0" && atn.Trim() == "0")
                                        {
                                            table1forpage1.Cell(22, 2).SetContent("");
                                        }
                                        else
                                        {
                                            table1forpage1.Cell(22, 2).SetContent(dv[0]["PassMonth"].ToString() + "-" + dv[0]["PassYear"].ToString() + "-" + dv[0]["NoOfAttempts"].ToString());
                                        }
                                        table1forpage1.Cell(23, 2).SetContent(dv[0]["LastTC"].ToString());
                                        table1forpage1.Cell(24 + 1, 2).SetContent(dv[0]["Certificate_Date"].ToString());
                                        table1forpage1.Cell(25 + 1, 2).SetContent(dv[0]["Certificate_No"].ToString());
                                    }
                                }
                                Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 65, 120, mydoc.PageWidth, 600));
                                mypdfpage.Add(newpdftabpage2);
                                Double getheigh = newpdftabpage2.Area.Height;
                                getheigh = Math.Round(getheigh, 2);
                                #endregion
                            }
                            mypdfpage.SaveToDocument();
                        }
                    }
                }
                if (d == 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Select Any Student";
                    return;
                }
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "StudentStrengthStatusReport" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                    Response.Buffer = true;
                    Response.Clear();
                    mydoc.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }

    public void imgbtn_settingpdf_Click(object sender, EventArgs e)
    {
        div_settingpdf.Visible = false;
    }

    public void img_settingpdf_Click(object sender, EventArgs e)
    {
        div_settingpdf.Visible = true;
        load1();
        lst_setting2.Items.Clear();
    }

    public void btnMvOneRt1_Click(object sender, EventArgs e)
    {
        try
        {
            bool ok = true;
            if (lst_setting1.Items.Count > 0 && lst_setting1.SelectedItem.Value != "")
            {
                for (int j = 0; j < lst_setting2.Items.Count; j++)
                {
                    if (lst_setting2.Items[j].Value == lst_setting1.SelectedItem.Value)
                    {
                        ok = false;
                    }
                }
                if (ok)
                {
                    ListItem lst = new ListItem(lst_setting1.SelectedItem.Text, lst_setting1.SelectedItem.Value);
                    lst_setting2.Items.Add(lst);
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }

    public void btnMvTwoRt1_Click(object sender, EventArgs e)
    {
        try
        {
            lst_setting2.Items.Clear();
            if (lst_setting1.Items.Count > 0)
            {
                for (int j = 0; j < lst_setting1.Items.Count; j++)
                {
                    lst_setting2.Items.Add(new ListItem(lst_setting1.Items[j].Text.ToString(), lst_setting1.Items[j].Value.ToString()));
                }
            }
            lst_setting1.Items.Clear();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }

    public void btnMvOneLt1_Click(object sender, EventArgs e)
    {
        try
        {
            if (lst_setting2.Items.Count > 0 && lst_setting2.SelectedItem.Value != "")
            {
                lst_setting2.Items.RemoveAt(lst_setting2.SelectedIndex);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }

    public void btnMvTwoLt1_Click(object sender, EventArgs e)
    {
        try
        {
            lst_setting2.Items.Clear();
            load1();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }

    public void load1()
    {
        lst_setting1.Items.Clear();
        lst_setting1.Items.Add(new ListItem("Roll No", "1"));
        lst_setting1.Items.Add(new ListItem("Name of Candidate", "2"));
        lst_setting1.Items.Add(new ListItem("Course", "3"));
        lst_setting1.Items.Add(new ListItem("Semester", "4"));
        lst_setting1.Items.Add(new ListItem("Father's / Guardian's Name", "5"));
        lst_setting1.Items.Add(new ListItem("DOB", "6"));
        lst_setting1.Items.Add(new ListItem("Parent Occupation", "7"));
        lst_setting1.Items.Add(new ListItem("Mother Tongue", "8"));
        lst_setting1.Items.Add(new ListItem("Religion", "9"));
        lst_setting1.Items.Add(new ListItem("Citizen", "10"));
        lst_setting1.Items.Add(new ListItem("Community", "11"));
        lst_setting1.Items.Add(new ListItem("Caste", "12"));
        lst_setting1.Items.Add(new ListItem("VisualHandy", "13"));
        lst_setting1.Items.Add(new ListItem("First Graduate", "14"));
        lst_setting1.Items.Add(new ListItem("SeatType", "15"));
        lst_setting1.Items.Add(new ListItem("Address", "16"));
        lst_setting1.Items.Add(new ListItem("Street", "17"));
        lst_setting1.Items.Add(new ListItem("City", "18"));
        lst_setting1.Items.Add(new ListItem("State", "19"));
        lst_setting1.Items.Add(new ListItem("Country", "20"));
        lst_setting1.Items.Add(new ListItem("Student Mobile", "21"));
        lst_setting1.Items.Add(new ListItem("PinCode", "22"));
        lst_setting1.Items.Add(new ListItem("Parent Phone No", "23"));
        //lst_setting1.Items.Add(new ListItem("MissionaryChild", "MissionaryChild"));  
        // lst_setting1.Items.Add(new ListItem("Entry Level Qualification", ""));
        lst_setting1.Items.Add(new ListItem("(a) HSC / Diploma / Degree", "24"));
        lst_setting1.Items.Add(new ListItem("(b) Board Of University", "25"));
        lst_setting1.Items.Add(new ListItem("(c) Institute Name", "26"));
        lst_setting1.Items.Add(new ListItem("(d) Month And Year Of Passing Number Of                Attempt", "27"));
        lst_setting1.Items.Add(new ListItem("Medium", "28"));
        lst_setting1.Items.Add(new ListItem("Part1 Language", "29"));
        lst_setting1.Items.Add(new ListItem("Part2 Language", "30"));
        lst_setting1.Items.Add(new ListItem("University Code", "31"));
        lst_setting1.Items.Add(new ListItem("CGPA", "125"));
        lst_setting1.Items.Add(new ListItem("No Of Arrear", "126"));
        lst_setting1.Items.Add(new ListItem("Referred By", "127"));//added
    }

    public void loadvalue1()
    {
        if (colval == "1")
        {
            loadval = "Roll_No";
        }
        if (colval == "2")
        {
            loadval = "Stud_Name";
        }
        if (colval == "3")
        {
            loadval = "Course";
        }
        if (colval == "4")
        {
            loadval = "Current_Semester";
        }
        if (colval == "5")
        {
            loadval = "parent_name";
        }
        if (colval == "6")
        {
            loadval = "dob";
        }
        if (colval == "7")
        {
            loadval = "parent_occu";
        }
        if (colval == "8")
        {
            loadval = "MotherTongue";
        }
        if (colval == "9")
        {
            loadval = "religion";
        }
        if (colval == "10")
        {
            loadval = "Citizen";
        }
        if (colval == "11")
        {
            loadval = "Community";
        }
        if (colval == "12")
        {
            loadval = "Caste";
        }
        if (colval == "13")
        {
            loadval = "visualhandy";
        }
        if (colval == "14")
        {
            loadval = "first_graduate";
        }
        if (colval == "15")
        {
            loadval = "Quota";
        }
        if (colval == "16")
        {
            loadval = "parent_addressC";
        }
        if (colval == "17")
        {
            loadval = "Streetc";
        }
        if (colval == "18")
        {
            loadval = "Cityc";
        }
        if (colval == "19")
        {
            loadval = "StateC";
        }
        if (colval == "20")
        {
            loadval = "CountryC";
        }
        if (colval == "21")
        {
            loadval = "Student_Mobile";
        }
        if (colval == "22")
        {
            loadval = "PinC";
        }
        if (colval == "23")
        {
            loadval = "PhNoC";
        }
        if (colval == "24")
        {
            loadval = "Institute_name";
        }
        if (colval == "25")
        {
            loadval = "University";
        }
        if (colval == "26")
        {
            loadval = "Institute_name";
        }
        if (colval == "27")
        {
            loadval = "PassMonth";
        }
        if (colval == "28")
        {
            loadval = "medium";
        }
        if (colval == "29")
        {
            loadval = "Part1Language";
        }
        if (colval == "30")
        {
            loadval = "Part2Language";
        }
        if (colval == "31")
        {
            loadval = "University";
        }
        if (colval == "40")
        {
            loadval = "uni_state";
        }
        if (colval == "125")
        {
            loadval = "CGPA";
        }
        if (colval == "126")
        {
            loadval = "No Of Arrear";
        }
        if (colval == "127")//added by abarna
        {
            loadval = "Refered By";
        }
        if (colval == "126")
        {
            loadval = "No_of_arrear";
        }
        if (colval == "128")
        {
            loadval = "DATEPART (day,dob)";
        }
        if (colval == "129")
        {
            loadval = "DATEPART(MONTH, dob)";
        }
        if (colval == "130")
        {
            loadval = "DATEPART(YEAR, dob)";
        }
        if (colval == "131")
        {
            loadval = "subject_name";

        }
        if (colval == "132")
        {
            loadval = "acronym";

        }
        if (colval == "133")
        {
            loadval = "building_description";

        }
        if (colval == "134")
        {
            loadval = "Building_Acronym";

        }
        if (colval == "135")
        {
            loadval = "Discontinue_Date";

        }
        if (colval == "136")
        {
            loadval = "reason";

        }
    }

    public void loadlcolumns1()
    {
        try
        {
            string linkname = "StudentStrengthPDF column order settings";
            string columnvalue = string.Empty;
            DataSet dscol = new DataSet();
            string selcol = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + collegecode1 + "' ";
            dscol.Clear();
            dscol = d2.select_method_wo_parameter(selcol, "Text");
            if (lst_setting2.Items.Count > 0)
            {
                string val = string.Empty;
                for (int j = 0; j < lst_setting2.Items.Count; j++)
                {
                    val = lst_setting2.Items[j].Value;
                    if (columnvalue == "")
                    {
                        columnvalue = val;
                    }
                    else
                    {
                        columnvalue = columnvalue + ',' + val;
                    }
                }
                string clsinsert = " if exists(select * from New_InsSettings where LinkName='" + linkname + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "' ) update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkname + "','" + columnvalue + "','" + usercode + "','" + collegecode1 + "')";
                int clsupdate = d2.update_method_wo_parameter(clsinsert, "Text");
            }
            else
            {
                if (dscol.Tables[0].Rows.Count > 0)
                {
                    for (int col = 0; col < dscol.Tables[0].Rows.Count; col++)
                    {
                        string value = Convert.ToString(dscol.Tables[0].Rows[col]["LinkValue"]);
                        if (value != "")
                        {
                            string[] valuesplit = value.Split(',');
                            if (valuesplit.Length > 0)
                            {
                                for (int k = 0; k < valuesplit.Length; k++)
                                {
                                    colval = Convert.ToString(valuesplit[k]);
                                    loadtext1();
                                    lst_setting2.Items.Add(new ListItem(loadval, colval));
                                }
                            }
                        }
                        else
                        {
                            string val = string.Empty;
                            for (int j = 0; j < lst_setting2.Items.Count; j++)
                            {
                                val = lst_setting2.Items[j].Value;
                                if (columnvalue == "")
                                {
                                    columnvalue = val;
                                }
                                else
                                {
                                    columnvalue = columnvalue + ',' + val;
                                }
                            }
                            string clsinsert = " if exists(select * from New_InsSettings where LinkName='" + linkname + "') update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkname + "','" + columnvalue + "','" + usercode + "','" + collegecode1 + "')";
                            int clsupdate = d2.update_method_wo_parameter(clsinsert, "Text");
                        }
                    }
                }
                else
                {
                    string val = string.Empty;
                    for (int j = 0; j < lst_setting2.Items.Count; j++)
                    {
                        val = lst_setting2.Items[j].Value;
                        if (columnvalue == "")
                        {
                            columnvalue = val;
                        }
                        else
                        {
                            columnvalue = columnvalue + ',' + val;
                        }
                    }
                    string clsinsert = " if exists(select * from New_InsSettings where LinkName='" + linkname + "') update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkname + "','" + columnvalue + "','" + usercode + "','" + collegecode1 + "')";
                    int clsupdate = d2.update_method_wo_parameter(clsinsert, "Text");
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }

    public void loadtext1()
    {
        if (colval == "1")
        {
            loadval = "Roll No";
        }
        if (colval == "2")
        {
            loadval = "Name of Candidate";
        }
        if (colval == "3")
        {
            loadval = "Course";
        }
        if (colval == "4")
        {
            loadval = "Semester";
        }
        if (colval == "5")
        {
            loadval = "Father's / Guardian's ";
        }
        if (colval == "6")
        {
            loadval = "DOB";
        }
        if (colval == "7")
        {
            loadval = "Parent Occupation";
        }
        if (colval == "8")
        {
            loadval = "Mother Tongue";
        }
        if (colval == "9")
        {
            loadval = "Religion";
        }
        if (colval == "10")
        {
            loadval = "Citizen";
        }
        if (colval == "11")
        {
            loadval = "Community";
        }
        if (colval == "12")
        {
            loadval = "Caste";
        }
        if (colval == "13")
        {
            loadval = "VisualHandy";
        }
        if (colval == "14")
        {
            loadval = "First Graduate";
        }
        if (colval == "15")
        {
            loadval = "SeatType";
        }
        if (colval == "16")
        {
            loadval = "Address";
        }
        if (colval == "17")
        {
            loadval = "Street";
        }
        if (colval == "18")
        {
            loadval = "City";
        }
        if (colval == "19")
        {
            loadval = "State";
        }
        if (colval == "20")
        {
            loadval = "Country";
        }
        if (colval == "21")
        {
            loadval = "Student Mobile";
        }
        if (colval == "22")
        {
            loadval = "PinCode";
        }
        if (colval == "23")
        {
            loadval = "Parent Phone No";
        }
        if (colval == "24")
        {
            loadval = "(a) HSC / Diploma / Degree";
        }
        if (colval == "25")
        {
            loadval = "(b) Board Of University";
        }
        if (colval == "26")
        {
            loadval = "(c) Institute Name";
        }
        if (colval == "27")
        {
            loadval = "(d) Month And Year Of Passing Number Of                Attempt";
        }
        if (colval == "28")
        {
            loadval = "Medium";
        }
        if (colval == "29")
        {
            loadval = "Part1 Language";
        }
        if (colval == "30")
        {
            loadval = "Part2 Language";
        }
        if (colval == "31")
        {
            loadval = "University";
        }
        if (colval == "125")
        {
            loadval = "CGPA";
        }
        if (colval == "126")
        {
            loadval = "No Of Arrear";
        }
        if (colval == "127")//addded abarna
        {
            loadval = "Referred By";
        }
        if (colval == "128")
        {
            loadval = "Dob[DD]";

        }
        if (colval == "129")
        {
            loadval = "Dob[MM]";

        }
        if (colval == "130")
        {
            loadval = "Dob[YYYY]";

        }

        if (colval == "131")
        {
            loadval = "subject_name";

        }
        if (colval == "132")
        {
            loadval = "acronym";

        }
        if (colval == "133")
        {
            loadval = "building_description";

        }
        if (colval == "134")
        {
            loadval = "Building_Acronym";

        }
        if (colval == "135")
        {
            loadval = "Discontinue_Date";

        }
        if (colval == "136")
        {
            loadval = "Reason";

        }
        if (colval == "137")
        {
            loadval = "StudMessType";

        }
        if (colval == "138")
        {
            loadval = "entryusercode";

        }
        if (colval == "139")
        {
            loadval = "board";

        }
    }

    public void btnok1_click(object sender, EventArgs e)
    {
        if (lst_setting2.Items.Count > 0)
        {
            div_settingpdf.Visible = false;
            lblalerterrnew.Visible = false;
            loadlcolumns1();
        }
        else
        {
            lblalerterrnew.Visible = true;
            lblalerterrnew.Text = "Please select atleast one colunm then proceed!";
        }
    }

    public void btnclose1_click(object sender, EventArgs e)
    {
        div_settingpdf.Visible = false;
    }

    public void stud_detailsettingbased()
    {
        try
        {
            double page2col = 0;
            double binddatatb = 0;
            string activerow = string.Empty;
            string checkvalue = string.Empty;
            int j = 0;
            int cc = 0;
            int d = 0;
            DataSet ds1 = new DataSet();
            if (Fpspread2.Visible == true)
            {
                Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
                System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
                System.Drawing.Font Fontbolda = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
                System.Drawing.Font Fontbold16 = new System.Drawing.Font("Book Antiqua", 16, FontStyle.Bold);
                System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Regular);
                System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
                System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
                System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
                System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
                System.Drawing.Font Fontsmall1bold = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
                System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);
                Gios.Pdf.PdfPage mypdfpage;
                PdfTextArea collinfo1;
                string printvalue = string.Empty;
                for (int i = 0; i < Fpspread2.Sheets[0].RowCount; i++)
                {
                    if (i != 0)
                    {
                        checkvalue = Convert.ToString(Fpspread2.Sheets[0].Cells[i, 1].Value);
                        if (checkvalue == "1")
                        {
                            d = 1;
                            //activerow = Fpspread2.ActiveSheetView.ActiveRow.ToString();
                            //  string rollno = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(i), 2].Tag);
                            string appno = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(i), 2].Tag);
                            int coltop = 0;
                            string Collvalue = string.Empty;
                            mypdfpage = mydoc.NewPage();
                            #region
                            string strquery = "Select * from Collinfo where college_code=" + Session["collegecode"].ToString() + "";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(strquery, "Text");
                            strquery = "Select * from Collinfo where college_code=" + Session["collegecode"].ToString() + "";
                            ds1.Clear();
                            ds1 = d2.select_method_wo_parameter(strquery, "Text");
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                string collinfo = Convert.ToString(ds1.Tables[0].Rows[0]["collname"]);
                                string university = Convert.ToString(ds1.Tables[0].Rows[0]["university"]);
                                string affliatedby = Convert.ToString(ds1.Tables[0].Rows[0]["affliatedby"]);
                                string address1 = ds1.Tables[0].Rows[0]["Address1"].ToString();
                                string address2 = ds1.Tables[0].Rows[0]["Address2"].ToString();
                                string address3 = ds1.Tables[0].Rows[0]["Address3"].ToString();
                                string district = ds1.Tables[0].Rows[0]["district"].ToString();
                                string state = ds1.Tables[0].Rows[0]["State"].ToString();
                                string pincode = ds1.Tables[0].Rows[0]["Pincode"].ToString();
                                string phone = ds1.Tables[0].Rows[0]["Phoneno"].ToString();
                                string fax = ds1.Tables[0].Rows[0]["Faxno"].ToString();
                                string email = ds1.Tables[0].Rows[0]["Email"].ToString();
                                string website = ds1.Tables[0].Rows[0]["Website"].ToString();
                                if (collinfo != "")
                                {
                                    collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 15, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds1.Tables[0].Rows[0]["collname"].ToString() + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                if (university != "")
                                {
                                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 30, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds1.Tables[0].Rows[0]["university"].ToString() + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                else if (affliatedby != "")
                                {
                                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 30, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds1.Tables[0].Rows[0]["affliatedby"].ToString() + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                if (address1 != "" || address1 != "" || address3 != "")
                                {
                                    if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                                    {
                                        Collvalue = address1;
                                    }
                                    if (address2.Trim() != "" && address2 != null && address2.Length > 1)
                                    {
                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                        {
                                            Collvalue = Collvalue + ',' + address2;
                                        }
                                        else
                                        {
                                            Collvalue = address2;
                                        }
                                    }
                                    if (address3.Trim() != "" && address3 != null && address3.Length > 1)
                                    {
                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                        {
                                            Collvalue = Collvalue + ',' + address3;
                                        }
                                        else
                                        {
                                            Collvalue = address3;
                                        }
                                    }
                                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 45, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                else if (address3 != "")
                                {
                                    string address11 = ds1.Tables[0].Rows[0]["Address3"].ToString();
                                    if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                                    {
                                        Collvalue = address1;
                                    }
                                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 45, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                if (district != "" || state != "" || pincode != "")
                                {
                                    if (district.Trim() != "" && district != null && district.Length > 1)
                                    {
                                        Collvalue = district;
                                    }
                                    if (state.Trim() != "" && state != null && state.Length > 1)
                                    {
                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                        {
                                            Collvalue = Collvalue + ',' + state;
                                        }
                                        else
                                        {
                                            Collvalue = state;
                                        }
                                    }
                                    if (pincode.Trim() != "" && pincode != null && pincode.Length > 1)
                                    {
                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                        {
                                            Collvalue = Collvalue + '-' + pincode;
                                        }
                                        else
                                        {
                                            Collvalue = pincode;
                                        }
                                    }
                                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 60, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                if (phone != "" || fax != "")
                                {
                                    if (phone.Trim() != "" && phone != null && phone.Length > 1)
                                    {
                                        Collvalue = "Phone :" + phone;
                                    }
                                    if (fax.Trim() != "" && fax != null && fax.Length > 1)
                                    {
                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                        {
                                            Collvalue = Collvalue + " , Fax : " + fax;
                                        }
                                        else
                                        {
                                            Collvalue = "Fax :" + fax;
                                        }
                                    }
                                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 75, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                if (email != "" || website != "")
                                {
                                    if (email.Trim() != "" && email != null && email.Length > 1)
                                    {
                                        Collvalue = "Email :" + email;
                                    }
                                    if (website.Trim() != "" && website != null && website.Length > 1)
                                    {
                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                        {
                                            Collvalue = Collvalue + " , Web Site : " + website;
                                        }
                                        else
                                        {
                                            Collvalue = "Web Site :" + website;
                                        }
                                    }
                                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 90, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                {
                                    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                    mypdfpage.Add(LogoImage, 45, 25, 400);
                                }
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                                {
                                    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                    mypdfpage.Add(LogoImage, 480, 25, 400);
                                }
                            }
                            #endregion
                            DataView dv = new DataView();
                            //  string appno = d2.GetFunction("select App_No from Registration where Roll_No='" + rollno + "'");
                            string sql = string.Empty;
                            // string sql = "select a.sex,a.mother,a.MissionaryChild,a.parent_phnoc,a.parent_pincodec,a.Student_Mobile,a.Countryc,a.parent_statec,a.Cityc,a.Streetc,a.parent_addressC,a.co_curricular,a.citizen,a.visualhandy,a.first_graduate,a.TamilOrginFromAndaman ,a.seattype,a.mother_tongue,a.parent_occu,a.caste,a.community,a.religion,a.parent_name,r.Roll_No,r.Stud_Name,r.Batch_Year,r.Current_Semester,c.Course_Name,Dt.Dept_Name,r.Sections,CONVERT(VARCHAR(11),dob,103) as dob from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='1' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and  r.Roll_no='" + rollno + "'";
                            sql = "SELECT a.MissionaryChild,a.visualhandy,a.first_graduate,a.parent_occu,a.current_semester,A.App_No,Roll_No,Roll_Admit,Reg_No,R.Stud_Name,Course_Name+'-'+Dept_Name Course,Adm_Date, CONVERT(VARCHAR(11),DOB,103) as DOB,CASE WHEN Sex = 0 THEN 'Male' ELSE 'Female' END Sex,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.SeatType = T.TextCode AND L.App_No = A.App_No) Quota,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.BldGrp = T.TextCode AND L.App_No = A.App_No) BloodGroup,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Citizen = T.TextCode AND L.App_No = A.App_No) Citizen,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Religion = T.TextCode AND L.App_No = A.App_No) Religion,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Community = T.TextCode AND L.App_No = A.App_No) Community,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Caste = T.TextCode AND L.App_No = A.App_No) Caste,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Mother_Tongue = T.TextCode AND L.App_No = A.App_No) MotherTongue,ISNULL(IDMark,'') IDMark,ISNULL(Parent_Name,'') Parent_Name,ISNULL(Mother,'') Mother, Parent_AddressC,StreetC,CityC,Parent_AddressP,StreetP,CityP,CASE WHEN ISNUMERIC(DistrictP) = 1 THEN (SELECT ISNULL(TextVal,'') FROM Textvaltable WHERE Textcode = A.DistrictP) ELSE ISNULL(DistrictP,'') END DistrictP,CASE WHEN ISNUMERIC(DistrictC) = 1 THEN (SELECT ISNULL(TextVal,'') FROM Textvaltable WHERE Textcode = A.DistrictC) ELSE ISNULL(DistrictC,'') END DistrictC,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Parent_StateC = T.TextCode AND L.App_No = A.App_No) StateC,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Parent_StateP = T.TextCode AND L.App_No = A.App_No) StateP,CASE WHEN ISNUMERIC(CountryP) = 1 THEN (SELECT ISNULL(TextVal,'') FROM Textvaltable WHERE Textcode = A.CountryP) ELSE ISNULL(CountryP,'') END CountryP,CASE WHEN ISNUMERIC(CountryC) = 1 THEN (SELECT ISNULL(TextVal,'') FROM Textvaltable WHERE Textcode = A.CountryC) ELSE ISNULL(CountryC,'') END CountryC,ISNULL(Parent_PinCodeC,'') PinC,ISNULL(Parent_PinCodeP,'') PinP,ISNULL(Parent_PhNoC,'') PhNoC,ISNULL(Parent_PhNoP,'') PhNoP, ISNULL(Student_Mobile,'') Student_Mobile,ISNULL(StuPer_ID,'') StuPer_ID FROM Registration R INNER JOIN Applyn A ON A.App_No = R.App_No INNER JOIN Degree G ON G.Degree_Code = R.Degree_Code AND G.College_Code = R.College_Code INNER JOIN Course C ON C.Course_ID = G.Course_ID AND C.College_Code = G.College_Code INNER JOIN Department D ON D.Dept_Code = G.Dept_Code AND D.College_Code = G.College_Code WHERE a.App_no='" + appno + "'";
                            //R.Roll_No='" + rollno + "'
                            sql = sql + "   sELECT ISNULL(m.TextVal,'') medium,a.app_no,university_code,ISNULL(pt.TExtVal,'') Part2Language,ISNULL(pa.TExtVal,'') Part1Language,ISNULL(C.TExtVal,'') Course,ISNULL(U.TextVal,'') University,ISNULL(Institute_Name,'') Institute_Name,ISNULL(PassMonth,'') PassMonth,ISNULL(PassYear,'') PassYear,ISNULL(NoOfAttempts ,0) NoOfAttempts,ISNULL(Certificate_No,'') Certificate_No,ISNULL(Certificate_Date,'') Certificate_Date FROM Stud_prev_details P INNER JOIN applyn A ON P.app_no = A.app_no LEFT JOIN TextValTable C ON C.TextCode = P.course_code  LEFT JOIN TextValTable U ON U.TextCode = P.university_code  LEFT JOIN TextValTable m ON m.TextCode = P.medium  LEFT JOIN TextValTable pa ON pa.TextCode = P.Part1Language LEFT JOIN TextValTable pt ON pt.TextCode = P.Part2Language Where p.app_no = a.app_no And a.app_no = '" + appno + "'";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(sql, "Text");
                            Gios.Pdf.PdfTable table1forpage1;
                            Gios.Pdf.PdfTable table1forpage1datas;
                            Boolean pdfstart = true;
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                                {
                                    int row = lst_setting2.Items.Count;
                                    table1forpage1datas = mydoc.NewTable(Fontsmall1, row, 100, 4);
                                    table1forpage1datas.VisibleHeaders = false;
                                    table1forpage1datas.SetBorders(Color.Black, 1, BorderType.None);
                                    table1forpage1datas.Columns[0].SetWidth(100);
                                    table1forpage1datas.Columns[1].SetWidth(190);
                                    table1forpage1datas.Columns[2].SetWidth(600);
                                    #region
                                    coltop = 0;
                                    pdfstart = false;
                                    collinfo1 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 125, 595, 50), System.Drawing.ContentAlignment.TopCenter, "ADMISSION REGISTER");
                                    mypdfpage.Add(collinfo1);
                                    collinfo1 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 127, 595, 50), System.Drawing.ContentAlignment.TopCenter, "____________________");
                                    mypdfpage.Add(collinfo1);
                                    table1forpage1 = mydoc.NewTable(Fontsmall1, row, 100, 4);
                                    table1forpage1.VisibleHeaders = false;
                                    table1forpage1.SetBorders(Color.Black, 1, BorderType.None);
                                    table1forpage1.Columns[0].SetWidth(500);
                                    table1forpage1.Columns[1].SetWidth(60);
                                    table1forpage1.Columns[2].SetWidth(400);
                                    string linkname = "StudentStrengthPDF column order settings";
                                    string columnvalue = string.Empty;
                                    DataSet dscol = new DataSet();
                                    string selcol = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + collegecode1 + "' ";
                                    dscol.Clear();
                                    dscol = d2.select_method_wo_parameter(selcol, "Text");
                                    if (dscol.Tables.Count > 0)
                                    {
                                        if (dscol.Tables[0].Rows.Count > 0)
                                        {
                                            for (int c = 0; c < dscol.Tables[0].Rows.Count; c++)
                                            {
                                                string value = Convert.ToString(dscol.Tables[0].Rows[c]["LinkValue"]);
                                                if (value != "")
                                                {
                                                    string[] valuesplit = value.Split(',');
                                                    if (valuesplit.Length > 0)
                                                    {
                                                        for (int k = 0; k < valuesplit.Length; k++)
                                                        {
                                                            cc++;
                                                            colval = Convert.ToString(valuesplit[k]);
                                                            loadvalue1();
                                                            string col = loadval;
                                                            loadtext1();
                                                            //string header = Convert.ToString(lst_setting1.Items[Convert.ToInt32(colval)].Text);
                                                            //colval = Convert.ToString(lst_setting1.Items[Convert.ToInt32(colval)].Value);
                                                            string header = loadval;
                                                            table1forpage1.Cell(k, 0).SetContent(header);
                                                            table1forpage1.Cell(k, 1).SetContent(":");
                                                            if (col == "Institute_name" || col == "Part1Language" || col == "Part2Language" || col == "university_code" || col == "uni_state" || col == "medium" || col == "University" || col == "PassMonth")
                                                            {
                                                                if (ds.Tables[1].Rows.Count > 0)
                                                                {
                                                                    ds.Tables[1].DefaultView.RowFilter = "app_no='" + appno + "'";
                                                                    dv = ds.Tables[1].DefaultView;
                                                                    if (dv.Count > 0)
                                                                    {
                                                                        printvalue = Convert.ToString(dv[0][col]);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    printvalue = string.Empty;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (col == "parent_occu")
                                                                {
                                                                    printvalue = d2.GetFunction("select textval from TextValTable where TextCode='" + Convert.ToString(ds.Tables[0].Rows[ii][col]) + "'");
                                                                }
                                                                else if (col == "")
                                                                {
                                                                    printvalue = string.Empty;
                                                                }
                                                                else if (col == "visualhandy")
                                                                {
                                                                    col = Convert.ToString(ds.Tables[0].Rows[ii][col]);
                                                                    if (col == "True")
                                                                    {
                                                                        printvalue = "YES";
                                                                    }
                                                                    else
                                                                    {
                                                                        printvalue = "NO";
                                                                    }
                                                                }
                                                                else if (col == "first_graduate")
                                                                {
                                                                    col = Convert.ToString(ds.Tables[0].Rows[ii][col]);
                                                                    if (col == "True")
                                                                    {
                                                                        printvalue = "YES";
                                                                    }
                                                                    else
                                                                    {
                                                                        printvalue = "NO";
                                                                    }
                                                                }
                                                                else if (col == "PassMonth")
                                                                {
                                                                    string year = Convert.ToString(ds.Tables[1].Rows[ii]["PassYear"]);
                                                                    string noofatmp = Convert.ToString(ds.Tables[1].Rows[ii]["NoOfAttempts"]);
                                                                    if (year != "" && noofatmp != "")
                                                                    {
                                                                        printvalue = Convert.ToString(ds.Tables[1].Rows[ii]["PassMonth"]) + "-" + Convert.ToString(ds.Tables[1].Rows[ii]["PassYear"]) + "-" + Convert.ToString(ds.Tables[1].Rows[ii]["NoOfAttempts"]);
                                                                    }
                                                                    else if (year != "" && noofatmp == "")
                                                                    {
                                                                        printvalue = Convert.ToString(ds.Tables[1].Rows[ii]["PassMonth"]) + "-" + Convert.ToString(ds.Tables[1].Rows[ii]["PassYear"]);
                                                                    }
                                                                    else
                                                                    {
                                                                        printvalue = Convert.ToString(ds.Tables[1].Rows[ii]["PassMonth"]);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    printvalue = Convert.ToString(ds.Tables[0].Rows[ii][col]);
                                                                }
                                                                if (printvalue == "0")
                                                                {
                                                                    printvalue = string.Empty;
                                                                }
                                                            }
                                                            if (printvalue == "")
                                                            {
                                                                printvalue = string.Empty;
                                                            }
                                                            table1forpage1.Cell(k, 2).SetContent(printvalue);
                                                            table1forpage1.Cell(k, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                            table1forpage1.Cell(k, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                            table1forpage1.Cell(k, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 55, 160, 480, 600));
                                    mypdfpage.Add(newpdftabpage2);
                                    Double getheigh = newpdftabpage2.Area.Height;
                                    getheigh = Math.Round(getheigh, 2);
                                    #endregion
                                }
                                mypdfpage.SaveToDocument();
                            }
                        }
                    }
                }
                if (d == 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Select Any Student";
                    return;
                }
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "StudentStrengthStatusReport" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                    Response.Buffer = true;
                    Response.Clear();
                    mydoc.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }

    public void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    public void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string attendance = "Student Strength Status Report";
            string pagename = "StudentStrengthStatusReport.aspx";
            Printcontrol.loadspreaddetails(Fpspread2, pagename, attendance);
            Printcontrol.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }

    public void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string report = txt_excelname.Text;
            if (report.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread2, report);
                lbl_norec.Visible = false;
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
            if (txt_excelname.Text == "")
            {
                lbl_norec.Visible = true;
            }
            else
            {
                lbl_norec.Visible = false;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }

    public void clear()
    {
        Fpspread1.Visible = false;
        lbl_headernamespd2.Visible = false;
        imgbtn_columsetting.Visible = false;
        Fpspread2.Visible = false;
        div_report.Visible = false;
        btn_viewsprd2.Visible = false;
        lnk_admisstionformbtn.Visible = false;
        img_settingpdf.Visible = false;
    }

    public bool checkedOK()
    {
        bool Ok = false;
        Fpspread2.SaveChanges();
        for (int i = 1; i < Fpspread2.Sheets[0].Rows.Count; i++)
        {
            byte check = Convert.ToByte(Fpspread2.Sheets[0].Cells[i, 1].Value);
            if (check == 1)
            {
                Ok = true;
            }
        }
        return Ok;
    }

    protected void fpspread2_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        string activerow = string.Empty;
        string activecol = string.Empty;
        activerow = Fpspread2.ActiveSheetView.ActiveRow.ToString();
        activecol = Fpspread2.ActiveSheetView.ActiveColumn.ToString();
        string actrow = e.SheetView.ActiveRow.ToString();
        string actcol = e.SheetView.ActiveColumn.ToString();
        string value = string.Empty;
        if (Convert.ToInt32(activecol) == 1 && Convert.ToInt32(activerow) == 0)
        {
            value = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Value);
            if (value == "1")
            {
                for (int i = 0; i < Fpspread2.Sheets[0].Rows.Count; i++)
                {
                    Fpspread2.Sheets[0].Cells[i, 1].Value = 1;
                }
            }
            else
            {
                for (int i = 0; i < Fpspread2.Sheets[0].Rows.Count; i++)
                {
                    Fpspread2.Sheets[0].Cells[i, 1].Value = 0;
                }
            }
        }
        else
        {
            if (Convert.ToInt32(activecol) == 1)
            {
                int ii = 0;
                for (int i = 0; i < Fpspread2.Sheets[0].Rows.Count; i++)
                {
                    value = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(i), Convert.ToInt32(activecol)].Value);
                    if (value == "1")
                    {
                    }
                    else
                    {
                        ii = 1;
                    }
                    if (ii == 1)
                    {
                        Fpspread2.Sheets[0].Cells[0, 1].Value = 0;
                    }
                    else
                    {
                        Fpspread2.Sheets[0].Cells[0, 1].Value = 1;
                    }
                }
            }
        }
    }

    public void columnordertype()
    {
        ddl_colord.Items.Clear();
        ddl_coltypeadd.Items.Clear();
        string query = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='StudentStatusReport' and CollegeCode='" + ddlcollege.SelectedItem.Value + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_colord.DataSource = ds;
            ddl_colord.DataTextField = "MasterValue";
            ddl_colord.DataValueField = "MasterCode";
            ddl_colord.DataBind();
            ddl_colord.Items.Insert(0, new ListItem("Select", "0"));
            ddl_coltypeadd.DataSource = ds;
            ddl_coltypeadd.DataTextField = "MasterValue";
            ddl_coltypeadd.DataValueField = "MasterCode";
            ddl_coltypeadd.DataBind();
            ddl_coltypeadd.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            ddl_colord.Items.Insert(0, new ListItem("Select", "0"));
            ddl_coltypeadd.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    public void btn_addtype_OnClick(object sender, EventArgs e)
    {
        imgdiv33.Visible = true;
        panel_description11.Visible = true;
    }

    public void btn_deltype_OnClick(object sender, EventArgs e)
    {
        if (ddl_coltypeadd.SelectedIndex == -1)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No records found";
        }
        else if (ddl_coltypeadd.SelectedIndex == 0)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Select any record";
        }
        else if (ddl_coltypeadd.SelectedIndex != 0)
        {
            string sql = "delete from CO_MasterValues where MasterCode='" + ddl_coltypeadd.SelectedItem.Value.ToString() + "' and MasterCriteria='StudentStatusReport' and CollegeCode='" + ddlcollege.SelectedItem.Value + "' ";
            int delete = d2.update_method_wo_parameter(sql, "TEXT");
            if (delete != 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Deleted Sucessfully";
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "No records found";
            }
            columnordertype();
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No records found";
        }
    }

    public void btndescpopadd_Click(object sender, EventArgs e)
    {
        if (txt_description11.Text != "")
        {
            string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='StudentStatusReport' and CollegeCode ='" + ddlcollege.SelectedItem.Value + "') update CO_MasterValues set MasterValue ='" + txt_description11.Text + "' where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='StudentStatusReport' and CollegeCode ='" + ddlcollege.SelectedItem.Value + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values ('" + txt_description11.Text + "','StudentStatusReport','" + ddlcollege.SelectedItem.Value + "')";
            int insert = d2.update_method_wo_parameter(sql, "TEXT");
            if (insert != 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Added sucessfully";
                txt_description11.Text = string.Empty;
                //imgdiv33.Visible = false;           
            }
        }
        else
        {
            imgdiv2.Visible = true;
            pnl2.Visible = true;
            lbl_alert.Text = "Enter the description";
        }
        columnordertype();
    }

    public void btndescpopexit_Click(object sender, EventArgs e)
    {
        panel_description11.Visible = false;
        imgdiv33.Visible = false;
    }

    protected void btnExcelNew_Click(object sender, EventArgs e)
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
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }

    protected void btnprintmasterNew_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Admission Report";
            string pagename = "StudentStrengthStatusReport.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            rptprint.Visible = true;
            Printmaster1.Visible = true;
            // 
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }

    //18.6.2016

    public void cb_resident_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_resident.Checked == true)
        {
            txt_resident.Enabled = true;
        }
        else
        {
            txt_resident.Enabled = false;
        }
    }

    public void cb_residency_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_residency, cbl_residency, txt_resident, "Residency", "--Select--");
    }

    public void cbl_residency_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_residency, cbl_residency, txt_resident, "Residency", "--Select--");
    }

    public void cb_sports_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_sports.Checked == true)
        {
            txt_sports.Enabled = true;
        }
        else
        {
            txt_sports.Enabled = false;
        }
    }

    public void cb_sport_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_sport, cbl_sport, txt_sports, "Sports", "--Select--");
    }

    public void cbl_sport_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_sport, cbl_sport, txt_sports, "Sports", "--Select--");
    }

    public void cb_lang_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_lang.Checked == true)
        {
            txt_lang.Enabled = true;
        }
        else
        {
            txt_lang.Enabled = false;
        }
    }

    public void cb_language_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_language, cbl_language, txt_lang, "Language", "--Select--");
    }

    public void cbl_language_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_language, cbl_language, txt_lang, "Language", "--Select--");
    }

    public void cb_mothertng_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_mothertng.Checked == true)
        {
            txt_mothertng.Enabled = true;
        }
        else
        {
            txt_mothertng.Enabled = false;
        }
    }

    public void cb_mothertongue_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_mothertongue, cbl_mothertongue, txt_mothertng, "Mother Tongue", "--Select--");
    }

    public void cbl_mothertongue_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_mothertongue, cbl_mothertongue, txt_mothertng, "Mother Tongue", "--Select--");
    }

    public void cb_phychallange_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_phychallange.Checked == true)
        {
            txt_phychallage.Enabled = true;
        }
        else
        {
            txt_phychallage.Enabled = false;
        }
    }

    public void cb_board_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_board.Checked == true)
        {
            txtBoardUniv.Enabled = true;
            loadBoardUniv();
        }
        else
        {
            txtBoardUniv.Enabled = false;
        }

    }

    public void cb_unistate_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_state.Checked == true)
        {
            txtstate.Enabled = true;
        }
        else
        {
            txtstate.Enabled = false;
        }
    }

    public void cb_quota_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cbquotacheck.Checked == true)
        {
            txtQuota.Enabled = true;
        }
        else
        {
            txtQuota.Enabled = false;
        }
    }

    public void cb_phychlg_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_phychlg, cbl_phychlg, txt_phychallage, "Physical Challaged", "--Select--");
    }

    public void cbl_phychlg_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_phychlg, cbl_phychlg, txt_phychallage, "Physical Challaged", "--Select--");
    }

    public void cb_trans_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_trans.Checked == true)
        {
            txt_transport.Enabled = true;
        }
        else
        {
            txt_transport.Enabled = false;
        }
    }

    public void cb_transport_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_transport, cbl_transport, txt_transport, "Transport", "--Select--");
    }

    public void cbl_transport_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_transport, cbl_transport, txt_transport, "Transport", "--Select--");
    }

    public void detailcolumn()
    {
        tdlblstudtype.Visible = false;
        tdstudetype.Visible = false;
        tdseattype.Visible = false;
        tdseattype1.Visible = false;
        tdtype.Visible = false;
        tdtype1.Visible = false;
        tdrelichk.Visible = false;
        tdrelichk1.Visible = false;
        tdcommchk.Visible = false;
        tdcommchk1.Visible = false;
        tdresident.Visible = false;
        tdresident1.Visible = false;
        tdsports.Visible = false;
        tdsports1.Visible = false;
        tdlang.Visible = false;
        tdlang1.Visible = false;
        tdmothertng.Visible = false;
        tdphychallange.Visible = false;
        tdtransport.Visible = false;
    }

    public void bindresidency()
    {
        string type = string.Empty;
        string[] residency = { "Campus Required", "Not Required" };
        for (int i = 0; i < 2; i++)
        {
            cbl_residency.Items.Add(new System.Web.UI.WebControls.ListItem(residency[i], Convert.ToString(i)));
        }
        if (cbl_residency.Items.Count > 0)
        {
            for (int i = 0; i < cbl_residency.Items.Count; i++)
            {
                cbl_residency.Items[i].Selected = true;
                type = Convert.ToString(cbl_residency.Items[i].Text);
            }
            if (cbl_residency.Items.Count == 1)
            {
                txt_resident.Text = "Residency(" + type + ")";
            }
            else
            {
                txt_resident.Text = "Residency(" + cbl_residency.Items.Count + ")";
            }
            cb_residency.Checked = true;
        }
    }

    public void bindsports()
    {
        string type = string.Empty;
        cbl_sport.Items.Clear();
        string qur = "select Distinct DistinctSport,T.TextVal  from applyn a,TextValTable t where convert(varchar(100), a.DistinctSport) =convert(varchar(100), t.TextCode) and a.college_code ='" + ddlcollege.SelectedItem.Value + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(qur, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_sport.DataSource = ds;
            cbl_sport.DataTextField = "TextVal";
            cbl_sport.DataValueField = "DistinctSport";
            cbl_sport.DataBind();
            cbl_sport.Items.Insert(0, new ListItem("IsSports", "DistinctSport"));
        }
        if (cbl_sport.Items.Count > 0)
        {
            for (int i = 0; i < cbl_sport.Items.Count; i++)
            {
                cbl_sport.Items[i].Selected = true;
                type = Convert.ToString(cbl_sport.Items[i].Text);
            }
            if (cbl_sport.Items.Count == 1)
            {
                txt_sports.Text = "Sports(" + type + ")";
            }
            else
            {
                txt_sports.Text = "Sports(" + cbl_sport.Items.Count + ")";
            }
            cb_sport.Checked = true;
        }
    }

    public void bindlanguage()
    {
        string type = string.Empty;
        cbl_language.Items.Clear();
        string qur = "select Distinct Part1Language,T.TextVal  from applyn a,Stud_prev_details s,TextValTable t where a.app_no =s.app_no and s.Part1Language =t.TextCode and a.college_code='" + ddlcollege.SelectedItem.Value + "' and  t.TextVal not in('---Select---') ";
        ds.Clear();
        ds = d2.select_method_wo_parameter(qur, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_language.DataSource = ds;
            cbl_language.DataTextField = "TextVal";
            cbl_language.DataValueField = "Part1Language";
            cbl_language.DataBind();
        }
        if (cbl_language.Items.Count > 0)
        {
            for (int i = 0; i < cbl_language.Items.Count; i++)
            {
                cbl_language.Items[i].Selected = true;
                type = Convert.ToString(cbl_language.Items[i].Text);
            }
            if (cbl_language.Items.Count == 1)
            {
                txt_lang.Text = "Language(" + type + ")";
            }
            else
            {
                txt_lang.Text = "Language(" + cbl_language.Items.Count + ")";
            }
            cb_language.Checked = true;
        }
    }

    public void bindmothertongue()
    {
        string type = string.Empty;
        cbl_mothertongue.Items.Clear();
        string qur = "select Distinct mother_tongue ,T.TextVal  from applyn a,TextValTable t where  a.mother_tongue =t.TextCode and a.college_code='" + ddlcollege.SelectedItem.Value + "' ";
        ds.Clear();
        ds = d2.select_method_wo_parameter(qur, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_mothertongue.DataSource = ds;
            cbl_mothertongue.DataTextField = "TextVal";
            cbl_mothertongue.DataValueField = "mother_tongue";
            cbl_mothertongue.DataBind();
        }
        if (cbl_mothertongue.Items.Count > 0)
        {
            for (int i = 0; i < cbl_mothertongue.Items.Count; i++)
            {
                cbl_mothertongue.Items[i].Selected = true;
                type = Convert.ToString(cbl_mothertongue.Items[i].Text);
            }
            if (cbl_mothertongue.Items.Count == 1)
            {
                txt_mothertng.Text = "Mother Tongue(" + type + ")";
            }
            else
            {
                txt_mothertng.Text = "Mother Tongue(" + cbl_mothertongue.Items.Count + ")";
            }
            cb_mothertongue.Checked = true;
        }
    }

    public void bindphysicalchallaged()
    {
        string type = string.Empty;
        string[] physical = { "IsDisable", "Visually Challanged", "Physically Challanged", "Learning Disability", "Others" };
        string[] physical1 = { "isdisable", "visualhandy ", "handy", "islearningdis", "isdisabledisc" };
        for (int i = 0; i < 5; i++)
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

    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = string.Empty;
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

    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = string.Empty;
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

    public void viewcolumorder()
    {
        try
        {
            lb_column1.Items.Clear();
            if (ddl_coltypeadd.SelectedItem.Text != "Select")
            {
                string q = "select LinkValue from New_InsSettings where LinkName='" + ddl_coltypeadd.SelectedItem.Text + "' and college_code='" + ddlcollege.SelectedItem.Value + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string vall = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
                    string[] sp = vall.Split(',');
                    for (int y = 0; y < sp.Length; y++)
                    {
                        colval = sp[y];
                        loadtext();
                        lb_column1.Items.Add(new System.Web.UI.WebControls.ListItem(loadval, Convert.ToString(sp[y])));
                    }
                }
            }
        }
        catch
        {
        }
    }

    public void cb_Gender_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_gen, cbl_gen, txt_gen, "Gender", "--Select--");
    }

    public void cbl_gen_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_gen, cbl_gen, txt_gen, "Gender", "--Select--");
    }

    protected void cb_EnGender_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_Gender.Checked == true)
        {
            txt_gen.Enabled = true;
            cb_gen.Checked = true;
            cb_Gender_CheckedChanged(sender, e);
        }
        else
        {
            txt_gen.Enabled = false;
        }
    }

    public void ddl_coltypeadd_SelectedIndexChanged(object sender, EventArgs e)
    {
        viewcolumorder();
    }

    public void ddl_status_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddl_status.SelectedItem.Value) > 3)
        {
            cb_trans.Enabled = true;
        }
        else
        {
            cb_trans.Enabled = false;
        }
        //Added By Saranyadevi 24.2.2018
        if (Convert.ToInt32(ddl_status.SelectedItem.Value) == 5)
        {
            tdcbdisreason.Visible = true;
            tddisreason.Visible = true;
        }
        else
        {
            tdcbdisreason.Visible = false;
            tddisreason.Visible = false;
        }
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
        fields.Add(0);
        lbl.Add(lbl_Stream);
        fields.Add(1);
        lbl.Add(lbl_degree);
        fields.Add(2);
        lbl.Add(lbl_branch);
        fields.Add(3);
        lbl.Add(lbl_org_sem);
        fields.Add(4);
        //Name -0, Stream - 1 ,Degree - 2, Branch - 3, Term - 4
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }

    protected void lnk_admisstionform_Click(object sender, EventArgs e)
    {
        Admissionform();
        //Fpspread2
    }

    protected void Admissionform()
    {
        try
        {
            Fpspread2.SaveChanges(); bool nothingselect = false;
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
            string checkvalue = string.Empty;
            DataSet ds1 = new DataSet();
            if (Fpspread2.Visible == true)
            {
                Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
                System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
                System.Drawing.Font Fontbolda = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
                System.Drawing.Font Fontbold16 = new System.Drawing.Font("Book Antiqua", 16, FontStyle.Bold);
                System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Regular);
                System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
                System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
                System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
                System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
                System.Drawing.Font Fontsmall1bold = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
                System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);
                System.Drawing.Font Fontboldu = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Underline);
                Font Fonttimes = new Font("Times New Roman", 10, FontStyle.Regular);
                PdfTextArea collinfo1;
                for (int i = 1; i < Fpspread2.Sheets[0].RowCount; i++)
                {
                    checkvalue = Convert.ToString(Fpspread2.Sheets[0].Cells[i, 1].Value);
                    if (checkvalue == "1")
                    {
                        nothingselect = true;
                        string appno = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(i), 2].Text);
                        int coltop = 0;
                        string Collvalue = string.Empty;
                        mypdfpage = mydoc.NewPage();
                        #region
                        string strquery = "Select * from Collinfo where college_code=" + Session["collegecode"].ToString() + "";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(strquery, "Text");
                        strquery = "Select * from Collinfo where college_code=" + Session["collegecode"].ToString() + "";
                        ds1.Clear();
                        ds1 = d2.select_method_wo_parameter(strquery, "Text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            string collinfo = Convert.ToString(ds1.Tables[0].Rows[0]["collname"]);
                            string university = Convert.ToString(ds1.Tables[0].Rows[0]["university"]);
                            string affliatedby = Convert.ToString(ds1.Tables[0].Rows[0]["affliatedby"]);
                            string address1 = ds1.Tables[0].Rows[0]["Address1"].ToString();
                            string address2 = ds1.Tables[0].Rows[0]["Address2"].ToString();
                            string address3 = ds1.Tables[0].Rows[0]["Address3"].ToString();
                            string district = ds1.Tables[0].Rows[0]["district"].ToString();
                            string state = ds1.Tables[0].Rows[0]["State"].ToString();
                            string pincode = ds1.Tables[0].Rows[0]["Pincode"].ToString();
                            string phone = ds1.Tables[0].Rows[0]["Phoneno"].ToString();
                            string fax = ds1.Tables[0].Rows[0]["Faxno"].ToString();
                            string email = ds1.Tables[0].Rows[0]["Email"].ToString();
                            string website = ds1.Tables[0].Rows[0]["Website"].ToString();
                            if (collinfo != "")
                            {
                                if (collinfo.Contains("Gnanamani"))
                                {
                                    collinfo = "Gnanamani Educational Institutions";
                                }
                                collinfo1 = new PdfTextArea(Fontbold16, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 25, mydoc.PageWidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + collinfo);
                                mypdfpage.Add(collinfo1);
                            }
                            //if (university != "")
                            //{
                            //    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 30, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds1.Tables[0].Rows[0]["university"].ToString() + "");
                            //    mypdfpage.Add(collinfo1);
                            //}
                            //else if (affliatedby != "")
                            //{
                            //    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 30, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds1.Tables[0].Rows[0]["affliatedby"].ToString() + "");
                            //    mypdfpage.Add(collinfo1);
                            //}
                            if (address1 != "" || address1 != "" || address3 != "")
                            {
                                if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                                {
                                    Collvalue = address1;
                                }
                                if (address2.Trim() != "" && address2 != null && address2.Length > 1)
                                {
                                    if (Collvalue.Trim() != "" && Collvalue != null)
                                    {
                                        Collvalue = Collvalue + ',' + ' ' + address2;
                                    }
                                    else
                                    {
                                        Collvalue = address2;
                                    }
                                }
                                if (address3.Trim() != "" && address3 != null && address3.Length > 1)
                                {
                                    if (Collvalue.Trim() != "" && Collvalue != null)
                                    {
                                        Collvalue = Collvalue + ',' + ' ' + address3;
                                    }
                                    else
                                    {
                                        Collvalue = address3;
                                    }
                                }
                                collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 45, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                mypdfpage.Add(collinfo1);
                            }
                            else if (address3 != "")
                            {
                                string address11 = ds1.Tables[0].Rows[0]["Address3"].ToString();
                                if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                                {
                                    Collvalue = address1;
                                }
                                collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 45, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                mypdfpage.Add(collinfo1);
                            }
                            if (district != "" || pincode != "")
                            {
                                if (district.Trim() != "" && district != null && district.Length > 1)
                                {
                                    Collvalue = district;
                                }
                                if (pincode.Trim() != "" && pincode != null && pincode.Length > 1)
                                {
                                    if (Collvalue.Trim() != "" && Collvalue != null)
                                    {
                                        Collvalue = Collvalue + ' ' + '-' + ' ' + pincode;
                                    }
                                    else
                                    {
                                        Collvalue = pincode;
                                    }
                                }
                                collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 60, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                mypdfpage.Add(collinfo1);
                            }
                            if (phone != "" || fax != "")
                            {
                                if (phone.Trim() != "" && phone != null && phone.Length > 1)
                                {
                                    Collvalue = "Phone :" + phone;
                                }
                                if (fax.Trim() != "" && fax != null && fax.Length > 1)
                                {
                                    if (Collvalue.Trim() != "" && Collvalue != null)
                                    {
                                        Collvalue = Collvalue + " , Fax : " + fax;
                                    }
                                    else
                                    {
                                        Collvalue = "Fax :" + fax;
                                    }
                                }
                                //collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 75, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                //mypdfpage.Add(collinfo1);
                            }
                            if (email != "" || website != "")
                            {
                                if (email.Trim() != "" && email != null && email.Length > 1)
                                {
                                    Collvalue = "Email :" + email;
                                }
                                if (website.Trim() != "" && website != null && website.Length > 1)
                                {
                                    if (Collvalue.Trim() != "" && Collvalue != null)
                                    {
                                        Collvalue = Collvalue + " , Web Site : " + website;
                                    }
                                    else
                                    {
                                        Collvalue = "Web Site :" + website;
                                    }
                                }
                                //collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 90, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                //mypdfpage.Add(collinfo1);
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                            {
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                mypdfpage.Add(LogoImage, 50, 20, 450);
                            }
                        }
                        #endregion
                        DataView dv = new DataView();
                        string sql = string.Empty;//barath 08.04.17

                        string course = d2.GetFunction("select s.course_entno  from Stud_prev_details s where  s.app_no = '" + appno + "' order by markPriority desc ");//course_entno
                        //sql = "SELECT case when isnull(a.direct_refer,0)=0 then 'Direct' when a.direct_refer=1 then 'Staff' when a.direct_refer=2 then 'Student' when a.direct_refer=3 then 'Others' else '' end Direct_refer, case when a.direct_refer=1 then (select appl_name from staff_appl_master where convert(varchar(20),appl_id)=a.refer_stcode) when direct_refer=1 then (select Stud_Name from Registration where convert(varchar(20),App_No)=a.refer_stcode) else '' end refer_stcode,r.Boarding,r.Stud_Type, a.app_formno, a.parentF_Mobile,r.batch_year,r.degree_code, r.Current_Semester ,A.App_No,Roll_No,Roll_Admit, Reg_No,R.Stud_Name,Course_Name, Dept_Name,CONVERT(varchar(10), Adm_Date,103)as Adm_Date, CONVERT(VARCHAR(11),DOB,103) as DOB,CASE WHEN Sex = 0 THEN 'Male' ELSE 'Female' END Sex,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.parent_income = T.TextCode AND L.App_No = A.App_No) parent_income,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.SeatType = T.TextCode AND L.App_No = A.App_No) Quota,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.BldGrp = T.TextCode AND L.App_No = A.App_No) BloodGroup,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Citizen = T.TextCode AND L.App_No = A.App_No) Citizen,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Religion = T.TextCode AND L.App_No = A.App_No) Religion,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Community = T.TextCode AND L.App_No = A.App_No) Community,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Caste = T.TextCode AND L.App_No = A.App_No) Caste,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Mother_Tongue = T.TextCode AND L.App_No = A.App_No) MotherTongue,ISNULL(IDMark,'') IDMark,ISNULL(Parent_Name,'') Parent_Name,ISNULL(Mother,'') Mother, Parent_AddressC,StreetC,CityC,Parent_AddressP,StreetP,CityP,CASE WHEN ISNUMERIC(DistrictP) = 1 THEN (SELECT ISNULL(TextVal,'') FROM Textvaltable WHERE Textcode = A.DistrictP) ELSE ISNULL(DistrictP,'') END DistrictP,CASE WHEN ISNUMERIC(DistrictC) = 1 THEN (SELECT ISNULL(TextVal,'') FROM Textvaltable WHERE Textcode = A.DistrictC) ELSE ISNULL(DistrictC,'') END DistrictC,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Parent_StateC = T.TextCode AND L.App_No = A.App_No) StateC,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Parent_StateP = T.TextCode AND L.App_No = A.App_No) StateP,CASE WHEN ISNUMERIC(CountryP) = 1 THEN (SELECT ISNULL(TextVal,'') FROM Textvaltable WHERE Textcode = A.CountryP) ELSE ISNULL(CountryP,'') END CountryP,CASE WHEN ISNUMERIC(CountryC) = 1 THEN (SELECT ISNULL(TextVal,'') FROM Textvaltable WHERE Textcode = A.CountryC) ELSE ISNULL(CountryC,'') END CountryC,ISNULL(Parent_PinCodeC,'') PinC,ISNULL(Parent_PinCodeP,'') PinP,ISNULL(Parent_PhNoC,'') PhNoC,ISNULL(Parent_PhNoP,'') PhNoP, ISNULL(Student_Mobile,'') Student_Mobile,ISNULL(StuPer_ID,'') StuPer_ID FROM Registration R INNER JOIN Applyn A ON A.App_No = R.App_No INNER JOIN Degree G ON G.Degree_Code = R.Degree_Code AND G.College_Code = R.College_Code INNER JOIN Course C ON C.Course_ID = G.Course_ID AND C.College_Code = G.College_Code INNER JOIN Department D ON D.Dept_Code = G.Dept_Code AND D.College_Code = G.College_Code WHERE R.App_No='" + appno + "'";
                        sql = " select a.mode,case when isnull(a.direct_refer,0)=0 then 'Direct' when a.direct_refer=1 then 'Staff' when a.direct_refer=3 then 'Student' when a.direct_refer=2 then 'Others' else '' end Direct_refer, case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent,a.Stud_Type, a.app_formno, a.parentF_Mobile,a.batch_year,a.degree_code, a.Current_Semester ,A.App_No,Roll_No,Roll_Admit, Reg_No,a.Stud_Name,Course_Name, Dept_Name,CONVERT(varchar(10), a.date_applied,103)as Adm_Date, CONVERT(VARCHAR(11),DOB,103) as DOB,CASE WHEN Sex = 0 THEN 'Male' ELSE 'Female' END Sex,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.parent_income = T.TextCode AND L.App_No = A.App_No) parent_income,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.SeatType = T.TextCode AND L.App_No = A.App_No) Quota,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.BldGrp = T.TextCode AND L.App_No = A.App_No) BloodGroup,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Citizen = T.TextCode AND L.App_No = A.App_No) Citizen,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Religion = T.TextCode AND L.App_No = A.App_No) Religion,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Community = T.TextCode AND L.App_No = A.App_No) Community,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Caste = T.TextCode AND L.App_No = A.App_No) Caste,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Mother_Tongue = T.TextCode AND L.App_No = A.App_No) MotherTongue,ISNULL(IDMark,'') IDMark,ISNULL(Parent_Name,'') Parent_Name,ISNULL(Mother,'') Mother, Parent_AddressC,StreetC,CityC,Parent_AddressP,StreetP,CityP,CASE WHEN ISNUMERIC(DistrictP) = 1 THEN (SELECT ISNULL(TextVal,'') FROM Textvaltable WHERE Textcode = A.DistrictP) ELSE ISNULL(DistrictP,'') END DistrictP,CASE WHEN ISNUMERIC(DistrictC) = 1 THEN (SELECT ISNULL(TextVal,'') FROM Textvaltable WHERE Textcode = A.DistrictC) ELSE ISNULL(DistrictC,'') END DistrictC,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Parent_StateC = T.TextCode AND L.App_No = A.App_No) StateC,(SELECT ISNULL(TextVal,'') FROM Applyn L,TextValTable T WHERE L.Parent_StateP = T.TextCode AND L.App_No = A.App_No) StateP,CASE WHEN ISNUMERIC(CountryP) = 1 THEN (SELECT ISNULL(TextVal,'') FROM Textvaltable WHERE Textcode = A.CountryP) ELSE ISNULL(CountryP,'') END CountryP,CASE WHEN ISNUMERIC(CountryC) = 1 THEN (SELECT ISNULL(TextVal,'') FROM Textvaltable WHERE Textcode = A.CountryC) ELSE ISNULL(CountryC,'') END CountryC,ISNULL(Parent_PinCodeC,'') PinC,ISNULL(Parent_PinCodeP,'') PinP,ISNULL(Parent_PhNoC,'') PhNoC,ISNULL(Parent_PhNoP,'') PhNoP, ISNULL(Student_Mobile,'') Student_Mobile,ISNULL(StuPer_ID,'') StuPer_ID from applyn a left join Registration r on a.app_no = r.app_no INNER JOIN Degree G ON G.Degree_Code = a.Degree_Code AND G.College_Code = a.College_Code INNER JOIN Course C ON C.Course_ID = G.Course_ID AND C.College_Code = G.College_Code INNER JOIN Department D ON D.Dept_Code = G.Dept_Code AND D.College_Code = G.College_Code WHERE a.App_No='" + appno + "'";
                        sql = sql + " SELECT (select textval from textvaltable where convert(varchar (100),TextCode)=isnull(p.branch_code,'0'))as branch_code,P.percentage,P.securedmark,P.totalmark,Cut_Of_Mark,PCM_Percentage,(select textval from textvaltable where convert(varchar (100),TextCode)=isnull(medium,'0'))as medium,marksheetno,a.app_no,ISNULL(C.TExtVal,'') Course,ISNULL(U.TextVal,'') University,ISNULL(Institute_Name,'') Institute_Name,ISNULL(PassMonth,'') PassMonth,ISNULL(PassYear,'') PassYear,ISNULL(NoOfAttempts ,0) NoOfAttempts,ISNULL(p.Certificate_No,'') Certificate_No,CONVERT(VARCHAR(11),p.Certificate_No,103) as Certificate_Date,(select textval from textvaltable where TextCode=isnull(course_code,'0'))course_code,LastTCNo+' - '+convert(varchar(10),LastTCDate,103)LastTC  FROM Stud_prev_details P INNER JOIN applyn A ON P.app_no = A.app_no LEFT JOIN TextValTable C ON C.TextCode = P.course_code LEFT JOIN TextValTable U ON U.TextCode = P.university_code Where p.app_no = a.app_no And a.app_no = '" + appno + "'";// and p.course_entno='" + course + "'";
                        sql = sql + "   select s.app_no, psubjectno,registerno,acual_marks,max_marks from perv_marks_history p,Stud_prev_details s where p.course_entno=s.course_entno  and s.app_no ='" + appno + "' ";//select psubjectno,registerno,acual_marks,max_marks from perv_marks_history where course_entno='" + course + "'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(sql, "Text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            int left1, left2 = 0;
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                PdfArea ph = new PdfArea(mydocument, 450, 20, 128, 60);
                                PdfRectangle phr = new PdfRectangle(mydocument, ph, Color.Black);
                                mypdfpage.Add(phr);
                                coltop = 30;
                                PdfTextArea ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, 452, coltop, 125, 50), System.Drawing.ContentAlignment.TopLeft, "ADMIN NO : " + Convert.ToString(dr["app_formno"]).ToUpper());
                                mypdfpage.Add(ptc);
                                coltop += 18;
                                ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 452, coltop, 125, 50), System.Drawing.ContentAlignment.TopLeft, "GATE PASS NO: ");
                                mypdfpage.Add(ptc);
                                coltop += 18;
                                ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, 452, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "DATE          : " + Convert.ToString(dr["Adm_Date"]));
                                mypdfpage.Add(ptc);
                                PdfArea pa1 = new PdfArea(mydocument, 14, 14, 565, 810);// 14, 12, 560, 825);
                                PdfRectangle pr1 = new PdfRectangle(mydocument, pa1, Color.Black);
                                mypdfpage.Add(pr1);
                                coltop += 40;
                                collinfo1 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "ADMISSION FORM");
                                mypdfpage.Add(collinfo1);
                                collinfo1 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "________________");
                                mypdfpage.Add(collinfo1);
                                coltop += 20;
                                left1 = 40;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                         new PdfArea(mydocument, left1, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Degree ");
                                mypdfpage.Add(ptc);
                                left2 = 180;
                                ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                      new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(dr["Course_Name"]) + "");
                                mypdfpage.Add(ptc);
                                coltop += 20;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, left1, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Year & Branch ");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                      new PdfArea(mydocument, left2, coltop, 450, 50), System.Drawing.ContentAlignment.TopLeft, ": " + getRomanletter(Convert.ToString(dr["Current_Semester"])) + " & " + Convert.ToString(dr["Dept_Name"]));
                                mypdfpage.Add(ptc);
                                coltop += 20;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, left1, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Name of the Student");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                      new PdfArea(mydocument, left2, coltop, 450, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(dr["Stud_Name"]).ToUpper());
                                mypdfpage.Add(ptc);
                                coltop += 20;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, left1, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Gender");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                                 new PdfArea(mydocument, left2, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(dr["sex"]).ToUpper());
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, left1 + 200, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Date Of Birth : ");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                                 new PdfArea(mydocument, left1 + 270, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(dr["dob"]).ToUpper());
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocument, left1 + 340, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Community : ");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                                 new PdfArea(mydocument, left1 + 400, coltop, 150, 50), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(dr["Community"]).ToUpper());
                                mypdfpage.Add(ptc);
                                coltop += 20;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, left1, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Father / Guardian Name");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                                 new PdfArea(mydocument, left2, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(dr["parent_name"]).ToUpper());
                                mypdfpage.Add(ptc);
                                coltop += 20;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, left1, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Address ");
                                mypdfpage.Add(ptc);
                                string addressline1 = Convert.ToString(dr["parent_addressP"]).Replace("\r\n", " ");
                                string addressline2 = Convert.ToString(dr["StreetP"]);
                                string addressline3 = "";
                                if (addressline2.Contains('/') == true)
                                {
                                    string[] splitaddress = addressline2.Split('/');
                                    if (splitaddress.Length > 1)
                                    {
                                        if (splitaddress[0] != "")
                                        {
                                            addressline2 = "," + Convert.ToString(splitaddress[0]);
                                        }
                                        else
                                        {
                                            addressline2 = string.Empty;
                                        }
                                        if (splitaddress[1] != "")
                                        {
                                            addressline3 = "," + Convert.ToString(splitaddress[1]);
                                        }
                                        else
                                        {
                                            addressline3 = string.Empty;
                                        }
                                    }
                                    else
                                    {
                                        addressline2 = Convert.ToString(splitaddress[0]);
                                    }
                                }
                                string CityC = Convert.ToString(dr["Cityp"]);
                                if (!CityC.Any(char.IsLetter))
                                    CityC = d2.GetFunction("select textval from textvaltable where TextCriteria ='city' and TextCode='" + CityC + "'");
                                string pinC = Convert.ToString(dr["Pinp"]);
                                if (CityC != "" && pinC != "")
                                {
                                    pinC = "-" + pinC;
                                }
                                ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left2, coltop, 450, 50), System.Drawing.ContentAlignment.TopLeft, ": " + addressline1.Trim() + "" + addressline2 + "" + addressline3);
                                mypdfpage.Add(ptc);
                                coltop += 20;
                                ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, left2, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, " " + CityC + " " + pinC);
                                mypdfpage.Add(ptc);
                                coltop += 20;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, left1, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "District ");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, left2, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(dr["Districtp"]));
                                mypdfpage.Add(ptc);
                                coltop += 20;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                      new PdfArea(mydocument, left1, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "State ");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                             new PdfArea(mydocument, left2, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(dr["Statep"]));
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocument, left1 + 305, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "SSLC Total:");
                                mypdfpage.Add(ptc);

                                coltop += 20;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                      new PdfArea(mydocument, left1, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Parent Annual Income ");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                             new PdfArea(mydocument, left2, coltop, 100, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(dr["parent_income"]));
                                mypdfpage.Add(ptc);
                                coltop += 20;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocument, left1, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Phone No ");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                             new PdfArea(mydocument, left2, coltop, 100, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(dr["parentF_Mobile"]));
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocument, left1 + 250, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Mobile No ");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, left1 + 350, coltop, 100, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(dr["Student_Mobile"]));
                                mypdfpage.Add(ptc);
                                coltop += 10;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocument, 14, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_________________________________________________________________________________________________________________");
                                mypdfpage.Add(ptc);
                                coltop += 20;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocument, left1, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Student Type");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, left2, coltop, 100, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(dr["Stud_Type"]));
                                mypdfpage.Add(ptc);

                                //if (Convert.ToString(dr["Stud_Type"]).ToUpper() == "HOSTLER")
                                //{
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                      new PdfArea(mydocument, left1 + 230, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Hostel Amount  : ");
                                mypdfpage.Add(ptc);
                                //}

                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                             new PdfArea(mydocument, left1 + 350, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, "Boarding Point :");//Convert.ToString(dr["Boarding"]).ToUpper()
                                mypdfpage.Add(ptc);
                                coltop += 20;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocument, left1, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Name and address of the institution last studied");
                                mypdfpage.Add(ptc);
                                if (ds.Tables[1].Rows.Count > 0)
                                {
                                    ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, left1 + 230, coltop, 300, 80), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(ds.Tables[1].Rows[0]["Institute_name"]).ToUpper());
                                    mypdfpage.Add(ptc);
                                }
                                coltop += 20;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocument, left1, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Exam Register No");
                                mypdfpage.Add(ptc);
                                if (ds.Tables[2].Rows.Count > 0)
                                {
                                    ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, left2, coltop, 100, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(ds.Tables[2].Rows[0]["registerno"]));
                                    mypdfpage.Add(ptc);
                                }
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                      new PdfArea(mydocument, left1 + 230, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Medium of Study ");
                                mypdfpage.Add(ptc);
                                if (ds.Tables[1].Rows.Count > 0)
                                {
                                    ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                                     new PdfArea(mydocument, left1 + 300, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, " : " + Convert.ToString(ds.Tables[1].Rows[0]["medium"]).ToUpper());
                                    mypdfpage.Add(ptc);
                                }
                                string previous = "HSC Mark";
                                if (Convert.ToString(dr["mode"]) == "3")
                                {
                                    previous = "Diploma";
                                    coltop += 20;
                                    ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                         new PdfArea(mydocument, left1, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, previous);
                                    mypdfpage.Add(ptc);
                                    ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, left2, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(ds.Tables[1].Rows[0]["branch_code"]).ToUpper());
                                    mypdfpage.Add(ptc);
                                }
                                else
                                {
                                    coltop += 20;
                                    ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                         new PdfArea(mydocument, left1, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, previous);
                                    mypdfpage.Add(ptc);
                                }
                                coltop += 20;
                                #region Hsc mark
                                if (ds.Tables[2].Rows.Count > 0)
                                {
                                    Gios.Pdf.PdfTable table2 = mydocument.NewTable(Fontsmall, ds.Tables[2].Rows.Count + 1, 3, 1);
                                    table2 = mydocument.NewTable(Fonttimes, ds.Tables[2].Rows.Count + 1, 3, 1);
                                    table2.VisibleHeaders = false;
                                    table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                    table2.Columns[0].SetWidth(30);
                                    table2.Columns[1].SetWidth(100);
                                    table2.Columns[2].SetWidth(80);
                                    table2.CellRange(0, 0, 0, 2).SetFont(Fontsmall1bold);
                                    table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(0, 0).SetContent("S.No");
                                    table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(0, 1).SetContent("Subject");
                                    table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(0, 2).SetContent("Mark");
                                    int count = 0;
                                    foreach (DataRow dr1 in ds.Tables[2].Rows)
                                    {
                                        count++;
                                        string subjectname = getsubjectname(Convert.ToString(dr1["psubjectno"]));
                                        table2.Cell(count, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(count, 0).SetContent(count);
                                        table2.Cell(count, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table2.Cell(count, 1).SetContent(subjectname.ToUpperInvariant());
                                        table2.Cell(count, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(count, 2).SetContent(Convert.ToString(dr1["acual_marks"]) + " / " + Convert.ToString(dr1["max_marks"]));
                                    }
                                    Gios.Pdf.PdfTablePage myprov_pdfpage1 = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, coltop, 550, 700));
                                    mypdfpage.Add(myprov_pdfpage1);
                                    coltop += Convert.ToInt32(myprov_pdfpage1.Area.Height) + 10;
                                }

                                if (Convert.ToString(dr["mode"]) == "3")
                                {//P.percentage,P.securedmark,P.totalmark

                                    DataView dv1 = new DataView();
                                    ds.Tables[1].DefaultView.RowFilter = " securedmark <> '0' and percentage <> '0' ";
                                    dv1 = ds.Tables[1].DefaultView;
                                    ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, left1, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Total Mark");
                                    mypdfpage.Add(ptc); ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                          new PdfArea(mydocument, left1 + 230, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Percentage");
                                    mypdfpage.Add(ptc);
                                    if (dv1.Count > 0)
                                    {
                                        ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, left2, coltop, 100, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(dv1[0]["totalmark"]));//ds.Tables[1].Rows[0]["totalmark"]));
                                        mypdfpage.Add(ptc);

                                        ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                                         new PdfArea(mydocument, left1 + 300, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, " : " + Convert.ToString(dv1[0]["percentage"]));//ds.Tables[1].Rows[0]["percentage"]).ToUpper());
                                        mypdfpage.Add(ptc);
                                    }

                                }
                                else
                                {
                                    DataView dv1 = new DataView();
                                    ds.Tables[1].DefaultView.RowFilter = " securedmark='0' and percentage='0' ";
                                    dv1 = ds.Tables[1].DefaultView;

                                    ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                         new PdfArea(mydocument, left1, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "PCM Percentage");
                                    mypdfpage.Add(ptc);

                                    ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                         new PdfArea(mydocument, left1 + 230, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Cut Of Mark");
                                    mypdfpage.Add(ptc);
                                    if (dv1.Count > 0)
                                    {
                                        ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, left2, coltop, 100, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(dv1[0]["PCM_Percentage"]));//ds.Tables[1].Rows[0]["PCM_Percentage"]));
                                        mypdfpage.Add(ptc);
                                        ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                                         new PdfArea(mydocument, left1 + 300, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, " : " + Convert.ToString(dv1[0]["Cut_Of_Mark"]));// Convert.ToString(ds.Tables[1].Rows[0]["Cut_Of_Mark"]).ToUpper());
                                        mypdfpage.Add(ptc);
                                    }
                                }
                                #endregion
                                coltop += 15;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocument, left1, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Referred By :");
                                mypdfpage.Add(ptc);

                                if (Convert.ToString(dr["Direct_refer"]).ToUpper() != "OTHERS")
                                {
                                    ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                                     new PdfArea(mydocument, left1 + 70, coltop, 150, 50), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(dr["Direct_refer"]).ToUpper());
                                    mypdfpage.Add(ptc);
                                }
                                else
                                {
                                    ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                                         new PdfArea(mydocument, left1 + 70, coltop, 150, 50), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(dr["refer_stcode"]).ToUpper());
                                    mypdfpage.Add(ptc);
                                }
                                if (Convert.ToString(dr["Direct_refer"]).ToUpper() != "OTHERS")
                                {
                                    //if (Convert.ToString(dr["refer_stcode"]).Trim() != "")
                                    //{
                                    ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                         new PdfArea(mydocument, left1 + 230, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Referred Name ");
                                    mypdfpage.Add(ptc);
                                    ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, left1 + 300, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, " : " + Convert.ToString(dr["refer_stcode"]).ToUpper());
                                    mypdfpage.Add(ptc);
                                    //}
                                }
                                else
                                {
                                    ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                          new PdfArea(mydocument, left1 + 230, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Referred Name ");
                                    mypdfpage.Add(ptc);
                                    ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, left1 + 300, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, " : " + Convert.ToString(dr["refer_agent"]).ToUpper());
                                    mypdfpage.Add(ptc);
                                }
                                coltop += 25;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.TopCenter, "Referrer's Sign.                                                   Student Sign.                                                      Parents/Guardian Sign.");
                                mypdfpage.Add(ptc);


                                coltop += 5;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocument, 14, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_________________________________________________________________________________________________________________");
                                mypdfpage.Add(ptc);
                                coltop += 20;
                                collinfo1 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, mydoc.PageWidth, 50), System.Drawing.ContentAlignment.TopCenter, "* FEES ONCE PAID WILL NOT BE REFUNDED");
                                mypdfpage.Add(collinfo1);
                                //coltop += 10;
                                //collinfo1 = new PdfTextArea(tamil, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, mydoc.PageWidth, 50), System.Drawing.ContentAlignment.TopCenter, "Nசெலுத்தப்படும் கட்டணம் திருப்பித் தரப்படமாட்டாது");
                                //mypdfpage.Add(collinfo1);
                                coltop += 20;
                                collinfo1 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "Office Use Only");
                                mypdfpage.Add(collinfo1);
                                coltop += 10;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocument, 14, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_________________________________________________________________________________________________________________");
                                mypdfpage.Add(ptc);
                                coltop += 20;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocument, left1, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "College Name");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, left2, coltop, 400, 50), System.Drawing.ContentAlignment.TopLeft, ": ");//+ Convert.ToString(ddlcollege.SelectedItem.Text)
                                mypdfpage.Add(ptc);
                                coltop += 20;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocument, left1, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Course");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                         new PdfArea(mydocument, left2, coltop, 400, 50), System.Drawing.ContentAlignment.TopLeft, ": ");// + Convert.ToString(dr["Course_Name"]) + " - " + Convert.ToString(dr["Dept_Name"])
                                mypdfpage.Add(ptc);
                                coltop += 20;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocument, left1, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Quota");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, left2, coltop, 400, 50), System.Drawing.ContentAlignment.TopLeft, ": ");//+ Convert.ToString(dr["Quota"])
                                mypdfpage.Add(ptc);
                                coltop += 20;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocument, left1, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Fees Details");
                                mypdfpage.Add(ptc);
                                //coltop += 20;
                                left1 += 145; left2 += 80;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocument, left1, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Admision Fee ");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                      new PdfArea(mydocument, left2, coltop, 100, 50), System.Drawing.ContentAlignment.TopLeft, "   : " + Convert.ToString(""));
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocument, left2 + 120, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Sem Fee / Year Fee :");
                                mypdfpage.Add(ptc);
                                coltop += 20;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocument, left1, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Cut Off Scholarship :");
                                mypdfpage.Add(ptc);
                                //ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                //                                      new PdfArea(mydocument, left1, coltop, 100, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(""));
                                //mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocument, left2 + 120, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Special Scholarship :");
                                mypdfpage.Add(ptc);
                                coltop += 20; left1 = 40;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocument, left1, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Paid Amount & Paid Date");
                                mypdfpage.Add(ptc); left2 = 180;
                                ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, left2, coltop, 100, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(""));
                                mypdfpage.Add(ptc);
                                coltop += 20;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, left1, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Remarks ");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocument, left2, coltop, 100, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(""));
                                mypdfpage.Add(ptc);
                                coltop += 40;
                                ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, left1, coltop, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Admission i/c Signature with Name");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, 0, coltop, mydoc.PageWidth, 50), System.Drawing.ContentAlignment.TopCenter, "CAO ");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, -50, coltop, mydoc.PageWidth, 50), System.Drawing.ContentAlignment.TopRight, "Chairman ");
                                mypdfpage.Add(ptc);
                            }
                            mypdfpage.SaveToDocument();
                        }
                    }
                }
                if (nothingselect == false)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Select Any Student";
                    return;
                }
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "StudentStrengthStatusReport" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                    Response.Buffer = true;
                    Response.Clear();
                    mydoc.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
        }
        catch
        {
        }
    }

    protected string getRomanletter(string sem)
    {
        string romanLettervalue = "";
        if (sem == "1" || sem == "2")
            romanLettervalue = "I";
        else if (sem == "3" || sem == "4")
            romanLettervalue = "II";
        else if (sem == "5" || sem == "6")
            romanLettervalue = "III";
        else if (sem == "7" || sem == "8")
            romanLettervalue = "IV";
        else if (sem == "9" || sem == "10")
            romanLettervalue = "V";
        return romanLettervalue;
    }

    protected string getsubjectname(string subjectcode)
    {
        string subjectname = "";
        try
        {
            subjectname = d2.GetFunction("select textval from textvaltable where TextCriteria='subje' and textval<>'' and TextCode='" + subjectcode + "' and college_code ='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'  order by textval");
            if (subjectname.Trim() == "0")
            {
                subjectname = "-";
            }
        }
        catch { }
        return subjectname;
    }

    #region roll,reg,adm no settings
    private void RollAndRegSettings()
    {
        try
        {
            DataSet dsl = new DataSet();
            string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
            dsl = d2.select_method_wo_parameter(Master1, "text");
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Admission"] = "0";
            if (dsl.Tables[0].Rows.Count > 0)
            {
                for (int hf = 0; hf < dsl.Tables[0].Rows.Count; hf++)
                {
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Roll No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Register No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Admission No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Admission"] = "1";
                    }
                }
                settingValueRollAndReg(Convert.ToString(Session["Rollflag"]), Convert.ToString(Session["Regflag"]), Convert.ToString(Session["Admission"]));
            }
        }
        catch { }
    }
    private void settingValueRollAndReg(string rollvalue, string regvalue, string addmis)
    {
        // Tuple<byte, byte>
        string rollval = rollvalue;
        string regval = regvalue;
        string addVal = addmis;
        try
        {
            if (rollval != "" && regval != "")
            {
                if (rollval == "0" && regval == "0" && addVal == "0")
                    roll = 0;
                else if (rollval == "1" && regval == "1" && addVal == "1")
                    roll = 1;
                else if (rollval == "1" && regval == "0" && addVal == "0")
                    roll = 2;
                else if (rollval == "0" && regval == "1" && addVal == "0")
                    roll = 3;
                else if (rollval == "0" && regval == "0" && addVal == "1")
                    roll = 4;
                else if (rollval == "1" && regval == "1" && addVal == "0")
                    roll = 5;
                else if (rollval == "0" && regval == "1" && addVal == "1")
                    roll = 6;
                else if (rollval == "1" && regval == "0" && addVal == "1")
                    roll = 7;
            }
        }
        catch { }
        // return new Tuple<byte, byte>(roll,reg);

    }
    #endregion

    protected void cb_BoardUniv_checkedchange(object sender, EventArgs e)
    {

        {
            txtBoardUniv.Text = "Board";
            if (cb_BoardUniv.Checked)
            {
                for (int i = 0; i < cbl_BoardUniv.Items.Count; i++)
                {
                    cbl_BoardUniv.Items[i].Selected = true;
                }
                txtBoardUniv.Text = "Board(" + cbl_BoardUniv.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < cbl_BoardUniv.Items.Count; i++)
                {
                    cbl_BoardUniv.Items[i].Selected = false;
                }
            }
        }

    }

    protected void cbl_BoardUniv_SelectedIndexChanged(object sender, EventArgs e)
    {
        cb_BoardUniv.Checked = false;
        int cnt = 0;
        for (int i = 0; i < cbl_BoardUniv.Items.Count; i++)
        {
            if (cbl_BoardUniv.Items[i].Selected)
            {
                cnt++;
            }
        }
        if (cnt == cbl_BoardUniv.Items.Count)
        {
            cb_BoardUniv.Checked = true;
        }
        txtBoardUniv.Text = "Board(" + cnt + ")";


    }

    protected void cb_state_checkedchange(object sender, EventArgs e)
    {

        {
            txtstate.Text = "State";
            if (cb_states.Checked)
            {
                for (int i = 0; i < cbl_state.Items.Count; i++)
                {
                    cbl_state.Items[i].Selected = true;
                }
                txtstate.Text = "State(" + cbl_state.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < cbl_state.Items.Count; i++)
                {
                    cbl_state.Items[i].Selected = false;
                }
            }
        }

    }

    protected void cbl_state_SelectedIndexChanged(object sender, EventArgs e)
    {
        cb_states.Checked = false;
        int cnt = 0;
        for (int i = 0; i < cbl_state.Items.Count; i++)
        {
            if (cbl_state.Items[i].Selected)
            {
                cnt++;
            }
        }
        if (cnt == cbl_state.Items.Count)
        {
            cb_states.Checked = true;
        }
        txtstate.Text = "State(" + cnt + ")";


    }

    private void loadBoardUniv()
    {
        cbl_BoardUniv.Items.Clear();
        try
        {
            //degree();
            string batchyear = Convert.ToString(cbl_batch.SelectedItem.Value);
            //string degreecode = Convert.ToString(cbl_degree.SelectedItem.Value);
            string branch = rs.GetSelectedItemsValueAsString(cbl_branch);
            string collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);

            //string query = "  select distinct t.TextVal,p.university_code from Registration R inner join applyn a on r.App_No = a.app_no inner join Stud_prev_details p on p.app_no = r.App_No and isnull(p.university_code,0)<>0 inner join textvaltable t on  isnull(t.TextVal,'')<>'' and textcode=convert(varchar(100),isnull(p.university_code,0)) and r.Batch_Year in('" + batchyear + "') and r.college_code in('" + collegecode + "') ";//and c.type='" + type + "'commented by abarna
            string query = "   select distinct TextVal,TextCode from textvaltable t,Stud_prev_details s,applyn a where T.TextCode= S.course_code and a.app_no=s.app_no and a.batch_year in('" + batchyear + "') and a.degree_code in('" + branch + "')  and t.college_code in(" + collegecode + ") and Textval is not null and Textval<>'' order by Textval asc";//
            // DataSet dsBoardUniv = d2.select_method_wo_parameter(query, "Text");
            DataSet dsBoardUniv = d2.select_method_wo_parameter(query, "Text");// and r.degree_code in('" + degreecode + "') 
            if (dsBoardUniv.Tables.Count > 0 && dsBoardUniv.Tables[0].Rows.Count > 0)
            {
                cbl_BoardUniv.DataSource = dsBoardUniv;
                cbl_BoardUniv.DataTextField = "TextVal";
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
        catch { }
    }

    //private void loadState()//delsij
    //{
    //    cbl_state.Items.Clear();
    //    try
    //    {
    //        string batchyear = Convert.ToString(cbl_batch.SelectedItem.Value);
    //        string degreecode = Convert.ToString(cbl_degree.SelectedItem.Value);
    //        string collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);

    //        string query = "  select distinct t.TextVal,p.uni_state from Registration R inner join applyn a on r.App_No = a.app_no inner join Stud_prev_details p on p.app_no = r.App_No and isnull(p.uni_state,0)<>0 inner join textvaltable t on  isnull(t.TextVal,'')<>'' and textcode=convert(varchar(100),isnull(p.uni_state,0)) and r.Batch_Year in('" + batchyear + "') and r.degree_code in('" + degreecode + "') and r.college_code in('" + collegecode + "')";
    //        DataSet dsBoardUniv = d2.select_method_wo_parameter(query, "Text");
    //        if (dsBoardUniv.Tables.Count > 0 && dsBoardUniv.Tables[0].Rows.Count > 0)
    //        {
    //            cbl_state.DataSource = dsBoardUniv;
    //            cbl_state.DataTextField = "TextVal";
    //            cbl_state.DataValueField = "uni_state";
    //            cbl_state.DataBind();
    //            for (int i = 0; i < cbl_state.Items.Count; i++)
    //            {
    //                cbl_state.Items[i].Selected = true;
    //            }
    //            txtstate.Text = "State(" + cbl_state.Items.Count + ")";
    //            cb_states.Checked = true;
    //        }
    //    }
    //    catch { }
    //}


    public void bindstate()//added
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
                            txtstate.Text = "" + comm + "";
                        }
                        else
                        {
                            txtstate.Text = "State(" + cbl_state.Items.Count + ")";
                        }
                        cb_states.Checked = true;
                    }
                }
            }
            else
            {
                txtstate.Text = "--Select--";
                cb_states.Checked = false;
            }
        }
        catch
        {
        }
    }

    public string getCblSelectedText(CheckBoxList cblSelected)
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

    #region Quota
    protected void loadquota()
    {
        try
        {
            ds.Clear();
            cblQuota.Items.Clear();

            string itemheader = "";
            for (int i = 0; i < cbl_seat.Items.Count; i++)
            {
                if (cbl_seat.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_seat.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_seat.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (itemheader.Trim() != "")
            {
                // string deptquery = "select distinct LedgerName,LedgerPK from FM_LedgerMaster l,FM_HeaderMaster h where l.HeaderFK = h.HeaderPK and l.HeaderFK in('" + itemheader + "') and l.LedgerMode='0' and l.CollegeCode =" + collegecode1 + "";
                // string degree = Convert.ToString(getCblSelectedValue(cbl_dept));
                //string deptquery = "select distinct cat_code,category_name from seattype_cat where quota in('" + itemheader + "') and  degree_code in('" + degree + "')";
                //select distinct quota from seattype_cat where quota in('" + itemheader + "') and college_code='" + collegecode1 + "';

                string deptquery = "select distinct quotaid,quotaname from stu_quotaseetinges where settype in('" + itemheader + "') and  collegecode in('" + ddlcollege.SelectedItem.Value + "')";//abar

                ds.Clear();
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cblQuota.DataSource = ds;
                    cblQuota.DataTextField = "quotaname";
                    cblQuota.DataValueField = "quotaid";
                    cblQuota.DataBind();

                    for (int i = 0; i < cblQuota.Items.Count; i++)
                    {
                        cblQuota.Items[i].Selected = true;
                    }
                    txtQuota.Text = "Quota(" + cblQuota.Items.Count + ")";
                    cbQuota.Checked = true;

                }
                else
                {
                    txtQuota.Text = "--Select--";
                    cbQuota.Checked = false;
                }
            }
            else
            {
                txtQuota.Text = "--Select--";
                cbQuota.Checked = false;
            }
        }
        catch
        {
        }
    }
    protected void cbQuota_checkedchange(object sender, EventArgs e)
    {
        string ledgername = "";
        if (cbQuota.Checked == true)
        {
            for (int i = 0; i < cblQuota.Items.Count; i++)
            {
                cblQuota.Items[i].Selected = true;
                ledgername = Convert.ToString(cblQuota.Items[i].Text);
            }
            if (cblQuota.Items.Count == 1)
            {
                txtQuota.Text = "" + ledgername + "";
            }
            else
            {
                txtQuota.Text = "Quota (" + (cblQuota.Items.Count) + ")";
            }
            // txt_ledger.Text = "Ledger(" + (cbl_ledger.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cblQuota.Items.Count; i++)
            {
                cblQuota.Items[i].Selected = false;
            }
            txtQuota.Text = "--Select--";
        }

    }
    protected void cblQuota_SelectedIndexChange(object sender, EventArgs e)
    {
        string ledgername = "";
        txtQuota.Text = "--Select--";
        cbQuota.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cblQuota.Items.Count; i++)
        {
            if (cblQuota.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                ledgername = Convert.ToString(cblQuota.Items[i].Text);
            }
        }
        if (commcount > 0)
        {
            //   txt_ledger.Text = "Ledger(" + commcount.ToString() + ")";
            if (commcount == cblQuota.Items.Count)
            {
                cbQuota.Checked = true;
            }
            if (commcount == 1)
            {
                txtQuota.Text = "" + ledgername + "";
            }
            else
            {
                txtQuota.Text = "Quota (" + commcount.ToString() + ")";
            }
        }

    }

    #endregion

}
/*03.11.16 velammal change
 08.03.17 gnanamani pdf admission form
 */
