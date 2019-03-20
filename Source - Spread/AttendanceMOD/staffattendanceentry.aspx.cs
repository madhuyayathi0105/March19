using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class staffattendanceentry : System.Web.UI.Page
{
    DAccess2 dacc = new DAccess2();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    string grouporusercode  = string.Empty;
    string group_user = "", singleuser = "", usercode = "", collegecode  = string.Empty;
    Hashtable hat = new Hashtable();
    string strquery  = string.Empty;
    Boolean flag_true = false;
    int Att_mark_column = 0, present_count = 0, count_master = 0, Att_mark_row = 0, absent_count = 0;
    Hashtable present_calcflag = new Hashtable();
    Hashtable absent_calcflag = new Hashtable();
    DataSet ds_attndmaster = new DataSet();
    Boolean nullflag = false;
    static string minimum_day  = string.Empty;
    static string collegename  = string.Empty;
    static string coursename  = string.Empty;
    string SenderID = string.Empty;
    string Password = string.Empty;

    //added by rajasekar 18/11/2018
    DataTable dtl = new DataTable();
    DataTable dtl1 = new DataTable();
    DataTable dtGrid = new DataTable();
    DataRow dtrow = null;
    ArrayList colofvisfalse = new ArrayList();
    ArrayList rowsofcolour = new ArrayList();
    string tagof0 = "";
    string noteof1 = "";
    

    //=================================//
    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["dtGrid"] != null)
            {
                Session.Remove("dtGrid");
            }
            if (Session["arrColHdrNames2"] != null)
            {
                Session.Remove("arrColHdrNames2");
            }
        }
        callGridBind();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblholireason.Visible = false;
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
            errmsg.Visible = false;
            if (!Page.IsPostBack)
            {
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                panhrdetails.Visible = false;
                btnsave.Visible = false;
                btndeselect.Visible = false;
                btnselect.Visible = false;
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                btnsave.Visible = false;


                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                string Master = "select * from Master_Settings where " + grouporusercode + "";
                DataSet ds = dacc.select_method(Master, hat, "Text");
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
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Student_Type" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Studflag"] = "1";
                    }
                }
                bindbatch();
                binddegree();
                if (ddldegree.Items.Count > 0)
                {
                    bindbranch();
                    bindsem();
                    bindsec();
                    load_subject();
                    loadhours();
                }
                else
                {
                    ddldegree.Enabled = false;
                    ddlbranch.Enabled = false;
                    ddlduration.Enabled = false;
                    ddlsec.Enabled = false;
                    txtFromDate.Enabled = false;
                }
                if (Session["Staff_Code"] != null && Session["Staff_Code"].ToString() != "")
                {
                    lblstaff.Visible = false;
                    txtstaff.Visible = false;
                    pstaff.Visible = false;
                    chklsstaff.Items.Insert(0, "" + Session["Staff_Code"] + "");
                }
            }
        }
        catch (Exception ex) { }
    }

    public void bindbatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            ds = dacc.select_method_wo_parameter("bind_batch", "sp");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();
            }
            int count1 = ds.Tables[1].Rows.Count;
            if (count > 0)
            {
                int max_bat = 0;
                max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
                ddlbatch.SelectedValue = max_bat.ToString();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Session["collegecode"].ToString(), "staffattendanceentry");
        }
    }

    public void binddegree()
    {
        try
        {
            ddldegree.Items.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Clear();
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds = dacc.select_method("bind_degree", hat, "sp");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Session["collegecode"].ToString(), "staffattendanceentry");
        }
    }

    public void bindbranch()
    {
        try
        {
            ddlbranch.Items.Clear();
            hat.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("course_id", ddldegree.SelectedValue);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds = dacc.select_method("bind_branch", hat, "sp");
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Session["collegecode"].ToString(), "staffattendanceentry");
        }
    }

    public void bindsem()
    {
        try
        {
            ddlduration.Items.Clear();
            string duration  = string.Empty;
            Boolean first_year = false;
            hat.Clear();
            collegecode = Session["collegecode"].ToString();
            hat.Add("degree_code", ddlbranch.SelectedValue.ToString());
            hat.Add("batch_year", ddlbatch.SelectedValue.ToString());
            hat.Add("college_code", collegecode);
            ds = dacc.select_method("bind_sem", hat, "sp");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlduration.Enabled = true;
                duration = ds.Tables[0].Rows[0][0].ToString();
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
                {
                    if (first_year == false)
                    {
                        ddlduration.Items.Add(loop_val.ToString());
                    }
                    else if (first_year == true && loop_val != 2)
                    {
                        ddlduration.Items.Add(loop_val.ToString());
                    }
                }
            }
            else
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ddlduration.Enabled = true;
                    duration = ds.Tables[1].Rows[0][0].ToString();
                    first_year = Convert.ToBoolean(ds.Tables[1].Rows[0][1].ToString());
                    for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
                    {
                        if (first_year == false)
                        {
                            ddlduration.Items.Add(loop_val.ToString());
                        }
                        else if (first_year == true && loop_val != 2)
                        {
                            ddlduration.Items.Add(loop_val.ToString());
                        }
                    }
                }
                else
                {
                    ddlduration.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Session["collegecode"].ToString(), "staffattendanceentry");
        }
    }

    public void bindsec()
    {
        try
        {
            ddlsec.Items.Clear();
            strquery = "select distinct sections from registration where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and sections<>'-1' and sections is not null and sections<>' ' and delflag=0 and exam_flag<>'Debar'";
            ds = dacc.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsec.Enabled = true;
                ddlsec.DataSource = ds;
                ddlsec.DataTextField = "sections";
                ddlsec.DataBind();
            }
            else
            {
                ddlsec.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Session["collegecode"].ToString(), "staffattendanceentry");
        }
    }

    public void load_subject()
    {
        try
        {
            loadhours();
            hat.Clear();
            hat.Add("Batch_Year", ddlbatch.SelectedValue.ToString());
            hat.Add("DegCode", ddlbranch.SelectedValue.ToString());
            hat.Add("Sems", ddlduration.SelectedItem.ToString());
            hat.Add("staffcode", Session["Staff_Code"].ToString());
            if (ddlsec.SelectedValue.ToString() == "" || ddlsec.SelectedValue.ToString() == "-1" || ddlsec.Enabled == false)
            {
                hat.Add("sec", "");
            }
            else
            {
                hat.Add("sec", ddlsec.SelectedValue.ToString());
            }
            ds = dacc.select_method("single_subjectwise_attnd", hat, "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlsubject.Enabled = true;
                ddlsubject.DataSource = ds;
                ddlsubject.DataTextField = "subject_name";
                ddlsubject.DataValueField = "subject_no";
                ddlsubject.DataBind();
                ddlsubject.Items.Insert(0, "--Select--");
            }
            else
            {
                ddlsubject.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Session["collegecode"].ToString(), "staffattendanceentry");
        }
    }

    public void loadhours()
    {
        try
        {
            panhrdetails.Visible = false;
            clear();
            txthour.Text = "---Select---";
            chklshour.Items.Clear();
            chkhours.Checked = false;
            if (ddlsubject.Enabled == true)
            {
                if (ddlsubject.Text != "--Select--")
                {
                    if (ddlduration.SelectedValue != "")
                    {
                        string byear = ddlbatch.SelectedValue.ToString();
                        string degree_code = ddlbranch.SelectedValue.ToString();
                        string semester = ddlduration.SelectedValue.ToString();
                        string subjectno = ddlsubject.SelectedValue.ToString();
                        string strsec  = string.Empty;
                        string sec  = string.Empty;
                        if (ddlsec.SelectedValue.ToString() == "" || ddlsec.SelectedValue.ToString() == "-1" || ddlsec.Enabled == false)
                        {
                            strsec = " ";
                        }
                        else
                        {
                            strsec = "and sections='" + ddlsec.SelectedItem.ToString() + "'";
                            sec = ddlsec.SelectedItem.ToString();
                        }
                        string[] sp = txtFromDate.Text.ToString().Split('/');
                        DateTime dt = Convert.ToDateTime(sp[1] + '/' + sp[0] + '/' + sp[2]);
                        string day = dt.ToString("ddd");
                        strquery = "select * from Alternate_Schedule where degree_code='" + degree_code + "' and batch_year='" + byear + "' and semester='" + semester + "' " + strsec + " and FromDate='" + dt.ToString("MM/dd/yyyy") + "'";
                        DataSet ds1 = dacc.select_method_wo_parameter(strquery, "Text");
                        hat.Clear();
                        strquery = "select * from PeriodAttndSchedule where degree_code='" + ddlbranch.SelectedValue.ToString() + "' and semester='" + ddlduration.SelectedValue.ToString() + "'";
                        ds.Dispose();
                        ds.Reset();
                        ds = dacc.select_method_wo_parameter(strquery, "Text");
                        string conhrs  = string.Empty;
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string nhrs = ds.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString();
                            string fhrs = ds.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString();
                            string shfrs = ds.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString();
                            if (nhrs.ToString() != null && nhrs.ToString().Trim() != "0" && nhrs.ToString().Trim() != "")
                            {
                                int thrs = Convert.ToInt32(nhrs);
                                int ihrs = Convert.ToInt32(fhrs);
                                int sehrs = Convert.ToInt32(shfrs);
                                int starthour = 1;
                                int endhrs = thrs;
                                string holiquery = "select * from holidayStudents where degree_code='" + degree_code + "' and semester='" + semester + "' and holiday_date='" + dt.ToString("MM/dd/yyyy") + "'";
                                DataSet dsholiday = dacc.select_method_wo_parameter(holiquery, "Text");
                                if (dsholiday.Tables[0].Rows.Count > 0)
                                {
                                    Boolean ful = Convert.ToBoolean(dsholiday.Tables[0].Rows[0]["halforfull"]);
                                    Boolean fhlf = Convert.ToBoolean(dsholiday.Tables[0].Rows[0]["morning"]);
                                    Boolean shlf = Convert.ToBoolean(dsholiday.Tables[0].Rows[0]["evening"]);
                                    if (ful == false)
                                    {
                                        starthour = endhrs + endhrs;
                                        errmsg.Visible = true;
                                        errmsg.Text = "Selected Day is Holiday Reason - " + dsholiday.Tables[0].Rows[0]["holiday_desc"].ToString() + "";
                                        return;
                                    }
                                    else if (ful == true && fhlf == true)
                                    {
                                        starthour = ihrs + 1;
                                    }
                                    else if (ful == true && shlf == true)
                                    {
                                        endhrs = ihrs;
                                    }
                                }
                                if (ds1.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = starthour; i <= endhrs; i++)
                                    {
                                        string hrd = day + i;
                                        string gethr = ds1.Tables[0].Rows[0][hrd].ToString();
                                        if (gethr.Trim() != "" && gethr != null)
                                        {
                                            string[] spd = gethr.Split(';');
                                            for (int s = 0; s <= spd.GetUpperBound(0); s++)
                                            {
                                                string[] sub = spd[s].Split('-');
                                                if (sub.GetUpperBound(0) > 1)
                                                {
                                                    string getval = sub[0].ToString();
                                                    if (getval == subjectno)
                                                    {
                                                        if (conhrs == "")
                                                        {
                                                            conhrs = i.ToString();
                                                        }
                                                        else
                                                        {
                                                            conhrs = conhrs + ',' + i.ToString();
                                                        }
                                                        hat.Add(i, i);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            hat.Add(i, i);
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = starthour; i <= endhrs; i++)
                                    {
                                        hat.Add(i, i);
                                    }
                                }
                                if (conhrs.Trim() != "")
                                {
                                    txthour.Text = conhrs;
                                }
                                for (int i = starthour; i <= endhrs; i++)
                                {
                                    if (hat.Contains(i))
                                    {
                                        chklshour.Items.Add("" + i + "");
                                        if (conhrs.Trim() != "")
                                        {
                                            string[] sh = conhrs.Split(',');
                                            for (int s = 0; s <= sh.GetUpperBound(0); s++)
                                            {
                                                string va = sh[s].ToString();
                                                if (va == i.ToString())
                                                {
                                                    chklshour.Items[chklshour.Items.Count - 1].Selected = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Session["collegecode"].ToString(), "staffattendanceentry");
        }
    }

    public void loadstaff()
    {
        try
        {
            chkstaff.Checked = false;
            if (Session["Staff_Code"] == null || Session["Staff_Code"].ToString() == "")
            {
                txtstaff.Text = "---Select---";
                chklsstaff.Items.Clear();
                if (ddlsubject.Enabled == true)
                {
                    string section  = string.Empty;
                    if (ddlsubject.Text != "--Select--")
                    {
                        string sems  = string.Empty;
                        if (ddlduration.SelectedValue != "")
                        {
                            string sections  = string.Empty;
                            if (ddlsec.SelectedValue.ToString() == "" || ddlsec.SelectedValue.ToString() == "-1" || ddlsec.Enabled == false)
                            {
                                sections  = string.Empty;
                            }
                            else
                            {
                                sections = " and s.sections='" + ddlsec.SelectedItem.ToString() + "'";
                            }
                            sems = "and SM.semester=" + ddlduration.SelectedValue.ToString() + "";
                            string getsubject = " and  s.subject_no ='" + ddlsubject.SelectedValue.ToString() + "'";
                            string staffquery = "select distinct m.staff_name,m.staff_code from staffmaster m,stafftrans t,staff_selector s where m.staff_code=t.staff_code and m.staff_code=s.staff_code and t.latestrec = 1 and m.resign = 0 and m.settled = 0 and s.batch_year=" + ddlbatch.SelectedValue.ToString() + " " + section + " " + getsubject + " " + sections + "";
                            ds.Dispose();
                            ds.Reset();
                            ds = dacc.select_method(staffquery, hat, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                chklsstaff.DataSource = ds.Tables[0];
                                chklsstaff.DataTextField = "Staff_name";
                                chklsstaff.DataValueField = "Staff_code";
                                chklsstaff.DataBind();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Session["collegecode"].ToString(), "staffattendanceentry");
        }
    }

    public void clear()
    {
        Showgrid.Visible = false;
        btnsave.Visible = false;
        btndeselect.Visible = false;
        btnselect.Visible = false;
        errmsg.Visible = false;
        panhrdetails.Visible = false;
        lblholireason.Visible = false;
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        load_subject();
        if (Session["Staff_Code"] == null || Session["Staff_Code"].ToString() == "")
        {
            loadstaff();
        }
        loadhours();
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        bindbranch();
        bindsem();
        bindsec();
        load_subject();
        if (Session["Staff_Code"] == null || Session["Staff_Code"].ToString() == "")
        {
            loadstaff();
        }
        loadhours();
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        bindsem();
        bindsec();
        load_subject();
        if (Session["Staff_Code"] == null || Session["Staff_Code"].ToString() == "")
        {
            loadstaff();
        }
        loadhours();
    }

    protected void ddlduration_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        bindsec();
        load_subject();
        if (Session["Staff_Code"] == null || Session["Staff_Code"].ToString() == "")
        {
            loadstaff();
        }
        loadhours();
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        load_subject();
        if (Session["Staff_Code"] == null || Session["Staff_Code"].ToString() == "")
        {
            loadstaff();
        }
        loadhours();
    }

    protected void ddlsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        if (Session["Staff_Code"] == null || Session["Staff_Code"].ToString() == "")
        {
            loadstaff();
        }
        loadhours();
    }

    protected void chklsstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Session["Staff_Code"] == null || Session["Staff_Code"].ToString() == "")
            {
                chkstaff.Checked = false;
                clear();
                int cout = 0;
                txtstaff.Text = "---Select---";
                for (int i = 0; i < chklsstaff.Items.Count; i++)
                {
                    if (chklsstaff.Items[i].Selected == true)
                    {
                        cout++;
                    }
                }
                if (cout > 0)
                {
                    txtstaff.Text = "Staff (" + cout + ")";
                    if (cout == chklsstaff.Items.Count)
                    {
                        chkstaff.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Session["collegecode"].ToString(), "staffattendanceentry");
        }
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            strquery = "select CONVERT(nvarchar(50),end_date,101) as enddate,CONVERT(nvarchar(50),start_date,101) as startdate from seminfo where degree_code='" + ddlbranch.SelectedValue.ToString() + "' and batch_year='" + ddlbatch.SelectedValue.ToString() + "' and semester='" + ddlduration.SelectedValue.ToString() + "'";
            ds.Dispose();
            ds.Reset();
            ds = dacc.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string start = ds.Tables[0].Rows[0]["startdate"].ToString();
                string endd = ds.Tables[0].Rows[0]["enddate"].ToString();
                if (start.Trim() != "" && start != null && endd.Trim() != "" && endd != null)
                {
                    DateTime dtst = Convert.ToDateTime(ds.Tables[0].Rows[0]["startdate"].ToString());
                    DateTime dtet = Convert.ToDateTime(ds.Tables[0].Rows[0]["enddate"].ToString());
                    string[] sp = txtFromDate.Text.ToString().Split('/');
                    DateTime dt = Convert.ToDateTime(sp[1] + '/' + sp[0] + '/' + sp[2]);
                    DateTime dtnow = DateTime.Now;
                    if (dt > dtnow)
                    {
                        errmsg.Visible = true;
                        errmsg.Text = "You Can't Mark Attendance for the Date Greater than Today";
                        txtFromDate.Text = dtnow.ToString("dd/MM/yyyy");
                        return;
                    }
                    if (dtst <= dt && dtet >= dt)
                    {
                        load_subject();
                        if (Session["Staff_Code"] == null || Session["Staff_Code"].ToString() == "")
                        {
                            loadstaff();
                        }
                        loadhours();
                    }
                    else
                    {
                        txtFromDate.Text = dtnow.ToString("dd/MM/yyyy");
                        errmsg.Visible = true;
                        errmsg.Text = "Please Enter Date between Semester Start Date and End Date";
                    }
                }
                else
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Update Semester Parameters";
                }
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Update Semester Parameters";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Session["collegecode"].ToString(), "staffattendanceentry");
        }
    }

    protected void chkhours_ChekedChange(object sender, EventArgs e)
    {
        txthour.Text = "---Select---";
        if (chkhours.Checked == true)
        {
            if (chklshour.Items.Count > 0)
            {
                for (int i = 0; i < chklshour.Items.Count; i++)
                {
                    chklshour.Items[i].Selected = true;
                    if (strquery == "")
                    {
                        strquery = chklshour.Items[i].Text;
                    }
                    else
                    {
                        strquery = strquery + ',' + chklshour.Items[i].Text;
                    }
                }
                txthour.Text = strquery;
            }
        }
        else
        {
            for (int i = 0; i < chklshour.Items.Count; i++)
            {
                chklshour.Items[i].Selected = false;
            }
        }
    }

    protected void chklshour_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            strquery  = string.Empty;
            int cou = 0;
            chkhours.Checked = false;
            for (int i = 0; i < chklshour.Items.Count; i++)
            {
                if (chklshour.Items[i].Selected == true)
                {
                    cou++;
                    if (strquery == "")
                    {
                        strquery = chklshour.Items[i].Text;
                    }
                    else
                    {
                        strquery = strquery + ',' + chklshour.Items[i].Text;
                    }
                }
            }
            if (strquery == "")
            {
                strquery = "---Select---";
            }
            if (cou == chklshour.Items.Count && chklshour.Items.Count > 0)
            {
                chkhours.Checked = true;
            }
            txthour.Text = strquery;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Session["collegecode"].ToString(), "staffattendanceentry");
        }
    }

    protected void chkstaff_ChekedChange(object sender, EventArgs e)
    {
        txtstaff.Text = "---Select---";
        if (chkstaff.Checked == true)
        {
            for (int i = 0; i < chklsstaff.Items.Count; i++)
            {
                chklsstaff.Items[i].Selected = true;
            }
            txtstaff.Text = "Staff (" + chklsstaff.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklsstaff.Items.Count; i++)
            {
                chklsstaff.Items[i].Selected = false;
            }
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        clear();
        loadcollegename();
        load_attnd_spread();
        loadattendancecount();
    }

    public void load_attnd_spread()
    {
        try
        {
            lblholireason.Visible = false;
            Showgrid.Visible = false;
            string byear = ddlbatch.SelectedValue.ToString();
            string degree_code = ddlbranch.SelectedValue.ToString();
            string semester = ddlduration.SelectedValue.ToString();
            string subject_no = ddlsubject.SelectedValue.ToString();
            string strsec  = string.Empty;
            if (ddlsec.SelectedValue.ToString() == "" || ddlsec.SelectedValue.ToString() == "-1" || ddlsec.Enabled == false)
            {
                strsec = " ";
            }
            else
            {
                strsec = "and r.sections='" + ddlsec.SelectedItem.ToString() + "'";
            }
            string[] sp = txtFromDate.Text.ToString().Split('/');
            DateTime dt = Convert.ToDateTime(sp[1] + '/' + sp[0] + '/' + sp[2]);
            int monthyear = (Convert.ToInt32(sp[2]) * 12) + Convert.ToInt32(sp[1]);
            strquery = "select CONVERT(nvarchar(50),end_date,101) as enddate,CONVERT(nvarchar(50),start_date,101) as startdate from seminfo where degree_code='" + ddlbranch.SelectedValue.ToString() + "' and batch_year='" + ddlbatch.SelectedValue.ToString() + "' and semester='" + ddlduration.SelectedValue.ToString() + "'";
            ds.Dispose();
            ds.Reset();
            ds = dacc.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string start = ds.Tables[0].Rows[0]["startdate"].ToString();
                string endd = ds.Tables[0].Rows[0]["enddate"].ToString();
                if (start.Trim() != "" && start != null && endd.Trim() != "" && endd != null)
                {
                    DateTime dtst = Convert.ToDateTime(ds.Tables[0].Rows[0]["startdate"].ToString());
                    DateTime dtet = Convert.ToDateTime(ds.Tables[0].Rows[0]["enddate"].ToString());
                    DateTime dtnow = DateTime.Now;
                    if (dt > dtnow)
                    {
                        errmsg.Visible = true;
                        errmsg.Text = "You Can't Mark Attendance for the Date Greater than Today";
                        return;
                    }
                    if (dtst <= dt && dtet >= dt)
                    {
                    }
                    else
                    {
                        errmsg.Visible = true;
                        errmsg.Text = "Please Enter the Attendance Date Between Semester Start Date and End Date";
                        return;
                    }
                }
                else
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Update the Semester Parameters";
                    return;
                }
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Update the Semester Parameters";
                return;
            }
            string holiquery = "select * from holidayStudents where degree_code='" + degree_code + "' and semester='" + semester + "' and holiday_date='" + dt.ToString("MM/dd/yyyy") + "'";
            DataSet dsholiday = dacc.select_method_wo_parameter(holiquery, "Text");
            if (dsholiday.Tables[0].Rows.Count > 0)
            {
                Boolean ful = Convert.ToBoolean(dsholiday.Tables[0].Rows[0]["halforfull"]);
                Boolean fhlf = Convert.ToBoolean(dsholiday.Tables[0].Rows[0]["morning"]);
                Boolean shlf = Convert.ToBoolean(dsholiday.Tables[0].Rows[0]["evening"]);
                if (ful == false)
                {
                    lblholireason.Visible = true;
                    lblholireason.Text = "Selected Day is Holiday Reason - " + dsholiday.Tables[0].Rows[0]["holiday_desc"].ToString() + "";
                    return;
                }
                else if (ful == true && fhlf == true)
                {
                    lblholireason.Visible = true;
                    lblholireason.Text = "" + dt.ToString("dd/MM/yyyy") + "- is Morning Holiday 	Reason - " + dsholiday.Tables[0].Rows[0]["holiday_desc"].ToString() + "";
                }
                else if (ful == true && shlf == true)
                {
                    lblholireason.Visible = true;
                    lblholireason.Text = "" + dt.ToString("dd/MM/yyyy") + "- is Evening Holiday 	Reason - " + dsholiday.Tables[0].Rows[0]["holiday_desc"].ToString() + "";
                }
            }
            string sel_date = dt.ToString("MM/dd/yyyy");
            if (ddlsubject.Enabled == false || ddlsubject.Items.Count == 0 || ddlsubject.SelectedValue.ToString() == "--Select--")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select the Subject and then Proceed";
                lblholireason.Visible = false;
                return;
            }
            if (txthour.Text.ToString() == "---Select---" || chklshour.Items.Count == 0)
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select the Hours";
                lblholireason.Visible = false;
                return;
            }
            string staffcode  = string.Empty;
            if (Session["Staff_Code"] != null && Session["Staff_Code"].ToString() != "")
            {
                staffcode = Session["Staff_Code"].ToString();
            }
            else
            {
                staffcode  = string.Empty;
                for (int st = 0; st < chklsstaff.Items.Count; st++)
                {
                    if (chklsstaff.Items[st].Selected == true)
                    {
                        if (staffcode == "")
                        {
                            staffcode = chklsstaff.Items[st].Value;
                        }
                        else
                        {
                            staffcode = staffcode + '-' + chklsstaff.Items[st].Value;
                        }
                    }
                }
            }
            if (staffcode.Trim() == "" || staffcode == null)
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select Staff and then Proceed";
                lblholireason.Visible = false;
                return;
            }
            
            string strorder = "ORDER BY registration.Roll_No";
            string serialno = dacc.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
            if (serialno.Trim() == "1")
            {
                strorder = "ORDER BY r.serialno";
            }
            else
            {
                string orderby_Setting = dacc.GetFunction("select value from master_Settings where settings='order_by'");
                if (orderby_Setting == "0")
                {
                    strorder = "ORDER BY r.Roll_No";
                }
                else if (orderby_Setting == "1")
                {
                    strorder = "ORDER BY r.Reg_No";
                }
                else if (orderby_Setting == "2")
                {
                    strorder = "ORDER BY Registration.Stud_Name";
                }
                else if (orderby_Setting == "0,1,2")
                {
                    strorder = "ORDER BY r.Roll_No,r.Reg_No,r.Stud_Name";
                }
                else if (orderby_Setting == "0,1")
                {
                    strorder = "ORDER BY r.Roll_No,r.Reg_No";
                }
                else if (orderby_Setting == "1,2")
                {
                    strorder = "ORDER BY r.Reg_No,r.Stud_Name";
                }
                else if (orderby_Setting == "0,2")
                {
                    strorder = "ORDER BY r.Roll_No,r.Stud_Name";
                }
            }
            string strstudentquery = "Select distinct r.roll_no,r.reg_no,r.roll_admit ,r.stud_name,r.stud_type,r.serialno,r.app_no from registration r,SubjectChooser sc,applyn a where r.roll_no = sc.roll_no and r.Degree_Code =" + degree_code + " and Semester = '" + semester + "' and Subject_No = '" + subject_no.ToString() + "' and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' and Semester = '" + semester + "' " + strsec + " and r.app_no=a.app_no" + "    and adm_date<='" + sel_date + "'  and sc.Semester=r.current_semester " + strorder + "";
            DataSet dsstudentquery = dacc.select_method_wo_parameter(strstudentquery, "Text");
            if (dsstudentquery.Tables[0].Rows.Count > 0)
            {
                btnsave.Visible = true;
                btndeselect.Visible = true;
                btnselect.Visible = true;
                Showgrid.Visible = true;
                

                dtrow = dtl.NewRow();
                dtl.Rows.Add(dtrow);

                dtrow = dtl.NewRow();
                dtl.Rows.Add(dtrow);

                dtrow = dtl.NewRow();
                dtl.Rows.Add(dtrow);

                int colu = 0;


                dtl.Columns.Add("S.No", typeof(string));
                dtl.Rows[0][colu] = "S.No";
                colu++;
                colofvisfalse.Add("true");
                

                dtl.Columns.Add("Select", typeof(string));
                dtl.Rows[0][colu] = "Select";
                colu++;
                colofvisfalse.Add("true");
                

                dtl.Columns.Add("Roll No", typeof(string));
                dtl.Rows[0][colu] = "Roll No";
                colu++;
                colofvisfalse.Add("true");
                

                dtl.Columns.Add("Reg No", typeof(string));
                dtl.Rows[0][colu] = "Reg No";
                colu++;
                colofvisfalse.Add("true");
                

                dtl.Columns.Add("Student Name", typeof(string));
                dtl.Rows[0][colu] = "Student Name";
                colu++;
                colofvisfalse.Add("true");
                

                dtl.Columns.Add("Student Type", typeof(string));
                dtl.Rows[0][colu] = "Student Type";
                colu++;
                colofvisfalse.Add("true");
                
                if (Session["Rollflag"].ToString() != "0")
                {
                    
                    colofvisfalse[2] = "true";
                }
                else
                {
                    
                    colofvisfalse[2] = "false";
                }
                if (Session["Regflag"].ToString() != "0")
                {
                    
                    colofvisfalse[3] = "true";
                }
                else
                {
                    
                    colofvisfalse[3] = "false";
                }
                if (Session["Studflag"].ToString() != "0")
                {
                    
                    colofvisfalse[5] = "true";
                }
                else
                {
                    
                    colofvisfalse[5] = "false";
                }
                
                string[] strcomo = new string[20];
                string[] strcomo1 = new string[20];
                string[] attnd_rights1 = new string[100];
                int i = 0, j = 0;
                string odrights = dacc.GetFunction("select rights from  OD_Master_Setting where " + grouporusercode + "");
                if (odrights.Trim() != null && odrights.Trim() != "" && odrights.Trim() != "0")
                {
                    string od_rights  = string.Empty;
                    od_rights = odrights;
                    string[] split_od_rights = od_rights.Split(',');
                    strcomo = new string[split_od_rights.GetUpperBound(0) + 2];
                    strcomo1 = new string[split_od_rights.GetUpperBound(0) + 3];
                    strcomo1[j++] = "Select for All";
                    strcomo1[j++]  = string.Empty;
                    strcomo[i++] = " ";
                    for (int od_temp = 0; od_temp <= split_od_rights.GetUpperBound(0); od_temp++)
                    {
                        if (od_temp == split_od_rights.GetUpperBound(0))
                        {
                            strcomo[i++] = " ";
                        }
                        strcomo[i++] = split_od_rights[od_temp].ToString();
                        strcomo1[j++] = split_od_rights[od_temp].ToString();
                    }
                }
                else
                {
                    strcomo[0] = " ";
                    strcomo[1] = "P";
                    strcomo[2] = "A";
                    strcomo[3] = "OD";
                    strcomo[4] = "SOD";
                    strcomo[5] = "ML";
                    strcomo[6] = "NSS";
                    strcomo[7] = "L";
                    strcomo[8] = "NCC";
                    strcomo[9] = "HS";
                    strcomo[10] = "PP";
                    strcomo[11] = "SYOD";
                    strcomo[12] = "COD";
                    strcomo[13] = "OOD";
                    strcomo[14] = " ";
                    strcomo[15] = "LA";
                    strcomo1[0] = "Select for All";
                    strcomo1[1]  = string.Empty;
                    strcomo1[2] = "P";
                    strcomo1[3] = "A";
                    strcomo1[4] = "OD";
                    strcomo1[5] = "SOD";
                    strcomo1[6] = "ML";
                    strcomo1[7] = "NSS";
                    strcomo1[8] = "L";
                    strcomo1[9] = "NCC";
                    strcomo1[10] = "HS";
                    strcomo1[11] = "PP";
                    strcomo1[12] = "SYOD";
                    strcomo1[13] = "COD";
                    strcomo1[14] = "OOD";
                    strcomo1[15] = "LA";
                }
                
                
                
                
                
                string[] sh = txthour.Text.ToString().Split(',');
                int srno = 0;
                string hrv  = string.Empty;
                string date = dt.Day.ToString();
                for (i = 0; i <= sh.GetUpperBound(0); i++)
                {
                    
                    srno++;
                    

                    dtl.Columns.Add("", typeof(string));
                    dtl.Rows[0][colu] = txtFromDate.Text.ToString();
                    
                    

                    
                    dtl.Rows[1][colu] = sh[i].ToString();
                    colu++;
                    colofvisfalse.Add("true");
                    

                    
                    if (hrv == "")
                    {
                        hrv = "d" + date + "d" + sh[i].ToString();
                    }
                    else
                    {
                        hrv = hrv + ',' + "d" + date + "d" + sh[i].ToString();
                    }
                    
                    tagof0 += "d" + date + "d" + sh[i].ToString() + "~";
                }
                
                strquery = "select r.roll_no,r.app_no," + hrv + " from attendance a,Registration r where r.Roll_No=a.roll_no and month_year=" + monthyear + " and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and r.current_semester=" + ddlduration.SelectedValue.ToString() + " and r.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 " + strsec + " and adm_date<='" + sel_date + "'";
                DataSet dsat = dacc.select_method_wo_parameter(strquery, "Text");
                srno = 0;
                rowsofcolour.Add(" ");
                rowsofcolour.Add(" ");
                rowsofcolour.Add(" ");
                for (i = 0; i < dsstudentquery.Tables[0].Rows.Count; i++)
                {
                    srno++;
                    
                    dtrow = dtl.NewRow();
                    dtl.Rows.Add(dtrow);
                    
                    dtl.Rows[dtl.Rows.Count - 1][0] = srno.ToString();
                    
                    dtl.Rows[dtl.Rows.Count - 1][2] = dsstudentquery.Tables[0].Rows[i]["roll_no"].ToString();
                    
                    noteof1 += dsstudentquery.Tables[0].Rows[i]["app_no"].ToString() +"~";
                    
                    dtl.Rows[dtl.Rows.Count - 1][3] = dsstudentquery.Tables[0].Rows[i]["reg_no"].ToString();
                    
                    dtl.Rows[dtl.Rows.Count - 1][4] = dsstudentquery.Tables[0].Rows[i]["stud_name"].ToString();
                   
                    dtl.Rows[dtl.Rows.Count - 1][5] = dsstudentquery.Tables[0].Rows[i]["stud_type"].ToString();
                    if (dsstudentquery.Tables[0].Rows[i]["stud_type"].ToString() == "Hostler")
                    {
                        
                        rowsofcolour.Add("LightYellow");
                    }
                    else
                    {
                        
                        rowsofcolour.Add("MediumSeaGreen");
                    }
                    DataTable dtat = dsat.Tables[0];
                    dtat.DefaultView.RowFilter = " roll_no='" + dsstudentquery.Tables[0].Rows[i]["roll_no"].ToString() + "' ";
                    DataView dvat = dtat.DefaultView;
                    for (j = 6;  j < dtl.Columns.Count; j++)
                    {
                        
                        
                        string[] hourtag = tagof0.Split('~');
                        string hour = hourtag[j - 6];
                        
                        if (dvat.Count > 0)
                        {
                            string valu = dvat[0][hour].ToString();
                            valu = Attmark(valu);
                            if (valu.Trim().ToLower() != "h")
                            {
                                if (valu.Trim() != "" && valu != null && valu != "-1")
                                {
                                    nullflag = true;
                                }
                                

                                dtl.Rows[dtl.Rows.Count - 1][j] = valu;
                            }
                        }
                    }
                }
                
                dtrow = dtl.NewRow();
                dtl.Rows.Add(dtrow);
                dtrow = dtl.NewRow();
                dtl.Rows.Add(dtrow);
               
                dtl.Rows[dtl.Rows.Count - 2][0] = "No Of Student(s) Present:";
                dtl.Rows[dtl.Rows.Count - 1][0] = "No Of Student(s) Absent:";

                

                dtl.Columns.Add(noteof1, typeof(string));

                
                

                colofvisfalse.Add("false");


                Session["dtGrid"] = dtl;
                callGridBind();
                Showgrid.DataSource = dtl;
                Showgrid.DataBind();
                Showgrid.HeaderRow.Visible = false;

                if (nullflag == true)
                {
                    btnsave.Text = "Update";
                }
                else
                {
                    btnsave.Text = "Save";
                }
            }
            else
            {
                Showgrid.Visible = false;
                errmsg.Text = "No Records Found";
                errmsg.Visible = true;
            }
            
           
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Session["collegecode"].ToString(), "staffattendanceentry");
        }
    }

    public string Attvalues(string Att_str1)
    {
        string Attvalue;
        Attvalue  = string.Empty;
        if (Att_str1 == "P")
        {
            Attvalue = "1";
        }
        else if (Att_str1 == "A")
        {
            Attvalue = "2";
        }
        else if (Att_str1 == "OD")
        {
            Attvalue = "3";
        }
        else if (Att_str1 == "ML")
        {
            Attvalue = "4";
        }
        else if (Att_str1 == "SOD")
        {
            Attvalue = "5";
        }
        else if (Att_str1 == "NSS")
        {
            Attvalue = "6";
        }
        else if (Att_str1 == "H")
        {
            Attvalue = "7";
        }
        else if (Att_str1 == "NJ")
        {
            Attvalue = "8";
        }
        else if (Att_str1 == "S")
        {
            Attvalue = "9";
        }
        else if (Att_str1 == "L")
        {
            Attvalue = "10";
        }
        else if (Att_str1 == "NCC")
        {
            Attvalue = "11";
        }
        else if (Att_str1 == "HS")
        {
            Attvalue = "12";
        }
        else if (Att_str1 == "PP")
        {
            Attvalue = "13";
        }
        else if (Att_str1 == "SYOD")
        {
            Attvalue = "14";
        }
        else if (Att_str1 == "COD")
        {
            Attvalue = "15";
        }
        else if (Att_str1 == "OOD")
        {
            Attvalue = "16";
        }
        else if (Att_str1 == "LA")
        {
            Attvalue = "17";
        }
        else
        {
            Attvalue  = string.Empty;
        }
        return Attvalue;
    }

    public string Attmark(string Attstr_mark)
    {
        string Att_mark  = string.Empty;
        if (Attstr_mark == "1")
        {
            Att_mark = "P";
        }
        else if (Attstr_mark == "2")
        {
            Att_mark = "A";
        }
        else if (Attstr_mark == "3")
        {
            Att_mark = "OD";
        }
        else if (Attstr_mark == "4")
        {
            Att_mark = "ML";
        }
        else if (Attstr_mark == "5")
        {
            Att_mark = "SOD";
        }
        else if (Attstr_mark == "6")
        {
            Att_mark = "NSS";
        }
        else if (Attstr_mark == "7")
        {
            Att_mark = "H";
        }
        else if (Attstr_mark == "8")
        {
            Att_mark = "NJ";
        }
        else if (Attstr_mark == "9")
        {
            Att_mark = "S";
        }
        else if (Attstr_mark == "10")
        {
            Att_mark = "L";
        }
        else if (Attstr_mark == "11")
        {
            Att_mark = "NCC";
        }
        else if (Attstr_mark == "12")
        {
            Att_mark = "HS";
        }
        else if (Attstr_mark == "13")
        {
            Att_mark = "PP";
        }
        else if (Attstr_mark == "14")
        {
            Att_mark = "SYOD";
        }
        else if (Attstr_mark == "15")
        {
            Att_mark = "COD";
        }
        else if (Attstr_mark == "16")
        {
            Att_mark = "OOD";
        }
        else if (Attstr_mark == "17")
        {
            Att_mark = "LA";
        }
        else
        {
            Att_mark  = string.Empty;
        }
        return Att_mark;
    }

    

    protected void btnselect_Click(object sender, EventArgs e)
    {
        try
        {
 

            if (Showgrid.Rows.Count > 1)
            {
                for (int row = 3; row < Showgrid.Rows.Count - 2; row++)
                {
                    for (int col = 6; col < Showgrid.HeaderRow.Cells.Count-1; col++)
                    {
                        if (Showgrid.Rows[Convert.ToInt32(row)].Cells[Convert.ToInt32(col)].Text != "")
                        {
                            DropDownList vv = Showgrid.Rows[Convert.ToInt32(row)].Cells[Convert.ToInt32(col)].FindControl("ddlselectallhourwise_" + Convert.ToInt32(row) + "_" + col) as DropDownList;
                            string gg = vv.SelectedValue;
                            if ((gg != "OD") && (gg != "S"))
                            {


                               


                                vv.SelectedValue = "P";

                            }
                        }
                    }
                }
            }

            loadattendancecount();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Session["collegecode"].ToString(), "staffattendanceentry");
        }
    }

    protected void btndeselect_Click(object sender, EventArgs e)
    {
        try
        {
            

            if (Showgrid.Rows.Count > 1)
            {
                for (int row = 3; row < Showgrid.Rows.Count - 2; row++)
                {
                    for (int col = 6; col < Showgrid.HeaderRow.Cells.Count-1; col++)
                    {
                        if (Showgrid.Rows[Convert.ToInt32(row)].Cells[Convert.ToInt32(col)].Text != "")
                        {
                            DropDownList vv = Showgrid.Rows[Convert.ToInt32(row)].Cells[Convert.ToInt32(col)].FindControl("ddlselectallhourwise_" + Convert.ToInt32(row) + "_" + col) as DropDownList;
                            string gg = vv.SelectedValue;
                            if ((gg != "OD") && (gg != "S"))
                            {


                                


                                vv.SelectedValue = "";

                            }
                        }
                    }
                }
            }
            loadattendancecount();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Session["collegecode"].ToString(), "staffattendanceentry");
            
        }
    }

    public void loadattendancecount()
    {
        try
        {
            present_calcflag.Clear();
            absent_calcflag.Clear();
            hat.Clear();
            hat.Add("colege_code", Session["collegecode"].ToString());
            ds_attndmaster = dacc.select_method("ATT_MASTER_SETTING", hat, "sp");
            count_master = (ds_attndmaster.Tables[0].Rows.Count);
            if (count_master > 0)
            {
                for (count_master = 0; count_master < ds_attndmaster.Tables[0].Rows.Count; count_master++)
                {
                    if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "0")
                    {
                        present_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                    }
                    if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "1")
                    {
                        absent_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                    }
                }
            }
            for (Att_mark_column = 6; Att_mark_column < Showgrid.HeaderRow.Cells.Count - 1; Att_mark_column++)
            {
                absent_count = 0;
                present_count = 0;
                for (Att_mark_row = 1; Att_mark_row < Showgrid.Rows.Count - 4; Att_mark_row++)
                {
                    if (Showgrid.Rows[Att_mark_row +2].Cells[Att_mark_column].Text.Trim() != "" && Showgrid.Rows[Att_mark_row+2].Cells[Att_mark_column].Text.Trim() != null) //condn 09.08.12 m ythili
                    {
                        
                        int rr = Att_mark_row + 2;
                        DropDownList vv = Showgrid.Rows[Att_mark_row + 2].Cells[Att_mark_column].FindControl("ddlselectallhourwise_" + rr + "_" + Att_mark_column) as DropDownList;
                        
                        string getvalue = Attvalues(vv.SelectedValue); 

                        if (present_calcflag.ContainsKey(getvalue))
                        {
                            present_count++;
                        }
                        if (absent_calcflag.ContainsKey(getvalue))
                        {
                            absent_count++;
                        }
                    }
                }
                
                
                    Showgrid.Rows[Showgrid.Rows.Count - 2].Cells[Att_mark_column].Text = present_count.ToString();
                    Showgrid.Rows[Showgrid.Rows.Count - 1].Cells[Att_mark_column].Text = absent_count.ToString();
                    
                
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Session["collegecode"].ToString(), "staffattendanceentry");
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            
            Boolean savefalg = false;
            int savevalue = 0;
            int insert = 0;
            string insertvalues  = string.Empty;
            string updatevalues  = string.Empty;
            string monthandyear  = string.Empty;
            string byear = ddlbatch.SelectedValue.ToString();
            string degree_code = ddlbranch.SelectedValue.ToString();
            string semester = ddlduration.SelectedValue.ToString();
            string subject_no = ddlsubject.SelectedValue.ToString();
            string str_Date;
            string str_rollno;
            string str_hour;
            string Atyear;
            string Atmonth;
            long strdate;
            string str_day;
            string Att_mark;
            string Att_value;
            string dcolumn;
            string Splitmondate;
            str_Date  = string.Empty;
            str_rollno  = string.Empty;
            str_hour  = string.Empty;
            str_day  = string.Empty;
            Att_mark  = string.Empty;
            Att_value  = string.Empty;
            dcolumn  = string.Empty;
            Splitmondate  = string.Empty;
            string Subtype  = string.Empty;
            string staffcode  = string.Empty;
            string appNo  = string.Empty;
            // start ***************** Added by jairam *************** 12-09-2014
            loadcollegename();
            string savehoursqlstrq;
            int totalhor;
            string noofhours_save  = string.Empty;
            string no_firsthalf  = string.Empty;
            string no_secondhalf  = string.Empty;
            string no_minpresent_firsthalf  = string.Empty;
            string no_minpresent_secondhalf  = string.Empty;
            string min_per_day  = string.Empty;
            savehoursqlstrq = "select No_of_hrs_per_day,no_of_hrs_I_half_day,no_of_hrs_II_half_day,min_pres_II_half_day ,min_pres_I_half_day,min_hrs_per_day  from PeriodAttndSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + "";
            ds.Clear();
            ds = dacc.select_method_wo_parameter(savehoursqlstrq, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                noofhours_save = ds.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString();
                no_firsthalf = ds.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString();
                no_secondhalf = ds.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString();
                no_minpresent_firsthalf = ds.Tables[0].Rows[0]["min_pres_I_half_day"].ToString();
                no_minpresent_secondhalf = ds.Tables[0].Rows[0]["min_pres_II_half_day"].ToString();
                min_per_day = ds.Tables[0].Rows[0]["min_hrs_per_day"].ToString();
            }
            totalhor = Convert.ToInt32(noofhours_save);
            //End ********** jairam ****************
            if (Session["Staff_Code"] != null && Session["Staff_Code"].ToString() != "")
            {
                staffcode = Session["Staff_Code"].ToString();
            }
            else
            {
                staffcode  = string.Empty;
                for (int st = 0; st < chklsstaff.Items.Count; st++)
                {
                    if (chklsstaff.Items[st].Selected == true)
                    {
                        if (staffcode == "")
                        {
                            staffcode = chklsstaff.Items[st].Value;
                        }
                        else
                        {
                            staffcode = staffcode + '-' + chklsstaff.Items[st].Value;
                        }
                    }
                }
            }
            if (staffcode.Trim() == "" || staffcode == null)
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select Staff and then Proceed";
                return;
            }
            string altvalue = subject_no + '-' + staffcode + "-S";
            string islab = "S";
            strquery = "select lab,Subject.Subtype_no from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subject_no + "'";
            ds = dacc.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string la = ds.Tables[0].Rows[0]["lab"].ToString();
                if (la.ToString().Trim().ToLower() == "true" || la.ToString().Trim() == "1")
                {
                    altvalue = subject_no + '-' + staffcode + "-L";
                    islab = "L";
                }
                Subtype = ds.Tables[0].Rows[0]["Subtype_no"].ToString();
            }
            string strsec  = string.Empty;
            string sec  = string.Empty;
            if (ddlsec.SelectedValue.ToString() == "" || ddlsec.SelectedValue.ToString() == "-1" || ddlsec.Enabled == false)
            {
                strsec = " ";
            }
            else
            {
                strsec = "and sections='" + ddlsec.SelectedItem.ToString() + "'";
                sec = ddlsec.SelectedItem.ToString();
            }
            Hashtable hathour = new Hashtable();
            for (int Att_column = 6; Att_column <= Showgrid.HeaderRow.Cells.Count - 2; Att_column++)
            {
                for (int Att_row = 1;  Att_row < Showgrid.Rows.Count - 4; Att_row++)
                {
                    string get = "";


                    if (Showgrid.Rows[Att_row+2].Cells[Att_column].Text != "")
                    {
                        int rr = Att_row + 2;

                        DropDownList vv = Showgrid.Rows[Att_row + 2].Cells[Convert.ToInt32(Att_column)].FindControl("ddlselectallhourwise_" + rr + "_" + Att_column) as DropDownList;
                        get = vv.SelectedValue;
                    }


                    if (get.Trim() != "" && get != null && get != "0")
                    {
                        

                        str_hour = Showgrid.Rows[1].Cells[Att_column].Text;

                        string[] split_hr = str_hour.Split(new Char[] { '-' });
                        str_hour = str_hour[0].ToString();
                        if (!hathour.Contains(str_hour))
                        {
                            hathour.Add(str_hour, str_hour);
                            
                            Att_row = Showgrid.Rows.Count-2;
                        }
                    }
                }
            }
            Boolean allatflag = false;
            for (int Att_column = 6;  Att_column <= Showgrid.HeaderRow.Cells.Count - 2; Att_column++)
            {
                Boolean altflag = false;
                for (int Att_row = 1; Att_row <= Showgrid.Rows.Count - 5; Att_row++)
                {
                    


                    Att_mark = string.Empty;
                    if (Showgrid.Rows[Att_row + 2].Cells[Att_column].Text != "")
                    {
                        int rr = Att_row + 2;

                        DropDownList vv = Showgrid.Rows[Att_row + 2].Cells[Convert.ToInt32(Att_column)].FindControl("ddlselectallhourwise_" + rr + "_" + Att_column) as DropDownList;
                        Att_mark = vv.SelectedValue;
                    }

                    if (Att_mark == "System.Object")
                    {
                        Att_mark  = string.Empty;
                    }
                    if (Att_mark.Trim() != "" && Att_mark != null && Att_mark != "0")
                    {
                        altflag = true;
                        allatflag = true;
                    }
                }
                str_Date = txtFromDate.Text.ToString();
                string[] tmpdate = str_Date.ToString().Split(new char[] { ' ' });
                str_Date = tmpdate[0].ToString();
                Splitmondate = str_Date.ToString();
                string[] split = Splitmondate.Split(new Char[] { '/' });
                str_day = split[0].ToString();
                Atmonth = split[1].ToString();
                Atyear = split[2].ToString();
                DateTime dtgetdate = Convert.ToDateTime(split[1].ToString() + '/' + split[0].ToString() + '/' + split[2].ToString());
                string altdate = dtgetdate.ToString("MM/dd/yyyy");
                strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                

                str_hour = Showgrid.Rows[1].Cells[Att_column].Text;

                string[] split_hr = str_hour.Split(new Char[] { '-' });
                str_hour = str_hour[0].ToString();
                dcolumn = "d" + dtgetdate.Day.ToString() + "d" + str_hour;
                string alday = dtgetdate.ToString("ddd") + str_hour;
                string strDay = dtgetdate.ToString("ddd");
                Att_value = Attvalues(Att_mark);
                string alternatset  = string.Empty;
                if (altflag == true)
                {
                    if (hathour.Contains(str_hour))
                    {
                        alternatset = altvalue;
                    }
                }
                string alternatequery = "if exists (select * from Alternate_Schedule where degree_code='" + degree_code + "' and semester='" + semester + "' and batch_year='" + byear + "' " + strsec + " and FromDate='" + altdate + "')";
                alternatequery = alternatequery + " update Alternate_Schedule set " + alday + "='" + alternatset + "' where degree_code='" + degree_code + "' and semester='" + semester + "' and batch_year='" + byear + "' and Sections='" + sec + "' and FromDate='" + altdate + "'";
                alternatequery = alternatequery + " else insert into Alternate_Schedule (degree_code,batch_year,semester,Sections,FromDate," + alday + ") values('" + degree_code + "','" + byear + "','" + semester + "','" + sec + "','" + altdate + "','" + alternatset + "')";
                insert = dacc.update_method_wo_parameter(alternatequery, "Text");
                strquery = "delete from subjectChooser_New where semester='" + semester + "' and fromdate='" + altdate + "' and subject_no='" + subject_no + "' and roll_no in( select roll_no from Registration where  batch_year='" + byear + "' and degree_code='" + degree_code + "' and Current_Semester='" + semester + "' " + strsec + " and cc=0 and delflag=0 and exam_flag<>'debar' )";
                insert = dacc.update_method_wo_parameter(strquery, "Text");
                strquery = "delete from laballoc_new where  batch_year='" + byear + "' and degree_code='" + degree_code + "' and semester='" + semester + "' " + strsec + " and day_value='" + strDay + "' and hour_value='" + str_hour + "' and fdate='" + altdate + "'";
                insert = dacc.update_method_wo_parameter(strquery, "Text");
                if (islab == "L")
                {
                    strquery = "insert into subjectChooser_New (semester,roll_no,subject_no,subtype_no,Batch,fromdate,todate) ";
                    strquery = strquery + "(select s.semester,s.roll_no,s.subject_no,s.subtype_no,'B1','" + altdate + "' as fromdate ,'" + altdate + "' as todate from Registration r ,subjectChooser s where r.Roll_No=s.roll_no and r.Current_Semester=s.semester and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'Debar' and s.subject_no='" + subject_no + "' and batch_year='" + byear + "' and r.degree_code='" + degree_code + "' and s.semester='" + semester + "' " + strsec + ")";
                    insert = dacc.update_method_wo_parameter(strquery, "Text");
                    strquery = "insert into laballoc_new (Batch_Year,Degree_Code,Semester,Sections,Subject_No,Stu_Batch,Day_Value,Hour_Value,fdate,tdate,Staff_Code) ";
                    strquery = strquery + "values('" + byear + "','" + degree_code + "','" + semester + "','" + sec + "','" + subject_no + "','B1','" + strDay + "','" + str_hour + "','" + altdate + "','" + altdate + "','" + Session["Staff_Code"].ToString() + "')";
                    insert = dacc.update_method_wo_parameter(strquery, "Text");
                }
            }
            //start ***************** Added by jairam *************** 12-09-2014
            string hourwise  = string.Empty;
            string daywise  = string.Empty;
            string hourwisedata  = string.Empty;
            string daywisedata  = string.Empty;
            string minimum  = string.Empty;
            string minimun_data  = string.Empty;
            string settingquery  = string.Empty;
            settingquery = "select TextName,Taxtval from Attendance_Settings where  College_Code ='" + Session["collegecode"].ToString() + "'and user_id='" + Session["usercode"].ToString() + "'";
            ds.Clear();
            ds = dacc.select_method_wo_parameter(settingquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ArrayList addarray = new ArrayList();
                DataView dv_demand_data = new DataView();
                ds.Tables[0].DefaultView.RowFilter = "TextName in ('Hour','Day','Minimun Absent Day','Minimun Days')";
                dv_demand_data = ds.Tables[0].DefaultView;
                if (dv_demand_data.Count > 0)
                {
                    for (int i = 0; i < dv_demand_data.Count; i++)
                    {
                        if (dv_demand_data[i]["TextName"].ToString() == "Hour" && Convert.ToInt32(dv_demand_data[i]["Taxtval"]) == 1)
                        {
                            hourwise = "1";
                            hourwisedata = "Hour";
                        }
                        else if (dv_demand_data[i]["TextName"].ToString() == "Hour" && Convert.ToInt32(dv_demand_data[i]["Taxtval"]) == 0)
                        {
                            hourwise = "0";
                        }
                        if (dv_demand_data[i]["TextName"].ToString() == "Day" && Convert.ToInt32(dv_demand_data[i]["Taxtval"]) == 1)
                        {
                            daywise = "1";
                            daywisedata = "Day";
                        }
                        else if (dv_demand_data[i]["TextName"].ToString() == "Day" && Convert.ToInt32(dv_demand_data[i]["Taxtval"]) == 0)
                        {
                            daywise = "0";
                        }
                        if (dv_demand_data[i]["TextName"].ToString() == "Minimun Absent Day" && Convert.ToInt32(dv_demand_data[i]["Taxtval"]) == 1)
                        {
                            minimum = "1";
                            minimun_data = "Minimun Absent Day";
                        }
                        else if (dv_demand_data[i]["TextName"].ToString() == "Minimun Absent Day" && Convert.ToInt32(dv_demand_data[i]["Taxtval"]) == 0)
                        {
                            minimum = "0";
                        }
                        if (dv_demand_data[i]["TextName"].ToString() == "Minimun Days" && Convert.ToString(dv_demand_data[i]["Taxtval"]) != "")
                        {
                            minimum_day = Convert.ToString(dv_demand_data[i]["Taxtval"]);
                        }
                        else if (dv_demand_data[i]["TextName"].ToString() == "Minimun Days" && Convert.ToString(dv_demand_data[i]["Taxtval"]) == "")
                        {
                            minimum_day  = string.Empty;
                        }
                    }
                }
            }
            present_calcflag.Clear();
            absent_calcflag.Clear();
            hat.Clear();
            hat.Add("colege_code", Session["collegecode"].ToString());
            ds_attndmaster = dacc.select_method("ATT_MASTER_SETTING", hat, "sp");
            count_master = (ds_attndmaster.Tables[0].Rows.Count);
            if (count_master > 0)
            {
                for (count_master = 0; count_master < ds_attndmaster.Tables[0].Rows.Count; count_master++)
                {
                    if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "0")
                    {
                        present_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                    }
                    if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "1")
                    {
                        absent_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                    }
                }
            }
            // ********************* End  **************************
            if (Showgrid.HeaderRow.Cells.Count-1 >1)
            {
               
                string[] noteval = Showgrid.HeaderRow.Cells[Showgrid.HeaderRow.Cells.Count - 1].Text.Split('~');
                for (int Att_row = 1; Att_row <= Showgrid.Rows.Count - 5; Att_row++)
                {
                    insertvalues  = string.Empty;
                    updatevalues  = string.Empty;
                    monthandyear  = string.Empty;
                    string values  = string.Empty;
                    for (int Att_column = 6; Att_column <= Showgrid.HeaderRow.Cells.Count - 2; Att_column++)
                    {
                        

                        str_rollno = Showgrid.Rows[Att_row + 2].Cells[2].Text.ToString();

                        
                        appNo = noteval[Att_row - 1];
                        str_Date = txtFromDate.Text.ToString();
                        string[] tmpdate = str_Date.ToString().Split(new char[] { ' ' });
                        str_Date = tmpdate[0].ToString();
                        Splitmondate = str_Date.ToString();
                        string[] split = Splitmondate.Split(new Char[] { '/' });
                        str_day = split[0].ToString();
                        Atmonth = split[1].ToString();
                        Atyear = split[2].ToString();
                        DateTime dtgetdate = Convert.ToDateTime(split[1].ToString() + '/' + split[0].ToString() + '/' + split[2].ToString());
                        string altdate = dtgetdate.ToString("MM/dd/yyyy");
                        strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                        
                        str_hour = Showgrid.Rows[1].Cells[Att_column].Text;
                        string[] split_hr = str_hour.Split(new Char[] { '-' });
                        str_hour = str_hour[0].ToString();
                        
                        Att_mark = string.Empty;
                        if (Showgrid.Rows[Att_row + 2].Cells[Att_column].Text != "")
                        {
                            int rr = Att_row + 2;

                            DropDownList vv = Showgrid.Rows[Att_row + 2].Cells[Convert.ToInt32(Att_column)].FindControl("ddlselectallhourwise_" + rr + "_" + Att_column) as DropDownList;
                            Att_mark = vv.SelectedValue;
                        }

                        dcolumn = "d" + dtgetdate.Day.ToString() + "d" + str_hour;
                        string alday = dtgetdate.ToString("ddd") + str_hour;
                        string strDay = dtgetdate.ToString("ddd");
                        Att_value = Attvalues(Att_mark);
                        //start ***************** Added by jairam *************** 12-09-2014
                        if (minimum != "1")
                        {
                            if (hourwise == "1")
                            {
                                if (absent_calcflag.Count > 0)
                                {
                                    if (absent_calcflag.Contains(Att_value) == true)
                                    {
                                        SendingSms(str_rollno, Splitmondate, str_hour, collegename, coursename, hourwisedata);
                                        sendvoicecall(str_rollno, Splitmondate, str_hour, ddlbatch.SelectedValue.ToString(), ddlbranch.SelectedItem.Value.ToString(), collegename, coursename, hourwisedata);
                                    }
                                }
                            }
                        }
                        // ************************** End **************************
                        if (Att_value == "")
                        {
                            Att_value = "0";
                        }
                        if (Att_value != "0")
                        {
                            nullflag = true;
                        }
                        if (insertvalues == "")
                        {
                            insertvalues = dcolumn;
                            values = Att_value;
                            updatevalues = dcolumn + "=" + Att_value;
                        }
                        else
                        {
                            insertvalues = insertvalues + ',' + dcolumn;
                            values = values + ',' + Att_value;
                            updatevalues = updatevalues + ',' + dcolumn + "=" + Att_value;
                        }
                        //if (Att_row == 1)
                        //{
                        //    if (hathour.Contains(str_hour))
                        //    {
                        //        string alternatequery = "if exists (select * from Alternate_Schedule where degree_code='" + degree_code + "' and semester='" + semester + "' and batch_year='" + byear + "' " + strsec + " and FromDate='" + altdate + "')";
                        //        alternatequery = alternatequery + " update Alternate_Schedule set " + alday + "='" + altvalue + "' where degree_code='" + degree_code + "' and semester='" + semester + "' and batch_year='" + byear + "' and Sections='" + sec + "' and FromDate='" + altdate + "'";
                        //        alternatequery = alternatequery + " else insert into Alternate_Schedule (degree_code,batch_year,semester,Sections,FromDate," + alday + ") values('" + degree_code + "','" + byear + "','" + semester + "','" + sec + "','" + altdate + "','" + altvalue + "')";
                        //        insert = dacc.update_method_wo_parameter(alternatequery, "Text");
                        //        strquery = "delete from subjectChooser_New where semester='" + semester + "' and fromdate='" + altdate + "' and subject_no='" + subject_no + "' and roll_no in( select roll_no from Registration where  batch_year='" + byear + "' and degree_code='" + degree_code + "' and Current_Semester='" + semester + "' " + strsec + " and cc=0 and delflag=0 and exam_flag<>'debar' )";
                        //        insert = dacc.update_method_wo_parameter(strquery, "Text");
                        //        strquery = "delete from laballoc_new where  batch_year='" + byear + "' and degree_code='" + degree_code + "' and semester='" + semester + "' " + strsec + " and day_value='" + strDay + "' and hour_value='" + str_hour + "' and fdate='" + altdate + "'";
                        //        insert = dacc.update_method_wo_parameter(strquery, "Text");
                        //        if (islab == "L")
                        //        {
                        //            strquery = "insert into subjectChooser_New (semester,roll_no,subject_no,subtype_no,Batch,fromdate,todate) ";
                        //            strquery = strquery + "(select s.semester,s.roll_no,s.subject_no,s.subtype_no,'B1','" + altdate + "' as fromdate ,'" + altdate + "' as todate from Registration r ,subjectChooser s where r.Roll_No=s.roll_no and r.Current_Semester=s.semester and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'Debar' and s.subject_no='" + subject_no + "' and batch_year='" + byear + "' and r.degree_code='" + degree_code + "' and s.semester='" + semester + "' " + strsec + ")";
                        //            insert = dacc.update_method_wo_parameter(strquery, "Text");
                        //            strquery = "insert into laballoc_new (Batch_Year,Degree_Code,Semester,Sections,Subject_No,Stu_Batch,Day_Value,Hour_Value,fdate,tdate,Staff_Code) ";
                        //            strquery = strquery + "values('" + byear + "','" + degree_code + "','" + semester + "','" + sec + "','" + subject_no + "','B1','" + strDay + "','" + str_hour + "','" + altdate + "','" + altdate + "','" + Session["Staff_Code"].ToString() + "')";
                        //            insert = dacc.update_method_wo_parameter(strquery, "Text");
                        //        }
                        //    }
                        //}
                        if (monthandyear == "")
                        {
                            monthandyear = strdate.ToString();
                        }
                        if (monthandyear != strdate.ToString() || ( Att_column == Showgrid.HeaderRow.Cells.Count - 2))
                        {
                            hat.Clear();
                            hat.Add("Att_App_no", appNo);
                            hat.Add("Att_CollegeCode", Session["collegecode"].ToString());
                            hat.Add("rollno", str_rollno);
                            hat.Add("monthyear", monthandyear);
                            hat.Add("columnname", insertvalues);
                            hat.Add("colvalues", values);
                            hat.Add("coulmnvalue", updatevalues);
                            insert = dacc.insert_method("sp_ins_upd_student_attendance_Dead", hat, "sp");
                            insertvalues  = string.Empty;
                            updatevalues  = string.Empty;
                            monthandyear  = string.Empty;
                            values  = string.Empty;
                            if (monthandyear != strdate.ToString())
                            {
                                monthandyear = strdate.ToString();
                                insertvalues = dcolumn;
                                values = Att_value;
                                updatevalues = dcolumn + "=" + Att_value;
                            }
                            savefalg = true;
                            if (btnsave.Text == "Update")
                            {
                                savevalue = 2;
                            }
                            else
                            {
                                savevalue = 1;
                            }
                        }
                        //start ***************** Added by jairam *************** 12-09-2014
                        if (minimum != "1")
                        {
                            if (daywise == "1")
                            {
                                string fromdate = txtFromDate.Text;
                                //  string todate = TxtToDate.Text;
                                string[] fromdatesplit = fromdate.Split('/');
                                // string[] todatesplit = todate.Split('-');
                                DateTime newfromdate = Convert.ToDateTime(fromdatesplit[1].ToString() + "/" + fromdatesplit[0].ToString() + "/" + fromdatesplit[2].ToString());
                                //   DateTime newtodate = Convert.ToDateTime(todatesplit[1].ToString() + "/" + todatesplit[0].ToString() + "/" + todatesplit[2].ToString());
                                string newdate = newfromdate.ToString("dd/MM/yyyy");
                                string[] newdatesplit = newdate.Split('/');
                                string date_value = newdatesplit[0].ToString();
                                date_value = date_value.TrimStart('0');
                                string date_value_table = "d" + date_value;
                                string month_value = newdatesplit[1].ToString();
                                string year_value = newdatesplit[2].ToString();
                                string monty_year_value = Convert.ToString((Convert.ToInt32(year_value) * 12 + Convert.ToInt32(month_value)));
                                string date_value_table_day  = string.Empty;
                                for (int k = 1; k <= totalhor; k++)
                                {
                                    if (date_value_table_day == "")
                                    {
                                        date_value_table_day = date_value_table + "d" + k;
                                    }
                                    else
                                    {
                                        date_value_table_day = date_value_table_day + "," + date_value_table + "d" + k;
                                    }
                                }
                                int split_day_hour = 0;
                                int first_split_present = 0;
                                int second_split_absent = 0;
                                int notconsider = 0;
                                int first_split_absent = 0;
                                int second_split_present = 0;
                                int firstempty_count = 0;
                                int secondempty_count = 0;
                                bool attendflag = false;
                                string absent_count_query  = string.Empty;
                                absent_count_query = "Select " + date_value_table_day + " from attendance where roll_no ='" + str_rollno + "'and month_year in ('" + monty_year_value + "')";
                                ds.Clear();
                                ds = dacc.select_method_wo_parameter(absent_count_query, "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                                    {
                                        split_day_hour++;
                                        string attendvalue = Convert.ToString(ds.Tables[0].Rows[0][j]);
                                        if (attendvalue != "")
                                        {
                                            if (present_calcflag.Count > 0)
                                            {
                                                if (split_day_hour <= Convert.ToInt32(no_firsthalf))
                                                {
                                                    if (present_calcflag.Contains(attendvalue) == true)
                                                    {
                                                        first_split_present++;
                                                    }
                                                    else if (absent_calcflag.Contains(attendvalue) == true)
                                                    {
                                                        first_split_absent++;
                                                    }
                                                    else if (attendvalue == "" || attendvalue == "0" || attendvalue == null || attendvalue == "H")
                                                    {
                                                        firstempty_count++;
                                                    }
                                                    else
                                                    {
                                                        notconsider++;
                                                    }
                                                }
                                                else
                                                {
                                                    if (present_calcflag.Contains(attendvalue) == true)
                                                    {
                                                        second_split_present++;
                                                    }
                                                    else if (absent_calcflag.Contains(attendvalue) == true)
                                                    {
                                                        second_split_absent++;
                                                    }
                                                    else if (attendvalue == "" || attendvalue == null || attendvalue == "0" || attendvalue == "H")
                                                    {
                                                        secondempty_count++;
                                                    }
                                                    else
                                                    {
                                                        notconsider++;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                attendflag = true;
                                            }
                                        }
                                    }
                                    if (attendflag == false)
                                    {
                                        if (firstempty_count < Convert.ToInt32(no_minpresent_firsthalf))
                                        {
                                            if (secondempty_count < Convert.ToInt32(no_minpresent_secondhalf))
                                            {
                                                if (first_split_present < Convert.ToInt32(no_minpresent_firsthalf) && second_split_present < Convert.ToInt32(no_minpresent_secondhalf))
                                                {
                                                    if (first_split_absent != 0 && second_split_absent != 0)
                                                    {
                                                        SendingSms(str_rollno, Splitmondate, str_hour, collegename, coursename, daywisedata);
                                                        sendvoicecall(str_rollno, Splitmondate, str_hour, ddlbatch.SelectedValue.ToString(), ddlbranch.SelectedItem.Value.ToString(), collegename, coursename, daywisedata);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                // **************************** End *********************************
                loadattendancecount();
                if (allatflag == false)
                {
                    btnsave.Text = "Save";
                }
                else
                {
                    btnsave.Text = "Update";
                }
                if (savefalg == true)
                {
                    string entrycode = Session["Entry_Code"].ToString();
                    string formname = "Student Attendance Entry";
                    string toa = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                    string doa = DateTime.Now.ToString("MM/dd/yyy");
                    string section  = string.Empty;
                    if (ddlsec.SelectedValue.ToString() != "" && ddlsec.SelectedValue.ToString() != "All" && ddlsec.SelectedValue.ToString() != null && ddlsec.SelectedValue.ToString() != "0")
                    {
                        section = ":Sections -" + ddlsec.SelectedValue.ToString();
                    }
                    string details = "" + ddlbranch.SelectedValue.ToString() + ":Sem - " + ddlduration.SelectedValue.ToString() + ":Batch Year -" + ddlbatch.SelectedValue.ToString() + " " + section + "";
                    string modules = "0";
                    string act_diff = " ";
                    string ctsname = "Update Attendance Information";
                    if (savevalue == 1)
                    {
                        ctsname = "Save the Attendance Inforamtion";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Saved successfully')", true);
                    }
                    else
                    {
                        ctsname = "Update the Attendance Information";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Updated successfully')", true);
                    }
                    string strlogdetails = "insert into UserLog (Entry_Code,Form_Name,UsrAction,TOA,DOA,Details,Module,Act_Diff,ctrNam) values ('" + entrycode + "','" + formname + "','" + savevalue + "','" + toa + "','" + doa + "','" + details + "','" + modules + "','" + act_diff + "','" + ctsname + "')";
                    int a = dacc.update_method_wo_parameter(strlogdetails, "Text");
                    btnsave.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Session["collegecode"].ToString(), "staffattendanceentry");
        }
    }

    protected void btndetails_Click(object sender, EventArgs e)
    {
        try
        {
            lblholihrdetails.Visible = false;
            Boolean setflag = false;
            lblerrhr.Visible = false;
            panhrdetails.Visible = true;
            grid_sliplist.Visible = false;
            

            dtrow = dtl1.NewRow();
            dtl1.Rows.Add(dtrow);

            int colu = 0;


            dtl1.Columns.Add("S.No", typeof(string));
            dtl1.Rows[0][colu] = "S.No";
            colu++;

            dtl1.Columns.Add("Hour", typeof(string));
            dtl1.Rows[0][colu] = "Hour";
            colu++;

            dtl1.Columns.Add("Subject Name", typeof(string));
            dtl1.Rows[0][colu] = "Subject Name";
            colu++;

            dtl1.Columns.Add("Staff Name", typeof(string));
            dtl1.Rows[0][colu] = "Staff Name";
            colu++;

            
            string byear = ddlbatch.SelectedValue.ToString();
            string degree_code = ddlbranch.SelectedValue.ToString();
            string semester = ddlduration.SelectedValue.ToString();
            string strsec  = string.Empty;
            string sec  = string.Empty;
            if (ddlsec.SelectedValue.ToString() == "" || ddlsec.SelectedValue.ToString() == "-1" || ddlsec.Enabled == false)
            {
                strsec = " ";
            }
            else
            {
                strsec = "and sections='" + ddlsec.SelectedItem.ToString() + "'";
                sec = ddlsec.SelectedItem.ToString();
            }
            string[] sp = txtFromDate.Text.ToString().Split('/');
            DateTime dt = Convert.ToDateTime(sp[1] + '/' + sp[0] + '/' + sp[2]);
            string day = dt.ToString("ddd");
            strquery = "select CONVERT(nvarchar(50),end_date,101) as enddate,CONVERT(nvarchar(50),start_date,101) as startdate from seminfo where degree_code='" + ddlbranch.SelectedValue.ToString() + "' and batch_year='" + ddlbatch.SelectedValue.ToString() + "' and semester='" + ddlduration.SelectedValue.ToString() + "'";
            ds.Dispose();
            ds.Reset();
            ds = dacc.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string start = ds.Tables[0].Rows[0]["startdate"].ToString();
                string endd = ds.Tables[0].Rows[0]["enddate"].ToString();
                if (start.Trim() != "" && start != null && endd.Trim() != "" && endd != null)
                {
                    DateTime dtst = Convert.ToDateTime(ds.Tables[0].Rows[0]["startdate"].ToString());
                    DateTime dtet = Convert.ToDateTime(ds.Tables[0].Rows[0]["enddate"].ToString());
                    if (dtst <= dt && dtet >= dt)
                    {
                    }
                    else
                    {
                        lblerrhr.Visible = true;
                        lblerrhr.Text = "Please Enter the Attendance Date Between Semester Start Date and End Date";
                        return;
                    }
                }
                else
                {
                    lblerrhr.Visible = true;
                    lblerrhr.Text = "Please Update the Semester Parameters";
                    return;
                }
            }
            else
            {
                lblerrhr.Visible = true;
                lblerrhr.Text = "Please Update the Semester Parameters";
                return;
            }
            if (ddlsubject.Items.Count == 0 || ddlsubject.Enabled == false)
            {
                lblerrhr.Visible = true;
                lblerrhr.Text = "Please Update the Subject Information";
                return;
            }
            int totalhor = 0;
            strquery = "select * from PeriodAttndSchedule where degree_code='" + ddlbranch.SelectedValue.ToString() + "' and semester='" + ddlduration.SelectedValue.ToString() + "'";
            ds.Dispose();
            ds.Reset();
            ds = dacc.select_method_wo_parameter(strquery, "Text");
            string conhrs  = string.Empty;
            if (ds.Tables[0].Rows.Count > 0)
            {
                string nhrs = ds.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString();
                string fhrs = ds.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString();
                string shfrs = ds.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString();
                if (nhrs.ToString() != null && nhrs.ToString().Trim() != "0" && nhrs.ToString().Trim() != "")
                {
                    grid_sliplist.Visible = true;
                    strquery = "select * from Alternate_Schedule where degree_code='" + degree_code + "' and batch_year='" + byear + "' and semester='" + semester + "' " + strsec + " and FromDate='" + dt.ToString("MM/dd/yyyy") + "'";
                    ds.Dispose();
                    ds.Reset();
                    ds = dacc.select_method_wo_parameter(strquery, "Text");
                    int thrs = Convert.ToInt32(nhrs);
                    int ihrs = Convert.ToInt32(fhrs);
                    int sehrs = Convert.ToInt32(shfrs);
                    int starthour = 1;
                    int endhrs = thrs;
                    string holiquery = "select * from holidayStudents where degree_code='" + degree_code + "' and semester='" + semester + "' and holiday_date='" + dt.ToString("MM/dd/yyyy") + "'";
                    DataSet dsholiday = dacc.select_method_wo_parameter(holiquery, "Text");
                    if (dsholiday.Tables[0].Rows.Count > 0)
                    {
                        Boolean ful = Convert.ToBoolean(dsholiday.Tables[0].Rows[0]["halforfull"]);
                        Boolean fhlf = Convert.ToBoolean(dsholiday.Tables[0].Rows[0]["morning"]);
                        Boolean shlf = Convert.ToBoolean(dsholiday.Tables[0].Rows[0]["evening"]);
                        if (ful == false)
                        {
                            starthour = endhrs + endhrs;
                            lblholihrdetails.Visible = true;
                            lblholihrdetails.Text = "Selected Day is Holiday Reason - " + dsholiday.Tables[0].Rows[0]["holiday_desc"].ToString() + "";
                            grid_sliplist.Visible = false;
                            return;
                        }
                        else if (ful == true && fhlf == true)
                        {
                            starthour = ihrs + 1;
                            lblholihrdetails.Visible = true;
                            lblholihrdetails.Text = "" + dt.ToString("dd/MM/yyyy") + "- is Morning Holiday 	Reason - " + dsholiday.Tables[0].Rows[0]["holiday_desc"].ToString() + "";
                        }
                        else if (ful == true && shlf == true)
                        {
                            endhrs = ihrs;
                            lblholihrdetails.Visible = true;
                            lblholihrdetails.Text = "" + dt.ToString("dd/MM/yyyy") + "- is Evening Holiday 	Reason - " + dsholiday.Tables[0].Rows[0]["holiday_desc"].ToString() + "";
                        }
                    }
                    for (int i = starthour; i <= endhrs; i++)
                    {
                        
                        dtrow = dtl1.NewRow();
                        dtl1.Rows.Add(dtrow);
                        

                        dtl1.Rows[dtl1.Rows.Count - 1][0] = Convert.ToString(i.ToString());
                        dtl1.Rows[dtl1.Rows.Count - 1][1] = Convert.ToString(i.ToString());

                        string hr = day + i;
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string hrvalue = ds.Tables[0].Rows[0][hr].ToString();
                            string[] sphr = hrvalue.Split('-');
                            string staffname  = string.Empty;
                            for (int j = 0; j < sphr.GetUpperBound(0); j++)
                            {
                                string val = sphr[j].ToString();
                                if (j == 0)
                                {
                                    setflag = true;
                                    string subject = dacc.GetFunction("Select Subject_name From Subject where subject_no='" + val + "'");
                                    if (subject != null && subject.Trim() != "" && subject.Trim() != "0")
                                    {
                                        
                                        dtl1.Rows[dtl1.Rows.Count - 1][2] = subject;
                                    }
                                    else
                                    {
                                        
                                        dtl1.Rows[dtl1.Rows.Count - 1][2] = string.Empty; 
                                    }
                                }
                                else
                                {
                                    string staff = dacc.GetFunction("Select Staff_name From staffmaster where staff_code='" + val + "'");
                                    if (staff != null && staff.Trim() != "" && staff.Trim() != "0")
                                    {
                                        if (staffname == "")
                                        {
                                            staffname = staff;
                                        }
                                        else
                                        {
                                            staffname = staffname + " , " + staff;
                                        }
                                    }
                                    if (staffname != null && staffname.Trim() != "" && staffname.Trim() != "0")
                                    {
                                       
                                        dtl1.Rows[dtl1.Rows.Count - 1][3] = staffname;
                                    }
                                    else
                                    {
                                       
                                        dtl1.Rows[dtl1.Rows.Count - 1][3] = string.Empty;
                                    }
                                }
                            }
                        }
                        else
                        {
                            

                            dtl1.Rows[dtl1.Rows.Count - 1][2] = string.Empty;
                            dtl1.Rows[dtl1.Rows.Count - 1][3] = string.Empty;
                        }
                    }

                    if (dtl1.Rows.Count > 2)
                    {
                        grid_sliplist.DataSource = dtl1;
                        grid_sliplist.DataBind();
                        grid_sliplist.HeaderRow.Visible = false;
                        grid_sliplist.Width = 700;
                        for (int i = 0; i < grid_sliplist.Rows.Count; i++)
                        {
                            for (int j = 0; j < grid_sliplist.HeaderRow.Cells.Count; j++)
                            {

                                if (i == 0)
                                {
                                    grid_sliplist.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                    grid_sliplist.Rows[i].Cells[j].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                    grid_sliplist.Rows[i].Cells[j].BorderColor = Color.Black;
                                    grid_sliplist.Rows[i].Cells[j].Font.Bold = true;
                                }
                                else
                                {
                                    if (grid_sliplist.HeaderRow.Cells[j].Text == "Subject Name" || grid_sliplist.HeaderRow.Cells[j].Text == "Staff Name" )
                                        grid_sliplist.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Left;

                                    else
                                        grid_sliplist.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;

                                }
                            }

                        }
                    }
                    if (setflag == false)
                    {
                        grid_sliplist.Visible = false;
                        lblerrhr.Visible = true;
                        lblerrhr.Text = "No Records Found";
                        lblholihrdetails.Visible = false;
                    }
                }
                else
                {
                    grid_sliplist.Visible = false;
                    lblerrhr.Visible = true;
                    lblerrhr.Text = "Please Update Semester Information";
                    lblholihrdetails.Visible = false;
                }
            }
            else
            {
                grid_sliplist.Visible = false;
                lblerrhr.Visible = true;
                lblerrhr.Text = "Please Update Semester Information";
                lblholihrdetails.Visible = false;
            }
            
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Session["collegecode"].ToString(), "staffattendanceentry");
        }
    }

    protected void exit_sliplist_Click(object sender, EventArgs e)
    {
        try
        {
            panhrdetails.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Session["collegecode"].ToString(), "staffattendanceentry");
        }
    }

    public void loadcollegename()
    {
        try
        {
            string collquery = "Select collname from collinfo where college_code=" + Session["collegecode"].ToString() + "";
            DataSet datacol = new DataSet();
            datacol.Clear();
            datacol = dacc.select_method_wo_parameter(collquery, "Text");
            if (datacol.Tables[0].Rows.Count > 0)
            {
                collegename = datacol.Tables[0].Rows[0]["collname"].ToString();
            }
            string degreequery = "select distinct Course_Name,Dept_Name from Department dep, Degree deg, course c where dep.Dept_Code=deg.Dept_Code and c.Course_Id=deg.Course_Id and deg.college_code =" + Session["collegecode"].ToString() + " and Degree_Code=" + ddlbranch.SelectedItem.Value + "";
            DataSet dscode = new DataSet();
            dscode = dacc.select_method_wo_parameter(degreequery, "Text");
            if (dscode.Tables[0].Rows.Count > 0)
            {
                string course = dscode.Tables[0].Rows[0]["Course_Name"].ToString();
                string deptname = dscode.Tables[0].Rows[0]["Dept_Name"].ToString();
                coursename = course + "-" + deptname;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Session["collegecode"].ToString(), "staffattendanceentry");
        }
    }

    public void SendingSms(string rollno, string date, string hour, string college, string course, string setting)
    {
        try
        {
            string Gender  = string.Empty;
            string collegename1  = string.Empty;
            string Hour = hour;
            string hour_check  = string.Empty;
            string MsgText  = string.Empty;
            string RecepientNo  = string.Empty;
            int check = 0;
            string user_id  = string.Empty;
            collegename1 = college;
            string coursename1 = course;
            string[] split = date.Split(new Char[] { '/' });
            string datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
            date = datefrom;
            if (Convert.ToInt16(hour) == 1)
            {
                Hour = hour + "st ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) == 2)
            {
                Hour = hour + "nd ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) == 3)
            {
                Hour = hour + "rd ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) > 3)
            {
                Hour = hour + "th ";
                hour_check = hour;
            }
            string str1  = string.Empty;
            string group_code = Session["group_code"].ToString();
            if (group_code.Contains(";"))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where  college_code='" + Session["collegecode"].ToString() + "' and USER_ID='" + group_code + "'";
            }
            else
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where college_code='" + Session["collegecode"].ToString() + "' and USER_ID='" + Session["usercode"].ToString() + "'";
            }
            Boolean flage = false;
            DataSet ds1;
            ds1 = dacc.select_method_wo_parameter(str1, "txt");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int jj = 0; jj < ds1.Tables[0].Rows.Count; jj++)
                {
                    if (ds1.Tables[0].Rows[jj]["TextName"].ToString() == "Attendance Sms for Absent" && ds1.Tables[0].Rows[jj]["Taxtval"].ToString() == "1")
                    {
                        flage = true;
                    }
                }
                if (flage == true)
                {
                    for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
                    {
                        if (ds1.Tables[0].Rows[k]["TextName"].ToString() == "Period" && ds1.Tables[0].Rows[k]["Taxtval"].ToString() != "")
                        {
                            string splihours = ds1.Tables[0].Rows[k]["Taxtval"].ToString();
                            string[] fin_split = splihours.Split(',');
                            int count = fin_split.Length;
                            for (int i = 0; i < count; i++)
                            {
                                string final_Hours = fin_split[i];
                                if (hour_check == final_Hours)
                                {
                                    check = check + 1;
                                }
                            }
                        }
                    }
                }
            }
            if (check > 0)
            {
                check = 0;
                string ssr = "select * from Track_Value where college_code='" + Session["collegecode"].ToString() + "'";
                DataSet dstrack;
                dstrack = dacc.select_method_wo_parameter(ssr, "txt");
                if (dstrack.Tables[0].Rows.Count > 0)
                {
                    user_id = Convert.ToString(dstrack.Tables[0].Rows[0]["SMS_User_ID"]);
                    string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollno + "' and r.college_code='" + Session["collegecode"].ToString() + "'";
                    DataSet dsMobile;
                    dsMobile = dacc.select_method_wo_parameter(Phone, "txt");
                    DataSet ds;
                    if (ds1.Tables[0].Rows.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToInt16(dsMobile.Tables[0].Rows[0]["Gender"].ToString()) == 0)
                        {
                            Gender = "Your Son ";
                        }
                        else
                        {
                            Gender = "Your Daughter ";
                        }
                        DateTime dt = Convert.ToDateTime(date);
                        string section  = string.Empty;
                        if (ddlsec.Enabled == true)
                        {
                            section = Convert.ToString(ddlsec.SelectedItem.Text);
                        }
                        if (setting == "Hour")
                        {
                            MsgText = "Dear Parent, Good Morning. This Message from" + " " + collegename1 + ". Your ward " + dsMobile.Tables[0].Rows[0]["StudName"].ToString() + " of " + coursename1 + "-" + section + " is found absent  " + Hour + " hour. Thank you !!!";
                        }
                        for (int jj1 = 0; jj1 < ds1.Tables[0].Rows.Count; jj1++)
                        {
                            if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                            {
                                if (dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != "0")
                                {
                                    RecepientNo = dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString();
                                    string getval = dacc.GetUserapi(user_id);
                                    string[] spret = getval.Split('-');
                                    if (spret.GetUpperBound(0) == 1)
                                    {
                                        SenderID = spret[0].ToString();
                                        Password = spret[0].ToString();
                                        Session["api"] = user_id;
                                        Session["senderid"] = SenderID;
                                    }
                                    //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                    //// string strpath = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + RecepientNo + "&message=" + MsgText + "&sender=" + SenderID;
                                    //string isst = "0";
                                    //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                    int nofosmssend = d2.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0");
                                }
                            }
                            if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                            {
                                if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0")
                                {
                                    RecepientNo = dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString();
                                    string getval = dacc.GetUserapi(user_id);
                                    string[] spret = getval.Split('-');
                                    if (spret.GetUpperBound(0) == 1)
                                    {
                                        SenderID = spret[0].ToString();
                                        Password = spret[0].ToString();
                                        Session["api"] = user_id;
                                        Session["senderid"] = SenderID;
                                    }
                                    //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                    ////  string strpath = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + RecepientNo + "&message=" + MsgText + "&sender=" + SenderID;
                                    //string isst = "0";
                                    //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                    int nofosmssend = d2.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0");
                                }
                            }
                            if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                            {
                                if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0")
                                {
                                    RecepientNo = dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString();
                                    string getval = dacc.GetUserapi(user_id);
                                    string[] spret = getval.Split('-');
                                    if (spret.GetUpperBound(0) == 1)
                                    {
                                        SenderID = spret[0].ToString();
                                        Password = spret[0].ToString();
                                        Session["api"] = user_id;
                                        Session["senderid"] = SenderID;
                                    }
                                    //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                    ////string strpath = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + RecepientNo + "&message=" + MsgText + "&sender=" + SenderID;
                                    //string isst = "0";
                                    //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                    int nofosmssend = d2.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0");
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (flage == true)
                {
                    if (setting == "Day")
                    {
                        string ssr = "select * from Track_Value where college_code='" + Session["collegecode"].ToString() + "'";
                        DataSet dstrack;
                        dstrack = dacc.select_method_wo_parameter(ssr, "txt");
                        if (dstrack.Tables[0].Rows.Count > 0)
                        {
                            user_id = Convert.ToString(dstrack.Tables[0].Rows[0]["SMS_User_ID"]);
                            string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollno + "' and r.college_code='" + Session["collegecode"].ToString() + "'";
                            DataSet dsMobile;
                            dsMobile = dacc.select_method_wo_parameter(Phone, "txt");
                            DataSet ds;
                            if (ds1.Tables[0].Rows.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
                            {
                                if (Convert.ToInt16(dsMobile.Tables[0].Rows[0]["Gender"].ToString()) == 0)
                                {
                                    Gender = "Your Son ";
                                }
                                else
                                {
                                    Gender = "Your Daughter ";
                                }
                                DateTime dt = Convert.ToDateTime(date);
                                string section  = string.Empty;
                                if (ddlsec.Enabled == true)
                                {
                                    section = Convert.ToString(ddlsec.SelectedItem.Text);
                                }
                                MsgText = "Dear Parent, Good Morning. This Message from " + " " + collegename1 + ". Your ward " + dsMobile.Tables[0].Rows[0]["StudName"].ToString() + " of " + coursename1 + "-" + section + " is found absent today. Thank you !!!";
                                for (int jj1 = 0; jj1 < ds1.Tables[0].Rows.Count; jj1++)
                                {
                                    if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                    {
                                        if (dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != "0")
                                        {
                                            RecepientNo = dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString();
                                            string getval = dacc.GetUserapi(user_id);
                                            string[] spret = getval.Split('-');
                                            if (spret.GetUpperBound(0) == 1)
                                            {
                                                SenderID = spret[0].ToString();
                                                Password = spret[0].ToString();
                                                Session["api"] = user_id;
                                                Session["senderid"] = SenderID;
                                            }
                                            //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                            //// string strpath = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + RecepientNo + "&message=" + MsgText + "&sender=" + SenderID;
                                            //string isst = "0";
                                            //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                            int nofosmssend = d2.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0");
                                        }
                                    }
                                    if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                    {
                                        if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0")
                                        {
                                            RecepientNo = dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString();
                                            string getval = dacc.GetUserapi(user_id);
                                            string[] spret = getval.Split('-');
                                            if (spret.GetUpperBound(0) == 1)
                                            {
                                                SenderID = spret[0].ToString();
                                                Password = spret[0].ToString();
                                                Session["api"] = user_id;
                                                Session["senderid"] = SenderID;
                                            }
                                            //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                            ////  string strpath = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + RecepientNo + "&message=" + MsgText + "&sender=" + SenderID;
                                            //string isst = "0";
                                            //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                            int nofosmssend = d2.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0");
                                        }
                                    }
                                    if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                    {
                                        if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0")
                                        {
                                            RecepientNo = dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString();
                                            string getval = dacc.GetUserapi(user_id);
                                            string[] spret = getval.Split('-');
                                            if (spret.GetUpperBound(0) == 1)
                                            {
                                                SenderID = spret[0].ToString();
                                                Password = spret[0].ToString();
                                                Session["api"] = user_id;
                                                Session["senderid"] = SenderID;
                                            }
                                            //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                            ////string strpath = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + RecepientNo + "&message=" + MsgText + "&sender=" + SenderID;
                                            //string isst = "0";
                                            //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                            int nofosmssend = d2.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Session["collegecode"].ToString(), "staffattendanceentry");
        }
    }

    public void sendvoicecall(string rollno, string date, string hour, string batch, string degree, string college, string course, string setting)
    {
        try
        {
            string Hour = hour;
            string hour_check  = string.Empty;
            string roll = rollno;
            string batchyear = batch;
            string coursename = course;
            string voicelanguage  = string.Empty;
            string collegename = college;
            string MsgText  = string.Empty;
            string RecepientNo  = string.Empty;
            int check = 0;
            string[] split = date.Split(new Char[] { '/' });
            string datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
            date = datefrom;
            if (Convert.ToInt16(hour) == 1)
            {
                Hour = hour + "st ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) == 2)
            {
                Hour = hour + "nd ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) == 3)
            {
                Hour = hour + "rd ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) > 3)
            {
                Hour = hour + "th ";
                hour_check = hour;
            }
            string str1  = string.Empty;
            string group_code = Session["group_code"].ToString();
            if (group_code.Contains(";"))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where  college_code='" + Session["collegecode"].ToString() + "' and USER_ID='" + group_code + "'";
            }
            else
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where college_code='" + Session["collegecode"].ToString() + "' and USER_ID='" + Session["usercode"].ToString() + "'";
            }
            string section_voice  = string.Empty;
            if (ddlsec.Enabled == true)
            {
                section_voice = Convert.ToString(ddlsec.SelectedItem.Text);
            }
            Boolean flage = false;
            DataSet ds1;
            ds1 = dacc.select_method_wo_parameter(str1, "txt");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int jj = 0; jj < ds1.Tables[0].Rows.Count; jj++)
                {
                    if (ds1.Tables[0].Rows[jj]["TextName"].ToString() == "Voice Call for Absent" && ds1.Tables[0].Rows[jj]["Taxtval"].ToString() == "1")
                    {
                        flage = true;
                    }
                }
                if (flage == true)
                {
                    for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
                    {
                        if (ds1.Tables[0].Rows[k]["TextName"].ToString() == "Period" && ds1.Tables[0].Rows[k]["Taxtval"].ToString() != "")
                        {
                            string splihours = ds1.Tables[0].Rows[k]["Taxtval"].ToString();
                            string[] fin_split = splihours.Split(',');
                            int count = fin_split.Length;
                            for (int i = 0; i < count; i++)
                            {
                                string final_Hours = fin_split[i];
                                if (hour_check == final_Hours)
                                {
                                    check = check + 1;
                                }
                            }
                        }
                    }
                }
            }
            if (check > 0)
            {
                check = 0;
                string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName,VoiceLang from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollno + "' and r.college_code='" + Session["collegecode"].ToString() + "'";
                DataSet dsMobile;
                dsMobile = dacc.select_method_wo_parameter(Phone, "txt");
                string voicelang = Convert.ToString(dsMobile.Tables[0].Rows[0]["VoiceLang"]);
                if (voicelang != "")
                {
                    string langquery  = string.Empty;
                    langquery = "select TextVal from textvaltable where TextCode  ='" + voicelang + "' and TextCriteria='PLang' and college_code=" + Session["collegecode"] + "";
                    DataSet datalang = new DataSet();
                    datalang = dacc.select_method_wo_parameter(langquery, "Text");
                    if (datalang.Tables[0].Rows.Count > 0)
                    {
                        voicelanguage = datalang.Tables[0].Rows[0]["TextVal"].ToString();
                    }
                }
                // voicelanguage = "English";
                if (ds1.Tables[0].Rows.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
                {
                    string gender  = string.Empty;
                    if (Convert.ToInt16(dsMobile.Tables[0].Rows[0]["Gender"].ToString()) == 0)
                    {
                        gender = "MALE";
                    }
                    else
                    {
                        gender = "FEMALE";
                    }
                    string orginalname  = string.Empty;
                    string student_name = Convert.ToString(dsMobile.Tables[0].Rows[0]["StudName"]);
                    if (student_name.Contains(".") == true)
                    {
                        string[] splitname = student_name.Split('.');
                        for (int i = 0; i <= splitname.GetUpperBound(0); i++)
                        {
                            string lengthname = splitname[i].ToString();
                            if (lengthname.Trim().Length > 2)
                            {
                                orginalname = splitname[i].ToString();
                            }
                        }
                    }
                    else
                    {
                        string[] split2ndname = student_name.Split(' ');
                        if (split2ndname.Length > 0)
                        {
                            for (int k = 0; k <= split2ndname.GetUpperBound(0); k++)
                            {
                                string firstname = split2ndname[k].ToString();
                                if (firstname.Trim().Length > 2)
                                {
                                    if (orginalname == "")
                                    {
                                        orginalname = firstname.ToString();
                                    }
                                    else
                                    {
                                        orginalname = orginalname + " " + firstname.ToString();
                                    }
                                }
                            }
                        }
                    }
                    DateTime dt = Convert.ToDateTime(date);
                    biz.lbinfotech.www.Data h = new biz.lbinfotech.www.Data();
                    MsgText = "ABSETN AT ";
                    for (int jj1 = 0; jj1 < ds.Tables[0].Rows.Count; jj1++)
                    {
                        if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                        {
                            if (dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != "0")
                            {
                                RecepientNo = dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString();
                                string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILYHOUR", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                            }
                        }
                        if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                        {
                            if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0")
                            {
                                RecepientNo = dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString();
                                string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILYHOUR", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                            }
                        }
                        if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                        {
                            if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0")
                            {
                                RecepientNo = dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString();
                                string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILYHOUR", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                            }
                        }
                    }
                }
            }
            else
            {
                if (flage == true)
                {
                    if (setting == "Day")
                    {
                        string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName,VoiceLang from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollno + "' and r.college_code='" + Session["collegecode"].ToString() + "'";
                        DataSet dsMobile;
                        dsMobile = dacc.select_method_wo_parameter(Phone, "txt");
                        string voicelang = Convert.ToString(dsMobile.Tables[0].Rows[0]["VoiceLang"]);
                        if (voicelang != "")
                        {
                            string langquery  = string.Empty;
                            langquery = "select TextVal from textvaltable where TextCode  ='" + voicelang + "' and TextCriteria='PLang' and college_code=" + Session["collegecode"] + "";
                            DataSet datalang = new DataSet();
                            datalang = dacc.select_method_wo_parameter(langquery, "Text");
                            if (datalang.Tables[0].Rows.Count > 0)
                            {
                                voicelanguage = datalang.Tables[0].Rows[0]["TextVal"].ToString();
                            }
                        }
                        // voicelanguage = "English";
                        if (ds1.Tables[0].Rows.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
                        {
                            string gender  = string.Empty;
                            if (Convert.ToInt16(dsMobile.Tables[0].Rows[0]["Gender"].ToString()) == 0)
                            {
                                gender = "MALE";
                            }
                            else
                            {
                                gender = "FEMALE";
                            }
                            string orginalname  = string.Empty;
                            string student_name = Convert.ToString(dsMobile.Tables[0].Rows[0]["StudName"]);
                            if (student_name.Contains(".") == true)
                            {
                                string[] splitname = student_name.Split('.');
                                for (int i = 0; i <= splitname.GetUpperBound(0); i++)
                                {
                                    string lengthname = splitname[i].ToString();
                                    if (lengthname.Trim().Length > 2)
                                    {
                                        orginalname = splitname[i].ToString();
                                    }
                                }
                            }
                            else
                            {
                                string[] split2ndname = student_name.Split(' ');
                                if (split2ndname.Length > 0)
                                {
                                    for (int k = 0; k <= split2ndname.GetUpperBound(0); k++)
                                    {
                                        string firstname = split2ndname[k].ToString();
                                        if (firstname.Trim().Length > 2)
                                        {
                                            if (orginalname == "")
                                            {
                                                orginalname = firstname.ToString();
                                            }
                                            else
                                            {
                                                orginalname = orginalname + " " + firstname.ToString();
                                            }
                                        }
                                    }
                                }
                            }
                            DateTime dt = Convert.ToDateTime(date);
                            biz.lbinfotech.www.Data h = new biz.lbinfotech.www.Data();
                            MsgText = "ABSETN AT ";
                            for (int jj1 = 0; jj1 < ds1.Tables[0].Rows.Count; jj1++)
                            {
                                if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                {
                                    if (dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != "0")
                                    {
                                        RecepientNo = dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString();
                                        string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILY", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                                    }
                                }
                                if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                {
                                    if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0")
                                    {
                                        RecepientNo = dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString();
                                        string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILY", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                                    }
                                }
                                if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                {
                                    if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0")
                                    {
                                        RecepientNo = dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString();
                                        string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILY", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                                    }
                                }
                            }
                        }
                    }
                    else if (setting == "Minimun Absent Day")
                    {
                        string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName,VoiceLang from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollno + "' and r.college_code='" + Session["collegecode"].ToString() + "'";
                        DataSet dsMobile;
                        dsMobile = dacc.select_method_wo_parameter(Phone, "txt");
                        string voicelang = Convert.ToString(dsMobile.Tables[0].Rows[0]["VoiceLang"]);
                        if (voicelang != "")
                        {
                            string langquery  = string.Empty;
                            langquery = "select TextVal from textvaltable where TextCode  ='" + voicelang + "' and TextCriteria='PLang' and college_code=" + Session["collegecode"] + "";
                            DataSet datalang = new DataSet();
                            datalang = dacc.select_method_wo_parameter(langquery, "Text");
                            if (datalang.Tables[0].Rows.Count > 0)
                            {
                                voicelanguage = datalang.Tables[0].Rows[0]["TextVal"].ToString();
                            }
                        }
                        //voicelanguage = "English";
                        if (ds1.Tables[0].Rows.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
                        {
                            string gender  = string.Empty;
                            if (Convert.ToInt16(dsMobile.Tables[0].Rows[0]["Gender"].ToString()) == 0)
                            {
                                gender = "MALE";
                            }
                            else
                            {
                                gender = "FEMALE";
                            }
                            string orginalname  = string.Empty;
                            string student_name = Convert.ToString(dsMobile.Tables[0].Rows[0]["StudName"]);
                            if (student_name.Contains(".") == true)
                            {
                                string[] splitname = student_name.Split('.');
                                for (int i = 0; i <= splitname.GetUpperBound(0); i++)
                                {
                                    string lengthname = splitname[i].ToString();
                                    if (lengthname.Trim().Length > 2)
                                    {
                                        orginalname = splitname[i].ToString();
                                    }
                                }
                            }
                            else
                            {
                                string[] split2ndname = student_name.Split(' ');
                                if (split2ndname.Length > 0)
                                {
                                    for (int k = 0; k <= split2ndname.GetUpperBound(0); k++)
                                    {
                                        string firstname = split2ndname[k].ToString();
                                        if (firstname.Trim().Length > 2)
                                        {
                                            if (orginalname == "")
                                            {
                                                orginalname = firstname.ToString();
                                            }
                                            else
                                            {
                                                orginalname = orginalname + " " + firstname.ToString();
                                            }
                                        }
                                    }
                                }
                            }
                            DateTime dt = Convert.ToDateTime(date);
                            biz.lbinfotech.www.Data h = new biz.lbinfotech.www.Data();
                            MsgText = "ABSETN AT ";
                            for (int jj1 = 0; jj1 < ds.Tables[0].Rows.Count; jj1++)
                            {
                                if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                {
                                    if (dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != "0")
                                    {
                                        RecepientNo = dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString();
                                        string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILY", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                                    }
                                }
                                if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                {
                                    if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0")
                                    {
                                        RecepientNo = dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString();
                                        string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILY", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                                    }
                                }
                                if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                {
                                    if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0")
                                    {
                                        RecepientNo = dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString();
                                        string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILY", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Session["collegecode"].ToString(), "staffattendanceentry");
        }
    }

    public void smsreport(string uril, string isstaff, DateTime dt, string phone, string msg)
    {
        try
        {
            string phoneno = phone;
            string message = msg;
            string date = dt.ToString("MM/dd/yyyy") + ' ' + DateTime.Now.ToString("hh:mm:ss");
            WebRequest request = WebRequest.Create(uril);
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            StreamReader sr = new StreamReader(data);
            string strvel = sr.ReadToEnd();
            string groupmsgid  = string.Empty;
            groupmsgid = strvel;
            int sms = 0;
            string smsreportinsert  = string.Empty;
            string[] split_mobileno = phoneno.Split(new Char[] { ',' });
            for (int icount = 0; icount <= split_mobileno.GetUpperBound(0); icount++)
            {
                smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date ,sender_id)values( '" + split_mobileno[icount] + "','" + groupmsgid + "','" + message + "','" + Session["collegecode"].ToString() + "','" + isstaff + "','" + date + "' ,'" + Session["UserCode"].ToString() + "')"; // Added by jairam 21-11-2014
                sms = dacc.insert_method(smsreportinsert, hat, "Text");
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Session["collegecode"].ToString(), "staffattendanceentry");
        }
    }
    public void callGridBind()
    {
        //string uid = this.Page.Request.Params.Get("__EVENTTARGET");
        //if (uid != null && uid.Contains("gridLedgeDetails"))
        //{
        if (Session["dtGrid"] != null)
        {
            dtGrid = (DataTable)Session["dtGrid"];
            Showgrid.DataSource = dtGrid;
            Showgrid.DataBind();
            Showgrid.HeaderRow.Visible = false;
        }
        else
        {
            Showgrid.DataSource = null;
            Showgrid.DataBind();
        }

        //}
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
     {
         try
         {
             if (e.Row.RowType == DataControlRowType.DataRow)
             {
                 int colcout = 0;
                 for (int ii = 0; ii < colofvisfalse.Count; ii++)
                 {
                     if (colofvisfalse[ii].ToString() == "false" || colofvisfalse[ii].ToString() == "False")
                     {
                         e.Row.Cells[ii].Visible = false;

                         if (ii < 6)
                             colcout++;
                     }
                 }
                 if (e.Row.RowIndex == 0)
                 {
                     e.Row.BackColor = Color.FromArgb(12, 166, 202);
                     e.Row.Cells[0].RowSpan = 2;
                     e.Row.Cells[1].RowSpan = 2;
                     e.Row.Cells[2].RowSpan = 2;
                     e.Row.Cells[3].RowSpan = 2;
                     e.Row.Cells[4].RowSpan = 2;
                     e.Row.Cells[5].RowSpan = 2;




                     for (int cCnt = 6; cCnt < e.Row.Cells.Count; )
                     {
                         int count = 1;
                         for (int i = 1; i + cCnt < e.Row.Cells.Count && e.Row.Cells[cCnt].Text.Trim() == e.Row.Cells[cCnt + i].Text.Trim(); i++)
                         {
                             count++;
                             
                         }
                         e.Row.Cells[cCnt].ColumnSpan = count;
                         e.Row.Cells[cCnt].HorizontalAlign = HorizontalAlign.Left;
                         for (int i = 1; i < count; i++)
                             e.Row.Cells[cCnt + i].Visible = false;



                         cCnt += count;

                         
                     }
                     e.Row.HorizontalAlign = HorizontalAlign.Center;
                     e.Row.Font.Bold = true;
                 }
                 else if (e.Row.RowIndex == 1)
                 {
                     e.Row.BackColor = Color.FromArgb(12, 166, 202);
                     e.Row.Cells[0].Visible = false;
                     e.Row.Cells[1].Visible = false;
                     e.Row.Cells[2].Visible = false;
                     e.Row.Cells[3].Visible = false;
                     e.Row.Cells[4].Visible = false;
                     e.Row.Cells[5].Visible = false;

                     //e.Row.Cells.RemoveAt(0);
                     //e.Row.Cells.RemoveAt(0);
                     //e.Row.Cells.RemoveAt(0);
                     //e.Row.Cells.RemoveAt(0);
                     //e.Row.Cells.RemoveAt(0);
                     //e.Row.Cells.RemoveAt(0);
                     e.Row.HorizontalAlign = HorizontalAlign.Center;
                     e.Row.Font.Bold = true;



                 }
                 else if (e.Row.RowIndex == 2)
                 {



                     for (int ii = 0; ii < e.Row.Cells.Count; ii++)
                     {
                         if (ii == 0)
                             e.Row.Cells[0].ColumnSpan = 6 - colcout;
                         else if (ii < 6)
                             e.Row.Cells[ii].Visible = false;
                         else if (ii >= 6)
                         {
                             DropDownList ddlselectall = new DropDownList();
                             ddlselectall.Attributes.Add("style", "background-color:MistyRose;");
                             ddlselectall.Width = 80;
                             
                             string[] attnd_rights1 = new string[100];
                             int i = 0, j = 0;
                             string odrights = dacc.GetFunction("select rights from  OD_Master_Setting where " + grouporusercode + "");
                             if (odrights.Trim() != null && odrights.Trim() != "" && odrights.Trim() != "0")
                             {
                                 string od_rights = string.Empty;
                                 od_rights = odrights;
                                 string[] split_od_rights = od_rights.Split(',');
                                 
                                 ddlselectall.Items.Add("Select for All");
                                 ddlselectall.Items.Add("");
                                 
                                 
                                 for (int od_temp = 0; od_temp <= split_od_rights.GetUpperBound(0); od_temp++)
                                 {
                                     if (od_temp == split_od_rights.GetUpperBound(0))
                                     {
                                        
                                     }
                                     
                                     
                                     ddlselectall.Items.Add(split_od_rights[od_temp].ToString());
                                 }
                             }
                             else
                             {
                                 

                                 ddlselectall.Items.Add("Select for All");
                                 ddlselectall.Items.Add("");
                                 ddlselectall.Items.Add("P");
                                 ddlselectall.Items.Add("A");
                                 ddlselectall.Items.Add("OD");
                                 ddlselectall.Items.Add("SOD");
                                 ddlselectall.Items.Add("ML");
                                 ddlselectall.Items.Add("NSS");
                                 ddlselectall.Items.Add("L");
                                 ddlselectall.Items.Add("NCC");
                                 ddlselectall.Items.Add("HS");
                                 ddlselectall.Items.Add("PP");
                                 ddlselectall.Items.Add("SYOD");
                                 ddlselectall.Items.Add("COD");
                                 ddlselectall.Items.Add("OOD");
                                 ddlselectall.Items.Add("LA");

                             }
                             

                             ddlselectall.SelectedIndexChanged += new EventHandler(ddlselectall_Indexchanged);
                             ddlselectall.AutoPostBack = true;
                             ddlselectall.ID = "ddlselectall_" + ii;

                             e.Row.Cells[ii].Controls.Add(ddlselectall);
                             e.Row.Cells[ii].BackColor = Color.MistyRose;

                             //DropDownList ddlMode = new DropDownList();
                             //ddlMode.Width = 90;
                             //ddlMode.Attributes.Add("style", "background-color:#ff6600;");
                             //ddlMode.Items.Add("Regular");
                             //ddlMode.Items.Add("Monthwise");
                             //ddlMode.SelectedIndexChanged += new EventHandler(ddlMode_Indexchanged);
                             //ddlMode.AutoPostBack = true;
                             //ddlMode.ID = "ddlMode_" + colInd;

                             //TextBox lblMode = new TextBox();
                             //// lblMode.Visible = false;
                             //lblMode.ID = "lblMode_" + colInd;
                             //lblMode.Attributes.Add("style", "display:none;");

                             //e.Row.Cells[colInd].Controls.Add(lblMode);
                             //e.Row.Cells[colInd].Controls.Add(ddlMode);
                         }
                     }




                 }

                 else
                 {

                     for (int ii = 0; ii < e.Row.Cells.Count; ii++)
                     {
                         e.Row.Cells[ii].BackColor = Color.MistyRose;
                         if (dtGrid.Rows.Count > 0)
                         {
                             if (e.Row.RowIndex > dtGrid.Rows.Count - 3)
                             {
                                 e.Row.Cells[ii].HorizontalAlign = HorizontalAlign.Center;
                                 if (ii == 0)
                                     e.Row.Cells[0].ColumnSpan = 6 - colcout;
                                 else if (ii < 6)
                                     e.Row.Cells[ii].Visible = false;


                             }
                             else
                             {
                                 
                                 if (ii == 1)
                                 {
                                     DropDownList ddlselectalldaywise = new DropDownList();
                                     ddlselectalldaywise.Attributes.Add("style", "background-color:MistyRose;");
                                     ddlselectalldaywise.Width = 60;
                                    
                                     string[] attnd_rights1 = new string[100];
                                     int i = 0, j = 0;
                                     string odrights = dacc.GetFunction("select rights from  OD_Master_Setting where " + grouporusercode + "");
                                     if (odrights.Trim() != null && odrights.Trim() != "" && odrights.Trim() != "0")
                                     {
                                         string od_rights = string.Empty;
                                         od_rights = odrights;
                                         string[] split_od_rights = od_rights.Split(',');
                                         
                                         
                                         ddlselectalldaywise.Items.Add("");
                                         
                                         for (int od_temp = 0; od_temp <= split_od_rights.GetUpperBound(0); od_temp++)
                                         {
                                             if (od_temp == split_od_rights.GetUpperBound(0))
                                             {
                                                
                                                 ddlselectalldaywise.Items.Add("");
                                             }
                                             
                                             ddlselectalldaywise.Items.Add(split_od_rights[od_temp].ToString());

                                             
                                         }
                                     }
                                     else
                                     {
                                     

                                     
                                         ddlselectalldaywise.Items.Add("");
                                         ddlselectalldaywise.Items.Add("P");
                                         ddlselectalldaywise.Items.Add("A");
                                         ddlselectalldaywise.Items.Add("OD");
                                         ddlselectalldaywise.Items.Add("SOD");
                                         ddlselectalldaywise.Items.Add("ML");
                                         ddlselectalldaywise.Items.Add("NSS");
                                         ddlselectalldaywise.Items.Add("L");
                                         ddlselectalldaywise.Items.Add("NCC");
                                         ddlselectalldaywise.Items.Add("HS");
                                         ddlselectalldaywise.Items.Add("PP");
                                         ddlselectalldaywise.Items.Add("SYOD");
                                         ddlselectalldaywise.Items.Add("COD");
                                         ddlselectalldaywise.Items.Add("OOD");
                                         ddlselectalldaywise.Items.Add(" ");
                                         ddlselectalldaywise.Items.Add("LA");

                                     }
                                    

                                     ddlselectalldaywise.SelectedIndexChanged += new EventHandler(ddlselectalldaywise_Indexchanged);
                                     ddlselectalldaywise.AutoPostBack = true;
                                     ddlselectalldaywise.ID = "ddlselectalldaywise_" + e.Row.RowIndex;

                                     e.Row.Cells[ii].Controls.Add(ddlselectalldaywise);

                                 }
                                 else if ( ii < 6)
                                 {
                                     if (ii == 0)
                                         e.Row.Cells[ii].HorizontalAlign = HorizontalAlign.Center;

                                     if (rowsofcolour.Count != 0 && ii !=1)
                                     {
                                         if (rowsofcolour[e.Row.RowIndex] == "MediumSeaGreen")
                                             e.Row.Cells[ii].BackColor = Color.MediumSeaGreen;
                                         else if (rowsofcolour[e.Row.RowIndex] == "LightYellow")
                                             e.Row.Cells[ii].BackColor = Color.LightYellow;


                                     }
                                 }
                                 else if (ii >= 6)
                                 {
                                     DropDownList ddlselectallhourwise = new DropDownList();
                                     ddlselectallhourwise.Attributes.Add("style", "background-color:MistyRose;");
                                     ddlselectallhourwise.Width = 80;


                                     string[] attnd_rights1 = new string[100];
                                     int i = 0, j = 0;
                                     string odrights = dacc.GetFunction("select rights from  OD_Master_Setting where " + grouporusercode + "");
                                     if (odrights.Trim() != null && odrights.Trim() != "" && odrights.Trim() != "0")
                                     {
                                         string od_rights = string.Empty;
                                         od_rights = odrights;
                                         string[] split_od_rights = od_rights.Split(',');


                                         ddlselectallhourwise.Items.Add("");

                                         for (int od_temp = 0; od_temp <= split_od_rights.GetUpperBound(0); od_temp++)
                                         {
                                             if (od_temp == split_od_rights.GetUpperBound(0))
                                             {

                                                 ddlselectallhourwise.Items.Add("");
                                             }

                                             ddlselectallhourwise.Items.Add(split_od_rights[od_temp].ToString());


                                         }
                                     }
                                     else
                                     {



                                         ddlselectallhourwise.Items.Add("");
                                         ddlselectallhourwise.Items.Add("P");
                                         ddlselectallhourwise.Items.Add("A");
                                         ddlselectallhourwise.Items.Add("OD");
                                         ddlselectallhourwise.Items.Add("SOD");
                                         ddlselectallhourwise.Items.Add("ML");
                                         ddlselectallhourwise.Items.Add("NSS");
                                         ddlselectallhourwise.Items.Add("L");
                                         ddlselectallhourwise.Items.Add("NCC");
                                         ddlselectallhourwise.Items.Add("HS");
                                         ddlselectallhourwise.Items.Add("PP");
                                         ddlselectallhourwise.Items.Add("SYOD");
                                         ddlselectallhourwise.Items.Add("COD");
                                         ddlselectallhourwise.Items.Add("OOD");
                                         ddlselectallhourwise.Items.Add(" ");
                                         ddlselectallhourwise.Items.Add("LA");

                                     }

                                     

                                     ddlselectallhourwise.AutoPostBack = false;
                                     ddlselectallhourwise.ID = "ddlselectallhourwise_" + e.Row.RowIndex + "_" + ii;
                                     string[] text = dtGrid.Rows[e.Row.RowIndex][ii].ToString().Split('~');
                                     if (text.Length > 1)
                                     {

                                         e.Row.Cells[ii].Text = "";
                                         e.Row.Cells[ii].BackColor = Color.LightGreen;



                                     }
                                     else
                                     {
                                         ddlselectallhourwise.SelectedValue = dtGrid.Rows[e.Row.RowIndex][ii].ToString();
                                         e.Row.Cells[ii].Controls.Add(ddlselectallhourwise);
                                     }

                                 }
                             }
                         }

                     }
                 }
             }
         }
         catch (Exception ex)
         {
             d2.sendErrorMail(ex, Session["collegecode"].ToString(), "staffattendanceentry");

         }
     }


    protected void ddlselectall_Indexchanged(object sender, EventArgs e)
    {
        string uid = this.Page.Request.Params.Get("__EVENTTARGET");
        if (uid != null && uid.Contains("ddlselectall_"))
        {
            string[] values = uid.Split('$');
            //string row = values[3].Replace("ctl", "");
            string col = values[4].Replace("ddlselectall_", "");
            Control ctrl = Page.FindControl(uid);
            DropDownList ddl = (DropDownList)ctrl;
            string attendance = ddl.SelectedValue;
            if (attendance != "Select for All")
                for (int rows = 3; rows < Showgrid.Rows.Count - 2; rows++)
                {
                    if (Showgrid.Rows[rows].Cells[Convert.ToInt32(col)].Text != "")
                    {
                        DropDownList vv = Showgrid.Rows[rows].Cells[Convert.ToInt32(col)].FindControl("ddlselectallhourwise_" + rows + "_" + col) as DropDownList;
                        string gg = vv.SelectedValue;
                        if ((gg != "OD") && (gg != "S"))
                        {
                            (Showgrid.Rows[rows].FindControl("ddlselectallhourwise_" + rows + "_" + col) as DropDownList).SelectedIndex = (Showgrid.Rows[rows].FindControl("ddlselectallhourwise_" + rows + "_" + col) as DropDownList).Items.IndexOf((Showgrid.Rows[rows].FindControl("ddlselectallhourwise_" + rows + "_" + col) as DropDownList).Items.FindByValue(attendance));
                        }
                    }
                }
            ddl.SelectedValue = "Select for All";
        }
    }

    protected void ddlselectalldaywise_Indexchanged(object sender, EventArgs e)
    {
        string uid = this.Page.Request.Params.Get("__EVENTTARGET");
        if (uid != null && uid.Contains("ddlselectalldaywise_"))
        {
            string[] values = uid.Split('$');
            //string row = values[3].Replace("ctl", "");
            string row = values[4].Replace("ddlselectalldaywise_", "");
            Control ctrl = Page.FindControl(uid);
            DropDownList ddl = (DropDownList)ctrl;
            string attendance = ddl.SelectedValue;
            if (attendance != "Select for All")
                for (int col = 6; col < Showgrid.HeaderRow.Cells.Count; col++)
                {
                    if (Showgrid.Rows[Convert.ToInt32(row)].Cells[col].Text != "")
                    {
                        DropDownList vv = Showgrid.Rows[Convert.ToInt32(row)].Cells[Convert.ToInt32(col)].FindControl("ddlselectallhourwise_" + Convert.ToInt32(row) + "_" + col) as DropDownList;
                        string gg = vv.SelectedValue;
                        if ((gg != "OD") && (gg != "S"))
                        {
                            (Showgrid.Rows[Convert.ToInt32(row)].FindControl("ddlselectallhourwise_" + row + "_" + col) as DropDownList).SelectedIndex = (Showgrid.Rows[Convert.ToInt32(row)].FindControl("ddlselectallhourwise_" + row + "_" + col) as DropDownList).Items.IndexOf((Showgrid.Rows[Convert.ToInt32(row)].FindControl("ddlselectallhourwise_" + row + "_" + col) as DropDownList).Items.FindByValue(attendance));
                        }
                    }
                }

            ddl.SelectedValue = "";
        }
    }
}