using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class SplhourBatchAllocation : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    string user_code = "";
    string college_code = "";
    string college = "";
    string batch = "";
    string degreevalue = "";
    string semester = "";
    string section = "";
    string selectdate = "";
    string subject_no = "";
    static string[] addnewlist;
    bool testflage = false;
    string userCollegeCode = string.Empty;

    #region
    DataTable dtable = new DataTable();
    DataRow dtrow = null;
    DataTable dtable1 = new DataTable();
    DataRow dtrow1 = null;
    DataTable dttag = new DataTable();
    DataRow drtag = null;
    DataTable dttabletag = new DataTable();
    DataRow dttablerow = null;


    static Hashtable subcode = new Hashtable();
    static Hashtable subno = new Hashtable();
    static Hashtable ddltag = new Hashtable();
    ArrayList addnewlis = new ArrayList();
    static string hour = "";
    static int rowIndex = -1;
    static int selectedCellIndex = -1;
    DataTable dtddl = new DataTable();
    DataRow drddl = null;
    #endregion

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["dtable1"] != null)
            {
                Session.Remove("dtable1");
            }
            if (Session["subcode"] != null)
            {
                Session.Remove("subcode");
            }
        }
        callGridBind();
    }

    public void callGridBind()
    {
        if (Session["dtable1"] != null)
        {
            DataTable dtGrid = (DataTable)Session["dtable1"];
            gview1.DataSource = dtGrid;
            gview1.DataBind();
        }
        else
        {
            gview1.DataSource = null;
            gview1.DataBind();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("AttendanceHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/AttendanceMOD/AttendanceHome.aspx");
                    return;
                }
            }
            user_code = Session["usercode"].ToString();
            college_code = Session["collegecode"].ToString();
            userCollegeCode = Convert.ToString(Session["collegecode"]);
            if (!IsPostBack)
            {
                bindcollege();
                BindBatch();
                degree();
                bindbranch();
                bindsem();
                bindsection();
                btngo.Visible = true;
                subtable.Visible = false;
                gview1.Visible = false;
                Btnsave.Visible = false;
                string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
                ds = d2.select_method_wo_parameter(Master1, "text");
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                    {
                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Student_Type" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                        {
                            Session["Studflag"] = "1";
                        }
                    }
                }
            }
        }
        catch (Exception ex) { }
    }

    public void bindcollege()
    {
        try
        {
            string group_code = Session["group_code"].ToString();
            string columnfield = "";
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.Enabled = true;
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch
        {
        }
    }

    public void BindBatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "Batch_year";
                ddlbatch.DataValueField = "Batch_year";
                ddlbatch.DataBind();
                ddlbatch.SelectedIndex = ddlbatch.Items.Count - 1;
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
            ds.Clear();
            string group_code = Session["group_code"].ToString();
            string columnfield = "";
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            ds = d2.select_method_wo_parameter("select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code='" + ddlcollege.SelectedItem.Value + "' and deptprivilages.Degree_code=degree.Degree_code " + columnfield + " order by degree.Course_Id ", "Text");
            ddldegree.Items.Clear();
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();

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
            ddldepartment.Items.Clear();
            string commname = "";
            string branch = ddldegree.SelectedItem.Value;

            string group_code = Session["group_code"].ToString();
            string columnfield = "";
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }

            if (branch != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code " + columnfield + " order by degree.Degree_code";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code " + columnfield + " order by degree.Degree_code";
            }
            ds = d2.select_method_wo_parameter(commname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldepartment.DataSource = ds;
                ddldepartment.DataTextField = "dept_name";
                ddldepartment.DataValueField = "degree_code";
                ddldepartment.DataBind();
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
            ds.Clear();
            ddlsem.Items.Clear();
            ds = d2.BindSem(ddldepartment.SelectedItem.Value, ddlbatch.SelectedItem.Text, ddlcollege.SelectedItem.Value);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string duration = Convert.ToString(ds.Tables[0].Rows[0][0]);
                if (duration.Trim() != "")
                {
                    for (int i = 1; i <= Convert.ToInt32(duration); i++)
                    {
                        ddlsem.Items.Add(Convert.ToString(i));
                    }
                }

            }
        }
        catch
        {

        }
    }

    public void bindsection()
    {
        try
        {
            ds.Clear();
            ds = d2.BindSectionDetail(ddlbatch.SelectedItem.Text, ddldepartment.SelectedItem.Value);
            ddlsection.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsection.DataSource = ds;
                ddlsection.DataTextField = "sections";
                ddlsection.DataValueField = "sections";
                ddlsection.DataBind();
            }
            if (ddlsection.Items.Count > 0)
            {
                ddlsection.Enabled = true;
            }
            else
            {
                ddlsection.Enabled = false;
            }

        }
        catch
        {

        }
    }

    protected void ddlbatch_Change(object sender, EventArgs e)
    {
        try
        {
            bindsem();
            bindsection();
            btngo.Visible = true;
            mainvlaue.Visible = false;
            subtable.Visible = false;
        }
        catch
        {

        }
    }

    protected void ddldepartment_Change(object sender, EventArgs e)
    {
        bindsem();
        bindsection();
        btngo.Visible = true;
        mainvlaue.Visible = false;
        subtable.Visible = false;
        // fpspread.Visible = false;
        // rptprint.Visible = false;
    }

    protected void ddldegree_Change(object sender, EventArgs e)
    {
        bindbranch();
        bindsem();
        bindsection();
        btngo.Visible = true;
        mainvlaue.Visible = false;
        subtable.Visible = false;
        //fpspread.Visible = false;
        //  rptprint.Visible = false;
    }

    protected void ddlsem_Change(object sender, EventArgs e)
    {
        try
        {
            mainvlaue.Visible = false;
            btngo.Visible = true;
            subtable.Visible = false;
        }
        catch
        {
        }
    }

    protected void ddlsection_Change(object sender, EventArgs e)
    {
        try
        {
            mainvlaue.Visible = false;
            btngo.Visible = true;
            subtable.Visible = false;
        }
        catch
        {
        }
    }

    protected void btngo_click(object sender, EventArgs e)
    {
        try
        {
            college = Convert.ToString(ddlcollege.SelectedItem.Value);
            batch = Convert.ToString(ddlbatch.SelectedItem.Text);
            degreevalue = Convert.ToString(ddldepartment.SelectedItem.Value);
            semester = Convert.ToString(ddlsem.SelectedItem.Text);
            section = "";
            string sectionquery = "";
            if (ddlsection.Enabled == true)
            {
                section = Convert.ToString(ddlsection.SelectedItem.Text);
                sectionquery = "and sections='" + section + "'";
            }
            string selectquery = "select CONVERT(varchar(10), date,103) as date,hrentry_no  from specialhr_master where degree_code =" + degreevalue + " and semester =" + semester + " and batch_year =" + batch + " " + sectionquery + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlspecialdate.DataSource = ds;
                ddlspecialdate.DataTextField = "date";
                ddlspecialdate.DataValueField = "hrentry_no";
                ddlspecialdate.DataBind();
                if (ddlspecialdate.Items.Count > 0)
                {
                    string subjecquery = "select distinct sh.subject_no,subject_name  from specialhr_details sh,subject s where sh.subject_no =s.subject_no  and hrentry_no  in ('" + ddlspecialdate.SelectedItem.Value + "')";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(subjecquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlsubject.DataSource = ds;
                        ddlsubject.DataTextField = "subject_name";
                        ddlsubject.DataValueField = "subject_no";
                        ddlsubject.DataBind();
                    }

                }
                subtable.Visible = true;
                errorlable.Visible = false;
                btngo.Visible = false;
            }
            else
            {
                subtable.Visible = false;
                errorlable.Text = "No Records Found";
                errorlable.Visible = true;
                mainvlaue.Visible = false;
                btngo.Visible = true;
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Special Hour Batch Allocation"); }
    }

    protected void ddlspecialdate_Change(object sender, EventArgs e)
    {
        try
        {
            string subjecquery = "select distinct sh.subject_no,subject_name  from specialhr_details sh,subject s where sh.subject_no =s.subject_no  and hrentry_no  in ('" + ddlspecialdate.SelectedItem.Value + "') ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(subjecquery, "Text");
            ddlsubject.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsubject.DataSource = ds;
                ddlsubject.DataTextField = "subject_name";
                ddlsubject.DataValueField = "subject_no";
                ddlsubject.DataBind();
            }
        }
        catch
        {

        }
    }

    protected void btnallocate_click(object sender, EventArgs e)
    {
        try
        {
            addnewlis.Clear();
            subcode.Clear();
            subno.Clear();
            ddltag.Clear();
            int sno = 0;
            selectdate = Convert.ToString(ddlspecialdate.SelectedItem.Text);
            subject_no = Convert.ToString(ddlsubject.SelectedItem.Value);
            college = Convert.ToString(ddlcollege.SelectedItem.Value);
            batch = Convert.ToString(ddlbatch.SelectedItem.Text);
            degreevalue = Convert.ToString(ddldepartment.SelectedItem.Value);
            semester = Convert.ToString(ddlsem.SelectedItem.Text);
            section = "";
            string sectionquery = "";
            if (ddlsection.Enabled == true)
            {
                section = Convert.ToString(ddlsection.SelectedItem.Text);
                sectionquery = "and sections='" + section + "'";
            }
            if (txt_noofbatchs.Text.Trim() != "")
            {
                ddllabbatch.Items.Clear();
                ddllabbatch.Items.Add("Select");
                int ch = 0;
                addnewlist = new string[Convert.ToInt32(txt_noofbatchs.Text)];
                dtddl.Columns.Add("Batch");
                for (int i = 1; i <= Convert.ToInt32(txt_noofbatchs.Text); i++)
                {
                    ddllabbatch.Items.Add("B" + i + "");
                    addnewlist[ch] = ("B" + i + "");
                    addnewlis.Add("B" + i + "");

                    drddl = dtddl.NewRow();
                    drddl["Batch"] = "B" + i + "";
                    dtddl.Rows.Add(drddl);
                    ch++;
                }
            }

            DataSet ds_batch = new DataSet();
            string batchcomboxquery = "select distinct subjectChooser_New_Spl.batch as batch from subjectChooser_New_Spl,Registration where subjectChooser_New_Spl.roll_no= registration.roll_no and semester ='" + ddlsem.SelectedItem.Text + "' and  registration.degree_Code = '" + ddldepartment.SelectedItem.Value + "' and registration.batch_year = '" + ddlbatch.SelectedItem.Text + "' " + sectionquery + " and batch<>''";
            ds_batch = d2.select_method_wo_parameter(batchcomboxquery, "text");

            if (ds_batch.Tables[0].Rows.Count > 0)
            {
                cbbatchlist.DataSource = ds_batch;
                cbbatchlist.DataValueField = "batch";
                cbbatchlist.DataTextField = "batch";
                cbbatchlist.DataBind();
            }

            string islab = d2.GetFunction("select sm.Lab  from subject s,sub_sem sm where s.subType_no =sm.subType_no and s.subject_no ='" + subject_no + "'");
            if (islab.Trim() == "1" || islab.Trim() == "True")
            {
                string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
                string strorder = "ORDER BY Roll_No";
                if (orderby_Setting == "0")
                {
                    strorder = "ORDER BY Roll_No";
                }
                else if (orderby_Setting == "1")
                {
                    strorder = "ORDER BY Reg_No";
                }
                else if (orderby_Setting == "2")
                {
                    strorder = "ORDER BY Stud_Name";
                }
                else if (orderby_Setting == "0,1,2")
                {
                    strorder = "ORDER BY Roll_No,Reg_No,Stud_Name";
                }
                else if (orderby_Setting == "0,1")
                {
                    strorder = "ORDER BY Roll_No,Reg_No";
                }
                else if (orderby_Setting == "1,2")
                {
                    strorder = "ORDER BY Reg_No,Stud_Name";
                }
                else if (orderby_Setting == "0,2")
                {
                    strorder = "ORDER BY Roll_No,Stud_Name";
                }
                string stu_namequery = "select  roll_no as rollno,Reg_No , stud_name as studentname  from registration where degree_code='" + degreevalue + "' and batch_year='" + batch + "' " + sectionquery + " and current_semester='" + semester + "' and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' " + strorder + "";
                ds.Clear();
                ds = d2.select_method_wo_parameter(stu_namequery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {

                    ArrayList batchadd = new ArrayList();

                    dtable.Columns.Add("Roll No");
                    dtable.Columns.Add("Reg No");
                    dtable.Columns.Add("Student Name");
                    dtable.Columns.Add("Batch");

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        dtrow = dtable.NewRow();

                        dtrow["Roll No"] = Convert.ToString(ds.Tables[0].Rows[i]["rollno"]);
                        dtrow["Reg No"] = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                        dtrow["Student Name"] = Convert.ToString(ds.Tables[0].Rows[i]["studentname"]);

                        string selectedbatch = "select distinct batch from subjectChooser_New_Spl where roll_no='" + Convert.ToString(ds.Tables[0].Rows[i]["rollno"]) + "' and semester='" + ddlsem.SelectedItem.Text + "' and batch is not null and batch<>''";
                        DataSet ds_selebatch = new DataSet();
                        ds_selebatch = d2.select_method_wo_parameter(selectedbatch, "text");
                        string bat = "";
                        if (ds_selebatch.Tables[0].Rows.Count > 0)
                        {
                            bat = ds_selebatch.Tables[0].Rows[0]["batch"].ToString();
                            if (!batchadd.Contains(bat))
                            {
                                batchadd.Add(Convert.ToString(bat));
                            }
                        }
                        if (bat == "")
                        {
                            dtrow["Batch"] = "";
                        }
                        else
                        {
                            dtrow["Batch"] = bat;
                        }
                        dtable.Rows.Add(dtrow);
                    }
                    mainvlaue.Visible = true;
                    gview.DataSource = dtable;
                    gview.DataBind();
                    gview.Visible = true;
                    errorlable.Visible = false;
                    Fieldset2.Visible = true;

                    if (ddllabbatch.Items.Count > 0)
                    {
                        for (int row = 0; row < gview.Rows.Count; row++)
                        {
                            DropDownList ddl = (gview.Rows[row].FindControl("ddlbatch") as DropDownList);
                            ddl.DataSource = dtddl;
                            ddl.DataTextField = "Batch";
                            ddl.DataValueField = "Batch";
                            ddl.DataBind();
                            ddl.Items.Insert(0, "");

                            string str = (gview.Rows[row].FindControl("lblbatch") as Label).Text;
                            (gview.Rows[row].FindControl("ddlbatch") as DropDownList).SelectedIndex = (gview.Rows[row].FindControl("ddlbatch") as DropDownList).Items.IndexOf((gview.Rows[row].FindControl("ddlbatch") as DropDownList).Items.FindByValue(str));
                            if (ddl.SelectedItem != null)
                            {
                                if (ddl.SelectedItem.Text != str)
                                {
                                    (gview.Rows[row].FindControl("ddlbatch") as DropDownList).SelectedIndex = (gview.Rows[row].FindControl("ddlbatch") as DropDownList).Items.IndexOf((gview.Rows[row].FindControl("ddlbatch") as DropDownList).Items.FindByValue(string.Empty));
                                }
                            }
                        }
                    }

                    if (gview.Rows.Count > 0)
                    {
                        Btnsave.Enabled = false;
                        Btndelete.Enabled = true;
                    }


                    if (Session["Rollflag"].ToString() == "1")
                    {
                        gview.Columns[2].Visible = true;
                    }
                    else
                    {
                        gview.Columns[2].Visible = false;
                    }
                    if (Session["Regflag"].ToString() == "1")
                    {
                        gview.Columns[3].Visible = true;
                    }
                    else
                    {
                        gview.Columns[3].Visible = false;
                    }

                    if (ddlspecialdate.Items.Count > 0)
                    {
                        //dtable1.Columns.Add("S.No");
                        dtable1.Columns.Add("Day"); dttabletag.Columns.Add("Day");
                        dtable1.Columns.Add("Hour"); dttabletag.Columns.Add("Hour");

                        if (txt_noofbatchs.Text.Trim() != "")
                        {
                            Session["addnewlis"] = addnewlis;
                        }
                        else
                        {
                        }
                        string subjecquery = "  select distinct sh.subject_no,subject_name,subject_code ,start_time,end_time,date,sh.staff_code  from specialhr_details sh,subject s ,specialhr_master m ,sub_sem su  where sh.subject_no =s.subject_no and su.subType_no =s.subType_no and Lab='1' and  sh.subject_no =s.subject_no and m.hrentry_no =sh.hrentry_no   and sh.hrentry_no  in ('" + ddlspecialdate.SelectedItem.Value + "')";
                        subjecquery = subjecquery + "   select distinct subject_code,sh.subject_no  from specialhr_details sh,subject s,sub_sem su where sh.subject_no =s.subject_no  and su.subType_no =s.subType_no and Lab='1'  and hrentry_no  in ('" + ddlspecialdate.SelectedItem.Value + "')";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(subjecquery, "Text");
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            int i = dtable1.Columns.Count - 1;
                            for (int k = 0; k < ds.Tables[1].Rows.Count; k++)
                            {
                                i++;

                                dtable1.Columns.Add(Convert.ToString(ds.Tables[1].Rows[k]["subject_code"]));
                                dttabletag.Columns.Add(Convert.ToString(ds.Tables[1].Rows[k]["subject_no"]));
                                subcode.Add(i, Convert.ToString(ds.Tables[1].Rows[k]["subject_code"]));
                                subno.Add(Convert.ToString(ds.Tables[1].Rows[k]["subject_code"]), Convert.ToString(ds.Tables[1].Rows[k]["subject_no"]));
                            }
                            Session["subcode"] = subcode;
                        }

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataView dnew = new DataView();
                            ArrayList addarray = new ArrayList();
                            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                            {
                                string date = Convert.ToString(ds.Tables[0].Rows[k]["date"]);
                                DateTime dt = Convert.ToDateTime(date);
                                string dayvalue = dt.ToString("ddd");
                                DateTime dt1 = Convert.ToDateTime(ds.Tables[0].Rows[k]["start_time"]);
                                string start = dt1.ToString("hh:mm");
                                DateTime dt2 = Convert.ToDateTime(ds.Tables[0].Rows[k]["end_time"]);
                                string end = dt2.ToString("hh:mm");
                                string hour = start + "-" + end;

                                if (!addarray.Contains(hour))
                                {

                                    sno++;
                                    dtrow1 = dtable1.NewRow();
                                    addarray.Add(hour);
                                    int col = 2;

                                    //dtrow1["S.No"] = Convert.ToString(sno);
                                    dtrow1["Day"] = dt.ToString("ddd");
                                    dtrow1["Hour"] = Convert.ToString(hour);
                                    for (int k1 = 0; k1 < ds.Tables[1].Rows.Count; k1++)
                                    {


                                        ds.Tables[0].DefaultView.RowFilter = "subject_no='" + Convert.ToString(ds.Tables[1].Rows[k1]["subject_no"]) + "' and start_time ='" + dt1 + "' and end_time='" + dt2 + "' ";
                                        dnew = ds.Tables[0].DefaultView;
                                        if (dnew.Count > 0)
                                        {
                                            dt = Convert.ToDateTime(dnew[0]["start_time"]);
                                            string start1 = dt.ToString("hh:mm");
                                            dt = Convert.ToDateTime(dnew[0]["end_time"]);
                                            string end1 = dt.ToString("hh:mm");
                                            string hour2 = start1 + "-" + end1;
                                            string staff_code = Convert.ToString(dnew[0]["staff_code"]);
                                            if (hour == hour2)
                                            {

                                                ddltag.Add(col, Convert.ToString(staff_code));
                                                col++;
                                                string getvalue = d2.GetFunction("select Stu_Batch  from LabAlloc_New_Spl where Degree_Code ='" + degreevalue + "' and Batch_Year=" + batch + " and Semester=" + semester + " and Subject_No='" + Convert.ToString(ds.Tables[1].Rows[k1]["subject_no"]) + "' and Day_Value ='" + dayvalue + "' and Hour_Value ='" + hour + "' and fdate ='" + date + "' and Staff_Code ='" + staff_code + "' " + sectionquery + "");
                                                if (getvalue.Trim() != "" && getvalue.Trim() != "0")
                                                {
                                                    if (getvalue.Contains(',') == true)
                                                    {
                                                        //if (batchadd.Contains(Convert.ToString(getvalue)))
                                                        //{

                                                        //}
                                                    }
                                                    else
                                                    {
                                                        if (batchadd.Contains(Convert.ToString(getvalue)))
                                                        {
                                                            //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Text = getvalue;
                                                            //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].BackColor = Color.LightBlue;
                                                        }
                                                        else
                                                        {
                                                            //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].BackColor = Color.LightBlue;
                                                        }

                                                    }
                                                }
                                                else
                                                {
                                                    //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].BackColor = Color.LightBlue;
                                                }
                                            }
                                            else
                                            {
                                                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Locked = true;
                                            }
                                        }
                                        else
                                        {
                                            //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Locked = true;
                                        }
                                    }
                                    dtable1.Rows.Add(dtrow1);
                                }
                            }
                        }
                        Session["staffcode"] = ddltag;
                        gview1.Visible = false;
                        btnbatchsave.Visible = true;
                        lnkmultiple.Visible = false;
                        mainvlaue.Visible = true;
                        Session["dttabletag"] = dttabletag;
                        Session["dtable1"] = dtable1;
                        gview1.DataSource = dtable1;
                        gview1.DataBind();
                        gview1.Visible = true;
                        DataTable data = new DataTable(); data.Columns.Add("batch");
                        DataRow droww = null;

                        if (gview1.Rows.Count > 0)
                        {
                            if (ddllabbatch.Items.Count > 1)
                            {
                                for (int i = 1; i < ddllabbatch.Items.Count; i++)
                                {
                                    droww = data.NewRow();
                                    droww["batch"] = ddllabbatch.Items[i].Text;
                                    data.Rows.Add(droww);
                                }
                                for (int row = 0; row < gview1.Rows.Count; row++)
                                {
                                    for (int cell = 1; cell < gview1.Rows[row].Cells.Count - 2; cell++)
                                    {
                                        (gview1.Rows[row].FindControl("Chklst_lab" + cell) as CheckBoxList).DataSource = data;
                                        (gview1.Rows[row].FindControl("Chklst_lab" + cell) as CheckBoxList).DataTextField = "batch";
                                        (gview1.Rows[row].FindControl("Chklst_lab" + cell) as CheckBoxList).DataValueField = "batch";
                                        (gview1.Rows[row].FindControl("Chklst_lab" + cell) as CheckBoxList).DataBind();
                                    }
                                }
                            }
                        }

                        for (int vis = dtable1.Columns.Count + 1; vis < gview1.HeaderRow.Cells.Count; vis++)
                        {
                            gview1.Columns[vis].Visible = false;
                        }

                        for (int row = 0; row < gview.Rows.Count; row++)
                        {
                            //gview.Rows[row].Cells[5].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
                else
                {
                    gview.Visible = false;
                    errorlable.Text = "No Records Found";
                    errorlable.Visible = true;
                    Fieldset2.Visible = false;
                    mainvlaue.Visible = false;
                }
            }
            else
            {
                gview.Visible = false;
                errorlable.Text = "This is Not a Lab Subject";
                errorlable.Visible = true;
                Fieldset2.Visible = false;
                mainvlaue.Visible = false;
            }
        }
        catch (Exception ex) { }//d2.sendErrorMail(ex, userCollegeCode, "Special Hour Batch Allocation"); }
    }

    protected void gview1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                DataTable dtable1 = (DataTable)Session["dtable1"];
                for (int cell = 3; cell <= dtable1.Columns.Count; cell++)
                {
                    if (gview1.Columns[cell].Visible == true)
                    {
                        e.Row.Cells[cell].Text = Convert.ToString(dtable1.Columns[cell - 1].ColumnName);

                    }
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Special Hour Batch Allocation"); }
    }

    protected void selectgo_Click(object sender, EventArgs e)
    {
        string from = fromno.Text;
        string to = tono.Text;
        errorlable.Visible = false;
        string batch = string.Empty;
        if (ddllabbatch.Text != "Select" && ddllabbatch.Text != "-1" && ddllabbatch.Text != "")
        {
            if (from != null && from != "" && to != null && to != "")
            {
                int m = Convert.ToInt32(fromno.Text);
                int n = Convert.ToInt32(tono.Text);
                if (m != 0 && n != 0)
                {
                    if (gview.Rows.Count >= n)
                    {
                        batch = ddllabbatch.Text;
                        for (int rowcount = m; rowcount <= n; rowcount++)
                        {
                            if (txt_noofbatchs.Text != "" && txt_noofbatchs.Text != "0" && txt_noofbatchs.Text != null && ddllabbatch.SelectedItem.ToString() != null && ddllabbatch.SelectedItem.ToString() != "" && ddllabbatch.SelectedItem.ToString() != "--Select--")
                            {
                                DropDownList ddl = (gview.Rows[rowcount - 1].FindControl("ddlbatch") as DropDownList);
                                (gview.Rows[rowcount - 1].FindControl("ddlbatch") as DropDownList).SelectedIndex = (gview.Rows[rowcount - 1].FindControl("ddlbatch") as DropDownList).Items.IndexOf((gview.Rows[rowcount - 1].FindControl("ddlbatch") as DropDownList).Items.FindByValue(batch));

                                Btnsave.Enabled = false;
                                Btndelete.Enabled = true;
                            }
                            else
                            {
                                errorlable.Visible = true;
                                errorlable.Text = "Please Add No of Batch";
                            }
                        }
                    }
                    else
                    {
                        errorlable.Visible = true;
                        errorlable.Text = "Please Enter Available Student Count";
                    }
                }
                else
                {
                    errorlable.Visible = true;
                    errorlable.Text = "Please Enter Greater than Zero";
                }
            }
            else
            {
                errorlable.Visible = true;
                errorlable.Text = "Please Enter Values";
            }
        }
        else
        {
            errorlable.Visible = true;
            errorlable.Text = "Please Select Batch";
        }
        fromno.Text = string.Empty;
        tono.Text = string.Empty;
    }

    protected void Btnsave_Click(object sender, EventArgs e)
    {
        errorlable.Visible = false;
        DataTable dt = new DataTable();
        string selectedbatch=string.Empty;
        if (gview.Rows.Count > 0)
        {
            //if (ddllabbatch.SelectedItem.Text != "Select")
            //{
                dt = (DataTable)Session["dttabletag"];
                testflage = false;

                string subjectvalue = "";
                if (gview1.Columns.Count > 0)
                {
                    for (int j = 2; j < dt.Columns.Count; j++)
                    {
                        //string subcod = Convert.ToString(subcode[j]);
                        //string gettag = Convert.ToString(subno[subcod]);
                        string gettag = dt.Columns[j].ColumnName;

                        if (subjectvalue.Trim() == "")
                        {
                            subjectvalue = gettag;
                        }
                        else
                        {
                            subjectvalue = subjectvalue + "," + gettag;
                        }
                    }
                }

                for (int jk = 0; jk < gview.Rows.Count; jk++)
                {
                    DropDownList ddl = (gview.Rows[jk].FindControl("ddlbatch") as DropDownList);
                    selectedbatch = ddl.SelectedItem.Text;
                    string rollno = (gview.Rows[jk].FindControl("lblroll")as Label).Text;
                    //CheckBox chk = (CheckBox)gview.Rows[jk].FindControl("chck");
                    //if (chk.Checked)
                    //{
                        ds.Clear();
                        string batchsql = "select * from subjectchooser,sub_sem,subject where subjectchooser.roll_no='" + rollno + "' and semester = '" + ddlsem.SelectedItem.Text + "' and subjectchooser.subject_no in(" + subjectvalue + ")  and subjectchooser.subtype_no=sub_sem.subtype_no and subjectchooser.subject_no=subject.subject_no";
                        ds = d2.select_method_wo_parameter(batchsql, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int subno = 0; subno < ds.Tables[0].Rows.Count; subno++)
                            {
                                testflage = true;
                                string ssub_no = ds.Tables[0].Rows[subno]["subject_no"].ToString();
                                string paper_order = ds.Tables[0].Rows[subno]["paper_order"].ToString();
                                string subtype = ds.Tables[0].Rows[subno]["subtype_no"].ToString();
                                
                                string updatquery = " if exists (select * from subjectChooser_New_Spl where roll_no='" + rollno + "' and subject_no='" + ssub_no.ToString() + "')";
                                updatquery = updatquery + " update subjectChooser_New_Spl set batch ='" + selectedbatch + "' where roll_no='" + rollno + "' and subject_no='" + ssub_no.ToString() + "' else ";
                                updatquery = updatquery + " insert into subjectChooser_New_Spl(semester,roll_no,subject_no,paper_order,subtype_no,Batch) values('" + ddlsem.SelectedItem.Text + "','" + rollno + "','" + ssub_no.ToString() + "','" + paper_order + "','" + subtype + "','" + selectedbatch + "')";
                                int u = d2.update_method_wo_parameter(updatquery, "Text");
                            }
                        }
                    //}
                }
                //btnallocate_click(sender, e);
                //if (testflage == true)
                //{
                //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('saved successfully')", true);
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Select Student and then Proceed')", true);
                //}
            //}
            //else
            //{
            //    errorlable.Visible = true;
            //    errorlable.Text = "Please Select Batch";
            //}
        }
    }

    protected void Btndelete_Click(object sender, EventArgs e)
    {
        if (gview.Rows.Count > 0)
        {
            testflage = false;
            for (int jk = 0; jk < gview.Rows.Count; jk++)
            {
                //CheckBox chk = (CheckBox)gview.Rows[jk].FindControl("chck");
                //if (chk.Checked)
                //{
                    testflage = true;
                    DropDownList ddl = (gview.Rows[jk].FindControl("ddlbatch") as DropDownList);
                    string rollno = (gview.Rows[jk].FindControl("lblroll")as Label).Text;
                    string deletbatch = "update subjectChooser_New_Spl set batch ='' where roll_no='" + rollno + "' and semester='" + ddlsem.SelectedItem.Text + "' ";
                    int d = d2.update_method_wo_parameter(deletbatch, "Text");
                    (gview.Rows[jk].FindControl("ddlbatch") as DropDownList).SelectedIndex = (gview.Rows[jk].FindControl("ddlbatch") as DropDownList).Items.IndexOf((gview.Rows[jk].FindControl("ddlbatch") as DropDownList).Items.FindByValue(string.Empty));
                //}
            }
            if (testflage == true)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted successfully')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('There is no student to delete batch')", true);
        }
    }

    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox1.Checked == true)
            {
                fromno.Enabled = true;
                tono.Enabled = true;
                Btnsave.Enabled = false;
                Btndelete.Enabled = true;
            }
            if (CheckBox1.Checked == false)
            {
                fromno.Enabled = false;
                tono.Enabled = false;
                Btnsave.Enabled = false;
                Btndelete.Enabled = true;
            }
        }
        catch
        {

        }
    }

    protected void Batchallotsave_Click(object sender, EventArgs e)
    {
        try
        {
            Btnsave_Click(sender, e);
            if (gview1.Rows.Count > 0)
            {
                DataTable dt = (DataTable)Session["dttabletag"];
                Hashtable Shas = (Hashtable)Session["staffcode"];
                section = "";
                testflage = false;
                string sectionquery = "";
                if (ddlsection.Enabled == true)
                {
                    section = Convert.ToString(ddlsection.SelectedItem.Text);
                    sectionquery = "and sections='" + section + "'";
                }

                foreach (GridViewRow row in gview1.Rows)
                {
                    string dayvalue = (row.FindControl("lbldate") as Label).Text;
                    string hourvalue = (row.FindControl("lblhour") as Label).Text;
                    string date = Convert.ToString(ddlspecialdate.SelectedItem.Text);
                    string[] splitdate = date.Split('/');
                    date = Convert.ToString(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);

                    if (gview1.HeaderRow.Cells.Count > 3)
                    {
                        int col = 1;
                        for (int jk = 3; jk < gview1.Columns.Count; jk++)
                        {
                            if (gview1.Columns[jk].Visible == true)
                            {
                                col++;
                                testflage = true;

                                string subcod = gview1.HeaderRow.Cells[jk].Text;
                                string subjectno = dt.Columns[col].ColumnName;
                                string staff_code = Convert.ToString(Shas[col]);
                                string batch_value = string.Empty;

                                CheckBoxList ddlMode = (CheckBoxList)row.FindControl("Chklst_lab" + (jk - 2));

                                for (int val = 0; val < ddlMode.Items.Count; val++)
                                {
                                    if (ddlMode.Items[val].Selected)
                                    {
                                        if (batch_value == "")
                                        {
                                            batch_value = ddlMode.Items[val].Text.Trim();
                                        }
                                        else
                                        {
                                            batch_value = batch_value + "," + ddlMode.Items[val].Text.Trim();
                                        }
                                    }
                                }
                                ddlMode.ClearSelection();
                                CheckBox chk = (row.FindControl("chk_lab" + (jk - 2)) as CheckBox);
                                chk.Checked = false;
                                if (batch_value.Trim() != "")
                                {
                                    string insertquery = "if not exists ( select * from LabAlloc_New_Spl where Degree_Code ='" + ddldepartment.SelectedItem.Value + "' and Batch_Year=" + ddlbatch.SelectedItem.Text + " and Semester=" + ddlsem.SelectedItem.Text + " and Subject_No='" + subjectno + "' and Day_Value ='" + dayvalue + "' and Hour_Value ='" + hourvalue + "' and fdate ='" + date + "' and Staff_Code ='" + staff_code + "' " + sectionquery + ") insert into  LabAlloc_New_Spl (Degree_Code,Semester,Batch_Year,Subject_No,Day_Value,Hour_Value,Stu_Batch,Staff_Code ,Sections ,fdate) values ('" + ddldepartment.SelectedItem.Value + "','" + ddlsem.SelectedItem.Text + "','" + ddlbatch.SelectedItem.Text + "','" + subjectno + "','" + dayvalue + "','" + hourvalue + "','" + batch_value + "','" + staff_code + "','" + section + "','" + date + "') else update LabAlloc_New_Spl set Stu_Batch ='" + batch_value + "' where Degree_Code ='" + ddldepartment.SelectedItem.Value + "' and Batch_Year=" + ddlbatch.SelectedItem.Text + " and Semester=" + ddlsem.SelectedItem.Text + " and Subject_No='" + subjectno + "' and Day_Value ='" + dayvalue + "' and Hour_Value ='" + hourvalue + "' and fdate ='" + date + "' and Staff_Code ='" + staff_code + "' " + sectionquery + " ";
                                    int up = d2.update_method_wo_parameter(insertquery, "Text");
                                }
                            }
                        }
                    }
                }
                if (testflage == true)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
                    btnallocate_click(sender, e);
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Special Hour Batch Allocation"); }
    }

    protected void OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //for (int i = 1; i < e.Row.Cells.Count; i++)
            //{
            //TableCell cell = e.Row.Cells[2];
            //cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
            //cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
            //cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
            //   , SelectedGridCellIndex.ClientID, 2
            //   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            //}
        }
    }

    protected void SelectedIndexChanged(Object sender, EventArgs e)
    {
        var grid = (GridView)sender;
        GridViewRow selectedRow = grid.SelectedRow;
        rowIndex = grid.SelectedIndex;//RAY
        //selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);

        hour = Convert.ToString(gview1.Rows[rowIndex].Cells[selectedCellIndex].Text);
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
    }

    protected void btnsub_Clcik(object sender, EventArgs e)
    {
    }

    protected void ddlcollege_change(object sender, EventArgs e)
    {
        try
        {
            BindBatch();
            degree();
            bindbranch();
            bindsem();
            bindsection();
            btngo.Visible = true;
            mainvlaue.Visible = false;
            subtable.Visible = false;
        }
        catch
        {

        }
    }

    protected void checklab1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            string txt_lab = Convert.ToString(gview1.Rows[0].FindControl("txt_lab1"));
            CheckBox chkall = (CheckBox)gview1.Rows[0].FindControl("chk_lab1");
            

            CheckBoxList lis = (CheckBoxList)gview1.Rows[0].FindControl("Chklst_lab1");

            CheckBox ddlLabTest = (CheckBox)sender;
            var row = ddlLabTest.NamingContainer;
            string rowIndxS = ddlLabTest.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
            int rowIndx = Convert.ToInt32(rowIndxS) - 2;
            string colIndxS = ddlLabTest.UniqueID.ToString().Split('$')[4].Replace("chkPeriod", string.Empty);
            int colIndx = Convert.ToInt32(colIndxS);
            CheckBox ddlAddLabTestShortName = (CheckBox)row.FindControl("chk_lab" + colIndx);
            CheckBoxList cbl = (CheckBoxList)row.FindControl("Chklst_lab" + colIndx);
            TextBox txtB = (TextBox)row.FindControl("txt_lab" + colIndx);
            
        }
        catch (Exception ex)
        {
            //lbl_err.Visible = true;
            //lbl_err.Text = ex.ToString();
        }
    }

    protected void cheklist_Degree_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    public void clear()
    {

    }

    protected void chkBranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox ddlLabTest = (CheckBox)sender;
            var row = ddlLabTest.NamingContainer;
            string rowIndxS = ddlLabTest.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
            int rowIndx = Convert.ToInt32(rowIndxS) - 2;
            string colIndxS = ddlLabTest.UniqueID.ToString().Split('$')[4].Replace("chk_lab", string.Empty);
            int colIndx = Convert.ToInt32(colIndxS);
            CheckBox ddlAddLabTestShortName = (CheckBox)row.FindControl("chk_lab" + colIndx);
            CheckBoxList cbl = (CheckBoxList)row.FindControl("Chklst_lab" + colIndx);
            TextBox txtB = (TextBox)row.FindControl("txt_lab" + colIndx);
            CallCheckboxChange(ddlAddLabTestShortName, cbl, txtB, "Batch", "--Select--");
        }
        catch
        {
        }
    }
    protected void cblBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBoxList ddlLabTest = (CheckBoxList)sender;
            var row = ddlLabTest.NamingContainer;
            string rowIndxS = ddlLabTest.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
            int rowIndx = Convert.ToInt32(rowIndxS) - 2;
            string colIndxS = ddlLabTest.UniqueID.ToString().Split('$')[4].Replace("Chklst_lab", string.Empty);
            int colIndx = Convert.ToInt32(colIndxS);
            CheckBox ddlAddLabTestShortName = (CheckBox)row.FindControl("chk_lab" + colIndx);
            CheckBoxList cbl = (CheckBoxList)row.FindControl("Chklst_lab" + colIndx);
            TextBox txtB = (TextBox)row.FindControl("txt_lab" + colIndx);
            CallCheckboxListChange(ddlAddLabTestShortName, cbl, txtB, "Batch", "--Select--");
        }
        catch
        {
        }
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
                    if (name == string.Empty)
                    {
                        name = Convert.ToString(cbl.Items[sel].Text);
                    }
                    else
                    {
                        name = name + "," + Convert.ToString(cbl.Items[sel].Text);
                    }
                }
                txt.Text = name;
                //if (cbl.Items.Count == 1)
                //{
                //    txt.Text = "" + name + "";
                //}
                //else
                //{
                //    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                //}
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
            string name = string.Empty;
            cb.Checked = false;
            txt.Text = deft;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                    if (name == string.Empty)
                    {
                        name = Convert.ToString(cbl.Items[sel].Text);
                    }
                    else
                    {
                        name = name + "," + Convert.ToString(cbl.Items[sel].Text);
                    }
                }
            }
            txt.Text = name;
            //if (count > 0)
            //{
            //    if (count == 1)
            //    {
            //        txt.Text = "" + name + "";
            //    }
            //    else
            //    {
            //        txt.Text = dipst + "(" + count + ")";
            //    }
            //    if (cbl.Items.Count == count)
            //    {
            //        cb.Checked = true;
            //    }
            //}
        }
        catch { }
    }
}

